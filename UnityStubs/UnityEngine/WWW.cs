using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Networking;

namespace UnityEngine;

public class WWW : CustomYieldInstruction, IDisposable
{
	private sealed class WWWDownloadHandler : DownloadHandler
	{
		public string LocalPath;

		internal override string Complete(byte[] body, string contentType, string localPath, string url)
		{
			LocalPath = localPath;
			return base.Complete(body, contentType, localPath, url);
		}
	}

	private readonly UnityWebRequest _uwr;

	public string url => _uwr.url;

	public bool isDone => _uwr.isDone;

	public string error => _uwr.error;

	public float progress => Math.Max(0f, _uwr.downloadProgress);

	public float uploadProgress => Math.Max(0f, _uwr.uploadProgress);

	public int bytesDownloaded => (int)_uwr.downloadedBytes;

	public byte[] bytes
	{
		get
		{
			if (!isDone)
			{
				return Array.Empty<byte>();
			}
			return _uwr.downloadHandler.data ?? Array.Empty<byte>();
		}
	}

	public string text
	{
		get
		{
			if (!isDone)
			{
				return "";
			}
			return _uwr.downloadHandler.text ?? "";
		}
	}

	public Dictionary<string, string> responseHeaders
	{
		get
		{
			if (!isDone)
			{
				return new Dictionary<string, string>();
			}
			return _uwr.GetResponseHeaders() ?? new Dictionary<string, string>();
		}
	}

	public ThreadPriority threadPriority { get; set; }

	public override bool keepWaiting => !_uwr.isDone;

	public Texture2D texture => CreateTexture(readable: true);

	public Texture2D textureNonReadable => CreateTexture(readable: false);

	public WWW(string url)
		: this(UnityWebRequest.Get(url), null)
	{
	}

	public WWW(string url, WWWForm form)
		: this(UnityWebRequest.Post(url, form), null)
	{
	}

	public WWW(string url, byte[] postData)
		: this(url, postData, null)
	{
	}

	public WWW(string url, byte[] postData, Dictionary<string, string> headers)
		: this(new UnityWebRequest(url, (postData == null) ? "GET" : "POST", null, (postData == null) ? null : new UploadHandlerRaw(postData)), headers)
	{
	}

	private WWW(UnityWebRequest request, Dictionary<string, string> headers)
	{
		_uwr = request;
		_uwr.downloadHandler = new WWWDownloadHandler();
		if (headers != null)
		{
			foreach (KeyValuePair<string, string> header in headers)
			{
				_uwr.SetRequestHeader(header.Key, header.Value);
			}
		}
		_uwr.SendWebRequest();
	}

	private Texture2D CreateTexture(bool readable)
	{
		Texture2D texture2D = new Texture2D(2, 2);
		texture2D.LoadImage(bytes, !readable);
		return texture2D;
	}

	public void LoadImageIntoTexture(Texture2D texture)
	{
		texture.LoadImage(bytes);
	}

	public AudioClip GetAudioClip()
	{
		return GetAudioClip(threeD: true, stream: false, AudioType.UNKNOWN);
	}

	public AudioClip GetAudioClip(bool threeD)
	{
		return GetAudioClip(threeD, stream: false, AudioType.UNKNOWN);
	}

	public AudioClip GetAudioClip(bool threeD, bool stream)
	{
		return GetAudioClip(threeD, stream, AudioType.UNKNOWN);
	}

	public AudioClip GetAudioClip(bool threeD, bool stream, AudioType audioType)
	{
		if (!isDone || error != null)
		{
			return null;
		}
		WWWDownloadHandler wWWDownloadHandler = (WWWDownloadHandler)_uwr.downloadHandler;
		if (wWWDownloadHandler.LocalPath == null)
		{
			return AudioClip.LoadFromMemory(wWWDownloadHandler.data, Path.GetFileNameWithoutExtension(url));
		}
		return AudioClip.LoadFromFile(wWWDownloadHandler.LocalPath);
	}

	public void Dispose()
	{
		_uwr.Dispose();
	}
}
