using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;

namespace Unity.Collections;

public struct NativeParallelHashSet<T> : INativeDisposable, IDisposable, IEnumerable<T>, IEnumerable
{
	public struct Enumerator : IEnumerator<T>, IEnumerator, IDisposable
	{
		private readonly T[] _snapshot;

		private int _index;

		public T Current => _snapshot[_index];

		object IEnumerator.Current => Current;

		internal Enumerator(T[] snapshot)
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

	public unsafe NativeParallelHashSet(int capacity, AllocatorManager.AllocatorHandle allocator)
	{
		_header = NativeParallelHashSetStorage<T>.Create(capacity, allocator.ToAllocator);
	}

	public unsafe bool Add(T item)
	{
		NativeParallelHashSetState<T> nativeParallelHashSetState = NativeParallelHashSetStorage<T>.Get(_header);
		lock (nativeParallelHashSetState.SyncRoot)
		{
			return nativeParallelHashSetState.Set.Add(item);
		}
	}

	public unsafe void Clear()
	{
		NativeParallelHashSetState<T> nativeParallelHashSetState = NativeParallelHashSetStorage<T>.Get(_header);
		lock (nativeParallelHashSetState.SyncRoot)
		{
			nativeParallelHashSetState.Set.Clear();
		}
	}

	public unsafe bool Contains(T item)
	{
		NativeParallelHashSetState<T> nativeParallelHashSetState = NativeParallelHashSetStorage<T>.Get(_header);
		lock (nativeParallelHashSetState.SyncRoot)
		{
			return nativeParallelHashSetState.Set.Contains(item);
		}
	}

	public unsafe int Count()
	{
		NativeParallelHashSetState<T> nativeParallelHashSetState = NativeParallelHashSetStorage<T>.Get(_header);
		lock (nativeParallelHashSetState.SyncRoot)
		{
			return nativeParallelHashSetState.Set.Count;
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
			NativeParallelHashSetStorage<T>.Remove(_header);
			UnsafeUtility.Free(_header, allocator);
			_header = null;
		}
	}

	public unsafe Enumerator GetEnumerator()
	{
		NativeParallelHashSetState<T> nativeParallelHashSetState = NativeParallelHashSetStorage<T>.Get(_header);
		lock (nativeParallelHashSetState.SyncRoot)
		{
			T[] array = new T[nativeParallelHashSetState.Set.Count];
			nativeParallelHashSetState.Set.CopyTo(array);
			return new Enumerator(array);
		}
	}

	IEnumerator<T> IEnumerable<T>.GetEnumerator()
	{
		return GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
