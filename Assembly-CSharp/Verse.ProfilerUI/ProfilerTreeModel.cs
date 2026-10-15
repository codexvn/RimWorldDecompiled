using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using Waylith.Profiling;

namespace Verse.ProfilerUI;

public sealed class ProfilerTreeModel
{
	public enum SortColumn
	{
		Total,
		Self,
		Calls,
		Alloc,
		Label
	}

	public sealed class Row
	{
		public ProfilerLabelId labelId;

		public int depth;

		public ulong pathKey;

		public float totalMicroseconds;

		public float selfMicroseconds;

		public int calls;

		public long allocBytes;

		public bool hasChildren;

		public bool expanded;

		public float baselineTotalMicroseconds = float.NaN;

		internal ulong parentKey;

		public readonly List<int> pathIds = new List<int>();

		public bool CanDrill => labelId.IsValid;

		public bool HasBaseline => !float.IsNaN(baselineTotalMicroseconds);

		internal void Reset(ProfilerLabelId labelId)
		{
			this.labelId = labelId;
			depth = 0;
			pathKey = 0uL;
			totalMicroseconds = 0f;
			selfMicroseconds = 0f;
			calls = 0;
			allocBytes = 0L;
			hasChildren = false;
			expanded = false;
			baselineTotalMicroseconds = float.NaN;
			parentKey = 0uL;
			pathIds.Clear();
		}
	}

	public struct WindowSummary
	{
		public int framesIncluded;

		public float averageFrameMs;

		public float averageTickerDelta;

		public float averageNodeCount;

		public float averageOverflowedScopes;

		public long averageAllocBytes;

		public float averageAllocCount;

		public int gen0Collections;

		public int gen1Collections;

		public int gen2Collections;

		public long averagePromotedBytes;
	}

	private enum View
	{
		Flamegraph,
		CallTree,
		Pie
	}

	private enum Descent
	{
		None,
		Expanded,
		Full,
		All
	}

	public const ulong RootPathKey = 14695981039346656037uL;

	private readonly Comparison<Row> rowComparison;

	public readonly List<ProfilerLabelId> Path = new List<ProfilerLabelId>();

	public readonly List<Row> DisplayRows = new List<Row>();

	private List<Row> emitTarget;

	public long clipStartNs = -1L;

	public long clipEndNs = -1L;

	private ProfilerWindowRequest pending = new ProfilerWindowRequest
	{
		followNewest = true,
		newestOrdinal = -1,
		oldestOrdinal = -1
	};

	private ProfilerWindowRequest submitted;

	private int[] submittedThreadIds;

	private int submittedThreadVersion = -1;

	private int submittedSelectionVersion = -1;

	private readonly List<int> threadIdScratch = new List<int>();

	private readonly Stack<Row> rowPool = new Stack<Row>();

	private readonly Stack<List<Row>> rowListPool = new Stack<List<Row>>();

	private readonly Dictionary<ProfilerLabelId, int> levelLookup = new Dictionary<ProfilerLabelId, int>();

	private readonly HashSet<ulong> expandedKeys = new HashSet<ulong>();

	private bool expandAllPending;

	private readonly HashSet<ulong> subtreePendingKeys = new HashSet<ulong>();

	public const ulong InvertedRootPathKey = 88172645463325252uL;

	private bool foldRecursion = true;

	private ProfilerWindowResult result;

	private ProfilerPathTable.Entry[] pathTable = Array.Empty<ProfilerPathTable.Entry>();

	private int pathTableCount;

	private readonly List<int> rootPathScratch = new List<int>();

	private readonly List<int> drillPathScratch = new List<int>();

	private const int LevelLookupThreshold = 16;

	private const float InvertedMinRowFraction = 0.00024414062f;

	private readonly Dictionary<ulong, Row> rowsByInvertedKey = new Dictionary<ulong, Row>();

	private readonly Dictionary<ulong, (int start, int end)> childRangesByParentKey = new Dictionary<ulong, (int, int)>();

	private List<(int leafPathId, Row row, ulong key, int callerPathId)> invertedChains = new List<(int, Row, ulong, int)>();

