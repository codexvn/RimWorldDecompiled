using System;
using System.Globalization;
using UnityEngine;

namespace Unity.Mathematics;

public struct float4 : IEquatable<float4>, IFormattable
{
	public float x;

	public float y;

	public float z;

	public float w;

	public static readonly float4 zero = new float4(0f, 0f, 0f, 0f);

	public float2 xy => new float2(x, y);

	public float3 xyz => new float3(x, y, z);

	public float this[int index]
	{
		get
		{
			return index switch
			{
				0 => x, 
				1 => y, 
				2 => z, 
				3 => w, 
				_ => throw new IndexOutOfRangeException(), 
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
			case 2:
				z = value;
				break;
			case 3:
				w = value;
				break;
			default:
				throw new IndexOutOfRangeException();
			}
		}
	}

	public float4(float x, float y, float z, float w)
	{
		this.x = x;
		this.y = y;
		this.z = z;
		this.w = w;
	}

	public bool Equals(float4 rhs)
	{
		if (x.Equals(rhs.x) && y.Equals(rhs.y) && z.Equals(rhs.z))
		{
			return w.Equals(rhs.w);
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj is float4 rhs)
		{
			return Equals(rhs);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(x, y, z, w);
	}

	public string ToString(string format, IFormatProvider formatProvider)
	{
		return $"({x.ToString(format, formatProvider)}, {y.ToString(format, formatProvider)}, {z.ToString(format, formatProvider)}, {w.ToString(format, formatProvider)})";
	}

	public override string ToString()
	{
		return ToString(null, CultureInfo.InvariantCulture);
	}

	public static implicit operator float4(Vector4 v)
	{
		return new float4(v.x, v.y, v.z, v.w);
	}

	public static implicit operator Vector4(float4 v)
	{
		return new Vector4(v.x, v.y, v.z, v.w);
	}

	public static float4 operator *(float4 lhs, float4 rhs)
	{
		return new float4(lhs.x * rhs.x, lhs.y * rhs.y, lhs.z * rhs.z, lhs.w * rhs.w);
	}

	public static float4 operator *(float4 lhs, float rhs)
	{
		return new float4(lhs.x * rhs, lhs.y * rhs, lhs.z * rhs, lhs.w * rhs);
	}

	public static float4 operator +(float4 lhs, float4 rhs)
	{
		return new float4(lhs.x + rhs.x, lhs.y + rhs.y, lhs.z + rhs.z, lhs.w + rhs.w);
	}

	public static float4 operator -(float4 lhs, float4 rhs)
	{
		return new float4(lhs.x - rhs.x, lhs.y - rhs.y, lhs.z - rhs.z, lhs.w - rhs.w);
	}
}
