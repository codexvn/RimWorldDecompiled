using System.IO;
using PsdSharp.Descriptors;
using PsdSharp.Parsing;

namespace PsdSharp.TaggedBlocks;

public class SmartObjectLayerData : TaggedBlock
{
	public int Version { get; }

	public Descriptor Descriptor { get; }

	public SmartObjectLayerData(byte[] rawData)
		: base(AdditionalLayerInfoKey.SmartObjectLayerData2, rawData)
	{
		BigEndianReader bigEndianReader = new BigEndianReader(new MemoryStream(rawData));
		bigEndianReader.Skip(4);
		Version = bigEndianReader.ReadInt32();
		Descriptor = new Descriptor(bigEndianReader);
	}
}
