using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;

namespace Unity.Collections.LowLevel.Unsafe;

public struct UnsafeList<T> : INativeDisposable, IDisposable, INativeList<T>, IIndexable<T>, IEnumerable<T>, IEnumerable where T : unmanaged
{
	public struct Enumerator : IEnumerator<T>, IEnumerator, IDisposable
	{
		private UnsafeList<T> _list;

		private int _index;

		public T Current => _list[_index];

		object IEnumerator.Current => Current;

		internal Enumerator(UnsafeList<T> list)
		{
			_list = list;
			_index = -1;
		}

		public void Dispose()
		{
		}

		public bool MoveNext()
		{
			return ++_index < _list.Length;
		}

		public void Reset()
		{
			_index = -1;
		}
	}

	private unsafe void* _buffer;

	private int _length;

	private int _capacity;

	private Allocator _allocator;

	public const int MaxCapacity = int.MaxValue;

	public unsafe T* Ptr;

	public unsafe bool IsCreated => _buffer != null;

	public int Capacity
	{
		get
		{
			return _capacity;
		}
		set
		{
			if (value < _length)
			{
				throw new ArgumentOutOfRangeException("value", "Capacity cannot be smaller than Length.");
			}
			EnsureCapacity(value, clearNewMemory: false, _length);
		}
	}

	public bool IsEmpty => _length == 0;

	public unsafe T this[int index]
	{
		get
		{
			ValidateIndex(index);
			return UnsafeUtility.ReadArrayElement<T>(_buffer, index);
		}
		set
		{
			ValidateIndex(index);
			UnsafeUtility.WriteArrayElement(_buffer, index, value);
		}
	}

	public int Length
	{
		get
		{
			return _length;
		}
		set
		{
			Resize(value);
		}
	}

	public unsafe void RemoveAt(int index)
	{
		if ((uint)index >= (uint)_length)
		{
			throw new ArgumentOutOfRangeException("index");
		}
		int num = UnsafeUtility.SizeOf<T>();
		byte* ptr = (byte*)_buffer + (long)index * (long)num;
		UnsafeUtility.MemCpy(ptr, ptr + num, (long)(_length - index - 1) * (long)num);
		_length--;
	}

	public unsafe UnsafeList(int initialCapacity, AllocatorManager.AllocatorHandle allocator, NativeArrayOptions options = NativeArrayOptions.UninitializedMemory)
	{
		NativeCollectionStorage.EnsureSupportedElementType<T>();
		_buffer = null;
		_length = 0;
		_capacity = 0;
		_allocator = allocator.ToAllocator;
		Ptr = null;
		if (initialCapacity > 0)
		{
			EnsureCapacity(initialCapacity, options == NativeArrayOptions.ClearMemory, 0);
		}
	}

	public unsafe void Add(in T value)
	{
		EnsureCapacity(_length + 1, clearNewMemory: false, _length);
		UnsafeUtility.WriteArrayElement(_buffer, _length, value);
		_length++;
	}

	public void Clear()
	{
		_length = 0;
	}

	public JobHandle Dispose(JobHandle inputDeps)
	{
		Dispose();
		return inputDeps;
	}

	public unsafe void Dispose()
	{
		if (_buffer != null)
		{
			UnsafeUtility.Free(_buffer, _allocator);
		}
		_buffer = null;
		_length = 0;
		_capacity = 0;
		_allocator = Allocator.Invalid;
		Ptr = null;
	}

	public T ElementAt(int index)
	{
		return this[index];
	}

	public unsafe void Resize(int length, NativeArrayOptions options = NativeArrayOptions.UninitializedMemory)
	{
		ArgumentOutOfRangeException.ThrowIfNegative(length, "length");
		int length2 = _length;
		if (length > length2)
		{
			EnsureCapacity(length, options == NativeArrayOptions.ClearMemory, length2);
			if (options == NativeArrayOptions.ClearMemory)
			{
				UnsafeUtility.MemClear((byte*)_buffer + (long)length2 * (long)UnsafeUtility.SizeOf<T>(), (long)(length - length2) * (long)UnsafeUtility.SizeOf<T>());
			}
		}
		_length = length;
	}

	IEnumerator<T> IEnumerable<T>.GetEnumerator()
	{
		return new Enumerator(this);
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return new Enumerator(this);
	}

	private unsafe void EnsureCapacity(int requiredCapacity, bool clearNewMemory, int preserveCount)
	{
		if (requiredCapacity > _capacity)
		{
			void* buffer = _buffer;
			int capacity = _capacity;
			NativeCollectionStorage.EnsureElementCapacity<T>(ref buffer, ref capacity, requiredCapacity, _allocator, clearNewMemory, preserveCount);
			_buffer = buffer;
			_capacity = capacity;
			Ptr = (T*)_buffer;
		}
	}

	private void ValidateIndex(int index)
	{
		if ((uint)index >= (uint)_length)
		{
			throw new IndexOutOfRangeException();
		}
	}
}
