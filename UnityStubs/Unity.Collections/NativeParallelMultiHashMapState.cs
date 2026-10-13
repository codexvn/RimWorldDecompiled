using System.Collections.Generic;

namespace Unity.Collections;

internal sealed class NativeParallelMultiHashMapState<TKey, TValue>
{
	public Dictionary<TKey, List<TValue>> Dictionary { get; }

	public object SyncRoot { get; } = new object();

	public int TotalCount { get; set; }

	public NativeParallelMultiHashMapState(int capacity)
	{
		Dictionary = ((capacity > 0) ? new Dictionary<TKey, List<TValue>>(capacity) : new Dictionary<TKey, List<TValue>>());
	}
}
