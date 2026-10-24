using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace Waylith.VideoDecode.Theora;

internal sealed class TheoraDecoder : IDisposable
{
	private sealed class QuantRanges
	{
		public int Count;

		public int[] Sizes;

		public byte[][] BaseMatrices;
	}

	public const int Padding = 16;

	private const long EobFinish = long.MaxValue;

	private const int FullRun = 4129;

	private readonly List<nint> allocations = new List<nint>();

	private readonly TheoraPlane[] planes = new TheoraPlane[3];

	private unsafe readonly short*[] huffmanTables = new short*[80];

	private readonly byte[] loopFilterLimits = new byte[64];

	private unsafe readonly byte*[] frameMemory = new byte*[3];

	private unsafe readonly byte*[] frameTop = new byte*[9];

	private unsafe readonly byte*[] frameBase = new byte*[3];

	private readonly int[] references = new int[3];

	private unsafe readonly byte*[] referenceData = new byte*[4];

	private readonly int[] strides = new int[3];

	private readonly int[] codedCounts = new int[3];

	private readonly int[] tokenStarts = new int[192];

	private readonly long[] eobRunStarts = new long[192];

	private readonly byte[] qualities = new byte[3];

	private TheoraBits bits;

	private TheoraInfo info;

	private unsafe TheoraFragment* fragments;

	private unsafe nint* fragmentOffsets;

	private unsafe short* fragmentMvs;

	private unsafe int* superBlockMaps;

	private unsafe byte* superBlockFlags;

	private unsafe int* macroBlockMaps;

	private unsafe sbyte* macroBlockModes;

	private unsafe int* codedFragments;

	private unsafe ushort* dequantTables;

	private unsafe byte* tokens;

	private unsafe short* coefficients;

	private unsafe sbyte* boundingValues;

	private long frameSize;

	private int fragmentCount;

	private int superBlockCount;

	private int macroBlockCount;

	private int totalCoded;

	private int tokenCapacity;

	private int tokenCount;

	private int lumaStride;

	private int chromaStride;

	private int chromaWidth;

	private int chromaHeight;

	private int chromaPaddingX;

	private int chromaPaddingY;

	private int qualityCount;

	private int output = -1;

	private bool keyFrame;

	private const int C1S7 = 64277;

	private const int C2S6 = 60547;

	private const int C3S5 = 54491;

	private const int C4S4 = 46341;

	private const int C5S3 = 36410;

	private const int C6S2 = 25080;

	private const int C7S1 = 12785;

	private const int MaxFrameSize = 8192;

	private const int QuantMax = 4096;

	private readonly QuantRanges[] quantRanges = new QuantRanges[6];

	private readonly ushort[] acScale = new ushort[64];

	private readonly ushort[] dcScale = new ushort[64];

	private int headersRead;

	public ref readonly TheoraInfo Info => ref info;

	public bool KeyFrame => keyFrame;

	public bool HasPicture => output >= 0;

	public bool HeadersDone => headersRead == 3;

	public unsafe byte* Plane(int pli)
	{
		return frameTop[output * 3 + pli];
	}

	public int PlaneStride(int pli)
	{
		if (pli != 0)
		{
			return chromaStride;
		}
		return lumaStride;
	}

	public int PlaneWidth(int pli)
	{
		if (pli != 0)
		{
			return chromaWidth;
		}
		return info.FrameWidth;
	}

	public int PlaneHeight(int pli)
	{
		if (pli != 0)
		{
			return chromaHeight;
		}
		return info.FrameHeight;
	}

	public void Reset()
	{
		references[0] = (references[1] = (references[2] = -1));
		output = -1;
	}

	public unsafe bool Decode(ReadOnlySpan<byte> packet)
	{
		if (!HeadersDone)
		{
			throw new InvalidOperationException("theora: frame before the headers");
		}
		fixed (byte* buffer = packet)
		{
			if (packet.Length == 0)
			{
				keyFrame = false;
				totalCoded = 0;
			}
			else
			{
				bits.Init(buffer, packet.Length);
				ReadFrameHeader();
				if (keyFrame)
				{
					MarkAllIntra();
				}
				else
				{
					ReadCodedFlags();
				}
			}
			if (!keyFrame && (references[0] < 0 || references[1] < 0))
			{
				InitDummyFrame();
			}
			if (totalCoded <= 0)
			{
				return false;
			}
			int i;
			for (i = 0; i == references[0] || i == references[1]; i++)
			{
			}
			references[2] = i;
			referenceData[2] = frameBase[i];
			if (!keyFrame)
			{
				ReadMacroBlockModes();
				ReadMotionVectors();
			}
			ReadBlockQualities();
			ReadResidualTokens();
			Reconstruct(i);
			if (keyFrame)
			{
				references[0] = i;
				referenceData[0] = frameBase[i];
			}
			references[1] = i;
			referenceData[1] = frameBase[i];
			output = i;
			return true;
		}
	}

	private void ReadFrameHeader()
	{
		if (bits.Read1() != 0)
		{
			throw new InvalidDataException("theora: not a data packet");
		}
		keyFrame = bits.Read1() == 0;
		qualities[0] = (byte)bits.Read(6);
		qualityCount = 1;
		if (bits.Read1() != 0)
		{
			qualities[1] = (byte)bits.Read(6);
			qualityCount = 2;
			if (bits.Read1() != 0)
			{
				qualities[2] = (byte)bits.Read(6);
				qualityCount = 3;
			}
		}
		if (keyFrame && bits.Read(3) != 0L)
		{
			throw new NotSupportedException("theora: reserved key frame bits are set");
		}
	}

	private unsafe void InitDummyFrame()
	{
		references[0] = (references[1] = (references[2] = 0));
		referenceData[0] = (referenceData[1] = (referenceData[2] = frameBase[0]));
		new Span<byte>(frameMemory[0], checked((int)frameSize)).Fill(128);
		output = 0;
	}

	private unsafe void MarkAllIntra()
	{
		int num = 0;
		int num2 = 0;
		int i = 0;
		int num3 = 0;
		for (int j = 0; j < 3; j++)
		{
			for (num3 += planes[j].SuperBlockCount; i < num3; i++)
			{
				for (int k = 0; k < 4; k++)
				{
					if ((superBlockFlags[i * 3 + 2] & (1 << k)) == 0)
					{
						continue;
					}
					for (int l = 0; l < 4; l++)
					{
						int num4 = superBlockMaps[i * 16 + k * 4 + l];
						if (num4 >= 0)
						{
							fragments[num4].Coded = 1;
							fragments[num4].Reference = 2;
							fragments[num4].Mode = 1;
							codedFragments[num++] = num4;
						}
					}
				}
			}
			codedCounts[j] = num - num2;
			num2 = num;
		}
		totalCoded = num;
	}

	private unsafe int ReadSuperBlockRun()
	{
		int num = bits.Token(TheoraTables.SuperBlockRunTree);
		if (num >= 16)
		{
			int num2 = num & 0x1F;
			num = 6 + num2 + (int)bits.Read(num - num2 >> 4);
		}
		return num;
	}

	private unsafe int ReadPartialFlags()
	{
		int num = bits.Read1();
		int num2 = 0;
		int num3 = 0;
		while (num2 < superBlockCount)
		{
			int num4 = ReadSuperBlockRun();
			bool flag = num4 >= 4129;
			do
			{
				superBlockFlags[num2 * 3 + 1] = (byte)num;
				superBlockFlags[num2 * 3] = 0;
				num3 += num;
				num2++;
			}
			while (--num4 > 0 && num2 < superBlockCount);
			num = ((!flag || num2 >= superBlockCount) ? (num ^ 1) : bits.Read1());
		}
		return num3;
	}

