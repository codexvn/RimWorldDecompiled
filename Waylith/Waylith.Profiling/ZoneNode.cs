namespace Waylith.Profiling;

public struct ZoneNode
{
	internal const uint OpenEnd = uint.MaxValue;

	internal const uint MaxRelativeNs = 4294967294u;

	public int pathId;

	internal uint startNs;

	internal uint endNs;

	public int subtreeEnd;

	public readonly long StartNanoseconds => startNs;

	public readonly long EndNanoseconds => endNs;

	public readonly bool IsOpen => endNs == uint.MaxValue;
}
