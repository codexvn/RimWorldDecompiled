using System;
using System.Globalization;
using Waylith.Math;

namespace UnityEngine;

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
			return (byte)Math.Clamp((int)MathF.Round(value * 255f), 0, 255);
		}
	}

	public static implicit operator Color(Color32 c)
	{
		return new Color((float)(int)c.r / 255f, (float)(int)c.g / 255f, (float)(int)c.b / 255f, (float)(int)c.a / 255f);
	}

	public static Color32 Lerp(Color32 a, Color32 b, float t)
	{
		return LerpUnclamped(a, b, Math.Clamp(t, 0f, 1f));
	}

	public static Color32 LerpUnclamped(Color32 a, Color32 b, float t)
	{
		return new Color32(Mix(a.r, b.r, t), Mix(a.g, b.g, t), Mix(a.b, b.b, t), Mix(a.a, b.a, t));
		static byte Mix(byte x, byte y, float t)
		{
			return (byte)Math.Clamp((int)MathF.Round((float)(int)x + (float)(y - x) * t), 0, 255);
		}
	}

	internal static Waylith.Math.Color32 ToCore(Color32 value)
	{
		return new Waylith.Math.Color32(value.r, value.g, value.b, value.a);
	}

	internal static Color32 FromCore(Waylith.Math.Color32 value)
	{
		return new Color32(value.r, value.g, value.b, value.a);
	}
}
