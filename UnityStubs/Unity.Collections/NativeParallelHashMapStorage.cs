using System;
using System.Collections.Concurrent;

namespace Unity.Collections;

internal static class NativeParallelHashMapStorage<TKey, TValue>
{
	private static readonly ConcurrentDictionary<nint, NativeParallelHashMapState<TKey, TValue>> States = new ConcurrentDictionary<nint, NativeParallelHashMapState<TKey, TValue>>();

	public unsafe static NativeMapHeader* Create(int capacity, Allocator allocator)
	{
		NativeCollectionStorage.EnsureSupportedElementType<TKey>();
		NativeCollectionStorage.EnsureSupportedElementType<TValue>();
		NativeMapHeader* ptr = NativeCollectionStorage.AllocateHeader<NativeMapHeader>(allocator);
		ptr->Allocator = allocator;
		States[(nint)ptr] = new NativeParallelHashMapState<TKey, TValue>(capacity);
		return ptr;
	}

	public unsafe static NativeParallelHashMapState<TKey, TValue> Get(NativeMapHeader* header)
	{
		if (header == null)
		{
			throw new InvalidOperationException($"NativeParallelHashMap<{typeof(TKey).Name}, {typeof(TValue).Name}> is not created.");
		}
		if (!States.TryGetValue((nint)header, out var value))
		{
			throw new InvalidOperationException($"NativeParallelHashMap<{typeof(TKey).Name}, {typeof(TValue).Name}> state is missing.");
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
