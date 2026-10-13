using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Brutal.Logging;
using Brutal.Logging.Internal;

namespace Waylith.Profiling;

public static class ProfilerWindowAggregator
{
	private struct WalkEntry
	{
		public int subtreeEnd;

		public int target;

		public double clippedSpan;

		public double childClippedSpan;
	}

	public const int BaselineWindowFrames = 128;

	private const int BaselineRecomputeIntervalFrames = 30;

	private static readonly LogCategory Log = LogCategory.Make("FrameProfiler");

	public static readonly Lock ResultLock = new Lock();

	private static ProfilerWindowRequest? pendingRequest;

	private static ProfilerWindowResult current = new ProfilerWindowResult();

	private static ProfilerWindowResult spare = new ProfilerWindowResult();

	private static int framesSinceBaseline;

	private static readonly ConcurrentQueue<FrameProfiler.FrameSnapshot> Evicted = new ConcurrentQueue<FrameProfiler.FrameSnapshot>();

	private static readonly AutoResetEvent WakeSignal = new AutoResetEvent(initialState: false);

	private static Thread? thread;

	private static volatile bool shutdownRequested;

	private static readonly List<FrameProfiler.FrameSnapshot> EvictedScratch = new List<FrameProfiler.FrameSnapshot>();

	private static WalkEntry[] walkStack = new WalkEntry[64];

	private static int walkDepth;

	private static readonly List<ZoneChunk> ChunkScratch = new List<ZoneChunk>();

	private static int[] baselineMarks = Array.Empty<int>();

	private static int[] baselineSlots = Array.Empty<int>();

	private static int baselineMark;

	private static float[] baselineTotals = Array.Empty<float>();

	private static float[] baselineSelfs = Array.Empty<float>();

	private static int[] baselinePaths = Array.Empty<int>();

	private static int[] baselineSampleCounts = Array.Empty<int>();

	private static int[] baselineSampleStarts = Array.Empty<int>();

	private static float[] baselineTotalScratch = Array.Empty<float>();

	private static float[] baselineSelfScratch = Array.Empty<float>();

	private static float[] medianTotalScratch = Array.Empty<float>();

	private static float[] medianSelfScratch = Array.Empty<float>();

	public static ProfilerWindowResult Current => current;

	public static long EstimateBytes()
	{
		return Estimate(current) + Estimate(spare);
		static long Estimate(ProfilerWindowResult result)
		{
			return (long)result.byPathId.Length * (long)Unsafe.SizeOf<PathAccumulator>() + result.listed.Length + (long)result.touchedPathIds.Length * 4L + ((long)result.baselineTotalByPathId.Length + (long)result.baselineSelfByPathId.Length) * 4;
		}
	}

	public static void Submit(ProfilerWindowRequest request)
	{
		Volatile.Write(ref pendingRequest, request);
		WakeSignal.Set();
	}

	internal static void Signal()
	{
		WakeSignal.Set();
	}

	internal static void OnEvicted(FrameProfiler.FrameSnapshot snapshot)
	{
		Evicted.Enqueue(snapshot);
		WakeSignal.Set();
	}

	internal static void EnsureStarted()
	{
		if (thread == null)
		{
			shutdownRequested = false;
			thread = new Thread(Loop)
			{
				Name = "ProfilerAggregator",
				IsBackground = true,
				Priority = ThreadPriority.BelowNormal
			};
			thread.Start();
		}
	}

	internal static void Shutdown()
	{
		if (thread != null)
		{
			shutdownRequested = true;
			WakeSignal.Set();
			thread.Join(1000);
			thread = null;
		}
	}

