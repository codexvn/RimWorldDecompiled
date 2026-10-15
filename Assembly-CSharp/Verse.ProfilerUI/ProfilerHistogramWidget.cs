using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Waylith.Profiling;

namespace Verse.ProfilerUI;

public sealed class ProfilerHistogramWidget
{
	public const float Height = 104f;

	public const float ScaleMs = 66.666664f;

	private static readonly int[] GuideFps = new int[7] { 120, 60, 30, 20, 15, 10, 5 };

	private static readonly long[] AllocGuideBytes = new long[5] { 1024L, 8192L, 65536L, 524288L, 4194304L };

	private static readonly Color GuideColor = ProfilerGui.Hex(4281020476u);

	private static readonly Color BudgetGuideColor = ProfilerGui.Hex(4285217330u);

	private static readonly Color SelectionColor = ProfilerGui.Hex(2352631667u);

	private static readonly Color SelectionStrokeColor = ProfilerGui.Hex(4284381608u);

	private static readonly Color FadedSelectionStrokeColor = ProfilerGui.Hex(1717467560u);

	private static readonly Color HoverColor = ProfilerGui.Hex(318767103u);

	private static readonly Color OutsideCapacityColor = ProfilerGui.Hex(1140850688u);

	private static readonly Color PausedGapColor = ProfilerGui.Hex(822083583u);

	private static readonly Color[] BucketColors = new Color[5]
	{
		ProfilerGui.Hex(4284267883u),
		ProfilerGui.Hex(4292265034u),
		ProfilerGui.Hex(4292905288u),
		ProfilerGui.Hex(4292893770u),
		ProfilerGui.Hex(4293306446u)
	};

	private const float AllocLog2Floor = 7f;

	private const float AllocLog2Range = 18f;

	private const float MinZoom = 0.25f;

	private const float PanPerNotch = 0.15f;

	private const float MaxBarPitch = 32f;

	private const float GutterWidth = 92f;

	private const int AllocBucket = 4;

	private const float GcRowHeight = 9f;

	private const float UnambiguousHoverPitch = 5f;

	private const float ScrollUnitsPerNotch = 3f;

	private readonly ProfilerSession session;

	private readonly ProfilerBarRing bars = new ProfilerBarRing();

	private readonly ProfilerBarRing gcMarks = new ProfilerBarRing();

	private readonly ProfilerBarRing pauseMarks = new ProfilerBarRing();

	private readonly List<string> spikeLabels = new List<string>();

	private readonly ProfilerViewAnimation viewAnimation = new ProfilerViewAnimation();

	private long lastPushedEndNs = -1L;

	private int lastVersion = -1;

	private int lastOrdinal = -1;

	private int lastCaptureCount = -1;

	private int lastSpikeCount = -1;

	private int lastPushedCaptureCount = -1;

	private int lastSlotCapacity = -1;

	private float zoom = 1f;

	private float rightOffset;

	private float viewLeft;

	private float width;

	private float gcRowTop;

	private bool selecting;

	private bool panning;

	private float panLastX;

	private int hoverOffset = -1;

	private int lastSeenSelectedOrdinal = -1;

	private static readonly string[] guideLabels = new string[GuideFps.Length];

	private int lastTooltipFrameIndex = int.MinValue;

	private string lastTooltip;

	public bool AllocMode { get; private set; }

	private int SlotCapacity
	{
		get
		{
			if (!session.IsSpikes)
			{
				return FrameProfiler.HistoryCapacity;
			}
			return 24;
		}
	}

	private int SelectedOffset
	{
		get
		{
			if (session.IsSpikes || !session.FollowingNewest)
			{
				return session.DisplayOffset;
			}
			return -1;
		}
	}

	private int SelectedOldestOffset
	{
		get
		{
			if (session.IsSpikes || !session.FollowingNewest)
			{
				return session.DisplayOffsetOldest;
			}
			return -1;
		}
	}

	public ProfilerHistogramWidget(ProfilerSession session)
	{
		this.session = session;
	}

	public static float NormalizeAlloc(long bytes)
	{
		if (bytes > 0)
		{
			return Mathf.Max(0.0001f, Mathf.Clamp01((MathF.Log2(bytes) - 7f) / 18f));
		}
		return 0f;
	}

	public void SetAllocMode(bool value)
	{
		if (AllocMode != value)
		{
			AllocMode = value;
			Clear();
		}
	}

	public void NotifySourceChanged()
	{
		lastPushedCaptureCount = -1;
	}