	private List<(int leafPathId, Row row, ulong key, int callerPathId)> invertedChainsNext = new List<(int, Row, ulong, int)>();

	private ulong[] siblingSortKeys = Array.Empty<ulong>();

	private Row[] siblingSortRows = Array.Empty<Row>();

	public const string UnprofiledLabel = "Unprofiled";

	public SortColumn ActiveSortColumn { get; set; }

	public bool SortAscending { get; set; }

	public ProfilerWindowRequest EmittedRequest { get; private set; }

	public ProfilerWindowRequest SubmittedRequest => submitted;

	public int ExpansionVersion { get; private set; }

	public bool Inverted { get; set; }

	public bool FoldRecursion
	{
		get
		{
			return foldRecursion;
		}
		set
		{
			if (foldRecursion != value)
			{
				foldRecursion = value;
				ExpansionVersion++;
			}
		}
	}

	public static ulong CombineKey(ulong parentKey, ProfilerLabelId labelId)
	{
		return (parentKey ^ (uint)(labelId.Value + 2)) * 1099511628211L;
	}

	public ProfilerTreeModel()
	{
		rowComparison = CompareRows;
		emitTarget = DisplayRows;
	}

	private int CompareRows(Row lhs, Row rhs)
	{
		int num = ActiveSortColumn switch
		{
			SortColumn.Self => lhs.selfMicroseconds.CompareTo(rhs.selfMicroseconds), 
			SortColumn.Calls => lhs.calls.CompareTo(rhs.calls), 
			SortColumn.Alloc => lhs.allocBytes.CompareTo(rhs.allocBytes), 
			SortColumn.Label => string.CompareOrdinal(LabelForRow(lhs), LabelForRow(rhs)), 
			_ => lhs.totalMicroseconds.CompareTo(rhs.totalMicroseconds), 
		};
		if (!SortAscending)
		{
			num = -num;
		}
		if (num != 0)
		{
			return num;
		}
		return string.CompareOrdinal(LabelForRow(lhs), LabelForRow(rhs));
	}

	public void SetWindow(bool spikes, bool followNewest, int newestOrdinal, int oldestOrdinal, FrameProfiler.FrameSnapshot fallback)
	{
		pending.spikes = spikes;
		pending.followNewest = followNewest;
		pending.newestOrdinal = newestOrdinal;
		pending.oldestOrdinal = oldestOrdinal;
		pending.fallbackSnapshot = fallback;
	}

	private int[] SelectedThreadIds()
	{
		if (submittedThreadVersion == ProfilerThreadRegistry.Version && submittedSelectionVersion == ProfilerThreadRegistry.SelectionVersion)
		{
			return submittedThreadIds;
		}
		submittedThreadVersion = ProfilerThreadRegistry.Version;
		submittedSelectionVersion = ProfilerThreadRegistry.SelectionVersion;
		if (ProfilerThreadRegistry.AllSelected)
		{
			return submittedThreadIds = null;
		}
		threadIdScratch.Clear();
		foreach (ProfilerThreadRegistry.Entry entry in ProfilerThreadRegistry.Entries)
		{
			if (ProfilerThreadRegistry.IsSelected(entry.threadId))
			{
				threadIdScratch.Add(entry.threadId);
			}
		}
		threadIdScratch.Sort();
		if (submittedThreadIds != null && CollectionsMarshal.AsSpan(threadIdScratch).SequenceEqual(submittedThreadIds))
		{
			return submittedThreadIds;
		}
		return submittedThreadIds = threadIdScratch.ToArray();
	}

	private void SubmitRequestIfChanged()
	{
		pending.clipStartNs = clipStartNs;
		pending.clipEndNs = clipEndNs;
		pending.foldRecursion = foldRecursion;
		pending.selectedThreadIds = SelectedThreadIds();
		if (submitted == null || !submitted.SameWindowAs(pending))
		{
			submitted = pending;
			pending = submitted.Clone();
			ProfilerWindowAggregator.Submit(submitted);
		}
	}

