using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Waylith.Math;

public static class Matrix4x4Extensions
{
	[SpecialName]
	public sealed class _003CG_003E_002466BD7A15C6E26570B0C01F1DAE028D22
	{
		[SpecialName]
		public static class _003CM_003E_002471C40E2E36B6E47C4397D85F1B18A3E9
		{
		}

		[SpecialName]
		public static class _003CM_003E_00243C808A77E77D8DAC9BF6D2888247626C
		{
		}

		[SpecialName]
		public static class _003CM_003E_002466BD7A15C6E26570B0C01F1DAE028D22
		{
		}

		[ExtensionMarker("<M>$71C40E2E36B6E47C4397D85F1B18A3E9")]
		public Vector3 lossyScale
		{
			[ExtensionMarker("<M>$71C40E2E36B6E47C4397D85F1B18A3E9")]
			get
			{
				throw new NotSupportedException();
			}
		}

		[ExtensionMarker("<M>$66BD7A15C6E26570B0C01F1DAE028D22")]
		public static Matrix4x4 zero
		{
			[ExtensionMarker("<M>$66BD7A15C6E26570B0C01F1DAE028D22")]
			get
			{
				throw new NotSupportedException();
			}
		}

		[ExtensionMarker("<M>$71C40E2E36B6E47C4397D85F1B18A3E9")]
		public Vector3 GetPosition()
		{
			throw new NotSupportedException();
		}

		[ExtensionMarker("<M>$71C40E2E36B6E47C4397D85F1B18A3E9")]
		public float GetElement(int row, int column)
		{
			throw new NotSupportedException();
		}

		[ExtensionMarker("<M>$71C40E2E36B6E47C4397D85F1B18A3E9")]
		public float GetElement(int index)
		{
			throw new NotSupportedException();
		}

		[ExtensionMarker("<M>$71C40E2E36B6E47C4397D85F1B18A3E9")]
		public Vector3 MultiplyPoint(Vector3 point)
		{
			throw new NotSupportedException();
		}

		[ExtensionMarker("<M>$71C40E2E36B6E47C4397D85F1B18A3E9")]
		public Vector3 MultiplyPoint3x4(Vector3 point)
		{
			throw new NotSupportedException();
		}

		[ExtensionMarker("<M>$71C40E2E36B6E47C4397D85F1B18A3E9")]
		public Vector3 MultiplyVector(Vector3 vector)
		{
			throw new NotSupportedException();
		}

		[ExtensionMarker("<M>$3C808A77E77D8DAC9BF6D2888247626C")]
		public void SetElement(int index, float value)
		{
			throw new NotSupportedException();
		}

		[ExtensionMarker("<M>$3C808A77E77D8DAC9BF6D2888247626C")]
		public void SetElement(int row, int column, float value)
		{
			throw new NotSupportedException();
		}

		[ExtensionMarker("<M>$3C808A77E77D8DAC9BF6D2888247626C")]
		public void SetTRS(Vector3 pos, Quaternion q, Vector3 s)
		{
			throw new NotSupportedException();
		}

		[ExtensionMarker("<M>$66BD7A15C6E26570B0C01F1DAE028D22")]
		public static Matrix4x4 FromColumns(Vector4 column0, Vector4 column1, Vector4 column2, Vector4 column3)
		{
			throw new NotSupportedException();
		}

		[ExtensionMarker("<M>$66BD7A15C6E26570B0C01F1DAE028D22")]
		public static Matrix4x4 Inverse(Matrix4x4 m)
		{
			throw new NotSupportedException();
		}

		[ExtensionMarker("<M>$66BD7A15C6E26570B0C01F1DAE028D22")]
		public static Vector4 operator *(Matrix4x4 lhs, Vector4 vector)
		{
			throw new NotSupportedException();
		}

		[ExtensionMarker("<M>$66BD7A15C6E26570B0C01F1DAE028D22")]
		public static Matrix4x4 Ortho(float left, float right, float bottom, float top, float zNear, float zFar)
		{
			throw new NotSupportedException();
		}

