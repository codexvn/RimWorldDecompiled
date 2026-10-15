using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Verse.ProfilerUI;

public sealed class ProfilerRowListView
{
	public const float HeaderHeight = 22f;

	public const float CrumbBarHeight = 24f;

	private const float RowHeight = 20f;

	private const float IndentPerDepth = 14f;

	private const float LeftPad = 12f;

	private const float BarColumnWidth = 60f;

	private const float ScrollBarWidth = 6f;

	private const float WheelRows = 3f;

	private const float ScrollUnitsPerNotch = 3f;

	private const float DetailWidthShare = 0.4f;

	private static readonly Color RowBorder = ProfilerGui.Hex(4279835940u);

	private static readonly Color CrumbHover = ProfilerGui.Hex(352321535u);

	private readonly ProfilerSession session;

	private readonly List<ProfilerRowData> rowPool = new List<ProfilerRowData>();

	private readonly List<(string label, int rowIndex)> crumbs = new List<(string, int)>();

	private int selectedIndex = -1;

	private bool revealPending;

	private float scrollY;

	private bool draggingThumb;

	private float thumbGrabOffset;

	public readonly List<ProfilerRowData> Rows = new List<ProfilerRowData>();

	public ProfilerColumn[] Columns;

	public Func<string> LabelHeader;

	public Action LabelSort;

	public string DetailHeader;

	public Func<string> RootCrumb;

	public bool SearchMatchesTruncated;

	public Action SelectedByUser;

	public Action<int> ToggleExpansion;

	public Action<int> ActivateLeaf;

	public Action ContextMenu;

	public int SelectedIndex => selectedIndex;

	public string SearchStatus { get; private set; } = "";

	private float ColumnsWidth
	{
		get
		{
			float num = 72f;
			for (int i = 0; i < Columns.Length; i++)
			{
				num += Columns[i].width;
			}
			return num;
		}
	}

	public ProfilerRowListView(ProfilerSession session)
	{
		this.session = session;
	}

	public void RequestReveal()
	{
		revealPending = true;
	}

	public void Resize(int count)
	{
		while (Rows.Count > count)
		{
			rowPool.Add(Rows[Rows.Count - 1]);
			Rows.RemoveAt(Rows.Count - 1);
		}
		while (Rows.Count < count)
		{
			if (rowPool.Count > 0)
			{
				Rows.Add(rowPool[rowPool.Count - 1]);
				rowPool.RemoveAt(rowPool.Count - 1);
			}
			else
			{
				Rows.Add(new ProfilerRowData());
			}
		}
	}

	public void Notify_RowsChanged()
	{
		if (selectedIndex >= Rows.Count)
		{
			selectedIndex = Rows.Count - 1;
		}
		UpdateSearchStatus();
		RefreshBreadcrumbs();
	}

	public void SelectQuietly(int index)
	{
		if (selectedIndex != index)
		{
			selectedIndex = index;
			UpdateSearchStatus();
			RefreshBreadcrumbs();
		}
	}

	public void Select(int index)
	{
		if (selectedIndex != index)
		{
			SelectQuietly(index);
			revealPending = true;
			SelectedByUser?.Invoke();
		}
	}

	public void UpdateSearchStatus()
	{
		if (!session.Search.IsActive)
		{
			SearchStatus = "";
			return;
		}
		int num = 0;
		int num2 = -1;
		for (int i = 0; i < Rows.Count; i++)
		{
			if (!Rows[i].Dimmed)
			{
				num++;
				if (i == selectedIndex)
				{
					num2 = num;
				}
			}
		}
		string text = (SearchMatchesTruncated ? (num + "+") : num.ToString());
		string text2 = ((num == 1 && !SearchMatchesTruncated) ? "match" : "matches");
		SearchStatus = ((num2 > 0) ? (num2 + "/" + text + " " + text2) : (text + " " + text2));
	}

	public void SelectNextMatch(int direction)
	{
		if (!session.Search.IsActive || Rows.Count == 0)
		{
			return;
		}
		int num = selectedIndex;
		if (num < 0 && direction < 0)
		{
			num = 0;
		}
		for (int i = 1; i <= Rows.Count; i++)
		{
			int index = ((num + i * direction) % Rows.Count + Rows.Count) % Rows.Count;
			if (!Rows[index].Dimmed)
			{
				Select(index);
				break;
			}
		}
	}

	public void SelectFirstMatch()
	{
		for (int i = 0; i < Rows.Count; i++)
		{
			if (!Rows[i].Dimmed)
			{
				SelectQuietly(i);
				revealPending = true;
				break;
			}
		}
	}

	public void MoveSelection(int direction)
	{
		if (Rows.Count != 0)
		{
			Select(Mathf.Clamp(selectedIndex + direction, 0, Rows.Count - 1));
		}
	}

