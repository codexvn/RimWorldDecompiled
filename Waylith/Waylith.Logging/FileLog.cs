using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Microsoft.Extensions.Logging;
using Waylith.Runtime;

namespace Waylith.Logging;

public static class FileLog
{
	public const string LogFileName = "Player.log";

	private const string PreviousLogFileName = "Player-prev.log";

	public const string LogSubfolder = "Logs";

	private const int MaxBacklogLines = 5000;

	private const int FlushIntervalMs = 1000;

	private static readonly Lock _lock = new Lock();

	[ThreadStatic]
	private static StringBuilder? _scratch;

	private static StreamWriter? _writer;

	private static string? _path;

	private static List<string>? _backlog = new List<string>();

	private static int _droppedBacklog;

	private static bool _started;

	private static bool _shutdown;

	private static Timer? _flushTimer;

	private static bool _processExitHooked;

	private static int _failSafeReported;

	public static string? StartedRoot { get; private set; }

	public static bool Disabled { get; set; }

	public static bool CaptureStackTraces { get; set; } = true;

	internal static bool IsCapturingStacks
	{
		get
		{
			if (!Disabled)
			{
				return CaptureStackTraces;
			}
			return false;
		}
	}

	public static void SetDirectory(string persistentDataPath)
	{
		if (Disabled || string.IsNullOrWhiteSpace(persistentDataPath))
		{
			return;
		}
		try
		{
			using (_lock.EnterScope())
			{
				if (!_started && !_shutdown && StartLocked(persistentDataPath))
				{
					_flushTimer = new Timer(delegate
					{
						Flush();
					}, null, 1000, 1000);
					HookProcessExit();
				}
			}
		}
		catch (Exception e)
		{
			FailSafe(e);
		}
	}

	internal static void Write(LogLevel level, string? category, string? message, string? stackText)
	{
		if (Disabled)
		{
			return;
		}
		try
		{
			string text = FormatEntry(level, category, message, stackText);
			using (_lock.EnterScope())
			{
				if (_shutdown)
				{
					return;
				}
				if (_writer != null)
				{
					_writer.Write(text);
					if (level >= LogLevel.Warning)
					{
						_writer.Flush();
					}
					return;
				}
				if (_backlog == null)
				{
					_backlog = new List<string>();
				}
				if (_backlog.Count < 5000)
				{
					_backlog.Add(text);
				}
				else
				{
					_droppedBacklog++;
				}
			}
		}
		catch (Exception e)
		{
			FailSafe(e);
		}
	}

	internal static void NoteCrashReport(Exception? exception, string? reportPath)
	{
		if (Disabled)
		{
			return;
		}
		try
		{
			StringBuilder stringBuilder = new StringBuilder(512);
			stringBuilder.Append("=== CRASH ===");
			if (!string.IsNullOrEmpty(reportPath))
			{
				stringBuilder.Append('\n').Append("Crash report written to: ").Append(reportPath);
			}
			if (exception != null)
			{
				stringBuilder.Append('\n').Append(exception);
			}
			Write(LogLevel.Critical, "Crash", stringBuilder.ToString(), null);
			Flush();
		}
		catch (Exception e)
		{
			FailSafe(e);
		}
	}

	internal static void CopyLogTo(TextWriter writer)
	{
		string path;
		using (_lock.EnterScope())
		{
			_writer?.Flush();
			path = _path;
			if (path == null)
			{
				List<string> backlog = _backlog;
				if (backlog == null || backlog.Count <= 0)
				{
					writer.WriteLine(Disabled ? "N/A (file logging disabled)" : "N/A (nothing logged yet)");
					return;
				}
				foreach (string item in _backlog)
				{
					writer.Write(item);
				}
				if (_droppedBacklog > 0)
				{
					writer.WriteLine($"({_droppedBacklog} earlier log line(s) were dropped, pre-init backlog overflow)");
				}
				return;
			}
		}
		using FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
		using StreamReader streamReader = new StreamReader(stream, Encoding.UTF8);
		char[] array = new char[16384];
		int count;
		while ((count = streamReader.Read(array, 0, array.Length)) > 0)
		{
			writer.Write(array, 0, count);
		}
	}

	private static void Flush()
	{
		if (Disabled)
		{
			return;
		}
		try
		{
			using (_lock.EnterScope())
			{
				_writer?.Flush();
			}
		}
		catch (Exception e)
		{
			FailSafe(e);
		}
	}

	public static void Shutdown()
	{
		try
		{
			using (_lock.EnterScope())
			{
				if (_shutdown)
				{
					return;
				}
				if (!_started)
				{
					List<string> backlog = _backlog;
					if (backlog != null && backlog.Count > 0)
					{
						StartLocked(ResolveFallbackDirectory());
					}
				}
				_shutdown = true;
				try
				{
					_flushTimer?.Dispose();
				}
				catch
				{
				}
				_flushTimer = null;
				if (_writer != null)
				{
					try
					{
						_writer.Write(FormatPlain(LogLevel.Information, "Log closed."));
						_writer.Flush();
						_writer.Dispose();
					}
					catch
					{
					}
					_writer = null;
				}
			}
		}
		catch (Exception e)
		{
			FailSafe(e);
		}
	}

