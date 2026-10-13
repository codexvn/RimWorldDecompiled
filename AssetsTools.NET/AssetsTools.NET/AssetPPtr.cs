using AssetsTools.NET.Extra;

namespace AssetsTools.NET;

public class AssetPPtr
{
	public string FilePath { get; set; }

	public int FileId { get; set; }

	public long PathId { get; set; }

	public AssetPPtr()
	{
		FilePath = string.Empty;
		FileId = 0;
		PathId = 0L;
	}

	public AssetPPtr(int fileId, long pathId)
	{
		FilePath = string.Empty;
		FileId = fileId;
		PathId = pathId;
	}

	public AssetPPtr(string fileName, long pathId)
	{
		FilePath = fileName;
		FileId = 0;
		PathId = pathId;
	}

	public AssetPPtr(string fileName, int fileId, long pathId)
	{
		FilePath = fileName;
		FileId = fileId;
		PathId = pathId;
	}

	public bool HasFilePath()
	{
		if (FilePath != string.Empty)
		{
			return FilePath != null;
		}
		return false;
	}

	public bool IsNull()
	{
		if (HasFilePath())
		{
			return PathId == 0;
		}
		if (FileId == 0)
		{
			return PathId == 0;
		}
		return false;
	}

	public void SetFilePathFromFile(AssetsFile file)
	{
		int num = FileId - 1;
		if (FileId > 0 && file.Metadata.Externals.Count < num)
		{
			FilePath = file.Metadata.Externals[num].PathName;
		}
	}

	public void SetFilePathFromFile(AssetsManager am, AssetsFileInstance fileInst)
	{
		if (FileId == 0)
		{
			FilePath = fileInst.path;
			return;
		}
		int depIdx = FileId - 1;
		AssetsFileInstance dependency = fileInst.GetDependency(am, depIdx);
		if (dependency != null)
		{
			FilePath = dependency.path;
		}
	}

	public static AssetPPtr FromField(AssetTypeValueField field)
	{
		return new AssetPPtr(field["m_FileID"].AsInt, field["m_PathID"].AsLong);
	}

	public override bool Equals(object obj)
	{
		if (!(obj is AssetPPtr))
		{
			return false;
		}
		AssetPPtr assetPPtr = (AssetPPtr)obj;
		if (assetPPtr.HasFilePath() && HasFilePath())
		{
			if (assetPPtr.PathId == PathId)
			{
				return assetPPtr.FilePath == FilePath;
			}
			return false;
		}
		if (!assetPPtr.HasFilePath() && !HasFilePath())
		{
			if (assetPPtr.PathId == PathId)
			{
				return assetPPtr.FileId == FileId;
			}
			return false;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return (17 * 23 + (HasFilePath() ? FilePath.GetHashCode() : FileId.GetHashCode())) * 23 + PathId.GetHashCode();
	}
}
