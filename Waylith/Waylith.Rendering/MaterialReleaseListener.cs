using System;
using System.Collections.Concurrent;

namespace Waylith.Rendering;

public sealed class MaterialReleaseListener : IDisposable
{
	private readonly ConcurrentQueue<int> _releasedIds = new ConcurrentQueue<int>();

	public MaterialReleaseListener()
	{
		Material.RegisterReleaseListener(this);
	}

	internal void Enqueue(int materialId)
	{
		_releasedIds.Enqueue(materialId);
	}

	public bool TryDequeueReleased(out int materialId)
	{
		return _releasedIds.TryDequeue(out materialId);
	}

	public void Dispose()
	{
		Material.UnregisterReleaseListener(this);
	}
}
