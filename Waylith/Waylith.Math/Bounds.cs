using System;
using System.Globalization;
using System.Numerics;

namespace Waylith.Math;

public struct Bounds : IEquatable<Bounds>, IFormattable
{
	private Vector3 _center;

	private Vector3 _extents;

	public Vector3 center
	{
		get
		{
			return _center;
		}
		set
		{
			_center = value;
		}
	}

	public Vector3 extents
	{
		get
		{
			return _extents;
		}
		set
		{
			_extents = value;
		}
	}

	public Vector3 size
	{
		get
		{
			return _extents * 2f;
		}
		set
		{
			_extents = value * 0.5f;
		}
	}

	public Vector3 min
	{
		get
		{
			return _center - _extents;
		}
		set
		{
			SetMinMax(value, max);
		}
	}

	public Vector3 max
	{
		get
		{
			return _center + _extents;
		}
		set
		{
			SetMinMax(min, value);
		}
	}

	public Bounds(Vector3 center, Vector3 size)
	{
		_center = center;
		_extents = size * 0.5f;
	}

	public bool Equals(Bounds other)
	{
		if (_center.Equals(other._center))
		{
			return _extents.Equals(other._extents);
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj is Bounds other)
		{
			return Equals(other);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(_center, _extents);
	}

	public string ToString(string format, IFormatProvider formatProvider)
	{
		return "Center: " + _center.ToString(format, formatProvider) + ", Extents: " + _extents.ToString(format, formatProvider);
	}

	public override string ToString()
	{
		return ToString(null, CultureInfo.InvariantCulture);
	}

	public Vector3 ClosestPoint(Vector3 point)
	{
		Vector3 value = min;
		Vector3 value2 = max;
		return Vector3.Min(Vector3.Max(point, value), value2);
	}

	public bool Contains(Vector3 point)
	{
		Vector3 vector = min;
		Vector3 vector2 = max;
		if (point.X >= vector.X && point.X <= vector2.X && point.Y >= vector.Y && point.Y <= vector2.Y && point.Z >= vector.Z)
		{
			return point.Z <= vector2.Z;
		}
		return false;
	}

	public void Encapsulate(Bounds bounds)
	{
		SetMinMax(Vector3.Min(min, bounds.min), Vector3.Max(max, bounds.max));
	}

	public void Encapsulate(Vector3 point)
	{
		SetMinMax(Vector3.Min(min, point), Vector3.Max(max, point));
	}

	public void Expand(float amount)
	{
		_extents += new Vector3(amount, amount, amount) * 0.5f;
	}

	public bool IntersectRay(Ray ray)
	{
		float distance;
		return IntersectRay(ray, out distance);
	}

	public bool IntersectRay(Ray ray, out float distance)
	{
		distance = 0f;
		float tMin2 = float.NegativeInfinity;
		float tMax2 = float.PositiveInfinity;
		Vector3 vector = min;
		Vector3 vector2 = max;
		if (!UpdateSlab(ray.origin.X, ray.direction.X, vector.X, vector2.X, ref tMin2, ref tMax2) || !UpdateSlab(ray.origin.Y, ray.direction.Y, vector.Y, vector2.Y, ref tMin2, ref tMax2) || !UpdateSlab(ray.origin.Z, ray.direction.Z, vector.Z, vector2.Z, ref tMin2, ref tMax2))
		{
			return false;
		}
		distance = ((tMin2 >= 0f) ? tMin2 : tMax2);
		return distance >= 0f;
		static bool UpdateSlab(float origin, float direction, float min, float max, ref float tMin, ref float tMax)
		{
			if (System.Math.Abs(direction) < 1E-06f)
			{
				if (origin >= min)
				{
					return origin <= max;
				}
				return false;
			}
			float num = 1f / direction;
			float num2 = (min - origin) * num;
			float num3 = (max - origin) * num;
			if (num2 > num3)
			{
				float num4 = num3;
				num3 = num2;
				num2 = num4;
			}
			tMin = System.Math.Max(tMin, num2);
			tMax = System.Math.Min(tMax, num3);
			return tMin <= tMax;
		}
	}

	public bool Intersects(Bounds bounds)
	{
		Vector3 vector = min;
		Vector3 vector2 = max;
		Vector3 vector3 = bounds.min;
		Vector3 vector4 = bounds.max;
		if (vector.X <= vector4.X && vector2.X >= vector3.X && vector.Y <= vector4.Y && vector2.Y >= vector3.Y && vector.Z <= vector4.Z)
		{
			return vector2.Z >= vector3.Z;
		}
		return false;
	}

	public void SetMinMax(Vector3 min, Vector3 max)
	{
		_extents = (max - min) * 0.5f;
		_center = min + _extents;
	}
}
