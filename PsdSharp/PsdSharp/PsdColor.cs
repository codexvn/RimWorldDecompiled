using System;
using System.Buffers.Binary;
using PsdSharp.Compat;
using PsdSharp.Parsing;

namespace PsdSharp;

public struct PsdColor
{
	public ColorSpaceId ColorSpace { get; set; }

	public byte[] Data { get; set; }

	public PsdColor(ColorSpaceId colorSpace, byte[] data)
	{
		ColorSpace = colorSpace;
		Data = data;
	}

	public static PsdColor FromRgb(ushort r, ushort g, ushort b)
	{
		byte[] array = new byte[10];
		BinaryPrimitives.WriteUInt16BigEndian(array.AsSpan(0, 2), r);
		BinaryPrimitives.WriteUInt16BigEndian(array.AsSpan(2, 2), g);
		BinaryPrimitives.WriteUInt16BigEndian(array.AsSpan(4, 2), b);
		return new PsdColor(ColorSpaceId.Rgb, array);
	}

	public static PsdColor FromRgb(byte r, byte g, byte b)
	{
		return FromRgb((ushort)r, (ushort)g, (ushort)b);
	}

	internal static PsdColor FromPhotoshopData(BigEndianReader reader)
	{
		ushort colorSpace = reader.ReadUInt16();
		byte[] array = new byte[8];
		reader.ReadIntoBuffer(array);
		return new PsdColor((ColorSpaceId)colorSpace, array);
	}

	public (ushort Red, ushort Green, ushort Blue) ToRgb()
	{
		switch (ColorSpace)
		{
		case ColorSpaceId.Rgb:
		{
			ushort item = BinaryPrimitives.ReadUInt16BigEndian(Data.AsSpan(0, 2));
			ushort item2 = BinaryPrimitives.ReadUInt16BigEndian(Data.AsSpan(2, 2));
			ushort item3 = BinaryPrimitives.ReadUInt16BigEndian(Data.AsSpan(4, 2));
			return (Red: item, Green: item2, Blue: item3);
		}
		case ColorSpaceId.Hsb:
		{
			float h = (float)(int)BinaryPrimitives.ReadUInt16BigEndian(Data.AsSpan(0, 2)) / 65535f;
			float s = (float)(int)BinaryPrimitives.ReadUInt16BigEndian(Data.AsSpan(2, 2)) / 65535f;
			float v = (float)(int)BinaryPrimitives.ReadUInt16BigEndian(Data.AsSpan(4, 2)) / 65535f;
			return HsbToRgb(h, s, v);
		}
		case ColorSpaceId.Cmyk:
		{
			ushort c = BinaryPrimitives.ReadUInt16BigEndian(Data.AsSpan(0, 2));
			ushort m = BinaryPrimitives.ReadUInt16BigEndian(Data.AsSpan(2, 2));
			ushort y = BinaryPrimitives.ReadUInt16BigEndian(Data.AsSpan(4, 2));
			ushort k = BinaryPrimitives.ReadUInt16BigEndian(Data.AsSpan(6, 2));
			return CmykToRgb(c, m, y, k);
		}
		case ColorSpaceId.Lab:
		{
			ushort l = BinaryPrimitives.ReadUInt16BigEndian(Data.AsSpan(0, 2));
			short a = BinaryPrimitives.ReadInt16BigEndian(Data.AsSpan(2, 2));
			short b = BinaryPrimitives.ReadInt16BigEndian(Data.AsSpan(4, 2));
			return LabToRgb(l, a, b);
		}
		case ColorSpaceId.Grayscale:
		{
			ushort num = (ushort)Math.Round(MathCompat.Clamp((float)(int)BinaryPrimitives.ReadUInt16BigEndian(Data.AsSpan(0, 2)) / 10000f, 0f, 1f) * 65535f);
			return (Red: num, Green: num, Blue: num);
		}
		default:
			return (Red: 0, Green: 0, Blue: 0);
		}
	}

