using System;
using System.Globalization;
using System.Numerics;

namespace UnityEngine;

public struct Matrix4x4 : IEquatable<Matrix4x4>, IFormattable
{
	public float m00;

	public float m01;

	public float m02;

	public float m03;

	public float m10;

	public float m11;

	public float m12;

	public float m13;

	public float m20;

	public float m21;

	public float m22;

	public float m23;

	public float m30;

	public float m31;

	public float m32;

	public float m33;

	private System.Numerics.Matrix4x4 _value
	{
		readonly get
		{
			return new System.Numerics.Matrix4x4(m00, m01, m02, m03, m10, m11, m12, m13, m20, m21, m22, m23, m30, m31, m32, m33);
		}
		set
		{
			m00 = value.M11;
			m01 = value.M12;
			m02 = value.M13;
			m03 = value.M14;
			m10 = value.M21;
			m11 = value.M22;
			m12 = value.M23;
			m13 = value.M24;
			m20 = value.M31;
			m21 = value.M32;
			m22 = value.M33;
			m23 = value.M34;
			m30 = value.M41;
			m31 = value.M42;
			m32 = value.M43;
			m33 = value.M44;
		}
	}

	public readonly Quaternion rotation
	{
		get
		{
			Vector3 vector = new Vector3(m00, m10, m20);
			Vector3 vector2 = new Vector3(m01, m11, m21);
			Vector3 vector3 = new Vector3(m02, m12, m22);
			if (vector2.sqrMagnitude < 1E-12f)
			{
				vector2 = Vector3.Cross(vector3, vector);
			}
			if (vector3.sqrMagnitude < 1E-12f)
			{
				vector3 = Vector3.Cross(vector, vector2);
			}
			return Quaternion.LookRotation(vector3, vector2);
		}
	}

	public static Matrix4x4 identity
	{
		get
		{
			Matrix4x4 result = default(Matrix4x4);
			result._value = System.Numerics.Matrix4x4.Identity;
			return result;
		}
	}

	public Matrix4x4 inverse => Inverse(this);

	public float this[int row, int column]
	{
		get
		{
			return this[row * 4 + column];
		}
		set
		{
			this[row * 4 + column] = value;
		}
	}

	public Vector3 lossyScale => new Vector3(new Vector3(m00, m10, m20).magnitude, new Vector3(m01, m11, m21).magnitude, new Vector3(m02, m12, m22).magnitude);

	public Matrix4x4 transpose => FromNumerics(System.Numerics.Matrix4x4.Transpose(_value));

	public static Matrix4x4 zero => default(Matrix4x4);

	public readonly bool isIdentity => _value.IsIdentity;

	public float this[int index]
	{
		get
		{
			return index switch
			{
				0 => m00, 
				1 => m01, 
				2 => m02, 
				3 => m03, 
				4 => m10, 
				5 => m11, 
				6 => m12, 
				7 => m13, 
				8 => m20, 
				9 => m21, 
				10 => m22, 
				11 => m23, 
				12 => m30, 
				13 => m31, 
				14 => m32, 
				15 => m33, 
				_ => throw new IndexOutOfRangeException("index"), 
			};
		}
		set
		{
			switch (index)
			{
			case 0:
				m00 = value;
				break;
			case 1:
				m01 = value;
				break;
			case 2:
				m02 = value;
				break;
			case 3:
				m03 = value;
				break;
			case 4:
				m10 = value;
				break;
			case 5:
				m11 = value;
				break;
			case 6:
				m12 = value;
				break;
			case 7:
				m13 = value;
				break;
			case 8:
				m20 = value;
				break;
			case 9:
				m21 = value;
				break;
			case 10:
				m22 = value;
				break;
			case 11:
				m23 = value;
				break;
			case 12:
				m30 = value;
				break;
			case 13:
				m31 = value;
				break;
			case 14:
				m32 = value;
				break;
			case 15:
				m33 = value;
				break;
			default:
				throw new IndexOutOfRangeException("index");
			}
		}
	}

	public Vector4 GetColumn(int index)
	{
		return index switch
		{
			0 => new Vector4(m00, m10, m20, m30), 
			1 => new Vector4(m01, m11, m21, m31), 
			2 => new Vector4(m02, m12, m22, m32), 
			_ => new Vector4(m03, m13, m23, m33), 
		};
	}

