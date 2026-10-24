using System.IO;
using PsdSharp.Descriptors;
using PsdSharp.Parsing;

namespace PsdSharp.TaggedBlocks;

public class BlackWhite : TaggedBlock
{
	public int DescriptorVersion { get; set; }

	public Descriptor Descriptor { get; set; }

	public BlackWhite(byte[] rawData)
		: base(AdditionalLayerInfoKey.BlackWhite, rawData)
	{
		BigEndianReader bigEndianReader = new BigEndianReader(new MemoryStream(rawData));
		DescriptorVersion = bigEndianReader.ReadInt32();
		Descriptor = new Descriptor(bigEndianReader);
	}
}