	private static void Loop()
	{
		while (!shutdownRequested)
		{
			WakeSignal.WaitOne(100);
			try
			{
				using (FrameProfiler.BeginRead())
				{
					DrainEvicted();
					ProfilerWindowRequest profilerWindowRequest = Volatile.Read(in pendingRequest);
					if (profilerWindowRequest != null)
					{
						Process(profilerWindowRequest);
					}
				}
			}
			catch (Exception value)
			{
				LogCategory log = Log;
				bool shouldAppend;
				LogString<Error> message = new LogString<Error>(28, 1, log, out shouldAppend);
				if (shouldAppend)
				{
					message.AppendLiteral("Profiler aggregator failed: ");
					message.AppendFormatted(value);
				}
				log.Error(message, "Loop", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Profiling\\ProfilerWindowAggregator.cs", 224);
			}
		}
		DrainEvicted();
	}

	private static void DrainEvicted()
	{
		if (Evicted.IsEmpty)
		{
			return;
		}
		using (ResultLock.EnterScope())
		{
			ProfilerWindowResult profilerWindowResult = current;
			ProfilerWindowRequest request = profilerWindowResult.request;
			bool flag = request != null && !request.spikes && profilerWindowResult.epoch == ProfilerWorker.HistoryEpoch;
			FrameProfiler.FrameSnapshot result;
			while (Evicted.TryDequeue(out result))
			{
				EvictedScratch.Add(result);
				int num = Volatile.Read(in result.captureOrdinal);
				if (flag && num >= profilerWindowResult.oldestOrdinal && num <= profilerWindowResult.newestOrdinal)
				{
					Accumulate(profilerWindowResult, result, -1);
					profilerWindowResult.oldestOrdinal = num + 1;
				}
			}
		}
		foreach (FrameProfiler.FrameSnapshot item in EvictedScratch)
		{
			item.Release();
		}
		EvictedScratch.Clear();
	}

	private static void Process(ProfilerWindowRequest request)
	{
		ProfilerWindowResult profilerWindowResult = current;
		int num = (request.spikes ? ProfilerWorker.SpikeEpoch : ProfilerWorker.HistoryEpoch);
		if (profilerWindowResult.request == null || profilerWindowResult.torn || profilerWindowResult.epoch != num || !request.SameWindowAs(profilerWindowResult.request))
		{
			Rebuild(spare, request, num);
			ComputeBaseline(spare, request);
			using (ResultLock.EnterScope())
			{
				ProfilerWindowResult profilerWindowResult2 = spare;
				ProfilerWindowResult profilerWindowResult3 = current;
				current = profilerWindowResult2;
				spare = profilerWindowResult3;
			}
			framesSinceBaseline = 0;
		}
		else
		{
			framesSinceBaseline += (request.spikes ? 1 : Advance(profilerWindowResult, request));
			if (framesSinceBaseline >= 30)
			{
				framesSinceBaseline = 0;
				ComputeBaseline(profilerWindowResult, request);
			}
		}
	}

	private static void Rebuild(ProfilerWindowResult result, ProfilerWindowRequest request, int epoch)
	{
		result.Reset(request, ProfilerPathTable.Count);
		result.epoch = epoch;
		FrameProfiler.FrameSnapshot snapshot2;
		if (request.spikes)
		{
			int spikeCount = ProfilerWorker.SpikeCount;
			for (int i = 0; i < spikeCount; i++)
			{
				if (!ProfilerWorker.TryGetSpikeSnapshot(i, out FrameProfiler.FrameSnapshot snapshot))
				{
					continue;
				}
				int num = Volatile.Read(in snapshot.captureOrdinal);
				bool num2;
				if (!request.followNewest)
				{
					if (num < request.oldestOrdinal)
					{
						continue;
					}
					num2 = num <= request.newestOrdinal;
				}
				else
				{
					num2 = i == 0;
				}
				if (num2)
				{
					Include(result, snapshot, num);
				}
			}
		}
		else if (ProfilerWorker.TryGetHistorySnapshot(0, out snapshot2))
		{
			int num3 = Volatile.Read(in snapshot2.captureOrdinal);
			int historyCount = ProfilerWorker.HistoryCount;
			int num4 = (request.followNewest ? num3 : System.Math.Min(request.newestOrdinal, num3));
			int num5 = (request.followNewest ? (num3 - historyCount + 1) : System.Math.Max(request.oldestOrdinal, num3 - historyCount + 1));
			for (int num6 = num4; num6 >= num5; num6--)
			{
				if (ProfilerWorker.TryGetHistoryByOrdinal(num6, out FrameProfiler.FrameSnapshot snapshot3))
				{
					Include(result, snapshot3, num6);
				}
			}
		}
		if (result.frames == 0)
		{
			FrameProfiler.FrameSnapshot fallbackSnapshot = request.fallbackSnapshot;
			if (fallbackSnapshot != null && fallbackSnapshot.valid)
			{
				Include(result, fallbackSnapshot, Volatile.Read(in fallbackSnapshot.captureOrdinal));
			}
		}
	}

	private static int Advance(ProfilerWindowResult result, ProfilerWindowRequest request)
	{
		if (!ProfilerWorker.TryGetHistorySnapshot(0, out FrameProfiler.FrameSnapshot snapshot))
		{
			return 0;
		}
		int num = Volatile.Read(in snapshot.captureOrdinal);
		int num2 = (request.followNewest ? num : System.Math.Min(request.newestOrdinal, num));
		if (num2 <= result.newestOrdinal)
		{
			return 0;
		}
		int num3 = 0;
		using (ResultLock.EnterScope())
		{
			result.EnsureCapacity(ProfilerPathTable.Count);
			for (int i = result.newestOrdinal + 1; i <= num2; i++)
			{
				if (ProfilerWorker.TryGetHistoryByOrdinal(i, out FrameProfiler.FrameSnapshot snapshot2))
				{
					Include(result, snapshot2, i);
					num3++;
				}
			}
			return num3;
		}
	}

	private static void Include(ProfilerWindowResult result, FrameProfiler.FrameSnapshot snapshot, int ordinal)
	{
		if (ordinal < 0)
		{
			return;
		}
		if (Volatile.Read(in snapshot.captureOrdinal) != ordinal)
		{
			result.torn = true;
			return;
		}
		Accumulate(result, snapshot, 1);
		if (result.newestOrdinal < 0 || ordinal > result.newestOrdinal)
		{
			result.newestOrdinal = ordinal;
		}
		if (result.oldestOrdinal < 0 || ordinal < result.oldestOrdinal)
		{
			result.oldestOrdinal = ordinal;
		}
	}

	private static void Accumulate(ProfilerWindowResult result, FrameProfiler.FrameSnapshot snapshot, int sign)
	{
		ProfilerWindowRequest request = result.request;
		int num = Volatile.Read(in snapshot.captureOrdinal);
		if (num < 0)
		{
			return;
		}
		result.frames += sign;
		bool flag = false;
		if (request.ClipActive)
		{
			if (request.clipStartNs <= snapshot.beginTimeNanoseconds && request.clipEndNs >= snapshot.endTimeNanoseconds)
			{
				result.wholeFrames += sign;
			}
			flag = request.clipStartNs > snapshot.beginTimeNanoseconds || request.clipEndNs < snapshot.beginTimeNanoseconds + snapshot.nodesEndNanoseconds;
		}
		int count = ProfilerPathTable.Count;
		ProfilerPathTable.Entry[] entries = ProfilerPathTable.Entries;
		for (int i = 0; i < snapshot.sectionCount; i++)
		{
			if (request.IncludesThread(snapshot.sections[i].threadId))
			{
				if (flag)
				{
					AccumulateClippedSection(result, snapshot, i, sign, entries, count);
				}
				else
				{
					AccumulateSection(result, snapshot, i, sign, entries, count);
				}
			}
		}
		if (Volatile.Read(in snapshot.captureOrdinal) != num)
		{
			result.torn = true;
		}
	}

	private static int TargetPathOf(int pathId, bool fold, ProfilerPathTable.Entry[] table, int tableCount)
	{
		if ((uint)pathId >= (uint)tableCount)
		{
			return -1;
		}
		if (!fold)
		{
			return pathId;
		}
		return table[pathId].foldedPathId;
	}

	private static bool FoldsIntoParent(int pathId, ProfilerPathTable.Entry[] table)
	{
		ref ProfilerPathTable.Entry reference = ref table[pathId];
		if (reference.parentPathId > 0)
		{
			return table[reference.parentPathId].labelId == reference.labelId;
		}
		return false;
	}

	private static void AccumulateSection(ProfilerWindowResult result, FrameProfiler.FrameSnapshot snapshot, int sectionIndex, int sign, ProfilerPathTable.Entry[] table, int tableCount)
	{
		bool foldRecursion = result.request.foldRecursion;
		FrameProfiler.ThreadSection threadSection = snapshot.sections[sectionIndex];
		FrameProfiler.PathEntry[] pathEntries = snapshot.pathEntries;
		int num = threadSection.firstPathEntry + threadSection.pathEntryCount;
		for (int i = threadSection.firstPathEntry; i < num; i++)
		{
			ref FrameProfiler.PathEntry reference = ref pathEntries[i];
			int num2 = TargetPathOf(reference.pathId, foldRecursion, table, tableCount);
			if (num2 >= 0)
			{
				ref PathAccumulator reference2 = ref result.Touch(num2);
				reference2.entries += sign;
				reference2.selfMicroseconds += (double)sign * (double)reference.selfMicroseconds;
				reference2.calls += sign * reference.calls;
				reference2.selfAllocBytes += (long)sign * (long)reference.selfAllocBytes;
				if (!foldRecursion || !FoldsIntoParent(reference.pathId, table))
				{
					reference2.totalMicroseconds += (double)sign * (double)reference.totalMicroseconds;
					reference2.allocBytes += (long)sign * (long)reference.allocBytes;
				}
			}
		}
	}

	private static float ClipFactor(ProfilerWindowRequest request, long startNs, long endNs, out bool beginsInside)
	{
		long num = System.Math.Min(endNs, request.clipEndNs) - System.Math.Max(startNs, request.clipStartNs);
		if (num <= 0)
		{
			beginsInside = false;
			return 0f;
		}
		beginsInside = startNs >= request.clipStartNs && startNs < request.clipEndNs;
		long num2 = endNs - startNs;
		if (num2 > 0)
		{
			return System.Math.Min(1f, (float)num / (float)num2);
		}
		return 1f;
	}

	private static void PushWalk(in WalkEntry entry)
	{
		if (walkDepth == walkStack.Length)
		{
			Array.Resize(ref walkStack, walkDepth * 2);
		}
		walkStack[walkDepth++] = entry;
	}

	private static void AccumulateClippedSection(ProfilerWindowResult result, FrameProfiler.FrameSnapshot snapshot, int sectionIndex, int sign, ProfilerPathTable.Entry[] table, int tableCount)
	{
		ProfilerWindowRequest request = result.request;
		bool foldRecursion = request.foldRecursion;
		FrameProfiler.ThreadSection threadSection = snapshot.sections[sectionIndex];
		if (threadSection.pathEntryCount == 0)
		{
			return;
		}
		snapshot.SectionZoneRange(sectionIndex, out var startNs, out var endNs);
		ref FrameProfiler.PathEntry reference = ref snapshot.pathEntries[threadSection.firstPathEntry];
		int num = TargetPathOf(reference.pathId, foldRecursion, table, tableCount);
		if (num < 0)
		{
			return;
		}
		long num2 = ((sectionIndex == 0) ? startNs : (startNs + threadSection.startNanoseconds));
		long endNs2 = ((sectionIndex == 0) ? endNs : System.Math.Max(num2, startNs + threadSection.endNanoseconds));
		bool beginsInside;
		float num3 = ClipFactor(request, num2, endNs2, out beginsInside);
		ref PathAccumulator reference2 = ref result.Touch(num);
		reference2.entries += sign;
		reference2.selfMicroseconds += (double)sign * (double)(reference.selfMicroseconds * num3);
		if (beginsInside)
		{
			reference2.selfAllocBytes += (long)sign * (long)reference.selfAllocBytes;
		}
		ProfilerWorker.Assembler.CollectChunks(snapshot, threadSection.threadId, startNs, endNs, ChunkScratch);
		foreach (ZoneChunk item in ChunkScratch)
		{
			ZoneNode[] nodes = item.nodes;
			int num4 = System.Math.Min(item.Count, nodes.Length);
			FrameProfiler.NodeAllocStat[] allocStats = item.allocStats;
			walkDepth = 0;
			WalkEntry entry = default(WalkEntry);
			entry.subtreeEnd = int.MaxValue;
			entry.target = num;
			PushWalk(in entry);
			int num5 = 0;
			while (num5 < num4)
			{
				while (walkDepth > 1 && num5 >= walkStack[walkDepth - 1].subtreeEnd)
				{
					PopWalk(result, sign);
				}
				ref ZoneNode reference3 = ref nodes[num5];
				long num6 = item.baseNs + reference3.StartNanoseconds;
				int num7;
				long num8;
				if (!reference3.IsOpen)
				{
					num7 = ((reference3.subtreeEnd > num5) ? 1 : 0);
					if (num7 != 0)
					{
						num8 = item.baseNs + reference3.EndNanoseconds;
						goto IL_01d5;
					}
				}
				else
				{
					num7 = 0;
				}
				num8 = endNs;
				goto IL_01d5;
				IL_01d5:
				long num9 = num8;
				int num10 = ((num7 != 0) ? reference3.subtreeEnd : num4);
				if (num6 >= endNs || num9 <= startNs)
				{
					num5 = num10;
					continue;
				}
				long num11 = System.Math.Max(num6, startNs);
				long num12 = System.Math.Max(num11, System.Math.Min(num9, endNs));
				int num13 = TargetPathOf(reference3.pathId, foldRecursion, table, tableCount);
				if (num13 < 0)
				{
					num5 = num10;
					continue;
				}
				bool beginsInside2;
				float num14 = ClipFactor(request, num11, num12, out beginsInside2);
				bool flag = num13 == walkStack[walkDepth - 1].target;
				if (!flag && num14 <= 0f)
				{
					num5 = num10;
					continue;
				}
				ref PathAccumulator reference4 = ref result.Touch(num13);
				if (!flag)
				{
					reference4.entries += sign;
				}
				if (beginsInside2)
				{
					reference4.calls += sign;
					if (allocStats != null && num5 < allocStats.Length)
					{
						FrameProfiler.NodeAllocStat nodeAllocStat = allocStats[num5];
						int num15 = ((nodeAllocStat.totalBytes > 0) ? nodeAllocStat.totalBytes : nodeAllocStat.selfBytes);
						if (!flag)
						{
							reference4.allocBytes += (long)sign * (long)num15;
						}
						reference4.selfAllocBytes += (long)sign * (long)nodeAllocStat.selfBytes;
					}
				}
				entry = default(WalkEntry);
				entry.subtreeEnd = num10;
				entry.target = num13;
				entry.clippedSpan = (double)(num12 - num11) * (double)num14;
				PushWalk(in entry);
				num5++;
			}
			while (walkDepth > 1)
			{
				PopWalk(result, sign);
			}
		}
	}

	private static void PopWalk(ProfilerWindowResult result, int sign)
	{
		ref WalkEntry reference = ref walkStack[--walkDepth];
		double num = System.Math.Max(0.0, reference.clippedSpan - reference.childClippedSpan) * 0.001;
		result.byPathId[reference.target].selfMicroseconds += (double)sign * num;
		ref WalkEntry reference2 = ref walkStack[walkDepth - 1];
		reference2.childClippedSpan += reference.clippedSpan;
		if (reference.target != reference2.target)
		{
			result.byPathId[reference.target].totalMicroseconds += (double)sign * reference.clippedSpan * 0.001;
		}
	}

	private static void ComputeBaseline(ProfilerWindowResult result, ProfilerWindowRequest request)
	{
		int count = ProfilerPathTable.Count;
		ProfilerPathTable.Entry[] entries = ProfilerPathTable.Entries;
		if (baselineMarks.Length < count)
		{
			int newSize = System.Math.Max(count, baselineMarks.Length * 2);
			Array.Resize(ref baselineMarks, newSize);
			Array.Resize(ref baselineSlots, newSize);
			Array.Resize(ref baselineSampleCounts, newSize);
			Array.Resize(ref baselineSampleStarts, newSize);
		}
		int num = System.Math.Min(128, ProfilerWorker.HistoryCount);
		int num2 = 0;
		for (int i = 0; i < num; i++)
		{
			if (!ProfilerWorker.TryGetHistorySnapshot(i, out FrameProfiler.FrameSnapshot snapshot))
			{
				continue;
			}
			int num3 = num2;
			int num4 = ++baselineMark;
			for (int j = 0; j < snapshot.sectionCount; j++)
			{
				if (!request.IncludesThread(snapshot.sections[j].threadId))
				{
					continue;
				}
				FrameProfiler.ThreadSection threadSection = snapshot.sections[j];
				int num5 = threadSection.firstPathEntry + threadSection.pathEntryCount;
				for (int k = threadSection.firstPathEntry; k < num5; k++)
				{
					ref FrameProfiler.PathEntry reference = ref snapshot.pathEntries[k];
					int num6 = TargetPathOf(reference.pathId, request.foldRecursion, entries, count);
					if (num6 < 0)
					{
						continue;
					}
					int num7;
					if (baselineMarks[num6] == num4)
					{
						num7 = baselineSlots[num6];
					}
					else
					{
						baselineMarks[num6] = num4;
						num7 = (baselineSlots[num6] = num2++);
						if (num2 > baselinePaths.Length)
						{
							int newSize2 = System.Math.Max(1024, baselinePaths.Length * 2);
							Array.Resize(ref baselinePaths, newSize2);
							Array.Resize(ref baselineTotals, newSize2);
							Array.Resize(ref baselineSelfs, newSize2);
						}
						baselinePaths[num7] = num6;
						baselineTotals[num7] = 0f;
						baselineSelfs[num7] = 0f;
					}
					baselineSelfs[num7] += reference.selfMicroseconds;
					if (!request.foldRecursion || !FoldsIntoParent(reference.pathId, entries))
					{
						baselineTotals[num7] += reference.totalMicroseconds;
					}
				}
			}
			if (Volatile.Read(in snapshot.captureOrdinal) < 0)
			{
				num2 = num3;
			}
		}
		Array.Clear(baselineSampleCounts, 0, count);
		for (int l = 0; l < num2; l++)
		{
			baselineSampleCounts[baselinePaths[l]]++;
		}
		int num8 = 0;
		for (int m = 0; m < count; m++)
		{
			baselineSampleStarts[m] = num8;
			num8 += baselineSampleCounts[m];
		}
		if (baselineTotalScratch.Length < num2)
		{
			baselineTotalScratch = new float[System.Math.Max(num2, baselineTotalScratch.Length * 2)];
			baselineSelfScratch = new float[baselineTotalScratch.Length];
		}
		Array.Clear(baselineSampleCounts, 0, count);
		for (int n = 0; n < num2; n++)
		{
			int num9 = baselinePaths[n];
			int num10 = baselineSampleStarts[num9] + baselineSampleCounts[num9]++;
			baselineTotalScratch[num10] = baselineTotals[n];
			baselineSelfScratch[num10] = baselineSelfs[n];
		}
		if (medianTotalScratch.Length < count)
		{
			medianTotalScratch = new float[System.Math.Max(count, medianTotalScratch.Length * 2)];
			medianSelfScratch = new float[medianTotalScratch.Length];
		}
		float[] array = medianTotalScratch;
		float[] array2 = medianSelfScratch;
		for (int num11 = 0; num11 < count; num11++)
		{
			int num12 = baselineSampleCounts[num11];
			if (num12 == 0)
			{
				array[num11] = float.NaN;
				array2[num11] = float.NaN;
				continue;
			}
			int num13 = baselineSampleStarts[num11];
			Array.Sort(baselineTotalScratch, num13, num12);
			Array.Sort(baselineSelfScratch, num13, num12);
			array[num11] = baselineTotalScratch[num13 + num12 / 2];
			array2[num11] = baselineSelfScratch[num13 + num12 / 2];
		}
		using (ResultLock.EnterScope())
		{
			ProfilerWindowResult profilerWindowResult = result;
			float[] baselineTotalByPathId = array;
			float[] baselineTotalByPathId2 = result.baselineTotalByPathId;
			profilerWindowResult.baselineTotalByPathId = baselineTotalByPathId;
			medianTotalScratch = baselineTotalByPathId2;
			profilerWindowResult = result;
			baselineTotalByPathId = array2;
			float[] baselineSelfByPathId = result.baselineSelfByPathId;
			profilerWindowResult.baselineSelfByPathId = baselineTotalByPathId;
			medianSelfScratch = baselineSelfByPathId;
			result.hasBaseline = true;
		}
	}
}
