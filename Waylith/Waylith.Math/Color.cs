using System;
using System.Globalization;
using System.Numerics;

namespace Waylith.Math;

public struct Color : IEquatable<Color>, IFormattable
{
	private Vector4 _value;

	public float r
	{
		readonly get
		{
			return _value.X;
		}
		set
		{
			_value.X = value;
		}
	}

	public float g
	{
		readonly get
		{
			return _value.Y;
		}
		set
		{
			_value.Y = value;
		}
	}

	public float b
	{
		readonly get
		{
			return _value.Z;
		}
		set
		{
			_value.Z = value;
		}
	}

	public float a
	{
		readonly get
		{
			return _value.W;
		}
		set
		{
			_value.W = value;
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

	public Color(float r, float g, float b, float a)
	{
		this = default(Color);
		_value = new Vector4(r, g, b, a);
	}

	public Color(float r, float g, float b)
		: this(r, g, b, 1f)
	{
	}

	public bool Equals(Color other)
	{
		return _value.Equals(other._value);
	}

	public override bool Equals(object obj)
	{
		if (obj is Color other)
		{
			return Equals(other);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return _value.GetHashCode();
	}

	public string ToString(string format, IFormatProvider formatProvider)
	{
		return $"RGBA({r.ToString(format, formatProvider)}, {g.ToString(format, formatProvider)}, {b.ToString(format, formatProvider)}, {a.ToString(format, formatProvider)})";
	}

	public override string ToString()
	{
		return ToString("F2", CultureInfo.InvariantCulture);
	}

	public static Color HSVToRGB(float H, float S, float V)
	{
		return HSVToRGB(H, S, V, hdr: false);
	}

	public static Color HSVToRGB(float h, float s, float v, bool hdr)
	{
		double num = System.Math.Floor(h * 6f);
		double num2 = (double)(h * 6f) - num;
		double num3 = (double)v * (1.0 - (double)s);
		double num4 = (double)v * (1.0 - num2 * (double)s);
		double num5 = (double)v * (1.0 - (1.0 - num2) * (double)s);
		double num6 = num % 6.0;
		double num7;
		double num8;
		double num9;
		if (num6 != 0.0)
		{
			if (num6 != 1.0)
			{
				if (num6 != 2.0)
				{
					if (num6 != 3.0)
					{
						if (num6 != 4.0)
						{
							if (num6 == 5.0)
							{
								num7 = v;
								num8 = num3;
								num9 = num4;
							}
							else
							{
								num7 = (num8 = (num9 = 0.0));
							}
						}
						else
						{
							num7 = num5;
							num8 = num3;
							num9 = v;
						}
					}
					else
					{
						num7 = num3;
						num8 = num4;
						num9 = v;
					}
				}
				else
				{
					num7 = num3;
					num8 = v;
					num9 = num5;
				}
			}
			else
			{
				num7 = num4;
				num8 = v;
				num9 = num3;
			}
		}
		else
		{
			num7 = v;
			num8 = num5;
			num9 = num3;
		}
		return new Color((float)num7, (float)num8, (float)num9, 1f);
	}

	public static Color Lerp(Color a, Color b, float t)
	{
		return FromNumerics(Vector4.Lerp(a._value, b._value, System.Math.Clamp(t, 0f, 1f)));
	}

	public static Color operator +(Color a, Color b)
	{
		return FromNumerics(a._value + b._value);
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
		return c._value;
	}

	public static implicit operator Color(Vector4 v)
	{
		return FromNumerics(v);
	}

	public static Color operator *(Color a, Color b)
	{
		return FromNumerics(a._value * b._value);
	}

	public static Color operator *(Color a, float b)
	{
		return FromNumerics(a._value * b);
	}

	public static Color operator -(Color a, Color b)
	{
		return FromNumerics(a._value - b._value);
	}

	public static void RGBToHSV(Color rgbColor, out float H, out float S, out float V)
	{
		float num = MathF.Max(rgbColor.r, MathF.Max(rgbColor.g, rgbColor.b));
		float num2 = MathF.Min(rgbColor.r, MathF.Min(rgbColor.g, rgbColor.b));
		float num3 = num - num2;
		V = num;
		S = ((num > 0f) ? (num3 / num) : 0f);
		if (num3 <= 0f)
		{
			H = 0f;
			return;
		}
		float num4 = ((num == rgbColor.r) ? ((rgbColor.g - rgbColor.b) / num3) : ((num != rgbColor.g) ? (4f + (rgbColor.r - rgbColor.g) / num3) : (2f + (rgbColor.b - rgbColor.r) / num3)));
		num4 /= 6f;
		H = ((num4 < 0f) ? (num4 + 1f) : num4);
	}

	internal static Vector4 ToNumerics(Color value)
	{
		return value._value;
	}

	internal static Color FromNumerics(Vector4 value)
	{
		Color result = default(Color);
		result._value = value;
		return result;
	}
}
