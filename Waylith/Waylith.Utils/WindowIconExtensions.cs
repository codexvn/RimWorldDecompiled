using System;
using System.Buffers.Binary;
using System.IO;
using Brutal.GlfwApi;
using Brutal.StbApi;

namespace Waylith.Utils;

public static class WindowIconExtensions
{
	private ref struct IconDirectory
	{
		private const int IconDirSize = 6;

		private const int IconDirEntrySize = 16;

		private ReadOnlySpan<byte> _buffer;

		public int Count { get; }

		public readonly ReadOnlySpan<byte> this[int index]
		{
			get
			{
				if ((uint)index >= (uint)Count)
				{
					throw new IndexOutOfRangeException();
				}
				ReadOnlySpan<byte> buffer = _buffer;
				ReadOnlySpan<byte> source = buffer.Slice(6 + index * 16);
				source = source.Slice(8);
				uint length = BinaryPrimitives.ReadUInt32LittleEndian(source);
				source = source.Slice(4);
				uint start = BinaryPrimitives.ReadUInt32LittleEndian(source);
				source = source.Slice(4);
				return _buffer.Slice((int)start, (int)length);
			}
		}

		public IconDirectory(ReadOnlySpan<byte> buffer)
		{
			_buffer = buffer;
			ushort num = BinaryPrimitives.ReadUInt16LittleEndian(buffer);
			buffer = buffer.Slice(2);
			ushort num2 = BinaryPrimitives.ReadUInt16LittleEndian(buffer);
			buffer = buffer.Slice(2);
			ushort num3 = BinaryPrimitives.ReadUInt16LittleEndian(buffer);
			buffer = buffer.Slice(2);
			if (num != 0 || num2 != 1)
			{
				throw new InvalidDataException();
			}
			Count = num3;
		}
	}

	public unsafe static void SetIconFromIco(this GlfwWindow window, ReadOnlySpan<byte> ico)
	{
		ArgumentNullException.ThrowIfNull(window, "window");
		IconDirectory iconDirectory = new IconDirectory(ico);
		if (iconDirectory.Count > 16)
		{
			throw new InvalidDataException();
		}
		Span<StbImage8> span = stackalloc StbImage8[iconDirectory.Count];
		try
		{
			GlfwImage* ptr = stackalloc GlfwImage[iconDirectory.Count];
			for (int i = 0; i < iconDirectory.Count; i++)
			{
				StbImage8 stbImage = Stb.LoadFromMemory(iconDirectory[i], 4);
				span[i] = stbImage;
				ptr[i] = new GlfwImage
				{
					Width = stbImage.Width,
					Height = stbImage.Height,
					Pixels = stbImage.Data
				};
			}
			Glfw.PInvoke.SetWindowIcon(window, iconDirectory.Count, ptr);
			Glfw.PollEvents();
		}
		finally
		{
			Span<StbImage8> span2 = span;
			for (int j = 0; j < span2.Length; j++)
			{
				StbImage8 stbImage2 = span2[j];
				stbImage2.Dispose();
			}
		}
	}
}
