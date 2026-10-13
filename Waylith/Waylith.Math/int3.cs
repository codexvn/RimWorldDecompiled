using System;
using System.Globalization;

namespace Waylith.Math;

public struct int3 : IEquatable<int3>, IFormattable
{
	public int x;

	public int y;

	public int z;

	public static readonly int3 zero = new int3(0, 0, 0);

	public int2 xz => new int2(x, z);

	public int2 zx => new int2(z, x);

	public int3 zyx => new int3(z, y, x);

	public int this[int index]
	{
		get
		{
			return index switch
			{
				0 => x, 
				1 => y, 
				2 => z, 
				_ => throw new IndexOutOfRangeException(), 
			};
		}
		set
		{
			switch (index)
			{
			case 0:
				x = value;
				break;
			case 1:
				y = value;
				break;
			case 2:
				z = value;
				break;
			default:
				throw new IndexOutOfRangeException();
			}
		}
	}

	public int3(int x, int y, int z)
	{
		this.x = x;
		this.y = y;
		this.z = z;
	}

	public int3(int v)
		: this(v, v, v)
	{
	}

	public bool Equals(int3 rhs)
	{
		if (x == rhs.x && y == rhs.y)
		{
			return z == rhs.z;
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj is int3 rhs)
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

	public static bool3 operator >(int3 lhs, int rhs)
	{
		return new bool3(lhs.x > rhs, lhs.y > rhs, lhs.z > rhs);
	}

	public static bool3 operator >(int3 lhs, int3 rhs)
	{
		return new bool3(lhs.x > rhs.x, lhs.y > rhs.y, lhs.z > rhs.z);
	}

	public static bool3 operator >=(int3 lhs, int rhs)
	{
		return new bool3(lhs.x >= rhs, lhs.y >= rhs, lhs.z >= rhs);
	}

	public static bool3 operator !=(int3 lhs, int3 rhs)
	{
		return new bool3(lhs.x != rhs.x, lhs.y != rhs.y, lhs.z != rhs.z);
	}

	public static bool3 operator <=(int3 lhs, int3 rhs)
	{
		return new bool3(lhs.x <= rhs.x, lhs.y <= rhs.y, lhs.z <= rhs.z);
	}

	public static int3 operator +(int3 lhs, int3 rhs)
	{
		return new int3(lhs.x + rhs.x, lhs.y + rhs.y, lhs.z + rhs.z);
	}

	public static int3 operator /(int3 lhs, int rhs)
	{
		return new int3(lhs.x / rhs, lhs.y / rhs, lhs.z / rhs);
	}

	public static bool3 operator ==(int3 lhs, int3 rhs)
	{
		return new bool3(lhs.x == rhs.x, lhs.y == rhs.y, lhs.z == rhs.z);
	}

	public static explicit operator int3(float3 v)
	{
		return new int3((int)v.x, (int)v.y, (int)v.z);
	}

	public static bool3 operator >=(int3 lhs, int3 rhs)
	{
		return new bool3(lhs.x >= rhs.x, lhs.y >= rhs.y, lhs.z >= rhs.z);
	}

	public static implicit operator int3(int v)
	{
		return new int3(v);
	}

	public static int3 operator ++(int3 val)
	{
		return new int3(val.x + 1, val.y + 1, val.z + 1);
	}

	public static bool3 operator <(int3 lhs, int rhs)
	{
		return new bool3(lhs.x < rhs, lhs.y < rhs, lhs.z < rhs);
	}

	public static bool3 operator <(int3 lhs, int3 rhs)
	{
		return new bool3(lhs.x < rhs.x, lhs.y < rhs.y, lhs.z < rhs.z);
	}

	public static bool3 operator <=(int3 lhs, int rhs)
	{
		return new bool3(lhs.x <= rhs, lhs.y <= rhs, lhs.z <= rhs);
	}

	public static int3 operator *(int3 lhs, int rhs)
	{
		return new int3(lhs.x * rhs, lhs.y * rhs, lhs.z * rhs);
	}

	public static int3 operator -(int3 lhs, int3 rhs)
	{
		return new int3(lhs.x - rhs.x, lhs.y - rhs.y, lhs.z - rhs.z);
	}

	public static int3 operator -(int3 lhs, int rhs)
	{
		return new int3(lhs.x - rhs, lhs.y - rhs, lhs.z - rhs);
	}

	public static int3 operator -(int3 val)
	{
		return new int3(-val.x, -val.y, -val.z);
	}
}
