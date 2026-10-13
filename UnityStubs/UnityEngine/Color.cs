using System;
using System.Globalization;
using System.Numerics;
using Waylith.Math;

namespace UnityEngine;

public struct Color : IEquatable<Color>, IFormattable
{
	public float r;

	public float g;

	public float b;

	public float a;

	public float this[int index]
	{
		readonly get
		{
			return index switch
			{
				0 => r, 
				1 => g, 
				2 => b, 
				_ => a, 
			};
		}
		set
		{
			switch (index)
			{
			case 0:
				r = value;
				break;
			case 1:
				g = value;
				break;
			case 2:
				b = value;
				break;
			default:
				a = value;
				break;
			}
		}
	}

	public static Color black => new Color(0f, 0f, 0f, 1f);

	public static Color blue => new Color(0f, 0f, 1f, 1f);

	public static Color clear => new Color(0f, 0f, 0f, 0f);

	public static Color cyan => new Color(0f, 1f, 1f, 1f);

	public static Color gray => new Color(0.5f, 0.5f, 0.5f, 1f);

	public static Color green => new Color(0f, 1f, 0f, 1f);

	public static Color limeGreen => new Color(0.557f, 1f, 0.098f, 1f);

	public static Color grey => gray;

	public static Color indianRed => new Color(0.804f, 0.361f, 0.361f, 1f);

	public static Color magenta => new Color(1f, 0f, 1f, 1f);

	public static Color red => new Color(1f, 0f, 0f, 1f);

	public static Color white => new Color(1f, 1f, 1f, 1f);

	public static Color yellow => new Color(1f, 0.922f, 0.016f, 1f);

	public readonly float grayscale => 0.299f * r + 0.587f * g + 0.114f * b;

	public readonly Color linear => new Color(Mathf.GammaToLinearSpace(r), Mathf.GammaToLinearSpace(g), Mathf.GammaToLinearSpace(b), a);

	public readonly Color gamma => new Color(Mathf.LinearToGammaSpace(r), Mathf.LinearToGammaSpace(g), Mathf.LinearToGammaSpace(b), a);

	public readonly float maxColorComponent => Math.Max(Math.Max(r, g), b);

	public Color(float r, float g, float b, float a)
	{
		this.r = r;
		this.g = g;
		this.b = b;
		this.a = a;
	}

	public Color(float r, float g, float b)
		: this(r, g, b, 1f)
	{
	}

	public readonly bool Equals(Color other)
	{
		if (r.Equals(other.r) && g.Equals(other.g) && b.Equals(other.b))
		{
			return a.Equals(other.a);
		}
		return false;
	}

	public override readonly bool Equals(object obj)
	{
		if (obj is Color other)
		{
			return Equals(other);
		}
		return false;
	}

	public override readonly int GetHashCode()
	{
		return HashCode.Combine(r, g, b, a);
	}

	public readonly string ToString(string format, IFormatProvider formatProvider)
	{
		if (string.IsNullOrEmpty(format))
		{
			format = "F3";
		}
		return $"RGBA({r.ToString(format, formatProvider)}, {g.ToString(format, formatProvider)}, {b.ToString(format, formatProvider)}, {a.ToString(format, formatProvider)})";
	}

	public override readonly string ToString()
	{
		return ToString(null, CultureInfo.InvariantCulture);
	}

	public static Color HSVToRGB(float H, float S, float V)
	{
		return HSVToRGB(H, S, V, hdr: false);
	}

	public static Color HSVToRGB(float h, float s, float v, bool hdr)
	{
		return FromCore(Waylith.Math.Color.HSVToRGB(h, s, v, hdr));
	}

	public static Color Lerp(Color a, Color b, float t)
	{
		return LerpUnclamped(a, b, Math.Clamp(t, 0f, 1f));
	}

	public static Color LerpUnclamped(Color a, Color b, float t)
	{
		return new Color(a.r + (b.r - a.r) * t, a.g + (b.g - a.g) * t, a.b + (b.b - a.b) * t, a.a + (b.a - a.a) * t);
	}

	public readonly string ToString(string format)
	{
		return ToString(format, CultureInfo.InvariantCulture);
	}

	public static Color operator /(Color a, float b)
	{
		return new Color(a.r / b, a.g / b, a.b / b, a.a / b);
	}

	public static Color operator +(Color a, Color b)
	{
		return new Color(a.r + b.r, a.g + b.g, a.b + b.b, a.a + b.a);
	}

	public static bool operator ==(Color lhs, Color rhs)
	{
		return lhs.Equals(rhs);
	}

	public static bool operator !=(Color lhs, Color rhs)
	{
		return !lhs.Equals(rhs);
	}

	public static implicit operator Vector4(Color c)
	{
		return new Vector4(c.r, c.g, c.b, c.a);
	}

	public static implicit operator Color(Vector4 v)
	{
		return new Color(v.x, v.y, v.z, v.w);
	}

	public static Color operator *(Color a, Color b)
	{
		return new Color(a.r * b.r, a.g * b.g, a.b * b.b, a.a * b.a);
	}

	public static Color operator *(Color a, float b)
	{
		return new Color(a.r * b, a.g * b, a.b * b, a.a * b);
	}

	public static Color operator *(float b, Color a)
	{
		return new Color(a.r * b, a.g * b, a.b * b, a.a * b);
	}

	public static Color operator -(Color a, Color b)
	{
		return new Color(a.r - b.r, a.g - b.g, a.b - b.b, a.a - b.a);
	}

	public static void RGBToHSV(Color rgbColor, out float H, out float S, out float V)
	{
		Waylith.Math.Color.RGBToHSV(ToCore(rgbColor), out H, out S, out V);
	}

	internal static System.Numerics.Vector4 ToNumerics(Color value)
	{
		return new System.Numerics.Vector4(value.r, value.g, value.b, value.a);
	}

	internal static Color FromNumerics(System.Numerics.Vector4 value)
	{
		return new Color(value.X, value.Y, value.Z, value.W);
	}

	internal static Waylith.Math.Color ToCore(Color value)
	{
		return new Waylith.Math.Color(value.r, value.g, value.b, value.a);
	}

	internal static Color FromCore(Waylith.Math.Color value)
	{
		return new Color(value.r, value.g, value.b, value.a);
	}
}
