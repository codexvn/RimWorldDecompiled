using System;
using System.Globalization;
using System.Numerics;

namespace UnityEngine;

public struct Vector2 : IEquatable<Vector2>, IFormattable
{
	public float x;

	public float y;

	public const float kEpsilon = 1E-05f;

	public const float kEpsilonNormalSqrt = 1E-15f;

	public float this[int index]
	{
		readonly get
		{
			if (index != 0)
			{
				return y;
			}
			return x;
		}
		set
		{
			if (index == 0)
			{
				x = value;
			}
			else
			{
				y = value;
			}
		}
	}

	public static Vector2 down => new Vector2(0f, -1f);

	public static Vector2 zero => new Vector2(0f, 0f);

	public static Vector2 one => new Vector2(1f, 1f);

	public static Vector2 up => new Vector2(0f, 1f);

	public static Vector2 right => new Vector2(1f, 0f);

	public static Vector2 left => new Vector2(-1f, 0f);

	public static Vector2 negativeInfinity => new Vector2(float.NegativeInfinity, float.NegativeInfinity);

	public static Vector2 positiveInfinity => new Vector2(float.PositiveInfinity, float.PositiveInfinity);

	public readonly float magnitude => (float)Math.Sqrt(x * x + y * y);

	public readonly float sqrMagnitude => x * x + y * y;

	public Vector2 normalized
	{
		get
		{
			float num = magnitude;
			if (!(num > 1E-05f))
			{
				return zero;
			}
			return this / num;
		}
	}

	public Vector2(float x, float y)
	{
		this.x = x;
		this.y = y;
	}

	public static float Dot(Vector2 lhs, Vector2 rhs)
	{
		return lhs.x * rhs.x + lhs.y * rhs.y;
	}

	public static Vector2 Perpendicular(Vector2 inDirection)
	{
		return new Vector2(0f - inDirection.y, inDirection.x);
	}

	public static Vector2 SmoothDamp(Vector2 current, Vector2 target, ref Vector2 currentVelocity, float smoothTime)
	{
		return SmoothDamp(current, target, ref currentVelocity, smoothTime, float.PositiveInfinity, Time.deltaTime);
	}

	public static Vector2 SmoothDamp(Vector2 current, Vector2 target, ref Vector2 currentVelocity, float smoothTime, float maxSpeed)
	{
		return SmoothDamp(current, target, ref currentVelocity, smoothTime, maxSpeed, Time.deltaTime);
	}

	public static Vector2 SmoothDamp(Vector2 current, Vector2 target, ref Vector2 currentVelocity, float smoothTime, float maxSpeed, float deltaTime)
	{
		Vector3 currentVelocity2 = new Vector3(currentVelocity.x, currentVelocity.y, 0f);
		Vector3 vector = Vector3.SmoothDamp(new Vector3(current.x, current.y, 0f), new Vector3(target.x, target.y, 0f), ref currentVelocity2, smoothTime, maxSpeed, deltaTime);
		currentVelocity = new Vector2(currentVelocity2.x, currentVelocity2.y);
		return new Vector2(vector.x, vector.y);
	}

	public static Vector2 MoveTowards(Vector2 current, Vector2 target, float maxDistanceDelta)
	{
		float num = target.x - current.x;
		float num2 = target.y - current.y;
		float num3 = num * num + num2 * num2;
		if (num3 == 0f || (maxDistanceDelta >= 0f && num3 <= maxDistanceDelta * maxDistanceDelta))
		{
			return target;
		}
		float num4 = (float)Math.Sqrt(num3);
		return new Vector2(current.x + num / num4 * maxDistanceDelta, current.y + num2 / num4 * maxDistanceDelta);
	}

	public static Vector2 Min(Vector2 a, Vector2 b)
	{
		return new Vector2(Mathf.Min(a.x, b.x), Mathf.Min(a.y, b.y));
	}

	public static Vector2 Max(Vector2 a, Vector2 b)
	{
		return new Vector2(Mathf.Max(a.x, b.x), Mathf.Max(a.y, b.y));
	}

	public static float SqrMagnitude(Vector2 a)
	{
		return a.x * a.x + a.y * a.y;
	}

	public readonly bool Equals(Vector2 other)
	{
		if (x.Equals(other.x))
		{
			return y.Equals(other.y);
		}
		return false;
	}