	public void Clear()
	{
		bars.Clear();
		gcMarks.Clear();
		pauseMarks.Clear();
		spikeLabels.Clear();
		lastPushedEndNs = -1L;
		lastPushedCaptureCount = -1;
		lastVersion = -1;
	}

	public void Refresh()
	{
		if (lastSlotCapacity != SlotCapacity)
		{
			lastSlotCapacity = SlotCapacity;
			viewAnimation.Cancel();
			zoom = 1f;
			rightOffset = 0f;
		}
		if (lastVersion != session.Version || lastOrdinal != session.Ordinal || lastCaptureCount != FrameProfiler.CompletedFrameCount || lastSpikeCount != FrameProfiler.SpikeCount)
		{
			lastVersion = session.Version;
			lastOrdinal = session.Ordinal;
			lastCaptureCount = FrameProfiler.CompletedFrameCount;
			lastSpikeCount = FrameProfiler.SpikeCount;
			int capacity = Mathf.Max(1, SlotCapacity);
			bars.EnsureCapacity(capacity);
			gcMarks.EnsureCapacity(capacity);
			pauseMarks.EnsureCapacity(capacity);
			if (session.IsSpikes)
			{
				RefreshSpikeBars();
			}
			else
			{
				RefreshHistoryBars();
			}
		}
	}

	private void RefreshSpikeBars()
	{
		bars.Clear();
		gcMarks.Clear();
		pauseMarks.Clear();
		spikeLabels.Clear();
		lastPushedEndNs = -1L;
		lastPushedCaptureCount = -1;
		int spikeCount = FrameProfiler.SpikeCount;
		for (int i = 0; i < spikeCount; i++)
		{
			spikeLabels.Add(FrameProfiler.TryGetSpikeSnapshot(i, out FrameProfiler.FrameSnapshot snapshot) ? ("#" + snapshot.frameIndex) : "");
		}
		for (int num = spikeCount - 1; num >= 0; num--)
		{
			PushBar(FrameProfiler.TryGetSpikeSnapshot(num, out FrameProfiler.FrameSnapshot snapshot2) ? snapshot2 : null);
		}
	}

	private void RefreshHistoryBars()
	{
		spikeLabels.Clear();
		int completedFrameCount = FrameProfiler.CompletedFrameCount;
		int historyCount = FrameProfiler.HistoryCount;
		int num = completedFrameCount - lastPushedCaptureCount;
		int num2;
		int num3;
		if (lastPushedCaptureCount >= 0 && num >= 0 && num <= historyCount)
		{
			num2 = ((Mathf.Min(SlotCapacity, bars.Count + num) == historyCount) ? 1 : 0);
			if (num2 != 0)
			{
				num3 = num - 1;
				goto IL_005a;
			}
		}
		else
		{
			num2 = 0;
		}
		num3 = historyCount - 1;
		goto IL_005a;
		IL_005a:
		int num4 = num3;
		if (num2 == 0)
		{
			bars.Clear();
			gcMarks.Clear();
			pauseMarks.Clear();
			lastPushedEndNs = -1L;
		}
		for (int num5 = num4; num5 >= 0; num5--)
		{
			PushBar(FrameProfiler.TryGetHistorySnapshot(num5, out FrameProfiler.FrameSnapshot snapshot) ? snapshot : null);
		}
		lastPushedCaptureCount = completedFrameCount;
	}

	private void PushBar(FrameProfiler.FrameSnapshot snapshot)
	{
		if (snapshot == null)
		{
			bars.Push(0f);
			gcMarks.Push(0f);
			pauseMarks.Push(0f);
		}
		else
		{
			bars.Push(AllocMode ? NormalizeAlloc(snapshot.allocatedBytes) : Mathf.Clamp01(snapshot.totalMs / 66.666664f));
			gcMarks.Push(GcMarkValue(snapshot));
			pauseMarks.Push(session.IsSpikes ? 0f : PauseGapMsBefore(snapshot));
			lastPushedEndNs = snapshot.endTimeNanoseconds;
		}
	}

	private float PauseGapMsBefore(FrameProfiler.FrameSnapshot snapshot)
	{
		if (lastPushedEndNs < 0)
		{
			return 0f;
		}
		for (int i = 0; i < FrameProfiler.CaptureGapCount; i++)
		{
			FrameProfiler.CaptureGap captureGap = FrameProfiler.GetCaptureGap(i);
			if (captureGap.beginTimeNanoseconds < snapshot.beginTimeNanoseconds && captureGap.endTimeNanoseconds > lastPushedEndNs)
			{
				return (float)(captureGap.endTimeNanoseconds - captureGap.beginTimeNanoseconds) / 1000000f;
			}
		}
		return 0f;
	}

