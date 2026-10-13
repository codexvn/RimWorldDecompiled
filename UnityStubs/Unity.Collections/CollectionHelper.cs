using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Collections;

public static class CollectionHelper
{
	private static int s_nextSafetyId = 1;

	private static readonly Dictionary<string, int> s_safetyIds = new Dictionary<string, int>();

	private static readonly object s_lock = new object();

	public static AtomicSafetyHandle CreateSafetyHandle(Allocator allocator)
	{
		return AtomicSafetyHandle.Create();
	}

	public static void SetStaticSafetyId(ref AtomicSafetyHandle handle, ref int staticSafetyId, string typeName)
	{
		if (staticSafetyId == 0)
		{
			lock (s_lock)
			{
				if (!s_safetyIds.TryGetValue(typeName, out var value))
				{
					value = s_nextSafetyId++;
					s_safetyIds[typeName] = value;
				}
				staticSafetyId = value;
			}
		}
		AtomicSafetyHandle.State state = handle.TryGetState();
		if (state != null)
		{
			state.StaticSafetyId = staticSafetyId;
			state.TypeName = typeName;
		}
	}

	public static void DisposeSafetyHandle(ref AtomicSafetyHandle handle)
	{
		AtomicSafetyHandle.Release(ref handle);
	}
}
