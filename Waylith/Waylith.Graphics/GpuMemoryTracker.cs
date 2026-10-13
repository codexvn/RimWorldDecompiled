using System;
using System.Collections.Generic;
using Brutal.Logging;
using Brutal.Logging.Internal;
using Brutal.VulkanApi.Abstractions;

namespace Waylith.Graphics;

public static class GpuMemoryTracker
{
	public readonly record struct CategoryStat(GpuMemoryCategory Category, long Bytes, long Count);

	public readonly record struct LabelStat(long Bytes, long Count, string? Detail = null);

	public readonly record struct TrackedAllocation(GpuMemoryCategory Category, long Bytes, string? Name, AllocKey Id = default(AllocKey), string? Detail = null);

	public readonly record struct TopLevelStat(string Label, long Bytes, long Count);

	public readonly record struct AllocationSnapshot(string EffectiveLabel, long Bytes, string? Name);

	public readonly record struct AllocKey(nint Handle, bool IsImage);

	private static readonly LogCategory Logger = LogCategory.Make("GpuMemoryTracker");

	private static readonly int CategoryCount = Enum.GetValues<GpuMemoryCategory>().Length;

	private static readonly object Gate = new object();

	private static readonly Dictionary<AllocKey, (GpuMemoryCategory Category, long Bytes, string? Label, string? Name, string? Detail)> Live = new Dictionary<AllocKey, (GpuMemoryCategory, long, string, string, string)>();

	private static readonly HashSet<AllocKey> WarnedHandles = new HashSet<AllocKey>();

	private static readonly long[] BytesByCategory = new long[CategoryCount];

	private static readonly long[] CountByCategory = new long[CategoryCount];

	private static readonly Dictionary<string, string> TopLevelByLabel = new Dictionary<string, string>();

	private static readonly Dictionary<string, LabelStat> ByTopLevel = new Dictionary<string, LabelStat>();

	private static string EffectiveLabel(GpuMemoryCategory category, string? label)
	{
		if (!string.IsNullOrEmpty(label))
		{
			return label;
		}
		return category.ToString();
	}

	private static string TopLevelOf(GpuMemoryCategory category, string? label)
	{
		string text = EffectiveLabel(category, label);
		if (TopLevelByLabel.TryGetValue(text, out string value))
		{
			return value;
		}
		int num = text.IndexOf('/');
		value = ((num < 0) ? text : text.Substring(0, num));
		TopLevelByLabel[text] = value;
		return value;
	}

	private static void AddTopLevel(GpuMemoryCategory category, string? label, long bytes, long count)
	{
		string key = TopLevelOf(category, label);
		ByTopLevel.TryGetValue(key, out var value);
		ByTopLevel[key] = new LabelStat(value.Bytes + bytes, value.Count + count);
	}

	public static void Add(GpuMemoryCategory category, in BufferAllocationInfo allocation, string? label = null, string? name = null)
	{
		Track(category, new AllocKey(allocation.VkBuffer.VkHandle, IsImage: false), (long)allocation.AllocationInfo.AllocSize, label, name);
	}

	public static void Remove(GpuMemoryCategory category, in BufferAllocationInfo allocation)
	{
		Untrack(new AllocKey(allocation.VkBuffer.VkHandle, IsImage: false));
	}

	public static void Add(GpuMemoryCategory category, in ImageAllocationInfo allocation, string? label = null, string? name = null)
	{
		Track(category, new AllocKey(allocation.Image.VkHandle, IsImage: true), (long)allocation.MemAllocationInfo.AllocSize, label, name);
	}

	public static void Remove(GpuMemoryCategory category, in ImageAllocationInfo allocation)
	{
		Untrack(new AllocKey(allocation.Image.VkHandle, IsImage: true));
	}

	public static void SetName(in ImageAllocationInfo allocation, string? name)
	{
		AllocKey key = new AllocKey(allocation.Image.VkHandle, IsImage: true);
		if (key.Handle == 0)
		{
			return;
		}
		lock (Gate)
		{
			if (Live.TryGetValue(key, out (GpuMemoryCategory, long, string, string, string) value))
			{
				Live[key] = (value.Item1, value.Item2, value.Item3, name, value.Item5);
			}
		}
	}

