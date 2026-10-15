namespace Verse.ProfilerUI;

public readonly struct ProfilerZoneSelection
{
	public readonly int ordinal;

	public readonly int threadId;

	public readonly int pathId;

	public ProfilerZoneSelection(int ordinal, int threadId, int pathId)
	{
		this.ordinal = ordinal;
		this.threadId = threadId;
		this.pathId = pathId;
	}
}
