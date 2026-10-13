using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Waylith.Profiling;

internal sealed class ProfilerFrameAssembler
{
	public delegate bool StatsLookup(int frameNumber, out FrameStatsRecord stats);

	public delegate int PauseCollector(long beginNs, long endNs, FrameProfiler.PauseSpan[] into, out float pauseMs, out long promotedBytes);

	private sealed class ThreadBucket
	{
		public ThreadTimeline thread;

		public FrameProfiler.PathEntry[] entries = new FrameProfiler.PathEntry[64];

		public int count;

		public readonly Dictionary<int, int> entryIndexByPath = new Dictionary<int, int>();

		public long busyNs;

		public long startNs;

		public long endNs;

		public int zonesRecorded;

		public int overflowed;

		public int unbalanced;

		public int nodes;

		public long rootSelfAllocBytes;

		public int rootSelfAllocCount;

		public long allocCount;

		public bool HasContent
		{
			get
			{
				if (count <= 0 && rootSelfAllocCount <= 0 && unbalanced <= 0 && overflowed <= 0)
				{
					return thread.isMain;
				}
				return true;
			}
		}

		public void Reset(ThreadTimeline owner)
		{
			thread = owner;
			count = 0;
			entryIndexByPath.Clear();
			busyNs = 0L;
			startNs = long.MaxValue;
			endNs = 0L;
			zonesRecorded = 0;
			overflowed = 0;
			unbalanced = 0;
			nodes = 0;
			rootSelfAllocBytes = 0L;
			rootSelfAllocCount = 0;
			allocCount = 0L;
		}
	}

	private sealed class PendingMark
	{
		public int seq;

		public int frameNumber;

		public long beginNs;

		public long endNs;

		public long boundaryNs;

		public bool ended;

		public readonly List<ThreadBucket> buckets = new List<ThreadBucket>();

		public readonly Dictionary<int, ThreadBucket> bucketByThread = new Dictionary<int, ThreadBucket>();

		public readonly Dictionary<long, int> typeCounterIndices = new Dictionary<long, int>();

		public FrameProfiler.TypeAllocStat[] typeCounters = new FrameProfiler.TypeAllocStat[64];

		public int typeCounterCount;

		public int allocOverflowCount;

		public long allocOverflowBytes;
	}

	private const int SoftChunkNodes = 4096;

	private const int MinChunkCutNodes = 1024;

	private const int HardChunkNodes = 131072;

	private const int TopAllocTypeCapacity = 256;

	private const int MaxTrackedTypesPerFrame = 2048;

	private const long DiscardableScopeMinNs = 5000L;

	private const long MaxChunkSpanNs = 2147483648L;

	private const long StalePublishTimeoutNs = 150000000L;

	internal static long RetainedNodeBudget = 4194304L;

	public StatsLookup? statsLookup;

	public PauseCollector? pauseCollector;

	public Action<FrameProfiler.FrameSnapshot>? onFramePublished;

	public Action<long>? onZonesEvicted;

	private readonly Lock timelineLock = new Lock();

	private readonly List<ThreadTimeline> timelines = new List<ThreadTimeline>();

	private readonly Dictionary<int, ThreadTimeline> timelinesByThreadId = new Dictionary<int, ThreadTimeline>();

	private long retainedNodeSlots;

	private const int MaxGraveyardChunks = 256;

	private const int MaxPooledChunks = 128;

	private readonly List<ZoneChunk> chunkGraveyard = new List<ZoneChunk>();

	private readonly List<ZoneChunk> chunkPool = new List<ZoneChunk>();

	private const int MaxPooledSnapshots = 64;

	private const int MaxPooledPathEntryArrays = 2048;

	private readonly List<FrameProfiler.PathEntry[]> pathEntryPool = new List<FrameProfiler.PathEntry[]>();

	private long pooledBytes;

	private readonly ConcurrentQueue<FrameProfiler.FrameSnapshot> snapshotGraveyard = new ConcurrentQueue<FrameProfiler.FrameSnapshot>();

	private readonly Stack<FrameProfiler.FrameSnapshot> snapshotPool = new Stack<FrameProfiler.FrameSnapshot>();

	private readonly FrameProfiler.PauseSpan[] pauseScratch = new FrameProfiler.PauseSpan[1024];

	private readonly List<PendingMark> marks = new List<PendingMark>();

	private readonly Stack<PendingMark> markPool = new Stack<PendingMark>();

	private readonly Stack<ThreadBucket> bucketPool = new Stack<ThreadBucket>();

	private int nextMarkSeq;

	private const int AutoMuteMinSamples = 2000;

	private const int AutoMuteDiscardRatio = 10;

	private int[]? autoMuteDiscarded;

	private int[]? autoMuteKept;

	private readonly List<int> autoMuteBatch = new List<int>();

	private bool autoMuteWasEnabled = true;

	public long PooledBytes => Volatile.Read(in pooledBytes);

	public long RetainedNodeSlots => Volatile.Read(in retainedNodeSlots);

	internal static long ChunkBytes(ZoneChunk chunk)
	{
		long num = (long)chunk.nodes.Length * (long)Unsafe.SizeOf<ZoneNode>();
		FrameProfiler.NodeAllocStat[]? allocStats = chunk.allocStats;
		return num + (long)((allocStats != null) ? allocStats.Length : 0) * (long)Unsafe.SizeOf<FrameProfiler.NodeAllocStat>();
	}

	private static long SnapshotArrayBytes(FrameProfiler.FrameSnapshot snapshot)
	{
		return (long)snapshot.pathEntries.Length * (long)Unsafe.SizeOf<FrameProfiler.PathEntry>() + (long)snapshot.sections.Length * (long)Unsafe.SizeOf<FrameProfiler.ThreadSection>() + (long)snapshot.pauses.Length * (long)Unsafe.SizeOf<FrameProfiler.PauseSpan>() + (long)snapshot.topAllocTypes.Length * (long)Unsafe.SizeOf<FrameProfiler.TypeAllocStat>();
	}

	private FrameProfiler.PathEntry[] TakePathEntries(int length)
	{
		for (int num = pathEntryPool.Count - 1; num >= 0; num--)
		{
			FrameProfiler.PathEntry[] array = pathEntryPool[num];
			if (array.Length == length)
			{
				pathEntryPool.RemoveAt(num);
				pooledBytes -= (long)length * (long)Unsafe.SizeOf<FrameProfiler.PathEntry>();
				return array;
			}
		}
		return new FrameProfiler.PathEntry[length];
	}

