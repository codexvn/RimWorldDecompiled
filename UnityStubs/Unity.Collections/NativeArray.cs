using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;

namespace Unity.Collections;

public struct NativeArray<T> : IDisposable, IEnumerable<T>, IEnumerable, IEquatable<NativeArray<T>>
{
	public struct Enumerator : IEnumerator<T>, IEnumerator, IDisposable
	{
		private NativeArray<T> _array;

		private int _index;

		public T Current => _array[_index];

		object IEnumerator.Current => Current;

		internal Enumerator(NativeArray<T> array)
		{
			_array = array;
			_index = -1;
		}

		public void Dispose()
		{
		}

		public bool MoveNext()
		{
			return ++_index < _array.Length;
		}

		public void Reset()
		{
			_index = -1;
		}
	}

	public struct ReadOnly : IEnumerable<T>, IEnumerable
	{
		public struct Enumerator : IEnumerator<T>, IEnumerator, IDisposable
		{
			private NativeArray<T>.Enumerator _inner;

			public T Current => _inner.Current;

			object IEnumerator.Current => Current;

			internal Enumerator(NativeArray<T> array)
			{
				_inner = new NativeArray<T>.Enumerator(array);
			}

			public void Dispose()
			{
			}

			public bool MoveNext()
			{
				return _inner.MoveNext();
			}

			public void Reset()
			{
				_inner.Reset();
			}
		}

		private NativeArray<T> _array;

		public T this[int index] => _array[index];

		public int Length => _array.Length;

		public bool IsCreated => _array.IsCreated;

		internal ReadOnly(NativeArray<T> array)
		{
			_array = array;
		}

		public Enumerator GetEnumerator()
		{
			return new Enumerator(_array);
		}

		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public T UnsafeElementAt(int index)
		{
			return _array[index];
		}

		internal unsafe void* GetUnsafeReadOnlyPtrInternal()
		{
			return _array.GetUnsafeReadOnlyPtrInternal();
		}

		[Conditional("DEBUG")]
		internal void ValidateRange(int start, int length)
		{
		}
	}

	internal unsafe void* _buffer;

	internal int _length;

	internal Allocator _allocator;

	internal byte _ownsBuffer;

	public unsafe bool IsCreated
	{
		get
		{
			if (_buffer == null)
			{
				if (_length == 0)
				{
					return _ownsBuffer != 0;
				}
				return false;
			}
			return true;
		}
	}

	public unsafe T this[int index]
	{
		get
		{
			return UnsafeUtility.ReadArrayElement<T>((byte*)GetUnsafeReadOnlyPtrInternal() + (long)index * (long)UnsafeUtility.SizeOf<T>(), 0);
		}
		set
		{
			UnsafeUtility.WriteArrayElement((byte*)GetUnsafePtrInternal() + (long)index * (long)UnsafeUtility.SizeOf<T>(), 0, value);
		}
	}

	public int Length => _length;

	public unsafe NativeArray(int length, Allocator allocator, NativeArrayOptions options = NativeArrayOptions.ClearMemory)
	{
		NativeCollectionStorage.EnsureSupportedElementType<T>();
		_length = Math.Max(0, length);
		_allocator = allocator;
		_ownsBuffer = 1;
		_buffer = ((_length == 0) ? null : UnsafeUtility.Malloc((long)_length * (long)UnsafeUtility.SizeOf<T>(), UnsafeUtility.AlignOf<T>(), allocator));
		if (_buffer != null && options == NativeArrayOptions.ClearMemory)
		{
			UnsafeUtility.MemClear(_buffer, (long)_length * (long)UnsafeUtility.SizeOf<T>());
		}
	}

	public NativeArray(T[] array, Allocator allocator)
	{
		if (array == null)
		{
			throw new ArgumentNullException("array");
		}
		this = new NativeArray<T>(array.Length, allocator, NativeArrayOptions.UninitializedMemory);
		CopyFrom(array);
	}

	internal unsafe NativeArray(void* buffer, int length, Allocator allocator, bool ownsBuffer)
	{
		_buffer = buffer;
		_length = length;
		_allocator = allocator;
		_ownsBuffer = (ownsBuffer ? ((byte)1) : ((byte)0));
	}

	public ReadOnly AsReadOnly()
	{
		return new ReadOnly(this);
	}

	public static void Copy(ReadOnly src, NativeArray<T> dst)
	{
		Copy(src, 0, dst, 0, Math.Min(src.Length, dst.Length));
	}

