using LudeonTK;

namespace Verse.ProfilerUI;

public static class DebugActionsFrameProfiler
{
	[DebugAction("General", "Frame profiler", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.Playing)]
	private static void OpenFrameProfiler()
	{
		Toggle();
	}

	[DebugAction("General", "Frame profiler", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.Entry)]
	private static void OpenFrameProfilerEntry()
	{
		Toggle();
	}

	private static void Toggle()
	{
		if (!Find.WindowStack.TryRemove(typeof(Window_FrameProfiler)))
		{
			Find.WindowStack.Add(new Window_FrameProfiler());
		}
	}
}
