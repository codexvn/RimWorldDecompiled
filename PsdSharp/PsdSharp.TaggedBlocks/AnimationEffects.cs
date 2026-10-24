using System.IO;
using PsdSharp.Descriptors;
using PsdSharp.Parsing;

namespace PsdSharp.TaggedBlocks;

public class AnimationEffects : TaggedBlock
{
	public int DescriptorVersion { get; }

	public Descriptor Descriptor { get; }

	public AnimationEffects(byte[] rawData)
		: base(AdditionalLayerInfoKey.AnimationEffects, rawData)
	{
		BigEndianReader bigEndianReader = new BigEndianReader(new MemoryStream(rawData));
		DescriptorVersion = bigEndianReader.ReadInt32();
		Descriptor = new Descriptor(bigEndianReader);
	}
}
