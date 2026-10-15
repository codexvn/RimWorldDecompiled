using System.Collections.Generic;
using System.Text;
using RimWorld;
using UnityEngine;
using Verse.Sound;
using Waylith.Profiling;

namespace Verse.ProfilerUI;

public sealed class ProfilerThreadFilter
{
	private sealed class Item
	{
		public int threadId;

		public string label;

		public bool isMain;

		public Color color;

		public float value;

		public float fraction;

		public string busyText = "";

		public string utilText = "";
	}

	private const float RowHeight = 24f;

	private const float PopupWidth = 360f;

	private const float HeaderHeight = 26f;

	private static readonly Color DotOff = ProfilerGui.Hex(4281942093u);

	private static readonly Color BarOff = ProfilerGui.Hex(4281547333u);

	private static readonly Color BarOn = ProfilerGui.Hex(4284185510u);

	private static readonly Color BarHot = ProfilerGui.Hex(4291324496u);

	private static readonly Color MultiDot = ProfilerGui.Hex(4285303992u);

	private readonly ProfilerSession session;

	private readonly List<Item> items = new List<Item>();

	private readonly List<FrameProfiler.FrameSnapshot> frameScratch = new List<FrameProfiler.FrameSnapshot>();

	private readonly Dictionary<int, float> valueByThreadId = new Dictionary<int, float>();

	private ProfilerRefreshGate refreshGate;

	private int lastRegistryVersion = -1;

	private int lastSelectionVersion = -1;

	private bool open;

	private bool statsCoverItems;

	private Rect buttonRect;

	private Rect popupRect;

	private string buttonLabel = "Main";

	private string buttonBusyText = "";

	private Color buttonDot = MultiDot;

	private const float StatsInterval = 0.25f;

	private float lastStatsTime;

	private bool showBytes;

	public bool ShowBytes
	{
		get
		{
			return showBytes;
		}
		set
		{
			if (showBytes != value)
			{
				showBytes = value;
				refreshGate.Invalidate();
			}
		}
	}

	public ProfilerThreadFilter(ProfilerSession session)
	{
		this.session = session;
	}

	public void Refresh()
	{
		if (lastRegistryVersion != ProfilerThreadRegistry.Version)
		{
			lastRegistryVersion = ProfilerThreadRegistry.Version;
			items.Clear();
			IReadOnlyList<ProfilerThreadRegistry.Entry> entries = ProfilerThreadRegistry.Entries;
			for (int i = 0; i < entries.Count; i++)
			{
				items.Add(new Item
				{
					threadId = entries[i].threadId,
					label = FrameProfiler.GetLabel(entries[i].nameLabelId),
					isMain = entries[i].isMain,
					color = Color.Lerp(ProfilerPalette.ColorFor(entries[i].nameLabelId), Color.white, 0.15f)
				});
			}
			statsCoverItems = false;
		}
		bool flag = lastSelectionVersion != ProfilerThreadRegistry.SelectionVersion;
		lastSelectionVersion = ProfilerThreadRegistry.SelectionVersion;
		if (items.Count <= 1)
		{
			return;
		}
		bool num = (open && !statsCoverItems) || flag;
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		if (num || (!(realtimeSinceStartup - lastStatsTime < 0.25f) && refreshGate.ShouldRefresh(session)))
		{
			lastStatsTime = realtimeSinceStartup;
			UpdateStats();
			if (open && items.Count >= 3)
			{
				items.Sort(CompareItems);
			}
			UpdateSummary();
		}
	}

	private static int CompareItems(Item lhs, Item rhs)
	{
		if (lhs.isMain != rhs.isMain)
		{
			if (!lhs.isMain)
			{
				return 1;
			}
			return -1;
		}
		int num = rhs.value.CompareTo(lhs.value);
		if (num == 0)
		{
			return string.CompareOrdinal(lhs.label, rhs.label);
		}
		return num;
	}

