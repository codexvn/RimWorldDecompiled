namespace UnityEngine.Networking;

public class UploadHandlerRaw : UploadHandler
{
	public UploadHandlerRaw(byte[] data)
	{
		Payload = data;
	}
}
