using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using UnityStubs.Gui;
using Waylith.Profiling;

namespace Verse.ProfilerUI;

public sealed class ProfilerFlamechartWidget
{
	private struct ZoneDraw
	{
		public ZoneChunk chunk;

		public int index;

		public int depth;

		public double startUs;

		public double endUs;

		public bool folded;
	}

	private sealed class FrameZones
	{
		public List<ZoneDraw>[] perBand = Array.Empty<List<ZoneDraw>>();

		public double[] minVisibleUs = Array.Empty<double>();

		public void Invalidate()
		{
			Array.Fill(minVisibleUs, double.NaN);
		}
	}

	private struct PauseNode
	{
		public int frameIndex;

		public int pauseIndex;

		public float x0;

		public float x1;

		public float laneY;
	}

	private struct CollapsedRun
	{
		public float x0;

		public float x1;

		public float laneY;

		public int count;

		public double totalMicroseconds;

		public int frameIndex;

		public int band;

		public ZoneChunk chunk;

		public int nodeIndex;

		public Color color;
	}

	private struct FrameEntry
	{
		public FrameProfiler.FrameSnapshot snapshot;

		public int ordinal;

		public double startUs;

		public double endUs;

		public double boundaryUs;

		public double maxEndUs;

		public FrameZones zones;
	}

	private struct PausedGapSpan
	{
		public float x0;

		public float x1;

		public double durationUs;
	}

	private struct ZoneHit
	{
		public int frameIndex;

		public int band;

		public ZoneChunk chunk;

		public int nodeIndex;

		public int depth;

		public double startUs;

		public double endUs;

		public bool IsRoot => chunk == null;
	}

	private const float RulerHeight = 18f;

	private static readonly float GcLaneHeight = (FrameProfiler.RuntimePausesSupported ? 13f : 0f);

	private static readonly float PlotTop = 18f + GcLaneHeight;

	private const float GutterWidth = 126f;

	private const double MinViewSpanUs = 0.005;

	private const float LaneHeight = 19f;

	private const float LaneGap = 1f;

	private const float MinVisSize = 3f;

	private const float MinLabelWidth = 40f;

	private const float LabelGap = 4f;

	private const float DetailedFrameMinWidth = 6f;

	private const float FrameSeparatorMinWidth = 28f;

	private const int MaxLabels = 160;

	private const int MaxRulerTicks = 24;

	private static readonly Color RulerTextColor = ProfilerGui.TextFaint;

	private static readonly Color RulerTickColor = ProfilerGui.Hex(4280164906u);

	private static readonly Color GutterLineColor = ProfilerGui.SurfaceBorder;

	private static readonly Color LaneNameColor = ProfilerGui.TextMuted;

	private static readonly Color FrameSeparatorColor = ProfilerGui.Hex(654311423u);

	private static readonly Color BandTintColor = ProfilerGui.Hex(83886079u);

	private static readonly Color BandSeparatorColor = ProfilerGui.Hex(771751936u);

	private static readonly Color ScrollThumbColor = ProfilerGui.Hex(1191182335u);

	private static readonly Color CollapsedRunColor = ProfilerGui.Hex(1509949440u);

	private static readonly Color SelectionBandColor = ProfilerGui.Hex(385875967u);

	private static readonly Color GcPauseColor = ProfilerGui.Regression;

	private static readonly Color PauseWaitColor = ProfilerGui.Hex(1725984863u);

	private static readonly Color OtherPauseColor = ProfilerGui.Hex(4287266774u);

	private static readonly Color PauseBandColor = ProfilerGui.Hex(820015199u);

	private static readonly Color OutsideRangeColor = ProfilerGui.Hex(1140850688u);

	private bool contentVisible;

	private bool newestZonesStale = true;

	private float width;

	private float height;

	private float[] laneLabelEnds = new float[64];

	private readonly List<ZoneChunk> chunkScratch = new List<ZoneChunk>();

	private readonly Stack<FrameZones> frameZonesPool = new Stack<FrameZones>();

	private readonly List<int> ancestorEnds = new List<int>();

	private readonly List<PauseNode> pauseNodes = new List<PauseNode>();

	private const int MaxCollapsedRuns = 4096;

	private readonly List<CollapsedRun> collapsedRuns = new List<CollapsedRun>();

	private CollapsedRun[] pendingRuns = new CollapsedRun[64];

	private const int MinBandLanes = 1;

	private const float BandGapLanes = 0.4f;

	private int[] bandLaneStart = new int[8];

	private float[] bandTopLane = new float[8];

	private int[] bandLanes = new int[8];

	private bool[] bandCollapsed = new bool[8];

	private int[] bandVisibleDepth = new int[8];

	private float[] bandBusyMs = new float[8];

	private int[] bandThreadIds = new int[8];

	private ProfilerLabelId[] bandNameLabelIds = new ProfilerLabelId[8];

	private int[] bandStickyDepth = new int[8];

	private long[] bandShrinkDeadline = new long[8];

	private float[] bandAnimStart = new float[8];

	private float[] bandAnimLanes = new float[8];

	private int bandCount;

	private float animTotalLanes;

	private int totalIndexLanes;

	private int lastBandRegistryVersion = -1;

	private bool bandLayoutSnapPending = true;

	private long bandAnimTimestamp;

	private readonly HashSet<int> collapsedThreadIds = new HashSet<int>();

	private int anchorThreadId = -1;

	private float anchorIntraLanes;

	private float anchorViewY;

	private static readonly long BandShrinkDelayTicks = Stopwatch.Frequency * 3 / 2;

	private ZoneChunk hoverChunk;

	private int hoverNodeIndex = -1;

	private int hoverRootFrame = -1;

	private int hoverPauseFrame = -1;

	private int hoverPauseIndex = -1;

	private Vector2 lastMousePosition;

	private readonly float[] labelZoneX0 = new float[160];

	private readonly float[] labelZoneX1 = new float[160];

	private readonly int[] labelZoneDepth = new int[160];

	private readonly List<PausedGapSpan> pausedGapSpans = new List<PausedGapSpan>();

	private readonly List<FrameEntry> frameEntries = new List<FrameEntry>();

	private long layoutOriginNs = -1L;

	private readonly ProfilerPanZoomView view = new ProfilerPanZoomView(0.005);

	private static readonly (double, double, int, int) ForcedSelectionKey = (double.NaN, double.NaN, -2, -2);

	private (double rangeStart, double rangeEnd, int newestOrdinal, int oldestOrdinal) lastSelectionKey = ForcedSelectionKey;

	private bool snapView;

	private bool dragArmed;

	private bool dragSelecting;

	private float dragArmX;

	private double dragStartUs;

	private double dragEndUs;

	private bool suppressSelectionZoom;

	private readonly ProfilerSession session;

	private readonly StringBuilder durationScratch = new StringBuilder();

	private readonly Dictionary<ushort, float> labelWidths = new Dictionary<ushort, float>();

	private FrameProfiler.FrameSnapshot RetainedSnapshot;

	private int SelectedOffset = -1;

	private int SelectedOldestOffset = -1;

	private double SelectionRangeStart = -1.0;

	private double SelectionRangeEnd = -1.0;

	private int SelectedZoneOrdinal = -1;

	private readonly List<int> SelectedPathIds = new List<int>();

	public Action<ProfilerZoneSelection> ZoneSelected;

	private static readonly Color[] LightColors = BuildPalette(0.3f);

	private static readonly Color[] BaseColors = BuildPalette(0f);

	private float metaPauseMs;

	private int metaPauseCount;

	private int lastLayoutNewestOrdinal = int.MinValue;

	private int lastLayoutCount = -1;

	private bool lastLayoutUseSpikes;

	private bool lastLayoutRetained;

	private double lastLayoutViewStartUs;

	private double lastLayoutViewSpanUs;

	private readonly Dictionary<int, string> frameNames = new Dictionary<int, string>();

	private bool mouseInside;

	private string tooltipText;

	private bool hoverRectVisible;

	private Rect hoverRectBounds;

	private bool pauseBandVisible;

	private Rect pauseBandBounds;

	private static readonly Color HoverStrokeColor = ProfilerGui.Hex(2868903935u);

	private static readonly Color SelectionStrokeColor = ProfilerGui.Hex(uint.MaxValue);

	private static readonly Color PausedGapColor = ProfilerGui.Hex(822083583u);

	private bool UseSpikes => session.IsSpikes;

	private ProfilerFrameSource Source
	{
		get
		{
			if (!UseSpikes)
			{
				return ProfilerFrameSource.History;
			}
			return ProfilerFrameSource.Spikes;
		}
	}

	private int SourceCount => ProfilerFrameStore.CountOf(Source);

	private float BandGapFor(int band)
	{
		if (bandLanes[band] > 1)
		{
			return 0.4f;
		}
		return 0f;
	}

	public ProfilerFlamechartWidget(ProfilerSession session)
	{
		this.session = session;
	}

	public void NotifySourceChanged()
	{
	}

	public void HighlightZone(int ordinal, int pathId)
	{
		SelectedPathIds.Clear();
		if (pathId > 0)
		{
			SelectedPathIds.Add(pathId);
		}
		SelectedZoneOrdinal = ordinal;
	}

	public void HighlightZones(int ordinal, List<int> pathIds)
	{
		SelectedPathIds.Clear();
		SelectedPathIds.AddRange(pathIds);
		SelectedZoneOrdinal = ordinal;
	}

	public void ClearHighlight()
	{
		HighlightZone(-1, -1);
	}

	public void Clear()
	{
		ClearHighlight();
		ClearEntries();
		lastLayoutNewestOrdinal = int.MinValue;
		lastLayoutCount = -1;
	}

	public void Refresh()
	{
		bool selectionAgedOut = session.SelectionAgedOut;
		RetainedSnapshot = (selectionAgedOut ? session.Snapshot : null);
		int newestOffset;
		int oldestOffset;
		bool flag = session.TryGetSelectionBand(out newestOffset, out oldestOffset);
		SelectedOffset = ((!selectionAgedOut) ? (flag ? newestOffset : (-1)) : 0);
		SelectedOldestOffset = ((!selectionAgedOut) ? (flag ? oldestOffset : (-1)) : 0);
		SelectionRangeStart = session.SelectionStartNs;
		SelectionRangeEnd = session.SelectionEndNs;
	}

	public void Draw(Rect rect)
	{
		ProfilerGui.Fill(rect, ProfilerGui.SunkenBackground);
		GUI.BeginGroup(rect);
		try
		{
			width = rect.width;
			height = rect.height;
			HandleInput();
			if (ProfilerGui.Painting)
			{
				Paint();
			}
		}
		finally
		{
			GUI.EndGroup();
		}
	}

	private void Quad(Color color, float x0, float y0, float x1, float y1)
	{
		x0 = Mathf.Max(0f, x0);
		y0 = Mathf.Max(0f, y0);
		x1 = Mathf.Min(width, x1);
		y1 = Mathf.Min(height, y1);
		if (x1 > x0 && y1 > y0)
		{
			GuiBackend.Current.FillRect(Rect.MinMaxRect(x0, y0, x1, y1), color);
		}
	}

	private void OutlineQuad(Color color, float x0, float y0, float x1, float y1)
	{
		Quad(color, x0, y0, x1, y0 + 1f);
		Quad(color, x0, y1 - 1f, x1, y1);
		Quad(color, x0, y0 + 1f, x0 + 1f, y1 - 1f);
		Quad(color, x1 - 1f, y0 + 1f, x1, y1 - 1f);
	}

