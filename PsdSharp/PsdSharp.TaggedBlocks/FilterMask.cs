using System.IO;
using PsdSharp.Parsing;

namespace PsdSharp.TaggedBlocks;

public class FilterMask : TaggedBlock
{
	public byte[] ColorSpace { get; set; }

	public short Opacity { get; set; }

	public FilterMask(byte[] rawData)
		: base(AdditionalLayerInfoKey.FilterMask, rawData)
	{
		BigEndianReader bigEndianReader = new BigEndianReader(new MemoryStream(rawData));
		ColorSpace = new byte[10];
		bigEndianReader.ReadIntoBuffer(ColorSpace);
		Opacity = bigEndianReader.ReadInt16();
	}
}
