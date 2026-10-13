using System;

namespace Waylith.Math;

public struct RectInt : IEquatable<RectInt>, IFormattable
{
	public int x { get; set; }

	public int y { get; set; }

	public int width { get; set; }

	public int height { get; set; }

	public int xMin
	{
		readonly get
		{
			return x;
		}
		set
		{
			int num = xMax;
			x = value;
			width = num - x;
		}
	}

	public int yMin
	{
		readonly get
		{
			return y;
		}
		set
		{
			int num = yMax;
			y = value;
			height = num - y;
		}
	}

	public readonly int xMax => x + width;

	public readonly int yMax => y + height;

	public readonly Vector2Int min => new Vector2Int(xMin, yMin);

	public readonly Vector2Int max => new Vector2Int(xMax, yMax);

	public RectInt(int x, int y, int width, int height)
	{
		this.x = x;
		this.y = y;
		this.width = width;
		this.height = height;
	}

	public RectInt(Vector2Int position, Vector2Int size)
		: this(position.x, position.y, size.x, size.y)
	{
	}

	public readonly bool Contains(Vector2Int position)
	{
		if (position.x >= xMin && position.x < xMax && position.y >= yMin)
		{
			return position.y < yMax;
		}
		return false;
	}

	public readonly bool Equals(RectInt other)
	{
		if (x == other.x && y == other.y && width == other.width)
		{
			return height == other.height;
		}
		return false;
	}

	public override readonly bool Equals(object obj)
	{
		if (obj is RectInt other)
		{
			return Equals(other);
		}
		return false;
	}

	public override readonly int GetHashCode()
	{
		return HashCode.Combine(x, y, width, height);
	}

	public override readonly string ToString()
	{
		return ToString(null, null);
	}

	public readonly string ToString(string format, IFormatProvider formatProvider)
	{
		return $"({x}, {y}, {width}, {height})";
	}
}