	private static void Text(string text, float x, float y, float maxWidth, Color color)
	{
		ProfilerGui.Label(new Rect(x, y, maxWidth, 18f), text, color);
	}

	private static Color[] BuildPalette(float lighten)
	{
		Color[] array = new Color[14];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = Color.Lerp(ProfilerPalette.ColorAt(i), Color.white, lighten);
		}
		return array;
	}

	private static Color BaseColorFor(ProfilerLabelId labelId)
	{
		return BaseColors[ProfilerPalette.ColorIndexFor(labelId)];
	}

	private void Paint()
	{
		float num = width;
		float num2 = height;
		bool flag = BuildFrameLayout();
		if (frameEntries.Count == 0 || num < 142f || num2 < 16f)
		{
			view.CancelAnimation();
			contentVisible = false;
			return;
		}
		contentVisible = true;
		(double, double, int, int) tuple = CurrentSelectionKey();
		int num3;
		if (!flag)
		{
			(double, double, int, int) tuple2 = tuple;
			(double, double, int, int) tuple3 = lastSelectionKey;
			num3 = ((tuple2.Item1 != tuple3.Item1 || tuple2.Item2 != tuple3.Item2 || tuple2.Item3 != tuple3.Item3 || tuple2.Item4 != tuple3.Item4) ? 1 : 0);
		}
		else
		{
			num3 = 0;
		}
		bool flag2 = (byte)num3 != 0;
		lastSelectionKey = tuple;
		double startUs;
		double endUs;
		if (flag2 && suppressSelectionZoom)
		{
			suppressSelectionZoom = false;
		}
		else if (flag2 && TryGetSelectionRange(out startUs, out endUs))
		{
			double num4 = Math.Max((endUs - startUs) * 0.35, 2000.0);
			view.TopLane = 0f;
			if (snapView)
			{
				view.SetTo(startUs - num4, endUs - startUs + num4 * 2.0);
			}
			else
			{
				view.AnimateTo(startUs - num4, endUs - startUs + num4 * 2.0);
			}
		}
		else if (!view.UserView)
		{
			FrameEntry frameEntry = frameEntries[frameEntries.Count - 1];
			view.Span = Math.Max(1000.0, frameEntry.endUs - frameEntry.startUs);
			view.Start = frameEntry.endUs - view.Span;
			view.TopLane = 0f;
		}
		if (!flag2 && !flag)
		{
			snapView = false;
		}
		view.Advance();
		view.ClampRange();
		LayoutBands();
		AdvanceBandLayout();
		ClampView();
		RebuildRuler(num);
		RebuildPausedGaps(num, num2);
		RebuildSelectionBand(num, num2);
		RebuildBandChrome(num, num2);
		RebuildPauseLane(num);
		IGuiBackend current = GuiBackend.Current;
		current.PushClipRect(new Rect(0f, PlotTop, num, Mathf.Max(0f, num2 - PlotTop)));
		RebuildZones(num, num2);
		current.PopClipRect();
		RebuildOutsideRange(num, num2);
		RebuildSelectionRects(num);
		RebuildScrollThumb(num, num2);
		current.PushClipRect(new Rect(0f, PlotTop, num, Mathf.Max(0f, num2 - PlotTop)));
		RebuildGutterLabels(num2);
		current.PopClipRect();
		RebuildGutter(num, num2);
		PaintHover();
	}

	private void RebuildGutter(float w, float h)
	{
		Quad(GutterLineColor, 126f, 0f, 127f, h);
		Quad(GutterLineColor, 126f, 17f, Mathf.Max(126f, w), 18f);
		if (FrameProfiler.RuntimePausesSupported)
		{
			Quad(GutterLineColor, 126f, PlotTop - 1f, Mathf.Max(126f, w), PlotTop);
		}
		if (FrameProfiler.RuntimePausesSupported)
		{
			ProfilerGui.Label(new Rect(8f, 17f, 24f, GcLaneHeight + 2f), "GC", LaneNameColor);
			StringBuilder scratch = ProfilerGui.Scratch;
			scratch.Clear();
			if (metaPauseCount > 0)
			{
				ProfilerTreeModel.AppendMs(scratch, metaPauseMs, "0.00");
			}
			else
			{
				scratch.Append('—');
			}
			ProfilerGui.Label(new Rect(30f, 17f, 92f, GcLaneHeight + 2f), scratch.ToString(), RulerTextColor);
		}
	}

	private void RebuildGutterLabels(float h)
	{
		metaPauseMs = 0f;
		metaPauseCount = 0;
		for (int i = 0; i < bandCount; i++)
		{
			bandBusyMs[i] = 0f;
		}
		int num = ((SelectedOffset >= 0 && SelectedOffset < frameEntries.Count) ? SelectedOffset : 0);
		int num2 = Mathf.Clamp(Mathf.Max(num, SelectedOldestOffset), num, frameEntries.Count - 1);
		for (int j = num; j <= num2; j++)
		{
			FrameProfiler.FrameSnapshot snapshot = frameEntries[frameEntries.Count - 1 - j].snapshot;
			metaPauseMs += snapshot.pauseMs;
			metaPauseCount += snapshot.pauseCount;
			bandBusyMs[0] += snapshot.totalMs;
			for (int k = 1; k < snapshot.sectionCount; k++)
			{
				int num3 = BandIndexForThread(snapshot.sections[k].threadId);
				if (num3 > 0)
				{
					bandBusyMs[num3] += (float)snapshot.sections[k].busyNanoseconds / 1000000f;
				}
			}
		}
		StringBuilder scratch = ProfilerGui.Scratch;
		for (int l = 0; l < bandCount; l++)
		{
			float num4 = BandLaneTop(l, 0);
			float num5 = num4 + bandAnimLanes[l] * 19f;
			if (num5 <= PlotTop || num4 >= h)
			{
				continue;
			}
			bool flag = bandCollapsed[l];
			bool flag2 = bandAnimLanes[l] < 1.5f;
			Color color = (flag ? RulerTextColor : LaneNameColor);
			float num6 = Mathf.Max(num4, Mathf.Min(PlotTop, num5 - (flag2 ? 16f : 31f)));
			ProfilerGui.Arrow(new Rect(4f, num6 + (flag2 ? 1f : 3f), 14f, 16f), !flag, RulerTextColor);
			scratch.Clear();
			scratch.Append(FrameProfiler.GetLabel(bandNameLabelIds[l]));
			if (flag2)
			{
				scratch.Append(" (");
				ProfilerTreeModel.AppendMs(scratch, bandBusyMs[l]);
				scratch.Append(')');
				ProfilerGui.Label(new Rect(21f, num6 + 1f, 101f, 16f), scratch.ToString(), color);
				continue;
			}
			ProfilerGui.Label(new Rect(21f, num6 + 2f, 101f, 16f), scratch.ToString(), color);
			scratch.Clear();
			ProfilerTreeModel.AppendMs(scratch, bandBusyMs[l]);
			if (l > 0)
			{
				scratch.Append(" busy");
			}
			ProfilerGui.Label(new Rect(21f, num6 + 16f, 101f, 16f), scratch.ToString(), RulerTextColor);
		}
	}

	private void RebuildBandChrome(float w, float h)
	{
		for (int i = 0; i < bandCount; i++)
		{
			float num = BandLaneTop(i, 0);
			float num2 = num + bandAnimLanes[i] * 19f;
			if (!(num2 <= PlotTop) && !(num >= h))
			{
				if ((i & 1) == 1)
				{
					Quad(BandTintColor, 0f, Mathf.Max(PlotTop, num), w, Mathf.Min(h, num2 + BandGapFor(i) * 19f));
				}
				float num3 = num2 + BandGapFor(i) * 19f - 1f;
				if (i < bandCount - 1 && num3 > PlotTop && num3 < h)
				{
					Quad(BandSeparatorColor, 0f, num3, w, num3 + 1f);
				}
			}
		}
	}

	private void RebuildScrollThumb(float w, float h)
	{
		float num = h - PlotTop;
		float num2 = animTotalLanes * 19f;
		if (num > 0f && num2 > num)
		{
			float num3 = Mathf.Max(24f, num * num / num2);
			float num4 = PlotTop + view.TopLane * 19f / num2 * num;
			Quad(ScrollThumbColor, w - 4f, Mathf.Max(PlotTop, num4), w - 1f, Mathf.Min(h, num4 + num3));
		}
	}

	private bool TryGetSourceSnapshot(int offset, out FrameProfiler.FrameSnapshot snapshot)
	{
		return ProfilerFrameStore.TryGetAt(Source, offset, out snapshot);
	}

	private bool BuildFrameLayout()
	{
		FrameProfiler.FrameSnapshot retainedSnapshot = RetainedSnapshot;
		int num = ((retainedSnapshot != null) ? 1 : SourceCount);
		FrameProfiler.FrameSnapshot snapshot;
		int num2 = retainedSnapshot?.captureOrdinal ?? (TryGetSourceSnapshot(0, out snapshot) ? snapshot.captureOrdinal : (-1));
		bool flag = UseSpikes != lastLayoutUseSpikes || retainedSnapshot != null != lastLayoutRetained;
		if (!flag && num2 == lastLayoutNewestOrdinal && num == lastLayoutCount)
		{
			return false;
		}
		newestZonesStale = true;
		if (flag)
		{
			view.UserView = false;
			view.TopLane = 0f;
			view.CancelAnimation();
			snapView = true;
		}
		bool flag2 = retainedSnapshot == null && !UseSpikes;
		long num3 = layoutOriginNs;
		int num4 = ((flag2 && !flag) ? KeepAgedEntries(num, num2 - lastLayoutNewestOrdinal) : 0);
		if (num4 == 0)
		{
			ClearEntries();
			layoutOriginNs = -1L;
		}
		double cursor = 0.0;
		if (retainedSnapshot != null)
		{
			layoutOriginNs = retainedSnapshot.beginTimeNanoseconds;
			AddFrameEntry(retainedSnapshot, ref cursor);
		}
		else if (!flag2)
		{
			for (int num5 = num - 1; num5 >= 0; num5--)
			{
				if (TryGetSourceSnapshot(num5, out var snapshot2))
				{
					if (layoutOriginNs < 0)
					{
						layoutOriginNs = snapshot2.beginTimeNanoseconds;
					}
					AddFrameEntry(snapshot2, ref cursor);
				}
			}
		}
		else if (num4 == 0 || !AppendWallEntries(num - num4, incremental: true))
		{
			ClearEntries();
			layoutOriginNs = -1L;
			AppendWallEntries(num - 1, incremental: false);
		}
		int num6;
		if (frameEntries.Count <= 0)
		{
			num6 = num2;
		}
		else
		{
			List<FrameEntry> list = frameEntries;
			num6 = list[list.Count - 1].ordinal;
		}
		lastLayoutNewestOrdinal = num6;
		lastLayoutCount = ((frameEntries.Count > 0) ? frameEntries.Count : num);
		lastLayoutUseSpikes = UseSpikes;
		lastLayoutRetained = retainedSnapshot != null;
		if (!flag && num3 >= 0 && layoutOriginNs >= 0 && layoutOriginNs != num3)
		{
			double num7 = (double)(layoutOriginNs - num3) / 1000.0;
			view.ShiftOrigin(num7);
			if (dragArmed || dragSelecting)
			{
				dragStartUs -= num7;
				dragEndUs -= num7;
			}
		}
		view.Extent = 1.0;
		for (int i = 0; i < frameEntries.Count; i++)
		{
			view.Extent = Math.Max(view.Extent, frameEntries[i].maxEndUs);
		}
		return flag;
	}

	private int KeepAgedEntries(int count, int advanced)
	{
		int num = count - advanced;
		if (advanced <= 0 || num <= 0 || num > frameEntries.Count)
		{
			return 0;
		}
		int num2 = frameEntries.Count - num;
		if (frameEntries[num2].snapshot.captureOrdinal != frameEntries[num2].ordinal)
		{
			return 0;
		}
		DropEntries(num2);
		long beginTimeNanoseconds = frameEntries[0].snapshot.beginTimeNanoseconds;
		double num3 = (double)(beginTimeNanoseconds - layoutOriginNs) / 1000.0;
		if (num3 != 0.0)
		{
			Span<FrameEntry> span = CollectionsMarshal.AsSpan(frameEntries);
			for (int i = 0; i < span.Length; i++)
			{
				ref FrameEntry reference = ref span[i];
				reference.startUs -= num3;
				reference.endUs -= num3;
				reference.boundaryUs -= num3;
				reference.maxEndUs -= num3;
			}
		}
		layoutOriginNs = beginTimeNanoseconds;
		return num;
	}

	private bool AppendWallEntries(int fromOffset, bool incremental)
	{
		double num = 0.0;
		for (int num2 = Math.Min(fromOffset, SourceCount - 1); num2 >= 0; num2--)
		{
			if (!TryGetSourceSnapshot(num2, out var snapshot))
			{
				continue;
			}
			if (incremental)
			{
				List<FrameEntry> list = frameEntries;
				int ordinal = list[list.Count - 1].ordinal;
				if (snapshot.captureOrdinal <= ordinal)
				{
					continue;
				}
				if (snapshot.captureOrdinal != ordinal + 1)
				{
					return false;
				}
			}
			if (layoutOriginNs < 0)
			{
				layoutOriginNs = snapshot.beginTimeNanoseconds;
			}
			num = (double)(snapshot.beginTimeNanoseconds - layoutOriginNs) / 1000.0;
			AddFrameEntry(snapshot, ref num);
		}
		return true;
	}

	private void ClearEntries()
	{
		DropEntries(frameEntries.Count);
	}

	private void DropEntries(int count)
	{
		for (int i = 0; i < count; i++)
		{
			frameEntries[i].snapshot.Release();
			frameEntries[i].zones.Invalidate();
			frameZonesPool.Push(frameEntries[i].zones);
		}
		frameEntries.RemoveRange(0, count);
	}

	private void AddFrameEntry(FrameProfiler.FrameSnapshot snapshot, ref double cursor)
	{
		snapshot.Retain();
		double num = Math.Max(1.0, (double)(snapshot.endTimeNanoseconds - snapshot.beginTimeNanoseconds) / 1000.0);
		double boundaryUs = cursor + Math.Max(num, (double)snapshot.nodesEndNanoseconds / 1000.0);
		double num2 = cursor + num;
		for (int i = 0; i < snapshot.sectionCount; i++)
		{
			num2 = Math.Max(num2, cursor + (double)snapshot.sections[i].endNanoseconds / 1000.0);
		}
		for (int j = 0; j < snapshot.pauseCount; j++)
		{
			num2 = Math.Max(num2, cursor + (double)snapshot.pauses[j].endMicroseconds);
		}
		frameEntries.Add(new FrameEntry
		{
			snapshot = snapshot,
			ordinal = snapshot.captureOrdinal,
			startUs = cursor,
			endUs = cursor + num,
			boundaryUs = boundaryUs,
			maxEndUs = num2,
			zones = ((frameZonesPool.Count > 0) ? frameZonesPool.Pop() : new FrameZones())
		});
		cursor += num;
	}

	private long EntryTimeNs(in FrameEntry entry, double frameUs)
	{
		return entry.snapshot.beginTimeNanoseconds + (long)((frameUs - entry.startUs) * 1000.0);
	}

	private (double rangeStart, double rangeEnd, int newestOrdinal, int oldestOrdinal) CurrentSelectionKey()
	{
		int item = -1;
		int item2 = -1;
		int count = frameEntries.Count;
		int selectedOffset = SelectedOffset;
		int num = Mathf.Max(selectedOffset, SelectedOldestOffset);
		if (selectedOffset >= 0 && selectedOffset < count && num < count)
		{
			item = frameEntries[count - 1 - selectedOffset].snapshot.captureOrdinal;
			item2 = frameEntries[count - 1 - num].snapshot.captureOrdinal;
		}
		return (rangeStart: SelectionRangeStart, rangeEnd: SelectionRangeEnd, newestOrdinal: item, oldestOrdinal: item2);
	}

	private bool TryGetSelectionRange(out double startUs, out double endUs)
	{
		startUs = 0.0;
		endUs = 0.0;
		if (dragSelecting)
		{
			startUs = Math.Min(dragStartUs, dragEndUs);
			endUs = Math.Max(dragStartUs, dragEndUs);
			return endUs > startUs;
		}
		if (SelectionRangeStart >= 0.0 && SelectionRangeEnd > SelectionRangeStart && layoutOriginNs >= 0)
		{
			startUs = (SelectionRangeStart - (double)layoutOriginNs) / 1000.0;
			endUs = (SelectionRangeEnd - (double)layoutOriginNs) / 1000.0;
			return true;
		}
		int count = frameEntries.Count;
		int selectedOffset = SelectedOffset;
		int num = Mathf.Max(selectedOffset, SelectedOldestOffset);
		if (selectedOffset < 0 || selectedOffset >= count || num >= count)
		{
			return false;
		}
		startUs = frameEntries[count - 1 - num].startUs;
		endUs = frameEntries[count - 1 - selectedOffset].endUs;
		return endUs > startUs;
	}

	private static float PlotWidth(float w)
	{
		return w - 126f;
	}

	private float TimeToX(double timeUs, float w)
	{
		return view.ToX(timeUs, 126f, PlotWidth(w));
	}

	private double XToTime(float x, float w)
	{
		return view.ToValue(x, 126f, PlotWidth(w));
	}

	private int BandIndexForThread(int threadId)
	{
		for (int i = 0; i < bandCount; i++)
		{
			if (bandThreadIds[i] == threadId)
			{
				return i;
			}
		}
		return 0;
	}

	private int SectionIndexForBand(in FrameEntry entry, int band)
	{
		FrameProfiler.FrameSnapshot snapshot = entry.snapshot;
		if (band < snapshot.sectionCount && snapshot.sections[band].threadId == bandThreadIds[band])
		{
			return band;
		}
		for (int i = 0; i < snapshot.sectionCount; i++)
		{
			if (snapshot.sections[i].threadId == bandThreadIds[band])
			{
				return i;
			}
		}
		return -1;
	}

	private void FrameRangeFor(in FrameEntry entry, int band, out long startNs, out long endNs, out double startUs, out double endUs)
	{
		startUs = entry.startUs;
		endUs = ((band == 0) ? entry.endUs : entry.boundaryUs);
		startNs = entry.snapshot.beginTimeNanoseconds;
		endNs = EntryTimeNs(in entry, endUs);
	}

	private void CollectZones(in FrameEntry entry, int band, double minVisibleUs, List<ZoneDraw> into)
	{
		into.Clear();
		FrameRangeFor(in entry, band, out var startNs, out var endNs, out var startUs, out var _);
		FrameProfiler.CollectChunks(entry.snapshot, bandThreadIds[band], startNs, endNs, chunkScratch);
		foreach (ZoneChunk item in chunkScratch)
		{
			ZoneNode[] nodes = item.nodes;
			int num = Math.Min(item.Count, nodes.Length);
			ancestorEnds.Clear();
			int num2 = 0;
			while (num2 < num)
			{
				while (ancestorEnds.Count > 0)
				{
					int num3 = num2;
					List<int> list = ancestorEnds;
					if (num3 < list[list.Count - 1])
					{
						break;
					}
					ancestorEnds.RemoveAt(ancestorEnds.Count - 1);
				}
				ref ZoneNode reference = ref nodes[num2];
				int num4;
				int num5;
				if (!reference.IsOpen)
				{
					num4 = ((reference.subtreeEnd > num2) ? 1 : 0);
					if (num4 != 0)
					{
						num5 = reference.subtreeEnd;
						goto IL_00e8;
					}
				}
				else
				{
					num4 = 0;
				}
				num5 = num;
				goto IL_00e8;
				IL_00e8:
				int num6 = num5;
				long num7 = item.baseNs + reference.StartNanoseconds;
				long num8 = ((num4 != 0) ? (item.baseNs + reference.EndNanoseconds) : endNs);
				if (num7 >= endNs || num8 <= startNs)
				{
					num2 = num6;
					continue;
				}
				int depth = ancestorEnds.Count + 1;
				double num9 = startUs + (double)(Math.Max(num7, startNs) - startNs) / 1000.0;
				double num10 = startUs + (double)(Math.Min(num8, endNs) - startNs) / 1000.0;
				bool flag = num10 - num9 < minVisibleUs;
				into.Add(new ZoneDraw
				{
					chunk = item,
					index = num2,
					depth = depth,
					startUs = num9,
					endUs = Math.Max(num9, num10),
					folded = flag
				});
				if (flag)
				{
					num2 = num6;
					continue;
				}
				ancestorEnds.Add(num6);
				num2++;
			}
		}
	}

	private List<ZoneDraw> CollectZonesCached(in FrameEntry entry, int frameIndex, int band, double minVisibleUs)
	{
		FrameZones zones = entry.zones;
		if (zones.perBand.Length <= band)
		{
			int num = zones.perBand.Length;
			int num2 = Mathf.NextPowerOfTwo(band + 1);
			Array.Resize(ref zones.perBand, num2);
			Array.Resize(ref zones.minVisibleUs, num2);
			Array.Fill(zones.minVisibleUs, double.NaN, num, num2 - num);
		}
		List<ZoneDraw>[] perBand = zones.perBand;
		List<ZoneDraw> list = perBand[band] ?? (perBand[band] = new List<ZoneDraw>());
		if (zones.minVisibleUs[band] != minVisibleUs)
		{
			CollectZones(in entry, band, minVisibleUs, list);
			zones.minVisibleUs[band] = minVisibleUs;
		}
		return list;
	}

	private double MinVisibleUs(float w)
	{
		double a = view.Span * 3.0 / (double)Mathf.Max(1f, PlotWidth(w));
		return Math.Pow(2.0, Math.Floor(Math.Log(a, 2.0) * 4.0) / 4.0);
	}

	private bool FrameDetailed(in FrameEntry entry, float w)
	{
		return (entry.endUs - entry.startUs) / view.Span * (double)PlotWidth(w) >= 6.0;
	}

	private void LayoutBands()
	{
		bool flag = lastBandRegistryVersion != ProfilerThreadRegistry.Version;
		if (flag)
		{
			for (int i = 0; i < frameEntries.Count; i++)
			{
				frameEntries[i].zones.Invalidate();
			}
		}
		else if (newestZonesStale)
		{
			for (int j = Mathf.Max(0, frameEntries.Count - 2); j < frameEntries.Count; j++)
			{
				frameEntries[j].zones.Invalidate();
			}
		}
		newestZonesStale = false;
		IReadOnlyList<ProfilerThreadRegistry.Entry> entries = ProfilerThreadRegistry.Entries;
		bandCount = Mathf.Max(1, entries.Count);
		if (bandLaneStart.Length < bandCount)
		{
			int num = Mathf.NextPowerOfTwo(bandCount);
			bandLaneStart = new int[num];
			bandTopLane = new float[num];
			bandLanes = new int[num];
			bandCollapsed = new bool[num];
			bandVisibleDepth = new int[num];
			bandBusyMs = new float[num];
			bandThreadIds = new int[num];
			bandNameLabelIds = new ProfilerLabelId[num];
			bandStickyDepth = new int[num];
			bandShrinkDeadline = new long[num];
			bandAnimStart = new float[num];
			bandAnimLanes = new float[num];
		}
		if (flag)
		{
			lastBandRegistryVersion = ProfilerThreadRegistry.Version;
			Array.Clear(bandStickyDepth, 0, bandStickyDepth.Length);
			Array.Clear(bandShrinkDeadline, 0, bandShrinkDeadline.Length);
			bandLayoutSnapPending = true;
		}
		for (int k = 0; k < bandCount; k++)
		{
			bandVisibleDepth[k] = 0;
			if (entries.Count > 0)
			{
				bandThreadIds[k] = entries[k].threadId;
				bandNameLabelIds[k] = entries[k].nameLabelId;
			}
		}
		if (entries.Count == 0 && frameEntries.Count > 0)
		{
			List<FrameEntry> list = frameEntries;
			FrameProfiler.FrameSnapshot snapshot = list[list.Count - 1].snapshot;
			bandThreadIds[0] = ((snapshot.sectionCount > 0) ? snapshot.sections[0].threadId : 0);
			bandNameLabelIds[0] = ((snapshot.sectionCount > 0) ? snapshot.sections[0].nameLabelId : ProfilerLabelId.None);
		}
		float w = width;
		Mathf.Max(1f, PlotWidth(w));
		double minVisibleUs = MinVisibleUs(w);
		double num2 = view.Start + view.Span;
		for (int l = 0; l < frameEntries.Count; l++)
		{
			FrameEntry entry = frameEntries[l];
			if (entry.maxEndUs <= view.Start || entry.startUs >= num2 || !FrameDetailed(in entry, w))
			{
				continue;
			}
			for (int m = 0; m < bandCount; m++)
			{
				if (bandCollapsed[m] || SectionIndexForBand(in entry, m) < 0)
				{
					continue;
				}
				List<ZoneDraw> list2 = CollectZonesCached(in entry, l, m, minVisibleUs);
				for (int n = 0; n < list2.Count; n++)
				{
					ZoneDraw zoneDraw = list2[n];
					if (!zoneDraw.folded && zoneDraw.depth > bandVisibleDepth[m] && !(zoneDraw.endUs <= view.Start) && !(zoneDraw.startUs >= num2))
					{
						bandVisibleDepth[m] = zoneDraw.depth;
					}
				}
			}
		}
		bool flag2 = Math.Abs(view.Start - lastLayoutViewStartUs) > view.Span * 0.001 || Math.Abs(view.Span - lastLayoutViewSpanUs) > view.Span * 0.001;
		lastLayoutViewStartUs = view.Start;
		lastLayoutViewSpanUs = view.Span;
		bool flag3 = view.UserView && flag2;
		long timestamp = Stopwatch.GetTimestamp();
		int num3 = 0;
		float num4 = 0f;
		for (int num5 = 0; num5 < bandCount; num5++)
		{
			if (flag3 || bandVisibleDepth[num5] >= bandStickyDepth[num5])
			{
				bandStickyDepth[num5] = bandVisibleDepth[num5];
				bandShrinkDeadline[num5] = 0L;
			}
			else if (bandShrinkDeadline[num5] == 0L)
			{
				bandShrinkDeadline[num5] = timestamp + BandShrinkDelayTicks;
			}
			else if (timestamp > bandShrinkDeadline[num5])
			{
				bandStickyDepth[num5] = bandVisibleDepth[num5];
				bandShrinkDeadline[num5] = 0L;
			}
			bandLaneStart[num5] = num3;
			bandTopLane[num5] = num4;
			bandCollapsed[num5] = collapsedThreadIds.Contains(bandThreadIds[num5]);
			bandLanes[num5] = (bandCollapsed[num5] ? 1 : Mathf.Max(1, bandStickyDepth[num5] + 1));
			num3 += bandLanes[num5];
			num4 += (float)bandLanes[num5] + BandGapFor(num5);
		}
		totalIndexLanes = num3;
	}

	private void ToggleBandCollapsed(int band)
	{
		if (!collapsedThreadIds.Add(bandThreadIds[band]))
		{
			collapsedThreadIds.Remove(bandThreadIds[band]);
		}
		anchorThreadId = -1;
	}

	private void AdvanceBandLayout()
	{
		long timestamp = Stopwatch.GetTimestamp();
		float num = Mathf.Clamp((float)((double)(timestamp - bandAnimTimestamp) / (double)Stopwatch.Frequency), 0f, 0.1f);
		bandAnimTimestamp = timestamp;
		float num2 = (bandLayoutSnapPending ? 1f : (1f - Mathf.Exp((0f - num) * 18f)));
		bandLayoutSnapPending = false;
		bool flag = false;
		for (int i = 0; i < bandCount; i++)
		{
			bandAnimStart[i] += (bandTopLane[i] - bandAnimStart[i]) * num2;
			bandAnimLanes[i] += ((float)bandLanes[i] - bandAnimLanes[i]) * num2;
			if (Mathf.Abs(bandAnimStart[i] - bandTopLane[i]) > 0.01f || Mathf.Abs(bandAnimLanes[i] - (float)bandLanes[i]) > 0.01f)
			{
				flag = true;
				continue;
			}
			bandAnimStart[i] = bandTopLane[i];
			bandAnimLanes[i] = bandLanes[i];
		}
		view.Animation.SetContentAnimating(flag);
		int num3 = bandCount - 1;
		animTotalLanes = bandAnimStart[num3] + bandAnimLanes[num3] + BandGapFor(num3);
		if (anchorThreadId < 0)
		{
			return;
		}
		for (int j = 0; j < bandCount; j++)
		{
			if (bandThreadIds[j] == anchorThreadId)
			{
				view.TopLane = bandAnimStart[j] + anchorIntraLanes - (anchorViewY - PlotTop) / 19f;
				break;
			}
		}
		if (!flag && !view.Animation.Active)
		{
			anchorThreadId = -1;
		}
	}

	private bool TryFindBand(float lane, out int bandIndex, out float intraLanes)
	{
		for (int i = 0; i < bandCount; i++)
		{
			float num = lane - bandAnimStart[i];
			if (num >= 0f && num < bandAnimLanes[i] + BandGapFor(i))
			{
				bandIndex = i;
				intraLanes = num;
				return true;
			}
		}
		bandIndex = 0;
		intraLanes = 0f;
		return false;
	}

	private bool TryFindBandForLane(float lane, out int bandIndex, out int intraDepth)
	{
		if (TryFindBand(lane, out bandIndex, out var intraLanes) && !bandCollapsed[bandIndex] && intraLanes < Mathf.Min(bandLanes[bandIndex], bandAnimLanes[bandIndex]))
		{
			intraDepth = Mathf.FloorToInt(intraLanes);
			return true;
		}
		intraDepth = 0;
		return false;
	}

	private float BandLaneTop(int band, int intraDepth)
	{
		return PlotTop + (bandAnimStart[band] + (float)intraDepth - view.TopLane) * 19f;
	}

	private void RebuildZones(float w, float h)
	{
		pauseNodes.Clear();
		Mathf.Max(0, Mathf.FloorToInt(view.TopLane));
		int num = Mathf.Max(totalIndexLanes, Mathf.CeilToInt(view.TopLane + (h - PlotTop) / 19f));
		if (laneLabelEnds.Length <= num || pendingRuns.Length < laneLabelEnds.Length)
		{
			int num2 = Mathf.NextPowerOfTwo(Mathf.Max(num + 1, laneLabelEnds.Length));
			laneLabelEnds = new float[num2];
			pendingRuns = new CollapsedRun[num2];
		}
		collapsedRuns.Clear();
		for (int i = 0; i < laneLabelEnds.Length; i++)
		{
			laneLabelEnds[i] = float.MinValue;
			pendingRuns[i].count = 0;
		}
		int labelCount = 0;
		double num3 = view.Start + view.Span;
		double minVisibleUs = MinVisibleUs(w);
		for (int j = 0; j < frameEntries.Count; j++)
		{
			FrameEntry entry = frameEntries[j];
			if (entry.maxEndUs <= view.Start || entry.startUs >= num3)
			{
				continue;
			}
			float num4 = TimeToX(entry.startUs, w);
			float num5 = TimeToX(entry.endUs, w);
			if (num4 >= 126f && num4 <= w && num5 - num4 >= 28f)
			{
				Quad(FrameSeparatorColor, num4, 18f, num4 + 1f, Mathf.Max(18f, h));
			}
			bool flag = FrameDetailed(in entry, w);
			for (int k = 0; k < bandCount; k++)
			{
				if (bandCollapsed[k])
				{
					continue;
				}
				float num6 = BandLaneTop(k, 0);
				bool flag2 = num6 + 19f > PlotTop && num6 < h;
				if (!flag && !flag2)
				{
					continue;
				}
				int num7 = SectionIndexForBand(in entry, k);
				if (num7 < 0)
				{
					continue;
				}
				ref FrameProfiler.ThreadSection reference = ref entry.snapshot.sections[num7];
				double num8 = ((k == 0) ? entry.startUs : (entry.startUs + (double)reference.startNanoseconds / 1000.0));
				double num9 = ((k == 0) ? entry.endUs : (entry.startUs + (double)reference.endNanoseconds / 1000.0));
				bool flag3 = flag2 && num9 > view.Start && num8 < num3;
				ProfilerLabelId labelId = ((k == 0) ? FrameProfiler.RootLabelId : reference.nameLabelId);
				Color color = BaseColorFor(labelId);
				if (!flag)
				{
					if (flag3)
					{
						AccumulateCollapsed(bandLaneStart[k], num6, Mathf.Max(126f, TimeToX(num8, w)), Mathf.Min(w, TimeToX(num9, w)), (k == 0) ? (entry.snapshot.totalMs * 1000f) : ((float)reference.busyNanoseconds * 0.001f), color, j, k, null, -1);
					}
					continue;
				}
				if (flag3)
				{
					float x = Mathf.Max(126f, TimeToX(num8, w));
					float x2 = Mathf.Min(w, TimeToX(num9, w));
					string name = ((k == 0) ? FrameName(entry.snapshot.frameIndex) : FrameProfiler.GetLabel(reference.nameLabelId));
					float durationUs = ((k == 0) ? (entry.snapshot.totalMs * 1000f) : ((float)reference.busyNanoseconds * 0.001f));
					DrawZoneQuad(ProfilerPalette.ColorIndexFor(labelId), x, x2, num6, h, bandLaneStart[k], ref labelCount, name, (k == 0) ? ProfilerLabelId.None : reference.nameLabelId, durationUs);
				}
				List<ZoneDraw> list = CollectZonesCached(in entry, j, k, minVisibleUs);
				for (int l = 0; l < list.Count; l++)
				{
					ZoneDraw zoneDraw = list[l];
					if (zoneDraw.depth >= bandLanes[k] || (float)zoneDraw.depth >= bandAnimLanes[k])
					{
						continue;
					}
					int lane = bandLaneStart[k] + zoneDraw.depth;
					float num10 = BandLaneTop(k, zoneDraw.depth);
					if (!(num10 + 19f <= PlotTop) && !(num10 >= h) && !(zoneDraw.endUs <= view.Start) && !(zoneDraw.startUs >= num3))
					{
						float x3 = Mathf.Max(126f, TimeToX(zoneDraw.startUs, w));
						float x4 = Mathf.Min(w, TimeToX(zoneDraw.endUs, w));
						ProfilerLabelId profilerLabelId = FrameProfiler.PathLabel(zoneDraw.chunk.nodes[zoneDraw.index].pathId);
						if (zoneDraw.folded)
						{
							AccumulateCollapsed(lane, num10, x3, x4, (float)(zoneDraw.endUs - zoneDraw.startUs), BaseColorFor(profilerLabelId), j, k, zoneDraw.chunk, zoneDraw.index);
						}
						else
						{
							DrawZoneQuad(ProfilerPalette.ColorIndexFor(profilerLabelId), x3, x4, num10, h, lane, ref labelCount, FrameProfiler.GetLabel(profilerLabelId), profilerLabelId, ZoneDurationUs(zoneDraw.chunk, zoneDraw.index, in entry, k));
						}
					}
				}
				if (k == 0)
				{
					DrawPauseNodes(in entry, j, list, w, h, num3);
				}
			}
		}
		for (int m = 0; m < pendingRuns.Length; m++)
		{
			FlushCollapsed(m);
		}
	}

	private void DrawZoneQuad(int colorIndex, float x0, float x1, float laneY, float h, int lane, ref int labelCount, string name, ProfilerLabelId nameId, float durationUs)
	{
		float num = Mathf.Max(PlotTop, laneY);
		float num2 = Mathf.Min(h, laneY + 19f - 1f);
		if (x1 - x0 > 4f && num2 - num > 3f)
		{
			Quad(LightColors[colorIndex], x0 + 0.5f, num, x1 - 0.5f, num2);
			Quad(BaseColors[colorIndex], x0 + 1.5f, num + 1f, x1 - 1.5f, num2 - 1f);
		}
		else
		{
			Quad(BaseColors[colorIndex], x0, num, x1, num2);
		}
		if (x1 - x0 < 40f || labelCount >= 160 || (uint)lane >= (uint)laneLabelEnds.Length || (x0 < laneLabelEnds[lane] - 0.5f && OverlapsDrawnLabel(labelCount, lane, x0, x1)))
		{
			return;
		}
		laneLabelEnds[lane] = Mathf.Max(laneLabelEnds[lane], x1);
		labelZoneX0[labelCount] = x0;
		labelZoneX1[labelCount] = x1;
		labelZoneDepth[labelCount] = lane;
		labelCount++;
		float num3 = x0 + 3f;
		float num4 = x1 - x0 - 6f;
		float num5 = NameWidth(name, nameId);
		if (num5 + 4f + 30f <= num4)
		{
			durationScratch.Clear();
			ProfilerTreeModel.AppendMs(durationScratch, durationUs / 1000f, "0.0", "");
			string text = durationScratch.ToString();
			if (num5 + 4f + ProfilerGui.Width(text) <= num4)
			{
				Text(name, num3, laneY, num5 + 4f, ProfilerPalette.ZoneLabelColor);
				Text(text, num3 + num5 + 4f, laneY, num4 - num5 - 4f + 4f, ProfilerPalette.ZoneLabelColor);
				return;
			}
		}
		Text(name, num3, laneY, num4, ProfilerPalette.ZoneLabelColor);
	}

	private float NameWidth(string name, ProfilerLabelId nameId)
	{
		if (!nameId.IsValid)
		{
			return ProfilerGui.Width(name);
		}
		if (!labelWidths.TryGetValue(nameId.Value, out var value))
		{
			value = (labelWidths[nameId.Value] = ProfilerGui.Width(name));
		}
		return value;
	}

	private string FrameName(int frameIndex)
	{
		if (frameNames.TryGetValue(frameIndex, out var value))
		{
			return value;
		}
		if (frameNames.Count > 512)
		{
			frameNames.Clear();
		}
		return frameNames[frameIndex] = "Frame " + frameIndex;
	}

	private float ZoneDurationUs(ZoneChunk chunk, int index, in FrameEntry entry, int band)
	{
		ref ZoneNode reference = ref chunk.nodes[index];
		if (!reference.IsOpen)
		{
			return (float)((double)(reference.EndNanoseconds - reference.StartNanoseconds) / 1000.0);
		}
		FrameRangeFor(in entry, band, out var _, out var endNs, out var _, out var _);
		return (float)Math.Max(0.0, (double)(endNs - (chunk.baseNs + reference.StartNanoseconds)) / 1000.0);
	}

	private static float ZoneSelfUs(ZoneChunk chunk, int index)
	{
		ZoneNode[] nodes = chunk.nodes;
		ref ZoneNode reference = ref nodes[index];
		long num = reference.EndNanoseconds - reference.StartNanoseconds;
		int num2 = Math.Min(reference.subtreeEnd, Math.Min(chunk.Count, nodes.Length));
		int num3 = index + 1;
		while (num3 < num2)
		{
			ref ZoneNode reference2 = ref nodes[num3];
			if (reference2.IsOpen || reference2.subtreeEnd <= num3)
			{
				break;
			}
			num -= reference2.EndNanoseconds - reference2.StartNanoseconds;
			num3 = reference2.subtreeEnd;
		}
		return (float)((double)Math.Max(0L, num) / 1000.0);
	}

	private void RebuildPauseLane(float w)
	{
		double num = view.Start + view.Span;
		for (int i = 0; i < frameEntries.Count; i++)
		{
			FrameEntry entry = frameEntries[i];
			if (!(entry.maxEndUs <= view.Start) && !(entry.startUs >= num))
			{
				DrawPauseLane(in entry, w, num);
			}
		}
	}

	private void DrawPauseLane(in FrameEntry entry, float w, double viewEndUs)
	{
		for (int i = 0; i < entry.snapshot.pauseCount; i++)
		{
			FrameProfiler.PauseSpan pauseSpan = entry.snapshot.pauses[i];
			double num = entry.startUs + (double)pauseSpan.startMicroseconds;
			double num2 = entry.startUs + (double)pauseSpan.endMicroseconds;
			if (!(num2 <= view.Start) && !(num >= viewEndUs))
			{
				float num3 = Mathf.Max(126f, TimeToX(num, w));
				float num4 = Mathf.Min(w, Mathf.Max(num3 + 1f, TimeToX(num2, w)));
				Color color = (IsGcReason(pauseSpan.reason) ? GcPauseColor : OtherPauseColor);
				float num5 = Mathf.Clamp(TimeToX(entry.startUs + (double)pauseSpan.suspendedMicroseconds, w), num3, num4);
				if (num5 - num3 >= 1f && num4 - num5 >= 1f)
				{
					Quad(PauseWaitColor, num3, 20f, num5, PlotTop - 2f);
					Quad(color, num5, 20f, num4, PlotTop - 2f);
				}
				else
				{
					Quad(color, num3, 20f, num4, PlotTop - 2f);
				}
			}
		}
	}

	private void DrawPauseNodes(in FrameEntry entry, int frameIndex, List<ZoneDraw> zones, float w, float h, double viewEndUs)
	{
		FrameProfiler.FrameSnapshot snapshot = entry.snapshot;
		for (int i = 0; i < snapshot.pauseCount; i++)
		{
			FrameProfiler.PauseSpan pauseSpan = snapshot.pauses[i];
			double num = entry.startUs + (double)pauseSpan.suspendedMicroseconds;
			double num2 = entry.startUs + (double)pauseSpan.endMicroseconds;
			if (num2 <= num || num2 <= view.Start || num >= viewEndUs)
			{
				continue;
			}
			int num3 = 1;
			for (int j = 0; j < zones.Count; j++)
			{
				ZoneDraw zoneDraw = zones[j];
				if (!zoneDraw.folded && zoneDraw.startUs <= num && zoneDraw.endUs >= num2 && zoneDraw.depth + 1 > num3)
				{
					num3 = zoneDraw.depth + 1;
				}
			}
			if (num3 >= bandLanes[0] || (float)num3 >= bandAnimLanes[0])
			{
				continue;
			}
			float num4 = BandLaneTop(0, num3);
			if (!(num4 + 19f <= PlotTop) && !(num4 >= h))
			{
				float num5 = Mathf.Max(126f, TimeToX(num, w));
				float num6 = Mathf.Min(w, Mathf.Max(num5 + 1f, TimeToX(num2, w)));
				ProfilerLabelId pauseLabelId = FrameProfiler.GetPauseLabelId(pauseSpan.reason, pauseSpan.condemnedGeneration);
				int labelCount = 160;
				if (num6 - num5 >= 40f)
				{
					labelCount = 0;
				}
				DrawZoneQuad(ProfilerPalette.ColorIndexFor(pauseLabelId), num5, num6, num4, h, bandLaneStart[0] + num3, ref labelCount, FrameProfiler.GetLabel(pauseLabelId), pauseLabelId, (float)(num2 - num));
				pauseNodes.Add(new PauseNode
				{
					frameIndex = frameIndex,
					pauseIndex = i,
					x0 = num5,
					x1 = num6,
					laneY = num4
				});
			}
		}
	}

	private void AccumulateCollapsed(int lane, float laneY, float x0, float x1, float totalMicroseconds, Color color, int frameIndex, int band, ZoneChunk chunk, int nodeIndex)
	{
		if ((uint)lane < (uint)pendingRuns.Length)
		{
			ref CollapsedRun reference = ref pendingRuns[lane];
			if (reference.count > 0 && x0 - reference.x1 > 3f)
			{
				FlushCollapsed(lane);
			}
			if (reference.count == 0)
			{
				reference.x0 = x0;
				reference.x1 = float.MinValue;
				reference.color = color;
				reference.frameIndex = frameIndex;
				reference.band = band;
				reference.chunk = chunk;
				reference.nodeIndex = nodeIndex;
			}
			reference.laneY = laneY;
			reference.x1 = Mathf.Max(reference.x1, Mathf.Max(x1, x0 + 3f));
			reference.count++;
			reference.totalMicroseconds += totalMicroseconds;
		}
	}

	private void FlushCollapsed(int lane)
	{
		ref CollapsedRun reference = ref pendingRuns[lane];
		if (reference.count == 0)
		{
			return;
		}
		float num = Mathf.Max(PlotTop, reference.laneY);
		float num2 = Mathf.Min(height, reference.laneY + 19f - 1f);
		if (num2 > num)
		{
			Quad(reference.color, reference.x0, num, reference.x1, num2);
			float num3 = reference.laneY + 9f;
			float num4 = Mathf.Max(PlotTop, num3 - 2f);
			float num5 = Mathf.Min(height, num3 + 2f);
			if (num5 > num4)
			{
				Quad(CollapsedRunColor, reference.x0, num4, reference.x1, num5);
			}
			if (collapsedRuns.Count < 4096)
			{
				collapsedRuns.Add(reference);
			}
		}
		reference.count = 0;
		reference.totalMicroseconds = 0.0;
		reference.x1 = float.MinValue;
		reference.chunk = null;
	}

	private bool OverlapsDrawnLabel(int drawnCount, int depth, float x0, float x1)
	{
		for (int i = 0; i < drawnCount; i++)
		{
			if (labelZoneDepth[i] == depth && x0 < labelZoneX1[i] - 0.5f && x1 > labelZoneX0[i] + 0.5f)
			{
				return true;
			}
		}
		return false;
	}

	private static bool IsGcReason(RuntimePauseReason reason)
	{
		if (reason == RuntimePauseReason.GC || reason == RuntimePauseReason.GCPrep)
		{
			return true;
		}
		return false;
	}

	private void RebuildRuler(float w)
	{
		float num = NiceStep((float)(view.Span / 1000.0) / 8f);
		int num2 = 0;
		float num3 = 0f;
		while (num2 < 24)
		{
			double num4 = view.Start + (double)num3 * 1000.0;
			if (!(num4 >= view.Start + view.Span))
			{
				float num5 = TimeToX(num4, w);
				Quad(RulerTickColor, num5, 0f, num5 + 1f, height);
				ProfilerGui.Scratch.Clear();
				AppendRulerTime(ProfilerGui.Scratch, num3, num);
				ProfilerGui.Label(new Rect(num5 + 3f, 0f, 90f, 18f), ProfilerGui.Scratch.ToString(), RulerTextColor);
				num2++;
				num3 += num;
				continue;
			}
			break;
		}
	}

	private static void AppendRulerTime(StringBuilder builder, float relMs, float stepMs)
	{
		float num;
		string value;
		string text;
		if (stepMs >= 1f)
		{
			num = relMs;
			value = "ms";
			text = "0";
		}
		else if (stepMs >= 0.001f)
		{
			num = relMs * 1000f;
			value = "µs";
			text = "0";
		}
		else
		{
			num = relMs * 1000000f;
			value = "ns";
			text = ((stepMs * 1000000f >= 1f) ? "0" : "0.##");
		}
		builder.Append(num.ToString(text)).Append(' ').Append(value);
	}

	private static float NiceStep(float raw)
	{
		if (raw <= 0f)
		{
			return 1E-09f;
		}
		float num = Mathf.Pow(10f, Mathf.Floor(MathF.Log10(raw)));
		float num2 = raw / num;
		return ((num2 < 1.5f) ? 1f : ((num2 < 3.5f) ? 2f : ((num2 < 7.5f) ? 5f : 10f))) * num;
	}

	private void RebuildPausedGaps(float w, float h)
	{
		pausedGapSpans.Clear();
		if (UseSpikes || lastLayoutRetained || layoutOriginNs < 0)
		{
			return;
		}
		double num = view.Start + view.Span;
		for (int i = 1; i < frameEntries.Count; i++)
		{
			double maxEndUs = frameEntries[i - 1].maxEndUs;
			double startUs = frameEntries[i].startUs;
			if (!(startUs <= maxEndUs) && !(startUs <= view.Start) && !(maxEndUs >= num) && OverlapsCaptureGap(maxEndUs, startUs))
			{
				float num2 = Mathf.Max(126f, TimeToX(maxEndUs, w));
				float num3 = Mathf.Min(w, TimeToX(startUs, w));
				if (!(num3 - num2 < 2f))
				{
					Quad(PausedGapColor, num2, 18f, num3, h);
					pausedGapSpans.Add(new PausedGapSpan
					{
						x0 = num2,
						x1 = num3,
						durationUs = startUs - maxEndUs
					});
				}
			}
		}
	}

	private bool OverlapsCaptureGap(double startUs, double endUs)
	{
		for (int i = 0; i < FrameProfiler.CaptureGapCount; i++)
		{
			FrameProfiler.CaptureGap captureGap = FrameProfiler.GetCaptureGap(i);
			double num = (double)(captureGap.beginTimeNanoseconds - layoutOriginNs) / 1000.0;
			double num2 = (double)(captureGap.endTimeNanoseconds - layoutOriginNs) / 1000.0;
			if (num < endUs && num2 > startUs)
			{
				return true;
			}
		}
		return false;
	}

	private void RebuildOutsideRange(float w, float h)
	{
		float num = TimeToX(0.0, w);
		float num2 = TimeToX(view.Extent, w);
		if (num > 126f)
		{
			Quad(OutsideRangeColor, 126f, 18f, Mathf.Min(w, num), h);
		}
		if (num2 < w)
		{
			Quad(OutsideRangeColor, Mathf.Max(126f, num2), 18f, w, h);
		}
	}

	private void RebuildSelectionBand(float w, float h)
	{
		if (!TryGetSelectionRange(out var startUs, out var endUs))
		{
			return;
		}
		float num = Mathf.Max(126f, TimeToX(startUs, w));
		float num2 = Mathf.Min(w, TimeToX(endUs, w));
		if (num2 <= num)
		{
			return;
		}
		Quad(SelectionBandColor, num, 0f, num2, PlotTop);
		int bandIndex = -1;
		if (dragSelecting && lastMousePosition.y >= PlotTop && !TryFindBand((lastMousePosition.y - PlotTop) / 19f + view.TopLane, out bandIndex, out var _))
		{
			bandIndex = -1;
		}
		bool flag = ProfilerThreadRegistry.Entries.Count == 0;
		for (int i = 0; i < bandCount; i++)
		{
			if (dragSelecting && bandIndex >= 0)
			{
				if (i != bandIndex)
				{
					continue;
				}
			}
			else if (!flag && !ProfilerThreadRegistry.IsSelected(bandThreadIds[i]))
			{
				continue;
			}
			float num3 = BandLaneTop(i, 0);
			float num4 = num3 + (bandAnimLanes[i] + BandGapFor(i)) * 19f;
			if (!(num4 <= PlotTop) && !(num3 >= h))
			{
				Quad(SelectionBandColor, num, Mathf.Max(PlotTop, num3), num2, Mathf.Min(h, num4));
			}
		}
	}

	private void RebuildSelectionRects(float w)
	{
		int selectedZoneOrdinal = SelectedZoneOrdinal;
		List<int> selectedPathIds = SelectedPathIds;
		if (selectedZoneOrdinal < 0 || selectedPathIds == null || selectedPathIds.Count == 0 || !TryGetFrameEntryIndexByOrdinal(selectedZoneOrdinal, out var frameIndex))
		{
			return;
		}
		FrameEntry entry = frameEntries[frameIndex];
		if (!FrameDetailed(in entry, w))
		{
			return;
		}
		ProfilerPathTable.Entry[] entries = ProfilerPathTable.Entries;
		int count = ProfilerPathTable.Count;
		double minVisibleUs = MinVisibleUs(w);
		for (int i = 0; i < bandCount; i++)
		{
			if (bandCollapsed[i] || SectionIndexForBand(in entry, i) < 0)
			{
				continue;
			}
			List<ZoneDraw> list = CollectZonesCached(in entry, frameIndex, i, minVisibleUs);
			for (int j = 0; j < list.Count; j++)
			{
				ZoneDraw zoneDraw = list[j];
				if (zoneDraw.folded || zoneDraw.depth >= bandLanes[i])
				{
					continue;
				}
				int pathId = zoneDraw.chunk.nodes[zoneDraw.index].pathId;
				int item = (((uint)pathId < (uint)count) ? entries[pathId].foldedPathId : pathId);
				if (!selectedPathIds.Contains(pathId) && !selectedPathIds.Contains(item))
				{
					continue;
				}
				float num = Mathf.Max(126f, TimeToX(zoneDraw.startUs, w));
				float num2 = Mathf.Min(w, TimeToX(zoneDraw.endUs, w));
				if (!(num2 - num < 1f))
				{
					num2 = num + Mathf.Max(2f, num2 - num);
					float num3 = BandLaneTop(i, zoneDraw.depth);
					float num4 = num3 + 19f - 1f;
					if (!(num4 <= PlotTop) && !(num3 >= height))
					{
						num3 = Mathf.Max(PlotTop, num3);
						num4 = Mathf.Min(height, num4);
						OutlineQuad(SelectionStrokeColor, num, num3, num2, num4);
					}
				}
			}
		}
	}

	private bool TryGetFrameEntryIndexByOrdinal(int ordinal, out int frameIndex)
	{
		for (int num = frameEntries.Count - 1; num >= 0; num--)
		{
			if (frameEntries[num].snapshot.captureOrdinal == ordinal)
			{
				frameIndex = num;
				return true;
			}
		}
		frameIndex = -1;
		return false;
	}

	private bool TryHitTest(Vector2 position, out ZoneHit hit)
	{
		hit = default(ZoneHit);
		float num = width;
		if (num <= 126f || frameEntries.Count == 0)
		{
			return false;
		}
		if (position.y < PlotTop || position.x < 126f)
		{
			return false;
		}
		double num2 = XToTime(position.x, num);
		float lane = (position.y - PlotTop) / 19f + view.TopLane;
		if (!TryFindBandForLane(lane, out var bandIndex, out var intraDepth))
		{
			return false;
		}
		int num3 = -1;
		for (int num4 = frameEntries.Count - 1; num4 >= 0; num4--)
		{
			FrameEntry frameEntry = frameEntries[num4];
			double num5 = ((bandIndex == 0) ? frameEntry.endUs : frameEntry.boundaryUs);
			if (num2 >= frameEntry.startUs && num2 < num5)
			{
				num3 = num4;
				break;
			}
		}
		if (num3 >= 0)
		{
			return TryHitTestFrame(num3, num2, bandIndex, intraDepth, num, out hit);
		}
		return false;
	}

	private bool TryHitTestCollapsedRun(Vector2 position, out CollapsedRun hit)
	{
		for (int i = 0; i < collapsedRuns.Count; i++)
		{
			CollapsedRun collapsedRun = collapsedRuns[i];
			if (position.x >= collapsedRun.x0 - 1f && position.x <= collapsedRun.x1 + 1f && position.y >= collapsedRun.laneY && position.y <= collapsedRun.laneY + 19f)
			{
				hit = collapsedRun;
				return true;
			}
		}
		hit = default(CollapsedRun);
		return false;
	}

	private bool TryHitTestFrame(int frameIndex, double timeUs, int bandIndex, int intraDepth, float w, out ZoneHit hit)
	{
		hit = default(ZoneHit);
		FrameEntry entry = frameEntries[frameIndex];
		if (!FrameDetailed(in entry, w))
		{
			return false;
		}
		int num = SectionIndexForBand(in entry, bandIndex);
		if (num < 0)
		{
			return false;
		}
		FrameProfiler.ThreadSection threadSection = entry.snapshot.sections[num];
		double num2 = ((bandIndex == 0) ? entry.startUs : (entry.startUs + (double)threadSection.startNanoseconds / 1000.0));
		double num3 = ((bandIndex == 0) ? entry.endUs : (entry.startUs + (double)threadSection.endNanoseconds / 1000.0));
		if (intraDepth == 0)
		{
			if (timeUs < num2 || timeUs > num3)
			{
				return false;
			}
			hit = new ZoneHit
			{
				frameIndex = frameIndex,
				band = bandIndex,
				nodeIndex = -1,
				startUs = num2,
				endUs = num3
			};
			return true;
		}
		double minVisibleUs = MinVisibleUs(w);
		List<ZoneDraw> list = CollectZonesCached(in entry, frameIndex, bandIndex, minVisibleUs);
		for (int i = 0; i < list.Count; i++)
		{
			ZoneDraw zoneDraw = list[i];
			if (!zoneDraw.folded && zoneDraw.depth == intraDepth && !(timeUs < zoneDraw.startUs) && !(timeUs > zoneDraw.endUs))
			{
				hit = new ZoneHit
				{
					frameIndex = frameIndex,
					band = bandIndex,
					chunk = zoneDraw.chunk,
					nodeIndex = zoneDraw.index,
					depth = zoneDraw.depth,
					startUs = zoneDraw.startUs,
					endUs = zoneDraw.endUs
				};
				return true;
			}
		}
		return false;
	}

	private void HandleInput()
	{
		Event current = Event.current;
		Vector2 mousePosition = current.mousePosition;
		bool flag = mousePosition.x >= 0f && mousePosition.y >= 0f && mousePosition.x < width && mousePosition.y < height;
		if (current.type == EventType.mouseDown && flag)
		{
			lastMousePosition = mousePosition;
			if (current.button == 0)
			{
				if (current.clickCount < 2 && layoutOriginNs >= 0 && mousePosition.x >= 126f && width > 126f && contentVisible)
				{
					dragArmed = true;
					dragArmX = mousePosition.x;
					dragStartUs = XToTime(mousePosition.x, width);
					dragEndUs = dragStartUs;
				}
				else if (contentVisible)
				{
					HandleLeftClick(mousePosition, current.clickCount);
				}
			}
			else if (dragArmed || dragSelecting)
			{
				dragArmed = false;
				dragSelecting = false;
			}
			else
			{
				view.BeginPan(mousePosition);
			}
			current.Use();
		}
		else if (current.type == EventType.scrollWheel && flag)
		{
			lastMousePosition = mousePosition;
			current.Use();
			if (!(width <= 126f) && contentVisible && view.HandleWheel(current.delta, mousePosition.x, 126f, PlotWidth(width)) && mousePosition.y >= PlotTop && TryFindBand((mousePosition.y - PlotTop) / 19f + view.TopLane, out var bandIndex, out var intraLanes))
			{
				anchorThreadId = bandThreadIds[bandIndex];
				anchorIntraLanes = intraLanes;
				anchorViewY = mousePosition.y;
			}
		}
		else
		{
			if (current.type != EventType.repaint)
			{
				return;
			}
			lastMousePosition = mousePosition;
			mouseInside = flag;
			if (dragArmed || dragSelecting)
			{
				if (!Input.GetMouseButton(0))
				{
					bool flag2 = dragSelecting;
					dragArmed = false;
					dragSelecting = false;
					if (contentVisible)
					{
						if (flag2)
						{
							CommitRangeSelection();
						}
						else
						{
							HandleLeftClick(mousePosition, 1);
						}
					}
				}
				else
				{
					if (!dragSelecting && Mathf.Abs(mousePosition.x - dragArmX) > 4f)
					{
						dragSelecting = true;
					}
					if (dragSelecting)
					{
						dragEndUs = XToTime(Mathf.Max(126f, mousePosition.x), width);
					}
				}
			}
			else if (view.Panning)
			{
				if (!Input.GetMouseButton(1) && !Input.GetMouseButton(2))
				{
					view.EndPan();
				}
				else if (width > 126f && view.TryPan(mousePosition, PlotWidth(width), 19f))
				{
					anchorThreadId = -1;
					ClampView();
				}
			}
		}
	}

	private void PaintHover()
	{
		if (dragSelecting)
		{
			ClearHover();
			StringBuilder scratch = ProfilerGui.Scratch;
			scratch.Clear();
			ProfilerTreeModel.AppendMs(scratch, (float)(Math.Abs(dragEndUs - dragStartUs) / 1000.0), "0.00");
			ProfilerGui.SetTooltip(scratch.ToString());
			return;
		}
		if (!mouseInside || view.Panning || dragArmed)
		{
			HideTooltip();
			return;
		}
		UpdateTooltip(lastMousePosition);
		if (pauseBandVisible)
		{
			Quad(PauseBandColor, pauseBandBounds.xMin, pauseBandBounds.yMin, pauseBandBounds.xMax, pauseBandBounds.yMax);
		}
		if (hoverRectVisible)
		{
			OutlineQuad(HoverStrokeColor, hoverRectBounds.xMin, hoverRectBounds.yMin, hoverRectBounds.xMax, hoverRectBounds.yMax);
		}
		if (tooltipText != null)
		{
			ProfilerGui.SetTooltip(tooltipText);
		}
	}

	private void HandleLeftClick(Vector2 position, int clickCount)
	{
		if (position.x < 126f && position.y >= PlotTop)
		{
			float lane = (position.y - PlotTop) / 19f + view.TopLane;
			if (TryFindBand(lane, out var bandIndex, out var _))
			{
				ToggleBandCollapsed(bandIndex);
			}
			return;
		}
		ZoneHit hit;
		bool flag = TryHitTest(position, out hit);
		CollapsedRun hit2 = default(CollapsedRun);
		bool flag2 = !flag && TryHitTestCollapsedRun(position, out hit2);
		if (flag2 && hit2.count == 1 && hit2.chunk != null && (uint)hit2.frameIndex < (uint)frameEntries.Count)
		{
			hit = HitFromRun(in hit2);
			flag = true;
			flag2 = false;
		}
		if (clickCount >= 2)
		{
			if (flag)
			{
				ZoomToZone(in hit);
			}
			else if (flag2)
			{
				ZoomToRun(in hit2, position);
			}
			else
			{
				ResetView();
			}
		}
		else if (flag)
		{
			suppressSelectionZoom = true;
			FrameEntry entry = frameEntries[hit.frameIndex];
			int pathId = (hit.IsRoot ? RootPathIdFor(in entry, hit.band) : hit.chunk.nodes[hit.nodeIndex].pathId);
			ZoneSelected?.Invoke(new ProfilerZoneSelection(entry.ordinal, bandThreadIds[hit.band], pathId));
		}
	}

	private static int RootPathIdFor(in FrameEntry entry, int band)
	{
		FrameProfiler.FrameSnapshot snapshot = entry.snapshot;
		for (int i = 0; i < snapshot.sectionCount; i++)
		{
			FrameProfiler.ThreadSection threadSection = snapshot.sections[i];
			if (threadSection.pathEntryCount > 0 && ((band == 0) ? (i == 0) : (i > 0)))
			{
				return snapshot.pathEntries[threadSection.firstPathEntry].pathId;
			}
		}
		return -1;
	}

	private ZoneHit HitFromRun(in CollapsedRun run)
	{
		FrameEntry entry = frameEntries[run.frameIndex];
		int band = run.band;
		FrameRangeFor(in entry, band, out var startNs, out var endNs, out var startUs, out var _);
		ref ZoneNode reference = ref run.chunk.nodes[run.nodeIndex];
		long val = run.chunk.baseNs + reference.StartNanoseconds;
		long val2 = (reference.IsOpen ? endNs : (run.chunk.baseNs + reference.EndNanoseconds));
		ZoneHit result = default(ZoneHit);
		result.frameIndex = run.frameIndex;
		result.band = band;
		result.chunk = run.chunk;
		result.nodeIndex = run.nodeIndex;
		result.startUs = startUs + (double)(Math.Max(val, startNs) - startNs) / 1000.0;
		result.endUs = startUs + (double)(Math.Min(val2, endNs) - startNs) / 1000.0;
		return result;
	}

	private void ZoomToRun(in CollapsedRun run, Vector2 position)
	{
		float w = width;
		double num = XToTime(run.x0, w);
		double num2 = Math.Max(XToTime(run.x1, w) - num, view.MinSpan);
		if (num2 > view.Span * 0.5)
		{
			double num3 = XToTime(Mathf.Max(126f, position.x), w);
			num2 = view.Span * 0.5;
			num = num3 - (num3 - view.Start) * 0.5;
		}
		suppressSelectionZoom = true;
		view.AnimateTo(num, num2);
	}

	private void ZoomToZone(in ZoneHit hit)
	{
		suppressSelectionZoom = true;
		view.AnimateTo(hit.startUs, Math.Max(view.MinSpan, hit.endUs - hit.startUs));
	}

	private void CommitRangeSelection()
	{
		double num = Math.Min(dragStartUs, dragEndUs);
		double num2 = Math.Max(dragStartUs, dragEndUs);
		if (num2 <= num || layoutOriginNs < 0)
		{
			return;
		}
		int num3 = -1;
		int num4 = -1;
		for (int i = 0; i < frameEntries.Count; i++)
		{
			FrameEntry frameEntry = frameEntries[i];
			if (!(((i + 1 < frameEntries.Count) ? frameEntries[i + 1].startUs : Math.Max(frameEntry.endUs, frameEntry.maxEndUs)) <= num) && !(frameEntry.startUs >= num2))
			{
				if (num4 < 0)
				{
					num4 = i;
				}
				num3 = i;
			}
		}
		if (num3 >= 0)
		{
			float lane = (lastMousePosition.y - PlotTop) / 19f + view.TopLane;
			if (lastMousePosition.y >= PlotTop && TryFindBand(lane, out var bandIndex, out var _))
			{
				ProfilerThreadRegistry.SelectOnly(bandThreadIds[bandIndex]);
			}
			int count = frameEntries.Count;
			long num5 = layoutOriginNs + (long)(num * 1000.0);
			long endNs = Math.Max(layoutOriginNs + (long)(num2 * 1000.0), num5 + 1);
			suppressSelectionZoom = true;
			session.SelectTimeRange(count - 1 - num3, count - 1 - num4, num5, endNs);
		}
	}

	private void ResetView()
	{
		view.TopLane = 0f;
		anchorThreadId = -1;
		lastSelectionKey = ForcedSelectionKey;
		if (!TryGetSelectionRange(out var _, out var _))
		{
			if (frameEntries.Count == 0)
			{
				view.UserView = false;
				return;
			}
			List<FrameEntry> list = frameEntries;
			FrameEntry frameEntry = list[list.Count - 1];
			double num = Math.Max(1000.0, frameEntry.endUs - frameEntry.startUs);
			view.AnimateTo(frameEntry.endUs - num, num, releaseUserViewOnFinish: true);
		}
	}

	private void ClampView()
	{
		view.Clamp(Mathf.Max(1f, (height - PlotTop) / 19f), animTotalLanes);
	}

	private void UpdateTooltip(Vector2 position)
	{
		int frameIndex2;
		int pauseIndex2;
		CollapsedRun hit;
		ZoneHit hit2;
		if (!contentVisible)
		{
			HideTooltip();
		}
		else if (position.y >= 18f && position.y < PlotTop && position.x >= 126f)
		{
			if (!TryHitTestPause(position, out var frameIndex, out var pauseIndex))
			{
				HideTooltip();
			}
			else
			{
				ShowPauseTooltip(position, frameIndex, pauseIndex);
			}
		}
		else if (TryHitTestPauseNode(position, out frameIndex2, out pauseIndex2))
		{
			ShowPauseTooltip(position, frameIndex2, pauseIndex2);
		}
		else if (TryHitTestCollapsedRun(position, out hit))
		{
			if (hit.count == 1 && hit.chunk != null && (uint)hit.frameIndex < (uint)frameEntries.Count)
			{
				ShowZoneTooltip(position, HitFromRun(in hit));
			}
			else
			{
				ShowCollapsedTooltip(position, in hit);
			}
		}
		else if (!TryHitTest(position, out hit2))
		{
			if (TryHitTestPausedGap(position, out var durationUs))
			{
				ShowPausedGapTooltip(position, durationUs);
			}
			else
			{
				HideTooltip();
			}
		}
		else
		{
			ShowZoneTooltip(position, in hit2);
		}
	}

	private bool TryHitTestPausedGap(Vector2 position, out double durationUs)
	{
		durationUs = 0.0;
		if (position.y < PlotTop)
		{
			return false;
		}
		for (int i = 0; i < pausedGapSpans.Count; i++)
		{
			PausedGapSpan pausedGapSpan = pausedGapSpans[i];
			if (position.x >= pausedGapSpan.x0 && position.x <= pausedGapSpan.x1)
			{
				durationUs = pausedGapSpan.durationUs;
				return true;
			}
		}
		return false;
	}

	private void ShowPausedGapTooltip(Vector2 position, double durationUs)
	{
		ClearHover();
		StringBuilder scratch = ProfilerGui.Scratch;
		scratch.Clear();
		scratch.Append("Profiling paused\nno frames captured for ");
		ProfilerTreeModel.AppendMs(scratch, (float)(durationUs / 1000.0));
		tooltipText = scratch.ToString();
	}

	private void ShowZoneTooltip(Vector2 position, in ZoneHit hit)
	{
		bool num = hit.IsRoot && hoverRootFrame == frameEntries[hit.frameIndex].ordinal && hoverChunk == null && hoverNodeIndex == hit.band;
		bool flag = !hit.IsRoot && hit.chunk == hoverChunk && hit.nodeIndex == hoverNodeIndex;
		if ((num || flag) && hoverPauseIndex < 0)
		{
			UpdateHoverRect(in hit);
			return;
		}
		hoverChunk = hit.chunk;
		hoverNodeIndex = (hit.IsRoot ? hit.band : hit.nodeIndex);
		hoverRootFrame = (hit.IsRoot ? frameEntries[hit.frameIndex].ordinal : (-1));
		hoverPauseFrame = -1;
		hoverPauseIndex = -1;
		pauseBandVisible = false;
		UpdateHoverRect(in hit);
		FrameEntry entry = frameEntries[hit.frameIndex];
		StringBuilder scratch = ProfilerGui.Scratch;
		scratch.Clear();
		if (hit.IsRoot)
		{
			FrameProfiler.ThreadSection threadSection = entry.snapshot.sections[SectionIndexForBand(in entry, hit.band)];
			if (hit.band == 0)
			{
				scratch.Append($"Frame {entry.snapshot.frameIndex}");
				scratch.Append("\ntotal ");
				ProfilerTreeModel.AppendMs(scratch, entry.snapshot.totalMs, "0.000");
			}
			else
			{
				scratch.Append(FrameProfiler.GetLabel(threadSection.nameLabelId));
				scratch.Append("\nbusy ");
				ProfilerTreeModel.AppendMs(scratch, (float)threadSection.busyNanoseconds / 1000000f, "0.000");
			}
			FrameProfiler.PathEntry pathEntry = entry.snapshot.pathEntries[threadSection.firstPathEntry];
			if (pathEntry.selfMicroseconds > 0f)
			{
				scratch.Append(" / unprofiled ");
				ProfilerTreeModel.AppendMs(scratch, pathEntry.selfMicroseconds / 1000f, "0.000");
			}
			if (pathEntry.allocBytes > 0)
			{
				scratch.Append("\nalloc ");
				ProfilerTreeModel.AppendAllocBytes(scratch, pathEntry.allocBytes);
			}
		}
		else
		{
			ref ZoneNode reference = ref hit.chunk.nodes[hit.nodeIndex];
			scratch.Append(FrameProfiler.GetLabel(FrameProfiler.PathLabel(reference.pathId)));
			scratch.Append("\ntotal ");
			ProfilerTreeModel.AppendMs(scratch, ZoneDurationUs(hit.chunk, hit.nodeIndex, in entry, hit.band) / 1000f, "0.000");
			if (reference.IsOpen)
			{
				scratch.Append(" / still running");
			}
			else
			{
				scratch.Append(" / self ");
				ProfilerTreeModel.AppendMs(scratch, ZoneSelfUs(hit.chunk, hit.nodeIndex) / 1000f, "0.000");
			}
			FrameProfiler.NodeAllocStat[] allocStats = hit.chunk.allocStats;
			if (allocStats != null && hit.nodeIndex < allocStats.Length)
			{
				FrameProfiler.NodeAllocStat nodeAllocStat = allocStats[hit.nodeIndex];
				int num2 = Math.Max(nodeAllocStat.totalBytes, nodeAllocStat.selfBytes);
				if (num2 > 0)
				{
					scratch.Append("\nalloc ");
					ProfilerTreeModel.AppendAllocBytes(scratch, num2);
					if (nodeAllocStat.count > 0)
					{
						scratch.Append($" ({nodeAllocStat.count:n0} objs)");
					}
				}
			}
		}
		tooltipText = scratch.ToString();
	}

	private bool TryHitTestPause(Vector2 position, out int frameIndex, out int pauseIndex)
	{
		frameIndex = -1;
		pauseIndex = -1;
		float num = width;
		if (num <= 126f || frameEntries.Count == 0)
		{
			return false;
		}
		double num2 = view.Start + view.Span;
		for (int i = 0; i < frameEntries.Count; i++)
		{
			FrameEntry frameEntry = frameEntries[i];
			if (frameEntry.endUs <= view.Start || frameEntry.startUs >= num2)
			{
				continue;
			}
			FrameProfiler.FrameSnapshot snapshot = frameEntry.snapshot;
			for (int j = 0; j < snapshot.pauseCount; j++)
			{
				FrameProfiler.PauseSpan pauseSpan = snapshot.pauses[j];
				float num3 = TimeToX(frameEntry.startUs + (double)pauseSpan.startMicroseconds, num);
				float num4 = Mathf.Max(num3 + 1f, TimeToX(frameEntry.startUs + (double)pauseSpan.endMicroseconds, num));
				if (position.x >= num3 - 1f && position.x <= num4 + 1f)
				{
					frameIndex = i;
					pauseIndex = j;
					return true;
				}
			}
		}
		return false;
	}

	private bool TryHitTestPauseNode(Vector2 position, out int frameIndex, out int pauseIndex)
	{
		for (int i = 0; i < pauseNodes.Count; i++)
		{
			PauseNode pauseNode = pauseNodes[i];
			if (position.x >= pauseNode.x0 - 1f && position.x <= pauseNode.x1 + 1f && position.y >= pauseNode.laneY && position.y <= pauseNode.laneY + 19f)
			{
				frameIndex = pauseNode.frameIndex;
				pauseIndex = pauseNode.pauseIndex;
				return true;
			}
		}
		frameIndex = -1;
		pauseIndex = -1;
		return false;
	}

	private void ShowPauseTooltip(Vector2 position, int frameIndex, int pauseIndex)
	{
		FrameEntry frameEntry = frameEntries[frameIndex];
		hoverChunk = null;
		hoverNodeIndex = -1;
		hoverRootFrame = -1;
		hoverPauseFrame = frameIndex;
		hoverPauseIndex = pauseIndex;
		FrameProfiler.PauseSpan pauseSpan = frameEntry.snapshot.pauses[pauseIndex];
		float num = width;
		float num2 = Mathf.Max(126f, TimeToX(frameEntry.startUs + (double)pauseSpan.startMicroseconds, num));
		float num3 = Mathf.Min(num, Mathf.Max(num2 + 1f, TimeToX(frameEntry.startUs + (double)pauseSpan.endMicroseconds, num)));
		hoverRectBounds = Rect.MinMaxRect(num2, 20f, num3, 18f + GcLaneHeight - 2f);
		hoverRectVisible = true;
		float xmin = Mathf.Clamp(TimeToX(frameEntry.startUs + (double)pauseSpan.suspendedMicroseconds, num), num2, num3);
		pauseBandBounds = Rect.MinMaxRect(xmin, PlotTop, num3, Mathf.Max(PlotTop, height));
		pauseBandVisible = true;
		StringBuilder scratch = ProfilerGui.Scratch;
		scratch.Clear();
		scratch.Append(FrameProfiler.DescribePause(pauseSpan.reason, pauseSpan.condemnedGeneration));
		scratch.Append("\nstopped ");
		ProfilerTreeModel.AppendMs(scratch, (float)(pauseSpan.endMicroseconds - pauseSpan.suspendedMicroseconds) / 1000f, "0.000");
		scratch.Append(" / waiting for threads ");
		ProfilerTreeModel.AppendMs(scratch, (float)(pauseSpan.suspendedMicroseconds - pauseSpan.startMicroseconds) / 1000f, "0.000");
		if (pauseSpan.collectedBytes > 0 || pauseSpan.promotedBytes > 0)
		{
			scratch.Append("\ncollected ");
			ProfilerTreeModel.AppendAllocBytes(scratch, Math.Max(0L, pauseSpan.collectedBytes));
			scratch.Append(" / promoted ");
			ProfilerTreeModel.AppendAllocBytes(scratch, pauseSpan.promotedBytes);
		}
		scratch.Append("\ntriggered by ");
		if (pauseSpan.triggeringThreadId == 0)
		{
			scratch.Append("a runtime thread");
		}
		else if (pauseSpan.triggeringThreadId == Environment.CurrentManagedThreadId)
		{
			scratch.Append("the main thread");
		}
		else
		{
			scratch.Append($"thread {pauseSpan.triggeringThreadId}");
		}
		tooltipText = scratch.ToString();
	}

	private void ShowCollapsedTooltip(Vector2 position, in CollapsedRun run)
	{
		ClearHover();
		StringBuilder scratch = ProfilerGui.Scratch;
		scratch.Clear();
		scratch.Append($"Zones too small to display: {run.count:n0}");
		scratch.Append("\ntotal ");
		ProfilerTreeModel.AppendMs(scratch, (float)(run.totalMicroseconds / 1000.0), "0.000");
		tooltipText = scratch.ToString();
	}

	private void ClearHover()
	{
		hoverChunk = null;
		hoverNodeIndex = -1;
		hoverRootFrame = -1;
		hoverPauseFrame = -1;
		hoverPauseIndex = -1;
		hoverRectVisible = false;
		pauseBandVisible = false;
	}

	private void HideTooltip()
	{
		ClearHover();
		tooltipText = null;
	}

	private void UpdateHoverRect(in ZoneHit hit)
	{
		float num = width;
		float num2 = Mathf.Max(126f, TimeToX(hit.startUs, num));
		float num3 = Mathf.Min(num, TimeToX(hit.endUs, num));
		float num4 = BandLaneTop(hit.band, hit.depth);
		float num5 = Mathf.Max(PlotTop, num4);
		float num6 = Mathf.Min(height, num4 + 19f - 1f);
		if (num3 - num2 < 1f || num6 <= num5)
		{
			hoverRectVisible = false;
			return;
		}
		hoverRectBounds = Rect.MinMaxRect(num2, num5, num3, num6);
		hoverRectVisible = true;
	}
}
