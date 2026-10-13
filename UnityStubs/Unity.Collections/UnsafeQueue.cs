using System;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;

namespace Unity.Collections;

public struct UnsafeQueue<T> : INativeDisposable, IDisposable
{
	private unsafe NativeQueueHeader* _header;

	public unsafe bool IsCreated => _header != null;

	public unsafe UnsafeQueue(AllocatorManager.AllocatorHandle allocator)
	{
		NativeCollectionStorage.EnsureSupportedElementType<T>();
		_header = NativeCollectionStorage.AllocateHeader<NativeQueueHeader>(allocator.ToAllocator);
		_header->Allocator = allocator.ToAllocator;
		_header->Buffer = null;
		_header->Capacity = 0;
		_header->Head = 0;
		_header->Count = 0;
		EnsureCapacityForCount(4);
	}

	public unsafe void Clear()
	{
		if (_header != null)
		{
			_header->Head = 0;
			_header->Count = 0;
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

	public unsafe void Enqueue(T value)
	{
		EnsureCreated();
		EnsureCapacityForCount(_header->Count + 1);
		int index = WrapIndex(_header->Head + _header->Count);
		UnsafeUtility.WriteArrayElement(_header->Buffer, index, value);
		_header->Count++;
	}

	public unsafe bool TryDequeue(out T item)
	{
		EnsureCreated();
		if (_header->Count == 0)
		{
			item = default(T);
			return false;
		}
		item = UnsafeUtility.ReadArrayElement<T>(_header->Buffer, _header->Head);
		_header->Head = WrapIndex(_header->Head + 1);
		_header->Count--;
		if (_header->Count == 0)
		{
			_header->Head = 0;
		}
		return true;
	}

	private unsafe void EnsureCreated()
	{
		if (_header == null)
		{
			throw new InvalidOperationException("UnsafeQueue<" + typeof(T).Name + "> is not created.");
		}
	}

	private unsafe int WrapIndex(int index)
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
			T[] array = ((count > 0) ? new T[count] : Array.Empty<T>());
			for (int i = 0; i < count; i++)
			{
				array[i] = UnsafeUtility.ReadArrayElement<T>(_header->Buffer, WrapIndex(_header->Head + i));
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
}
