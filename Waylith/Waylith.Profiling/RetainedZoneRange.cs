using System;

namespace Waylith.Profiling;

public sealed class RetainedZoneRange
{
	public int threadId;

	public ZoneChunk[] chunks = Array.Empty<ZoneChunk>();
}
