using System.Collections.Generic;
using AssetsTools.NET.Extra;

namespace AssetsTools.NET;

public class AssetsFileMetadata
{
	private Dictionary<long, int> _quickLookup;

	public string UnityVersion { get; set; }

	public uint TargetPlatform { get; set; }

	public bool TypeTreeEnabled { get; set; }

	public List<TypeTreeType> TypeTreeTypes { get; set; }

	public IList<AssetFileInfo> AssetInfos { get; set; }

	public List<AssetPPtr> ScriptTypes { get; set; }

	public List<AssetsFileExternal> Externals { get; set; }

	public List<TypeTreeType> RefTypes { get; set; }

	public string UserInformation { get; set; }

	public void Read(AssetsFileReader reader, AssetsFileHeader header)
	{
		Read(reader, header.Version);
	}

	public void Read(AssetsFileReader reader, uint version)
	{
		_quickLookup = null;
		UnityVersion = reader.ReadNullTerminated();
		TargetPlatform = reader.ReadUInt32();
		if (version >= 13)
		{
			TypeTreeEnabled = reader.ReadBoolean();
		}
		int num = reader.ReadInt32();
		TypeTreeTypes = new List<TypeTreeType>(num);
		for (int i = 0; i < num; i++)
		{
			TypeTreeType typeTreeType = new TypeTreeType();
			typeTreeType.Read(reader, version, TypeTreeEnabled, isRefType: false);
			TypeTreeTypes.Add(typeTreeType);
		}
		int num2 = reader.ReadInt32();
		reader.Align();
		AssetInfos = new List<AssetFileInfo>(num2);
		for (int j = 0; j < num2; j++)
		{
			AssetFileInfo assetFileInfo = new AssetFileInfo();
			assetFileInfo.Read(reader, version);
			assetFileInfo.TypeId = assetFileInfo.GetTypeId(this, version);
			AssetInfos.Add(assetFileInfo);
		}
		int num3 = reader.ReadInt32();
		ScriptTypes = new List<AssetPPtr>(num3);
		for (int k = 0; k < num3; k++)
		{
			int fileId = reader.ReadInt32();
			reader.Align();
			long pathId = reader.ReadInt64();
			AssetPPtr item = new AssetPPtr(fileId, pathId);
			ScriptTypes.Add(item);
		}
		int num4 = reader.ReadInt32();
		Externals = new List<AssetsFileExternal>(num4);
		for (int l = 0; l < num4; l++)
		{
			AssetsFileExternal assetsFileExternal = new AssetsFileExternal();
			assetsFileExternal.Read(reader);
			Externals.Add(assetsFileExternal);
		}
		if (version >= 20)
		{
			int num5 = reader.ReadInt32();
			RefTypes = new List<TypeTreeType>(num5);
			for (int m = 0; m < num5; m++)
			{
				TypeTreeType typeTreeType2 = new TypeTreeType();
				typeTreeType2.Read(reader, version, TypeTreeEnabled, isRefType: true);
				RefTypes.Add(typeTreeType2);
			}
		}
		if (version >= 5)
		{
			UserInformation = reader.ReadNullTerminated();
		}
	}

	public void Write(AssetsFileWriter writer, uint version)
	{
		writer.WriteNullTerminated(UnityVersion);
		writer.Write(TargetPlatform);
		if (version >= 13)
		{
			writer.Write(TypeTreeEnabled);
		}
		writer.Write(TypeTreeTypes.Count);
		for (int i = 0; i < TypeTreeTypes.Count; i++)
		{
			TypeTreeTypes[i].Write(writer, version, TypeTreeEnabled);
		}
		writer.Write(AssetInfos.Count);
		writer.Align();
		for (int j = 0; j < AssetInfos.Count; j++)
		{
			AssetInfos[j].Write(writer, version);
		}
		writer.Write(ScriptTypes.Count);
		for (int k = 0; k < ScriptTypes.Count; k++)
		{
			writer.Write(ScriptTypes[k].FileId);
			writer.Align();
			writer.Write(ScriptTypes[k].PathId);
		}
		writer.Write(Externals.Count);
		for (int l = 0; l < Externals.Count; l++)
		{
			Externals[l].Write(writer);
		}
		if (version >= 20)
		{
			writer.Write(RefTypes.Count);
			for (int m = 0; m < RefTypes.Count; m++)
			{
				RefTypes[m].Write(writer, version, TypeTreeEnabled);
			}
		}
		if (version >= 5)
		{
			writer.WriteNullTerminated(UserInformation);
		}
	}

	public AssetFileInfo GetAssetInfo(long pathId)
	{
		if (_quickLookup != null)
		{
			if (_quickLookup.ContainsKey(pathId))
			{
				return AssetInfos[_quickLookup[pathId]];
			}
		}
		else
		{
			for (int i = 0; i < AssetInfos.Count; i++)
			{
				AssetFileInfo assetFileInfo = AssetInfos[i];
				if (assetFileInfo.PathId == pathId)
				{
					return assetFileInfo;
				}
			}
		}
		return null;
	}