	public void ActivateSelected(bool expandOnly)
	{
		int num = selectedIndex;
		if (num < 0 || num >= Rows.Count)
		{
			return;
		}
		if (!Rows[num].HasChildren)
		{
			if (!expandOnly)
			{
				ActivateLeaf?.Invoke(num);
			}
			else if (num + 1 < Rows.Count)
			{
				Select(num + 1);
			}
		}
		else if (expandOnly && Rows[num].Expanded)
		{
			if (num + 1 < Rows.Count && Rows[num + 1].Depth > Rows[num].Depth)
			{
				Select(num + 1);
			}
		}
		else
		{
			ToggleExpansion?.Invoke(num);
		}
	}

	public void CollapseSelected(bool collapseExpanded = true)
	{
		int num = selectedIndex;
		if (num < 0 || num >= Rows.Count)
		{
			return;
		}
		if (collapseExpanded && Rows[num].Expanded)
		{
			ToggleExpansion?.Invoke(num);
			return;
		}
		int depth = Rows[num].Depth;
		for (int num2 = num - 1; num2 >= 0; num2--)
		{
			if (Rows[num2].Depth < depth)
			{
				Select(num2);
				return;
			}
		}
		if (num > 0)
		{
			Select(num - 1);
		}
	}

	public bool CopySelectedRow()
	{
		StringBuilder builder = new StringBuilder();
		if (!ProfilerClipboard.AppendRow(builder, Rows, selectedIndex))
		{
			return false;
		}
		ProfilerClipboard.Put(builder);
		return true;
	}

	private void RefreshBreadcrumbs()
	{
		crumbs.Clear();
		if (selectedIndex < 0 || selectedIndex >= Rows.Count)
		{
			return;
		}
		int num = int.MaxValue;
		int num2 = selectedIndex;
		while (num2 >= 0 && num > 0)
		{
			if (Rows[num2].Depth < num)
			{
				num = Rows[num2].Depth;
				crumbs.Add((Rows[num2].Label, num2));
			}
			num2--;
		}
		crumbs.Reverse();
	}

	private void LabelAndDetailLayout(float w, out float labelX, out float detailX, out float detailWidth)
	{
		labelX = ColumnsWidth + 10f;
		float num = w - labelX - 6f - 8f;
		detailWidth = ((DetailHeader != null) ? Mathf.Floor(num * 0.4f) : 0f);
		detailX = w - 6f - 8f - detailWidth;
	}

	public void DrawHeader(Rect rect)
	{
		ProfilerGui.Fill(new Rect(rect.x, rect.yMax - 1f, rect.width, 1f), ProfilerGui.Divider);
		float num = rect.x + 12f + 60f;
		for (int i = 0; i < Columns.Length; i++)
		{
			ProfilerColumn profilerColumn = Columns[i];
			Rect rect2 = new Rect(num, rect.y, profilerColumn.width, rect.height - 1f);
			num += profilerColumn.width;
			bool flag = profilerColumn.sort != null && ProfilerGui.Hovered(rect2);
			ProfilerGui.Label(rect2, profilerColumn.header(), flag ? ProfilerGui.TextDim : ProfilerGui.TextFaint, TextAnchor.MiddleRight);
			if (profilerColumn.sort != null && ProfilerGui.Pressed(rect2))
			{
				profilerColumn.sort();
			}
		}
		LabelAndDetailLayout(rect.width, out var labelX, out var detailX, out var detailWidth);
		Rect rect3 = new Rect(rect.x + labelX, rect.y, 120f, rect.height - 1f);
		bool flag2 = LabelSort != null && ProfilerGui.Hovered(rect3);
		ProfilerGui.Label(rect3, LabelHeader(), flag2 ? ProfilerGui.TextDim : ProfilerGui.TextFaint);
		if (LabelSort != null && ProfilerGui.Pressed(rect3))
		{
			LabelSort();
		}
		if (DetailHeader != null)
		{
			ProfilerGui.Label(new Rect(rect.x + detailX, rect.y, detailWidth, rect.height - 1f), DetailHeader, ProfilerGui.TextFaint);
		}
	}