	private void ReturnPathEntries(FrameProfiler.PathEntry[] array)
	{
		if (array.Length != 0)
		{
			if (pathEntryPool.Count == 2048)
			{
				pooledBytes -= (long)pathEntryPool[0].Length * (long)Unsafe.SizeOf<FrameProfiler.PathEntry>();
				pathEntryPool.RemoveAt(0);
			}
			pathEntryPool.Add(array);
			pooledBytes += (long)array.Length * (long)Unsafe.SizeOf<FrameProfiler.PathEntry>();
		}
	}

	public ThreadTimeline GetTimeline(int threadId, string threadName, bool isMain)
	{
		using (timelineLock.EnterScope())
		{
			if (timelinesByThreadId.TryGetValue(threadId, out ThreadTimeline value))
			{
				value.retired = false;
				return value;
			}
			ThreadTimeline threadTimeline = new ThreadTimeline(threadId, threadName, isMain);
			timelines.Add(threadTimeline);
			timelinesByThreadId.Add(threadId, threadTimeline);
			return threadTimeline;
		}
	}

	public void RetireTimeline(int threadId)
	{
		ThreadTimeline value;
		using (timelineLock.EnterScope())
		{
			if (!timelinesByThreadId.TryGetValue(threadId, out value))
			{
				return;
			}
		}
		while (value.depth > 0)
		{
			CloseZone(value, value.lastTimestampNs);
		}
		value.retired = true;
		DropEmptyRetired(value);
	}

	private void DropEmptyRetired(ThreadTimeline timeline)
	{
		if (!timeline.retired || timeline.chunks.Count > 0)
		{
			return;
		}
		using (timelineLock.EnterScope())
		{
			timelines.Remove(timeline);
			timelinesByThreadId.Remove(timeline.threadId);
		}
	}

	public void Reset()
	{
		using (timelineLock.EnterScope())
		{
			foreach (ThreadTimeline timeline in timelines)
			{
				timeline.chunks.Clear();
				timeline.current = null;
				for (int i = 0; i < timeline.depth; i++)
				{
					timeline.stack[i] = ThreadTimeline.OpenSlot.Open(-1, timeline.lastTimestampNs);
				}
				timeline.markSeq = -1;
				timeline.enteredSeq = -1;
				timeline.carryNodes = 0;
				timeline.lastTimestampNs = 0L;
			}
			retainedNodeSlots = 0L;
		}
		foreach (PendingMark mark in marks)
		{
			ReleaseMark(mark);
		}
		marks.Clear();
		autoMuteBatch.Clear();
		if (autoMuteDiscarded != null)
		{
			Array.Clear(autoMuteDiscarded);
		}
		if (autoMuteKept != null)
		{
			Array.Clear(autoMuteKept);
		}
	}

	private PendingMark? MarkBySeq(int seq)
	{
		if (marks.Count == 0)
		{
			return null;
		}
		int num = seq - marks[0].seq;
		if (num < 0 || num >= marks.Count)
		{
			return null;
		}
		return marks[num];
	}

	private ThreadBucket BucketFor(PendingMark mark, ThreadTimeline thread)
	{
		if (mark.bucketByThread.TryGetValue(thread.threadId, out ThreadBucket value))
		{
			return value;
		}
		value = ((bucketPool.Count > 0) ? bucketPool.Pop() : new ThreadBucket());
		value.Reset(thread);
		mark.buckets.Add(value);
		mark.bucketByThread.Add(thread.threadId, value);
		return value;
	}

	private ThreadBucket? CursorBucket(ThreadTimeline thread)
	{
		PendingMark pendingMark = MarkBySeq(thread.markSeq);
		if (pendingMark != null)
		{
			return BucketFor(pendingMark, thread);
		}
		return null;
	}

	private static ref FrameProfiler.PathEntry EntryFor(ThreadBucket bucket, int pathId)
	{
		if (!bucket.entryIndexByPath.TryGetValue(pathId, out var value))
		{
			value = bucket.count++;
			if (value == bucket.entries.Length)
			{
				Array.Resize(ref bucket.entries, bucket.entries.Length * 2);
			}
			bucket.entries[value] = new FrameProfiler.PathEntry
			{
				pathId = pathId
			};
			bucket.entryIndexByPath.Add(pathId, value);
		}
		return ref bucket.entries[value];
	}

	private void ReleaseMark(PendingMark mark)
	{
		foreach (ThreadBucket bucket in mark.buckets)
		{
			bucketPool.Push(bucket);
		}
		mark.buckets.Clear();
		mark.bucketByThread.Clear();
		mark.typeCounterIndices.Clear();
		mark.typeCounterCount = 0;
		mark.allocOverflowCount = 0;
		mark.allocOverflowBytes = 0L;
		mark.ended = false;
		mark.boundaryNs = 0L;
		mark.endNs = 0L;
		markPool.Push(mark);
	}

	public bool ProcessEvent(ThreadTimeline thread, in ProfilerEvent evt, long nowNs)
	{
		long num = FrameProfiler.TimestampToNanoseconds(evt.Timestamp);
		if (!thread.isMain && marks.Count > 0)
		{
			List<PendingMark> list = marks;
			PendingMark pendingMark = list[list.Count - 1];
			if (pendingMark.ended && pendingMark.boundaryNs == 0L && num >= pendingMark.endNs && nowNs - pendingMark.endNs <= 150000000)
			{
				return false;
			}
		}
		switch (evt.Type)
		{
		case ProfilerEventType.FrameBegin:
			if (thread.isMain)
			{
				BeginFrame(thread, num, evt.AsFrameMark.FrameNumber);
			}
			break;
		case ProfilerEventType.FrameEnd:
			if (thread.isMain)
			{
				EndFrame(thread, num);
			}
			break;
		case ProfilerEventType.ZoneBegin:
			Advance(thread, num);
			OpenZone(thread, num, evt.AsZoneBegin.Label);
			break;
		case ProfilerEventType.ZoneEnd:
			Advance(thread, num);
			CloseZone(thread, num);
			break;
		case ProfilerEventType.Alloc:
			Advance(thread, num);
			RecordAlloc(thread, evt.AsAlloc.Size, evt.AsAlloc.TypeId);
			break;
		}
		thread.lastTimestampNs = num;
		return true;
	}

