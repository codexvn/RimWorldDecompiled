using System;
using System.Runtime.InteropServices;

namespace Unity.Burst;

public struct SharedStatic<T> where T : unmanaged
{
	private static class ContextStorage<TContext>
	{
		public static readonly nint DataPtr;

		static ContextStorage()
		{
			DataPtr = GCHandle.Alloc(new T[1], GCHandleType.Pinned).AddrOfPinnedObject();
		}
	}

	private static class SubContextStorage<TContext, TSubContext>
	{
		public static readonly nint DataPtr;

		static SubContextStorage()
		{
			DataPtr = GCHandle.Alloc(new T[1], GCHandleType.Pinned).AddrOfPinnedObject();
		}
	}

	private nint _dataPtr;

	public unsafe ref T Data
	{
		get
		{
			if (_dataPtr == IntPtr.Zero)
			{
				throw new InvalidOperationException("SharedStatic has not been initialized. Use GetOrCreate to create an instance.");
			}
			return ref *(T*)_dataPtr;
		}
	}

	public static SharedStatic<T> GetOrCreate<TContext>()
	{
		SharedStatic<T> result = default(SharedStatic<T>);
		result._dataPtr = ContextStorage<TContext>.DataPtr;
		return result;
	}

	public static SharedStatic<T> GetOrCreate<TContext, TSubContext>()
	{
		SharedStatic<T> result = default(SharedStatic<T>);
		result._dataPtr = SubContextStorage<TContext, TSubContext>.DataPtr;
		return result;
	}
}
