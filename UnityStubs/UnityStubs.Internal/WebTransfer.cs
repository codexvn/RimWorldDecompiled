using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine.Networking;
using Waylith.Assets;

namespace UnityStubs.Internal;

internal sealed class WebTransfer
{
	private const string UserAgent = "UnityPlayer/2022.3.35f1 (UnityWebRequest/1.0, libcurl/8.5.0-DEV)";

	private static readonly HttpRequestOptionsKey<CertificateHandler> CertificateHandlerKey = new HttpRequestOptionsKey<CertificateHandler>("UnityCertificateHandler");

	private static readonly HashSet<string> ContentHeaderNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
	{
		"Allow", "Content-Disposition", "Content-Encoding", "Content-Language", "Content-Length", "Content-Location", "Content-MD5", "Content-Range", "Content-Type", "Expires",
		"Last-Modified"
	};

	private static readonly HttpClient Client = CreateClient(pooled: true);

	private static readonly HttpClient UnpooledClient = CreateClient(pooled: false);

	private readonly CancellationTokenSource cancel = new CancellationTokenSource();

	private volatile bool aborted;

	private long downloadedBytes;

	private long expectedBytes = -1L;

	public Task<WebTransferResponse> Task { get; }

	public bool UploadFinished { get; private set; }

	public long DownloadedBytes => Interlocked.Read(in downloadedBytes);

	public float DownloadProgress
	{
		get
		{
			long num = Interlocked.Read(in expectedBytes);
			if (num <= 0)
			{
				return 0f;
			}
			return Math.Clamp((float)DownloadedBytes / (float)num, 0f, 1f);
		}
	}

	public WebTransfer(WebTransferRequest request)
	{
		WebTransfer webTransfer = this;
		if (request.TimeoutSeconds > 0)
		{
			cancel.CancelAfter(TimeSpan.FromSeconds(request.TimeoutSeconds));
		}
		Task = System.Threading.Tasks.Task.Run(() => webTransfer.RunAsync(request));
	}

	public void Abort()
	{
		aborted = true;
		cancel.Cancel();
	}

	private async Task<WebTransferResponse> RunAsync(WebTransferRequest request)
	{
		_ = 1;
		try
		{
			Uri uri = ResolveUri(request.Url);
			if (uri.IsFile)
			{
				return await ReadFileAsync(LocalFilePath(uri));
			}
			return await SendAsync(uri, request);
		}
		catch (OperationCanceledException)
		{
			return Failed(aborted ? "Request aborted" : "Request timeout");
		}
		catch (HttpRequestException ex2)
		{
			return Failed(ex2.HttpRequestError switch
			{
				HttpRequestError.NameResolutionError => "Cannot resolve destination host", 
				HttpRequestError.ConnectionError => "Cannot connect to destination host", 
				HttpRequestError.SecureConnectionError => "Unable to complete SSL connection", 
				_ => ex2.Message, 
			});
		}
		catch (Exception ex3)
		{
			return Failed(ex3.Message);
		}
	}

	private static Uri ResolveUri(string url)
	{
		if (Uri.TryCreate(url, UriKind.Absolute, out Uri result))
		{
			return result;
		}
		return new Uri("http://" + url);
	}

	private static string LocalFilePath(Uri uri)
	{
		if (!uri.IsUnc || OperatingSystem.IsWindows())
		{
			return uri.LocalPath;
		}
		string text = Uri.UnescapeDataString(uri.AbsolutePath);
		if (!uri.Host.Equals("localhost", StringComparison.OrdinalIgnoreCase))
		{
			return "/" + uri.Host + text;
		}
		return text;
	}

