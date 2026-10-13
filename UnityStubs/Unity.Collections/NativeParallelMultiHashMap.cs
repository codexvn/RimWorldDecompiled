using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;

namespace Unity.Collections;

public struct NativeParallelMultiHashMap<TKey, TValue> : INativeDisposable, IDisposable, IEnumerable<KeyValue<TKey, TValue>>, IEnumerable
{
	public struct Enumerator : IEnumerator<TValue>, IEnumerator, IDisposable
	{
		private readonly TValue[] _snapshot;

		private int _index;

		public TValue Current => _snapshot[_index];

		object IEnumerator.Current => Current;

		internal Enumerator(TValue[] snapshot)
		{
			_snapshot = snapshot;
			_index = -1;
		}

		public void Dispose()
		{
		}

		public Enumerator GetEnumerator()
		{
			return this;
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

	public struct KeyValueEnumerator : IEnumerator<KeyValue<TKey, TValue>>, IEnumerator, IDisposable
	{
		private readonly KeyValue<TKey, TValue>[] _snapshot;

		private int _index;

		public KeyValue<TKey, TValue> Current => _snapshot[_index];

		object IEnumerator.Current => Current;

		internal KeyValueEnumerator(KeyValue<TKey, TValue>[] snapshot)
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

	public unsafe NativeParallelMultiHashMap(int capacity, AllocatorManager.AllocatorHandle allocator)
	{
		_header = NativeParallelMultiHashMapStorage<TKey, TValue>.Create(capacity, allocator.ToAllocator);
	}

	public unsafe void Add(TKey key, TValue item)
	{
		NativeParallelMultiHashMapState<TKey, TValue> nativeParallelMultiHashMapState = NativeParallelMultiHashMapStorage<TKey, TValue>.Get(_header);
		lock (nativeParallelMultiHashMapState.SyncRoot)
		{
			if (!nativeParallelMultiHashMapState.Dictionary.TryGetValue(key, out var value))
			{
				value = new List<TValue>();
				nativeParallelMultiHashMapState.Dictionary[key] = value;
			}
			value.Add(item);
			nativeParallelMultiHashMapState.TotalCount++;
		}
	}

	public unsafe int Count()
	{
		NativeParallelMultiHashMapState<TKey, TValue> nativeParallelMultiHashMapState = NativeParallelMultiHashMapStorage<TKey, TValue>.Get(_header);
		lock (nativeParallelMultiHashMapState.SyncRoot)
		{
			return nativeParallelMultiHashMapState.TotalCount;
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
			NativeParallelMultiHashMapStorage<TKey, TValue>.Remove(_header);
			UnsafeUtility.Free(_header, allocator);
			_header = null;
		}
	}

	public unsafe KeyValueEnumerator GetEnumerator()
	{
		NativeParallelMultiHashMapState<TKey, TValue> nativeParallelMultiHashMapState = NativeParallelMultiHashMapStorage<TKey, TValue>.Get(_header);
		lock (nativeParallelMultiHashMapState.SyncRoot)
		{
			KeyValue<TKey, TValue>[] array = new KeyValue<TKey, TValue>[nativeParallelMultiHashMapState.TotalCount];
			int num = 0;
			foreach (KeyValuePair<TKey, List<TValue>> item in nativeParallelMultiHashMapState.Dictionary)
			{
				foreach (TValue item2 in item.Value)
				{
					array[num++] = new KeyValue<TKey, TValue>(item.Key, item2);
				}
			}
			return new KeyValueEnumerator(array);
		}
	}

	public unsafe Enumerator GetValuesForKey(TKey key)
	{
		NativeParallelMultiHashMapState<TKey, TValue> nativeParallelMultiHashMapState = NativeParallelMultiHashMapStorage<TKey, TValue>.Get(_header);
		lock (nativeParallelMultiHashMapState.SyncRoot)
		{
			if (!nativeParallelMultiHashMapState.Dictionary.TryGetValue(key, out var value) || value.Count == 0)
			{
				return new Enumerator(Array.Empty<TValue>());
			}
			return new Enumerator(value.ToArray());
		}
	}

	public unsafe bool TryGetFirstValue(TKey key, out TValue item, out NativeParallelMultiHashMapIterator<TKey> it)
	{
		NativeParallelMultiHashMapState<TKey, TValue> nativeParallelMultiHashMapState = NativeParallelMultiHashMapStorage<TKey, TValue>.Get(_header);
		lock (nativeParallelMultiHashMapState.SyncRoot)
		{
			if (nativeParallelMultiHashMapState.Dictionary.TryGetValue(key, out var value) && value.Count != 0)
			{
				item = value[0];
				it = new NativeParallelMultiHashMapIterator<TKey>
				{
					Header = (nint)_header,
					Key = key,
					Index = 0
				};
				return true;
			}
		}
		item = default(TValue);
		it = default(NativeParallelMultiHashMapIterator<TKey>);
		return false;
	}

	public bool TryGetNextValue(out TValue item, ref NativeParallelMultiHashMapIterator<TKey> it)
	{
		NativeParallelMultiHashMapState<TKey, TValue> nativeParallelMultiHashMapState = NativeParallelMultiHashMapStorage<TKey, TValue>.Get(it.Header);
		lock (nativeParallelMultiHashMapState.SyncRoot)
		{
			if (nativeParallelMultiHashMapState.Dictionary.TryGetValue(it.Key, out var value))
			{
				int num = it.Index + 1;
				if ((uint)num < (uint)value.Count)
				{
					it.Index = num;
					item = value[num];
					return true;
				}
			}
		}
		item = default(TValue);
		return false;
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
