using System;
using Brutal.Logging;
using Brutal.Logging.Internal;

namespace UnityEngine;

public static class Debug
{
	private sealed class DebugLogHandler : ILogHandler
	{
		public void LogFormat(LogType logType, Object context, string format, params object[] args)
		{
			string value = ((args != null && args.Length > 0) ? string.Format(format, args) : format);
			bool shouldAppend;
			switch (logType)
			{
			case LogType.Error:
			case LogType.Assert:
			case LogType.Exception:
			{
				LogCategory category = Debug.category;
				LogCategory logCategory2 = category;
				LogString<Error> message2 = new LogString<Error>(0, 2, category, out shouldAppend);
				if (shouldAppend)
				{
					message2.AppendFormatted(Prefix(context));
					message2.AppendFormatted(value);
				}
				logCategory2.Error(message2, "LogFormat", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\Debug.cs", 19);
				break;
			}
			case LogType.Warning:
			{
				LogCategory category = Debug.category;
				LogCategory logCategory3 = category;
				LogString<Warning> message3 = new LogString<Warning>(0, 2, category, out shouldAppend);
				if (shouldAppend)
				{
					message3.AppendFormatted(Prefix(context));
					message3.AppendFormatted(value);
				}
				logCategory3.Warning(message3, "LogFormat", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\Debug.cs", 20);
				break;
			}
			default:
			{
				LogCategory category = Debug.category;
				LogCategory logCategory = category;
				LogString<Information> message = new LogString<Information>(0, 2, category, out shouldAppend);
				if (shouldAppend)
				{
					message.AppendFormatted(Prefix(context));
					message.AppendFormatted(value);
				}
				logCategory.Info(message, "LogFormat", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\Debug.cs", 21);
				break;
			}
			}
		}

		public void LogException(Exception exception, Object context)
		{
			LogCategory category = Debug.category;
			bool shouldAppend;
			LogString<Error> message = new LogString<Error>(0, 2, category, out shouldAppend);
			if (shouldAppend)
			{
				message.AppendFormatted(Prefix(context));
				message.AppendFormatted(exception);
			}
			category.Error(message, "LogException", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\Debug.cs", 25);
		}
	}

	private static LogCategory category = LogCategory.Make("UnityDebug");

	public static bool developerConsoleVisible { get; set; }

	public static ILogger unityLogger { get; } = new Logger(new DebugLogHandler());

	public static bool isDebugBuild => false;

	public static void Assert(bool condition)
	{
		Assert(condition, "Assertion failed");
	}

	public static void Assert(bool condition, string message)
	{
		Assert(condition, message, null);
	}

	public static void Assert(bool condition, string message, Object context)
	{
		if (!condition)
		{
			LogError("[" + ((context != null) ? $"[{context}] " : "") + "]" + message);
		}
	}

	public static void Log(object message)
	{
		Log($"{message}", null);
	}

	public static void Log(object message, Object context)
	{
		if (unityLogger.logEnabled)
		{
			LogCategory logCategory = category;
			bool shouldAppend;
			LogString<Information> message2 = new LogString<Information>(0, 2, logCategory, out shouldAppend);
			if (shouldAppend)
			{
				message2.AppendFormatted(Prefix(context));
				message2.AppendFormatted(message);
			}
			logCategory.Info(message2, "Log", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\Debug.cs", 41);
		}
	}

	public static void LogError(object message)
	{
		LogError(message, null);
	}

	public static void LogException(Exception exception)
	{
		LogError(exception.ToString());
	}

	public static void LogWarning(object message)
	{
		LogWarning(message, null);
	}

	public static void LogWarning(object message, Object context)
	{
		if (unityLogger.logEnabled)
		{
			LogCategory logCategory = category;
			bool shouldAppend;
			LogString<Warning> message2 = new LogString<Warning>(0, 2, logCategory, out shouldAppend);
			if (shouldAppend)
			{
				message2.AppendFormatted(Prefix(context));
				message2.AppendFormatted(message);
			}
			logCategory.Warning(message2, "LogWarning", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\Debug.cs", 49);
		}
	}

	public static void LogError(object message, Object context)
	{
		if (unityLogger.logEnabled)
		{
			LogCategory logCategory = category;
			bool shouldAppend;
			LogString<Error> message2 = new LogString<Error>(0, 2, logCategory, out shouldAppend);
			if (shouldAppend)
			{
				message2.AppendFormatted(Prefix(context));
				message2.AppendFormatted(message);
			}
			logCategory.Error(message2, "LogError", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\Debug.cs", 54);
		}
	}

	public static void LogException(Exception exception, Object context)
	{
		LogError(exception.ToString(), context);
	}

	public static void LogFormat(string format, params object[] args)
	{
		Log(string.Format(format, args));
	}

	public static void LogFormat(Object context, string format, params object[] args)
	{
		Log(string.Format(format, args), context);
	}

	public static void LogWarningFormat(string format, params object[] args)
	{
		LogWarning(string.Format(format, args));
	}

	public static void LogWarningFormat(Object context, string format, params object[] args)
	{
		LogWarning(string.Format(format, args), context);
	}

	public static void LogErrorFormat(string format, params object[] args)
	{
		LogError(string.Format(format, args));
	}

	public static void LogErrorFormat(Object context, string format, params object[] args)
	{
		LogError(string.Format(format, args), context);
	}

	public static void LogAssertion(object message)
	{
		LogError(message);
	}

	public static void LogAssertionFormat(string format, params object[] args)
	{
		LogError(string.Format(format, args));
	}

	private static string Prefix(Object context)
	{
		if (!(context != null))
		{
			return "";
		}
		return $"[{context}] ";
	}
}