	public static void SetCategory(in ImageAllocationInfo allocation, GpuMemoryCategory category, string? label)
	{
		AllocKey key = new AllocKey(allocation.Image.VkHandle, IsImage: true);
		if (key.Handle == 0)
		{
			return;
		}
		lock (Gate)
		{
			if (Live.TryGetValue(key, out (GpuMemoryCategory, long, string, string, string) value))
			{
				BytesByCategory[(int)value.Item1] -= value.Item2;
				CountByCategory[(int)value.Item1]--;
				AddTopLevel(value.Item1, value.Item3, -value.Item2, -1L);
				Live[key] = (category, value.Item2, label, value.Item4, value.Item5);
				BytesByCategory[(int)category] += value.Item2;
				CountByCategory[(int)category]++;
				AddTopLevel(category, label, value.Item2, 1L);
			}
		}
	}

	public static void SetDetail(in ImageAllocationInfo allocation, string? detail)
	{
		AllocKey key = new AllocKey(allocation.Image.VkHandle, IsImage: true);
		if (key.Handle == 0)
		{
			return;
		}
		lock (Gate)
		{
			if (Live.TryGetValue(key, out (GpuMemoryCategory, long, string, string, string) value))
			{
				Live[key] = (value.Item1, value.Item2, value.Item3, value.Item4, detail);
			}
		}
	}