	public static void Copy(NativeArray<T> src, NativeArray<T> dst)
	{
		Copy(src, 0, dst, 0, Math.Min(src.Length, dst.Length));
	}

	public static void Copy(NativeArray<T> src, NativeArray<T> dst, int length)
	{
		Copy(src, 0, dst, 0, length);
	}

	public unsafe static void Copy(NativeArray<T> src, int srcIndex, NativeArray<T> dst, int dstIndex, int length)
	{
		if (length != 0)
		{
			UnsafeUtility.MemCpy((byte*)dst.GetUnsafePtrInternal() + (long)dstIndex * (long)UnsafeUtility.SizeOf<T>(), (byte*)src.GetUnsafeReadOnlyPtrInternal() + (long)srcIndex * (long)UnsafeUtility.SizeOf<T>(), (long)length * (long)UnsafeUtility.SizeOf<T>());
		}
	}

	public unsafe static void Copy(ReadOnly src, int srcIndex, NativeArray<T> dst, int dstIndex, int length)
	{
		if (length != 0)
		{
			UnsafeUtility.MemCpy((byte*)dst.GetUnsafePtrInternal() + (long)dstIndex * (long)UnsafeUtility.SizeOf<T>(), (byte*)src.GetUnsafeReadOnlyPtrInternal() + (long)srcIndex * (long)UnsafeUtility.SizeOf<T>(), (long)length * (long)UnsafeUtility.SizeOf<T>());
		}
	}

	public static void Copy(T[] src, NativeArray<T> dst)
	{
		Copy(src, 0, dst, 0, Math.Min(src.Length, dst.Length));
	}

	public static void Copy(T[] src, NativeArray<T> dst, int length)
	{
		Copy(src, 0, dst, 0, length);
	}

	public static void Copy(NativeArray<T> src, T[] dst)
	{
		Copy(src, 0, dst, 0, Math.Min(src.Length, dst.Length));
	}

	public static void Copy(ReadOnly src, T[] dst)
	{
		Copy(src, 0, dst, 0, Math.Min(src.Length, dst.Length));
	}

	public static void Copy(NativeArray<T> src, T[] dst, int length)
	{
		Copy(src, 0, dst, 0, length);
	}

	public static void Copy(ReadOnly src, T[] dst, int length)
	{
		Copy(src, 0, dst, 0, length);
	}

	public unsafe static void Copy(T[] src, int srcIndex, NativeArray<T> dst, int dstIndex, int length)
	{
		ArgumentNullException.ThrowIfNull(src, "src");
		if (srcIndex < 0 || length < 0 || srcIndex + length > src.Length)
		{
			throw new ArgumentOutOfRangeException("srcIndex");
		}
		if (length != 0)
		{
			fixed (byte* ptr = &Unsafe.As<T, byte>(ref MemoryMarshal.GetArrayDataReference(src)))
			{
				UnsafeUtility.MemCpy((byte*)dst.GetUnsafePtrInternal() + (long)dstIndex * (long)UnsafeUtility.SizeOf<T>(), ptr + (long)srcIndex * (long)UnsafeUtility.SizeOf<T>(), (long)length * (long)UnsafeUtility.SizeOf<T>());
			}
		}
	}

	public unsafe static void Copy(NativeArray<T> src, int srcIndex, T[] dst, int dstIndex, int length)
	{
		ArgumentNullException.ThrowIfNull(dst, "dst");
		if (dstIndex < 0 || length < 0 || dstIndex + length > dst.Length)
		{
			throw new ArgumentOutOfRangeException("dstIndex");
		}
		if (length != 0)
		{
			fixed (byte* ptr = &Unsafe.As<T, byte>(ref MemoryMarshal.GetArrayDataReference(dst)))
			{
				UnsafeUtility.MemCpy(ptr + (long)dstIndex * (long)UnsafeUtility.SizeOf<T>(), (byte*)src.GetUnsafeReadOnlyPtrInternal() + (long)srcIndex * (long)UnsafeUtility.SizeOf<T>(), (long)length * (long)UnsafeUtility.SizeOf<T>());
			}
		}
	}

	public unsafe static void Copy(ReadOnly src, int srcIndex, T[] dst, int dstIndex, int length)
	{
		ArgumentNullException.ThrowIfNull(dst, "dst");
		if (dstIndex < 0 || length < 0 || dstIndex + length > dst.Length)
		{
			throw new ArgumentOutOfRangeException("dstIndex");
		}
		if (length != 0)
		{
			fixed (byte* ptr = &Unsafe.As<T, byte>(ref MemoryMarshal.GetArrayDataReference(dst)))
			{
				UnsafeUtility.MemCpy(ptr + (long)dstIndex * (long)UnsafeUtility.SizeOf<T>(), (byte*)src.GetUnsafeReadOnlyPtrInternal() + (long)srcIndex * (long)UnsafeUtility.SizeOf<T>(), (long)length * (long)UnsafeUtility.SizeOf<T>());
			}
		}
	}