	private void UpdateStats()
	{
		session.CollectFrames(session.Span, frameScratch);
		if (frameScratch.Count == 0)
		{
			return;
		}
		valueByThreadId.Clear();
		float num = 0f;
		for (int i = 0; i < frameScratch.Count; i++)
		{
			FrameProfiler.FrameSnapshot frameSnapshot = frameScratch[i];
			if (!showBytes)
			{
				num += frameSnapshot.totalMs;
			}
			for (int j = 0; j < frameSnapshot.sectionCount; j++)
			{
				float num2 = (showBytes ? ((float)frameSnapshot.pathEntries[frameSnapshot.sections[j].firstPathEntry].allocBytes) : ((j == 0) ? frameSnapshot.totalMs : ((float)frameSnapshot.sections[j].busyNanoseconds / 1000000f)));
				if (showBytes)
				{
					num += num2;
				}
				valueByThreadId.TryGetValue(frameSnapshot.sections[j].threadId, out var value);
				valueByThreadId[frameSnapshot.sections[j].threadId] = value + num2;
			}
		}
		int count = frameScratch.Count;
		num /= (float)count;
		statsCoverItems = open;
		float num3 = 0f;
		StringBuilder scratch = ProfilerGui.Scratch;
		for (int k = 0; k < items.Count; k++)
		{
			Item item = items[k];
			valueByThreadId.TryGetValue(item.threadId, out var value2);
			value2 /= (float)count;
			if (ProfilerThreadRegistry.IsSelected(item.threadId))
			{
				num3 += value2;
			}
			if (open)
			{
				item.value = value2;
				item.fraction = ((num > 0f) ? Mathf.Clamp01(value2 / num) : 0f);
				scratch.Clear();
				AppendValue(scratch, value2);
				item.busyText = scratch.ToString();
				item.utilText = Mathf.RoundToInt(item.fraction * 100f) + (showBytes ? "% of alloc" : "% busy");
			}
		}
		scratch.Clear();
		AppendValue(scratch, num3);
		buttonBusyText = scratch.ToString();
	}

	private void AppendValue(StringBuilder builder, float value)
	{
		if (showBytes)
		{
			ProfilerTreeModel.AppendAllocBytes(builder, (long)value);
		}
		else
		{
			ProfilerTreeModel.AppendMs(builder, value);
		}
	}

	private void UpdateSummary()
	{
		IReadOnlyList<ProfilerThreadRegistry.Entry> entries = ProfilerThreadRegistry.Entries;
		ProfilerThreadRegistry.GetSelectionSummary(out var selected, out var singleIndex);
		buttonDot = MultiDot;
		if (entries.Count > 1 && selected == entries.Count)
		{
			buttonLabel = "All threads";
		}
		else if (selected == 1 && singleIndex >= 0)
		{
			buttonLabel = (entries[singleIndex].isMain ? "Main" : FrameProfiler.GetLabel(entries[singleIndex].nameLabelId));
			buttonDot = Color.Lerp(ProfilerPalette.ColorFor(entries[singleIndex].nameLabelId), Color.white, 0.15f);
		}
		else
		{
			buttonLabel = ((selected == 0) ? "No threads" : (selected + " threads"));
		}
	}

	public void HandleInput()
	{
		if (!open)
		{
			return;
		}
		ProfilerGui.AddOverlay(popupRect);
		Event current = Event.current;
		if (current.type != 0 && current.type != EventType.scrollWheel)
		{
			return;
		}
		if (!popupRect.Contains(current.mousePosition))
		{
			if (current.type == EventType.mouseDown && !buttonRect.Contains(current.mousePosition))
			{
				open = false;
			}
			return;
		}
		if (current.type == EventType.mouseDown && current.button == 0)
		{
			float num = current.mousePosition.y - popupRect.y;
			if (num < 26f)
			{
				ToggleSelectAll();
			}
			else
			{
				int num2 = Mathf.FloorToInt((num - 26f) / 24f);
				if (num2 >= 0 && num2 < items.Count)
				{
					int threadId = items[num2].threadId;
					bool flag = !ProfilerThreadRegistry.IsSelected(threadId);
					ProfilerThreadRegistry.SetSelected(threadId, flag);
					(flag ? SoundDefOf.Checkbox_TurnedOn : SoundDefOf.Checkbox_TurnedOff).PlayOneShotOnCamera();
					session.Invalidate();
				}
			}
		}
		current.Use();
	}

	private void ToggleSelectAll()
	{
		IReadOnlyList<ProfilerThreadRegistry.Entry> entries = ProfilerThreadRegistry.Entries;
		bool allSelected = ProfilerThreadRegistry.AllSelected;
		(allSelected ? SoundDefOf.Checkbox_TurnedOff : SoundDefOf.Checkbox_TurnedOn).PlayOneShotOnCamera();
		for (int i = 0; i < entries.Count; i++)
		{
			if (!allSelected)
			{
				ProfilerThreadRegistry.SetSelected(entries[i].threadId, value: true);
			}
			else if (!entries[i].isMain)
			{
				ProfilerThreadRegistry.SetSelected(entries[i].threadId, value: false);
			}
		}
		session.Invalidate();
	}

