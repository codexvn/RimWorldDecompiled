using System;

namespace Waylith.Math;

public struct bool3 : IEquatable<bool3>
{
	public bool x;

	public bool y;

	public bool z;

	public bool3(bool x, bool y, bool z)
	{
		this.x = x;
		this.y = y;
		this.z = z;
	}

	public bool Equals(bool3 rhs)
	{
		if (x == rhs.x && y == rhs.y)
		{
			return z == rhs.z;
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj is bool3 rhs)
		{
			return Equals(rhs);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(x, y, z);
	}

	public static bool3 operator &(bool3 lhs, bool3 rhs)
	{
		return new bool3(lhs.x & rhs.x, lhs.y & rhs.y, lhs.z & rhs.z);
	}

	public static bool3 operator |(bool3 lhs, bool3 rhs)
	{
		return new bool3(lhs.x | rhs.x, lhs.y | rhs.y, lhs.z | rhs.z);
	}
}