	private static float GcMarkValue(FrameProfiler.FrameSnapshot snapshot)
	{
		if (snapshot.gen0Collections + snapshot.gen1Collections + snapshot.gen2Collections == 0)
		{
			return 0f;
		}
		return Mathf.Max(snapshot.pauseMs, 0.0001f);
	}

	public void Draw(Rect rect)
	{
		ProfilerGui.Fill(rect, ProfilerGui.SunkenBackground);
		GUI.BeginGroup(rect);
		try
		{
			width = rect.width;
			int count = bars.Count;
			if (!(width < 8f) && count != 0)
			{
				HandleInput(new Rect(0f, 0f, rect.width, rect.height), count);
				if (ProfilerGui.Painting)
				{
					Paint(rect.width, rect.height, count);
				}
			}
		}
		finally
		{
			GUI.EndGroup();
		}
	}

	private void HandleInput(Rect local, int n)
	{
		Event current = Event.current;
		Vector2 mousePosition = current.mousePosition;
		bool flag = local.Contains(mousePosition);
		if (current.type == EventType.mouseDown && flag)
		{
			if (current.button == 0)
			{
				if (current.clickCount >= 2)
				{
					session.ClearSelection();
				}
				else
				{
					selecting = true;
					int newestOffset;
					if (hoverOffset >= 0)
					{
						session.BeginSelection(hoverOffset);
					}
					else if (TryGetOffsetAt(mousePosition.x, out newestOffset))
					{
						session.BeginSelection(newestOffset);
					}
				}
			}
			else
			{
				panning = true;
				panLastX = mousePosition.x;
			}
			current.Use();
		}
		else if (current.type == EventType.scrollWheel && flag)
		{
			OnWheel(current.delta, mousePosition.x, n);
			current.Use();
		}
		else
		{
			if (current.type != EventType.repaint)
			{
				return;
			}
			if (selecting)
			{
				int newestOffset2;
				if (!Input.GetMouseButton(0))
				{
					selecting = false;
				}
				else if (TryGetOffsetAt(mousePosition.x, out newestOffset2))
				{
					session.ExtendSelection(newestOffset2);
				}
			}
			if (panning)
			{
				if (!Input.GetMouseButton(1) && !Input.GetMouseButton(2))
				{
					panning = false;
				}
				else
				{
					PanTo(mousePosition.x, n);
				}
			}
			hoverOffset = ((flag && !selecting && !panning) ? FindHover(mousePosition) : (-1));
		}
	}

