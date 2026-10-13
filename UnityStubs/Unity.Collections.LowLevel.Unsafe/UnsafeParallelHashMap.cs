using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;

namespace Unity.Collections.LowLevel.Unsafe;

public struct UnsafeParallelHashMap<TKey, TValue> : INativeDisposable, IDisposable, IEnumerable<KeyValue<TKey, TValue>>, IEnumerable
{
	public struct Enumerator : IEnumerator<KeyValue<TKey, TValue>>, IEnumerator, IDisposable
	{
		private readonly KeyValue<TKey, TValue>[] _snapshot;

		private int _index;

		public KeyValue<TKey, TValue> Current => _snapshot[_index];

		object IEnumerator.Current => Current;

		internal Enumerator(KeyValue<TKey, TValue>[] snapshot)
		{
			_snapshot = snapshot;
			_index = -1;
		}

		public void Dispose()
		{
		}

		public bool MoveNext()
		{
			return ++_index < _snapshot.Length;
		}

		public void Reset()
		{
			_index = -1;
		}
	}

	internal unsafe NativeMapHeader* _header;

	public const int MaxCapacity = 1073741823;

	public unsafe TValue this[TKey key]
	{
		get
		{
			NativeParallelHashMapState<TKey, TValue> nativeParallelHashMapState = NativeParallelHashMapStorage<TKey, TValue>.Get(_header);
			lock (nativeParallelHashMapState.SyncRoot)
			{
				return nativeParallelHashMapState.Dictionary[key];
			}
		}
	}

	public unsafe UnsafeParallelHashMap(int capacity, AllocatorManager.AllocatorHandle allocator)
	{
		_header = NativeParallelHashMapStorage<TKey, TValue>.Create(capacity, allocator.ToAllocator);
	}

	public JobHandle Dispose(JobHandle inputDeps)
	{
		Dispose();
		return inputDeps;
	}

	public unsafe void Dispose()
	{
		if (_header != null)
		{
			Allocator allocator = _header->Allocator;
			NativeParallelHashMapStorage<TKey, TValue>.Remove(_header);
			UnsafeUtility.Free(_header, allocator);
			_header = null;
		}
	}

	public unsafe bool TryAdd(TKey key, TValue item)
	{
		NativeParallelHashMapState<TKey, TValue> nativeParallelHashMapState = NativeParallelHashMapStorage<TKey, TValue>.Get(_header);
		lock (nativeParallelHashMapState.SyncRoot)
		{
			if (nativeParallelHashMapState.Dictionary.ContainsKey(key))
			{
				return false;
			}
			nativeParallelHashMapState.Dictionary.Add(key, item);
			return true;
		}
	}

	IEnumerator<KeyValue<TKey, TValue>> IEnumerable<KeyValue<TKey, TValue>>.GetEnumerator()
	{
		return GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	public unsafe Enumerator GetEnumerator()
	{
		NativeParallelHashMapState<TKey, TValue> nativeParallelHashMapState = NativeParallelHashMapStorage<TKey, TValue>.Get(_header);
		lock (nativeParallelHashMapState.SyncRoot)
		{
			KeyValue<TKey, TValue>[] array = new KeyValue<TKey, TValue>[nativeParallelHashMapState.Dictionary.Count];
			int num = 0;
			foreach (KeyValuePair<TKey, TValue> item in nativeParallelHashMapState.Dictionary)
			{
				array[num++] = new KeyValue<TKey, TValue>(item.Key, item.Value);
			}
			return new Enumerator(array);
		}
	}
}
