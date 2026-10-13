using System;

namespace UnityStubs.Internal;

internal static class SmolV
{
	private const uint SpirvMagic = 119734787u;

	private const uint SmolMagic = 1397575500u;

	private const int OpVectorShuffle = 79;

	private const int OpVectorShuffleCompact = 13;

	private const int OpDecorate = 71;

	private const int OpMemberDecorate = 72;

	private const int OpLoad = 61;

	private const int OpAccessChain = 65;

	private const int OpControlBarrier = 224;

	private const int OpMemoryBarrier = 225;

	private const int OpLoopMerge = 246;

	private const int OpSelectionMerge = 247;

	private const int OpBranch = 249;

	private const int OpBranchConditional = 250;

	private const int OpMemoryNamedBarrier = 329;

	private static readonly byte[] OpData = new byte[367]
	{
		0, 3, 0, 4, 0, 0, 0, 0, 4, 3,
		0, 1, 7, 23, 4, 4, 4, 4, 3, 5,
		5, 5, 5, 5, 5, 5, 5, 5, 5, 5,
		5, 5, 5, 5, 5, 5, 5, 5, 5, 4,
		3, 3, 3, 3, 75, 7, 3, 3, 3, 3,
		3, 75, 3, 3, 7, 3, 0, 75, 3, 7,
		3, 15, 20, 0, 0, 7, 3, 3, 3, 3,
		3, 4, 4, 1, 0, 0, 3, 15, 23, 23,
		75, 15, 23, 11, 3, 3, 3, 23, 23, 31,
		31, 23, 23, 31, 31, 23, 31, 31, 23, 28,
		11, 11, 11, 19, 11, 19, 11, 11, 3, 11,
		11, 11, 11, 11, 11, 11, 11, 11, 11, 11,
		11, 11, 11, 15, 11, 3, 11, 11, 19, 19,
		19, 19, 19, 19, 19, 19, 19, 19, 19, 19,
		19, 19, 19, 19, 19, 19, 19, 19, 19, 19,
		19, 19, 19, 3, 11, 11, 11, 11, 11, 11,
		11, 19, 19, 19, 19, 19, 19, 19, 11, 27,
		19, 19, 19, 19, 19, 19, 19, 19, 19, 19,
		19, 19, 19, 19, 19, 19, 19, 19, 19, 19,
		19, 19, 3, 3, 19, 19, 19, 19, 19, 19,
		11, 35, 27, 27, 11, 11, 3, 3, 3, 3,
		3, 3, 3, 3, 3, 3, 3, 3, 0, 0,
		0, 0, 3, 3, 24, 16, 3, 3, 0, 3,
		3, 3, 3, 3, 3, 3, 3, 3, 3, 3,
		3, 3, 3, 3, 3, 3, 20, 12, 1, 8,
		28, 0, 0, 0, 0, 0, 0, 0, 3, 3,
		0, 3, 3, 3, 3, 3, 3, 3, 3, 3,
		3, 3, 3, 3, 3, 3, 3, 3, 3, 3,
		0, 0, 3, 3, 3, 3, 3, 0, 0, 3,
		3, 3, 3, 3, 3, 3, 3, 0, 0, 3,
		3, 0, 0, 3, 3, 23, 23, 31, 31, 23,
		23, 31, 31, 23, 31, 31, 11, 0, 3, 0,
		3, 3, 3, 3, 3, 3, 3, 3, 7, 20,
		3, 4, 4, 15, 15, 15, 15, 15, 15, 15,
		15, 15, 15, 15, 15, 15, 15, 15, 15, 15,
		15, 15, 15, 15, 15, 15, 15, 15, 15, 15,
		15, 15, 15, 15, 15, 15, 15
	};

	public static bool IsSmolV(ReadOnlySpan<byte> data)
	{
		if (data.Length >= 24)
		{
			return BitConverter.ToUInt32(data) == 1397575500;
		}
		return false;
	}

	public static byte[] Decode(ReadOnlySpan<byte> data)
	{
		if (!IsSmolV(data))
		{
			return null;
		}
		byte[] array = Decode(data, beforeZero: false);
		if (array == null && BitConverter.ToUInt32(data.Slice(4)) >> 24 == 0)
		{
			array = Decode(data, beforeZero: true);
		}
		return array;
	}

