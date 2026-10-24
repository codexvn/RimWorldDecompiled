using System.IO;
using PsdSharp.Descriptors;
using PsdSharp.Parsing;

namespace PsdSharp.TaggedBlocks;

public class UnicodePathName : TaggedBlock
{
	public int DescriptorVersion { get; }

	public Descriptor Descriptor { get; }

	public UnicodePathName(byte[] rawData)
		: base(AdditionalLayerInfoKey.UnicodePathName, rawData)
	{
		BigEndianReader bigEndianReader = new BigEndianReader(new MemoryStream(rawData));
		DescriptorVersion = bigEndianReader.ReadInt32();
		Descriptor = new Descriptor(bigEndianReader);
	}
}
