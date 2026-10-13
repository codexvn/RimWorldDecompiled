using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Waylith.Math;

public static class Vector2Extensions
{
	[SpecialName]
	public sealed class _003CG_003E_002431F76E721F56097EE9FE33FA9ECF3447
	{
		[SpecialName]
		public static class _003CM_003E_0024640F6DDBE3C46CF98F2A8C08F00F6EC1
		{
		}

		[SpecialName]
		public static class _003CM_003E_00241D461CAC6A6C78874D145E53DE67B450
		{
		}

		[SpecialName]
		public static class _003CM_003E_002431F76E721F56097EE9FE33FA9ECF3447
		{
		}

		[ExtensionMarker("<M>$640F6DDBE3C46CF98F2A8C08F00F6EC1")]
		public Vector2 normalized
		{
			[ExtensionMarker("<M>$640F6DDBE3C46CF98F2A8C08F00F6EC1")]
			get
			{
				throw new NotSupportedException();
			}
		}

		[ExtensionMarker("<M>$31F76E721F56097EE9FE33FA9ECF3447")]
		public static float kEpsilon
		{
			[ExtensionMarker("<M>$31F76E721F56097EE9FE33FA9ECF3447")]
			get
			{
				throw new NotSupportedException();
			}
		}

		[ExtensionMarker("<M>$31F76E721F56097EE9FE33FA9ECF3447")]
		public static float kEpsilonNormalSqrt
		{
			[ExtensionMarker("<M>$31F76E721F56097EE9FE33FA9ECF3447")]
			get
			{
				throw new NotSupportedException();
			}
		}

		[ExtensionMarker("<M>$31F76E721F56097EE9FE33FA9ECF3447")]
		public static Vector2 zero
		{
			[ExtensionMarker("<M>$31F76E721F56097EE9FE33FA9ECF3447")]
			get
			{
				throw new NotSupportedException();
			}
		}

		[ExtensionMarker("<M>$31F76E721F56097EE9FE33FA9ECF3447")]
		public static Vector2 one
		{
			[ExtensionMarker("<M>$31F76E721F56097EE9FE33FA9ECF3447")]
			get
			{
				throw new NotSupportedException();
			}
		}

		[ExtensionMarker("<M>$31F76E721F56097EE9FE33FA9ECF3447")]
		public static Vector2 up
		{
			[ExtensionMarker("<M>$31F76E721F56097EE9FE33FA9ECF3447")]
			get
			{
				throw new NotSupportedException();
			}
		}

		[ExtensionMarker("<M>$31F76E721F56097EE9FE33FA9ECF3447")]
		public static Vector2 right
		{
			[ExtensionMarker("<M>$31F76E721F56097EE9FE33FA9ECF3447")]
			get
			{
				throw new NotSupportedException();
			}
		}

		[ExtensionMarker("<M>$31F76E721F56097EE9FE33FA9ECF3447")]
		public static Vector2 left
		{
			[ExtensionMarker("<M>$31F76E721F56097EE9FE33FA9ECF3447")]
			get
			{
				throw new NotSupportedException();
			}
		}

		[ExtensionMarker("<M>$31F76E721F56097EE9FE33FA9ECF3447")]
		public static Vector2 down
		{
			[ExtensionMarker("<M>$31F76E721F56097EE9FE33FA9ECF3447")]
			get
			{
				throw new NotSupportedException();
			}
		}

		[ExtensionMarker("<M>$31F76E721F56097EE9FE33FA9ECF3447")]
		public static Vector2 negativeInfinity
		{
			[ExtensionMarker("<M>$31F76E721F56097EE9FE33FA9ECF3447")]
			get
			{
				throw new NotSupportedException();
			}
		}

		[ExtensionMarker("<M>$31F76E721F56097EE9FE33FA9ECF3447")]
		public static Vector2 positiveInfinity
		{
			[ExtensionMarker("<M>$31F76E721F56097EE9FE33FA9ECF3447")]
			get
			{
				throw new NotSupportedException();
			}
		}

		[ExtensionMarker("<M>$640F6DDBE3C46CF98F2A8C08F00F6EC1")]
		public float SqrMagnitude()
		{
			throw new NotSupportedException();
		}

		[ExtensionMarker("<M>$640F6DDBE3C46CF98F2A8C08F00F6EC1")]
		public Vector3 ToVector3()
		{
			throw new NotSupportedException();
		}

		[ExtensionMarker("<M>$640F6DDBE3C46CF98F2A8C08F00F6EC1")]
		public Vector4 ToVector4()
		{
			throw new NotSupportedException();
		}

		[ExtensionMarker("<M>$1D461CAC6A6C78874D145E53DE67B450")]
		public void Normalize()
		{
			throw new NotSupportedException();
		}

		[ExtensionMarker("<M>$1D461CAC6A6C78874D145E53DE67B450")]
		public void Scale(Vector2 scale)
		{
			throw new NotSupportedException();
		}

		[ExtensionMarker("<M>$31F76E721F56097EE9FE33FA9ECF3447")]
		public static Vector2 LerpClamped(Vector2 a, Vector2 b, float t)
		{
			throw new NotSupportedException();
		}
	}

	public static Vector2 get_normalized(Vector2 v)
	{
		float num = v.Length();
		if (!(num > get_kEpsilon()))
		{
			return Vector2.Zero;
		}
		return v / num;
	}

	public static float SqrMagnitude(this Vector2 v)
	{
		return v.LengthSquared();
	}

	public static Vector3 ToVector3(this Vector2 v)
	{
		return new Vector3(v, 0f);
	}

	public static Vector4 ToVector4(this Vector2 v)
	{
		return new Vector4(v, 0f, 0f);
	}

	public static void Normalize(this ref Vector2 v)
	{
		v = get_normalized(v);
	}

	public static void Scale(this ref Vector2 v, Vector2 scale)
	{
		v *= scale;
	}

	public static float get_kEpsilon()
	{
		return 1E-05f;
	}

	public static float get_kEpsilonNormalSqrt()
	{
		return 1E-15f;
	}

	public static Vector2 get_zero()
	{
		return new Vector2(0f, 0f);
	}

	public static Vector2 get_one()
	{
		return new Vector2(1f, 1f);
	}

	public static Vector2 get_up()
	{
		return new Vector2(0f, 1f);
	}

	public static Vector2 get_right()
	{
		return new Vector2(1f, 0f);
	}

	public static Vector2 get_left()
	{
		return new Vector2(-1f, 0f);
	}

	public static Vector2 get_down()
	{
		return new Vector2(0f, -1f);
	}

	public static Vector2 get_negativeInfinity()
	{
		return new Vector2(float.NegativeInfinity, float.NegativeInfinity);
	}

	public static Vector2 get_positiveInfinity()
	{
		return new Vector2(float.PositiveInfinity, float.PositiveInfinity);
	}

	public static Vector2 LerpClamped(Vector2 a, Vector2 b, float t)
	{
		return Vector2.Lerp(a, b, System.Math.Clamp(t, 0f, 1f));
	}
}
