using System;
using System.IO;

namespace Waylith.Runtime;

public sealed class WaylithAppState
{
	public int MainThreadId { get; internal set; }

	public string ProductName { get; set; } = "Waylith";

	public string Version { get; set; } = "0.0.0";

	public string DataPath { get; internal set; } = string.Empty;

	public string PersistentDataPath { get; internal set; } = string.Empty;

	public string LogDataPath { get; internal set; } = string.Empty;

	public string CacheDataPath { get; internal set; } = string.Empty;

	public string TemporaryCachePath { get; internal set; } = Path.GetTempPath();

	public event Action ShuttingDown = delegate
	{
	};

	internal void InitializeDefaults()
	{
		string currentDirectory = Environment.CurrentDirectory;
		if (string.IsNullOrWhiteSpace(DataPath))
		{
			DataPath = Path.GetFullPath(Path.Combine(currentDirectory, "../Assets"));
		}
		if (string.IsNullOrWhiteSpace(PersistentDataPath))
		{
			PersistentDataPath = currentDirectory;
		}
		if (string.IsNullOrWhiteSpace(LogDataPath))
		{
			LogDataPath = PersistentDataPath;
		}
		if (string.IsNullOrWhiteSpace(CacheDataPath))
		{
			CacheDataPath = PersistentDataPath;
		}
		if (string.IsNullOrWhiteSpace(TemporaryCachePath))
		{
			TemporaryCachePath = Path.GetTempPath();
		}
	}

	internal void FireShuttingDown()
	{
		this.ShuttingDown();
	}
}
