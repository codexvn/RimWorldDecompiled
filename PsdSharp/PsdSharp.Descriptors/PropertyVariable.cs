using PsdSharp.Parsing;

namespace PsdSharp.Descriptors;

public class PropertyVariable : Variable
{
	public readonly string Name;

	public readonly string ClassId;

	public readonly string KeyId;

	public override object Value => (Name, ClassId, KeyId);

	internal PropertyVariable(string key, BigEndianReader reader)
		: base(key)
	{
		Name = reader.ReadUnicodeString().String;
		int num = reader.ReadInt32();
		ClassId = reader.ReadFixedLengthString((num > 0) ? num : 4);
		int num2 = reader.ReadInt32();
		KeyId = reader.ReadFixedLengthString((num2 > 0) ? num2 : 4);
	}
}
