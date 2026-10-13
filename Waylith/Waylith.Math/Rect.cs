using System;
using System.Numerics;

namespace Waylith.Math;

public struct Rect : IEquatable<Rect>, IFormattable
{
	private float _x;

	private float _y;

	private float _width;

	private float _height;

	public float x
	{
		get
		{
			return _x;
		}
		set
		{
			_x = value;
		}
	}

	public float y
	{
		get
		{
			return _y;
		}
		set
		{
			_y = value;
		}
	}

	public float width
	{
		get
		{
			return _width;
		}
		set
		{
			_width = value;
		}
	}

	public float height
	{
		get
		{
			return _height;
		}
		set
		{
			_height = value;
		}
	}

	public Vector2 position
	{
		get
		{
			return new Vector2(_x, _y);
		}
		set
		{
			_x = value.X;
			_y = value.Y;
		}
	}

	public Vector2 size
	{
		get
		{
			return new Vector2(_width, _height);
		}
		set
		{
			_width = value.X;
			_height = value.Y;
		}
	}

	public float xMin
	{
		get
		{
			return _x;
		}
		set
		{
			float num = xMax;
			_x = value;
			_width = num - _x;
		}
	}

	public float xMax
	{
		get
		{
			return _x + _width;
		}
		set
		{
			_width = value - _x;
		}
	}

	public float yMin
	{
		get
		{
			return _y;
		}
		set
		{
			float num = yMax;
			_y = value;
			_height = num - _y;
		}
	}

	public float yMax
	{
		get
		{
			return _y + _height;
		}
		set
		{
			_height = value - _y;
		}
	}

	public Vector2 min
	{
		get
		{
			return new Vector2(xMin, yMin);
		}
		set
		{
			xMin = value.X;
			yMin = value.Y;
		}
	}

	public Vector2 max
	{
		get
		{
			return new Vector2(xMax, yMax);
		}
		set
		{
			xMax = value.X;
			yMax = value.Y;
		}
	}

	public Vector2 center
	{
		get
		{
			return new Vector2(_x + _width * 0.5f, _y + _height * 0.5f);
		}
		set
		{
			_x = value.X - _width * 0.5f;
			_y = value.Y - _height * 0.5f;
		}
	}

	public Rect(float x, float y, float width, float height)
	{
		_x = x;
		_y = y;
		_width = width;
		_height = height;
	}

	public Rect(Vector2 position, Vector2 size)
		: this(position.X, position.Y, size.X, size.Y)
	{
	}

	public Rect(Rect source)
		: this(source.x, source.y, source.width, source.height)
	{
	}

	public bool Contains(Vector2 point)
	{
		if (point.X >= xMin && point.X < xMax && point.Y >= yMin)
		{
			return point.Y < yMax;
		}
		return false;
	}

	public bool Overlaps(Rect other)
	{
		if (other.xMax > xMin && other.xMin < xMax && other.yMax > yMin)
		{
			return other.yMin < yMax;
		}
		return false;
	}

	public bool Equals(Rect other)
	{
		if (_x.Equals(other._x) && _y.Equals(other._y) && _width.Equals(other._width))
		{
			return _height.Equals(other._height);
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj is Rect other)
		{
			return Equals(other);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(_x, _y, _width, _height);
	}

	public static bool operator ==(Rect lhs, Rect rhs)
	{
		return lhs.Equals(rhs);
	}

	public static bool operator !=(Rect lhs, Rect rhs)
	{
		return !lhs.Equals(rhs);
	}

	public static Rect MinMaxRect(float xmin, float ymin, float xmax, float ymax)
	{
		return new Rect(xmin, ymin, xmax - xmin, ymax - ymin);
	}

	public static Vector2 NormalizedToPoint(Rect rectangle, Vector2 normalizedRectCoordinates)
	{
		return new Vector2(rectangle.x + rectangle.width * normalizedRectCoordinates.X, rectangle.y + rectangle.height * normalizedRectCoordinates.Y);
	}

	public static Vector2 PointToNormalized(Rect rectangle, Vector2 point)
	{
		return new Vector2((rectangle.width != 0f) ? ((point.X - rectangle.x) / rectangle.width) : 0f, (rectangle.height != 0f) ? ((point.Y - rectangle.y) / rectangle.height) : 0f);
	}

	public string ToString(string format, IFormatProvider formatProvider)
	{
		return $"({_x.ToString(format, formatProvider)}, {_y.ToString(format, formatProvider)}, {_width.ToString(format, formatProvider)}, {_height.ToString(format, formatProvider)})";
	}

	public override string ToString()
	{
		return $"({_x}, {_y}, {_width}, {_height})";
	}
}
