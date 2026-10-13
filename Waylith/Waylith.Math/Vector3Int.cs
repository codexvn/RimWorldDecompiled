using System;
using System.Globalization;
using System.Numerics;

namespace Waylith.Math;

public struct Vector3Int : IEquatable<Vector3Int>, IFormattable
{
	public int x { get; set; }

	public int y { get; set; }

	public int z { get; set; }

	public static Vector3Int zero => new Vector3Int(0, 0, 0);

	public static Vector3Int one => new Vector3Int(1, 1, 1);

	public Vector3Int(int x, int y, int z)
	{
		this.x = x;
		this.y = y;
		this.z = z;
	}

	public bool Equals(Vector3Int other)
	{
		if (x == other.x && y == other.y)
		{
			return z == other.z;
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj is Vector3Int other)
		{
			return Equals(other);
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

	public static bool operator ==(Vector3Int lhs, Vector3Int rhs)
	{
		return lhs.Equals(rhs);
	}

	public static bool operator !=(Vector3Int lhs, Vector3Int rhs)
	{
		return !lhs.Equals(rhs);
	}

	public static Vector3Int operator +(Vector3Int lhs, Vector3Int rhs)
	{
		return new Vector3Int(lhs.x + rhs.x, lhs.y + rhs.y, lhs.z + rhs.z);
	}

	public static Vector3Int operator -(Vector3Int lhs, Vector3Int rhs)
	{
		return new Vector3Int(lhs.x - rhs.x, lhs.y - rhs.y, lhs.z - rhs.z);
	}

	public static Vector3Int operator -(Vector3Int value)
	{
		return new Vector3Int(-value.x, -value.y, -value.z);
	}

	public static Vector3Int operator *(Vector3Int lhs, int rhs)
	{
		return new Vector3Int(lhs.x * rhs, lhs.y * rhs, lhs.z * rhs);
	}

	public static Vector3Int operator *(int lhs, Vector3Int rhs)
	{
		return rhs * lhs;
	}

	public static Vector3Int operator /(Vector3Int lhs, int rhs)
	{
		return new Vector3Int(lhs.x / rhs, lhs.y / rhs, lhs.z / rhs);
	}

	public static implicit operator Vector3(Vector3Int value)
	{
		return new Vector3(value.x, value.y, value.z);
	}
}
