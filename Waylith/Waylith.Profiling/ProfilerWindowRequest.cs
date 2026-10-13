using System;

namespace Waylith.Profiling;

public sealed class ProfilerWindowRequest
{
	public bool spikes;

	public bool followNewest;

	public int newestOrdinal;

	public int oldestOrdinal;

	public long clipStartNs = -1L;

	public long clipEndNs = -1L;

	public int[]? selectedThreadIds;

	public bool foldRecursion;

	public FrameProfiler.FrameSnapshot? fallbackSnapshot;

	public bool ClipActive
	{
		get
		{
			if (clipStartNs >= 0)
			{
				return clipEndNs > clipStartNs;
			}
			return false;
		}
	}

	public bool SameWindowAs(ProfilerWindowRequest other)
	{
		if (spikes == other.spikes && followNewest == other.followNewest && (followNewest || (newestOrdinal == other.newestOrdinal && oldestOrdinal == other.oldestOrdinal)) && clipStartNs == other.clipStartNs && clipEndNs == other.clipEndNs && foldRecursion == other.foldRecursion && fallbackSnapshot == other.fallbackSnapshot)
		{
			return SameThreads(selectedThreadIds, other.selectedThreadIds);
		}
		return false;
	}

	private static bool SameThreads(int[]? lhs, int[]? rhs)
	{
		if (lhs != rhs)
		{
			if (lhs != null && rhs != null)
			{
				return ((ReadOnlySpan<int>)lhs.AsSpan()).SequenceEqual((ReadOnlySpan<int>)rhs);
			}
			return false;
		}
		return true;
	}

	public ProfilerWindowRequest Clone()
	{
		return (ProfilerWindowRequest)MemberwiseClone();
	}

	public bool IncludesThread(int threadId)
	{
		if (selectedThreadIds != null)
		{
			return Array.BinarySearch(selectedThreadIds, threadId) >= 0;
		}
		return true;
	}
}
