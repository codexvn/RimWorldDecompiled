using System;
using System.Globalization;

namespace UnityEngine;

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
		Vector3 rhs = min;
		Vector3 rhs2 = max;
		return Vector3.Min(Vector3.Max(point, rhs), rhs2);
	}

	public float SqrDistance(Vector3 point)
	{
		return (ClosestPoint(point) - point).sqrMagnitude;
	}

	public bool Contains(Vector3 point)
	{
		Vector3 vector = min;
		Vector3 vector2 = max;
		if (point.x >= vector.x && point.x <= vector2.x && point.y >= vector.y && point.y <= vector2.y && point.z >= vector.z)
		{
			return point.z <= vector2.z;
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
		if (!UpdateSlab(ray.origin.x, ray.direction.x, vector.x, vector2.x, ref tMin2, ref tMax2) || !UpdateSlab(ray.origin.y, ray.direction.y, vector.y, vector2.y, ref tMin2, ref tMax2) || !UpdateSlab(ray.origin.z, ray.direction.z, vector.z, vector2.z, ref tMin2, ref tMax2))
		{
			return false;
		}
		distance = ((tMin2 >= 0f) ? tMin2 : tMax2);
		return distance >= 0f;
		static bool UpdateSlab(float origin, float direction, float min, float max, ref float tMin, ref float tMax)
		{
			if (Math.Abs(direction) < 1E-06f)
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
			tMin = Math.Max(tMin, num2);
			tMax = Math.Min(tMax, num3);
			return tMin <= tMax;
		}
	}

	public bool Intersects(Bounds bounds)
	{
		Vector3 vector = min;
		Vector3 vector2 = max;
		Vector3 vector3 = bounds.min;
		Vector3 vector4 = bounds.max;
		if (vector.x <= vector4.x && vector2.x >= vector3.x && vector.y <= vector4.y && vector2.y >= vector3.y && vector.z <= vector4.z)
		{
			return vector2.z >= vector3.z;
		}
		return false;
	}

	public void SetMinMax(Vector3 min, Vector3 max)
	{
		_extents = (max - min) * 0.5f;
		_center = min + _extents;
	}
}