	private void BeginFrame(ThreadTimeline main, long ts, int frameNumber)
	{
		if (marks.Count > 0)
		{
			List<PendingMark> list = marks;
			PendingMark pendingMark = list[list.Count - 1];
			if (!pendingMark.ended)
			{
				EndFrame(main, ts);
			}
			pendingMark.boundaryNs = ts;
		}
		PendingMark pendingMark2 = ((markPool.Count > 0) ? markPool.Pop() : new PendingMark());
		pendingMark2.seq = nextMarkSeq++;
		pendingMark2.frameNumber = frameNumber;
		pendingMark2.beginNs = ts;
		marks.Add(pendingMark2);
		BucketFor(pendingMark2, main);
		Advance(main, ts);
	}

	private void EndFrame(ThreadTimeline main, long ts)
	{
		if (marks.Count != 0)
		{
			List<PendingMark> list = marks;
			PendingMark pendingMark = list[list.Count - 1];
			if (!pendingMark.ended)
			{
				pendingMark.endNs = ts;
				pendingMark.ended = true;
				Advance(main, ts);
			}
		}
	}

	private void Advance(ThreadTimeline thread, long ts)
	{
		if (thread.markSeq < 0)
		{
			if (marks.Count == 0)
			{
				return;
			}
			thread.markSeq = marks[0].seq;
			for (int i = 0; i < thread.depth; i++)
			{
				ref ThreadTimeline.OpenSlot reference = ref thread.stack[i];
				reference.accountedUntil = System.Math.Max(reference.accountedUntil, marks[0].beginNs);
				reference.childNs = 0L;
			}
			thread.carryNodes = OpenNodeCount(thread);
		}
		while (true)
		{
			PendingMark pendingMark = MarkBySeq(thread.markSeq);
			if (pendingMark == null)
			{
				break;
			}
			if (thread.enteredSeq != pendingMark.seq)
			{
				EnterMark(thread, pendingMark);
			}
			long num = ((thread.isMain && pendingMark.ended) ? pendingMark.endNs : pendingMark.boundaryNs);
			if (num == 0L || ts < num)
			{
				break;
			}
			Flush(thread, pendingMark, num);
			thread.markSeq++;
		}
	}

	private void EnterMark(ThreadTimeline thread, PendingMark mark)
	{
		thread.enteredSeq = mark.seq;
		for (int i = 0; i < thread.depth; i++)
		{
			ref ThreadTimeline.OpenSlot reference = ref thread.stack[i];
			if (reference.accountedUntil < mark.beginNs)
			{
				reference.accountedUntil = mark.beginNs;
				reference.childNs = 0L;
			}
		}
		if (thread.carryNodes != 0)
		{
			ThreadBucket threadBucket = BucketFor(mark, thread);
			threadBucket.nodes += thread.carryNodes;
			threadBucket.startNs = System.Math.Min(threadBucket.startNs, mark.beginNs);
			thread.carryNodes = 0;
		}
	}

	private void Flush(ThreadTimeline thread, PendingMark mark, long boundary)
	{
		if (thread.depth == 0)
		{
			return;
		}
		ThreadBucket threadBucket = BucketFor(mark, thread);
		ZoneChunk current = thread.current;
		for (int num = thread.depth - 1; num >= 0; num--)
		{
			ref ThreadTimeline.OpenSlot reference = ref thread.stack[num];
			long num2 = System.Math.Max(0L, boundary - reference.accountedUntil);
			long subtreeAlloc = reference.subtreeAlloc;
			if (reference.node >= 0)
			{
				Account(ref EntryFor(threadBucket, current.nodes[reference.node].pathId), num2, reference.childNs, subtreeAlloc, reference.selfAlloc);
			}
			if (num > 0)
			{
				ref ThreadTimeline.OpenSlot reference2 = ref thread.stack[num - 1];
				reference2.childNs += num2;
				reference2.subtreeAlloc += subtreeAlloc;
			}
			else
			{
				threadBucket.busyNs += num2;
			}
			reference.accountedUntil = boundary;
			reference.childNs = 0L;
			reference.selfAlloc = 0L;
			reference.subtreeAlloc = 0L;
			reference.flushed = true;
		}
		threadBucket.endNs = System.Math.Max(threadBucket.endNs, boundary);
		thread.carryNodes = OpenNodeCount(thread);
	}

	private void OpenZone(ThreadTimeline thread, long ts, ProfilerLabelId labelId)
	{
		ThreadBucket threadBucket = CursorBucket(thread);
		if (threadBucket != null)
		{
			threadBucket.zonesRecorded++;
		}
		if (thread.depth == thread.stack.Length)
		{
			Array.Resize(ref thread.stack, thread.stack.Length * 2);
		}
		ZoneChunk zoneChunk = thread.current;
		if (zoneChunk == null || (zoneChunk.openCount == 0 && (ChunkFull(zoneChunk) || SpanFull(zoneChunk, ts))))
		{
			zoneChunk = NewChunk(thread, ts);
		}
		else if (zoneChunk.count + thread.depth + 1 > 131072 || SpanFull(zoneChunk, ts))
		{
			zoneChunk = SplitChunk(thread, ts);
		}
		int num = thread.depth++;
		if (zoneChunk.count >= 131072)
		{
			if (threadBucket != null)
			{
				threadBucket.overflowed++;
			}
			thread.stack[num] = ThreadTimeline.OpenSlot.Open(-1, ts);
			return;
		}
		if (zoneChunk.count == zoneChunk.nodes.Length)
		{
			GrowChunk(zoneChunk);
		}
		int pathId = ((num > 0) ? ChildPathId(ref thread.stack[num - 1], zoneChunk, thread.rootPathId, labelId) : ProfilerPathTable.GetOrAdd(thread.rootPathId, labelId));
		int count = zoneChunk.count;
		zoneChunk.nodes[count] = new ZoneNode
		{
			pathId = pathId,
			startNs = RelativeNs(ts - zoneChunk.baseNs),
			endNs = uint.MaxValue,
			subtreeEnd = -1
		};
		Volatile.Write(ref zoneChunk.count, count + 1);
		zoneChunk.openCount++;
		zoneChunk.endNs = long.MaxValue;
		thread.stack[num] = ThreadTimeline.OpenSlot.Open(count, ts);
		if (threadBucket != null)
		{
			EntryFor(threadBucket, pathId).calls++;
			threadBucket.nodes++;
			threadBucket.startNs = System.Math.Min(threadBucket.startNs, ts);
		}
	}

