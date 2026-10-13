using System;
using System.Buffers.Binary;

namespace UnityStubs.Internal;

internal static class Bc7BlockFlip
{
	private ref struct Block
	{
		public int Mode;

		public int Partition;

		public int Rotation;

		public int IndexSelection;

		public Span<byte> Endpoints;

		public Span<byte> PBits;

		public Span<byte> Indices;

		public Span<byte> Indices2;
	}

	private static readonly byte[][] Modes = new byte[8][]
	{
		new byte[10] { 3, 4, 0, 0, 4, 0, 1, 0, 3, 0 },
		new byte[10] { 2, 6, 0, 0, 6, 0, 0, 1, 3, 0 },
		new byte[10] { 3, 6, 0, 0, 5, 0, 0, 0, 2, 0 },
		new byte[10] { 2, 6, 0, 0, 7, 0, 1, 0, 2, 0 },
		new byte[10] { 1, 0, 2, 1, 5, 6, 0, 0, 2, 3 },
		new byte[10] { 1, 0, 2, 0, 7, 8, 0, 0, 2, 2 },
		new byte[10] { 1, 0, 0, 0, 7, 7, 1, 0, 4, 0 },
		new byte[10] { 2, 6, 0, 0, 5, 5, 1, 0, 2, 0 }
	};

	private static readonly ushort[] Shapes2 = new ushort[64]
	{
		52428, 34952, 61166, 60616, 51328, 65260, 65224, 60544, 51200, 65516,
		65152, 59392, 65512, 65280, 65520, 61440, 63248, 142, 28928, 2254,
		140, 29456, 12544, 36046, 2188, 12560, 26214, 13932, 6120, 4080,
		29070, 14748, 43690, 61680, 23130, 13260, 15420, 21930, 38550, 42330,
		29646, 5064, 12876, 15324, 27030, 49980, 39270, 1632, 626, 1252,
		20032, 10016, 51510, 37740, 14790, 25500, 37686, 40134, 33150, 59160,
		52464, 4044, 30532, 60962
	};

	private static readonly uint[] Shapes3 = new uint[64]
	{
		2858963024u, 1784303680u, 1515864576u, 1414570152u, 2779054080u, 2694860880u, 1431675040u, 1515868240u, 2857697280u, 2857719040u,
		2863289600u, 2425393296u, 2492765332u, 2762253476u, 2846200912u, 705315408u, 2777960512u, 172118100u, 2779096320u, 1436590240u,
		2829603924u, 1785348160u, 2762231808u, 437912832u, 5285028u, 2862977168u, 342452500u, 1768494080u, 2693105056u, 2860651540u,
		1352967248u, 1784283648u, 2846195712u, 1351655592u, 2829094992u, 606348324u, 11162880u, 613566756u, 608801316u, 1352993360u,
		1342874960u, 2863285316u, 1717960704u, 2778768800u, 1352683680u, 1764256040u, 1152035396u, 1717986816u, 2856600644u, 1420317864u,
		2508232064u, 2526451200u, 2824098984u, 2157286784u, 2853442580u, 2526412800u, 2863272980u, 2689618080u, 2695210400u, 2516582400u,
		1082146944u, 2846402984u, 2863311428u, 709513812u
	};

	private static readonly byte[] Anchor2 = new byte[64]
	{
		15, 15, 15, 15, 15, 15, 15, 15, 15, 15,
		15, 15, 15, 15, 15, 15, 15, 2, 8, 2,
		2, 8, 8, 15, 2, 8, 2, 2, 8, 8,
		2, 2, 15, 15, 6, 8, 2, 8, 15, 15,
		2, 8, 2, 2, 2, 15, 15, 6, 6, 2,
		6, 8, 15, 15, 2, 2, 15, 15, 15, 15,
		15, 2, 2, 15
	};

	private static readonly byte[] Anchor3A = new byte[64]
	{
		3, 3, 15, 15, 8, 3, 15, 15, 8, 8,
		6, 6, 6, 5, 3, 3, 3, 3, 8, 15,
		3, 3, 6, 10, 5, 8, 8, 6, 8, 5,
		15, 15, 8, 15, 3, 5, 6, 10, 8, 15,
		15, 3, 15, 5, 15, 15, 15, 15, 3, 15,
		5, 5, 5, 8, 5, 10, 5, 10, 8, 13,
		15, 12, 3, 3
	};

