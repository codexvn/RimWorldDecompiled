using PsdSharp.Parsing;

namespace PsdSharp.Descriptors;

public class AliasVariable : Variable
{
	public readonly byte[] FsSpecOrPath;

	public override object Value => FsSpecOrPath;

	internal AliasVariable(string key, BigEndianReader reader)
		: base(key)
	{
		uint num = reader.ReadUInt32();
		FsSpecOrPath = new byte[num];
		reader.ReadIntoBuffer(FsSpecOrPath);
	}
}
