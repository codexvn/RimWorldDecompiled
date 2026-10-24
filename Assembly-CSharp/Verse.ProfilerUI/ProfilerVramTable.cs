using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using RimWorld;
using UnityEngine;
using Waylith;
using Waylith.Diagnostics;
using Waylith.Graphics;
using Waylith.Profiling;

namespace Verse.ProfilerUI;

public sealed class ProfilerVramTable : ProfilerRowSection
{
	private enum EntryKind : byte
	{
		Node,
		Self,
		Individual,
		Overflow
	}

	private struct Entry
	{
		public VramProfiler.Node node;

		public int depth;

		public EntryKind kind;

		public string name;

		public long bytes;

		public long deltaBytes;

		public string detail;
	}

	private const int MaxIndividualRows = 200;

	private const long ExportIndividualsMinDeltaBytes = 262144L;

	private const int ExportIndividualsCap = 48;

	private static readonly long CaptureIntervalTicks = Stopwatch.Frequency / 2;

	private readonly HashSet<string> expandedPaths = new HashSet<string>();

	private readonly List<Entry> entries = new List<Entry>();

	private readonly List<VramProfiler.IndividualAllocation> individualsScratch = new List<VramProfiler.IndividualAllocation>();

	private long lastCaptureTimestamp;

	private bool captureRequested = true;

	private bool expandMatchesOnRefresh;

	private bool sortByDelta;

	private bool lastHadBaseline;

	private string footer = "";

	public bool HasBaseline => VramProfiler.HasBaseline;

	public bool SortByDelta
	{
		get
		{
			return sortByDelta;
		}
		set
		{
			sortByDelta = value;
			captureRequested = true;
		}
	}

	protected override string CopyHeader
	{
		get
		{
			if (!HasBaseline)
			{
				return "%\tbytes\tallocs\tlabel\tdetail";
			}
			return "%\tbytes\tΔ\tallocs\tlabel\tdetail";
		}
	}

	protected override float FooterHeight => 22f;

	public ProfilerVramTable(ProfilerSession session)
		: base(session)
	{
		View.LabelHeader = () => "label";
		View.DetailHeader = "detail";
		View.RootCrumb = () => "GPU";
		BuildColumns();
	}

	private void BuildColumns()
	{
		lastHadBaseline = HasBaseline;
		List<ProfilerColumn> list = new List<ProfilerColumn>
		{
			new ProfilerColumn(ProfilerColumnKind.Percent, 44f, () => "%"),
			new ProfilerColumn(ProfilerColumnKind.Total, 90f, () => "bytes")
		};
		if (HasBaseline)
		{
			list.Add(new ProfilerColumn(ProfilerColumnKind.Delta, 90f, () => "Δ base"));
		}
		list.Add(new ProfilerColumn(ProfilerColumnKind.Calls, 64f, () => "allocs"));
		View.Columns = list.ToArray();
	}

	public override void Invalidate()
	{
		captureRequested = true;
	}

	public void CaptureBaseline()
	{
		VramProfiler.CaptureBaseline();
		captureRequested = true;
	}

	public void ClearBaseline()
	{
		VramProfiler.ClearBaseline();
		captureRequested = true;
	}

	public override void Notify_SearchChanged()
	{
		expandMatchesOnRefresh = true;
		captureRequested = true;
	}

	public override void Refresh()
	{
		long timestamp = Stopwatch.GetTimestamp();
		if (!captureRequested && timestamp - lastCaptureTimestamp < CaptureIntervalTicks)
		{
			return;
		}
		lastCaptureTimestamp = timestamp;
		captureRequested = false;
		if (lastHadBaseline != HasBaseline)
		{
			BuildColumns();
		}
		VramProfiler.Capture(sortByDelta && HasBaseline);
		if (expandMatchesOnRefresh)
		{
			expandMatchesOnRefresh = false;
			if (session.Search.IsActive)
			{
				ExpandSearchMatches(VramProfiler.Root);
			}
		}
		SyncRows();
		WriteFooter();
	}

