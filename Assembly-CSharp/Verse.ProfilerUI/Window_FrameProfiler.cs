using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UnityEngine;
using Waylith.Profiling;

namespace Verse.ProfilerUI;

public sealed class Window_FrameProfiler : Window
{
	private const float TitleBarHeight = 30f;

	private const float ToolbarHeight = 32f;

	private const float StatusBarHeight = 24f;

	private const float TabStripHeight = 33f;

	private const float SidePad = 12f;

	private const float ResizeGrip = 16f;

	private const float NavRepeatDelay = 0.35f;

	private const float NavRepeatInterval = 0.05f;

	private const string SearchControlName = "FrameProfilerSearch";

	private const string Gap = "    ";

	private static readonly Vector2 MinSize = new Vector2(640f, 420f);

	private static readonly string[] SourceLabels = new string[2];

	private static readonly string[] HistogramModeLabels = new string[2] { "time", "alloc" };

	private static readonly string[] DisplayLabels = new string[2] { "Call tree", "Flamegraph" };

	private static readonly string[] TabLabels = new string[3] { "CPU", "Alloc", "VRAM" };

	private readonly ProfilerSession session;

	private readonly ProfilerHistogramWidget histogram;

	private readonly ProfilerCallTreeTable callTree;

	private readonly ProfilerAllocTable allocTable;

	private readonly ProfilerVramTable vramTable;

	private ProfilerRowSection activeSection;

	private readonly ProfilerFlamechartWidget flamechart;

	private readonly ProfilerThreadFilter threadFilter;

	private readonly ProfilerSettingsDrawer settings;

	private readonly List<int> zoneScratch = new List<int>();

	private readonly List<ProfilerLabelId> pathScratch = new List<ProfilerLabelId>();

	private float flameFraction = 0.45454547f;

	private bool draggingSplitter;

	private ProfilerRefreshGate summaryGate;

	private bool hadData;

	private bool movingWindow;

	private bool resizingWindow;

	private Vector2 dragGrab;

	private KeyCode navKey;

	private float navNextTime;

	private string frameStepperText = "Frame —";

	private string medianText = "";

	private string p99Text = "";

	private string budgetText = "";

	private string summaryHead = "";

	private string summaryTotalLabel = "";

	private string summaryTotal = "";

	private string summaryDelta = "";

	private string summaryTail = "";

	private int summaryTotalSeverity;

	private int summaryDeltaSeverity;

	private string statusText = "";

	private long slowUpdateTimestamp;

	private float overheadPercent;

	private int lastToolbarVersion = -1;

	private int lastToolbarOrdinal = int.MinValue;

	private int lastToolbarCaptureCount = -1;

	private string searchQuery = "";

	private readonly List<(float totalMicroseconds, int pathId)> allocJumpScratch = new List<(float, int)>();

	private bool mouseHeldFromWindow;

	private bool anyButtonLastFrame;

	private bool searchFocused;

	private bool unfocusSearchPending;

	public override Vector2 InitialSize => new Vector2(1100f, 760f);

	protected override float Margin => 0f;

	public override bool IsDebug => true;

	public Window_FrameProfiler()
	{
		layer = WindowLayer.Super;
		doWindowBackground = false;
		drawShadow = false;
		doCloseX = false;
		closeOnAccept = false;
		closeOnClickedOutside = false;
		absorbInputAroundWindow = false;
		preventCameraMotion = false;
		forcePause = false;
		focusWhenOpened = true;
		soundAppear = null;
		soundClose = null;
		session = new ProfilerSession();
		histogram = new ProfilerHistogramWidget(session);
		callTree = new ProfilerCallTreeTable(session);
		callTree.CopyViewRequested = CopyView;
		allocTable = new ProfilerAllocTable(session);
		allocTable.CopyViewRequested = CopyView;
		allocTable.ZoneJumpRequested = OnAllocZoneJump;
		allocTable.EnableTrackingRequested = EnableAllocTracking;
		vramTable = new ProfilerVramTable(session);
		vramTable.CopyViewRequested = CopyView;
		activeSection = callTree;
		callTree.RowSelectedByUser = OnRowSelectedByUser;
		flamechart = new ProfilerFlamechartWidget(session);
		threadFilter = new ProfilerThreadFilter(session);
		settings = new ProfilerSettingsDrawer(session);
		flamechart.ZoneSelected = OnZoneSelected;
		session.SelectionReset = OnSelectionReset;
		session.DisplayedFrameChanged = flamechart.ClearHighlight;
	}

	public override void PreOpen()
	{
		base.PreOpen();
		Rect rect = ProfilerPrefs.Current.windowRect;
		if (rect.width >= MinSize.x && rect.height >= MinSize.y)
		{
			windowRect = rect;
		}
		ClampToScreen();
		FrameProfiler.SetEnabled(value: true);
		ProfilerPrefs.Apply();
	}