	public void SetColumn(int index, Vector4 column)
	{
		switch (index)
		{
		case 0:
			m00 = column.x;
			m10 = column.y;
			m20 = column.z;
			m30 = column.w;
			break;
		case 1:
			m01 = column.x;
			m11 = column.y;
			m21 = column.z;
			m31 = column.w;
			break;
		case 2:
			m02 = column.x;
			m12 = column.y;
			m22 = column.z;
			m32 = column.w;
			break;
		default:
			m03 = column.x;
			m13 = column.y;
			m23 = column.z;
			m33 = column.w;
			break;
		}
	}

	public Vector4 GetRow(int index)
	{
		return index switch
		{
			0 => new Vector4(m00, m01, m02, m03), 
			1 => new Vector4(m10, m11, m12, m13), 
			2 => new Vector4(m20, m21, m22, m23), 
			_ => new Vector4(m30, m31, m32, m33), 
		};
	}

	public void SetRow(int index, Vector4 row)
	{
		switch (index)
		{
		case 0:
			m00 = row.x;
			m01 = row.y;
			m02 = row.z;
			m03 = row.w;
			break;
		case 1:
			m10 = row.x;
			m11 = row.y;
			m12 = row.z;
			m13 = row.w;
			break;
		case 2:
			m20 = row.x;
			m21 = row.y;
			m22 = row.z;
			m23 = row.w;
			break;
		default:
			m30 = row.x;
			m31 = row.y;
			m32 = row.z;
			m33 = row.w;
			break;
		}
	}

	public static bool operator ==(Matrix4x4 lhs, Matrix4x4 rhs)
	{
		return lhs.Equals(rhs);
	}

	public static bool operator !=(Matrix4x4 lhs, Matrix4x4 rhs)
	{
		return !lhs.Equals(rhs);
	}

	public bool Equals(Matrix4x4 other)
	{
		return _value.Equals(other._value);
	}

	public string ToString(string format, IFormatProvider formatProvider)
	{
		return $"[{m00.ToString(format, formatProvider)}, {m01.ToString(format, formatProvider)}, {m02.ToString(format, formatProvider)}, {m03.ToString(format, formatProvider)}; {m10.ToString(format, formatProvider)}, {m11.ToString(format, formatProvider)}, {m12.ToString(format, formatProvider)}, {m13.ToString(format, formatProvider)}; {m20.ToString(format, formatProvider)}, {m21.ToString(format, formatProvider)}, {m22.ToString(format, formatProvider)}, {m23.ToString(format, formatProvider)}; {m30.ToString(format, formatProvider)}, {m31.ToString(format, formatProvider)}, {m32.ToString(format, formatProvider)}, {m33.ToString(format, formatProvider)}]";
	}

	public Matrix4x4(Vector4 column0, Vector4 column1, Vector4 column2, Vector4 column3)
	{
		this = default(Matrix4x4);
		m00 = column0.x;
		m10 = column0.y;
		m20 = column0.z;
		m30 = column0.w;
		m01 = column1.x;
		m11 = column1.y;
		m21 = column1.z;
		m31 = column1.w;
		m02 = column2.x;
		m12 = column2.y;
		m22 = column2.z;
		m32 = column2.w;
		m03 = column3.x;
		m13 = column3.y;
		m23 = column3.z;
		m33 = column3.w;
	}

	public Vector3 GetPosition()
	{
		return new Vector3(m03, m13, m23);
	}

	public static Matrix4x4 Multiply(in Matrix4x4 lhs, in Matrix4x4 rhs)
	{
		return FromNumerics(lhs._value * rhs._value);
	}

	public static Matrix4x4 Inverse(Matrix4x4 m)
	{
		System.Numerics.Matrix4x4.Invert(m._value, out var result);
		return FromNumerics(result);
	}

	public Vector3 MultiplyPoint(Vector3 point)
	{
		Vector4 vector = this * new Vector4(point.x, point.y, point.z, 1f);
		if (MathF.Abs(vector.w) > 1E-06f)
		{
			return new Vector3(vector.x / vector.w, vector.y / vector.w, vector.z / vector.w);
		}
		return new Vector3(vector.x, vector.y, vector.z);
	}

