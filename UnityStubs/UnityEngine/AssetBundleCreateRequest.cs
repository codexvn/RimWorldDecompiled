namespace UnityEngine;

public class AssetBundleCreateRequest : AsyncOperation
{
	public AssetBundle assetBundle { get; internal set; }

	internal AssetBundleCreateRequest(AssetBundle bundle)
		: base(isDone: true)
	{
		assetBundle = bundle;
	}
}