	public void CopyFrom(T[] array)
	{
		ArgumentNullException.ThrowIfNull(array, "array");
		if (array.Length != _length)
		{
			throw new ArgumentException("Source length does not match native array length.", "array");
		}
		for (int i = 0; i < array.Length; i++)
		{
			this[i] = array[i];
		}
	}

	public void CopyFrom(NativeArray<T> array)
	{
		Copy(array, this);
	}

	public void CopyTo(NativeArray<T> array)
	{
		Copy(this, array);
	}

	public unsafe void Dispose()
	{
		if (_ownsBuffer != 0 && _buffer != null)
		{
			UnsafeUtility.Free(_buffer, _allocator);
		}
		_buffer = null;
		_ownsBuffer = 0;
		_allocator = Allocator.Invalid;
	}

	public JobHandle Dispose(JobHandle inputDeps)
	{
		if (!IsCreated)
		{
			return inputDeps;
		}
		inputDeps.Complete();
		Dispose();
		return inputDeps;
	}

	public unsafe bool Equals(NativeArray<T> other)
	{
		if (_buffer == other._buffer && _length == other._length)
		{
			return _ownsBuffer == other._ownsBuffer;
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj is NativeArray<T> other)
		{
			return Equals(other);
		}
		return false;
	}

	public unsafe override int GetHashCode()
	{
		return (int)_buffer;
	}

	public static bool operator ==(NativeArray<T> left, NativeArray<T> right)
	{
		return left.Equals(right);
	}

	public static bool operator !=(NativeArray<T> left, NativeArray<T> right)
	{
		return !left.Equals(right);
	}

	IEnumerator<T> IEnumerable<T>.GetEnumerator()
	{
		return GetEnumerator();
	}

	public Enumerator GetEnumerator()
	{
		return new Enumerator(this);
	}

	public unsafe NativeArray<T> GetSubArray(int start, int length)
	{
		return new NativeArray<T>((byte*)GetUnsafeReadOnlyPtrInternal() + (long)start * (long)UnsafeUtility.SizeOf<T>(), length, _allocator, ownsBuffer: false);
	}

	public unsafe NativeArray<T0> Reinterpret<T0>()
	{
		NativeCollectionStorage.EnsureSupportedElementType<T0>();
		int num = checked(_length * UnsafeUtility.SizeOf<T>());
		int num2 = UnsafeUtility.SizeOf<T0>();
		if (num2 <= 0 || num % num2 != 0)
		{
			throw new InvalidOperationException($"Cannot reinterpret NativeArray<{typeof(T).Name}> as NativeArray<{typeof(T0).Name}>.");
		}
		return new NativeArray<T0>(_buffer, num / num2, _allocator, ownsBuffer: false);
	}

	public T[] ToArray()
	{
		T[] array = new T[_length];
		for (int i = 0; i < _length; i++)
		{
			array[i] = this[i];
		}
		return array;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	internal unsafe void* GetUnsafePtrInternal()
	{
		return _buffer;
	}

	internal unsafe void* GetUnsafeReadOnlyPtrInternal()
	{
		return GetUnsafePtrInternal();
	}

	[Conditional("DEBUG")]
	private void EnsureCreated()
	{
		if (!IsCreated)
		{
			throw new InvalidOperationException("NativeArray<" + typeof(T).Name + "> is not created.");
		}
	}

	[Conditional("DEBUG")]
	private unsafe void ValidateIndex(int index)
	{
		if ((uint)index >= (uint)_length)
		{
			throw new IndexOutOfRangeException($"Index {index} out of range for length {_length}");
		}
		if (_buffer == null)
		{
			throw new ObjectDisposedException("NativeArray<" + typeof(T).Name + ">");
		}
	}

	[Conditional("DEBUG")]
	internal unsafe void ValidateRange(int start, int length)
	{
		if (start < 0 || length < 0 || start + length > _length)
		{
			throw new ArgumentOutOfRangeException();
		}
		if (_buffer == null && length > 0)
		{
			throw new ObjectDisposedException("NativeArray<" + typeof(T).Name + ">");
		}
	}
}
