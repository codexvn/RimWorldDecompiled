using System;
using System.Buffers;
using System.Buffers.Binary;
using System.IO;
using System.IO.Compression;
using System.Runtime.Intrinsics;

namespace Waylith.Rendering;

public static class PngDecoder
{
	private static ReadOnlySpan<byte> Signature => new byte[8] { 137, 80, 78, 71, 13, 10, 26, 10 };

	public static byte[]? TryDecodeRgba32(ReadOnlySpan<byte> file, out int width, out int height)
	{
		width = (height = 0);
		if (file.Length < 33 || !file.Slice(0, 8).SequenceEqual(Signature))
		{
			return null;
		}
		byte[] array = ArrayPool<byte>.Shared.Rent(file.Length);
		try
		{
			return Decode(file, array, ref width, ref height);
		}
		finally
		{
			ArrayPool<byte>.Shared.Return(array);
		}
	}

	private static byte[]? Decode(ReadOnlySpan<byte> file, byte[] compressed, ref int width, ref int height)
	{
		int num = 0;
		int num2 = 0;
		int num3 = 8;
		while (num3 + 8 <= file.Length)
		{
			int num4 = BinaryPrimitives.ReadInt32BigEndian(file.Slice(num3));
			uint num5 = BinaryPrimitives.ReadUInt32BigEndian(file.Slice(num3 + 4));
			int num6 = num3 + 8;
			if (num4 < 0 || num4 > file.Length - num6)
			{
				return null;
			}
			ReadOnlySpan<byte> source = file.Slice(num6, num4);
			switch (num5)
			{
			case 1229472850u:
			{
				if (num4 < 13)
				{
					return null;
				}
				width = BinaryPrimitives.ReadInt32BigEndian(source);
				height = BinaryPrimitives.ReadInt32BigEndian(source.Slice(4));
				byte num7 = source[8];
				byte b = source[9];
				byte b2 = source[12];
				if (num7 != 8 || b2 != 0 || source[10] != 0 || source[11] != 0 || width <= 0 || height <= 0 || (long)width * (long)height > 429496729)
				{
					return null;
				}
				num = b switch
				{
					2 => 3, 
					6 => 4, 
					_ => 0, 
				};
				if (num == 0)
				{
					return null;
				}
				break;
			}
			case 1951551059u:
				return null;
			case 1229209940u:
				source.CopyTo(compressed.AsSpan(num2));
				num2 += source.Length;
				break;
			case 1229278788u:
				num3 = file.Length;
				continue;
			}
			num3 = num6 + num4 + 4;
		}
		if (num == 0 || num2 == 0)
		{
			return null;
		}
		byte[] array = GC.AllocateUninitializedArray<byte>(width * height * 4);
		try
		{
			using MemoryStream stream = new MemoryStream(compressed, 0, num2, writable: false);
			using ZLibStream inflater = new ZLibStream(stream, CompressionMode.Decompress);
			return ((num == 4) ? InflateRgba(inflater, array, width, height) : InflateRgb(inflater, array, width, height)) ? array : null;
		}
		catch (Exception ex) when (((ex is InvalidDataException || ex is EndOfStreamException) ? 1 : 0) != 0)
		{
			return null;
		}
	}

	private unsafe static bool InflateRgba(Stream inflater, byte[] rgba, int width, int height)
	{
		int num = width * 4;
		byte[] array = new byte[num + 1];
		fixed (byte* ptr3 = array)
		{
			fixed (byte* ptr = rgba)
			{
				for (int i = 0; i < height; i++)
				{
					inflater.ReadExactly(array);
					uint* ptr2 = (uint*)(ptr + (long)i * (long)num);
					if (!UnfilterRow(*ptr3, (uint*)(ptr3 + 1), ptr2, (i == 0) ? null : (ptr2 - width), width, writeThree: false))
					{
						return false;
					}
				}
			}
		}
		return true;
	}