	private bool ExpandSearchMatches(VramProfiler.Node node)
	{
		bool flag = false;
		for (int i = 0; i < node.Children.Count; i++)
		{
			if (ExpandSearchMatches(node.Children[i]))
			{
				flag = true;
			}
		}
		if (flag && node.FullPath.Length > 0)
		{
			expandedPaths.Add(node.FullPath);
		}
		if (!flag)
		{
			return session.Search.Matches(node.Segment);
		}
		return true;
	}

	private static string SelfKey(VramProfiler.Node node)
	{
		return node.FullPath + "\n(self)";
	}

	private void SyncRows()
	{
		entries.Clear();
		AppendChildren(entries, VramProfiler.Root, 0, allGroups: false);
		View.Resize(entries.Count);
		long totalBytes = Math.Max(1L, VramProfiler.Root.TotalBytes);
		for (int i = 0; i < entries.Count; i++)
		{
			Shape(entries[i], View.Rows[i], totalBytes);
		}
		View.Notify_RowsChanged();
	}

	private void AppendChildren(List<Entry> into, VramProfiler.Node parent, int depth, bool allGroups)
	{
		for (int i = 0; i < parent.Children.Count; i++)
		{
			AppendNode(into, parent.Children[i], depth, allGroups);
		}
	}

	private void AppendNode(List<Entry> into, VramProfiler.Node node, int depth, bool allGroups)
	{
		into.Add(new Entry
		{
			node = node,
			depth = depth,
			kind = EntryKind.Node
		});
		if (node.HasChildren)
		{
			if (!allGroups && !expandedPaths.Contains(node.FullPath))
			{
				return;
			}
			if (node.DirectBytes > 0 || node.DirectBaselineBytes > 0)
			{
				into.Add(new Entry
				{
					node = node,
					depth = depth + 1,
					kind = EntryKind.Self
				});
				if (expandedPaths.Contains(SelfKey(node)))
				{
					AppendIndividuals(into, node, depth + 2);
				}
			}
			AppendChildren(into, node, depth + 1, allGroups);
		}
		else if (expandedPaths.Contains(node.FullPath))
		{
			AppendIndividuals(into, node, depth + 1);
		}
	}

	protected override bool BuildFullSubtree(int index, List<ProfilerRowData> into)
	{
		if (index >= entries.Count)
		{
			return false;
		}
		Entry item = entries[index];
		List<Entry> list = new List<Entry>();
		if (item.kind == EntryKind.Node && item.node.HasChildren)
		{
			AppendNode(list, item.node, item.depth, allGroups: true);
		}
		else
		{
			list.Add(item);
			if (item.kind == EntryKind.Self || (item.kind == EntryKind.Node && item.node.DirectCount > 0))
			{
				AppendIndividuals(list, item.node, item.depth + 1);
			}
		}
		long totalBytes = Math.Max(1L, VramProfiler.Root.TotalBytes);
		for (int i = 0; i < list.Count; i++)
		{
			ProfilerRowData profilerRowData = new ProfilerRowData();
			Shape(list[i], profilerRowData, totalBytes);
			into.Add(profilerRowData);
		}
		return true;
	}

	private void AppendIndividuals(List<Entry> into, VramProfiler.Node node, int depth)
	{
		VramProfiler.CollectIndividuals(node.FullPath, sortByDelta && HasBaseline, individualsScratch);
		int num = Math.Min(individualsScratch.Count, 200);
		for (int i = 0; i < num; i++)
		{
			VramProfiler.IndividualAllocation individualAllocation = individualsScratch[i];
			into.Add(new Entry
			{
				node = node,
				depth = depth,
				kind = EntryKind.Individual,
				name = (individualAllocation.Freed ? (individualAllocation.Name + " (freed)") : individualAllocation.Name),
				bytes = individualAllocation.Bytes,
				deltaBytes = individualAllocation.DeltaBytes,
				detail = individualAllocation.Detail
			});
		}
		if (individualsScratch.Count > num)
		{
			long num2 = 0L;
			long num3 = 0L;
			for (int j = num; j < individualsScratch.Count; j++)
			{
				num2 += individualsScratch[j].Bytes;
				num3 += individualsScratch[j].DeltaBytes;
			}
			into.Add(new Entry
			{
				node = node,
				depth = depth,
				kind = EntryKind.Overflow,
				name = "… " + (individualsScratch.Count - num) + " more",
				bytes = num2,
				deltaBytes = num3
			});
		}
	}

