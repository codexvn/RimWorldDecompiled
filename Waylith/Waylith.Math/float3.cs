using System;
using System.Globalization;
using System.Numerics;

namespace Waylith.Math;

public struct float3 : IEquatable<float3>, IFormattable
{
	public float x;

	public float y;

	public float z;

	public static readonly float3 zero = new float3(0f, 0f, 0f);

	public float2 xy => new float2(x, y);

	public float3(float x, float y, float z)
	{
		this.x = x;
		this.y = y;
		this.z = z;
	}

	public float3(int v)
		: this(v, v, v)
	{
	}

	public float3(float2 xy, float z)
		: this(xy.x, xy.y, z)
	{
	}

	public float3(float v)
		: this(v, v, v)
	{
	}

	public bool Equals(float3 rhs)
	{
		if (x.Equals(rhs.x) && y.Equals(rhs.y))
		{
			return z.Equals(rhs.z);
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj is float3 rhs)
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
		return $"({x.ToString(format, formatProvider)}, {y.ToString(format, formatProvider)}, {z.ToString(format, formatProvider)})";
	}

	public override string ToString()
	{
		return ToString(null, CultureInfo.InvariantCulture);
	}

	public static float3 operator +(float3 lhs, float3 rhs)
	{
		return new float3(lhs.x + rhs.x, lhs.y + rhs.y, lhs.z + rhs.z);
	}

	public static float3 operator /(float3 lhs, float rhs)
	{
		return new float3(lhs.x / rhs, lhs.y / rhs, lhs.z / rhs);
	}

	public static implicit operator float3(Vector3 v)
	{
		return new float3(v.X, v.Y, v.Z);
	}

	public static implicit operator Vector3(float3 v)
	{
		return new Vector3(v.x, v.y, v.z);
	}

	public static implicit operator float3(float v)
	{
		return new float3(v);
	}

	public static float3 operator *(float3 lhs, float3 rhs)
	{
		return new float3(lhs.x * rhs.x, lhs.y * rhs.y, lhs.z * rhs.z);
	}

	public static float3 operator *(float3 lhs, float rhs)
	{
		return new float3(lhs.x * rhs, lhs.y * rhs, lhs.z * rhs);
	}

	public static float3 operator *(float lhs, float3 rhs)
	{
		return rhs * lhs;
	}

	public static float3 operator -(float3 lhs, float3 rhs)
	{
		return new float3(lhs.x - rhs.x, lhs.y - rhs.y, lhs.z - rhs.z);
	}

	public static float3 operator -(float3 val)
	{
		return new float3(0f - val.x, 0f - val.y, 0f - val.z);
	}
}
