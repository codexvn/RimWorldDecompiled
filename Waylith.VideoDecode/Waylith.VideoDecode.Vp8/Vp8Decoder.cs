using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;

namespace Waylith.VideoDecode.Vp8;

internal sealed class Vp8Decoder : IDisposable
{
	private const int ContextSize = 9;

	private const int CoefficientCount = 400;

	private const int ProbsPerType = 264;

	private readonly Vp8Frame[] frames = new Vp8Frame[4];

	private int newIndex;

	private int lastIndex;

	private int goldenIndex;

	private int altRefIndex;

	private int mbCols;

	private int mbRows;

	private int modeStride;

	private bool decodedKeyFrame;

	private unsafe void* scratch;

	private unsafe void* memory;

	private unsafe Vp8ModeInfo* modeInfo;

	private unsafe byte* aboveContext;

	private unsafe readonly Vp8FrameContext* context;

	private unsafe readonly Vp8FrameContext* savedContext;

	private unsafe readonly short* coefficients;

	private unsafe readonly byte* eobs;

	private unsafe readonly byte* leftContext;

	private unsafe readonly byte* filterScratch;

	private unsafe readonly byte* bModeProbs;

	private bool keyFrame;

	private bool bilinear;

	private int fullPixelMask;

	private bool simpleFilter;

	private int filterLevel;

	private int sharpness;

	private bool segmentationEnabled;

	private bool updateSegmentMap;

	private bool segmentValuesAbsolute;

	private readonly sbyte[] segmentQuantizer = new sbyte[4];

	private readonly sbyte[] segmentFilterLevel = new sbyte[4];

	private readonly byte[] segmentTreeProbs = new byte[3];

	private bool filterDeltasEnabled;

	private readonly sbyte[] refFilterDeltas = new sbyte[4];

	private readonly sbyte[] modeFilterDeltas = new sbyte[4];

	private int baseQuantizer;

	private int y1DcDelta;

	private int y2DcDelta;

	private int y2AcDelta;

	private int uvDcDelta;

	private int uvAcDelta;

	private readonly short[] dequant = new short[24];

	private bool refreshGolden;

	private bool refreshAltRef;

	private bool refreshLast;

	private bool refreshEntropy;

	private int copyToGolden;

	private int copyToAltRef;

	private readonly int[] signBias = new int[4];

	private bool macroblockSkipEnabled;

	private int probSkipFalse;

	private int probIntra;

	private int probLast;

	private int probGolden;

	private Vp8BoolDecoder header;

	private readonly Vp8BoolDecoder[] tokenDecoders = new Vp8BoolDecoder[8];

	private int partitionCount;

	private const int MaxFilterLevel = 63;

	private readonly byte[] interiorLimits = new byte[64];

	private readonly byte[] blockEdgeLimits = new byte[64];

	private readonly byte[] macroblockEdgeLimits = new byte[64];

	private readonly byte[] filterLevels = new byte[64];

	private int limitsSharpness = -1;

	private const int EdgeMargin = 128;

	private int toLeft;

	private int toRight;

	private int toTop;

	private int toBottom;

	private const int FilterScratchSize = 672;

	public int Width { get; private set; }

	public int Height { get; private set; }

	public Vp8Frame Shown { get; private set; }

	private static ReadOnlySpan<byte> ModeClass => new byte[10] { 1, 1, 1, 1, 0, 2, 2, 1, 2, 3 };

	public unsafe Vp8Decoder()
	{
		int num = 1520 + 2 * sizeof(Vp8FrameContext) + 16;
		scratch = NativeMemory.AllocZeroed((nuint)num);
		coefficients = (short*)scratch;
		eobs = (byte*)(coefficients + 400);
		leftContext = eobs + 32;
		filterScratch = leftContext + 16;
		context = (Vp8FrameContext*)(filterScratch + 672);
		savedContext = context + 1;
		bModeProbs = (byte*)(savedContext + 1);
		Vp8Tables.BModeProbs.CopyTo(new Span<byte>(bModeProbs, 9));
		UpdateSharpness();
	}

	public unsafe bool Decode(ReadOnlySpan<byte> data)
	{
		if (data.Length == 0)
		{
			return false;
		}
		fixed (byte* data2 = data)
		{
			return DecodeFrame(data2, data.Length);
		}
	}

	private static InvalidDataException Corrupt(string what)
	{
		return new InvalidDataException("corrupt vp8 frame: " + what);
	}

	private unsafe bool DecodeFrame(byte* data, int size)
	{
		if (size < 3)
		{
			throw Corrupt("truncated packet");
		}
		byte* ptr = data + size;
		keyFrame = (*data & 1) == 0;
		int num = (*data >> 1) & 7;
		bool result = ((*data >> 4) & 1) != 0;
		int num2 = (*data | (data[1] << 8) | (data[2] << 16)) >> 5;
		if (num2 == 0)
		{
			throw Corrupt("empty first partition");
		}
		data += 3;
		bool flag = (uint)(num - 1) <= 2u;
		bilinear = flag;
		fullPixelMask = ((num == 3) ? (-8) : (-1));
		if (keyFrame)
		{
			if (ptr - data < 7)
			{
				throw Corrupt("truncated key frame header");
			}
			if (*data != 157 || data[1] != 1 || data[2] != 42)
			{
				throw Corrupt("bad key frame sync code");
			}
			int num3 = (data[3] | (data[4] << 8)) & 0x3FFF;
			int num4 = (data[5] | (data[6] << 8)) & 0x3FFF;
			data += 7;
			if (num3 == 0 || num4 == 0)
			{
				throw Corrupt("zero sized picture");
			}
			if (num3 != Width || num4 != Height || frames[0] == null)
			{
				Allocate(num3, num4);
			}
		}
		else if (!decodedKeyFrame)
		{
			throw Corrupt("stream does not start with a key frame");
		}
		if (ptr - data < num2)
		{
			throw Corrupt("truncated first partition");
		}
		newIndex = Array.FindIndex(frames, (Vp8Frame f) => f.References == 0);
		frames[newIndex].References = 1;
		try
		{
			ReadFrameHeader(data, ptr, num2);
			ReadModes();
			new Span<byte>(aboveContext, mbCols * 9).Clear();
			DecodeRows(frames[newIndex]);
		}
		catch
		{
			frames[newIndex].References--;
			throw;
		}
		decodedKeyFrame = true;
		if (!refreshEntropy)
		{
			*context = *savedContext;
		}
		SwapFrames();
		return result;
	}

	private unsafe void Allocate(int width, int height)
	{
		FreeFrames();
		Width = width;
		Height = height;
		int num = (width + 15) & -16;
		int num2 = (height + 15) & -16;
		for (int i = 0; i < frames.Length; i++)
		{
			frames[i] = new Vp8Frame(num, num2)
			{
				References = ((i != 0) ? 1 : 0)
			};
		}
		newIndex = 0;
		lastIndex = 1;
		goldenIndex = 2;
		altRefIndex = 3;
		mbCols = num >> 4;
		mbRows = num2 >> 4;
		modeStride = mbCols + 1;
		nuint num3 = (nuint)((mbCols + 1) * (mbRows + 1) * sizeof(Vp8ModeInfo));
		memory = NativeMemory.AllocZeroed(num3 + (nuint)(mbCols * 9));
		modeInfo = (Vp8ModeInfo*)((byte*)memory + (nint)modeStride * (nint)sizeof(Vp8ModeInfo) + sizeof(Vp8ModeInfo));
		aboveContext = (byte*)memory + num3;
		decodedKeyFrame = false;
		Shown = null;
	}