	private async Task<WebTransferResponse> ReadFileAsync(string path)
	{
		if (!ContentPack.TryReadFile(path, out byte[] data))
		{
			if (!File.Exists(path))
			{
				return Failed("Cannot connect to destination host");
			}
			data = await File.ReadAllBytesAsync(path, cancel.Token);
		}
		Interlocked.Exchange(ref expectedBytes, data.Length);
		Interlocked.Exchange(ref downloadedBytes, data.Length);
		UploadFinished = true;
		return new WebTransferResponse
		{
			Headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase),
			Body = data,
			LocalPath = path
		};
	}

	private async Task<WebTransferResponse> SendAsync(Uri uri, WebTransferRequest request)
	{
		using HttpRequestMessage message = new HttpRequestMessage(new HttpMethod(request.Method ?? "GET"), uri);
		message.Headers.TryAddWithoutValidation("User-Agent", "UnityPlayer/2022.3.35f1 (UnityWebRequest/1.0, libcurl/8.5.0-DEV)");
		message.Headers.TryAddWithoutValidation("X-Unity-Version", "2022.3.35f1");
		if (request.CertificateHandler != null)
		{
			message.Options.Set(CertificateHandlerKey, request.CertificateHandler);
		}
		if (request.Body != null)
		{
			message.Content = new ByteArrayContent(request.Body);
			message.Content.Headers.TryAddWithoutValidation("Content-Type", request.BodyContentType ?? "application/octet-stream");
		}
		foreach (var (text3, value) in request.Headers)
		{
			if (ContentHeaderNames.Contains(text3))
			{
				message.Content?.Headers.Remove(text3);
				message.Content?.Headers.TryAddWithoutValidation(text3, value);
			}
			else
			{
				message.Headers.Remove(text3);
				message.Headers.TryAddWithoutValidation(text3, value);
			}
		}
		using HttpResponseMessage response = await ((request.CertificateHandler != null) ? UnpooledClient : Client).SendAsync(message, HttpCompletionOption.ResponseHeadersRead, cancel.Token);
		UploadFinished = true;
		Dictionary<string, string> headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
		CopyHeaders(response.Headers, headers);
		CopyHeaders(response.Content.Headers, headers);
		Interlocked.Exchange(ref expectedBytes, response.Content.Headers.ContentLength ?? (-1));
		WebTransferResponse result;
		await using (Stream stream = await response.Content.ReadAsStreamAsync(cancel.Token))
		{
			using MemoryStream body = new MemoryStream();
			byte[] buffer = new byte[65536];
			int num;
			while ((num = await stream.ReadAsync(buffer, cancel.Token)) > 0)
			{
				body.Write(buffer, 0, num);
				Interlocked.Add(ref downloadedBytes, num);
			}
			result = new WebTransferResponse
			{
				StatusCode = (long)response.StatusCode,
				StatusLine = $"HTTP/{response.Version.Major}.{response.Version.Minor} {response.StatusCode} {response.ReasonPhrase}",
				Headers = headers,
				Body = body.ToArray()
			};
		}
		return result;
	}

	private static void CopyHeaders(HttpHeaders source, Dictionary<string, string> target)
	{
		foreach (KeyValuePair<string, IEnumerable<string>> item in source)
		{
			item.Deconstruct(out var key, out var value);
			string key2 = key;
			IEnumerable<string> values = value;
			target[key2] = string.Join(", ", values);
		}
	}

	private static WebTransferResponse Failed(string error)
	{
		return new WebTransferResponse
		{
			ConnectionError = error
		};
	}

	private static HttpClient CreateClient(bool pooled)
	{
		SocketsHttpHandler socketsHttpHandler = new SocketsHttpHandler();
		socketsHttpHandler.PooledConnectionLifetime = (pooled ? Timeout.InfiniteTimeSpan : TimeSpan.Zero);
		socketsHttpHandler.AllowAutoRedirect = true;
		socketsHttpHandler.MaxAutomaticRedirections = 32;
		socketsHttpHandler.AutomaticDecompression = DecompressionMethods.All;
		socketsHttpHandler.SslOptions = new SslClientAuthenticationOptions();
		socketsHttpHandler.SslOptions.RemoteCertificateValidationCallback = (object sender, X509Certificate? certificate, X509Chain? chain, SslPolicyErrors errors) => (sender is HttpRequestMessage httpRequestMessage && httpRequestMessage.Options.TryGetValue(CertificateHandlerKey, out var value)) ? (certificate != null && value.Validate(certificate.GetRawCertData())) : (errors == SslPolicyErrors.None);
		return new HttpClient(socketsHttpHandler)
		{
			Timeout = Timeout.InfiniteTimeSpan
		};
	}
}
