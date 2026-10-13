using System;
using System.Globalization;
using System.Numerics;

namespace Waylith.Math;

public struct Vector2Int : IEquatable<Vector2Int>, IFormattable
{
	public int x { get; set; }

	public int y { get; set; }

	public static Vector2Int zero => new Vector2Int(0, 0);

	public static Vector2Int one => new Vector2Int(1, 1);

	public Vector2Int(int x, int y)
	{
		this.x = x;
		this.y = y;
	}

	public static Vector2Int Min(Vector2Int lhs, Vector2Int rhs)
	{
		return new Vector2Int(System.Math.Min(lhs.x, rhs.x), System.Math.Min(lhs.y, rhs.y));
	}

	public static Vector2Int Max(Vector2Int lhs, Vector2Int rhs)
	{
		return new Vector2Int(System.Math.Max(lhs.x, rhs.x), System.Math.Max(lhs.y, rhs.y));
	}

	public static Vector2Int RoundToInt(Vector2 v)
	{
		return new Vector2Int((int)System.Math.Round(v.X), (int)System.Math.Round(v.Y));
	}

	public bool Equals(Vector2Int other)
	{
		if (x == other.x)
		{
			return y == other.y;
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj is Vector2Int other)
		{
			return Equals(other);
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

	public static bool operator ==(Vector2Int lhs, Vector2Int rhs)
	{
		return lhs.Equals(rhs);
	}

	public static bool operator !=(Vector2Int lhs, Vector2Int rhs)
	{
		return !lhs.Equals(rhs);
	}

	public static Vector2Int operator +(Vector2Int lhs, Vector2Int rhs)
	{
		return new Vector2Int(lhs.x + rhs.x, lhs.y + rhs.y);
	}

	public static Vector2Int operator -(Vector2Int lhs, Vector2Int rhs)
	{
		return new Vector2Int(lhs.x - rhs.x, lhs.y - rhs.y);
	}

	public static Vector2Int operator -(Vector2Int value)
	{
		return new Vector2Int(-value.x, -value.y);
	}

	public static Vector2Int operator *(Vector2Int lhs, int rhs)
	{
		return new Vector2Int(lhs.x * rhs, lhs.y * rhs);
	}

	public static Vector2Int operator *(int lhs, Vector2Int rhs)
	{
		return rhs * lhs;
	}

	public static Vector2Int operator /(Vector2Int lhs, int rhs)
	{
		return new Vector2Int(lhs.x / rhs, lhs.y / rhs);
	}

	public static implicit operator Vector2(Vector2Int value)
	{
		return new Vector2(value.x, value.y);
	}
}
