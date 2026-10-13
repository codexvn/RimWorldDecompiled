using System;
using System.Collections;
using System.Collections.Generic;

namespace UnityEngine;

public struct RectInt : IEquatable<RectInt>, IFormattable
{
	public struct PositionEnumerator : IEnumerator<Vector2Int>, IEnumerator, IDisposable
	{
		private readonly Vector2Int _min;

		private readonly Vector2Int _max;

		private Vector2Int _current;

		public Vector2Int Current => _current;

		object IEnumerator.Current => Current;

		public PositionEnumerator(Vector2Int min, Vector2Int max)
		{
			_min = (_current = min);
			_max = max;
			Reset();
		}

		public PositionEnumerator GetEnumerator()
		{
			return this;
		}

		public bool MoveNext()
		{
			if (_current.y >= _max.y)
			{
				return false;
			}
			_current.x++;
			if (_current.x >= _max.x)
			{
				_current.x = _min.x;
				_current.y++;
				if (_current.y >= _max.y)
				{
					return false;
				}
			}
			return true;
		}

		public void Reset()
		{
			_current = _min;
			_current.x--;
		}

		public void Dispose()
		{
		}
	}

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

	public Vector2Int position
	{
		readonly get
		{
			return new Vector2Int(x, y);
		}
		set
		{
			x = value.x;
			y = value.y;
		}
	}

	public Vector2Int size
	{
		readonly get
		{
			return new Vector2Int(width, height);
		}
		set
		{
			width = value.x;
			height = value.y;
		}
	}

	public Vector2Int min
	{
		readonly get
		{
			return new Vector2Int(xMin, yMin);
		}
		set
		{
			xMin = value.x;
			yMin = value.y;
		}
	}

	public Vector2Int max
	{
		readonly get
		{
			return new Vector2Int(xMax, yMax);
		}
		set
		{
			width = value.x - x;
			height = value.y - y;
		}
	}

	public readonly Vector2 center => new Vector2((float)x + (float)width / 2f, (float)y + (float)height / 2f);

	public readonly PositionEnumerator allPositionsWithin => new PositionEnumerator(min, max);

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

	public void SetMinMax(Vector2Int minPosition, Vector2Int maxPosition)
	{
		x = minPosition.x;
		y = minPosition.y;
		width = maxPosition.x - minPosition.x;
		height = maxPosition.y - minPosition.y;
	}

	public readonly bool Contains(Vector2Int position)
	{
		if (position.x >= xMin && position.y >= yMin && position.x < xMax)
		{
			return position.y < yMax;
		}
		return false;
	}

	public readonly bool Overlaps(RectInt other)
	{
		if (other.xMin < xMax && other.xMax > xMin && other.yMin < yMax)
		{
			return other.yMax > yMin;
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
