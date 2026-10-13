namespace UnityEngine.Networking;

public class UnityWebRequestAsyncOperation : AsyncOperation
{
	public UnityWebRequest webRequest { get; internal set; }

	internal override bool IsDoneCore => webRequest.isDone;

	internal override float ProgressCore
	{
		get
		{
			if (!webRequest.isDone)
			{
				return webRequest.downloadProgress;
			}
			return 1f;
		}
	}

	internal UnityWebRequestAsyncOperation(UnityWebRequest request)
	{
		webRequest = request;
	}
}
