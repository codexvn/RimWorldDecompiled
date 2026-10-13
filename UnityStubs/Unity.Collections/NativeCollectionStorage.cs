using System;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Collections;

internal static class NativeCollectionStorage
{
	public static void EnsureSupportedElementType<T>()
	{
		if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
		{
			throw new NotSupportedException($"Native collections only support blittable value types. '{typeof(T)}' is not supported.");
		}
	}

	public static int ElementSizeOf<T>()
	{
		return UnsafeUtility.SizeOf<T>();
	}

	public unsafe static void ClearElements<T>(void* destination, int count)
	{
		if (destination != null && count > 0)
		{
			UnsafeUtility.MemClear(destination, (long)count * (long)ElementSizeOf<T>());
		}
	}

	public unsafe static THeader* AllocateHeader<THeader>(Allocator allocator) where THeader : unmanaged
	{
		THeader* ptr = (THeader*)UnsafeUtility.Malloc(sizeof(THeader), UnsafeUtility.AlignOf<THeader>(), allocator);
		UnsafeUtility.MemClear(ptr, sizeof(THeader));
		return ptr;
	}

	public unsafe static void EnsureElementCapacity<T>(ref void* buffer, ref int capacity, int requiredCapacity, Allocator allocator, bool clearNewMemory, int preserveCount)
	{
		EnsureSupportedElementType<T>();
		if (requiredCapacity > capacity)
		{
			int num;
			for (num = ((capacity == 0) ? 4 : capacity); num < requiredCapacity; num = checked(num * 2))
			{
			}
			int num2 = ElementSizeOf<T>();
			void* ptr = UnsafeUtility.Malloc((long)num * (long)num2, UnsafeUtility.AlignOf<T>(), allocator);
			if (preserveCount > 0 && buffer != null)
			{
				UnsafeUtility.MemCpy(ptr, buffer, (long)preserveCount * (long)num2);
			}
			if (clearNewMemory && num > preserveCount)
			{
				UnsafeUtility.MemClear((byte*)ptr + (long)preserveCount * (long)num2, (long)(num - preserveCount) * (long)num2);
			}
			if (buffer != null)
			{
				UnsafeUtility.Free(buffer, allocator);
			}
			buffer = ptr;
			capacity = num;
		}
	}
}
