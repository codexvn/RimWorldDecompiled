using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Waylith.Math;

public static class QuaternionExtensions
{
	[SpecialName]
	public sealed class _003CG_003E_0024EBAADAB256A7E03FD11C69C216F004C1
	{
		[SpecialName]
		public static class _003CM_003E_0024CCD21FE71D85B035D8E775D84F644780
		{
		}

		[SpecialName]
		public static class _003CM_003E_0024EBAADAB256A7E03FD11C69C216F004C1
		{
		}

		[ExtensionMarker("<M>$CCD21FE71D85B035D8E775D84F644780")]
		public Vector3 eulerAngles
		{
			[ExtensionMarker("<M>$CCD21FE71D85B035D8E775D84F644780")]
			get
			{
				throw new NotSupportedException();
			}
		}

		[ExtensionMarker("<M>$EBAADAB256A7E03FD11C69C216F004C1")]
		public static float kEpsilon
		{
			[ExtensionMarker("<M>$EBAADAB256A7E03FD11C69C216F004C1")]
			get
			{
				throw new NotSupportedException();
			}
		}

		[ExtensionMarker("<M>$EBAADAB256A7E03FD11C69C216F004C1")]
		public static Quaternion identity
		{
			[ExtensionMarker("<M>$EBAADAB256A7E03FD11C69C216F004C1")]
			get
			{
				throw new NotSupportedException();
			}
		}

		[ExtensionMarker("<M>$CCD21FE71D85B035D8E775D84F644780")]
		public void ToAngleAxis(out float angle, out Vector3 axis)
		{
			throw new NotSupportedException();
		}

		[ExtensionMarker("<M>$CCD21FE71D85B035D8E775D84F644780")]
		public string ToUnityString()
		{
			throw new NotSupportedException();
		}

		[ExtensionMarker("<M>$EBAADAB256A7E03FD11C69C216F004C1")]
		public static float Angle(Quaternion a, Quaternion b)
		{
			throw new NotSupportedException();
		}

		[ExtensionMarker("<M>$EBAADAB256A7E03FD11C69C216F004C1")]
		public static Quaternion AngleAxis(float angle, Vector3 axis)
		{
			throw new NotSupportedException();
		}

		[ExtensionMarker("<M>$EBAADAB256A7E03FD11C69C216F004C1")]
		public static Quaternion Euler(Vector3 euler)
		{
			throw new NotSupportedException();
		}

		[ExtensionMarker("<M>$EBAADAB256A7E03FD11C69C216F004C1")]
		public static Quaternion Euler(float x, float y, float z)
		{
			throw new NotSupportedException();
		}

		[ExtensionMarker("<M>$EBAADAB256A7E03FD11C69C216F004C1")]
		public static Quaternion FromToRotation(Vector3 fromDirection, Vector3 toDirection)
		{
			throw new NotSupportedException();
		}

		[ExtensionMarker("<M>$EBAADAB256A7E03FD11C69C216F004C1")]
		public static Quaternion LerpClamped(Quaternion a, Quaternion b, float t)
		{
			throw new NotSupportedException();
		}

		[ExtensionMarker("<M>$EBAADAB256A7E03FD11C69C216F004C1")]
		public static Quaternion SlerpClamped(Quaternion a, Quaternion b, float t)
		{
			throw new NotSupportedException();
		}

		[ExtensionMarker("<M>$EBAADAB256A7E03FD11C69C216F004C1")]
		public static Quaternion LookRotation(Vector3 forward)
		{
			throw new NotSupportedException();
		}

		[ExtensionMarker("<M>$EBAADAB256A7E03FD11C69C216F004C1")]
		public static Quaternion LookRotation(Vector3 forward, Vector3 upwards)
		{
			throw new NotSupportedException();
		}

		[ExtensionMarker("<M>$EBAADAB256A7E03FD11C69C216F004C1")]
		public static Quaternion FromRotationMatrix(Matrix4x4 matrix)
		{
			throw new NotSupportedException();
		}

		[ExtensionMarker("<M>$EBAADAB256A7E03FD11C69C216F004C1")]
		public static Vector3 operator *(Quaternion rotation, Vector3 point)
		{
			throw new NotSupportedException();
		}
	}

	public static Vector3 get_eulerAngles(Quaternion q)
	{
		Quaternion quaternion2 = NormalizeWithMagnitudeClamp(q);
		float num = 2f * (quaternion2.X * quaternion2.W - quaternion2.Y * quaternion2.Z);
		float num2;
		float num3;
		float num4;
		if (MathF.Abs(num) < 0.9999995f)
		{
			num2 = MathF.Asin(System.Math.Clamp(num, -1f, 1f));
			num3 = MathF.Atan2(2f * (quaternion2.X * quaternion2.Z + quaternion2.Y * quaternion2.W), 1f - 2f * (quaternion2.X * quaternion2.X + quaternion2.Y * quaternion2.Y));
			num4 = MathF.Atan2(2f * (quaternion2.X * quaternion2.Y + quaternion2.Z * quaternion2.W), 1f - 2f * (quaternion2.X * quaternion2.X + quaternion2.Z * quaternion2.Z));
		}
		else
		{
			num2 = MathF.CopySign((float)System.Math.PI / 2f, num);
			float num5 = 2f * (quaternion2.X * quaternion2.Y - quaternion2.Z * quaternion2.W);
			float x = 1f - 2f * (quaternion2.Y * quaternion2.Y + quaternion2.Z * quaternion2.Z);
			num3 = MathF.Atan2((num > 0f) ? num5 : (0f - num5), x);
			num4 = 0f;
		}
		return new Vector3(Wrap360(num2 * 57.29578f), Wrap360(num3 * 57.29578f), Wrap360(num4 * 57.29578f));
	}

