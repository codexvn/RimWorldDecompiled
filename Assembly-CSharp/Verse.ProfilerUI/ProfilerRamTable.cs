using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using RimWorld;
using UnityEngine;
using Waylith.Diagnostics;
using Waylith.Profiling;

namespace Verse.ProfilerUI;

public sealed class ProfilerRamTable : ProfilerRowSection
{
	private struct Entry
	{
		public MemoryReportSection section;

		public int item;
	}

	private static readonly long CaptureIntervalTicks = Stopwatch.Frequency * 2;

	private readonly HashSet<string> expandedSections = new HashSet<string>();

	private readonly List<Entry> entries = new List<Entry>();

	private ProcessMemorySnapshot snapshot;

	private long lastCaptureTimestamp;

	private bool captureRequested = true;

	private string footer = "";

	protected override string CopyHeader => "%\tbytes\tcount\tbucket\tdetail";

	protected override float FooterHeight => 22f;

	public ProfilerRamTable(ProfilerSession session)
		: base(session)
	{
		View.LabelHeader = () => "bucket";
		View.DetailHeader = "detail";
		View.RootCrumb = () => "RAM";
		View.Columns = new ProfilerColumn[3]
		{
			new ProfilerColumn(ProfilerColumnKind.Percent, 44f, () => "%"),
			new ProfilerColumn(ProfilerColumnKind.Total, 90f, () => "bytes"),
			new ProfilerColumn(ProfilerColumnKind.Calls, 64f, () => "count")
		};
		expandedSections.Add("Process");
		expandedSections.Add("Managed heap (GC)");
		expandedSections.Add("Native heap (malloc)");
	}

	public override void Invalidate()
	{
		captureRequested = true;
	}

	public override void Notify_SearchChanged()
	{
		captureRequested = true;
	}

	public override void Refresh()
	{
		long timestamp = Stopwatch.GetTimestamp();
		if (captureRequested || timestamp - lastCaptureTimestamp >= CaptureIntervalTicks)
		{
			lastCaptureTimestamp = timestamp;
			captureRequested = false;
			snapshot = ProcessMemoryReport.Capture();
			SyncRows();
			WriteFooter();
		}
	}

	private void SyncRows()
	{
		entries.Clear();
		if (snapshot == null)
		{
			return;
		}
		foreach (MemoryReportSection section in snapshot.Sections)
		{
			entries.Add(new Entry
			{
				section = section,
				item = -1
			});
			if (expandedSections.Contains(section.Name))
			{
				for (int i = 0; i < section.Items.Count; i++)
				{
					entries.Add(new Entry
					{
						section = section,
						item = i
					});
				}
			}
		}
		View.Resize(entries.Count);
		for (int j = 0; j < entries.Count; j++)
		{
			Shape(entries[j], View.Rows[j]);
		}
		View.Notify_RowsChanged();
	}

	protected override bool BuildFullSubtree(int index, List<ProfilerRowData> into)
	{
		if (index >= entries.Count)
		{
			return false;
		}
		Entry entry = entries[index];
		ProfilerRowData profilerRowData = new ProfilerRowData();
		Shape(in entry, profilerRowData);
		into.Add(profilerRowData);
		if (entry.item >= 0)
		{
			return true;
		}
		for (int i = 0; i < entry.section.Items.Count; i++)
		{
			ProfilerRowData profilerRowData2 = new ProfilerRowData();
			Shape(new Entry
			{
				section = entry.section,
				item = i
			}, profilerRowData2);
			into.Add(profilerRowData2);
		}
		return true;
	}

	private void Shape(in Entry entry, ProfilerRowData data)
	{
		data.Reset();
		data.TotalIsBytes = true;
		data.CallsGrouped = true;
		long num = Math.Max(1L, snapshot.ResidentBytes);
		if (entry.item < 0)
		{
			MemoryReportSection section = entry.section;
			data.Depth = 0;
			data.TotalBytes = section.Bytes;
			data.Calls = section.Count;
			data.Label = section.Name;
			data.Detail = section.Detail ?? "";
			data.HasChildren = section.Items.Count > 0;
			data.Expanded = expandedSections.Contains(section.Name);
			data.Dimmed = session.Search.IsActive && !session.Search.Matches(section.Name);
		}
		else
		{
			MemoryReportItem memoryReportItem = entry.section.Items[entry.item];
			data.Depth = 1;
			data.TotalBytes = memoryReportItem.Bytes;
			data.Calls = memoryReportItem.Count;
			data.Label = memoryReportItem.Name;
			data.Detail = memoryReportItem.Detail ?? "";
			data.Dimmed = session.Search.IsActive && !session.Search.Matches(memoryReportItem.Name);
		}
		data.BarFraction = Mathf.Clamp01((float)data.TotalBytes / (float)num);
	}

	protected override void ToggleExpansionAt(int index)
	{
		if (index < 0 || index >= entries.Count)
		{
			return;
		}
		Entry entry = entries[index];
		if (entry.item < 0 && entry.section.Items.Count != 0)
		{
			View.Select(index);
			if (!expandedSections.Add(entry.section.Name))
			{
				expandedSections.Remove(entry.section.Name);
			}
			SyncRows();
		}
	}

	public override void ExpandAll()
	{
		if (snapshot == null)
		{
			return;
		}
		foreach (MemoryReportSection section in snapshot.Sections)
		{
			expandedSections.Add(section.Name);
		}
		SyncRows();
	}

	public override void CollapseAll()
	{
		expandedSections.Clear();
		SyncRows();
	}

	private void WriteFooter()
	{
		StringBuilder scratch = ProfilerGui.Scratch;
		scratch.Clear();
		scratch.Append("resident ").Append(VramProfiler.FormatBytes(snapshot.ResidentBytes));
		if (snapshot.HasMappingDetail)
		{
			scratch.Append(" / anonymous ").Append(VramProfiler.FormatBytes(snapshot.AnonymousBytes)).Append(" / file backed ")
				.Append(VramProfiler.FormatBytes(snapshot.FileBackedBytes));
		}
		else
		{
			scratch.Append(" / private ").Append(VramProfiler.FormatBytes(snapshot.PrivateBytes));
		}
		footer = scratch.ToString();
	}

	protected override void AppendCopyContext(StringBuilder builder)
	{
		builder.AppendLine(footer);
	}

	protected override void DrawFooter(Rect rect)
	{
		ProfilerGui.Fill(new Rect(rect.x, rect.y, rect.width, 1f), ProfilerGui.Divider);
		ProfilerGui.Label(new Rect(rect.x + 12f, rect.y + 1f, rect.width - 24f, rect.height - 1f), footer, ProfilerGui.TextFaint);
	}

	public void Export()
	{
		string text = Path.Combine(GenFilePaths.SaveDataFolderPath, "MemoryReport_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt");
		File.WriteAllText(text, ProcessMemoryReport.BuildReport());
		Messages.Message("Memory report written to " + text, MessageTypeDefOf.SilentInput, historical: false);
	}
}
