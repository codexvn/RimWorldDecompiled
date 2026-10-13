using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading;
using Brutal.Logging;
using Brutal.Logging.Internal;
using WaylithClr;

namespace Waylith.Profiling;

internal static class ProfilerWorker
{
	private struct PendingPause
	{
		public long StartNs;

		public long SuspendedNs;

		public long EndNs;

		public RuntimePauseReason Reason;

		public int CondemnedGeneration;

		public int TriggeringThreadId;

		public long PromotedBytes;

		public long CollectedBytes;
	}

	private const int MaxPendingPauses = 1024;

	private const float SpikeDeltaThresholdMs = 8f;

	private const float SmoothingAlpha = 0.04f;

	private const int SpikeWarmupFrames = 8;

	private static readonly LogCategory Log = LogCategory.Make("FrameProfiler");

	private static Thread? workerThread;

	private static readonly AutoResetEvent WakeSignal = new AutoResetEvent(initialState: false);

	private static volatile bool shutdownRequested;

	private const int StatsRingSize = 256;

	private static readonly FrameStatsRecord[] StatsRing = new FrameStatsRecord[256];

	private static int statsWrite;

	private static int statsRead;

	private static readonly Lock RingLock = new Lock();

	private static FrameProfiler.FrameSnapshot?[] history = Array.Empty<FrameProfiler.FrameSnapshot>();

	private static readonly FrameProfiler.FrameSnapshot?[] spikes = new FrameProfiler.FrameSnapshot[24];

	private static int historyCount;

	private static int historyNext;

	private static int spikeCount;

	private static int spikeNext;

	private static int spikeEpoch;

	private static int completedFrameCount;

	private static int historyEpoch;

	private const long NodeSlotsPerFrameBudget = 16384L;

	private static readonly Lock LifecycleLock = new Lock();

	private static readonly List<ThreadEventQueue> KnownQueues = new List<ThreadEventQueue>();

	internal static readonly ProfilerFrameAssembler Assembler = CreateAssembler();

	private static long lastDroppedEventsTotal;

	private static long retiredDroppedEvents;

	private static float smoothedFrameMs;

	private static readonly List<FrameProfiler.FrameSnapshot> DropScratch = new List<FrameProfiler.FrameSnapshot>();

	private static int retireSweepCountdown;

	private static readonly PendingPause[] PendingPauses = new PendingPause[1024];

	private static int pendingPauseCount;

	private static readonly SelfProfiler.RuntimePause[] PauseDrainScratch = new SelfProfiler.RuntimePause[256];

	public static int HistoryCount
	{
		get
		{
			using (RingLock.EnterScope())
			{
				return historyCount;
			}
		}
	}

	public static int SpikeCount
	{
		get
		{
			using (RingLock.EnterScope())
			{
				return spikeCount;
			}
		}
	}

	public static int CompletedFrameCount
	{
		get
		{
			using (RingLock.EnterScope())
			{
				return completedFrameCount;
			}
		}
	}

	public static int HistoryEpoch => Volatile.Read(in historyEpoch);

	public static int SpikeEpoch => Volatile.Read(in spikeEpoch);

	public static void PushFrameStats(in FrameStatsRecord record)
	{
		int num = statsWrite;
		StatsRing[num % 256] = record;
		Volatile.Write(ref statsWrite, num + 1);
	}

	public static FrameProfiler.RetainedMemory GetRetainedMemory()
	{
		FrameProfiler.RetainedMemory memory = default(FrameProfiler.RetainedMemory);
		using (RingLock.EnterScope())
		{
			FrameProfiler.FrameSnapshot[] array = history;
			foreach (FrameProfiler.FrameSnapshot frameSnapshot in array)
			{
				if (frameSnapshot != null)
				{
					memory.historyBytes += AccumulateArrayBytes(frameSnapshot, ref memory);
					memory.historySnapshots++;
				}
			}
			array = spikes;
			foreach (FrameProfiler.FrameSnapshot frameSnapshot2 in array)
			{
				if (frameSnapshot2 != null)
				{
					memory.spikeBytes += AccumulateArrayBytes(frameSnapshot2, ref memory);
					memory.spikeSnapshots++;
				}
			}
		}
		memory.nodeSlots = Assembler.RetainedNodeSlots;
		memory.timelineBytes = Assembler.TimelineBytes();
		memory.nodeBytes = memory.nodeSlots * Unsafe.SizeOf<ZoneNode>();
		memory.allocStatBytes = memory.timelineBytes - memory.nodeBytes;
		return memory;
	}

