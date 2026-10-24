namespace Waylith.VideoDecode.Theora;

internal struct TheoraBits
{
	private const int WindowSize = 64;

	private const int LotsOfBits = 1073741824;

	private unsafe byte* stop;

	private unsafe byte* ptr;

	private ulong window;

	private int bits;

	private bool eof;

	public unsafe long BytesLeft
	{
		get
		{
			if (!eof)
			{
				return stop - ptr + (bits >> 3);
			}
			return -1L;
		}
	}

	public unsafe void Init(byte* buffer, long length)
	{
		ptr = buffer;
		stop = buffer + length;
		window = 0uL;
		bits = 0;
		eof = false;
	}

	private unsafe ulong Refill(int count)
	{
		int num = bits;
		uint num2 = (uint)(64 - num);
		while (7 < num2 && ptr < stop)
		{
			num2 -= 8;
			window |= (ulong)(*(ptr++)) << (int)num2;
		}
		num = (int)(64 - num2);
		if (count > num)
		{
			if (ptr >= stop)
			{
				eof = true;
				num = 1073741824;
			}
			else
			{
				window |= (uint)(*ptr >> (num & 7));
			}
		}
		bits = num;
		return window;
	}

	public long Read(int count)
	{
		if (count == 0)
		{
			return 0L;
		}
		if (bits < count)
		{
			Refill(count);
		}
		ulong result = window >> 64 - count;
		bits -= count;
		window <<= 1;
		window <<= count - 1;
		return (long)result;
	}

	public int Read1()
	{
		if (bits < 1)
		{
			Refill(1);
		}
		int result = (int)(window >> 63);
		bits--;
		window <<= 1;
		return result;
	}

	public unsafe int Token(short* tree)
	{
		byte* ptr = this.ptr;
		ulong num = window;
		int num2 = bits;
		int num3 = 0;
		int num4;
		while (true)
		{
			num4 = tree[num3];
			if (num4 > num2)
			{
				uint num5 = (uint)(64 - num2);
				do
				{
					if (ptr >= stop)
					{
						num5 = 3221225472u;
						break;
					}
					num5 -= 8;
					num |= (ulong)(*(ptr++)) << (int)num5;
				}
				while (num5 >= 8);
				num2 = (int)(64 - num5);
			}
			int num6 = (int)(num >> 64 - num4);
			num3 = tree[num3 + 1 + num6];
			if (num3 <= 0)
			{
				break;
			}
			num <<= num4;
			num2 -= num4;
		}
		num3 = -num3;
		num4 = num3 >> 8;
		num <<= num4;
		num2 -= num4;
		this.ptr = ptr;
		window = num;
		bits = num2;
		return num3 & 0xFF;
	}
}
