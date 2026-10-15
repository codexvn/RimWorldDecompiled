using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityStubs.Gui;
using Waylith.Profiling;

namespace Verse.ProfilerUI;

public sealed class ProfilerCallTreeTable : ProfilerRowSection
{
	private struct Bar
	{
		public float x0;

		public float x1;

		public float y0;

		public float y1;

		public double u0;

		public double u1;

		public int rowIndex;
	}

	private const int AutoExpandMatchLimit = 24;

	private const int AutoExpandMinQueryLength = 2;

	private static readonly Comparison<(float totalMicroseconds, int pathId)> MatchComparison = ((float totalMicroseconds, int pathId) lhs, (float totalMicroseconds, int pathId) rhs) => rhs.totalMicroseconds.CompareTo(lhs.totalMicroseconds);

	private readonly List<FrameProfiler.FrameSnapshot> frames = new List<FrameProfiler.FrameSnapshot>();

	private readonly List<(float totalMicroseconds, int pathId)> searchMatchScratch = new List<(float, int)>();

	private readonly List<ProfilerLabelId> searchPathScratch = new List<ProfilerLabelId>();

	private readonly HashSet<ulong> searchPathKeyScratch = new HashSet<ulong>();

	private readonly List<ProfilerTreeModel.Row> fullRowScratch = new List<ProfilerTreeModel.Row>();

	private ProfilerRefreshGate refreshGate;

	private bool selectFirstMatchOnRebuild;

	private ulong pendingSelectKey;

	private ProfilerLabelId? previousSelectionLabelId;

	private ulong previousSelectionPathKey;

	private double frameTotalMicroseconds = 1.0;

	public Action RowSelectedByUser;

	private const float FlameLaneHeight = 19f;

	private const float FlameLaneGap = 1f;

	private const float MinBarWidth = 0.75f;

	private const float MinLabelWidth = 40f;

	private const int MaxLabels = 240;

	private const double MinViewSpan = 1E-07;

	private static readonly Color DimColor = ProfilerGui.Hex(2566914048u);

	private static readonly Color FlameHoverStroke = ProfilerGui.Hex(2868903935u);

	private readonly ProfilerPanZoomView flameView = new ProfilerPanZoomView(1E-07);

	private readonly List<Bar> bars = new List<Bar>();

	private double[] laneCursor = new double[64];

	private bool revealInFlamegraph;

	private bool selectingFromFlamegraph;

	private int flameHoverRow = -1;

	private string flameTooltip;

	private ProfilerTreeModel Tree => session.Tree;

	private List<ProfilerRowData> rows => View.Rows;

	private int selectedIndex => View.SelectedIndex;

	protected override string CopyHeader => "%\ttotal\tself\tcalls\talloc\tscope";

	private ProfilerTreeModel.Row SelectedRow
	{
		get
		{
			if (selectedIndex < 0 || selectedIndex >= Tree.DisplayRows.Count)
			{
				return null;
			}
			return Tree.DisplayRows[selectedIndex];
		}
	}

	public bool FlamegraphMode { get; private set; }

	public ProfilerCallTreeTable(ProfilerSession session)
		: base(session)
	{
		View.Columns = new ProfilerColumn[6]
		{
			new ProfilerColumn(ProfilerColumnKind.Percent, 44f, () => "%"),
			new ProfilerColumn(ProfilerColumnKind.Total, 90f, () => HeaderText("total", ProfilerTreeModel.SortColumn.Total), delegate
			{
				SetSortColumn(ProfilerTreeModel.SortColumn.Total);
			}),
			new ProfilerColumn(ProfilerColumnKind.Self, 90f, () => HeaderText("self", ProfilerTreeModel.SortColumn.Self), delegate
			{
				SetSortColumn(ProfilerTreeModel.SortColumn.Self);
			}),
			new ProfilerColumn(ProfilerColumnKind.Delta, 64f, () => "Δ median"),
			new ProfilerColumn(ProfilerColumnKind.Calls, 52f, () => HeaderText("calls", ProfilerTreeModel.SortColumn.Calls), delegate
			{
				SetSortColumn(ProfilerTreeModel.SortColumn.Calls);
			}),
			new ProfilerColumn(ProfilerColumnKind.Alloc, 66f, () => HeaderText("alloc", ProfilerTreeModel.SortColumn.Alloc), delegate
			{
				SetSortColumn(ProfilerTreeModel.SortColumn.Alloc);
			})
		};
		View.LabelHeader = () => HeaderText("scope", ProfilerTreeModel.SortColumn.Label);
		View.LabelSort = delegate
		{
			SetSortColumn(ProfilerTreeModel.SortColumn.Label);
		};
		View.RootCrumb = RootCrumbLabel;
		View.SelectedByUser = OnSelectedByUser;
	}

