using System;

namespace Waylith.VideoDecode.Aac;

internal ref struct AacBits
{
	private readonly ReadOnlySpan<byte> data;

	private int position;

	public readonly bool Overrun => position > data.Length * 8;

	public AacBits(ReadOnlySpan<byte> data)
	{
		position = 0;
		this.data = data;
	}

	public int Read(int count)
	{
		int num = position >> 3;
		uint num2 = 0u;
		for (int i = 0; i < 4; i++)
		{
			num2 = (num2 << 8) | (uint)((num + i < data.Length) ? data[num + i] : 0);
		}
		uint result = num2 << (position & 7) >> 1 >> 31 - count;
		position += count;
		return (int)result;
	}

	public void Skip(int count)
	{
		position += count;
	}

	public void Align()
	{
		position = (position + 7) & -8;
	}
}
