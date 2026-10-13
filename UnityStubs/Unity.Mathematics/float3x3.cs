using System;
using System.Globalization;

namespace Unity.Mathematics;

public struct float3x3 : IEquatable<float3x3>, IFormattable
{
	public float3 c0;

	public float3 c1;

	public float3 c2;

	public float3x3(float3 c0, float3 c1, float3 c2)
	{
		this.c0 = c0;
		this.c1 = c1;
		this.c2 = c2;
	}

	public bool Equals(float3x3 rhs)
	{
		if (c0.Equals(rhs.c0) && c1.Equals(rhs.c1))
		{
			return c2.Equals(rhs.c2);
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj is float3x3 rhs)
		{
			return Equals(rhs);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(c0, c1, c2);
	}

	public string ToString(string format, IFormatProvider formatProvider)
	{
		return $"float3x3({c0.ToString(format, formatProvider)}, {c1.ToString(format, formatProvider)}, {c2.ToString(format, formatProvider)})";
	}

	public override string ToString()
	{
		return ToString(null, CultureInfo.InvariantCulture);
	}

	public static float3x3 EulerXYZ(float3 xyz)
	{
		math.sincos(xyz.x, out var s, out var c);
		math.sincos(xyz.y, out var s2, out var c2);
		math.sincos(xyz.z, out var s3, out var c3);
		return new float3x3(new float3(c2 * c3, c2 * s3, 0f - s2), new float3(c3 * s * s2 - c * s3, c * c3 + s * s2 * s3, c2 * s), new float3(s * s3 + c * c3 * s2, c * s2 * s3 - c3 * s, c * c2));
	}
}
