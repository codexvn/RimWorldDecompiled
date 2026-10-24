using PsdSharp.Parsing;

namespace PsdSharp.Descriptors;

public class EnumVariable : Variable
{
	public readonly string TypeId;

	public readonly string Enum;

	public override object Value => (TypeId, Enum);

	internal EnumVariable(string key, BigEndianReader reader)
		: base(key)
	{
		int num = reader.ReadInt32();
		TypeId = reader.ReadFixedLengthString((num > 0) ? num : 4);
		int num2 = reader.ReadInt32();
		Enum = reader.ReadFixedLengthString((num2 > 0) ? num2 : 4);
	}
}
