using System;
using System.Globalization;
using System.Numerics;

namespace UnityEngine;

public struct Vector3 : IEquatable<Vector3>, IFormattable
{
	public float x;

	public float y;

	public float z;

	public const float kEpsilon = 1E-05f;

	public const float kEpsilonNormalSqrt = 1E-15f;

	public float this[int index]
	{
		readonly get
		{
			return index switch
			{
				0 => x, 
				1 => y, 
				_ => z, 
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
			default:
				z = value;
				break;
			}
		}
	}

	public static Vector3 back => new Vector3(0f, 0f, -1f);

	public static Vector3 down => new Vector3(0f, -1f, 0f);

	public static Vector3 forward => new Vector3(0f, 0f, 1f);

	public static Vector3 left => new Vector3(-1f, 0f, 0f);

	public readonly float magnitude => (float)Math.Sqrt(x * x + y * y + z * z);

	public static Vector3 negativeInfinity => new Vector3(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);

	public readonly Vector3 normalized
	{
		get
		{
			if (!(magnitude > 1E-05f))
			{
				return zero;
			}
			return this / magnitude;
		}
	}

	public static Vector3 one => new Vector3(1f, 1f, 1f);

	public static Vector3 positiveInfinity => new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);

	public static Vector3 right => new Vector3(1f, 0f, 0f);

	public readonly float sqrMagnitude => x * x + y * y + z * z;

	public static Vector3 up => new Vector3(0f, 1f, 0f);

	public static Vector3 zero => new Vector3(0f, 0f, 0f);

	public Vector3(float x, float y, float z)
	{
		this.x = x;
		this.y = y;
		this.z = z;
	}

	public Vector3(float x, float y)
		: this(x, y, 0f)
	{
	}

	public void Set(float newX, float newY, float newZ)
	{
		x = newX;
		y = newY;
		z = newZ;
	}

	public static float Angle(Vector3 from, Vector3 to)
	{
		double num = Math.Sqrt((double)Dot(from, from) * (double)Dot(to, to));
		if (num < 1E-15)
		{
			return 0f;
		}
		return (float)(Math.Acos(Math.Clamp((double)Dot(from, to) / num, -1.0, 1.0)) * 57.2957795);
	}

	public static float SignedAngle(Vector3 from, Vector3 to, Vector3 axis)
	{
		Vector3 rhs = Cross(from, to);
		return Angle(from, to) * Mathf.Sign(Dot(axis, rhs));
	}

	public static Vector3 RotateTowards(Vector3 current, Vector3 target, float maxRadiansDelta, float maxMagnitudeDelta)
	{
		float num = current.magnitude;
		float num2 = target.magnitude;
		float num3 = Mathf.MoveTowards(num, num2, maxMagnitudeDelta);
		if (num <= 1E-05f || num2 <= 1E-05f)
		{
			if (num > 1E-05f)
			{
				return current.normalized * num3;
			}
			if (num2 > 1E-05f)
			{
				return target.normalized * num3;
			}
			return zero;
		}
		Vector3 vector = current.normalized;
		Vector3 vector2 = target.normalized;
		float num4 = (float)Math.Acos(Math.Clamp(Dot(vector, vector2), -1f, 1f));
		if (num4 <= 1E-05f)
		{
			return vector * num3;
		}
		float num5 = Mathf.Clamp(maxRadiansDelta, 0f - num4, num4);
		float num6 = num5 / num4;
		float num7 = Mathf.Sin(num4);
		if (num7 > 1E-05f)
		{
			float num8 = Mathf.Sin((1f - num6) * num4) / num7;
			float num9 = Mathf.Sin(num6 * num4) / num7;
			return (vector * num8 + vector2 * num9).normalized * num3;
		}
		Vector3 vector3 = Cross(vector, up);
		if (vector3.sqrMagnitude <= 1E-05f)
		{
			vector3 = Cross(vector, right);
		}
		return (Quaternion.AngleAxis(num5 * 57.29578f, vector3.normalized) * vector).normalized * num3;
	}

	public static Vector3 SmoothDamp(Vector3 current, Vector3 target, ref Vector3 currentVelocity, float smoothTime, float maxSpeed = float.PositiveInfinity, float deltaTime = -1f)
	{
		if (deltaTime < 0f)
		{
			deltaTime = Time.deltaTime;
		}
		if (deltaTime <= 0f)
		{
			return current;
		}
		smoothTime = Mathf.Max(0.0001f, smoothTime);
		float num = 2f / smoothTime;
		float num2 = num * deltaTime;
		float num3 = 1f / (1f + num2 + 0.48f * num2 * num2 + 0.235f * num2 * num2 * num2);
		float num4 = current.x - target.x;
		float num5 = current.y - target.y;
		float num6 = current.z - target.z;
		Vector3 vector = target;
		float num7 = maxSpeed * smoothTime;
		float num8 = num7 * num7;
		float num9 = num4 * num4 + num5 * num5 + num6 * num6;
		if (num9 > num8)
		{
			float num10 = (float)Math.Sqrt(num9);
			num4 = num4 / num10 * num7;
			num5 = num5 / num10 * num7;
			num6 = num6 / num10 * num7;
		}
		float num11 = current.x - num4;
		float num12 = current.y - num5;
		float num13 = current.z - num6;
		float num14 = (currentVelocity.x + num * num4) * deltaTime;
		float num15 = (currentVelocity.y + num * num5) * deltaTime;
		float num16 = (currentVelocity.z + num * num6) * deltaTime;
		currentVelocity.x = (currentVelocity.x - num * num14) * num3;
		currentVelocity.y = (currentVelocity.y - num * num15) * num3;
		currentVelocity.z = (currentVelocity.z - num * num16) * num3;
		float num17 = num11 + (num4 + num14) * num3;
		float num18 = num12 + (num5 + num15) * num3;
		float num19 = num13 + (num6 + num16) * num3;
		float num20 = vector.x - current.x;
		float num21 = vector.y - current.y;
		float num22 = vector.z - current.z;
		float num23 = num17 - vector.x;
		float num24 = num18 - vector.y;
		float num25 = num19 - vector.z;
		if (num20 * num23 + num21 * num24 + num22 * num25 > 0f)
		{
			num17 = vector.x;
			num18 = vector.y;
			num19 = vector.z;
			currentVelocity.x = (num17 - vector.x) / deltaTime;
			currentVelocity.y = (num18 - vector.y) / deltaTime;
			currentVelocity.z = (num19 - vector.z) / deltaTime;
		}
		return new Vector3(num17, num18, num19);
	}

	public static Vector3 Cross(Vector3 lhs, Vector3 rhs)
	{
		return new Vector3(lhs.y * rhs.z - lhs.z * rhs.y, lhs.z * rhs.x - lhs.x * rhs.z, lhs.x * rhs.y - lhs.y * rhs.x);
	}

	public static float Distance(Vector3 a, Vector3 b)
	{
		return (a - b).magnitude;
	}

	public static float Magnitude(Vector3 vector)
	{
		return vector.magnitude;
	}

	public static float SqrMagnitude(Vector3 vector)
	{
		return vector.sqrMagnitude;
	}

	public static Vector3 Reflect(Vector3 inDirection, Vector3 inNormal)
	{
		return -2f * Dot(inNormal, inDirection) * inNormal + inDirection;
	}

	public static float Dot(Vector3 lhs, Vector3 rhs)
	{
		return lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z;
	}

	public static Vector3 Lerp(Vector3 a, Vector3 b, float t)
	{
		return LerpUnclamped(a, b, Math.Clamp(t, 0f, 1f));
	}

	public static Vector3 LerpUnclamped(Vector3 a, Vector3 b, float t)
	{
		return new Vector3(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t, a.z + (b.z - a.z) * t);
	}

	public static Vector3 Max(Vector3 lhs, Vector3 rhs)
	{
		return new Vector3(Mathf.Max(lhs.x, rhs.x), Mathf.Max(lhs.y, rhs.y), Mathf.Max(lhs.z, rhs.z));
	}

	public static Vector3 Min(Vector3 lhs, Vector3 rhs)
	{
		return new Vector3(Mathf.Min(lhs.x, rhs.x), Mathf.Min(lhs.y, rhs.y), Mathf.Min(lhs.z, rhs.z));
	}

	public static Vector3 MoveTowards(Vector3 current, Vector3 target, float maxDistanceDelta)
	{
		Vector3 vector = target - current;
		float num = vector.magnitude;
		if (num <= maxDistanceDelta || num <= 1E-05f)
		{
			return target;
		}
		return current + vector / num * maxDistanceDelta;
	}

	public void Normalize()
	{
		this = normalized;
	}

	public static Vector3 Normalize(Vector3 value)
	{
		return value.normalized;
	}

	public static Vector3 operator +(Vector3 a, Vector3 b)
	{
		return new Vector3(a.x + b.x, a.y + b.y, a.z + b.z);
	}

	public static Vector3 operator /(Vector3 a, float d)
	{
		return new Vector3(a.x / d, a.y / d, a.z / d);
	}

	public static Vector3 operator *(float d, Vector3 a)
	{
		return a * d;
	}

	public static Vector3 operator *(Vector3 a, float d)
	{
		return new Vector3(a.x * d, a.y * d, a.z * d);
	}

	public static Vector3 operator -(Vector3 a, Vector3 b)
	{
		return new Vector3(a.x - b.x, a.y - b.y, a.z - b.z);
	}

	public static Vector3 operator -(Vector3 a)
	{
		return new Vector3(0f - a.x, 0f - a.y, 0f - a.z);
	}

	public static Vector3 ProjectOnPlane(Vector3 vector, Vector3 planeNormal)
	{
		float num = Dot(planeNormal, planeNormal);
		if (num < 1E-05f)
		{
			return vector;
		}
		return vector - planeNormal * (Dot(vector, planeNormal) / num);
	}

	public void Scale(Vector3 scale)
	{
		x *= scale.x;
		y *= scale.y;
		z *= scale.z;
	}

	public static Vector3 Scale(Vector3 a, Vector3 b)
	{
		return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
	}

	public static Vector3 Slerp(Vector3 a, Vector3 b, float t)
	{
		return SlerpUnclamped(a, b, Math.Clamp(t, 0f, 1f));
	}

	public static Vector3 SlerpUnclamped(Vector3 a, Vector3 b, float t)
	{
		float num = a.magnitude;
		float num2 = b.magnitude;
		if (num < 1E-05f || num2 < 1E-05f)
		{
			return LerpUnclamped(a, b, t);
		}
		Vector3 vector = a / num;
		Vector3 vector2 = b / num2;
		float num3 = Math.Clamp(Dot(vector, vector2), -1f, 1f);
		Vector3 vector3 = vector2 - vector * num3;
		if (vector3.sqrMagnitude < 9.9999994E-11f)
		{
			if (num3 > 0f)
			{
				return LerpUnclamped(a, b, t);
			}
			vector3 = Cross(vector, (Math.Abs(vector.x) < 0.9f) ? right : up);
		}
		float num4 = MathF.Acos(num3) * t;
		return (vector * MathF.Cos(num4) + vector3.normalized * MathF.Sin(num4)) * (num + (num2 - num) * t);
	}

	public readonly bool Equals(Vector3 other)
	{
		if (x.Equals(other.x) && y.Equals(other.y))
		{
			return z.Equals(other.z);
		}
		return false;
	}

	public override readonly bool Equals(object obj)
	{
		if (obj is Vector3 other)
		{
			return Equals(other);
		}
		return false;
	}

	public override readonly int GetHashCode()
	{
		return HashCode.Combine(x, y, z);
	}

	public static bool operator ==(Vector3 lhs, Vector3 rhs)
	{
		return lhs.Equals(rhs);
	}

	public static bool operator !=(Vector3 lhs, Vector3 rhs)
	{
		return !lhs.Equals(rhs);
	}

	public readonly string ToString(string format, IFormatProvider formatProvider)
	{
		return $"({x.ToString(format, formatProvider)}, {y.ToString(format, formatProvider)}, {z.ToString(format, formatProvider)})";
	}

	public readonly string ToString(string format)
	{
		return ToString(format, CultureInfo.InvariantCulture);
	}

	public override readonly string ToString()
	{
		return $"({x}, {y}, {z})";
	}

	internal static System.Numerics.Vector3 ToNumerics(Vector3 value)
	{
		return new System.Numerics.Vector3(value.x, value.y, value.z);
	}

	internal static Vector3 FromNumerics(System.Numerics.Vector3 value)
	{
		return new Vector3(value.X, value.Y, value.Z);
	}

	public static Vector3 ClampMagnitude(Vector3 vector, float maxLength)
	{
		float num = vector.sqrMagnitude;
		if (num <= maxLength * maxLength)
		{
			return vector;
		}
		float num2 = MathF.Sqrt(num);
		return new Vector3(vector.x / num2 * maxLength, vector.y / num2 * maxLength, vector.z / num2 * maxLength);
	}
}