	private static void Track(GpuMemoryCategory category, AllocKey key, long bytes, string? label, string? name)
	{
		if (key.Handle == 0 || bytes <= 0)
		{
			return;
		}
		bool flag = false;
		GpuMemoryCategory value = GpuMemoryCategory.MeshVertex;
		long value2 = 0L;
		lock (Gate)
		{
			if (Live.TryGetValue(key, out (GpuMemoryCategory, long, string, string, string) value3))
			{
				BytesByCategory[(int)value3.Item1] -= value3.Item2;
				CountByCategory[(int)value3.Item1]--;
				AddTopLevel(value3.Item1, value3.Item3, -value3.Item2, -1L);
				if (WarnedHandles.Add(key))
				{
					flag = true;
					(value, value2, _, _, _) = value3;
				}
			}
			Live[key] = (category, bytes, label, name, null);
			BytesByCategory[(int)category] += bytes;
			CountByCategory[(int)category]++;
			AddTopLevel(category, label, bytes, 1L);
		}
		if (flag)
		{
			LogCategory logger = Logger;
			bool shouldAppend;
			LogString<Warning> message = new LogString<Warning>(212, 7, logger, out shouldAppend);
			if (shouldAppend)
			{
				message.AppendLiteral("Duplicate GPU allocation for ");
				message.AppendFormatted(key.IsImage ? "image" : "buffer");
				message.AppendLiteral(" handle 0x");
				message.AppendFormatted(key.Handle, 0, "X");
				message.AppendLiteral(": adding ");
				message.AppendFormatted(category);
				message.AppendLiteral(" (");
				message.AppendFormatted(bytes);
				message.AppendLiteral(" bytes) while it is already tracked as ");
				message.AppendFormatted(value);
				message.AppendLiteral(" (");
				message.AppendFormatted(value2);
				message.AppendLiteral(" bytes). ");
				message.AppendLiteral("This means an unbalanced Add/Remove in a caller (the byte total stays correct, but the source should ");
				message.AppendLiteral("be fixed).\n");
				message.AppendFormatted(Environment.StackTrace);
			}
			logger.Warning(message, "Track", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Graphics\\GpuMemoryTracker.cs", 198);
		}
	}

	private static void Untrack(AllocKey key)
	{
		if (key.Handle == 0)
		{
			return;
		}
		lock (Gate)
		{
			if (Live.Remove(key, out (GpuMemoryCategory, long, string, string, string) value))
			{
				BytesByCategory[(int)value.Item1] -= value.Item2;
				CountByCategory[(int)value.Item1]--;
				AddTopLevel(value.Item1, value.Item3, -value.Item2, -1L);
				WarnedHandles.Remove(key);
			}
		}
	}

	public static long GetBytes(GpuMemoryCategory category)
	{
		lock (Gate)
		{
			return BytesByCategory[(int)category];
		}
	}

	public static long TotalBytes()
	{
		lock (Gate)
		{
			long num = 0L;
			for (int i = 0; i < CategoryCount; i++)
			{
				num += BytesByCategory[i];
			}
			return num;
		}
	}

	public static long MeshBytes()
	{
		lock (Gate)
		{
			return BytesByCategory[0] + BytesByCategory[1];
		}
	}

	public static long TextureBytes()
	{
		lock (Gate)
		{
			return BytesByCategory[2] + BytesByCategory[3] + BytesByCategory[4] + BytesByCategory[5];
		}
	}

	public static CategoryStat[] Snapshot()
	{
		CategoryStat[] array = new CategoryStat[CategoryCount];
		lock (Gate)
		{
			for (int i = 0; i < CategoryCount; i++)
			{
				array[i] = new CategoryStat((GpuMemoryCategory)i, BytesByCategory[i], CountByCategory[i]);
			}
			return array;
		}
	}

	public static void CollectTopLevel(List<TopLevelStat> into)
	{
		ArgumentNullException.ThrowIfNull(into, "into");
		into.Clear();
		lock (Gate)
		{
			foreach (var (label, labelStat2) in ByTopLevel)
			{
				if (labelStat2.Count > 0)
				{
					into.Add(new TopLevelStat(label, labelStat2.Bytes, labelStat2.Count));
				}
			}
		}
	}

	public static void AggregateByLabel(Dictionary<string, LabelStat> into)
	{
		ArgumentNullException.ThrowIfNull(into, "into");
		into.Clear();
		lock (Gate)
		{
			foreach (var value2 in Live.Values)
			{
				string key = EffectiveLabel(value2.Category, value2.Label);
				into.TryGetValue(key, out var value);
				into[key] = new LabelStat(value.Bytes + value2.Bytes, value.Count + 1, (value.Count == 0L) ? value2.Detail : null);
			}
		}
	}

	public static void CollectAllocations(string effectiveLabel, List<TrackedAllocation> into)
	{
		ArgumentNullException.ThrowIfNull(into, "into");
		into.Clear();
		lock (Gate)
		{
			foreach (var (id, tuple2) in Live)
			{
				if (EffectiveLabel(tuple2.Item1, tuple2.Item3) == effectiveLabel)
				{
					into.Add(new TrackedAllocation(tuple2.Item1, tuple2.Item2, tuple2.Item4, id, tuple2.Item5));
				}
			}
		}
	}

	public static void CollectAll(List<TrackedAllocation> into)
	{
		ArgumentNullException.ThrowIfNull(into, "into");
		into.Clear();
		lock (Gate)
		{
			foreach (var (id, tuple2) in Live)
			{
				into.Add(new TrackedAllocation(tuple2.Item1, tuple2.Item2, tuple2.Item4 ?? tuple2.Item3, id, tuple2.Item5));
			}
		}
	}

	public static void SnapshotAllocations(Dictionary<AllocKey, AllocationSnapshot> into)
	{
		ArgumentNullException.ThrowIfNull(into, "into");
		into.Clear();
		lock (Gate)
		{
			foreach (KeyValuePair<AllocKey, (GpuMemoryCategory, long, string, string, string)> item in Live)
			{
				item.Deconstruct(out var key, out var value);
				AllocKey key2 = key;
				(GpuMemoryCategory, long, string, string, string) tuple = value;
				into[key2] = new AllocationSnapshot(EffectiveLabel(tuple.Item1, tuple.Item3), tuple.Item2, tuple.Item4);
			}
		}
	}

	public static void CollectAllocationsByCategory(ReadOnlySpan<GpuMemoryCategory> categories, List<TrackedAllocation> into)
	{
		ArgumentNullException.ThrowIfNull(into, "into");
		into.Clear();
		lock (Gate)
		{
			foreach (KeyValuePair<AllocKey, (GpuMemoryCategory, long, string, string, string)> item in Live)
			{
				item.Deconstruct(out var key, out var value);
				AllocKey id = key;
				(GpuMemoryCategory, long, string, string, string) tuple = value;
				for (int i = 0; i < categories.Length; i++)
				{
					if (tuple.Item1 == categories[i])
					{
						into.Add(new TrackedAllocation(tuple.Item1, tuple.Item2, tuple.Item4 ?? tuple.Item3, id));
						break;
					}
				}
			}
		}
	}
}
