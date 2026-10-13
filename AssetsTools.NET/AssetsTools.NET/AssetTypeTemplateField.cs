using System;
using System.Collections.Generic;
using System.Linq;
using AssetsTools.NET.Extra;

namespace AssetsTools.NET;

public class AssetTypeTemplateField
{
	public string Name { get; set; }

	public string Type { get; set; }

	public AssetValueType ValueType { get; set; }

	public bool IsArray { get; set; }

	public bool IsAligned { get; set; }

	public bool HasValue { get; set; }

	public List<AssetTypeTemplateField> Children { get; set; }

	public void FromTypeTree(TypeTreeType typeTreeType)
	{
		int fieldIndex = 0;
		FromTypeTree(typeTreeType, ref fieldIndex);
	}

	private void FromTypeTree(TypeTreeType typeTreeType, ref int fieldIndex)
	{
		TypeTreeNode typeTreeNode = typeTreeType.Nodes[fieldIndex];
		Name = typeTreeNode.GetNameString(typeTreeType.StringBuffer);
		Type = typeTreeNode.GetTypeString(typeTreeType.StringBuffer);
		ValueType = AssetTypeValueField.GetValueTypeByTypeName(Type);
		IsArray = Net35Polyfill.HasFlag(typeTreeNode.TypeFlags, TypeTreeNodeFlags.Array);
		IsAligned = (typeTreeNode.MetaFlags & 0x4000) != 0;
		HasValue = ValueType != AssetValueType.None;
		Children = new List<AssetTypeTemplateField>();
		fieldIndex++;
		while (fieldIndex < typeTreeType.Nodes.Count)
		{
			if (typeTreeType.Nodes[fieldIndex].Level <= typeTreeNode.Level)
			{
				fieldIndex--;
				break;
			}
			AssetTypeTemplateField assetTypeTemplateField = new AssetTypeTemplateField();
			assetTypeTemplateField.FromTypeTree(typeTreeType, ref fieldIndex);
			Children.Add(assetTypeTemplateField);
			fieldIndex++;
		}
		if (ValueType == AssetValueType.String && !Children[0].IsArray && Children[0].ValueType != 0)
		{
			Type = Children[0].Type;
			ValueType = Children[0].ValueType;
			Children.Clear();
		}
		if (IsArray)
		{
			ValueType = ((Children[1].ValueType == AssetValueType.UInt8) ? AssetValueType.ByteArray : AssetValueType.Array);
		}
		Children.TrimExcess();
	}

	public void FromClassDatabase(ClassDatabaseFile cldbFile, ClassDatabaseType cldbType, bool preferEditor = false)
	{
		if (cldbType.EditorRootNode == null && cldbType.ReleaseRootNode == null)
		{
			throw new Exception("No root nodes were found!");
		}
		ClassDatabaseTypeNode preferredNode = cldbType.GetPreferredNode(preferEditor);
		FromClassDatabase(cldbFile.StringTable, preferredNode);
	}

	private void FromClassDatabase(ClassDatabaseStringTable strTable, ClassDatabaseTypeNode node)
	{
		Name = strTable.GetString(node.FieldName);
		Type = strTable.GetString(node.TypeName);
		if (Type == "SInt32")
		{
			Type = "int";
		}
		else if (Type == "UInt32")
		{
			Type = "unsigned int";
		}
		ValueType = AssetTypeValueField.GetValueTypeByTypeName(Type);
		IsArray = node.TypeFlags == 1;
		IsAligned = (node.MetaFlag & 0x4000) != 0;
		HasValue = ValueType != AssetValueType.None;
		Children = new List<AssetTypeTemplateField>(node.Children.Count);
		foreach (ClassDatabaseTypeNode child in node.Children)
		{
			AssetTypeTemplateField assetTypeTemplateField = new AssetTypeTemplateField();
			assetTypeTemplateField.FromClassDatabase(strTable, child);
			Children.Add(assetTypeTemplateField);
		}
		if (ValueType == AssetValueType.String && !Children[0].IsArray && Children[0].ValueType != 0)
		{
			Type = Children[0].Type;
			ValueType = Children[0].ValueType;
			Children.Clear();
			Children.TrimExcess();
		}
		if (IsArray)
		{
			ValueType = ((Children[1].ValueType == AssetValueType.UInt8) ? AssetValueType.ByteArray : AssetValueType.Array);
		}
	}