	private static int ChildPathId(ref ThreadTimeline.OpenSlot parent, ZoneChunk chunk, int rootPathId, ProfilerLabelId labelId)
	{
		if (parent.lastChildLabel == labelId && parent.lastChildPath >= 0)
		{
			return parent.lastChildPath;
		}
		int parentPathId = ((parent.node >= 0) ? chunk.nodes[parent.node].pathId : rootPathId);
		parent.lastChildLabel = labelId;
		parent.lastChildPath = ProfilerPathTable.GetOrAdd(parentPathId, labelId);
		return parent.lastChildPath;
	}

	private static void Account(ref FrameProfiler.PathEntry entry, long portionNs, long childNs, long subtreeAlloc, long selfAlloc)
	{
		entry.totalMicroseconds += (float)portionNs * 0.001f;
		entry.selfMicroseconds += (float)System.Math.Max(0L, portionNs - childNs) * 0.001f;
		entry.allocBytes = ClampToInt(entry.allocBytes + subtreeAlloc);
		entry.selfAllocBytes = ClampToInt(entry.selfAllocBytes + selfAlloc);
	}

	private static int OpenNodeCount(ThreadTimeline thread)
	{
		int num = 0;
		for (int i = 0; i < thread.depth; i++)
		{
			if (thread.stack[i].node >= 0)
			{
				num++;
			}
		}
		return num;
	}

	private ZoneChunk SplitChunk(ThreadTimeline thread, long ts)
	{
		ZoneChunk current = thread.current;
		for (int num = thread.depth - 1; num >= 0; num--)
		{
			ref ThreadTimeline.OpenSlot reference = ref thread.stack[num];
			if (reference.node >= 0)
			{
				ref ZoneNode reference2 = ref current.nodes[reference.node];
				long num2 = System.Math.Max(ts - (current.baseNs + reference2.startNs), reference.childTotalNs);
				reference2.endNs = System.Math.Max(reference2.startNs, RelativeNs(ts - current.baseNs));
				reference2.subtreeEnd = current.count;
				if (current.allocStats != null)
				{
					current.allocStats[reference.node].totalBytes = ClampToInt(reference.subtreeAllocTotal);
				}
				if (num > 0)
				{
					thread.stack[num - 1].childTotalNs += num2;
				}
			}
		}
		current.openCount = 0;
		current.endNs = ts;
		ZoneChunk zoneChunk = NewChunk(thread, ts);
		while (zoneChunk.nodes.Length < thread.depth + 1)
		{
			GrowChunk(zoneChunk);
		}
		for (int i = 0; i < thread.depth; i++)
		{
			ref ThreadTimeline.OpenSlot reference3 = ref thread.stack[i];
			if (reference3.node >= 0)
			{
				int count = zoneChunk.count;
				zoneChunk.nodes[count] = new ZoneNode
				{
					pathId = current.nodes[reference3.node].pathId,
					startNs = 0u,
					endNs = uint.MaxValue,
					subtreeEnd = -1
				};
				Volatile.Write(ref zoneChunk.count, count + 1);
				zoneChunk.openCount++;
				reference3.node = count;
				reference3.childTotalNs = 0L;
				reference3.subtreeAllocTotal = 0L;
			}
		}
		zoneChunk.endNs = long.MaxValue;
		return zoneChunk;
	}

	private static bool SpanFull(ZoneChunk chunk, long ts)
	{
		return ts - chunk.baseNs >= 2147483648u;
	}

	private static uint RelativeNs(long value)
	{
		if (value > 0)
		{
			if (value < 4294967294u)
			{
				return (uint)value;
			}
			return 4294967294u;
		}
		return 0u;
	}

	private static bool ChunkFull(ZoneChunk chunk)
	{
		if (chunk.count < 4096)
		{
			if (chunk.count >= 1024)
			{
				return chunk.count * 2 >= chunk.nodes.Length;
			}
			return false;
		}
		return true;
	}

	private static int ChunkCapacityFor(int needed)
	{
		return BucketedCapacity(needed, 256, 131072);
	}

	private static int BucketedCapacity(int needed, int minimum, int maximum)
	{
		int num = minimum;
		while (num < needed && num < maximum)
		{
			num = System.Math.Min(maximum, num + num / 5);
		}
		return num;
	}

	private ZoneChunk NewChunk(ThreadTimeline thread, long baseNs)
	{
		int num = 256;
		if (thread.current != null)
		{
			TrimChunk(thread.current);
			num = ChunkCapacityFor(thread.current.count + thread.current.count / 4);
		}
		ZoneChunk zoneChunk = TakePooledChunk(num, baseNs) ?? new ZoneChunk
		{
			baseNs = baseNs,
			nodes = new ZoneNode[num],
			endNs = baseNs
		};
		using (timelineLock.EnterScope())
		{
			thread.chunks.Add(zoneChunk);
			retainedNodeSlots += zoneChunk.nodes.Length;
		}
		thread.current = zoneChunk;
		EnforceNodeBudget();
		return zoneChunk;
	}

	private void GrowChunk(ZoneChunk chunk)
	{
		int num = ChunkCapacityFor(chunk.nodes.Length * 2);
		ZoneNode[] array = new ZoneNode[num];
		Array.Copy(chunk.nodes, array, chunk.count);
		if (chunk.allocStats != null)
		{
			FrameProfiler.NodeAllocStat[] array2 = new FrameProfiler.NodeAllocStat[num];
			Array.Copy(chunk.allocStats, array2, chunk.count);
			chunk.allocStats = array2;
		}
		using (timelineLock.EnterScope())
		{
			retainedNodeSlots += num - chunk.nodes.Length;
			chunk.nodes = array;
		}
		EnforceNodeBudget();
	}

	private void EnforceNodeBudget()
	{
		long num = long.MinValue;
		using (timelineLock.EnterScope())
		{
			while (retainedNodeSlots > RetainedNodeBudget)
			{
				ThreadTimeline threadTimeline = null;
				long num2 = long.MaxValue;
				foreach (ThreadTimeline timeline in timelines)
				{
					if (timeline.chunks.Count != 0)
					{
						ZoneChunk zoneChunk = timeline.chunks[0];
						if (zoneChunk != timeline.current && zoneChunk.endNs < num2)
						{
							num2 = zoneChunk.endNs;
							threadTimeline = timeline;
						}
					}
				}
				if (threadTimeline == null)
				{
					break;
				}
				num = System.Math.Max(num, num2);
				retainedNodeSlots -= threadTimeline.chunks[0].nodes.Length;
				RetireChunk(threadTimeline.chunks[0]);
				threadTimeline.chunks.RemoveAt(0);
			}
		}
		if (num != long.MinValue)
		{
			onZonesEvicted?.Invoke(num);
		}
	}

