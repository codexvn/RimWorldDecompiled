using System;

namespace Waylith.Rendering;

public readonly struct DrawSortKey : IComparable<DrawSortKey>, IEquatable<DrawSortKey>
{
	public readonly ulong High;

	public readonly ulong Low;

	private const int TransparentShift = 63;

	private const int QueueShift = 50;

	private const int HostOrderShift = 34;

	private const int PipelineShift = 19;

	private const int TransparentOrderShift = 21;

	public DrawSortKey(ulong high, ulong low)
	{
		High = high;
		Low = low;
	}

	public static DrawSortKey Make(int renderQueue, bool transparent, ushort hostOrder, int pipelineSortId, int queueOrder)
	{
		ulong num = ((ulong)(uint)(renderQueue & 0x1FFF) << 50) | ((ulong)hostOrder << 34);
		if (transparent)
		{
			num |= 0x8000000000000000uL;
		}
		ulong num2 = (uint)(pipelineSortId & 0x1FFFFF);
		ulong num3 = (uint)(queueOrder & 0x7FFFF);
		ulong low = (transparent ? ((num3 << 21) | num2) : ((num2 << 19) | num3));
		return new DrawSortKey(num, low);
	}

	public int CompareTo(DrawSortKey other)
	{
		if (High == other.High)
		{
			return Low.CompareTo(other.Low);
		}
		if (High >= other.High)
		{
			return 1;
		}
		return -1;
	}

	public bool Equals(DrawSortKey other)
	{
		if (High == other.High)
		{
			return Low == other.Low;
		}
		return false;
	}

	public override bool Equals(object? obj)
	{
		if (obj is DrawSortKey other)
		{
			return Equals(other);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(High, Low);
	}

	public override string ToString()
	{
		return $"{High:X16}:{Low:X16}";
	}

	public static bool operator ==(DrawSortKey left, DrawSortKey right)
	{
		return left.Equals(right);
	}

	public static bool operator !=(DrawSortKey left, DrawSortKey right)
	{
		return !left.Equals(right);
	}
}
