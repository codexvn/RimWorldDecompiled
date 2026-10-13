using System;
using System.Globalization;

namespace Unity.Mathematics;

public struct half4 : IEquatable<half4>, IFormattable
{
	public half x;

	public half y;

	public half z;

	public half w;

	public bool Equals(half4 rhs)
	{
		if (x.Equals(rhs.x) && y.Equals(rhs.y) && z.Equals(rhs.z))
		{
			return w.Equals(rhs.w);
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj is half4 rhs)
		{
			return Equals(rhs);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(x, y, z, w);
	}

	public string ToString(string format, IFormatProvider formatProvider)
	{
		return $"({((float)x).ToString(format, formatProvider)}, {((float)y).ToString(format, formatProvider)}, {((float)z).ToString(format, formatProvider)}, {((float)w).ToString(format, formatProvider)})";
	}

	public override string ToString()
	{
		return ToString(null, CultureInfo.InvariantCulture);
	}

	public static explicit operator half4(float4 v)
	{
		half4 result = default(half4);
		result.x = (half)v.x;
		result.y = (half)v.y;
		result.z = (half)v.z;
		result.w = (half)v.w;
		return result;
	}
}