	private unsafe void ReadFullFlags()
	{
		int i;
		for (i = 0; superBlockFlags[i * 3 + 1] != 0; i++)
		{
		}
		int num = bits.Read1();
		do
		{
			int num2 = ReadSuperBlockRun();
			bool flag = num2 >= 4129;
			for (; i < superBlockCount; i++)
			{
				if (superBlockFlags[i * 3 + 1] == 0)
				{
					if (num2-- <= 0)
					{
						break;
					}
					superBlockFlags[i * 3] = (byte)num;
				}
			}
			num = ((!flag || i >= superBlockCount) ? (num ^ 1) : bits.Read1());
		}
		while (i < superBlockCount);
	}

	private unsafe void ReadCodedFlags()
	{
		int num = ReadPartialFlags();
		if (num < superBlockCount)
		{
			ReadFullFlags();
		}
		int num2 = ((num > 0) ? (bits.Read1() ^ 1) : 0);
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		int num6 = 0;
		int i = 0;
		int num7 = 0;
		int* ptr = codedFragments + fragmentCount;
		for (int j = 0; j < 3; j++)
		{
			for (num7 += planes[j].SuperBlockCount; i < num7; i++)
			{
				byte* ptr2 = superBlockFlags + i * 3;
				for (int k = 0; k < 4; k++)
				{
					if ((ptr2[2] & (1 << k)) == 0)
					{
						continue;
					}
					int num8 = 0;
					for (int l = 0; l < 4; l++)
					{
						int num9 = superBlockMaps[i * 16 + k * 4 + l];
						if (num9 < 0)
						{
							continue;
						}
						int num10;
						if (*ptr2 != 0)
						{
							num10 = 1;
						}
						else if (ptr2[1] == 0)
						{
							num10 = 0;
						}
						else
						{
							if (num3 <= 0)
							{
								num3 = bits.Token(TheoraTables.BlockRunTree);
								num2 ^= 1;
							}
							num3--;
							num10 = num2;
						}
						if (num10 != 0)
						{
							codedFragments[num4++] = num9;
						}
						else
						{
							*(ptr - ++num5) = num9;
						}
						num8 |= num10;
						fragments[num9].Coded = (byte)num10;
						fragments[num9].Reference = 3;
					}
					if (j == 0)
					{
						macroBlockModes[(i << 2) | k] = (sbyte)num8;
					}
				}
			}
			codedCounts[j] = num4 - num6;
			num6 = num4;
		}
		totalCoded = num4;
	}

	private unsafe void ReadMacroBlockModes()
	{
		Span<byte> span = stackalloc byte[8];
		int num = (int)bits.Read(3);
		ReadOnlySpan<byte> readOnlySpan;
		if (num == 0)
		{
			span.Clear();
			for (int i = 0; i < 8; i++)
			{
				span[(int)bits.Read(3)] = TheoraTables.ModeAlphabets[6][i];
			}
			readOnlySpan = span;
		}
		else
		{
			readOnlySpan = TheoraTables.ModeAlphabets[num - 1];
		}
		short* tree = ((num == 7) ? TheoraTables.ClcModeTree : TheoraTables.VlcModeTree);
		for (int j = 0; j < macroBlockCount; j++)
		{
			if (macroBlockModes[j] > 0)
			{
				macroBlockModes[j] = (sbyte)readOnlySpan[bits.Token(tree)];
			}
		}
	}

	private static short PackMv(int x, int y)
	{
		return (short)((x & 0xFF) | (y * 256));
	}

	private static int MvX(short mv)
	{
		return (sbyte)mv;
	}

	private static int MvY(short mv)
	{
		return mv >> 8;
	}

	private unsafe short ReadMv(short* tree)
	{
		int x = bits.Token(tree) - 32;
		int y = bits.Token(tree) - 32;
		return PackMv(x, y);
	}

	private static int RoundedDivide(int dividend, int shift, int rounding)
	{
		return dividend + ((dividend < 0) ? (-1) : 0) + rounding >> shift;
	}

	private unsafe void SetChromaMvs(short* chroma, short* luma)
	{
		switch (info.PixelFormat)
		{
		case 0:
			*chroma = PackMv(RoundedDivide(MvX(*luma) + MvX(luma[1]) + MvX(luma[2]) + MvX(luma[3]), 2, 2), RoundedDivide(MvY(*luma) + MvY(luma[1]) + MvY(luma[2]) + MvY(luma[3]), 2, 2));
			return;
		case 2:
			*chroma = PackMv(RoundedDivide(MvX(*luma) + MvX(luma[1]), 1, 1), RoundedDivide(MvY(*luma) + MvY(luma[1]), 1, 1));
			chroma[2] = PackMv(RoundedDivide(MvX(luma[2]) + MvX(luma[3]), 1, 1), RoundedDivide(MvY(luma[2]) + MvY(luma[3]), 1, 1));
			return;
		}
		for (int i = 0; i < 4; i++)
		{
			chroma[i] = luma[i];
		}
	}

	private unsafe void ReadMotionVectors()
	{
		short* tree = ((bits.Read1() != 0) ? TheoraTables.ClcMvTree : TheoraTables.VlcMvTree);
		byte[] array = TheoraTables.MacroBlockSlots[info.PixelFormat];
		short num = 0;
		short num2 = 0;
		short* ptr = stackalloc short[4];
		short* ptr2 = stackalloc short[4];
		for (int i = 0; i < macroBlockCount; i++)
		{
			int num3 = macroBlockModes[i];
			if (num3 < 0)
			{
				continue;
			}
			int* ptr3 = macroBlockMaps + i * 12;
			if (num3 == 7)
			{
				num2 = num;
				for (int j = 0; j < 4; j++)
				{
					int num4 = ptr3[j];
					if (fragments[num4].Coded != 0)
					{
						fragments[num4].Reference = 1;
						fragments[num4].Mode = 7;
						num = (ptr[j] = ReadMv(tree));
						fragmentMvs[num4] = ptr[j];
					}
					else
					{
						ptr[j] = 0;
					}
				}
				SetChromaMvs(ptr2, ptr);
				for (int k = 4; k < array.Length; k++)
				{
					int num5 = ptr3[(int)array[k]];
					if (fragments[num5].Coded != 0)
					{
						fragments[num5].Reference = 1;
						fragments[num5].Mode = 7;
						fragmentMvs[num5] = ptr2[array[k] & 3];
					}
				}
				continue;
			}
			short num6 = 0;
			switch (num3)
			{
			case 2:
				num2 = num;
				num = (num6 = ReadMv(tree));
				break;
			case 3:
				num6 = num;
				break;
			case 4:
				num6 = num2;
				num2 = num;
				num = num6;
				break;
			case 6:
				num6 = ReadMv(tree);
				break;
			}
			int num7 = ((num3 == 1) ? 2 : ((num3 < 5 || num3 == 7) ? 1 : 0));
			for (int l = 0; l < array.Length; l++)
			{
				int num8 = ptr3[(int)array[l]];
				if (fragments[num8].Coded != 0)
				{
					fragments[num8].Reference = (byte)num7;
					fragments[num8].Mode = (byte)num3;
					fragmentMvs[num8] = num6;
				}
			}
		}
	}

