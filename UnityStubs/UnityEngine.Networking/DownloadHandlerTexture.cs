using System;

namespace UnityEngine.Networking;

public class DownloadHandlerTexture : DownloadHandler
{
	private readonly bool _readable;

	public Texture2D texture { get; private set; }

	public DownloadHandlerTexture()
		: this(readable: true)
	{
	}

	public DownloadHandlerTexture(bool readable)
	{
		_readable = readable;
	}

	public static Texture2D GetContent(UnityWebRequest www)
	{
		if (!(www.downloadHandler is DownloadHandlerTexture downloadHandlerTexture))
		{
			throw new InvalidOperationException("Cannot get content from an unsupported DownloadHandler");
		}
		return downloadHandlerTexture.texture;
	}

	internal override string Complete(byte[] body, string contentType, string localPath, string url)
	{
		base.Complete(body, contentType, localPath, url);
		Texture2D texture2D = new Texture2D(2, 2);
		if (!texture2D.LoadImage(body, !_readable))
		{
			Object.Destroy(texture2D);
			return "Failed to decode texture";
		}
		texture = texture2D;
		return null;
	}
}
