using System;
using System.Numerics;
using System.Runtime.InteropServices;

namespace Waylith.Rendering;

[StructLayout(LayoutKind.Explicit)]
public struct MaterialUniformValue : IEquatable<MaterialUniformValue>
{
	[FieldOffset(0)]
	public MaterialUniformType Type;

	[FieldOffset(16)]
	public Matrix4x4 Matrix4x4;

	[FieldOffset(16)]
	public Vector4 Vector4;

	[FieldOffset(16)]
	public int Int;

	[FieldOffset(16)]
	public float Float;

	private MaterialUniformValue(MaterialUniformType type)
	{
		this = default(MaterialUniformValue);
		Type = type;
	}

	public static MaterialUniformValue Zero(MaterialUniformType type)
	{
		return new MaterialUniformValue(type);
	}

	public static MaterialUniformValue FromMatrix(MaterialUniformType type, Matrix4x4 value)
	{
		MaterialUniformValue result = new MaterialUniformValue(type);
		result.Matrix4x4 = value;
		return result;
	}

	public static MaterialUniformValue FromVector(MaterialUniformType type, Vector4 value)
	{
		MaterialUniformValue result = new MaterialUniformValue(type);
		result.Vector4 = value;
		return result;
	}

	public static MaterialUniformValue FromInt(int value)
	{
		MaterialUniformValue result = new MaterialUniformValue(MaterialUniformType.Int);
		result.Int = value;
		return result;
	}

	public static MaterialUniformValue FromFloat(float value)
	{
		MaterialUniformValue result = new MaterialUniformValue(MaterialUniformType.Float);
		result.Float = value;
		return result;
	}

	public bool Equals(MaterialUniformValue other)
	{
		if (Type != other.Type)
		{
			return false;
		}
		return Type switch
		{
			MaterialUniformType.Matrix3x3 => Matrix4x4.Equals(other.Matrix4x4), 
			MaterialUniformType.Matrix4x4 => Matrix4x4.Equals(other.Matrix4x4), 
			MaterialUniformType.Vector2 => Vector4.Equals(other.Vector4), 
			MaterialUniformType.Vector3 => Vector4.Equals(other.Vector4), 
			MaterialUniformType.TextureST => Vector4.Equals(other.Vector4), 
			MaterialUniformType.TexelSize => Vector4.Equals(other.Vector4), 
			MaterialUniformType.Vector4 => Vector4.Equals(other.Vector4), 
			MaterialUniformType.Int => Int == other.Int, 
			MaterialUniformType.Float => Float.Equals(other.Float), 
			_ => false, 
		};
	}

	public override bool Equals(object? obj)
	{
		if (obj is MaterialUniformValue other)
		{
			return Equals(other);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return Type switch
		{
			MaterialUniformType.Matrix3x3 => HashCode.Combine(Type, Matrix4x4), 
			MaterialUniformType.Matrix4x4 => HashCode.Combine(Type, Matrix4x4), 
			MaterialUniformType.Vector2 => HashCode.Combine(Type, Vector4), 
			MaterialUniformType.Vector3 => HashCode.Combine(Type, Vector4), 
			MaterialUniformType.TextureST => HashCode.Combine(Type, Vector4), 
			MaterialUniformType.TexelSize => HashCode.Combine(Type, Vector4), 
			MaterialUniformType.Vector4 => HashCode.Combine(Type, Vector4), 
			MaterialUniformType.Int => HashCode.Combine(Type, Int), 
			MaterialUniformType.Float => HashCode.Combine(Type, Float), 
			_ => Type.GetHashCode(), 
		};
	}

	public static bool operator ==(MaterialUniformValue left, MaterialUniformValue right)
	{
		return left.Equals(right);
	}

	public static bool operator !=(MaterialUniformValue left, MaterialUniformValue right)
	{
		return !left.Equals(right);
	}
}
