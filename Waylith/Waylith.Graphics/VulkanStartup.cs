using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Brutal.GlfwApi;
using Brutal.Logging;
using Brutal.Logging.Internal;
using Brutal.VulkanApi;
using Waylith.Logging;

namespace Waylith.Graphics;

public static class VulkanStartup
{
	private static readonly LogCategory Logger = LogCategory.Make("VulkanStartup");

	private const int MaxStage = 3;

	private const string MarkerFileName = "VulkanInit.pending";

	private const string StickyFileName = "VulkanFallbackStage.txt";

	private static nint _loaderHandle;

	private static bool _alternativeLoaderAvailable;

	private static string? _markerPath;

	private static string? _stickyPath;

	public static VulkanLoaderChoice LoaderChoice { get; set; }

	public static bool DisableImplicitLayers { get; set; }

	public static bool FallbackDisabled { get; set; }

	public static bool ResetFallback { get; set; }

	public static int Stage { get; private set; } = 1;

	public static string? StageReason { get; private set; }

	public static string ActiveLoaderSource { get; private set; } = "default search";

	public static string? ActiveLoaderPath { get; private set; }

	public static string? ActiveLoaderVersion { get; private set; }

	public static string? SystemLoaderVersion { get; private set; }

	public static string? BundledLoaderVersion { get; private set; }

	public static bool SystemLoaderMissing { get; private set; }

	public static bool? LoaderMatchesBinding { get; private set; }

	public static bool InitializationPending { get; private set; }

	[DllImport("libc")]
	private static extern int setenv(string name, string value, int overwrite);

