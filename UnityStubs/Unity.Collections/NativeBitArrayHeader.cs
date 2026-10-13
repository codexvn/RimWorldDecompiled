namespace Unity.Collections;

internal struct NativeBitArrayHeader
{
	public unsafe ulong* Buffer;

	public int Length;

	public int WordCount;

	public Allocator Allocator;
}