		[ExtensionMarker("<M>$66BD7A15C6E26570B0C01F1DAE028D22")]
		public static Matrix4x4 Rotate(Quaternion q)
		{
			throw new NotSupportedException();
		}

		[ExtensionMarker("<M>$66BD7A15C6E26570B0C01F1DAE028D22")]
		public static Matrix4x4 Scale(Vector3 vector)
		{
			throw new NotSupportedException();
		}

		[ExtensionMarker("<M>$66BD7A15C6E26570B0C01F1DAE028D22")]
		public static Matrix4x4 Translate(Vector3 vector)
		{
			throw new NotSupportedException();
		}

		[ExtensionMarker("<M>$66BD7A15C6E26570B0C01F1DAE028D22")]
		public static Matrix4x4 TRS(Vector3 pos, Quaternion q, Vector3 s)
		{
			throw new NotSupportedException();
		}

		[ExtensionMarker("<M>$66BD7A15C6E26570B0C01F1DAE028D22")]
		public static Matrix4x4 Perspective(float fovYDegrees, float aspect, float zNear, float zFar)
		{
			throw new NotSupportedException();
		}
	}

	public static Vector3 get_lossyScale(Matrix4x4 m)
	{
		return new Vector3(new Vector3(m.M11, m.M21, m.M31).Length(), new Vector3(m.M12, m.M22, m.M32).Length(), new Vector3(m.M13, m.M23, m.M33).Length());
	}

	public static Vector3 GetPosition(this Matrix4x4 m)
	{
		return new Vector3(m.M14, m.M24, m.M34);
	}

	public static float GetElement(this Matrix4x4 m, int row, int column)
	{
		return m.GetElement(row * 4 + column);
	}

	public static float GetElement(this Matrix4x4 m, int index)
	{
		return index switch
		{
			0 => m.M11, 
			1 => m.M12, 
			2 => m.M13, 
			3 => m.M14, 
			4 => m.M21, 
			5 => m.M22, 
			6 => m.M23, 
			7 => m.M24, 
			8 => m.M31, 
			9 => m.M32, 
			10 => m.M33, 
			11 => m.M34, 
			12 => m.M41, 
			13 => m.M42, 
			14 => m.M43, 
			15 => m.M44, 
			_ => throw new IndexOutOfRangeException("index"), 
		};
	}

	public static Vector3 MultiplyPoint(this Matrix4x4 m, Vector3 point)
	{
		Vector4 vector = m * new Vector4(point.X, point.Y, point.Z, 1f);
		if (MathF.Abs(vector.W) > 1E-06f)
		{
			return new Vector3(vector.X / vector.W, vector.Y / vector.W, vector.Z / vector.W);
		}
		return new Vector3(vector.X, vector.Y, vector.Z);
	}

	public static Vector3 MultiplyPoint3x4(this Matrix4x4 m, Vector3 point)
	{
		Vector4 vector = Vector4.Transform(new Vector4(point.X, point.Y, point.Z, 1f), Matrix4x4.Transpose(m));
		return new Vector3(vector.X, vector.Y, vector.Z);
	}

	public static Vector3 MultiplyVector(this Matrix4x4 m, Vector3 vector)
	{
		Vector4 vector2 = Vector4.Transform(new Vector4(vector.X, vector.Y, vector.Z, 0f), Matrix4x4.Transpose(m));
		return new Vector3(vector2.X, vector2.Y, vector2.Z);
	}

	public static void SetElement(this ref Matrix4x4 m, int index, float value)
	{
		switch (index)
		{
		case 0:
			m.M11 = value;
			break;
		case 1:
			m.M12 = value;
			break;
		case 2:
			m.M13 = value;
			break;
		case 3:
			m.M14 = value;
			break;
		case 4:
			m.M21 = value;
			break;
		case 5:
			m.M22 = value;
			break;
		case 6:
			m.M23 = value;
			break;
		case 7:
			m.M24 = value;
			break;
		case 8:
			m.M31 = value;
			break;
		case 9:
			m.M32 = value;
			break;
		case 10:
			m.M33 = value;
			break;
		case 11:
			m.M34 = value;
			break;
		case 12:
			m.M41 = value;
			break;
		case 13:
			m.M42 = value;
			break;
		case 14:
			m.M43 = value;
			break;
		case 15:
			m.M44 = value;
			break;
		default:
			throw new IndexOutOfRangeException("index");
		}
	}

