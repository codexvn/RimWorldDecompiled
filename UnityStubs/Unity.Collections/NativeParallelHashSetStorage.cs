using System;
using System.Collections.Concurrent;

namespace Unity.Collections;

internal static class NativeParallelHashSetStorage<T>
{
	private static readonly ConcurrentDictionary<nint, NativeParallelHashSetState<T>> States = new ConcurrentDictionary<nint, NativeParallelHashSetState<T>>();

	public unsafe static NativeMapHeader* Create(int capacity, Allocator allocator)
	{
		NativeCollectionStorage.EnsureSupportedElementType<T>();
		NativeMapHeader* ptr = NativeCollectionStorage.AllocateHeader<NativeMapHeader>(allocator);
		ptr->Allocator = allocator;
		States[(nint)ptr] = new NativeParallelHashSetState<T>(capacity);
		return ptr;
	}

	public unsafe static NativeParallelHashSetState<T> Get(NativeMapHeader* header)
	{
		if (header == null)
		{
			throw new InvalidOperationException("NativeParallelHashSet<" + typeof(T).Name + "> is not created.");
		}
		if (!States.TryGetValue((nint)header, out var value))
		{
			throw new InvalidOperationException("NativeParallelHashSet<" + typeof(T).Name + "> state is missing.");
		}
		return value;
	}

	public unsafe static void Remove(NativeMapHeader* header)
	{
		if (header != null)
		{
			States.TryRemove((nint)header, out var _);
		}
	}
}