	private void Shape(in Entry entry, ProfilerRowData data, long totalBytes)
	{
		VramProfiler.Node node = entry.node;
		data.Reset();
		data.TotalIsBytes = true;
		data.HasDelta = HasBaseline;
		data.DeltaIsBytes = true;
		data.CallsGrouped = true;
		data.Depth = entry.depth;
		switch (entry.kind)
		{
		case EntryKind.Self:
			data.TotalBytes = node.DirectBytes;
			data.DeltaBytes = node.DirectDeltaBytes;
			data.Calls = node.DirectCount;
			data.Label = "(self)";
			data.HasChildren = true;
			data.Expanded = expandedPaths.Contains(SelfKey(node));
			data.Dimmed = session.Search.IsActive && !session.Search.Matches(node.FullPath);
			data.Detail = "";
			break;
		case EntryKind.Individual:
		case EntryKind.Overflow:
			data.TotalBytes = entry.bytes;
			data.DeltaBytes = entry.deltaBytes;
			data.Label = entry.name;
			data.Dimmed = session.Search.IsActive && !session.Search.Matches(entry.name);
			data.Detail = entry.detail ?? "";
			break;
		default:
		{
			bool flag = node.HasChildren || node.DirectCount > 0;
			data.TotalBytes = node.TotalBytes;
			data.DeltaBytes = node.DeltaBytes;
			data.Calls = node.TotalCount;
			data.Label = node.Segment;
			data.HasChildren = flag;
			data.Expanded = flag && expandedPaths.Contains(node.FullPath);
			data.Dimmed = session.Search.IsActive && !session.Search.Matches(node.FullPath);
			data.Detail = node.SoleDetail ?? "";
			break;
		}
		}
		data.BarFraction = Mathf.Clamp01((float)data.TotalBytes / (float)totalBytes);
		data.DeltaSeverity = Math.Sign(data.DeltaBytes);
	}

	protected override void ToggleExpansionAt(int index)
	{
		if (index < 0 || index >= entries.Count)
		{
			return;
		}
		Entry entry = entries[index];
		string text = null;
		if (entry.kind == EntryKind.Node && (entry.node.HasChildren || entry.node.DirectCount > 0))
		{
			text = entry.node.FullPath;
		}
		else if (entry.kind == EntryKind.Self)
		{
			text = SelfKey(entry.node);
		}
		if (text != null)
		{
			View.Select(index);
			if (!expandedPaths.Add(text))
			{
				expandedPaths.Remove(text);
			}
			SyncRows();
		}
	}

	public override void ExpandAll()
	{
		ExpandSubtree(VramProfiler.Root);
		SyncRows();
	}

	private void ExpandSubtree(VramProfiler.Node node)
	{
		for (int i = 0; i < node.Children.Count; i++)
		{
			VramProfiler.Node node2 = node.Children[i];
			if (node2.HasChildren)
			{
				expandedPaths.Add(node2.FullPath);
				ExpandSubtree(node2);
			}
		}
	}

	public override void CollapseAll()
	{
		expandedPaths.Clear();
		SyncRows();
	}

	private void WriteFooter()
	{
		StringBuilder scratch = ProfilerGui.Scratch;
		scratch.Clear();
		scratch.Append("tracked ").Append(VramProfiler.FormatBytes(VramProfiler.Root.TotalBytes));
		if (HasBaseline)
		{
			scratch.Append(" / Δ since baseline ");
			AppendSignedBytes(scratch, VramProfiler.Root.DeltaBytes);
		}
		if (GpuMemoryReport.TryGetSummary(out var summary))
		{
			scratch.Append(summary.DriverReported ? " / driver " : " / VMA est. ").Append(VramProfiler.FormatBytes(summary.DeviceLocalDriverUsageBytes)).Append(" used / ")
				.Append(VramProfiler.FormatBytes(summary.DeviceLocalBudgetBytes))
				.Append(" budget")
				.Append(" / sysmem ")
				.Append(VramProfiler.FormatBytes(summary.HostVmaAllocationBytes));
		}
		footer = scratch.ToString();
	}