	private static (ushort Red, ushort Green, ushort Blue) HsbToRgb(float h, float s, float v)
	{
		float num3;
		float num2;
		float num;
		if (s == 0f)
		{
			num3 = (num2 = (num = v));
		}
		else
		{
			h = (h - MathF.Floor(h)) * 6f;
			int num4 = (int)MathF.Floor(h);
			float num5 = h - (float)num4;
			float num6 = v * (1f - s);
			float num7 = v * (1f - num5 * s);
			float num8 = v * (1f - (1f - num5) * s);
			switch (num4)
			{
			case 0:
				num3 = v;
				num2 = num8;
				num = num6;
				break;
			case 1:
				num3 = num7;
				num2 = v;
				num = num6;
				break;
			case 2:
				num3 = num6;
				num2 = v;
				num = num8;
				break;
			case 3:
				num3 = num6;
				num2 = num7;
				num = v;
				break;
			case 4:
				num3 = num8;
				num2 = num6;
				num = v;
				break;
			default:
				num3 = v;
				num2 = num6;
				num = num7;
				break;
			}
		}
		return (Red: (ushort)Math.Round(num3 * 65535f), Green: (ushort)Math.Round(num2 * 65535f), Blue: (ushort)Math.Round(num * 65535f));
	}

	private static (ushort Red, ushort Green, ushort Blue) CmykToRgb(ushort c16, ushort m16, ushort y16, ushort k16)
	{
		float num = 1f - (float)(int)c16 / 65535f;
		float num2 = 1f - (float)(int)m16 / 65535f;
		float num3 = 1f - (float)(int)y16 / 65535f;
		float num4 = 1f - (float)(int)k16 / 65535f;
		float num5 = (1f - num) * (1f - num4);
		float num6 = (1f - num2) * (1f - num4);
		float num7 = (1f - num3) * (1f - num4);
		return (Red: (ushort)Math.Round(num5 * 65535f), Green: (ushort)Math.Round(num6 * 65535f), Blue: (ushort)Math.Round(num7 * 65535f));
	}

	private static (ushort Red, ushort Green, ushort Blue) LabToRgb(ushort l16, short a16, short b16)
	{
		float num = (float)(int)l16 * 0.01f;
		float num2 = (float)a16 / 100f;
		float num3 = (float)b16 / 100f;
		float num4 = (num + 16f) / 116f;
		float num5 = num2 / 500f + num4;
		float num6 = num4 - num3 / 200f;
		float num7 = num5 * num5 * num5;
		float num8 = num4 * num4 * num4;
		float num9 = num6 * num6 * num6;
		num5 = ((num7 > 0.008856f) ? num7 : ((num5 - 0.13793103f) / 7.787f));
		num4 = ((num8 > 0.008856f) ? num8 : ((num4 - 0.13793103f) / 7.787f));
		num6 = ((num9 > 0.008856f) ? num9 : ((num6 - 0.13793103f) / 7.787f));
		num5 *= 0.95047f;
		num4 *= 1f;
		num6 *= 1.08883f;
		float c2 = 3.2406f * num5 - 1.5372f * num4 - 0.4986f * num6;
		float c3 = -0.9689f * num5 + 1.8758f * num4 + 0.0415f * num6;
		float c4 = 0.0557f * num5 - 0.204f * num4 + 1.057f * num6;
		float value = Gamma(c2);
		c3 = Gamma(c3);
		c4 = Gamma(c4);
		float num10 = MathCompat.Clamp(value, 0f, 1f);
		c3 = MathCompat.Clamp(c3, 0f, 1f);
		return new ValueTuple<ushort, ushort, ushort>(item3: (ushort)Math.Round(MathCompat.Clamp(c4, 0f, 1f) * 65535f), item1: (ushort)Math.Round(num10 * 65535f), item2: (ushort)Math.Round(c3 * 65535f));
		static float Gamma(float c)
		{
			if (!(c <= 0.0031308f))
			{
				return 1.055f * MathF.Pow(c, 5f / 12f) - 0.055f;
			}
			return 12.92f * c;
		}
	}
}
