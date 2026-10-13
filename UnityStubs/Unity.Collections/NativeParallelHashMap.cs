using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;

namespace Unity.Collections;

public struct NativeParallelHashMap<TKey, TValue> : INativeDisposable, IDisposable, IEnumerable<KeyValue<TKey, TValue>>, IEnumerable
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

	public unsafe bool IsCreated => _header != null;

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
		set
		{
			NativeParallelHashMapState<TKey, TValue> nativeParallelHashMapState = NativeParallelHashMapStorage<TKey, TValue>.Get(_header);
			lock (nativeParallelHashMapState.SyncRoot)
			{
				nativeParallelHashMapState.Dictionary[key] = value;
			}
		}
	}

	public unsafe NativeParallelHashMap(int capacity, AllocatorManager.AllocatorHandle allocator)
	{
		_header = NativeParallelHashMapStorage<TKey, TValue>.Create(capacity, allocator.ToAllocator);
	}

	public unsafe bool ContainsKey(TKey key)
	{
		NativeParallelHashMapState<TKey, TValue> nativeParallelHashMapState = NativeParallelHashMapStorage<TKey, TValue>.Get(_header);
		lock (nativeParallelHashMapState.SyncRoot)
		{
			return nativeParallelHashMapState.Dictionary.ContainsKey(key);
		}
	}

	public unsafe int Count()
	{
		NativeParallelHashMapState<TKey, TValue> nativeParallelHashMapState = NativeParallelHashMapStorage<TKey, TValue>.Get(_header);
		lock (nativeParallelHashMapState.SyncRoot)
		{
			return nativeParallelHashMapState.Dictionary.Count;
		}
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

	public unsafe bool TryGetValue(TKey key, out TValue item)
	{
		NativeParallelHashMapState<TKey, TValue> nativeParallelHashMapState = NativeParallelHashMapStorage<TKey, TValue>.Get(_header);
		lock (nativeParallelHashMapState.SyncRoot)
		{
			return nativeParallelHashMapState.Dictionary.TryGetValue(key, out item);
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
}
