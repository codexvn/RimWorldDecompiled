using System;
using System.Globalization;
using System.Numerics;

namespace UnityEngine;

public struct Vector4 : IEquatable<Vector4>, IFormattable
{
	public float x;

	public float y;

	public float z;

	public float w;

	public const float kEpsilon = 1E-05f;

	public float this[int index]
	{
		get
		{
			return index switch
			{
				0 => x, 
				1 => y, 
				2 => z, 
				3 => w, 
				_ => throw new IndexOutOfRangeException("Invalid Vector4 index!"), 
			};
		}
		set
		{
			switch (index)
			{
			case 0:
				x = value;
				break;
			case 1:
				y = value;
				break;
			case 2:
				z = value;
				break;
			case 3:
				w = value;
				break;
			default:
				throw new IndexOutOfRangeException("Invalid Vector4 index!");
			}
		}
	}

	private System.Numerics.Vector4 _value
	{
		readonly get
		{
			return new System.Numerics.Vector4(x, y, z, w);
		}
		set
		{
			x = value.X;
			y = value.Y;
			z = value.Z;
			w = value.W;
		}
	}

	public float magnitude => _value.Length();

	public Vector4 normalized
	{
		get
		{
			if (!(_value.LengthSquared() <= 9.9999994E-11f))
			{
				return FromNumerics(System.Numerics.Vector4.Normalize(_value));
			}
			return zero;
		}
	}

	public float sqrMagnitude => _value.LengthSquared();

	public static Vector4 one => new Vector4(1f, 1f, 1f, 1f);

	public static Vector4 zero => new Vector4(0f, 0f, 0f, 0f);

	public bool Equals(Vector4 other)
	{
		return _value.Equals(other._value);
	}

	public static bool operator !=(Vector4 lhs, Vector4 rhs)
	{
		return !lhs.Equals(rhs);
	}

	public string ToString(string format, IFormatProvider formatProvider)
	{
		return $"({x.ToString(format, formatProvider)}, {y.ToString(format, formatProvider)}, {z.ToString(format, formatProvider)}, {w.ToString(format, formatProvider)})";
	}

	public Vector4(float x, float y, float z, float w)
	{
		this.x = x;
		this.y = y;
		this.z = z;
		this.w = w;
	}

	public Vector4(float x, float y, float z)
		: this(x, y, z, 0f)
	{
	}

	public Vector4(float x, float y)
		: this(x, y, 0f, 0f)
	{
	}

	public static float Distance(Vector4 a, Vector4 b)
	{
		return System.Numerics.Vector4.Distance(a._value, b._value);
	}

	public static float Dot(Vector4 a, Vector4 b)
	{
		return System.Numerics.Vector4.Dot(a._value, b._value);
	}

	public static Vector4 Lerp(Vector4 a, Vector4 b, float t)
	{
		return FromNumerics(System.Numerics.Vector4.Lerp(a._value, b._value, Math.Clamp(t, 0f, 1f)));
	}

	public static Vector4 LerpUnclamped(Vector4 a, Vector4 b, float t)
	{
		return FromNumerics(System.Numerics.Vector4.Lerp(a._value, b._value, t));
	}

	public void Normalize()
	{
		this = normalized;
	}

	public void Scale(Vector4 scale)
	{
		this = FromNumerics(_value * scale._value);
	}

	public static Vector4 operator +(Vector4 a, Vector4 b)
	{
		return FromNumerics(a._value + b._value);
	}

	public static Vector4 operator -(Vector4 a, Vector4 b)
	{
		return FromNumerics(a._value - b._value);
	}

	public static Vector4 operator -(Vector4 a)
	{
		return FromNumerics(-a._value);
	}

	public static bool operator ==(Vector4 lhs, Vector4 rhs)
	{
		return lhs.Equals(rhs);
	}

	public static implicit operator Vector4(Vector3 v)
	{
		return new Vector4(v.x, v.y, v.z, 0f);
	}

	public static implicit operator Vector3(Vector4 v)
	{
		return new Vector3(v.x, v.y, v.z);
	}

	public static implicit operator Vector4(Vector2 v)
	{
		return new Vector4(v.x, v.y, 0f, 0f);
	}

	public static implicit operator Vector2(Vector4 v)
	{
		return new Vector2(v.x, v.y);
	}

	public static Vector4 operator *(float d, Vector4 a)
	{
		return a * d;
	}

	public static Vector4 operator *(Vector4 a, float d)
	{
		return FromNumerics(a._value * d);
	}

	public static Vector4 operator *(Vector4 a, Vector4 b)
	{
		return FromNumerics(a._value * b._value);
	}

	public static Vector4 operator /(Vector4 a, float d)
	{
		return FromNumerics(a._value / d);
	}

	public static Vector4 operator /(Vector4 a, Vector4 b)
	{
		return FromNumerics(a._value / b._value);
	}

	public override bool Equals(object obj)
	{
		if (obj is Vector4 other)
		{
			return Equals(other);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return _value.GetHashCode();
	}

	public override string ToString()
	{
		return ToString(null, CultureInfo.InvariantCulture);
	}

	public string ToString(string format)
	{
		return ToString(format, CultureInfo.InvariantCulture);
	}

	internal static System.Numerics.Vector4 ToNumerics(Vector4 value)
	{
		return value._value;
	}

	internal static Vector4 FromNumerics(System.Numerics.Vector4 value)
	{
		Vector4 result = default(Vector4);
		result._value = value;
		return result;
	}
}
