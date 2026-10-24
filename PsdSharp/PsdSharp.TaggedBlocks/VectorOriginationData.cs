using System.IO;
using PsdSharp.Descriptors;
using PsdSharp.Parsing;

namespace PsdSharp.TaggedBlocks;

public class VectorOriginationData : TaggedBlock
{
	public int Version { get; set; }

	public int DescriptorVersion { get; set; }

	public Descriptor Descriptor { get; set; }

	public VectorOriginationData(byte[] rawData)
		: base(AdditionalLayerInfoKey.VectorOriginationData, rawData)
	{
		BigEndianReader bigEndianReader = new BigEndianReader(new MemoryStream(rawData));
		Version = bigEndianReader.ReadInt32();
		DescriptorVersion = bigEndianReader.ReadInt32();
		Descriptor = new Descriptor(bigEndianReader);
	}
}