	private static readonly byte[] Anchor3B = new byte[64]
	{
		15, 8, 8, 3, 15, 15, 3, 8, 15, 15,
		15, 15, 15, 15, 15, 8, 15, 8, 15, 3,
		15, 8, 15, 8, 3, 15, 6, 10, 15, 15,
		10, 8, 15, 3, 15, 10, 10, 8, 9, 10,
		6, 15, 8, 15, 3, 6, 6, 8, 15, 3,
		15, 15, 15, 15, 15, 15, 15, 15, 15, 15,
		3, 15, 15, 8
	};

	private static readonly byte[] Weights2 = new byte[4] { 0, 21, 43, 64 };

	private static readonly byte[] Weights3 = new byte[8] { 0, 9, 18, 27, 37, 46, 55, 64 };

	private static readonly byte[] Weights4 = new byte[16]
	{
		0, 4, 9, 13, 17, 21, 26, 30, 34, 38,
		43, 47, 51, 55, 60, 64
	};

	private const ushort Inexact = ushort.MaxValue;

	private static readonly ushort[][][] Twins = new ushort[5][][];

	private static ReadOnlySpan<byte> Relabels => new byte[18]
	{
		0, 1, 2, 1, 0, 2, 0, 2, 1, 2,
		1, 0, 1, 2, 0, 2, 0, 1
	};

	public static void FlipRows(Span<byte> data, int validRows)
	{
		if (validRows < 2)
		{
			return;
		}
		Block block = default(Block);
		Span<byte> endpoints = stackalloc byte[24];
		block.Endpoints = endpoints;
		Span<byte> pBits = stackalloc byte[6];
		block.PBits = pBits;
		Span<byte> indices = stackalloc byte[16];
		block.Indices = indices;
		Span<byte> indices2 = stackalloc byte[16];
		block.Indices2 = indices2;
		Block block2 = block;
		if (!Read(data, ref block2))
		{
			return;
		}
		byte[] array = Modes[block2.Mode];
		int num = array[0];
		ushort num2 = (ushort)((num != 1) ? TwinsFor(validRows)[(array[1] == 4) ? 2 : (num - 2)][block2.Partition] : 0);
		Span<byte> span = stackalloc byte[64];
		if (num2 == ushort.MaxValue)
		{
			Span<byte> rgba = stackalloc byte[64];
			Decode(in block2, rgba);
			for (int i = 0; i < 16; i++)
			{
				rgba.Slice(RowMap(i, validRows) * 4, 4).CopyTo(span.Slice(i * 4, 4));
			}
		}
		MoveRows(block2.Indices, validRows);
		MoveRows(block2.Indices2, validRows);
		if (num > 1)
		{
			Span<byte> span2 = stackalloc byte[24];
			Span<byte> span3 = stackalloc byte[6];
			block2.Endpoints.CopyTo(span2);
			block2.PBits.CopyTo(span3);
			if (num2 != ushort.MaxValue)
			{
				block2.Partition = num2 & 0x3F;
				Relabel(ref block2, span2, span3, Relabels.Slice((num2 >> 6) * 3, 3));
			}
			else
			{
				int num3 = int.MaxValue;
				int partition = 0;
				int num4 = 0;
				for (int j = 0; j < 1 << (int)array[1]; j++)
				{
					for (int k = 0; k < ((num == 2) ? 2 : 6); k++)
					{
						block2.Partition = j;
						Relabel(ref block2, span2, span3, Relabels.Slice(k * 3, 3));
						int num5 = Refit(ref block2, span);
						if (num5 < num3)
						{
							int num6 = j;
							num4 = k;
							partition = num6;
							num3 = num5;
						}
					}
				}
				block2.Partition = partition;
				Relabel(ref block2, span2, span3, Relabels.Slice(num4 * 3, 3));
				RefitEndpoints(ref block2, span, num3);
				Refit(ref block2, span);
			}
		}
		bool flag = array[9] != 0;
		for (int l = 0; l < num; l++)
		{
			int index = AnchorOf(num, block2.Partition, l);
			if (block2.Indices[index] >> array[8] - 1 != 0)
			{
				bool flag2 = flag && block2.IndexSelection == 1;
				SwapEndpoints(ref block2, l, !flag || !flag2, !flag || flag2, array[6] != 0);
				Invert(block2.Indices, array[8], num, block2.Partition, l);
			}
		}
		if (flag && block2.Indices2[0] >> array[9] - 1 != 0)
		{
			bool flag3 = block2.IndexSelection == 0;
			SwapEndpoints(ref block2, 0, !flag3, flag3, pbits: false);
			Invert(block2.Indices2, array[9], 1, 0, 0);
		}
		Write(in block2, data);
	}

