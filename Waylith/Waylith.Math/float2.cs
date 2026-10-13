using System;
using System.Globalization;
using System.Numerics;

namespace Waylith.Math;

public struct float2 : IEquatable<float2>, IFormattable
{
	public float x;

	public float y;

	public static readonly float2 zero = new float2(0f, 0f);

	public float2(float x, float y)
	{
		this.x = x;
		this.y = y;
	}

	public bool Equals(float2 rhs)
	{
		if (x.Equals(rhs.x))
		{
			return y.Equals(rhs.y);
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj is float2 rhs)
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
		return $"({x.ToString(format, formatProvider)}, {y.ToString(format, formatProvider)})";
	}

	public override string ToString()
	{
		return ToString(null, CultureInfo.InvariantCulture);
	}

	public static implicit operator float2(Vector2 v)
	{
		return new float2(v.X, v.Y);
	}

	public static implicit operator Vector2(float2 v)
	{
		return new Vector2(v.x, v.y);
	}

	public static float2 operator +(float2 lhs, float2 rhs)
	{
		return new float2(lhs.x + rhs.x, lhs.y + rhs.y);
	}

	public static float2 operator -(float2 lhs, float2 rhs)
	{
		return new float2(lhs.x - rhs.x, lhs.y - rhs.y);
	}

	public static float2 operator *(float2 lhs, float rhs)
	{
		return new float2(lhs.x * rhs, lhs.y * rhs);
	}
}
