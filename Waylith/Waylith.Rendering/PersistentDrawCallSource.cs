using System.Collections.Generic;
using System.Threading;

namespace Waylith.Rendering;

public class PersistentDrawCallSource
{
	private readonly List<FinalDrawItem>[] buffers = new List<FinalDrawItem>[2]
	{
		new List<FinalDrawItem>(),
		new List<FinalDrawItem>()
	};

	private volatile int activeIndex;

	private static long nextId;

	public long Id { get; } = Interlocked.Increment(ref nextId);

	public IReadOnlyList<FinalDrawItem> DrawItemsReadOnly => buffers[activeIndex];

	public void Reset()
	{
		buffers[1 - activeIndex].Clear();
	}

	public void Clear()
	{
		buffers[0].Clear();
		buffers[1].Clear();
	}

	public void Add(FinalDrawItem? drawItem)
	{
		if (drawItem != null)
		{
			buffers[1 - activeIndex].Add(drawItem);
		}
	}

	public void Commit()
	{
		activeIndex = 1 - activeIndex;
	}
}
