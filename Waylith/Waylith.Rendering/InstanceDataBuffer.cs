using System;

namespace Waylith.Rendering;

public sealed class InstanceDataBuffer
{
	public const int SlotCount = 16;

	private readonly byte[]?[] slots = new byte[16][];

	private readonly int[] lengths = new int[16];

	private int current = -1;

	public int Stride { get; private set; }

	public int Count { get; private set; }

	public int CurrentGeneration => current;

	public int Write(ReadOnlySpan<byte> bytes, int stride, int count)
	{
		current = (current + 1) % 16;
		byte[] array = slots[current];
		if (array == null || array.Length < bytes.Length)
		{
			array = new byte[System.Math.Max((array == null) ? bytes.Length : System.Math.Max(bytes.Length, array.Length * 2), 1)];
			slots[current] = array;
		}
		bytes.CopyTo(array);
		lengths[current] = bytes.Length;
		Stride = stride;
		Count = count;
		return current;
	}

	public ReadOnlySpan<byte> GetBytes(int generation)
	{
		if ((uint)generation >= 16u)
		{
			return ReadOnlySpan<byte>.Empty;
		}
		byte[] array = slots[generation];
		if (array != null)
		{
			return array.AsSpan(0, lengths[generation]);
		}
		return ReadOnlySpan<byte>.Empty;
	}
}
