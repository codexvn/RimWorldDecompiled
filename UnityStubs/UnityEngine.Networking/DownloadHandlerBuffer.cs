namespace UnityEngine.Networking;

public class DownloadHandlerBuffer : DownloadHandler
{
	public static string GetContent(UnityWebRequest www)
	{
		return www.downloadHandler?.text;
	}
}
