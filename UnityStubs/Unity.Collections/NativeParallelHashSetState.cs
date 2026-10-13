using System.Collections.Generic;

namespace Unity.Collections;

internal sealed class NativeParallelHashSetState<T>
{
	public HashSet<T> Set { get; }

	public object SyncRoot { get; } = new object();

	public NativeParallelHashSetState(int capacity)
	{
		Set = ((capacity > 0) ? new HashSet<T>() : new HashSet<T>());
	}
}
