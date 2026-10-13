using System.Collections.Generic;

namespace AssetsTools.NET;

public class AssetBundleBlockAndDirInfo
{
	public Hash128 Hash { get; set; }

	public AssetBundleBlockInfo[] BlockInfos { get; set; }

	public List<AssetBundleDirectoryInfo> DirectoryInfos { get; set; }

	public void Read(AssetsFileReader reader)
	{
		Hash = new Hash128(reader.ReadBytes(16));
		int num = reader.ReadInt32();
		BlockInfos = new AssetBundleBlockInfo[num];
		for (int i = 0; i < num; i++)
		{
			BlockInfos[i] = new AssetBundleBlockInfo();
			BlockInfos[i].DecompressedSize = reader.ReadUInt32();
			BlockInfos[i].CompressedSize = reader.ReadUInt32();
			BlockInfos[i].Flags = reader.ReadUInt16();
		}
		int num2 = reader.ReadInt32();
		DirectoryInfos = new List<AssetBundleDirectoryInfo>(num2);
		for (int j = 0; j < num2; j++)
		{
			AssetBundleDirectoryInfo item = new AssetBundleDirectoryInfo
			{
				Offset = reader.ReadInt64(),
				DecompressedSize = reader.ReadInt64(),
				Flags = reader.ReadUInt32(),
				Name = reader.ReadNullTerminated()
			};
			DirectoryInfos.Add(item);
		}
	}

	public void Write(AssetsFileWriter writer)
	{
		if (Hash.data == null)
		{
			writer.Write(0uL);
			writer.Write(0uL);
		}
		else
		{
			writer.Write(Hash.data);
		}
		int num = BlockInfos.Length;
		writer.Write(num);
		for (int i = 0; i < num; i++)
		{
			writer.Write(BlockInfos[i].DecompressedSize);
			writer.Write(BlockInfos[i].CompressedSize);
			writer.Write(BlockInfos[i].Flags);
		}
		int count = DirectoryInfos.Count;
		writer.Write(count);
		for (int j = 0; j < count; j++)
		{
			writer.Write(DirectoryInfos[j].Offset);
			writer.Write(DirectoryInfos[j].DecompressedSize);
			writer.Write(DirectoryInfos[j].Flags);
			writer.WriteNullTerminated(DirectoryInfos[j].Name);
		}
	}
}
