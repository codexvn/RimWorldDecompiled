using System;
using System.Globalization;
using System.Numerics;

namespace Waylith.Math;

public struct quaternion : IEquatable<quaternion>, IFormattable
{
	public float4 value;

	public static readonly quaternion identity = new quaternion
	{
		value = new float4(0f, 0f, 0f, 1f)
	};

	public quaternion(float3x3 m)
	{
		float x = m.c0.x;
		float x2 = m.c1.x;
		float x3 = m.c2.x;
		float y = m.c0.y;
		float y2 = m.c1.y;
		float y3 = m.c2.y;
		float z = m.c0.z;
		float z2 = m.c1.z;
		float z3 = m.c2.z;
		float num = x + y2 + z3;
		if (num > 0f)
		{
			float num2 = MathF.Sqrt(num + 1f) * 2f;
			value = new float4((z2 - y3) / num2, (x3 - z) / num2, (y - x2) / num2, 0.25f * num2);
		}
		else if (x > y2 && x > z3)
		{
			float num3 = MathF.Sqrt(1f + x - y2 - z3) * 2f;
			value = new float4(0.25f * num3, (x2 + y) / num3, (x3 + z) / num3, (z2 - y3) / num3);
		}
		else if (y2 > z3)
		{
			float num4 = MathF.Sqrt(1f + y2 - x - z3) * 2f;
			value = new float4((x2 + y) / num4, 0.25f * num4, (y3 + z2) / num4, (x3 - z) / num4);
		}
		else
		{
			float num5 = MathF.Sqrt(1f + z3 - x - y2) * 2f;
			value = new float4((x3 + z) / num5, (y3 + z2) / num5, 0.25f * num5, (y - x2) / num5);
		}
	}

	public bool Equals(quaternion x)
	{
		return value.Equals(x.value);
	}

	public override bool Equals(object obj)
	{
		if (obj is quaternion x)
		{
			return Equals(x);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return value.GetHashCode();
	}

	public string ToString(string format, IFormatProvider formatProvider)
	{
		return value.ToString(format, formatProvider);
	}

	public override string ToString()
	{
		return ToString(null, CultureInfo.InvariantCulture);
	}

	public static quaternion AxisAngle(float3 axis, float angle)
	{
		float3 float5 = math.normalize(axis);
		float x = angle * 0.5f;
		float num = MathF.Sin(x);
		quaternion result = default(quaternion);
		result.value = new float4(float5.x * num, float5.y * num, float5.z * num, MathF.Cos(x));
		return result;
	}

	public static quaternion Euler(float x, float y, float z, math.RotationOrder order = math.RotationOrder.Default)
	{
		quaternion quaternion2 = AxisAngle(new float3(1f, 0f, 0f), x);
		quaternion quaternion3 = AxisAngle(new float3(0f, 1f, 0f), y);
		quaternion quaternion4 = AxisAngle(new float3(0f, 0f, 1f), z);
		return order switch
		{
			math.RotationOrder.XYZ => math.mul(math.mul(quaternion4, quaternion3), quaternion2), 
			math.RotationOrder.XZY => math.mul(math.mul(quaternion3, quaternion4), quaternion2), 
			math.RotationOrder.YXZ => math.mul(math.mul(quaternion4, quaternion2), quaternion3), 
			math.RotationOrder.YZX => math.mul(math.mul(quaternion2, quaternion4), quaternion3), 
			math.RotationOrder.ZYX => math.mul(math.mul(quaternion2, quaternion3), quaternion4), 
			_ => math.mul(math.mul(quaternion3, quaternion2), quaternion4), 
		};
	}

	public static quaternion Euler(float3 xyz, math.RotationOrder order = math.RotationOrder.Default)
	{
		return Euler(xyz.x, xyz.y, xyz.z, order);
	}

	public static quaternion LookRotationSafe(float3 forward, float3 up)
	{
		float3 float5 = math.normalize(forward);
		if (math.lengthsq(float5) < 1E-12f)
		{
			return identity;
		}
		float3 float6 = math.normalize(math.cross(up, float5));
		if (math.lengthsq(float6) < 1E-12f)
		{
			return identity;
		}
		float3 c = math.cross(float5, float6);
		return new quaternion(new float3x3(float6, c, float5));
	}

	public static implicit operator quaternion(float4 v)
	{
		quaternion result = default(quaternion);
		result.value = v;
		return result;
	}

	public static implicit operator quaternion(Quaternion q)
	{
		quaternion result = default(quaternion);
		result.value = new float4(q.X, q.Y, q.Z, q.W);
		return result;
	}

	public static implicit operator Quaternion(quaternion q)
	{
		return new Quaternion(q.value.x, q.value.y, q.value.z, q.value.w);
	}
}