	public static void SetElement(this ref Matrix4x4 m, int row, int column, float value)
	{
		m.SetElement(row * 4 + column, value);
	}

	public static void SetTRS(this ref Matrix4x4 m, Vector3 pos, Quaternion q, Vector3 s)
	{
		m = TRS(pos, q, s);
	}

	public static Matrix4x4 get_zero()
	{
		return default(Matrix4x4);
	}

	public static Matrix4x4 FromColumns(Vector4 column0, Vector4 column1, Vector4 column2, Vector4 column3)
	{
		return new Matrix4x4(column0.X, column1.X, column2.X, column3.X, column0.Y, column1.Y, column2.Y, column3.Y, column0.Z, column1.Z, column2.Z, column3.Z, column0.W, column1.W, column2.W, column3.W);
	}

	public static Matrix4x4 Inverse(Matrix4x4 m)
	{
		Matrix4x4.Invert(m, out var result);
		return result;
	}

	public static Vector4 op_Multiply(Matrix4x4 lhs, Vector4 vector)
	{
		return Vector4.Transform(vector, Matrix4x4.Transpose(lhs));
	}

	public static Matrix4x4 Ortho(float left, float right, float bottom, float top, float zNear, float zFar)
	{
		Matrix4x4 identity = Matrix4x4.Identity;
		identity.M11 = 2f / (right - left);
		identity.M22 = -2f / (top - bottom);
		identity.M33 = 1f / (zFar - zNear);
		identity.M14 = (0f - (right + left)) / (right - left);
		identity.M24 = (top + bottom) / (top - bottom);
		identity.M34 = zFar / (zFar - zNear);
		return identity;
	}

	public static Matrix4x4 Rotate(Quaternion q)
	{
		if (Quaternion.Dot(q, q) <= 1E-12f)
		{
			return Matrix4x4.Identity;
		}
		return Matrix4x4.Transpose(Matrix4x4.CreateFromQuaternion(Quaternion.Normalize(q)));
	}

	public static Matrix4x4 Scale(Vector3 vector)
	{
		Matrix4x4 result = default(Matrix4x4);
		result.M11 = vector.X;
		result.M22 = vector.Y;
		result.M33 = vector.Z;
		result.M44 = 1f;
		return result;
	}

	public static Matrix4x4 Translate(Vector3 vector)
	{
		Matrix4x4 identity = Matrix4x4.Identity;
		identity.M14 = vector.X;
		identity.M24 = vector.Y;
		identity.M34 = vector.Z;
		return identity;
	}

	public static Matrix4x4 TRS(Vector3 pos, Quaternion q, Vector3 s)
	{
		Matrix4x4 result = Rotate(q);
		result.M11 *= s.X;
		result.M21 *= s.X;
		result.M31 *= s.X;
		result.M12 *= s.Y;
		result.M22 *= s.Y;
		result.M32 *= s.Y;
		result.M13 *= s.Z;
		result.M23 *= s.Z;
		result.M33 *= s.Z;
		result.M14 = pos.X;
		result.M24 = pos.Y;
		result.M34 = pos.Z;
		return result;
	}

	public static Matrix4x4 Perspective(float fovYDegrees, float aspect, float zNear, float zFar)
	{
		float num = 1f / MathF.Tan(0.5f * fovYDegrees * ((float)System.Math.PI / 180f));
		Matrix4x4 result = default(Matrix4x4);
		result.M11 = num / System.Math.Max(1E-06f, aspect);
		result.M22 = 0f - num;
		result.M33 = zNear / (zFar - zNear);
		result.M34 = zFar * zNear / (zFar - zNear);
		result.M43 = -1f;
		return result;
	}
}
