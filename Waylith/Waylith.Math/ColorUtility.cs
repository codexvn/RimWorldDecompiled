using System;
using System.Globalization;

namespace Waylith.Math;

public static class ColorUtility
{
	public static string ToHtmlStringRGBA(Color color)
	{
		Color32 color2 = color;
		return $"{color2.r:X2}{color2.g:X2}{color2.b:X2}{color2.a:X2}";
	}

	public static bool TryParseHtmlString(string htmlString, out Color color)
	{
		color = default(Color);
		if (string.IsNullOrWhiteSpace(htmlString))
		{
			return false;
		}
		ReadOnlySpan<char> readOnlySpan = htmlString.AsSpan().Trim();
		if (!readOnlySpan.IsEmpty && readOnlySpan[0] == '#')
		{
			readOnlySpan = readOnlySpan.Slice(1);
		}
		if (readOnlySpan.Length != 6 && readOnlySpan.Length != 8)
		{
			return false;
		}
		if (!byte.TryParse(readOnlySpan.Slice(0, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var result) || !byte.TryParse(readOnlySpan.Slice(2, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var result2) || !byte.TryParse(readOnlySpan.Slice(4, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var result3))
		{
			return false;
		}
		byte result4 = byte.MaxValue;
		if (readOnlySpan.Length == 8 && !byte.TryParse(readOnlySpan.Slice(6, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out result4))
		{
			return false;
		}
		color = new Color32(result, result2, result3, result4);
		return true;
	}
}
