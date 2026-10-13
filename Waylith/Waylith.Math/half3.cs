using System;
using System.Globalization;

namespace Waylith.Math;

public struct half3 : IEquatable<half3>, IFormattable
{
	public half x;

	public half y;

	public half z;

	public bool Equals(half3 rhs)
	{
		if (x.Equals(rhs.x) && y.Equals(rhs.y))
		{
			return z.Equals(rhs.z);
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj is half3 rhs)
		{
			return Equals(rhs);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(x, y, z);
	}

	public string ToString(string format, IFormatProvider formatProvider)
	{
		return $"({((float)x).ToString(format, formatProvider)}, {((float)y).ToString(format, formatProvider)}, {((float)z).ToString(format, formatProvider)})";
	}

	public override string ToString()
	{
		return ToString(null, CultureInfo.InvariantCulture);
	}

	public static explicit operator half3(float3 v)
	{
		half3 result = default(half3);
		result.x = (half)v.x;
		result.y = (half)v.y;
		result.z = (half)v.z;
		return result;
	}
}
