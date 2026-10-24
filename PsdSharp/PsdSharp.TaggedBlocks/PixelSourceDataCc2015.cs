using System;

namespace PsdSharp.TaggedBlocks;

public class PixelSourceDataCc2015 : TaggedBlock
{
	public new byte[] RawData { get; } = rawData.AsSpan(8).ToArray();

	public PixelSourceDataCc2015(byte[] rawData)
		: base(AdditionalLayerInfoKey.PixelSourceDataCc2015, rawData)
	{
	}
}