	private static int RowMap(int texel, int validRows)
	{
		int num = texel >> 2;
		if (num >= validRows)
		{
			return texel;
		}
		return (validRows - 1 - num << 2) | (texel & 3);
	}

	private static void MoveRows(Span<byte> indices, int validRows)
	{
		Span<byte> destination = stackalloc byte[16];
		indices.CopyTo(destination);
		for (int i = 0; i < 16; i++)
		{
			indices[i] = destination[RowMap(i, validRows)];
		}
	}

	private static int SubsetOf(int subsets, int partition, int texel)
	{
		return subsets switch
		{
			2 => (Shapes2[partition] >> texel) & 1, 
			3 => (int)((Shapes3[partition] >> 2 * texel) & 3), 
			_ => 0, 
		};
	}

	private static int AnchorOf(int subsets, int partition, int subset)
	{
		return subset switch
		{
			0 => 0, 
			1 => (subsets == 2) ? Anchor2[partition] : Anchor3A[partition], 
			_ => Anchor3B[partition], 
		};
	}

	private static void SwapEndpoints(ref Block block, int subset, bool colour, bool alpha, bool pbits)
	{
		int num = subset * 8;
		int num2 = num + 4;
		for (int i = 0; i < 4; i++)
		{
			if ((i < 3) ? colour : alpha)
			{
				ref byte reference = ref block.Endpoints[num + i];
				ref byte reference2 = ref block.Endpoints[num2 + i];
				byte b = block.Endpoints[num2 + i];
				byte b2 = block.Endpoints[num + i];
				reference = b;
				reference2 = b2;
			}
		}
		if (pbits)
		{
			ref byte reference = ref block.PBits[subset * 2];
			ref byte reference3 = ref block.PBits[subset * 2 + 1];
			byte b2 = block.PBits[subset * 2 + 1];
			byte b = block.PBits[subset * 2];
			reference = b2;
			reference3 = b;
		}
	}

	private static void Invert(Span<byte> indices, int bits, int subsets, int partition, int subset)
	{
		int num = (1 << bits) - 1;
		for (int i = 0; i < 16; i++)
		{
			if (SubsetOf(subsets, partition, i) == subset)
			{
				indices[i] = (byte)(num - indices[i]);
			}
		}
	}

	private static void Relabel(ref Block block, ReadOnlySpan<byte> endpoints, ReadOnlySpan<byte> pbits, ReadOnlySpan<byte> relabel)
	{
		byte[] array = Modes[block.Mode];
		for (int i = 0; i < array[0]; i++)
		{
			int num = relabel[i];
			endpoints.Slice(i * 8, 8).CopyTo(block.Endpoints.Slice(num * 8, 8));
			if (array[6] != 0)
			{
				pbits.Slice(i * 2, 2).CopyTo(block.PBits.Slice(num * 2, 2));
			}
			else if (array[7] != 0)
			{
				block.PBits[num] = pbits[i];
			}
		}
	}

	private static ushort[][] TwinsFor(int validRows)
	{
		ushort[][] array = Twins[validRows];
		if (array != null)
		{
			return array;
		}
		array = new ushort[3][]
		{
			new ushort[64],
			new ushort[64],
			new ushort[16]
		};
		for (int i = 0; i < 3; i++)
		{
			int num = ((i == 0) ? 2 : 3);
			int num2 = array[i].Length;
			for (int j = 0; j < num2; j++)
			{
				array[i][j] = ushort.MaxValue;
				for (int k = 0; k < num2; k++)
				{
					for (int l = 0; l < ((num == 2) ? 2 : 6); l++)
					{
						int m;
						for (m = 0; m < 16 && Relabels[l * 3 + SubsetOf(num, j, RowMap(m, validRows))] == SubsetOf(num, k, m); m++)
						{
						}
						if (m == 16)
						{
							array[i][j] = (ushort)(k | (l << 6));
						}
					}
				}
			}
		}
		return Twins[validRows] = array;
	}

	private static int Refit(ref Block block, ReadOnlySpan<byte> wanted)
	{
		byte[] array = Modes[block.Mode];
		Span<byte> endpoints = stackalloc byte[24];
		Unquantize(in block, endpoints);
		byte[] array2 = WeightsFor(array[8]);
		Span<byte> span = stackalloc byte[4];
		int num = 0;
		for (int i = 0; i < 16; i++)
		{
			int num2 = SubsetOf(array[0], block.Partition, i) * 8;
			int num3 = int.MaxValue;
			for (int j = 0; j < array2.Length; j++)
			{
				for (int k = 0; k < 4; k++)
				{
					span[k] = Mix(endpoints[num2 + k], endpoints[num2 + 4 + k], array2[j]);
				}
				int num4 = Distance(span, wanted.Slice(i * 4, 4));
				if (num4 < num3)
				{
					ref byte reference = ref block.Indices[i];
					byte b = (byte)j;
					num3 = num4;
					reference = b;
				}
			}
			num += num3;
		}
		return num;
	}

