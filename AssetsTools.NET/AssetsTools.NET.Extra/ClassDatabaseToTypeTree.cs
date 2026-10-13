using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssetsTools.NET.Extra;

public class ClassDatabaseToTypeTree
{
	private ClassDatabaseFile cldbFile;

	private uint stringTablePos;

	private Dictionary<string, uint> stringTableLookup;

	private Dictionary<string, uint> commonStringTableLookup;

	private List<TypeTreeNode> typeTreeNodes;

	public static TypeTreeType Convert(ClassDatabaseFile classes, string name, bool preferEditor = false)
	{
		ClassDatabaseType classDatabaseType = classes.FindAssetClassByName(name);
		if (classDatabaseType == null)
		{
			return null;
		}
		return Convert(classes, classDatabaseType, preferEditor);
	}

	public static TypeTreeType Convert(ClassDatabaseFile classes, int id, bool preferEditor = false)
	{
		ClassDatabaseType classDatabaseType = classes.FindAssetClassByID(id);
		if (classDatabaseType == null)
		{
			return null;
		}
		return Convert(classes, classDatabaseType, preferEditor);
	}

	public static TypeTreeType Convert(ClassDatabaseFile classes, ClassDatabaseType type, bool preferEditor = false)
	{
		return new ClassDatabaseToTypeTree().ConvertInternal(classes, type, preferEditor);
	}

	private TypeTreeType ConvertInternal(ClassDatabaseFile classes, ClassDatabaseType type, bool preferEditor = false)
	{
		TypeTreeType typeTreeType = new TypeTreeType
		{
			TypeId = type.ClassId,
			ScriptTypeIndex = ushort.MaxValue,
			IsStrippedType = false,
			ScriptIdHash = Hash128.NewBlankHash(),
			TypeHash = Hash128.NewBlankHash(),
			TypeDependencies = new int[0]
		};
		cldbFile = classes;
		stringTablePos = 0u;
		stringTableLookup = new Dictionary<string, uint>();
		commonStringTableLookup = new Dictionary<string, uint>();
		typeTreeNodes = new List<TypeTreeNode>();
		InitializeDefaultStringTableIndices();
		ClassDatabaseTypeNode preferredNode = type.GetPreferredNode(preferEditor);
		ConvertFields(preferredNode, 0);
		StringBuilder stringBuilder = new StringBuilder();
		foreach (KeyValuePair<string, uint> item in stringTableLookup.OrderBy((KeyValuePair<string, uint> n) => n.Value).ToList())
		{
			stringBuilder.Append(item.Key + "\0");
		}
		typeTreeType.StringBuffer = stringBuilder.ToString();
		typeTreeType.Nodes = typeTreeNodes;
		typeTreeType.TypeHash = ComputeHash(typeTreeType);
		return typeTreeType;
	}

	private void InitializeDefaultStringTableIndices()
	{
		int num = 0;
		foreach (ushort commonStringBufferIndex in cldbFile.CommonStringBufferIndices)
		{
			string @string = cldbFile.StringTable.GetString(commonStringBufferIndex);
			if (@string != string.Empty)
			{
				commonStringTableLookup.Add(@string, (uint)num);
				num += @string.Length + 1;
			}
		}
	}

	private void ConvertFields(ClassDatabaseTypeNode node, int depth)
	{
		string @string = cldbFile.GetString(node.FieldName);
		string string2 = cldbFile.GetString(node.TypeName);
		uint nameStrOffset;
		if (stringTableLookup.ContainsKey(@string))
		{
			nameStrOffset = stringTableLookup[@string];
		}
		else if (commonStringTableLookup.ContainsKey(@string))
		{
			nameStrOffset = commonStringTableLookup[@string] + 2147483648u;
		}
		else
		{
			nameStrOffset = stringTablePos;
			stringTableLookup.Add(@string, stringTablePos);
			stringTablePos += (uint)(@string.Length + 1);
		}
		uint typeStrOffset;
		if (stringTableLookup.ContainsKey(string2))
		{
			typeStrOffset = stringTableLookup[string2];
		}
		else if (commonStringTableLookup.ContainsKey(string2))
		{
			typeStrOffset = commonStringTableLookup[string2] + 2147483648u;
		}
		else
		{
			typeStrOffset = stringTablePos;
			stringTableLookup.Add(string2, stringTablePos);
			stringTablePos += (uint)(string2.Length + 1);
		}
		typeTreeNodes.Add(new TypeTreeNode
		{
			Level = (byte)depth,
			MetaFlags = node.MetaFlag,
			Index = (uint)typeTreeNodes.Count,
			TypeFlags = (TypeTreeNodeFlags)node.TypeFlags,
			NameStrOffset = nameStrOffset,
			ByteSize = node.ByteSize,
			TypeStrOffset = typeStrOffset,
			Version = node.Version
		});
		foreach (ClassDatabaseTypeNode child in node.Children)
		{
			ConvertFields(child, depth + 1);
		}
	}

	private static Hash128 ComputeHash(TypeTreeType typeTree)
	{
		MD4 mD = new MD4();
		Update(typeTree, mD);
		return new Hash128(mD.Digest());
	}

	private static void Update(TypeTreeType typeTree, MD4 md4)
	{
		List<TypeTreeNode>.Enumerator enumerator = typeTree.Nodes.GetEnumerator();
		while (enumerator.MoveNext())
		{
			TypeTreeNode current = enumerator.Current;
			md4.Update(Encoding.UTF8.GetBytes(current.GetTypeString(typeTree.StringBuffer)));
			md4.Update(Encoding.UTF8.GetBytes(current.GetNameString(typeTree.StringBuffer)));
			md4.Update(BitConverter.GetBytes(current.ByteSize));
			md4.Update(BitConverter.GetBytes(System.Convert.ToInt32(current.TypeFlags)));
			md4.Update(BitConverter.GetBytes(System.Convert.ToInt32(current.Version)));
			md4.Update(BitConverter.GetBytes(System.Convert.ToInt32(current.MetaFlags & 0x4000)));
		}
	}
}
