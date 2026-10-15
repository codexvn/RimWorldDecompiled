using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using Waylith.Profiling;
using Waylith.Profiling.Instrumentation;

namespace Verse.ProfilerUI;

public sealed class ProfilerSettingsDrawer
{
	public const string ControlPrefix = "FrameProfiler";

	private const float Width = 400f;

	private const float HeaderHeight = 38f;

	private const float RowHeight = 30f;

	private const float SectionHeight = 28f;

	private const float Pad = 14f;

	private const float TargetsHeight = 96f;

	private const float ScrollUnitsPerNotch = 3f;

	private static readonly char[] TargetSeparators = new char[2] { '\n', '\r' };

	private readonly ProfilerSession session;

	private string capacityDraft;

	private string fpsDraft;

	private string targetsDraft;

	private string targetsMatchText = "";

	private string targetsUnmatchedText = "";

	private bool targetsDirty;

	private float scrollY;

	private float contentHeight;

	private Rect panelRect;

	private EventType maskedType = EventType.ignore;

	private bool masked;

	private bool bodyHovered;

	public bool IsOpen { get; private set; }

	public ProfilerSettingsDrawer(ProfilerSession session)
	{
		this.session = session;
	}

	public void Toggle()
	{
		IsOpen = !IsOpen;
		if (IsOpen)
		{
			LoadDrafts();
		}
	}

	private void LoadDrafts()
	{
		capacityDraft = FrameProfiler.HistoryCapacity.ToString();
		fpsDraft = (1000f / session.BudgetMs).ToString("0", CultureInfo.InvariantCulture);
		targetsDraft = string.Join("\n", ProfilerPrefs.Current.instrumentationTargets);
		RefreshTargetMatches();
	}

	public void MaskInput(Rect windowRect)
	{
		masked = false;
		if (IsOpen)
		{
			panelRect = PanelRect(windowRect);
			ProfilerGui.AddOverlay(panelRect);
			Event current = Event.current;
			if ((current.type == EventType.mouseDown || current.type == EventType.scrollWheel) && panelRect.Contains(current.mousePosition))
			{
				maskedType = current.type;
				current.type = EventType.ignore;
				masked = true;
			}
		}
	}

	private static Rect PanelRect(Rect windowRect)
	{
		return new Rect(windowRect.xMax - 400f - 1f, windowRect.y + 31f, 400f, windowRect.height - 32f);
	}

	public void Draw(Rect windowRect)
	{
		if (IsOpen)
		{
			Event current = Event.current;
			if (masked && current.type == EventType.ignore)
			{
				current.type = maskedType;
			}
			masked = false;
			ProfilerGui.ClearOverlays();
			panelRect = PanelRect(windowRect);
			if (panelRect.Contains(current.mousePosition))
			{
				ProfilerGui.ClearTooltip();
			}
			ProfilerGui.Fill(panelRect, ProfilerGui.TitleBar);
			ProfilerGui.Fill(new Rect(panelRect.x, panelRect.y, 1f, panelRect.height), ProfilerGui.FocusBorder);
			DrawHeader(new Rect(panelRect.x + 1f, panelRect.y, panelRect.width - 1f, 38f));
			Rect position = new Rect(panelRect.x + 1f, panelRect.y + 38f, panelRect.width - 1f, panelRect.height - 38f);
			float max = Mathf.Max(0f, contentHeight - position.height);
			if (current.type == EventType.scrollWheel && position.Contains(current.mousePosition))
			{
				scrollY += current.delta.y / 3f * 40f;
				current.Use();
			}
			scrollY = Mathf.Clamp(scrollY, 0f, max);
			bodyHovered = position.Contains(current.mousePosition);
			GUI.BeginGroup(position);
			try
			{
				contentHeight = DrawBody(position.width, 0f - scrollY) + scrollY;
			}
			finally
			{
				GUI.EndGroup();
			}
			if (current.type == EventType.mouseDown && panelRect.Contains(current.mousePosition))
			{
				current.Use();
			}
		}
	}

