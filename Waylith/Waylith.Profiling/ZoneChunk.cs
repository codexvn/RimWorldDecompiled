using System;
using System.Threading;

namespace Waylith.Profiling;

public sealed class ZoneChunk
{
	public long baseNs;

	public ZoneNode[] nodes = Array.Empty<ZoneNode>();

	public FrameProfiler.NodeAllocStat[]? allocStats;

	public int count;

	public long endNs;

	internal int openCount;

	public int Count => Volatile.Read(in count);
}
