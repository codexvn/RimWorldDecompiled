namespace Unity.Collections;

internal struct NativeListHeader
{
	public unsafe void* Buffer;

	public int Length;

	public int Capacity;

	public Allocator Allocator;
}
