using System;

namespace PsdSharp.Images.DataConversion;

public static class PixelDataConverter
{
	public static byte[] GetInterleavedBuffer(ImageData imageData, ColorType colorType)
	{
		ColorMode colorMode = imageData.ColorMode;
		if (colorMode - 3 <= ColorMode.Grayscale)
		{
			return RgbInterleaver.ToInterleavedBuffer(imageData, colorType);
		}
		if (imageData.ColorMode == ColorMode.Bitmap)
		{
			return BitmapPlanarToRgbInterleavedConverter.Convert(imageData, colorType);
		}
		colorMode = imageData.ColorMode;
		if ((colorMode == ColorMode.Grayscale || colorMode - 7 <= ColorMode.Grayscale) ? true : false)
		{
			return GreyscalePlanarToRgbInterleavedConverter.Convert(imageData, colorType);
		}
		if (imageData.ColorMode == ColorMode.Indexed)
		{
			return IndexedPalleteToRgbInterleavedConverter.Convert(imageData, colorType);
		}
		if (imageData.ColorMode == ColorMode.Lab)
		{
			return LabPlanarToRgbInterleavedConverter.Convert(imageData, colorType);
		}
		throw new NotSupportedException($"Color mode {imageData.ColorMode} is not supported.");
	}
}