	private unsafe static bool InflateRgb(Stream inflater, byte[] rgba, int width, int height)
	{
		int num = width * 3;
		byte[] array = new byte[num + 2];
		byte[] array2 = new byte[num + 2];
		fixed (byte* ptr3 = rgba)
		{
			for (int i = 0; i < height; i++)
			{
				inflater.ReadExactly(array.AsSpan(0, num + 1));
				fixed (byte* ptr = array)
				{
					fixed (byte* ptr2 = array2)
					{
						if (!UnfilterRow(*ptr, (uint*)(ptr + 1), (uint*)(ptr + 1), (uint*)((i == 0) ? null : (ptr2 + 1)), width, writeThree: true))
						{
							return false;
						}
						uint* ptr4 = (uint*)(ptr3 + (long)i * (long)width * 4);
						for (int j = 0; j < width; j++)
						{
							ptr4[j] = *(uint*)(ptr + 1 + j * 3) | 0xFF000000u;
						}
					}
				}
				byte[] array3 = array2;
				array2 = array;
				array = array3;
			}
		}
		return true;
	}

	private unsafe static bool UnfilterRow(byte filter, uint* filtered, uint* output, uint* up, int width, bool writeThree)
	{
		int num = (writeThree ? 3 : 4);
		switch (filter)
		{
		case 0:
			if (filtered != output)
			{
				Buffer.MemoryCopy(filtered, output, (long)width * (long)num, (long)width * (long)num);
			}
			return true;
		case 1:
		{
			uint y2 = 0u;
			for (int j = 0; j < width * num; j += num)
			{
				Store((byte*)output + j, y2 = AddBytes(*(uint*)((byte*)filtered + j), y2), writeThree);
			}
			return true;
		}
		case 2:
		{
			for (int k = 0; k < width * num; k += num)
			{
				Store((byte*)output + k, AddBytes(*(uint*)((byte*)filtered + k), (up != null) ? (*(uint*)((byte*)up + k)) : 0u), writeThree);
			}
			return true;
		}
		case 3:
		{
			uint x = 0u;
			for (int i = 0; i < width * num; i += num)
			{
				uint y = ((up != null) ? (*(uint*)((byte*)up + i)) : 0u);
				Store((byte*)output + i, x = AddBytes(*(uint*)((byte*)filtered + i), AverageBytes(x, y)), writeThree);
			}
			return true;
		}
		case 4:
			PaethRow((byte*)filtered, (byte*)output, (byte*)up, width, num, writeThree);
			return true;
		default:
			return false;
		}
	}

	private unsafe static void PaethRow(byte* source, byte* target, byte* above, int width, int step, bool writeThree)
	{
		Vector128<short> vector = Vector128<short>.Zero;
		Vector128<short> vector2 = Vector128<short>.Zero;
		Vector128<short> vector3 = Vector128.Create((short)255);
		for (int i = 0; i < width * step; i += step)
		{
			Vector128<short> vector4 = ((above == null) ? Vector128<short>.Zero : Widen(*(uint*)(above + i)));
			Vector128<short> left = Vector128.Abs(vector4 - vector2);
			Vector128<short> vector5 = Vector128.Abs(vector - vector2);
			Vector128<short> right = Vector128.Abs(vector + vector4 - vector2 - vector2);
			Vector128<short> condition = Vector128.LessThanOrEqual(left, vector5) & Vector128.LessThanOrEqual(left, right);
			Vector128<short> condition2 = Vector128.LessThanOrEqual(vector5, right);
			Vector128<short> vector6 = Vector128.ConditionalSelect(condition, vector, Vector128.ConditionalSelect(condition2, vector4, vector2));
			Vector128<short> vector7 = (Widen(*(uint*)(source + i)) + vector6) & vector3;
			Store(target + i, Vector128.Narrow(vector7.AsUInt16(), vector7.AsUInt16()).AsUInt32().ToScalar(), writeThree);
			vector = vector7;
			vector2 = vector4;
		}
	}

	private static Vector128<short> Widen(uint pixel)
	{
		return Vector128.WidenLower(Vector128.CreateScalarUnsafe(pixel).AsByte()).AsInt16();
	}

	private unsafe static void Store(byte* target, uint value, bool writeThree)
	{
		if (writeThree)
		{
			*(ushort*)target = (ushort)value;
			target[2] = (byte)(value >> 16);
		}
		else
		{
			*(uint*)target = value;
		}
	}

	private static uint AddBytes(uint x, uint y)
	{
		return ((x & 0x7F7F7F7F) + (y & 0x7F7F7F7F)) ^ ((x ^ y) & 0x80808080u);
	}

	private static uint AverageBytes(uint x, uint y)
	{
		return (x & y) + (((x ^ y) & 0xFEFEFEFEu) >> 1);
	}
}
