using System;
using UnityEngine;

namespace Verse.ProfilerUI;

internal sealed class ProfilerPanZoomView
{
	private const float PanPerNotch = 0.15f;

	private const float ZoomPerNotch = 1.25f;

	private const double PanSlack = 0.5;

	private const float ScrollUnitsPerNotch = 3f;

	private float panLastX;

	private float panLastY;

	private bool releaseUserViewOnFinish;

	public double Start { get; set; }

	public double Span { get; set; } = 1.0;

	public float TopLane { get; set; }

	public double Extent { get; set; } = 1.0;

	public bool UserView { get; set; }

	public bool Panning { get; private set; }

	public double MinSpan { get; }

	private double MaxSpan => Math.Max(MinSpan, Extent * 4.0);

	public ProfilerViewAnimation Animation { get; } = new ProfilerViewAnimation();

	public ProfilerPanZoomView(double minSpan)
	{
		MinSpan = minSpan;
	}

	public float ToX(double value, float plotX0, float plotWidth)
	{
		return plotX0 + (float)((value - Start) / Span) * plotWidth;
	}

	public double ToValue(float x, float plotX0, float plotWidth)
	{
		return Start + (double)(x - plotX0) / (double)plotWidth * Span;
	}

	private double ClampStart(double start, double span)
	{
		double num = span * 0.5;
		return Math.Clamp(start, 0.0 - num, Math.Max(0.0 - num, Extent - span + num));
	}

	public void ClampRange()
	{
		Span = Math.Clamp(Span, MinSpan, MaxSpan);
		Start = ClampStart(Start, Span);
	}

	public void Clamp(float visibleLanes, float totalLanes)
	{
		ClampRange();
		TopLane = Mathf.Clamp(TopLane, 0f, Mathf.Max(0f, totalLanes - visibleLanes));
	}

	public void SetTo(double start, double span)
	{
		CancelAnimation();
		Span = Math.Clamp(span, MinSpan, MaxSpan);
		Start = ClampStart(start, Span);
		UserView = true;
	}

	public void AnimateTo(double start, double span, bool releaseUserViewOnFinish = false)
	{
		span = Math.Clamp(span, MinSpan, MaxSpan);
		Animation.Start(Start, Span, ClampStart(start, span), span);
		UserView = true;
		this.releaseUserViewOnFinish = releaseUserViewOnFinish;
	}

	public void CancelAnimation()
	{
		Animation.Cancel();
		releaseUserViewOnFinish = false;
	}

	public bool Advance()
	{
		if (!Animation.Active)
		{
			return false;
		}
		double start = Start;
		double span = Span;
		bool num = Animation.Advance(ref start, ref span);
		Start = start;
		Span = Math.Max(MinSpan, span);
		if (num)
		{
			return false;
		}
		if (releaseUserViewOnFinish)
		{
			UserView = false;
		}
		releaseUserViewOnFinish = false;
		return true;
	}

	public void ShiftOrigin(double delta)
	{
		Start -= delta;
		Animation.ShiftBy(0.0 - delta);
	}

	public void BeginPan(Vector2 position)
	{
		Panning = true;
		panLastX = position.x;
		panLastY = position.y;
	}

	public void EndPan()
	{
		Panning = false;
	}

	public bool TryPan(Vector2 position, float plotWidth, float laneHeight)
	{
		if (!Panning || (position.x == panLastX && position.y == panLastY))
		{
			return false;
		}
		CancelAnimation();
		Start -= (double)((position.x - panLastX) / plotWidth) * Span;
		TopLane -= (position.y - panLastY) / laneHeight;
		panLastX = position.x;
		panLastY = position.y;
		UserView = true;
		return true;
	}

	public bool HandleWheel(Vector2 delta, float mouseX, float plotX0, float plotWidth)
	{
		double num = (Animation.Active ? Animation.TargetStart : Start);
		double num2 = (Animation.Active ? Animation.TargetSpan : Span);
		if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
		{
			CancelAnimation();
			Span = num2;
			Start = num + (double)(delta.x / 3f * 0.15f) * num2;
			UserView = true;
			return false;
		}
		double num3 = num + (double)(Mathf.Max(plotX0, mouseX) - plotX0) / (double)plotWidth * num2;
		float num4 = Mathf.Pow(1.25f, delta.y / 3f);
		double num5 = Math.Clamp(num2 * (double)num4, MinSpan, MaxSpan);
		AnimateTo(num3 - (num3 - num) * (num5 / num2), num5);
		return true;
	}
}
