using System;

namespace Unity.Mathematics;

public struct bool4 : IEquatable<bool4>
{
	public bool x;

	public bool y;

	public bool z;

	public bool w;

	public bool this[int index]
	{
		get
		{
			return index switch
			{
				0 => x, 
				1 => y, 
				2 => z, 
				3 => w, 
				_ => throw new IndexOutOfRangeException($"Index {index} out of range for bool4"), 
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
			throw new IndexOutOfRangeException($"Index {index} out of range for bool4");
		}
	}

	public bool4(bool x, bool y, bool z, bool w)
	{
		this.x = x;
		this.y = y;
		this.z = z;
		this.w = w;
	}

	public bool4(bool v)
		: this(v, v, v, v)
	{
	}

	public bool Equals(bool4 rhs)
	{
		if (x == rhs.x && y == rhs.y && z == rhs.z)
		{
			return w == rhs.w;
		}
		return false;
	}

	public override bool Equals(object o)
	{
		if (o is bool4 rhs)
		{
			return Equals(rhs);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(x, y, z, w);
	}

	public override string ToString()
	{
		return $"bool4({x}, {y}, {z}, {w})";
	}

	public static implicit operator bool4(bool v)
	{
		return new bool4(v);
	}

	public static bool4 operator ==(bool4 lhs, bool4 rhs)
	{
		return new bool4(lhs.x == rhs.x, lhs.y == rhs.y, lhs.z == rhs.z, lhs.w == rhs.w);
	}

	public static bool4 operator !=(bool4 lhs, bool4 rhs)
	{
		return new bool4(lhs.x != rhs.x, lhs.y != rhs.y, lhs.z != rhs.z, lhs.w != rhs.w);
	}

	public static bool4 operator !(bool4 val)
	{
		return new bool4(!val.x, !val.y, !val.z, !val.w);
	}

	public static bool4 operator &(bool4 lhs, bool4 rhs)
	{
		return new bool4(lhs.x & rhs.x, lhs.y & rhs.y, lhs.z & rhs.z, lhs.w & rhs.w);
	}

	public static bool4 operator |(bool4 lhs, bool4 rhs)
	{
		return new bool4(lhs.x | rhs.x, lhs.y | rhs.y, lhs.z | rhs.z, lhs.w | rhs.w);
	}

	public static bool4 operator ^(bool4 lhs, bool4 rhs)
	{
		return new bool4(lhs.x ^ rhs.x, lhs.y ^ rhs.y, lhs.z ^ rhs.z, lhs.w ^ rhs.w);
	}
}
