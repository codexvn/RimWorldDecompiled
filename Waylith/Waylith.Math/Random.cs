using System;

namespace Waylith.Math;

public struct Random
{
	private const uint DefaultState = 1851936439u;

	public uint state;

	public Random(uint seed)
	{
		state = ((seed == 0) ? 1851936439u : seed);
		NextState();
	}

	public static Random CreateFromIndex(uint index)
	{
		return new Random(WangHash(index + 62));
	}

	private static uint WangHash(uint n)
	{
		n = n ^ 0x3D ^ (n >> 16);
		n *= 9;
		n ^= n >> 4;
		n *= 668265261;
		n ^= n >> 15;
		return n;
	}

	public void InitState(uint seed = 1851936439u)
	{
		state = ((seed == 0) ? 1851936439u : seed);
		NextState();
	}

	public bool NextBool()
	{
		return (NextState() & 1) == 1;
	}

	public int NextInt()
	{
		return (int)NextState() ^ int.MinValue;
	}

	public int NextInt(int max)
	{
		return (int)(NextState() * max >>> 32);
	}

	public int NextInt(int min, int max)
	{
		uint num = (uint)(max - min);
		return (int)((long)NextState() * (long)num >>> 32) + min;
	}

	public uint NextUInt()
	{
		return NextState() - 1;
	}

	public uint NextUInt(uint max)
	{
		return (uint)((ulong)((long)NextState() * (long)max) >> 32);
	}

	public uint NextUInt(uint min, uint max)
	{
		uint num = max - min;
		return (uint)(int)((long)NextState() * (long)num >>> 32) + min;
	}

	public float NextFloat()
	{
		return BitConverter.UInt32BitsToSingle(0x3F800000 | (NextState() >> 9)) - 1f;
	}

	public float NextFloat(float max)
	{
		return NextFloat() * max;
	}

	public float NextFloat(float min, float max)
	{
		return NextFloat() * (max - min) + min;
	}

	public double NextDouble()
	{
		ulong num = ((ulong)NextState() << 20) ^ NextState();
		return BitConverter.UInt64BitsToDouble(0x3FF0000000000000L | num) - 1.0;
	}

	private uint NextState()
	{
		uint result = state;
		state ^= state << 13;
		state ^= state >> 17;
		state ^= state << 5;
		return result;
	}
}