	private void DrawHeader(Rect rect)
	{
		ProfilerGui.Label(new Rect(rect.x + 14f, rect.y, 80f, rect.height), "Settings", ProfilerGui.TextMain);
		Rect rect2 = new Rect(rect.xMax - 14f - 48f, rect.y + 8f, 48f, 22f);
		if (ProfilerGui.Chip(rect2, "Done", active: false, enabled: true, null))
		{
			IsOpen = false;
		}
		float num = ProfilerGui.Width("Reset to defaults") + 18f;
		if (ProfilerGui.Ghost(new Rect(rect2.x - num - 6f, rect2.y, num, 22f), "Reset to defaults"))
		{
			ProfilerPrefs.Reset();
			FrameProfiler.ClearMutedLabels();
			OnFoldRecursionChanged();
			LoadDrafts();
		}
		ProfilerGui.Fill(new Rect(rect.x, rect.yMax - 1f, rect.width, 1f), ProfilerGui.Divider);
	}

	private float DrawBody(float w, float y)
	{
		ProfilerPrefsData prefs = ProfilerPrefs.Current;
		Section(ref y, w, "CAPTURE");
		if (Checkbox(ref y, w, "Start on boot", prefs.startOnBoot))
		{
			Change(delegate
			{
				prefs.startOnBoot = !prefs.startOnBoot;
			});
		}
		if (Checkbox(ref y, w, "Keep running when closed", prefs.keepRunningWhenClosed))
		{
			Change(delegate
			{
				prefs.keepRunningWhenClosed = !prefs.keepRunningWhenClosed;
			});
		}
		if (Field(ref y, w, "Ring buffer", "frames", "Capacity", ref capacityDraft, FrameProfiler.HistoryCapacity.ToString()) && int.TryParse(capacityDraft.Replace(" ", "").Replace(",", ""), out var frames))
		{
			Change(delegate
			{
				prefs.historyCapacity = frames;
			});
			capacityDraft = FrameProfiler.HistoryCapacity.ToString();
		}
		if (Checkbox(ref y, w, "Allocation tracking", prefs.allocTracking, FrameProfiler.AllocationTrackingSupported))
		{
			Change(delegate
			{
				prefs.allocTracking = !prefs.allocTracking;
			});
		}
		if (Checkbox(ref y, w, "Main thread only", prefs.mainThreadOnly))
		{
			Change(delegate
			{
				prefs.mainThreadOnly = !prefs.mainThreadOnly;
			});
		}
		if (Checkbox(ref y, w, "Ignore throttle", prefs.ignoreThrottle))
		{
			Change(delegate
			{
				prefs.ignoreThrottle = !prefs.ignoreThrottle;
			});
		}
		Section(ref y, w, "INSTRUMENTATION");
		bool isSupported = ProfilerInstrumentation.IsSupported;
		if (Checkbox(ref y, w, "Enabled", prefs.instrumentation, isSupported))
		{
			Change(delegate
			{
				prefs.instrumentation = !prefs.instrumentation;
			});
		}
		DrawTargets(ref y, w, isSupported);
		if (Checkbox(ref y, w, "Auto-mute trivial scopes", prefs.autoMuteTrivial))
		{
			Change(delegate
			{
				prefs.autoMuteTrivial = !prefs.autoMuteTrivial;
			});
		}
		Section(ref y, w, "TIMELINE");
		if (Field(ref y, w, "FPS target", "fps", "Fps", ref fpsDraft, (1000f / session.BudgetMs).ToString("0", CultureInfo.InvariantCulture)) && float.TryParse(fpsDraft.Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out var fps) && fps > 0f)
		{
			Change(delegate
			{
				prefs.budgetMs = 1000f / fps;
			});
			fpsDraft = (1000f / session.BudgetMs).ToString("0", CultureInfo.InvariantCulture);
		}
		Section(ref y, w, "CALL TREE");
		if (Checkbox(ref y, w, "Fold recursion", prefs.foldRecursion))
		{
			Change(delegate
			{
				prefs.foldRecursion = !prefs.foldRecursion;
			});
			OnFoldRecursionChanged();
		}
		return y + 14f;
	}

