using System;
using System.Buffers.Binary;

namespace PsdSharp.ImageResources;

public class ResolutionInfo : ImageResource
{
	public enum ResolutionDisplayUnit : short
	{
		PixelsPerInch = 1,
		PixelsPerCentimeter
	}

	public enum SizeDisplayUnit : short
	{
		Inches = 1,
		Centimeters,
		Points,
		Picas,
		Columns
	}

	private const double FixedPointDivisor = 65536.0;

	public double HorizontalResolutionDPI { get; set; }

	public ResolutionDisplayUnit HorizontalResolutionDisplayUnit { get; set; }

	public SizeDisplayUnit WidthDisplayUnit { get; set; }

	public double VerticalResolutionDPI { get; set; }

	public ResolutionDisplayUnit VerticalResolutionDisplayUnit { get; set; }

	public SizeDisplayUnit HeightDisplayUnit { get; set; }

	public ResolutionInfo(string? name, byte[] rawData)
	{
		base.Id = ImageResourceId.ResolutionInfo;
		base.Name = name;
		base.RawData = rawData;
		HorizontalResolutionDPI = (double)BinaryPrimitives.ReadInt32BigEndian(rawData.AsSpan(0, 4)) / 65536.0;
		HorizontalResolutionDisplayUnit = (ResolutionDisplayUnit)BinaryPrimitives.ReadInt16BigEndian(rawData.AsSpan(4, 2));
		WidthDisplayUnit = (SizeDisplayUnit)BinaryPrimitives.ReadInt16BigEndian(rawData.AsSpan(6, 2));
		VerticalResolutionDPI = (double)BinaryPrimitives.ReadInt32BigEndian(rawData.AsSpan(8, 4)) / 65536.0;
		VerticalResolutionDisplayUnit = (ResolutionDisplayUnit)BinaryPrimitives.ReadInt16BigEndian(rawData.AsSpan(12, 2));
		HeightDisplayUnit = (SizeDisplayUnit)BinaryPrimitives.ReadInt16BigEndian(rawData.AsSpan(14, 2));
	}

	public ResolutionInfo()
	{
	}
}
