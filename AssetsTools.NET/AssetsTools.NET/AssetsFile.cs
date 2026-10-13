using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using AssetsTools.NET.Extra;

namespace AssetsTools.NET;

public class AssetsFile
{
	public AssetsFileHeader Header { get; set; }

	public AssetsFileMetadata Metadata { get; set; }

	public AssetsFileReader Reader { get; set; }

	public IList<AssetFileInfo> AssetInfos => Metadata.AssetInfos;

	public void Close()
	{
		Reader.Close();
	}

	public void Read(AssetsFileReader reader)
	{
		Reader = reader;
		Header = new AssetsFileHeader();
		Header.Read(reader);
		Metadata = new AssetsFileMetadata();
		Metadata.Read(reader, Header);
	}

	public void Read(Stream stream)
	{
		Read(new AssetsFileReader(stream));
	}

	public void Write(AssetsFileWriter writer, long filePos = 0L)
	{
		long num = filePos;
		if (filePos == -1)
		{
			num = writer.Position;
		}
		else
		{
			writer.Position = filePos;
		}
		Header.Write(writer);
		List<AssetFileInfo> list = new List<AssetFileInfo>();
		int count = Metadata.AssetInfos.Count;
		for (int j = 0; j < count; j++)
		{
			AssetFileInfo assetFileInfo = Metadata.AssetInfos[j];
			ContentReplacerType replacerType = assetFileInfo.ReplacerType;
			if (replacerType != ContentReplacerType.Remove)
			{
				if (replacerType == ContentReplacerType.AddOrModify && assetFileInfo.Replacer == null)
				{
					throw new Exception("Replacer must be non-null when status is Modified!");
				}
				list.Add(new AssetFileInfo
				{
					PathId = assetFileInfo.PathId,
					ByteOffset = assetFileInfo.ByteOffset,
					ByteSize = assetFileInfo.ByteSize,
					TypeIdOrIndex = assetFileInfo.TypeIdOrIndex,
					OldTypeId = assetFileInfo.OldTypeId,
					ScriptTypeIndex = assetFileInfo.ScriptTypeIndex,
					Stripped = assetFileInfo.Stripped,
					Replacer = assetFileInfo.Replacer
				});
			}
		}
		list.Sort((AssetFileInfo i1, AssetFileInfo i2) => i1.PathId.CompareTo(i2.PathId));
		AssetsFileMetadata assetsFileMetadata = new AssetsFileMetadata
		{
			UnityVersion = Metadata.UnityVersion,
			TargetPlatform = Metadata.TargetPlatform,
			TypeTreeEnabled = Metadata.TypeTreeEnabled,
			TypeTreeTypes = Metadata.TypeTreeTypes,
			AssetInfos = list,
			ScriptTypes = Metadata.ScriptTypes,
			Externals = Metadata.Externals,
			RefTypes = Metadata.RefTypes,
			UserInformation = Metadata.UserInformation
		};
		long position = writer.Position;
		assetsFileMetadata.Write(writer, Header.Version);
		int num2 = (int)(writer.Position - position);
		if (writer.Position < 4096)
		{
			while (writer.Position < 4096)
			{
				writer.Write((byte)0);
			}
		}
		else if (writer.Position % 16 == 0L)
		{
			writer.Position += 16L;
		}
		else
		{
			writer.Align16();
		}
		long position2 = writer.Position;
		for (int k = 0; k < list.Count; k++)
		{
			AssetFileInfo assetFileInfo2 = list[k];
			long position3 = writer.Position;
			long byteOffset = position3 - position2;
			if (assetFileInfo2.ReplacerType == ContentReplacerType.AddOrModify)
			{
				assetFileInfo2.Replacer.Write(writer);
			}
			else
			{
				Reader.Position = assetFileInfo2.GetAbsoluteByteOffset(this);
				Reader.BaseStream.CopyToCompat(writer.BaseStream, assetFileInfo2.ByteSize);
			}
			assetFileInfo2.ByteOffset = byteOffset;
			assetFileInfo2.ByteSize = (uint)(writer.Position - position3);
			if (k != list.Count - 1)
			{
				writer.Align8();
			}
		}
		long num3 = writer.Position - num;
		AssetsFileHeader obj = new AssetsFileHeader
		{
			MetadataSize = num2,
			FileSize = num3,
			Version = Header.Version,
			DataOffset = position2,
			Endianness = Header.Endianness
		};
		writer.Position = num;
		obj.Write(writer);
		writer.Position = position;
		assetsFileMetadata.Write(writer, Header.Version);
		writer.Position = num + num3;
	}

	public ushort GetScriptIndex(AssetFileInfo info)
	{
		if (Header.Version < 16)
		{
			return info.ScriptTypeIndex;
		}
		return Metadata.TypeTreeTypes[info.TypeIdOrIndex].ScriptTypeIndex;
	}

	public static bool IsAssetsFile(string filePath)
	{
		using AssetsFileReader assetsFileReader = new AssetsFileReader(filePath);
		return IsAssetsFile(assetsFileReader, 0L, assetsFileReader.BaseStream.Length);
	}

	public static bool IsAssetsFile(AssetsFileReader reader, long offset, long length)
	{
		reader.BigEndian = true;
		if (length < 48)
		{
			return false;
		}
		reader.Position = offset;
		if (reader.ReadStringLength(5) == "Unity")
		{
			return false;
		}
		reader.Position = offset + 8;
		int num = reader.ReadInt32();
		if (num > 99)
		{
			return false;
		}
		reader.Position = offset + 20;
		if (num >= 22)
		{
			reader.Position += 28L;
		}
		string text = "";
		char c;
		while (reader.Position < reader.BaseStream.Length && (c = (char)reader.ReadByte()) != 0)
		{
			text += c;
			if (text.Length > 255)
			{
				return false;
			}
		}
		string text2 = Regex.Replace(text, "[a-zA-Z0-9\\.\\n]", "");
		string text3 = Regex.Replace(text, "[^a-zA-Z0-9\\.\\n]", "");
		if (text2 == "")
		{
			return text3.Length > 0;
		}
		return false;
	}

	public AssetFileInfo GetAssetInfo(long pathId)
	{
		return Metadata.GetAssetInfo(pathId);
	}

	public void GenerateQuickLookup()
	{
		Metadata.GenerateQuickLookup();
	}

	public List<AssetFileInfo> GetAssetsOfType(int typeId)
	{
		return Metadata.GetAssetsOfType(typeId);
	}

	public List<AssetFileInfo> GetAssetsOfType(AssetClassID typeId)
	{
		return Metadata.GetAssetsOfType(typeId);
	}

	public List<AssetFileInfo> GetAssetsOfType(int typeId, ushort scriptIndex)
	{
		return Metadata.GetAssetsOfType(typeId, scriptIndex);
	}

	public List<AssetFileInfo> GetAssetsOfType(AssetClassID typeId, ushort scriptIndex)
	{
		return Metadata.GetAssetsOfType(typeId, scriptIndex);
	}
}
