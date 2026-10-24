using System.Numerics;
using Waylith.VideoDecode.H264;

namespace Waylith.VideoDecode;

internal struct Nv12ToRgbaPush
{
	public Vector4 RowR;

	public Vector4 RowG;

	public Vector4 RowB;

	public int CropX;

	public int CropY;

	public int Width;

	public int Height;

	public static Nv12ToRgbaPush ForStream(H264Sps sps)
	{
		return Create(MatrixFor(sps), sps.VideoSignalTypePresent && sps.VideoFullRange, sps.CropX, sps.CropY, sps.VisibleWidth, sps.VisibleHeight);
	}

	public static YuvMatrix MatrixFor(H264Sps sps)
	{
		if (!sps.ColourDescriptionPresent || sps.MatrixCoefficients == 2)
		{
			if (sps.VisibleHeight < 720)
			{
				return YuvMatrix.Bt601;
			}
			return YuvMatrix.Bt709;
		}
		if (sps.MatrixCoefficients != 1)
		{
			return YuvMatrix.Bt601;
		}
		return YuvMatrix.Bt709;
	}

	public static Nv12ToRgbaPush Create(YuvMatrix matrix, bool fullRange, int cropX, int cropY, int width, int height)
	{
		float num2;
		float num3;
		if (matrix != YuvMatrix.Bt709)
		{
			float num = 0.114f;
			num2 = num;
			num3 = 0.299f;
		}
		else
		{
			float num = 0.0722f;
			num2 = num;
			num3 = 0.2126f;
		}
		float num4 = 1f - num3 - num2;
		float num5 = (fullRange ? 1f : 1.1643835f);
		float num6 = (fullRange ? 1f : 1.1383928f);
		float num7 = (fullRange ? 0f : 0.0627451f);
		float num8 = 2f * (1f - num3) * num6;
		float num9 = 2f * (1f - num2) * num6;
		float num10 = -2f * num2 * (1f - num2) / num4 * num6;
		float num11 = -2f * num3 * (1f - num3) / num4 * num6;
		Nv12ToRgbaPush result = default(Nv12ToRgbaPush);
		result.RowR = new Vector4(num5, 0f, num8, (0f - num5) * num7 - num8 * 0.5019608f);
		result.RowG = new Vector4(num5, num10, num11, (0f - num5) * num7 - (num10 + num11) * 0.5019608f);
		result.RowB = new Vector4(num5, num9, 0f, (0f - num5) * num7 - num9 * 0.5019608f);
		result.CropX = cropX;
		result.CropY = cropY;
		result.Width = width;
		result.Height = height;
		return result;
	}
}
