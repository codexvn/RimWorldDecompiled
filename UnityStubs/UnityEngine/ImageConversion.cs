using System;
using System.IO;
using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Experimental.Rendering;

namespace UnityEngine;

public static class ImageConversion
{
	private static uint[] _crcTable;

	public static byte[] EncodeToPNG(this Texture2D tex)
	{
		ArgumentNullException.ThrowIfNull(tex, "tex");
		return EncodePng(tex.Rgba32TopDown(), tex.width, tex.height);
	}

	internal static byte[] EncodePng(ReadOnlySpan<byte> pixels, int width, int height)
	{
		int num = width * 4;
		byte[] array = new byte[height * (1 + num)];
		for (int i = 0; i < height; i++)
		{
			pixels.Slice(i * num, num).CopyTo(array.AsSpan(i * (1 + num) + 1));
		}
		byte[] array2;
		using (MemoryStream memoryStream = new MemoryStream())
		{
			using (ZLibStream zLibStream = new ZLibStream(memoryStream, CompressionLevel.Fastest, leaveOpen: true))
			{
				zLibStream.Write(array, 0, array.Length);
			}
			array2 = memoryStream.ToArray();
		}
		using MemoryStream memoryStream2 = new MemoryStream();
		memoryStream2.Write(new byte[8] { 137, 80, 78, 71, 13, 10, 26, 10 });
		Span<byte> span = stackalloc byte[13];
		WriteBigEndian(span, 0, width);
		WriteBigEndian(span, 4, height);
		span[8] = 8;
		span[9] = 6;
		span[10] = 0;
		span[11] = 0;
		span[12] = 0;
		WriteChunk(memoryStream2, "IHDR", span);
		WriteChunk(memoryStream2, "IDAT", array2);
		WriteChunk(memoryStream2, "IEND", default(ReadOnlySpan<byte>));
		return memoryStream2.ToArray();
	}

	public static byte[] EncodeArrayToPNG(Array array, GraphicsFormat format, uint width, uint height, uint rowBytes = 0u)
	{
		return EncodePng(ToRgba32(array, format, (int)width, (int)height, (int)rowBytes), (int)width, (int)height);
	}

	public static byte[] EncodeArrayToJPG(Array array, GraphicsFormat format, uint width, uint height, uint rowBytes = 0u, int quality = 75)
	{
		byte[] data = ToRgba32(array, format, (int)width, (int)height, (int)rowBytes);
		Texture2D texture2D = new Texture2D((int)width, (int)height, TextureFormat.RGBA32, mipChain: false);
		try
		{
			texture2D.SetPixelData(data, 0);
			return texture2D.EncodeToJPG(quality);
		}
		finally
		{
			Object.DestroyImmediate(texture2D);
		}
	}

	private static byte[] ToRgba32(Array array, GraphicsFormat format, int width, int height, int rowBytes)
	{
		ArgumentNullException.ThrowIfNull(array, "array");
		int num;
		switch (format)
		{
		case GraphicsFormat.R8G8B8A8_SRGB:
		case GraphicsFormat.R8G8B8A8_UNorm:
		case GraphicsFormat.B8G8R8A8_SRGB:
		case GraphicsFormat.B8G8R8A8_UNorm:
			num = 4;
			break;
		case GraphicsFormat.R8G8B8_SRGB:
		case GraphicsFormat.R8G8B8_UNorm:
			num = 3;
			break;
		case GraphicsFormat.R8_UNorm:
			num = 1;
			break;
		case GraphicsFormat.R16G16B16A16_SFloat:
			num = 8;
			break;
		case GraphicsFormat.R32G32B32A32_SFloat:
			num = 16;
			break;
		default:
			throw new ArgumentException($"Unsupported pixel format {format}.", "format");
		}
		int num2 = num;
		int num3 = RuntimeHelpers.SizeOf(array.GetType().GetElementType().TypeHandle);
		ReadOnlySpan<byte> readOnlySpan = MemoryMarshal.CreateReadOnlySpan(in Unsafe.As<byte, byte>(ref MemoryMarshal.GetArrayDataReference(array)), array.Length * num3);
		int num4 = ((rowBytes > 0) ? rowBytes : (width * num2));
		if (readOnlySpan.Length < num4 * (height - 1) + width * num2)
		{
			throw new ArgumentException("Array is too small for the given dimensions and format.", "array");
		}
		bool flag = ((format == GraphicsFormat.B8G8R8A8_SRGB || format == GraphicsFormat.B8G8R8A8_UNorm) ? true : false);
		bool flag2 = flag;
		byte[] array2 = new byte[width * height * 4];
		for (int i = 0; i < height; i++)
		{
			ReadOnlySpan<byte> readOnlySpan2 = readOnlySpan.Slice(i * num4, width * num2);
			Span<byte> span = array2.AsSpan(i * width * 4, width * 4);
			for (int j = 0; j < width; j++)
			{
				int num5 = j * 4;
				switch (num2)
				{
				case 4:
					span[num5] = readOnlySpan2[j * 4 + (flag2 ? 2 : 0)];
					span[num5 + 1] = readOnlySpan2[j * 4 + 1];
					span[num5 + 2] = readOnlySpan2[j * 4 + ((!flag2) ? 2 : 0)];
					span[num5 + 3] = readOnlySpan2[j * 4 + 3];
					break;
				case 3:
					span[num5] = readOnlySpan2[j * 3];
					span[num5 + 1] = readOnlySpan2[j * 3 + 1];
					span[num5 + 2] = readOnlySpan2[j * 3 + 2];
					span[num5 + 3] = byte.MaxValue;
					break;
				case 1:
					span[num5] = (span[num5 + 1] = (span[num5 + 2] = readOnlySpan2[j]));
					span[num5 + 3] = byte.MaxValue;
					break;
				case 8:
				{
					for (int l = 0; l < 4; l++)
					{
						span[num5 + l] = ToByte((float)BitConverter.ToHalf(readOnlySpan2.Slice(j * 8 + l * 2, 2)));
					}
					break;
				}
				default:
				{
					for (int k = 0; k < 4; k++)
					{
						span[num5 + k] = ToByte(BitConverter.ToSingle(readOnlySpan2.Slice(j * 16 + k * 4, 4)));
					}
					break;
				}
				}
			}
		}
		return array2;
		static byte ToByte(float v)
		{
			return (byte)Math.Clamp((int)MathF.Round(v * 255f), 0, 255);
		}
	}

