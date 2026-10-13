using System;
using System.Globalization;
using UnityEngine;

namespace Unity.Mathematics;

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
		result.c0 = new float4(m.m00, m.m10, m.m20, m.m30);
		result.c1 = new float4(m.m01, m.m11, m.m21, m.m31);
		result.c2 = new float4(m.m02, m.m12, m.m22, m.m32);
		result.c3 = new float4(m.m03, m.m13, m.m23, m.m33);
		return result;
	}
}
