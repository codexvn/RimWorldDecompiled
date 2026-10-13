using System;
using System.Numerics;
using System.Runtime.InteropServices;

namespace Waylith.Rendering;

public struct MaterialUniformBytes : IEquatable<MaterialUniformBytes>
{
	public const int MaxBytes = 512;

	private unsafe fixed byte bytes[512];

	public void Write(int offset, MaterialUniformValue value)
	{
		switch (value.Type)
		{
		case MaterialUniformType.Matrix3x3:
			WriteMatrix3x3(offset, value.Matrix4x4);
			break;
		case MaterialUniformType.Matrix4x4:
			WriteMatrix4x4(offset, value.Matrix4x4);
			break;
		case MaterialUniformType.Vector2:
			WriteVector2(offset, value.Vector4);
			break;
		case MaterialUniformType.Vector3:
			WriteVector3(offset, value.Vector4);
			break;
		case MaterialUniformType.Vector4:
			WriteValue(offset, value.Vector4);
			break;
		case MaterialUniformType.TextureST:
		case MaterialUniformType.TexelSize:
			WriteValue(offset, value.Vector4);
			break;
		case MaterialUniformType.Int:
			WriteValue(offset, value.Int);
			break;
		case MaterialUniformType.Float:
			WriteValue(offset, value.Float);
			break;
		default:
			throw new ArgumentOutOfRangeException("value");
		}
	}

	public readonly Vector4 ReadVector4(int offset)
	{
		return ReadValue<Vector4>(offset);
	}

	public readonly float ReadFloat(int offset)
	{
		return ReadValue<float>(offset);
	}

	public readonly int ReadInt(int offset)
	{
		return ReadValue<int>(offset);
	}

	public readonly bool Equals(in MaterialUniformBytes other)
	{
		ReadOnlySpan<byte> span = MemoryMarshal.AsBytes(MemoryMarshal.CreateReadOnlySpan(in this, 1));
		ReadOnlySpan<byte> other2 = MemoryMarshal.AsBytes(MemoryMarshal.CreateReadOnlySpan(in other, 1));
		return span.SequenceEqual(other2);
	}

	public readonly bool Equals(MaterialUniformBytes other)
	{
		return Equals(in other);
	}

	public override readonly bool Equals(object? obj)
	{
		if (obj is MaterialUniformBytes other)
		{
			return Equals(in other);
		}
		return false;
	}

	public override readonly int GetHashCode()
	{
		ReadOnlySpan<ulong> readOnlySpan = MemoryMarshal.Cast<byte, ulong>(MemoryMarshal.AsBytes(MemoryMarshal.CreateReadOnlySpan(in this, 1)));
		ulong num = 14695981039346656037uL;
		ReadOnlySpan<ulong> readOnlySpan2 = readOnlySpan;
		for (int i = 0; i < readOnlySpan2.Length; i++)
		{
			ulong num2 = readOnlySpan2[i];
			num = (num ^ num2) * 1099511628211L;
		}
		return (int)(num ^ (num >> 32));
	}

	private void WriteMatrix3x3(int offset, Matrix4x4 value)
	{
		WriteValue(offset, new Vector3(value.M11, value.M12, value.M13));
		WriteValue(offset + 16, new Vector3(value.M21, value.M22, value.M23));
		WriteValue(offset + 32, new Vector3(value.M31, value.M32, value.M33));
	}

	private void WriteVector2(int offset, Vector4 value)
	{
		WriteValue(offset, new Vector2(value.X, value.Y));
	}

	private void WriteVector3(int offset, Vector4 value)
	{
		WriteValue(offset, new Vector3(value.X, value.Y, value.Z));
	}

	private void WriteMatrix4x4(int offset, Matrix4x4 value)
	{
		WriteValue(offset, new Vector4(value.M11, value.M12, value.M13, value.M14));
		WriteValue(offset + 16, new Vector4(value.M21, value.M22, value.M23, value.M24));
		WriteValue(offset + 32, new Vector4(value.M31, value.M32, value.M33, value.M34));
		WriteValue(offset + 48, new Vector4(value.M41, value.M42, value.M43, value.M44));
	}

	private unsafe void WriteValue<T>(int offset, T value) where T : unmanaged
	{
		if (offset < 0 || offset + sizeof(T) > 512)
		{
			throw new ArgumentOutOfRangeException("offset");
		}
		fixed (byte* ptr = bytes)
		{
			*(T*)(ptr + offset) = value;
		}
	}

	private unsafe readonly T ReadValue<T>(int offset) where T : unmanaged
	{
		if (offset < 0 || offset + sizeof(T) > 512)
		{
			throw new ArgumentOutOfRangeException("offset");
		}
		fixed (byte* ptr = bytes)
		{
			return *(T*)(ptr + offset);
		}
	}

	public static bool operator ==(in MaterialUniformBytes left, in MaterialUniformBytes right)
	{
		return left.Equals(in right);
	}

	public static bool operator !=(in MaterialUniformBytes left, in MaterialUniformBytes right)
	{
		return !(left == right);
	}
}
