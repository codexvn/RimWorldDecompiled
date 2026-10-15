using System;
using System.Diagnostics.CodeAnalysis;
using Waylith.Profiling;

namespace Verse.ProfilerUI;

internal static class ProfilerFrameStore
{
	public sealed class FramePin
	{
		private readonly ProfilerFrameSource source;

		private FrameProfiler.FrameSnapshot? retainedCopy;

		public bool FollowingNewest => PinnedOrdinal < 0;

		public int PinnedOrdinal { get; private set; } = -1;

		public int Count => CountOf(source);

		public FramePin(ProfilerFrameSource source)
		{
			this.source = source;
		}

		public void FollowNewest()
		{
			PinnedOrdinal = -1;
			retainedCopy?.Release();
			retainedCopy = null;
		}

		public bool PinAtOffset(int newestOffset)
		{
			using (FrameProfiler.BeginRead())
			{
				if (!TryGetAt(source, newestOffset, out FrameProfiler.FrameSnapshot snapshot))
				{
					return false;
				}
				PinnedOrdinal = snapshot.captureOrdinal;
				RefreshRetainedCopy(snapshot);
				return true;
			}
		}

		public bool TryResolveOffset(out int newestOffset)
		{
			newestOffset = -1;
			if (!FollowingNewest)
			{
				return TryOrdinalToOffset(source, PinnedOrdinal, out newestOffset);
			}
			return false;
		}

		public bool TryGetSnapshot([NotNullWhen(true)] out FrameProfiler.FrameSnapshot? snapshot)
		{
			using (FrameProfiler.BeginRead())
			{
				if (FollowingNewest)
				{
					return TryGetAt(source, 0, out snapshot);
				}
				if (TryResolveOffset(out var newestOffset) && TryGetAt(source, newestOffset, out snapshot))
				{
					RefreshRetainedCopy(snapshot);
					return true;
				}
				if (retainedCopy != null)
				{
					snapshot = retainedCopy;
					return true;
				}
				snapshot = null;
				return false;
			}
		}

		private void RefreshRetainedCopy(FrameProfiler.FrameSnapshot snapshot)
		{
			if (retainedCopy != snapshot)
			{
				snapshot.Retain();
				snapshot.RetainZones();
				retainedCopy?.Release();
				retainedCopy = snapshot;
			}
		}
	}

	private static float[] statsScratch = Array.Empty<float>();

	public static int CountOf(ProfilerFrameSource source)
	{
		if (source != 0)
		{
			return FrameProfiler.SpikeCount;
		}
		return FrameProfiler.HistoryCount;
	}

	public static bool TryGetAt(ProfilerFrameSource source, int newestOffset, [NotNullWhen(true)] out FrameProfiler.FrameSnapshot? snapshot)
	{
		if (source != 0)
		{
			return FrameProfiler.TryGetSpikeSnapshot(newestOffset, out snapshot);
		}
		return FrameProfiler.TryGetHistorySnapshot(newestOffset, out snapshot);
	}

	public static bool TryOrdinalToOffset(ProfilerFrameSource source, int ordinal, out int newestOffset)
	{
		newestOffset = -1;
		if (ordinal < 0)
		{
			return false;
		}
		if (source == ProfilerFrameSource.History)
		{
			if (!TryGetAt(source, 0, out FrameProfiler.FrameSnapshot snapshot))
			{
				return false;
			}
			int num = snapshot.captureOrdinal - ordinal;
			if (num < 0 || num >= FrameProfiler.HistoryCount)
			{
				return false;
			}
			newestOffset = num;
			return true;
		}
		for (int i = 0; i < FrameProfiler.SpikeCount; i++)
		{
			if (TryGetAt(source, i, out FrameProfiler.FrameSnapshot snapshot2) && snapshot2.captureOrdinal == ordinal)
			{
				newestOffset = i;
				return true;
			}
		}
		return false;
	}

	public static ProfilerFrameStats ComputeStats()
	{
		int historyCount = FrameProfiler.HistoryCount;
		if (statsScratch.Length < historyCount)
		{
			statsScratch = new float[FrameProfiler.HistoryCapacity];
		}
		int num = 0;
		for (int i = 0; i < historyCount; i++)
		{
			if (FrameProfiler.TryGetHistorySnapshot(i, out FrameProfiler.FrameSnapshot snapshot))
			{
				statsScratch[num++] = snapshot.totalMs;
			}
		}
		if (num == 0)
		{
			return default(ProfilerFrameStats);
		}
		Array.Sort(statsScratch, 0, num);
		ProfilerFrameStats result = default(ProfilerFrameStats);
		result.medianMs = statsScratch[num / 2];
		result.p99Ms = statsScratch[Math.Min(num - 1, (int)((float)num * 0.99f))];
		return result;
	}
}
