using System;
using System.Collections.Generic;
using Waylith.Graphics;

namespace Waylith.Profiling;

public static class VramProfiler
{
	public sealed class Node
	{
		public readonly List<Node> Children = new List<Node>();

		public string? Segment { get; set; }

		public string? FullPath { get; set; }

		public long TotalBytes { get; set; }

		public long TotalCount { get; set; }

		public long DirectBytes { get; set; }

		public long DirectCount { get; set; }

		public long BaselineBytes { get; set; }

		public long BaselineCount { get; set; }

		public long DirectBaselineBytes { get; set; }

		public long DirectBaselineCount { get; set; }

		public string? Detail { get; set; }

		public long DeltaBytes => TotalBytes - BaselineBytes;

		public long DeltaCount => TotalCount - BaselineCount;

		public long DirectDeltaBytes => DirectBytes - DirectBaselineBytes;

		public long DirectDeltaCount => DirectCount - DirectBaselineCount;

		public bool HasChildren => Children.Count > 0;

		public string? SoleDetail
		{
			get
			{
				if (TotalCount != 1)
				{
					return null;
				}
				Node node = this;
				while (node.DirectCount == 0L && node.Children.Count == 1)
				{
					node = node.Children[0];
				}
				return node.Detail;
			}
		}

		public Node? FindChild(string segment)
		{
			for (int i = 0; i < Children.Count; i++)
			{
				if (Children[i].Segment == segment)
				{
					return Children[i];
				}
			}
			return null;
		}
	}

	private sealed class BaselineLabelAllocs
	{
		public readonly HashSet<GpuMemoryTracker.AllocKey> Ids = new HashSet<GpuMemoryTracker.AllocKey>();

		public readonly List<(GpuMemoryTracker.AllocKey Id, long Bytes, string Name)> Entries = new List<(GpuMemoryTracker.AllocKey, long, string)>();
	}

	public readonly record struct IndividualAllocation(string Name, long Bytes, long DeltaBytes, bool Freed, string? Detail = null);

	private static readonly Dictionary<string, GpuMemoryTracker.LabelStat> scratch = new Dictionary<string, GpuMemoryTracker.LabelStat>();

	private static readonly Dictionary<GpuMemoryTracker.AllocKey, GpuMemoryTracker.AllocationSnapshot> allocSnapshotScratch = new Dictionary<GpuMemoryTracker.AllocKey, GpuMemoryTracker.AllocationSnapshot>();

	private static Dictionary<string, GpuMemoryTracker.LabelStat>? baseline;

	private static Dictionary<string, BaselineLabelAllocs>? baselineAllocs;

	private static readonly List<GpuMemoryTracker.TrackedAllocation> allocScratch = new List<GpuMemoryTracker.TrackedAllocation>();

	private static readonly HashSet<GpuMemoryTracker.AllocKey> liveIdScratch = new HashSet<GpuMemoryTracker.AllocKey>();

	public static Node Root { get; private set; } = new Node
	{
		Segment = string.Empty,
		FullPath = string.Empty
	};

	public static bool HasBaseline => baseline != null;

	public static void CaptureBaseline()
	{
		Dictionary<string, GpuMemoryTracker.LabelStat> into = new Dictionary<string, GpuMemoryTracker.LabelStat>();
		GpuMemoryTracker.AggregateByLabel(into);
		GpuMemoryTracker.SnapshotAllocations(allocSnapshotScratch);
		Dictionary<string, BaselineLabelAllocs> dictionary = new Dictionary<string, BaselineLabelAllocs>();
		foreach (var (allocKey2, allocationSnapshot2) in allocSnapshotScratch)
		{
			if (!dictionary.TryGetValue(allocationSnapshot2.EffectiveLabel, out var value))
			{
				value = new BaselineLabelAllocs();
				dictionary.Add(allocationSnapshot2.EffectiveLabel, value);
			}
			value.Ids.Add(allocKey2);
			value.Entries.Add((allocKey2, allocationSnapshot2.Bytes, allocationSnapshot2.Name ?? "(unnamed)"));
		}
		allocSnapshotScratch.Clear();
		baseline = into;
		baselineAllocs = dictionary;
	}

	public static void ClearBaseline()
	{
		baseline = null;
		baselineAllocs = null;
	}