	private unsafe void ReadBlockQualities()
	{
		if (qualityCount == 1)
		{
			for (int i = 0; i < totalCoded; i++)
			{
				fragments[codedFragments[i]].Quality = 0;
			}
			return;
		}
		int num = bits.Read1();
		int num2 = 0;
		int num3 = 0;
		while (num3 < totalCoded)
		{
			int num4 = ReadSuperBlockRun();
			bool flag = num4 >= 4129;
			do
			{
				fragments[codedFragments[num3++]].Quality = (byte)num;
				num2 += num;
			}
			while (--num4 > 0 && num3 < totalCoded);
			num = ((!flag || num3 >= totalCoded) ? (num ^ 1) : bits.Read1());
		}
		if (qualityCount != 3 || num2 <= 0)
		{
			return;
		}
		for (num3 = 0; fragments[codedFragments[num3]].Quality == 0; num3++)
		{
		}
		num = bits.Read1();
		do
		{
			int num5 = ReadSuperBlockRun();
			bool flag2 = num5 >= 4129;
			for (; num3 < totalCoded; num3++)
			{
				int num6 = codedFragments[num3];
				if (fragments[num6].Quality != 0)
				{
					if (num5-- <= 0)
					{
						break;
					}
					byte* quality = &fragments[num6].Quality;
					*quality += (byte)num;
				}
			}
			num = ((!flag2 || num3 >= totalCoded) ? (num ^ 1) : bits.Read1());
		}
		while (num3 < totalCoded);
	}

	public unsafe void Dispose()
	{
		foreach (nint allocation in allocations)
		{
			NativeMemory.Free((void*)allocation);
		}
		allocations.Clear();
		headersRead = 0;
		output = -1;
	}

	private unsafe void Reconstruct(int self)
	{
		int* ptr = stackalloc int[192];
		long* ptr2 = stackalloc long[192];
		int* ptr3 = stackalloc int[12];
		int** ptr4 = stackalloc int*[3];
		int** ptr5 = stackalloc int*[3];
		int* ptr6 = stackalloc int[3];
		int* ptr7 = stackalloc int[3];
		tokenStarts.CopyTo(new Span<int>(ptr, 192));
		eobRunStarts.CopyTo(new Span<long>(ptr2, 192));
		new Span<int>(ptr3, 12).Clear();
		int* ptr8 = codedFragments;
		int* ptr9 = codedFragments + fragmentCount;
		for (int i = 0; i < 3; i++)
		{
			ptr4[i] = ptr8;
			ptr5[i] = ptr9;
			ptr8 += codedCounts[i];
			ptr9 += codedCounts[i] - planes[i].FragmentCount;
		}
		int num = loopFilterLimits[qualities[0]];
		if (num != 0)
		{
			InitLoopFilter(num);
		}
		new Span<short>(coefficients, 64).Clear();
		int num2 = (((info.PixelFormat & 2) == 0) ? 1 : 0);
		int num3 = 4 << num2;
		int num4 = 0;
		int num5 = 1;
		int num6 = 0;
		while (num5 != 0)
		{
			num5 = ((num6 + num3 < planes[0].VerticalFragments) ? 1 : 0);
			for (int j = 0; j < 3; j++)
			{
				int num7 = ((j != 0) ? num2 : 0);
				ptr6[j] = num6 >> num7;
				ptr7[j] = System.Math.Min(planes[j].VerticalFragments, ptr6[j] + (num3 >> num7));
				int num8 = UnpredictDc(j, ptr6[j], ptr7[j], ptr3 + j * 4);
				int num9 = (ptr7[j] - ptr6[j]) * planes[j].HorizontalFragments - num8;
				ReconstructFragments(j, ptr4[j], num8, ptr + j * 64, ptr2 + j * 64);
				ptr4[j] += num8;
				if (num9 > 0)
				{
					ptr5[j] -= num9;
					CopyFragments(referenceData[2], referenceData[1], strides[j], ptr5[j], num9);
				}
				int num10 = 0;
				int num11 = 0;
				if (num != 0)
				{
					num10 = num4;
					num11 = num5;
					LoopFilterRows(j, ptr6[j] - num10, ptr7[j] - num11);
				}
				FillSideBorders(self, j, (ptr6[j] - num10 << 3) - (num10 << 1), (ptr7[j] - num11 << 3) - (num11 << 1));
			}
			num4 = 1;
			num6 += num3;
		}
		for (int k = 0; k < 3; k++)
		{
			FillCapBorders(self, k);
		}
	}

	private unsafe int UnpredictDc(int pli, int rowStart, int rowEnd, int* last)
	{
		int horizontalFragments = planes[pli].HorizontalFragments;
		int num = 0;
		TheoraFragment* ptr = fragments + planes[pli].FragmentOffset + rowStart * horizontalFragments;
		for (int i = rowStart; i < rowEnd; i++)
		{
			if (i == 0)
			{
				int num2 = 0;
				while (num2 < horizontalFragments)
				{
					if (ptr->Coded != 0)
					{
						last[(int)ptr->Reference] = (ptr->Dc += (short)last[(int)ptr->Reference]);
						num++;
					}
					num2++;
					ptr++;
				}
				continue;
			}
			TheoraFragment* ptr2 = ptr - horizontalFragments;
			int num3 = -1;
			int num4 = -1;
			int num5 = ptr2->Reference;
			int num6 = 0;
			while (num6 < horizontalFragments)
			{
				int num7 = ((num6 + 1 >= horizontalFragments) ? (-1) : ptr2[1].Reference);
				if (ptr->Coded != 0)
				{
					int reference = ptr->Reference;
					int num8;
					switch ((int)(((num3 == reference) ? 1u : 0u) | (uint)((num4 == reference) ? 2 : 0) | (uint)((num5 == reference) ? 4 : 0) | (uint)((num7 == reference) ? 8 : 0)))
					{
					default:
						num8 = last[reference];
						break;
					case 1:
					case 3:
						num8 = ptr[-1].Dc;
						break;
					case 2:
						num8 = ptr2[-1].Dc;
						break;
					case 4:
					case 6:
					case 12:
						num8 = ptr2->Dc;
						break;
					case 5:
						num8 = (ptr[-1].Dc + ptr2->Dc) / 2;
						break;
					case 8:
						num8 = ptr2[1].Dc;
						break;
					case 9:
					case 11:
					case 13:
						num8 = (75 * ptr[-1].Dc + 53 * ptr2[1].Dc) / 128;
						break;
					case 10:
						num8 = (ptr2[-1].Dc + ptr2[1].Dc) / 2;
						break;
					case 14:
						num8 = (3 * (ptr2[-1].Dc + ptr2[1].Dc) + 10 * ptr2->Dc) / 16;
						break;
					case 7:
					case 15:
					{
						int dc = ptr[-1].Dc;
						int dc2 = ptr2[-1].Dc;
						int dc3 = ptr2->Dc;
						num8 = (29 * (dc + dc3) - 26 * dc2) / 32;
						if (System.Math.Abs(num8 - dc3) > 128)
						{
							num8 = dc3;
						}
						else if (System.Math.Abs(num8 - dc) > 128)
						{
							num8 = dc;
						}
						else if (System.Math.Abs(num8 - dc2) > 128)
						{
							num8 = dc2;
						}
						break;
					}
					}
					last[reference] = (ptr->Dc += (short)num8);
					num++;
					num3 = reference;
				}
				else
				{
					num3 = -1;
				}
				num4 = num5;
				num5 = num7;
				num6++;
				ptr++;
				ptr2++;
			}
		}
		return num;
	}

