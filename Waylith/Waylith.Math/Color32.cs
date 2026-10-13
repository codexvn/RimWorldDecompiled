using System;
using System.Globalization;

namespace Waylith.Math;

public struct Color32 : IEquatable<Color32>, IFormattable
{
	public byte r;

	public byte g;

	public byte b;

	public byte a;

	public bool Equals(Color32 other)
	{
		if (r == other.r && g == other.g && b == other.b)
		{
			return a == other.a;
		}
		return false;
	}

	public string ToString(string format, IFormatProvider formatProvider)
	{
		return $"RGBA({r}, {g}, {b}, {a})";
	}

	public override string ToString()
	{
		return ToString("F2", CultureInfo.InvariantCulture);
	}

	public Color32(byte r, byte g, byte b, byte a)
	{
		this.r = r;
		this.g = g;
		this.b = b;
		this.a = a;
	}

	public static implicit operator Color32(Color c)
	{
		return new Color32(ClampToByte(c.r), ClampToByte(c.g), ClampToByte(c.b), ClampToByte(c.a));
		static byte ClampToByte(float value)
		{
			return (byte)System.Math.Clamp((int)MathF.Round(value * 255f), 0, 255);
		}
	}

	public static implicit operator Color(Color32 c)
	{
		return new Color((float)(int)c.r / 255f, (float)(int)c.g / 255f, (float)(int)c.b / 255f, (float)(int)c.a / 255f);
	}
}