	public override void Invalidate()
	{
		refreshGate.Invalidate();
	}

	public void ResetSelection()
	{
		View.SelectQuietly(-1);
	}

	private void OnSelectedByUser()
	{
		revealInFlamegraph = !selectingFromFlamegraph;
		RowSelectedByUser?.Invoke();
	}

	public override void Refresh()
	{
		bool num = refreshGate.ShouldRefresh(session);
		bool flag = Tree.EmittedRequest != Tree.SubmittedRequest;
		if (!num && !flag)
		{
			return;
		}
		ProfilerTreeModel.Row selectedRow = SelectedRow;
		previousSelectionLabelId = selectedRow?.labelId;
		previousSelectionPathKey = selectedRow?.pathKey ?? 0;
		if (FlamegraphMode)
		{
			Tree.BuildAllLevelRows();
			frameTotalMicroseconds = 0.0;
			List<ProfilerTreeModel.Row> displayRows = Tree.DisplayRows;
			for (int i = 0; i < displayRows.Count; i++)
			{
				if (displayRows[i].depth == 0)
				{
					frameTotalMicroseconds += displayRows[i].totalMicroseconds;
				}
			}
		}
		else
		{
			session.CollectFrames(session.Span, frames);
			Tree.BuildTreeRows();
			float num2 = 0f;
			for (int j = 0; j < frames.Count; j++)
			{
				num2 += frames[j].totalMs;
			}
			frameTotalMicroseconds = num2 / (float)Mathf.Max(1, frames.Count) * 1000f;
		}
		if (frameTotalMicroseconds <= 0.0)
		{
			frameTotalMicroseconds = 1.0;
		}
		List<ProfilerTreeModel.Row> displayRows2 = Tree.DisplayRows;
		View.Resize(displayRows2.Count);
		for (int k = 0; k < displayRows2.Count; k++)
		{
			Shape(displayRows2[k], rows[k], frameTotalMicroseconds);
		}
		View.SelectQuietly(ResolveSelectionAfterRebuild());
		View.Notify_RowsChanged();
	}

	private void Shape(ProfilerTreeModel.Row row, ProfilerRowData data, double frameTotalMicroseconds)
	{
		float num = float.NaN;
		int deltaSeverity = 0;
		if (row.HasBaseline)
		{
			num = (row.totalMicroseconds - row.baselineTotalMicroseconds) / 1000f;
			deltaSeverity = ((num >= 0.05f) ? 1 : ((num <= -0.05f) ? (-1) : 0));
		}
		data.Reset();
		data.BarFraction = Mathf.Clamp01((float)((double)row.totalMicroseconds / frameTotalMicroseconds));
		data.TotalMicroseconds = row.totalMicroseconds;
		data.HasSelf = true;
		data.SelfMicroseconds = row.selfMicroseconds;
		data.HasDelta = !FlamegraphMode;
		data.DeltaMs = num;
		data.DeltaSeverity = deltaSeverity;
		data.Calls = row.calls;
		data.AllocBytes = row.allocBytes;
		data.Swatch = ProfilerPalette.ColorFor(row.labelId);
		data.Label = ProfilerTreeModel.LabelForRow(row);
		data.Depth = row.depth;
		data.HasChildren = row.labelId.IsValid && row.hasChildren;
		data.Expanded = row.expanded;
		data.Dimmed = session.Search.IsActive && !MatchesSearch(row);
	}

	private bool MatchesSearch(ProfilerTreeModel.Row row)
	{
		if (!row.labelId.IsValid)
		{
			return session.Search.Matches("Unprofiled");
		}
		return session.Search.Matches(row.labelId);
	}

