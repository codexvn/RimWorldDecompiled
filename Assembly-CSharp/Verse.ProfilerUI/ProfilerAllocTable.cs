using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Waylith.Profiling;

namespace Verse.ProfilerUI;

public sealed class ProfilerAllocTable : ProfilerRowSection
{
	private sealed class ZoneRow
	{
		public ProfilerLabelId labelId;

		public string typeName;

		public long bytes;

		public long count;
	}

	private sealed class TypeRow
	{
		public string name;

		public long bytes;

		public long count;

		public readonly List<ZoneRow> zones = new List<ZoneRow>();
	}

	private static readonly Comparison<TypeRow> TypeRowComparison = (TypeRow lhs, TypeRow rhs) => rhs.bytes.CompareTo(lhs.bytes);

	private static readonly Comparison<ZoneRow> ZoneRowComparison = (ZoneRow lhs, ZoneRow rhs) => rhs.bytes.CompareTo(lhs.bytes);

	private readonly List<FrameProfiler.FrameSnapshot> frames = new List<FrameProfiler.FrameSnapshot>();

	private readonly Dictionary<Type, int> typeLookup = new Dictionary<Type, int>();

	private readonly Dictionary<string, int> rowLookup = new Dictionary<string, int>();

	private readonly Dictionary<(int typeIndex, ProfilerLabelId labelId), int> zoneLookup = new Dictionary<(int, ProfilerLabelId), int>();

	private readonly List<TypeRow> aggregated = new List<TypeRow>();

	private readonly List<TypeRow> invertedScratch = new List<TypeRow>();

	private readonly HashSet<string> expandedTypes = new HashSet<string>();

	private readonly List<(int typeIndex, int zoneIndex)> displayRows = new List<(int, int)>();

	private readonly Stack<TypeRow> typeRowPool = new Stack<TypeRow>();

	private readonly Stack<ZoneRow> zoneRowPool = new Stack<ZoneRow>();

	private ProfilerRefreshGate refreshGate;

	private bool lastTracking;

	private bool expandMatchesOnRefresh;

	private bool inverted;

	private long overflowCount;

	private long overflowBytes;

	private long untrackedBytes;

	private string footer = "";

	public Action<ProfilerLabelId> ZoneJumpRequested;

	public Action EnableTrackingRequested;

	public bool IsInverted
	{
		get
		{
			return inverted;
		}
		set
		{
			if (value != inverted)
			{
				inverted = value;
				expandedTypes.Clear();
				View.SelectQuietly(-1);
				Invalidate();
			}
		}
	}

	private string GroupingHeader
	{
		get
		{
			if (!inverted)
			{
				return "type / zone";
			}
			return "zone / type";
		}
	}

	protected override string CopyHeader => "%\tbytes/frame\tobjs/frame\t" + GroupingHeader;

	protected override float FooterHeight
	{
		get
		{
			if (footer.Length <= 0)
			{
				return 0f;
			}
			return 22f;
		}
	}

	public ProfilerAllocTable(ProfilerSession session)
		: base(session)
	{
		View.Columns = new ProfilerColumn[3]
		{
			new ProfilerColumn(ProfilerColumnKind.Percent, 44f, () => "%"),
			new ProfilerColumn(ProfilerColumnKind.Total, 100f, () => "bytes/frame"),
			new ProfilerColumn(ProfilerColumnKind.Calls, 90f, () => "objs/frame")
		};
		View.LabelHeader = () => GroupingHeader;
		View.RootCrumb = ProfilerCallTreeTable.RootCrumbLabel;
		View.ActivateLeaf = ActivateLeafAt;
	}

	public override void Invalidate()
	{
		refreshGate.Invalidate();
	}

	protected override void AppendCopyContext(StringBuilder builder)
	{
		if (footer.Length > 0)
		{
			builder.AppendLine(footer);
		}
	}

	private void ActivateLeafAt(int index)
	{
		var (index2, num) = displayRows[index];
		if (num >= 0)
		{
			ZoneJumpRequested?.Invoke(aggregated[index2].zones[num].labelId);
		}
	}

