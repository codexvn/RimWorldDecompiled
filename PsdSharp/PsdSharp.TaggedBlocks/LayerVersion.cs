using System.IO;
using PsdSharp.Parsing;

namespace PsdSharp.TaggedBlocks;

public class LayerVersion : TaggedBlock
{
	public int MinimumPhotoshopVersionRequired;

	public LayerVersion(byte[] rawData)
		: base(AdditionalLayerInfoKey.LayerVersion, rawData)
	{
		BigEndianReader bigEndianReader = new BigEndianReader(new MemoryStream(rawData));
		MinimumPhotoshopVersionRequired = bigEndianReader.ReadInt32();
	}
}
