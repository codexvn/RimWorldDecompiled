using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Waylith;
using Waylith.Profiling;

namespace Verse.ProfilerUI;

public sealed class ProfilerSession
{
	private const int StatsRecomputeIntervalFrames = 10;

	private const float RefreshBudgetShare = 0.1f;

	private const int MaxRefreshIntervalFrames = 60;

	private readonly ProfilerFrameStore.FramePin historyPin = new ProfilerFrameStore.FramePin(ProfilerFrameSource.History);

	private readonly ProfilerFrameStore.FramePin spikePin = new ProfilerFrameStore.FramePin(ProfilerFrameSource.Spikes);

	private int selectionAnchorOrdinal = -1;

	private int selectionOldestOrdinal = -1;

	private long manualRangeStartNs = -1L;

	private long manualRangeEndNs = -1L;

	private int statsCaptureBucket = -1;

	private int lastRefreshVersion = -1;

	private int lastRefreshOrdinal = -1;

	private int lastRefreshSpan = -1;

	public Action DisplayedFrameChanged;

	public Action SelectionReset;

	private ProfilerFrameStats stats;

	public ProfilerTreeModel Tree { get; } = new ProfilerTreeModel();

	public ProfilerSearchModel Search { get; } = new ProfilerSearchModel();

	public ProfilerFrameSource Source { get; private set; }

	public bool IsSpikes => Source == ProfilerFrameSource.Spikes;

	public bool IsPaused => FrameProfiler.Paused;

	public bool FollowingNewest => ActivePin.FollowingNewest;

	public bool HasData { get; private set; }

	public FrameProfiler.FrameSnapshot Snapshot { get; private set; }

	public int Ordinal
	{
		get
		{
			if (!HasData)
			{
				return -1;
			}
			return Snapshot.captureOrdinal;
		}
	}

	public int Span { get; private set; } = 1;

	public long SelectionStartNs { get; private set; } = -1L;

	public long SelectionEndNs { get; private set; } = -1L;

	public int DisplayOffset { get; private set; }

	public int DisplayOffsetOldest { get; private set; }

	public bool SelectionAgedOut { get; private set; }

	public int Version { get; private set; }

	public int RefreshTick { get; private set; }

	public int RefreshIntervalFrames { get; private set; } = 1;

	public ProfilerFrameStats Stats
	{
		get
		{
			int num = FrameProfiler.CompletedFrameCount / 10;
			if (statsCaptureBucket != num)
			{
				statsCaptureBucket = num;
				stats = ProfilerFrameStore.ComputeStats();
			}
			return stats;
		}
	}

	public float BudgetMs
	{
		get
		{
			if (!(ProfilerPrefs.Current.budgetMs > 0f))
			{
				if (EngineWindow.TargetFrameRate <= 0)
				{
					return 16.666666f;
				}
				return 1000f / (float)EngineWindow.TargetFrameRate;
			}
			return ProfilerPrefs.Current.budgetMs;
		}
	}

	private ProfilerFrameStore.FramePin ActivePin
	{
		get
		{
			if (!IsSpikes)
			{
				return historyPin;
			}
			return spikePin;
		}
	}

	public ProfilerSession()
	{
		Tree.FoldRecursion = ProfilerPrefs.Current.foldRecursion;
	}

	public void Invalidate()
	{
		Version++;
	}

	public void Poll()
	{
		using (FrameProfiler.BeginRead())
		{
			HasData = TryResolveDisplayedFrame(out var snapshot);
			FrameProfiler.FrameSnapshot snapshot2 = Snapshot;
			Snapshot = (HasData ? snapshot : null);
			Snapshot?.Retain();
			snapshot2?.Release();
			Span = ((!HasData) ? 1 : ResolveSelectionSpan());
			ProfilerThreadRegistry.Sync(Source, (SelectionAgedOut && HasData) ? Snapshot : null);
			UpdateSelectionRange();
			AdvanceRefreshTick();
		}
	}

	private void AdvanceRefreshTick()
	{
		int num = Mathf.Max(Mathf.Abs(Ordinal - lastRefreshOrdinal), Mathf.Abs(Span - lastRefreshSpan));
		if (Version != lastRefreshVersion || num >= RefreshIntervalFrames)
		{
			RefreshTick++;
			lastRefreshVersion = Version;
			lastRefreshOrdinal = Ordinal;
			lastRefreshSpan = Span;
		}
	}

