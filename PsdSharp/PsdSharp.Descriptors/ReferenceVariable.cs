using System.Collections.Generic;
using PsdSharp.Parsing;

namespace PsdSharp.Descriptors;

public class ReferenceVariable : Variable
{
	public IReadOnlyCollection<Variable> Variables { get; set; }

	public override object Value => Variables;

	internal ReferenceVariable(string key, BigEndianReader reader)
		: base(key)
	{
		int num = reader.ReadInt32();
		List<Variable> list = new List<Variable>();
		for (int i = 0; i < num; i++)
		{
			string text = reader.ReadFixedLengthString(4);
			list.Add(Variable.Read(text, text, reader));
		}
		Variables = list;
	}
}
