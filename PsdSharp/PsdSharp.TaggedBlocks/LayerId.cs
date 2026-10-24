using System.IO;
using PsdSharp.Parsing;

namespace PsdSharp.TaggedBlocks;

public class LayerId : TaggedBlock
{
	public int Id { get; set; }

	public LayerId(byte[] rawData)
		: base(AdditionalLayerInfoKey.LayerId, rawData)
	{
		BigEndianReader bigEndianReader = new BigEndianReader(new MemoryStream(rawData));
		Id = bigEndianReader.ReadInt32();
	}
}
