namespace Waylith.Diagnostics;

public readonly record struct MemoryReportItem(string Name, long Bytes, long Count = 0L, string? Detail = null);