	public override void PostClose()
	{
		base.PostClose();
		ProfilerPrefs.Current.windowRect = windowRect;
		ProfilerPrefs.Save();
		flamechart.Clear();
		if (ProfilerPrefs.Current.keepRunningWhenClosed)
		{
			FrameProfiler.SetPaused(value: false);
		}
		else
		{
			FrameProfiler.SetEnabled(value: false);
		}
	}

	private void OnSelectionReset()
	{
		flamechart.ClearHighlight();
		callTree.ResetSelection();
		histogram.NotifySourceChanged();
	}

	private void SwitchSection(ProfilerRowSection section)
	{
		if (activeSection != section)
		{
			activeSection = section;
			threadFilter.ShowBytes = section == allocTable;
			section.View.SelectQuietly(-1);
			section.Invalidate();
		}
	}

	private void EnableAllocTracking()
	{
		ProfilerPrefs.Current.allocTracking = true;
		ProfilerPrefs.Apply();
		ProfilerPrefs.Save();
	}

	private void OnAllocZoneJump(ProfilerLabelId labelId)
	{
		if (!session.HasData)
		{
			return;
		}
		session.Tree.CollectPathsWithLabel(labelId, allocJumpScratch);
		if (allocJumpScratch.Count == 0)
		{
			return;
		}
		(float, int) tuple = allocJumpScratch[0];
		for (int i = 1; i < allocJumpScratch.Count; i++)
		{
			if (allocJumpScratch[i].totalMicroseconds > tuple.Item1)
			{
				tuple = allocJumpScratch[i];
			}
		}
		RevealPathInCallTree(tuple.Item2);
		flamechart.HighlightZone(session.Snapshot.captureOrdinal, tuple.Item2);
	}

	private void RevealPathInCallTree(int pathId)
	{
		session.Tree.AppendLabelChain(pathId, pathScratch);
		SwitchSection(callTree);
		if (session.Tree.Inverted)
		{
			session.Tree.Inverted = false;
			session.Invalidate();
		}
		callTree.ExpandTo(pathScratch);
	}

	private void OnRowSelectedByUser()
	{
		if (callTree.TryGetSelectedZones(zoneScratch, out var ordinal))
		{
			flamechart.HighlightZones(ordinal, zoneScratch);
		}
		else
		{
			flamechart.ClearHighlight();
		}
	}

	private void OnZoneSelected(ProfilerZoneSelection zone)
	{
		bool flag = ProfilerPathTable.Entries[zone.pathId].parentPathId == 0;
		bool flag2 = ProfilerThreadRegistry.IsSelected(zone.threadId);
		if (!session.PinOrdinal(zone.ordinal, !flag && flag2, out var selectionChanged))
		{
			return;
		}
		FrameProfiler.FrameSnapshot snapshot;
		bool flag3 = session.TryGetSnapshotForOrdinal(zone.ordinal, out snapshot);
		bool flag4 = flag3 && snapshot.sectionCount > 0 && snapshot.sections[0].threadId == zone.threadId;
		if ((selectionChanged || !flag2) && !flag)
		{
			ProfilerThreadRegistry.SelectOnly(zone.threadId);
			if (!flag4 && flag3)
			{
				for (int i = 1; i < snapshot.sectionCount; i++)
				{
					if (snapshot.sections[i].threadId == zone.threadId)
					{
						long num = snapshot.beginTimeNanoseconds + snapshot.sections[i].startNanoseconds;
						long endNs = Math.Max(snapshot.beginTimeNanoseconds + snapshot.sections[i].endNanoseconds, num + 1);
						session.SelectZoneTimeRange(zone.ordinal, num, endNs);
						break;
					}
				}
			}
			else if (flag3 && !flag2 && !selectionChanged)
			{
				session.SelectZoneTimeRange(zone.ordinal, snapshot.beginTimeNanoseconds, snapshot.endTimeNanoseconds);
			}
		}
		flamechart.HighlightZone(zone.ordinal, flag ? (-1) : zone.pathId);
		if (!flag)
		{
			RevealPathInCallTree(zone.pathId);
		}
	}

	public override void WindowUpdate()
	{
		base.WindowUpdate();
		using (FrameProfiler.BeginRead())
		{
			UpdateNavRepeat();
			UpdateCameraBlock();
			long timestamp = Stopwatch.GetTimestamp();
			int refreshTick = session.RefreshTick;
			session.Poll();
			if (!session.HasData)
			{
				RefreshToolbar();
				if (activeSection == vramTable)
				{
					vramTable.Refresh();
				}
				if (hadData)
				{
					callTree.Clear();
					allocTable.Clear();
					histogram.Clear();
					flamechart.Clear();
				}
				hadData = false;
			}
			else
			{
				hadData = true;
				RefreshToolbar();
				threadFilter.Refresh();
				activeSection.Refresh();
				histogram.Refresh();
				flamechart.Refresh();
				RefreshSummary();
				if (session.RefreshTick != refreshTick)
				{
					session.ReportRefreshCost(timestamp);
				}
			}
		}
	}