	private static bool Read(ReadOnlySpan<byte> data, ref Block block)
	{
		UInt128 bits = new UInt128(BinaryPrimitives.ReadUInt64LittleEndian(data.Slice(8)), BinaryPrimitives.ReadUInt64LittleEndian(data));
		int i;
		for (i = 0; i < 8 && ((data[0] >> i) & 1) == 0; i++)
		{
		}
		if (i == 8)
		{
			return false;
		}
		byte[] array = Modes[i];
		int pos = i + 1;
		block.Mode = i;
		block.Partition = Take(bits, ref pos, array[1]);
		block.Rotation = Take(bits, ref pos, array[2]);
		block.IndexSelection = Take(bits, ref pos, array[3]);
		int num = array[0];
		int num2 = num * 2;
		for (int j = 0; j < 4; j++)
		{
			byte count = ((j < 3) ? array[4] : array[5]);
			for (int k = 0; k < num2; k++)
			{
				block.Endpoints[k * 4 + j] = (byte)Take(bits, ref pos, count);
			}
		}
		int num3 = ((array[6] != 0) ? num2 : ((array[7] != 0) ? num : 0));
		for (int l = 0; l < num3; l++)
		{
			block.PBits[l] = (byte)Take(bits, ref pos, 1);
		}
		for (int m = 0; m < 16; m++)
		{
			block.Indices[m] = (byte)Take(bits, ref pos, (int)array[8] - (IsAnchor(num, block.Partition, m) ? 1 : 0));
		}
		if (array[9] != 0)
		{
			for (int n = 0; n < 16; n++)
			{
				block.Indices2[n] = (byte)Take(bits, ref pos, (int)array[9] - ((n == 0) ? 1 : 0));
			}
		}
		return true;
	}

	private static void Write(in Block block, Span<byte> data)
	{
		byte[] array = Modes[block.Mode];
		UInt128 bits = (UInt128)1 << block.Mode;
		int pos = block.Mode + 1;
		Put(ref bits, ref pos, array[1], block.Partition);
		Put(ref bits, ref pos, array[2], block.Rotation);
		Put(ref bits, ref pos, array[3], block.IndexSelection);
		int num = array[0];
		int num2 = num * 2;
		for (int i = 0; i < 4; i++)
		{
			byte count = ((i < 3) ? array[4] : array[5]);
			for (int j = 0; j < num2; j++)
			{
				Put(ref bits, ref pos, count, block.Endpoints[j * 4 + i]);
			}
		}
		int num3 = ((array[6] != 0) ? num2 : ((array[7] != 0) ? num : 0));
		for (int k = 0; k < num3; k++)
		{
			Put(ref bits, ref pos, 1, block.PBits[k]);
		}
		for (int l = 0; l < 16; l++)
		{
			Put(ref bits, ref pos, (int)array[8] - (IsAnchor(num, block.Partition, l) ? 1 : 0), block.Indices[l]);
		}
		if (array[9] != 0)
		{
			for (int m = 0; m < 16; m++)
			{
				Put(ref bits, ref pos, (int)array[9] - ((m == 0) ? 1 : 0), block.Indices2[m]);
			}
		}
		BinaryPrimitives.WriteUInt64LittleEndian(data, (ulong)bits);
		BinaryPrimitives.WriteUInt64LittleEndian(data.Slice(8), (ulong)(bits >> 64));
	}

	private static bool IsAnchor(int subsets, int partition, int texel)
	{
		if (texel != 0 && (subsets < 2 || texel != AnchorOf(subsets, partition, 1)))
		{
			if (subsets == 3)
			{
				return texel == AnchorOf(subsets, partition, 2);
			}
			return false;
		}
		return true;
	}

	private static int Take(UInt128 bits, ref int pos, int count)
	{
		if (count == 0)
		{
			return 0;
		}
		int result = (int)(uint)(bits >> pos) & ((1 << count) - 1);
		pos += count;
		return result;
	}

	private static void Put(ref UInt128 bits, ref int pos, int count, int value)
	{
		if (count != 0)
		{
			bits |= (UInt128)(uint)value << pos;
			pos += count;
		}
	}

