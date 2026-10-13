using System;
using Waylith.Runtime;

namespace Waylith.UnityCompat;

public sealed class UnityAppState
{
	private static WaylithAppState CoreApp => WaylithRuntime.Current?.App ?? throw new InvalidOperationException("Waylith runtime has not been initialized.");

	public int MainThreadId => CoreApp.MainThreadId;

	public bool IsBatchMode { get; set; }

	public bool IsEditor { get; set; } = true;

	public bool IsPlaying { get; set; } = true;

	public int ExitCode { get; set; }

	public bool RunInBackground
	{
		get
		{
			return EngineWindow.RunInBackground;
		}
		set
		{
			EngineWindow.RunInBackground = value;
		}
	}

	public bool IsFocused { get; set; } = true;

	public int TargetFrameRate
	{
		get
		{
			return EngineWindow.TargetFrameRate;
		}
		set
		{
			EngineWindow.TargetFrameRate = value;
		}
	}

	public string ProductName
	{
		get
		{
			return CoreApp.ProductName;
		}
		set
		{
			CoreApp.ProductName = value;
		}
	}

	public string UnityVersion { get; set; } = "Waylith";

	public string Version
	{
		get
		{
			return CoreApp.Version;
		}
		set
		{
			CoreApp.Version = value;
		}
	}

	public string DataPath => CoreApp.DataPath;

	public string PersistentDataPath => CoreApp.PersistentDataPath;

	public string CacheDataPath => CoreApp.CacheDataPath;

	public string TemporaryCachePath => CoreApp.TemporaryCachePath;

	private event Action<string>? QuitRequested;

	public event Action ShuttingDown
	{
		add
		{
			CoreApp.ShuttingDown += value;
		}
		remove
		{
			CoreApp.ShuttingDown -= value;
		}
	}

	public void RegisterQuitHandler(Action<string> handler)
	{
		QuitRequested += handler;
	}

	public void UnregisterQuitHandler(Action<string> handler)
	{
		QuitRequested -= handler;
	}

	public void RequestQuit(string reason)
	{
		IsPlaying = false;
		this.QuitRequested?.Invoke(reason);
	}
}