	public static void ToAngleAxis(this Quaternion q, out float angle, out Vector3 axis)
	{
		Quaternion quaternion2 = NormalizeWithMagnitudeClamp(q);
		angle = 2f * MathF.Acos(System.Math.Clamp(quaternion2.W, -1f, 1f)) * 57.29578f;
		float num = MathF.Sqrt(MathF.Max(0f, 1f - quaternion2.W * quaternion2.W));
		axis = ((num < 1E-06f) ? Vector3Extensions.get_right() : new Vector3(quaternion2.X / num, quaternion2.Y / num, quaternion2.Z / num));
	}

	public static string ToUnityString(this Quaternion q)
	{
		return $"({q.X}, {q.Y}, {q.Z}, {q.W})";
	}

	public static float get_kEpsilon()
	{
		return 1E-06f;
	}

	public static Quaternion get_identity()
	{
		return new Quaternion(0f, 0f, 0f, 1f);
	}

	public static float Angle(Quaternion a, Quaternion b)
	{
		return MathF.Acos(MathF.Min(MathF.Abs(Quaternion.Dot(a, b)), 1f)) * 2f * 57.29578f;
	}

	public static Quaternion AngleAxis(float angle, Vector3 axis)
	{
		if (axis.LengthSquared() <= Vector3Extensions.get_kEpsilon())
		{
			return get_identity();
		}
		float angle2 = angle * ((float)System.Math.PI / 180f);
		return Quaternion.CreateFromAxisAngle(Vector3Extensions.get_normalized(axis), angle2);
	}

	public static Quaternion Euler(Vector3 euler)
	{
		return Euler(euler.X, euler.Y, euler.Z);
	}

	public static Quaternion Euler(float x, float y, float z)
	{
		Quaternion rhs = AngleAxis(x, Vector3Extensions.get_right());
		Quaternion lhs = AngleAxis(y, Vector3Extensions.get_up());
		return NormalizeWithMagnitudeClamp(NormalizedMultiply(rhs: AngleAxis(z, Vector3Extensions.get_forward()), lhs: NormalizedMultiply(lhs, rhs)));
	}

	public static Quaternion FromToRotation(Vector3 fromDirection, Vector3 toDirection)
	{
		Vector3 vector = Vector3Extensions.get_normalized(fromDirection);
		Vector3 vector2 = Vector3Extensions.get_normalized(toDirection);
		float num = Vector3.Dot(vector, vector2);
		if (num >= 1f - get_kEpsilon())
		{
			return get_identity();
		}
		if (num <= -1f + get_kEpsilon())
		{
			Vector3 v = Vector3.Cross(Vector3Extensions.get_right(), vector);
			if (v.LengthSquared() <= Vector3Extensions.get_kEpsilon())
			{
				v = Vector3.Cross(Vector3Extensions.get_up(), vector);
			}
			return AngleAxis(180f, Vector3Extensions.get_normalized(v));
		}
		Vector3 vector3 = Vector3.Cross(vector, vector2);
		float num2 = MathF.Sqrt((1f + num) * 2f);
		float num3 = 1f / num2;
		return NormalizeWithMagnitudeClamp(new Quaternion(vector3.X * num3, vector3.Y * num3, vector3.Z * num3, num2 * 0.5f));
	}

	public static Quaternion LerpClamped(Quaternion a, Quaternion b, float t)
	{
		return Quaternion.Normalize(Quaternion.Lerp(a, b, Mathf.Clamp01(t)));
	}

	public static Quaternion SlerpClamped(Quaternion a, Quaternion b, float t)
	{
		return Quaternion.Normalize(Quaternion.Slerp(a, b, Mathf.Clamp01(t)));
	}

	public static Quaternion LookRotation(Vector3 forward)
	{
		return LookRotation(forward, Vector3Extensions.get_up());
	}

	public static Quaternion LookRotation(Vector3 forward, Vector3 upwards)
	{
		float num = Vector3Extensions.get_kEpsilon() * Vector3Extensions.get_kEpsilon();
		if (forward.LengthSquared() <= num)
		{
			return get_identity();
		}
		Vector3 vector = Vector3.Normalize(forward);
		Vector3 value = Vector3.Cross((upwards.LengthSquared() <= num) ? Vector3.UnitY : Vector3.Normalize(upwards), vector);
		if (value.LengthSquared() <= num)
		{
			value = Vector3.Cross(Vector3.UnitY, vector);
			if (value.LengthSquared() <= num)
			{
				value = Vector3.UnitX;
			}
		}
		value = Vector3.Normalize(value);
		Vector3 vector2 = Vector3.Cross(vector, value);
		return Quaternion.CreateFromRotationMatrix(new Matrix4x4(value.X, value.Y, value.Z, 0f, vector2.X, vector2.Y, vector2.Z, 0f, vector.X, vector.Y, vector.Z, 0f, 0f, 0f, 0f, 1f));
	}

	public static Quaternion FromRotationMatrix(Matrix4x4 matrix)
	{
		return Quaternion.CreateFromRotationMatrix(Matrix4x4.Transpose(matrix));
	}

	public static Vector3 op_Multiply(Quaternion rotation, Vector3 point)
	{
		return Vector3.Transform(point, rotation);
	}

	private static Quaternion NormalizedMultiply(Quaternion lhs, Quaternion rhs)
	{
		return NormalizeWithMagnitudeClamp(lhs * rhs);
	}

	private static Quaternion NormalizeWithMagnitudeClamp(Quaternion q)
	{
		if (!(Quaternion.Dot(q, q) <= get_kEpsilon() * get_kEpsilon()))
		{
			return Quaternion.Normalize(q);
		}
		return get_identity();
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
}
