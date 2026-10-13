using System;

namespace Unity.Mathematics;

public struct int4 : IEquatable<int4>, IFormattable
{
	public int x;

	public int y;

	public int z;

	public int w;

	public int this[int index]
	{
		get
		{
			return index switch
			{
				0 => x, 
				1 => y, 
				2 => z, 
				3 => w, 
				_ => throw new IndexOutOfRangeException($"Index {index} out of range for int4"), 
			};
		}
		set
		{
			switch (index)
			{
			case 0:
				x = value;
				return;
			case 1:
				y = value;
				return;
			case 2:
				z = value;
				return;
			case 3:
				w = value;
				return;
			}
			throw new IndexOutOfRangeException($"Index {index} out of range for int4");
		}
	}

	public int4(int x, int y, int z, int w)
	{
		this.x = x;
		this.y = y;
		this.z = z;
		this.w = w;
	}

	public int4(int v)
		: this(v, v, v, v)
	{
	}

	public bool Equals(int4 rhs)
	{
		if (x == rhs.x && y == rhs.y && z == rhs.z)
		{
			return w == rhs.w;
		}
		return false;
	}

	public override bool Equals(object o)
	{
		if (o is int4 rhs)
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
		return $"int4({x.ToString(format, formatProvider)}, {y.ToString(format, formatProvider)}, {z.ToString(format, formatProvider)}, {w.ToString(format, formatProvider)})";
	}

	public override string ToString()
	{
		return $"int4({x}, {y}, {z}, {w})";
	}

	public static implicit operator int4(int v)
	{
		return new int4(v);
	}

	public static int4 operator *(int4 lhs, int4 rhs)
	{
		return new int4(lhs.x * rhs.x, lhs.y * rhs.y, lhs.z * rhs.z, lhs.w * rhs.w);
	}

	public static int4 operator *(int4 lhs, int rhs)
	{
		return new int4(lhs.x * rhs, lhs.y * rhs, lhs.z * rhs, lhs.w * rhs);
	}

	public static int4 operator *(int lhs, int4 rhs)
	{
		return new int4(lhs * rhs.x, lhs * rhs.y, lhs * rhs.z, lhs * rhs.w);
	}

	public static int4 operator +(int4 lhs, int4 rhs)
	{
		return new int4(lhs.x + rhs.x, lhs.y + rhs.y, lhs.z + rhs.z, lhs.w + rhs.w);
	}

	public static int4 operator +(int4 lhs, int rhs)
	{
		return new int4(lhs.x + rhs, lhs.y + rhs, lhs.z + rhs, lhs.w + rhs);
	}

	public static int4 operator +(int lhs, int4 rhs)
	{
		return new int4(lhs + rhs.x, lhs + rhs.y, lhs + rhs.z, lhs + rhs.w);
	}

	public static int4 operator -(int4 lhs, int4 rhs)
	{
		return new int4(lhs.x - rhs.x, lhs.y - rhs.y, lhs.z - rhs.z, lhs.w - rhs.w);
	}

	public static int4 operator -(int4 lhs, int rhs)
	{
		return new int4(lhs.x - rhs, lhs.y - rhs, lhs.z - rhs, lhs.w - rhs);
	}

	public static int4 operator -(int lhs, int4 rhs)
	{
		return new int4(lhs - rhs.x, lhs - rhs.y, lhs - rhs.z, lhs - rhs.w);
	}

	public static int4 operator /(int4 lhs, int4 rhs)
	{
		return new int4(lhs.x / rhs.x, lhs.y / rhs.y, lhs.z / rhs.z, lhs.w / rhs.w);
	}

	public static int4 operator /(int4 lhs, int rhs)
	{
		return new int4(lhs.x / rhs, lhs.y / rhs, lhs.z / rhs, lhs.w / rhs);
	}

	public static int4 operator /(int lhs, int4 rhs)
	{
		return new int4(lhs / rhs.x, lhs / rhs.y, lhs / rhs.z, lhs / rhs.w);
	}

	public static int4 operator -(int4 val)
	{
		return new int4(-val.x, -val.y, -val.z, -val.w);
	}

	public static int4 operator &(int4 lhs, int4 rhs)
	{
		return new int4(lhs.x & rhs.x, lhs.y & rhs.y, lhs.z & rhs.z, lhs.w & rhs.w);
	}

	public static int4 operator &(int4 lhs, int rhs)
	{
		return new int4(lhs.x & rhs, lhs.y & rhs, lhs.z & rhs, lhs.w & rhs);
	}

	public static int4 operator &(int lhs, int4 rhs)
	{
		return new int4(lhs & rhs.x, lhs & rhs.y, lhs & rhs.z, lhs & rhs.w);
	}

	public static bool4 operator ==(int4 lhs, int4 rhs)
	{
		return new bool4(lhs.x == rhs.x, lhs.y == rhs.y, lhs.z == rhs.z, lhs.w == rhs.w);
	}

	public static bool4 operator ==(int4 lhs, int rhs)
	{
		return new bool4(lhs.x == rhs, lhs.y == rhs, lhs.z == rhs, lhs.w == rhs);
	}

	public static bool4 operator ==(int lhs, int4 rhs)
	{
		return new bool4(lhs == rhs.x, lhs == rhs.y, lhs == rhs.z, lhs == rhs.w);
	}

	public static bool4 operator !=(int4 lhs, int4 rhs)
	{
		return new bool4(lhs.x != rhs.x, lhs.y != rhs.y, lhs.z != rhs.z, lhs.w != rhs.w);
	}

	public static bool4 operator !=(int4 lhs, int rhs)
	{
		return new bool4(lhs.x != rhs, lhs.y != rhs, lhs.z != rhs, lhs.w != rhs);
	}

	public static bool4 operator !=(int lhs, int4 rhs)
	{
		return new bool4(lhs != rhs.x, lhs != rhs.y, lhs != rhs.z, lhs != rhs.w);
	}
}