	private void RetireChunk(ZoneChunk chunk)
	{
		if (chunkGraveyard.Count == 256)
		{
			pooledBytes -= ChunkBytes(chunkGraveyard[0]);
			chunkGraveyard.RemoveAt(0);
		}
		chunkGraveyard.Add(chunk);
		pooledBytes += ChunkBytes(chunk);
	}

	public void RecycleSnapshot(FrameProfiler.FrameSnapshot snapshot)
	{
		snapshotGraveyard.Enqueue(snapshot);
	}

	public void ReclaimRetired()
	{
		if (FrameProfiler.ActiveReaders != 0)
		{
			return;
		}
		foreach (ZoneChunk item in chunkGraveyard)
		{
			if (chunkPool.Count == 128)
			{
				pooledBytes -= ChunkBytes(chunkPool[0]);
				chunkPool.RemoveAt(0);
			}
			chunkPool.Add(item);
		}
		chunkGraveyard.Clear();
		FrameProfiler.FrameSnapshot result;
		while (snapshotGraveyard.TryDequeue(out result))
		{
			if (snapshotPool.Count != 64)
			{
				result.Clear();
				snapshotPool.Push(result);
				pooledBytes += SnapshotArrayBytes(result);
			}
		}
	}

	private ZoneChunk? TakePooledChunk(int capacity, long baseNs)
	{
		for (int num = chunkPool.Count - 1; num >= 0; num--)
		{
			ZoneChunk zoneChunk = chunkPool[num];
			if (zoneChunk.nodes.Length == capacity)
			{
				chunkPool.RemoveAt(num);
				pooledBytes -= ChunkBytes(zoneChunk);
				zoneChunk.baseNs = baseNs;
				zoneChunk.endNs = baseNs;
				zoneChunk.openCount = 0;
				Volatile.Write(ref zoneChunk.count, 0);
				if (zoneChunk.allocStats != null)
				{
					Array.Clear(zoneChunk.allocStats);
				}
				return zoneChunk;
			}
		}
		return null;
	}

	private void TrimChunk(ZoneChunk chunk)
	{
		int count = chunk.count;
		int num = ChunkCapacityFor(count);
		if (count > chunk.nodes.Length / 2 || num == chunk.nodes.Length)
		{
			return;
		}
		ZoneNode[] array = new ZoneNode[num];
		Array.Copy(chunk.nodes, array, count);
		FrameProfiler.NodeAllocStat[] array2 = null;
		if (chunk.allocStats != null)
		{
			array2 = new FrameProfiler.NodeAllocStat[array.Length];
			Array.Copy(chunk.allocStats, array2, count);
		}
		using (timelineLock.EnterScope())
		{
			retainedNodeSlots -= chunk.nodes.Length - array.Length;
			chunk.allocStats = array2;
			chunk.nodes = array;
		}
	}

	public void EvictChunksBefore(long ns)
	{
		using (timelineLock.EnterScope())
		{
			for (int num = timelines.Count - 1; num >= 0; num--)
			{
				ThreadTimeline threadTimeline = timelines[num];
				int i;
				for (i = 0; i < threadTimeline.chunks.Count; i++)
				{
					ZoneChunk zoneChunk = threadTimeline.chunks[i];
					if (zoneChunk == threadTimeline.current || zoneChunk.endNs >= ns)
					{
						break;
					}
					retainedNodeSlots -= zoneChunk.nodes.Length;
					RetireChunk(zoneChunk);
				}
				if (i > 0)
				{
					threadTimeline.chunks.RemoveRange(0, i);
				}
				if (threadTimeline.retired && threadTimeline.chunks.Count == 0)
				{
					timelines.RemoveAt(num);
					timelinesByThreadId.Remove(threadTimeline.threadId);
				}
			}
		}
	}

	private void CloseZone(ThreadTimeline thread, long ts)
	{
		ThreadBucket threadBucket = CursorBucket(thread);
		if (thread.depth == 0)
		{
			if (threadBucket != null)
			{
				threadBucket.unbalanced++;
			}
			return;
		}
		int num = --thread.depth;
		ref ThreadTimeline.OpenSlot reference = ref thread.stack[num];
		int node = reference.node;
		if (node < 0)
		{
			return;
		}
		ZoneChunk current = thread.current;
		ref ZoneNode reference2 = ref current.nodes[node];
		long num2 = current.baseNs + reference2.startNs;
		long num3 = System.Math.Max(ts - num2, reference.childTotalNs);
		ProfilerLabelId labelId = ProfilerPathTable.Entries[reference2.pathId].labelId;
		bool flag = FrameProfiler.IsLabelDiscardable(labelId);
		if (flag && num3 < 5000 && node == current.count - 1 && reference.subtreeAllocTotal == 0L && !reference.flushed)
		{
			TrackDiscardableScope(labelId.Value, discarded: true);
			Volatile.Write(ref current.count, node);
			current.openCount--;
			if (current.openCount == 0)
			{
				current.endNs = ts;
			}
			if (threadBucket != null)
			{
				EntryFor(threadBucket, reference2.pathId).calls--;
				threadBucket.nodes--;
			}
			return;
		}
		if (flag)
		{
			TrackDiscardableScope(labelId.Value, discarded: false);
		}
		reference2.endNs = System.Math.Max(reference2.startNs, RelativeNs(ts - current.baseNs));
		reference2.subtreeEnd = current.count;
		current.openCount--;
		if (current.openCount == 0)
		{
			current.endNs = ts;
		}
		if (current.allocStats != null)
		{
			current.allocStats[node].totalBytes = ClampToInt(reference.subtreeAllocTotal);
		}
		long num4 = System.Math.Max(0L, ts - reference.accountedUntil);
		if (threadBucket != null)
		{
			Account(ref EntryFor(threadBucket, reference2.pathId), num4, reference.childNs, reference.subtreeAlloc, reference.selfAlloc);
			threadBucket.endNs = System.Math.Max(threadBucket.endNs, ts);
		}
		if (num > 0)
		{
			ref ThreadTimeline.OpenSlot reference3 = ref thread.stack[num - 1];
			reference3.childNs += num4;
			reference3.childTotalNs += num3;
			reference3.subtreeAlloc += reference.subtreeAlloc;
			reference3.subtreeAllocTotal += reference.subtreeAllocTotal;
		}
		else if (threadBucket != null)
		{
			threadBucket.busyNs += num4;
		}
	}

