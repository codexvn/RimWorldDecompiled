using System;
using System.Buffers;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;

namespace Waylith.Rendering;

public static class BcRdo
{
	private interface IBlockFormat
	{
		static abstract int BlockBytes { get; }

		static abstract int Stride { get; }

		static abstract int Components { get; }

		unsafe static abstract float MaxStdDev(byte* source);

		unsafe static abstract int Error(Vector128<byte> block, byte* source, byte* scratch);
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct Bc1Format : IBlockFormat
	{
		public static int BlockBytes => 8;

		public static int Stride => 8;

		public static int Components => 3;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static float MaxStdDev(byte* source)
		{
			return BlockMaxStdDev((uint*)source, 3);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int Error(Vector128<byte> block, byte* source, byte* scratch)
		{
			ulong num = block.AsUInt64().ToScalar();
			uint num2 = (uint)(num >> 32);
			if ((ushort)num <= (ushort)(num >> 16) && (num2 & (num2 >> 1) & 0x55555555) != 0)
			{
				return -1;
			}
			var (row, row2, row3, row4) = Bc7BlockEncoder.DecodeBc1Rows(num);
			return SquaredError(row, row2, row3, row4, source, rgbOnly: true);
		}
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct Bc7Format : IBlockFormat
	{
		public static int BlockBytes => 16;

		public static int Stride => 16;

		public static int Components => 4;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static float MaxStdDev(byte* source)
		{
			return BlockMaxStdDev((uint*)source, 4);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int Error(Vector128<byte> block, byte* source, byte* scratch)
		{
			if (block.ToScalar() == 0)
			{
				return -1;
			}
			Bc7BlockEncoder.DecodeBc7Block(new ReadOnlySpan<byte>(&block, 16), new Span<byte>(scratch, 64));
			return SquaredError(Vector128.Load(scratch), Vector128.Load(scratch + 16), Vector128.Load(scratch + 32), Vector128.Load(scratch + 48), source, rgbOnly: false);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int Error(Vector128<byte> block, byte* source, short* wideSource, byte* scratch, TrialBudget budget)
		{
			if (!Bc7BlockEncoder.TrySquaredErrorByRows(block, wideSource, budget, out var error))
			{
				return Error(block, source, scratch);
			}
			return error;
		}
	}

	private readonly struct TrialBudget : Bc7BlockEncoder.IErrorBudget
	{
		public TrialBudget(float errorScale, float threshMsErr, float smoothBlockMseScale, float bitsTimesLambda, float bestT)
		{
			_003CerrorScale_003EP = errorScale;
			_003CthreshMsErr_003EP = threshMsErr;
			_003CsmoothBlockMseScale_003EP = smoothBlockMseScale;
			_003CbitsTimesLambda_003EP = bitsTimesLambda;
			_003CbestT_003EP = bestT;
		}

		public bool Exceeded(int partial)
		{
			float num = (float)partial * _003CerrorScale_003EP;
			if (num < _003CthreshMsErr_003EP)
			{
				return !(num * _003CsmoothBlockMseScale_003EP + _003CbitsTimesLambda_003EP < _003CbestT_003EP);
			}
			return true;
		}
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct Bc4Format : IBlockFormat
	{
		public static int BlockBytes => 8;

		public static int Stride => 8;

		public static int Components => 1;

		public unsafe static float MaxStdDev(byte* source)
		{
			ulong num = 0uL;
			ulong num2 = 0uL;
			for (int i = 0; i < 16; i++)
			{
				uint num3 = source[i];
				num += num3;
				num2 += num3 * num3;
			}
			return Max(0f, MathF.Sqrt(16 * num2 - num * num) / 16f);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int Error(Vector128<byte> block, byte* source, byte* scratch)
		{
			Vector128<byte> vector = Bc7BlockEncoder.DecodeBc4Values(block.AsUInt64().ToScalar());
			Vector128<byte> vector2 = Vector128.Load(source);
			var (vector3, vector4) = Vector128.Widen(Vector128.Max(vector, vector2) - Vector128.Min(vector, vector2));
			if (Sse2.IsSupported)
			{
				return Vector128.Sum(Sse2.MultiplyAddAdjacent(vector3.AsInt16(), vector3.AsInt16()) + Sse2.MultiplyAddAdjacent(vector4.AsInt16(), vector4.AsInt16()));
			}
			return (int)Vector128.Sum(Squares(vector, vector2, Vector128<byte>.AllBitsSet));
		}
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct Bc5HalfFormat : IBlockFormat
	{
		public static int BlockBytes => 8;

		public static int Stride => 16;

		public static int Components => 1;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static float MaxStdDev(byte* source)
		{
			return Bc4Format.MaxStdDev(source);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int Error(Vector128<byte> block, byte* source, byte* scratch)
		{
			return Bc4Format.Error(block, source, scratch);
		}
	}

	private struct Bc1Trials
	{
		private readonly struct MixedRows
		{
			public readonly Vector128<int> Rows;

			public readonly int All;

			public MixedRows(Vector128<int> rows, int all)
			{
				Rows = rows;
				All = all;
			}
		}

		public const int Runs = 21;

		private unsafe byte* source;

		private ulong orig;

		private Vector128<byte> origPalette;

		private Vector128<int> origRows;

		private static ReadOnlySpan<byte> RunMixes => new byte[21]
		{
			1, 1, 2, 1, 2, 3, 1, 2, 3, 4,
			1, 2, 3, 4, 0, 5, 2, 3, 4, 0,
			0
		};

		private static ReadOnlySpan<byte> RunRowsFromPrev => new byte[21]
		{
			15, 7, 15, 3, 7, 15, 1, 3, 7, 15,
			0, 1, 3, 7, 15, 0, 0, 1, 3, 7,
			14
		};

		public unsafe void StartBlock(ulong block, byte* blockSource)
		{
			orig = block;
			source = blockSource;
			origPalette = Bc7BlockEncoder.Bc1Palette((uint)block);
			origRows = RowErrors(origPalette, (uint)(block >> 32), blockSource);
		}

		public unsafe readonly void Score(ulong prev, int* errors)
		{
			uint num = (uint)(orig >> 32);
			uint num2 = (uint)(prev >> 32);
			int endpoints = (int)prev;
			uint endpoints2 = Mix((uint)orig, (uint)prev, 4294967040u);
			uint endpoints3 = Mix((uint)orig, (uint)prev, 4294901760u);
			uint endpoints4 = Mix((uint)orig, (uint)prev, 4278190080u);
			uint endpoints5 = Mix((uint)orig, (uint)prev, 16777215u);
			MixedRows mixedRows = Mixes(Bc7BlockEncoder.Bc1Palette((uint)endpoints), num2, num, source);
			*errors = mixedRows.All;
			errors[1] = mixedRows.Rows[3];
			errors[3] = mixedRows.Rows[2];
			errors[6] = mixedRows.Rows[1];
			errors[10] = mixedRows.Rows[0];
			mixedRows = Mixes(Bc7BlockEncoder.Bc1Palette(endpoints2), num2, num, source);
			errors[2] = mixedRows.All;
			errors[4] = mixedRows.Rows[3];
			errors[7] = mixedRows.Rows[2];
			errors[11] = mixedRows.Rows[1];
			errors[16] = mixedRows.Rows[0];
			mixedRows = Mixes(Bc7BlockEncoder.Bc1Palette(endpoints3), num2, num, source);
			errors[5] = mixedRows.All;
			errors[8] = mixedRows.Rows[3];
			errors[12] = mixedRows.Rows[2];
			errors[17] = mixedRows.Rows[1];
			mixedRows = Mixes(Bc7BlockEncoder.Bc1Palette(endpoints4), num2, num, source);
			errors[9] = mixedRows.All;
			errors[13] = mixedRows.Rows[3];
			errors[18] = mixedRows.Rows[2];
			Vector128<int> prevRows = RowErrors(origPalette, num2, source);
			mixedRows = Mixes(prevRows, origRows);
			errors[14] = mixedRows.All;
			errors[19] = mixedRows.Rows[3];
			errors[20] = mixedRows.All - prevRows[0] + origRows[0];
			errors[15] = Vector128.Sum(RowErrors(Bc7BlockEncoder.Bc1Palette(endpoints5), num, source));
			uint num3 = (uint)((IsThreeColour((uint)endpoints) ? 2 : 0) | (IsThreeColour(endpoints2) ? 4 : 0) | (IsThreeColour(endpoints3) ? 8 : 0) | (IsThreeColour(endpoints4) ? 16 : 0) | (IsThreeColour(endpoints5) ? 32 : 0)) | (IsThreeColour((uint)orig) ? 1u : 0u);
			if (num3 != 0)
			{
				RejectTransparent(errors, num3, RowsUsingIndex3(num), RowsUsingIndex3(num2));
			}
		}

		private unsafe static void RejectTransparent(int* errors, uint threeColourMixes, uint origRows, uint prevRows)
		{
			for (int i = 0; i < 21; i++)
			{
				uint num = RunRowsFromPrev[i];
				if (((threeColourMixes >> (int)RunMixes[i]) & 1) != 0 && ((prevRows & num) | (origRows & ~num)) != 0)
				{
					errors[i] = -1;
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static uint RowsUsingIndex3(uint selectors)
		{
			uint num = selectors & (selectors >> 1) & 0x55555555;
			return (((num & 0xFF) != 0) ? 1u : 0u) | (uint)(((num & 0xFF00) != 0) ? 2 : 0) | (uint)(((num & 0xFF0000) != 0) ? 4 : 0) | (uint)(((num & 0xFF000000u) != 0) ? 8 : 0);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private unsafe static MixedRows Mixes(Vector128<byte> palette, uint prevSelectors, uint origSelectors, byte* source)
		{
			return Mixes(RowErrors(palette, prevSelectors, source), RowErrors(palette, origSelectors, source));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static MixedRows Mixes(Vector128<int> prevRows, Vector128<int> origRows)
		{
			Vector128<int> value = prevRows + ShiftUpOne(prevRows);
			value += ShiftUpTwo(value);
			Vector128<int> vector = origRows + ShiftDownOne(origRows);
			return new MixedRows(vector + ShiftDownTwo(vector) + ShiftUpOne(value), value[3]);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static Vector128<int> ShiftUpOne(Vector128<int> value)
		{
			if (!Sse2.IsSupported)
			{
				return Vector128.Shuffle(value, Vector128.Create(4, 0, 1, 2));
			}
			return Sse2.ShiftLeftLogical128BitLane(value, 4);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static Vector128<int> ShiftUpTwo(Vector128<int> value)
		{
			if (!Sse2.IsSupported)
			{
				return Vector128.Shuffle(value, Vector128.Create(4, 4, 0, 1));
			}
			return Sse2.ShiftLeftLogical128BitLane(value, 8);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static Vector128<int> ShiftDownOne(Vector128<int> value)
		{
			if (!Sse2.IsSupported)
			{
				return Vector128.Shuffle(value, Vector128.Create(1, 2, 3, 4));
			}
			return Sse2.ShiftRightLogical128BitLane(value, 4);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static Vector128<int> ShiftDownTwo(Vector128<int> value)
		{
			if (!Sse2.IsSupported)
			{
				return Vector128.Shuffle(value, Vector128.Create(2, 3, 4, 4));
			}
			return Sse2.ShiftRightLogical128BitLane(value, 8);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static uint Mix(uint orig, uint prev, uint fromPrev)
		{
			return (orig & ~fromPrev) | (prev & fromPrev);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool IsThreeColour(uint endpoints)
		{
			return (ushort)endpoints <= (ushort)(endpoints >> 16);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private unsafe static Vector128<int> RowErrors(Vector128<byte> palette, uint selectors, byte* source)
		{
			Vector128<byte> vector = Vector128.Create(16777215u).AsByte();
			var (vector2, vector3, vector4, vector5) = Bc7BlockEncoder.Bc1Rows(palette, selectors);
			if (Avx2.IsSupported)
			{
				Vector256<byte> vector6 = Vector256.Create(vector, vector);
				Vector256<byte> left = Vector256.Create(vector2, vector3);
				Vector256<byte> left2 = Vector256.Create(vector4, vector5);
				Vector256<byte> right = Vector256.Load(source);
				Vector256<byte> right2 = Vector256.Load(source + 32);
				Vector256<int> vector7 = Avx2.HorizontalAdd(Squares((Vector256.Max(left, right) - Vector256.Min(left, right)) & vector6), Squares((Vector256.Max(left2, right2) - Vector256.Min(left2, right2)) & vector6));
				Vector256<int> vector8 = Avx2.HorizontalAdd(vector7, vector7);
				return Sse2.UnpackLow(vector8.GetLower(), vector8.GetUpper());
			}
			return Vector128.Create(RowError(vector2, Vector128.Load(source), vector), RowError(vector3, Vector128.Load(source + 16), vector), RowError(vector4, Vector128.Load(source + 32), vector), RowError(vector5, Vector128.Load(source + 48), vector));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static int RowError(Vector128<byte> row, Vector128<byte> source, Vector128<byte> mask)
		{
			return (int)Vector128.Sum(SquaresPortable(row, source, mask));
		}
	}

	private struct Texels
	{
		public unsafe fixed byte Bytes[64];
	}

	private struct WideTexels
	{
		public unsafe fixed short Values[64];
	}

	private struct RunErrors
	{
		public unsafe fixed int Values[24];
	}

	private struct Best
	{
		public Vector128<byte> Block;

		public float T;

		public float Mse;

		public float MatchBits;

		public int MatchLen;

		public int SrcWindowOfs;

		public int DstWindowOfs;

		public int DstBlockOfs;
	}

	private struct Pass<TFormat> where TFormat : struct, IBlockFormat
	{
		private readonly record struct HashGroup(int FirstRun, int Count, int Length, Vector256<byte> Picks, Vector256<uint> Seeds);

		private static readonly float ErrorScale = 1f / (float)TFormat.Components / 16f;

		private static readonly byte[] RunLengths = BuildRuns(lengths: true);

		private static readonly byte[] RunOffsets = BuildRuns(lengths: false);

		private static readonly ulong[] RunsAtOffsetZero = BuildRunsAtOffsetZero();

		private static readonly int[] RunLiteralBits = BuildRunTable((int run) => (BlockBytes - RunLengths[run]) * 13);

		private static readonly int[] RunLiteralAndLengthBits = BuildRunTable((int run) => (BlockBytes - RunLengths[run]) * 13 + MatchLengthCost(RunLengths[run]));

		private static readonly int[] RunByteBits = BuildRunTable((int run) => (int)RunBits(RunOffsets[run], RunLengths[run]));

		private static readonly byte[] RunMasksByRun = BuildRunMasksByRun();

		private static readonly float[] ShortLiteralBits = BuildShortTable((int run) => (float)(BlockBytes - RunLengths[run]) * 13f);

		private static readonly float[] ShortLengthCosts = BuildShortTable((int run) => MatchLengthCost(RunLengths[run]));

		private static readonly float[] ShortAtOffsetZero = BuildShortTable((int run) => (RunOffsets[run] != 0) ? 0f : BitConverter.Int32BitsToSingle(-1));

		private unsafe readonly byte* blocks;

		private readonly int total;

		private unsafe readonly uint* pixels;

		private unsafe readonly float* scales;

		private readonly float lambda;

		private readonly float smoothBlockMaxMseScale;

		private readonly int blocksToCheck;

		private readonly int ringMask;

		private unsafe ulong* triedRuns;

		private unsafe int* firstOwner;

		private unsafe int* displaced;

		private unsafe uint* runHashes;

		private unsafe byte* decoded;

		private unsafe short* wideSource;

		private unsafe int* runErrors;

		private Bc1Trials bc1;

		private int prevMatchWindowOfsToFavorCont;

		private static readonly HashGroup[] HashGroups = BuildHashGroups();

		private static int BlockBytes => TFormat.BlockBytes;

		private static int Stride => TFormat.Stride;

		private static bool IsBc1 => typeof(TFormat) == typeof(Bc1Format);

		private static int RunCount => (BlockBytes - 2) * (BlockBytes - 1) / 2;

		private static int RunStride => (RunCount + 7) & -8;

		private static byte[] BuildRuns(bool lengths)
		{
			byte[] array = new byte[RunCount];
			int num = 0;
			for (int num2 = BlockBytes; num2 >= 3; num2--)
			{
				for (int i = 0; i <= BlockBytes - num2; i++)
				{
					array[num++] = (byte)(lengths ? num2 : i);
				}
			}
			return array;
		}

		private static ulong[] BuildRunsAtOffsetZero()
		{
			ulong[] array = new ulong[2];
			for (int i = 0; i < RunCount; i++)
			{
				if (RunOffsets[i] == 0)
				{
					array[i >> 6] |= (ulong)(1L << i);
				}
			}
			return array;
		}

		private static int[] BuildRunTable(Func<int, int> value)
		{
			int[] array = new int[RunCount];
			for (int i = 0; i < RunCount; i++)
			{
				array[i] = value(i);
			}
			return array;
		}

		private static byte[] BuildRunMasksByRun()
		{
			byte[] array = new byte[RunCount * 16];
			for (int i = 0; i < RunCount; i++)
			{
				RunMasks.AsSpan((RunOffsets[i] * 17 + RunLengths[i]) * 16, 16).CopyTo(array.AsSpan(i * 16));
			}
			return array;
		}

		private static float[] BuildShortTable(Func<int, float> value)
		{
			float[] array = new float[24];
			if (BlockBytes == 8)
			{
				for (int i = 0; i < RunCount; i++)
				{
					array[i] = value(i);
				}
			}
			return array;
		}

		public unsafe Pass(byte* blocks, int total, uint* pixels, float* scales, float lambda, int window, float smoothBlockMaxMseScale)
		{
			triedRuns = default(ulong*);
			firstOwner = default(int*);
			displaced = default(int*);
			runHashes = default(uint*);
			decoded = default(byte*);
			wideSource = default(short*);
			runErrors = default(int*);
			bc1 = default(Bc1Trials);
			prevMatchWindowOfsToFavorCont = 0;
			this.blocks = blocks;
			this.total = total;
			this.pixels = pixels;
			this.scales = scales;
			this.lambda = lambda;
			this.smoothBlockMaxMseScale = smoothBlockMaxMseScale;
			blocksToCheck = System.Math.Max(1, window / Stride);
			ringMask = (int)(BitOperations.RoundUpToPowerOf2((uint)blocksToCheck) - 1);
		}

		public unsafe int Run()
		{
			ulong[] array = new ulong[(ringMask + 1) * 2];
			int[] array2 = new int[8192];
			int[] array3 = new int[RunCount];
			array2.AsSpan().Fill(-1);
			uint[] array4 = new uint[(ringMask + 1) * RunStride];
			Texels texels = default(Texels);
			RunErrors runErrors = default(RunErrors);
			decoded = texels.Bytes;
			WideTexels wideTexels = default(WideTexels);
			wideSource = wideTexels.Values;
			this.runErrors = runErrors.Values;
			new Span<int>(this.runErrors, 24).Fill(-1);
			prevMatchWindowOfsToFavorCont = -1;
			int num = 0;
			fixed (ulong* ptr = array)
			{
				fixed (int* ptr2 = array2)
				{
					fixed (int* ptr3 = array3)
					{
						fixed (uint* ptr4 = array4)
						{
							triedRuns = ptr;
							firstOwner = ptr2;
							displaced = ptr3;
							runHashes = ptr4;
							for (int i = 0; i < total; i++)
							{
								int num2 = ProcessBlock(i);
								if (num2 < 0)
								{
									return -1;
								}
								num += num2;
							}
						}
					}
				}
			}
			return num;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private unsafe int ProcessBlock(int blockIndex)
		{
			byte* ptr = blocks + blockIndex * Stride;
			Vector128<byte> vector = Load(ptr);
			byte* ptr2 = (byte*)(pixels + blockIndex * 16);
			if (typeof(TFormat) == typeof(Bc7Format))
			{
				Bc7BlockEncoder.WidenRows(ptr2, wideSource);
			}
			int num = TFormat.Error(vector, ptr2, decoded);
			if (num < 0)
			{
				return -1;
			}
			if (IsBc1)
			{
				bc1.StartBlock(vector.AsUInt64().ToScalar(), ptr2);
			}
			float num2 = (float)num * ErrorScale;
			float num3 = System.Math.Clamp(TFormat.MaxStdDev(ptr2) / 18f, 0f, 1f);
			num3 *= num3;
			float num4 = smoothBlockMaxMseScale + (1f - smoothBlockMaxMseScale) * num3;
			if (scales[blockIndex] > 0f)
			{
				num4 = scales[blockIndex];
			}
			float num5 = 13f * (float)BlockBytes;
			float num6 = num2 * num4 + num5 * lambda;
			int firstBlockToCheck = System.Math.Max(0, blockIndex - blocksToCheck);
			float threshMsErr = 100f * Max(num2, 1f);
			Best best = default(Best);
			best.Block = vector;
			best.T = num6;
			best.Mse = num2;
			Best best2 = best;
			FirstPass(blockIndex, firstBlockToCheck, vector, ptr2, num2, num4, threshMsErr, ref best2);
			int result = 0;
			if (best2.T < num6)
			{
				Best best3 = ((best2.MatchLen <= BlockBytes - 3) ? SecondPass(blockIndex, firstBlockToCheck, ptr2, num4, threshMsErr, ref best2) : default(Best));
				Store(ptr, best2.Block);
				result = 1;
				if (BlockBytes == Stride)
				{
					prevMatchWindowOfsToFavorCont = ((best3.MatchLen != 0 && best2.DstWindowOfs <= best3.DstWindowOfs) ? ((best3.DstBlockOfs + best3.MatchLen == BlockBytes) ? (best3.SrcWindowOfs + best3.MatchLen) : (-1)) : ((best2.DstBlockOfs + best2.MatchLen == BlockBytes) ? (best2.SrcWindowOfs + best2.MatchLen) : (-1)));
				}
			}
			else
			{
				prevMatchWindowOfsToFavorCont = -1;
			}
			uint* ptr3 = runHashes + (blockIndex & ringMask) * RunStride;
			if (Avx2.IsSupported)
			{
				HashRuns(ptr, ptr3);
			}
			else if (Ssse3.IsSupported || AdvSimd.IsSupported)
			{
				HashRunsByFours(ptr, ptr3);
			}
			else
			{
				ref byte arrayDataReference = ref MemoryMarshal.GetArrayDataReference(RunLengths);
				ref byte arrayDataReference2 = ref MemoryMarshal.GetArrayDataReference(RunOffsets);
				for (int i = 0; i < RunCount; i++)
				{
					ptr3[i] = HashHsieh(ptr + (int)Unsafe.Add(ref arrayDataReference2, i), Unsafe.Add(ref arrayDataReference, i), Unsafe.Add(ref arrayDataReference2, i));
				}
			}
			Prepend(blockIndex);
			return result;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private unsafe static void HashRuns(byte* block, uint* hashes)
		{
			Vector128<byte> vector = Load(block);
			Vector256<byte> vector2 = Vector256.Create(vector, vector);
			Vector256<byte> vector3 = Vector256.Create((byte)2);
			Vector256<byte> vector4 = Vector256.Create((byte)4);
			HashGroup[] hashGroups = HashGroups;
			for (int i = 0; i < hashGroups.Length; i++)
			{
				HashGroup hashGroup = hashGroups[i];
				Vector256<byte> picks = hashGroup.Picks;
				Vector256<uint> vector5 = hashGroup.Seeds;
				for (int num = hashGroup.Length >> 2; num > 0; num--)
				{
					vector5 += Avx2.Shuffle(vector2, picks).AsUInt32();
					Vector256<uint> vector6 = (Avx2.Shuffle(vector2, picks + vector3).AsUInt32() << 11) ^ vector5;
					vector5 = (vector5 << 16) ^ vector6;
					vector5 += vector5 >>> 11;
					picks += vector4;
				}
				switch (hashGroup.Length & 3)
				{
				case 1:
					vector5 += SignExtendedByte(vector2, picks);
					vector5 ^= vector5 << 10;
					vector5 += vector5 >>> 1;
					break;
				case 2:
					vector5 += Avx2.Shuffle(vector2, picks).AsUInt32();
					vector5 ^= vector5 << 11;
					vector5 += vector5 >>> 17;
					break;
				case 3:
					vector5 += Avx2.Shuffle(vector2, picks).AsUInt32();
					vector5 ^= vector5 << 16;
					vector5 ^= SignExtendedByte(vector2, picks + vector3) << 18;
					vector5 += vector5 >>> 11;
					break;
				}
				vector5 ^= vector5 << 3;
				vector5 += vector5 >>> 5;
				vector5 ^= vector5 << 4;
				vector5 += vector5 >>> 17;
				vector5 ^= vector5 << 25;
				vector5 += vector5 >>> 6;
				vector5.Store(hashes + hashGroup.FirstRun);
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private unsafe static void HashRunsByFours(byte* block, uint* hashes)
		{
			Vector128<byte> bytes = Load(block);
			HashGroup[] hashGroups = HashGroups;
			for (int i = 0; i < hashGroups.Length; i++)
			{
				HashGroup hashGroup = hashGroups[i];
				HashFour(bytes, hashGroup.Picks.GetLower(), hashGroup.Seeds.GetLower(), hashGroup.Length).Store(hashes + hashGroup.FirstRun);
				if (hashGroup.Count > 4)
				{
					HashFour(bytes, hashGroup.Picks.GetUpper(), hashGroup.Seeds.GetUpper(), hashGroup.Length).Store(hashes + hashGroup.FirstRun + 4);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static Vector128<uint> HashFour(Vector128<byte> bytes, Vector128<byte> picks, Vector128<uint> h, int length)
		{
			Vector128<byte> vector = Vector128.Create((byte)2);
			for (int num = length >> 2; num > 0; num--)
			{
				h += Vector128.ShuffleNative(bytes, picks).AsUInt32();
				Vector128<uint> vector2 = (Vector128.ShuffleNative(bytes, picks + vector).AsUInt32() << 11) ^ h;
				h = (h << 16) ^ vector2;
				h += h >>> 11;
				picks += Vector128.Create((byte)4);
			}
			switch (length & 3)
			{
			case 1:
				h += SignExtendedByte(bytes, picks);
				h ^= h << 10;
				h += h >>> 1;
				break;
			case 2:
				h += Vector128.ShuffleNative(bytes, picks).AsUInt32();
				h ^= h << 11;
				h += h >>> 17;
				break;
			case 3:
				h += Vector128.ShuffleNative(bytes, picks).AsUInt32();
				h ^= h << 16;
				h ^= SignExtendedByte(bytes, picks + vector) << 18;
				h += h >>> 11;
				break;
			}
			h ^= h << 3;
			h += h >>> 5;
			h ^= h << 4;
			h += h >>> 17;
			h ^= h << 25;
			h += h >>> 6;
			return h;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static Vector128<uint> SignExtendedByte(Vector128<byte> bytes, Vector128<byte> picks)
		{
			return Vector128.ShiftRightArithmetic(Vector128.ShuffleNative(bytes, ((picks.AsUInt32() << 24) | Vector128.Create(8421504u)).AsByte()).AsInt32(), 24).AsUInt32();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static Vector256<uint> SignExtendedByte(Vector256<byte> bytes, Vector256<byte> picks)
		{
			return Vector256.ShiftRightArithmetic(Avx2.Shuffle(bytes, ((picks.AsUInt32() << 24) | Vector256.Create(8421504u)).AsByte()).AsInt32(), 24).AsUInt32();
		}

		private static HashGroup[] BuildHashGroups()
		{
			List<HashGroup> list = new List<HashGroup>();
			int j;
			for (int i = 0; i < RunCount; i += j)
			{
				int num = RunLengths[i];
				for (j = 0; j < 8 && i + j < RunCount && RunLengths[i + j] == num; j++)
				{
				}
				byte[] array = new byte[32];
				uint[] array2 = new uint[8];
				for (int k = 0; k < 8; k++)
				{
					int num2 = ((k < j) ? RunOffsets[i + k] : 0);
					array[k * 4] = (byte)num2;
					array[k * 4 + 1] = (byte)(num2 + 1);
					array[k * 4 + 2] = 128;
					array[k * 4 + 3] = 128;
					array2[k] = (uint)(num + (num2 << 16));
				}
				list.Add(new HashGroup(i, j, num, Vector256.Create(array), Vector256.Create(array2)));
			}
			return list.ToArray();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private unsafe readonly void Prepend(int blockIndex)
		{
			int* ptr = firstOwner;
			int* ptr2 = displaced;
			ulong* ptr3 = triedRuns;
			uint* ptr4 = runHashes;
			int num = blocksToCheck;
			int num2 = ringMask;
			uint* ptr5 = ptr4 + (blockIndex & num2) * RunStride;
			int num3 = blockIndex * 128;
			ulong num4 = 0uL;
			ulong num5 = 0uL;
			ulong num6 = 0uL;
			ulong num7 = 0uL;
			for (int i = 0; i < RunCount; i++)
			{
				uint num8 = ptr5[i];
				ref int reference = ref ptr[num8 & 0x1FFF];
				int num9 = reference;
				ulong num10;
				ulong num11;
				if (num9 >= num3)
				{
					num10 = (ulong)((ptr5[num9 - num3] != num8) ? 1 : 0);
					num11 = 0uL;
				}
				else
				{
					ptr2[i] = num9;
					num10 = 1uL;
					num11 = 1uL;
				}
				reference = num3 + i;
				if (i < 64)
				{
					num4 |= num10 << i;
					num6 |= num11 << i;
				}
				else
				{
					num5 |= num10 << i - 64;
					num7 |= num11 << i - 64;
				}
			}
			int num12 = blockIndex - num;
			for (int j = 0; j < 2; j++)
			{
				for (ulong num13 = ((j == 0) ? num6 : num7); num13 != 0L; num13 &= num13 - 1)
				{
					int num14 = j * 64 + BitOperations.TrailingZeroCount(num13);
					uint num15 = ptr5[num14];
					ref int reference2 = ref ptr[num15 & 0x1FFF];
					int num16 = ptr2[num14];
					int num17 = num16 >> 7;
					int num18 = num16 & 0x7F;
					if (num16 >= 0 && num17 > num12 && ptr4[(num17 & num2) * RunStride + num18] == ptr5[reference2 - num3])
					{
						ptr3[(num17 & num2) * 2 + (num18 >> 6)] &= (ulong)(~(1L << num18));
					}
					reference2 = num3 + num14;
				}
			}
			ptr3[(blockIndex & num2) * 2] = num4;
			ptr3[(blockIndex & num2) * 2 + 1] = num5;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private unsafe readonly void SkipClearedMatches(int prevBlockIndex, ref ulong tried0, ref ulong tried1)
		{
			uint* ptr = runHashes + (prevBlockIndex & ringMask) * RunStride;
			for (int i = 0; i < RunCount; i++)
			{
				uint num = ptr[i];
				if ((num & 0xFFFFFF00u) == 0 && firstOwner[num & 0x1FFF] == prevBlockIndex * 128 + i)
				{
					if (i < 64)
					{
						tried0 &= (ulong)(~(1L << i));
					}
					else
					{
						tried1 &= (ulong)(~(1L << i - 64));
					}
				}
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private unsafe readonly void FirstPass(int blockIndex, int firstBlockToCheck, Vector128<byte> orig, byte* source, float curMse, float smoothBlockMseScale, float threshMsErr, ref Best best)
		{
			byte* ptr = blocks;
			ulong* ptr2 = triedRuns;
			int num = ringMask;
			bool flag = (blockIndex & 0xFF) == 0;
			for (int num2 = blockIndex - 1; num2 >= firstBlockToCheck; num2--)
			{
				Vector128<byte> prev = Load(ptr + num2 * Stride);
				ulong tried = ptr2[(num2 & num) * 2];
				ulong tried2 = ptr2[(num2 & num) * 2 + 1];
				if (flag)
				{
					SkipClearedMatches(num2, ref tried, ref tried2);
				}
				bool flag2 = num2 * Stride == prevMatchWindowOfsToFavorCont;
				if (flag2)
				{
					tried |= RunsAtOffsetZero[0];
					tried2 |= RunsAtOffsetZero[1];
				}
				int distanceCost = MatchDistanceCost((blockIndex - num2) * Stride);
				if (BlockBytes == 8)
				{
					ScoreShortRuns(blockIndex, num2, orig, prev, (uint)tried, flag2, distanceCost, source, smoothBlockMseScale, threshMsErr, ref best);
				}
				else
				{
					ScoreRuns(blockIndex, num2, orig, prev, tried, tried2, flag2, distanceCost, source, curMse, smoothBlockMseScale, threshMsErr, ref best);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private unsafe readonly void ScoreShortRuns(int blockIndex, int prevBlockIndex, Vector128<byte> orig, Vector128<byte> prev, uint tried, bool continuesMatch, int distanceCost, byte* source, float smoothBlockMseScale, float threshMsErr, ref Best best)
		{
			int* ptr = runErrors;
			if (IsBc1)
			{
				bc1.Score(prev.AsUInt64().ToScalar(), ptr);
			}
			else
			{
				ref byte arrayDataReference = ref MemoryMarshal.GetArrayDataReference(RunMasks);
				for (int i = 0; i < RunCount; i++)
				{
					Vector128<byte> condition = Vector128.LoadUnsafe(in arrayDataReference, (nuint)((RunOffsets[i] * 17 + RunLengths[i]) * 16));
					ptr[i] = TFormat.Error(Vector128.ConditionalSelect(condition, prev, orig), source, decoded);
				}
			}
			Vector256<float> left = ShortGroupCost(0, ptr, tried, continuesMatch, distanceCost, smoothBlockMseScale, threshMsErr);
			Vector256<float> vector = ShortGroupCost(1, ptr, tried, continuesMatch, distanceCost, smoothBlockMseScale, threshMsErr);
			Vector256<float> vector2 = ShortGroupCost(2, ptr, tried, continuesMatch, distanceCost, smoothBlockMseScale, threshMsErr);
			float num = HorizontalMin(Vector256.Min(Vector256.Min(left, vector), vector2));
			if (num < best.T)
			{
				int num2 = BitOperations.TrailingZeroCount(Vector256.Equals(left, Vector256.Create(num)).ExtractMostSignificantBits() | (Vector256.Equals(vector, Vector256.Create(num)).ExtractMostSignificantBits() << 8) | (Vector256.Equals(vector2, Vector256.Create(num)).ExtractMostSignificantBits() << 16));
				int num3 = RunLengths[num2];
				int num4 = RunOffsets[num2];
				ref byte arrayDataReference2 = ref MemoryMarshal.GetArrayDataReference(RunMasks);
				best.T = num;
				best.Mse = (float)ptr[num2] * ErrorScale;
				best.Block = Vector128.ConditionalSelect(Vector128.LoadUnsafe(in arrayDataReference2, (nuint)((num4 * 17 + num3) * 16)), prev, orig);
				best.MatchLen = num3;
				best.SrcWindowOfs = prevBlockIndex * Stride + num4;
				best.DstWindowOfs = blockIndex * Stride + num4;
				best.DstBlockOfs = num4;
				best.MatchBits = ((continuesMatch && num4 == 0) ? 1f : ((float)(MatchLengthCost(num3) + distanceCost)));
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private unsafe readonly Vector256<float> ShortGroupCost(int group, int* errors, uint tried, bool continuesMatch, int distanceCost, float smoothBlockMseScale, float threshMsErr)
		{
			Vector256<float> vector = Vector256.Create(lambda);
			Vector256<float> vector2 = Vector256.Create(ShortLiteralBits, group * 8);
			Vector256<float> vector3 = (vector2 + (Vector256.Create(ShortLengthCosts, group * 8) + Vector256.Create((float)distanceCost))) * vector;
			if (continuesMatch)
			{
				vector3 = Vector256.ConditionalSelect(Vector256.Create(ShortAtOffsetZero, group * 8), (vector2 + Vector256.Create(1f)) * vector, vector3);
			}
			Vector256<int> vector4 = Vector256.Load(errors + group * 8);
			Vector256<float> vector5 = Vector256.ConvertToSingle(vector4) * ErrorScale;
			Vector256<float> left = vector5 * smoothBlockMseScale + vector3;
			Vector256<int> vector6 = Vector256.Create(1, 2, 4, 8, 16, 32, 64, 128);
			return Vector256.ConditionalSelect(Vector256.Equals(Vector256.Create((int)(tried >> group * 8)) & vector6, vector6).AsSingle() & Vector256.GreaterThanOrEqual(vector4, Vector256<int>.Zero).AsSingle() & Vector256.LessThan(vector5, Vector256.Create(threshMsErr)), left, Vector256.Create(float.PositiveInfinity));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private unsafe readonly void ScoreRuns(int blockIndex, int prevBlockIndex, Vector128<byte> orig, Vector128<byte> prev, ulong tried0, ulong tried1, bool continuesMatch, int distanceCost, byte* source, float curMse, float smoothBlockMseScale, float threshMsErr, ref Best best)
		{
			ref byte arrayDataReference = ref MemoryMarshal.GetArrayDataReference(RunMasksByRun);
			ref byte arrayDataReference2 = ref MemoryMarshal.GetArrayDataReference(RunLengths);
			ref byte arrayDataReference3 = ref MemoryMarshal.GetArrayDataReference(RunOffsets);
			ref int arrayDataReference4 = ref MemoryMarshal.GetArrayDataReference(RunLiteralBits);
			ref int arrayDataReference5 = ref MemoryMarshal.GetArrayDataReference(RunLiteralAndLengthBits);
			ref int arrayDataReference6 = ref MemoryMarshal.GetArrayDataReference(RunByteBits);
			int num = (int)(~Vector128.Equals(prev, orig).ExtractMostSignificantBits());
			float num2 = lambda;
			float num3 = best.T;
			for (int i = 0; i < 2; i++)
			{
				ulong num4 = ((i == 0) ? tried0 : tried1);
				while (num4 != 0L)
				{
					int num5 = i * 64 + BitOperations.TrailingZeroCount(num4);
					num4 &= num4 - 1;
					int num6 = Unsafe.Add(ref arrayDataReference3, num5);
					bool flag = continuesMatch && num6 == 0;
					float num7 = (float)(flag ? (Unsafe.Add(ref arrayDataReference4, num5) + 1) : (Unsafe.Add(ref arrayDataReference5, num5) + distanceCost)) * num2;
					if (num7 >= num3)
					{
						continue;
					}
					Vector128<byte> block = Vector128.ConditionalSelect(Vector128.LoadUnsafe(in arrayDataReference, (nuint)num5 * (nuint)16u), prev, orig);
					float num8;
					if ((num & Unsafe.Add(ref arrayDataReference6, num5)) == 0)
					{
						num8 = curMse;
					}
					else
					{
						int num9 = ((typeof(TFormat) == typeof(Bc7Format)) ? Bc7Format.Error(block, source, wideSource, decoded, new TrialBudget(ErrorScale, threshMsErr, smoothBlockMseScale, num7, num3)) : TFormat.Error(block, source, decoded));
						if (num9 < 0)
						{
							continue;
						}
						num8 = (float)num9 * ErrorScale;
					}
					if (num8 < threshMsErr)
					{
						float num10 = num8 * smoothBlockMseScale + num7;
						if (num10 < num3)
						{
							num3 = num10;
							best.T = num10;
							best.Mse = num8;
							best.Block = block;
							int matchLen = (best.MatchLen = Unsafe.Add(ref arrayDataReference2, num5));
							best.SrcWindowOfs = prevBlockIndex * Stride + num6;
							best.DstWindowOfs = blockIndex * Stride + num6;
							best.DstBlockOfs = num6;
							best.MatchBits = (flag ? 1f : ((float)(MatchLengthCost(matchLen) + distanceCost)));
						}
					}
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static float HorizontalMin(Vector256<float> value)
		{
			Vector128<float> vector = Vector128.Min(value.GetLower(), value.GetUpper());
			vector = Vector128.Min(vector, Vector128.Shuffle(vector, Vector128.Create(2, 3, 0, 1)));
			return MathF.Min(vector.ToScalar(), vector.GetElement(1));
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private unsafe readonly Best SecondPass(int blockIndex, int firstBlockToCheck, byte* source, float smoothBlockMseScale, float threshMsErr, ref Best best)
		{
			byte* ptr = blocks;
			byte* scratch = decoded;
			short* ptr2 = wideSource;
			float num = lambda;
			ref byte arrayDataReference = ref MemoryMarshal.GetArrayDataReference(RunMasks);
			Best result = default(Best);
			uint num2 = RunBits(best.DstBlockOfs, best.MatchLen);
			Vector128<byte> block = best.Block;
			float mse = best.Mse;
			int matchLen = best.MatchLen;
			float matchBits = best.MatchBits;
			float num3 = best.T;
			for (int num4 = blockIndex - 1; num4 >= firstBlockToCheck; num4--)
			{
				Vector128<byte> left = Load(ptr + num4 * Stride);
				uint num5 = ~Vector128.Equals(left, block).ExtractMostSignificantBits();
				int dist = (blockIndex - num4) * Stride;
				for (int i = 3; i <= BlockBytes - matchLen; i++)
				{
					float num6 = ((float)(BlockBytes - i - matchLen) * 13f + (float)MatchCostEstimate(dist, i) + matchBits) * num;
					if (num6 >= num3)
					{
						continue;
					}
					for (int j = 0; j <= BlockBytes - i; j++)
					{
						uint num7 = RunBits(j, i);
						if ((num7 & num2) != 0)
						{
							continue;
						}
						Vector128<byte> block2 = Vector128.ConditionalSelect(Vector128.LoadUnsafe(in arrayDataReference, (nuint)((j * 17 + i) * 16)), left, block);
						float num8;
						if ((num5 & num7) == 0)
						{
							num8 = mse;
						}
						else
						{
							int num9 = ((typeof(TFormat) == typeof(Bc7Format)) ? Bc7Format.Error(block2, source, ptr2, scratch, new TrialBudget(ErrorScale, threshMsErr, smoothBlockMseScale, num6, num3)) : TFormat.Error(block2, source, scratch));
							if (num9 < 0)
							{
								continue;
							}
							num8 = (float)num9 * ErrorScale;
						}
						if (!(num8 < threshMsErr))
						{
							continue;
						}
						float num10 = num8 * smoothBlockMseScale + num6;
						if (num10 < num3)
						{
							num3 = num10;
							best.T = num10;
							best.Block = block2;
							result.MatchLen = i;
							result.SrcWindowOfs = num4 * Stride + j;
							result.DstWindowOfs = blockIndex * Stride + j;
							result.DstBlockOfs = j;
							if (num6 >= num3)
							{
								break;
							}
						}
					}
				}
			}
			return result;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private unsafe static Vector128<byte> Load(byte* block)
		{
			if (BlockBytes != 16)
			{
				return Vector128.CreateScalar(Unsafe.ReadUnaligned<ulong>(block)).AsByte();
			}
			return Vector128.Load(block);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private unsafe static void Store(byte* block, Vector128<byte> value)
		{
			if (BlockBytes == 16)
			{
				value.Store(block);
			}
			else
			{
				Unsafe.WriteUnaligned(block, value.AsUInt64().ToScalar());
			}
		}
	}

	private const int MinMatchLength = 3;

	private const float LiteralBits = 13f;

	private const float MatchContinueBits = 1f;

	private const float MaxAllowedRmsIncreaseRatio = 10f;

	private const float MaxSmoothBlockStdDev = 18f;

	private const int HashSize = 8192;

	private static readonly byte[] RunMasks = BuildRunMasks();

	public unsafe static int ReduceEntropy(Span<byte> blocks, BcEncodeTarget target, ReadOnlySpan<byte> rgba, int width, int height, int rowPitch, float lambda, int lookbackWindow)
	{
		if (target == BcEncodeTarget.None)
		{
			throw new ArgumentOutOfRangeException("target");
		}
		int num = (width + 3) / 4;
		int num2 = (height + 3) / 4;
		int num3 = num * num2;
		if (blocks.Length < num3 * target.BlockBytes())
		{
			throw new ArgumentException("Too few blocks for the image.", "blocks");
		}
		if ((uint)(target - 3) <= 1u)
		{
			return ReduceEntropyChannels(blocks, target, rgba, width, height, rowPitch, lambda, lookbackWindow, num, num2);
		}
		bool flag = target == BcEncodeTarget.Bc7;
		uint[] array = ArrayPool<uint>.Shared.Rent(num3 * 16);
		float[] array2 = ArrayPool<float>.Shared.Rent(num3);
		try
		{
			GatherBlocks(rgba, width, height, rowPitch, num, num2, array);
			float num4 = 15f + 35f * System.Math.Min(1f, lambda / (flag ? 4f : 8f));
			ComputeBlockMseScales(array, num, num2, array2);
			for (int i = 0; i < num3; i++)
			{
				if (array2[i] > 0f)
				{
					array2[i] = Max(num4, array2[i] * System.Math.Min(lambda, 3f));
				}
			}
			int window = (flag ? System.Math.Max(16, lookbackWindow) : lookbackWindow);
			fixed (byte* blocks2 = blocks)
			{
				fixed (uint* pixels = array)
				{
					fixed (float* scales = array2)
					{
						return flag ? new Pass<Bc7Format>(blocks2, num3, pixels, scales, lambda, window, num4).Run() : new Pass<Bc1Format>(blocks2, num3, pixels, scales, lambda, window, num4).Run();
					}
				}
			}
		}
		finally
		{
			ArrayPool<uint>.Shared.Return(array);
			ArrayPool<float>.Shared.Return(array2);
		}
	}

	private unsafe static int ReduceEntropyChannels(Span<byte> blocks, BcEncodeTarget target, ReadOnlySpan<byte> rgba, int width, int height, int rowPitch, float lambda, int lookbackWindow, int blocksX, int blocksY)
	{
		int num = blocksX * blocksY;
		uint[] array = ArrayPool<uint>.Shared.Rent(num * 16);
		uint[] array2 = ArrayPool<uint>.Shared.Rent(num * 16);
		float[] array3 = ArrayPool<float>.Shared.Rent(num);
		try
		{
			GatherBlocks(rgba, width, height, rowPitch, blocksX, blocksY, array);
			array3.AsSpan(0, num).Fill(-1f);
			float smoothBlockMaxMseScale = 10f + 20f * System.Math.Min(1f, lambda / 4f);
			bool flag = target == BcEncodeTarget.Bc5;
			int num2 = 0;
			fixed (byte* ptr5 = blocks)
			{
				fixed (uint* ptr = array)
				{
					fixed (uint* ptr3 = array2)
					{
						fixed (float* scales = array3)
						{
							for (int i = 0; i < ((!flag) ? 1 : 2); i++)
							{
								for (int j = 0; j < num; j++)
								{
									byte* ptr2 = (byte*)(ptr + j * 16);
									byte* ptr4 = (byte*)(ptr3 + j * 16);
									for (int k = 0; k < 16; k++)
									{
										ptr4[k] = ptr2[k * 4 + i];
									}
								}
								num2 += (flag ? new Pass<Bc5HalfFormat>(ptr5 + i * 8, num, ptr3, scales, lambda, System.Math.Max(16, lookbackWindow), smoothBlockMaxMseScale).Run() : new Pass<Bc4Format>(ptr5, num, ptr3, scales, lambda, lookbackWindow, smoothBlockMaxMseScale).Run());
							}
						}
					}
				}
			}
			return num2;
		}
		finally
		{
			ArrayPool<uint>.Shared.Return(array);
			ArrayPool<uint>.Shared.Return(array2);
			ArrayPool<float>.Shared.Return(array3);
		}
	}

	private static byte[] BuildRunMasks()
	{
		byte[] array = new byte[4624];
		for (int i = 0; i <= 16; i++)
		{
			for (int j = 0; i + j <= 16; j++)
			{
				array.AsSpan((i * 17 + j) * 16 + i, j).Fill(byte.MaxValue);
			}
		}
		return array;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static uint RunBits(int ofs, int len)
	{
		return (uint)((1 << len) - 1 << ofs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static float Max(float a, float b)
	{
		if (!(a < b))
		{
			return a;
		}
		return b;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static int MatchCostEstimate(int dist, int matchLen)
	{
		return MatchLengthCost(matchLen) + MatchDistanceCost(dist);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static int MatchLengthCost(int matchLen)
	{
		if (matchLen < 12)
		{
			if (matchLen < 8)
			{
				if (matchLen < 6)
				{
					return 6;
				}
				return 7;
			}
			return 8;
		}
		return 9;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static int MatchDistanceCost(int dist)
	{
		int num = 5;
		if (dist < 512)
		{
			num += ((dist >= 4) ? (BitOperations.Log2((uint)dist) - 1) : 0);
		}
		else
		{
			num += BitOperations.Log2((uint)System.Math.Min(dist, 32767) >> 8) + 7;
			while (dist >= 32768)
			{
				num++;
				dist >>= 1;
			}
		}
		return num;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private unsafe static uint HashHsieh(byte* buffer, int len, int salt)
	{
		uint num = (uint)(len + (salt << 16));
		int num2 = len & 3;
		for (int num3 = len >> 2; num3 > 0; num3--)
		{
			num += Unsafe.ReadUnaligned<ushort>(buffer);
			uint num4 = (uint)(Unsafe.ReadUnaligned<ushort>(buffer + 2) << 11) ^ num;
			num = (num << 16) ^ num4;
			buffer += 4;
			num += num >> 11;
		}
		switch (num2)
		{
		case 1:
			num += (uint)(sbyte)(*buffer);
			num ^= num << 10;
			num += num >> 1;
			break;
		case 2:
			num += Unsafe.ReadUnaligned<ushort>(buffer);
			num ^= num << 11;
			num += num >> 17;
			break;
		case 3:
			num += Unsafe.ReadUnaligned<ushort>(buffer);
			num ^= num << 16;
			num ^= (uint)((sbyte)buffer[2] << 18);
			num += num >> 11;
			break;
		}
		num ^= num << 3;
		num += num >> 5;
		num ^= num << 4;
		num += num >> 17;
		num ^= num << 25;
		return num + (num >> 6);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private unsafe static int SquaredError(Vector128<byte> row0, Vector128<byte> row1, Vector128<byte> row2, Vector128<byte> row3, byte* source, bool rgbOnly)
	{
		Vector128<byte> vector = (rgbOnly ? Vector128.Create(16777215u).AsByte() : Vector128<byte>.AllBitsSet);
		if (Avx2.IsSupported)
		{
			Vector256<byte> vector2 = Vector256.Create(vector, vector);
			Vector256<byte> left = Vector256.Create(row0, row1);
			Vector256<byte> left2 = Vector256.Create(row2, row3);
			Vector256<byte> right = Vector256.Load(source);
			Vector256<byte> right2 = Vector256.Load(source + 32);
			Vector256<byte> difference = (Vector256.Max(left, right) - Vector256.Min(left, right)) & vector2;
			Vector256<byte> difference2 = (Vector256.Max(left2, right2) - Vector256.Min(left2, right2)) & vector2;
			return Vector256.Sum(Squares(difference) + Squares(difference2));
		}
		return (int)Vector128.Sum(Squares(row0, Vector128.Load(source), vector) + Squares(row1, Vector128.Load(source + 16), vector) + Squares(row2, Vector128.Load(source + 32), vector) + Squares(row3, Vector128.Load(source + 48), vector));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Vector256<int> Squares(Vector256<byte> difference)
	{
		Vector256<short> vector = Avx2.UnpackLow(difference, Vector256<byte>.Zero).AsInt16();
		Vector256<short> vector2 = Avx2.UnpackHigh(difference, Vector256<byte>.Zero).AsInt16();
		return Avx2.MultiplyAddAdjacent(vector, vector) + Avx2.MultiplyAddAdjacent(vector2, vector2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Vector128<uint> Squares(Vector128<byte> a, Vector128<byte> b, Vector128<byte> mask)
	{
		if (!AdvSimd.IsSupported)
		{
			return SquaresPortable(a, b, mask);
		}
		return SquaresNeon(a, b, mask);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Vector128<uint> SquaresNeon(Vector128<byte> a, Vector128<byte> b, Vector128<byte> mask)
	{
		Vector128<byte> vector = AdvSimd.AbsoluteDifference(a, b) & mask;
		return AdvSimd.AddPairwiseWideningAndAdd(AdvSimd.AddPairwiseWidening(AdvSimd.MultiplyWideningLower(vector.GetLower(), vector.GetLower())), AdvSimd.MultiplyWideningUpper(vector, vector));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Vector128<uint> SquaresPortable(Vector128<byte> a, Vector128<byte> b, Vector128<byte> mask)
	{
		var (source, source2) = Vector128.Widen((Vector128.Max(a, b) - Vector128.Min(a, b)) & mask);
		var (vector, vector2) = Vector128.Widen(source);
		var (vector3, vector4) = Vector128.Widen(source2);
		return vector * vector + vector2 * vector2 + vector3 * vector3 + vector4 * vector4;
	}

	private unsafe static float BlockMaxStdDev(uint* texels, int components)
	{
		float num = 0f;
		for (int i = 0; i < components; i++)
		{
			ulong num2 = 0uL;
			ulong num3 = 0uL;
			for (int j = 0; j < 16; j++)
			{
				uint num4 = ((byte*)texels)[j * 4 + i];
				num2 += num4;
				num3 += num4 * num4;
			}
			num = Max(num, MathF.Sqrt(16 * num3 - num2 * num2) / 16f);
		}
		return num;
	}

	private static void GatherBlocks(ReadOnlySpan<byte> rgba, int width, int height, int rowPitch, int blocksX, int blocksY, uint[] pixels)
	{
		ReadOnlySpan<uint> readOnlySpan = MemoryMarshal.Cast<byte, uint>(rgba.Slice(0, (height - 1) * rowPitch + width * 4));
		int num = rowPitch / 4;
		for (int i = 0; i < blocksY; i++)
		{
			for (int j = 0; j < blocksX; j++)
			{
				Span<uint> span = pixels.AsSpan((i * blocksX + j) * 16, 16);
				for (int k = 0; k < 4; k++)
				{
					int num2 = System.Math.Min(i * 4 + k, height - 1);
					for (int l = 0; l < 4; l++)
					{
						span[k * 4 + l] = readOnlySpan[num2 * num + System.Math.Min(j * 4 + l, width - 1)];
					}
				}
			}
		}
	}

	private unsafe static void ComputeBlockMseScales(uint[] pixels, int blocksX, int blocksY, float[] scales)
	{
		int num = blocksX * blocksY;
		byte[] array = new byte[num];
		byte[] array2 = new byte[num];
		fixed (uint* ptr = pixels)
		{
			for (int j = 0; j < num; j++)
			{
				byte* ptr2 = (byte*)(ptr + j * 16);
				uint num2 = 0u;
				for (int k = 0; k < 16; k++)
				{
					num2 += (uint)(13938 * ptr2[k * 4] + 46869 * ptr2[k * 4 + 1] + 4729 * ptr2[k * 4 + 2] + 32768 >>> 16);
				}
				float num3 = System.Math.Clamp(BlockMaxStdDev(ptr + j * 16, 3) / 2.9f, 0f, 1f);
				num3 *= num3;
				float num4 = (float)num2 / 16f;
				if (num4 < 13f || num4 >= 222f)
				{
					num3 = 1f;
				}
				array[j] = (byte)System.Math.Min((int)(num3 * 255f + 0.5f), 255);
			}
		}
		for (int l = 0; l < blocksY; l++)
		{
			for (int m = 0; m < blocksX; m++)
			{
				array2[l * blocksX + m] = (byte)((CountBusyNeighbours(array, blocksX, blocksY, m, l) > 0) ? 255u : 0u);
			}
		}
		byte[] array3 = array2;
		array2 = array;
		array = array3;
		for (int n = 0; n < 32; n++)
		{
			array.CopyTo(array2, 0);
			bool flag = false;
			for (int num5 = 0; num5 < blocksY; num5++)
			{
				for (int num6 = 0; num6 < blocksX; num6++)
				{
					if (array[num5 * blocksX + num6] < byte.MaxValue && CountBusyNeighbours(array, blocksX, blocksY, num6, num5) >= 5)
					{
						array2[num5 * blocksX + num6] = byte.MaxValue;
						flag = true;
					}
				}
			}
			byte[] array4 = array2;
			array2 = array;
			array = array4;
			if (!flag)
			{
				break;
			}
		}
		byte[] filled = array2;
		array.CopyTo(filled, 0);
		List<int> list = new List<int>();
		Stack<int> stack = new Stack<int>();
		for (int num7 = 0; num7 < num; num7++)
		{
			if (filled[num7] != 0)
			{
				continue;
			}
			list.Clear();
			stack.Push(num7);
			filled[num7] = byte.MaxValue;
			int result;
			while (stack.TryPop(out result))
			{
				list.Add(result);
				int num8 = result % blocksX;
				int num9 = result / blocksX;
				if (num8 > 0)
				{
					Visit(result - 1);
				}
				if (num8 + 1 < blocksX)
				{
					Visit(result + 1);
				}
				if (num9 > 0)
				{
					Visit(result - blocksX);
				}
				if (num9 + 1 < blocksY)
				{
					Visit(result + blocksX);
				}
			}
			if (list.Count >= 64)
			{
				continue;
			}
			foreach (int item in list)
			{
				array[item] = byte.MaxValue;
			}
		}
		for (int num10 = 0; num10 < num; num10++)
		{
			scales[num10] = ((array[num10] == 0) ? 120f : (-1f));
		}
		void Visit(int i)
		{
			if (filled[i] == 0)
			{
				filled[i] = byte.MaxValue;
				stack.Push(i);
			}
		}
	}

	private static int CountBusyNeighbours(byte[] vis, int blocksX, int blocksY, int x, int y)
	{
		int num = 0;
		for (int i = -1; i <= 1; i++)
		{
			for (int j = -1; j <= 1; j++)
			{
				if (vis[System.Math.Clamp(y + i, 0, blocksY - 1) * blocksX + System.Math.Clamp(x + j, 0, blocksX - 1)] == byte.MaxValue)
				{
					num++;
				}
			}
		}
		return num;
	}
}
