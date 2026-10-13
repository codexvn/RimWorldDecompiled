using System;
using System.Net.Http.Headers;
using System.Text;

namespace UnityEngine.Networking;

public class DownloadHandler : IDisposable
{
	private byte[] _data;

	private string _contentType;

	public bool isDone { get; private set; }

	public string error { get; private set; }

	public string text
	{
		get
		{
			if (_data != null)
			{
				return ResolveEncoding(_contentType).GetString(_data);
			}
			return null;
		}
	}

	public byte[] data => _data;

	public void Dispose()
	{
	}

	internal virtual string Complete(byte[] body, string contentType, string localPath, string url)
	{
		_data = body;
		_contentType = contentType;
		return null;
	}

	internal void MarkDone(string handlerError)
	{
		error = handlerError;
		isDone = true;
	}

	private static Encoding ResolveEncoding(string contentType)
	{
		if (contentType != null && MediaTypeHeaderValue.TryParse(contentType, out MediaTypeHeaderValue parsedValue) && !string.IsNullOrEmpty(parsedValue.CharSet))
		{
			try
			{
				return Encoding.GetEncoding(parsedValue.CharSet.Trim('"'));
			}
			catch (ArgumentException)
			{
			}
		}
		return Encoding.UTF8;
	}
}
