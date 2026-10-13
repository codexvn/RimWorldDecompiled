using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Console;

namespace Waylith.Logging;

public class WaylithLogFormatter : ConsoleFormatter
{
	public static bool EnableStackTraces = false;

	private const string ColorWhite = "\u001b[37m\u001b[22m";

	private const string ColorGrey = "\u001b[90m\u001b[22m";

	private const string ColorPaleBlue = "\u001b[34m\u001b[1m";

	private const string ColorYellow = "\u001b[33m\u001b[1m";

	private const string ColorRed = "\u001b[31m\u001b[1m";

	private static readonly string[] LogLevelNames = new string[6] { "TRACE", "DEBUG", " INFO", " WARN", "ERROR", " CRIT" };

	public WaylithLogFormatter()
		: base("WaylithFormatter")
	{
	}

	public static string MonitorLogFormatter(LogLevel logLevel, string category, string message)
	{
		return $"{DateTime.Now:HH:mm:ss.fff} {LogLevelNames[(int)logLevel]} {message}";
	}

	public override void Write<TState>(in LogEntry<TState> logEntry, IExternalScopeProvider? scopeProvider, TextWriter textWriter)
	{
		string text = logEntry.Formatter(logEntry.State, logEntry.Exception);
		string value = logEntry.LogLevel switch
		{
			LogLevel.Information => "\u001b[34m\u001b[1m", 
			LogLevel.Warning => "\u001b[33m\u001b[1m", 
			LogLevel.Error => "\u001b[31m\u001b[1m", 
			LogLevel.Critical => "\u001b[31m\u001b[1m", 
			_ => "\u001b[37m\u001b[22m", 
		};
		string value2 = LogLevelNames[(int)logEntry.LogLevel];
		string value3 = $"{"\u001b[90m\u001b[22m"}{DateTime.Now:HH:mm:ss}";
		textWriter.Write($"{value3} {value}{value2} {"\u001b[37m\u001b[22m"}{text}");
		textWriter.WriteLine();
		StackTrace stackTrace = ((EnableStackTraces || FileLog.IsCapturingStacks || LogStream.HasListeners) ? new StackTrace(7, fNeedFileInfo: true) : null);
		string text2 = ((stackTrace != null) ? FileLog.FormatStack(stackTrace) : null);
		if (EnableStackTraces && stackTrace != null)
		{
			StackFrame[] frames = stackTrace.GetFrames();
			foreach (StackFrame stackFrame in frames)
			{
				string value4 = (stackFrame.HasSource() ? $"in {stackFrame.GetFileName()}:{stackFrame.GetFileLineNumber()}" : null);
				textWriter.Write($"{value3}    {"\u001b[90m\u001b[22m"}at {FormatMethod(stackFrame.GetMethod())} [0x{stackFrame.GetILOffset():X}] {value4}");
				textWriter.WriteLine();
			}
		}
		FileLog.Write(logEntry.LogLevel, logEntry.Category, text, FileLog.CaptureStackTraces ? text2 : null);
		LogStream.Publish(logEntry.LogLevel, logEntry.Category, text, text2);
	}

	private string FormatMethod(MethodBase? method)
	{
		if (method == null)
		{
			return "<unknown-method>";
		}
		return $"{method.DeclaringType?.FullName}.{method.Name}({string.Join(", ", from p in method.GetParameters()
			select p.ParameterType.Name + " " + p.Name)})";
	}
}