	private Row RentRow(ProfilerLabelId labelId)
	{
		Row obj = ((rowPool.Count > 0) ? rowPool.Pop() : new Row());
		obj.Reset(labelId);
		return obj;
	}

	public void ReleaseRows(List<Row> rows)
	{
		for (int i = 0; i < rows.Count; i++)
		{
			rowPool.Push(rows[i]);
		}
		rows.Clear();
	}

	private List<Row> RentRowList()
	{
		if (rowListPool.Count <= 0)
		{
			return new List<Row>();
		}
		return rowListPool.Pop();
	}

	private void ReturnRowList(List<Row> list)
	{
		list.Clear();
		rowListPool.Push(list);
	}

	public void SetExpanded(ulong pathKey, bool expanded)
	{
		if (expanded ? expandedKeys.Add(pathKey) : expandedKeys.Remove(pathKey))
		{
			ExpansionVersion++;
		}
	}

	public void ExpandAll()
	{
		expandAllPending = true;
		ExpansionVersion++;
	}

	public void ExpandSubtree(ulong pathKey)
	{
		expandedKeys.Add(pathKey);
		subtreePendingKeys.Add(pathKey);
		ExpansionVersion++;
	}

	public void CollapseAll()
	{
		expandAllPending = false;
		if (expandedKeys.Count != 0)
		{
			expandedKeys.Clear();
			ExpansionVersion++;
		}
	}

	private bool IsExpanded(ulong pathKey)
	{
		if (!expandAllPending)
		{
			return expandedKeys.Contains(pathKey);
		}
		return true;
	}

	public void BuildAllLevelRows()
	{
		Build(View.Flamegraph);
	}

	public void BuildTreeRows()
	{
		Build(View.CallTree);
	}

	public void BuildPieRows()
	{
		Build(View.Pie);
	}

	private void Build(View view)
	{
		ReleaseRows(DisplayRows);
		SubmitRequestIfChanged();
		using (ProfilerWindowAggregator.ResultLock.EnterScope())
		{
			EmittedRequest = (BeginEmit() ? result.request : null);
			if (EmittedRequest == null)
			{
				return;
			}
			int divisor = Math.Max(1, result.FramesIncluded);
			bool flag = Inverted && view != View.Pie;
			if (flag)
			{
				EmitInverted(divisor, view == View.CallTree);
			}
			else
			{
				CollectRoots(rootPathScratch);
				List<int> list = rootPathScratch;
				ulong key = 14695981039346656037uL;
				Descent descent = Descent.All;
				switch (view)
				{
				case View.CallTree:
					descent = Descent.Expanded;
					break;
				case View.Pie:
					descent = Descent.None;
					list = ResolveDrillPath(list, ref key);
					break;
				}
				EmitLevel(list, key, 0, divisor, unprofiled: true, descent);
			}
			if (view == View.CallTree)
			{
				ApplyBaseline(flag);
			}
		}
		expandAllPending = false;
		subtreePendingKeys.Clear();
	}

	public void BuildFullTreeRows(List<Row> into)
	{
		using (ProfilerWindowAggregator.ResultLock.EnterScope())
		{
			if (!BeginEmit())
			{
				return;
			}
			int divisor = Math.Max(1, result.FramesIncluded);
			emitTarget = into;
			try
			{
				if (Inverted)
				{
					EmitInverted(divisor, gate: false, prune: false);
					return;
				}
				CollectRoots(rootPathScratch);
				EmitLevel(rootPathScratch, 14695981039346656037uL, 0, divisor, unprofiled: true, Descent.Full);
			}
			finally
			{
				emitTarget = DisplayRows;
			}
		}
	}

	private bool BeginEmit()
	{
		result = ProfilerWindowAggregator.Current;
		if (result.request == null)
		{
			return false;
		}
		pathTable = ProfilerPathTable.Entries;
		pathTableCount = Math.Min(ProfilerPathTable.Count, result.byPathId.Length);
		return true;
	}

	public void CollectMatchingPaths(ProfilerSearchModel search, List<(float totalMicroseconds, int pathId)> into)
	{
		CollectPaths(into, (ProfilerPathTable.Entry[] table, int pathId) => table[pathId].parentPathId != 0 && search.Matches(table[pathId].labelId));
	}

