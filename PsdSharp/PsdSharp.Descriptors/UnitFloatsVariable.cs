using PsdSharp.Parsing;

namespace PsdSharp.Descriptors;

public class UnitFloatsVariable : Variable
{
	public readonly UnitFloatVariable.UnitType Unit;

	public readonly double[] Values;

	public override object Value => Values;

	internal UnitFloatsVariable(string key, BigEndianReader reader)
		: base(key)
	{
		Unit = UnitFloatVariable.UnitType.Get(reader.ReadFixedLengthString(4));
		Values = new double[reader.ReadUInt32()];
		for (int i = 0; i < Values.Length; i++)
		{
			Values[i] = reader.ReadDouble();
		}
	}
}
