using System;
using System.IO;
using System.Linq;
using Brutal.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using UnityStubs.Internal;
using Waylith;
using Waylith.Audio.MiniAudio;
using Waylith.Logging;

namespace RimWorldWaylith;

internal static class Program
{
	private static int Main(string[] args)
	{
		WindowsConsole.OpenIfRequested(args);
		InitLogger(args);
		Environment.SetEnvironmentVariable("MONOMOD_DMDType", "dynamicmethod");
		ConfigDependentCommandLine commandLine = new ConfigDependentCommandLine();
		bool headless = HasSwitch("Headless");
		StubWarner.Suppressed = HasSwitch("SuppressUnityStubs");
		WaylithLogFormatter.EnableStackTraces = !HasSwitch("DisableLogStackTraces");
		Bootstrap.WatchShaders = HasSwitch("WatchShaders");
		FileLog.Disabled = HasSwitch("NoFileLog");
		FileLog.SetDirectory(Bootstrap.ResolveEarlyLogRoot());
		if (HasSwitch("NoFileLogCallstacks"))
		{
			FileLog.CaptureStackTraces = false;
		}
		if (HasSwitch("CrashHandler"))
		{
			CrashHandlerLauncher.TryLaunch(HasSwitch("CrashDumpMini") ? "mini" : (HasSwitch("CrashDumpHeap") ? "heap" : "full"), HasSwitch("CrashUI"), HasSwitch("CrashCaptureBreakpoints"));
		}
		int num = EngineApplication.Run(new EngineApplicationOptions
		{
			Title = "RimWorld by Ludeon Studios",
			WaylandAppId = "RimWorld by Ludeon Studios",
			Headless = headless,
			CommandLineAccessor = commandLine,
			EnableVideoDecode = true,
			DisallowFullscreenExclusive = HasSwitch("DisplayCaptureCompatibilityMode"),
			BundledVulkanLoaderPath = (OperatingSystem.IsWindows() ? Path.Combine(AppContext.BaseDirectory, "vulkan", "vulkan-1.dll") : null),
			ShowCrashDialog = !HasSwitch("NoCrashDialog"),
			CrashReportInstructions = "Please report the crash to Ludeon Studios and attach the newest report from that folder together with the game log.",
			AudioBackend = new MiniAudioBackend()
		}, Bootstrap.Initialize);
		Environment.Exit(num);
		return num;
		bool HasSwitch(string name)
		{
			return commandLine.Args.Any((string a) => a.Trim('-', '+').Equals(name, StringComparison.OrdinalIgnoreCase));
		}
	}

	private static void InitLogger(string[] args)
	{
		bool verbose = HasSwitch("Verbose");
		LogSystem.IsEnabled = true;
		LogSystem.Initialize(delegate(ILoggingBuilder builder)
		{
			builder.AddConsole(delegate(ConsoleLoggerOptions options)
			{
				options.FormatterName = "WaylithFormatter";
			}).AddConsoleFormatter<WaylithLogFormatter, ConsoleFormatterOptions>();
			builder.SetMinimumLevel((!verbose) ? LogLevel.Debug : LogLevel.Trace);
			builder.AddFilter(delegate(string? category, LogLevel level)
			{
				if (category == "Verbose")
				{
					return verbose;
				}
				return (category == "Noesis") ? (level >= LogLevel.Warning) : (level >= LogLevel.Debug);
			});
		});
		if (verbose)
		{
			VerboseLog.Log("Verbose logging enabled.");
		}
		bool HasSwitch(string name)
		{
			return args.Any((string a) => a.Trim('-', '+').Equals(name, StringComparison.OrdinalIgnoreCase));
		}
	}
}