	private int ResolveSelectionAfterRebuild()
	{
		List<ProfilerTreeModel.Row> displayRows = Tree.DisplayRows;
		if (pendingSelectKey != 0L)
		{
			for (int i = 0; i < displayRows.Count; i++)
			{
				if (displayRows[i].pathKey == pendingSelectKey)
				{
					pendingSelectKey = 0uL;
					View.RequestReveal();
					revealInFlamegraph = true;
					return i;
				}
			}
			if (Tree.EmittedRequest == Tree.SubmittedRequest)
			{
				pendingSelectKey = 0uL;
			}
		}
		if (selectFirstMatchOnRebuild)
		{
			selectFirstMatchOnRebuild = false;
			if (session.Search.IsActive)
			{
				for (int j = 0; j < displayRows.Count; j++)
				{
					if (MatchesSearch(displayRows[j]))
					{
						View.RequestReveal();
						revealInFlamegraph = true;
						return j;
					}
				}
			}
		}
		ProfilerLabelId? profilerLabelId = previousSelectionLabelId;
		if (profilerLabelId.HasValue)
		{
			ProfilerLabelId valueOrDefault = profilerLabelId.GetValueOrDefault();
			for (int k = 0; k < displayRows.Count; k++)
			{
				if (displayRows[k].labelId == valueOrDefault && displayRows[k].pathKey == previousSelectionPathKey)
				{
					return k;
				}
			}
		}
		return Mathf.Min(selectedIndex, displayRows.Count - 1);
	}

	public bool TryGetSelectedZones(List<int> pathIds, out int ordinal)
	{
		pathIds.Clear();
		ordinal = -1;
		ProfilerTreeModel.Row selectedRow = SelectedRow;
		if (!session.HasData || selectedRow == null || !selectedRow.labelId.IsValid || selectedRow.pathIds.Count == 0)
		{
			return false;
		}
		pathIds.AddRange(selectedRow.pathIds);
		ordinal = session.Ordinal;
		return true;
	}

	public void ExpandTo(List<ProfilerLabelId> pathLabelIds)
	{
		pendingSelectKey = Tree.ExpandTo(pathLabelIds);
		Invalidate();
	}

	public override void Notify_SearchChanged()
	{
		View.SearchMatchesTruncated = false;
		ProfilerSearchModel search = session.Search;
		if (!Tree.Inverted && search.Query.Length >= 2 && session.HasData)
		{
			ExpandToMatches(search);
		}
		selectFirstMatchOnRebuild = true;
		Invalidate();
	}

	private void ExpandToMatches(ProfilerSearchModel search)
	{
		Tree.CollectMatchingPaths(search, searchMatchScratch);
		searchMatchScratch.Sort(MatchComparison);
		searchPathKeyScratch.Clear();
		for (int i = 0; i < searchMatchScratch.Count; i++)
		{
			Tree.AppendLabelChain(searchMatchScratch[i].pathId, searchPathScratch);
			ulong num = 14695981039346656037uL;
			for (int j = 0; j < searchPathScratch.Count; j++)
			{
				num = ProfilerTreeModel.CombineKey(num, searchPathScratch[j]);
			}
			if (searchPathKeyScratch.Add(num))
			{
				if (searchPathKeyScratch.Count > 24)
				{
					View.SearchMatchesTruncated = true;
					break;
				}
				Tree.ExpandTo(searchPathScratch);
			}
		}
	}

	public override void CollapseSelected()
	{
		View.CollapseSelected(!FlamegraphMode);
	}

	public override void ExpandAll()
	{
		Tree.ExpandAll();
		Invalidate();
	}

	public override void CollapseAll()
	{
		Tree.CollapseAll();
		Invalidate();
	}

	protected override void ToggleExpansionAt(int index)
	{
		if (index < 0 || index >= Tree.DisplayRows.Count)
		{
			return;
		}
		View.Select(index);
		if (!FlamegraphMode)
		{
			ProfilerTreeModel.Row row = Tree.DisplayRows[index];
			if (row.labelId.IsValid && row.hasChildren)
			{
				Tree.SetExpanded(row.pathKey, !row.expanded);
				pendingSelectKey = row.pathKey;
				Invalidate();
			}
		}
	}

