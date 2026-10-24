using System;
using System.Runtime.InteropServices;

namespace Waylith.VideoDecode.Vp8;

internal static class Vp8Pointers
{
	public unsafe static readonly byte* CoefBands = Copy(Vp8Tables.CoefBands);

	public unsafe static readonly byte* ZigZag = Copy(Vp8Tables.ZigZag);

	public unsafe static readonly byte** Categories = CopyCategories();

	public unsafe static readonly short* SixTapFilters = Copy(Vp8Tables.SixTapFilters);

	public unsafe static readonly sbyte* BModeTree = Copy(Vp8Tables.BModeTree);

	public unsafe static readonly sbyte* YModeTree = Copy(Vp8Tables.YModeTree);

	public unsafe static readonly sbyte* KfYModeTree = Copy(Vp8Tables.KfYModeTree);

	public unsafe static readonly sbyte* UvModeTree = Copy(Vp8Tables.UvModeTree);

	public unsafe static readonly sbyte* SmallMvTree = Copy(Vp8Tables.SmallMvTree);

	public unsafe static readonly byte* KfYModeProbs = Copy(Vp8Tables.KfYModeProbs);

	public unsafe static readonly byte* KfUvModeProbs = Copy(Vp8Tables.KfUvModeProbs);

	public unsafe static readonly byte* KfBModeProbs = Copy(Vp8Tables.KfBModeProbs);

	public unsafe static readonly byte* ModeContexts = Copy(Vp8Tables.ModeContexts);

	public unsafe static readonly byte* SubMvRefProbs = Copy(Vp8Tables.SubMvRefProbs);

	private unsafe static T* Copy<T>(ReadOnlySpan<T> table) where T : unmanaged
	{
		T* ptr = (T*)NativeMemory.Alloc((nuint)(table.Length * sizeof(T)));
		table.CopyTo(new Span<T>(ptr, table.Length));
		return ptr;
	}

	private unsafe static byte** CopyCategories()
	{
		byte** ptr = (byte**)NativeMemory.Alloc((nuint)(4 * sizeof(byte*)));
		*ptr = Copy(Vp8Tables.Cat3);
		ptr[1] = Copy(Vp8Tables.Cat4);
		ptr[2] = Copy(Vp8Tables.Cat5);
		ptr[3] = Copy(Vp8Tables.Cat6);
		return ptr;
	}
}
