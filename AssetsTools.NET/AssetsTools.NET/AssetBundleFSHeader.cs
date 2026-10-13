namespace AssetsTools.NET;

public class AssetBundleFSHeader
{
	public long TotalFileSize { get; set; }

	public uint CompressedSize { get; set; }

	public uint DecompressedSize { get; set; }

	public AssetBundleFSHeaderFlags Flags { get; set; }

	public void Read(AssetsFileReader reader)
	{
		TotalFileSize = reader.ReadInt64();
		CompressedSize = reader.ReadUInt32();
		DecompressedSize = reader.ReadUInt32();
		Flags = (AssetBundleFSHeaderFlags)reader.ReadUInt32();
	}

	public void Write(AssetsFileWriter writer)
	{
		writer.Write(TotalFileSize);
		writer.Write(CompressedSize);
		writer.Write(DecompressedSize);
		writer.Write((uint)Flags);
	}
}
