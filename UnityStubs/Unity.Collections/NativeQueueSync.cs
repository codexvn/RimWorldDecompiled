using System.Collections.Concurrent;

namespace Unity.Collections;

internal static class NativeQueueSync
{
	private static readonly ConcurrentDictionary<nint, object> Roots = new ConcurrentDictionary<nint, object>();

	public static object GetOrCreate(nint key)
	{
		return Roots.GetOrAdd(key, (nint _) => new object());
	}

	public static void Remove(nint key)
	{
		Roots.TryRemove(key, out var _);
	}
}
