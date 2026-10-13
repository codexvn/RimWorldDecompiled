using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Waylith.Profiling;

public static class ProfilerPathTable
{
	public struct Entry
	{
		public int parentPathId;

		public ProfilerLabelId labelId;

		public int foldedPathId;

		public int firstChild;

		public int nextSibling;
	}

	public const int VirtualRoot = 0;

	public const int Overflow = 1;

	public const int Capacity = 2097152;

	private const int None = -1;

	private const int LookupBytesPerEntry = 32;

	private static Entry[] entries = CreateEntries(1024);

	private static int count = 2;

	private static readonly Dictionary<long, int> Lookup = new Dictionary<long, int>();

	public static int Count => Volatile.Read(in count);

	public static Entry[] Entries => Volatile.Read(in entries);

	public static long EstimateBytes()
	{
		return (long)Volatile.Read(in entries).Length * (long)Unsafe.SizeOf<Entry>() + (long)Volatile.Read(in count) * 32L;
	}

	internal static int GetOrAdd(int parentPathId, ProfilerLabelId labelId)
	{
		long key = ((long)parentPathId << 16) | labelId.Value;
		if (Lookup.TryGetValue(key, out var value))
		{
			return value;
		}
		if (count + 2 > 2097152)
		{
			return 1;
		}
		Entry entry = entries[parentPathId];
		int foldedPathId = ((labelId == entry.labelId) ? entry.foldedPathId : ((entry.foldedPathId != parentPathId) ? GetOrAdd(entry.foldedPathId, labelId) : count));
		value = count;
		if (value == entries.Length)
		{
			Entry[] array = new Entry[System.Math.Min(2097152, entries.Length * 2)];
			Array.Copy(entries, array, entries.Length);
			Volatile.Write(ref entries, array);
		}
		ref Entry reference = ref entries[parentPathId];
		entries[value] = new Entry
		{
			parentPathId = parentPathId,
			labelId = labelId,
			foldedPathId = foldedPathId,
			firstChild = -1,
			nextSibling = reference.firstChild
		};
		reference.firstChild = value;
		Lookup.Add(key, value);
		Volatile.Write(ref count, value + 1);
		return value;
	}

	private static Entry[] CreateEntries(int capacity)
	{
		Entry[] array = new Entry[capacity];
		array[0] = new Entry
		{
			parentPathId = -1,
			labelId = ProfilerLabelId.None,
			foldedPathId = 0,
			firstChild = 1,
			nextSibling = -1
		};
		array[1] = new Entry
		{
			parentPathId = 0,
			labelId = FrameProfiler.OverflowLabelId,
			foldedPathId = 1,
			firstChild = -1,
			nextSibling = -1
		};
		return array;
	}
}