	private void RecordAlloc(ThreadTimeline thread, int size, ushort typeId)
	{
		PendingMark pendingMark = MarkBySeq(thread.markSeq);
		ThreadBucket threadBucket = ((pendingMark == null) ? null : BucketFor(pendingMark, thread));
		if (threadBucket != null)
		{
			threadBucket.allocCount++;
		}
		ProfilerLabelId labelId = thread.nameLabelId;
		int num = ((thread.depth > 0) ? thread.stack[thread.depth - 1].node : (-1));
		if (num >= 0)
		{
			ref ThreadTimeline.OpenSlot reference = ref thread.stack[thread.depth - 1];
			ZoneChunk current = thread.current;
			ZoneChunk zoneChunk = current;
			if (zoneChunk.allocStats == null)
			{
				zoneChunk.allocStats = new FrameProfiler.NodeAllocStat[current.nodes.Length];
			}
			ref FrameProfiler.NodeAllocStat reference2 = ref current.allocStats[num];
			reference2.selfBytes = ClampToInt((long)reference2.selfBytes + (long)size);
			reference2.count = ClampToInt((long)reference2.count + 1L);
			reference.selfAlloc += size;
			reference.subtreeAlloc += size;
			reference.subtreeAllocTotal += size;
			labelId = ProfilerPathTable.Entries[current.nodes[num].pathId].labelId;
		}
		else if (threadBucket != null)
		{
			threadBucket.rootSelfAllocBytes += size;
			threadBucket.rootSelfAllocCount++;
		}
		if (pendingMark == null)
		{
			return;
		}
		long key = (long)(((ulong)labelId.Value << 32) | (uint)(((thread.threadId & 0xFFFF) << 16) | typeId));
		if (!pendingMark.typeCounterIndices.TryGetValue(key, out var value))
		{
			if (pendingMark.typeCounterCount >= 2048)
			{
				pendingMark.allocOverflowCount++;
				pendingMark.allocOverflowBytes += size;
				return;
			}
			value = pendingMark.typeCounterCount++;
			if (value == pendingMark.typeCounters.Length)
			{
				Array.Resize(ref pendingMark.typeCounters, pendingMark.typeCounters.Length * 2);
			}
			pendingMark.typeCounterIndices[key] = value;
			pendingMark.typeCounters[value] = new FrameProfiler.TypeAllocStat
			{
				type = FrameProfiler.GetAllocType(typeId),
				labelId = labelId,
				threadId = thread.threadId
			};
		}
		ref FrameProfiler.TypeAllocStat reference3 = ref pendingMark.typeCounters[value];
		reference3.count++;
		reference3.bytes += size;
	}

	private void TrackDiscardableScope(int labelId, bool discarded)
	{
		if (!FrameProfiler.AutoMuteTrivialLabels)
		{
			return;
		}
		if (autoMuteDiscarded == null)
		{
			autoMuteDiscarded = new int[65535];
		}
		if (autoMuteKept == null)
		{
			autoMuteKept = new int[65535];
		}
		if (!discarded)
		{
			autoMuteKept[labelId]++;
			return;
		}
		ref int reference = ref autoMuteDiscarded[labelId];
		if (reference >= 0)
		{
			int num = ++reference;
			if (num >= 2000 && autoMuteKept[labelId] * 10 < num)
			{
				autoMuteBatch.Add(labelId);
				autoMuteDiscarded[labelId] = -1073741824;
			}
		}
	}

	private void FlushAutoMutes()
	{
		bool autoMuteTrivialLabels = FrameProfiler.AutoMuteTrivialLabels;
		if (autoMuteTrivialLabels != autoMuteWasEnabled)
		{
			autoMuteWasEnabled = autoMuteTrivialLabels;
			if (autoMuteDiscarded != null)
			{
				Array.Clear(autoMuteDiscarded);
			}
			if (autoMuteKept != null)
			{
				Array.Clear(autoMuteKept);
			}
			autoMuteBatch.Clear();
		}
		if (autoMuteBatch.Count != 0)
		{
			FrameProfiler.AutoMuteLabels(autoMuteBatch);
			autoMuteBatch.Clear();
		}
	}

	public void PublishReady(long nowNs)
	{
		while (marks.Count > 0)
		{
			PendingMark pendingMark = marks[0];
			if (!pendingMark.ended)
			{
				break;
			}
			bool num = marks.Count > 1 && marks[1].ended;
			bool flag = marks.Count > 2 && marks[2].ended;
			bool flag2 = nowNs - pendingMark.endNs > 150000000;
			if ((!num || !AllThreadsPassed(pendingMark)) && !flag && !flag2)
			{
				break;
			}
			long num2 = ((pendingMark.boundaryNs != 0L) ? pendingMark.boundaryNs : pendingMark.endNs);
			using (timelineLock.EnterScope())
			{
				foreach (ThreadTimeline timeline in timelines)
				{
					if (timeline.markSeq >= 0 && timeline.markSeq <= pendingMark.seq)
					{
						Flush(timeline, pendingMark, timeline.isMain ? pendingMark.endNs : num2);
						timeline.markSeq = pendingMark.seq + 1;
						PendingMark pendingMark2 = MarkBySeq(timeline.markSeq);
						if (pendingMark2 != null)
						{
							EnterMark(timeline, pendingMark2);
						}
					}
				}
			}
			FrameProfiler.FrameSnapshot obj = Publish(pendingMark, num2);
			marks.RemoveAt(0);
			ReleaseMark(pendingMark);
			onFramePublished?.Invoke(obj);
		}
		FlushAutoMutes();
	}

	private bool AllThreadsPassed(PendingMark mark)
	{
		using (timelineLock.EnterScope())
		{
			foreach (ThreadTimeline timeline in timelines)
			{
				if (!timeline.retired && timeline.markSeq >= 0 && timeline.markSeq <= mark.seq)
				{
					return false;
				}
			}
		}
		return true;
	}