	public void ReportRefreshCost(long startTimestamp)
	{
		float num = (float)Stopwatch.GetElapsedTime(startTimestamp).TotalMilliseconds;
		RefreshIntervalFrames = Mathf.Clamp(Mathf.CeilToInt(num / (BudgetMs * 0.1f)), 1, 60);
	}

	private void UpdateSelectionRange()
	{
		SelectionStartNs = -1L;
		SelectionEndNs = -1L;
		int newestOffset;
		int oldestOffset;
		FrameProfiler.FrameSnapshot snapshot;
		FrameProfiler.FrameSnapshot snapshot2;
		if (manualRangeEndNs > manualRangeStartNs && manualRangeStartNs >= 0)
		{
			SelectionStartNs = manualRangeStartNs;
			SelectionEndNs = manualRangeEndNs;
		}
		else if (!IsSpikes && HasData && TryGetSelectionBand(out newestOffset, out oldestOffset) && TryGetSnapshotAt(oldestOffset, out snapshot) && TryGetSnapshotAt(newestOffset, out snapshot2))
		{
			SelectionStartNs = snapshot.beginTimeNanoseconds;
			SelectionEndNs = snapshot2.endTimeNanoseconds;
		}
		Tree.clipStartNs = SelectionStartNs;
		Tree.clipEndNs = SelectionEndNs;
		ProfilerFrameStore.FramePin activePin = ActivePin;
		int a = ((selectionOldestOrdinal >= 0) ? selectionOldestOrdinal : activePin.PinnedOrdinal);
		Tree.SetWindow(IsSpikes, activePin.FollowingNewest, activePin.PinnedOrdinal, Mathf.Min(a, activePin.PinnedOrdinal), (SelectionAgedOut && HasData) ? Snapshot : null);
	}

	public void SelectTimeRange(int newestOffset, int oldestOffset, long startNs, long endNs)
	{
		if (!IsSpikes && endNs > startNs)
		{
			PinHistoryFrame(newestOffset);
			if (oldestOffset > newestOffset)
			{
				ExtendSelection(oldestOffset);
			}
			manualRangeStartNs = startNs;
			manualRangeEndNs = endNs;
			Version++;
		}
	}

	public bool TryGetSnapshotAt(int offset, out FrameProfiler.FrameSnapshot snapshot)
	{
		return ProfilerFrameStore.TryGetAt(Source, offset, out snapshot);
	}

	public void CollectFrames(int span, List<FrameProfiler.FrameSnapshot> into)
	{
		into.Clear();
		if (!HasData)
		{
			return;
		}
		if (span <= 1)
		{
			into.Add(Snapshot);
			return;
		}
		for (int i = DisplayOffset; i < DisplayOffset + span; i++)
		{
			if (TryGetSnapshotAt(i, out var snapshot))
			{
				into.Add(snapshot);
			}
		}
		if (into.Count == 0)
		{
			into.Add(Snapshot);
		}
	}

	public ProfilerTreeModel.WindowSummary SummarizeSelection()
	{
		ProfilerTreeModel.WindowSummary result = default(ProfilerTreeModel.WindowSummary);
		long num = 0L;
		for (int i = DisplayOffset; i < DisplayOffset + Span; i++)
		{
			if (TryGetSnapshotAt(i, out var snapshot))
			{
				result.framesIncluded++;
				num += Mathf.RoundToInt(snapshot.totalMs * 1000f);
				result.averageTickerDelta += snapshot.tickDelta;
				result.averageNodeCount += snapshot.publishedNodeCount;
				result.averageOverflowedScopes += snapshot.overflowedScopes;
				result.averageAllocBytes += snapshot.DisplayAllocBytes;
				result.averageAllocCount += snapshot.allocatedObjectCount;
				result.gen0Collections += snapshot.gen0Collections;
				result.gen1Collections += snapshot.gen1Collections;
				result.gen2Collections += snapshot.gen2Collections;
				result.averagePromotedBytes += snapshot.promotedBytes;
			}
		}
		if (result.framesIncluded == 0)
		{
			return result;
		}
		result.averageFrameMs = (float)num / (1000f * (float)result.framesIncluded);
		result.averageTickerDelta /= result.framesIncluded;
		result.averageNodeCount /= result.framesIncluded;
		result.averageOverflowedScopes /= result.framesIncluded;
		result.averageAllocBytes /= result.framesIncluded;
		result.averageAllocCount /= result.framesIncluded;
		result.averagePromotedBytes /= result.framesIncluded;
		return result;
	}

	public void SetPaused(bool paused)
	{
		if (FrameProfiler.Paused != paused)
		{
			FrameProfiler.SetPaused(paused);
			Version++;
		}
	}

