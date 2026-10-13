using System.Collections.Generic;

namespace Waylith.Profiling;

public sealed class ThreadTimeline
{
	internal struct OpenSlot
	{
		public int node;

		public long accountedUntil;

		public long childNs;

		public long childTotalNs;

		public long selfAlloc;

		public long subtreeAlloc;

		public long subtreeAllocTotal;

		public bool flushed;

		public ProfilerLabelId lastChildLabel;

		public int lastChildPath;

		public static OpenSlot Open(int node, long ts)
		{
			OpenSlot result = default(OpenSlot);
			result.node = node;
			result.accountedUntil = ts;
			result.lastChildPath = -1;
			return result;
		}
	}

	public readonly int threadId;

	public readonly bool isMain;

	public readonly ProfilerLabelId nameLabelId;

	public readonly int rootPathId;

	internal readonly List<ZoneChunk> chunks = new List<ZoneChunk>();

	internal ZoneChunk? current;

	internal OpenSlot[] stack = new OpenSlot[64];

	internal int depth;

	internal int markSeq = -1;

	internal int enteredSeq = -1;

	internal int carryNodes;

	internal long lastTimestampNs;

	internal bool retired;

	internal ThreadTimeline(int threadId, string threadName, bool isMain)
	{
		this.threadId = threadId;
		this.isMain = isMain;
		nameLabelId = FrameProfiler.GetOrCreateLabelId(threadName);
		rootPathId = ProfilerPathTable.GetOrAdd(0, isMain ? FrameProfiler.RootLabelId : nameLabelId);
	}
}
