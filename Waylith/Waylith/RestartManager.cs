using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Brutal.Logging;
using Brutal.Logging.Internal;
using Waylith.CLI;

namespace Waylith;

public static class RestartManager
{
	private sealed class HangupTolerantWriter(TextWriter inner) : TextWriter()
	{
		private TextWriter inner = inner;

		public override Encoding Encoding => inner.Encoding;

		public override void Write(char value)
		{
			Guarded(delegate(TextWriter w)
			{
				w.Write(value);
			});
		}

		public override void Write(string? value)
		{
			Guarded(delegate(TextWriter w)
			{
				w.Write(value);
			});
		}

		public override void WriteLine(string? value)
		{
			Guarded(delegate(TextWriter w)
			{
				w.WriteLine(value);
			});
		}

		public override void Flush()
		{
			Guarded(delegate(TextWriter w)
			{
				w.Flush();
			});
		}

		private void Guarded(Action<TextWriter> write)
		{
			try
			{
				write(inner);
			}
			catch (IOException)
			{
				inner = TextWriter.Null;
			}
		}
	}

	private const string WaitForPidSwitch = "-WaitForExitingPid";

	private const int WaitTimeoutMs = 15000;

	private static readonly LogCategory Log = LogCategory.Make("Restart");

	private static volatile bool _restartRequested;

	public static void RequestRestart()
	{
		_restartRequested = true;
		Log.Info("Restart requested; will relaunch after this instance shuts down.", "RequestRestart", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\RestartManager.cs", 22);
	}

	public static void WaitForPrecedingInstanceExit(ICommandLine commandLineAccessor)
	{
		try
		{
			int num = TryReadWaitPid(commandLineAccessor);
			if (num <= 0)
			{
				return;
			}
			Console.SetOut(new HangupTolerantWriter(Console.Out));
			Console.SetError(new HangupTolerantWriter(Console.Error));
			TryDetachFromParentSession();
			Console.Error.WriteLine($"[Restart] Waiting for previous instance (pid {num}) to exit...");
			try
			{
				if (!Process.GetProcessById(num).WaitForExit(15000))
				{
					Console.Error.WriteLine($"[Restart] Previous instance (pid {num}) still alive after {15000}ms; starting anyway.");
				}
			}
			catch (ArgumentException)
			{
			}
		}
		catch (Exception value)
		{
			Console.Error.WriteLine($"[Restart] Error while waiting for previous instance: {value}");
		}
	}

	public static void LaunchReplacementIfRequested(ICommandLine commandLineAccessor)
	{
		if (!_restartRequested)
		{
			return;
		}
		bool shouldAppend;
		try
		{
			ProcessStartInfo processStartInfo = BuildRestartStartInfo(commandLineAccessor);
			LogCategory log = Log;
			LogCategory logCategory = log;
			LogString<Information> message = new LogString<Information>(24, 2, log, out shouldAppend);
			if (shouldAppend)
			{
				message.AppendLiteral("Launching replacement: ");
				message.AppendFormatted(processStartInfo.FileName);
				message.AppendLiteral(" ");
				message.AppendFormatted(string.Join(' ', processStartInfo.ArgumentList));
			}
			logCategory.Info(message, "LaunchReplacementIfRequested", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\RestartManager.cs", 69);
			Process process = Process.Start(processStartInfo);
			log = Log;
			LogCategory logCategory2 = log;
			message = new LogString<Information>(28, 1, log, out shouldAppend);
			if (shouldAppend)
			{
				message.AppendLiteral("Replacement launched (pid ");
				message.AppendFormatted(process?.Id.ToString() ?? "unknown");
				message.AppendLiteral(").");
			}
			logCategory2.Info(message, "LaunchReplacementIfRequested", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\RestartManager.cs", 71);
		}
		catch (Exception value)
		{
			LogCategory log = Log;
			LogCategory logCategory3 = log;
			LogString<Error> message2 = new LogString<Error>(38, 1, log, out shouldAppend);
			if (shouldAppend)
			{
				message2.AppendLiteral("Failed to launch replacement process: ");
				message2.AppendFormatted(value);
			}
			logCategory3.Error(message2, "LaunchReplacementIfRequested", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\RestartManager.cs", 75);
		}
	}

	private static ProcessStartInfo BuildRestartStartInfo(ICommandLine commandLineAccessor)
	{
		string text = Environment.ProcessPath ?? throw new InvalidOperationException("Environment.ProcessPath is null; cannot restart.");
		ProcessStartInfo processStartInfo = new ProcessStartInfo
		{
			FileName = text,
			WorkingDirectory = Environment.CurrentDirectory,
			UseShellExecute = false
		};
		string[] args = commandLineAccessor.Args;
		for (int i = ((!Path.GetFileNameWithoutExtension(text).Equals("dotnet", StringComparison.OrdinalIgnoreCase)) ? 1 : 0); i < args.Length; i++)
		{
			if (!args[i].StartsWith("-WaitForExitingPid", StringComparison.OrdinalIgnoreCase))
			{
				processStartInfo.ArgumentList.Add(args[i]);
			}
		}
		processStartInfo.ArgumentList.Add($"{"-WaitForExitingPid"}={Environment.ProcessId}");
		return processStartInfo;
	}

	private static int TryReadWaitPid(ICommandLine commandLineAccessor)
	{
		string text = "-WaitForExitingPid=";
		string[] args = commandLineAccessor.Args;
		foreach (string text2 in args)
		{
			if (text2.StartsWith(text, StringComparison.OrdinalIgnoreCase) && int.TryParse(text2.AsSpan(text.Length), out var result))
			{
				return result;
			}
		}
		return 0;
	}

	[DllImport("libc", SetLastError = true)]
	private static extern int setsid();

	private static void TryDetachFromParentSession()
	{
		if (!OperatingSystem.IsLinux() && !OperatingSystem.IsMacOS())
		{
			return;
		}
		try
		{
			if (setsid() == -1)
			{
				Console.Error.WriteLine("[Restart] setsid() failed; continuing without detaching.");
			}
		}
		catch (Exception ex)
		{
			Console.Error.WriteLine("[Restart] setsid() unavailable: " + ex.Message);
		}
	}
}