	public void CollectPathsWithLabel(ProfilerLabelId labelId, List<(float totalMicroseconds, int pathId)> into)
	{
		CollectPaths(into, (ProfilerPathTable.Entry[] table, int pathId) => table[pathId].labelId == labelId);
	}

	private void CollectPaths(List<(float totalMicroseconds, int pathId)> into, Func<ProfilerPathTable.Entry[], int, bool> matches)
	{
		into.Clear();
		using (ProfilerWindowAggregator.ResultLock.EnterScope())
		{
			if (!BeginEmit())
			{
				return;
			}
			for (int i = 0; i < result.touchedCount; i++)
			{
				int num = result.touchedPathIds[i];
				if (Present(num) && matches(pathTable, num))
				{
					into.Add(((float)result.byPathId[num].totalMicroseconds, num));
				}
			}
		}
	}

	public void AppendLabelChain(int pathId, List<ProfilerLabelId> into)
	{
		into.Clear();
		ProfilerPathTable.Entry[] entries = ProfilerPathTable.Entries;
		if ((uint)pathId < (uint)ProfilerPathTable.Count)
		{
			if (foldRecursion)
			{
				pathId = entries[pathId].foldedPathId;
			}
			int num = pathId;
			while (entries[num].parentPathId != 0)
			{
				into.Add(entries[num].labelId);
				num = entries[num].parentPathId;
			}
			into.Reverse();
		}
	}

	private bool Present(int pathId)
	{
		if ((uint)pathId < (uint)pathTableCount && result.IsListed(pathId))
		{
			return result.byPathId[pathId].entries > 0;
		}
		return false;
	}

	private void CollectRoots(List<int> into)
	{
		into.Clear();
		for (int num = pathTable[0].firstChild; num >= 0; num = pathTable[num].nextSibling)
		{
			if (Present(num))
			{
				into.Add(num);
			}
		}
	}

	private bool HasPresentChild(List<int> parents)
	{
		for (int i = 0; i < parents.Count; i++)
		{
			for (int num = pathTable[parents[i]].firstChild; num >= 0; num = pathTable[num].nextSibling)
			{
				if (Present(num))
				{
					return true;
				}
			}
		}
		return false;
	}

	private List<int> ResolveDrillPath(List<int> roots, ref ulong key)
	{
		List<int> list = roots;
		for (int i = 0; i < Path.Count; i++)
		{
			ProfilerLabelId profilerLabelId = Path[i];
			List<int> list2 = ((list == drillPathScratch) ? rootPathScratch : drillPathScratch);
			list2.Clear();
			for (int j = 0; j < list.Count; j++)
			{
				for (int num = pathTable[list[j]].firstChild; num >= 0; num = pathTable[num].nextSibling)
				{
					if (pathTable[num].labelId == profilerLabelId && Present(num))
					{
						list2.Add(num);
					}
				}
			}
			if (list2.Count == 0)
			{
				Path.RemoveRange(i, Path.Count - i);
				break;
			}
			list = list2;
			key = CombineKey(key, profilerLabelId);
		}
		return list;
	}

