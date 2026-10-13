namespace Unity.Collections;

internal struct NativeQueueHeader
{
	public unsafe void* Buffer;

	public int Capacity;

	public int Head;

	public int Count;

	public Allocator Allocator;
}