	private void Paint(float w, float h, int n)
	{
		float num = ((spikeLabels.Count == n && Pitch(w) >= 24f) ? 14f : 0f);
		float num2 = (gcRowTop = h - num - 9f);
		int num3 = ((SelectedOffset >= 0) ? session.Ordinal : (-1));
		if (num3 != lastSeenSelectedOrdinal)
		{
			lastSeenSelectedOrdinal = num3;
			if (!selecting)
			{
				EnsureSelectionVisible(n);
			}
		}
		AdvanceAnimation();
		ClampRightOffset(n);
		float num4 = (float)Mathf.Max(1, SlotCapacity) / zoom;
		viewLeft = LeftAgeOf(n, num4, rightOffset);
		float num5 = Pitch(w);
		float num6 = Mathf.Clamp(num5 * 0.7f, 2f, Mathf.Max(2f, num5 - 1f));
		float num7 = num6 * 0.5f;
		float num8 = 92f + ((float)SlotCapacity - viewLeft) * num5;
		if (num8 < w)
		{
			ProfilerGui.Fill(Mathf.Max(92f, num8), 0f, w, h, OutsideCapacityColor);
		}
		PaintGuides(w, num2);
		int selectedOffset = SelectedOffset;
		int num9 = Mathf.Min(Mathf.Max(SelectedOldestOffset, selectedOffset), n);
		float num10 = Mathf.Max(92f, SlotCenterX(num9, num5, n) - num7);
		float num11 = Mathf.Min(w, SlotCenterX(selectedOffset, num5, n) + num7);
		bool flag = selectedOffset >= 0 && selectedOffset < n && num11 > num10;
		Rect rect = Rect.MinMaxRect(num10, 0f, num11, num2);
		if (flag)
		{
			ProfilerGui.Fill(rect, SelectionColor);
		}
		if (hoverOffset >= 0)
		{
			float num12 = SlotCenterX(hoverOffset, num5, n);
			ProfilerGui.Fill(Mathf.Max(92f, num12 - num5 * 0.5f), 0f, Mathf.Min(w, num12 + num5 * 0.5f), num2, HoverColor);
		}
		float budgetMs = session.BudgetMs;
		float budgetNorm = budgetMs / 66.666664f;
		int num13 = Mathf.Max(0, n - 2 - Mathf.CeilToInt(viewLeft + num4));
		int num14 = Mathf.Min(n - 1, n - Mathf.FloorToInt(viewLeft));
		for (int i = num13; i <= num14; i++)
		{
			float num15 = SlotCenterX(i, num5, n);
			float num16 = Mathf.Max(92f, num15 - num7);
			float num17 = Mathf.Min(w, num15 + num7);
			if (!(num17 <= 92f) && !(num16 >= w))
			{
				if (gcMarks[i] > 0f)
				{
					ProfilerGui.Fill(num16, num2 + 2f, num17, num2 + 9f - 1f, BucketColors[GcBucketOf(gcMarks[i], budgetMs)]);
				}
				if (pauseMarks[i] > 0f)
				{
					float num18 = num15 - num5 * 0.5f;
					float num19 = Mathf.Max(1.5f, (num5 - num6) * 0.5f);
					ProfilerGui.Fill(Mathf.Max(92f, num18 - num19), 0f, Mathf.Min(w, num18 + num19), num2 + 9f, PausedGapColor);
				}
				float num20 = bars[i];
				if (!AllocMode || num20 > 0f)
				{
					float num21 = Mathf.Max(1f, num20 * num2);
					ProfilerGui.Fill(num16, num2 - num21, num17, num2, BucketColors[AllocMode ? 4 : BucketOf(num20, budgetNorm)]);
				}
				if (num > 0f)
				{
					ProfilerGui.Label(new Rect(num15 - num5 * 0.5f, num2 + 9f, num5, num), spikeLabels[i], ProfilerGui.TextFaint, TextAnchor.MiddleCenter);
				}
			}
		}
		if (flag)
		{
			ProfilerGui.Outline(rect, (rect.width < 8f) ? FadedSelectionStrokeColor : SelectionStrokeColor);
		}
		if (hoverOffset >= 0)
		{
			PaintTooltip();
		}
	}

	private void PaintGuides(float w, float plotH)
	{
		StringBuilder scratch = ProfilerGui.Scratch;
		if (AllocMode)
		{
			long[] allocGuideBytes = AllocGuideBytes;
			foreach (long bytes in allocGuideBytes)
			{
				float num = plotH - NormalizeAlloc(bytes) * plotH;
				ProfilerGui.Fill(new Rect(92f, num, w - 92f, 1f), GuideColor);
				scratch.Clear();
				ProfilerTreeModel.AppendAllocBytes(scratch, bytes);
				ProfilerGui.Label(new Rect(0f, Mathf.Clamp(num - 8f, 0f, plotH - 16f), 84f, 16f), scratch.ToString(), ProfilerGui.TextFaint, TextAnchor.MiddleRight);
			}
			return;
		}
		float num2 = float.MaxValue;
		for (int j = 0; j < GuideFps.Length; j++)
		{
			float num3 = 1000f / (float)GuideFps[j];
			if (!(num3 > 66.666664f))
			{
				float num4 = plotH - num3 / 66.666664f * plotH;
				ProfilerGui.Fill(new Rect(92f, num4, w - 92f, 1f), GuideColor);
				float num5 = Mathf.Clamp(num4 - 8f, 0f, plotH - 16f);
				if (!(Mathf.Abs(num5 - num2) < 13f))
				{
					num2 = num5;
					ProfilerGui.Label(new Rect(0f, num5, 84f, 16f), GuideLabel(j, num3), ProfilerGui.TextFaint, TextAnchor.MiddleRight);
				}
			}
		}
		float budgetMs = session.BudgetMs;
		if (budgetMs > 0f && budgetMs <= 66.666664f)
		{
			ProfilerGui.Fill(new Rect(92f, plotH - budgetMs / 66.666664f * plotH, w - 92f, 1f), BudgetGuideColor);
		}
	}

