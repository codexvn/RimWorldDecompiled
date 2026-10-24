using System.IO;
using PsdSharp.Descriptors;
using PsdSharp.Parsing;

namespace PsdSharp.TaggedBlocks;

public class VectorStrokeData : TaggedBlock
{
	public int DescriptorVersion { get; set; }

	public Descriptor Descriptor { get; set; }

	public VectorStrokeData(byte[] rawData)
		: base(AdditionalLayerInfoKey.VectorStrokeData, rawData)
	{
		BigEndianReader bigEndianReader = new BigEndianReader(new MemoryStream(rawData));
		DescriptorVersion = bigEndianReader.ReadInt32();
		Descriptor = new Descriptor(bigEndianReader);
	}
}
