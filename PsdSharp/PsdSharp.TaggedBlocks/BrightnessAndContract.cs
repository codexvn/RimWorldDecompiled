using System.IO;
using PsdSharp.Parsing;

namespace PsdSharp.TaggedBlocks;

public class BrightnessAndContract : TaggedBlock
{
	public short Brightness { get; set; }

	public short Contrast { get; set; }

	public short MeanValue { get; set; }

	public bool LabColorOnly { get; set; }

	public BrightnessAndContract(byte[] rawData)
		: base(AdditionalLayerInfoKey.BrightnessAndContract, rawData)
	{
		BigEndianReader bigEndianReader = new BigEndianReader(new MemoryStream(rawData));
		Brightness = bigEndianReader.ReadInt16();
		Contrast = bigEndianReader.ReadInt16();
		MeanValue = bigEndianReader.ReadInt16();
		LabColorOnly = bigEndianReader.ReadByte() != 0;
	}
}