	public static void Prepare(EngineApplicationOptions options)
	{
		if (options.Headless)
		{
			return;
		}
		string? path = FileLog.StartedRoot ?? AppContext.BaseDirectory;
		_markerPath = Path.Combine(path, "VulkanInit.pending");
		_stickyPath = Path.Combine(path, "VulkanFallbackStage.txt");
		if (ResetFallback)
		{
			TryDelete(_markerPath);
			TryDelete(_stickyPath);
		}
		_alternativeLoaderAvailable = OperatingSystem.IsWindows() && File.Exists(options.BundledVulkanLoaderPath) && File.Exists(Path.Combine(Environment.SystemDirectory, "vulkan-1.dll"));
		if (!FallbackDisabled)
		{
			ResolveStage();
		}
		if (DisableImplicitLayers)
		{
			if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("VK_LOADER_LAYERS_DISABLE")))
			{
				SetNativeEnvironmentVariable("VK_LOADER_LAYERS_DISABLE", "~implicit~");
			}
			Logger.Warning("Implicit Vulkan layers are disabled for this run (overlays and capture tools will not hook the game).", "Prepare", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Graphics\\VulkanStartup.cs", 78);
		}
		if (OperatingSystem.IsWindows())
		{
			SelectWindowsLoader(options.BundledVulkanLoaderPath);
		}
	}

	private static void ResolveStage()
	{
		int num = ReadStage(_stickyPath);
		if (num > 1)
		{
			Stage = num;
			StageReason = "a previous launch only booted at this stage, -ResetVulkanFallback clears it";
		}
		int num2 = ReadStage(_markerPath);
		if (num2 > 0)
		{
			Stage = System.Math.Min(System.Math.Max(num2, Stage) + 1, 3);
			StageReason = "the previous launch died inside Vulkan initialisation";
			if (num2 >= 3)
			{
				StageReason += " at the last stage";
			}
		}
		if (Stage == 2 && !_alternativeLoaderAvailable)
		{
			Stage = 3;
		}
		if (Stage == 1)
		{
			return;
		}
		LogCategory logger = Logger;
		bool shouldAppend;
		LogString<Warning> message = new LogString<Warning>(25, 2, logger, out shouldAppend);
		if (shouldAppend)
		{
			message.AppendLiteral("Vulkan fallback stage ");
			message.AppendFormatted(Stage);
			message.AppendLiteral(": ");
			message.AppendFormatted(StageReason);
			message.AppendLiteral(".");
		}
		logger.Warning(message, "ResolveStage", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Graphics\\VulkanStartup.cs", 109);
		if (Stage >= 3)
		{
			if (LoaderChoice == VulkanLoaderChoice.Newest)
			{
				LoaderChoice = VulkanLoaderChoice.Bundled;
			}
			DisableImplicitLayers = true;
		}
	}

	private static void SelectWindowsLoader(string? bundledPath)
	{
		string text = Path.Combine(Environment.SystemDirectory, "vulkan-1.dll");
		SystemLoaderMissing = !File.Exists(text);
		if (SystemLoaderMissing)
		{
			text = null;
		}
		bool shouldAppend;
		LogCategory logger;
		if (bundledPath != null && !File.Exists(bundledPath))
		{
			logger = Logger;
			LogCategory logCategory = logger;
			LogString<Warning> message = new LogString<Warning>(36, 1, logger, out shouldAppend);
			if (shouldAppend)
			{
				message.AppendLiteral("Bundled Vulkan loader missing at '");
				message.AppendFormatted(bundledPath);
				message.AppendLiteral("'.");
			}
			logCategory.Warning(message, "SelectWindowsLoader", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Graphics\\VulkanStartup.cs", 126);
			bundledPath = null;
		}
		Version version = FileVersion(text);
		Version version2 = FileVersion(bundledPath);
		SystemLoaderVersion = version?.ToString();
		BundledLoaderVersion = version2?.ToString();
		bool flag = bundledPath != null && (text == null || version2 >= (version ?? new Version(1, 0)));
		shouldAppend = LoaderChoice switch
		{
			VulkanLoaderChoice.Bundled => true, 
			VulkanLoaderChoice.System => false, 
			_ => (Stage == 2) ? (!flag) : flag, 
		};
		bool flag2 = shouldAppend;
		string text2 = (flag2 ? "bundled" : "system");
		string text3 = (flag2 ? bundledPath : text);
		if (text3 == null)
		{
			logger = Logger;
			LogCategory logCategory2 = logger;
			LogString<Warning> message = new LogString<Warning>(50, 1, logger, out shouldAppend);
			if (shouldAppend)
			{
				message.AppendLiteral("No ");
				message.AppendFormatted(text2);
				message.AppendLiteral(" Vulkan loader available, taking the other one.");
			}
			logCategory2.Warning(message, "SelectWindowsLoader", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Graphics\\VulkanStartup.cs", 147);
			if (!flag2)
			{
				string text4 = bundledPath;
				text3 = text4;
				text2 = "bundled";
			}
			else
			{
				string text4 = text;
				text3 = text4;
				text2 = "system";
			}
		}
		if (text3 == null)
		{
			Logger.Error("No Vulkan loader found: neither the bundled vulkan-1.dll nor one in System32.", "SelectWindowsLoader", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Graphics\\VulkanStartup.cs", 152);
			return;
		}
		LogString<Error> message2;
		try
		{
			_loaderHandle = NativeLibrary.Load(text3);
		}
		catch (Exception ex)
		{
			logger = Logger;
			LogCategory logCategory3 = logger;
			message2 = new LogString<Error>(38, 3, logger, out shouldAppend);
			if (shouldAppend)
			{
				message2.AppendLiteral("Failed to load the ");
				message2.AppendFormatted(text2);
				message2.AppendLiteral(" Vulkan loader '");
				message2.AppendFormatted(text3);
				message2.AppendLiteral("': ");
				message2.AppendFormatted(ex.Message);
			}
			logCategory3.Error(message2, "SelectWindowsLoader", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Graphics\\VulkanStartup.cs", 162);
			return;
		}
		ActiveLoaderSource = text2;
		ActiveLoaderPath = text3;
		ActiveLoaderVersion = (flag2 ? version2 : version)?.ToString();
		logger = Logger;
		LogCategory logCategory4 = logger;
		LogString<Information> message3 = new LogString<Information>(54, 5, logger, out shouldAppend);
		if (shouldAppend)
		{
			message3.AppendLiteral("Vulkan loader: ");
			message3.AppendFormatted(text2);
			message3.AppendLiteral(" '");
			message3.AppendFormatted(text3);
			message3.AppendLiteral("' (file version ");
			message3.AppendFormatted(ActiveLoaderVersion ?? "unknown");
			message3.AppendLiteral(", system ");
			message3.AppendFormatted(SystemLoaderVersion ?? "missing");
			message3.AppendLiteral(", bundled ");
			message3.AppendFormatted(BundledLoaderVersion ?? "missing");
			message3.AppendLiteral(").");
		}
		logCategory4.Info(message3, "SelectWindowsLoader", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Graphics\\VulkanStartup.cs", 169);
		if (NativeLibrary.TryGetExport(_loaderHandle, "vkGetInstanceProcAddr", out var address))
		{
			Glfw.PInvoke.InitVulkanLoader(address);
			return;
		}
		logger = Logger;
		LogCategory logCategory5 = logger;
		message2 = new LogString<Error>(78, 1, logger, out shouldAppend);
		if (shouldAppend)
		{
			message2.AppendLiteral("'");
			message2.AppendFormatted(text3);
			message2.AppendLiteral("' exports no vkGetInstanceProcAddr, GLFW will search for a loader on its own.");
		}
		logCategory5.Error(message2, "SelectWindowsLoader", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Graphics\\VulkanStartup.cs", 174);
	}

	public unsafe static void VerifyBinding()
	{
		if (_loaderHandle == 0)
		{
			return;
		}
		delegate* unmanaged[Cdecl]<VkInstance, byte*, PfnVkVoidFunction> invoke = Vulkan.GetInstanceProcAddr.Invoke;
		nint address;
		nint num = (NativeLibrary.TryGetExport(_loaderHandle, "vkGetInstanceProcAddr", out address) ? address : 0);
		LoaderMatchesBinding = invoke == (delegate* unmanaged[Cdecl]<VkInstance, byte*, PfnVkVoidFunction>)num;
		bool shouldAppend;
		LogCategory logger;
		if (LoaderMatchesBinding == true)
		{
			logger = Logger;
			LogCategory logCategory = logger;
			LogString<Information> message = new LogString<Information>(36, 1, logger, out shouldAppend);
			if (shouldAppend)
			{
				message.AppendLiteral("Vulkan binding is using the ");
				message.AppendFormatted(ActiveLoaderSource);
				message.AppendLiteral(" loader.");
			}
			logCategory.Info(message, "VerifyBinding", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Graphics\\VulkanStartup.cs", 187);
			return;
		}
		logger = Logger;
		LogCategory logCategory2 = logger;
		LogString<Error> message2 = new LogString<Error>(79, 2, logger, out shouldAppend);
		if (shouldAppend)
		{
			message2.AppendLiteral("Vulkan binding loaded a different vulkan-1.dll than the ");
			message2.AppendFormatted(ActiveLoaderSource);
			message2.AppendLiteral(" loader selected at '");
			message2.AppendFormatted(ActiveLoaderPath);
			message2.AppendLiteral("'.");
		}
		logCategory2.Error(message2, "VerifyBinding", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Graphics\\VulkanStartup.cs", 189);
	}

	public static void BeginInstanceCreation()
	{
		if (FallbackDisabled || _markerPath == null)
		{
			return;
		}
		try
		{
			File.WriteAllText(_markerPath, Stage.ToString());
			InitializationPending = true;
		}
		catch
		{
		}
	}

	public static void EndInstanceCreation()
	{
		if (!InitializationPending)
		{
			return;
		}
		InitializationPending = false;
		TryDelete(_markerPath);
		if (Stage <= 1 || ReadStage(_stickyPath) == Stage)
		{
			return;
		}
		try
		{
			File.WriteAllText(_stickyPath, Stage.ToString());
		}
		catch
		{
		}
	}

	private static Version? FileVersion(string? path)
	{
		if (path == null)
		{
			return null;
		}
		try
		{
			FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(path);
			return new Version(versionInfo.FileMajorPart, versionInfo.FileMinorPart, versionInfo.FileBuildPart, versionInfo.FilePrivatePart);
		}
		catch
		{
			return null;
		}
	}

	private static int ReadStage(string? path)
	{
		try
		{
			int result;
			return (path != null && File.Exists(path) && int.TryParse(File.ReadAllText(path).Trim(), out result)) ? result : 0;
		}
		catch
		{
			return 0;
		}
	}

	private static void TryDelete(string? path)
	{
		try
		{
			if (path != null)
			{
				File.Delete(path);
			}
		}
		catch
		{
		}
	}

	private static void SetNativeEnvironmentVariable(string name, string value)
	{
		if (OperatingSystem.IsWindows())
		{
			Environment.SetEnvironmentVariable(name, value);
		}
		else
		{
			setenv(name, value, 1);
		}
	}
}