	private static long AccumulateArrayBytes(FrameProfiler.FrameSnapshot snapshot, ref FrameProfiler.RetainedMemory memory)
	{
		long num = 0L;
		RetainedZoneRange[] retainedZones = snapshot.retainedZones;
		if (retainedZones != null)
		{
			RetainedZoneRange[] array = retainedZones;
			for (int i = 0; i < array.Length; i++)
			{
				ZoneChunk[] chunks = array[i].chunks;
				foreach (ZoneChunk chunk in chunks)
				{
					num += ProfilerFrameAssembler.ChunkBytes(chunk);
				}
			}
		}
		long num2 = (long)snapshot.pauses.Length * (long)Unsafe.SizeOf<FrameProfiler.PauseSpan>();
		long num3 = (long)snapshot.topAllocTypes.Length * (long)Unsafe.SizeOf<FrameProfiler.TypeAllocStat>();
		long num4 = (long)snapshot.sections.Length * (long)Unsafe.SizeOf<FrameProfiler.ThreadSection>();
		long num5 = (long)snapshot.pathEntries.Length * (long)Unsafe.SizeOf<FrameProfiler.PathEntry>();
		memory.retainedZoneBytes += num;
		memory.pauseBytes += num2;
		memory.allocTypeBytes += num3;
		memory.sectionBytes += num4;
		memory.pathEntryBytes += num5;
		return num + num2 + num3 + num4 + num5;
	}

	public static void SetHistoryCapacity(int capacity)
	{
		using (RingLock.EnterScope())
		{
			ClearHistoryLocked();
			history = new FrameProfiler.FrameSnapshot[capacity];
		}
		ProfilerFrameAssembler.RetainedNodeBudget = System.Math.Max(4194304L, (long)capacity * 16384L);
	}

	public static void ClearHistory()
	{
		using (RingLock.EnterScope())
		{
			ClearHistoryLocked();
		}
	}

	private static void ClearHistoryLocked()
	{
		Array.Clear(history);
		historyCount = 0;
		historyNext = 0;
		Interlocked.Increment(ref historyEpoch);
	}

	public static void ClearSpikes()
	{
		using (RingLock.EnterScope())
		{
			ClearSpikesLocked();
		}
	}

	private static void ClearSpikesLocked()
	{
		spikeCount = 0;
		spikeNext = 0;
		spikeEpoch++;
		Array.Clear(spikes);
		Interlocked.Increment(ref historyEpoch);
	}

	public static void ResetRings()
	{
		using (RingLock.EnterScope())
		{
			ClearHistoryLocked();
			ClearSpikesLocked();
			completedFrameCount = 0;
		}
		smoothedFrameMs = 0f;
	}

	public static bool TryGetHistorySnapshot(int newestOffset, [NotNullWhen(true)] out FrameProfiler.FrameSnapshot? snapshot)
	{
		using (RingLock.EnterScope())
		{
			snapshot = null;
			if (newestOffset < 0 || newestOffset >= historyCount)
			{
				return false;
			}
			snapshot = history[RingIndex(historyNext, newestOffset, history.Length)];
			return snapshot?.valid ?? false;
		}
	}

	public static int CopyHistoryTotals(int sinceCompleted, Span<float> totalMs, Span<float> allocBytes, out int completed, out int count, out int epoch)
	{
		using (RingLock.EnterScope())
		{
			completed = completedFrameCount;
			count = historyCount;
			epoch = historyEpoch;
			int val = ((sinceCompleted < 0) ? historyCount : System.Math.Clamp(completed - sinceCompleted, 0, historyCount));
			val = System.Math.Min(val, System.Math.Min(totalMs.Length, allocBytes.Length));
			for (int i = 0; i < val; i++)
			{
				FrameProfiler.FrameSnapshot frameSnapshot = history[RingIndex(historyNext, i, history.Length)];
				bool flag = frameSnapshot?.valid ?? false;
				totalMs[i] = (flag ? frameSnapshot.totalMs : float.NaN);
				allocBytes[i] = (flag ? ((float)frameSnapshot.DisplayAllocBytes) : 0f);
			}
			return val;
		}
	}

