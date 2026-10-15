using System;
using System.Diagnostics;
using UnityEngine;

namespace Verse.ProfilerUI;

public sealed class ProfilerViewAnimation
{
	private const float DurationSeconds = 0.25f;

	private double fromStart;

	private double fromEnd;

	private double toStart;

	private double toEnd;

	private float progress = 1f;

	private long timestamp;

	private bool contentAnimating;

	public bool Active => progress < 1f;

	public bool NeedsRebuild
	{
		get
		{
			if (!Active)
			{
				return contentAnimating;
			}
			return true;
		}
	}

	public double TargetStart => toStart;

	public double TargetSpan => toEnd - toStart;

	public void Start(double currentStart, double currentSpan, double targetStart, double targetSpan)
	{
		fromStart = currentStart;
		fromEnd = currentStart + currentSpan;
		toStart = targetStart;
		toEnd = targetStart + targetSpan;
		progress = 0f;
		timestamp = Stopwatch.GetTimestamp();
	}

	public void Cancel()
	{
		progress = 1f;
	}

	public void SetContentAnimating(bool animating)
	{
		contentAnimating = animating;
	}

	public void ShiftBy(double delta)
	{
		fromStart += delta;
		fromEnd += delta;
		toStart += delta;
		toEnd += delta;
	}

	public bool Advance(ref double start, ref double span)
	{
		if (progress >= 1f)
		{
			return false;
		}
		long num = Stopwatch.GetTimestamp();
		float num2 = (float)((double)(num - timestamp) / (double)Stopwatch.Frequency);
		timestamp = num;
		progress = Mathf.Min(1f, progress + num2 / 0.25f);
		float t = Mathf.Sqrt(Mathf.Sin(progress * (float)Math.PI * 0.5f));
		start = Lerp(fromStart, toStart, t);
		span = Lerp(fromEnd, toEnd, t) - start;
		return progress < 1f;
	}

	private static double Lerp(double a, double b, float t)
	{
		return a + (b - a) * (double)t;
	}
}