	public static void Capture(bool sortByDeltaMagnitude = false)
	{
		GpuMemoryTracker.AggregateByLabel(scratch);
		GpuMemoryTracker.LabelStat value;
		if (baseline != null)
		{
			foreach (string key2 in baseline.Keys)
			{
				Dictionary<string, GpuMemoryTracker.LabelStat> dictionary = scratch;
				value = default(GpuMemoryTracker.LabelStat);
				dictionary.TryAdd(key2, value);
			}
		}
		Node node = new Node
		{
			Segment = string.Empty,
			FullPath = string.Empty
		};
		foreach (KeyValuePair<string, GpuMemoryTracker.LabelStat> item in scratch)
		{
			item.Deconstruct(out var key, out value);
			string text = key;
			GpuMemoryTracker.LabelStat labelStat = value;
			string text2 = (string.IsNullOrEmpty(text) ? "(unnamed)" : text);
			GpuMemoryTracker.LabelStat value2 = default(GpuMemoryTracker.LabelStat);
			baseline?.TryGetValue(text, out value2);
			node.TotalBytes += labelStat.Bytes;
			node.TotalCount += labelStat.Count;
			node.BaselineBytes += value2.Bytes;
			node.BaselineCount += value2.Count;
			Node node2 = node;
			int num = 0;
			while (num <= text2.Length)
			{
				int num2 = text2.IndexOf('/', num);
				int num3 = ((num2 < 0) ? text2.Length : num2);
				if (num3 > num)
				{
					string segment = text2.Substring(num, num3 - num);
					Node node3 = node2.FindChild(segment);
					if (node3 == null)
					{
						node3 = new Node
						{
							Segment = segment,
							FullPath = text2.Substring(0, num3)
						};
						node2.Children.Add(node3);
					}
					node3.TotalBytes += labelStat.Bytes;
					node3.TotalCount += labelStat.Count;
					node3.BaselineBytes += value2.Bytes;
					node3.BaselineCount += value2.Count;
					node2 = node3;
				}
				if (num2 < 0)
				{
					break;
				}
				num = num3 + 1;
			}
			node2.DirectBytes += labelStat.Bytes;
			node2.DirectCount += labelStat.Count;
			node2.Detail = ((labelStat.Count == 1) ? labelStat.Detail : null);
			node2.DirectBaselineBytes += value2.Bytes;
			node2.DirectBaselineCount += value2.Count;
		}
		Sort(node, sortByDeltaMagnitude && baseline != null);
		Root = node;
	}

	private static void Sort(Node node, bool byDeltaMagnitude)
	{
		if (byDeltaMagnitude)
		{
			node.Children.Sort((Node a, Node b) => System.Math.Abs(b.DeltaBytes).CompareTo(System.Math.Abs(a.DeltaBytes)));
		}
		else
		{
			node.Children.Sort((Node a, Node b) => b.TotalBytes.CompareTo(a.TotalBytes));
		}
		for (int i = 0; i < node.Children.Count; i++)
		{
			Sort(node.Children[i], byDeltaMagnitude);
		}
	}

	public static Node? Resolve(IReadOnlyList<string> path)
	{
		Node node = Root;
		for (int i = 0; i < path.Count; i++)
		{
			node = node.FindChild(path[i]);
			if (node == null)
			{
				return null;
			}
		}
		return node;
	}

	public static void CollectIndividuals(string label, bool sortByDeltaMagnitude, List<IndividualAllocation> into)
	{
		into.Clear();
		GpuMemoryTracker.CollectAllocations(label, allocScratch);
		BaselineLabelAllocs value = null;
		baselineAllocs?.TryGetValue(label, out value);
		for (int i = 0; i < allocScratch.Count; i++)
		{
			GpuMemoryTracker.TrackedAllocation trackedAllocation = allocScratch[i];
			long deltaBytes = 0L;
			if (baselineAllocs != null && (value == null || !value.Ids.Contains(trackedAllocation.Id)))
			{
				deltaBytes = trackedAllocation.Bytes;
			}
			into.Add(new IndividualAllocation(trackedAllocation.Name ?? "(unnamed)", trackedAllocation.Bytes, deltaBytes, Freed: false, trackedAllocation.Detail));
		}
		if (value != null)
		{
			liveIdScratch.Clear();
			for (int j = 0; j < allocScratch.Count; j++)
			{
				liveIdScratch.Add(allocScratch[j].Id);
			}
			for (int k = 0; k < value.Entries.Count; k++)
			{
				(GpuMemoryTracker.AllocKey, long, string) tuple = value.Entries[k];
				if (!liveIdScratch.Contains(tuple.Item1))
				{
					into.Add(new IndividualAllocation(tuple.Item3, 0L, -tuple.Item2, Freed: true));
				}
			}
			liveIdScratch.Clear();
		}
		allocScratch.Clear();
		if (sortByDeltaMagnitude)
		{
			into.Sort(delegate(IndividualAllocation a, IndividualAllocation b)
			{
				int num = System.Math.Abs(b.DeltaBytes).CompareTo(System.Math.Abs(a.DeltaBytes));
				return (num == 0) ? b.Bytes.CompareTo(a.Bytes) : num;
			});
		}
		else
		{
			into.Sort((IndividualAllocation a, IndividualAllocation b) => b.Bytes.CompareTo(a.Bytes));
		}
	}

	public static string FormatBytes(long bytes)
	{
		if (bytes < 1073741824)
		{
			if (bytes < 1048576)
			{
				if (bytes < 1024)
				{
					return $"{bytes} B";
				}
				return $"{(double)bytes / 1024.0:0.0} KiB";
			}
			return $"{(double)bytes / 1048576.0:0.0} MiB";
		}
		return $"{(double)bytes / 1073741824.0:0.00} GiB";
	}
}
