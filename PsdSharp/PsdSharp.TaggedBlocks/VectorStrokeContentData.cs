using System.IO;
using PsdSharp.Descriptors;
using PsdSharp.Parsing;

namespace PsdSharp.TaggedBlocks;

public class VectorStrokeContentData : TaggedBlock
{
	public new string Key { get; set; }

	public int DescriptorVersion { get; set; }

	public Descriptor Descriptor { get; set; }

	public VectorStrokeContentData(byte[] rawData)
		: base(AdditionalLayerInfoKey.VectorStrokeContentData, rawData)
	{
		BigEndianReader bigEndianReader = new BigEndianReader(new MemoryStream(rawData));
		Key = bigEndianReader.ReadFixedLengthString(4);
		DescriptorVersion = bigEndianReader.ReadInt32();
		Descriptor = new Descriptor(bigEndianReader);
	}
}