	private static void RefitEndpoints(ref Block block, ReadOnlySpan<byte> wanted, int bestError)
	{
		byte[] array = Modes[block.Mode];
		int num = ((array[6] != 0 || array[7] != 0) ? 1 : 0);
		Span<byte> destination = stackalloc byte[10];
		for (int i = 0; i < array[0]; i++)
		{
			int num2 = -1;
			int num3 = -1;
			int num4 = -1;
			for (int j = 0; j < 16; j++)
			{
				for (int k = j; k < 16; k++)
				{
					if (SubsetOf(array[0], block.Partition, j) == i && SubsetOf(array[0], block.Partition, k) == i)
					{
						int num5 = Distance(wanted.Slice(j * 4, 4), wanted.Slice(k * 4, 4));
						if (num5 > num4)
						{
							int num6 = j;
							int num7 = k;
							num4 = num5;
							num3 = num7;
							num2 = num6;
						}
					}
				}
			}
			Span<byte> destination2 = block.Endpoints.Slice(i * 8, 8);
			Span<byte> destination3 = ((array[6] != 0) ? block.PBits.Slice(i * 2, 2) : block.PBits.Slice(i, array[7]));
			destination2.CopyTo(destination);
			destination3.CopyTo(destination.Slice(8));
			for (int l = 0; l < 1 << destination3.Length; l++)
			{
				for (int m = 0; m < 2; m++)
				{
					int num8 = ((destination3.Length != 0) ? ((l >> ((destination3.Length == 2) ? m : 0)) & 1) : 0);
					for (int n = 0; n < 4; n++)
					{
						int num9 = ((n < 3) ? array[4] : array[5]);
						if (num9 != 0)
						{
							int num10 = wanted[((m == 0) ? num2 : num3) * 4 + n] >> 8 - num9 - num;
							destination2[m * 4 + n] = (byte)((num == 0) ? num10 : Math.Clamp(num10 - num8 + 1 >> 1, 0, (1 << num9) - 1));
						}
					}
				}
				for (int num11 = 0; num11 < destination3.Length; num11++)
				{
					destination3[num11] = (byte)((l >> num11) & 1);
				}
				int num12 = Refit(ref block, wanted);
				if (num12 < bestError)
				{
					bestError = num12;
					destination2.CopyTo(destination);
					destination3.CopyTo(destination.Slice(8));
				}
			}
			destination.Slice(0, 8).CopyTo(destination2);
			destination.Slice(8, destination3.Length).CopyTo(destination3);
		}
	}

	private static void Unquantize(in Block block, Span<byte> endpoints)
	{
		byte[] array = Modes[block.Mode];
		for (int i = 0; i < array[0] * 2; i++)
		{
			for (int j = 0; j < 4; j++)
			{
				int num = ((j < 3) ? array[4] : array[5]);
				if (num == 0)
				{
					endpoints[i * 4 + j] = byte.MaxValue;
					continue;
				}
				int num2 = block.Endpoints[i * 4 + j];
				if (array[6] != 0 || array[7] != 0)
				{
					num2 = (num2 << 1) | block.PBits[(array[6] != 0) ? i : (i / 2)];
					num++;
				}
				num2 <<= 8 - num;
				endpoints[i * 4 + j] = (byte)(num2 | (num2 >> num));
			}
		}
	}

	private static byte Mix(int a, int b, int weight)
	{
		return (byte)((64 - weight) * a + weight * b + 32 >> 6);
	}

	private static void Decode(in Block block, Span<byte> rgba)
	{
		byte[] array = Modes[block.Mode];
		Span<byte> endpoints = stackalloc byte[24];
		Unquantize(in block, endpoints);
		byte[] array2 = WeightsFor(array[8]);
		for (int i = 0; i < 16; i++)
		{
			int num = SubsetOf(array[0], block.Partition, i) * 8;
			for (int j = 0; j < 4; j++)
			{
				rgba[i * 4 + j] = Mix(endpoints[num + j], endpoints[num + 4 + j], array2[block.Indices[i]]);
			}
		}
	}

	private static byte[] WeightsFor(int bits)
	{
		return bits switch
		{
			2 => Weights2, 
			3 => Weights3, 
			_ => Weights4, 
		};
	}

	private static int Distance(ReadOnlySpan<byte> a, ReadOnlySpan<byte> b)
	{
		int num = 0;
		for (int i = 0; i < 4; i++)
		{
			int num2 = a[i] - b[i];
			num += num2 * num2;
		}
		return num;
	}
}
