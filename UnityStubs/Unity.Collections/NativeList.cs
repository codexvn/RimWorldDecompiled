using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;

namespace Unity.Collections;

public struct NativeList<T> : INativeDisposable, IDisposable, INativeList<T>, IIndexable<T>, IEnumerable<T>, IEnumerable where T : unmanaged
{
	internal unsafe NativeListHeader* _header;

	public const int MaxCapacity = int.MaxValue;

	public unsafe int Capacity
	{
		get
		{
			if (_header != null)
			{
				return _header->Capacity;
			}
			return 0;
		}
		set
		{
			SetCapacity(value);
		}
	}

	public unsafe bool IsCreated => _header != null;

	public bool IsEmpty => Length == 0;

	public int Count => Length;

	public unsafe T this[int index]
	{
		get
		{
			return UnsafeUtility.ReadArrayElement<T>(_header->Buffer, index);
		}
		set
		{
			UnsafeUtility.WriteArrayElement(_header->Buffer, index, value);
		}
	}

	public unsafe int Length
	{
		get
		{
			if (_header != null)
			{
				return _header->Length;
			}
			return 0;
		}
		set
		{
			Resize(value, NativeArrayOptions.UninitializedMemory);
		}
	}

	public NativeList(AllocatorManager.AllocatorHandle allocator)
		: this(0, allocator)
	{
	}

	public unsafe NativeList(int initialCapacity, AllocatorManager.AllocatorHandle allocator)
	{
		NativeCollectionStorage.EnsureSupportedElementType<T>();
		_header = NativeCollectionStorage.AllocateHeader<NativeListHeader>(allocator.ToAllocator);
		_header->Allocator = allocator.ToAllocator;
		_header->Buffer = null;
		_header->Length = 0;
		_header->Capacity = 0;
		if (initialCapacity > 0)
		{
			void* buffer = null;
			int capacity = 0;
			NativeCollectionStorage.EnsureElementCapacity<T>(ref buffer, ref capacity, initialCapacity, allocator.ToAllocator, clearNewMemory: false, 0);
			_header->Buffer = buffer;
			_header->Capacity = capacity;
		}
	}

	public unsafe void Add(in T value)
	{
		EnsureStateForMutation();
		EnsureCapacityForLength(_header->Length + 1);
		UnsafeUtility.WriteArrayElement(_header->Buffer, _header->Length, value);
		_header->Length++;
	}

	public unsafe void AddNoResize(T value)
	{
		EnsureStateForMutation();
		if (_header->Length >= Capacity)
		{
			throw new InvalidOperationException("NativeList capacity exceeded.");
		}
		UnsafeUtility.WriteArrayElement(_header->Buffer, _header->Length, value);
		_header->Length++;
	}

	public unsafe void AddRange(NativeArray<T> array)
	{
		if (array.Length != 0)
		{
			EnsureStateForMutation();
			int length = _header->Length;
			ResizeUninitialized(length + array.Length);
			NativeArray<T>.Copy(array, 0, AsArray(), length, array.Length);
		}
	}

	public unsafe void AddRange(NativeArray<T>.ReadOnly array)
	{
		if (array.Length != 0)
		{
			EnsureStateForMutation();
			int length = _header->Length;
			ResizeUninitialized(length + array.Length);
			NativeArray<T>.Copy(array, 0, AsArray(), length, array.Length);
		}
	}

	public void AddRange(NativeList<T> list)
	{
		AddRange(list.AsArray());
	}

	public unsafe NativeArray<T> AsArray()
	{
		return new NativeArray<T>((_header == null) ? null : _header->Buffer, (_header != null) ? _header->Length : 0, (_header != null) ? _header->Allocator : Allocator.Invalid, ownsBuffer: false);
	}

	public NativeArray<T>.ReadOnly AsReadOnly()
	{
		return AsArray().AsReadOnly();
	}