	private void SetSortColumn(ProfilerTreeModel.SortColumn column)
	{
		if (Tree.ActiveSortColumn == column)
		{
			Tree.SortAscending = !Tree.SortAscending;
		}
		else
		{
			Tree.ActiveSortColumn = column;
			Tree.SortAscending = column == ProfilerTreeModel.SortColumn.Label;
		}
		session.Invalidate();
	}

	private string HeaderText(string name, ProfilerTreeModel.SortColumn column)
	{
		if (Tree.ActiveSortColumn != column)
		{
			return name;
		}
		return name + (Tree.SortAscending ? " ↑" : " ↓");
	}

	protected override void AppendCopyContext(StringBuilder builder)
	{
		if (FlamegraphMode)
		{
			builder.AppendLine(Tree.Inverted ? "inverted flamegraph" : "flamegraph");
		}
		else if (Tree.Inverted)
		{
			builder.AppendLine("inverted call stack");
		}
	}

	protected override bool BuildFullSubtree(int index, List<ProfilerRowData> into)
	{
		if (FlamegraphMode || index >= Tree.DisplayRows.Count)
		{
			return false;
		}
		ulong pathKey = Tree.DisplayRows[index].pathKey;
		Tree.BuildFullTreeRows(fullRowScratch);
		int i;
		for (i = 0; i < fullRowScratch.Count && fullRowScratch[i].pathKey != pathKey; i++)
		{
		}
		for (int j = i; j < fullRowScratch.Count && (j == i || fullRowScratch[j].depth > fullRowScratch[i].depth); j++)
		{
			ProfilerRowData profilerRowData = new ProfilerRowData();
			Shape(fullRowScratch[j], profilerRowData, frameTotalMicroseconds);
			into.Add(profilerRowData);
		}
		Tree.ReleaseRows(fullRowScratch);
		return into.Count > 0;
	}

	public static string RootCrumbLabel()
	{
		IReadOnlyList<ProfilerThreadRegistry.Entry> entries = ProfilerThreadRegistry.Entries;
		ProfilerThreadRegistry.GetSelectionSummary(out var selected, out var singleIndex);
		switch (selected)
		{
		case 0:
			if (entries.Count != 0)
			{
				return "No threads";
			}
			return "Frame";
		case 1:
			if (!entries[singleIndex].isMain)
			{
				return FrameProfiler.GetLabel(entries[singleIndex].nameLabelId);
			}
			return "Frame";
		default:
			if (selected != entries.Count)
			{
				return selected + " threads";
			}
			return "All threads";
		}
	}

	public override void Draw(Rect rect)
	{
		if (!FlamegraphMode)
		{
			base.Draw(rect);
			return;
		}
		GUI.BeginGroup(rect);
		try
		{
			DrawFlamegraph(new Rect(0f, 0f, rect.width, rect.height - 24f));
			View.DrawBreadcrumbs(new Rect(0f, rect.height - 24f, rect.width, 24f));
		}
		finally
		{
			GUI.EndGroup();
		}
	}

	public void SetFlamegraphMode(bool value)
	{
		if (FlamegraphMode != value)
		{
			FlamegraphMode = value;
			flameView.CancelAnimation();
			flameView.UserView = false;
			ResetSelection();
			Invalidate();
		}
	}

	private void DrawFlamegraph(Rect rect)
	{
		ProfilerGui.Fill(rect, ProfilerGui.SunkenBackground);
		GUI.BeginGroup(rect);
		try
		{
			HandleFlamegraphInput(rect.width, rect.height);
			if (ProfilerGui.Painting)
			{
				PaintFlamegraph(rect.width, rect.height);
			}
		}
		finally
		{
			GUI.EndGroup();
		}
	}

