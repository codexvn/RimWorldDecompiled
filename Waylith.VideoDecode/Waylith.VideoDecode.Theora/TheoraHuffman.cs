using System.IO;
using System.Runtime.InteropServices;

namespace Waylith.VideoDecode.Theora;

internal static class TheoraHuffman
{
	private const int Slush = 2;

	private const int RootSlush = 7;

	public unsafe static short* ReadTree(ref TheoraBits bits)
	{
		byte* tokens = stackalloc byte[512];
		int count = Unpack(ref bits, tokens);
		int num = Collapse(null, tokens, count);
		if (num > 32767)
		{
			throw new InvalidDataException("theora: oversized huffman tree");
		}
		short* ptr = (short*)NativeMemory.Alloc((nuint)num, 2u);
		Collapse(ptr, tokens, count);
		return ptr;
	}

	private unsafe static int Unpack(ref TheoraBits bits, byte* tokens)
	{
		uint num = 0u;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		while (true)
		{
			int num5 = bits.Read1();
			if (bits.BytesLeft < 0)
			{
				throw new InvalidDataException("theora: truncated huffman tree");
			}
			if (num5 == 0)
			{
				if (++num2 > 32)
				{
					throw new InvalidDataException("theora: huffman codeword too long");
				}
				continue;
			}
			if (++num4 > 32)
			{
				throw new InvalidDataException("theora: too many huffman leaves");
			}
			int num6 = (int)bits.Read(5);
			byte b = TheoraTables.TokenMapLogEntries[num6];
			int num7 = TheoraTables.TokenMap[num6];
			for (int num8 = 1 << (int)b; num8 > 0; num8--)
			{
				tokens[num3 * 2] = (byte)num7++;
				tokens[num3 * 2 + 1] = (byte)(num2 + b);
				num3++;
			}
			if (num2 <= 0)
			{
				break;
			}
			uint num9 = 2147483648u >> num2 - 1;
			while (num2 > 0 && (num & num9) != 0)
			{
				num ^= num9;
				num9 <<= 1;
				num2--;
			}
			if (num2 <= 0)
			{
				break;
			}
			num |= num9;
		}
		return num3;
	}

	private unsafe static int SubtreeTokens(byte* tokens, int depth)
	{
		uint num = 0u;
		int num2 = 0;
		do
		{
			if (tokens[num2 * 2 + 1] - depth < 32)
			{
				num += (uint)(int.MinValue >>> tokens[num2++ * 2 + 1] - depth);
				continue;
			}
			num++;
			num2 += SubtreeTokens(tokens + num2 * 2, depth + 31);
		}
		while (num < 2147483648u);
		return num2;
	}

	private unsafe static int CollapseDepth(byte* tokens, int count, int depth)
	{
		int num = ((depth > 0) ? 2 : 7);
		int num2 = 1;
		int num3 = 2;
		bool flag = true;
		int result = 1;
		int num4;
		do
		{
			if (flag)
			{
				result = num2;
			}
			num2++;
			flag = false;
			num4 = num3;
			num3 = 0;
			int num5 = 0;
			while (num5 < count)
			{
				if (tokens[num5 * 2 + 1] < depth + num2)
				{
					num5++;
				}
				else if (tokens[num5 * 2 + 1] == depth + num2)
				{
					flag = true;
					num5++;
				}
				else
				{
					num5 += SubtreeTokens(tokens + num5 * 2, depth + num2);
				}
				num3++;
			}
		}
		while (num3 > num4 && num3 * num >= 1 << num2);
		return result;
	}

	private unsafe static int Collapse(short* tree, byte* tokens, int count)
	{
		short* ptr = stackalloc short[34];
		byte* ptr2 = stackalloc byte[34];
		byte* ptr3 = stackalloc byte[34];
		*ptr2 = 0;
		*ptr3 = (byte)(count - 1);
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		do
		{
			int num4 = CollapseDepth(tokens + num2 * 2, ptr3[num3] + 1 - num2, ptr2[num3]);
			ptr[num3] = (short)num;
			num += 1 + (1 << num4);
			if (tree != null)
			{
				tree[ptr[num3]++] = (short)num4;
			}
			while (true)
			{
				if (num2 <= ptr3[num3] && tokens[num2 * 2 + 1] <= ptr2[num3] + num4)
				{
					if (tree != null)
					{
						int num5 = 1 << ptr2[num3] + num4 - tokens[num2 * 2 + 1];
						short num6 = (short)(-((tokens[num2 * 2 + 1] - ptr2[num3] << 8) | tokens[num2 * 2]));
						while (num5-- > 0)
						{
							tree[ptr[num3]++] = num6;
						}
					}
					num2++;
					continue;
				}
				if (num2 <= ptr3[num3])
				{
					ptr2[num3 + 1] = (byte)(ptr2[num3] + num4);
					if (tree != null)
					{
						tree[ptr[num3]++] = (short)num;
					}
					num3++;
					ptr3[num3] = (byte)(num2 + SubtreeTokens(tokens + num2 * 2, ptr2[num3]) - 1);
					break;
				}
				if (num3-- > 0)
				{
					num4 = ptr2[num3 + 1] - ptr2[num3];
				}
				if (num3 < 0)
				{
					break;
				}
			}
		}
		while (num3 >= 0);
		return num;
	}
}