	private static void AppendSignedBytes(StringBuilder builder, long bytes)
	{
		if (bytes == 0L)
		{
			builder.Append('0');
		}
		else
		{
			builder.Append((bytes > 0) ? '+' : '-').Append(VramProfiler.FormatBytes(Math.Abs(bytes)));
		}
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
		VramProfiler.Capture(sortByDelta && HasBaseline);
		StringBuilder stringBuilder = new StringBuilder(16384);
		stringBuilder.Append("VRAM report ").Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")).AppendLine();
		stringBuilder.AppendLine(GpuMemoryReport.BuildCompactSummary());
		GpuMemoryHistory.AppendGrowthSummary(stringBuilder, 12, 1048576L);
		stringBuilder.Append("Frame rate cap: ").AppendLine(EngineApplication.FrameRateCapDescription);
		stringBuilder.Append("Foreign modules: ").AppendLine(string.Join(", ", ProcessModules.ForeignModuleLines()));
		stringBuilder.AppendLine();
		if (HasBaseline)
		{
			stringBuilder.Append("Δ since baseline: ");
			AppendSignedBytes(stringBuilder, VramProfiler.Root.DeltaBytes);
			stringBuilder.AppendLine().AppendLine();
		}
		StringBuilder stringBuilder2 = new StringBuilder(16384);
		stringBuilder2.AppendLine(CopyHeader);
		AppendExportRows(stringBuilder2, VramProfiler.Root, 0);
		stringBuilder.Append(ProfilerClipboard.AlignColumns(stringBuilder2.ToString()));
		string text = Path.Combine(GenFilePaths.SaveDataFolderPath, "VramReport_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt");
		File.WriteAllText(text, stringBuilder.ToString());
		Messages.Message("VRAM report written to " + text, MessageTypeDefOf.SilentInput, historical: false);
	}

	private void AppendExportRows(StringBuilder builder, VramProfiler.Node parent, int depth)
	{
		long num = Math.Max(1L, VramProfiler.Root.TotalBytes);
		for (int i = 0; i < parent.Children.Count; i++)
		{
			VramProfiler.Node node = parent.Children[i];
			builder.Append(((float)node.TotalBytes * 100f / (float)num).ToString("0.0")).Append('\t');
			builder.Append(VramProfiler.FormatBytes(node.TotalBytes)).Append('\t');
			if (HasBaseline)
			{
				AppendSignedBytes(builder, node.DeltaBytes);
				builder.Append('\t');
			}
			builder.Append(node.TotalCount).Append('\t');
			for (int j = 0; j < depth; j++)
			{
				builder.Append("  ");
			}
			builder.Append(node.Segment).Append('\t').Append(node.SoleDetail)
				.AppendLine();
			AppendExportIndividuals(builder, node, depth + 1);
			AppendExportRows(builder, node, depth + 1);
		}
	}

	private void AppendExportIndividuals(StringBuilder builder, VramProfiler.Node node, int depth)
	{
		if (!HasBaseline || node.DirectCount == 0L || Math.Abs(node.DirectDeltaBytes) < 262144)
		{
			return;
		}
		individualsScratch.Clear();
		VramProfiler.CollectIndividuals(node.FullPath, sortByDeltaMagnitude: true, individualsScratch);
		int num = 0;
		for (int i = 0; i < individualsScratch.Count; i++)
		{
			if (num >= 48)
			{
				break;
			}
			VramProfiler.IndividualAllocation individualAllocation = individualsScratch[i];
			if (individualAllocation.DeltaBytes != 0L)
			{
				builder.Append('\t');
				builder.Append(individualAllocation.Freed ? "freed" : VramProfiler.FormatBytes(individualAllocation.Bytes)).Append('\t');
				AppendSignedBytes(builder, individualAllocation.DeltaBytes);
				builder.Append('\t').Append('\t');
				for (int j = 0; j < depth; j++)
				{
					builder.Append("  ");
				}
				builder.Append(individualAllocation.Name).Append('\t').Append(individualAllocation.Detail)
					.AppendLine();
				num++;
			}
		}
	}
}