	public void DrawButton(ref float x, float y)
	{
		if (items.Count <= 1)
		{
			open = false;
			return;
		}
		float num = ProfilerGui.Width(buttonLabel);
		float num2 = ProfilerGui.Width(buttonBusyText);
		buttonRect = new Rect(x, y, 22f + num + 8f + num2 + 22f, 21f);
		x = buttonRect.xMax;
		ProfilerGui.Bordered(buttonRect, (open || ProfilerGui.Hovered(buttonRect)) ? ProfilerGui.ChipHover : ProfilerGui.ChipBackground, ProfilerGui.Divider);
		ProfilerGui.Fill(new Rect(buttonRect.x + 8f, buttonRect.y + 7f, 7f, 7f), buttonDot);
		ProfilerGui.Label(new Rect(buttonRect.x + 22f, y, num + 4f, 21f), buttonLabel, ProfilerGui.TextBright);
		ProfilerGui.Label(new Rect(buttonRect.x + 22f + num + 8f, y, num2 + 4f, 21f), buttonBusyText, ProfilerGui.TextFaint);
		ProfilerGui.Arrow(new Rect(buttonRect.xMax - 20f, y, 16f, 21f), down: true, ProfilerGui.TextDim);
		if (ProfilerGui.Pressed(buttonRect, open ? SoundDefOf.Click : SoundDefOf.FloatMenu_Open))
		{
			open = !open;
			statsCoverItems = false;
		}
	}

	public void DrawPopup(Rect bounds)
	{
		if (!open || !ProfilerGui.Painting)
		{
			return;
		}
		float a = 26f + (float)items.Count * 24f + 4f;
		popupRect = new Rect(Mathf.Min(buttonRect.x, bounds.xMax - 360f - 4f), buttonRect.yMax + 3f, 360f, Mathf.Min(a, bounds.yMax - buttonRect.yMax - 8f));
		ProfilerGui.Bordered(popupRect, ProfilerGui.TitleBar, ProfilerGui.FocusBorder);
		IReadOnlyList<ProfilerThreadRegistry.Entry> entries = ProfilerThreadRegistry.Entries;
		ProfilerThreadRegistry.GetSelectionSummary(out var selected, out var _);
		bool flag = selected == entries.Count;
		Vector2 mousePosition = Event.current.mousePosition;
		if (popupRect.Contains(mousePosition))
		{
			ProfilerGui.ClearTooltip();
		}
		Rect rect = new Rect(popupRect.x + 1f, popupRect.y + 1f, popupRect.width - 2f, 25f);
		MouseoverSounds.DoRegion(rect);
		if (rect.Contains(mousePosition))
		{
			ProfilerGui.Fill(rect, ProfilerGui.ChipBackground);
		}
		ProfilerGui.Fill(new Rect(rect.x + 10f, rect.y + 9f, 7f, 7f), flag ? MultiDot : DotOff);
		ProfilerGui.Label(new Rect(rect.x + 26f, rect.y, 160f, rect.height), flag ? ("all " + entries.Count + " threads") : ("select all " + entries.Count), ProfilerGui.TextBright);
		ProfilerGui.Label(new Rect(rect.x, rect.y, rect.width - 10f, rect.height), selected + " of " + entries.Count + " selected", ProfilerGui.TextFaint, TextAnchor.MiddleRight);
		ProfilerGui.Fill(new Rect(popupRect.x + 1f, rect.yMax, popupRect.width - 2f, 1f), ProfilerGui.Divider);
		for (int i = 0; i < items.Count; i++)
		{
			Rect rect2 = new Rect(popupRect.x + 1f, popupRect.y + 26f + (float)i * 24f, popupRect.width - 2f, 24f);
			if (!(rect2.yMax > popupRect.yMax))
			{
				Item item = items[i];
				bool flag2 = ProfilerThreadRegistry.IsSelected(item.threadId);
				MouseoverSounds.DoRegion(rect2);
				if (rect2.Contains(mousePosition))
				{
					ProfilerGui.Fill(rect2, ProfilerGui.ChipBackground);
				}
				ProfilerGui.Fill(new Rect(rect2.x + 10f, rect2.y + 8.5f, 7f, 7f), flag2 ? item.color : DotOff);
				ProfilerGui.Label(new Rect(rect2.x + 26f, rect2.y, 130f, rect2.height), item.label, flag2 ? ProfilerGui.TextMain : ProfilerGui.TextDim);
				Rect rect3 = new Rect(rect2.x + 162f, rect2.y + 9f, 60f, 6f);
				ProfilerGui.Fill(rect3, ProfilerGui.BarTrack);
				ProfilerGui.Fill(new Rect(rect3.x, rect3.y, rect3.width * item.fraction, rect3.height), (!flag2) ? BarOff : ((item.fraction >= 0.95f) ? BarHot : BarOn));
				ProfilerGui.Label(new Rect(rect2.x + 226f, rect2.y, 60f, rect2.height), item.busyText, ProfilerGui.TextDim, TextAnchor.MiddleRight);
				ProfilerGui.Label(new Rect(rect2.x + 288f, rect2.y, 62f, rect2.height), item.utilText, ProfilerGui.TextFaint, TextAnchor.MiddleRight);
				continue;
			}
			break;
		}
	}
}
