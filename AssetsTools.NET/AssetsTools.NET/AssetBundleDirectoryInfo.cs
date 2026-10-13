namespace AssetsTools.NET;

public class AssetBundleDirectoryInfo
{
	public long Offset;

	public long DecompressedSize;

	public uint Flags;

	public string Name;

	public IContentReplacer Replacer { get; set; }

	public ContentReplacerType ReplacerType
	{
		get
		{
			if (Replacer == null)
			{
				return ContentReplacerType.None;
			}
			return Replacer.GetReplacerType();
		}
	}

	public bool IsReplacerPreviewable
	{
		get
		{
			if (Replacer != null)
			{
				return Replacer.HasPreview();
			}
			return false;
		}
	}

	public void SetNewData(byte[] newBytes)
	{
		Replacer = new ContentReplacerFromBuffer(newBytes);
	}

	public void SetNewData(AssetsFile assetsFile)
	{
		Replacer = new ContentReplacerFromAssets(assetsFile);
	}

	public void SetRemoved()
	{
		Replacer = new ContentRemover();
	}

	public static AssetBundleDirectoryInfo Create(string name, bool isSerialized)
	{
		return new AssetBundleDirectoryInfo
		{
			Offset = -1L,
			DecompressedSize = 0L,
			Flags = (isSerialized ? 4u : 0u),
			Name = name
		};
	}
}
