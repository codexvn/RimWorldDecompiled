using System;

namespace AssetsTools.NET;

public class AssetBundleHeader
{
	public string Signature { get; set; }

	public uint Version { get; set; }

	public string GenerationVersion { get; set; }

	public string EngineVersion { get; set; }

	public AssetBundleFSHeader FileStreamHeader { get; set; }

	public void Read(AssetsFileReader reader)
	{
		reader.BigEndian = true;
		Signature = reader.ReadNullTerminated();
		Version = reader.ReadUInt32();
		GenerationVersion = reader.ReadNullTerminated();
		EngineVersion = reader.ReadNullTerminated();
		if (Signature == "UnityFS")
		{
			FileStreamHeader = new AssetBundleFSHeader();
			FileStreamHeader.Read(reader);
			return;
		}
		throw new NotSupportedException(Signature + " signature not supported!");
	}

	public void Write(AssetsFileWriter writer)
	{
		writer.BigEndian = true;
		writer.WriteNullTerminated(Signature);
		writer.Write(Version);
		writer.WriteNullTerminated(GenerationVersion);
		writer.WriteNullTerminated(EngineVersion);
		if (Signature == "UnityFS")
		{
			FileStreamHeader.Write(writer);
			return;
		}
		throw new NotSupportedException(Signature + " signature not supported!");
	}

	public long GetBundleInfoOffset()
	{
		if (Signature != "UnityFS")
		{
			throw new NotSupportedException(Signature + " signature not supported!");
		}
		AssetBundleFSHeaderFlags flags = FileStreamHeader.Flags;
		long totalFileSize = FileStreamHeader.TotalFileSize;
		long num = FileStreamHeader.CompressedSize;
		if ((flags & AssetBundleFSHeaderFlags.BlockAndDirAtEnd) != 0)
		{
			if (totalFileSize == 0L)
			{
				return -1L;
			}
			return totalFileSize - num;
		}
		long num2 = GenerationVersion.Length + EngineVersion.Length + 26;
		if (Version >= 7)
		{
			if ((flags & AssetBundleFSHeaderFlags.OldWebPluginCompatibility) != 0)
			{
				return (num2 + 10 + 15) & -16;
			}
			return (num2 + Signature.Length + 1 + 15) & -16;
		}
		if ((flags & AssetBundleFSHeaderFlags.OldWebPluginCompatibility) != 0)
		{
			return num2 + 10;
		}
		return num2 + Signature.Length + 1;
	}

	public long GetFileDataOffset()
	{
		if (Signature != "UnityFS")
		{
			throw new NotSupportedException(Signature + " signature not supported!");
		}
		AssetBundleFSHeaderFlags flags = FileStreamHeader.Flags;
		long num = FileStreamHeader.CompressedSize;
		long num2 = GenerationVersion.Length + EngineVersion.Length + 26;
		num2 = (((flags & AssetBundleFSHeaderFlags.OldWebPluginCompatibility) == 0) ? (num2 + (Signature.Length + 1)) : (num2 + 10));
		if (Version >= 7)
		{
			num2 = (num2 + 15) & -16;
		}
		if ((flags & AssetBundleFSHeaderFlags.BlockAndDirAtEnd) == 0)
		{
			num2 += num;
		}
		if ((flags & AssetBundleFSHeaderFlags.BlockInfoNeedPaddingAtStart) != 0)
		{
			num2 = (num2 + 15) & -16;
		}
		return num2;
	}

	public byte GetCompressionType()
	{
		if (Signature != "UnityFS")
		{
			throw new NotSupportedException(Signature + " signature not supported!");
		}
		return (byte)(FileStreamHeader.Flags & AssetBundleFSHeaderFlags.CompressionMask);
	}
}
