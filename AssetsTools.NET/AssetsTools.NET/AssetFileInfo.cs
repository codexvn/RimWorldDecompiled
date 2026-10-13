using System;
using System.Collections.Generic;
using AssetsTools.NET.Extra;

namespace AssetsTools.NET;

public class AssetFileInfo
{
	public long PathId { get; set; }

	public long ByteOffset { get; set; }

	public uint ByteSize { get; set; }

	public int TypeIdOrIndex { get; set; }

	public ushort OldTypeId { get; set; }

	public ushort ScriptTypeIndex { get; set; }

	public byte Stripped { get; set; }

	public int TypeId { get; set; }

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

	public void Read(AssetsFileReader reader, uint version)
	{
		reader.Align();
		if (version >= 14)
		{
			PathId = reader.ReadInt64();
		}
		else
		{
			PathId = reader.ReadUInt32();
		}
		if (version >= 22)
		{
			ByteOffset = reader.ReadInt64();
		}
		else
		{
			ByteOffset = reader.ReadUInt32();
		}
		ByteSize = reader.ReadUInt32();
		TypeIdOrIndex = reader.ReadInt32();
		if (version <= 15)
		{
			OldTypeId = reader.ReadUInt16();
		}
		if (version <= 16)
		{
			ScriptTypeIndex = reader.ReadUInt16();
		}
		if (15 <= version && version <= 16)
		{
			Stripped = reader.ReadByte();
		}
		Replacer = null;
	}

	public void Write(AssetsFileWriter writer, uint version)
	{
		writer.Align();
		if (version >= 14)
		{
			writer.Write(PathId);
		}
		else
		{
			writer.Write((uint)PathId);
		}
		if (version >= 22)
		{
			writer.Write(ByteOffset);
		}
		else
		{
			writer.Write((uint)ByteOffset);
		}
		writer.Write(ByteSize);
		writer.Write(TypeIdOrIndex);
		if (version <= 15)
		{
			writer.Write(OldTypeId);
		}
		if (version <= 16)
		{
			writer.Write(ScriptTypeIndex);
		}
		if (15 <= version && version <= 16)
		{
			writer.Write(Stripped);
		}
	}

	public int GetTypeId(AssetsFile assetsFile)
	{
		return GetTypeId(assetsFile.Metadata.TypeTreeTypes, assetsFile.Header.Version);
	}

	public int GetTypeId(AssetsFileMetadata metadata, uint version)
	{
		return GetTypeId(metadata.TypeTreeTypes, version);
	}

	public int GetTypeId(List<TypeTreeType> typeTreeTypes, uint version)
	{
		if (version < 16)
		{
			return TypeIdOrIndex;
		}
		if (TypeIdOrIndex >= typeTreeTypes.Count)
		{
			throw new IndexOutOfRangeException("TypeIndex is larger than type tree count!");
		}
		return typeTreeTypes[TypeIdOrIndex].TypeId;
	}

	public long GetAbsoluteByteOffset(AssetsFile assetsFile)
	{
		return assetsFile.Header.DataOffset + ByteOffset;
	}

	public long GetAbsoluteByteOffset(AssetsFileHeader header)
	{
		return header.DataOffset + ByteOffset;
	}

	public long GetAbsoluteByteOffset(long dataOffset)
	{
		return dataOffset + ByteOffset;
	}

	public void SetNewData(byte[] newBytes)
	{
		Replacer = new ContentReplacerFromBuffer(newBytes);
	}

	public void SetNewData(AssetTypeValueField baseField)
	{
		Replacer = new ContentReplacerFromBuffer(baseField.WriteToByteArray());
	}

	public void SetRemoved()
	{
		Replacer = new ContentRemover();
	}

	public static AssetFileInfo Create(AssetsFile assetsFile, long pathId, int typeId, ClassDatabaseFile classDatabase = null, bool preferEditor = false)
	{
		return Create(assetsFile, pathId, typeId, ushort.MaxValue, classDatabase, preferEditor);
	}

	public static AssetFileInfo Create(AssetsFile assetsFile, long pathId, int typeId, ushort scriptIndex = ushort.MaxValue, ClassDatabaseFile classDatabase = null, bool preferEditor = false)
	{
		uint version = assetsFile.Header.Version;
		int typeIdOrIndex;
		ushort oldTypeId;
		if (version >= 16)
		{
			int num = assetsFile.Metadata.FindTypeTreeTypeIndexByID(typeId, scriptIndex);
			if (num == -1)
			{
				if (classDatabase == null)
				{
					return null;
				}
				TypeTreeType typeTreeType;
				if (assetsFile.Metadata.TypeTreeEnabled)
				{
					typeTreeType = ClassDatabaseToTypeTree.Convert(classDatabase, typeId, preferEditor);
					if (typeTreeType == null)
					{
						return null;
					}
				}
				else
				{
					typeTreeType = new TypeTreeType
					{
						TypeId = typeId,
						IsStrippedType = false,
						ScriptTypeIndex = scriptIndex,
						ScriptIdHash = Hash128.NewBlankHash(),
						TypeHash = Hash128.NewBlankHash(),
						Nodes = new List<TypeTreeNode>(),
						StringBufferBytes = new byte[0],
						TypeDependencies = new int[0],
						IsRefType = false,
						TypeReference = null
					};
				}
				typeIdOrIndex = assetsFile.Metadata.TypeTreeTypes.Count;
				assetsFile.Metadata.TypeTreeTypes.Add(typeTreeType);
			}
			else
			{
				typeIdOrIndex = num;
			}
			oldTypeId = 0;
		}
		else
		{
			typeIdOrIndex = typeId;
			oldTypeId = (ushort)((typeId >= 0) ? ((ushort)typeId) : 114);
		}
		ushort scriptTypeIndex = (ushort)((version < 17) ? scriptIndex : 0);
		return new AssetFileInfo
		{
			PathId = pathId,
			ByteOffset = -1L,
			ByteSize = 0u,
			TypeIdOrIndex = typeIdOrIndex,
			OldTypeId = oldTypeId,
			ScriptTypeIndex = scriptTypeIndex,
			Stripped = 0,
			TypeId = typeId,
			Replacer = null
		};
	}
}