	public unsafe void Clear()
	{
		if (_header != null)
		{
			_header->Length = 0;
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
			if (_header->Buffer != null)
			{
				UnsafeUtility.Free(_header->Buffer, _header->Allocator);
			}
			Allocator allocator = _header->Allocator;
			UnsafeUtility.Free(_header, allocator);
			_header = null;
		}
	}

	public T ElementAt(int index)
	{
		return this[index];
	}

	public NativeArray<T>.Enumerator GetEnumerator()
	{
		return AsArray().GetEnumerator();
	}

	public unsafe void InsertRange(int index, int count)
	{
		ArgumentOutOfRangeException.ThrowIfNegative(count, "count");
		if (index < 0 || index > Length)
		{
			throw new ArgumentOutOfRangeException("index");
		}
		if (count != 0)
		{
			EnsureStateForMutation();
			int length = _header->Length;
			ResizeUninitialized(length + count);
			for (int num = length - 1; num >= index; num--)
			{
				this[num + count] = this[num];
			}
		}
	}

	public unsafe void RemoveAt(int index)
	{
		for (int i = index; i < Length - 1; i++)
		{
			this[i] = this[i + 1];
		}
		_header->Length--;
	}

	public unsafe void RemoveAtSwapBack(int index)
	{
		int num = Length - 1;
		if (index != num)
		{
			this[index] = this[num];
		}
		_header->Length--;
	}

	public unsafe void Resize(int length, NativeArrayOptions options)
	{
		ArgumentOutOfRangeException.ThrowIfNegative(length, "length");
		EnsureStateForMutation();
		int length2 = _header->Length;
		if (length > length2)
		{
			EnsureCapacityForLength(length, options);
			if (options == NativeArrayOptions.ClearMemory)
			{
				UnsafeUtility.MemClear((byte*)_header->Buffer + (long)length2 * (long)UnsafeUtility.SizeOf<T>(), (long)(length - length2) * (long)UnsafeUtility.SizeOf<T>());
			}
		}
		_header->Length = length;
	}

	public void ResizeUninitialized(int length)
	{
		Resize(length, NativeArrayOptions.UninitializedMemory);
	}

	public unsafe void SetCapacity(int capacity)
	{
		ArgumentOutOfRangeException.ThrowIfNegative(capacity, "capacity");
		EnsureStateForMutation();
		if (capacity < Length)
		{
			throw new ArgumentOutOfRangeException("capacity", "Capacity cannot be smaller than Length.");
		}
		void* buffer = _header->Buffer;
		int capacity2 = _header->Capacity;
		NativeCollectionStorage.EnsureElementCapacity<T>(ref buffer, ref capacity2, capacity, _header->Allocator, clearNewMemory: false, _header->Length);
		_header->Buffer = buffer;
		_header->Capacity = capacity2;
	}

	IEnumerator<T> IEnumerable<T>.GetEnumerator()
	{
		return GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	internal unsafe void* GetUnsafePtrInternal()
	{
		return _header->Buffer;
	}

	[Conditional("DEBUG")]
	private void EnsureStateForRead()
	{
		if (!IsCreated)
		{
			throw new InvalidOperationException("NativeList<" + typeof(T).Name + "> is not created.");
		}
	}

	private unsafe void EnsureStateForMutation()
	{
		if (_header == null)
		{
			this = new NativeList<T>(4, Allocator.Persistent);
		}
	}

	private unsafe void EnsureCapacityForLength(int requiredLength, NativeArrayOptions options = NativeArrayOptions.UninitializedMemory)
	{
		void* buffer = _header->Buffer;
		int capacity = _header->Capacity;
		NativeCollectionStorage.EnsureElementCapacity<T>(ref buffer, ref capacity, requiredLength, _header->Allocator, options == NativeArrayOptions.ClearMemory, _header->Length);
		_header->Buffer = buffer;
		_header->Capacity = capacity;
	}

	[Conditional("DEBUG")]
	private void ValidateIndex(int index)
	{
		if ((uint)index >= (uint)Length)
		{
			throw new IndexOutOfRangeException($"Index {index} is out of range for length {Length}");
		}
	}
}
