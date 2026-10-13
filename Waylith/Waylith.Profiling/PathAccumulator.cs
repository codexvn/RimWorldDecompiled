namespace Waylith.Profiling;

public struct PathAccumulator
{
	public int entries;

	public int calls;

	public double totalMicroseconds;

	public double selfMicroseconds;

	public long allocBytes;

	public long selfAllocBytes;
}
