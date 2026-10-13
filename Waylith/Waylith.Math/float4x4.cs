using System;
using System.Globalization;
using System.Numerics;

namespace Waylith.Math;

public struct float4x4 : IEquatable<float4x4>, IFormattable
{
	public float4 c0;

	public float4 c1;

	public float4 c2;

	public float4 c3;

	public bool Equals(float4x4 rhs)
	{
		if (c0.Equals(rhs.c0) && c1.Equals(rhs.c1) && c2.Equals(rhs.c2))
		{
			return c3.Equals(rhs.c3);
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj is float4x4 rhs)
		{
			return Equals(rhs);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(c0, c1, c2, c3);
	}

	public string ToString(string format, IFormatProvider formatProvider)
	{
		return $"float4x4({c0.ToString(format, formatProvider)}, {c1.ToString(format, formatProvider)}, {c2.ToString(format, formatProvider)}, {c3.ToString(format, formatProvider)})";
	}

	public override string ToString()
	{
		return ToString(null, CultureInfo.InvariantCulture);
	}

	public static implicit operator float4x4(Matrix4x4 m)
	{
		float4x4 result = default(float4x4);
		result.c0 = new float4(m.M11, m.M21, m.M31, m.M41);
		result.c1 = new float4(m.M12, m.M22, m.M32, m.M42);
		result.c2 = new float4(m.M13, m.M23, m.M33, m.M43);
		result.c3 = new float4(m.M14, m.M24, m.M34, m.M44);
		return result;
	}
}
