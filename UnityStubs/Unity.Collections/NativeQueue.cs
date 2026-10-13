using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;

namespace Unity.Collections;

public struct NativeQueue<T> : INativeDisposable, IDisposable, IEnumerable<T>, IEnumerable
{
	public struct Enumerator : IEnumerator<T>, IEnumerator, IDisposable
	{
		private NativeArray<T> _items;

		private int _index;

		public T Current => _items[_index];

		object IEnumerator.Current => Current;

		internal Enumerator(NativeQueue<T> queue)
		{
			_items = (queue.IsCreated ? queue.ToArray(Allocator.Temp) : default(NativeArray<T>));
			_index = -1;
		}

		public void Dispose()
		{
			if (_items.IsCreated)
			{
				_items.Dispose();
			}
		}

		public bool MoveNext()
		{
			return ++_index < _items.Length;
		}

		public void Reset()
		{
			_index = -1;
		}
	}

	public struct ParallelWriter
	{
		private unsafe NativeQueueHeader* _header;

		internal unsafe ParallelWriter(NativeQueueHeader* header)
		{
			_header = header;
		}

		public unsafe void Enqueue(T value)
		{
			if (_header == null)
			{
				throw new InvalidOperationException("NativeQueue<" + typeof(T).Name + ">.ParallelWriter is not created.");
			}
			lock (NativeQueueSync.GetOrCreate((nint)_header))
			{
				int num = _header->Count + 1;
				if (num > _header->Capacity)
				{
					int count = _header->Count;
					T[] array = new T[count];
					for (int i = 0; i < count; i++)
					{
						array[i] = UnsafeUtility.ReadArrayElement<T>(_header->Buffer, (_header->Capacity != 0) ? ((_header->Head + i) % _header->Capacity) : 0);
					}
					void* buffer = _header->Buffer;
					int capacity = _header->Capacity;
					NativeCollectionStorage.EnsureElementCapacity<T>(ref buffer, ref capacity, num, _header->Allocator, clearNewMemory: false, 0);
					_header->Buffer = buffer;
					_header->Capacity = capacity;
					for (int j = 0; j < count; j++)
					{
						UnsafeUtility.WriteArrayElement(_header->Buffer, j, array[j]);
					}
					_header->Head = 0;
					_header->Count = count;
				}
				int index = ((_header->Capacity != 0) ? ((_header->Head + _header->Count) % _header->Capacity) : 0);
				UnsafeUtility.WriteArrayElement(_header->Buffer, index, value);
				_header->Count++;
			}
		}
	}

	internal unsafe NativeQueueHeader* _header;

	public unsafe int Count
	{
		get
		{
			if (_header != null)
			{
				return _header->Count;
			}
			return 0;
		}
	}

	public unsafe bool IsCreated => _header != null;

	public unsafe NativeQueue(AllocatorManager.AllocatorHandle allocator)
	{
		NativeCollectionStorage.EnsureSupportedElementType<T>();
		_header = NativeCollectionStorage.AllocateHeader<NativeQueueHeader>(allocator.ToAllocator);
		_header->Allocator = allocator.ToAllocator;
		_header->Buffer = null;
		_header->Capacity = 0;
		_header->Head = 0;
		_header->Count = 0;
		EnsureCapacityForCount(4);
		NativeQueueSync.GetOrCreate((nint)_header);
	}

	public unsafe ParallelWriter AsParallelWriter()
	{
		EnsureCreated();
		return new ParallelWriter(_header);
	}

	public unsafe void Clear()
	{
		if (_header == null)
		{
			return;
		}
		lock (NativeQueueSync.GetOrCreate((nint)_header))
		{
			_header->Head = 0;
			_header->Count = 0;
		}
	}

	public T Dequeue()
	{
		if (!TryDequeue(out var item))
		{
			throw new InvalidOperationException("Queue is empty.");
		}
		return item;
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
			NativeQueueSync.Remove((nint)_header);
			if (_header->Buffer != null)
			{
				UnsafeUtility.Free(_header->Buffer, _header->Allocator);
			}
			Allocator allocator = _header->Allocator;
			UnsafeUtility.Free(_header, allocator);
			_header = null;
		}
	}

	public unsafe void Enqueue(T value)
	{
		EnsureCreated();
		lock (NativeQueueSync.GetOrCreate((nint)_header))
		{
			EnqueueNoLock(value);
		}
	}

	public bool IsEmpty()
	{
		return Count == 0;
	}

	public unsafe T Peek()
	{
		EnsureCreated();
		lock (NativeQueueSync.GetOrCreate((nint)_header))
		{
			if (_header->Count == 0)
			{
				throw new InvalidOperationException("Queue is empty.");
			}
			return UnsafeUtility.ReadArrayElement<T>(_header->Buffer, _header->Head);
		}
	}

	public unsafe NativeArray<T> ToArray(AllocatorManager.AllocatorHandle allocator)
	{
		EnsureCreated();
		lock (NativeQueueSync.GetOrCreate((nint)_header))
		{
			NativeArray<T> result = new NativeArray<T>(_header->Count, allocator.ToAllocator, NativeArrayOptions.UninitializedMemory);
			for (int i = 0; i < _header->Count; i++)
			{
				result[i] = UnsafeUtility.ReadArrayElement<T>(_header->Buffer, WrapIndexNoLock(_header->Head + i));
			}
			return result;
		}
	}

	public unsafe bool TryDequeue(out T item)
	{
		EnsureCreated();
		lock (NativeQueueSync.GetOrCreate((nint)_header))
		{
			if (_header->Count == 0)
			{
				item = default(T);
				return false;
			}
			item = UnsafeUtility.ReadArrayElement<T>(_header->Buffer, _header->Head);
			_header->Head = WrapIndexNoLock(_header->Head + 1);
			_header->Count--;
			if (_header->Count == 0)
			{
				_header->Head = 0;
			}
			return true;
		}
	}

	public Enumerator GetEnumerator()
	{
		return new Enumerator(this);
	}

	IEnumerator<T> IEnumerable<T>.GetEnumerator()
	{
		return GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	private void EnsureCreated()
	{
		if (!IsCreated)
		{
			throw new InvalidOperationException("NativeQueue<" + typeof(T).Name + "> is not created.");
		}
	}

	private unsafe int WrapIndexNoLock(int index)
	{
		if (_header->Capacity != 0)
		{
			return index % _header->Capacity;
		}
		return 0;
	}

	private unsafe void EnsureCapacityForCount(int requiredCount)
	{
		if (requiredCount > _header->Capacity)
		{
			int count = _header->Count;
			T[] array = new T[count];
			for (int i = 0; i < count; i++)
			{
				array[i] = UnsafeUtility.ReadArrayElement<T>(_header->Buffer, WrapIndexNoLock(_header->Head + i));
			}
			void* buffer = _header->Buffer;
			int capacity = _header->Capacity;
			NativeCollectionStorage.EnsureElementCapacity<T>(ref buffer, ref capacity, requiredCount, _header->Allocator, clearNewMemory: false, 0);
			_header->Buffer = buffer;
			_header->Capacity = capacity;
			for (int j = 0; j < count; j++)
			{
				UnsafeUtility.WriteArrayElement(_header->Buffer, j, array[j]);
			}
			_header->Head = 0;
			_header->Count = count;
		}
	}

	private unsafe void EnqueueNoLock(T value)
	{
		EnsureCapacityForCount(_header->Count + 1);
		int index = WrapIndexNoLock(_header->Head + _header->Count);
		UnsafeUtility.WriteArrayElement(_header->Buffer, index, value);
		_header->Count++;
	}
}