	private unsafe void ReconstructFragments(int pli, int* list, int count, int* tokenIndex, long* eobRuns)
	{
		byte* zigZag = TheoraTables.ZigZag;
		int* codeWords = TheoraTables.CodeWords;
		ushort num = *DequantTable(qualities[0], pli, 0);
		ushort num2 = *DequantTable(qualities[0], pli, 1);
		for (int i = 0; i < count; i++)
		{
			int num3 = list[i];
			TheoraFragment* ptr = fragments + num3;
			int num4 = ((ptr->Mode != 1) ? 1 : 0);
			ushort* ptr2 = DequantTable(qualities[ptr->Quality], pli, num4);
			int lastZz = 0;
			int num5 = 0;
			while (num5 < 64)
			{
				lastZz = num5;
				if (eobRuns[num5] != 0L)
				{
					eobRuns[num5]--;
					break;
				}
				int num6 = tokenIndex[num5];
				if (num6 + 3 > tokenCapacity)
				{
					throw new InvalidDataException("theora: token list overrun");
				}
				int num7 = tokens[num6++];
				int num8 = codeWords[num7];
				if (num7 < 15)
				{
					num8 += tokens[num6++] << ExtraBitsPosition(num7);
				}
				long num9 = (num8 >> 8) & 0xFFF;
				if (num7 == 0)
				{
					num9 += tokens[num6++] << 8;
					if (num9 == 0L)
					{
						num9 = long.MaxValue;
					}
				}
				byte b = (byte)num8;
				num8 ^= -(num8 & 0x100000);
				int num10 = num8 >> 21;
				eobRuns[num5] = num9;
				tokenIndex[num5] = num6;
				num5 += b;
				coefficients[(int)zigZag[num5 & 0x7F]] = (short)(num10 * ptr2[num5 & 0x7F]);
				num5 += ((num9 == 0) ? 1 : 0);
			}
			*coefficients = ptr->Dc;
			ReconstructFragment(num3, pli, lastZz, (num4 != 0) ? num2 : num);
		}
	}

	private unsafe void ReconstructFragment(int fragmentIndex, int pli, int lastZz, int dcQuant)
	{
		short* ptr = coefficients + 64;
		if (lastZz < 2)
		{
			short value = (short)(*coefficients * dcQuant + 15 >> 5);
			new Span<short>(ptr, 64).Fill(value);
		}
		else
		{
			*coefficients = (short)(*coefficients * dcQuant);
			if (lastZz <= 3)
			{
				InverseDct3(ptr, coefficients);
			}
			else if (lastZz <= 10)
			{
				InverseDct10(ptr, coefficients);
			}
			else
			{
				InverseDctFull(ptr, coefficients);
			}
		}
		nint num = fragmentOffsets[fragmentIndex];
		int reference = fragments[fragmentIndex].Reference;
		int num2 = strides[pli];
		byte* ptr2 = referenceData[2] + num;
		if (reference == 2)
		{
			for (int i = 0; i < 8; i++)
			{
				for (int j = 0; j < 8; j++)
				{
					ptr2[j] = Clamp255(ptr[i * 8 + j] + 128);
				}
				ptr2 += num2;
			}
			return;
		}
		int num3 = ((pli != 0 && (info.PixelFormat & 1) == 0) ? 64 : 0);
		int num4 = ((pli != 0 && (info.PixelFormat & 2) == 0) ? 64 : 0);
		short mv = fragmentMvs[fragmentIndex];
		int num5 = MvX(mv) + 31;
		int num6 = MvY(mv) + 31;
		byte* ptr3 = referenceData[reference] + num + TheoraTables.MvWhole[num4 + num6] * num2 + TheoraTables.MvWhole[num3 + num5];
		int num7 = TheoraTables.MvFraction[num3 + num5];
		int num8 = TheoraTables.MvFraction[num4 + num6];
		if (num7 != 0 || num8 != 0)
		{
			byte* ptr4 = ptr3 + num8 * num2 + num7;
			for (int k = 0; k < 8; k++)
			{
				for (int l = 0; l < 8; l++)
				{
					ptr2[l] = Clamp255(ptr[k * 8 + l] + (ptr3[l] + ptr4[l] >> 1));
				}
				ptr2 += num2;
				ptr3 += num2;
				ptr4 += num2;
			}
			return;
		}
		for (int m = 0; m < 8; m++)
		{
			for (int n = 0; n < 8; n++)
			{
				ptr2[n] = Clamp255(ptr[m * 8 + n] + ptr3[n]);
			}
			ptr2 += num2;
			ptr3 += num2;
		}
	}

	private static byte Clamp255(int value)
	{
		return (byte)((value >= 0) ? ((value > 255) ? 255u : ((uint)value)) : 0u);
	}

	private unsafe void CopyFragments(byte* target, byte* source, int stride, int* list, int count)
	{
		for (int i = 0; i < count; i++)
		{
			nint num = fragmentOffsets[list[i]];
			byte* ptr = target + num;
			byte* ptr2 = source + num;
			for (int j = 0; j < 8; j++)
			{
				*(long*)ptr = *(long*)ptr2;
				ptr += stride;
				ptr2 += stride;
			}
		}
	}

	private unsafe static void InverseDct8(short* y, short* x)
	{
		int num = 46341 * (short)(*x + x[4]) >> 16;
		int num2 = 46341 * (short)(*x - x[4]) >> 16;
		int num3 = (25080 * x[2] >> 16) - (60547 * x[6] >> 16);
		int num4 = (60547 * x[2] >> 16) + (25080 * x[6] >> 16);
		int num5 = (12785 * x[1] >> 16) - (64277 * x[7] >> 16);
		int num6 = (54491 * x[5] >> 16) - (36410 * x[3] >> 16);
		int num7 = (36410 * x[5] >> 16) + (54491 * x[3] >> 16);
		int num8 = (64277 * x[1] >> 16) + (12785 * x[7] >> 16);
		int num9 = num5 + num6;
		num6 = 46341 * (short)(num5 - num6) >> 16;
		num5 = num9;
		int num10 = num8 + num7;
		num7 = 46341 * (short)(num8 - num7) >> 16;
		num8 = num10;
		int num11 = num + num4;
		num4 = num - num4;
		num = num11;
		int num12 = num2 + num3;
		num3 = num2 - num3;
		num2 = num12;
		int num13 = num7 + num6;
		num6 = num7 - num6;
		num7 = num13;
		*y = (short)(num + num8);
		y[8] = (short)(num2 + num7);
		y[16] = (short)(num3 + num6);
		y[24] = (short)(num4 + num5);
		y[32] = (short)(num4 - num5);
		y[40] = (short)(num3 - num6);
		y[48] = (short)(num2 - num7);
		y[56] = (short)(num - num8);
	}

	private unsafe static void InverseDct8Of4(short* y, short* x)
	{
		int num = 46341 * *x >> 16;
		int num2 = 25080 * x[2] >> 16;
		int num3 = 60547 * x[2] >> 16;
		int num4 = 12785 * x[1] >> 16;
		int num5 = -(36410 * x[3] >> 16);
		int num6 = 54491 * x[3] >> 16;
		int num7 = 64277 * x[1] >> 16;
		int num8 = num4 + num5;
		num5 = 46341 * (short)(num4 - num5) >> 16;
		num4 = num8;
		int num9 = num7 + num6;
		num6 = 46341 * (short)(num7 - num6) >> 16;
		num7 = num9;
		int num10 = num + num2;
		num2 = num - num2;
		int num11 = num + num3;
		num3 = num - num3;
		num = num11;
		int num12 = num6 + num5;
		num5 = num6 - num5;
		num6 = num12;
		*y = (short)(num + num7);
		y[8] = (short)(num10 + num6);
		y[16] = (short)(num2 + num5);
		y[24] = (short)(num3 + num4);
		y[32] = (short)(num3 - num4);
		y[40] = (short)(num2 - num5);
		y[48] = (short)(num10 - num6);
		y[56] = (short)(num - num7);
	}

