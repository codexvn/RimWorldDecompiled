using PsdSharp.Parsing;

namespace PsdSharp.Descriptors;

public class ObjectArrayVariable : Variable
{
	public readonly uint Count;

	public readonly Descriptor Descriptor;

	public override object Value => Descriptor.Variables;

	internal ObjectArrayVariable(string key, BigEndianReader reader)
		: base(key)
	{
		Count = reader.ReadUInt32();
		Descriptor = new Descriptor(reader);
	}
}
