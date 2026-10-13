using System;
using System.Globalization;

namespace UnityEngine;

public class Logger : ILogger, ILogHandler
{
	public ILogHandler logHandler { get; set; }

	public bool logEnabled { get; set; }

	public LogType filterLogType { get; set; }

	public Logger(ILogHandler logHandler)
	{
		this.logHandler = logHandler;
		logEnabled = true;
		filterLogType = LogType.Log;
	}

	public bool IsLogTypeAllowed(LogType logType)
	{
		if (!logEnabled)
		{
			return false;
		}
		if (logType == LogType.Exception)
		{
			return true;
		}
		if (filterLogType != LogType.Exception)
		{
			return logType <= filterLogType;
		}
		return false;
	}

	private static string GetString(object message)
	{
		if (message != null)
		{
			if (message is IFormattable formattable)
			{
				return formattable.ToString(null, CultureInfo.InvariantCulture);
			}
			return message.ToString();
		}
		return "Null";
	}

	public void Log(LogType logType, object message)
	{
		Log(logType, message, null);
	}

	public void Log(LogType logType, object message, Object context)
	{
		if (IsLogTypeAllowed(logType))
		{
			logHandler.LogFormat(logType, context, "{0}", GetString(message));
		}
	}

	public void Log(LogType logType, string tag, object message)
	{
		Log(logType, tag, message, null);
	}

	public void Log(LogType logType, string tag, object message, Object context)
	{
		if (IsLogTypeAllowed(logType))
		{
			logHandler.LogFormat(logType, context, "{0}: {1}", tag, GetString(message));
		}
	}

	public void Log(object message)
	{
		Log(LogType.Log, message, null);
	}

	public void Log(string tag, object message)
	{
		Log(LogType.Log, tag, message, null);
	}

	public void Log(string tag, object message, Object context)
	{
		Log(LogType.Log, tag, message, context);
	}

	public void LogWarning(string tag, object message)
	{
		Log(LogType.Warning, tag, message, null);
	}

	public void LogWarning(string tag, object message, Object context)
	{
		Log(LogType.Warning, tag, message, context);
	}

	public void LogError(string tag, object message)
	{
		Log(LogType.Error, tag, message, null);
	}

	public void LogError(string tag, object message, Object context)
	{
		Log(LogType.Error, tag, message, context);
	}

	public void LogException(Exception exception)
	{
		LogException(exception, null);
	}

	public void LogException(Exception exception, Object context)
	{
		if (logEnabled)
		{
			logHandler.LogException(exception, context);
		}
	}

	public void LogFormat(LogType logType, string format, params object[] args)
	{
		LogFormat(logType, null, format, args);
	}

	public void LogFormat(LogType logType, Object context, string format, params object[] args)
	{
		if (IsLogTypeAllowed(logType))
		{
			logHandler.LogFormat(logType, context, format, args);
		}
	}
}
