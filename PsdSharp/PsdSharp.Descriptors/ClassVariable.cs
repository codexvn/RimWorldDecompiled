using PsdSharp.Parsing;

namespace PsdSharp.Descriptors;

public class ClassVariable : Variable
{
	public readonly string Name;

	public readonly string ClassId;

	public override object Value => (Name, ClassId);

	internal ClassVariable(string key, BigEndianReader reader)
		: base(key)
	{
		Name = reader.ReadUnicodeString().String;
		int num = reader.ReadInt32();
		ClassId = reader.ReadFixedLengthString((num > 0) ? num : 4);
	}
}