	private FrameProfiler.FrameSnapshot Publish(PendingMark mark, long boundary)
	{
		FrameProfiler.FrameSnapshot frameSnapshot = ((snapshotPool.Count > 0) ? snapshotPool.Pop() : new FrameProfiler.FrameSnapshot());
		pooledBytes -= SnapshotArrayBytes(frameSnapshot);
		long num = System.Math.Max(0L, mark.endNs - mark.beginNs);
		float pauseMs = 0f;
		long promotedBytes = 0L;
		int num2 = pauseCollector?.Invoke(mark.beginNs, mark.endNs, pauseScratch, out pauseMs, out promotedBytes) ?? 0;
		FrameStatsRecord stats = default(FrameStatsRecord);
		statsLookup?.Invoke(mark.frameNumber, out stats);
		frameSnapshot.valid = true;
		frameSnapshot.frameIndex = mark.frameNumber;
		frameSnapshot.beginTimeNanoseconds = mark.beginNs;
		frameSnapshot.endTimeNanoseconds = mark.endNs;
		frameSnapshot.boundaryTimeNanoseconds = boundary;
		frameSnapshot.tickStart = stats.TickStart;
		frameSnapshot.tickEnd = stats.TickEnd;
		frameSnapshot.tickDelta = System.Math.Max(0, stats.TickEnd - stats.TickStart);
		frameSnapshot.totalMs = (float)num / 1000000f;
		frameSnapshot.pauseCount = num2;
		frameSnapshot.pauseMs = pauseMs;
		FrameProfiler.FrameSnapshot.EnsureCapacity(ref frameSnapshot.pauses, num2);
		pauseScratch.AsSpan(0, num2).CopyTo(frameSnapshot.pauses);
		frameSnapshot.allocatedBytes = stats.AllocatedBytesDelta;
		frameSnapshot.allocationTracked = FrameProfiler.AllocationTracking;
		frameSnapshot.gen0Collections = stats.Gen0;
		frameSnapshot.gen1Collections = stats.Gen1;
		frameSnapshot.gen2Collections = stats.Gen2;
		frameSnapshot.promotedBytes = promotedBytes;
		SnapshotTypeCounters(frameSnapshot, mark);
		int num3 = 0;
		int num4 = 0;
		foreach (ThreadBucket bucket in mark.buckets)
		{
			if (bucket.HasContent)
			{
				num3++;
				num4 += bucket.count + 1;
			}
		}
		FrameProfiler.FrameSnapshot.EnsureCapacity(ref frameSnapshot.sections, num3);
		int num5 = BucketedCapacity(num4, 64, int.MaxValue);
		if (frameSnapshot.pathEntries.Length != num5)
		{
			ReturnPathEntries(frameSnapshot.pathEntries);
			frameSnapshot.pathEntries = TakePathEntries(num5);
		}
		frameSnapshot.publishedNodeCount = num2;
		foreach (ThreadBucket bucket2 in mark.buckets)
		{
			if (bucket2.HasContent)
			{
				EmitSection(frameSnapshot, mark, bucket2, num);
			}
		}
		return frameSnapshot;
	}

	private static void EmitSection(FrameProfiler.FrameSnapshot snapshot, PendingMark mark, ThreadBucket bucket, long frameNs)
	{
		ThreadTimeline thread = bucket.thread;
		bool isMain = thread.isMain;
		long num = (isMain ? System.Math.Max(0L, frameNs - bucket.busyNs) : 0);
		long num2 = (isMain ? frameNs : bucket.busyNs);
		snapshot.overflowedScopes += bucket.overflowed;
		snapshot.unbalancedEnds += bucket.unbalanced;
		snapshot.recordedScopes += bucket.zonesRecorded;
		int pathEntryCount = snapshot.pathEntryCount;
		snapshot.pathEntries[snapshot.pathEntryCount++] = new FrameProfiler.PathEntry
		{
			pathId = thread.rootPathId,
			totalMicroseconds = (float)num2 * 0.001f,
			selfMicroseconds = (float)num * 0.001f,
			calls = 1,
			selfAllocBytes = ClampToInt(bucket.rootSelfAllocBytes)
		};
		ProfilerPathTable.Entry[] entries = ProfilerPathTable.Entries;
		long num3 = bucket.rootSelfAllocBytes;
		for (int i = 0; i < bucket.count; i++)
		{
			ref FrameProfiler.PathEntry reference = ref bucket.entries[i];
			snapshot.pathEntries[snapshot.pathEntryCount++] = reference;
			if ((uint)reference.pathId < (uint)entries.Length && entries[reference.pathId].parentPathId == thread.rootPathId)
			{
				num3 += reference.allocBytes;
			}
		}
		snapshot.pathEntries[pathEntryCount].allocBytes = ClampToInt(num3);
		snapshot.trackedAllocBytes += num3;
		snapshot.allocatedObjectCount = ClampToInt(snapshot.allocatedObjectCount + bucket.allocCount);
		snapshot.publishedNodeCount += bucket.nodes + 1;
		long num4 = ((bucket.startNs == long.MaxValue) ? 0 : System.Math.Max(0L, bucket.startNs - mark.beginNs));
		long endNanoseconds = (isMain ? frameNs : System.Math.Max(num4, bucket.endNs - mark.beginNs));
		snapshot.sections[snapshot.sectionCount++] = new FrameProfiler.ThreadSection
		{
			threadId = thread.threadId,
			nameLabelId = thread.nameLabelId,
			recordedScopes = bucket.zonesRecorded,
			busyNanoseconds = bucket.busyNs,
			startNanoseconds = (isMain ? 0 : num4),
			endNanoseconds = endNanoseconds,
			firstPathEntry = pathEntryCount,
			pathEntryCount = snapshot.pathEntryCount - pathEntryCount
		};
	}

	private static void SnapshotTypeCounters(FrameProfiler.FrameSnapshot snapshot, PendingMark mark)
	{
		int num = (snapshot.topAllocTypeCount = System.Math.Min(256, mark.typeCounterCount));
		snapshot.allocOverflowCount = mark.allocOverflowCount;
		snapshot.allocOverflowBytes = mark.allocOverflowBytes;
		if (mark.typeCounterCount != 0)
		{
			Span<FrameProfiler.TypeAllocStat> span = mark.typeCounters.AsSpan(0, mark.typeCounterCount);
			span.Sort((FrameProfiler.TypeAllocStat lhs, FrameProfiler.TypeAllocStat rhs) => rhs.bytes.CompareTo(lhs.bytes));
			for (int i = num; i < span.Length; i++)
			{
				snapshot.allocOverflowCount += span[i].count;
				snapshot.allocOverflowBytes += span[i].bytes;
			}
			FrameProfiler.FrameSnapshot.EnsureCapacity(ref snapshot.topAllocTypes, num);
			span.Slice(0, num).CopyTo(snapshot.topAllocTypes);
		}
	}

	public void CollectTimelines(List<ThreadTimeline> into)
	{
		into.Clear();
		using (timelineLock.EnterScope())
		{
			into.AddRange(timelines);
		}
	}

