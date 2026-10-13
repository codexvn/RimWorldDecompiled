using System;
using System.Globalization;
using System.Numerics;

namespace UnityEngine;

public struct Quaternion : IEquatable<Quaternion>, IFormattable
{
	public float x;

	public float y;

	public float z;

	public float w;

	public const float kEpsilon = 1E-06f;

	public Vector3 eulerAngles
	{
		get
		{
			Quaternion quaternion = Normalize(this);
			float num = 2f * (quaternion.x * quaternion.w - quaternion.y * quaternion.z);
			float num2;
			float num3;
			float num4;
			if (MathF.Abs(num) < 0.9999995f)
			{
				num2 = MathF.Asin(Math.Clamp(num, -1f, 1f));
				num3 = MathF.Atan2(2f * (quaternion.x * quaternion.z + quaternion.y * quaternion.w), 1f - 2f * (quaternion.x * quaternion.x + quaternion.y * quaternion.y));
				num4 = MathF.Atan2(2f * (quaternion.x * quaternion.y + quaternion.z * quaternion.w), 1f - 2f * (quaternion.x * quaternion.x + quaternion.z * quaternion.z));
			}
			else
			{
				num2 = MathF.CopySign((float)Math.PI / 2f, num);
				float num5 = 2f * (quaternion.x * quaternion.y - quaternion.z * quaternion.w);
				float num6 = 1f - 2f * (quaternion.y * quaternion.y + quaternion.z * quaternion.z);
				num3 = MathF.Atan2((num > 0f) ? num5 : (0f - num5), num6);
				num4 = 0f;
			}
			return new Vector3(Wrap360(num2 * 57.29578f), Wrap360(num3 * 57.29578f), Wrap360(num4 * 57.29578f));
		}
	}

	public static Quaternion identity => new Quaternion(0f, 0f, 0f, 1f);

	public bool Equals(Quaternion other)
	{
		if (x.Equals(other.x) && y.Equals(other.y) && z.Equals(other.z))
		{
			return w.Equals(other.w);
		}
		return false;
	}

	public static bool operator ==(Quaternion lhs, Quaternion rhs)
	{
		return lhs.Equals(rhs);
	}

	public string ToString(string format, IFormatProvider formatProvider)
	{
		return $"({x.ToString(format, formatProvider)}, {y.ToString(format, formatProvider)}, {z.ToString(format, formatProvider)}, {w.ToString(format, formatProvider)})";
	}

	public string ToString(string format)
	{
		return ToString(format, CultureInfo.InvariantCulture);
	}

	public Quaternion(float x, float y, float z, float w)
	{
		this.x = x;
		this.y = y;
		this.z = z;
		this.w = w;
	}

	public static float Angle(Quaternion a, Quaternion b)
	{
		return MathF.Acos(MathF.Min(MathF.Abs(System.Numerics.Quaternion.Dot(ToNumerics(a), ToNumerics(b))), 1f)) * 2f * 57.29578f;
	}

	public static Quaternion AngleAxis(float angle, Vector3 axis)
	{
		if (axis.sqrMagnitude <= 1E-05f)
		{
			return identity;
		}
		float angle2 = angle * ((float)Math.PI / 180f);
		return FromNumerics(System.Numerics.Quaternion.CreateFromAxisAngle(Vector3.ToNumerics(axis.normalized), angle2));
	}

	public static Quaternion Euler(Vector3 euler)
	{
		return Euler(euler.x, euler.y, euler.z);
	}

	public static Quaternion Euler(float x, float y, float z)
	{
		Quaternion quaternion = AngleAxis(x, Vector3.right);
		Quaternion quaternion2 = AngleAxis(y, Vector3.up);
		Quaternion quaternion3 = AngleAxis(z, Vector3.forward);
		return Normalize(quaternion2 * quaternion * quaternion3);
	}

	public static Quaternion FromToRotation(Vector3 fromDirection, Vector3 toDirection)
	{
		Vector3 normalized = fromDirection.normalized;
		Vector3 normalized2 = toDirection.normalized;
		float num = Vector3.Dot(normalized, normalized2);
		if (num >= 0.999999f)
		{
			return identity;
		}
		if (num <= -0.999999f)
		{
			Vector3 vector = Vector3.Cross(Vector3.right, normalized);
			if (vector.sqrMagnitude <= 1E-05f)
			{
				vector = Vector3.Cross(Vector3.up, normalized);
			}
			return AngleAxis(180f, vector.normalized);
		}
		Vector3 vector2 = Vector3.Cross(normalized, normalized2);
		float num2 = MathF.Sqrt((1f + num) * 2f);
		float num3 = 1f / num2;
		return Normalize(new Quaternion(vector2.x * num3, vector2.y * num3, vector2.z * num3, num2 * 0.5f));
	}

	private static float Wrap360(float degrees)
	{
		degrees %= 360f;
		if (!(degrees < 0f))
		{
			return degrees;
		}
		return degrees + 360f;
	}

