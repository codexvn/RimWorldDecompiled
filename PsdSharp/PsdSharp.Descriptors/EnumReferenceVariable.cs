using PsdSharp.Parsing;

namespace PsdSharp.Descriptors;

public class EnumReferenceVariable : Variable
{
	public readonly string Name;

	public readonly string ClassId;

	public readonly string TypeId;

	public readonly string Enum;

	public override object Value => (Name, ClassId, TypeId, Enum);

	internal EnumReferenceVariable(string key, BigEndianReader reader)
		: base(key)
	{
		Name = reader.ReadUnicodeString().String;
		int num = reader.ReadInt32();
		ClassId = reader.ReadFixedLengthString((num > 0) ? num : 4);
		int num2 = reader.ReadInt32();
		TypeId = reader.ReadFixedLengthString((num2 > 0) ? num2 : 4);
		int num3 = reader.ReadInt32();
		Enum = reader.ReadFixedLengthString((num3 > 0) ? num3 : 4);
	}
}
