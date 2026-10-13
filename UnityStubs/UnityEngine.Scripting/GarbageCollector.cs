using UnityStubs.Internal;

namespace UnityEngine.Scripting;

public static class GarbageCollector
{
	public enum Mode
	{
		Disabled,
		Enabled,
		Manual
	}

	public delegate void ModeChangedCallback(Mode mode);

	private static Mode _mode = Mode.Enabled;

	public static Mode GCMode
	{
		get
		{
			return _mode;
		}
		set
		{
			if (_mode != value)
			{
				_mode = value;
				GarbageCollector.GCModeChanged?.Invoke(value);
			}
		}
	}

	public static bool isIncremental => false;

	public static ulong incrementalTimeSliceNanoseconds { get; set; }

	public static event ModeChangedCallback GCModeChanged;

	public static bool CollectIncremental(ulong nanoseconds = 0uL)
	{
		StubWarner.LogStub("CollectIncremental", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\GarbageCollector.cs", 35);
		return false;
	}
}
