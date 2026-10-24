using System.IO;
using PsdSharp.Descriptors;
using PsdSharp.Parsing;

namespace PsdSharp.TaggedBlocks;

public class ObjectsBasedEffectsLayerInfo : TaggedBlock
{
	public uint Version { get; set; }

	public uint DescriptorVersion { get; set; }

	public Descriptor Descriptor { get; set; }

	public ObjectsBasedEffectsLayerInfo(byte[] rawData)
		: base(AdditionalLayerInfoKey.ObjectsBasedEffectsLayerInfo, rawData)
	{
		BigEndianReader bigEndianReader = new BigEndianReader(new MemoryStream(rawData));
		Version = bigEndianReader.ReadUInt32();
		DescriptorVersion = bigEndianReader.ReadUInt32();
		Descriptor = new Descriptor(bigEndianReader);
	}
}
