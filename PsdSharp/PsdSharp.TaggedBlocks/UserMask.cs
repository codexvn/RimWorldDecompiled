using System.IO;
using PsdSharp.Parsing;

namespace PsdSharp.TaggedBlocks;

public class UserMask : TaggedBlock
{
	public byte[] ColorSpace { get; set; }

	public short Opacity { get; set; }

	public byte Flag { get; set; }

	public UserMask(byte[] rawData)
		: base(AdditionalLayerInfoKey.UserMask, rawData)
	{
		BigEndianReader bigEndianReader = new BigEndianReader(new MemoryStream(rawData));
		ColorSpace = new byte[10];
		bigEndianReader.ReadIntoBuffer(ColorSpace);
		Opacity = bigEndianReader.ReadInt16();
		Flag = bigEndianReader.ReadByte();
	}
}
