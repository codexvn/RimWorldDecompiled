using System.IO;
using PsdSharp.Descriptors;
using PsdSharp.Parsing;

namespace PsdSharp.TaggedBlocks;

public class ArtboardData : TaggedBlock
{
	public int DescriptorVersion { get; }

	public Descriptor Descriptor { get; }

	public ArtboardData(AdditionalLayerInfoKey key, byte[] rawData)
		: base(key, rawData)
	{
		BigEndianReader bigEndianReader = new BigEndianReader(new MemoryStream(rawData));
		DescriptorVersion = bigEndianReader.ReadInt32();
		Descriptor = new Descriptor(bigEndianReader);
	}
}