	private void OnFoldRecursionChanged()
	{
		session.Tree.FoldRecursion = ProfilerPrefs.Current.foldRecursion;
		session.Invalidate();
	}

	private static void Change(Action change)
	{
		change();
		ProfilerPrefs.Apply();
		ProfilerPrefs.Save();
	}

	private static string InstrumentationStats()
	{
		if (!ProfilerInstrumentation.IsEnabled)
		{
			return "not instrumented";
		}
		int mutedMethodCount = ProfilerInstrumentation.MutedMethodCount;
		int num = ProfilerInstrumentation.MatchedMethodCount - ProfilerInstrumentation.InstrumentedMethodCount - mutedMethodCount;
		return ProfilerInstrumentation.InstrumentedMethodCount.ToString("n0") + " instrumented / " + ((mutedMethodCount == 0) ? "" : (mutedMethodCount.ToString("n0") + " muted / ")) + num.ToString("n0") + " skipped as trivial";
	}

	private static void Section(ref float y, float w, string title)
	{
		ProfilerGui.Label(new Rect(14f, y + 8f, 160f, 20f), title, ProfilerGui.TextFaint);
		ProfilerGui.Fill(new Rect(14f, y + 28f - 1f, w - 28f, 1f), ProfilerGui.Divider);
		y += 28f;
	}

	private static void RowLabel(float y, float w, string label, bool enabled, float controlWidth)
	{
		ProfilerGui.Label(new Rect(14f, y, w - 28f - controlWidth, 30f), label, enabled ? ProfilerGui.TextMain : ProfilerGui.TextDisabled);
	}

	private bool Checkbox(ref float y, float w, string label, bool value, bool enabled = true)
	{
		RowLabel(y, w, label, enabled, 40f);
		bool checkOn = value;
		if (enabled && bodyHovered)
		{
			Widgets.ToggleInvisibleDraggable(new Rect(0f, y, w, 30f), ref checkOn, doMouseoverSound: true);
		}
		Widgets.CheckboxDraw(w - 14f - 24f, y + 3f, checkOn, !enabled);
		y += 30f;
		return checkOn != value;
	}

	private static bool Field(ref float y, float w, string label, string unit, string controlName, ref string draft, string applied)
	{
		RowLabel(y, w, label, enabled: true, 150f);
		float num = ProfilerGui.Width(unit) + 6f;
		Rect rect = new Rect(w - 14f - num - 64f, y + 4f, 64f, 22f);
		ProfilerGui.Label(new Rect(rect.xMax + 6f, rect.y, num, rect.height), unit, ProfilerGui.TextFaint);
		string text = "FrameProfiler" + controlName;
		Event current = Event.current;
		bool num2 = current.type == EventType.keyDown && (current.keyCode == KeyCode.Return || current.keyCode == KeyCode.KeypadEnter) && GUI.GetNameOfFocusedControl() == text;
		ProfilerGui.Bordered(rect, ProfilerGui.FieldBackground, ProfilerGui.Divider);
		Text.Font = GameFont.Tiny;
		GUI.SetNextControlName(text);
		draft = Widgets.TextField(rect.ContractedBy(1f), draft ?? applied);
		Text.Font = GameFont.Small;
		bool result = num2;
		if (draft != applied && ProfilerGui.Chip(new Rect(rect.x - 50f, rect.y, 44f, 22f), "Apply", active: false, enabled: true, null))
		{
			result = true;
		}
		if (num2)
		{
			current.Use();
		}
		y += 30f;
		return result;
	}