	private unsafe void ReadFrameHeader(byte* data, byte* end, int firstPartitionSize)
	{
		if (keyFrame)
		{
			Vp8Tables.DefaultMvProbs.CopyTo(new Span<byte>(context->Mv, 38));
			Vp8Tables.YModeProbs.CopyTo(new Span<byte>(context->YMode, 4));
			Vp8Tables.UvModeProbs.CopyTo(new Span<byte>(context->UvMode, 3));
			Vp8Tables.DefaultCoefProbs.CopyTo(new Span<byte>(context->Coef, 1056));
			Array.Clear(segmentQuantizer);
			Array.Clear(segmentFilterLevel);
			segmentValuesAbsolute = false;
			Array.Clear(refFilterDeltas);
			Array.Clear(modeFilterDeltas);
			refreshGolden = true;
			refreshAltRef = true;
			copyToGolden = 0;
			copyToAltRef = 0;
			signBias[2] = 0;
			signBias[3] = 0;
		}
		ref Vp8BoolDecoder reference = ref header;
		reference.Start(data, end - data);
		if (keyFrame)
		{
			reference.ReadBit();
			reference.ReadBit();
		}
		segmentationEnabled = reference.ReadBit() != 0;
		updateSegmentMap = false;
		if (segmentationEnabled)
		{
			updateSegmentMap = reference.ReadBit() != 0;
			if (reference.ReadBit() != 0)
			{
				segmentValuesAbsolute = reference.ReadBit() != 0;
				ReadSegmentValues(ref reference, segmentQuantizer, 7);
				ReadSegmentValues(ref reference, segmentFilterLevel, 6);
			}
			if (updateSegmentMap)
			{
				for (int i = 0; i < 3; i++)
				{
					segmentTreeProbs[i] = ((reference.ReadBit() != 0) ? ((byte)reference.ReadLiteral(8)) : byte.MaxValue);
				}
			}
		}
		simpleFilter = reference.ReadBit() != 0;
		filterLevel = reference.ReadLiteral(6);
		sharpness = reference.ReadLiteral(3);
		filterDeltasEnabled = reference.ReadBit() != 0;
		if (filterDeltasEnabled && reference.ReadBit() != 0)
		{
			ReadFilterDeltas(ref reference, refFilterDeltas);
			ReadFilterDeltas(ref reference, modeFilterDeltas);
		}
		SetupPartitions(ref reference, data + firstPartitionSize, end);
		baseQuantizer = reference.ReadLiteral(7);
		y1DcDelta = ReadQuantizerDelta(ref reference);
		y2DcDelta = ReadQuantizerDelta(ref reference);
		y2AcDelta = ReadQuantizerDelta(ref reference);
		uvDcDelta = ReadQuantizerDelta(ref reference);
		uvAcDelta = ReadQuantizerDelta(ref reference);
		BuildDequantizers();
		if (!keyFrame)
		{
			refreshGolden = reference.ReadBit() != 0;
			refreshAltRef = reference.ReadBit() != 0;
			copyToGolden = ((!refreshGolden) ? reference.ReadLiteral(2) : 0);
			copyToAltRef = ((!refreshAltRef) ? reference.ReadLiteral(2) : 0);
			if (copyToAltRef == 3 || copyToGolden == 3)
			{
				throw Corrupt("bad reference copy flag");
			}
			signBias[2] = reference.ReadBit();
			signBias[3] = reference.ReadBit();
		}
		refreshEntropy = reference.ReadBit() != 0;
		if (!refreshEntropy)
		{
			*savedContext = *context;
		}
		refreshLast = keyFrame || reference.ReadBit() != 0;
		ReadOnlySpan<byte> coefUpdateProbs = Vp8Tables.CoefUpdateProbs;
		byte* ptr = context->Coef;
		for (int j = 0; j < 1056; j++)
		{
			if (reference.Read(coefUpdateProbs[j]) != 0)
			{
				ptr[j] = (byte)reference.ReadLiteral(8);
			}
		}
	}

	private static void ReadSegmentValues(ref Vp8BoolDecoder bits, sbyte[] values, int width)
	{
		for (int i = 0; i < values.Length; i++)
		{
			values[i] = 0;
			if (bits.ReadBit() != 0)
			{
				int num = bits.ReadLiteral(width);
				values[i] = (sbyte)((bits.ReadBit() != 0) ? (-num) : num);
			}
		}
	}

	private static void ReadFilterDeltas(ref Vp8BoolDecoder bits, sbyte[] deltas)
	{
		for (int i = 0; i < deltas.Length; i++)
		{
			if (bits.ReadBit() != 0)
			{
				int num = bits.ReadLiteral(6);
				deltas[i] = (sbyte)((bits.ReadBit() != 0) ? (-num) : num);
			}
		}
	}

	private static int ReadQuantizerDelta(ref Vp8BoolDecoder bits)
	{
		if (bits.ReadBit() == 0)
		{
			return 0;
		}
		int num = bits.ReadLiteral(4);
		if (bits.ReadBit() == 0)
		{
			return num;
		}
		return -num;
	}

	private unsafe void SetupPartitions(ref Vp8BoolDecoder bits, byte* sizes, byte* end)
	{
		partitionCount = 1 << bits.ReadLiteral(2);
		byte* ptr = sizes + 3 * (partitionCount - 1);
		if (ptr > end)
		{
			throw Corrupt("truncated partition sizes");
		}
		for (int i = 0; i < partitionCount; i++)
		{
			long num = end - ptr;
			long num2 = num;
			if (i < partitionCount - 1)
			{
				num2 = sizes[i * 3] | (sizes[i * 3 + 1] << 8) | (sizes[i * 3 + 2] << 16);
				if (num > 0 && (num2 == 0L || num2 > num))
				{
					throw Corrupt($"bad length for partition {i + 1}");
				}
				num2 = System.Math.Min(num2, num);
			}
			tokenDecoders[i].Start(ptr, num2);
			ptr += num2;
		}
	}

	private void BuildDequantizers()
	{
		ReadOnlySpan<byte> dcQLookup = Vp8Tables.DcQLookup;
		ReadOnlySpan<short> acQLookup = Vp8Tables.AcQLookup;
		for (int i = 0; i < 4; i++)
		{
			int num = baseQuantizer;
			if (segmentationEnabled)
			{
				num = System.Math.Clamp(segmentValuesAbsolute ? segmentQuantizer[i] : (num + segmentQuantizer[i]), 0, 127);
			}
			Span<short> span = dequant.AsSpan(i * 6, 6);
			span[0] = dcQLookup[System.Math.Clamp(num + y1DcDelta, 0, 127)];
			span[1] = acQLookup[num];
			span[2] = (short)(dcQLookup[System.Math.Clamp(num + y2DcDelta, 0, 127)] * 2);
			span[3] = (short)System.Math.Max(8, acQLookup[System.Math.Clamp(num + y2AcDelta, 0, 127)] * 101581 >> 16);
			span[4] = System.Math.Min((short)132, (short)dcQLookup[System.Math.Clamp(num + uvDcDelta, 0, 127)]);
			span[5] = acQLookup[System.Math.Clamp(num + uvAcDelta, 0, 127)];
		}
	}

	private void SwapFrames()
	{
		if (copyToAltRef != 0)
		{
			Retarget(ref altRefIndex, (copyToAltRef == 1) ? lastIndex : goldenIndex);
		}
		if (copyToGolden != 0)
		{
			Retarget(ref goldenIndex, (copyToGolden == 1) ? lastIndex : altRefIndex);
		}
		if (refreshGolden)
		{
			Retarget(ref goldenIndex, newIndex);
		}
		if (refreshAltRef)
		{
			Retarget(ref altRefIndex, newIndex);
		}
		if (refreshLast)
		{
			Retarget(ref lastIndex, newIndex);
		}
		Shown = frames[newIndex];
		frames[newIndex].References--;
	}

	private void Retarget(ref int slot, int index)
	{
		if (frames[slot].References > 0)
		{
			frames[slot].References--;
		}
		slot = index;
		frames[index].References++;
	}

	private unsafe void DecodeRows(Vp8Frame frame)
	{
		int yStride = frame.YStride;
		int uvStride = frame.UvStride;
		if (filterLevel != 0)
		{
			PrepareLoopFilter();
		}
		frame.SetIntraTopLine();
		Vp8ModeInfo* ptr = modeInfo;
		int num = 0;
		for (int i = 0; i < mbRows; i++)
		{
			ref Vp8BoolDecoder tokens = ref tokenDecoders[i & (partitionCount - 1)];
			new Span<byte>(leftContext, 9).Clear();
			byte* ptr2 = frame.Y + i * 16 * yStride;
			byte* ptr3 = frame.U + i * 8 * uvStride;
			byte* ptr4 = frame.V + i * 8 * uvStride;
			for (int j = 0; j < 16; j++)
			{
				ptr2[j * yStride - 1] = 129;
			}
			for (int k = 0; k < 8; k++)
			{
				ptr3[k * uvStride - 1] = 129;
				ptr4[k * uvStride - 1] = 129;
			}
			byte* ptr5 = aboveContext;
			for (int l = 0; l < mbCols; l++)
			{
				DecodeMacroblock(frame, ptr, ref tokens, ptr5, ptr2, ptr3, ptr4, i, l);
				ptr2 += 16;
				ptr3 += 8;
				ptr4 += 8;
				ptr5 += 9;
				ptr++;
			}
			ptr++;
			for (int m = 14; m < 16; m++)
			{
				new Span<byte>(ptr2 + m * yStride, 4).Fill(ptr2[m * yStride - 1]);
			}
			for (int n = 6; n < 8; n++)
			{
				new Span<byte>(ptr3 + n * uvStride, 4).Fill(ptr3[n * uvStride - 1]);
				new Span<byte>(ptr4 + n * uvStride, 4).Fill(ptr4[n * uvStride - 1]);
			}
			if (filterLevel != 0)
			{
				if (i > 0)
				{
					FilterRow(frame, i - 1);
					if (i > 1)
					{
						frame.ExtendRowSides(num++);
					}
				}
			}
			else if (i > 0)
			{
				frame.ExtendRowSides(num++);
			}
		}
		if (filterLevel != 0)
		{
			FilterRow(frame, mbRows - 1);
		}
		while (num < mbRows)
		{
			frame.ExtendRowSides(num++);
		}
		frame.ExtendTopAndBottom();
	}

