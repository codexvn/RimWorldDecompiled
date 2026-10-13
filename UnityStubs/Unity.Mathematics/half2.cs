using System;
using System.Globalization;

namespace Unity.Mathematics;

public struct half2 : IEquatable<half2>, IFormattable
{
	public half x;

	public half y;

	public bool Equals(half2 rhs)
	{
		if (x.Equals(rhs.x))
		{
			return y.Equals(rhs.y);
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj is half2 rhs)
		{
			return Equals(rhs);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(x, y);
	}

	public string ToString(string format, IFormatProvider formatProvider)
	{
		return $"({((float)x).ToString(format, formatProvider)}, {((float)y).ToString(format, formatProvider)})";
	}

	public override string ToString()
	{
		return ToString(null, CultureInfo.InvariantCulture);
	}

	public static explicit operator half2(float2 v)
	{
		half2 result = default(half2);
		result.x = (half)v.x;
		result.y = (half)v.y;
		return result;
	}
}
