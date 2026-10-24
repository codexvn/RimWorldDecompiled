using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PsdSharp.Images.DataConversion;

internal static class RgbInterleaver
{
	public static byte[] ToInterleavedBuffer(ImageData imageData, ColorType desiredColorType)
	{
		ImageData.ChannelData[] array = imageData.GetChannels().ToArray();
		Dictionary<Channel, byte[]> channels = new Dictionary<Channel, byte[]>
		{
			{
				Channel.Red,
				array[0].GetData()
			},
			{
				Channel.Green,
				array[1].GetData()
			},
			{
				Channel.Blue,
				array[2].GetData()
			}
		};
		if (array.Length > 3)
		{
			channels.Add(Channel.Alpha, array[3].GetData());
		}
		int sourceBytesPerPixel = imageData.ChannelDepth / 8;
		int pixelCount = (int)(imageData.Width * imageData.Height);
		int destBytesPerPixel = (int)Math.Ceiling((double)desiredColorType.Channels.Sum((ChannelInfo x) => x.PixelCount) / 8.0);
		byte[] interleavedBuffer = new byte[pixelCount * destBytesPerPixel];
		ChannelInfo[] destinationChannels = desiredColorType.Channels;
		int chunkSize = 50000;
		Parallel.For(0, (pixelCount + chunkSize - 1) / chunkSize, delegate(int chunk)
		{
			int num = chunk * chunkSize;
			int num2 = Math.Min(num + chunkSize, pixelCount);
			for (int i = num; i < num2; i++)
			{
				int num3 = i * sourceBytesPerPixel;
				int num4 = i * destBytesPerPixel;
				for (int j = 0; j < destinationChannels.Length; j++)
				{
					if (!channels.ContainsKey(destinationChannels[j].Channel))
					{
						interleavedBuffer[num4 + j] = byte.MaxValue;
					}
					else
					{
						byte[] array2 = channels[destinationChannels[j].Channel];
						byte[] array3 = interleavedBuffer;
						int num5 = num4 + j;
						array3[num5] = sourceBytesPerPixel switch
						{
							1 => array2[num3], 
							2 => (byte)(BinaryPrimitives.ReadUInt16BigEndian(array2.AsSpan(num3, 2)) >> 8), 
							_ => (byte)(Math.Clamp(BinaryPrimitives.ReadSingleBigEndian(array2.AsSpan(num3, 4)), 0f, 1f) * 255f), 
						};
					}
				}
			}
		});
		return interleavedBuffer;
	}
}
