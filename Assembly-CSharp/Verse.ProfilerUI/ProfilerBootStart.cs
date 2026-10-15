using Waylith.Profiling;

namespace Verse.ProfilerUI;

[StaticConstructorOnStartup]
internal static class ProfilerBootStart
{
	static ProfilerBootStart()
	{
		if (ProfilerPrefs.Current.startOnBoot)
		{
			FrameProfiler.SetEnabled(value: true);
			ProfilerPrefs.Apply();
		}
	}
}