	public void AddAssetInfo(AssetFileInfo info)
	{
		if (_quickLookup != null)
		{
			_quickLookup[info.PathId] = AssetInfos.Count;
		}
		AssetInfos.Add(info);
	}

	public bool RemoveAssetInfo(AssetFileInfo info)
	{
		if (_quickLookup != null)
		{
			_quickLookup.Remove(info.PathId);
		}
		return AssetInfos.Remove(info);
	}

	public void GenerateQuickLookup()
	{
		_quickLookup = new Dictionary<long, int>();
		for (int i = 0; i < AssetInfos.Count; i++)
		{
			AssetFileInfo assetFileInfo = AssetInfos[i];
			_quickLookup[assetFileInfo.PathId] = i;
		}
	}

	public List<AssetFileInfo> GetAssetsOfType(int typeId)
	{
		List<AssetFileInfo> list = new List<AssetFileInfo>();
		foreach (AssetFileInfo assetInfo in AssetInfos)
		{
			if (assetInfo.TypeId == typeId)
			{
				list.Add(assetInfo);
			}
		}
		return list;
	}

	public List<AssetFileInfo> GetAssetsOfType(int typeId, ushort scriptIndex)
	{
		List<AssetFileInfo> list = new List<AssetFileInfo>();
		foreach (AssetFileInfo assetInfo in AssetInfos)
		{
			if (scriptIndex != ushort.MaxValue)
			{
				if (assetInfo.TypeId < 0)
				{
					if (assetInfo.ScriptTypeIndex != scriptIndex || (typeId != 114 && (typeId >= 0 || assetInfo.TypeId != typeId)))
					{
						continue;
					}
				}
				else if (assetInfo.TypeId != 114 || FindTypeTreeTypeByID(assetInfo.TypeId, assetInfo.ScriptTypeIndex).ScriptTypeIndex != scriptIndex || typeId != 114)
				{
					continue;
				}
			}
			else if (assetInfo.TypeId != typeId)
			{
				continue;
			}
			list.Add(assetInfo);
		}
		return list;
	}

	public List<AssetFileInfo> GetAssetsOfType(AssetClassID typeId)
	{
		return GetAssetsOfType((int)typeId);
	}

	public List<AssetFileInfo> GetAssetsOfType(AssetClassID typeId, ushort scriptIndex)
	{
		return GetAssetsOfType((int)typeId, scriptIndex);
	}

	public TypeTreeType FindTypeTreeTypeByID(int id)
	{
		foreach (TypeTreeType typeTreeType in TypeTreeTypes)
		{
			if (typeTreeType.TypeId == id)
			{
				return typeTreeType;
			}
		}
		return null;
	}

	public TypeTreeType FindTypeTreeTypeByID(int id, ushort scriptIndex)
	{
		foreach (TypeTreeType typeTreeType in TypeTreeTypes)
		{
			if (typeTreeType.TypeId == id)
			{
				if (typeTreeType.ScriptTypeIndex == scriptIndex)
				{
					return typeTreeType;
				}
				if (id < 0 && typeTreeType.ScriptTypeIndex == ushort.MaxValue)
				{
					return typeTreeType;
				}
			}
		}
		return null;
	}

	public int FindTypeTreeTypeIndexByID(int id, ushort scriptIndex)
	{
		int count = TypeTreeTypes.Count;
		for (int i = 0; i < count; i++)
		{
			TypeTreeType typeTreeType = TypeTreeTypes[i];
			if (typeTreeType.TypeId == id)
			{
				if (typeTreeType.ScriptTypeIndex == scriptIndex)
				{
					return i;
				}
				if (id < 0 && typeTreeType.ScriptTypeIndex == ushort.MaxValue)
				{
					return i;
				}
			}
		}
		return -1;
	}

	public TypeTreeType FindTypeTreeTypeByScriptIndex(ushort scriptIndex)
	{
		foreach (TypeTreeType typeTreeType in TypeTreeTypes)
		{
			if (typeTreeType.ScriptTypeIndex == scriptIndex)
			{
				return typeTreeType;
			}
		}
		return null;
	}

	public TypeTreeType FindTypeTreeTypeByName(string name)
	{
		foreach (TypeTreeType typeTreeType in TypeTreeTypes)
		{
			if (typeTreeType.Nodes.Count != 0 && typeTreeType.Nodes[0].GetTypeString(typeTreeType.StringBuffer) == name)
			{
				return typeTreeType;
			}
		}
		return null;
	}

	public TypeTreeType FindRefTypeByIndex(ushort scriptIndex)
	{
		foreach (TypeTreeType refType in RefTypes)
		{
			if (refType.ScriptTypeIndex == scriptIndex)
			{
				return refType;
			}
		}
		return null;
	}
}
