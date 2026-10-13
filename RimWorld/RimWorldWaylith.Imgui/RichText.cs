using System;
using System.Globalization;
using UnityEngine;

namespace RimWorldWaylith.Imgui;

internal static class RichText
{
	internal enum Tag
	{
		None,
		ColorOpen,
		ColorOpenInvalid,
		ColorClose,
		BoldOpen,
		BoldClose,
		ItalicOpen,
		ItalicClose,
		SizeOpen,
		SizeOpenInvalid,
		SizeClose
	}

	public const float MaxSize = 256f;

	private static readonly (string Name, Color Color)[] NamedColors = new(string, Color)[24]
	{
		("red", Color.red),
		("green", new Color(0f, 1f, 0f)),
		("blue", Color.blue),
		("white", Color.white),
		("black", Color.black),
		("yellow", Color.yellow),
		("cyan", Color.cyan),
		("aqua", Color.cyan),
		("magenta", Color.magenta),
		("fuchsia", Color.magenta),
		("gray", Color.gray),
		("grey", Color.gray),
		("clear", Color.clear),
		("orange", new Color(1f, 0.647f, 0f)),
		("brown", new Color(0.667f, 0.29f, 0f)),
		("darkblue", new Color(0f, 0f, 0.627f)),
		("lightblue", new Color(0.678f, 0.847f, 0.902f)),
		("lime", new Color(0f, 1f, 0f)),
		("maroon", new Color(0.5f, 0f, 0f)),
		("navy", new Color(0f, 0f, 0.5f)),
		("olive", new Color(0.5f, 0.5f, 0f)),
		("purple", new Color(0.5f, 0f, 0.5f)),
		("silver", new Color(0.753f, 0.753f, 0.753f)),
		("teal", new Color(0f, 0.5f, 0.5f))
	};

	public static Tag Parse(string s, int i, out int end, out Color color, out float size)
	{
		end = i;
		color = default(Color);
		size = 0f;
		int num = s.IndexOf('>', i + 1);
		if (num < 0)
		{
			return Tag.None;
		}
		ReadOnlySpan<char> span = s.AsSpan(i + 1, num - i - 1);
		Tag result;
		if (span.StartsWith("color=".AsSpan(), StringComparison.OrdinalIgnoreCase))
		{
			result = (TryParseColor(span.Slice(6), out color) ? Tag.ColorOpen : Tag.ColorOpenInvalid);
		}
		else if (MemoryExtensions.Equals(span, "/color".AsSpan(), StringComparison.OrdinalIgnoreCase))
		{
			result = Tag.ColorClose;
		}
		else if (MemoryExtensions.Equals(span, "b".AsSpan(), StringComparison.OrdinalIgnoreCase))
		{
			result = Tag.BoldOpen;
		}
		else if (MemoryExtensions.Equals(span, "/b".AsSpan(), StringComparison.OrdinalIgnoreCase))
		{
			result = Tag.BoldClose;
		}
		else if (MemoryExtensions.Equals(span, "i".AsSpan(), StringComparison.OrdinalIgnoreCase))
		{
			result = Tag.ItalicOpen;
		}
		else if (MemoryExtensions.Equals(span, "/i".AsSpan(), StringComparison.OrdinalIgnoreCase))
		{
			result = Tag.ItalicClose;
		}
		else if (span.StartsWith("size=".AsSpan(), StringComparison.OrdinalIgnoreCase))
		{
			result = (TryParseSize(span.Slice(5), out size) ? Tag.SizeOpen : Tag.SizeOpenInvalid);
		}
		else
		{
			if (!MemoryExtensions.Equals(span, "/size".AsSpan(), StringComparison.OrdinalIgnoreCase))
			{
				return Tag.None;
			}
			result = Tag.SizeClose;
		}
		end = num + 1;
		return result;
	}

	private static bool TryParseSize(ReadOnlySpan<char> spec, out float size)
	{
		spec = spec.Trim().Trim("\"'".AsSpan());
		if (spec.EndsWith("px".AsSpan(), StringComparison.OrdinalIgnoreCase))
		{
			spec = spec.Slice(0, spec.Length - 2);
		}
		if (!float.TryParse(spec, NumberStyles.Float, CultureInfo.InvariantCulture, out size) || size <= 0f)
		{
			return false;
		}
		size = MathF.Min(size, 256f);
		return true;
	}

	public static bool TryParseColor(ReadOnlySpan<char> spec, out Color color)
	{
		color = Color.white;
		spec = spec.Trim().Trim("\"'".AsSpan());
		if (spec.Length == 0)
		{
			return false;
		}
		if (spec[0] == '#')
		{
			return TryParseHex(spec.Slice(1), out color);
		}
		return TryParseNamed(spec, out color);
	}

	private static bool TryParseHex(ReadOnlySpan<char> hex, out Color color)
	{
		color = Color.white;
		float value = 1f;
		float value2;
		float value3;
		float value4;
		switch (hex.Length)
		{
		case 3:
			if (!Nibble(hex[0], out value2) || !Nibble(hex[1], out value3) || !Nibble(hex[2], out value4))
			{
				return false;
			}
			color = new Color(value2, value3, value4, 1f);
			return true;
		case 4:
			if (!Nibble(hex[0], out value2) || !Nibble(hex[1], out value3) || !Nibble(hex[2], out value4) || !Nibble(hex[3], out value))
			{
				return false;
			}
			color = new Color(value2, value3, value4, value);
			return true;
		case 8:
			if (!Byte(hex[6], hex[7], out value))
			{
				return false;
			}
			goto case 6;
		case 6:
			if (!Byte(hex[0], hex[1], out value2) || !Byte(hex[2], hex[3], out value3) || !Byte(hex[4], hex[5], out value4))
			{
				return false;
			}
			color = new Color(value2, value3, value4, value);
			return true;
		default:
			return false;
		}
	}

	private static bool Nibble(char c, out float value)
	{
		int num = HexDigit(c);
		value = ((num < 0) ? 0f : ((float)(num * 17) / 255f));
		return num >= 0;
	}

	private static bool Byte(char hi, char lo, out float value)
	{
		int num = HexDigit(hi);
		int num2 = HexDigit(lo);
		if (num < 0 || num2 < 0)
		{
			value = 0f;
			return false;
		}
		value = (float)((num << 4) | num2) / 255f;
		return true;
	}

	private static int HexDigit(char c)
	{
		switch (c)
		{
		case '0':
		case '1':
		case '2':
		case '3':
		case '4':
		case '5':
		case '6':
		case '7':
		case '8':
		case '9':
			return c - 48;
		case 'a':
		case 'b':
		case 'c':
		case 'd':
		case 'e':
		case 'f':
			return c - 97 + 10;
		case 'A':
		case 'B':
		case 'C':
		case 'D':
		case 'E':
		case 'F':
			return c - 65 + 10;
		default:
			return -1;
		}
	}

	private static bool TryParseNamed(ReadOnlySpan<char> name, out Color color)
	{
		(string, Color)[] namedColors = NamedColors;
		for (int i = 0; i < namedColors.Length; i++)
		{
			var (text, color2) = namedColors[i];
			if (MemoryExtensions.Equals(name, text.AsSpan(), StringComparison.OrdinalIgnoreCase))
			{
				color = color2;
				return true;
			}
		}
		color = Color.white;
		return false;
	}
}
