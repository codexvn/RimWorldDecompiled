using System;

namespace UnityEngine;

public struct Rect : IEquatable<Rect>, IFormattable
{
	private float m_XMin;

	private float m_YMin;

	private float m_Width;

	private float m_Height;

	public float x
	{
		get
		{
			return m_XMin;
		}
		set
		{
			m_XMin = value;
		}
	}

	public float y
	{
		get
		{
			return m_YMin;
		}
		set
		{
			m_YMin = value;
		}
	}

	public float width
	{
		get
		{
			return m_Width;
		}
		set
		{
			m_Width = value;
		}
	}

	public float height
	{
		get
		{
			return m_Height;
		}
		set
		{
			m_Height = value;
		}
	}

	public Vector2 position
	{
		get
		{
			return new Vector2(m_XMin, m_YMin);
		}
		set
		{
			m_XMin = value.x;
			m_YMin = value.y;
		}
	}

	public Vector2 size
	{
		get
		{
			return new Vector2(m_Width, m_Height);
		}
		set
		{
			m_Width = value.x;
			m_Height = value.y;
		}
	}

	public float xMin
	{
		get
		{
			return m_XMin;
		}
		set
		{
			float num = xMax;
			m_XMin = value;
			m_Width = num - m_XMin;
		}
	}

	public float xMax
	{
		get
		{
			return m_XMin + m_Width;
		}
		set
		{
			m_Width = value - m_XMin;
		}
	}

	public float yMin
	{
		get
		{
			return m_YMin;
		}
		set
		{
			float num = yMax;
			m_YMin = value;
			m_Height = num - m_YMin;
		}
	}

	public float yMax
	{
		get
		{
			return m_YMin + m_Height;
		}
		set
		{
			m_Height = value - m_YMin;
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
			xMin = value.x;
			yMin = value.y;
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
			xMax = value.x;
			yMax = value.y;
		}
	}

	public Vector2 center
	{
		get
		{
			return new Vector2(m_XMin + m_Width * 0.5f, m_YMin + m_Height * 0.5f);
		}
		set
		{
			m_XMin = value.x - m_Width * 0.5f;
			m_YMin = value.y - m_Height * 0.5f;
		}
	}

	public static Rect zero => new Rect(0f, 0f, 0f, 0f);

	public Rect(float x, float y, float width, float height)
	{
		m_XMin = x;
		m_YMin = y;
		m_Width = width;
		m_Height = height;
	}

	public void Set(float x, float y, float width, float height)
	{
		m_XMin = x;
		m_YMin = y;
		m_Width = width;
		m_Height = height;
	}

	public Rect(Vector2 position, Vector2 size)
		: this(position.x, position.y, size.x, size.y)
	{
	}

	public Rect(Rect source)
		: this(source.x, source.y, source.width, source.height)
	{
	}

	public bool Contains(Vector2 point)
	{
		if (point.x >= xMin && point.x < xMax && point.y >= yMin)
		{
			return point.y < yMax;
		}
		return false;
	}

	public bool Contains(Vector3 point)
	{
		return Contains(new Vector2(point.x, point.y));
	}

	public bool Contains(Vector3 point, bool allowInverse)
	{
		if (!allowInverse)
		{
			return Contains(point);
		}
		bool num = (width < 0f && point.x <= xMin && point.x > xMax) || (width >= 0f && point.x >= xMin && point.x < xMax);
		bool flag = (height < 0f && point.y <= yMin && point.y > yMax) || (height >= 0f && point.y >= yMin && point.y < yMax);
		return num && flag;
	}

	public bool Overlaps(Rect other)
	{
		if (other.xMax > xMin && other.xMin < xMax && other.yMax > yMin)
		{
			return other.yMin < yMax;
		}
		return false;
	}

	public bool Overlaps(Rect other, bool allowInverse)
	{
		if (!allowInverse)
		{
			return Overlaps(other);
		}
		return OrderMinMax(this).Overlaps(OrderMinMax(other));
	}

	private static Rect OrderMinMax(Rect r)
	{
		if (r.m_Width < 0f)
		{
			r.m_XMin += r.m_Width;
			r.m_Width = 0f - r.m_Width;
		}
		if (r.m_Height < 0f)
		{
			r.m_YMin += r.m_Height;
			r.m_Height = 0f - r.m_Height;
		}
		return r;
	}

	public bool Equals(Rect other)
	{
		if (m_XMin.Equals(other.m_XMin) && m_YMin.Equals(other.m_YMin) && m_Width.Equals(other.m_Width))
		{
			return m_Height.Equals(other.m_Height);
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
		return HashCode.Combine(m_XMin, m_YMin, m_Width, m_Height);
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
		return new Vector2(rectangle.x + rectangle.width * normalizedRectCoordinates.x, rectangle.y + rectangle.height * normalizedRectCoordinates.y);
	}

	public static Vector2 PointToNormalized(Rect rectangle, Vector2 point)
	{
		return new Vector2((rectangle.width != 0f) ? ((point.x - rectangle.x) / rectangle.width) : 0f, (rectangle.height != 0f) ? ((point.y - rectangle.y) / rectangle.height) : 0f);
	}

	public string ToString(string format, IFormatProvider formatProvider)
	{
		return $"({m_XMin.ToString(format, formatProvider)}, {m_YMin.ToString(format, formatProvider)}, {m_Width.ToString(format, formatProvider)}, {m_Height.ToString(format, formatProvider)})";
	}

	public override string ToString()
	{
		return $"({m_XMin}, {m_YMin}, {m_Width}, {m_Height})";
	}
}
