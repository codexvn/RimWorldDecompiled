using System;
using System.Globalization;

namespace Waylith.Math;

public struct int2 : IEquatable<int2>, IFormattable
{
	public int x;

	public int y;

	public static readonly int2 zero = new int2(0, 0);

	public int2(int x, int y)
	{
		this.x = x;
		this.y = y;
	}

	public bool Equals(int2 rhs)
	{
		if (x == rhs.x)
		{
			return y == rhs.y;
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj is int2 rhs)
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

	public static int2 operator +(int2 lhs, int2 rhs)
	{
		return new int2(lhs.x + rhs.x, lhs.y + rhs.y);
	}

	public static int2 operator +(int2 lhs, int rhs)
	{
		return new int2(lhs.x + rhs, lhs.y + rhs);
	}

	public static int2 operator /(int2 lhs, int rhs)
	{
		return new int2(lhs.x / rhs, lhs.y / rhs);
	}

	public static implicit operator int2(int v)
	{
		return new int2(v, v);
	}

	public static int2 operator *(int2 lhs, int rhs)
	{
		return new int2(lhs.x * rhs, lhs.y * rhs);
	}

	public static int2 operator *(int2 lhs, int2 rhs)
	{
		return new int2(lhs.x * rhs.x, lhs.y * rhs.y);
	}

	public static int2 operator -(int2 lhs, int2 rhs)
	{
		return new int2(lhs.x - rhs.x, lhs.y - rhs.y);
	}

	public static int2 operator -(int2 val)
	{
		return new int2(-val.x, -val.y);
	}
}
