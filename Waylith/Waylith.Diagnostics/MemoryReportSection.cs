using System.Collections.Generic;

namespace Waylith.Diagnostics;

public sealed class MemoryReportSection
{
	public required string Name { get; init; }

	public long Bytes { get; set; }

	public long Count { get; set; }

	public string? Detail { get; set; }

	public List<MemoryReportItem> Items { get; } = new List<MemoryReportItem>();
}