	public void DrawRows(Rect rect)
	{
		float height = rect.height;
		float num = (float)Rows.Count * 20f;
		float num2 = Mathf.Max(0f, num - height);
		if (revealPending && ProfilerGui.Painting)
		{
			revealPending = false;
			if (selectedIndex >= 0)
			{
				float num3 = (float)selectedIndex * 20f;
				if (num3 < scrollY)
				{
					scrollY = num3;
				}
				else if (num3 + 20f > scrollY + height)
				{
					scrollY = num3 + 20f - height;
				}
			}
		}
		scrollY = Mathf.Clamp(scrollY, 0f, num2);
		GUI.BeginGroup(rect);
		try
		{
			Rect local = new Rect(0f, 0f, rect.width, rect.height);
			Event current = Event.current;
			Vector2 mousePosition = current.mousePosition;
			bool flag = num2 > 0f && mousePosition.x >= rect.width - 6f - 4f;
			HandleScrollBar(local, num, num2);
			if (current.type == EventType.scrollWheel && local.Contains(mousePosition))
			{
				scrollY = Mathf.Clamp(scrollY + current.delta.y / 3f * 3f * 20f, 0f, num2);
				current.Use();
			}
			else if (current.type == EventType.mouseDown && local.Contains(mousePosition) && !flag)
			{
				int num4 = Mathf.FloorToInt((mousePosition.y + scrollY) / 20f);
				if (num4 >= 0 && num4 < Rows.Count)
				{
					OnRowMouseDown(num4, mousePosition.x, current);
				}
				current.Use();
			}
			if (ProfilerGui.Painting)
			{
				int num5 = Mathf.Max(0, Mathf.FloorToInt(scrollY / 20f));
				int num6 = Mathf.Min(Rows.Count - 1, Mathf.FloorToInt((scrollY + height) / 20f));
				int num7 = ((local.Contains(mousePosition) && !flag && !draggingThumb) ? Mathf.FloorToInt((mousePosition.y + scrollY) / 20f) : (-1));
				for (int i = num5; i <= num6; i++)
				{
					DrawRow(i, new Rect(0f, (float)i * 20f - scrollY, rect.width, 20f), i == num7);
				}
				if (num2 > 0f)
				{
					Rect rect2 = ThumbRect(local, num, num2);
					ProfilerGui.Fill(rect2, (draggingThumb || rect2.Contains(mousePosition)) ? ProfilerGui.ScrollThumbHover : ProfilerGui.ScrollThumb);
				}
			}
		}
		finally
		{
			GUI.EndGroup();
		}
	}

	private Rect ThumbRect(Rect local, float contentHeight, float maxScroll)
	{
		float num = Mathf.Max(24f, local.height * local.height / contentHeight);
		float y = (local.height - num) * (scrollY / maxScroll);
		return new Rect(local.width - 6f - 2f, y, 6f, num);
	}

	private void HandleScrollBar(Rect local, float contentHeight, float maxScroll)
	{
		if (maxScroll <= 0f)
		{
			draggingThumb = false;
			return;
		}
		Event current = Event.current;
		Rect rect = ThumbRect(local, contentHeight, maxScroll);
		if (current.type == EventType.mouseDown && current.button == 0 && current.mousePosition.x >= local.width - 6f - 4f && local.Contains(current.mousePosition))
		{
			draggingThumb = true;
			thumbGrabOffset = (rect.Contains(current.mousePosition) ? (current.mousePosition.y - rect.y) : (rect.height * 0.5f));
			current.Use();
		}
		if (!draggingThumb || current.type != EventType.repaint)
		{
			return;
		}
		if (!Input.GetMouseButton(0))
		{
			draggingThumb = false;
			return;
		}
		float num = local.height - rect.height;
		if (num > 0f)
		{
			scrollY = Mathf.Clamp((current.mousePosition.y - thumbGrabOffset) / num, 0f, 1f) * maxScroll;
		}
	}

	private void OnRowMouseDown(int index, float mouseX, Event e)
	{
		if (e.button == 1)
		{
			Select(index);
			ContextMenu?.Invoke();
		}
		else
		{
			if (e.button != 0)
			{
				return;
			}
			ProfilerRowData profilerRowData = Rows[index];
			float num = ColumnsWidth + 10f + (float)profilerRowData.Depth * 14f;
			if (profilerRowData.HasChildren && mouseX >= num && mouseX < num + 14f)
			{
				ToggleExpansion?.Invoke(index);
				return;
			}
			Select(index);
			if (e.clickCount >= 2)
			{
				ActivateSelected(expandOnly: false);
			}
		}
	}

