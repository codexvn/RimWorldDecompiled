using System;
using System.Collections.Concurrent;

namespace Unity.Collections;

internal static class NativeParallelMultiHashMapStorage<TKey, TValue>
{
	private static readonly ConcurrentDictionary<nint, NativeParallelMultiHashMapState<TKey, TValue>> States = new ConcurrentDictionary<nint, NativeParallelMultiHashMapState<TKey, TValue>>();

	public unsafe static NativeMapHeader* Create(int capacity, Allocator allocator)
	{
		NativeCollectionStorage.EnsureSupportedElementType<TKey>();
		NativeCollectionStorage.EnsureSupportedElementType<TValue>();
		NativeMapHeader* ptr = NativeCollectionStorage.AllocateHeader<NativeMapHeader>(allocator);
		ptr->Allocator = allocator;
		States[(nint)ptr] = new NativeParallelMultiHashMapState<TKey, TValue>(capacity);
		return ptr;
	}

	public unsafe static NativeParallelMultiHashMapState<TKey, TValue> Get(NativeMapHeader* header)
	{
		if (header == null)
		{
			throw new InvalidOperationException($"NativeParallelMultiHashMap<{typeof(TKey).Name}, {typeof(TValue).Name}> is not created.");
		}
		if (!States.TryGetValue((nint)header, out var value))
		{
			throw new InvalidOperationException($"NativeParallelMultiHashMap<{typeof(TKey).Name}, {typeof(TValue).Name}> state is missing.");
		}
		return value;
	}

	public static NativeParallelMultiHashMapState<TKey, TValue> Get(nint header)
	{
		if (header == 0)
		{
			throw new InvalidOperationException($"NativeParallelMultiHashMap<{typeof(TKey).Name}, {typeof(TValue).Name}> is not created.");
		}
		if (!States.TryGetValue(header, out var value))
		{
			throw new InvalidOperationException($"NativeParallelMultiHashMap<{typeof(TKey).Name}, {typeof(TValue).Name}> state is missing.");
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
