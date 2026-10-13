using System;
using System.Diagnostics;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;

namespace Unity.Collections;

public struct NativeBitArray : INativeDisposable, IDisposable
{
	public struct ReadOnly
	{
		private NativeBitArray _array;

		public int Length => _array.Length;

		internal ReadOnly(NativeBitArray array)
		{
			_array = array;
		}

		public bool IsSet(int pos)
		{
			return _array.IsSet(pos);
		}
	}

	private unsafe NativeBitArrayHeader* _header;

	public const int MaxCapacity = 2147483584;

	public unsafe bool IsCreated => _header != null;

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
	}

	public unsafe NativeBitArray(int numBits, AllocatorManager.AllocatorHandle allocator, NativeArrayOptions options = NativeArrayOptions.ClearMemory)
	{
		if (numBits < 0 || numBits > 2147483584)
		{
			throw new ArgumentOutOfRangeException("numBits");
		}
		_header = NativeCollectionStorage.AllocateHeader<NativeBitArrayHeader>(allocator.ToAllocator);
		_header->Allocator = allocator.ToAllocator;
		_header->Length = numBits;
		_header->WordCount = ((numBits != 0) ? (numBits + 63 >> 6) : 0);
		_header->Buffer = (ulong*)((_header->WordCount == 0) ? null : UnsafeUtility.Malloc((long)_header->WordCount * 8L, UnsafeUtility.AlignOf<ulong>(), allocator.ToAllocator));
		if (_header->Buffer != null && options == NativeArrayOptions.ClearMemory)
		{
			UnsafeUtility.MemClear(_header->Buffer, (long)_header->WordCount * 8L);
		}
	}

	public ReadOnly AsReadOnly()
	{
		return new ReadOnly(this);
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

	public unsafe void Clear()
	{
		if (_header != null && _header->Buffer != null && _header->WordCount != 0)
		{
			UnsafeUtility.MemClear(_header->Buffer, (long)_header->WordCount * 8L);
		}
	}

	public void Copy(int dstPos, ref NativeBitArray srcBitArray, int srcPos, int numBits)
	{
		for (int i = 0; i < numBits; i++)
		{
			Set(dstPos + i, srcBitArray.IsSet(srcPos + i));
		}
	}

	public unsafe bool IsSet(int pos)
	{
		if (_header->Buffer == null)
		{
			return false;
		}
		int num = pos >> 6;
		int num2 = pos & 0x3F;
		return ((_header->Buffer[num] >> num2) & 1) != 0;
	}

	public unsafe void Set(int pos, bool value)
	{
		if (_header->Buffer != null)
		{
			int num = pos >> 6;
			int num2 = pos & 0x3F;
			ulong num3 = (ulong)(1L << num2);
			if (value)
			{
				_header->Buffer[num] |= num3;
			}
			else
			{
				_header->Buffer[num] &= ~num3;
			}
		}
	}

	[Conditional("DEBUG")]
	private unsafe void EnsureCreated()
	{
		if (_header == null)
		{
			throw new InvalidOperationException("NativeBitArray is not created.");
		}
	}

	[Conditional("DEBUG")]
	private void ValidateIndex(int index)
	{
		if ((uint)index >= (uint)Length)
		{
			throw new IndexOutOfRangeException();
		}
	}

	[Conditional("DEBUG")]
	private void ValidateRange(int start, int length)
	{
		if (start < 0 || length < 0 || start + length > Length)
		{
			throw new ArgumentOutOfRangeException();
		}
	}
}
