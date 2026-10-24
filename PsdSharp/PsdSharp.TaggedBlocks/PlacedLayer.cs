using System.IO;
using PsdSharp.Descriptors;
using PsdSharp.Parsing;

namespace PsdSharp.TaggedBlocks;

public class PlacedLayer : TaggedBlock
{
	public enum PlacedLayerTypeEnum
	{
		Unknown,
		Vector,
		Raster,
		ImageStack
	}

	public int Version { get; }

	public string UniqueId { get; }

	public int PageNumber { get; }

	public int TotalPages { get; }

	public int AntiAliasPolicy { get; }

	public PlacedLayerTypeEnum PlacedLayerType { get; }

	public double[] Transformation { get; }

	public int WarpVersion { get; }

	public int WarpDescriptorVersion { get; }

	public Descriptor WarpDescriptor { get; }

	public PlacedLayer(AdditionalLayerInfoKey key, byte[] rawData)
		: base(key, rawData)
	{
		BigEndianReader bigEndianReader = new BigEndianReader(new MemoryStream(rawData));
		bigEndianReader.Skip(4);
		Version = bigEndianReader.ReadInt32();
		UniqueId = bigEndianReader.ReadPascalString(0).String;
		PageNumber = bigEndianReader.ReadInt32();
		TotalPages = bigEndianReader.ReadInt32();
		AntiAliasPolicy = bigEndianReader.ReadInt32();
		PlacedLayerType = (PlacedLayerTypeEnum)bigEndianReader.ReadInt32();
		Transformation = new double[8];
		for (int i = 0; i < 8; i++)
		{
			Transformation[i] = bigEndianReader.ReadDouble();
		}
		WarpVersion = bigEndianReader.ReadInt32();
		WarpDescriptorVersion = bigEndianReader.ReadInt32();
		WarpDescriptor = new Descriptor(bigEndianReader);
	}
}