	private int EmitLevel(List<int> parents, ulong parentKey, int depth, int divisor, bool unprofiled, Descent descent)
	{
		List<Row> list = RentRowList();
		double num = 0.0;
		long num2 = 0L;
		for (int i = 0; i < parents.Count; i++)
		{
			int num3 = parents[i];
			if (unprofiled)
			{
				ref PathAccumulator reference = ref result.byPathId[num3];
				num += reference.selfMicroseconds;
				num2 += reference.selfAllocBytes;
			}
			for (int num4 = pathTable[num3].firstChild; num4 >= 0; num4 = pathTable[num4].nextSibling)
			{
				if (Present(num4))
				{
					ProfilerLabelId labelId = pathTable[num4].labelId;
					int num5 = FindLevelRow(list, labelId);
					if (num5 < 0)
					{
						num5 = list.Count;
						Row row = RentRow(labelId);
						row.pathKey = CombineKey(parentKey, labelId);
						row.parentKey = parentKey;
						PropagateSubtreePending(parentKey, row.pathKey);
						list.Add(row);
						IndexLevelRow(list, labelId, num5);
					}
					ref PathAccumulator reference2 = ref result.byPathId[num4];
					Row row2 = list[num5];
					row2.pathIds.Add(num4);
					row2.totalMicroseconds += (float)reference2.totalMicroseconds;
					row2.selfMicroseconds += (float)reference2.selfMicroseconds;
					row2.calls += reference2.calls;
					row2.allocBytes += reference2.allocBytes;
				}
			}
		}
		if (unprofiled && (num > 0.0 || num2 > 0))
		{
			Row row3 = RentRow(ProfilerLabelId.None);
			row3.pathKey = CombineKey(parentKey, ProfilerLabelId.None);
			row3.parentKey = parentKey;
			row3.totalMicroseconds = (float)num;
			row3.selfMicroseconds = (float)num;
			row3.allocBytes = num2;
			list.Add(row3);
		}
		DivideRows(list, divisor);
		list.Sort(rowComparison);
		int count = list.Count;
		for (int j = 0; j < count; j++)
		{
			Row row4 = list[j];
			row4.depth = depth;
			emitTarget.Add(row4);
			if (!row4.labelId.IsValid)
			{
				continue;
			}
			switch (descent)
			{
			case Descent.All:
				if (EmitLevel(row4.pathIds, row4.pathKey, depth + 1, divisor, unprofiled: false, descent) > 0)
				{
					row4.hasChildren = true;
					MarkExpanded(row4);
				}
				break;
			case Descent.Expanded:
			case Descent.Full:
				row4.hasChildren = HasPresentChild(row4.pathIds);
				if (row4.hasChildren && (descent == Descent.Full || IsExpanded(row4.pathKey)))
				{
					MarkExpanded(row4);
					EmitLevel(row4.pathIds, row4.pathKey, depth + 1, divisor, unprofiled: true, descent);
				}
				break;
			case Descent.None:
				row4.hasChildren = HasPresentChild(row4.pathIds);
				break;
			}
		}
		ReturnRowList(list);
		return count;
	}

	private int FindLevelRow(List<Row> rows, ProfilerLabelId labelId)
	{
		if (rows.Count > 16)
		{
			if (!levelLookup.TryGetValue(labelId, out var value))
			{
				return -1;
			}
			return value;
		}
		for (int i = 0; i < rows.Count; i++)
		{
			if (rows[i].labelId == labelId)
			{
				return i;
			}
		}
		return -1;
	}

	private void IndexLevelRow(List<Row> rows, ProfilerLabelId labelId, int rowIndex)
	{
		if (rows.Count <= 16)
		{
			return;
		}
		if (rows.Count == 17)
		{
			levelLookup.Clear();
			for (int i = 0; i < rows.Count; i++)
			{
				levelLookup.Add(rows[i].labelId, i);
			}
		}
		else
		{
			levelLookup.Add(labelId, rowIndex);
		}
	}

	private void DivideRows(List<Row> rows, int divisor)
	{
		if (divisor > 1)
		{
			for (int i = 0; i < rows.Count; i++)
			{
				Row row = rows[i];
				row.totalMicroseconds /= divisor;
				row.selfMicroseconds /= divisor;
				row.calls = Mathf.RoundToInt((float)row.calls / (float)divisor);
				row.allocBytes /= divisor;
			}
		}
	}

	private void PropagateSubtreePending(ulong parentKey, ulong pathKey)
	{
		if (subtreePendingKeys.Count != 0 && subtreePendingKeys.Contains(parentKey))
		{
			subtreePendingKeys.Add(pathKey);
			expandedKeys.Add(pathKey);
		}
	}

	private void MarkExpanded(Row row)
	{
		row.expanded = true;
		if (expandAllPending)
		{
			expandedKeys.Add(row.pathKey);
		}
	}

