using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Waylith.Runtime;

namespace Unity.Collections.LowLevel.Unsafe;

public static class UnsafeUtility
{
	public struct AlignOfHelper<T>
	{
		public byte Padding;

		public T Value;
	}

	public const int kIsManaged = 1;

	public const int kIsNativeContainer = 2;

	public static ref TTo As<TFrom, TTo>(ref TFrom from)
	{
		return ref System.Runtime.CompilerServices.Unsafe.As<TFrom, TTo>(ref from);
	}

	public static int AlignOf<T0>()
	{
		return System.Runtime.CompilerServices.Unsafe.SizeOf<AlignOfHelper<T0>>() - System.Runtime.CompilerServices.Unsafe.SizeOf<T0>();
	}

	public unsafe static void Free(void* memory, Allocator allocator)
	{
		NativeAllocationRuntime.Free((nint)memory, ToAllocationKind(allocator));
	}

	public unsafe static void* Malloc(long size, int alignment, Allocator allocator)
	{
		return (void*)NativeAllocationRuntime.Allocate(size, alignment, ToAllocationKind(allocator));
	}

	public unsafe static void MemClear(void* destination, long size)
	{
		if (destination != null && size > 0)
		{
			NativeMemory.Clear(destination, checked((nuint)size));
		}
	}

	public unsafe static void MemCpy(void* destination, void* source, long size)
	{
		if (destination != null && source != null && size > 0)
		{
			Buffer.MemoryCopy(source, destination, size, size);
		}
	}

	public unsafe static void MemSet(void* destination, byte value, long size)
	{
		if (destination != null && size > 0)
		{
			NativeMemory.Fill(destination, checked((nuint)size), value);
		}
	}

	public unsafe static void* PinGCArrayAndGetDataAddress(Array target, out ulong gcHandle)
	{
		GCHandle value = GCHandle.Alloc(target, GCHandleType.Pinned);
		gcHandle = (ulong)GCHandle.ToIntPtr(value);
		return System.Runtime.CompilerServices.Unsafe.AsPointer(in MemoryMarshal.GetArrayDataReference(target));
	}

	public static void ReleaseGCObject(ulong gcHandle)
	{
		GCHandle.FromIntPtr((nint)gcHandle).Free();
	}

	public unsafe static T0 ReadArrayElement<T0>(void* source, int index)
	{
		return System.Runtime.CompilerServices.Unsafe.Read<T0>((byte*)source + index * System.Runtime.CompilerServices.Unsafe.SizeOf<T0>());
	}

	public static int SizeOf<T0>()
	{
		return System.Runtime.CompilerServices.Unsafe.SizeOf<T0>();
	}

	public unsafe static void WriteArrayElement<T0>(void* destination, int index, T0 value)
	{
		System.Runtime.CompilerServices.Unsafe.Write((byte*)destination + index * System.Runtime.CompilerServices.Unsafe.SizeOf<T0>(), value);
	}

	private static AllocationKind ToAllocationKind(Allocator allocator)
	{
		switch (allocator)
		{
		case Allocator.Temp:
			return AllocationKind.Temp;
		case Allocator.TempJob:
			return AllocationKind.TempJob;
		case Allocator.Persistent:
			return AllocationKind.Persistent;
		case Allocator.Domain:
			return AllocationKind.Domain;
		case Allocator.AudioKernel:
			return AllocationKind.AudioKernel;
		case Allocator.None:
			return AllocationKind.None;
		case Allocator.Invalid:
			return AllocationKind.Invalid;
		default:
			if (allocator >= Allocator.FirstUserIndex)
			{
				return AllocationKind.Persistent;
			}
			return AllocationKind.Invalid;
		}
	}
}