	private unsafe void DecodeMacroblock(Vp8Frame frame, Vp8ModeInfo* info, ref Vp8BoolDecoder tokens, byte* above, byte* y, byte* u, byte* v, int row, int col)
	{
		int yStride = frame.YStride;
		int uvStride = frame.UvStride;
		int mode = info->Mode;
		if (info->Skip != 0)
		{
			int length = ((info->Is4x4 != 0) ? 8 : 9);
			new Span<byte>(above, length).Clear();
			new Span<byte>(leftContext, length).Clear();
		}
		else if (tokens.ReadPastEnd)
		{
			new Span<byte>(eobs, 25).Clear();
		}
		else
		{
			info->Skip = ((ReadTokens(ref tokens, info, above) == 0) ? ((byte)1) : ((byte)0));
		}
		bool flag = info->Skip != 0;
		if (info->RefFrame == 0)
		{
			PredictIntra(u, uvStride, 8, info->UvMode, row > 0, col > 0);
			PredictIntra(v, uvStride, 8, info->UvMode, row > 0, col > 0);
			if (mode != 4)
			{
				PredictIntra(y, yStride, 16, mode, row > 0, col > 0);
			}
			else
			{
				if (flag)
				{
					new Span<byte>(eobs, 25).Clear();
				}
				uint value = Unsafe.ReadUnaligned<uint>(y - yStride + 16);
				Unsafe.WriteUnaligned(y + 3 * yStride + 16, value);
				Unsafe.WriteUnaligned(y + 7 * yStride + 16, value);
				Unsafe.WriteUnaligned(y + 11 * yStride + 16, value);
				for (int i = 0; i < 16; i++)
				{
					byte* dst = y + (i >> 2) * 4 * yStride + (i & 3) * 4;
					PredictIntra4x4(dst, yStride, info->Blocks[i]);
					if (eobs[i] > 1)
					{
						Vp8Transform.InverseDctAdd(coefficients + i * 16, dst, yStride);
					}
					else if (eobs[i] != 0)
					{
						Vp8Transform.DcAdd(coefficients[i * 16], dst, yStride);
					}
				}
			}
		}
		else
		{
			PredictInter(frame, info, y, u, v, row, col);
		}
		if (flag)
		{
			return;
		}
		if (mode != 4)
		{
			if (mode != 9)
			{
				if (eobs[24] > 1)
				{
					Vp8Transform.InverseWalsh(coefficients + 384, coefficients);
				}
				else
				{
					Vp8Transform.InverseWalshDc(coefficients[384], coefficients);
				}
			}
			for (int j = 0; j < 16; j++)
			{
				byte* dst2 = y + (j >> 2) * 4 * yStride + (j & 3) * 4;
				if (eobs[j] > 1)
				{
					Vp8Transform.InverseDctAdd(coefficients + j * 16, dst2, yStride);
				}
				else
				{
					Vp8Transform.DcAdd(coefficients[j * 16], dst2, yStride);
				}
			}
		}
		for (int k = 0; k < 8; k++)
		{
			byte* dst3 = ((k < 4) ? u : v) + ((k >> 1) & 1) * 4 * uvStride + (k & 1) * 4;
			if (eobs[16 + k] > 1)
			{
				Vp8Transform.InverseDctAdd(coefficients + (16 + k) * 16, dst3, uvStride);
			}
			else
			{
				Vp8Transform.DcAdd(coefficients[(16 + k) * 16], dst3, uvStride);
			}
		}
		new Span<short>(coefficients, 400).Clear();
	}

	private unsafe int ReadTokens(ref Vp8BoolDecoder bits, Vp8ModeInfo* info, byte* above)
	{
		byte* ptr = context->Coef;
		byte* ptr2 = leftContext;
		int num = 0;
		int num2 = 0;
		fixed (short* ptr3 = &dequant[(segmentationEnabled ? info->SegmentId : 0) * 6])
		{
			byte* probs;
			if (info->Is4x4 == 0)
			{
				int num3 = ReadCoefficients(ref bits, ptr + 264, above[8] + ptr2[8], 0, coefficients + 384, ptr3[2], ptr3[3]);
				above[8] = (ptr2[8] = ((num3 > 0) ? ((byte)1) : ((byte)0)));
				eobs[24] = (byte)num3;
				num += num3 - 16;
				probs = ptr;
				num2 = 1;
			}
			else
			{
				probs = ptr + 792;
			}
			for (int i = 0; i < 16; i++)
			{
				byte* ptr4 = above + (i & 3);
				byte* ptr5 = ptr2 + (i >> 2);
				int num4 = ReadCoefficients(ref bits, probs, *ptr4 + *ptr5, num2, coefficients + i * 16, *ptr3, ptr3[1]);
				*ptr4 = (*ptr5 = ((num4 > 0) ? ((byte)1) : ((byte)0)));
				num4 += num2;
				eobs[i] = (byte)num4;
				num += num4;
			}
			byte* probs2 = ptr + 528;
			for (int j = 16; j < 24; j++)
			{
				int num5 = 4 + ((j > 19) ? 2 : 0);
				byte* ptr6 = above + num5 + (j & 1);
				byte* ptr7 = ptr2 + num5 + (((j & 3) > 1) ? 1 : 0);
				int num6 = ReadCoefficients(ref bits, probs2, *ptr6 + *ptr7, 0, coefficients + j * 16, ptr3[4], ptr3[5]);
				*ptr6 = (*ptr7 = ((num6 > 0) ? ((byte)1) : ((byte)0)));
				eobs[j] = (byte)num6;
				num += num6;
			}
		}
		return num;
	}

	private unsafe static int ReadCoefficients(ref Vp8BoolDecoder bits, byte* probs, int neighbours, int n, short* output, int dcFactor, int acFactor)
	{
		byte* coefBands = Vp8Pointers.CoefBands;
		byte* ptr = probs + n * 33 + neighbours * 11;
		if (bits.Read(*ptr) == 0)
		{
			return 0;
		}
		do
		{
			n++;
			if (bits.Read(ptr[1]) == 0)
			{
				ptr = probs + coefBands[n] * 33;
				continue;
			}
			int magnitude;
			if (bits.Read(ptr[2]) == 0)
			{
				ptr = probs + coefBands[n] * 33 + 11;
				magnitude = 1;
			}
			else
			{
				if (bits.Read(ptr[3]) == 0)
				{
					magnitude = ((bits.Read(ptr[4]) != 0) ? (3 + bits.Read(ptr[5])) : 2);
				}
				else if (bits.Read(ptr[6]) == 0)
				{
					if (bits.Read(ptr[7]) == 0)
					{
						magnitude = 5 + bits.Read(159);
					}
					else
					{
						magnitude = 7 + 2 * bits.Read(165);
						magnitude += bits.Read(145);
					}
				}
				else
				{
					int num = bits.Read(ptr[8]);
					int num2 = bits.Read(ptr[9 + num]);
					int num3 = 2 * num + num2;
					magnitude = 0;
					for (byte* ptr2 = Vp8Pointers.Categories[num3]; *ptr2 != 0; ptr2++)
					{
						magnitude += magnitude + bits.Read(*ptr2);
					}
					magnitude += 3 + (8 << num3);
				}
				ptr = probs + coefBands[n] * 33 + 22;
			}
			byte b = Vp8Pointers.ZigZag[n - 1];
			output[(int)b] = (short)(bits.ReadSigned(magnitude) * ((b == 0) ? dcFactor : acFactor));
			if (n == 16 || bits.Read(*ptr) == 0)
			{
				return n;
			}
		}
		while (n != 16);
		return 16;
	}

	private unsafe void FreeFrames()
	{
		Vp8Frame[] array = frames;
		for (int i = 0; i < array.Length; i++)
		{
			array[i]?.Dispose();
		}
		Array.Clear(frames);
		NativeMemory.Free(memory);
		memory = null;
	}

	public unsafe void Dispose()
	{
		FreeFrames();
		NativeMemory.Free(scratch);
		scratch = null;
	}

	private void UpdateSharpness()
	{
		if (limitsSharpness == sharpness)
		{
			return;
		}
		limitsSharpness = sharpness;
		for (int i = 0; i <= 63; i++)
		{
			int num = i >> ((sharpness > 0) ? 1 : 0) >> ((sharpness > 4) ? 1 : 0);
			if (sharpness > 0 && num > 9 - sharpness)
			{
				num = 9 - sharpness;
			}
			if (num < 1)
			{
				num = 1;
			}
			interiorLimits[i] = (byte)num;
			blockEdgeLimits[i] = (byte)(2 * i + num);
			macroblockEdgeLimits[i] = (byte)(2 * (i + 2) + num);
		}
	}