	private void UpdateCameraBlock()
	{
		bool flag = Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2);
		if (!flag)
		{
			mouseHeldFromWindow = false;
		}
		else if (!anyButtonLastFrame)
		{
			mouseHeldFromWindow = windowRect.Contains(UI.MousePositionOnUIInverted);
			unfocusSearchPending = !mouseHeldFromWindow && searchFocused;
		}
		anyButtonLastFrame = flag;
		preventCameraMotion = mouseHeldFromWindow || searchFocused;
	}

	private void RefreshToolbar()
	{
		if (lastToolbarVersion != session.Version || lastToolbarOrdinal != session.Ordinal || lastToolbarCaptureCount != FrameProfiler.CompletedFrameCount)
		{
			lastToolbarVersion = session.Version;
			lastToolbarOrdinal = session.Ordinal;
			lastToolbarCaptureCount = FrameProfiler.CompletedFrameCount;
			frameStepperText = (session.HasData ? ("Frame " + session.Snapshot.frameIndex) : "Frame —");
			ProfilerFrameStats stats = session.Stats;
			medianText = FrameMs(stats.medianMs);
			p99Text = FrameMs(stats.p99Ms);
			budgetText = FrameMs(session.BudgetMs);
		}
	}

	private static string FrameMs(float ms)
	{
		StringBuilder scratch = ProfilerGui.Scratch;
		scratch.Clear();
		ProfilerTreeModel.AppendFrameMs(scratch, ms);
		return scratch.ToString();
	}

	private void RefreshSummary()
	{
		if (!summaryGate.ShouldRefresh(session))
		{
			return;
		}
		FrameProfiler.FrameSnapshot snapshot = session.Snapshot;
		ProfilerFrameStats stats = session.Stats;
		ProfilerTreeModel.WindowSummary windowSummary = ((session.Span > 1) ? session.SummarizeSelection() : default(ProfilerTreeModel.WindowSummary));
		StringBuilder scratch = ProfilerGui.Scratch;
		float num;
		if (session.Span > 1 && windowSummary.framesIncluded > 0)
		{
			num = windowSummary.averageFrameMs;
			summaryHead = "Avg over " + windowSummary.framesIncluded + " frames";
			summaryTotalLabel = "    frame ";
			scratch.Clear();
			ProfilerTreeModel.AppendFrameMs(scratch, num, "0.000");
			summaryTotal = scratch.ToString();
			scratch.Clear();
			scratch.Append("    ").Append("ticks +").Append(windowSummary.averageTickerDelta.ToString("0.0"))
				.Append("    ")
				.Append("nodes ")
				.Append(windowSummary.averageNodeCount.ToString("0"));
			AppendAllocTail(scratch, windowSummary.averageAllocBytes, Mathf.RoundToInt(windowSummary.averageAllocCount), windowSummary.gen0Collections, windowSummary.gen1Collections, windowSummary.gen2Collections, windowSummary.averagePromotedBytes);
			if (windowSummary.averageOverflowedScopes > 0f)
			{
				scratch.Append("    ").Append("dropped scopes/frame ").Append(windowSummary.averageOverflowedScopes.ToString("0.0"));
			}
			summaryTail = scratch.ToString();
		}
		else
		{
			num = snapshot.totalMs;
			summaryHead = "Frame " + snapshot.frameIndex;
			summaryTotalLabel = "    total ";
			summaryTotal = FrameMs(num);
			scratch.Clear();
			scratch.Append("    ").Append("ticks +").Append(snapshot.tickDelta)
				.Append("    ")
				.Append("nodes ")
				.Append(snapshot.publishedNodeCount);
			AppendAllocTail(scratch, snapshot.DisplayAllocBytes, snapshot.allocatedObjectCount, snapshot.gen0Collections, snapshot.gen1Collections, snapshot.gen2Collections, snapshot.promotedBytes);
			if (snapshot.overflowedScopes > 0)
			{
				scratch.Append("    ").Append("dropped scopes ").Append(snapshot.overflowedScopes);
			}
			if (snapshot.unbalancedEnds > 0)
			{
				scratch.Append("    ").Append("unbalanced ends ").Append(snapshot.unbalancedEnds);
			}
			if (snapshot.droppedEvents > 0)
			{
				scratch.Append("    ").Append("dropped events ").Append(snapshot.droppedEvents);
			}
			if (snapshot.isSpike)
			{
				scratch.Append("    ").Append("SPIKE");
			}
			summaryTail = scratch.ToString();
		}
		float num2 = num - stats.medianMs;
		summaryTotalSeverity = ((num > session.BudgetMs) ? 1 : 0);
		summaryDeltaSeverity = ((num2 >= 0.5f) ? 1 : ((num2 <= -0.5f) ? (-1) : 0));
		scratch.Clear();
		if (num2 >= 0f)
		{
			scratch.Append('+');
		}
		ProfilerTreeModel.AppendMs(scratch, num2);
		summaryDelta = scratch.ToString();
		long timestamp = Stopwatch.GetTimestamp();
		if (timestamp - slowUpdateTimestamp >= Stopwatch.Frequency)
		{
			slowUpdateTimestamp = timestamp;
			float num3 = (float)((snapshot.sectionCount > 0) ? snapshot.sections[0].recordedScopes : 0) * FrameProfiler.ScopeOverheadNanoseconds / 1000000f;
			float num4 = ((snapshot.totalMs > 0f) ? (num3 / snapshot.totalMs * 100f) : 0f);
			overheadPercent = ((overheadPercent > 0f) ? Mathf.Lerp(overheadPercent, num4, 0.3f) : num4);
		}
		scratch.Clear();
		float scopeOverheadNanoseconds = FrameProfiler.ScopeOverheadNanoseconds;
		if (scopeOverheadNanoseconds > 0f)
		{
			scratch.Append("overhead ").Append(scopeOverheadNanoseconds.ToString("0")).Append(" ns/scope");
			if (overheadPercent > 0f)
			{
				scratch.Append(" (~").Append(overheadPercent.ToString("0.00")).Append("% of frame)");
			}
			scratch.Append(" / ");
		}
		scratch.Append("timer res ").Append(FrameProfiler.TimerResolutionNanoseconds.ToString("0.#")).Append(" ns");
		statusText = scratch.ToString();
	}

	private static void AppendAllocTail(StringBuilder builder, long allocBytes, int allocCount, int gen0, int gen1, int gen2, long promotedBytes)
	{
		if (allocBytes > 0)
		{
			builder.Append("    ").Append("alloc ");
			ProfilerTreeModel.AppendAllocBytes(builder, allocBytes);
			if (allocCount > 0)
			{
				builder.Append(" (").Append(allocCount.ToString("n0")).Append(" objs)");
			}
		}
		if (promotedBytes > 0)
		{
			builder.Append("    ").Append("promoted ");
			ProfilerTreeModel.AppendAllocBytes(builder, promotedBytes);
		}
		if (gen0 + gen1 + gen2 > 0)
		{
			builder.Append("    ").Append("GC ").Append(gen0)
				.Append('/')
				.Append(gen1)
				.Append('/')
				.Append(gen2);
		}
	}

	public override void DoWindowContents(Rect inRect)
	{
		using (FrameProfiler.BeginRead())
		{
			if (unfocusSearchPending)
			{
				unfocusSearchPending = false;
				searchFocused = false;
				UI.UnfocusCurrentControl();
			}
			ProfilerGui.ClearOverlays();
			settings.MaskInput(inRect);
			threadFilter.HandleInput();
			HandleWindowDrag(inRect);
			HandleKeys();
			bool flag = IsTopmost();
			ProfilerGui.Fill(inRect, flag ? ProfilerGui.FocusBorder : ProfilerGui.SurfaceBorder);
			ProfilerGui.Fill(inRect.ContractedBy(1f), ProfilerGui.WindowBackground);
			float num = inRect.y + 1f;
			DrawTitleBar(new Rect(inRect.x + 1f, num, inRect.width - 2f, 30f), flag);
			num += 30f;
			DrawToolbar(new Rect(inRect.x + 1f, num, inRect.width - 2f, 32f));
			num += 32f;
			ProfilerGui.Fill(new Rect(inRect.x + 1f, num, inRect.width - 2f, 1f), ProfilerGui.Divider);
			num += 8f;
			DrawHistogramHeader(new Rect(inRect.x + 12f, num, inRect.width - 24f, 22f));
			num += 28f;
			histogram.Draw(new Rect(inRect.x + 12f, num, inRect.width - 24f, 104f));
			num += 110f;
			DrawSummary(new Rect(inRect.x + 12f, num, inRect.width - 24f, 16f));
			num += 23f;
			ProfilerGui.Fill(new Rect(inRect.x + 1f, num, inRect.width - 2f, 1f), ProfilerGui.Divider);
			float num2 = inRect.yMax - 24f - 1f;
			num = DrawFlameSection(inRect, num + 1f, num2);
			ProfilerGui.Fill(Rect.MinMaxRect(inRect.x + 1f, num + 1f, inRect.xMax - 1f, num2), ProfilerGui.SectionBackground);
			DrawTabStrip(new Rect(inRect.x + 12f, num + 6f, inRect.width - 24f, 21f));
			num += 33f;
			ProfilerGui.Fill(new Rect(inRect.x + 1f, num, inRect.width - 2f, 1f), ProfilerGui.Divider);
			Rect rect = Rect.MinMaxRect(inRect.x + 1f, num + 1f, inRect.xMax - 1f, num2);
			if (session.HasData || activeSection == vramTable)
			{
				activeSection.Draw(rect);
			}
			else
			{
				ProfilerGui.Label(rect, session.IsSpikes ? "No spikes captured yet." : "No completed frames captured yet.", ProfilerGui.TextFaint, TextAnchor.MiddleCenter);
			}
			DrawStatusBar(new Rect(inRect.x + 1f, num2, inRect.width - 2f, 24f));
			threadFilter.DrawPopup(inRect);
			settings.Draw(inRect);
			ProfilerGui.FlushTooltip(inRect);
		}
	}

	private bool IsTopmost()
	{
		if (!ProfilerGui.Painting)
		{
			return true;
		}
		IList<Window> windows = Find.WindowStack.Windows;
		if (windows.Count > 0)
		{
			return windows[windows.Count - 1] == this;
		}
		return false;
	}

	private float DrawFlameSection(Rect inRect, float top, float bottom)
	{
		float num = bottom - top - 5f;
		if (num < 160f)
		{
			return top - 1f;
		}
		float num2 = Mathf.Clamp(Mathf.Round(num * flameFraction), 80f, num - 80f);
		Rect rect = new Rect(inRect.x + 1f, top + num2, inRect.width - 2f, 5f);
		Event current = Event.current;
		if (current.type == EventType.mouseDown && current.button == 0 && rect.Contains(current.mousePosition))
		{
			draggingSplitter = true;
			current.Use();
		}
		else if (draggingSplitter && current.type == EventType.repaint)
		{
			if (!Input.GetMouseButton(0))
			{
				draggingSplitter = false;
			}
			else
			{
				flameFraction = Mathf.Clamp01((current.mousePosition.y - top - 2.5f) / num);
			}
		}
		if (session.HasData)
		{
			flamechart.Draw(new Rect(inRect.x + 1f, top, inRect.width - 2f, num2));
		}
		else
		{
			ProfilerGui.Fill(new Rect(inRect.x + 1f, top, inRect.width - 2f, num2), ProfilerGui.SunkenBackground);
		}
		ProfilerGui.Fill(rect, (draggingSplitter || rect.Contains(current.mousePosition)) ? ProfilerGui.FocusBorder : ProfilerGui.Divider);
		return rect.yMax - 1f;
	}

	private void DrawTitleBar(Rect rect, bool focused)
	{
		ProfilerGui.Fill(rect, focused ? ProfilerGui.TitleBar : ProfilerGui.TitleBarUnfocused);
		ProfilerGui.Fill(new Rect(rect.x, rect.yMax - 1f, rect.width, 1f), ProfilerGui.Divider);
		ProfilerGui.Run(rect.x + 12f, rect.y, rect.height, "Frame profiler", focused ? ProfilerGui.TextMain : ProfilerGui.TextDim);
		float num = rect.xMax - 8f;
		Rect rect2 = new Rect(num - 22f, rect.y + 4f, 22f, 22f);
		if (ProfilerGui.Ghost(rect2, "×"))
		{
			Close();
			return;
		}
		num = rect2.x - 8f;
		if (TitleToggle(ref num, rect, "Settings", settings.IsOpen))
		{
			settings.Toggle();
		}
	}

	private static bool TitleToggle(ref float right, Rect bar, string label, bool active)
	{
		float num = ProfilerGui.Width(label) + 18f;
		Rect rect = new Rect(right - num, bar.y + 5f, num, 20f);
		right = rect.x - 4f;
		return ProfilerGui.Chip(rect, label, active, enabled: true, null);
	}

	private void DrawToolbar(Rect rect)
	{
		float x = rect.x + 12f;
		float num = rect.y + 5f;
		Rect rect2 = new Rect(x, num, 64f, 22f);
		if (ProfilerGui.Chip(rect2, session.IsPaused ? "Resume" : "Pause", session.IsPaused, enabled: true, null))
		{
			session.SetPaused(!session.IsPaused);
		}
		x = rect2.xMax + 8f;
		ToolbarDivider(ref x, num);
		float num2 = Mathf.Max(72f, ProfilerGui.Width(frameStepperText)) + 16f;
		Rect rect3 = new Rect(x, num, 24f + num2 + 24f + 30f + 3f, 22f);
		ProfilerGui.Bordered(rect3, ProfilerGui.TitleBar, ProfilerGui.Divider);
		Rect rect4 = new Rect(rect3.x + 1f, num + 1f, 23f, 20f);
		if (ProfilerGui.Ghost(rect4, "◀"))
		{
			session.Step(1);
		}
		ProfilerGui.Fill(new Rect(rect4.xMax, num + 1f, 1f, 20f), ProfilerGui.Divider);
		ProfilerGui.Label(new Rect(rect4.xMax + 1f, num, num2, 22f), frameStepperText, ProfilerGui.TextBright, TextAnchor.MiddleCenter);
		rect4 = new Rect(rect4.xMax + 1f + num2, num + 1f, 24f, 20f);
		ProfilerGui.Fill(new Rect(rect4.x, num + 1f, 1f, 20f), ProfilerGui.Divider);
		if (ProfilerGui.Ghost(new Rect(rect4.x + 1f, rect4.y, 23f, 20f), "▶"))
		{
			session.Step(-1);
		}
		ProfilerGui.Fill(new Rect(rect4.xMax, num + 1f, 1f, 20f), ProfilerGui.Divider);
		if (ProfilerGui.Ghost(new Rect(rect4.xMax + 1f, rect4.y, 29f, 20f), "▶▶"))
		{
			session.JumpToNewest();
		}
		x = rect3.xMax + 8f;
		ToolbarDivider(ref x, num);
		SourceLabels[0] = "Ring buffer";
		SourceLabels[1] = "Spikes " + FrameProfiler.SpikeCount;
		int num3 = ProfilerGui.Segments(ref x, num, SourceLabels, session.IsSpikes ? 1 : 0, null, 1);
		if (num3 >= 0)
		{
			session.SwitchSource((num3 == 1) ? ProfilerFrameSource.Spikes : ProfilerFrameSource.History);
			histogram.NotifySourceChanged();
		}
		RightRun(RightRun(RightRun(RightRun(RightRun(RightRun(rect.xMax - 12f, num, budgetText, ProfilerGui.TextDim), num, "    budget ", ProfilerGui.TextFaint), num, p99Text, ProfilerGui.TextDim), num, "    p99 ", ProfilerGui.TextFaint), num, medianText, ProfilerGui.TextDim), num, "median ", ProfilerGui.TextFaint);
	}

	private static float RightRun(float right, float y, string text, Color color)
	{
		return ProfilerGui.RunRightAligned(right, y, 22f, text, color);
	}

	private static void ToolbarDivider(ref float x, float y)
	{
		ProfilerGui.Fill(new Rect(x, y + 3f, 1f, 16f), ProfilerGui.Divider);
		x += 9f;
	}

	private void DrawHistogramHeader(Rect rect)
	{
		float x = rect.x;
		int num = ProfilerGui.Segments(ref x, rect.y, HistogramModeLabels, histogram.AllocMode ? 1 : 0, null);
		if (num >= 0)
		{
			histogram.SetAllocMode(num == 1);
		}
		if (session.IsSpikes)
		{
			x = ProfilerGui.Run(x + 10f, rect.y, rect.height, "Saved spike frames", ProfilerGui.TextMuted);
			ProfilerGui.Run(x + 10f, rect.y, rect.height, "trigger from " + 25f.ToString("0") + " ms    from " + 1.75f.ToString("0.0#") + "× avg    keep last " + 24, ProfilerGui.TextFaint);
		}
		if (ProfilerGui.Ghost(new Rect(rect.xMax - 48f, rect.y, 48f, 21f), "Clear"))
		{
			session.ClearFrames();
			histogram.Clear();
		}
	}

	private void DrawSummary(Rect rect)
	{
		if (session.HasData)
		{
			ProfilerGui.Run(ProfilerGui.Run(ProfilerGui.Run(ProfilerGui.Run(ProfilerGui.Run(ProfilerGui.Run(rect.x, rect.y, rect.height, summaryHead, ProfilerGui.TextMain), rect.y, rect.height, summaryTotalLabel, ProfilerGui.TextDim), rect.y, rect.height, summaryTotal, (summaryTotalSeverity > 0) ? ProfilerGui.Regression : ProfilerGui.TextMain), rect.y, rect.height, "    Δ vs median ", ProfilerGui.TextDim), rect.y, rect.height, summaryDelta, (summaryDeltaSeverity > 0) ? ProfilerGui.Regression : ((summaryDeltaSeverity < 0) ? ProfilerGui.Improvement : ProfilerGui.TextDim)), rect.y, rect.height, summaryTail, ProfilerGui.TextDim);
		}
	}

	private void DrawTabStrip(Rect rect)
	{
		float x = rect.x;
		int num = ProfilerGui.Segments(ref x, rect.y - 0.5f, TabLabels, (activeSection != callTree) ? ((activeSection == allocTable) ? 1 : 2) : 0, null);
		if (num >= 0)
		{
			SwitchSection(num switch
			{
				1 => allocTable, 
				0 => callTree, 
				_ => vramTable, 
			});
		}
		x += 10f;
		if (activeSection == callTree)
		{
			int num2 = ProfilerGui.Segments(ref x, rect.y - 0.5f, DisplayLabels, callTree.FlamegraphMode ? 1 : 0, null);
			if (num2 >= 0)
			{
				callTree.SetFlamegraphMode(num2 == 1);
			}
			x += 10f;
		}
		if (activeSection != vramTable)
		{
			threadFilter.DrawButton(ref x, rect.y);
		}
		float xMax = rect.xMax;
		Rect rect2 = new Rect(xMax - 44f, rect.y, 44f, 21f);
		if (ProfilerGui.Ghost(rect2, "Copy"))
		{
			CopyView();
		}
		xMax = rect2.x - 8f;
		Rect rect3 = new Rect(xMax - 24f, rect.y, 24f, 21f);
		Rect rect4 = new Rect(rect3.x - 24f, rect.y, 24f, 21f);
		if (activeSection != callTree || !callTree.FlamegraphMode)
		{
			if (ProfilerGui.Ghost(rect3, "−"))
			{
				activeSection.CollapseAll();
			}
			if (ProfilerGui.Ghost(rect4, "+"))
			{
				activeSection.ExpandAll();
			}
		}
		xMax = rect4.x - 8f;
		if (activeSection == vramTable)
		{
			xMax = TabButton(xMax, rect.y, "Export", active: false, out var clicked);
			if (clicked)
			{
				vramTable.Export();
			}
			xMax = TabButton(xMax, rect.y, "Δ sort", vramTable.SortByDelta, out var clicked2);
			if (clicked2)
			{
				vramTable.SortByDelta = !vramTable.SortByDelta;
			}
			if (vramTable.HasBaseline)
			{
				xMax = TabButton(xMax, rect.y, "Clear Δ", active: false, out var clicked3);
				if (clicked3)
				{
					vramTable.ClearBaseline();
				}
			}
			xMax = TabButton(xMax, rect.y, "Baseline", active: false, out var clicked4);
			if (clicked4)
			{
				vramTable.CaptureBaseline();
			}
		}
		else
		{
			bool active = ((activeSection == allocTable) ? allocTable.IsInverted : session.Tree.Inverted);
			xMax = TabButton(xMax, rect.y, "↑↓ Invert", active, out var clicked5);
			if (clicked5 && activeSection == allocTable)
			{
				allocTable.IsInverted = !allocTable.IsInverted;
			}
			else if (clicked5)
			{
				session.Tree.Inverted = !session.Tree.Inverted;
				callTree.ResetSelection();
				session.Invalidate();
			}
		}
		Rect rect5 = new Rect(xMax - 180f, rect.y, 180f, 21f);
		ProfilerGui.Bordered(rect5, ProfilerGui.FieldBackground, ProfilerGui.Divider);
		Text.Font = GameFont.Tiny;
		GUI.SetNextControlName("FrameProfilerSearch");
		string text = Widgets.TextField(rect5.ContractedBy(1f), searchQuery);
		Text.Font = GameFont.Small;
		if (text != searchQuery)
		{
			searchQuery = text;
			session.Search.Query = text;
			callTree.Notify_SearchChanged();
			allocTable.Notify_SearchChanged();
			vramTable.Notify_SearchChanged();
		}
		if (ProfilerGui.Painting)
		{
			searchFocused = GUI.GetNameOfFocusedControl().StartsWith("FrameProfiler");
		}
		if (searchQuery.Length == 0 && GUI.GetNameOfFocusedControl() != "FrameProfilerSearch")
		{
			ProfilerGui.Label(new Rect(rect5.x + 8f, rect5.y, rect5.width, rect5.height), "search", ProfilerGui.TextFaint);
		}
		xMax = rect5.x - 6f;
		string searchStatus = activeSection.View.SearchStatus;
		if (searchStatus.Length > 0)
		{
			float num3 = ProfilerGui.Width(searchStatus);
			ProfilerGui.Label(new Rect(xMax - num3 - 4f, rect.y, num3 + 4f, 21f), searchStatus, ProfilerGui.TextDim);
		}
	}

	private static float TabButton(float right, float y, string label, bool active, out bool clicked)
	{
		float num = ProfilerGui.Width(label) + 18f;
		Rect rect = new Rect(right - num, y, num, 21f);
		clicked = ProfilerGui.Chip(rect, label, active, enabled: true, null);
		return rect.x - 8f;
	}

	private void DrawStatusBar(Rect rect)
	{
		ProfilerGui.Fill(new Rect(rect.x, rect.y, rect.width, 1f), ProfilerGui.Divider);
		ProfilerGui.Label(new Rect(rect.x + 12f, rect.y + 1f, rect.width - 24f - 16f, rect.height - 1f), statusText, ProfilerGui.TextFaint);
		Color color = ((ProfilerGui.Hovered(new Rect(rect.xMax - 16f, rect.yMax - 16f, 16f, 16f)) || resizingWindow) ? ProfilerGui.TextDim : ProfilerGui.TextFaint);
		for (int i = 0; i < 3; i++)
		{
			ProfilerGui.Fill(new Rect(rect.xMax - 6f - (float)i * 4f, rect.yMax - 6f, 2f, 2f), color);
		}
		for (int j = 0; j < 2; j++)
		{
			ProfilerGui.Fill(new Rect(rect.xMax - 6f - (float)j * 4f, rect.yMax - 10f, 2f, 2f), color);
		}
		ProfilerGui.Fill(new Rect(rect.xMax - 6f, rect.yMax - 14f, 2f, 2f), color);
	}

	private void HandleWindowDrag(Rect inRect)
	{
		Event current = Event.current;
		if (current.type == EventType.mouseDown && current.button == 0)
		{
			Rect rect = new Rect(inRect.xMax - 16f, inRect.yMax - 16f, 16f, 16f);
			Rect rect2 = new Rect(inRect.x, inRect.y, inRect.width - 200f, 30f);
			if (rect.Contains(current.mousePosition))
			{
				resizingWindow = true;
				dragGrab = windowRect.max - UI.MousePositionOnUIInverted;
				current.Use();
			}
			else if (rect2.Contains(current.mousePosition))
			{
				movingWindow = true;
				dragGrab = UI.MousePositionOnUIInverted - windowRect.position;
				current.Use();
			}
		}
		if (current.type != EventType.repaint || (!movingWindow && !resizingWindow))
		{
			return;
		}
		if (!Input.GetMouseButton(0))
		{
			movingWindow = false;
			resizingWindow = false;
			return;
		}
		Vector2 mousePositionOnUIInverted = UI.MousePositionOnUIInverted;
		if (movingWindow)
		{
			windowRect.position = mousePositionOnUIInverted - dragGrab;
		}
		else
		{
			Vector2 vector = mousePositionOnUIInverted + dragGrab;
			windowRect.width = Mathf.Max(MinSize.x, vector.x - windowRect.x);
			windowRect.height = Mathf.Max(MinSize.y, vector.y - windowRect.y);
		}
		ClampToScreen();
	}

	private void ClampToScreen()
	{
		windowRect.width = Mathf.Min(windowRect.width, UI.screenWidth);
		windowRect.height = Mathf.Min(windowRect.height, UI.screenHeight);
		windowRect.x = Mathf.Clamp(windowRect.x, 0f, (float)UI.screenWidth - windowRect.width);
		windowRect.y = Mathf.Clamp(windowRect.y, 0f, (float)UI.screenHeight - windowRect.height);
	}

	private void HandleKeys()
	{
		Event current = Event.current;
		if (current.type != EventType.keyDown)
		{
			return;
		}
		bool flag = GUI.GetNameOfFocusedControl() == "FrameProfilerSearch";
		if (current.keyCode == KeyCode.F3 || (flag && (current.keyCode == KeyCode.Return || current.keyCode == KeyCode.KeypadEnter)))
		{
			activeSection.View.SelectNextMatch((!current.shift) ? 1 : (-1));
			current.Use();
		}
		else
		{
			if (flag || GUI.GetNameOfFocusedControl().StartsWith("FrameProfiler"))
			{
				return;
			}
			bool flag2 = true;
			switch (current.keyCode)
			{
			case KeyCode.UpArrow:
				activeSection.View.MoveSelection(-1);
				StartNavRepeat(KeyCode.UpArrow);
				break;
			case KeyCode.DownArrow:
				activeSection.View.MoveSelection(1);
				StartNavRepeat(KeyCode.DownArrow);
				break;
			case KeyCode.Backspace:
			case KeyCode.LeftArrow:
				activeSection.CollapseSelected();
				break;
			case KeyCode.RightArrow:
				activeSection.View.ActivateSelected(expandOnly: true);
				break;
			case KeyCode.Return:
			case KeyCode.KeypadEnter:
				activeSection.View.ActivateSelected(expandOnly: false);
				break;
			case KeyCode.PageUp:
				session.Step(1);
				break;
			case KeyCode.PageDown:
				session.Step(-1);
				break;
			case KeyCode.Home:
				session.JumpToNewest();
				break;
			case KeyCode.Space:
				session.SetPaused(!session.IsPaused);
				break;
			case KeyCode.C:
				if (current.control && current.shift)
				{
					CopyView();
					break;
				}
				if (current.control)
				{
					activeSection.View.CopySelectedRow();
					break;
				}
				goto default;
			default:
				flag2 = false;
				break;
			}
			if (flag2)
			{
				current.Use();
			}
		}
	}

	private void StartNavRepeat(KeyCode key)
	{
		navKey = key;
		navNextTime = Time.realtimeSinceStartup + 0.35f;
	}

	private void UpdateNavRepeat()
	{
		if (navKey != 0)
		{
			if (!Input.GetKey(navKey))
			{
				navKey = KeyCode.None;
			}
			else if (!(Time.realtimeSinceStartup < navNextTime))
			{
				navNextTime = Time.realtimeSinceStartup + 0.05f;
				activeSection.View.MoveSelection((navKey != KeyCode.UpArrow) ? 1 : (-1));
			}
		}
	}

	private void CopyView()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(summaryHead).Append(summaryTotalLabel).Append(summaryTotal)
			.Append("    ")
			.Append("Δ vs median ")
			.Append(summaryDelta)
			.Append(summaryTail)
			.AppendLine();
		stringBuilder.Append(statusText).Append(" / median ").Append(medianText)
			.Append(" / p99 ")
			.Append(p99Text)
			.Append(" / budget ")
			.Append(budgetText)
			.AppendLine();
		activeSection.AppendCopyText(stringBuilder);
		ProfilerClipboard.Put(stringBuilder);
	}
}
