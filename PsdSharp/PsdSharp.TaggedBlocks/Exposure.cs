using System.IO;
using PsdSharp.Parsing;

namespace PsdSharp.TaggedBlocks;

public class Exposure : TaggedBlock
{
	public short Version { get; }

	public int ExposureValue { get; }

	public int Offset { get; }

	public int Gamma { get; }

	public Exposure(byte[] rawData)
		: base(AdditionalLayerInfoKey.Exposure, rawData)
	{
		BigEndianReader bigEndianReader = new BigEndianReader(new MemoryStream(rawData));
		Version = bigEndianReader.ReadInt16();
		ExposureValue = bigEndianReader.ReadInt32();
		Offset = bigEndianReader.ReadInt32();
		Gamma = bigEndianReader.ReadInt32();
	}
}
