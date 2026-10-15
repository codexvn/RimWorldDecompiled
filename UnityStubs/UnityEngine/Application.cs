using System;
using System.Diagnostics;
using System.IO;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Threading;
using Brutal.Logging;
using Brutal.Logging.Internal;
using Microsoft.Extensions.Logging;
using UnityEngine.Events;
using Waylith.Logging;
using Waylith.UnityCompat;

namespace UnityEngine;

public class Application
{
	public delegate void LogCallback(string condition, string stackTrace, LogType type);

	private static readonly LogCategory Log = LogCategory.Make("UnityCompat");

	private static UnityAction _onBeforeRender;

	private static bool _beforeRenderSubscribed;

	private static LogCallback _logMessageReceived;

	private static LogCallback _logMessageReceivedThreaded;

	private static int _logBridgeHooked;

	[ThreadStatic]
	private static bool _dispatchingLog;

	public static SystemLanguage systemLanguage => SystemLanguage.English;

	public static string dataPath => UnityRuntime.Current?.App.DataPath ?? Environment.CurrentDirectory;

	public static bool isBatchMode => UnityRuntime.Current?.App.IsBatchMode ?? false;

	public static bool isEditor => UnityRuntime.Current?.App.IsEditor ?? true;

	public static bool isPlaying => UnityRuntime.Current?.App.IsPlaying ?? true;

	public static bool isFocused => UnityRuntime.Current?.App.IsFocused ?? true;

	public static string persistentDataPath => UnityRuntime.Current?.App.PersistentDataPath ?? Environment.CurrentDirectory;

	public static RuntimePlatform platform { get; } = Environment.OSVersion.Platform switch
	{
		PlatformID.Unix => RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? RuntimePlatform.OSXPlayer : RuntimePlatform.LinuxPlayer, 
		PlatformID.Win32NT => RuntimePlatform.WindowsPlayer, 
		_ => throw new NotImplementedException(), 
	};

	public static NetworkReachability internetReachability
	{
		get
		{
			if (!NetworkInterface.GetIsNetworkAvailable())
			{
				return NetworkReachability.NotReachable;
			}
			return NetworkReachability.ReachableViaLocalAreaNetwork;
		}
	}

	public static string companyName => "Ludeon Studios";

	public static string consoleLogPath => Path.Combine(UnityRuntime.Current?.Core.App.LogDataPath ?? persistentDataPath, "Logs", "Player.log");

	public static string productName => UnityRuntime.Current?.App.ProductName ?? "RimWorld by Ludeon Studios";

	public static string temporaryCachePath => UnityRuntime.Current?.App.TemporaryCachePath ?? Path.GetTempPath();

	public static string unityVersion => UnityRuntime.Current?.App.UnityVersion ?? "Brutal";

	public static string version => UnityRuntime.Current?.App.Version ?? "0.0.0";

	public static bool runInBackground
	{
		get
		{
			return UnityRuntime.Current?.App.RunInBackground ?? true;
		}
		set
		{
			if (UnityRuntime.Current != null)
			{
				UnityRuntime.Current.App.RunInBackground = value;
			}
		}
	}

	public static int targetFrameRate
	{
		get
		{
			return UnityRuntime.Current?.App.TargetFrameRate ?? (-1);
		}
		set
		{
			if (UnityRuntime.Current != null)
			{
				UnityRuntime.Current.App.TargetFrameRate = value;
			}
		}
	}

	public static event Action quitting;

	public static event Func<bool> wantsToQuit;

	public static event UnityAction onBeforeRender
	{
		add
		{
			EnsureBeforeRenderSubscribed();
			_onBeforeRender = (UnityAction)Delegate.Combine(_onBeforeRender, value);
		}
		remove
		{
			_onBeforeRender = (UnityAction)Delegate.Remove(_onBeforeRender, value);
		}
	}

	public static event LogCallback logMessageReceived
	{
		add
		{
			EnsureLogBridge();
			_logMessageReceived = (LogCallback)Delegate.Combine(_logMessageReceived, value);
		}
		remove
		{
			_logMessageReceived = (LogCallback)Delegate.Remove(_logMessageReceived, value);
		}
	}

