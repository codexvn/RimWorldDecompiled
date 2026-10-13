using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using UnityStubs.Internal;
using Waylith.UnityCompat;

namespace UnityEngine.Networking;

public class UnityWebRequest : IDisposable
{
	public enum Result
	{
		ConnectionError = 2,
		DataProcessingError = 4,
		InProgress = 0,
		ProtocolError = 3,
		Success = 1
	}

	public const string kHttpVerbCREATE = "CREATE";

	public const string kHttpVerbDELETE = "DELETE";

	public const string kHttpVerbGET = "GET";

	public const string kHttpVerbHEAD = "HEAD";

	public const string kHttpVerbPOST = "POST";

	public const string kHttpVerbPUT = "PUT";

	private static readonly List<UnityWebRequest> InFlight = new List<UnityWebRequest>();

	private static bool _completionCheckSubscribed;

	private readonly List<KeyValuePair<string, string>> _requestHeaders = new List<KeyValuePair<string, string>>();

	private WebTransfer _transfer;

	private UnityWebRequestAsyncOperation _operation;

	private Dictionary<string, string> _responseHeaders;

	private bool _isDone;

	private Result _result;

	private string _error;

	private long _responseCode;

	public string url { get; set; }

	public Uri uri
	{
		get
		{
			return new Uri(url, UriKind.RelativeOrAbsolute);
		}
		set
		{
			url = value?.ToString();
		}
	}

	public string method { get; set; }

	public int timeout { get; set; }

	public int redirectLimit { get; set; } = 32;

	public bool disposeDownloadHandlerOnDispose { get; set; } = true;

	public bool disposeUploadHandlerOnDispose { get; set; } = true;

	public bool disposeCertificateHandlerOnDispose { get; set; } = true;

	public DownloadHandler downloadHandler { get; set; }

	public UploadHandler uploadHandler { get; set; }

	public CertificateHandler certificateHandler { get; set; }

	public bool isModifiable => _transfer == null;

	public bool isDone
	{
		get
		{
			PublishIfLanded();
			return _isDone;
		}
	}

	public Result result
	{
		get
		{
			PublishIfLanded();
			return _result;
		}
	}

	public string error
	{
		get
		{
			PublishIfLanded();
			return _error;
		}
	}

	public long responseCode
	{
		get
		{
			PublishIfLanded();
			return _responseCode;
		}
	}

	public bool isNetworkError => result == Result.ConnectionError;

	public bool isHttpError => result == Result.ProtocolError;

	public bool isError
	{
		get
		{
			Result result = this.result;
			if ((uint)(result - 2) <= 2u)
			{
				return true;
			}
			return false;
		}
	}

	public float downloadProgress
	{
		get
		{
			if (!isDone)
			{
				if (_transfer != null)
				{
					return _transfer.DownloadProgress;
				}
				return -1f;
			}
			return 1f;
		}
	}

	public ulong downloadedBytes
	{
		get
		{
			if (_transfer != null)
			{
				return (ulong)_transfer.DownloadedBytes;
			}
			return 0uL;
		}
	}

	public float uploadProgress
	{
		get
		{
			if (!isDone)
			{
				WebTransfer transfer = _transfer;
				if (transfer == null || !transfer.UploadFinished)
				{
					if (_transfer != null)
					{
						return 0f;
					}
					return -1f;
				}
			}
			return 1f;
		}
	}

	public ulong uploadedBytes
	{
		get
		{
			if (!(uploadProgress >= 1f) || uploadHandler?.data == null)
			{
				return 0uL;
			}
			return (ulong)uploadHandler.data.Length;
		}
	}

	public UnityWebRequest()
	{
		method = "GET";
	}

	public UnityWebRequest(string url)
		: this()
	{
		this.url = url;
	}

	public UnityWebRequest(Uri uri)
		: this(uri?.ToString())
	{
	}

	public UnityWebRequest(string url, string method)
	{
		this.url = url;
		this.method = method;
	}

