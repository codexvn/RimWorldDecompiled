using System;
using System.Collections.Generic;
using System.Globalization;

namespace UnityEngine;

public class ColorUtility
{
	private static readonly Dictionary<string, Color> NamedColors = new Dictionary<string, Color>(StringComparer.OrdinalIgnoreCase)
	{
		["red"] = new Color32(byte.MaxValue, 0, 0, byte.MaxValue),
		["cyan"] = new Color32(0, byte.MaxValue, byte.MaxValue, byte.MaxValue),
		["blue"] = new Color32(0, 0, byte.MaxValue, byte.MaxValue),
		["darkblue"] = new Color32(0, 0, 160, byte.MaxValue),
		["lightblue"] = new Color32(173, 216, 230, byte.MaxValue),
		["purple"] = new Color32(128, 0, 128, byte.MaxValue),
		["yellow"] = new Color32(byte.MaxValue, byte.MaxValue, 0, byte.MaxValue),
		["lime"] = new Color32(0, byte.MaxValue, 0, byte.MaxValue),
		["fuchsia"] = new Color32(byte.MaxValue, 0, byte.MaxValue, byte.MaxValue),
		["white"] = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue),
		["silver"] = new Color32(192, 192, 192, byte.MaxValue),
		["grey"] = new Color32(128, 128, 128, byte.MaxValue),
		["black"] = new Color32(0, 0, 0, byte.MaxValue),
		["orange"] = new Color32(byte.MaxValue, 165, 0, byte.MaxValue),
		["brown"] = new Color32(165, 42, 42, byte.MaxValue),
		["maroon"] = new Color32(128, 0, 0, byte.MaxValue),
		["green"] = new Color32(0, 128, 0, byte.MaxValue),
		["olive"] = new Color32(128, 128, 0, byte.MaxValue),
		["navy"] = new Color32(0, 0, 128, byte.MaxValue),
		["teal"] = new Color32(0, 128, 128, byte.MaxValue),
		["aqua"] = new Color32(0, byte.MaxValue, byte.MaxValue, byte.MaxValue),
		["magenta"] = new Color32(byte.MaxValue, 0, byte.MaxValue, byte.MaxValue)
	};

	public static string ToHtmlStringRGBA(Color color)
	{
		Color32 color2 = color;
		return $"{color2.r:X2}{color2.g:X2}{color2.b:X2}{color2.a:X2}";
	}

	public static string ToHtmlStringRGB(Color color)
	{
		Color32 color2 = color;
		return $"{color2.r:X2}{color2.g:X2}{color2.b:X2}";
	}

	public static bool TryParseHtmlString(string htmlString, out Color color)
	{
		color = default(Color);
		if (string.IsNullOrEmpty(htmlString))
		{
			return false;
		}
		if (htmlString[0] != '#')
		{
			if (!NamedColors.TryGetValue(htmlString, out var value))
			{
				return false;
			}
			color = value;
			return true;
		}
		ReadOnlySpan<char> s = htmlString.AsSpan(1);
		bool flag;
		switch (s.Length)
		{
		case 3:
		case 4:
		case 6:
		case 8:
			flag = true;
			break;
		default:
			flag = false;
			break;
		}
		if (!flag)
		{
			return false;
		}
		if (!uint.TryParse(s, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var result))
		{
			return false;
		}
		byte a = byte.MaxValue;
		byte r;
		byte g;
		byte b;
		if (s.Length <= 4)
		{
			int num = ((s.Length == 4) ? 12 : 8);
			r = Nibble(result >> num);
			g = Nibble(result >> num - 4);
			b = Nibble(result >> num - 8);
			if (s.Length == 4)
			{
				a = Nibble(result);
			}
		}
		else
		{
			int num2 = ((s.Length == 8) ? 24 : 16);
			r = (byte)(result >> num2);
			g = (byte)(result >> num2 - 8);
			b = (byte)(result >> num2 - 16);
			if (s.Length == 8)
			{
				a = (byte)result;
			}
		}
		color = new Color32(r, g, b, a);
		return true;
	}

	private static byte Nibble(uint v)
	{
		return (byte)((v & 0xF) * 17);
	}
}