	private unsafe static void InverseDct8Of3(short* y, short* x)
	{
		int num = 46341 * *x >> 16;
		int num2 = 25080 * x[2] >> 16;
		int num3 = 60547 * x[2] >> 16;
		int num4 = 12785 * x[1] >> 16;
		int num5 = 64277 * x[1] >> 16;
		int num6 = 46341 * num4 >> 16;
		int num7 = 46341 * num5 >> 16;
		int num8 = num + num2;
		num2 = num - num2;
		int num9 = num + num3;
		num3 = num - num3;
		num = num9;
		int num10 = num7 + num6;
		num6 = num7 - num6;
		num7 = num10;
		*y = (short)(num + num5);
		y[8] = (short)(num8 + num7);
		y[16] = (short)(num2 + num6);
		y[24] = (short)(num3 + num4);
		y[32] = (short)(num3 - num4);
		y[40] = (short)(num2 - num6);
		y[48] = (short)(num8 - num7);
		y[56] = (short)(num - num5);
	}

	private unsafe static void InverseDct8Of2(short* y, short* x)
	{
		int num = 46341 * *x >> 16;
		int num2 = 12785 * x[1] >> 16;
		int num3 = 64277 * x[1] >> 16;
		int num4 = 46341 * num2 >> 16;
		int num5 = 46341 * num3 >> 16;
		int num6 = num5 + num4;
		num4 = num5 - num4;
		num5 = num6;
		*y = (short)(num + num3);
		y[8] = (short)(num + num5);
		y[16] = (short)(num + num4);
		y[24] = (short)(num + num2);
		y[32] = (short)(num - num2);
		y[40] = (short)(num - num4);
		y[48] = (short)(num - num5);
		y[56] = (short)(num - num3);
	}

	private unsafe static void InverseDct8Of1(short* y, short* x)
	{
		short num = (short)(46341 * *x >> 16);
		for (int i = 0; i < 8; i++)
		{
			y[i << 3] = num;
		}
	}

	private unsafe static void InverseDct3(short* y, short* x)
	{
		short* ptr = stackalloc short[64];
		InverseDct8Of2(ptr, x);
		InverseDct8Of1(ptr + 1, x + 8);
		for (int i = 0; i < 8; i++)
		{
			InverseDct8Of2(y + i, ptr + i * 8);
		}
		for (int j = 0; j < 64; j++)
		{
			y[j] = (short)(y[j] + 8 >> 4);
		}
		*x = (x[1] = (x[8] = 0));
	}

	private unsafe static void InverseDct10(short* y, short* x)
	{
		short* ptr = stackalloc short[64];
		InverseDct8Of4(ptr, x);
		InverseDct8Of3(ptr + 1, x + 8);
		InverseDct8Of2(ptr + 2, x + 16);
		InverseDct8Of1(ptr + 3, x + 24);
		for (int i = 0; i < 8; i++)
		{
			InverseDct8Of4(y + i, ptr + i * 8);
		}
		for (int j = 0; j < 64; j++)
		{
			y[j] = (short)(y[j] + 8 >> 4);
		}
		*x = (x[1] = (x[2] = (x[3] = (x[8] = (x[9] = (x[10] = (x[16] = (x[17] = (x[24] = 0)))))))));
	}

	private unsafe static void InverseDctFull(short* y, short* x)
	{
		short* ptr = stackalloc short[64];
		for (int i = 0; i < 8; i++)
		{
			InverseDct8(ptr + i, x + i * 8);
		}
		for (int j = 0; j < 8; j++)
		{
			InverseDct8(y + j, ptr + j * 8);
		}
		for (int k = 0; k < 64; k++)
		{
			y[k] = (short)(y[k] + 8 >> 4);
		}
		new Span<short>(x, 64).Clear();
	}

	private unsafe void InitLoopFilter(int limit)
	{
		new Span<sbyte>(boundingValues, 256).Clear();
		for (int i = 0; i < limit; i++)
		{
			if (127 - i - limit >= 0)
			{
				boundingValues[127 - i - limit] = (sbyte)(i - limit);
			}
			boundingValues[127 - i] = (sbyte)(-i);
			boundingValues[127 + i] = (sbyte)i;
			if (127 + i + limit < 256)
			{
				boundingValues[127 + i + limit] = (sbyte)(limit - i);
			}
		}
	}

	private unsafe static void FilterVerticalEdge(byte* pixels, int stride, sbyte* bounding)
	{
		pixels -= 2;
		for (int i = 0; i < 8; i++)
		{
			int num = *pixels - pixels[3] + 3 * (pixels[2] - pixels[1]);
			num = bounding[num + 4 >> 3];
			pixels[1] = Clamp255(pixels[1] + num);
			pixels[2] = Clamp255(pixels[2] - num);
			pixels += stride;
		}
	}

	private unsafe static void FilterHorizontalEdge(byte* pixels, int stride, sbyte* bounding)
	{
		pixels -= stride * 2;
		for (int i = 0; i < 8; i++)
		{
			int num = pixels[i] - pixels[stride * 3 + i] + 3 * (pixels[stride * 2 + i] - pixels[stride + i]);
			num = bounding[num + 4 >> 3];
			pixels[stride + i] = Clamp255(pixels[stride + i] + num);
			pixels[stride * 2 + i] = Clamp255(pixels[stride * 2 + i] - num);
		}
	}

	private unsafe void LoopFilterRows(int pli, int rowStart, int rowEnd)
	{
		sbyte* bounding = boundingValues + 127;
		int horizontalFragments = planes[pli].HorizontalFragments;
		int fragmentOffset = planes[pli].FragmentOffset;
		int num = fragmentOffset + planes[pli].FragmentCount;
		int i = fragmentOffset + rowStart * horizontalFragments;
		int num2 = fragmentOffset + rowEnd * horizontalFragments;
		int num3 = strides[pli];
		byte* ptr = referenceData[2];
		for (; i < num2; i += horizontalFragments)
		{
			int num4 = i + horizontalFragments;
			for (int j = i; j < num4; j++)
			{
				if (fragments[j].Coded != 0)
				{
					byte* ptr2 = ptr + fragmentOffsets[j];
					if (j > i)
					{
						FilterVerticalEdge(ptr2, num3, bounding);
					}
					if (i > fragmentOffset)
					{
						FilterHorizontalEdge(ptr2, num3, bounding);
					}
					if (j + 1 < num4 && fragments[j + 1].Coded == 0)
					{
						FilterVerticalEdge(ptr2 + 8, num3, bounding);
					}
					if (j + horizontalFragments < num && fragments[j + horizontalFragments].Coded == 0)
					{
						FilterHorizontalEdge(ptr2 + num3 * 8, num3, bounding);
					}
				}
			}
		}
	}

	private unsafe void FillSideBorders(int frame, int pli, int rowStart, int rowEnd)
	{
		int num = ((pli == 0) ? 16 : chromaPaddingX);
		int num2 = PlaneWidth(pli);
		int num3 = strides[pli];
		byte* ptr = PlaneBase(frame, pli) + (long)rowStart * (long)num3;
		for (byte* ptr2 = PlaneBase(frame, pli) + (long)rowEnd * (long)num3; ptr != ptr2; ptr += num3)
		{
			new Span<byte>(ptr - num, num).Fill(*ptr);
			new Span<byte>(ptr + num2, num).Fill(ptr[num2 - 1]);
		}
	}

