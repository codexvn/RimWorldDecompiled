using System;
using System.Globalization;
using System.Numerics;

namespace Waylith.Math;

public struct Ray : IFormattable
{
	private Vector3 _direction;

	private Vector3 _origin;

	public Vector3 direction
	{
		get
		{
			return _direction;
		}
		set
		{
			_direction = Vector3Extensions.get_normalized(value);
		}
	}

	public Vector3 origin
	{
		get
		{
			return _origin;
		}
		set
		{
			_origin = value;
		}
	}

	public Ray(Vector3 origin, Vector3 direction)
	{
		_origin = origin;
		_direction = Vector3Extensions.get_normalized(direction);
	}

	public Vector3 GetPoint(float distance)
	{
		return _origin + _direction * distance;
	}

	public string ToString(string format, IFormatProvider formatProvider)
	{
		return "Origin: " + _origin.ToString(format, formatProvider) + ", Dir: " + _direction.ToString(format, formatProvider);
	}

	public override string ToString()
	{
		return ToString(null, CultureInfo.InvariantCulture);
	}
}