	public override void Refresh()
	{
		bool flag = lastTracking != FrameProfiler.AllocationTracking;
		if (!refreshGate.ShouldRefresh(session) && !flag)
		{
			return;
		}
		lastTracking = FrameProfiler.AllocationTracking;
		session.CollectFrames(session.Span, frames);
		Aggregate();
		if (inverted)
		{
			InvertAggregation();
		}
		bool num = expandMatchesOnRefresh;
		if (num)
		{
			expandMatchesOnRefresh = false;
			if (session.Search.IsActive)
			{
				ExpandSearchMatches();
			}
		}
		SyncRows();
		if (num && session.Search.IsActive)
		{
			View.SelectFirstMatch();
		}
		WriteFooter();
	}

	private void WriteFooter()
	{
		if (aggregated.Count == 0 || (overflowCount <= 0 && overflowBytes <= 0 && untrackedBytes <= 0))
		{
			footer = "";
			return;
		}
		StringBuilder scratch = ProfilerGui.Scratch;
		scratch.Clear();
		if (overflowBytes > 0 || overflowCount > 0)
		{
			scratch.Append("over table capacity: ");
			ProfilerTreeModel.AppendAllocBytes(scratch, overflowBytes);
			scratch.Append(" (").Append(overflowCount.ToString("n0")).Append(" objs)/frame");
		}
		if (untrackedBytes > 0)
		{
			if (scratch.Length > 0)
			{
				scratch.Append(" / ");
			}
			scratch.Append("untracked: ");
			ProfilerTreeModel.AppendAllocBytes(scratch, untrackedBytes);
			scratch.Append("/frame");
		}
		footer = scratch.ToString();
	}

	public override void Notify_SearchChanged()
	{
		expandMatchesOnRefresh = true;
		Invalidate();
	}

	private bool ZoneMatches(ZoneRow zone)
	{
		if (zone.typeName == null)
		{
			return session.Search.Matches(zone.labelId);
		}
		return session.Search.Matches(zone.typeName);
	}

	private void ExpandSearchMatches()
	{
		for (int i = 0; i < aggregated.Count; i++)
		{
			TypeRow typeRow = aggregated[i];
			for (int j = 0; j < typeRow.zones.Count; j++)
			{
				if (ZoneMatches(typeRow.zones[j]))
				{
					expandedTypes.Add(typeRow.name);
					break;
				}
			}
		}
	}

	public override void ExpandAll()
	{
		for (int i = 0; i < aggregated.Count; i++)
		{
			if (aggregated[i].zones.Count > 0)
			{
				expandedTypes.Add(aggregated[i].name);
			}
		}
		SyncRows();
	}

	public override void CollapseAll()
	{
		expandedTypes.Clear();
		SyncRows();
	}

	protected override void ToggleExpansionAt(int index)
	{
		if (index < 0 || index >= displayRows.Count)
		{
			return;
		}
		(int typeIndex, int zoneIndex) tuple = displayRows[index];
		var (index2, _) = tuple;
		if (tuple.zoneIndex < 0)
		{
			View.Select(index);
			string name = aggregated[index2].name;
			if (!expandedTypes.Add(name))
			{
				expandedTypes.Remove(name);
			}
			SyncRows();
		}
	}

	private TypeRow RentTypeRow(string name)
	{
		TypeRow obj = ((typeRowPool.Count > 0) ? typeRowPool.Pop() : new TypeRow());
		obj.name = name;
		obj.bytes = 0L;
		obj.count = 0L;
		return obj;
	}

	private ZoneRow RentZoneRow(ProfilerLabelId labelId)
	{
		ZoneRow obj = ((zoneRowPool.Count > 0) ? zoneRowPool.Pop() : new ZoneRow());
		obj.labelId = labelId;
		obj.typeName = null;
		obj.bytes = 0L;
		obj.count = 0L;
		return obj;
	}

	private void ReleaseTypeRow(TypeRow row)
	{
		for (int i = 0; i < row.zones.Count; i++)
		{
			zoneRowPool.Push(row.zones[i]);
		}
		row.zones.Clear();
		typeRowPool.Push(row);
	}