	public static bool TryGetHistoryByOrdinal(int ordinal, [NotNullWhen(true)] out FrameProfiler.FrameSnapshot? snapshot)
	{
		using (RingLock.EnterScope())
		{
			snapshot = null;
			if (historyCount == 0)
			{
				return false;
			}
			FrameProfiler.FrameSnapshot frameSnapshot = history[RingIndex(historyNext, 0, history.Length)];
			if (frameSnapshot == null)
			{
				return false;
			}
			int num = Volatile.Read(in frameSnapshot.captureOrdinal) - ordinal;
			if (num < 0 || num >= historyCount)
			{
				return false;
			}
			snapshot = history[RingIndex(historyNext, num, history.Length)];
			FrameProfiler.FrameSnapshot frameSnapshot2 = snapshot;
			return frameSnapshot2 != null && frameSnapshot2.valid && Volatile.Read(in snapshot.captureOrdinal) == ordinal;
		}
	}

	public static bool TryGetSpikeSnapshot(int newestOffset, [NotNullWhen(true)] out FrameProfiler.FrameSnapshot? snapshot)
	{
		using (RingLock.EnterScope())
		{
			snapshot = null;
			if (newestOffset < 0 || newestOffset >= spikeCount)
			{
				return false;
			}
			snapshot = spikes[RingIndex(spikeNext, newestOffset, spikes.Length)];
			return snapshot?.valid ?? false;
		}
	}

	private static int RingIndex(int next, int newestOffset, int length)
	{
		int i;
		for (i = next - 1 - newestOffset; i < 0; i += length)
		{
		}
		return i;
	}

	public static void EnsureStarted()
	{
		using (LifecycleLock.EnterScope())
		{
			if (workerThread == null)
			{
				shutdownRequested = false;
				workerThread = new Thread(WorkerLoop)
				{
					Name = "ProfilerWorker",
					IsBackground = true,
					Priority = ThreadPriority.BelowNormal
				};
				workerThread.Start();
				ProfilerWindowAggregator.EnsureStarted();
			}
		}
	}

	public static void Signal()
	{
		WakeSignal.Set();
	}

