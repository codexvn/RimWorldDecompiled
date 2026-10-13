using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace RimWorldWaylith;

internal static class CrashHandlerLauncher
{
	private const string SidecarExe = "Waylith.CrashHandler.exe";

	public static void TryLaunch(string dumpType, bool showUi, bool captureBreakpoints)
	{
		if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
		{
			return;
		}
		try
		{
			string text = Path.Combine(Path.GetDirectoryName(Environment.ProcessPath), "Waylith.CrashHandler.exe");
			if (!File.Exists(text))
			{
				Console.Error.WriteLine("[CrashHandler] sidecar not found at '" + text + "'; native crash capture disabled.");
				return;
			}
			string text2 = ResolveDumpDir();
			Directory.CreateDirectory(text2);
			string text3 = $"Waylith.CrashHandler.Ready.{Environment.ProcessId}.{Guid.NewGuid():N}";
			using EventWaitHandle eventWaitHandle = new EventWaitHandle(initialState: false, EventResetMode.ManualReset, text3);
			ProcessStartInfo processStartInfo = new ProcessStartInfo
			{
				FileName = text,
				UseShellExecute = false,
				CreateNoWindow = true
			};
			processStartInfo.ArgumentList.Add("--pid");
			processStartInfo.ArgumentList.Add(Environment.ProcessId.ToString());
			processStartInfo.ArgumentList.Add("--dump-dir");
			processStartInfo.ArgumentList.Add(text2);
			processStartInfo.ArgumentList.Add("--dump-type");
			processStartInfo.ArgumentList.Add(dumpType);
			processStartInfo.ArgumentList.Add("--ready-event");
			processStartInfo.ArgumentList.Add(text3);
			processStartInfo.ArgumentList.Add("--log");
			processStartInfo.ArgumentList.Add(Path.Combine(text2, "crash-handler.log"));
			processStartInfo.ArgumentList.Add("--product");
			processStartInfo.ArgumentList.Add("RimWorld");
			if (showUi)
			{
				processStartInfo.ArgumentList.Add("--show-ui");
			}
			if (captureBreakpoints)
			{
				processStartInfo.ArgumentList.Add("--capture-breakpoints");
			}
			if (Process.Start(processStartInfo) == null)
			{
				Console.Error.WriteLine("[CrashHandler] failed to start sidecar.");
			}
			else if (eventWaitHandle.WaitOne(TimeSpan.FromSeconds(5L)))
			{
				Console.Error.WriteLine("[CrashHandler] attached; dumps -> " + text2);
			}
			else
			{
				Console.Error.WriteLine("[CrashHandler] sidecar did not signal attach within 5s; continuing anyway.");
			}
		}
		catch (Exception ex)
		{
			Console.Error.WriteLine("[CrashHandler] launch error: " + ex);
		}
	}

	private static string ResolveDumpDir()
	{
		return Path.Combine(Bootstrap.ResolveLogDataRoot(), "Crashes", "Dumps");
	}
}
