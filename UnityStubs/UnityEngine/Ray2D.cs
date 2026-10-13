using System;
using System.Globalization;

namespace UnityEngine;

public struct Ray2D : IFormattable
{
	private Vector2 _direction;

	private Vector2 _origin;

	public Vector2 direction
	{
		get
		{
			return _direction;
		}
		set
		{
			_direction = value.normalized;
		}
	}

	public Vector2 origin
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

	public Ray2D(Vector2 origin, Vector2 direction)
	{
		_origin = origin;
		_direction = direction.normalized;
	}

	public Vector2 GetPoint(float distance)
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