	public UnityWebRequest(Uri uri, string method)
		: this(uri?.ToString(), method)
	{
	}

	public UnityWebRequest(string url, string method, DownloadHandler downloadHandler, UploadHandler uploadHandler)
	{
		this.url = url;
		this.method = method;
		this.downloadHandler = downloadHandler;
		this.uploadHandler = uploadHandler;
	}

	public static UnityWebRequest Get(string uri)
	{
		return new UnityWebRequest(uri, "GET", new DownloadHandlerBuffer(), null);
	}

	public static UnityWebRequest Get(Uri uri)
	{
		return Get(uri?.ToString());
	}

	public static UnityWebRequest Head(string uri)
	{
		return new UnityWebRequest(uri, "HEAD");
	}

	public static UnityWebRequest Delete(string uri)
	{
		return new UnityWebRequest(uri, "DELETE");
	}

	public static UnityWebRequest Put(string uri, byte[] bodyData)
	{
		return new UnityWebRequest(uri, "PUT", new DownloadHandlerBuffer(), new UploadHandlerRaw(bodyData));
	}

	public static UnityWebRequest Put(string uri, string bodyData)
	{
		return Put(uri, Encoding.UTF8.GetBytes(bodyData));
	}

	public static UnityWebRequest Post(string uri, WWWForm formData)
	{
		UnityWebRequest unityWebRequest = new UnityWebRequest(uri, "POST", new DownloadHandlerBuffer(), null);
		if (formData == null)
		{
			return unityWebRequest;
		}
		byte[] data = formData.data;
		if (data.Length != 0)
		{
			unityWebRequest.uploadHandler = new UploadHandlerRaw(data);
		}
		foreach (var (name, value) in formData.headers)
		{
			unityWebRequest.SetRequestHeader(name, value);
		}
		return unityWebRequest;
	}