	private unsafe void FillCapBorders(int frame, int pli)
	{
		int num = ((pli == 0) ? 16 : chromaPaddingX);
		int num2 = ((pli == 0) ? 16 : chromaPaddingY);
		int num3 = strides[pli];
		int length = PlaneWidth(pli) + (num << 1);
		byte* ptr = PlaneBase(frame, pli) - num;
		byte* ptr2 = ptr + (long)(PlaneHeight(pli) - 1) * (long)num3;
		for (int i = 0; i < num2; i++)
		{
			new ReadOnlySpan<byte>(ptr, length).CopyTo(new Span<byte>(ptr - num3, length));
			new ReadOnlySpan<byte>(ptr2, length).CopyTo(new Span<byte>(ptr2 + num3, length));
			ptr -= num3;
			ptr2 += num3;
		}
	}

	public static bool IsHeader(ReadOnlySpan<byte> packet)
	{
		if (packet.Length > 0)
		{
			return (packet[0] & 0x80) != 0;
		}
		return false;
	}

	public unsafe void ReadHeader(ReadOnlySpan<byte> packet)
	{
		if (packet.Length < 7 || packet[0] != 128 + headersRead || !packet.Slice(1, 6).SequenceEqual("theora"u8))
		{
			throw new InvalidDataException("theora: header packet out of order");
		}
		fixed (byte* ptr = packet)
		{
			bits.Init(ptr + 7, packet.Length - 7);
			switch (headersRead)
			{
			case 0:
				ReadInfo();
				break;
			case 2:
			{
				ReadQuantParameters();
				for (int i = 0; i < 80; i++)
				{
					huffmanTables[i] = Track(TheoraHuffman.ReadTree(ref bits));
				}
				Allocate();
				break;
			}
			}
		}
		headersRead++;
	}

	private void ReadInfo()
	{
		info.VersionMajor = (int)bits.Read(8);
		info.VersionMinor = (int)bits.Read(8);
		info.VersionSubminor = (int)bits.Read(8);
		if (info.VersionMajor > 3 || (info.VersionMajor == 3 && info.VersionMinor > 2))
		{
			throw new NotSupportedException($"theora bitstream version {info.VersionMajor}.{info.VersionMinor} is not supported");
		}
		info.FrameWidth = (int)bits.Read(16) << 4;
		info.FrameHeight = (int)bits.Read(16) << 4;
		info.PictureWidth = (int)bits.Read(24);
		info.PictureHeight = (int)bits.Read(24);
		info.PictureX = (int)bits.Read(8);
		int num = (int)bits.Read(8);
		info.FpsNumerator = (uint)bits.Read(32);
		info.FpsDenominator = (uint)bits.Read(32);
		if (info.FrameWidth == 0 || info.FrameHeight == 0 || info.PictureWidth + info.PictureX > info.FrameWidth || info.PictureHeight + num > info.FrameHeight || info.FpsNumerator == 0 || info.FpsDenominator == 0)
		{
			throw new InvalidDataException("theora: bad info header");
		}
		if (info.FrameWidth > 8192 || info.FrameHeight > 8192)
		{
			throw new NotSupportedException($"theora frame of {info.FrameWidth}x{info.FrameHeight} is too large");
		}
		info.PictureY = info.FrameHeight - info.PictureHeight - num;
		bits.Read(24);
		bits.Read(24);
		info.ColorSpace = (int)bits.Read(8);
		bits.Read(24);
		bits.Read(6);
		info.KeyframeGranuleShift = (int)bits.Read(5);
		info.PixelFormat = (int)bits.Read(2);
		if (info.PixelFormat == 1 || bits.Read(3) != 0L || bits.BytesLeft < 0)
		{
			throw new InvalidDataException("theora: bad info header");
		}
	}

	private static int BitLength(int value)
	{
		int num = 0;
		while (value != 0)
		{
			value >>= 1;
			num++;
		}
		return num;
	}

	private void ReadQuantParameters()
	{
		int count = (int)bits.Read(3);
		for (int i = 0; i < 64; i++)
		{
			loopFilterLimits[i] = (byte)bits.Read(count);
		}
		count = (int)bits.Read(4) + 1;
		for (int j = 0; j < 64; j++)
		{
			acScale[j] = (ushort)bits.Read(count);
		}
		count = (int)bits.Read(4) + 1;
		for (int k = 0; k < 64; k++)
		{
			dcScale[k] = (ushort)bits.Read(count);
		}
		byte[][] array = new byte[(int)bits.Read(9) + 1][];
		for (int l = 0; l < array.Length; l++)
		{
			array[l] = new byte[64];
			for (int m = 0; m < 64; m++)
			{
				array[l][m] = (byte)bits.Read(8);
			}
		}
		count = BitLength(array.Length - 1);
		Span<int> span = stackalloc int[64];
		Span<int> span2 = stackalloc int[64];
		for (int n = 0; n < 6; n++)
		{
			int num = n / 3;
			int num2 = n % 3;
			if (n > 0 && bits.Read1() == 0)
			{
				int num3 = ((num > 0 && bits.Read1() != 0) ? ((num - 1) * 3 + num2) : (n - 1));
				this.quantRanges[n] = this.quantRanges[num3];
				continue;
			}
			span2[0] = (int)bits.Read(count);
			int num4 = 0;
			int num5 = 0;
			while (num5 < 63)
			{
				int num6 = (int)bits.Read(BitLength(62 - num5)) + 1;
				span[num4] = num6;
				num5 += num6;
				span2[++num4] = (int)bits.Read(count);
			}
			if (num5 > 63)
			{
				throw new InvalidDataException("theora: bad quantizer ranges");
			}
			QuantRanges quantRanges = new QuantRanges
			{
				Count = num4,
				Sizes = span.Slice(0, num4).ToArray(),
				BaseMatrices = new byte[num4 + 1][]
			};
			for (int num7 = 0; num7 <= num4; num7++)
			{
				if (span2[num7] >= array.Length)
				{
					throw new InvalidDataException("theora: bad base matrix index");
				}
				quantRanges.BaseMatrices[num7] = array[span2[num7]];
			}
			this.quantRanges[n] = quantRanges;
		}
	}

	private unsafe ushort* DequantTable(int qi, int pli, int qti)
	{
		return dequantTables + ((qi * 3 + pli) * 2 + qti) * 64;
	}

	private unsafe void BuildDequantTables()
	{
		dequantTables = (ushort*)Track(NativeMemory.AllocZeroed(24640u, 2u));
		Span<byte> destination = stackalloc byte[64];
		for (int i = 0; i < 2; i++)
		{
			uint val = ((i == 0) ? 16u : 32u);
			uint val2 = ((i == 0) ? 8u : 16u);
			for (int j = 0; j < 3; j++)
			{
				QuantRanges quantRanges = this.quantRanges[i * 3 + j];
				int num = 0;
				for (int k = 0; k <= quantRanges.Count; k++)
				{
					quantRanges.BaseMatrices[k].CopyTo(destination);
					int num2 = num;
					int num3 = ((k == quantRanges.Count) ? (num + 1) : (num + quantRanges.Sizes[k]));
					while (true)
					{
						ushort* ptr = DequantTable(num, j, i);
						uint val3 = (uint)(dcScale[num] * destination[0]) / 100u << 2;
						*ptr = (ushort)System.Math.Max(val, System.Math.Min(val3, 4096u));
						for (int l = 1; l < 64; l++)
						{
							val3 = (uint)(acScale[num] * destination[TheoraTables.ZigZag[l]]) / 100u << 2;
							ptr[l] = (ushort)System.Math.Max(val2, System.Math.Min(val3, 4096u));
						}
						if (++num >= num3)
						{
							break;
						}
						int num4 = quantRanges.Sizes[k];
						for (int m = 0; m < 64; m++)
						{
							destination[m] = (byte)((2 * ((num3 - num) * quantRanges.BaseMatrices[k][m] + (num - num2) * quantRanges.BaseMatrices[k + 1][m]) + num4) / (2 * num4));
						}
					}
				}
			}
		}
	}

