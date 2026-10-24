using System.IO;
using PsdSharp.Descriptors;
using PsdSharp.Parsing;

namespace PsdSharp.TaggedBlocks;

public class PlacedLayerData : TaggedBlock
{
	public int Version { get; }

	public int DescriptorVersion { get; }

	public Descriptor Descriptor { get; }

	public PlacedLayerData(byte[] rawData)
		: base(AdditionalLayerInfoKey.SmartObjectLayerData1, rawData)
	{
		BigEndianReader bigEndianReader = new BigEndianReader(new MemoryStream(rawData));
		bigEndianReader.Skip(4);
		Version = bigEndianReader.ReadInt32();
		DescriptorVersion = bigEndianReader.ReadInt32();
		Descriptor = new Descriptor(bigEndianReader);
	}
}
