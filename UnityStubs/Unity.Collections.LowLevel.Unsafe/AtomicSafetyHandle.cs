using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Unity.Collections.LowLevel.Unsafe;

public struct AtomicSafetyHandle
{
	internal sealed class State
	{
		public string TypeName = "NativeContainer";

		public int StaticSafetyId;
	}

	private static int s_nextId = 1;

	private static readonly ConcurrentDictionary<int, State> s_registry = new ConcurrentDictionary<int, State>();

	private int _id;

	public static AtomicSafetyHandle Create()
	{
		int num = Interlocked.Increment(ref s_nextId);
		s_registry[num] = new State();
		AtomicSafetyHandle result = default(AtomicSafetyHandle);
		result._id = num;
		return result;
	}

	internal static void Release(ref AtomicSafetyHandle handle)
	{
		s_registry.TryRemove(handle._id, out var _);
		handle = default(AtomicSafetyHandle);
	}

	internal State TryGetState()
	{
		if (_id != 0 && s_registry.TryGetValue(_id, out var value))
		{
			return value;
		}
		return null;
	}

	public void CheckWriteAndThrow()
	{
		if (TryGetState() == null)
		{
			throw new ObjectDisposedException(null, "The native container has been disposed or is not valid.");
		}
	}

	public void CheckReadAndThrow()
	{
		if (TryGetState() == null)
		{
			throw new ObjectDisposedException(null, "The native container has been disposed or is not valid.");
		}
	}
}
