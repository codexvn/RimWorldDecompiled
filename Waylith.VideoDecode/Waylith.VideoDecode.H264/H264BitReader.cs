using System;

namespace Waylith.VideoDecode.H264;

internal ref struct H264BitReader
{
	private ReadOnlySpan<byte> data;

	private int currByte;

	private int bitsLeftInByte;

	private int prevTwoBytes;

	private int emulationPreventionBytes;

	public readonly int NumBitsLeft => bitsLeftInByte + data.Length * 8;

	public readonly int NumEmulationPreventionBytesRead => emulationPreventionBytes;

	public H264BitReader(ReadOnlySpan<byte> nal)
	{
		data = nal;
		currByte = 0;
		bitsLeftInByte = 0;
		prevTwoBytes = 65535;
		emulationPreventionBytes = 0;
	}

	private bool UpdateCurrByte()
	{
		if (data.IsEmpty)
		{
			return false;
		}
		if (data[0] == 3 && (prevTwoBytes & 0xFFFF) == 0)
		{
			data = data.Slice(1);
			emulationPreventionBytes++;
			prevTwoBytes = 65535;
			if (data.IsEmpty)
			{
				return false;
			}
		}
		currByte = data[0];
		data = data.Slice(1);
		bitsLeftInByte = 8;
		prevTwoBytes = ((prevTwoBytes & 0xFF) << 8) | currByte;
		return true;
	}

	public int ReadBits(int numBits)
	{
		int num = numBits;
		uint num2 = 0u;
		while (bitsLeftInByte < num)
		{
			num2 |= (uint)((currByte & ((1 << bitsLeftInByte) - 1)) << num - bitsLeftInByte);
			num -= bitsLeftInByte;
			if (!UpdateCurrByte())
			{
				throw new H264Exception("h264: ran out of bits");
			}
		}
		num2 |= (uint)(currByte >>> bitsLeftInByte - num);
		num2 &= (uint)((1 << numBits) - 1);
		bitsLeftInByte -= num;
		return (int)num2;
	}

	public bool ReadBool()
	{
		return ReadBits(1) != 0;
	}

	public int ReadUE()
	{
		int num = -1;
		int num2;
		do
		{
			num2 = ReadBits(1);
			num++;
		}
		while (num2 == 0);
		if (num > 31)
		{
			throw new H264Exception("h264: exp golomb code too long");
		}
		uint num3 = (uint)((1 << num) - 1);
		if (num == 31)
		{
			if (ReadBits(num) != 0)
			{
				throw new H264Exception("h264: exp golomb code out of range");
			}
			return (int)num3;
		}
		if (num > 0)
		{
			num3 += (uint)ReadBits(num);
		}
		return (int)num3;
	}

	public int ReadSE()
	{
		int num = ReadUE();
		if (num % 2 != 0)
		{
			return num / 2 + 1;
		}
		return -(num / 2);
	}

	public bool HasMoreRbspData()
	{
		if (bitsLeftInByte == 0 && !UpdateCurrByte())
		{
			return false;
		}
		if ((currByte & ((1 << bitsLeftInByte - 1) - 1)) != 0)
		{
			return true;
		}
		ReadOnlySpan<byte> readOnlySpan = data;
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			if (readOnlySpan[i] != 0)
			{
				return true;
			}
		}
		data = default(ReadOnlySpan<byte>);
		return false;
	}
}
