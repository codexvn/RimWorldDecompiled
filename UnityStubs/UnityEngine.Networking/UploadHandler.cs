using System;

namespace UnityEngine.Networking;

public class UploadHandler : IDisposable
{
	internal byte[] Payload;

	internal bool Sent;

	public string contentType { get; set; }

	public byte[] data => Payload;

	public float progress
	{
		get
		{
			if (!Sent)
			{
				return 0f;
			}
			return 1f;
		}
	}

	public void Dispose()
	{
	}
}
