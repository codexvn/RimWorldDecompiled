using System;
using System.Buffers.Binary;
using System.Linq;
using System.Threading.Tasks;

namespace PsdSharp.Images.DataConversion;

internal static class GreyscalePlanarToRgbInterleavedConverter
{
	public static byte[] Convert(ImageData imageData, ColorType destinationColorType)
	{
		byte[] sourceData = imageData.GetChannels().First().GetData();
		int sourceBytesPerPixel = imageData.ChannelDepth / 8;
		int pixelCount = (int)(imageData.Width * imageData.Height);
		int destBytesPerPixel = (int)Math.Ceiling((double)destinationColorType.Channels.Sum((ChannelInfo x) => x.PixelCount) / 8.0);
		byte[] interleavedBuffer = new byte[pixelCount * destBytesPerPixel];
		ChannelInfo[] destinationChannels = destinationColorType.Channels;
		int chunkSize = 50000;
		Parallel.For(0, (pixelCount + chunkSize - 1) / chunkSize, delegate(int chunk)
		{
			int num = chunk * chunkSize;
			int num2 = Math.Min(num + chunkSize, pixelCount);
			for (int i = num; i < num2; i++)
			{
				int num3 = i * sourceBytesPerPixel;
				int num4 = i * destBytesPerPixel;
				byte b = sourceBytesPerPixel switch
				{
					1 => sourceData[num3], 
					2 => (byte)(BinaryPrimitives.ReadUInt16BigEndian(sourceData.AsSpan(num3, 2)) >> 8), 
					_ => (byte)(Math.Clamp(BinaryPrimitives.ReadSingleBigEndian(sourceData.AsSpan(num3, 4)), 0f, 1f) * 255f), 
				};
				for (int j = 0; j < destinationChannels.Length; j++)
				{
					interleavedBuffer[num4 + j] = ((destinationChannels[j].Channel == Channel.Alpha) ? byte.MaxValue : b);
				}
			}
		});
		return interleavedBuffer;
	}
}