	private void HandleFlamegraphInput(float w, float h)
	{
		Event current = Event.current;
		Vector2 mousePosition = current.mousePosition;
		bool flag = mousePosition.x >= 0f && mousePosition.y >= 0f && mousePosition.x < w && mousePosition.y < h;
		if (current.type == EventType.mouseDown && flag)
		{
			if (current.button == 0)
			{
				Bar hit;
				bool flag2 = TryHitBar(mousePosition, out hit);
				if (current.clickCount >= 2)
				{
					if (flag2)
					{
						flameView.AnimateTo(hit.u0, hit.u1 - hit.u0);
					}
					else
					{
						flameView.TopLane = 0f;
						flameView.AnimateTo(0.0, flameView.Extent, releaseUserViewOnFinish: true);
					}
				}
				else if (flag2)
				{
					selectingFromFlamegraph = true;
					View.Select(hit.rowIndex);
					selectingFromFlamegraph = false;
				}
			}
			else
			{
				flameView.BeginPan(mousePosition);
			}
			current.Use();
		}
		else if (current.type == EventType.scrollWheel && flag)
		{
			flameView.HandleWheel(current.delta, mousePosition.x, 0f, w);
			current.Use();
		}
		else if (current.type == EventType.repaint && flameView.Panning)
		{
			if (!Input.GetMouseButton(1) && !Input.GetMouseButton(2))
			{
				flameView.EndPan();
			}
			else
			{
				flameView.TryPan(mousePosition, w, 19f);
			}
		}
	}

	private void PaintFlamegraph(float w, float h)
	{
		bars.Clear();
		if (rows.Count == 0 || w < 16f || h < 16f)
		{
			flameView.CancelAnimation();
			return;
		}
		float num = 0f;
		int num2 = 0;
		for (int i = 0; i < rows.Count; i++)
		{
			if (rows[i].Depth == 0)
			{
				num += rows[i].BarFraction;
			}
			num2 = Mathf.Max(num2, rows[i].Depth + 1);
		}
		flameView.Extent = ((num > 0f) ? ((double)num) : 1.0);
		flameView.Advance();
		if (!flameView.UserView)
		{
			flameView.TopLane = 0f;
			flameView.Start = 0.0;
			flameView.Span = flameView.Extent;
		}
		flameView.Clamp(Mathf.Max(1f, h / 19f), num2);
		if (laneCursor.Length <= num2)
		{
			laneCursor = new double[Mathf.NextPowerOfTwo(num2 + 1)];
		}
		laneCursor[0] = 0.0;
		IGuiBackend current = GuiBackend.Current;
		double num3 = flameView.Start + flameView.Span;
		int num4 = 0;
		int num5 = -1;
		for (int j = 0; j < rows.Count; j++)
		{
			ProfilerRowData profilerRowData = rows[j];
			int depth = profilerRowData.Depth;
			double num6 = laneCursor[depth];
			double num7 = num6 + (double)profilerRowData.BarFraction;
			laneCursor[depth] = num7;
			laneCursor[depth + 1] = num6;
			if (num7 <= flameView.Start || num6 >= num3)
			{
				j = SkipSubtree(j, depth);
				continue;
			}
			float num8 = flameView.ToX(num6, 0f, w);
			float num9 = flameView.ToX(num7, 0f, w);
			float num10 = ((float)depth - flameView.TopLane) * 19f;
			float num11 = num10 + 19f - 1f;
			if (num9 - num8 < 0.75f || num10 >= h)
			{
				j = SkipSubtree(j, depth);
			}
			else
			{
				if (num11 <= 0f)
				{
					continue;
				}
				Rect rect = Rect.MinMaxRect(Mathf.Max(0f, num8 + 0.5f), Mathf.Max(0f, num10), Mathf.Min(w, num9 - 0.5f), Mathf.Min(h, num11));
				if (rect.width > 0f && rect.height > 0f)
				{
					current.FillRect(rect, profilerRowData.Swatch);
					if (profilerRowData.Dimmed)
					{
						current.FillRect(rect, DimColor);
					}
				}
				if (j == selectedIndex)
				{
					num5 = bars.Count;
				}
				bars.Add(new Bar
				{
					x0 = num8,
					x1 = num9,
					y0 = num10,
					y1 = num11,
					u0 = num6,
					u1 = num7,
					rowIndex = j
				});
				float num12 = Mathf.Max(0f, num8);
				float num13 = Mathf.Min(w, num9);
				if (num13 - num12 >= 40f && num4 < 240)
				{
					num4++;
					ProfilerGui.Label(new Rect(num12 + 3f, num10, num13 - num12 - 6f, 18f), profilerRowData.Label, ProfilerPalette.ZoneLabelColor);
				}
			}
		}
		if (num5 >= 0)
		{
			ProfilerGui.Outline(BarRect(bars[num5], w), Color.white);
		}
		if (revealInFlamegraph)
		{
			revealInFlamegraph = false;
			RevealSelectedBar(num5 >= 0, h);
		}
		PaintFlamegraphHover(w, h);
	}