	private void DrawTargets(ref float y, float w, bool enabled)
	{
		ProfilerGui.Label(new Rect(14f, y + 4f, w - 28f, 18f), "Filters", enabled ? ProfilerGui.TextMain : ProfilerGui.TextDisabled);
		if (ProfilerGui.Chip(new Rect(w - 14f - 82f, y + 2f, 82f, 20f), "Assemblies...", active: false, enabled, null))
		{
			Find.WindowStack.TryRemove(typeof(Dialog_ProfilerAssemblies));
			Find.WindowStack.Add(new Dialog_ProfilerAssemblies(() => targetsDraft ?? "", delegate(string draft)
			{
				targetsDraft = draft;
				RefreshTargetMatches();
			}));
		}
		ProfilerGui.Label(new Rect(14f, y + 20f, w - 28f, 18f), "Assembly!Type.Full.Name::Method. One per line. Supports * and ? wildcards.", ProfilerGui.TextFaint);
		y += 40f;
		Rect rect = new Rect(14f, y, w - 28f, 96f);
		ProfilerGui.Bordered(rect, ProfilerGui.FieldBackground, ProfilerGui.Divider);
		Text.Font = GameFont.Tiny;
		GUI.SetNextControlName("FrameProfilerTargets");
		string text = Widgets.TextArea(rect.ContractedBy(1f), targetsDraft ?? "", !enabled);
		Text.Font = GameFont.Small;
		if (text != targetsDraft)
		{
			targetsDraft = text;
			RefreshTargetMatches();
		}
		y += 100f;
		bool flag = targetsDirty && enabled;
		float num = w - 28f - (flag ? 60f : 0f);
		string text2 = InstrumentationStats();
		string text3 = targetsMatchText + " / " + text2;
		bool flag2 = ProfilerGui.Width(text3) > num;
		ProfilerGui.Label(new Rect(14f, y, num, 20f), flag2 ? targetsMatchText : text3, ProfilerGui.TextFaint);
		if (flag && ProfilerGui.Chip(new Rect(w - 14f - 52f, y, 52f, 20f), "Apply", active: false, enabled: true, null))
		{
			List<string> targets = SplitTargets(targetsDraft);
			Change(delegate
			{
				ProfilerPrefs.Current.instrumentationTargets = targets;
			});
			RefreshTargetMatches();
		}
		y += 22f;
		if (flag2)
		{
			ProfilerGui.Label(new Rect(14f, y - 4f, w - 28f, 20f), text2, ProfilerGui.TextFaint);
			y += 18f;
		}
		if (targetsUnmatchedText.Length > 0)
		{
			ProfilerGui.Label(new Rect(14f, y, w - 28f, 18f), targetsUnmatchedText, ProfilerGui.Regression);
			y += 20f;
		}
	}

	private static List<string> SplitTargets(string text)
	{
		List<string> list = new List<string>();
		string[] array = text.Split(TargetSeparators, StringSplitOptions.RemoveEmptyEntries);
		for (int i = 0; i < array.Length; i++)
		{
			string text2 = array[i].Trim();
			if (text2.Length > 0)
			{
				list.Add(text2);
			}
		}
		return list;
	}

	private void RefreshTargetMatches()
	{
		List<string> list = SplitTargets(targetsDraft ?? "");
		int total;
		int[] array = ProfilerInstrumentation.CountMatches(list, out total);
		List<string> list2 = new List<string>();
		for (int i = 0; i < list.Count; i++)
		{
			if (array[i] == 0)
			{
				list2.Add(list[i]);
			}
		}
		targetsMatchText = total.ToString("n0") + " methods match";
		targetsUnmatchedText = ((list2.Count == 0) ? "" : ("no match: " + string.Join(", ", list2)));
		IReadOnlyList<string> targets = ProfilerInstrumentation.Targets;
		targetsDirty = list.Count != targets.Count;
		for (int j = 0; j < list.Count; j++)
		{
			if (targetsDirty)
			{
				break;
			}
			targetsDirty = list[j] != targets[j];
		}
	}
}