	private static string GuideLabel(int index, float ms)
	{
		if (guideLabels[index] != null)
		{
			return guideLabels[index];
		}
		StringBuilder scratch = ProfilerGui.Scratch;
		scratch.Clear();
		scratch.Append(GuideFps[index]).Append(" FPS (");
		ProfilerTreeModel.AppendMs(scratch, ms, "0.#");
		scratch.Append(')');
		return guideLabels[index] = scratch.ToString();
	}

	private static int BucketOf(float normalized, float budgetNorm)
	{
		if (normalized <= budgetNorm)
		{
			return 0;
		}
		if (normalized <= budgetNorm * 1.5f)
		{
			return 1;
		}
		if (normalized <= budgetNorm * 3f)
		{
			return 2;
		}
		return 3;
	}

	private static int GcBucketOf(float pauseMs, float budgetMs)
	{
		float num = ((budgetMs > 0f) ? (pauseMs / budgetMs) : 0f);
		if (!(num >= 0.3f))
		{
			if (!(num >= 0.1f))
			{
				return 1;
			}
			return 2;
		}
		return 3;
	}

	private int FindHover(Vector2 position)
	{
		if (position.x < 92f || !TryGetOffsetAt(position.x, out var newestOffset))
		{
			return -1;
		}
		if (position.y >= gcRowTop - 2f && position.y <= gcRowTop + 9f + 2f)
		{
			return FindGcMark(newestOffset);
		}
		if (position.y < gcRowTop)
		{
			if (!(Pitch(width) >= 5f))
			{
				return FindUnambiguousBar(newestOffset, position.y);
			}
			return newestOffset;
		}
		return -1;
	}

	private void PaintTooltip()
	{
		if (!session.TryGetSnapshotAt(hoverOffset, out var snapshot))
		{
			return;
		}
		if (snapshot.frameIndex != lastTooltipFrameIndex)
		{
			lastTooltipFrameIndex = snapshot.frameIndex;
			StringBuilder scratch = ProfilerGui.Scratch;
			scratch.Clear();
			scratch.Append("Frame ").Append(snapshot.frameIndex);
			scratch.Append("\ntotal ");
			ProfilerTreeModel.AppendMs(scratch, snapshot.totalMs, "0.000");
			if (snapshot.gen0Collections + snapshot.gen1Collections + snapshot.gen2Collections > 0)
			{
				scratch.Append(" / GC ").Append(snapshot.gen0Collections).Append('/')
					.Append(snapshot.gen1Collections)
					.Append('/')
					.Append(snapshot.gen2Collections);
				if (snapshot.promotedBytes > 0)
				{
					scratch.Append(", promoted ");
					ProfilerTreeModel.AppendAllocBytes(scratch, snapshot.promotedBytes);
				}
			}
			if (snapshot.pauseCount > 0)
			{
				scratch.Append('\n').Append(snapshot.pauseCount).Append((snapshot.pauseCount > 1) ? " pauses" : " pause")
					.Append(" / stopped ");
				ProfilerTreeModel.AppendMs(scratch, snapshot.pauseMs, "0.000");
			}
			if (snapshot.allocatedBytes > 0)
			{
				scratch.Append("\nalloc ");
				ProfilerTreeModel.AppendAllocBytes(scratch, snapshot.allocatedBytes);
				if (snapshot.allocatedObjectCount > 0)
				{
					scratch.Append(" (").Append(snapshot.allocatedObjectCount.ToString("n0")).Append(" objs)");
				}
			}
			if (hoverOffset < pauseMarks.Count && pauseMarks[hoverOffset] > 0f)
			{
				scratch.Append("\nprofiling paused ");
				ProfilerTreeModel.AppendMs(scratch, pauseMarks[hoverOffset]);
				scratch.Append(" before this frame");
			}
			lastTooltip = scratch.ToString();
		}
		ProfilerGui.SetTooltip(lastTooltip);
	}

	private int HoverSearchRadius()
	{
		return Mathf.FloorToInt(4f / Mathf.Max(0.01f, Pitch(width)));
	}

	private int FindUnambiguousBar(int hitOffset, float y)
	{
		int num = HoverSearchRadius();
		int num2 = -1;
		int num3 = Mathf.Min(bars.Count - 1, hitOffset + num);
		for (int i = Mathf.Max(0, hitOffset - num); i <= num3; i++)
		{
			if (!(y < gcRowTop - bars[i] * gcRowTop))
			{
				if (num2 >= 0)
				{
					return -1;
				}
				num2 = i;
			}
		}
		return num2;
	}