	public AssetTypeValueField MakeValue(AssetsFileReader reader, RefTypeManager refMan = null)
	{
		AssetTypeValueField valueField = new AssetTypeValueField
		{
			TemplateField = this
		};
		return ReadType(reader, valueField, refMan);
	}

	public AssetTypeValueField MakeValue(AssetsFileReader reader, long position, RefTypeManager refMan = null)
	{
		reader.Position = position;
		return MakeValue(reader, refMan);
	}

	public AssetTypeValueField ReadType(AssetsFileReader reader, AssetTypeValueField valueField, RefTypeManager refMan)
	{
		if (valueField.TemplateField.IsArray)
		{
			int count = valueField.TemplateField.Children.Count;
			if (count != 2)
			{
				throw new Exception($"Expected array to have two children, found {count} instead!");
			}
			AssetValueType valueType = valueField.TemplateField.Children[0].ValueType;
			if (valueType != AssetValueType.Int32 && valueType != AssetValueType.UInt32)
			{
				throw new Exception($"Expected int array size type, found {valueType} instead!");
			}
			if (valueField.TemplateField.ValueType == AssetValueType.ByteArray)
			{
				valueField.Children = new List<AssetTypeValueField>(0);
				int count2 = reader.ReadInt32();
				byte[] value = reader.ReadBytes(count2);
				if (valueField.TemplateField.IsAligned)
				{
					reader.Align();
				}
				valueField.Value = new AssetTypeValue(AssetValueType.ByteArray, value);
			}
			else
			{
				int num = reader.ReadInt32();
				valueField.Children = new List<AssetTypeValueField>(num);
				for (int i = 0; i < num; i++)
				{
					AssetTypeValueField assetTypeValueField = new AssetTypeValueField();
					assetTypeValueField.TemplateField = valueField.TemplateField.Children[1];
					valueField.Children.Add(ReadType(reader, assetTypeValueField, refMan));
				}
				valueField.Children.TrimExcess();
				if (valueField.TemplateField.IsAligned)
				{
					reader.Align();
				}
				AssetTypeArrayInfo assetTypeArrayInfo = default(AssetTypeArrayInfo);
				assetTypeArrayInfo.size = num;
				AssetTypeArrayInfo assetTypeArrayInfo2 = assetTypeArrayInfo;
				valueField.Value = new AssetTypeValue(AssetValueType.Array, assetTypeArrayInfo2);
			}
		}
		else
		{
			switch (valueField.TemplateField.ValueType)
			{
			case AssetValueType.None:
			{
				int count5 = valueField.TemplateField.Children.Count;
				valueField.Children = new List<AssetTypeValueField>(count5);
				for (int k = 0; k < count5; k++)
				{
					AssetTypeValueField assetTypeValueField2 = new AssetTypeValueField();
					assetTypeValueField2.TemplateField = valueField.TemplateField.Children[k];
					valueField.Children.Add(ReadType(reader, assetTypeValueField2, refMan));
				}
				valueField.Children.TrimExcess();
				valueField.Value = null;
				if (valueField.TemplateField.IsAligned)
				{
					reader.Align();
				}
				break;
			}
			case AssetValueType.String:
			{
				valueField.Children = new List<AssetTypeValueField>(0);
				int count3 = reader.ReadInt32();
				valueField.Value = new AssetTypeValue(reader.ReadBytes(count3), asString: true);
				reader.Align();
				break;
			}
			case AssetValueType.ManagedReferencesRegistry:
			{
				if (refMan == null)
				{
					throw new Exception("refMan MUST be set to deserialize objects with ref types!");
				}
				valueField.Children = new List<AssetTypeValueField>(0);
				ManagedReferencesRegistry managedReferencesRegistry = new ManagedReferencesRegistry();
				valueField.Value = new AssetTypeValue(managedReferencesRegistry);
				int count4 = valueField.TemplateField.Children.Count;
				if (count4 != 2)
				{
					throw new Exception($"Expected ManagedReferencesRegistry to have two children, found {count4} instead!");
				}
				managedReferencesRegistry.version = reader.ReadInt32();
				managedReferencesRegistry.references = new List<AssetTypeReferencedObject>(0);
				if (managedReferencesRegistry.version == 1)
				{
					while (true)
					{
						AssetTypeReferencedObject assetTypeReferencedObject = MakeReferencedObject(reader, managedReferencesRegistry.version, managedReferencesRegistry.references.Count, refMan);
						if (assetTypeReferencedObject.type.Equals(AssetTypeReference.TERMINUS))
						{
							break;
						}
						managedReferencesRegistry.references.Add(assetTypeReferencedObject);
					}
				}
				else
				{
					int num2 = reader.ReadInt32();
					for (int j = 0; j < num2; j++)
					{
						AssetTypeReferencedObject item = MakeReferencedObject(reader, managedReferencesRegistry.version, j, refMan);
						managedReferencesRegistry.references.Add(item);
					}
				}
				break;
			}
			default:
				if (valueField.TemplateField.Children.Count == 0)
				{
					valueField.Children = new List<AssetTypeValueField>(0);
					switch (valueField.TemplateField.ValueType)
					{
					case AssetValueType.Int8:
						valueField.Value = new AssetTypeValue(reader.ReadSByte());
						break;
					case AssetValueType.UInt8:
						valueField.Value = new AssetTypeValue(reader.ReadByte());
						break;
					case AssetValueType.Bool:
						valueField.Value = new AssetTypeValue(reader.ReadBoolean());
						break;
					case AssetValueType.Int16:
						valueField.Value = new AssetTypeValue(reader.ReadInt16());
						break;
					case AssetValueType.UInt16:
						valueField.Value = new AssetTypeValue(reader.ReadUInt16());
						break;
					case AssetValueType.Int32:
						valueField.Value = new AssetTypeValue(reader.ReadInt32());
						break;
					case AssetValueType.UInt32:
						valueField.Value = new AssetTypeValue(reader.ReadUInt32());
						break;
					case AssetValueType.Int64:
						valueField.Value = new AssetTypeValue(reader.ReadInt64());
						break;
					case AssetValueType.UInt64:
						valueField.Value = new AssetTypeValue(reader.ReadUInt64());
						break;
					case AssetValueType.Float:
						valueField.Value = new AssetTypeValue(reader.ReadSingle());
						break;
					case AssetValueType.Double:
						valueField.Value = new AssetTypeValue(reader.ReadDouble());
						break;
					}
					if (valueField.TemplateField.IsAligned)
					{
						reader.Align();
					}
				}
				else if (valueField.TemplateField.ValueType != 0)
				{
					throw new Exception("Cannot read value of field with children!");
				}
				break;
			}
		}
		return valueField;
	}