	private bool TryGetTimeline(int threadId, [NotNullWhen(true)] out ThreadTimeline? timeline)
	{
		using (timelineLock.EnterScope())
		{
			return timelinesByThreadId.TryGetValue(threadId, out timeline);
		}
	}

	private static bool Overlaps(ZoneChunk chunk, long startNs, long endNs)
	{
		if (chunk.baseNs < endNs)
		{
			return chunk.endNs > startNs;
		}
		return false;
	}

	public void CollectChunks(ThreadTimeline timeline, long startNs, long endNs, List<ZoneChunk> into)
	{
		into.Clear();
		using (timelineLock.EnterScope())
		{
			foreach (ZoneChunk chunk in timeline.chunks)
			{
				if (Overlaps(chunk, startNs, endNs))
				{
					into.Add(chunk);
				}
			}
		}
	}

	public void CollectChunks(FrameProfiler.FrameSnapshot frame, int threadId, long startNs, long endNs, List<ZoneChunk> into)
	{
		RetainedZoneRange[] retainedZones = frame.retainedZones;
		if (retainedZones == null)
		{
			if (TryGetTimeline(threadId, out ThreadTimeline timeline))
			{
				CollectChunks(timeline, startNs, endNs, into);
			}
			else
			{
				into.Clear();
			}
			return;
		}
		into.Clear();
		RetainedZoneRange[] array = retainedZones;
		foreach (RetainedZoneRange retainedZoneRange in array)
		{
			if (retainedZoneRange.threadId != threadId)
			{
				continue;
			}
			ZoneChunk[] chunks = retainedZoneRange.chunks;
			foreach (ZoneChunk zoneChunk in chunks)
			{
				if (Overlaps(zoneChunk, startNs, endNs))
				{
					into.Add(zoneChunk);
				}
			}
		}
	}

	public bool HasChunks(ThreadTimeline timeline, long startNs, long endNs)
	{
		using (timelineLock.EnterScope())
		{
			foreach (ZoneChunk chunk in timeline.chunks)
			{
				if (Overlaps(chunk, startNs, endNs))
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool HasChunks(FrameProfiler.FrameSnapshot frame, int threadId, long startNs, long endNs)
	{
		RetainedZoneRange[] retainedZones = frame.retainedZones;
		if (retainedZones == null)
		{
			if (TryGetTimeline(threadId, out ThreadTimeline timeline))
			{
				return HasChunks(timeline, startNs, endNs);
			}
			return false;
		}
		RetainedZoneRange[] array = retainedZones;
		foreach (RetainedZoneRange retainedZoneRange in array)
		{
			if (retainedZoneRange.threadId != threadId)
			{
				continue;
			}
			ZoneChunk[] chunks = retainedZoneRange.chunks;
			for (int j = 0; j < chunks.Length; j++)
			{
				if (Overlaps(chunks[j], startNs, endNs))
				{
					return true;
				}
			}
		}
		return false;
	}

	public void RetainZones(FrameProfiler.FrameSnapshot frame)
	{
		if (frame.retainedZones != null)
		{
			return;
		}
		using (FrameProfiler.BeginRead())
		{
			RetainedZoneRange[] array = new RetainedZoneRange[frame.sectionCount];
			List<ZoneChunk> list = new List<ZoneChunk>();
			for (int i = 0; i < frame.sectionCount; i++)
			{
				FrameProfiler.ThreadSection threadSection = frame.sections[i];
				frame.SectionZoneRange(i, out var startNs, out var endNs);
				RetainedZoneRange retainedZoneRange = (array[i] = new RetainedZoneRange
				{
					threadId = threadSection.threadId
				});
				if (!TryGetTimeline(threadSection.threadId, out ThreadTimeline timeline))
				{
					continue;
				}
				CollectChunks(timeline, startNs, endNs, list);
				List<ZoneChunk> list2 = new List<ZoneChunk>(list.Count);
				foreach (ZoneChunk item in list)
				{
					ZoneChunk zoneChunk = CopyChunkSlice(item, startNs, endNs);
					if (zoneChunk != null)
					{
						list2.Add(zoneChunk);
					}
				}
				retainedZoneRange.chunks = list2.ToArray();
			}
			frame.retainedZones = array;
		}
	}

	private static ZoneChunk? CopyChunkSlice(ZoneChunk chunk, long begin, long end)
	{
		ZoneNode[] nodes = chunk.nodes;
		int num = System.Math.Min(chunk.Count, nodes.Length);
		int num2 = -1;
		int num3 = -1;
		for (int i = 0; i < num; i++)
		{
			ref ZoneNode reference = ref nodes[i];
			if (chunk.baseNs + reference.StartNanoseconds >= end)
			{
				break;
			}
			if ((reference.IsOpen ? long.MaxValue : (chunk.baseNs + reference.EndNanoseconds)) > begin)
			{
				if (num2 < 0)
				{
					num2 = i;
				}
				num3 = i;
			}
		}
		if (num2 < 0)
		{
			return null;
		}
		int num4 = num3 - num2 + 1;
		ZoneChunk zoneChunk = new ZoneChunk
		{
			baseNs = chunk.baseNs,
			nodes = new ZoneNode[num4],
			count = num4,
			endNs = chunk.endNs
		};
		Array.Copy(nodes, num2, zoneChunk.nodes, 0, num4);
		for (int j = 0; j < num4; j++)
		{
			ref ZoneNode reference2 = ref zoneChunk.nodes[j];
			if (reference2.subtreeEnd > 0)
			{
				reference2.subtreeEnd = System.Math.Min(num4, reference2.subtreeEnd - num2);
			}
		}
		FrameProfiler.NodeAllocStat[] allocStats = chunk.allocStats;
		if (allocStats != null && num2 + num4 <= allocStats.Length)
		{
			zoneChunk.allocStats = new FrameProfiler.NodeAllocStat[num4];
			Array.Copy(allocStats, num2, zoneChunk.allocStats, 0, num4);
		}
		return zoneChunk;
	}

	public long TimelineBytes()
	{
		long num = 0L;
		using (timelineLock.EnterScope())
		{
			foreach (ThreadTimeline timeline in timelines)
			{
				foreach (ZoneChunk chunk in timeline.chunks)
				{
					num += ChunkBytes(chunk);
				}
			}
			return num;
		}
	}

	internal static int ClampToInt(long value)
	{
		return (int)System.Math.Clamp(value, 0L, 2147483647L);
	}
}
