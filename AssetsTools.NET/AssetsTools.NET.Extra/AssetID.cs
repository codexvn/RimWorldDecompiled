using System;

namespace AssetsTools.NET.Extra;

[Obsolete("use AssetPPtr")]
public class AssetID
{
	public string fileName;

	public long pathID;

	public AssetID(string fileName, long pathID)
	{
		this.fileName = fileName;
		this.pathID = pathID;
	}

	public override bool Equals(object obj)
	{
		if (!(obj is AssetID))
		{
			return false;
		}
		AssetID assetID = (AssetID)obj;
		if (assetID.fileName == fileName)
		{
			return assetID.pathID == pathID;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return (17 * 23 + fileName.GetHashCode()) * 23 + pathID.GetHashCode();
	}
}