	public static void Shutdown()
	{
		using (LifecycleLock.EnterScope())
		{
			if (workerThread != null)
			{
				shutdownRequested = true;
				WakeSignal.Set();
				if (!workerThread.Join(1000))
				{
					Log.Warning("Profiler worker did not stop within 1s; an open trace may be left unfinalized.", "Shutdown", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Profiling\\ProfilerWorker.cs", 302);
				}
				workerThread = null;
				ProfilerWindowAggregator.Shutdown();
			}
		}
	}

	private static ProfilerFrameAssembler CreateAssembler()
	{
		return new ProfilerFrameAssembler
		{
			statsLookup = PopFrameStats,
			pauseCollector = CollectFramePauses,
			onFramePublished = PublishSnapshot,
			onZonesEvicted = DropFramesStartingBefore
		};
	}

	private static void DropFramesStartingBefore(long ns)
	{
		DropScratch.Clear();
		using (RingLock.EnterScope())
		{
			while (historyCount > 1)
			{
				int num = RingIndex(historyNext, historyCount - 1, history.Length);
				FrameProfiler.FrameSnapshot frameSnapshot = history[num];
				if (frameSnapshot != null && frameSnapshot.beginTimeNanoseconds >= ns)
				{
					break;
				}
				history[num] = null;
				historyCount--;
				if (frameSnapshot != null)
				{
					DropScratch.Add(frameSnapshot);
				}
			}
		}
		foreach (FrameProfiler.FrameSnapshot item in DropScratch)
		{
			ProfilerWindowAggregator.OnEvicted(item);
		}
		DropScratch.Clear();
	}

	private static void WorkerLoop()
	{
		while (!shutdownRequested)
		{
			WakeSignal.WaitOne(ProfilerCore.Capturing ? 100 : 1000);
			try
			{
				using (FrameProfiler.Scope("ProfilerWorker.Drain"))
				{
					Assembler.ReclaimRetired();
					DrainRuntimePauses();
					ProfilerCore.CollectNewQueues(KnownQueues, KnownQueues.Count);
					for (int i = 0; i < 2; i++)
					{
						foreach (ThreadEventQueue knownQueue in KnownQueues)
						{
							bool flag = knownQueue.ManagedThreadId == ProfilerCore.MainThreadId;
							if (flag == (i == 0))
							{
								DrainQueue(knownQueue, Assembler.GetTimeline(knownQueue.ManagedThreadId, knownQueue.ThreadName, flag));
							}
						}
					}
					RetireExitedQueues();
					Assembler.PublishReady(FrameProfiler.TimestampToNanoseconds(ProfilerClock.GetTimestamp()));
				}
			}
			catch (Exception value)
			{
				LogCategory log = Log;
				bool shouldAppend;
				LogString<Error> message = new LogString<Error>(30, 1, log, out shouldAppend);
				if (shouldAppend)
				{
					message.AppendLiteral("Profiler worker drain failed: ");
					message.AppendFormatted(value);
				}
				log.Error(message, "WorkerLoop", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Profiling\\ProfilerWorker.cs", 383);
			}
		}
	}

	private static void RetireExitedQueues()
	{
		if (--retireSweepCountdown > 0)
		{
			return;
		}
		retireSweepCountdown = 16;
		for (int num = KnownQueues.Count - 1; num >= 0; num--)
		{
			ThreadEventQueue threadEventQueue = KnownQueues[num];
			if (threadEventQueue.OwnerExited && threadEventQueue.IsDrained)
			{
				retiredDroppedEvents += Volatile.Read(in threadEventQueue.DroppedEvents);
				ProfilerCore.UnregisterQueue(threadEventQueue);
				EventChunkPool.Release(threadEventQueue.ReadChunk);
				KnownQueues.RemoveAt(num);
				Assembler.RetireTimeline(threadEventQueue.ManagedThreadId);
			}
		}
	}

	private unsafe static void DrainQueue(ThreadEventQueue queue, ThreadTimeline timeline)
	{
		long nowNs = FrameProfiler.TimestampToNanoseconds(ProfilerClock.GetTimestamp());
		while (true)
		{
			EventChunk readChunk = queue.ReadChunk;
			int count = readChunk.Count;
			while (queue.ReadIndex < count)
			{
				ref ProfilerEvent reference = ref readChunk.Slots[queue.ReadIndex];
				if (reference.Type == ProfilerEventType.Reset)
				{
					HandleReset();
				}
				else if (!Assembler.ProcessEvent(timeline, in reference, nowNs))
				{
					return;
				}
				queue.ReadIndex++;
			}
			if (count < 8192)
			{
				break;
			}
			EventChunk eventChunk = Volatile.Read(in readChunk.Next);
			if (eventChunk == null)
			{
				break;
			}
			queue.ReadChunk = eventChunk;
			queue.ReadIndex = 0;
			EventChunkPool.Release(readChunk);
		}
	}

	private static void HandleReset()
	{
		Assembler.Reset();
		pendingPauseCount = 0;
		statsRead = Volatile.Read(in statsWrite);
	}

	private static bool PopFrameStats(int frameNumber, out FrameStatsRecord stats)
	{
		int num = Volatile.Read(in statsWrite);
		while (statsRead < num)
		{
			FrameStatsRecord frameStatsRecord = StatsRing[statsRead % 256];
			if (frameStatsRecord.FrameNumber > frameNumber)
			{
				break;
			}
			statsRead++;
			if (frameStatsRecord.FrameNumber == frameNumber)
			{
				stats = frameStatsRecord;
				return true;
			}
		}
		stats = new FrameStatsRecord
		{
			FrameNumber = frameNumber
		};
		return false;
	}

	private static void PublishSnapshot(FrameProfiler.FrameSnapshot snapshot)
	{
		long num = retiredDroppedEvents;
		foreach (ThreadEventQueue knownQueue in KnownQueues)
		{
			num += Volatile.Read(in knownQueue.DroppedEvents);
		}
		snapshot.droppedEvents = System.Math.Max(0L, num - lastDroppedEventsTotal);
		lastDroppedEventsTotal = num;
		float totalMs = snapshot.totalMs;
		bool num2 = completedFrameCount != 0;
		float num3 = (num2 ? smoothedFrameMs : totalMs);
		bool flag = completedFrameCount >= 8 && totalMs >= 25f && totalMs - num3 >= 8f && totalMs >= num3 * 1.75f;
		smoothedFrameMs = (num2 ? (smoothedFrameMs + (totalMs - smoothedFrameMs) * 0.04f) : totalMs);
		snapshot.isSpike = flag;
		if (flag)
		{
			Assembler.RetainZones(snapshot);
		}
		Volatile.Write(ref snapshot.holds, (!flag) ? 1 : 2);
		FrameProfiler.FrameSnapshot frameSnapshot = null;
		long num4 = long.MinValue;
		FrameProfiler.FrameSnapshot frameSnapshot2;
		using (RingLock.EnterScope())
		{
			snapshot.captureOrdinal = completedFrameCount + 1;
			completedFrameCount++;
			if (history.Length == 0)
			{
				snapshot.Release();
				if (flag)
				{
					snapshot.Release();
				}
				return;
			}
			frameSnapshot2 = history[historyNext];
			history[historyNext] = snapshot;
			if (++historyNext == history.Length)
			{
				historyNext = 0;
			}
			if (historyCount < history.Length)
			{
				historyCount++;
			}
			if (flag)
			{
				frameSnapshot = spikes[spikeNext];
				spikes[spikeNext] = snapshot;
				spikeNext = (spikeNext + 1) % spikes.Length;
				spikeEpoch++;
				if (spikeCount < spikes.Length)
				{
					spikeCount++;
				}
			}
			FrameProfiler.FrameSnapshot frameSnapshot3 = history[RingIndex(historyNext, historyCount - 1, history.Length)];
			if (frameSnapshot3 != null)
			{
				num4 = frameSnapshot3.beginTimeNanoseconds;
			}
		}
		if (num4 != long.MinValue)
		{
			Assembler.EvictChunksBefore(num4);
		}
		frameSnapshot?.Release();
		if (frameSnapshot2 != null)
		{
			ProfilerWindowAggregator.OnEvicted(frameSnapshot2);
		}
		else
		{
			ProfilerWindowAggregator.Signal();
		}
	}

	private static void DrainRuntimePauses()
	{
		int num = SelfProfiler.DrainRuntimePauses(PauseDrainScratch);
		if (num == 0)
		{
			return;
		}
		(long, long) pairing = ProfilerClock.SampleStopwatchPairing();
		for (int i = 0; i < num; i++)
		{
			if (pendingPauseCount >= 1024)
			{
				break;
			}
			SelfProfiler.RuntimePause runtimePause = PauseDrainScratch[i];
			PendingPauses[pendingPauseCount++] = new PendingPause
			{
				StartNs = FrameProfiler.TimestampToNanoseconds(ProfilerClock.FromStopwatchTimestamp(runtimePause.StartTimestamp, pairing)),
				SuspendedNs = ((runtimePause.ThreadsSuspendedTimestamp == 0L) ? 0 : FrameProfiler.TimestampToNanoseconds(ProfilerClock.FromStopwatchTimestamp(runtimePause.ThreadsSuspendedTimestamp, pairing))),
				EndNs = FrameProfiler.TimestampToNanoseconds(ProfilerClock.FromStopwatchTimestamp(runtimePause.EndTimestamp, pairing)),
				Reason = (RuntimePauseReason)runtimePause.Reason,
				CondemnedGeneration = runtimePause.CondemnedGeneration,
				TriggeringThreadId = runtimePause.TriggeringThreadId,
				PromotedBytes = runtimePause.PromotedBytes,
				CollectedBytes = runtimePause.CollectedBytes
			};
		}
	}

	private static int CollectFramePauses(long frameBeginNs, long frameEndNs, FrameProfiler.PauseSpan[] into, out float pauseMs, out long promotedBytes)
	{
		pauseMs = 0f;
		promotedBytes = 0L;
		int num = 0;
		for (int i = 0; i < pendingPauseCount; i++)
		{
			if (num >= into.Length)
			{
				break;
			}
			ref PendingPause reference = ref PendingPauses[i];
			if (reference.EndNs > frameBeginNs && reference.StartNs < frameEndNs)
			{
				int num2 = RelativeMicroseconds(reference.StartNs, frameBeginNs);
				int num3 = System.Math.Max(num2, RelativeMicroseconds(System.Math.Min(frameEndNs, reference.EndNs), frameBeginNs));
				int suspendedMicroseconds = ((reference.SuspendedNs == 0L) ? num3 : System.Math.Clamp(RelativeMicroseconds(reference.SuspendedNs, frameBeginNs), num2, num3));
				into[num++] = new FrameProfiler.PauseSpan
				{
					startMicroseconds = num2,
					suspendedMicroseconds = suspendedMicroseconds,
					endMicroseconds = num3,
					reason = reference.Reason,
					condemnedGeneration = reference.CondemnedGeneration,
					triggeringThreadId = reference.TriggeringThreadId,
					promotedBytes = reference.PromotedBytes,
					collectedBytes = reference.CollectedBytes
				};
				pauseMs += (float)(num3 - num2) / 1000f;
				promotedBytes += reference.PromotedBytes;
			}
		}
		int num4 = 0;
		for (int j = 0; j < pendingPauseCount; j++)
		{
			if (PendingPauses[j].EndNs > frameEndNs)
			{
				PendingPauses[num4++] = PendingPauses[j];
			}
		}
		pendingPauseCount = num4;
		return num;
	}

	private static int RelativeMicroseconds(long ns, long originNs)
	{
		return ProfilerFrameAssembler.ClampToInt((ns - originNs) / 1000);
	}
}
