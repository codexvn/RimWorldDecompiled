using System;
using Waylith.Logging;
using Waylith.Profiling;

namespace Waylith.Runtime;

public sealed class WaylithRuntime
{
	public static WaylithRuntime? Current { get; private set; }

	public WaylithAppState App { get; } = new WaylithAppState();

	public WaylithRuntime()
	{
		Current = this;
		App.InitializeDefaults();
		App.MainThreadId = Environment.CurrentManagedThreadId;
		FrameProfiler.IdentifyMainThread();
	}

	public void ConfigurePaths(string dataPath, string persistentDataPath, string temporaryCachePath, string? logDataPath = null, string? cacheDataPath = null)
	{
		App.DataPath = dataPath;
		App.PersistentDataPath = persistentDataPath;
		App.TemporaryCachePath = temporaryCachePath;
		App.LogDataPath = logDataPath ?? persistentDataPath;
		App.CacheDataPath = cacheDataPath ?? persistentDataPath;
		FileLog.SetDirectory(App.LogDataPath);
	}
}
