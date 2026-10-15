using System;
using System.Text;
using UnityEngine;

namespace Verse.ProfilerUI;

public sealed class ProfilerRowData
{
	public float BarFraction;

	public bool PercentSign;

	public bool TotalIsBytes;

	public double TotalMicroseconds;

	public long TotalBytes;

	public bool HasSelf;

	public double SelfMicroseconds;

	public bool HasDelta;

	public float DeltaMs;

	public bool DeltaIsBytes;

	public long DeltaBytes;

	public int DeltaSeverity;

	public long Calls;

	public bool CallsGrouped;

	public long AllocBytes;

	public Color Swatch;

	public string Label;

	public string Detail;

	public bool Dimmed;

	public int Depth;

	public bool HasChildren;

	public bool Expanded;

	public string PercentText;

	public string TotalText;

	public string SelfText;

	public string DeltaText;

	public string CallsText;

	public string AllocText;

	private string fittedLabel;

	private float fittedLabelWidth;

	public void Reset()
	{
		BarFraction = 0f;
		PercentSign = (TotalIsBytes = (HasSelf = (HasDelta = (DeltaIsBytes = (CallsGrouped = (Dimmed = (HasChildren = (Expanded = false))))))));
		TotalMicroseconds = (SelfMicroseconds = 0.0);
		TotalBytes = (DeltaBytes = (Calls = (AllocBytes = 0L)));
		DeltaMs = 0f;
		DeltaSeverity = (Depth = 0);
		Swatch = default(Color);
		Label = (Detail = null);
		PercentText = null;
		fittedLabel = null;
	}

	public void EnsureDisplayText()
	{
		if (PercentText == null)
		{
			StringBuilder scratch = ProfilerGui.Scratch;
			PercentText = Format(scratch, AppendPercent);
			TotalText = Format(scratch, AppendTotal);
			SelfText = Format(scratch, AppendSelf);
			DeltaText = Format(scratch, AppendDelta);
			CallsText = Format(scratch, delegate(StringBuilder b)
			{
				AppendCalls(b, CallsGrouped);
			});
			scratch.Clear();
			ProfilerTreeModel.AppendAllocBytes(scratch, AllocBytes);
			AllocText = scratch.ToString();
		}
	}

	private static string Format(StringBuilder scratch, Action<StringBuilder> append)
	{
		scratch.Clear();
		append(scratch);
		return scratch.ToString();
	}

	public string FittedLabel(float width)
	{
		if (fittedLabel == null || Mathf.Abs(fittedLabelWidth - width) > 0.5f)
		{
			fittedLabelWidth = width;
			fittedLabel = ProfilerGui.Ellipsize(Label ?? "", width);
		}
		return fittedLabel;
	}

	public void AppendPercent(StringBuilder builder)
	{
		builder.Append((BarFraction * 100f).ToString("0.0"));
		if (PercentSign)
		{
			builder.Append('%');
		}
	}

	public void AppendTotal(StringBuilder builder)
	{
		if (TotalIsBytes)
		{
			ProfilerTreeModel.AppendAllocBytes(builder, TotalBytes);
		}
		else
		{
			ProfilerTreeModel.AppendMs(builder, (float)(TotalMicroseconds / 1000.0), "0.000");
		}
	}

	public void AppendSelf(StringBuilder builder)
	{
		if (HasSelf)
		{
			ProfilerTreeModel.AppendMs(builder, (float)(SelfMicroseconds / 1000.0), "0.000");
		}
	}

	public void AppendDelta(StringBuilder builder)
	{
		if (!HasDelta)
		{
			return;
		}
		if (DeltaIsBytes)
		{
			if (DeltaBytes == 0L)
			{
				builder.Append('—');
				return;
			}
			builder.Append((DeltaBytes > 0) ? '+' : '-');
			ProfilerTreeModel.AppendAllocBytes(builder, Math.Abs(DeltaBytes));
		}
		else if (float.IsNaN(DeltaMs))
		{
			builder.Append('—');
		}
		else
		{
			builder.Append((DeltaMs >= 0f) ? "+" : "").Append(DeltaMs.ToString("0.0"));
		}
	}

	public void AppendCalls(StringBuilder builder, bool grouped)
	{
		if (Calls > 0)
		{
			if (grouped)
			{
				builder.Append(Calls.ToString("n0"));
			}
			else
			{
				builder.Append(Calls);
			}
		}
	}

	public void AppendCopyLine(StringBuilder builder)
	{
		AppendCopyLine(builder, Depth);
	}

	public void AppendCopyLine(StringBuilder builder, int depth)
	{
		AppendPercent(builder);
		builder.Append('\t');
		AppendTotal(builder);
		if (TotalIsBytes && HasDelta)
		{
			builder.Append('\t');
			AppendDelta(builder);
		}
		if (!TotalIsBytes)
		{
			builder.Append('\t');
			if (HasSelf)
			{
				AppendSelf(builder);
			}
			else
			{
				builder.Append('—');
			}
		}
		builder.Append('\t');
		AppendCalls(builder, grouped: false);
		if (!TotalIsBytes)
		{
			builder.Append('\t');
			ProfilerTreeModel.AppendAllocBytes(builder, AllocBytes);
		}
		builder.Append('\t');
		for (int i = 0; i < depth; i++)
		{
			builder.Append("  ");
		}
		builder.Append(Label);
		if (Detail != null)
		{
			builder.Append('\t').Append(Detail);
		}
	}
}