	private void Aggregate()
	{
		for (int i = 0; i < aggregated.Count; i++)
		{
			ReleaseTypeRow(aggregated[i]);
		}
		aggregated.Clear();
		typeLookup.Clear();
		rowLookup.Clear();
		zoneLookup.Clear();
		overflowCount = 0L;
		overflowBytes = 0L;
		untrackedBytes = 0L;
		bool flag = !ProfilerThreadRegistry.AllSelected;
		for (int j = 0; j < frames.Count; j++)
		{
			FrameProfiler.FrameSnapshot frameSnapshot = frames[j];
			if (!frameSnapshot.allocationTracked)
			{
				continue;
			}
			overflowCount += frameSnapshot.allocOverflowCount;
			overflowBytes += frameSnapshot.allocOverflowBytes;
			long num = 0L;
			for (int k = 0; k < frameSnapshot.topAllocTypeCount; k++)
			{
				FrameProfiler.TypeAllocStat typeAllocStat = frameSnapshot.topAllocTypes[k];
				num += typeAllocStat.bytes;
				if (!flag || ProfilerThreadRegistry.IsSelected(typeAllocStat.threadId))
				{
					if (!typeLookup.TryGetValue(typeAllocStat.type, out var value))
					{
						value = aggregated.Count;
						typeLookup.Add(typeAllocStat.type, value);
						aggregated.Add(RentTypeRow(ProfilerTypeNames.Get(typeAllocStat.type)));
					}
					TypeRow typeRow = aggregated[value];
					typeRow.bytes += typeAllocStat.bytes;
					typeRow.count += typeAllocStat.count;
					if (!zoneLookup.TryGetValue((value, typeAllocStat.labelId), out var value2))
					{
						value2 = typeRow.zones.Count;
						zoneLookup.Add((value, typeAllocStat.labelId), value2);
						typeRow.zones.Add(RentZoneRow(typeAllocStat.labelId));
					}
					ZoneRow zoneRow = typeRow.zones[value2];
					zoneRow.bytes += typeAllocStat.bytes;
					zoneRow.count += typeAllocStat.count;
				}
			}
			untrackedBytes += frameSnapshot.allocatedBytes - num - frameSnapshot.allocOverflowBytes;
		}
		untrackedBytes = Math.Max(0L, untrackedBytes);
		if (frames.Count > 1)
		{
			overflowCount /= frames.Count;
			overflowBytes /= frames.Count;
			untrackedBytes /= frames.Count;
			for (int l = 0; l < aggregated.Count; l++)
			{
				TypeRow typeRow2 = aggregated[l];
				typeRow2.bytes /= frames.Count;
				typeRow2.count /= frames.Count;
				for (int m = 0; m < typeRow2.zones.Count; m++)
				{
					typeRow2.zones[m].bytes /= frames.Count;
					typeRow2.zones[m].count /= frames.Count;
				}
			}
		}
		for (int num2 = aggregated.Count - 1; num2 >= 0; num2--)
		{
			TypeRow typeRow3 = aggregated[num2];
			if (typeRow3.bytes <= 0 && typeRow3.count <= 0)
			{
				ReleaseTypeRow(typeRow3);
				aggregated.RemoveAt(num2);
			}
			else
			{
				for (int num3 = typeRow3.zones.Count - 1; num3 >= 0; num3--)
				{
					if (typeRow3.zones[num3].bytes <= 0 && typeRow3.zones[num3].count <= 0)
					{
						zoneRowPool.Push(typeRow3.zones[num3]);
						typeRow3.zones.RemoveAt(num3);
					}
				}
				typeRow3.zones.Sort(ZoneRowComparison);
			}
		}
		aggregated.Sort(TypeRowComparison);
	}

	private void InvertAggregation()
	{
		invertedScratch.Clear();
		rowLookup.Clear();
		for (int i = 0; i < aggregated.Count; i++)
		{
			TypeRow typeRow = aggregated[i];
			for (int j = 0; j < typeRow.zones.Count; j++)
			{
				ZoneRow zoneRow = typeRow.zones[j];
				string label = FrameProfiler.GetLabel(zoneRow.labelId);
				if (!rowLookup.TryGetValue(label, out var value))
				{
					value = invertedScratch.Count;
					rowLookup.Add(label, value);
					invertedScratch.Add(RentTypeRow(label));
				}
				TypeRow typeRow2 = invertedScratch[value];
				typeRow2.bytes += zoneRow.bytes;
				typeRow2.count += zoneRow.count;
				ZoneRow zoneRow2 = RentZoneRow(zoneRow.labelId);
				zoneRow2.typeName = typeRow.name;
				zoneRow2.bytes = zoneRow.bytes;
				zoneRow2.count = zoneRow.count;
				typeRow2.zones.Add(zoneRow2);
			}
		}
		for (int k = 0; k < aggregated.Count; k++)
		{
			ReleaseTypeRow(aggregated[k]);
		}
		aggregated.Clear();
		aggregated.AddRange(invertedScratch);
		invertedScratch.Clear();
		for (int l = 0; l < aggregated.Count; l++)
		{
			aggregated[l].zones.Sort(ZoneRowComparison);
		}
		aggregated.Sort(TypeRowComparison);
	}