	private void PrepareLoopFilter()
	{
		UpdateSharpness();
		for (int i = 0; i < 4; i++)
		{
			int num = filterLevel;
			if (segmentationEnabled)
			{
				num = System.Math.Clamp(segmentValuesAbsolute ? segmentFilterLevel[i] : (num + segmentFilterLevel[i]), 0, 63);
			}
			Span<byte> span = filterLevels.AsSpan(i * 16, 16);
			if (!filterDeltasEnabled)
			{
				span.Fill((byte)num);
				continue;
			}
			int num2 = num + refFilterDeltas[0];
			span[0] = (byte)System.Math.Clamp(num2 + modeFilterDeltas[0], 0, 63);
			span[1] = (byte)System.Math.Clamp(num2, 0, 63);
			for (int j = 1; j < 4; j++)
			{
				for (int k = 1; k < 4; k++)
				{
					span[j * 4 + k] = (byte)System.Math.Clamp(num + refFilterDeltas[j] + modeFilterDeltas[k], 0, 63);
				}
			}
		}
	}

	private static int HighVarianceThreshold(bool keyFrame, int level)
	{
		if (level >= 40)
		{
			if (!keyFrame)
			{
				return 3;
			}
			return 2;
		}
		if (level >= 20)
		{
			if (!keyFrame)
			{
				return 2;
			}
			return 1;
		}
		return (level >= 15) ? 1 : 0;
	}

