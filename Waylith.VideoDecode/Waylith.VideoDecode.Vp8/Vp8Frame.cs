using System;
using System.Runtime.InteropServices;

namespace Waylith.VideoDecode.Vp8;

internal sealed class Vp8Frame : IDisposable
{
	public const int Border = 32;

	public readonly int Width;

	public readonly int Height;

	public readonly int YStride;

	public readonly int UvStride;

	public unsafe readonly byte* Y;

	public unsafe readonly byte* U;

	public unsafe readonly byte* V;

	public int References;

	private unsafe void* memory;

	public unsafe Vp8Frame(int alignedWidth, int alignedHeight)
	{
		Width = alignedWidth;
		Height = alignedHeight;
		YStride = (alignedWidth + 64 + 31) & -32;
		UvStride = YStride >> 1;
		nuint num = (nuint)((ulong)(alignedHeight + 64) * (ulong)YStride);
		nuint num2 = (nuint)((ulong)(alignedHeight / 2 + 32) * (ulong)UvStride);
		memory = NativeMemory.AllocZeroed(num + 2 * num2);
		byte* ptr = (byte*)memory;
		Y = ptr + 32 * YStride + 32;
		U = ptr + num + 16 * UvStride + 16;
		V = ptr + num + num2 + 16 * UvStride + 16;
	}

	public unsafe void SetIntraTopLine()
	{
		new Span<byte>(Y - 1 - YStride, Width + 5).Fill(127);
		new Span<byte>(U - 1 - UvStride, Width / 2 + 5).Fill(127);
		new Span<byte>(V - 1 - UvStride, Width / 2 + 5).Fill(127);
	}

	public unsafe void ExtendRowSides(int macroblockRow)
	{
		ExtendSides(Y + macroblockRow * 16 * YStride, YStride, Width, 16, 32);
		ExtendSides(U + macroblockRow * 8 * UvStride, UvStride, Width / 2, 8, 16);
		ExtendSides(V + macroblockRow * 8 * UvStride, UvStride, Width / 2, 8, 16);
	}

	public unsafe void ExtendTopAndBottom()
	{
		ExtendVertically(Y, YStride, Height, 32);
		ExtendVertically(U, UvStride, Height / 2, 16);
		ExtendVertically(V, UvStride, Height / 2, 16);
	}

	private unsafe static void ExtendSides(byte* row, int stride, int width, int rows, int border)
	{
		for (int i = 0; i < rows; i++)
		{
			new Span<byte>(row - border, border).Fill(*row);
			new Span<byte>(row + width, border).Fill(row[width - 1]);
			row += stride;
		}
	}

	private unsafe static void ExtendVertically(byte* plane, int stride, int height, int border)
	{
		ReadOnlySpan<byte> readOnlySpan = new ReadOnlySpan<byte>(plane - border, stride);
		ReadOnlySpan<byte> readOnlySpan2 = new ReadOnlySpan<byte>(plane - border + (height - 1) * stride, stride);
		for (int i = 1; i <= border; i++)
		{
			readOnlySpan.CopyTo(new Span<byte>(plane - border - i * stride, stride));
			readOnlySpan2.CopyTo(new Span<byte>(plane - border + (height - 1 + i) * stride, stride));
		}
	}

	public unsafe void Dispose()
	{
		NativeMemory.Free(memory);
		memory = null;
	}
}