	public static UnityWebRequest Post(string uri, string postData)
	{
		UnityWebRequest unityWebRequest = new UnityWebRequest(uri, "POST", new DownloadHandlerBuffer(), null);
		if (!string.IsNullOrEmpty(postData))
		{
			unityWebRequest.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(WebUtility.UrlEncode(postData)))
			{
				contentType = "application/x-www-form-urlencoded"
			};
		}
		return unityWebRequest;
	}

	public static UnityWebRequest Post(string uri, string postData, string contentType)
	{
		UnityWebRequest unityWebRequest = new UnityWebRequest(uri, "POST", new DownloadHandlerBuffer(), null);
		if (!string.IsNullOrEmpty(postData))
		{
			unityWebRequest.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(postData))
			{
				contentType = contentType
			};
		}
		return unityWebRequest;
	}

	public static UnityWebRequest Post(string uri, Dictionary<string, string> formFields)
	{
		UnityWebRequest unityWebRequest = new UnityWebRequest(uri, "POST", new DownloadHandlerBuffer(), null);
		if (formFields == null || formFields.Count == 0)
		{
			return unityWebRequest;
		}
		StringBuilder stringBuilder = new StringBuilder();
		foreach (var (value, value2) in formFields)
		{
			if (stringBuilder.Length > 0)
			{
				stringBuilder.Append('&');
			}
			stringBuilder.Append(WebUtility.UrlEncode(value)).Append('=').Append(WebUtility.UrlEncode(value2));
		}
		unityWebRequest.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(stringBuilder.ToString()))
		{
			contentType = "application/x-www-form-urlencoded"
		};
		return unityWebRequest;
	}

	public static UnityWebRequest Post(string uri, List<IMultipartFormSection> multipartFormSections)
	{
		return Post(uri, multipartFormSections, GenerateBoundary());
	}

	public static UnityWebRequest Post(string uri, List<IMultipartFormSection> multipartFormSections, byte[] boundary)
	{
		UnityWebRequest unityWebRequest = new UnityWebRequest(uri, "POST", new DownloadHandlerBuffer(), null);
		byte[] array = ((multipartFormSections != null && multipartFormSections.Count > 0) ? SerializeFormSections(multipartFormSections, boundary) : null);
		if (array != null)
		{
			unityWebRequest.uploadHandler = new UploadHandlerRaw(array)
			{
				contentType = "multipart/form-data; boundary=" + Encoding.UTF8.GetString(boundary)
			};
		}
		return unityWebRequest;
	}

	public static byte[] SerializeFormSections(List<IMultipartFormSection> multipartFormSections, byte[] boundary)
	{
		if (multipartFormSections == null || multipartFormSections.Count == 0)
		{
			return null;
		}
		ReadOnlySpan<byte> source = "\r\n"u8;
		ReadOnlySpan<byte> source2 = "--"u8;
		List<byte> list = new List<byte>();
		foreach (IMultipartFormSection multipartFormSection in multipartFormSections)
		{
			string text = "Content-Disposition: form-data";
			if (!string.IsNullOrEmpty(multipartFormSection.sectionName))
			{
				text = text + "; name=\"" + multipartFormSection.sectionName + "\"";
			}
			if (!string.IsNullOrEmpty(multipartFormSection.fileName))
			{
				text = text + "; filename=\"" + multipartFormSection.fileName + "\"";
			}
			text += "\r\n";
			if (!string.IsNullOrEmpty(multipartFormSection.contentType))
			{
				text = text + "Content-Type: " + multipartFormSection.contentType + "\r\n";
			}
			list.AddRange(source);
			list.AddRange(source2);
			list.AddRange(boundary);
			list.AddRange(source);
			list.AddRange(Encoding.UTF8.GetBytes(text));
			list.AddRange(source);
			list.AddRange(multipartFormSection.sectionData);
		}
		list.AddRange(source);
		list.AddRange(source2);
		list.AddRange(boundary);
		list.AddRange(source2);
		list.AddRange(source);
		return list.ToArray();
	}

	public static string EscapeURL(string s)
	{
		return EscapeURL(s, Encoding.UTF8);
	}

	public static string EscapeURL(string s, Encoding e)
	{
		if (s != null)
		{
			if (s.Length != 0)
			{
				return Uri.EscapeDataString(s).Replace("%20", "+");
			}
			return string.Empty;
		}
		return null;
	}

	public static string UnEscapeURL(string s)
	{
		return UnEscapeURL(s, Encoding.UTF8);
	}

	public static string UnEscapeURL(string s, Encoding e)
	{
		if (s != null)
		{
			if (s.IndexOf('%') >= 0 || s.IndexOf('+') >= 0)
			{
				return Uri.UnescapeDataString(s.Replace('+', ' '));
			}
			return s;
		}
		return null;
	}

	public static byte[] GenerateBoundary()
	{
		byte[] array = new byte[40];
		for (int i = 0; i < array.Length; i++)
		{
			int num = System.Random.Shared.Next(48, 110);
			if (num > 57)
			{
				num += 7;
			}
			if (num > 90)
			{
				num += 6;
			}
			array[i] = (byte)num;
		}
		return array;
	}

	public void SetRequestHeader(string name, string value)
	{
		ThrowIfSent();
		if (string.IsNullOrEmpty(name))
		{
			throw new ArgumentException("Cannot set a Request Header with a null or empty name");
		}
		_requestHeaders.RemoveAll((KeyValuePair<string, string> h) => string.Equals(h.Key, name, StringComparison.OrdinalIgnoreCase));
		_requestHeaders.Add(new KeyValuePair<string, string>(name, value));
	}

	public string GetRequestHeader(string name)
	{
		foreach (var (a, text3) in _requestHeaders)
		{
			if (string.Equals(a, name, StringComparison.OrdinalIgnoreCase))
			{
				return text3;
			}
		}
		return null;
	}

	public string GetResponseHeader(string name)
	{
		PublishIfLanded();
		if (_responseHeaders == null || !_responseHeaders.TryGetValue(name, out var value))
		{
			return null;
		}
		return value;
	}

	public Dictionary<string, string> GetResponseHeaders()
	{
		PublishIfLanded();
		if (_responseHeaders != null)
		{
			return new Dictionary<string, string>(_responseHeaders, StringComparer.OrdinalIgnoreCase);
		}
		return null;
	}

	public UnityWebRequestAsyncOperation SendWebRequest()
	{
		ThrowIfSent();
		_operation = new UnityWebRequestAsyncOperation(this);
		_result = Result.InProgress;
		_transfer = new WebTransfer(new WebTransferRequest
		{
			Url = url,
			Method = method,
			Headers = _requestHeaders.ToList(),
			Body = uploadHandler?.data,
			BodyContentType = uploadHandler?.contentType,
			TimeoutSeconds = timeout,
			CertificateHandler = certificateHandler
		});
		Track(this);
		return _operation;
	}

	public AsyncOperation Send()
	{
		return SendWebRequest();
	}

	public void Abort()
	{
		if (_transfer != null && !_isDone)
		{
			_transfer.Abort();
			Finish(Result.ConnectionError, "Request aborted", 0L);
		}
	}

	public void Dispose()
	{
		Abort();
		if (disposeDownloadHandlerOnDispose)
		{
			downloadHandler?.Dispose();
		}
		if (disposeUploadHandlerOnDispose)
		{
			uploadHandler?.Dispose();
		}
		if (disposeCertificateHandlerOnDispose)
		{
			certificateHandler?.Dispose();
		}
	}

	private void ThrowIfSent()
	{
		if (_transfer != null)
		{
			throw new InvalidOperationException("UnityWebRequest has already been sent and its request headers cannot be changed");
		}
	}

	private void PublishIfLanded()
	{
		if (_isDone || _transfer == null || !_transfer.Task.IsCompleted)
		{
			return;
		}
		WebTransferResponse webTransferResponse = _transfer.Task.Result;
		if (uploadHandler != null)
		{
			uploadHandler.Sent = true;
		}
		if (webTransferResponse.ConnectionError != null)
		{
			Finish(Result.ConnectionError, webTransferResponse.ConnectionError, 0L);
			return;
		}
		_responseHeaders = webTransferResponse.Headers;
		webTransferResponse.Headers.TryGetValue("Content-Type", out var value);
		string text = null;
		if (downloadHandler != null)
		{
			try
			{
				text = downloadHandler.Complete(webTransferResponse.Body, value, webTransferResponse.LocalPath, url);
			}
			catch (Exception ex)
			{
				text = ex.Message;
			}
			downloadHandler.MarkDone(text);
		}
		if (webTransferResponse.StatusCode >= 400)
		{
			Finish(Result.ProtocolError, webTransferResponse.StatusLine, webTransferResponse.StatusCode);
		}
		else if (text != null)
		{
			Finish(Result.DataProcessingError, text, webTransferResponse.StatusCode);
		}
		else
		{
			Finish(Result.Success, null, webTransferResponse.StatusCode);
		}
	}

	private void Finish(Result finalResult, string finalError, long code)
	{
		_result = finalResult;
		_error = finalError;
		_responseCode = code;
		_isDone = true;
	}

	private static void Track(UnityWebRequest request)
	{
		InFlight.Add(request);
		if (!_completionCheckSubscribed)
		{
			UnityWorld unityWorld = UnityRuntime.Current?.World;
			if (unityWorld != null)
			{
				unityWorld.Updating += CheckCompletions;
				_completionCheckSubscribed = true;
			}
		}
	}

	private static void CheckCompletions(UnityFrame frame)
	{
		for (int num = InFlight.Count - 1; num >= 0; num--)
		{
			UnityWebRequest unityWebRequest = InFlight[num];
			if (unityWebRequest.isDone)
			{
				InFlight.RemoveAt(num);
				unityWebRequest._operation.RaiseCompleted();
			}
		}
	}
}