	public void ClearFrames()
	{
		if (IsSpikes)
		{
			FrameProfiler.ClearSpikes();
		}
		else
		{
			FrameProfiler.ClearHistory();
		}
		ActivePin.FollowNewest();
		ProfilerThreadRegistry.Reset();
		statsCaptureBucket = -1;
		Version++;
	}

	public void SwitchSource(ProfilerFrameSource newSource)
	{
		if (Source != newSource)
		{
			historyPin.FollowNewest();
			spikePin.FollowNewest();
			Source = newSource;
			ProfilerThreadRegistry.Reset();
			ClearSelection();
		}
	}

	public void BeginSelection(int newestOffset)
	{
		if (IsSpikes)
		{
			if (spikePin.PinAtOffset(Mathf.Clamp(newestOffset, 0, FrameProfiler.SpikeCount - 1)))
			{
				selectionAnchorOrdinal = spikePin.PinnedOrdinal;
				selectionOldestOrdinal = spikePin.PinnedOrdinal;
				SetPaused(paused: true);
				Version++;
				DisplayedFrameChanged?.Invoke();
			}
		}
		else
		{
			PinHistoryFrame(newestOffset);
		}
	}

	public void PinHistoryFrame(int newestOffset)
	{
		manualRangeStartNs = -1L;
		manualRangeEndNs = -1L;
		int num = ProfilerFrameStore.CountOf(ProfilerFrameSource.History);
		if (num > 0)
		{
			int newestOffset2 = Mathf.Clamp(newestOffset, 0, num - 1);
			if (historyPin.PinAtOffset(newestOffset2))
			{
				selectionAnchorOrdinal = historyPin.PinnedOrdinal;
				selectionOldestOrdinal = historyPin.PinnedOrdinal;
				SetPaused(paused: true);
				Version++;
				DisplayedFrameChanged?.Invoke();
			}
		}
	}

	public void ExtendSelection(int newestOffset)
	{
		manualRangeStartNs = -1L;
		manualRangeEndNs = -1L;
		ProfilerFrameStore.FramePin activePin = ActivePin;
		int count = activePin.Count;
		if (count <= 0)
		{
			return;
		}
		if (activePin.FollowingNewest || selectionAnchorOrdinal < 0)
		{
			BeginSelection(newestOffset);
			return;
		}
		int offset = Mathf.Clamp(newestOffset, 0, count - 1);
		if (!TryGetSnapshotAt(offset, out var snapshot))
		{
			return;
		}
		int captureOrdinal = snapshot.captureOrdinal;
		int num = Mathf.Max(selectionAnchorOrdinal, captureOrdinal);
		int num2 = Mathf.Min(selectionAnchorOrdinal, captureOrdinal);
		if (num2 == selectionOldestOrdinal && ActivePin.PinnedOrdinal == num)
		{
			return;
		}
		selectionOldestOrdinal = num2;
		Version++;
		if (activePin.PinnedOrdinal != num)
		{
			int num3 = OrdinalToOffset(Source, num);
			if (num3 >= 0)
			{
				activePin.PinAtOffset(num3);
			}
		}
	}

	public void ClearSelection()
	{
		ActivePin.FollowNewest();
		selectionAnchorOrdinal = -1;
		selectionOldestOrdinal = -1;
		manualRangeStartNs = -1L;
		manualRangeEndNs = -1L;
		Version++;
		SelectionReset?.Invoke();
	}

	public void JumpToNewest()
	{
		ClearSelection();
		if (!IsSpikes)
		{
			SetPaused(paused: false);
		}
	}

	public void Step(int direction)
	{
		ProfilerFrameStore.FramePin activePin = ActivePin;
		int count = activePin.Count;
		if (count > 0)
		{
			int newestOffset;
			int num = (activePin.FollowingNewest ? (-1) : (activePin.TryResolveOffset(out newestOffset) ? newestOffset : count)) + direction;
			if (num <= -1 && direction < 0)
			{
				JumpToNewest();
			}
			else
			{
				BeginSelection(Mathf.Clamp(num, 0, count - 1));
			}
		}
	}

	public bool PinOrdinal(int ordinal, bool keepIfInsideSelection, out bool selectionChanged)
	{
		selectionChanged = false;
		ProfilerFrameStore.FramePin activePin = ActivePin;
		if ((keepIfInsideSelection && !activePin.FollowingNewest && selectionOldestOrdinal >= 0 && ordinal >= selectionOldestOrdinal && ordinal <= activePin.PinnedOrdinal) || (!activePin.FollowingNewest && activePin.PinnedOrdinal == ordinal))
		{
			return true;
		}
		int num = OrdinalToOffset(Source, ordinal);
		if (num < 0)
		{
			return false;
		}
		BeginSelection(num);
		selectionChanged = true;
		return true;
	}

