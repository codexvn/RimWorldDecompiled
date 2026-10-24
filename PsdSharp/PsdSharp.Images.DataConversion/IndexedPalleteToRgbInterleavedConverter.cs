using System;
using System.Linq;
using System.Threading.Tasks;

namespace PsdSharp.Images.DataConversion;

internal static class IndexedPalleteToRgbInterleavedConverter
{
	public static byte[] Convert(ImageData imageData, ColorType destinationColorType)
	{
		byte[] red = imageData.ColorModeData.AsSpan(0, 256).ToArray();
		byte[] green = imageData.ColorModeData.AsSpan(256, 256).ToArray();
		byte[] blue = imageData.ColorModeData.AsSpan(512, 256).ToArray();
		byte[] sourceData = imageData.GetChannels().First().GetData();
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
				byte b = sourceData[i];
				int num3 = i * destBytesPerPixel;
				for (int j = 0; j < destinationChannels.Length; j++)
				{
					byte[] array = interleavedBuffer;
					int num4 = num3 + j;
					array[num4] = destinationChannels[j].Channel switch
					{
						Channel.Red => red[b], 
						Channel.Green => green[b], 
						Channel.Blue => blue[b], 
						_ => byte.MaxValue, 
					};
				}
			}
		});
		return interleavedBuffer;
	}
}
