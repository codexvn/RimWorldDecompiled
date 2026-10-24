using System.Numerics;
using System.Runtime.CompilerServices;

namespace Waylith.VideoDecode.Vp8;

internal struct Vp8BoolDecoder
{
	private const int LotsOfBits = 1073741824;

	private const int ValueBits = 64;

	private unsafe byte* buffer;

	private unsafe byte* end;

	private ulong value;

	private int count;

	private uint range;

	public readonly bool ReadPastEnd
	{
		get
		{
			if (count > 64)
			{
				return count < 1073741824;
			}
			return false;
		}
	}

	public unsafe void Start(byte* source, long size)
	{
		buffer = source;
		end = source + size;
		value = 0uL;
		count = -8;
		range = 255u;
		Fill();
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private unsafe void Fill()
	{
		int num = 56 - (count + 8);
		long num2 = (end - buffer) * 8;
		long num3 = num + 8 - num2;
		int num4 = 0;
		if (num3 >= 0)
		{
			count += 1073741824;
			num4 = (int)num3;
		}
		if (num3 < 0 || num2 != 0L)
		{
			while (num >= num4)
			{
				count += 8;
				value |= (ulong)(*(buffer++)) << num;
				num -= 8;
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int Read(int probability)
	{
		uint num = (uint)(1 + ((int)(range - 1) * probability >>> 8));
		if (count < 0)
		{
			Fill();
		}
		ulong num2 = (ulong)num << 56;
		uint num3 = num;
		ulong num4 = value;
		int result = 0;
		if (num4 >= num2)
		{
			num3 = range - num;
			num4 -= num2;
			result = 1;
		}
		int num5 = BitOperations.LeadingZeroCount(num3) - 24;
		range = num3 << num5;
		value = num4 << num5;
		count -= num5;
		return result;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int ReadBit()
	{
		return Read(128);
	}

	public int ReadLiteral(int bits)
	{
		int num = 0;
		for (int num2 = bits - 1; num2 >= 0; num2--)
		{
			num |= Read(128) << num2;
		}
		return num;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int ReadSigned(int magnitude)
	{
		uint num = range + 1 >> 1;
		ulong num2 = (ulong)num << 56;
		if (count < 0)
		{
			Fill();
		}
		int result;
		if (value < num2)
		{
			range = num;
			result = magnitude;
		}
		else
		{
			range -= num;
			value -= num2;
			result = -magnitude;
		}
		range += range;
		value += value;
		count--;
		return result;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public unsafe int ReadTree(sbyte* tree, byte* probabilities)
	{
		int num = 0;
		while ((num = tree[num + Read(probabilities[num >> 1])]) > 0)
		{
		}
		return -num;
	}
}
