using System.Collections.Generic;

namespace Unity.Collections;

internal sealed class NativeParallelHashMapState<TKey, TValue>
{
	public Dictionary<TKey, TValue> Dictionary { get; }

	public object SyncRoot { get; } = new object();

	public NativeParallelHashMapState(int capacity)
	{
		Dictionary = ((capacity > 0) ? new Dictionary<TKey, TValue>(capacity) : new Dictionary<TKey, TValue>());
	}
}