	public Vector3 MultiplyPoint3x4(Vector3 point)
	{
		System.Numerics.Vector4 vector = System.Numerics.Vector4.Transform(new System.Numerics.Vector4(point.x, point.y, point.z, 1f), System.Numerics.Matrix4x4.Transpose(_value));
		return new Vector3(vector.X, vector.Y, vector.Z);
	}

	public Vector3 MultiplyVector(Vector3 vector)
	{
		System.Numerics.Vector4 vector2 = System.Numerics.Vector4.Transform(new System.Numerics.Vector4(vector.x, vector.y, vector.z, 0f), System.Numerics.Matrix4x4.Transpose(_value));
		return new Vector3(vector2.X, vector2.Y, vector2.Z);
	}

	public static Matrix4x4 operator *(Matrix4x4 lhs, Matrix4x4 rhs)
	{
		return Multiply(in lhs, in rhs);
	}

	public static Vector4 operator *(Matrix4x4 lhs, Vector4 vector)
	{
		return Vector4.FromNumerics(System.Numerics.Vector4.Transform(Vector4.ToNumerics(vector), System.Numerics.Matrix4x4.Transpose(lhs._value)));
	}

	public static Matrix4x4 Ortho(float left, float right, float bottom, float top, float zNear, float zFar)
	{
		Matrix4x4 result = identity;
		result.m00 = 2f / (right - left);
		result.m11 = -2f / (top - bottom);
		result.m22 = 1f / (zFar - zNear);
		result.m03 = (0f - (right + left)) / (right - left);
		result.m13 = (top + bottom) / (top - bottom);
		result.m23 = zFar / (zFar - zNear);
		return result;
	}

	public static Matrix4x4 Rotate(Quaternion q)
	{
		System.Numerics.Quaternion quaternion = Quaternion.ToNumerics(q);
		if (System.Numerics.Quaternion.Dot(quaternion, quaternion) <= 1E-12f)
		{
			return identity;
		}
		return FromNumerics(System.Numerics.Matrix4x4.Transpose(System.Numerics.Matrix4x4.CreateFromQuaternion(System.Numerics.Quaternion.Normalize(quaternion))));
	}

	public static Matrix4x4 Scale(Vector3 vector)
	{
		Matrix4x4 result = default(Matrix4x4);
		result.m00 = vector.x;
		result.m11 = vector.y;
		result.m22 = vector.z;
		result.m33 = 1f;
		return result;
	}

	public void SetTRS(Vector3 pos, Quaternion q, Vector3 s)
	{
		this = TRS(pos, q, s);
	}

	public static Matrix4x4 Translate(Vector3 vector)
	{
		Matrix4x4 result = default(Matrix4x4);
		result.m00 = 1f;
		result.m11 = 1f;
		result.m22 = 1f;
		result.m33 = 1f;
		result.m03 = vector.x;
		result.m13 = vector.y;
		result.m23 = vector.z;
		return result;
	}

	public static Matrix4x4 TRS(Vector3 pos, Quaternion q, Vector3 s)
	{
		Matrix4x4 result = Rotate(q);
		result.m00 *= s.x;
		result.m10 *= s.x;
		result.m20 *= s.x;
		result.m01 *= s.y;
		result.m11 *= s.y;
		result.m21 *= s.y;
		result.m02 *= s.z;
		result.m12 *= s.z;
		result.m22 *= s.z;
		result.m03 = pos.x;
		result.m13 = pos.y;
		result.m23 = pos.z;
		return result;
	}

	public static Matrix4x4 Perspective(float fovYDegrees, float aspect, float zNear, float zFar)
	{
		float num = 1f / MathF.Tan(0.5f * fovYDegrees * ((float)Math.PI / 180f));
		Matrix4x4 result = zero;
		result.m00 = num / Math.Max(1E-06f, aspect);
		result.m11 = 0f - num;
		result.m22 = zNear / (zFar - zNear);
		result.m23 = zFar * zNear / (zFar - zNear);
		result.m32 = -1f;
		return result;
	}

	public override bool Equals(object obj)
	{
		if (obj is Matrix4x4 other)
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

	public static System.Numerics.Matrix4x4 ToNumerics(Matrix4x4 m)
	{
		return m._value;
	}

	internal static Matrix4x4 FromNumerics(System.Numerics.Matrix4x4 m)
	{
		Matrix4x4 result = default(Matrix4x4);
		result._value = m;
		return result;
	}
}
