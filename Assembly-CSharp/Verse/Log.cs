using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using Brutal.Logging;
using LudeonTK;
using UnityEngine;
using Waylith.UnityCompat;

namespace Verse;

public static class Log
{
	public class LogLock : IDisposable
	{
		public LogLock()
		{
			logDisablers = Interlocked.Increment(ref logDisablers);
		}

		public void Dispose()
		{
			logDisablers = Interlocked.Decrement(ref logDisablers);
		}
	}

	private static LogMessageQueue messageQueue;

	private static ConcurrentQueue<LogMessage> offThreadMessages;

	private static HashSet<int> usedKeys;

	public static bool openOnMessage;

	private static bool currentlyLoggingError;

	private static int messageCount;

	private static bool reachedMaxMessagesLimit;

	private static object logLock;

	private static int logDisablers;

	private static readonly LogCategory logCategory;

	private const int StopLoggingAtMessageCount = 10000;

	public static IEnumerable<LogMessage> Messages
	{
		get
		{
			if (IsMainThread)
			{
				lock (logLock)
				{
					DrainOffThreadMessages();
				}
			}
			return messageQueue.Messages;
		}
	}

	private static bool IsMainThread => UnityRuntime.Current?.App.MainThreadId == Environment.CurrentManagedThreadId;

	private static bool PreventLogging
	{
		get
		{
			if (!reachedMaxMessagesLimit)
			{
				return logDisablers > 0;
			}
			return true;
		}
	}

	public static LogLock LockMessages()
	{
		return new LogLock();
	}

	static Log()
	{
		messageQueue = new LogMessageQueue();
		offThreadMessages = new ConcurrentQueue<LogMessage>();
		usedKeys = new HashSet<int>();
		openOnMessage = false;
		logLock = new object();
		logCategory = LogCategory.Make("Verse");
		Application.logMessageReceivedThreaded += Notify_LogMessageReceived;
	}

	public static void ResetMessageCount()
	{
		lock (logLock)
		{
			messageCount = 0;
			if (reachedMaxMessagesLimit)
			{
				Debug.unityLogger.logEnabled = true;
				reachedMaxMessagesLimit = false;
				Message("Message logging is now once again on.");
			}
		}
	}

	public static void Message(string text)
	{
		lock (logLock)
		{
			if (!PreventLogging)
			{
				logCategory.Info(text, "Message", "D:\\Workspaces\\rimworld-builds\\Assets\\Scripts\\LudeonTK\\Utility\\Log\\Log.cs", 91);
				PostMessage();
			}
		}
	}

	public static void Message(object obj)
	{
		Message(obj.ToString());
	}

	public static void Warning(string text)
	{
		lock (logLock)
		{
			if (!PreventLogging)
			{
				logCategory.Warning(text, "Warning", "D:\\Workspaces\\rimworld-builds\\Assets\\Scripts\\LudeonTK\\Utility\\Log\\Log.cs", 108);
				PostMessage();
				if (Prefs.OpenLogOnWarnings)
				{
					TryOpenLogWindow();
				}
			}
		}
	}

	public static void WarningOnce(string text, int key)
	{
		lock (logLock)
		{
			if (!PreventLogging && usedKeys.Add(key))
			{
				Warning(text);
			}
		}
	}

	public static void Error(string text)
	{
		lock (logLock)
		{
			if (PreventLogging)
			{
				return;
			}
			logCategory.Error(text, "Error", "D:\\Workspaces\\rimworld-builds\\Assets\\Scripts\\LudeonTK\\Utility\\Log\\Log.cs", 137);
			if (currentlyLoggingError)
			{
				return;
			}
			currentlyLoggingError = true;
			try
			{
				if (DebugSettings.pauseOnError && Current.ProgramState == ProgramState.Playing)
				{
					Find.TickManager.Pause();
				}
				PostMessage();
			}
			catch (Exception ex)
			{
				logCategory.Error("An error occurred while logging an error: " + ex, "Error", "D:\\Workspaces\\rimworld-builds\\Assets\\Scripts\\LudeonTK\\Utility\\Log\\Log.cs", 151);
			}
			finally
			{
				currentlyLoggingError = false;
			}
		}
	}

	public static void ErrorOnce(string text, int key)
	{
		lock (logLock)
		{
			if (!PreventLogging && usedKeys.Add(key))
			{
				Error(text);
			}
		}
	}

	public static void Clear()
	{
		lock (logLock)
		{
			EditWindow_Log.ClearSelectedMessage();
			messageQueue.Clear();
			offThreadMessages.Clear();
			ResetMessageCount();
		}
	}

	public static void TryOpenLogWindow()
	{
		if (StaticConstructorOnStartupUtility.coreStaticAssetsLoaded || UnityData.IsInMainThread)
		{
			EditWindow_Log.TryAutoOpen();
		}
	}

	private static void PostMessage()
	{
		if (openOnMessage)
		{
			TryOpenLogWindow();
			EditWindow_Log.SelectLastMessage(expandDetailsPane: true);
		}
	}

	private static void Notify_LogMessageReceived(string text, string stackTrace, LogType type)
	{
		lock (logLock)
		{
			if (!PreventLogging)
			{
				int num = type switch
				{
					LogType.Log => 0, 
					LogType.Warning => 1, 
					_ => 2, 
				};
				LogMessage logMessage = new LogMessage((LogMessageType)num, text, stackTrace);
				if (IsMainThread)
				{
					DrainOffThreadMessages();
					messageQueue.Enqueue(logMessage);
				}
				else
				{
					offThreadMessages.Enqueue(logMessage);
				}
				if (num == 2 && (!PlayDataLoader.Loaded || Prefs.DevMode))
				{
					TryOpenLogWindow();
				}
			}
		}
	}

	private static void DrainOffThreadMessages()
	{
		LogMessage result;
		while (offThreadMessages.TryDequeue(out result))
		{
			messageQueue.Enqueue(result);
		}
	}

	public static void Notify_MessageReceivedThreadedInternal(string msg, string stackTrace, LogType type)
	{
		if (++messageCount == 10000)
		{
			Warning("Reached max messages limit. Stopping logging to avoid spam.");
			Notify_LogMessageReceived("Reached max messages limit. Stopping logging to avoid spam.", StackTraceUtility.ExtractStackTrace(), LogType.Warning);
			reachedMaxMessagesLimit = true;
			Debug.unityLogger.logEnabled = false;
		}
	}
}
