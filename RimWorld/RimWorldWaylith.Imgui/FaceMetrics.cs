using System;
using System.Buffers.Binary;
using System.IO;
using Brutal.ImGuiApi;

namespace RimWorldWaylith.Imgui;

internal readonly struct FaceMetrics
{
	public readonly float Ascender;

	public readonly float Descender;

	public readonly float LineSpacing;

	public FaceMetrics(float ascender, float descender, float lineSpacing)
	{
		Ascender = ascender;
		Descender = descender;
		LineSpacing = lineSpacing;
	}

	public LineMetrics At(float size)
	{
		float num = Snap(Ascender * size);
		float num2 = LineSpacing * size;
		return new LineMetrics(num, num - Snap(Descender * size), Snap(num2), num2);
	}

	private static float Snap(float v)
	{
		return MathF.Floor(v + 0.5f);
	}

	public static FaceMetrics Load(string path, ImFontPtr font, float size)
	{
		if (!TryRead(path, out var metrics))
		{
			return FromBaked(font, size);
		}
		return metrics;
	}

	public static FaceMetrics Load(ReadOnlySpan<byte> data, ImFontPtr font, float size)
	{
		if (!TryRead(data, out var metrics))
		{
			return FromBaked(font, size);
		}
		return metrics;
	}

	private static FaceMetrics FromBaked(ImFontPtr font, float size)
	{
		ImFontBakedPtr fontBaked = font.GetFontBaked(size);
		return new FaceMetrics(fontBaked.Ascent / size, fontBaked.Descent / size, (fontBaked.Ascent - fontBaked.Descent) / size);
	}

	private static bool TryRead(string path, out FaceMetrics metrics)
	{
		metrics = default(FaceMetrics);
		if (File.Exists(path))
		{
			return TryRead(File.ReadAllBytes(path), out metrics);
		}
		return false;
	}

	public static bool TryRead(ReadOnlySpan<byte> data, out FaceMetrics metrics)
	{
		metrics = default(FaceMetrics);
		if (data.Length < 12)
		{
			return false;
		}
		int num = -1;
		int num2 = -1;
		ushort num3 = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(4));
		if (12 + num3 * 16 > data.Length)
		{
			return false;
		}
		for (int i = 0; i < num3; i++)
		{
			ReadOnlySpan<byte> source = data.Slice(12 + i * 16, 16);
			int num4 = (int)BinaryPrimitives.ReadUInt32BigEndian(source.Slice(8));
			switch (BinaryPrimitives.ReadUInt32BigEndian(source))
			{
			case 1751474532u:
				num = num4;
				break;
			case 1751672161u:
				num2 = num4;
				break;
			}
		}
		if (num < 0 || num2 < 0 || num + 20 > data.Length || num2 + 10 > data.Length)
		{
			return false;
		}
		float num5 = (int)BinaryPrimitives.ReadUInt16BigEndian(data.Slice(num + 18));
		if (num5 == 0f)
		{
			return false;
		}
		float num6 = BinaryPrimitives.ReadInt16BigEndian(data.Slice(num2 + 4));
		float num7 = BinaryPrimitives.ReadInt16BigEndian(data.Slice(num2 + 6));
		float num8 = BinaryPrimitives.ReadInt16BigEndian(data.Slice(num2 + 8));
		metrics = new FaceMetrics(num6 / num5, num7 / num5, (num6 - num7 + num8) / num5);
		return true;
	}
}