	private unsafe void* Track(void* memory)
	{
		allocations.Add((nint)memory);
		return memory;
	}

	private unsafe short* Track(short* memory)
	{
		return (short*)Track((void*)memory);
	}

	private unsafe void Allocate()
	{
		BuildDequantTables();
		int num = (((info.PixelFormat & 1) == 0) ? 1 : 0);
		int num2 = (((info.PixelFormat & 2) == 0) ? 1 : 0);
		int num3 = info.FrameWidth >> 3;
		int num4 = info.FrameHeight >> 3;
		int num5 = num3 + num >> num;
		int num6 = num4 + num2 >> num2;
		int num7 = num3 * num4;
		int num8 = num5 * num6;
		int num9 = (num3 + 3 >> 2) * (num4 + 3 >> 2);
		int num10 = (num5 + 3 >> 2) * (num6 + 3 >> 2);
		planes[0] = new TheoraPlane
		{
			HorizontalFragments = num3,
			VerticalFragments = num4,
			FragmentCount = num7,
			SuperBlockCount = num9
		};
		planes[1] = new TheoraPlane
		{
			HorizontalFragments = num5,
			VerticalFragments = num6,
			FragmentOffset = num7,
			FragmentCount = num8,
			SuperBlockOffset = num9,
			SuperBlockCount = num10
		};
		planes[2] = planes[1];
		planes[2].FragmentOffset = num7 + num8;
		planes[2].SuperBlockOffset = num9 + num10;
		fragmentCount = num7 + 2 * num8;
		superBlockCount = num9 + 2 * num10;
		macroBlockCount = num9 << 2;
		fragments = (TheoraFragment*)Track(NativeMemory.AllocZeroed((nuint)fragmentCount, (nuint)sizeof(TheoraFragment)));
		fragmentMvs = (short*)Track(NativeMemory.AllocZeroed((nuint)fragmentCount, 2u));
		superBlockMaps = (int*)Track(NativeMemory.Alloc((nuint)superBlockCount * (nuint)16u, 4u));
		superBlockFlags = (byte*)Track(NativeMemory.AllocZeroed((nuint)superBlockCount, 3u));
		macroBlockMaps = (int*)Track(NativeMemory.Alloc((nuint)macroBlockCount * (nuint)12u, 4u));
		macroBlockModes = (sbyte*)Track(NativeMemory.AllocZeroed((nuint)macroBlockCount, 1u));
		codedFragments = (int*)Track(NativeMemory.AllocZeroed((nuint)fragmentCount, 4u));
		tokenCapacity = 129 * fragmentCount + 16;
		tokens = (byte*)Track(NativeMemory.AllocZeroed((nuint)tokenCapacity, 1u));
		coefficients = (short*)Track(NativeMemory.AllocZeroed(128u, 2u));
		boundingValues = (sbyte*)Track(NativeMemory.AllocZeroed(256u, 1u));
		for (int i = 0; i < 3; i++)
		{
			MapSuperBlocks(in planes[i]);
		}
		MapMacroBlocks();
		AllocateFrames(num, num2);
	}

	private unsafe void MapSuperBlocks(in TheoraPlane plane)
	{
		ReadOnlySpan<byte> readOnlySpan = new byte[32]
		{
			0, 0, 0, 1, 3, 2, 3, 3, 0, 3,
			0, 2, 3, 1, 3, 0, 1, 0, 1, 3,
			2, 0, 2, 3, 1, 1, 1, 2, 2, 1,
			2, 2
		};
		int num = plane.SuperBlockOffset;
		int num2 = plane.FragmentOffset;
		for (int i = 0; i < plane.VerticalFragments; i += 4)
		{
			int num3 = System.Math.Min(plane.VerticalFragments - i, 4);
			int num4 = 0;
			while (num4 < plane.HorizontalFragments)
			{
				int num5 = System.Math.Min(plane.HorizontalFragments - num4, 4);
				int* ptr = superBlockMaps + num * 16;
				new Span<int>(ptr, 16).Fill(-1);
				int num6 = num2 + num4;
				for (int j = 0; j < num3; j++)
				{
					for (int k = 0; k < num5; k++)
					{
						ptr[readOnlySpan[(j * 4 + k) * 2] * 4 + readOnlySpan[(j * 4 + k) * 2 + 1]] = num6 + k;
					}
					num6 += plane.HorizontalFragments;
				}
				for (int l = 0; l < 4; l++)
				{
					if (ptr[l * 4 + (l & (l << 1))] >= 0)
					{
						byte* num7 = superBlockFlags + (num * 3 + 2);
						*num7 |= (byte)(1 << l);
					}
				}
				num4 += 4;
				num++;
			}
			num2 += plane.HorizontalFragments << 2;
		}
	}

	private unsafe void MapMacroBlocks()
	{
		new Span<int>(macroBlockMaps, macroBlockCount * 12).Fill(-1);
		ref TheoraPlane reference = ref planes[0];
		int horizontalFragments = planes[1].HorizontalFragments;
		int num = 0;
		for (int i = 0; i < reference.VerticalFragments; i += 4)
		{
			int num2 = 0;
			while (num2 < reference.HorizontalFragments)
			{
				for (int j = 0; j < 2; j++)
				{
					for (int k = 0; k < 2; k++)
					{
						int num3 = (num << 2) | TheoraTables.MacroBlockOrder[j * 2 + k];
						int num4 = num2 | (k << 1);
						int num5 = i | (j << 1);
						if (num4 >= reference.HorizontalFragments || num5 >= reference.VerticalFragments)
						{
							macroBlockModes[num3] = -1;
							continue;
						}
						int* ptr = macroBlockMaps + num3 * 12;
						for (int l = 0; l < 2; l++)
						{
							for (int m = 0; m < 2; m++)
							{
								ptr[(l << 1) | m] = (num5 + l) * reference.HorizontalFragments + num4 + m;
							}
						}
						switch (info.PixelFormat)
						{
						case 0:
						{
							int num6 = (num5 >> 1) * horizontalFragments + (num4 >> 1);
							ptr[4] = num6 + planes[1].FragmentOffset;
							ptr[8] = num6 + planes[2].FragmentOffset;
							break;
						}
						case 2:
						{
							int num7 = num5 * horizontalFragments + (num4 >> 1);
							for (int num8 = 0; num8 < 2; num8++)
							{
								ptr[4 + (num8 << 1)] = num7 + planes[1].FragmentOffset;
								ptr[8 + (num8 << 1)] = num7 + planes[2].FragmentOffset;
								num7 += horizontalFragments;
							}
							break;
						}
						default:
						{
							for (int n = 0; n < 4; n++)
							{
								ptr[4 + n] = ptr[n] + planes[1].FragmentOffset;
								ptr[8 + n] = ptr[n] + planes[2].FragmentOffset;
							}
							break;
						}
						}
					}
				}
				num2 += 4;
				num++;
			}
		}
	}

