namespace UnityEngine.Networking;

public class UnityWebRequestTexture
{
	public static UnityWebRequest GetTexture(string uri)
	{
		return GetTexture(uri, nonReadable: false);
	}

	public static UnityWebRequest GetTexture(string uri, bool nonReadable)
	{
		return new UnityWebRequest(uri, "GET", new DownloadHandlerTexture(!nonReadable), null);
	}
}