	private int FindGcMark(int offset)
	{
		if (offset < gcMarks.Count && gcMarks[offset] > 0f)
		{
			return offset;
		}
		int num = HoverSearchRadius();
		for (int i = 1; i <= num; i++)
		{
			if (offset - i >= 0 && offset - i < gcMarks.Count && gcMarks[offset - i] > 0f)
			{
				return offset - i;
			}
			if (offset + i < gcMarks.Count && gcMarks[offset + i] > 0f)
			{
				return offset + i;
			}
		}
		return -1;
	}

	private void EnsureSelectionVisible(int barCount)
	{
		int selectedOffset = SelectedOffset;
		if (!(width <= 0f) && selectedOffset >= 0 && selectedOffset < barCount)
		{
			float num = (float)SlotCapacity / zoom;
			float num2 = LeftAgeOf(barCount, num, rightOffset);
			int num3 = barCount - 1 - selectedOffset;
			if (!((float)num3 >= num2) || !((float)num3 < num2 + num))
			{
				viewAnimation.Cancel();
				rightOffset = Mathf.Max(0f, (float)selectedOffset - num * 0.5f);
			}
		}
	}

	private void OnWheel(Vector2 delta, float mouseX, int n)
	{
		int num = Mathf.Max(1, SlotCapacity);
		float num2 = (viewAnimation.Active ? ((float)viewAnimation.TargetStart) : rightOffset);
		float num3 = (viewAnimation.Active ? ((float)viewAnimation.TargetSpan) : ((float)num / zoom));
		if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
		{
			viewAnimation.Cancel();
			zoom = (float)num / num3;
			rightOffset = num2 + delta.x / 3f * 0.15f * num3;
			ClampRightOffset(n);
			return;
		}
		float p = (0f - delta.y) / 3f;
		float num4 = Mathf.Max(92f, mouseX);
		float num5 = LeftAgeOf(n, num3, num2) + (num4 - 92f) / PitchOfSpan(num3, width);
		float num6 = Mathf.Pow(1.25f, p);
		float num7 = Mathf.Clamp(max: Mathf.Max(0.25f, 32f * (float)num / Mathf.Max(1f, width - 92f)), value: (float)num / num3 * num6, min: 0.25f);
		float num8 = (float)num / num7;
		float num9 = num5 - (num4 - 92f) / PitchOfSpan(num8, width);
		float num10 = Mathf.Clamp((float)n - num8 - num9, 0f, Mathf.Max(0f, (float)n - num8));
		viewAnimation.Start(rightOffset, (float)num / zoom, num10, num8);
	}

	private void AdvanceAnimation()
	{
		if (viewAnimation.Active)
		{
			int num = Mathf.Max(1, SlotCapacity);
			double start = rightOffset;
			double span = (float)num / zoom;
			viewAnimation.Advance(ref start, ref span);
			rightOffset = (float)start;
			zoom = (float)num / Mathf.Max(1f, (float)span);
		}
	}

	private void PanTo(float x, int n)
	{
		float num = Pitch(width);
		if (!(num <= 0f))
		{
			if (x != panLastX)
			{
				viewAnimation.Cancel();
			}
			rightOffset += (x - panLastX) / num;
			panLastX = x;
			ClampRightOffset(n);
		}
	}

	private bool TryGetOffsetAt(float positionX, out int newestOffset)
	{
		newestOffset = 0;
		float num = Pitch(width);
		int count = bars.Count;
		if (num <= 0f || count <= 0)
		{
			return false;
		}
		float num2 = Mathf.Clamp(positionX, 92f, width);
		float f = viewLeft + (num2 - 92f) / num;
		newestOffset = Mathf.Clamp(count - 1 - Mathf.FloorToInt(f), 0, count - 1);
		return true;
	}

	private static float LeftAgeOf(int count, float span, float offset)
	{
		return Mathf.Max(0f, (float)count - span - offset);
	}

	private float Pitch(float w)
	{
		return (w - 92f) / (float)Mathf.Max(1, SlotCapacity) * zoom;
	}

	private static float PitchOfSpan(float span, float w)
	{
		return (w - 92f) / Mathf.Max(1f, span);
	}

	private void ClampRightOffset(int barCount)
	{
		float b = (float)barCount - (float)Mathf.Max(1, SlotCapacity) / zoom;
		rightOffset = Mathf.Clamp(rightOffset, 0f, Mathf.Max(0f, b));
	}

	private float SlotCenterX(float newestOffset, float pitch, int barCount)
	{
		return 92f + ((float)(barCount - 1) - newestOffset - viewLeft + 0.5f) * pitch;
	}
}