	private void DrawRow(int index, Rect rect, bool hovered)
	{
		ProfilerRowData profilerRowData = Rows[index];
		if (index == selectedIndex)
		{
			ProfilerGui.Fill(rect, ProfilerGui.RowSelected);
		}
		else if (hovered)
		{
			ProfilerGui.Fill(rect, ProfilerGui.FieldBackground);
		}
		ProfilerGui.Fill(new Rect(rect.x, rect.yMax - 1f, rect.width, 1f), RowBorder);
		float alpha = (profilerRowData.Dimmed ? 0.35f : 1f);
		float num = 12f;
		Rect rect2 = new Rect(num, rect.y + 6.5f, 56f, 7f);
		ProfilerGui.Fill(rect2, Fade(ProfilerGui.BarTrack, alpha));
		ProfilerGui.Fill(new Rect(rect2.x, rect2.y, rect2.width * profilerRowData.BarFraction, rect2.height), Fade(ProfilerGui.BarFill, alpha));
		num += 60f;
		profilerRowData.EnsureDisplayText();
		for (int i = 0; i < Columns.Length; i++)
		{
			ProfilerColumn profilerColumn = Columns[i];
			ProfilerGui.Label(new Rect(num, rect.y, profilerColumn.width, rect.height), CellText(profilerRowData, profilerColumn.kind), Fade(CellColor(profilerRowData, profilerColumn.kind), alpha), TextAnchor.MiddleRight);
			num += profilerColumn.width;
		}
		LabelAndDetailLayout(rect.width, out var labelX, out var detailX, out var detailWidth);
		num = labelX + (float)profilerRowData.Depth * 14f;
		Rect cell = new Rect(num, rect.y, 14f, rect.height);
		if (profilerRowData.HasChildren)
		{
			ProfilerGui.Arrow(cell, profilerRowData.Expanded, Fade(ProfilerGui.TextDim, alpha));
		}
		num += 16f;
		float num2 = ((DetailHeader != null) ? (detailX - 8f) : (rect.width - 6f - 8f)) - num;
		if (num2 > 20f)
		{
			ProfilerGui.Label(new Rect(num, rect.y, num2, rect.height), profilerRowData.FittedLabel(num2), Fade(ProfilerGui.TextMain, alpha));
		}
		if (DetailHeader != null && !string.IsNullOrEmpty(profilerRowData.Detail))
		{
			ProfilerGui.Label(new Rect(detailX, rect.y, detailWidth, rect.height), profilerRowData.Detail, Fade(ProfilerGui.TextFaint, alpha));
		}
	}

	private static string CellText(ProfilerRowData row, ProfilerColumnKind kind)
	{
		return kind switch
		{
			ProfilerColumnKind.Percent => row.PercentText, 
			ProfilerColumnKind.Total => row.TotalText, 
			ProfilerColumnKind.Self => row.SelfText, 
			ProfilerColumnKind.Delta => row.DeltaText, 
			ProfilerColumnKind.Calls => row.CallsText, 
			_ => row.AllocText, 
		};
	}

	private static Color CellColor(ProfilerRowData row, ProfilerColumnKind kind)
	{
		switch (kind)
		{
		case ProfilerColumnKind.Total:
			return ProfilerGui.TextMain;
		case ProfilerColumnKind.Delta:
			if (row.DeltaSeverity <= 0)
			{
				if (row.DeltaSeverity >= 0)
				{
					return ProfilerGui.TextFaint;
				}
				return ProfilerGui.Improvement;
			}
			return ProfilerGui.Regression;
		default:
			return ProfilerGui.TextDim;
		}
	}

	private static Color Fade(Color color, float alpha)
	{
		color.a *= alpha;
		return color;
	}

	public void DrawBreadcrumbs(Rect rect)
	{
		ProfilerGui.Fill(new Rect(rect.x, rect.y, rect.width, 1f), ProfilerGui.Divider);
		float x = rect.x + 12f;
		float y = rect.y + 3f;
		float num = rect.height - 5f;
		Crumb(ref x, y, num, RootCrumb(), (crumbs.Count > 0) ? crumbs[0].rowIndex : (-1), crumbs.Count == 0);
		int num2 = 0;
		float num3 = rect.width - 12f - (x - rect.x);
		float num4 = 0f;
		for (int num5 = crumbs.Count - 1; num5 >= 0; num5--)
		{
			num4 += Mathf.Min(180f, ProfilerGui.Width(crumbs[num5].label)) + 24f;
			if (num4 > num3 && num5 < crumbs.Count - 1)
			{
				num2 = num5 + 1;
				break;
			}
		}
		if (num2 > 0)
		{
			x = ProfilerGui.Run(x, y, num, " ›  …", ProfilerGui.TextFaint);
		}
		for (int i = num2; i < crumbs.Count; i++)
		{
			x = ProfilerGui.Run(x, y, num, " › ", ProfilerGui.TextFaint);
			Crumb(ref x, y, num, crumbs[i].label, crumbs[i].rowIndex, i == crumbs.Count - 1);
		}
	}

	private void Crumb(ref float x, float y, float h, string label, int rowIndex, bool last)
	{
		string text = ProfilerGui.Ellipsize(label, 180f);
		float num = ProfilerGui.Width(text) + 10f;
		Rect rect = new Rect(x, y, num, h);
		if (rowIndex >= 0 && ProfilerGui.Hovered(rect))
		{
			ProfilerGui.Fill(rect, CrumbHover);
		}
		ProfilerGui.Label(rect, text, last ? ProfilerGui.TextMain : ProfilerGui.TextDim, TextAnchor.MiddleCenter);
		if (rowIndex >= 0 && rowIndex < Rows.Count && ProfilerGui.Pressed(rect))
		{
			Select(rowIndex);
		}
		x += num;
	}
}
