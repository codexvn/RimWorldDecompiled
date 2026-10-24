using System;
using System.Collections.Generic;

namespace Waylith.Diagnostics;

public sealed class ProcessMemorySnapshot
{
	public DateTime CapturedAt { get; init; }

	public long ResidentBytes { get; init; }

	public long AnonymousBytes { get; init; }

	public long FileBackedBytes { get; init; }

	public long SharedBytes { get; init; }

	public long PrivateBytes { get; init; }

	public bool HasMappingDetail { get; init; }

	public List<MemoryReportSection> Sections { get; } = new List<MemoryReportSection>();
}