	private static bool StartLocked(string persistentDataPath)
	{
		try
		{
			string text = Path.Combine(persistentDataPath, "Logs");
			Directory.CreateDirectory(text);
			string text2 = Path.Combine(text, "Player.log");
			string text3 = Path.Combine(text, "Player-prev.log");
			try
			{
				if (File.Exists(text2))
				{
					if (File.Exists(text3))
					{
						File.Delete(text3);
					}
					File.Move(text2, text3);
				}
			}
			catch
			{
			}
			_writer = new StreamWriter(new FileStream(text2, FileMode.Create, FileAccess.Write, FileShare.ReadWrite), new UTF8Encoding(encoderShouldEmitUTF8Identifier: false))
			{
				AutoFlush = false
			};
			_started = true;
			_path = text2;
			StartedRoot = persistentDataPath;
			WriteHeaderLocked(text2);
			if (_backlog != null)
			{
				foreach (string item in _backlog)
				{
					_writer.Write(item);
				}
				if (_droppedBacklog > 0)
				{
					_writer.Write(FormatPlain(LogLevel.Warning, $"{_droppedBacklog} earlier log line(s) were dropped (pre-init backlog overflow)."));
				}
				_backlog = null;
			}
			_writer.Flush();
			return true;
		}
		catch (Exception e)
		{
			FailSafe(e);
			return false;
		}
	}

	private static void WriteHeaderLocked(string path)
	{
		if (_writer != null)
		{
			WaylithAppState waylithAppState = SafeApp();
			_writer.Write("=== ");
			_writer.Write(waylithAppState?.ProductName ?? "Waylith");
			_writer.Write(" log ===");
			_writer.Write("\n");
			_writer.Write($"Started:   {DateTimeOffset.Now:yyyy-MM-dd HH:mm:ss.fff zzz}\n");
			_writer.Write("Version:   " + (waylithAppState?.Version ?? Assembly.GetEntryAssembly()?.GetName().Version?.ToString() ?? "unknown") + "\n");
			_writer.Write($"OS:        {RuntimeInformation.OSDescription} ({RuntimeInformation.OSArchitecture})\n");
			_writer.Write("Runtime:   " + RuntimeInformation.FrameworkDescription + "\n");
			_writer.Write($"Process:   pid {Environment.ProcessId}\n");
			_writer.Write("Log file:  " + path + "\n");
			try
			{
				_writer.Write("Command:   " + Environment.CommandLine + "\n");
			}
			catch
			{
			}
			_writer.Write("\n");
		}
	}

	private static string FormatEntry(LogLevel level, string? category, string? message, string? stackText)
	{
		StringBuilder stringBuilder = _scratch ?? (_scratch = new StringBuilder(256));
		stringBuilder.Clear();
		stringBuilder.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
		stringBuilder.Append(' ').Append(LevelName(level));
		if (!string.IsNullOrEmpty(category))
		{
			stringBuilder.Append(" [").Append(category).Append(']');
		}
		stringBuilder.Append(' ').Append(message ?? string.Empty);
		stringBuilder.Append('\n');
		stringBuilder.Append(stackText);
		return stringBuilder.ToString();
	}

	private static string FormatPlain(LogLevel level, string message)
	{
		return FormatEntry(level, null, message, null);
	}

	internal static string FormatStack(StackTrace stack)
	{
		StringBuilder stringBuilder = new StringBuilder(1024);
		StackFrame[] frames = stack.GetFrames();
		foreach (StackFrame stackFrame in frames)
		{
			stringBuilder.Append("    at ").Append(FormatMethod(stackFrame.GetMethod()));
			stringBuilder.Append(" [0x").Append(stackFrame.GetILOffset().ToString("X")).Append(']');
			if (stackFrame.HasSource())
			{
				stringBuilder.Append(" in ").Append(stackFrame.GetFileName()).Append(':')
					.Append(stackFrame.GetFileLineNumber());
			}
			stringBuilder.Append('\n');
		}
		return stringBuilder.ToString();
	}

	private static string FormatMethod(MethodBase? method)
	{
		if (method == null)
		{
			return "<unknown-method>";
		}
		StringBuilder stringBuilder = new StringBuilder(96);
		stringBuilder.Append(method.DeclaringType?.FullName).Append('.').Append(method.Name)
			.Append('(');
		ParameterInfo[] parameters = method.GetParameters();
		for (int i = 0; i < parameters.Length; i++)
		{
			if (i > 0)
			{
				stringBuilder.Append(", ");
			}
			stringBuilder.Append(parameters[i].ParameterType.Name).Append(' ').Append(parameters[i].Name);
		}
		stringBuilder.Append(')');
		return stringBuilder.ToString();
	}

	private static string LevelName(LogLevel level)
	{
		return level switch
		{
			LogLevel.Trace => "TRACE", 
			LogLevel.Debug => "DEBUG", 
			LogLevel.Information => " INFO", 
			LogLevel.Warning => " WARN", 
			LogLevel.Error => "ERROR", 
			LogLevel.Critical => " CRIT", 
			_ => "     ", 
		};
	}

	private static void HookProcessExit()
	{
		if (!_processExitHooked)
		{
			_processExitHooked = true;
			AppDomain.CurrentDomain.ProcessExit += delegate
			{
				Shutdown();
			};
		}
	}

	private static string ResolveFallbackDirectory()
	{
		string text = SafeApp()?.LogDataPath;
		if (string.IsNullOrWhiteSpace(text))
		{
			return AppContext.BaseDirectory;
		}
		return text;
	}

	private static WaylithAppState? SafeApp()
	{
		try
		{
			return WaylithRuntime.Current?.App;
		}
		catch
		{
			return null;
		}
	}

	private static void FailSafe(Exception e)
	{
		if (Interlocked.Exchange(ref _failSafeReported, 1) != 0)
		{
			return;
		}
		try
		{
			Console.Error.WriteLine($"[FileLog] disabling file logging after error: {e}");
		}
		catch
		{
		}
		Disabled = true;
		try
		{
			using (_lock.EnterScope())
			{
				try
				{
					_flushTimer?.Dispose();
				}
				catch
				{
				}
				_flushTimer = null;
				try
				{
					_writer?.Dispose();
				}
				catch
				{
				}
				_writer = null;
				_backlog = null;
			}
		}
		catch
		{
		}
	}
}