	public AssetTypeTemplateField Clone()
	{
		return new AssetTypeTemplateField
		{
			Name = Name,
			Type = Type,
			ValueType = ValueType,
			IsArray = IsArray,
			IsAligned = IsAligned,
			HasValue = HasValue,
			Children = Children.Select((AssetTypeTemplateField c) => c.Clone()).ToList()
		};
	}

	private AssetTypeReferencedObject MakeReferencedObject(AssetsFileReader reader, int registryVersion, int referenceIndex, RefTypeManager refMan)
	{
		AssetTypeReferencedObject assetTypeReferencedObject = new AssetTypeReferencedObject();
		if (registryVersion == 1)
		{
			assetTypeReferencedObject.rid = referenceIndex;
		}
		else
		{
			assetTypeReferencedObject.rid = reader.ReadInt64();
		}
		AssetTypeReference assetTypeReference = new AssetTypeReference();
		assetTypeReference.ReadAsset(reader);
		assetTypeReferencedObject.type = assetTypeReference;
		AssetTypeTemplateField templateField = refMan.GetTemplateField(assetTypeReference);
		if (templateField != null)
		{
			assetTypeReferencedObject.data = new AssetTypeValueField
			{
				TemplateField = templateField
			};
			assetTypeReferencedObject.data = ReadType(reader, assetTypeReferencedObject.data, refMan);
		}
		else
		{
			assetTypeReferencedObject.data = AssetTypeValueField.DUMMY_FIELD;
		}
		return assetTypeReferencedObject;
	}

	public override string ToString()
	{
		return Type + " " + Name;
	}
}