	private static byte[] Decode(ReadOnlySpan<byte> data, bool beforeZero)
	{
		uint num = BitConverter.ToUInt32(data.Slice(4));
		int num2 = (int)(num >> 24);
		if (num2 > 1)
		{
			return null;
		}
		int num3 = ((num2 == 0) ? 331 : 367);
		byte[] array = new byte[BitConverter.ToUInt32(data.Slice(20))];
		int o = 0;
		int p = 24;
		if (!Write(array, ref o, 119734787u) || !Write(array, ref o, num & 0xFFFFFF))
		{
			return null;
		}
		for (int i = 8; i < 20; i += 4)
		{
			if (!Write(array, ref o, BitConverter.ToUInt32(data.Slice(i))))
			{
				return null;
			}
		}
		uint num4 = 0u;
		uint num5 = 0u;
		while (p < data.Length)
		{
			uint num6 = ReadVarint(data, ref p);
			uint num7 = (num6 >> 20 << 4) | ((num6 >> 4) & 0xF);
			int num8 = RemapOp((int)(((num6 >> 4) & 0xFFF0) | (num6 & 0xF)));
			num7++;
			switch (num8)
			{
			case 13:
			case 79:
				num7 += 4;
				break;
			case 71:
				num7 += 2;
				break;
			case 61:
			case 65:
				num7 += 3;
				break;
			}
			bool flag = num8 == 13;
			if (flag)
			{
				num8 = 79;
			}
			if (!Write(array, ref o, (num7 << 16) | (uint)num8))
			{
				return null;
			}
			byte b = (byte)((num8 < num3) ? OpData[num8] : 0);
			uint num9 = 1u;
			if ((b & 2) != 0)
			{
				if (!Write(array, ref o, ReadVarint(data, ref p)))
				{
					return null;
				}
				num9++;
			}
			if ((b & 1) != 0)
			{
				num4 += (uint)ZigDecode(ReadVarint(data, ref p));
				if (!Write(array, ref o, num4))
				{
					return null;
				}
				num9++;
			}
			if (num8 == 71 || num8 == 72)
			{
				uint num10 = ReadVarint(data, ref p);
				num5 += (uint)(beforeZero ? ((int)num10) : ZigDecode(num10));
				if (!Write(array, ref o, num5))
				{
					return null;
				}
				num9++;
			}
			if (num8 == 72 && !beforeZero)
			{
				if (p >= data.Length)
				{
					return null;
				}
				int num11 = data[p++];
				uint num12 = 0u;
				uint num13 = 0u;
				for (int j = 0; j < num11; j++)
				{
					num12 += ReadVarint(data, ref p);
					uint num14 = ReadVarint(data, ref p);
					int num15 = DecorationExtraOps(num14);
					uint num16 = ((num15 < 0) ? (ReadVarint(data, ref p) + 4) : ((uint)(4 + num15)));
					if (j != 0 && (!Write(array, ref o, (num16 << 16) | (uint)num8) || !Write(array, ref o, num5)))
					{
						return null;
					}
					if (!Write(array, ref o, num12) || !Write(array, ref o, num14))
					{
						return null;
					}
					if (num14 == 35)
					{
						if (num16 != 5)
						{
							return null;
						}
						num13 += ReadVarint(data, ref p);
						if (!Write(array, ref o, num13))
						{
							return null;
						}
						continue;
					}
					for (uint num17 = 4u; num17 < num16; num17++)
					{
						if (!Write(array, ref o, ReadVarint(data, ref p)))
						{
							return null;
						}
					}
				}
				continue;
			}
			int num18 = b >> 3;
			bool flag2 = !beforeZero;
			if (!flag2)
			{
				bool flag3;
				switch (num8)
				{
				case 224:
				case 225:
				case 246:
				case 247:
				case 249:
				case 250:
				case 329:
					flag3 = true;
					break;
				default:
					flag3 = false;
					break;
				}
				flag2 = flag3;
			}
			bool flag4 = flag2;
			int num19 = 0;
			while (num19 < num18 && num9 < num7)
			{
				uint num20 = ReadVarint(data, ref p);
				if (!Write(array, ref o, num4 - (uint)(flag4 ? ZigDecode(num20) : ((int)num20))))
				{
					return null;
				}
				num19++;
				num9++;
			}
			if (flag && num7 <= 9)
			{
				if (p >= data.Length)
				{
					return null;
				}
				uint num21 = data[p++];
				if (num7 > 5 && !Write(array, ref o, (num21 >> 6) & 3))
				{
					return null;
				}
				if (num7 > 6 && !Write(array, ref o, (num21 >> 4) & 3))
				{
					return null;
				}
				if (num7 > 7 && !Write(array, ref o, (num21 >> 2) & 3))
				{
					return null;
				}
				if (num7 > 8 && !Write(array, ref o, num21 & 3))
				{
					return null;
				}
				continue;
			}
			if ((b & 4) != 0)
			{
				for (; num9 < num7; num9++)
				{
					if (!Write(array, ref o, ReadVarint(data, ref p)))
					{
						return null;
					}
				}
				continue;
			}
			for (; num9 < num7; num9++)
			{
				if (p + 4 > data.Length || !Write(array, ref o, BitConverter.ToUInt32(data.Slice(p))))
				{
					return null;
				}
				p += 4;
			}
		}
		if (o != array.Length)
		{
			return null;
		}
		return array;
	}

	private static bool Write(byte[] output, ref int o, uint value)
	{
		if (o + 4 > output.Length)
		{
			return false;
		}
		BitConverter.TryWriteBytes(output.AsSpan(o), value);
		o += 4;
		return true;
	}

	private static uint ReadVarint(ReadOnlySpan<byte> data, ref int p)
	{
		uint num = 0u;
		int num2 = 0;
		while (p < data.Length)
		{
			byte b = data[p++];
			num |= (uint)((b & 0x7F) << num2);
			num2 += 7;
			if ((b & 0x80) == 0)
			{
				break;
			}
		}
		return num;
	}

	private static int ZigDecode(uint u)
	{
		if ((u & 1) == 0)
		{
			return (int)(u >> 1);
		}
		return (int)(u >> 1) ^ -1;
	}

	private static int DecorationExtraOps(uint decoration)
	{
		switch (decoration)
		{
		case 0u:
		case 2u:
		case 3u:
		case 4u:
		case 5u:
			return 0;
		default:
			if (decoration >= 29 && decoration <= 37)
			{
				return 1;
			}
			return -1;
		}
	}

	private static int RemapOp(int op)
	{
		return op switch
		{
			71 => 0, 
			0 => 71, 
			61 => 1, 
			1 => 61, 
			62 => 2, 
			2 => 62, 
			65 => 3, 
			3 => 65, 
			79 => 4, 
			4 => 79, 
			72 => 7, 
			7 => 72, 
			248 => 8, 
			8 => 248, 
			59 => 9, 
			9 => 59, 
			133 => 10, 
			10 => 133, 
			129 => 11, 
			11 => 129, 
			32 => 14, 
			14 => 32, 
			127 => 15, 
			15 => 127, 
			_ => op, 
		};
	}
}
