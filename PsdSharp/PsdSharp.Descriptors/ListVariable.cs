using System.Collections.Generic;
using PsdSharp.Parsing;

namespace PsdSharp.Descriptors;

public class ListVariable : Variable
{
	public IReadOnlyCollection<Variable> Variables { get; }

	public override object Value => Variables;

	internal ListVariable(string key, BigEndianReader reader)
		: base(key)
	{
		uint num = reader.ReadUInt32();
		List<Variable> list = new List<Variable>();
		for (int i = 0; i < num; i++)
		{
			list.Add(Variable.Read(i.ToString(), reader));
		}
		Variables = list.AsReadOnly();
	}
}
