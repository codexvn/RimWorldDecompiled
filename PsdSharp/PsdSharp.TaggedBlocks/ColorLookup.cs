using System.IO;
using PsdSharp.Descriptors;
using PsdSharp.Parsing;

namespace PsdSharp.TaggedBlocks;

public class ColorLookup : TaggedBlock
{
	public short Version { get; set; }

	public int DescriptorVersion { get; set; }

	public Descriptor Descriptor { get; set; }

	public ColorLookup(byte[] rawData)
		: base(AdditionalLayerInfoKey.ColorLookup, rawData)
	{
		BigEndianReader bigEndianReader = new BigEndianReader(new MemoryStream(rawData));
		Version = bigEndianReader.ReadInt16();
		DescriptorVersion = bigEndianReader.ReadInt32();
		Descriptor = new Descriptor(bigEndianReader);
	}
}
