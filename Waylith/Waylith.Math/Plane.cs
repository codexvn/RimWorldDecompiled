using System;
using System.Globalization;
using System.Numerics;

namespace Waylith.Math;

public struct Plane : IEquatable<Plane>, IFormattable
{
	private Vector3 _normal;

	private float _distance;

	public const int size = 16;

	public float distance => _distance;

	public Vector3 normal => _normal;

	public Plane(Vector3 inNormal, Vector3 inPoint)
	{
		_normal = Vector3Extensions.get_normalized(inNormal);
		_distance = 0f - Vector3.Dot(_normal, inPoint);
	}

	public Plane(Vector3 inNormal, float d)
	{
		_normal = Vector3Extensions.get_normalized(inNormal);
		_distance = d;
	}

	public Vector3 ClosestPointOnPlane(Vector3 point)
	{
		float num = Vector3.Dot(_normal, point) + _distance;
		return point - _normal * num;
	}

	public bool Raycast(Ray ray, out float enter)
	{
		float num = Vector3.Dot(ray.direction, _normal);
		if (MathF.Abs(num) < 1E-06f)
		{
			enter = 0f;
			return false;
		}
		enter = (0f - (Vector3.Dot(ray.origin, _normal) + _distance)) / num;
		return enter >= 0f;
	}

	public bool Equals(Plane other)
	{
		if (_normal.Equals(other._normal))
		{
			return _distance.Equals(other._distance);
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj is Plane other)
		{
			return Equals(other);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(_normal, _distance);
	}

	public string ToString(string format, IFormatProvider formatProvider)
	{
		return "Normal: " + _normal.ToString(format, formatProvider) + ", Distance: " + _distance.ToString(format, formatProvider);
	}

	public override string ToString()
	{
		return ToString(null, CultureInfo.InvariantCulture);
	}
}
