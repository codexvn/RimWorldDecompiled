using System.Collections.Generic;

namespace AssetsTools.NET;

public class ClassDatabaseTypeNode
{
	public ushort TypeName { get; set; }

	public ushort FieldName { get; set; }

	public int ByteSize { get; set; }

	public ushort Version { get; set; }

	public byte TypeFlags { get; set; }

	public uint MetaFlag { get; set; }

	public List<ClassDatabaseTypeNode> Children { get; set; }

	public void Read(AssetsFileReader reader)
	{
		TypeName = reader.ReadUInt16();
		FieldName = reader.ReadUInt16();
		ByteSize = reader.ReadInt32();
		Version = reader.ReadUInt16();
		TypeFlags = reader.ReadByte();
		MetaFlag = reader.ReadUInt32();
		int num = reader.ReadUInt16();
		Children = new List<ClassDatabaseTypeNode>(num);
		for (int i = 0; i < num; i++)
		{
			ClassDatabaseTypeNode classDatabaseTypeNode = new ClassDatabaseTypeNode();
			classDatabaseTypeNode.Read(reader);
			Children.Add(classDatabaseTypeNode);
		}
	}

	public void Write(AssetsFileWriter writer)
	{
		writer.Write(TypeName);
		writer.Write(FieldName);
		writer.Write(ByteSize);
		writer.Write(TypeFlags);
		writer.Write(MetaFlag);
		writer.Write((ushort)Children.Count);
		for (int i = 0; i < Children.Count; i++)
		{
			Children[i].Write(writer);
		}
	}
}
