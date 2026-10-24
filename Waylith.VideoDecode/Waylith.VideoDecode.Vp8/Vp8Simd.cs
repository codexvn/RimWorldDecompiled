using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;

namespace Waylith.VideoDecode.Vp8;

internal static class Vp8Simd
{
	public static bool Enabled { get; set; } = Vector128.IsHardwareAccelerated;

	public static bool PortableZip { get; set; }

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private unsafe static Vector128<short> Load8(byte* p)
	{
		return Vector128.WidenLower(Vector128.CreateScalarUnsafe(Unsafe.ReadUnaligned<ulong>(p)).AsByte()).AsInt16();
	}

	public unsafe static void SixTapPass(byte* src, int srcStride, int step, short* filter, byte* dst, int dstStride, int width, int height)
	{
		Vector128<short> vector = Vector128.Create(*filter);
		Vector128<short> vector2 = Vector128.Create(filter[1]);
		Vector128<short> vector3 = Vector128.Create(filter[2]);
		Vector128<short> vector4 = Vector128.Create(filter[3]);
		Vector128<short> vector5 = Vector128.Create(filter[4]);
		Vector128<short> vector6 = Vector128.Create(filter[5]);
		Vector128<short> vector7 = Vector128.Create((short)8256);
		Vector128<short> vector8 = Vector128.Create((short)64);
		Vector128<short> right = Vector128.Create((short)255);
		for (int i = 0; i < height; i++)
		{
			for (int j = 0; j < width; j += 8)
			{
				byte* ptr = src + j;
				Vector128<byte> vector9 = Vector128.Narrow(Vector128.Min(Vector128.Max(Vector128.ShiftRightLogical((Load8(ptr - 2 * step) * vector + Load8(ptr - step) * vector2 + Load8(ptr) * vector3 + Load8(ptr + step) * vector4 + Load8(ptr + 2 * step) * vector5 + Load8(ptr + 3 * step) * vector6 + vector7).AsUInt16(), 7).AsInt16() - vector8, Vector128<short>.Zero), right).AsUInt16(), Vector128<ushort>.Zero);
				if (width >= 8)
				{
					Unsafe.WriteUnaligned(dst + j, vector9.AsUInt64().ToScalar());
				}
				else
				{
					Unsafe.WriteUnaligned(dst + j, vector9.AsUInt32().ToScalar());
				}
			}
			src += srcStride;
			dst += dstStride;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Vector128<byte> ZipLow(Vector128<byte> a, Vector128<byte> b)
	{
		if (Sse2.IsSupported && !PortableZip)
		{
			return Sse2.UnpackLow(a, b);
		}
		if (AdvSimd.Arm64.IsSupported && !PortableZip)
		{
			return AdvSimd.Arm64.ZipLow(a, b);
		}
		return Vector128.Shuffle(a, Vector128.Create(0, byte.MaxValue, 1, byte.MaxValue, 2, byte.MaxValue, 3, byte.MaxValue, 4, byte.MaxValue, 5, byte.MaxValue, 6, byte.MaxValue, 7, byte.MaxValue)) | Vector128.Shuffle(b, Vector128.Create(byte.MaxValue, 0, byte.MaxValue, 1, byte.MaxValue, 2, byte.MaxValue, 3, byte.MaxValue, 4, byte.MaxValue, 5, byte.MaxValue, 6, byte.MaxValue, 7));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Vector128<byte> ZipHigh(Vector128<byte> a, Vector128<byte> b)
	{
		if (Sse2.IsSupported && !PortableZip)
		{
			return Sse2.UnpackHigh(a, b);
		}
		if (AdvSimd.Arm64.IsSupported && !PortableZip)
		{
			return AdvSimd.Arm64.ZipHigh(a, b);
		}
		return Vector128.Shuffle(a, Vector128.Create(8, byte.MaxValue, 9, byte.MaxValue, 10, byte.MaxValue, 11, byte.MaxValue, 12, byte.MaxValue, 13, byte.MaxValue, 14, byte.MaxValue, 15, byte.MaxValue)) | Vector128.Shuffle(b, Vector128.Create(byte.MaxValue, 8, byte.MaxValue, 9, byte.MaxValue, 10, byte.MaxValue, 11, byte.MaxValue, 12, byte.MaxValue, 13, byte.MaxValue, 14, byte.MaxValue, 15));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Vector128<byte> ZipLow16(Vector128<byte> a, Vector128<byte> b)
	{
		if (Sse2.IsSupported && !PortableZip)
		{
			return Sse2.UnpackLow(a.AsUInt16(), b.AsUInt16()).AsByte();
		}
		if (AdvSimd.Arm64.IsSupported && !PortableZip)
		{
			return AdvSimd.Arm64.ZipLow(a.AsUInt16(), b.AsUInt16()).AsByte();
		}
		return Vector128.Shuffle(a, Vector128.Create(0, 1, byte.MaxValue, byte.MaxValue, 2, 3, byte.MaxValue, byte.MaxValue, 4, 5, byte.MaxValue, byte.MaxValue, 6, 7, byte.MaxValue, byte.MaxValue)) | Vector128.Shuffle(b, Vector128.Create(byte.MaxValue, byte.MaxValue, 0, 1, byte.MaxValue, byte.MaxValue, 2, 3, byte.MaxValue, byte.MaxValue, 4, 5, byte.MaxValue, byte.MaxValue, 6, 7));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Vector128<byte> ZipHigh16(Vector128<byte> a, Vector128<byte> b)
	{
		if (Sse2.IsSupported && !PortableZip)
		{
			return Sse2.UnpackHigh(a.AsUInt16(), b.AsUInt16()).AsByte();
		}
		if (AdvSimd.Arm64.IsSupported && !PortableZip)
		{
			return AdvSimd.Arm64.ZipHigh(a.AsUInt16(), b.AsUInt16()).AsByte();
		}
		return Vector128.Shuffle(a, Vector128.Create(8, 9, byte.MaxValue, byte.MaxValue, 10, 11, byte.MaxValue, byte.MaxValue, 12, 13, byte.MaxValue, byte.MaxValue, 14, 15, byte.MaxValue, byte.MaxValue)) | Vector128.Shuffle(b, Vector128.Create(byte.MaxValue, byte.MaxValue, 8, 9, byte.MaxValue, byte.MaxValue, 10, 11, byte.MaxValue, byte.MaxValue, 12, 13, byte.MaxValue, byte.MaxValue, 14, 15));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Vector128<byte> ZipLow32(Vector128<byte> a, Vector128<byte> b)
	{
		if (Sse2.IsSupported && !PortableZip)
		{
			return Sse2.UnpackLow(a.AsUInt32(), b.AsUInt32()).AsByte();
		}
		if (AdvSimd.Arm64.IsSupported && !PortableZip)
		{
			return AdvSimd.Arm64.ZipLow(a.AsUInt32(), b.AsUInt32()).AsByte();
		}
		return Vector128.Shuffle(a, Vector128.Create(0, 1, 2, 3, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, 4, 5, 6, 7, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)) | Vector128.Shuffle(b, Vector128.Create(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, 0, 1, 2, 3, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, 4, 5, 6, 7));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Vector128<byte> ZipHigh32(Vector128<byte> a, Vector128<byte> b)
	{
		if (Sse2.IsSupported && !PortableZip)
		{
			return Sse2.UnpackHigh(a.AsUInt32(), b.AsUInt32()).AsByte();
		}
		if (AdvSimd.Arm64.IsSupported && !PortableZip)
		{
			return AdvSimd.Arm64.ZipHigh(a.AsUInt32(), b.AsUInt32()).AsByte();
		}
		return Vector128.Shuffle(a, Vector128.Create(8, 9, 10, 11, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, 12, 13, 14, 15, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)) | Vector128.Shuffle(b, Vector128.Create(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, 8, 9, 10, 11, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, 12, 13, 14, 15));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Vector128<byte> Low64(Vector128<byte> a, Vector128<byte> b)
	{
		return Vector128.Create(a.AsUInt64().ToScalar(), b.AsUInt64().ToScalar()).AsByte();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Vector128<byte> High64(Vector128<byte> a, Vector128<byte> b)
	{
		return Vector128.Create(a.AsUInt64().GetElement(1), b.AsUInt64().GetElement(1)).AsByte();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private unsafe static Vector128<byte> Row8(byte* p)
	{
		return Vector128.CreateScalarUnsafe(Unsafe.ReadUnaligned<ulong>(p)).AsByte();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private unsafe static void Transpose8(byte* s, int stride, out Vector128<byte> c01, out Vector128<byte> c23, out Vector128<byte> c45, out Vector128<byte> c67)
	{
		Vector128<byte> a = ZipLow(Row8(s), Row8(s + stride));
		Vector128<byte> b = ZipLow(Row8(s + 2 * stride), Row8(s + 3 * stride));
		Vector128<byte> a2 = ZipLow(Row8(s + 4 * stride), Row8(s + 5 * stride));
		Vector128<byte> b2 = ZipLow(Row8(s + 6 * stride), Row8(s + 7 * stride));
		Vector128<byte> a3 = ZipLow16(a, b);
		Vector128<byte> a4 = ZipHigh16(a, b);
		Vector128<byte> b3 = ZipLow16(a2, b2);
		Vector128<byte> b4 = ZipHigh16(a2, b2);
		c01 = ZipLow32(a3, b3);
		c23 = ZipHigh32(a3, b3);
		c45 = ZipLow32(a4, b4);
		c67 = ZipHigh32(a4, b4);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private unsafe static void LoadColumns(byte* first, byte* second, int stride, Vector128<byte>* columns)
	{
		Transpose8(first, stride, out var c, out var c2, out var c3, out var c4);
		Transpose8(second, stride, out var c5, out var c6, out var c7, out var c8);
		*columns = Low64(c, c5);
		columns[1] = High64(c, c5);
		columns[2] = Low64(c2, c6);
		columns[3] = High64(c2, c6);
		columns[4] = Low64(c3, c7);
		columns[5] = High64(c3, c7);
		columns[6] = Low64(c4, c8);
		columns[7] = High64(c4, c8);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private unsafe static void StoreColumns(byte* first, byte* second, int stride, Vector128<byte>* columns)
	{
		Vector128<byte> a = ZipLow(*columns, columns[1]);
		Vector128<byte> b = ZipLow(columns[2], columns[3]);
		Vector128<byte> a2 = ZipLow(columns[4], columns[5]);
		Vector128<byte> b2 = ZipLow(columns[6], columns[7]);
		StoreRows(first, stride, ZipLow16(a, b), ZipHigh16(a, b), ZipLow16(a2, b2), ZipHigh16(a2, b2));
		a = ZipHigh(*columns, columns[1]);
		b = ZipHigh(columns[2], columns[3]);
		a2 = ZipHigh(columns[4], columns[5]);
		b2 = ZipHigh(columns[6], columns[7]);
		StoreRows(second, stride, ZipLow16(a, b), ZipHigh16(a, b), ZipLow16(a2, b2), ZipHigh16(a2, b2));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private unsafe static void StoreRows(byte* s, int stride, Vector128<byte> a, Vector128<byte> b, Vector128<byte> c, Vector128<byte> d)
	{
		Vector128<ulong> vector = ZipLow32(a, c).AsUInt64();
		Vector128<ulong> vector2 = ZipHigh32(a, c).AsUInt64();
		Vector128<ulong> vector3 = ZipLow32(b, d).AsUInt64();
		Vector128<ulong> vector4 = ZipHigh32(b, d).AsUInt64();
		Unsafe.WriteUnaligned(s, vector.ToScalar());
		Unsafe.WriteUnaligned(s + stride, vector.GetElement(1));
		Unsafe.WriteUnaligned(s + 2 * stride, vector2.ToScalar());
		Unsafe.WriteUnaligned(s + 3 * stride, vector2.GetElement(1));
		Unsafe.WriteUnaligned(s + 4 * stride, vector3.ToScalar());
		Unsafe.WriteUnaligned(s + 5 * stride, vector3.GetElement(1));
		Unsafe.WriteUnaligned(s + 6 * stride, vector4.ToScalar());
		Unsafe.WriteUnaligned(s + 7 * stride, vector4.GetElement(1));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Vector128<byte> AbsDiff(Vector128<byte> a, Vector128<byte> b)
	{
		return Vector128.SubtractSaturate(a, b) | Vector128.SubtractSaturate(b, a);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private unsafe static Vector128<sbyte> Masks(Vector128<byte>* p, int edgeLimit, int interior, int threshold, out Vector128<sbyte> highVariance)
	{
		Vector128<byte> left = AbsDiff(p[2], p[3]);
		Vector128<byte> right = AbsDiff(p[5], p[4]);
		Vector128<byte> vector = Vector128.SubtractSaturate(Vector128.Max(Vector128.Max(Vector128.Max(AbsDiff(*p, p[1]), AbsDiff(p[1], p[2])), Vector128.Max(left, right)), Vector128.Max(AbsDiff(p[6], p[5]), AbsDiff(p[7], p[6]))), Vector128.Create((byte)interior));
		Vector128<byte> vector2 = AbsDiff(p[3], p[4]);
		Vector128<byte> vector3 = Vector128.SubtractSaturate(Vector128.AddSaturate(Vector128.AddSaturate(vector2, vector2), Vector128.ShiftRightLogical(AbsDiff(p[2], p[5]), 1)), Vector128.Create((byte)edgeLimit));
		highVariance = ~Vector128.Equals(Vector128.SubtractSaturate(Vector128.Max(left, right), Vector128.Create((byte)threshold)), Vector128<byte>.Zero).AsSByte();
		return Vector128.Equals(vector | vector3, Vector128<byte>.Zero).AsSByte();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private unsafe static bool FilterBlockEdge(Vector128<byte>* p, int edgeLimit, int interior, int threshold)
	{
		Vector128<sbyte> highVariance;
		Vector128<sbyte> vector = Masks(p, edgeLimit, interior, threshold, out highVariance);
		if (vector == Vector128<sbyte>.Zero)
		{
			return false;
		}
		Vector128<byte> vector2 = Vector128.Create((byte)128);
		Vector128<sbyte> left = (p[2] ^ vector2).AsSByte();
		Vector128<sbyte> vector3 = (p[3] ^ vector2).AsSByte();
		Vector128<sbyte> left2 = (p[4] ^ vector2).AsSByte();
		Vector128<sbyte> vector4 = (p[5] ^ vector2).AsSByte();
		Vector128<sbyte> left3 = Vector128.SubtractSaturate(left, vector4) & highVariance;
		Vector128<sbyte> right = Vector128.SubtractSaturate(left2, vector3);
		Vector128<sbyte> left4 = Vector128.AddSaturate(Vector128.AddSaturate(Vector128.AddSaturate(left3, right), right), right) & vector;
		Vector128<sbyte> vector5 = Vector128.ShiftRightArithmetic(Vector128.AddSaturate(left4, Vector128.Create((sbyte)4)), 3);
		Vector128<sbyte> right2 = Vector128.ShiftRightArithmetic(Vector128.AddSaturate(left4, Vector128.Create((sbyte)3)), 3);
		left2 = Vector128.SubtractSaturate(left2, vector5);
		vector3 = Vector128.AddSaturate(vector3, right2);
		Vector128<sbyte> right3 = Vector128.AndNot(Vector128.ShiftRightArithmetic(vector5 + Vector128<sbyte>.One, 1), highVariance);
		vector4 = Vector128.SubtractSaturate(vector4, right3);
		left = Vector128.AddSaturate(left, right3);
		p[2] = left.AsByte() ^ vector2;
		p[3] = vector3.AsByte() ^ vector2;
		p[4] = left2.AsByte() ^ vector2;
		p[5] = vector4.AsByte() ^ vector2;
		return true;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Vector128<sbyte> Weighted(Vector128<sbyte> value, short weight)
	{
		Vector128<short> vector = Vector128.Create(weight);
		Vector128<short> vector2 = Vector128.Create((short)63);
		Vector128<short> lower = Vector128.ShiftRightArithmetic(Vector128.WidenLower(value) * vector + vector2, 7);
		Vector128<short> upper = Vector128.ShiftRightArithmetic(Vector128.WidenUpper(value) * vector + vector2, 7);
		return Vector128.Narrow(lower, upper);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private unsafe static bool FilterMacroblockEdge(Vector128<byte>* p, int edgeLimit, int interior, int threshold)
	{
		Vector128<sbyte> highVariance;
		Vector128<sbyte> vector = Masks(p, edgeLimit, interior, threshold, out highVariance);
		if (vector == Vector128<sbyte>.Zero)
		{
			return false;
		}
		Vector128<byte> vector2 = Vector128.Create((byte)128);
		Vector128<sbyte> left = (p[1] ^ vector2).AsSByte();
		Vector128<sbyte> left2 = (p[2] ^ vector2).AsSByte();
		Vector128<sbyte> vector3 = (p[3] ^ vector2).AsSByte();
		Vector128<sbyte> left3 = (p[4] ^ vector2).AsSByte();
		Vector128<sbyte> vector4 = (p[5] ^ vector2).AsSByte();
		Vector128<sbyte> left4 = (p[6] ^ vector2).AsSByte();
		Vector128<sbyte> right = Vector128.SubtractSaturate(left3, vector3);
		Vector128<sbyte> vector5 = Vector128.AddSaturate(Vector128.AddSaturate(Vector128.AddSaturate(Vector128.SubtractSaturate(left2, vector4), right), right), right) & vector;
		Vector128<sbyte> left5 = vector5 & highVariance;
		left3 = Vector128.SubtractSaturate(left3, Vector128.ShiftRightArithmetic(Vector128.AddSaturate(left5, Vector128.Create((sbyte)4)), 3));
		vector3 = Vector128.AddSaturate(vector3, Vector128.ShiftRightArithmetic(Vector128.AddSaturate(left5, Vector128.Create((sbyte)3)), 3));
		Vector128<sbyte> value = Vector128.AndNot(vector5, highVariance);
		Vector128<sbyte> right2 = Weighted(value, 27);
		left3 = Vector128.SubtractSaturate(left3, right2);
		vector3 = Vector128.AddSaturate(vector3, right2);
		right2 = Weighted(value, 18);
		vector4 = Vector128.SubtractSaturate(vector4, right2);
		left2 = Vector128.AddSaturate(left2, right2);
		right2 = Weighted(value, 9);
		left4 = Vector128.SubtractSaturate(left4, right2);
		left = Vector128.AddSaturate(left, right2);
		p[1] = left.AsByte() ^ vector2;
		p[2] = left2.AsByte() ^ vector2;
		p[3] = vector3.AsByte() ^ vector2;
		p[4] = left3.AsByte() ^ vector2;
		p[5] = vector4.AsByte() ^ vector2;
		p[6] = left4.AsByte() ^ vector2;
		return true;
	}

	public unsafe static void HorizontalEdge(byte* s, int stride, bool macroblockEdge, int edgeLimit, int interior, int threshold)
	{
		Vector128<byte>* ptr = stackalloc Vector128<byte>[8];
		for (int i = 0; i < 8; i++)
		{
			ptr[i] = Vector128.Load(s + (i - 4) * stride);
		}
		if (macroblockEdge ? FilterMacroblockEdge(ptr, edgeLimit, interior, threshold) : FilterBlockEdge(ptr, edgeLimit, interior, threshold))
		{
			for (int j = 1; j < 7; j++)
			{
				ptr[j].Store(s + (j - 4) * stride);
			}
		}
	}

	public unsafe static void HorizontalEdgeChroma(byte* u, byte* v, int stride, bool macroblockEdge, int edgeLimit, int interior, int threshold)
	{
		Vector128<byte>* ptr = stackalloc Vector128<byte>[8];
		for (int i = 0; i < 8; i++)
		{
			ptr[i] = Vector128.Create(Unsafe.ReadUnaligned<ulong>(u + (i - 4) * stride), Unsafe.ReadUnaligned<ulong>(v + (i - 4) * stride)).AsByte();
		}
		if (macroblockEdge ? FilterMacroblockEdge(ptr, edgeLimit, interior, threshold) : FilterBlockEdge(ptr, edgeLimit, interior, threshold))
		{
			for (int j = 1; j < 7; j++)
			{
				Unsafe.WriteUnaligned(u + (j - 4) * stride, ptr[j].AsUInt64().ToScalar());
				Unsafe.WriteUnaligned(v + (j - 4) * stride, ptr[j].AsUInt64().GetElement(1));
			}
		}
	}

	public unsafe static void VerticalEdge(byte* first, byte* second, int stride, bool macroblockEdge, int edgeLimit, int interior, int threshold)
	{
		Vector128<byte>* ptr = stackalloc Vector128<byte>[8];
		LoadColumns(first - 4, second - 4, stride, ptr);
		if (macroblockEdge ? FilterMacroblockEdge(ptr, edgeLimit, interior, threshold) : FilterBlockEdge(ptr, edgeLimit, interior, threshold))
		{
			StoreColumns(first - 4, second - 4, stride, ptr);
		}
	}
}