	private static Rect BarRect(in Bar bar, float w)
	{
		return Rect.MinMaxRect(Mathf.Max(0f, bar.x0), bar.y0, Mathf.Min(w, bar.x1), bar.y1);
	}

	private void PaintFlamegraphHover(float w, float h)
	{
		Vector2 mousePosition = Event.current.mousePosition;
		if (!(mousePosition.x >= 0f) || !(mousePosition.y >= 0f) || !(mousePosition.x < w) || !(mousePosition.y < h) || flameView.Panning || !TryHitBar(mousePosition, out var bar))
		{
			flameHoverRow = -1;
			return;
		}
		if (bar.rowIndex != flameHoverRow || flameTooltip == null)
		{
			flameHoverRow = bar.rowIndex;
			flameTooltip = FlamegraphTooltip(rows[bar.rowIndex]);
		}
		ProfilerGui.Outline(BarRect(in bar, w), FlameHoverStroke);
		ProfilerGui.SetTooltip(flameTooltip);
	}

	private static string FlamegraphTooltip(ProfilerRowData data)
	{
		StringBuilder scratch = ProfilerGui.Scratch;
		scratch.Clear();
		scratch.Append(data.Label);
		scratch.Append("\ntotal ");
		ProfilerTreeModel.AppendMs(scratch, (float)(data.TotalMicroseconds / 1000.0), "0.000");
		if (data.HasSelf)
		{
			scratch.Append(" / self ");
			ProfilerTreeModel.AppendMs(scratch, (float)(data.SelfMicroseconds / 1000.0), "0.000");
		}
		scratch.Append(" / ").Append((data.BarFraction * 100f).ToString("0.0")).Append('%');
		if (data.Calls > 0)
		{
			scratch.Append("\ncalls ").Append(data.Calls);
		}
		if (data.AllocBytes > 0)
		{
			scratch.Append((data.Calls > 0) ? " / alloc " : "\nalloc ");
			ProfilerTreeModel.AppendAllocBytes(scratch, data.AllocBytes);
		}
		return scratch.ToString();
	}

	private void RevealSelectedBar(bool alreadyVisible, float h)
	{
		if (alreadyVisible || selectedIndex < 0 || selectedIndex >= rows.Count)
		{
			return;
		}
		laneCursor[0] = 0.0;
		double num = 0.0;
		double num2 = 0.0;
		for (int i = 0; i <= selectedIndex; i++)
		{
			ProfilerRowData profilerRowData = rows[i];
			num = laneCursor[profilerRowData.Depth];
			num2 = num + (double)profilerRowData.BarFraction;
			laneCursor[profilerRowData.Depth] = num2;
			laneCursor[profilerRowData.Depth + 1] = num;
		}
		if (!(num2 <= num))
		{
			int depth = rows[selectedIndex].Depth;
			float num3 = h / 19f;
			if ((float)depth < flameView.TopLane || (float)(depth + 1) > flameView.TopLane + num3)
			{
				flameView.TopLane = Mathf.Max(0f, (float)depth - num3 * 0.5f);
			}
			double num4 = num2 - num;
			flameView.AnimateTo(num - num4, num4 * 3.0);
		}
	}

	private int SkipSubtree(int index, int depth)
	{
		while (index + 1 < rows.Count && rows[index + 1].Depth > depth)
		{
			index++;
		}
		return index;
	}

	private bool TryHitBar(Vector2 position, out Bar hit)
	{
		for (int i = 0; i < bars.Count; i++)
		{
			Bar bar = bars[i];
			if (position.x >= bar.x0 && position.x < bar.x1 && position.y >= bar.y0 && position.y < bar.y1)
			{
				hit = bar;
				return true;
			}
		}
		hit = default(Bar);
		return false;
	}
}