	public ulong ExpandTo(List<ProfilerLabelId> pathLabelIds)
	{
		ulong num = 14695981039346656037uL;
		for (int i = 0; i < pathLabelIds.Count; i++)
		{
			if (i > 0)
			{
				expandedKeys.Add(num);
			}
			num = CombineKey(num, pathLabelIds[i]);
		}
		ExpansionVersion++;
		return num;
	}

	private Row InvertedRowFor(List<Row> rows, ulong key, ulong parentKey, ProfilerLabelId labelId)
	{
		if (rowsByInvertedKey.TryGetValue(key, out var value))
		{
			return value;
		}
		value = RentRow(labelId);
		value.pathKey = key;
		value.parentKey = parentKey;
		PropagateSubtreePending(parentKey, key);
		rowsByInvertedKey.Add(key, value);
		rows.Add(value);
		return value;
	}

	private static void AccumulateInverted(Row row, in PathAccumulator leaf)
	{
		float num = (float)leaf.selfMicroseconds;
		row.totalMicroseconds += num;
		row.selfMicroseconds += num;
		row.allocBytes += leaf.selfAllocBytes;
	}

	private void EmitInverted(int divisor, bool gate, bool prune = true)
	{
		List<Row> list = RentRowList();
		rowsByInvertedKey.Clear();
		List<(int, Row, ulong, int)> list2 = invertedChains;
		list2.Clear();
		float num = 0f;
		for (int i = 0; i < result.touchedCount; i++)
		{
			int num2 = result.touchedPathIds[i];
			if (Present(num2))
			{
				ref PathAccumulator reference = ref result.byPathId[num2];
				int parentPathId = pathTable[num2].parentPathId;
				if (parentPathId != 0 && (!(reference.selfMicroseconds <= 0.0) || reference.selfAllocBytes > 0))
				{
					ProfilerLabelId labelId = pathTable[num2].labelId;
					ulong num3 = CombineKey(88172645463325252uL, labelId);
					Row row = InvertedRowFor(list, num3, 88172645463325252uL, labelId);
					row.pathIds.Add(num2);
					AccumulateInverted(row, in reference);
					row.calls += reference.calls;
					num += (float)reference.selfMicroseconds;
					list2.Add((num2, row, num3, parentPathId));
				}
			}
		}
		float num4 = (prune ? (num * 0.00024414062f) : 0f);
		List<(int, Row, ulong, int)> list3 = invertedChainsNext;
		while (list2.Count > 0)
		{
			list3.Clear();
			for (int j = 0; j < list2.Count; j++)
			{
				var (num5, row2, parentKey, num6) = list2[j];
				if (num6 == 0)
				{
					continue;
				}
				if (gate)
				{
					row2.hasChildren = true;
					if (!IsExpanded(row2.pathKey))
					{
						continue;
					}
				}
				else if (row2.totalMicroseconds < num4)
				{
					continue;
				}
				ProfilerLabelId labelId2 = pathTable[num6].labelId;
				ulong num7 = CombineKey(parentKey, labelId2);
				Row row3 = InvertedRowFor(list, num7, parentKey, labelId2);
				row3.pathIds.Add(num5);
				AccumulateInverted(row3, in result.byPathId[num5]);
				row2.hasChildren = true;
				list3.Add((num5, row3, num7, pathTable[num6].parentPathId));
			}
			List<(int, Row, ulong, int)> list4 = list3;
			list3 = list2;
			list2 = list4;
		}
		invertedChains = list2;
		invertedChainsNext = list3;
		DivideRows(list, divisor);
		EmitPathRows(list, 88172645463325252uL);
		ReturnRowList(list);
	}