	private unsafe void FilterRow(Vp8Frame frame, int row)
	{
		int yStride = frame.YStride;
		int uvStride = frame.UvStride;
		byte* ptr = frame.Y + row * 16 * yStride;
		byte* ptr2 = frame.U + row * 8 * uvStride;
		byte* ptr3 = frame.V + row * 8 * uvStride;
		Vp8ModeInfo* ptr4 = modeInfo + row * modeStride;
		ReadOnlySpan<byte> modeClass = ModeClass;
		int num = 0;
		while (num < mbCols)
		{
			int num2 = filterLevels[ptr4->SegmentId * 16 + ptr4->RefFrame * 4 + modeClass[ptr4->Mode]];
			if (num2 != 0)
			{
				bool flag = ptr4->Mode != 4 && ptr4->Mode != 9 && ptr4->Skip != 0;
				int edgeLimit = macroblockEdgeLimits[num2];
				int edgeLimit2 = blockEdgeLimits[num2];
				if (simpleFilter)
				{
					if (num > 0)
					{
						SimpleEdge(ptr, 1, yStride, edgeLimit);
					}
					if (!flag)
					{
						SimpleEdge(ptr + 4, 1, yStride, edgeLimit2);
						SimpleEdge(ptr + 8, 1, yStride, edgeLimit2);
						SimpleEdge(ptr + 12, 1, yStride, edgeLimit2);
					}
					if (row > 0)
					{
						SimpleEdge(ptr, yStride, 1, edgeLimit);
					}
					if (!flag)
					{
						SimpleEdge(ptr + 4 * yStride, yStride, 1, edgeLimit2);
						SimpleEdge(ptr + 8 * yStride, yStride, 1, edgeLimit2);
						SimpleEdge(ptr + 12 * yStride, yStride, 1, edgeLimit2);
					}
				}
				else
				{
					int interior = interiorLimits[num2];
					int threshold = HighVarianceThreshold(keyFrame, num2);
					if (Vp8Simd.Enabled)
					{
						if (num > 0)
						{
							Vp8Simd.VerticalEdge(ptr, ptr + 8 * yStride, yStride, macroblockEdge: true, edgeLimit, interior, threshold);
							Vp8Simd.VerticalEdge(ptr2, ptr3, uvStride, macroblockEdge: true, edgeLimit, interior, threshold);
						}
						if (!flag)
						{
							Vp8Simd.VerticalEdge(ptr + 4, ptr + 4 + 8 * yStride, yStride, macroblockEdge: false, edgeLimit2, interior, threshold);
							Vp8Simd.VerticalEdge(ptr + 8, ptr + 8 + 8 * yStride, yStride, macroblockEdge: false, edgeLimit2, interior, threshold);
							Vp8Simd.VerticalEdge(ptr + 12, ptr + 12 + 8 * yStride, yStride, macroblockEdge: false, edgeLimit2, interior, threshold);
							Vp8Simd.VerticalEdge(ptr2 + 4, ptr3 + 4, uvStride, macroblockEdge: false, edgeLimit2, interior, threshold);
						}
						if (row > 0)
						{
							Vp8Simd.HorizontalEdge(ptr, yStride, macroblockEdge: true, edgeLimit, interior, threshold);
							Vp8Simd.HorizontalEdgeChroma(ptr2, ptr3, uvStride, macroblockEdge: true, edgeLimit, interior, threshold);
						}
						if (!flag)
						{
							Vp8Simd.HorizontalEdge(ptr + 4 * yStride, yStride, macroblockEdge: false, edgeLimit2, interior, threshold);
							Vp8Simd.HorizontalEdge(ptr + 8 * yStride, yStride, macroblockEdge: false, edgeLimit2, interior, threshold);
							Vp8Simd.HorizontalEdge(ptr + 12 * yStride, yStride, macroblockEdge: false, edgeLimit2, interior, threshold);
							Vp8Simd.HorizontalEdgeChroma(ptr2 + 4 * uvStride, ptr3 + 4 * uvStride, uvStride, macroblockEdge: false, edgeLimit2, interior, threshold);
						}
					}
					else
					{
						if (num > 0)
						{
							MacroblockEdge(ptr, 1, yStride, 16, edgeLimit, interior, threshold);
							MacroblockEdge(ptr2, 1, uvStride, 8, edgeLimit, interior, threshold);
							MacroblockEdge(ptr3, 1, uvStride, 8, edgeLimit, interior, threshold);
						}
						if (!flag)
						{
							BlockEdge(ptr + 4, 1, yStride, 16, edgeLimit2, interior, threshold);
							BlockEdge(ptr + 8, 1, yStride, 16, edgeLimit2, interior, threshold);
							BlockEdge(ptr + 12, 1, yStride, 16, edgeLimit2, interior, threshold);
							BlockEdge(ptr2 + 4, 1, uvStride, 8, edgeLimit2, interior, threshold);
							BlockEdge(ptr3 + 4, 1, uvStride, 8, edgeLimit2, interior, threshold);
						}
						if (row > 0)
						{
							MacroblockEdge(ptr, yStride, 1, 16, edgeLimit, interior, threshold);
							MacroblockEdge(ptr2, uvStride, 1, 8, edgeLimit, interior, threshold);
							MacroblockEdge(ptr3, uvStride, 1, 8, edgeLimit, interior, threshold);
						}
						if (!flag)
						{
							BlockEdge(ptr + 4 * yStride, yStride, 1, 16, edgeLimit2, interior, threshold);
							BlockEdge(ptr + 8 * yStride, yStride, 1, 16, edgeLimit2, interior, threshold);
							BlockEdge(ptr + 12 * yStride, yStride, 1, 16, edgeLimit2, interior, threshold);
							BlockEdge(ptr2 + 4 * uvStride, uvStride, 1, 8, edgeLimit2, interior, threshold);
							BlockEdge(ptr3 + 4 * uvStride, uvStride, 1, 8, edgeLimit2, interior, threshold);
						}
					}
				}
			}
			num++;
			ptr4++;
			ptr += 16;
			ptr2 += 8;
			ptr3 += 8;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static int ClampSigned(int value)
	{
		if (value >= -128)
		{
			if (value <= 127)
			{
				return value;
			}
			return 127;
		}
		return -128;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private unsafe static bool WithinLimits(byte* s, int across, int edgeLimit, int interior)
	{
		byte num = s[-4 * across];
		int num2 = s[-3 * across];
		int num3 = s[-2 * across];
		int num4 = s[-across];
		int num5 = *s;
		int num6 = s[across];
		int num7 = s[2 * across];
		int num8 = s[3 * across];
		if (System.Math.Abs(num - num2) <= interior && System.Math.Abs(num2 - num3) <= interior && System.Math.Abs(num3 - num4) <= interior && System.Math.Abs(num6 - num5) <= interior && System.Math.Abs(num7 - num6) <= interior && System.Math.Abs(num8 - num7) <= interior)
		{
			return System.Math.Abs(num4 - num5) * 2 + System.Math.Abs(num3 - num6) / 2 <= edgeLimit;
		}
		return false;
	}

	private unsafe static void BlockEdge(byte* s, int across, int along, int count, int edgeLimit, int interior, int threshold)
	{
		for (int i = 0; i < count; i++, s += along)
		{
			if (!WithinLimits(s, across, edgeLimit, interior))
			{
				continue;
			}
			int num = (sbyte)(s[-2 * across] ^ 0x80);
			int num2 = (sbyte)(s[-across] ^ 0x80);
			int num3 = (sbyte)(*s ^ 0x80);
			int num4 = (sbyte)(s[across] ^ 0x80);
			int num5;
			int num6;
			if (System.Math.Abs(num - num2) <= threshold)
			{
				num5 = ((System.Math.Abs(num4 - num3) > threshold) ? 1 : 0);
				if (num5 == 0)
				{
					num6 = 0;
					goto IL_0075;
				}
			}
			else
			{
				num5 = 1;
			}
			num6 = ClampSigned(num - num4);
			goto IL_0075;
			IL_0075:
			int num7 = num6;
			num7 = ClampSigned(num7 + 3 * (num3 - num2));
			int num8 = ClampSigned(num7 + 4) >> 3;
			int num9 = ClampSigned(num7 + 3) >> 3;
			*s = (byte)(ClampSigned(num3 - num8) ^ 0x80);
			s[-across] = (byte)(ClampSigned(num2 + num9) ^ 0x80);
			if (num5 == 0)
			{
				num7 = num8 + 1 >> 1;
				s[across] = (byte)(ClampSigned(num4 - num7) ^ 0x80);
				s[-2 * across] = (byte)(ClampSigned(num + num7) ^ 0x80);
			}
		}
	}

	private unsafe static void MacroblockEdge(byte* s, int across, int along, int count, int edgeLimit, int interior, int threshold)
	{
		int num = 0;
		while (num < count)
		{
			if (WithinLimits(s, across, edgeLimit, interior))
			{
				int num2 = (sbyte)(s[-3 * across] ^ 0x80);
				int num3 = (sbyte)(s[-2 * across] ^ 0x80);
				int num4 = (sbyte)(s[-across] ^ 0x80);
				int num5 = (sbyte)(*s ^ 0x80);
				int num6 = (sbyte)(s[across] ^ 0x80);
				int num7 = (sbyte)(s[2 * across] ^ 0x80);
				bool num8 = System.Math.Abs(num3 - num4) > threshold || System.Math.Abs(num6 - num5) > threshold;
				int num9 = ClampSigned(ClampSigned(num3 - num6) + 3 * (num5 - num4));
				if (num8)
				{
					*s = (byte)(ClampSigned(num5 - (ClampSigned(num9 + 4) >> 3)) ^ 0x80);
					s[-across] = (byte)(ClampSigned(num4 + (ClampSigned(num9 + 3) >> 3)) ^ 0x80);
				}
				else
				{
					int num10 = ClampSigned(63 + num9 * 27 >> 7);
					*s = (byte)(ClampSigned(num5 - num10) ^ 0x80);
					s[-across] = (byte)(ClampSigned(num4 + num10) ^ 0x80);
					num10 = ClampSigned(63 + num9 * 18 >> 7);
					s[across] = (byte)(ClampSigned(num6 - num10) ^ 0x80);
					s[-2 * across] = (byte)(ClampSigned(num3 + num10) ^ 0x80);
					num10 = ClampSigned(63 + num9 * 9 >> 7);
					s[2 * across] = (byte)(ClampSigned(num7 - num10) ^ 0x80);
					s[-3 * across] = (byte)(ClampSigned(num2 + num10) ^ 0x80);
				}
			}
			num++;
			s += along;
		}
	}

	private unsafe static void SimpleEdge(byte* s, int across, int along, int edgeLimit)
	{
		int num = 0;
		while (num < 16)
		{
			int num2 = s[-2 * across];
			int num3 = s[-across];
			int num4 = *s;
			int num5 = s[across];
			if (System.Math.Abs(num3 - num4) * 2 + System.Math.Abs(num2 - num5) / 2 <= edgeLimit)
			{
				sbyte num6 = (sbyte)(num2 ^ 0x80);
				int num7 = (sbyte)(num3 ^ 0x80);
				int num8 = (sbyte)(num4 ^ 0x80);
				int num9 = (sbyte)(num5 ^ 0x80);
				int num10 = ClampSigned(ClampSigned(num6 - num9) + 3 * (num8 - num7));
				*s = (byte)(ClampSigned(num8 - (ClampSigned(num10 + 4) >> 3)) ^ 0x80);
				s[-across] = (byte)(ClampSigned(num7 + (ClampSigned(num10 + 3) >> 3)) ^ 0x80);
			}
			num++;
			s += along;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static int PackMv(int row, int col)
	{
		return (ushort)row | (col << 16);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static int MvRow(int mv)
	{
		return (short)mv;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static int MvCol(int mv)
	{
		return mv >> 16;
	}

	private unsafe void ReadModes()
	{
		ref Vp8BoolDecoder reference = ref header;
		macroblockSkipEnabled = reference.ReadBit() != 0;
		probSkipFalse = (macroblockSkipEnabled ? reference.ReadLiteral(8) : 0);
		if (!keyFrame)
		{
			probIntra = reference.ReadLiteral(8);
			probLast = reference.ReadLiteral(8);
			probGolden = reference.ReadLiteral(8);
			if (reference.ReadBit() != 0)
			{
				for (int i = 0; i < 4; i++)
				{
					context->YMode[i] = (byte)reference.ReadLiteral(8);
				}
			}
			if (reference.ReadBit() != 0)
			{
				for (int j = 0; j < 3; j++)
				{
					context->UvMode[j] = (byte)reference.ReadLiteral(8);
				}
			}
			ReadOnlySpan<byte> mvUpdateProbs = Vp8Tables.MvUpdateProbs;
			for (int k = 0; k < 38; k++)
			{
				if (reference.Read(mvUpdateProbs[k]) != 0)
				{
					int num = reference.ReadLiteral(7);
					context->Mv[k] = (byte)((num == 0) ? 1u : ((uint)(num << 1)));
				}
			}
		}
		Vp8ModeInfo* ptr = modeInfo;
		toTop = 0;
		toBottom = (mbRows - 1) * 16 << 3;
		for (int l = 0; l < mbRows; l++)
		{
			toLeft = 0;
			toRight = (mbCols - 1) * 16 << 3;
			for (int m = 0; m < mbCols; m++)
			{
				ReadMacroblockMode(ref reference, ptr);
				toLeft -= 128;
				toRight -= 128;
				ptr++;
			}
			toTop -= 128;
			toBottom -= 128;
			ptr++;
		}
	}

	private unsafe void ReadMacroblockMode(ref Vp8BoolDecoder bits, Vp8ModeInfo* info)
	{
		if (updateSegmentMap)
		{
			if (bits.Read(segmentTreeProbs[0]) != 0)
			{
				info->SegmentId = (byte)(2 + bits.Read(segmentTreeProbs[2]));
			}
			else
			{
				info->SegmentId = (byte)bits.Read(segmentTreeProbs[1]);
			}
		}
		else if (keyFrame)
		{
			info->SegmentId = 0;
		}
		info->Skip = (byte)(macroblockSkipEnabled ? ((uint)bits.Read(probSkipFalse)) : 0u);
		info->Is4x4 = 0;
		if (keyFrame)
		{
			ReadKeyFrameMode(ref bits, info);
		}
		else
		{
			ReadInterFrameMode(ref bits, info);
		}
	}

	private unsafe void ReadKeyFrameMode(ref Vp8BoolDecoder bits, Vp8ModeInfo* info)
	{
		info->RefFrame = 0;
		info->Mode = (byte)bits.ReadTree(Vp8Pointers.KfYModeTree, Vp8Pointers.KfYModeProbs);
		if (info->Mode == 4)
		{
			info->Is4x4 = 1;
			for (int i = 0; i < 16; i++)
			{
				int num = AboveBlockMode(info, i);
				int num2 = LeftBlockMode(info, i);
				info->Blocks[i] = bits.ReadTree(Vp8Pointers.BModeTree, Vp8Pointers.KfBModeProbs + (num * 10 + num2) * 9);
			}
		}
		info->UvMode = (byte)bits.ReadTree(Vp8Pointers.UvModeTree, Vp8Pointers.KfUvModeProbs);
	}

	private unsafe static int BlockModeOf(Vp8ModeInfo* info, int block)
	{
		return info->Mode switch
		{
			4 => info->Blocks[block], 
			1 => 2, 
			2 => 3, 
			3 => 1, 
			_ => 0, 
		};
	}

	private unsafe int AboveBlockMode(Vp8ModeInfo* info, int block)
	{
		if (block >= 4)
		{
			return info->Blocks[block - 4];
		}
		return BlockModeOf(info - modeStride, block + 12);
	}

	private unsafe static int LeftBlockMode(Vp8ModeInfo* info, int block)
	{
		if ((block & 3) != 0)
		{
			return info->Blocks[block - 1];
		}
		return BlockModeOf(info - 1, block + 3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private int Biased(int mv, int neighbourFrame, int frame)
	{
		if (signBias[neighbourFrame] == signBias[frame])
		{
			return mv;
		}
		return PackMv(-MvRow(mv), -MvCol(mv));
	}

	private int ClampToEdges(int mv)
	{
		int row = System.Math.Clamp(MvRow(mv), toTop - 128, toBottom + 128);
		int col = System.Math.Clamp(MvCol(mv), toLeft - 128, toRight + 128);
		return PackMv(row, col);
	}

	private bool BeyondEdges(int mv)
	{
		int num = MvRow(mv);
		int num2 = MvCol(mv);
		if (num2 >= toLeft - 128 && num2 <= toRight + 128 && num >= toTop - 128)
		{
			return num > toBottom + 128;
		}
		return true;
	}

	private unsafe void ReadInterFrameMode(ref Vp8BoolDecoder bits, Vp8ModeInfo* info)
	{
		info->RefFrame = (byte)bits.Read(probIntra);
		if (info->RefFrame == 0)
		{
			info->Mv = 0;
			info->Mode = (byte)bits.ReadTree(Vp8Pointers.YModeTree, context->YMode);
			if (info->Mode == 4)
			{
				info->Is4x4 = 1;
				for (int i = 0; i < 16; i++)
				{
					info->Blocks[i] = bits.ReadTree(Vp8Pointers.BModeTree, bModeProbs);
				}
			}
			info->UvMode = (byte)bits.ReadTree(Vp8Pointers.UvModeTree, context->UvMode);
			return;
		}
		Vp8ModeInfo* ptr = info - modeStride;
		Vp8ModeInfo* ptr2 = info - 1;
		Vp8ModeInfo* ptr3 = ptr - 1;
		byte* modeContexts = Vp8Pointers.ModeContexts;
		info->NeedToClampMvs = 0;
		if (bits.Read(probLast) != 0)
		{
			info->RefFrame = (byte)(2 + bits.Read(probGolden));
		}
		int refFrame = info->RefFrame;
		int* ptr4 = stackalloc int[4];
		int* ptr5 = stackalloc int[4];
		*ptr4 = (ptr4[1] = (ptr4[2] = 0));
		*ptr5 = (ptr5[1] = (ptr5[2] = (ptr5[3] = 0)));
		int num = 0;
		if (ptr->RefFrame != 0)
		{
			if (ptr->Mv != 0)
			{
				ptr4[++num] = Biased(ptr->Mv, ptr->RefFrame, refFrame);
			}
			ptr5[num] += 2;
		}
		if (ptr2->RefFrame != 0)
		{
			if (ptr2->Mv != 0)
			{
				int num2 = Biased(ptr2->Mv, ptr2->RefFrame, refFrame);
				if (num2 != ptr4[num])
				{
					ptr4[++num] = num2;
				}
				ptr5[num] += 2;
			}
			else
			{
				*ptr5 += 2;
			}
		}
		if (ptr3->RefFrame != 0)
		{
			if (ptr3->Mv != 0)
			{
				int num3 = Biased(ptr3->Mv, ptr3->RefFrame, refFrame);
				if (num3 != ptr4[num])
				{
					ptr4[++num] = num3;
				}
				ptr5[num]++;
			}
			else
			{
				(*ptr5)++;
			}
		}
		if (bits.Read(modeContexts[*ptr5 * 4]) == 0)
		{
			info->Mode = 7;
			info->Mv = 0;
			return;
		}
		if (ptr5[3] > 0 && ptr4[num] == ptr4[1])
		{
			ptr5[1]++;
		}
		if (ptr5[2] > ptr5[1])
		{
			ref int reference = ref ptr5[1];
			int* num4 = ptr5 + 2;
			int num5 = ptr5[2];
			int num6 = ptr5[1];
			reference = num5;
			*num4 = num6;
			reference = ref ptr4[1];
			int* num7 = ptr4 + 2;
			num6 = ptr4[2];
			num5 = ptr4[1];
			reference = num6;
			*num7 = num5;
		}
		if (bits.Read(modeContexts[ptr5[1] * 4 + 1]) == 0)
		{
			info->Mode = 5;
			info->Mv = ClampToEdges(ptr4[1]);
			return;
		}
		if (bits.Read(modeContexts[ptr5[2] * 4 + 2]) == 0)
		{
			info->Mode = 6;
			info->Mv = ClampToEdges(ptr4[2]);
			return;
		}
		int best = ClampToEdges(ptr4[(ptr5[1] >= *ptr5) ? 1 : 0]);
		int num8 = (((ptr->Mode == 9) ? 1 : 0) + ((ptr2->Mode == 9) ? 1 : 0)) * 2 + ((ptr3->Mode == 9) ? 1 : 0);
		if (bits.Read(modeContexts[num8 * 4 + 3]) != 0)
		{
			ReadSplitMvs(ref bits, info, ptr2, ptr, best);
			info->Mv = info->Blocks[15];
			info->Mode = 9;
			info->Is4x4 = 1;
		}
		else
		{
			info->Mode = 8;
			info->Mv = ReadMvFrom(ref bits, best);
			info->NeedToClampMvs = (BeyondEdges(info->Mv) ? ((byte)1) : ((byte)0));
		}
	}

	private unsafe int ReadMvFrom(ref Vp8BoolDecoder bits, int best)
	{
		short row = (short)(ReadMvComponent(ref bits, context->Mv) * 2 + MvRow(best));
		short col = (short)(ReadMvComponent(ref bits, context->Mv + 19) * 2 + MvCol(best));
		return PackMv(row, col);
	}

	private unsafe static int ReadMvComponent(ref Vp8BoolDecoder bits, byte* p)
	{
		int num = 0;
		if (bits.Read(*p) != 0)
		{
			for (int i = 0; i < 3; i++)
			{
				num += bits.Read(p[9 + i]) << i;
			}
			for (int num2 = 9; num2 > 3; num2--)
			{
				num += bits.Read(p[9 + num2]) << num2;
			}
			if ((num & 0xFFF0) == 0 || bits.Read(p[12]) != 0)
			{
				num += 8;
			}
		}
		else
		{
			num = bits.ReadTree(Vp8Pointers.SmallMvTree, p + 2);
		}
		if (num != 0 && bits.Read(p[1]) != 0)
		{
			num = -num;
		}
		return num;
	}

	private unsafe void ReadSplitMvs(ref Vp8BoolDecoder bits, Vp8ModeInfo* info, Vp8ModeInfo* left, Vp8ModeInfo* above, int best)
	{
		int num = 3;
		int num2 = 16;
		if (bits.Read(110) != 0)
		{
			num = 2;
			num2 = 4;
			if (bits.Read(111) != 0)
			{
				num = bits.Read(150);
				num2 = 2;
			}
		}
		ReadOnlySpan<byte> readOnlySpan = Vp8Tables.SplitFirstBlock.Slice(num * 16, 16);
		ReadOnlySpan<byte> readOnlySpan2 = Vp8Tables.SplitFillBlocks.Slice(num * 16, 16);
		byte b = Vp8Tables.SplitFillCount[num];
		for (int i = 0; i < num2; i++)
		{
			int num3 = readOnlySpan[i];
			int num4 = (((num3 & 3) != 0) ? info->Blocks[num3 - 1] : ((left->Mode != 9) ? left->Mv : left->Blocks[num3 + 3]));
			int num5 = ((num3 >= 4) ? info->Blocks[num3 - 4] : ((above->Mode != 9) ? above->Mv : above->Blocks[num3 + 12]));
			byte* ptr = Vp8Pointers.SubMvRefProbs + (int)(((uint)(((num5 == 0) ? 4 : 0) | ((num4 == 0) ? 2 : 0)) | ((num4 == num5) ? 1u : 0u)) * 3);
			int num6 = ((bits.Read(*ptr) == 0) ? num4 : ((bits.Read(ptr[1]) == 0) ? num5 : ((bits.Read(ptr[2]) != 0) ? ReadMvFrom(ref bits, best) : 0)));
			if (BeyondEdges(num6))
			{
				info->NeedToClampMvs = 1;
			}
			for (int j = 0; j < b; j++)
			{
				info->Blocks[(int)readOnlySpan2[i * b + j]] = num6;
			}
		}
		info->Partitioning = (byte)num;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static byte Clip(int value)
	{
		return (byte)(((uint)value <= 255u) ? ((uint)value) : ((value >= 0) ? 255u : 0u));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static byte Avg2(int a, int b)
	{
		return (byte)(a + b + 1 >> 1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static byte Avg3(int a, int b, int c)
	{
		return (byte)(a + 2 * b + c + 2 >> 2);
	}

	private unsafe static void Fill(byte* dst, int stride, int size, byte value)
	{
		for (int i = 0; i < size; i++)
		{
			new Span<byte>(dst + i * stride, size).Fill(value);
		}
	}

	private unsafe static void PredictIntra(byte* dst, int stride, int size, int mode, bool hasAbove, bool hasLeft)
	{
		byte* ptr = dst - stride;
		switch (mode)
		{
		case 0:
		{
			int num = 0;
			int num2 = 0;
			if (hasAbove)
			{
				for (int k = 0; k < size; k++)
				{
					num += ptr[k];
				}
				num2 += size;
			}
			if (hasLeft)
			{
				for (int l = 0; l < size; l++)
				{
					num += dst[l * stride - 1];
				}
				num2 += size;
			}
			Fill(dst, stride, size, (byte)((num2 == 0) ? 128u : ((uint)((num + (num2 >> 1)) / num2))));
			return;
		}
		case 1:
		{
			for (int i = 0; i < size; i++)
			{
				Buffer.MemoryCopy(ptr, dst + i * stride, size, size);
			}
			return;
		}
		case 2:
		{
			for (int j = 0; j < size; j++)
			{
				new Span<byte>(dst + j * stride, size).Fill(dst[j * stride - 1]);
			}
			return;
		}
		}
		int num3 = ptr[-1];
		for (int m = 0; m < size; m++)
		{
			byte* ptr2 = dst + m * stride;
			int num4 = ptr2[-1] - num3;
			for (int n = 0; n < size; n++)
			{
				ptr2[n] = Clip(num4 + ptr[n]);
			}
		}
	}

	private unsafe static void PredictIntra4x4(byte* dst, int stride, int mode)
	{
		byte* num = dst - stride;
		int num2 = num[-1];
		int num3 = *num;
		int num4 = num[1];
		int num5 = num[2];
		int num6 = num[3];
		int num7 = num[4];
		int num8 = num[5];
		int num9 = num[6];
		int num10 = num[7];
		int num11 = dst[-1];
		int num12 = dst[stride - 1];
		int num13 = dst[2 * stride - 1];
		int num14 = dst[3 * stride - 1];
		byte* ptr = dst + stride;
		byte* ptr2 = dst + 2 * stride;
		byte* ptr3 = dst + 3 * stride;
		switch (mode)
		{
		case 0:
		{
			uint value2 = (uint)((byte)(num3 + num4 + num5 + num6 + num11 + num12 + num13 + num14 + 4 >> 3) * 16843009);
			Unsafe.WriteUnaligned(dst, value2);
			Unsafe.WriteUnaligned(ptr, value2);
			Unsafe.WriteUnaligned(ptr2, value2);
			Unsafe.WriteUnaligned(ptr3, value2);
			break;
		}
		case 1:
		{
			for (int i = 0; i < 4; i++)
			{
				byte* num15 = dst + i * stride;
				int num16 = num15[-1] - num2;
				*num15 = Clip(num16 + num3);
				num15[1] = Clip(num16 + num4);
				num15[2] = Clip(num16 + num5);
				num15[3] = Clip(num16 + num6);
			}
			break;
		}
		case 2:
		{
			*dst = Avg3(num2, num3, num4);
			dst[1] = Avg3(num3, num4, num5);
			dst[2] = Avg3(num4, num5, num6);
			dst[3] = Avg3(num5, num6, num7);
			uint value = Unsafe.ReadUnaligned<uint>(dst);
			Unsafe.WriteUnaligned(ptr, value);
			Unsafe.WriteUnaligned(ptr2, value);
			Unsafe.WriteUnaligned(ptr3, value);
			break;
		}
		case 3:
			Unsafe.WriteUnaligned(dst, (uint)(Avg3(num2, num11, num12) * 16843009));
			Unsafe.WriteUnaligned(ptr, (uint)(Avg3(num11, num12, num13) * 16843009));
			Unsafe.WriteUnaligned(ptr2, (uint)(Avg3(num12, num13, num14) * 16843009));
			Unsafe.WriteUnaligned(ptr3, (uint)(Avg3(num13, num14, num14) * 16843009));
			break;
		case 4:
			*dst = Avg3(num3, num4, num5);
			dst[1] = (*ptr = Avg3(num4, num5, num6));
			dst[2] = (ptr[1] = (*ptr2 = Avg3(num5, num6, num7)));
			dst[3] = (ptr[2] = (ptr2[1] = (*ptr3 = Avg3(num6, num7, num8))));
			ptr[3] = (ptr2[2] = (ptr3[1] = Avg3(num7, num8, num9)));
			ptr2[3] = (ptr3[2] = Avg3(num8, num9, num10));
			ptr3[3] = Avg3(num9, num10, num10);
			break;
		case 5:
			*ptr3 = Avg3(num12, num13, num14);
			ptr3[1] = (*ptr2 = Avg3(num11, num12, num13));
			ptr3[2] = (ptr2[1] = (*ptr = Avg3(num2, num11, num12)));
			ptr3[3] = (ptr2[2] = (ptr[1] = (*dst = Avg3(num3, num2, num11))));
			ptr2[3] = (ptr[2] = (dst[1] = Avg3(num4, num3, num2)));
			ptr[3] = (dst[2] = Avg3(num5, num4, num3));
			dst[3] = Avg3(num6, num5, num4);
			break;
		case 6:
			*dst = (ptr2[1] = Avg2(num2, num3));
			dst[1] = (ptr2[2] = Avg2(num3, num4));
			dst[2] = (ptr2[3] = Avg2(num4, num5));
			dst[3] = Avg2(num5, num6);
			*ptr3 = Avg3(num13, num12, num11);
			*ptr2 = Avg3(num12, num11, num2);
			*ptr = (ptr3[1] = Avg3(num11, num2, num3));
			ptr[1] = (ptr3[2] = Avg3(num2, num3, num4));
			ptr[2] = (ptr3[3] = Avg3(num3, num4, num5));
			ptr[3] = Avg3(num4, num5, num6);
			break;
		case 7:
			*dst = Avg2(num3, num4);
			dst[1] = (*ptr2 = Avg2(num4, num5));
			dst[2] = (ptr2[1] = Avg2(num5, num6));
			dst[3] = (ptr2[2] = Avg2(num6, num7));
			ptr2[3] = Avg3(num7, num8, num9);
			*ptr = Avg3(num3, num4, num5);
			ptr[1] = (*ptr3 = Avg3(num4, num5, num6));
			ptr[2] = (ptr3[1] = Avg3(num5, num6, num7));
			ptr[3] = (ptr3[2] = Avg3(num6, num7, num8));
			ptr3[3] = Avg3(num8, num9, num10);
			break;
		case 8:
			*dst = (ptr[2] = Avg2(num11, num2));
			*ptr = (ptr2[2] = Avg2(num12, num11));
			*ptr2 = (ptr3[2] = Avg2(num13, num12));
			*ptr3 = Avg2(num14, num13);
			dst[3] = Avg3(num3, num4, num5);
			dst[2] = Avg3(num2, num3, num4);
			dst[1] = (ptr[3] = Avg3(num11, num2, num3));
			ptr[1] = (ptr2[3] = Avg3(num12, num11, num2));
			ptr2[1] = (ptr3[3] = Avg3(num13, num12, num11));
			ptr3[1] = Avg3(num14, num13, num12);
			break;
		default:
			*dst = Avg2(num11, num12);
			dst[2] = (*ptr = Avg2(num12, num13));
			ptr[2] = (*ptr2 = Avg2(num13, num14));
			dst[1] = Avg3(num11, num12, num13);
			dst[3] = (ptr[1] = Avg3(num12, num13, num14));
			ptr[3] = (ptr2[1] = Avg3(num13, num14, num14));
			ptr2[3] = (ptr2[2] = (*ptr3 = (ptr3[1] = (ptr3[2] = (ptr3[3] = (byte)num14)))));
			break;
		}
	}

	private Vp8Frame ReferenceFrame(int which)
	{
		return frames[which switch
		{
			2 => goldenIndex, 
			1 => lastIndex, 
			_ => altRefIndex, 
		}];
	}

	private int ClampToBorder(int mv)
	{
		int num = MvRow(mv);
		int num2 = MvCol(mv);
		if (num2 < toLeft - 152)
		{
			num2 = toLeft - 128;
		}
		else if (num2 > toRight + 144)
		{
			num2 = toRight + 128;
		}
		if (num < toTop - 152)
		{
			num = toTop - 128;
		}
		else if (num > toBottom + 144)
		{
			num = toBottom + 128;
		}
		return PackMv(num, num2);
	}

	private void ClampChromaToBorder(ref int row, ref int col)
	{
		if (2 * col < toLeft - 152)
		{
			col = toLeft - 128 >> 1;
		}
		if (2 * col > toRight + 144)
		{
			col = toRight + 128 >> 1;
		}
		if (2 * row < toTop - 152)
		{
			row = toTop - 128 >> 1;
		}
		if (2 * row > toBottom + 144)
		{
			row = toBottom + 128 >> 1;
		}
	}

	private unsafe void PredictInter(Vp8Frame frame, Vp8ModeInfo* info, byte* y, byte* u, byte* v, int mbRow, int mbCol)
	{
		Vp8Frame vp8Frame = ReferenceFrame(info->RefFrame);
		int yStride = frame.YStride;
		int uvStride = frame.UvStride;
		byte* ptr = vp8Frame.Y + (y - frame.Y);
		byte* ptr2 = vp8Frame.U + (u - frame.U);
		byte* ptr3 = vp8Frame.V + (v - frame.V);
		bool flag = info->NeedToClampMvs != 0;
		toLeft = -(mbCol * 16 << 3);
		toRight = (mbCols - 1 - mbCol) * 16 << 3;
		toTop = -(mbRow * 16 << 3);
		toBottom = (mbRows - 1 - mbRow) * 16 << 3;
		if (info->Mode != 9)
		{
			int mv = (flag ? ClampToBorder(info->Mv) : info->Mv);
			int num = MvRow(mv);
			int num2 = MvCol(mv);
			PredictBlock(ptr + (num >> 3) * yStride + (num2 >> 3), yStride, num2 & 7, num & 7, y, yStride, 16, 16);
			num = ((num + (1 | (num >> 31))) / 2) & fullPixelMask;
			num2 = ((num2 + (1 | (num2 >> 31))) / 2) & fullPixelMask;
			ClampChromaToBorder(ref num, ref num2);
			int num3 = (num >> 3) * uvStride + (num2 >> 3);
			PredictBlock(ptr2 + num3, uvStride, num2 & 7, num & 7, u, uvStride, 8, 8);
			PredictBlock(ptr3 + num3, uvStride, num2 & 7, num & 7, v, uvStride, 8, 8);
			return;
		}
		int* ptr4 = info->Blocks;
		if (info->Partitioning < 3)
		{
			for (int i = 0; i < 4; i++)
			{
				int num4 = (i >> 1) * 8 + (i & 1) * 2;
				int mv2 = (flag ? ClampToBorder(ptr4[num4]) : ptr4[num4]);
				int num5 = (num4 >> 2) * 4 * yStride + (num4 & 3) * 4;
				PredictBlock(ptr + num5 + (MvRow(mv2) >> 3) * yStride + (MvCol(mv2) >> 3), yStride, MvCol(mv2) & 7, MvRow(mv2) & 7, y + num5, yStride, 8, 8);
			}
		}
		else
		{
			for (int j = 0; j < 16; j += 2)
			{
				int first = (flag ? ClampToBorder(ptr4[j]) : ptr4[j]);
				int second = (flag ? ClampToBorder(ptr4[j + 1]) : ptr4[j + 1]);
				int num6 = (j >> 2) * 4 * yStride + (j & 3) * 4;
				PredictPair(ptr + num6, y + num6, yStride, first, second);
			}
		}
		int* ptr5 = stackalloc int[4];
		for (int k = 0; k < 2; k++)
		{
			for (int l = 0; l < 2; l++)
			{
				int num7 = k * 8 + l * 2;
				int num8 = MvRow(ptr4[num7]) + MvRow(ptr4[num7 + 1]) + MvRow(ptr4[num7 + 4]) + MvRow(ptr4[num7 + 5]);
				num8 = ((num8 + 4 + (num8 >> 31) * 8) / 8) & fullPixelMask;
				int num9 = MvCol(ptr4[num7]) + MvCol(ptr4[num7 + 1]) + MvCol(ptr4[num7 + 4]) + MvCol(ptr4[num7 + 5]);
				num9 = ((num9 + 4 + (num9 >> 31) * 8) / 8) & fullPixelMask;
				if (flag)
				{
					ClampChromaToBorder(ref num8, ref num9);
				}
				ptr5[k * 2 + l] = PackMv(num8, num9);
			}
		}
		for (int m = 0; m < 4; m += 2)
		{
			int num10 = (m >> 1) * 4 * uvStride;
			PredictPair(ptr2 + num10, u + num10, uvStride, ptr5[m], ptr5[m + 1]);
			PredictPair(ptr3 + num10, v + num10, uvStride, ptr5[m], ptr5[m + 1]);
		}
	}

	private unsafe void PredictPair(byte* reference, byte* dst, int stride, int first, int second)
	{
		if (first == second)
		{
			PredictBlock(reference + (MvRow(first) >> 3) * stride + (MvCol(first) >> 3), stride, MvCol(first) & 7, MvRow(first) & 7, dst, stride, 8, 4);
			return;
		}
		PredictBlock(reference + (MvRow(first) >> 3) * stride + (MvCol(first) >> 3), stride, MvCol(first) & 7, MvRow(first) & 7, dst, stride, 4, 4);
		PredictBlock(reference + 4 + (MvRow(second) >> 3) * stride + (MvCol(second) >> 3), stride, MvCol(second) & 7, MvRow(second) & 7, dst + 4, stride, 4, 4);
	}

	private unsafe void PredictBlock(byte* src, int srcStride, int xOffset, int yOffset, byte* dst, int dstStride, int width, int height)
	{
		if ((xOffset | yOffset) == 0)
		{
			int num = 0;
			while (num < height)
			{
				switch (width)
				{
				case 16:
					Vector128.Load(src).Store(dst);
					break;
				case 8:
					Unsafe.WriteUnaligned(dst, Unsafe.ReadUnaligned<ulong>(src));
					break;
				default:
					Unsafe.WriteUnaligned(dst, Unsafe.ReadUnaligned<uint>(src));
					break;
				}
				num++;
				src += srcStride;
				dst += dstStride;
			}
		}
		else if (bilinear)
		{
			PredictBilinear(src, srcStride, xOffset, yOffset, dst, dstStride, width, height);
		}
		else
		{
			PredictSixTap(src, srcStride, xOffset, yOffset, dst, dstStride, width, height);
		}
	}

	private unsafe void PredictSixTap(byte* src, int srcStride, int xOffset, int yOffset, byte* dst, int dstStride, int width, int height)
	{
		short* filter = Vp8Pointers.SixTapFilters + xOffset * 6;
		short* filter2 = Vp8Pointers.SixTapFilters + yOffset * 6;
		if (yOffset == 0)
		{
			SixTapPass(src, srcStride, 1, filter, dst, dstStride, width, height);
			return;
		}
		if (xOffset == 0)
		{
			SixTapPass(src, srcStride, srcStride, filter2, dst, dstStride, width, height);
			return;
		}
		byte* ptr = filterScratch;
		SixTapPass(src - 2 * srcStride, srcStride, 1, filter, ptr, width, width, height + 5);
		SixTapPass(ptr + 2 * width, width, width, filter2, dst, dstStride, width, height);
	}

	private unsafe static void SixTapPass(byte* src, int srcStride, int step, short* filter, byte* dst, int dstStride, int width, int height)
	{
		if (Vp8Simd.Enabled)
		{
			Vp8Simd.SixTapPass(src, srcStride, step, filter, dst, dstStride, width, height);
			return;
		}
		int num = *filter;
		int num2 = filter[1];
		int num3 = filter[2];
		int num4 = filter[3];
		int num5 = filter[4];
		int num6 = filter[5];
		for (int i = 0; i < height; i++)
		{
			for (int j = 0; j < width; j++)
			{
				byte* ptr = src + j;
				int num7 = ptr[-2 * step] * num + ptr[-step] * num2 + *ptr * num3 + ptr[step] * num4 + ptr[2 * step] * num5 + ptr[3 * step] * num6 + 64;
				dst[j] = Clip(num7 >> 7);
			}
			src += srcStride;
			dst += dstStride;
		}
	}

	private unsafe void PredictBilinear(byte* src, int srcStride, int xOffset, int yOffset, byte* dst, int dstStride, int width, int height)
	{
		int num = 128 - 16 * xOffset;
		int num2 = 16 * xOffset;
		int num3 = 128 - 16 * yOffset;
		int num4 = 16 * yOffset;
		ushort* ptr = (ushort*)filterScratch;
		for (int i = 0; i <= height; i++)
		{
			byte* ptr2 = src + i * srcStride;
			for (int j = 0; j < width; j++)
			{
				ptr[i * width + j] = (ushort)(ptr2[j] * num + ptr2[j + 1] * num2 + 64 >> 7);
			}
		}
		for (int k = 0; k < height; k++)
		{
			ushort* ptr3 = ptr + k * width;
			for (int l = 0; l < width; l++)
			{
				dst[l] = (byte)(ptr3[l] * num3 + ptr3[l + width] * num4 + 64 >> 7);
			}
			dst += dstStride;
		}
	}
}