	public override readonly bool Equals(object obj)
	{
		if (obj is Vector2 other)
		{
			return Equals(other);
		}
		return false;
	}

	public override readonly int GetHashCode()
	{
		return HashCode.Combine(x, y);
	}

	public static float Distance(Vector2 a, Vector2 b)
	{
		return (a - b).magnitude;
	}

	public static Vector2 Lerp(Vector2 a, Vector2 b, float t)
	{
		t = Math.Clamp(t, 0f, 1f);
		return LerpUnclamped(a, b, t);
	}

	public static Vector2 LerpUnclamped(Vector2 a, Vector2 b, float t)
	{
		return new Vector2(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t);
	}

	public void Normalize()
	{
		this = normalized;
	}

	public static Vector2 ClampMagnitude(Vector2 vector, float maxLength)
	{
		float num = vector.sqrMagnitude;
		if (num <= maxLength * maxLength)
		{
			return vector;
		}
		float num2 = MathF.Sqrt(num);
		return new Vector2(vector.x / num2 * maxLength, vector.y / num2 * maxLength);
	}

	public void Scale(Vector2 scale)
	{
		x *= scale.x;
		y *= scale.y;
	}

	public static Vector2 Scale(Vector2 a, Vector2 b)
	{
		return new Vector2(a.x * b.x, a.y * b.y);
	}

	public readonly float SqrMagnitude()
	{
		return sqrMagnitude;
	}

	public void Set(float newX, float newY)
	{
		x = newX;
		y = newY;
	}

	public static bool operator ==(Vector2 lhs, Vector2 rhs)
	{
		return lhs.Equals(rhs);
	}

	public static bool operator !=(Vector2 lhs, Vector2 rhs)
	{
		return !lhs.Equals(rhs);
	}

	public static Vector2 operator +(Vector2 a, Vector2 b)
	{
		return new Vector2(a.x + b.x, a.y + b.y);
	}

	public static Vector2 operator -(Vector2 a, Vector2 b)
	{
		return new Vector2(a.x - b.x, a.y - b.y);
	}

	public static Vector2 operator -(Vector2 a)
	{
		return new Vector2(0f - a.x, 0f - a.y);
	}

	public static Vector2 operator *(Vector2 a, float d)
	{
		return new Vector2(a.x * d, a.y * d);
	}

	public static Vector2 operator *(float d, Vector2 a)
	{
		return a * d;
	}

	public static Vector2 operator *(Vector2 a, Vector2 b)
	{
		return new Vector2(a.x * b.x, a.y * b.y);
	}

	public static Vector2 operator /(Vector2 a, float d)
	{
		return new Vector2(a.x / d, a.y / d);
	}

	public static Vector2 operator /(Vector2 a, Vector2 b)
	{
		return new Vector2(a.x / b.x, a.y / b.y);
	}

	public static implicit operator Vector2(Vector3 v)
	{
		return new Vector2(v.x, v.y);
	}

	public static implicit operator Vector3(Vector2 v)
	{
		return new Vector3(v.x, v.y, 0f);
	}

	public readonly string ToString(string format, IFormatProvider formatProvider)
	{
		return $"({x.ToString(format, formatProvider)}, {y.ToString(format, formatProvider)})";
	}

	public readonly string ToString(string format)
	{
		return ToString(format, CultureInfo.InvariantCulture);
	}

	public override readonly string ToString()
	{
		return $"({x}, {y})";
	}

	internal static System.Numerics.Vector2 ToNumerics(Vector2 value)
	{
		return new System.Numerics.Vector2(value.x, value.y);
	}

	internal static Vector2 FromNumerics(System.Numerics.Vector2 value)
	{
		return new Vector2(value.X, value.Y);
	}

	public static float Angle(Vector2 from, Vector2 to)
	{
		float num = MathF.Sqrt(from.sqrMagnitude * to.sqrMagnitude);
		if (num < 1E-15f)
		{
			return 0f;
		}
		return MathF.Acos(Math.Clamp(Dot(from, to) / num, -1f, 1f)) * 57.29578f;
	}

	public static float SignedAngle(Vector2 from, Vector2 to)
	{
		float num = Angle(from, to);
		int num2 = MathF.Sign(from.x * to.y - from.y * to.x);
		return num * (float)num2;
	}
}