	public static byte[] EncodeToJPG(this Texture2D tex)
	{
		return tex.EncodeToJPG(75);
	}

	public unsafe static byte[] EncodeToJPG(this Texture2D tex, int quality)
	{
		ArgumentNullException.ThrowIfNull(tex, "tex");
		MemoryStream memoryStream = new MemoryStream();
		GCHandle value = GCHandle.Alloc(memoryStream);
		try
		{
			fixed (byte* data = tex.Rgba32TopDown())
			{
				if (stbi_write_jpg_to_func((delegate* unmanaged[Cdecl]<void*, void*, int, void>)(delegate*<void*, void*, int, void>)(&AppendToStream), (void*)GCHandle.ToIntPtr(value), tex.width, tex.height, 4, data, Math.Clamp(quality, 1, 100)) == 0)
				{
					return Array.Empty<byte>();
				}
			}
		}
		finally
		{
			value.Free();
		}
		return memoryStream.ToArray();
	}

	[DllImport("stb", ExactSpelling = true)]
	[LibraryImport("stb")]
	private unsafe static extern int stbi_write_jpg_to_func(delegate* unmanaged[Cdecl]<void*, void*, int, void> func, void* context, int w, int h, int comp, void* data, int quality);

	[UnmanagedCallersOnly(CallConvs = new Type[] { typeof(CallConvCdecl) })]
	private unsafe static void AppendToStream(void* context, void* data, int size)
	{
		((MemoryStream)GCHandle.FromIntPtr((nint)context).Target).Write(new ReadOnlySpan<byte>(data, size));
	}

	private static void WriteBigEndian(Span<byte> dest, int offset, int value)
	{
		dest[offset] = (byte)(value >> 24);
		dest[offset + 1] = (byte)(value >> 16);
		dest[offset + 2] = (byte)(value >> 8);
		dest[offset + 3] = (byte)value;
	}

	private static void WriteChunk(MemoryStream stream, string type, ReadOnlySpan<byte> data)
	{
		Span<byte> span = stackalloc byte[8];
		WriteBigEndian(span, 0, data.Length);
		for (int i = 0; i < 4; i++)
		{
			span[4 + i] = (byte)type[i];
		}
		stream.Write(span);
		stream.Write(data);
		uint crc = Crc32(span.Slice(4, 4), uint.MaxValue);
		crc = Crc32(data, crc) ^ 0xFFFFFFFFu;
		Span<byte> span2 = stackalloc byte[4];
		WriteBigEndian(span2, 0, (int)crc);
		stream.Write(span2);
	}

	private static uint Crc32(ReadOnlySpan<byte> data, uint crc)
	{
		if (_crcTable == null)
		{
			uint[] array = new uint[256];
			for (uint num = 0u; num < 256; num++)
			{
				uint num2 = num;
				for (int i = 0; i < 8; i++)
				{
					num2 = (((num2 & 1) != 0) ? (0xEDB88320u ^ (num2 >> 1)) : (num2 >> 1));
				}
				array[num] = num2;
			}
			_crcTable = array;
		}
		ReadOnlySpan<byte> readOnlySpan = data;
		for (int j = 0; j < readOnlySpan.Length; j++)
		{
			byte b = readOnlySpan[j];
			crc = _crcTable[(crc ^ b) & 0xFF] ^ (crc >> 8);
		}
		return crc;
	}

	public static bool LoadImage(this Texture2D tex, byte[] data)
	{
		ArgumentNullException.ThrowIfNull(tex, "tex");
		return tex.LoadEncodedImage(data);
	}

	public static bool LoadImage(this Texture2D tex, byte[] data, bool markNonReadable)
	{
		return LoadImage(tex, data);
	}
}
