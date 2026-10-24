using System.Collections.Generic;
using PsdSharp.Parsing;

namespace PsdSharp.Descriptors;

public class DescriptorVariable : Variable
{
	public string Name { get; set; }

	public string ClassId { get; set; }

	public IReadOnlyCollection<Variable> Variables { get; set; }

	public override object Value => Variables;

	internal DescriptorVariable(string key, BigEndianReader reader)
		: base(key)
	{
		Name = reader.ReadUnicodeString().String;
		uint num = reader.ReadUInt32();
		ClassId = reader.ReadFixedLengthString((num != 0) ? num : 4u);
		uint num2 = reader.ReadUInt32();
		List<Variable> list = new List<Variable>();
		for (int i = 0; i < num2; i++)
		{
			uint num3 = reader.ReadUInt32();
			string key2 = reader.ReadFixedLengthString((num3 != 0) ? num3 : 4u);
			list.Add(Variable.Read(key2, reader));
		}
		Variables = list.AsReadOnly();
	}
}
