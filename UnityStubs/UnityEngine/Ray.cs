using System;
using System.Globalization;

namespace UnityEngine;

public struct Ray : IFormattable
{
	private Vector3 m_Direction;

	private Vector3 m_Origin;

	public Vector3 direction
	{
		get
		{
			return m_Direction;
		}
		set
		{
			m_Direction = value.normalized;
		}
	}

	public Vector3 origin
	{
		get
		{
			return m_Origin;
		}
		set
		{
			m_Origin = value;
		}
	}

	public Ray(Vector3 origin, Vector3 direction)
	{
		m_Origin = origin;
		m_Direction = direction.normalized;
	}

	public Vector3 GetPoint(float distance)
	{
		return m_Origin + m_Direction * distance;
	}

	public string ToString(string format, IFormatProvider formatProvider)
	{
		return "Origin: " + m_Origin.ToString(format, formatProvider) + ", Dir: " + m_Direction.ToString(format, formatProvider);
	}

	public override string ToString()
	{
		return ToString(null, CultureInfo.InvariantCulture);
	}
}
