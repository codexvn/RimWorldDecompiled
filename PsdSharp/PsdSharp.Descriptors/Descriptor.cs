using System.Collections.Generic;
using System.IO;
using PsdSharp.Parsing;

namespace PsdSharp.Descriptors;

public class Descriptor
{
	public string Name { get; set; }

	public string ClassId { get; set; }

	public IReadOnlyCollection<Variable> Variables { get; set; }

	internal Descriptor(byte[] rawData)
		: this(new BigEndianReader(new MemoryStream(rawData)))
	{
	}

	internal Descriptor(BigEndianReader reader)
	{
		Name = reader.ReadUnicodeString().String;
		uint num = reader.ReadUInt32();
		ClassId = reader.ReadFixedLengthString((num != 0) ? num : 4u);
		uint num2 = reader.ReadUInt32();
		List<Variable> list = new List<Variable>();
		for (int i = 0; i < num2; i++)
		{
			uint num3 = reader.ReadUInt32();
			string key = reader.ReadFixedLengthString((num3 != 0) ? num3 : 4u);
			list.Add(Variable.Read(key, reader));
		}
		Variables = list.AsReadOnly();
	}
}
