using System;
using System.IO;

namespace UnityEngine.Networking;

public class DownloadHandlerAudioClip : DownloadHandler
{
	public bool streamAudio { get; set; }

	public bool compressed { get; set; }

	public AudioClip audioClip { get; private set; }

	public DownloadHandlerAudioClip(string url, AudioType audioType)
	{
	}

	public DownloadHandlerAudioClip(Uri uri, AudioType audioType)
	{
	}

	public static AudioClip GetContent(UnityWebRequest www)
	{
		if (!(www.downloadHandler is DownloadHandlerAudioClip downloadHandlerAudioClip))
		{
			throw new InvalidOperationException("Cannot get content from an unsupported DownloadHandler");
		}
		return downloadHandlerAudioClip.audioClip;
	}

	internal override string Complete(byte[] body, string contentType, string localPath, string url)
	{
		base.Complete(body, contentType, localPath, url);
		try
		{
			audioClip = ((localPath != null) ? AudioClip.LoadFromFile(localPath) : AudioClip.LoadFromMemory(body, Path.GetFileNameWithoutExtension(url)));
			return null;
		}
		catch (Exception ex)
		{
			return "Failed to decode audio clip: " + ex.Message;
		}
	}
}