	public static Quaternion Inverse(Quaternion rotation)
	{
		if (System.Numerics.Quaternion.Dot(ToNumerics(rotation), ToNumerics(rotation)) <= 1E-06f)
		{
			return identity;
		}
		return FromNumerics(System.Numerics.Quaternion.Inverse(ToNumerics(rotation)));
	}

	public static Quaternion Lerp(Quaternion a, Quaternion b, float t)
	{
		return FromNumerics(System.Numerics.Quaternion.Normalize(System.Numerics.Quaternion.Lerp(ToNumerics(a), ToNumerics(b), Mathf.Clamp01(t))));
	}

	public static Quaternion LookRotation(Vector3 forward)
	{
		return LookRotation(forward, Vector3.up);
	}

	public static Quaternion LookRotation(Vector3 forward, Vector3 upwards)
	{
		float num = 9.9999994E-11f;
		System.Numerics.Vector3 value = Vector3.ToNumerics(forward);
		if (value.LengthSquared() <= num)
		{
			return identity;
		}
		System.Numerics.Vector3 vector = System.Numerics.Vector3.Normalize(value);
		System.Numerics.Vector3 value2 = Vector3.ToNumerics(upwards);
		System.Numerics.Vector3 value3 = System.Numerics.Vector3.Cross((value2.LengthSquared() <= num) ? System.Numerics.Vector3.UnitY : System.Numerics.Vector3.Normalize(value2), vector);
		if (value3.LengthSquared() <= num)
		{
			value3 = System.Numerics.Vector3.Cross(System.Numerics.Vector3.UnitY, vector);
			if (value3.LengthSquared() <= num)
			{
				value3 = System.Numerics.Vector3.UnitX;
			}
		}
		value3 = System.Numerics.Vector3.Normalize(value3);
		System.Numerics.Vector3 vector2 = System.Numerics.Vector3.Cross(vector, value3);
		return FromNumerics(System.Numerics.Quaternion.CreateFromRotationMatrix(new System.Numerics.Matrix4x4(value3.X, value3.Y, value3.Z, 0f, vector2.X, vector2.Y, vector2.Z, 0f, vector.X, vector.Y, vector.Z, 0f, 0f, 0f, 0f, 1f)));
	}

	public static bool operator !=(Quaternion lhs, Quaternion rhs)
	{
		return !lhs.Equals(rhs);
	}

	public static Quaternion operator *(Quaternion lhs, Quaternion rhs)
	{
		return Normalize(FromNumerics(System.Numerics.Quaternion.Multiply(ToNumerics(lhs), ToNumerics(rhs))));
	}

	public static Vector3 operator *(Quaternion rotation, Vector3 point)
	{
		return Vector3.FromNumerics(System.Numerics.Vector3.Transform(Vector3.ToNumerics(point), ToNumerics(rotation)));
	}

	public void ToAngleAxis(out float angle, out Vector3 axis)
	{
		Quaternion quaternion = Normalize(this);
		angle = 2f * MathF.Acos(Math.Clamp(quaternion.w, -1f, 1f)) * 57.29578f;
		float num = MathF.Sqrt(MathF.Max(0f, 1f - quaternion.w * quaternion.w));
		axis = ((num < 1E-06f) ? Vector3.right : new Vector3(quaternion.x / num, quaternion.y / num, quaternion.z / num));
	}

	public override bool Equals(object obj)
	{
		if (obj is Quaternion other)
		{
			return Equals(other);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(x, y, z, w);
	}

	public override string ToString()
	{
		return $"({x}, {y}, {z}, {w})";
	}

	internal static System.Numerics.Quaternion ToNumerics(Quaternion q)
	{
		return new System.Numerics.Quaternion(q.x, q.y, q.z, q.w);
	}

	internal static Quaternion FromNumerics(System.Numerics.Quaternion q)
	{
		return new Quaternion(q.X, q.Y, q.Z, q.W);
	}

	public static float Dot(Quaternion a, Quaternion b)
	{
		return System.Numerics.Quaternion.Dot(ToNumerics(a), ToNumerics(b));
	}

	private static Quaternion Normalize(Quaternion q)
	{
		if (!(System.Numerics.Quaternion.Dot(ToNumerics(q), ToNumerics(q)) <= 1E-12f))
		{
			return FromNumerics(System.Numerics.Quaternion.Normalize(ToNumerics(q)));
		}
		return identity;
	}

	internal static Quaternion FromRotationMatrix(Matrix4x4 matrix)
	{
		return FromNumerics(System.Numerics.Quaternion.CreateFromRotationMatrix(System.Numerics.Matrix4x4.Transpose(Matrix4x4.ToNumerics(matrix))));
	}

	public static Quaternion Slerp(Quaternion a, Quaternion b, float t)
	{
		return SlerpUnclamped(a, b, Mathf.Clamp01(t));
	}

	public static Quaternion SlerpUnclamped(Quaternion a, Quaternion b, float t)
	{
		System.Numerics.Quaternion quaternion = System.Numerics.Quaternion.Slerp(new System.Numerics.Quaternion(a.x, a.y, a.z, a.w), new System.Numerics.Quaternion(b.x, b.y, b.z, b.w), t);
		return new Quaternion(quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);
	}
}