	private void SyncRows()
	{
		long totalBytes = TotalBytes();
		displayRows.Clear();
		for (int i = 0; i < aggregated.Count; i++)
		{
			displayRows.Add((i, -1));
			if (expandedTypes.Contains(aggregated[i].name))
			{
				for (int j = 0; j < aggregated[i].zones.Count; j++)
				{
					displayRows.Add((i, j));
				}
			}
		}
		View.Resize(displayRows.Count);
		for (int k = 0; k < displayRows.Count; k++)
		{
			Shape(displayRows[k].typeIndex, displayRows[k].zoneIndex, View.Rows[k], totalBytes);
		}
		View.Notify_RowsChanged();
	}

	private void Shape(int typeIndex, int zoneIndex, ProfilerRowData data, long totalBytes)
	{
		TypeRow typeRow = aggregated[typeIndex];
		data.Reset();
		data.TotalIsBytes = true;
		data.CallsGrouped = true;
		if (zoneIndex < 0)
		{
			data.BarFraction = Mathf.Clamp01((float)typeRow.bytes / (float)totalBytes);
			data.TotalBytes = typeRow.bytes;
			data.Calls = typeRow.count;
			data.Label = typeRow.name;
			data.HasChildren = typeRow.zones.Count > 0;
			data.Expanded = expandedTypes.Contains(typeRow.name);
			data.Dimmed = session.Search.IsActive && !session.Search.Matches(typeRow.name);
		}
		else
		{
			ZoneRow zoneRow = typeRow.zones[zoneIndex];
			data.BarFraction = Mathf.Clamp01((float)zoneRow.bytes / (float)totalBytes);
			data.TotalBytes = zoneRow.bytes;
			data.Calls = zoneRow.count;
			data.Label = zoneRow.typeName ?? FrameProfiler.GetLabel(zoneRow.labelId);
			data.Depth = 1;
			data.Dimmed = session.Search.IsActive && !ZoneMatches(zoneRow);
		}
	}

	protected override bool BuildFullSubtree(int index, List<ProfilerRowData> into)
	{
		if (index >= displayRows.Count)
		{
			return false;
		}
		(int typeIndex, int zoneIndex) tuple = displayRows[index];
		int item = tuple.typeIndex;
		int item2 = tuple.zoneIndex;
		long totalBytes = TotalBytes();
		int num = ((item2 < 0) ? (aggregated[item].zones.Count - 1) : item2);
		for (int i = item2; i <= num; i++)
		{
			ProfilerRowData profilerRowData = new ProfilerRowData();
			Shape(item, i, profilerRowData, totalBytes);
			into.Add(profilerRowData);
		}
		return true;
	}

	private long TotalBytes()
	{
		long num = 0L;
		for (int i = 0; i < aggregated.Count; i++)
		{
			num += aggregated[i].bytes;
		}
		return Math.Max(1L, num);
	}

	public override void Draw(Rect rect)
	{
		if (aggregated.Count > 0)
		{
			base.Draw(rect);
			return;
		}
		if (FrameProfiler.AllocationTracking)
		{
			ProfilerGui.Label(rect, "No tracked allocations in the selected frames.", ProfilerGui.TextFaint, TextAnchor.MiddleCenter);
			return;
		}
		string text = (FrameProfiler.AllocationTrackingSupported ? "Allocation tracking is off." : "Allocation tracking is not supported by this runtime.");
		ProfilerGui.Label(new Rect(rect.x, rect.center.y - 24f, rect.width, 20f), text, ProfilerGui.TextFaint, TextAnchor.MiddleCenter);
		if (FrameProfiler.AllocationTrackingSupported && ProfilerGui.Chip(new Rect(rect.center.x - 40f, rect.center.y + 2f, 80f, 22f), "Enable it", active: false, enabled: true, null))
		{
			EnableTrackingRequested?.Invoke();
		}
	}

	protected override void DrawFooter(Rect rect)
	{
		ProfilerGui.Fill(new Rect(rect.x, rect.y, rect.width, 1f), ProfilerGui.Divider);
		ProfilerGui.Label(new Rect(rect.x + 12f, rect.y + 1f, rect.width - 24f, rect.height - 1f), footer, ProfilerGui.TextFaint);
	}
}
