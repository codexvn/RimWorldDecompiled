using System.IO;
using PsdSharp.Descriptors;
using PsdSharp.Parsing;

namespace PsdSharp.TaggedBlocks;

public class Vibrance : TaggedBlock
{
	public uint DescriptorVersion { get; set; }

	public Descriptor Descriptor { get; set; }

	public Vibrance(byte[] rawData)
		: base(AdditionalLayerInfoKey.ObjectsBasedEffectsLayerInfo, rawData)
	{
		BigEndianReader bigEndianReader = new BigEndianReader(new MemoryStream(rawData));
		DescriptorVersion = bigEndianReader.ReadUInt32();
		Descriptor = new Descriptor(bigEndianReader);
	}
}
