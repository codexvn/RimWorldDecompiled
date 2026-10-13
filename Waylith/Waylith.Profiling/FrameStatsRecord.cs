namespace Waylith.Profiling;

internal struct FrameStatsRecord
{
	public int FrameNumber;

	public int TickStart;

	public int TickEnd;

	public long AllocatedBytesDelta;

	public int Gen0;

	public int Gen1;

	public int Gen2;
}