	public void SelectZoneTimeRange(int ordinal, long startNs, long endNs)
	{
		if (ProfilerFrameStore.TryOrdinalToOffset(Source, ordinal, out var newestOffset))
		{
			SelectTimeRange(newestOffset, newestOffset, startNs, endNs);
		}
	}

	public bool TryGetSnapshotForOrdinal(int ordinal, out FrameProfiler.FrameSnapshot snapshot)
	{
		if (SelectionAgedOut && HasData && Snapshot.captureOrdinal == ordinal)
		{
			snapshot = Snapshot;
			return true;
		}
		snapshot = null;
		if (ProfilerFrameStore.TryOrdinalToOffset(Source, ordinal, out var newestOffset))
		{
			return TryGetSnapshotAt(newestOffset, out snapshot);
		}
		return false;
	}

	public bool TryGetSelectionBand(out int newestOffset, out int oldestOffset)
	{
		newestOffset = -1;
		oldestOffset = -1;
		if (IsSpikes)
		{
			if (DisplayOffset < 0 || DisplayOffset >= FrameProfiler.SpikeCount)
			{
				return false;
			}
		}
		else if (historyPin.FollowingNewest || DisplayOffset < 0 || DisplayOffset >= FrameProfiler.HistoryCount)
		{
			return false;
		}
		newestOffset = DisplayOffset;
		oldestOffset = DisplayOffsetOldest;
		return true;
	}

	private static int OrdinalToOffset(ProfilerFrameSource source, int ordinal)
	{
		if (!ProfilerFrameStore.TryOrdinalToOffset(source, ordinal, out var newestOffset))
		{
			return -1;
		}
		return newestOffset;
	}

	private bool TryResolveDisplayedFrame(out FrameProfiler.FrameSnapshot snapshot)
	{
		if (IsSpikes)
		{
			DisplayOffset = ((!spikePin.FollowingNewest) ? (spikePin.TryResolveOffset(out var newestOffset) ? newestOffset : (-1)) : 0);
			DisplayOffsetOldest = DisplayOffset;
			SelectionAgedOut = !spikePin.FollowingNewest && DisplayOffset < 0;
			if (!spikePin.FollowingNewest && DisplayOffset >= 0 && selectionOldestOrdinal >= 0 && selectionOldestOrdinal != spikePin.PinnedOrdinal)
			{
				int num = OrdinalToOffset(ProfilerFrameSource.Spikes, selectionOldestOrdinal);
				DisplayOffsetOldest = ((num >= DisplayOffset) ? num : (FrameProfiler.SpikeCount - 1));
			}
			return spikePin.TryGetSnapshot(out snapshot);
		}
		SelectionAgedOut = false;
		if (historyPin.FollowingNewest)
		{
			DisplayOffset = 0;
			DisplayOffsetOldest = Mathf.Max(0, FrameProfiler.HistoryCount - 1);
			return historyPin.TryGetSnapshot(out snapshot);
		}
		if (historyPin.TryResolveOffset(out var newestOffset2))
		{
			DisplayOffset = newestOffset2;
			DisplayOffsetOldest = Mathf.Min(newestOffset2 + (historyPin.PinnedOrdinal - selectionOldestOrdinal), FrameProfiler.HistoryCount - 1);
			return historyPin.TryGetSnapshot(out snapshot);
		}
		DisplayOffset = FrameProfiler.HistoryCount;
		DisplayOffsetOldest = DisplayOffset;
		SelectionAgedOut = true;
		return historyPin.TryGetSnapshot(out snapshot);
	}

	private int ResolveSelectionSpan()
	{
		if (IsSpikes)
		{
			if (spikePin.FollowingNewest || DisplayOffset < 0)
			{
				return 1;
			}
			return Mathf.Min(DisplayOffsetOldest, FrameProfiler.SpikeCount - 1) - DisplayOffset + 1;
		}
		int historyCount = FrameProfiler.HistoryCount;
		if (historyPin.FollowingNewest)
		{
			return Mathf.Max(1, historyCount);
		}
		if (DisplayOffset >= historyCount)
		{
			return 1;
		}
		return Mathf.Min(DisplayOffsetOldest, historyCount - 1) - DisplayOffset + 1;
	}
}