	private void EmitPathRows(List<Row> rows, ulong rootKey)
	{
		int count = rows.Count;
		if (siblingSortKeys.Length < count)
		{
			siblingSortKeys = new ulong[Math.Max(count, siblingSortKeys.Length * 2)];
			siblingSortRows = new Row[siblingSortKeys.Length];
		}
		for (int i = 0; i < count; i++)
		{
			siblingSortKeys[i] = rows[i].parentKey;
			siblingSortRows[i] = rows[i];
		}
		Array.Sort(siblingSortKeys, siblingSortRows, 0, count);
		childRangesByParentKey.Clear();
		int num = 0;
		while (num < count)
		{
			ulong num2 = siblingSortKeys[num];
			int j;
			for (j = num + 1; j < count && siblingSortKeys[j] == num2; j++)
			{
			}
			if (j - num > 1)
			{
				siblingSortRows.AsSpan(num, j - num).Sort(rowComparison);
			}
			childRangesByParentKey.Add(num2, (num, j));
			num = j;
		}
		EmitPathLevel(siblingSortRows, rootKey, 0);
		Array.Clear(siblingSortRows, 0, count);
	}

	private void EmitPathLevel(Row[] rows, ulong parentKey, int depth)
	{
		if (!childRangesByParentKey.TryGetValue(parentKey, out (int, int) value))
		{
			return;
		}
		var (i, _) = value;
		for (; i < value.Item2; i++)
		{
			Row row = rows[i];
			row.depth = depth;
			emitTarget.Add(row);
			if (row.labelId.IsValid && childRangesByParentKey.ContainsKey(row.pathKey))
			{
				MarkExpanded(row);
				EmitPathLevel(rows, row.pathKey, depth + 1);
			}
		}
	}

	private void ApplyBaseline(bool inverted)
	{
		if (!result.hasBaseline)
		{
			return;
		}
		float[] array = (inverted ? result.baselineSelfByPathId : result.baselineTotalByPathId);
		for (int i = 0; i < DisplayRows.Count; i++)
		{
			Row row = DisplayRows[i];
			if (!row.labelId.IsValid)
			{
				continue;
			}
			float num = 0f;
			bool flag = false;
			for (int j = 0; j < row.pathIds.Count; j++)
			{
				int num2 = row.pathIds[j];
				if ((uint)num2 < (uint)array.Length && !float.IsNaN(array[num2]))
				{
					num += array[num2];
					flag = true;
				}
			}
			row.baselineTotalMicroseconds = (flag ? num : float.NaN);
		}
	}

	public static string LabelForRow(Row row)
	{
		if (!row.labelId.IsValid)
		{
			return "Unprofiled";
		}
		return FrameProfiler.GetLabel(row.labelId);
	}

	public static void AppendAllocBytes(StringBuilder builder, long bytes, string separator = " ")
	{
		if (bytes > 0)
		{
			if (bytes < 1024)
			{
				builder.Append($"{bytes}{separator}B");
			}
			else if (bytes < 1048576)
			{
				builder.Append($"{(float)bytes / 1024f:0.0}{separator}KB");
			}
			else
			{
				builder.Append($"{(float)bytes / 1048576f:0.00}{separator}MB");
			}
		}
	}

	public static void AppendMs(StringBuilder builder, float ms, string format = "0.0", string separator = " ")
	{
		float num = Mathf.Abs(ms);
		float num2 = ms;
		string value;
		if (num >= 1000f)
		{
			num2 = ms / 1000f;
			value = "s";
			format = "0.00";
		}
		else if (num >= 1f)
		{
			value = "ms";
		}
		else if (num >= 0.001f)
		{
			num2 = ms * 1000f;
			value = "µs";
			format = "0.0";
		}
		else if (num > 0f)
		{
			num2 = ms * 1000000f;
			value = "ns";
			format = ((num2 >= 10f || num2 <= -10f) ? "0" : "0.##");
		}
		else
		{
			value = "ms";
			format = "0";
		}
		Span<char> destination = stackalloc char[64];
		if (num2.TryFormat(destination, out var charsWritten, format))
		{
			builder.Append(destination.Slice(0, charsWritten));
		}
		else
		{
			builder.Append(num2);
		}
		builder.Append(separator).Append(value);
	}

	public static void AppendFrameMs(StringBuilder builder, float ms, string format = "0.0")
	{
		AppendMs(builder, ms, format);
		builder.Append(" (").Append((ms > 0f) ? Mathf.RoundToInt(1000f / ms) : 0).Append(" FPS)");
	}
}
