namespace Verse.ProfilerUI;

internal struct ProfilerRefreshGate
{
	private bool valid;

	private int lastRefreshTick;

	private int lastRegistryVersion;

	private int lastSelectionVersion;

	public void Invalidate()
	{
		valid = false;
	}

	public bool ShouldRefresh(ProfilerSession session)
	{
		if (valid && lastRefreshTick == session.RefreshTick && lastRegistryVersion == ProfilerThreadRegistry.Version && lastSelectionVersion == ProfilerThreadRegistry.SelectionVersion)
		{
			return false;
		}
		valid = true;
		lastRefreshTick = session.RefreshTick;
		lastRegistryVersion = ProfilerThreadRegistry.Version;
		lastSelectionVersion = ProfilerThreadRegistry.SelectionVersion;
		return true;
	}
}
