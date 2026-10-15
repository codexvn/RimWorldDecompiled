using System.Collections.Generic;
using Waylith.Profiling;

namespace Verse.ProfilerUI;

internal static class ProfilerThreadRegistry
{
	public struct Entry
	{
		public int threadId;

		public ProfilerLabelId nameLabelId;

		public bool isMain;
	}

	private static readonly List<Entry> entries = new List<Entry>();

	private static readonly HashSet<int> selectedThreadIds = new HashSet<int>();

	private static readonly HashSet<int> presentThreadIds = new HashSet<int>();

	private static readonly List<ThreadTimeline> timelineScratch = new List<ThreadTimeline>();

	private static int summaryVersion = -1;

	private static int summarySelectionVersion = -1;

	private static int summarySelected;

	private static int summarySingleIndex;

	public static int Version { get; private set; }

	public static int SelectionVersion { get; private set; }

	public static IReadOnlyList<Entry> Entries => entries;

	public static bool AllSelected
	{
		get
		{
			for (int i = 0; i < entries.Count; i++)
			{
				if (!selectedThreadIds.Contains(entries[i].threadId))
				{
					return false;
				}
			}
			return true;
		}
	}

	public static void Sync(ProfilerFrameSource source, FrameProfiler.FrameSnapshot agedOutFrame)
	{
		using (FrameProfiler.BeginRead())
		{
			FrameProfiler.CollectTimelines(timelineScratch);
			presentThreadIds.Clear();
			if (agedOutFrame != null)
			{
				AddThreadsOf(agedOutFrame);
			}
			else if (source == ProfilerFrameSource.History)
			{
				int num = ProfilerFrameStore.CountOf(source);
				if (ProfilerFrameStore.TryGetAt(source, 0, out FrameProfiler.FrameSnapshot snapshot) && ProfilerFrameStore.TryGetAt(source, num - 1, out FrameProfiler.FrameSnapshot snapshot2))
				{
					long beginTimeNanoseconds = snapshot2.beginTimeNanoseconds;
					long endNs = snapshot.beginTimeNanoseconds + snapshot.nodesEndNanoseconds;
					foreach (ThreadTimeline item in timelineScratch)
					{
						if (item.isMain || FrameProfiler.HasChunks(item, beginTimeNanoseconds, endNs))
						{
							presentThreadIds.Add(item.threadId);
						}
					}
				}
			}
			else
			{
				for (int i = 0; i < ProfilerFrameStore.CountOf(source); i++)
				{
					if (ProfilerFrameStore.TryGetAt(source, i, out FrameProfiler.FrameSnapshot snapshot3))
					{
						AddThreadsOf(snapshot3);
					}
				}
			}
			Apply();
		}
	}

	private static void AddThreadsOf(FrameProfiler.FrameSnapshot frame)
	{
		long beginTimeNanoseconds = frame.beginTimeNanoseconds;
		long endNs = beginTimeNanoseconds + frame.nodesEndNanoseconds;
		foreach (ThreadTimeline item in timelineScratch)
		{
			if (item.isMain || FrameProfiler.HasChunks(frame, item.threadId, beginTimeNanoseconds, endNs))
			{
				presentThreadIds.Add(item.threadId);
			}
		}
	}

	private static void Apply()
	{
		bool flag = false;
		for (int num = entries.Count - 1; num >= 0; num--)
		{
			if (!presentThreadIds.Contains(entries[num].threadId))
			{
				selectedThreadIds.Remove(entries[num].threadId);
				entries.RemoveAt(num);
				flag = true;
			}
		}
		foreach (ThreadTimeline item in timelineScratch)
		{
			if (presentThreadIds.Contains(item.threadId) && IndexOf(item.threadId) < 0)
			{
				Entry entry = default(Entry);
				entry.threadId = item.threadId;
				entry.nameLabelId = item.nameLabelId;
				entry.isMain = item.isMain;
				Insert(entry);
				flag = true;
			}
		}
		if (flag)
		{
			Version++;
			SelectionVersion++;
		}
	}

	private static int IndexOf(int threadId)
	{
		for (int i = 0; i < entries.Count; i++)
		{
			if (entries[i].threadId == threadId)
			{
				return i;
			}
		}
		return -1;
	}

	private static void Insert(Entry entry)
	{
		int index = entries.Count;
		if (entry.isMain)
		{
			index = 0;
		}
		else
		{
			string label = FrameProfiler.GetLabel(entry.nameLabelId);
			for (int i = 0; i < entries.Count; i++)
			{
				if (!entries[i].isMain && string.CompareOrdinal(label, FrameProfiler.GetLabel(entries[i].nameLabelId)) < 0)
				{
					index = i;
					break;
				}
			}
		}
		entries.Insert(index, entry);
		if (entry.isMain)
		{
			selectedThreadIds.Add(entry.threadId);
		}
	}

	public static bool IsSelected(int threadId)
	{
		return selectedThreadIds.Contains(threadId);
	}

	public static void GetSelectionSummary(out int selected, out int singleIndex)
	{
		if (summaryVersion != Version || summarySelectionVersion != SelectionVersion)
		{
			summaryVersion = Version;
			summarySelectionVersion = SelectionVersion;
			summarySelected = 0;
			summarySingleIndex = -1;
			for (int i = 0; i < entries.Count; i++)
			{
				if (selectedThreadIds.Contains(entries[i].threadId))
				{
					summarySelected++;
					summarySingleIndex = i;
				}
			}
		}
		selected = summarySelected;
		singleIndex = summarySingleIndex;
	}

	public static void SetSelected(int threadId, bool value)
	{
		if (!(value ? (!selectedThreadIds.Add(threadId)) : (!selectedThreadIds.Remove(threadId))))
		{
			SelectionVersion++;
		}
	}

	public static void SelectOnly(int threadId)
	{
		if (selectedThreadIds.Count != 1 || !selectedThreadIds.Contains(threadId))
		{
			selectedThreadIds.Clear();
			selectedThreadIds.Add(threadId);
			SelectionVersion++;
		}
	}

	public static void Reset()
	{
		entries.Clear();
		selectedThreadIds.Clear();
		Version++;
		SelectionVersion++;
	}
}