	private unsafe void AllocateFrames(int horizontalDecimation, int verticalDecimation)
	{
		lumaStride = info.FrameWidth + 32;
		int num = info.FrameHeight + 32;
		chromaStride = ((lumaStride >> horizontalDecimation) + 15) & -16;
		int num2 = num >> verticalDecimation;
		chromaWidth = info.FrameWidth >> horizontalDecimation;
		chromaHeight = info.FrameHeight >> verticalDecimation;
		chromaPaddingX = 16 >> horizontalDecimation;
		chromaPaddingY = 16 >> verticalDecimation;
		long num3 = (long)lumaStride * (long)num;
		long num4 = (long)chromaStride * (long)num2;
		frameSize = num3 + 2 * num4;
		for (int i = 0; i < 3; i++)
		{
			byte* ptr = (byte*)Track(NativeMemory.AllocZeroed((nuint)frameSize));
			frameMemory[i] = ptr;
			frameTop[i * 3] = ptr + 16 + 16L * (long)lumaStride;
			frameTop[i * 3 + 1] = ptr + num3 + chromaPaddingX + (long)chromaPaddingY * (long)chromaStride;
			frameTop[i * 3 + 2] = frameTop[i * 3 + 1] + num4;
			frameBase[i] = frameTop[i * 3] + (long)(info.FrameHeight - 1) * (long)lumaStride;
		}
		strides[0] = -lumaStride;
		strides[1] = (strides[2] = -chromaStride);
		fragmentOffsets = (nint*)Track(NativeMemory.Alloc((nuint)fragmentCount, (nuint)sizeof(nint)));
		int num5 = 0;
		for (int j = 0; j < 3; j++)
		{
			byte* ptr2 = PlaneBase(0, j);
			for (int k = 0; k < planes[j].VerticalFragments; k++)
			{
				for (int l = 0; l < planes[j].HorizontalFragments; l++)
				{
					fragmentOffsets[num5++] = (nint)(ptr2 + l * 8 - frameBase[0]);
				}
				ptr2 += strides[j] * 8;
			}
		}
		references[0] = (references[1] = (references[2] = -1));
	}

	private unsafe byte* PlaneBase(int frame, int pli)
	{
		return frameTop[frame * 3 + pli] + (long)(((pli == 0) ? info.FrameHeight : chromaHeight) - 1) * (long)((pli == 0) ? lumaStride : chromaStride);
	}

	private static int ExtraBitsPosition(int token)
	{
		if (token >= 2)
		{
			if (token >= 12)
			{
				return 0;
			}
			return 21;
		}
		return 8;
	}

	private unsafe void ReadResidualTokens()
	{
		long* ptr = stackalloc long[192];
		int* ptr2 = stackalloc int[2];
		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 64; j++)
			{
				ptr[i * 64 + j] = codedCounts[i];
			}
		}
		*ptr2 = (int)bits.Read(4);
		ptr2[1] = (int)bits.Read(4);
		long eobs = ReadDcTokens(ptr2, ptr);
		*ptr2 = (int)bits.Read(4);
		ptr2[1] = (int)bits.Read(4);
		int k = 1;
		for (int l = 1; l < 5; l++)
		{
			*ptr2 += 16;
			ptr2[1] += 16;
			for (; k < TheoraTables.HuffmanListMax[l]; k++)
			{
				eobs = ReadAcTokens(k, ptr2, ptr, eobs);
			}
		}
	}

	private unsafe int ReadToken(short* tree, ref int ti)
	{
		if (ti + 3 > tokenCapacity)
		{
			throw new InvalidDataException("theora: more tokens than the frame can hold");
		}
		int num = bits.Token(tree);
		tokens[ti++] = (byte)num;
		int num2 = 0;
		if (num < 15)
		{
			num2 = (int)bits.Read(TheoraTables.TokenExtraBits[num]);
			tokens[ti++] = (byte)num2;
			if (num == 0)
			{
				tokens[ti++] = (byte)(num2 >> 8);
			}
			num2 <<= ExtraBitsPosition(num);
		}
		return TheoraTables.CodeWords[num] + num2;
	}

	private unsafe long ReadDcTokens(int* huffman, long* tokensLeft)
	{
		long* ptr = stackalloc long[64];
		int num = 0;
		int num2 = 0;
		long num3 = 0L;
		int ti = 0;
		for (int i = 0; i < 3; i++)
		{
			num += codedCounts[i];
			new Span<long>(ptr, 64).Clear();
			eobRunStarts[i * 64] = num3;
			tokenStarts[i * 64] = ti;
			long num4 = System.Math.Min(num3, num - num2);
			long num5 = num4;
			num3 -= num4;
			while (num4-- > 0)
			{
				fragments[codedFragments[num2++]].Dc = 0;
			}
			short* tree = huffmanTables[huffman[i + 1 >> 1]];
			while (num2 < num)
			{
				int num6 = ReadToken(tree, ref ti);
				num3 = (num6 >> 8) & 0xFFF;
				if (num6 == 0)
				{
					num3 = long.MaxValue;
				}
				if (num3 != 0L)
				{
					num4 = System.Math.Min(num3, num - num2);
					num5 += num4;
					num3 -= num4;
					while (num4-- > 0)
					{
						fragments[codedFragments[num2++]].Dc = 0;
					}
					continue;
				}
				byte b = (byte)num6;
				num6 ^= -(num6 & 0x100000);
				int num7 = num6 >> 21;
				if (b != 0)
				{
					num7 = 0;
				}
				ptr[(int)b]++;
				fragments[codedFragments[num2++]].Dc = (short)num7;
			}
			ptr[63] += num5;
			int num8 = 63;
			while (num8-- > 0)
			{
				ptr[num8] += ptr[num8 + 1];
			}
			int num9 = 64;
			while (num9-- > 0)
			{
				tokensLeft[i * 64 + num9] -= ptr[num9];
			}
		}
		tokenCount = ti;
		return num3;
	}

	private unsafe int ReadAcTokens(int zz, int* huffman, long* tokensLeft, long eobs)
	{
		long* ptr = stackalloc long[64];
		int ti = tokenCount;
		for (int i = 0; i < 3; i++)
		{
			eobRunStarts[i * 64 + zz] = eobs;
			tokenStarts[i * 64 + zz] = ti;
			ulong num = (ulong)tokensLeft[i * 64 + zz];
			new Span<long>(ptr, 64).Clear();
			long num2 = 0L;
			ulong num3 = 0uL;
			short* tree = huffmanTables[huffman[i + 1 >> 1]];
			while ((ulong)((long)num3 + eobs) < num)
			{
				num3 += (ulong)eobs;
				num2 += eobs;
				int num4 = ReadToken(tree, ref ti);
				byte b = (byte)num4;
				eobs = (num4 >> 8) & 0xFFF;
				if (num4 == 0)
				{
					eobs = long.MaxValue;
				}
				if (eobs == 0L)
				{
					ptr[(int)b]++;
					num3++;
				}
			}
			num2 += (long)(num - num3);
			eobs -= (long)(num - num3);
			ptr[63] += num2;
			int num5 = 63;
			while (num5-- > 0)
			{
				ptr[num5] += ptr[num5 + 1];
			}
			int num6 = 64 - zz;
			while (num6-- > 0)
			{
				tokensLeft[i * 64 + zz + num6] -= ptr[num6];
			}
		}
		tokenCount = ti;
		return (int)eobs;
	}
}