	public static event LogCallback logMessageReceivedThreaded
	{
		add
		{
			EnsureLogBridge();
			_logMessageReceivedThreaded = (LogCallback)Delegate.Combine(_logMessageReceivedThreaded, value);
		}
		remove
		{
			_logMessageReceivedThreaded = (LogCallback)Delegate.Remove(_logMessageReceivedThreaded, value);
		}
	}

	private static void EnsureBeforeRenderSubscribed()
	{
		if (_beforeRenderSubscribed)
		{
			return;
		}
		UnityWorld unityWorld = UnityRuntime.Current?.World;
		if (unityWorld != null)
		{
			unityWorld.PostLateUpdate += delegate
			{
				InvokeBeforeRender();
			};
			_beforeRenderSubscribed = true;
		}
	}

	private static void InvokeBeforeRender()
	{
		try
		{
			_onBeforeRender?.Invoke();
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
		}
	}

	private static void EnsureLogBridge()
	{
		if (Interlocked.Exchange(ref _logBridgeHooked, 1) == 0)
		{
			LogStream.MessageReceived += DispatchLog;
		}
	}

	private static void DispatchLog(LogLevel level, string category, string message, string stackTrace)
	{
		if (_dispatchingLog)
		{
			return;
		}
		_dispatchingLog = true;
		try
		{
			LogType logType;
			switch (level)
			{
			case LogLevel.Error:
			case LogLevel.Critical:
				logType = LogType.Error;
				break;
			case LogLevel.Warning:
				logType = LogType.Warning;
				break;
			default:
				logType = LogType.Log;
				break;
			}
			LogType type = logType;
			if (stackTrace == null)
			{
				stackTrace = string.Empty;
			}
			_logMessageReceivedThreaded?.Invoke(message, stackTrace, type);
			if (UnityRuntime.Current?.App.MainThreadId == Environment.CurrentManagedThreadId)
			{
				_logMessageReceived?.Invoke(message, stackTrace, type);
			}
		}
		catch (Exception value)
		{
			LogCategory log = Log;
			bool shouldAppend;
			LogString<Error> message2 = new LogString<Error>(27, 1, log, out shouldAppend);
			if (shouldAppend)
			{
				message2.AppendLiteral("Log message handler threw: ");
				message2.AppendFormatted(value);
			}
			log.Error(message2, "DispatchLog", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\Application.cs", 85);
		}
		finally
		{
			_dispatchingLog = false;
		}
	}

	public static void OpenURL(string url)
	{
		if (string.IsNullOrWhiteSpace(url))
		{
			return;
		}
		try
		{
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				Process.Start(new ProcessStartInfo(url)
				{
					UseShellExecute = true
				});
			}
			else
			{
				Process.Start(new ProcessStartInfo(RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? "open" : "xdg-open")
				{
					ArgumentList = { url }
				});
			}
		}
		catch (Exception ex)
		{
			LogCategory log = Log;
			bool shouldAppend;
			LogString<Error> message = new LogString<Error>(39, 2, log, out shouldAppend);
			if (shouldAppend)
			{
				message.AppendLiteral("Application.OpenURL failed to open '");
				message.AppendFormatted(url);
				message.AppendLiteral("': ");
				message.AppendFormatted(ex.Message);
			}
			log.Error(message, "OpenURL", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\Application.cs", 162);
		}
	}

	public static void Quit(int exitCode)
	{
		if (UnityRuntime.Current != null)
		{
			UnityRuntime.Current.App.ExitCode = exitCode;
		}
		Quit();
	}

	public static void Quit()
	{
		if (WantsToQuit())
		{
			if (UnityRuntime.Current != null)
			{
				UnityRuntime.Current.App.IsPlaying = false;
			}
			Application.quitting?.Invoke();
			UnityRuntime.Current?.App.RequestQuit("Application.Quit");
		}
	}

	private static bool WantsToQuit()
	{
		if (Application.wantsToQuit == null)
		{
			return true;
		}
		Delegate[] invocationList = Application.wantsToQuit.GetInvocationList();
		foreach (Delegate @delegate in invocationList)
		{
			try
			{
				if (!((Func<bool>)@delegate)())
				{
					return false;
				}
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
		}
		return true;
	}
}
