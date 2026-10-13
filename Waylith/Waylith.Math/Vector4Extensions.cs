using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Waylith.Math;

public static class Vector4Extensions
{
	[SpecialName]
	public sealed class _003CG_003E_002495E7E528B39EE5DF25E3F4A457C85A99
	{
		[SpecialName]
		public static class _003CM_003E_0024C1511917066858479A7E1200069E9D6A
		{
		}

		[SpecialName]
		public static class _003CM_003E_00248D3F554A1EA9DE796D3225BA5E077EE0
		{
		}

		[SpecialName]
		public static class _003CM_003E_002495E7E528B39EE5DF25E3F4A457C85A99
		{
		}

		[ExtensionMarker("<M>$C1511917066858479A7E1200069E9D6A")]
		public Vector4 normalized
		{
			[ExtensionMarker("<M>$C1511917066858479A7E1200069E9D6A")]
			get
			{
				throw new NotSupportedException();
			}
		}

		[ExtensionMarker("<M>$95E7E528B39EE5DF25E3F4A457C85A99")]
		public static float kEpsilon
		{
			[ExtensionMarker("<M>$95E7E528B39EE5DF25E3F4A457C85A99")]
			get
			{
				throw new NotSupportedException();
			}
		}

		[ExtensionMarker("<M>$95E7E528B39EE5DF25E3F4A457C85A99")]
		public static Vector4 zero
		{
			[ExtensionMarker("<M>$95E7E528B39EE5DF25E3F4A457C85A99")]
			get
			{
				throw new NotSupportedException();
			}
		}

		[ExtensionMarker("<M>$95E7E528B39EE5DF25E3F4A457C85A99")]
		public static Vector4 one
		{
			[ExtensionMarker("<M>$95E7E528B39EE5DF25E3F4A457C85A99")]
			get
			{
				throw new NotSupportedException();
			}
		}

		[ExtensionMarker("<M>$C1511917066858479A7E1200069E9D6A")]
		public string ToUnityString()
		{
			throw new NotSupportedException();
		}

		[ExtensionMarker("<M>$C1511917066858479A7E1200069E9D6A")]
		public Vector2 ToVector2()
		{
			throw new NotSupportedException();
		}

		[ExtensionMarker("<M>$C1511917066858479A7E1200069E9D6A")]
		public Vector3 ToVector3()
		{
			throw new NotSupportedException();
		}

		[ExtensionMarker("<M>$8D3F554A1EA9DE796D3225BA5E077EE0")]
		public void Normalize()
		{
			throw new NotSupportedException();
		}

		[ExtensionMarker("<M>$8D3F554A1EA9DE796D3225BA5E077EE0")]
		public void Scale(Vector4 scale)
		{
			throw new NotSupportedException();
		}

		[ExtensionMarker("<M>$95E7E528B39EE5DF25E3F4A457C85A99")]
		public static Vector4 LerpClamped(Vector4 a, Vector4 b, float t)
		{
			throw new NotSupportedException();
		}
	}

	public static Vector4 get_normalized(Vector4 v)
	{
		if (!(v.LengthSquared() <= get_kEpsilon() * get_kEpsilon()))
		{
			return Vector4.Normalize(v);
		}
		return Vector4.Zero;
	}

	public static string ToUnityString(this Vector4 v)
	{
		return $"({v.X}, {v.Y}, {v.Z}, {v.W})";
	}

	public static Vector2 ToVector2(this Vector4 v)
	{
		return new Vector2(v.X, v.Y);
	}

	public static Vector3 ToVector3(this Vector4 v)
	{
		return new Vector3(v.X, v.Y, v.Z);
	}

	public static void Normalize(this ref Vector4 v)
	{
		v = get_normalized(v);
	}

	public static void Scale(this ref Vector4 v, Vector4 scale)
	{
		v *= scale;
	}

	public static float get_kEpsilon()
	{
		return 1E-05f;
	}

	public static Vector4 get_zero()
	{
		return new Vector4(0f, 0f, 0f, 0f);
	}

	public static Vector4 get_one()
	{
		return new Vector4(1f, 1f, 1f, 1f);
	}

	public static Vector4 LerpClamped(Vector4 a, Vector4 b, float t)
	{
		return Vector4.Lerp(a, b, System.Math.Clamp(t, 0f, 1f));
	}
}
