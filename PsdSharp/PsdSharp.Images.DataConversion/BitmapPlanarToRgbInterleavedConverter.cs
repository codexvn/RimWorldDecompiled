using System.Linq;

namespace PsdSharp.Images.DataConversion;

internal static class BitmapPlanarToRgbInterleavedConverter
{
	public static byte[] Convert(ImageData imageData, ColorType destinationColorType)
	{
		byte[] data = imageData.GetChannels().First().GetData();
		uint num = (imageData.Width + 7) / 8;
		byte[] array = new byte[imageData.Width * imageData.Height * 4];
		ChannelInfo[] channels = destinationColorType.Channels;
		for (int i = 0; i < imageData.Height; i++)
		{
			long num2 = i * num;
			for (int j = 0; j < imageData.Width; j++)
			{
				long num3 = num2 + (j >> 3);
				int num4 = 7 - (j & 7);
				bool num5 = ((data[num3] >> num4) & 1) == 0;
				long num6 = (i * imageData.Width + j) * 4;
				byte b = (byte)(num5 ? 255u : 0u);
				array[num6] = ((channels[0].Channel == Channel.Alpha) ? byte.MaxValue : b);
				array[num6 + 1] = ((channels[1].Channel == Channel.Alpha) ? byte.MaxValue : b);
				array[num6 + 2] = ((channels[2].Channel == Channel.Alpha) ? byte.MaxValue : b);
				array[num6 + 3] = ((channels[3].Channel == Channel.Alpha) ? byte.MaxValue : b);
			}
		}
		return array;
	}
}
