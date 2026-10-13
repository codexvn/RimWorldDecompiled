using System;
using System.Diagnostics;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;

namespace Unity.Collections;

public struct NativeReference<T> : INativeDisposable, IDisposable, IEquatable<NativeReference<T>>
{
	public struct ReadOnly
	{
		private NativeReference<T> _reference;

		public bool IsCreated => _reference.IsCreated;

		public T Value => _reference.Value;

		internal ReadOnly(NativeReference<T> reference)
		{
			_reference = reference;
		}
	}

	private unsafe void* _buffer;

	private Allocator _allocator;

	private byte _ownsBuffer;

	public unsafe bool IsCreated => _buffer != null;

	public unsafe T Value
	{
		get
		{
			return UnsafeUtility.ReadArrayElement<T>(_buffer, 0);
		}
		set
		{
			UnsafeUtility.WriteArrayElement(_buffer, 0, value);
		}
	}

	public unsafe NativeReference(AllocatorManager.AllocatorHandle allocator, NativeArrayOptions options = NativeArrayOptions.ClearMemory)
	{
		NativeCollectionStorage.EnsureSupportedElementType<T>();
		_allocator = allocator.ToAllocator;
		_ownsBuffer = 1;
		_buffer = UnsafeUtility.Malloc(UnsafeUtility.SizeOf<T>(), UnsafeUtility.AlignOf<T>(), _allocator);
		if (_buffer != null && options == NativeArrayOptions.ClearMemory)
		{
			UnsafeUtility.MemClear(_buffer, UnsafeUtility.SizeOf<T>());
		}
	}

	public NativeReference(T value, AllocatorManager.AllocatorHandle allocator)
		: this(allocator, NativeArrayOptions.UninitializedMemory)
	{
		Value = value;
	}

	public JobHandle Dispose(JobHandle inputDeps)
	{
		Dispose();
		return inputDeps;
	}

	public unsafe void Dispose()
	{
		if (_ownsBuffer != 0 && _buffer != null)
		{
			UnsafeUtility.Free(_buffer, _allocator);
		}
		_buffer = null;
		_allocator = Allocator.Invalid;
		_ownsBuffer = 0;
	}

	public unsafe bool Equals(NativeReference<T> other)
	{
		if (_buffer == other._buffer && _allocator == other._allocator)
		{
			return _ownsBuffer == other._ownsBuffer;
		}
		return false;
	}

	public ReadOnly AsReadOnly()
	{
		return new ReadOnly(this);
	}

	[Conditional("DEBUG")]
	private void EnsureCreated()
	{
		if (!IsCreated)
		{
			throw new InvalidOperationException("NativeReference<" + typeof(T).Name + "> is not created.");
		}
	}
}
