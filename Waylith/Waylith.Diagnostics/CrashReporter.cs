using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Loader;
using System.Text;
using System.Threading;
using Brutal;
using Brutal.GlfwApi;
using Brutal.Strings;
using Brutal.VulkanApi;
using Waylith.Graphics;
using Waylith.Jobs;
using Waylith.Logging;
using Waylith.Runtime;

namespace Waylith.Diagnostics;

internal static class CrashReporter
{
	private static int _reportWritten;

	private static string? _lastReportPath;

	private static string ProductNameOf(EngineApplication? app)
	{
		return SafeGet(() => app?.Runtime.App.ProductName) ?? "Waylith";
	}

	public static void WriteDiagnostics(TextWriter writer, EngineApplication? app, string context, Exception? exception = null)
	{
		ArgumentNullException.ThrowIfNull(writer, "writer");
		writer.Write(BuildReport(app, context, exception, ProductNameOf(app) + ((exception == null) ? " Diagnostics" : " Crash Report")));
	}

	public static string? TryWriteDiagnosticsReport(EngineApplication? app, string context)
	{
		return TryWriteSnapshot(app, context, null, "diagnostics", ProductNameOf(app) + " Diagnostics");
	}

	public static string? TryWriteReport(Exception? exception, EngineApplication? app, string context)
	{
		if (Interlocked.Exchange(ref _reportWritten, 1) != 0)
		{
			return _lastReportPath;
		}
		string text = (_lastReportPath = TryWriteSnapshot(app, context, exception, "crash", ProductNameOf(app) + " Crash Report"));
		try
		{
			FileLog.NoteCrashReport(exception, text);
		}
		catch
		{
		}
		return text;
	}

	private static string? TryWriteSnapshot(EngineApplication? app, string context, Exception? exception, string filePrefix, string title)
	{
		try
		{
			string text = ResolveCrashDirectory(app);
			Directory.CreateDirectory(text);
			string text2 = DateTimeOffset.Now.ToString("yyyyMMdd-HHmmss");
			string text3 = Path.Combine(text, filePrefix + "-" + text2 + ".txt");
			File.WriteAllText(text3, BuildReport(app, context, exception, title), Encoding.UTF8);
			return text3;
		}
		catch (Exception value)
		{
			try
			{
				Console.Error.WriteLine($"Failed to write diagnostic report: {value}");
			}
			catch
			{
			}
			return null;
		}
	}

	private static string BuildReport(EngineApplication? app, string context, Exception? exception, string title)
	{
		StringBuilder stringBuilder = new StringBuilder(65536);
		AppendHeader(stringBuilder, title, app, context);
		AppendException(stringBuilder, exception);
		AppendSystemInfo(stringBuilder);
		AppendProcessInfo(stringBuilder, app);
		AppendVulkanInfo(stringBuilder, app);
		AppendGpuMemoryInfo(stringBuilder);
		AppendThreadPoolInfo(stringBuilder);
		AppendThreadDump(stringBuilder);
		return stringBuilder.ToString();
	}

	private static string ResolveCrashDirectory(EngineApplication? app)
	{
		string text = null;
		try
		{
			text = app?.Runtime.App.LogDataPath;
		}
		catch
		{
		}
		if (!string.IsNullOrWhiteSpace(text))
		{
			return Path.Combine(text, "Crashes");
		}
		return Path.Combine(AppContext.BaseDirectory, "Crashes");
	}

	private static void AppendHeader(StringBuilder report, string title, EngineApplication? app, string context)
	{
		report.AppendLine(title);
		StringBuilder stringBuilder = report;
		StringBuilder stringBuilder2 = stringBuilder;
		StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(11, 1, stringBuilder);
		handler.AppendLiteral("Generated: ");
		handler.AppendFormatted(DateTimeOffset.Now, "O");
		stringBuilder2.AppendLine(ref handler);
		stringBuilder = report;
		StringBuilder stringBuilder3 = stringBuilder;
		handler = new StringBuilder.AppendInterpolatedStringHandler(9, 1, stringBuilder);
		handler.AppendLiteral("Context: ");
		handler.AppendFormatted(context);
		stringBuilder3.AppendLine(ref handler);
		stringBuilder = report;
		StringBuilder stringBuilder4 = stringBuilder;
		handler = new StringBuilder.AppendInterpolatedStringHandler(9, 1, stringBuilder);
		handler.AppendLiteral("Product: ");
		handler.AppendFormatted(ProductNameOf(app));
		stringBuilder4.AppendLine(ref handler);
		stringBuilder = report;
		StringBuilder stringBuilder5 = stringBuilder;
		handler = new StringBuilder.AppendInterpolatedStringHandler(9, 1, stringBuilder);
		handler.AppendLiteral("Version: ");
		handler.AppendFormatted(SafeGet(() => app?.Runtime.App.Version) ?? "N/A");
		stringBuilder5.AppendLine(ref handler);
		report.AppendLine();
	}

	private static void AppendException(StringBuilder report, Exception? exception)
	{
		report.AppendLine("== Exception ==");
		report.AppendLine(exception?.ToString() ?? "N/A");
		report.AppendLine();
	}

	private static void AppendSystemInfo(StringBuilder report)
	{
		report.AppendLine("== System ==");
		StringBuilder stringBuilder = report;
		StringBuilder stringBuilder2 = stringBuilder;
		StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(4, 1, stringBuilder);
		handler.AppendLiteral("OS: ");
		handler.AppendFormatted(SafeGet(() => RuntimeInformation.OSDescription) ?? "N/A");
		stringBuilder2.AppendLine(ref handler);
		stringBuilder = report;
		StringBuilder stringBuilder3 = stringBuilder;
		handler = new StringBuilder.AppendInterpolatedStringHandler(17, 1, stringBuilder);
		handler.AppendLiteral("OS Architecture: ");
		handler.AppendFormatted(SafeGet(() => RuntimeInformation.OSArchitecture.ToString()) ?? "N/A");
		stringBuilder3.AppendLine(ref handler);
		stringBuilder = report;
		StringBuilder stringBuilder4 = stringBuilder;
		handler = new StringBuilder.AppendInterpolatedStringHandler(22, 1, stringBuilder);
		handler.AppendLiteral("Process Architecture: ");
		handler.AppendFormatted(SafeGet(() => RuntimeInformation.ProcessArchitecture.ToString()) ?? "N/A");
		stringBuilder4.AppendLine(ref handler);
		stringBuilder = report;
		StringBuilder stringBuilder5 = stringBuilder;
		handler = new StringBuilder.AppendInterpolatedStringHandler(11, 1, stringBuilder);
		handler.AppendLiteral("Framework: ");
		handler.AppendFormatted(SafeGet(() => RuntimeInformation.FrameworkDescription) ?? "N/A");
		stringBuilder5.AppendLine(ref handler);
		stringBuilder = report;
		StringBuilder stringBuilder6 = stringBuilder;
		handler = new StringBuilder.AppendInterpolatedStringHandler(5, 1, stringBuilder);
		handler.AppendLiteral("CLR: ");
		handler.AppendFormatted(SafeGet(() => Environment.Version.ToString()) ?? "N/A");
		stringBuilder6.AppendLine(ref handler);
		stringBuilder = report;
		StringBuilder stringBuilder7 = stringBuilder;
		handler = new StringBuilder.AppendInterpolatedStringHandler(14, 1, stringBuilder);
		handler.AppendLiteral("Machine Name: ");
		handler.AppendFormatted(SafeGet(() => Environment.MachineName) ?? "N/A");
		stringBuilder7.AppendLine(ref handler);
		stringBuilder = report;
		StringBuilder stringBuilder8 = stringBuilder;
		handler = new StringBuilder.AppendInterpolatedStringHandler(17, 1, stringBuilder);
		handler.AppendLiteral("Processor Count: ");
		handler.AppendFormatted(SafeGet(() => Environment.ProcessorCount.ToString()) ?? "N/A");
		stringBuilder8.AppendLine(ref handler);
		stringBuilder = report;
		StringBuilder stringBuilder9 = stringBuilder;
		handler = new StringBuilder.AppendInterpolatedStringHandler(16, 1, stringBuilder);
		handler.AppendLiteral("64-bit Process: ");
		handler.AppendFormatted(SafeGet(() => Environment.Is64BitProcess.ToString()) ?? "N/A");
		stringBuilder9.AppendLine(ref handler);
		stringBuilder = report;
		StringBuilder stringBuilder10 = stringBuilder;
		handler = new StringBuilder.AppendInterpolatedStringHandler(19, 1, stringBuilder);
		handler.AppendLiteral("Current Directory: ");
		handler.AppendFormatted(SafeGet(() => Environment.CurrentDirectory) ?? "N/A");
		stringBuilder10.AppendLine(ref handler);
		stringBuilder = report;
		StringBuilder stringBuilder11 = stringBuilder;
		handler = new StringBuilder.AppendInterpolatedStringHandler(14, 1, stringBuilder);
		handler.AppendLiteral("Process Path: ");
		handler.AppendFormatted(SafeGet(() => Environment.ProcessPath) ?? "N/A");
		stringBuilder11.AppendLine(ref handler);
		stringBuilder = report;
		StringBuilder stringBuilder12 = stringBuilder;
		handler = new StringBuilder.AppendInterpolatedStringHandler(16, 1, stringBuilder);
		handler.AppendLiteral("Base Directory: ");
		handler.AppendFormatted(SafeGet(() => AppContext.BaseDirectory) ?? "N/A");
		stringBuilder12.AppendLine(ref handler);
		stringBuilder = report;
		StringBuilder stringBuilder13 = stringBuilder;
		handler = new StringBuilder.AppendInterpolatedStringHandler(18, 1, stringBuilder);
		handler.AppendLiteral("Unload Requested: ");
		handler.AppendFormatted(SafeGet(() => AssemblyLoadContext.Default.IsCollectible.ToString()) ?? "N/A");
		stringBuilder13.AppendLine(ref handler);
		report.AppendLine();
	}

	private static void AppendProcessInfo(StringBuilder report, EngineApplication? app)
	{
		report.AppendLine("== Process ==");
		SafeAppend(report, delegate
		{
			Process process = Process.GetCurrentProcess();
			try
			{
				StringBuilder stringBuilder = report;
				StringBuilder stringBuilder2 = stringBuilder;
				StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(5, 1, stringBuilder);
				handler.AppendLiteral("PID: ");
				handler.AppendFormatted(process.Id);
				stringBuilder2.AppendLine(ref handler);
				stringBuilder = report;
				StringBuilder stringBuilder3 = stringBuilder;
				handler = new StringBuilder.AppendInterpolatedStringHandler(12, 1, stringBuilder);
				handler.AppendLiteral("Start Time: ");
				handler.AppendFormatted(TryFormat(() => process.StartTime.ToString("O")));
				stringBuilder3.AppendLine(ref handler);
				stringBuilder = report;
				StringBuilder stringBuilder4 = stringBuilder;
				handler = new StringBuilder.AppendInterpolatedStringHandler(8, 1, stringBuilder);
				handler.AppendLiteral("Uptime: ");
				handler.AppendFormatted(TryFormat(() => (DateTime.Now - process.StartTime).ToString()));
				stringBuilder4.AppendLine(ref handler);
				stringBuilder = report;
				StringBuilder stringBuilder5 = stringBuilder;
				handler = new StringBuilder.AppendInterpolatedStringHandler(13, 1, stringBuilder);
				handler.AppendLiteral("Working Set: ");
				handler.AppendFormatted(TryFormat(() => FormatBytes(process.WorkingSet64)));
				stringBuilder5.AppendLine(ref handler);
				stringBuilder = report;
				StringBuilder stringBuilder6 = stringBuilder;
				handler = new StringBuilder.AppendInterpolatedStringHandler(16, 1, stringBuilder);
				handler.AppendLiteral("Private Memory: ");
				handler.AppendFormatted(TryFormat(() => FormatBytes(process.PrivateMemorySize64)));
				stringBuilder6.AppendLine(ref handler);
				stringBuilder = report;
				StringBuilder stringBuilder7 = stringBuilder;
				handler = new StringBuilder.AppendInterpolatedStringHandler(14, 1, stringBuilder);
				handler.AppendLiteral("Managed Heap: ");
				handler.AppendFormatted(TryFormat(() => FormatBytes(GC.GetTotalMemory(forceFullCollection: false))));
				stringBuilder7.AppendLine(ref handler);
				stringBuilder = report;
				StringBuilder stringBuilder8 = stringBuilder;
				handler = new StringBuilder.AppendInterpolatedStringHandler(14, 1, stringBuilder);
				handler.AppendLiteral("Thread Count: ");
				handler.AppendFormatted(TryFormat(() => process.Threads.Count.ToString()));
				stringBuilder8.AppendLine(ref handler);
			}
			finally
			{
				if (process != null)
				{
					((IDisposable)process).Dispose();
				}
			}
		});
		SafeAppend(report, delegate
		{
			GCMemoryInfo gCMemoryInfo = GC.GetGCMemoryInfo();
			StringBuilder stringBuilder9 = report;
			StringBuilder stringBuilder10 = stringBuilder9;
			StringBuilder.AppendInterpolatedStringHandler handler2 = new StringBuilder.AppendInterpolatedStringHandler(31, 1, stringBuilder9);
			handler2.AppendLiteral("GC High Memory Load Threshold: ");
			handler2.AppendFormatted(FormatBytes(gCMemoryInfo.HighMemoryLoadThresholdBytes));
			stringBuilder10.AppendLine(ref handler2);
			stringBuilder9 = report;
			StringBuilder stringBuilder11 = stringBuilder9;
			handler2 = new StringBuilder.AppendInterpolatedStringHandler(14, 1, stringBuilder9);
			handler2.AppendLiteral("GC Heap Size: ");
			handler2.AppendFormatted(FormatBytes(gCMemoryInfo.HeapSizeBytes));
			stringBuilder11.AppendLine(ref handler2);
			stringBuilder9 = report;
			StringBuilder stringBuilder12 = stringBuilder9;
			handler2 = new StringBuilder.AppendInterpolatedStringHandler(21, 1, stringBuilder9);
			handler2.AppendLiteral("GC Fragmented Bytes: ");
			handler2.AppendFormatted(FormatBytes(gCMemoryInfo.FragmentedBytes));
			stringBuilder12.AppendLine(ref handler2);
			stringBuilder9 = report;
			StringBuilder stringBuilder13 = stringBuilder9;
			handler2 = new StringBuilder.AppendInterpolatedStringHandler(27, 1, stringBuilder9);
			handler2.AppendLiteral("GC Total Available Memory: ");
			handler2.AppendFormatted(FormatBytes(gCMemoryInfo.TotalAvailableMemoryBytes));
			stringBuilder13.AppendLine(ref handler2);
		});
		report.AppendLine();
		report.AppendLine("== App ==");
		StringBuilder stringBuilder14 = report;
		StringBuilder stringBuilder15 = stringBuilder14;
		StringBuilder.AppendInterpolatedStringHandler handler3 = new StringBuilder.AppendInterpolatedStringHandler(20, 1, stringBuilder14);
		handler3.AppendLiteral("Shutdown Requested: ");
		handler3.AppendFormatted(SafeGet(() => app?.ShutdownRequestedReason) ?? "N/A");
		stringBuilder15.AppendLine(ref handler3);
		stringBuilder14 = report;
		StringBuilder stringBuilder16 = stringBuilder14;
		handler3 = new StringBuilder.AppendInterpolatedStringHandler(19, 1, stringBuilder14);
		handler3.AppendLiteral("Stopwatch Elapsed: ");
		handler3.AppendFormatted(SafeGet(() => app?.Elapsed.ToString()) ?? "N/A");
		stringBuilder16.AppendLine(ref handler3);
		stringBuilder14 = report;
		StringBuilder stringBuilder17 = stringBuilder14;
		handler3 = new StringBuilder.AppendInterpolatedStringHandler(15, 1, stringBuilder14);
		handler3.AppendLiteral("Options Title: ");
		handler3.AppendFormatted(SafeGet(() => app?.Options.Title) ?? "N/A");
		stringBuilder17.AppendLine(ref handler3);
		if (app != null)
		{
			SafeAppend(report, delegate
			{
				StringBuilder stringBuilder18 = report;
				StringBuilder stringBuilder19 = stringBuilder18;
				StringBuilder.AppendInterpolatedStringHandler handler4 = new StringBuilder.AppendInterpolatedStringHandler(13, 1, stringBuilder18);
				handler4.AppendLiteral("Frame Count: ");
				handler4.AppendFormatted(Time.FrameCount);
				stringBuilder19.AppendLine(ref handler4);
				stringBuilder18 = report;
				StringBuilder stringBuilder20 = stringBuilder18;
				handler4 = new StringBuilder.AppendInterpolatedStringHandler(6, 1, stringBuilder18);
				handler4.AppendLiteral("Time: ");
				handler4.AppendFormatted(Time.Elapsed);
				stringBuilder20.AppendLine(ref handler4);
				stringBuilder18 = report;
				StringBuilder stringBuilder21 = stringBuilder18;
				handler4 = new StringBuilder.AppendInterpolatedStringHandler(12, 1, stringBuilder18);
				handler4.AppendLiteral("Delta Time: ");
				handler4.AppendFormatted(Time.DeltaTime);
				stringBuilder21.AppendLine(ref handler4);
			});
			SafeAppend(report, delegate
			{
				GlfwWindow window = app.Window;
				StringBuilder stringBuilder22 = report;
				StringBuilder stringBuilder23 = stringBuilder22;
				StringBuilder.AppendInterpolatedStringHandler handler5 = new StringBuilder.AppendInterpolatedStringHandler(14, 1, stringBuilder22);
				handler5.AppendLiteral("Window Title: ");
				handler5.AppendFormatted(window.Title);
				stringBuilder23.AppendLine(ref handler5);
				stringBuilder22 = report;
				StringBuilder stringBuilder24 = stringBuilder22;
				handler5 = new StringBuilder.AppendInterpolatedStringHandler(20, 1, stringBuilder22);
				handler5.AppendLiteral("Window ShouldClose: ");
				handler5.AppendFormatted(window.ShouldClose);
				stringBuilder24.AppendLine(ref handler5);
			});
			Func<string>[] array = CrashDiagnostics.Snapshot();
			foreach (Func<string?> provider in array)
			{
				SafeAppend(report, delegate
				{
					string text = provider();
					if (text != null && text.Length > 0)
					{
						report.AppendLine(text);
					}
				});
			}
			SafeAppend(report, delegate
			{
				WaylithJobRuntimeDiagnosticsSnapshot diagnosticsSnapshot = WaylithJobRuntime.GetDiagnosticsSnapshot();
				StringBuilder stringBuilder25 = report;
				StringBuilder.AppendInterpolatedStringHandler handler6 = new StringBuilder.AppendInterpolatedStringHandler(69, 6, stringBuilder25);
				handler6.AppendLiteral("WaylithJobs: frame=");
				handler6.AppendFormatted(diagnosticsSnapshot.CurrentFrame);
				handler6.AppendLiteral(", total=");
				handler6.AppendFormatted(diagnosticsSnapshot.TotalHandles);
				handler6.AppendLiteral(", pending=");
				handler6.AppendFormatted(diagnosticsSnapshot.PendingHandles);
				handler6.AppendLiteral(", running=");
				handler6.AppendFormatted(diagnosticsSnapshot.RunningHandles);
				handler6.AppendLiteral(", completed=");
				handler6.AppendFormatted(diagnosticsSnapshot.CompletedHandles);
				handler6.AppendLiteral(", faulted=");
				handler6.AppendFormatted(diagnosticsSnapshot.FaultedHandles);
				stringBuilder25.AppendLine(ref handler6);
			});
			SafeAppend(report, delegate
			{
				NativeAllocationRuntimeDiagnosticsSnapshot diagnosticsSnapshot2 = NativeAllocationRuntime.GetDiagnosticsSnapshot();
				StringBuilder stringBuilder26 = report;
				StringBuilder stringBuilder27 = stringBuilder26;
				StringBuilder.AppendInterpolatedStringHandler handler7 = new StringBuilder.AppendInterpolatedStringHandler(41, 3, stringBuilder26);
				handler7.AppendLiteral("NativeAllocations: frame=");
				handler7.AppendFormatted(diagnosticsSnapshot2.CurrentFrame);
				handler7.AppendLiteral(", count=");
				handler7.AppendFormatted(diagnosticsSnapshot2.TotalCount);
				handler7.AppendLiteral(", bytes=");
				handler7.AppendFormatted(FormatBytes(diagnosticsSnapshot2.TotalBytes));
				stringBuilder27.AppendLine(ref handler7);
				for (int j = 0; j < diagnosticsSnapshot2.Buckets.Length; j++)
				{
					UnityAllocationBucketDiagnostics unityAllocationBucketDiagnostics = diagnosticsSnapshot2.Buckets[j];
					if (unityAllocationBucketDiagnostics.Count != 0)
					{
						stringBuilder26 = report;
						StringBuilder stringBuilder28 = stringBuilder26;
						handler7 = new StringBuilder.AppendInterpolatedStringHandler(18, 3, stringBuilder26);
						handler7.AppendLiteral("  ");
						handler7.AppendFormatted(unityAllocationBucketDiagnostics.Kind);
						handler7.AppendLiteral(": count=");
						handler7.AppendFormatted(unityAllocationBucketDiagnostics.Count);
						handler7.AppendLiteral(", bytes=");
						handler7.AppendFormatted(FormatBytes(unityAllocationBucketDiagnostics.Bytes));
						stringBuilder28.AppendLine(ref handler7);
					}
				}
			});
		}
		report.AppendLine();
	}

	private static void AppendVulkanInfo(StringBuilder report, EngineApplication? app)
	{
		report.AppendLine("== Vulkan ==");
		if (app == null)
		{
			report.AppendLine("N/A");
			report.AppendLine();
			return;
		}
		SafeAppend(report, delegate
		{
			AppendVulkanInstanceInfo(report, app);
		});
		SafeAppend(report, delegate
		{
			AppendVulkanDeviceInfo(report, app);
		});
		SafeAppend(report, delegate
		{
			AppendVulkanSurfaceInfo(report, app);
		});
		SafeAppend(report, delegate
		{
			AppendVulkanQueueInfo(report, app);
		});
		SafeAppend(report, delegate
		{
			AppendVulkanMemoryInfo(report, app);
		});
		SafeAppend(report, delegate
		{
			AppendVulkanToolInfo(report, app);
		});
		report.AppendLine();
	}

	private unsafe static void AppendVulkanInstanceInfo(StringBuilder report, EngineApplication app)
	{
		report.AppendLine("-- Instance --");
		StringBuilder stringBuilder = report;
		StringBuilder stringBuilder2 = stringBuilder;
		StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(17, 1, stringBuilder);
		handler.AppendLiteral("Instance Handle: ");
		handler.AppendFormatted(FormatHandle(app.VulkanInstance.VkHandle));
		stringBuilder2.AppendLine(ref handler);
		stringBuilder = report;
		StringBuilder stringBuilder3 = stringBuilder;
		handler = new StringBuilder.AppendInterpolatedStringHandler(23, 1, stringBuilder);
		handler.AppendLiteral("Requested API Version: ");
		handler.AppendFormatted(FormatApiVersion(4202496));
		stringBuilder3.AppendLine(ref handler);
		stringBuilder = report;
		StringBuilder stringBuilder4 = stringBuilder;
		handler = new StringBuilder.AppendInterpolatedStringHandler(20, 1, stringBuilder);
		handler.AppendLiteral("Validation Enabled: ");
		handler.AppendFormatted(app.GraphicsDevice?.ValidationEnabled.ToString() ?? "N/A");
		stringBuilder4.AppendLine(ref handler);
		stringBuilder = report;
		StringBuilder stringBuilder5 = stringBuilder;
		handler = new StringBuilder.AppendInterpolatedStringHandler(21, 1, stringBuilder);
		handler.AppendLiteral("Debug Utils Enabled: ");
		handler.AppendFormatted(app.VulkanInstance.IsExtensionEnabled("VK_EXT_debug_utils"));
		stringBuilder5.AppendLine(ref handler);
		SafeAppend(report, delegate
		{
			int apiVersion = 0;
			Vulkan.EnumerateInstanceVersion(&apiVersion);
			StringBuilder stringBuilder6 = report;
			StringBuilder.AppendInterpolatedStringHandler handler2 = new StringBuilder.AppendInterpolatedStringHandler(22, 1, stringBuilder6);
			handler2.AppendLiteral("Instance API Version: ");
			handler2.AppendFormatted(FormatApiVersion(apiVersion));
			stringBuilder6.AppendLine(ref handler2);
		});
		AppendCollection(report, "Enabled Instance Extensions", (IEnumerable<string>)(((object)app.GraphicsDevice?.InstanceEnabledExtensions) ?? ((object)Array.Empty<string>())));
		AppendCollection(report, "Enabled Instance Layers", (IEnumerable<string>)(((object)app.GraphicsDevice?.InstanceEnabledLayers) ?? ((object)Array.Empty<string>())));
		AppendCollection(report, "Available Instance Extensions", GetAvailableInstanceExtensionLines());
		AppendCollection(report, "Available Instance Layers", GetAvailableInstanceLayerLines());
	}

	private unsafe static void AppendVulkanDeviceInfo(StringBuilder report, EngineApplication app)
	{
		report.AppendLine("-- Device --");
		StringBuilder stringBuilder = report;
		StringBuilder stringBuilder2 = stringBuilder;
		StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(24, 1, stringBuilder);
		handler.AppendLiteral("Physical Device Handle: ");
		handler.AppendFormatted(FormatHandle(app.PhysicalDevice.VkHandle));
		stringBuilder2.AppendLine(ref handler);
		stringBuilder = report;
		StringBuilder stringBuilder3 = stringBuilder;
		handler = new StringBuilder.AppendInterpolatedStringHandler(23, 1, stringBuilder);
		handler.AppendLiteral("Logical Device Handle: ");
		handler.AppendFormatted(FormatHandle(app.Device.VkHandle));
		stringBuilder3.AppendLine(ref handler);
		VkPhysicalDeviceDriverProperties properties = new VkPhysicalDeviceDriverProperties();
		VkPhysicalDeviceVulkan11Properties properties2 = new VkPhysicalDeviceVulkan11Properties();
		VkPhysicalDeviceVulkan12Properties vkPhysicalDeviceVulkan12Properties = new VkPhysicalDeviceVulkan12Properties();
		properties.Next = &properties2;
		properties2.Next = &vkPhysicalDeviceVulkan12Properties;
		VkPhysicalDeviceProperties2 vkPhysicalDeviceProperties = new VkPhysicalDeviceProperties2();
		vkPhysicalDeviceProperties.Next = &properties;
		VkPhysicalDeviceProperties2 pProperties = vkPhysicalDeviceProperties;
		app.PhysicalDevice.GetProperties2(ref pProperties);
		VkPhysicalDeviceProperties properties3 = pProperties.Properties;
		stringBuilder = report;
		StringBuilder stringBuilder4 = stringBuilder;
		handler = new StringBuilder.AppendInterpolatedStringHandler(13, 1, stringBuilder);
		handler.AppendLiteral("Device Name: ");
		handler.AppendFormatted(GetDeviceName(properties3));
		stringBuilder4.AppendLine(ref handler);
		stringBuilder = report;
		StringBuilder stringBuilder5 = stringBuilder;
		handler = new StringBuilder.AppendInterpolatedStringHandler(13, 1, stringBuilder);
		handler.AppendLiteral("Device Type: ");
		handler.AppendFormatted(properties3.DeviceType);
		stringBuilder5.AppendLine(ref handler);
		stringBuilder = report;
		StringBuilder stringBuilder6 = stringBuilder;
		handler = new StringBuilder.AppendInterpolatedStringHandler(20, 1, stringBuilder);
		handler.AppendLiteral("Device API Version: ");
		handler.AppendFormatted(FormatApiVersion(properties3.ApiVersion));
		stringBuilder6.AppendLine(ref handler);
		stringBuilder = report;
		StringBuilder stringBuilder7 = stringBuilder;
		handler = new StringBuilder.AppendInterpolatedStringHandler(16, 1, stringBuilder);
		handler.AppendLiteral("Driver Version: ");
		handler.AppendFormatted(FormatVersionWithHex(properties3.DriverVersion));
		stringBuilder7.AppendLine(ref handler);
		stringBuilder = report;
		StringBuilder stringBuilder8 = stringBuilder;
		handler = new StringBuilder.AppendInterpolatedStringHandler(13, 1, stringBuilder);
		handler.AppendLiteral("Vendor ID: 0x");
		handler.AppendFormatted(properties3.VendorID, "X");
		stringBuilder8.AppendLine(ref handler);
		stringBuilder = report;
		StringBuilder stringBuilder9 = stringBuilder;
		handler = new StringBuilder.AppendInterpolatedStringHandler(13, 1, stringBuilder);
		handler.AppendLiteral("Device ID: 0x");
		handler.AppendFormatted(properties3.DeviceID, "X");
		stringBuilder9.AppendLine(ref handler);
		stringBuilder = report;
		StringBuilder stringBuilder10 = stringBuilder;
		handler = new StringBuilder.AppendInterpolatedStringHandler(21, 1, stringBuilder);
		handler.AppendLiteral("Pipeline Cache UUID: ");
		handler.AppendFormatted(GetPipelineCacheUuid(properties3));
		stringBuilder10.AppendLine(ref handler);
		stringBuilder = report;
		StringBuilder stringBuilder11 = stringBuilder;
		handler = new StringBuilder.AppendInterpolatedStringHandler(11, 1, stringBuilder);
		handler.AppendLiteral("Driver ID: ");
		handler.AppendFormatted(properties.DriverID);
		stringBuilder11.AppendLine(ref handler);
		stringBuilder = report;
		StringBuilder stringBuilder12 = stringBuilder;
		handler = new StringBuilder.AppendInterpolatedStringHandler(13, 1, stringBuilder);
		handler.AppendLiteral("Driver Name: ");
		handler.AppendFormatted(GetDriverName(properties));
		stringBuilder12.AppendLine(ref handler);
		stringBuilder = report;
		StringBuilder stringBuilder13 = stringBuilder;
		handler = new StringBuilder.AppendInterpolatedStringHandler(13, 1, stringBuilder);
		handler.AppendLiteral("Driver Info: ");
		handler.AppendFormatted(GetDriverInfo(properties));
		stringBuilder13.AppendLine(ref handler);
		stringBuilder = report;
		StringBuilder stringBuilder14 = stringBuilder;
		handler = new StringBuilder.AppendInterpolatedStringHandler(20, 1, stringBuilder);
		handler.AppendLiteral("Driver Conformance: ");
		handler.AppendFormatted(FormatConformanceVersion(properties.ConformanceVersion));
		stringBuilder14.AppendLine(ref handler);
		stringBuilder = report;
		StringBuilder stringBuilder15 = stringBuilder;
		handler = new StringBuilder.AppendInterpolatedStringHandler(13, 1, stringBuilder);
		handler.AppendLiteral("Device UUID: ");
		handler.AppendFormatted(GetDeviceUuid(properties2));
		stringBuilder15.AppendLine(ref handler);
		stringBuilder = report;
		StringBuilder stringBuilder16 = stringBuilder;
		handler = new StringBuilder.AppendInterpolatedStringHandler(13, 1, stringBuilder);
		handler.AppendLiteral("Driver UUID: ");
		handler.AppendFormatted(GetDriverUuid(properties2));
		stringBuilder16.AppendLine(ref handler);
		stringBuilder = report;
		StringBuilder stringBuilder17 = stringBuilder;
		handler = new StringBuilder.AppendInterpolatedStringHandler(19, 1, stringBuilder);
		handler.AppendLiteral("Device LUID Valid: ");
		handler.AppendFormatted(properties2.DeviceLUIDValid);
		stringBuilder17.AppendLine(ref handler);
		stringBuilder = report;
		StringBuilder stringBuilder18 = stringBuilder;
		handler = new StringBuilder.AppendInterpolatedStringHandler(13, 1, stringBuilder);
		handler.AppendLiteral("Device LUID: ");
		handler.AppendFormatted(properties2.DeviceLUIDValid ? GetDeviceLuid(properties2) : "N/A");
		stringBuilder18.AppendLine(ref handler);
		stringBuilder = report;
		StringBuilder stringBuilder19 = stringBuilder;
		handler = new StringBuilder.AppendInterpolatedStringHandler(24, 1, stringBuilder);
		handler.AppendLiteral("Vulkan 1.2 Conformance: ");
		handler.AppendFormatted(FormatConformanceVersion(vkPhysicalDeviceVulkan12Properties.ConformanceVersion));
		stringBuilder19.AppendLine(ref handler);
		stringBuilder = report;
		StringBuilder stringBuilder20 = stringBuilder;
		handler = new StringBuilder.AppendInterpolatedStringHandler(23, 1, stringBuilder);
		handler.AppendLiteral("Graphics Queue Family: ");
		handler.AppendFormatted(app.GraphicsDevice?.GraphicsFamilyIndex);
		stringBuilder20.AppendLine(ref handler);
		stringBuilder = report;
		StringBuilder stringBuilder21 = stringBuilder;
		handler = new StringBuilder.AppendInterpolatedStringHandler(22, 1, stringBuilder);
		handler.AppendLiteral("Present Queue Family: ");
		handler.AppendFormatted(app.GraphicsDevice?.PresentFamilyIndex);
		stringBuilder21.AppendLine(ref handler);
		stringBuilder = report;
		StringBuilder stringBuilder22 = stringBuilder;
		handler = new StringBuilder.AppendInterpolatedStringHandler(27, 1, stringBuilder);
		handler.AppendLiteral("Video Decode Queue Family: ");
		handler.AppendFormatted(app.GraphicsDevice?.VideoDecodeFamilyIndex);
		stringBuilder22.AppendLine(ref handler);
		stringBuilder = report;
		StringBuilder stringBuilder23 = stringBuilder;
		handler = new StringBuilder.AppendInterpolatedStringHandler(33, 1, stringBuilder);
		handler.AppendLiteral("Enabled Device Extensions Count: ");
		handler.AppendFormatted(app.GraphicsDevice?.DeviceEnabledExtensions.Count);
		stringBuilder23.AppendLine(ref handler);
		stringBuilder = report;
		StringBuilder stringBuilder24 = stringBuilder;
		handler = new StringBuilder.AppendInterpolatedStringHandler(31, 1, stringBuilder);
		handler.AppendLiteral("Core Feature FillModeNonSolid: ");
		handler.AppendFormatted(app.PhysicalDevice.GetFeatures().FillModeNonSolid);
		stringBuilder24.AppendLine(ref handler);
		stringBuilder = report;
		StringBuilder stringBuilder25 = stringBuilder;
		handler = new StringBuilder.AppendInterpolatedStringHandler(32, 1, stringBuilder);
		handler.AppendLiteral("Core Feature SamplerAnisotropy: ");
		handler.AppendFormatted(app.PhysicalDevice.GetFeatures().SamplerAnisotropy);
		stringBuilder25.AppendLine(ref handler);
		stringBuilder = report;
		StringBuilder stringBuilder26 = stringBuilder;
		handler = new StringBuilder.AppendInterpolatedStringHandler(39, 1, stringBuilder);
		handler.AppendLiteral("Limit MinUniformBufferOffsetAlignment: ");
		handler.AppendFormatted(properties3.Limits.MinUniformBufferOffsetAlignment);
		stringBuilder26.AppendLine(ref handler);
		stringBuilder = report;
		StringBuilder stringBuilder27 = stringBuilder;
		handler = new StringBuilder.AppendInterpolatedStringHandler(27, 1, stringBuilder);
		handler.AppendLiteral("Limit NonCoherentAtomSize: ");
		handler.AppendFormatted(properties3.Limits.NonCoherentAtomSize);
		stringBuilder27.AppendLine(ref handler);
		stringBuilder = report;
		StringBuilder stringBuilder28 = stringBuilder;
		handler = new StringBuilder.AppendInterpolatedStringHandler(23, 1, stringBuilder);
		handler.AppendLiteral("Limit TimestampPeriod: ");
		handler.AppendFormatted(properties3.Limits.TimestampPeriod);
		stringBuilder28.AppendLine(ref handler);
		stringBuilder = report;
		StringBuilder stringBuilder29 = stringBuilder;
		handler = new StringBuilder.AppendInterpolatedStringHandler(28, 1, stringBuilder);
		handler.AppendLiteral("Limit MaxPushConstantsSize: ");
		handler.AppendFormatted(properties3.Limits.MaxPushConstantsSize);
		stringBuilder29.AppendLine(ref handler);
		stringBuilder = report;
		StringBuilder stringBuilder30 = stringBuilder;
		handler = new StringBuilder.AppendInterpolatedStringHandler(30, 1, stringBuilder);
		handler.AppendLiteral("Limit MaxBoundDescriptorSets: ");
		handler.AppendFormatted(properties3.Limits.MaxBoundDescriptorSets);
		stringBuilder30.AppendLine(ref handler);
		stringBuilder = report;
		StringBuilder stringBuilder31 = stringBuilder;
		handler = new StringBuilder.AppendInterpolatedStringHandler(27, 1, stringBuilder);
		handler.AppendLiteral("Limit MaxImageDimension2D: ");
		handler.AppendFormatted(properties3.Limits.MaxImageDimension2D);
		stringBuilder31.AppendLine(ref handler);
		AppendCollection(report, "Enabled Device Extensions", (IEnumerable<string>)(((object)app.GraphicsDevice?.DeviceEnabledExtensions) ?? ((object)Array.Empty<string>())));
		AppendCollection(report, "Available Device Extensions", GetAvailableDeviceExtensionLines(app.PhysicalDevice));
	}

	private static void AppendVulkanSurfaceInfo(StringBuilder report, EngineApplication app)
	{
		report.AppendLine("-- Surface / Swapchain --");
		WindowRenderContext renderContext = app.RenderContext;
		if (renderContext == null)
		{
			report.AppendLine("No window render context.");
			return;
		}
		VkSurfaceCapabilitiesKHR surfaceCapabilitiesKHR = app.PhysicalDevice.GetSurfaceCapabilitiesKHR(renderContext.Surface);
		StringBuilder stringBuilder = report;
		StringBuilder stringBuilder2 = stringBuilder;
		StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(16, 1, stringBuilder);
		handler.AppendLiteral("Surface Handle: ");
		handler.AppendFormatted(FormatHandle(renderContext.Surface.VkHandle));
		stringBuilder2.AppendLine(ref handler);
		stringBuilder = report;
		StringBuilder stringBuilder3 = stringBuilder;
		handler = new StringBuilder.AppendInterpolatedStringHandler(18, 1, stringBuilder);
		handler.AppendLiteral("Swapchain Handle: ");
		handler.AppendFormatted(FormatHandle(renderContext.Swapchain.VkHandle));
		stringBuilder3.AppendLine(ref handler);
		stringBuilder = report;
		StringBuilder stringBuilder4 = stringBuilder;
		handler = new StringBuilder.AppendInterpolatedStringHandler(14, 1, stringBuilder);
		handler.AppendLiteral("Color Format: ");
		handler.AppendFormatted(renderContext.ColorFormat);
		stringBuilder4.AppendLine(ref handler);
		stringBuilder = report;
		StringBuilder stringBuilder5 = stringBuilder;
		handler = new StringBuilder.AppendInterpolatedStringHandler(14, 1, stringBuilder);
		handler.AppendLiteral("Depth Format: ");
		handler.AppendFormatted(renderContext.DepthFormat);
		stringBuilder5.AppendLine(ref handler);
		stringBuilder = report;
		StringBuilder stringBuilder6 = stringBuilder;
		handler = new StringBuilder.AppendInterpolatedStringHandler(14, 1, stringBuilder);
		handler.AppendLiteral("Sample Count: ");
		handler.AppendFormatted(renderContext.SampleCount);
		stringBuilder6.AppendLine(ref handler);
		stringBuilder = report;
		StringBuilder stringBuilder7 = stringBuilder;
		handler = new StringBuilder.AppendInterpolatedStringHandler(14, 1, stringBuilder);
		handler.AppendLiteral("Final Layout: ");
		handler.AppendFormatted(renderContext.FinalLayout);
		stringBuilder7.AppendLine(ref handler);
		stringBuilder = report;
		StringBuilder stringBuilder8 = stringBuilder;
		handler = new StringBuilder.AppendInterpolatedStringHandler(26, 2, stringBuilder);
		handler.AppendLiteral("Chosen Surface Format: ");
		handler.AppendFormatted(renderContext.SurfaceFormat.Format);
		handler.AppendLiteral(" / ");
		handler.AppendFormatted(renderContext.SurfaceFormat.ColorSpace);
		stringBuilder8.AppendLine(ref handler);
		stringBuilder = report;
		StringBuilder stringBuilder9 = stringBuilder;
		handler = new StringBuilder.AppendInterpolatedStringHandler(14, 1, stringBuilder);
		handler.AppendLiteral("Present Mode: ");
		handler.AppendFormatted(renderContext.PresentMode);
		stringBuilder9.AppendLine(ref handler);
		stringBuilder = report;
		StringBuilder stringBuilder10 = stringBuilder;
		handler = new StringBuilder.AppendInterpolatedStringHandler(9, 2, stringBuilder);
		handler.AppendLiteral("Extent: ");
		handler.AppendFormatted(renderContext.Extent.Width);
		handler.AppendLiteral("x");
		handler.AppendFormatted(renderContext.Extent.Height);
		stringBuilder10.AppendLine(ref handler);
		stringBuilder = report;
		StringBuilder stringBuilder11 = stringBuilder;
		handler = new StringBuilder.AppendInterpolatedStringHandler(13, 1, stringBuilder);
		handler.AppendLiteral("Frame Index: ");
		handler.AppendFormatted(renderContext.FrameIndex);
		stringBuilder11.AppendLine(ref handler);
		stringBuilder = report;
		StringBuilder stringBuilder12 = stringBuilder;
		handler = new StringBuilder.AppendInterpolatedStringHandler(23, 1, stringBuilder);
		handler.AppendLiteral("Swapchain Image Count: ");
		handler.AppendFormatted(renderContext.ImageCount);
		stringBuilder12.AppendLine(ref handler);
		stringBuilder = report;
		StringBuilder stringBuilder13 = stringBuilder;
		handler = new StringBuilder.AppendInterpolatedStringHandler(18, 1, stringBuilder);
		handler.AppendLiteral("Frames In Flight: ");
		handler.AppendFormatted(renderContext.FramesInFlight);
		stringBuilder13.AppendLine(ref handler);
		stringBuilder = report;
		StringBuilder stringBuilder14 = stringBuilder;
		handler = new StringBuilder.AppendInterpolatedStringHandler(23, 1, stringBuilder);
		handler.AppendLiteral("Framebuffer Minimized: ");
		handler.AppendFormatted(renderContext.IsFramebufferMinimized());
		stringBuilder14.AppendLine(ref handler);
		stringBuilder = report;
		StringBuilder stringBuilder15 = stringBuilder;
		handler = new StringBuilder.AppendInterpolatedStringHandler(27, 1, stringBuilder);
		handler.AppendLiteral("Framebuffer Size Mismatch: ");
		handler.AppendFormatted(renderContext.HasFramebufferSizeMismatch());
		stringBuilder15.AppendLine(ref handler);
		stringBuilder = report;
		StringBuilder stringBuilder16 = stringBuilder;
		handler = new StringBuilder.AppendInterpolatedStringHandler(30, 2, stringBuilder);
		handler.AppendLiteral("Surface Min/Max Image Count: ");
		handler.AppendFormatted(surfaceCapabilitiesKHR.MinImageCount);
		handler.AppendLiteral("/");
		handler.AppendFormatted(FormatUnboundedCount(surfaceCapabilitiesKHR.MaxImageCount));
		stringBuilder16.AppendLine(ref handler);
		stringBuilder = report;
		StringBuilder stringBuilder17 = stringBuilder;
		handler = new StringBuilder.AppendInterpolatedStringHandler(24, 1, stringBuilder);
		handler.AppendLiteral("Surface Current Extent: ");
		handler.AppendFormatted(FormatExtent(surfaceCapabilitiesKHR.CurrentExtent));
		stringBuilder17.AppendLine(ref handler);
		stringBuilder = report;
		StringBuilder stringBuilder18 = stringBuilder;
		handler = new StringBuilder.AppendInterpolatedStringHandler(20, 1, stringBuilder);
		handler.AppendLiteral("Surface Min Extent: ");
		handler.AppendFormatted(FormatExtent(surfaceCapabilitiesKHR.MinImageExtent));
		stringBuilder18.AppendLine(ref handler);
		stringBuilder = report;
		StringBuilder stringBuilder19 = stringBuilder;
		handler = new StringBuilder.AppendInterpolatedStringHandler(20, 1, stringBuilder);
		handler.AppendLiteral("Surface Max Extent: ");
		handler.AppendFormatted(FormatExtent(surfaceCapabilitiesKHR.MaxImageExtent));
		stringBuilder19.AppendLine(ref handler);
		stringBuilder = report;
		StringBuilder stringBuilder20 = stringBuilder;
		handler = new StringBuilder.AppendInterpolatedStringHandler(32, 1, stringBuilder);
		handler.AppendLiteral("Surface Max Image Array Layers: ");
		handler.AppendFormatted(surfaceCapabilitiesKHR.MaxImageArrayLayers);
		stringBuilder20.AppendLine(ref handler);
		stringBuilder = report;
		StringBuilder stringBuilder21 = stringBuilder;
		handler = new StringBuilder.AppendInterpolatedStringHandler(25, 1, stringBuilder);
		handler.AppendLiteral("Surface Supported Usage: ");
		handler.AppendFormatted(surfaceCapabilitiesKHR.SupportedUsageFlags);
		stringBuilder21.AppendLine(ref handler);
		stringBuilder = report;
		StringBuilder stringBuilder22 = stringBuilder;
		handler = new StringBuilder.AppendInterpolatedStringHandler(30, 1, stringBuilder);
		handler.AppendLiteral("Surface Supported Transforms: ");
		handler.AppendFormatted(surfaceCapabilitiesKHR.SupportedTransforms);
		stringBuilder22.AppendLine(ref handler);
		stringBuilder = report;
		StringBuilder stringBuilder23 = stringBuilder;
		handler = new StringBuilder.AppendInterpolatedStringHandler(27, 1, stringBuilder);
		handler.AppendLiteral("Surface Current Transform: ");
		handler.AppendFormatted(surfaceCapabilitiesKHR.CurrentTransform);
		stringBuilder23.AppendLine(ref handler);
		stringBuilder = report;
		StringBuilder stringBuilder24 = stringBuilder;
		handler = new StringBuilder.AppendInterpolatedStringHandler(35, 1, stringBuilder);
		handler.AppendLiteral("Surface Supported Composite Alpha: ");
		handler.AppendFormatted(surfaceCapabilitiesKHR.SupportedCompositeAlpha);
		stringBuilder24.AppendLine(ref handler);
		AppendCollection(report, "Supported Surface Formats", GetSurfaceFormatLines(app.PhysicalDevice, renderContext.Surface));
		AppendCollection(report, "Supported Present Modes", GetPresentModeLines(app.PhysicalDevice, renderContext.Surface));
	}

	private static void AppendVulkanQueueInfo(StringBuilder report, EngineApplication app)
	{
		report.AppendLine("-- Queues --");
		StringBuilder stringBuilder = report;
		StringBuilder stringBuilder2 = stringBuilder;
		StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(23, 1, stringBuilder);
		handler.AppendLiteral("Graphics Queue Handle: ");
		handler.AppendFormatted(FormatHandle(app.GraphicsQueue.VkHandle));
		stringBuilder2.AppendLine(ref handler);
		stringBuilder = report;
		StringBuilder stringBuilder3 = stringBuilder;
		handler = new StringBuilder.AppendInterpolatedStringHandler(30, 2, stringBuilder);
		handler.AppendLiteral("Graphics Queue Family/Index: ");
		handler.AppendFormatted(app.GraphicsQueue.Family);
		handler.AppendLiteral("/");
		handler.AppendFormatted(app.GraphicsQueue.Index);
		stringBuilder3.AppendLine(ref handler);
		stringBuilder = report;
		StringBuilder stringBuilder4 = stringBuilder;
		handler = new StringBuilder.AppendInterpolatedStringHandler(22, 1, stringBuilder);
		handler.AppendLiteral("Present Queue Handle: ");
		handler.AppendFormatted(FormatHandle(app.PresentQueue.VkHandle));
		stringBuilder4.AppendLine(ref handler);
		stringBuilder = report;
		StringBuilder stringBuilder5 = stringBuilder;
		handler = new StringBuilder.AppendInterpolatedStringHandler(29, 2, stringBuilder);
		handler.AppendLiteral("Present Queue Family/Index: ");
		handler.AppendFormatted(app.PresentQueue.Family);
		handler.AppendLiteral("/");
		handler.AppendFormatted(app.PresentQueue.Index);
		stringBuilder5.AppendLine(ref handler);
		Span<VkQueueFamilyProperties> span = stackalloc VkQueueFamilyProperties[app.PhysicalDevice.GetQueueFamilyProperties_Count()];
		app.PhysicalDevice.GetQueueFamilyProperties_Write(span);
		VkSurfaceKHR? vkSurfaceKHR = app.RenderContext?.Surface;
		for (int i = 0; i < span.Length; i++)
		{
			VkQueueFamilyProperties vkQueueFamilyProperties = span[i];
			bool value = vkSurfaceKHR.HasValue && (bool)app.PhysicalDevice.GetSurfaceSupportKHR(i, vkSurfaceKHR.Value);
			stringBuilder = report;
			StringBuilder stringBuilder6 = stringBuilder;
			handler = new StringBuilder.AppendInterpolatedStringHandler(79, 8, stringBuilder);
			handler.AppendLiteral("  Family[");
			handler.AppendFormatted(i);
			handler.AppendLiteral("]: count=");
			handler.AppendFormatted(vkQueueFamilyProperties.QueueCount);
			handler.AppendLiteral(", flags=");
			handler.AppendFormatted(vkQueueFamilyProperties.QueueFlags);
			handler.AppendLiteral(", present=");
			handler.AppendFormatted(value);
			handler.AppendLiteral(", timestampBits=");
			handler.AppendFormatted(vkQueueFamilyProperties.TimestampValidBits);
			handler.AppendLiteral(", minTransferGranularity=");
			handler.AppendFormatted(vkQueueFamilyProperties.MinImageTransferGranularity.Width);
			handler.AppendLiteral("x");
			handler.AppendFormatted(vkQueueFamilyProperties.MinImageTransferGranularity.Height);
			handler.AppendLiteral("x");
			handler.AppendFormatted(vkQueueFamilyProperties.MinImageTransferGranularity.Depth);
			stringBuilder6.AppendLine(ref handler);
		}
	}

	private unsafe static void AppendVulkanMemoryInfo(StringBuilder report, EngineApplication app)
	{
		report.AppendLine("-- Memory --");
		VkPhysicalDeviceMemoryBudgetPropertiesEXT vkPhysicalDeviceMemoryBudgetPropertiesEXT = new VkPhysicalDeviceMemoryBudgetPropertiesEXT();
		VkPhysicalDeviceMemoryProperties2 pMemoryProperties = new VkPhysicalDeviceMemoryProperties2();
		bool flag = app.PhysicalDevice.SupportsExtension("VK_EXT_memory_budget");
		if (flag)
		{
			pMemoryProperties.Next = &vkPhysicalDeviceMemoryBudgetPropertiesEXT;
		}
		app.PhysicalDevice.GetMemoryProperties2(ref pMemoryProperties);
		VkPhysicalDeviceMemoryProperties memoryProperties = pMemoryProperties.MemoryProperties;
		StringBuilder stringBuilder = report;
		StringBuilder stringBuilder2 = stringBuilder;
		StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(14, 1, stringBuilder);
		handler.AppendLiteral("Memory Heaps: ");
		handler.AppendFormatted(memoryProperties.MemoryHeapCount);
		stringBuilder2.AppendLine(ref handler);
		stringBuilder = report;
		StringBuilder stringBuilder3 = stringBuilder;
		handler = new StringBuilder.AppendInterpolatedStringHandler(14, 1, stringBuilder);
		handler.AppendLiteral("Memory Types: ");
		handler.AppendFormatted(memoryProperties.MemoryTypeCount);
		stringBuilder3.AppendLine(ref handler);
		stringBuilder = report;
		StringBuilder stringBuilder4 = stringBuilder;
		handler = new StringBuilder.AppendInterpolatedStringHandler(35, 1, stringBuilder);
		handler.AppendLiteral("Memory Budget Extension Supported: ");
		handler.AppendFormatted(flag);
		stringBuilder4.AppendLine(ref handler);
		for (int i = 0; i < memoryProperties.MemoryHeapCount; i++)
		{
			VkMemoryHeap vkMemoryHeap = global::_003CPrivateImplementationDetails_003E.InlineArrayAsSpan<VkMemoryHeap_16, VkMemoryHeap>(ref memoryProperties.MemoryHeaps, 16)[i];
			string text = $"  Heap[{i}]: size={FormatByteSize64(vkMemoryHeap.Size)}, flags={vkMemoryHeap.Flags}";
			if (flag)
			{
				text = text + ", budget=" + FormatByteSize64(global::_003CPrivateImplementationDetails_003E.InlineArrayAsSpan<ByteSize64_16, ByteSize64>(ref vkPhysicalDeviceMemoryBudgetPropertiesEXT.HeapBudget, 16)[i]) + ", usage=" + FormatByteSize64(global::_003CPrivateImplementationDetails_003E.InlineArrayAsSpan<ByteSize64_16, ByteSize64>(ref vkPhysicalDeviceMemoryBudgetPropertiesEXT.HeapUsage, 16)[i]);
			}
			report.AppendLine(text);
		}
		for (int j = 0; j < memoryProperties.MemoryTypeCount; j++)
		{
			VkMemoryType vkMemoryType = global::_003CPrivateImplementationDetails_003E.InlineArrayAsSpan<VkMemoryType_32, VkMemoryType>(ref memoryProperties.MemoryTypes, 32)[j];
			stringBuilder = report;
			StringBuilder stringBuilder5 = stringBuilder;
			handler = new StringBuilder.AppendInterpolatedStringHandler(23, 3, stringBuilder);
			handler.AppendLiteral("  Type[");
			handler.AppendFormatted(j);
			handler.AppendLiteral("]: heap=");
			handler.AppendFormatted(vkMemoryType.HeapIndex);
			handler.AppendLiteral(", flags=");
			handler.AppendFormatted(vkMemoryType.PropertyFlags);
			stringBuilder5.AppendLine(ref handler);
		}
	}

	private static void AppendVulkanToolInfo(StringBuilder report, EngineApplication app)
	{
		report.AppendLine("-- Active Tools --");
		VkPhysicalDeviceProperties properties = app.PhysicalDevice.GetProperties();
		if (!SupportsToolProperties(app.PhysicalDevice, properties.ApiVersion))
		{
			report.AppendLine("Tool properties are not supported by this runtime.");
		}
		else
		{
			AppendCollection(report, "Tool Properties", GetToolLines(app.PhysicalDevice));
		}
	}

	private static void AppendGpuMemoryInfo(StringBuilder report)
	{
		report.AppendLine("== GPU Memory ==");
		SafeAppend(report, delegate
		{
			report.Append(GpuMemoryReport.BuildReport(includeVmaJson: false));
		});
	}

	private static void AppendThreadPoolInfo(StringBuilder report)
	{
		report.AppendLine("== ThreadPool ==");
		SafeAppend(report, delegate
		{
			ThreadPool.GetAvailableThreads(out var workerThreads, out var completionPortThreads);
			ThreadPool.GetMaxThreads(out var workerThreads2, out var completionPortThreads2);
			ThreadPool.GetMinThreads(out var workerThreads3, out var completionPortThreads3);
			StringBuilder stringBuilder = report;
			StringBuilder stringBuilder2 = stringBuilder;
			StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(38, 3, stringBuilder);
			handler.AppendLiteral("Worker Threads: available=");
			handler.AppendFormatted(workerThreads);
			handler.AppendLiteral(", min=");
			handler.AppendFormatted(workerThreads3);
			handler.AppendLiteral(", max=");
			handler.AppendFormatted(workerThreads2);
			stringBuilder2.AppendLine(ref handler);
			stringBuilder = report;
			StringBuilder stringBuilder3 = stringBuilder;
			handler = new StringBuilder.AppendInterpolatedStringHandler(34, 3, stringBuilder);
			handler.AppendLiteral("IO Threads: available=");
			handler.AppendFormatted(completionPortThreads);
			handler.AppendLiteral(", min=");
			handler.AppendFormatted(completionPortThreads3);
			handler.AppendLiteral(", max=");
			handler.AppendFormatted(completionPortThreads2);
			stringBuilder3.AppendLine(ref handler);
		});
		report.AppendLine();
	}

	private static void AppendThreadDump(StringBuilder report)
	{
		report.AppendLine("== Threads ==");
		SafeAppend(report, delegate
		{
			using Process process = Process.GetCurrentProcess();
			foreach (ProcessThread thread in process.Threads)
			{
				report.Append("Thread ");
				report.Append(thread.Id);
				report.Append(": state=");
				report.Append(TryFormat(() => thread.ThreadState.ToString()));
				report.Append(", wait=");
				report.Append(TryFormat(() => thread.WaitReason.ToString()));
				if (OperatingSystem.IsWindows() || OperatingSystem.IsLinux())
				{
					report.Append(", start=");
					report.Append(TryFormat(() => thread.StartTime.ToString("O")));
				}
				report.AppendLine();
			}
		});
		report.AppendLine();
	}

	private static List<string> GetAvailableInstanceExtensionLines()
	{
		Span<VkExtensionProperties> span = stackalloc VkExtensionProperties[Vulkan.EnumerateInstanceExtensionProperties_Count(default(ReadOnlySpan<byte>))];
		Vulkan.EnumerateInstanceExtensionProperties_Write(default(ReadOnlySpan<byte>), span);
		return FormatExtensionLines(span);
	}

	private static List<string> GetAvailableInstanceLayerLines()
	{
		int num = Vulkan.EnumerateInstanceLayerProperties_Count();
		Span<VkLayerProperties> span = stackalloc VkLayerProperties[num];
		Vulkan.EnumerateInstanceLayerProperties_Write(span);
		List<string> list = new List<string>(num);
		for (int i = 0; i < span.Length; i++)
		{
			VkLayerProperties layer = span[i];
			list.Add($"{GetLayerName(layer)} (spec={FormatVersionWithHex(layer.SpecVersion)}, impl={FormatVersionWithHex(layer.ImplementationVersion)}) - {GetLayerDescription(layer)}");
		}
		list.Sort(StringComparer.Ordinal);
		return list;
	}

	private static List<string> GetAvailableDeviceExtensionLines(PhysicalDevice physicalDevice)
	{
		Span<VkExtensionProperties> span = stackalloc VkExtensionProperties[physicalDevice.EnumerateDeviceExtensionProperties_Count(default(ReadOnlySpan<byte>))];
		physicalDevice.EnumerateDeviceExtensionProperties_Write(default(ReadOnlySpan<byte>), span);
		return FormatExtensionLines(span);
	}

	private static List<string> GetSurfaceFormatLines(PhysicalDevice physicalDevice, VkSurfaceKHR surface)
	{
		int surfaceFormatsKHR_Count = physicalDevice.GetSurfaceFormatsKHR_Count(surface);
		Span<VkSurfaceFormatKHR> span = stackalloc VkSurfaceFormatKHR[surfaceFormatsKHR_Count];
		physicalDevice.GetSurfaceFormatsKHR_Write(surface, span);
		List<string> list = new List<string>(surfaceFormatsKHR_Count);
		for (int i = 0; i < span.Length; i++)
		{
			list.Add($"{span[i].Format} / {span[i].ColorSpace}");
		}
		list.Sort(StringComparer.Ordinal);
		return list;
	}

	private static List<string> GetPresentModeLines(PhysicalDevice physicalDevice, VkSurfaceKHR surface)
	{
		int surfacePresentModesKHR_Count = physicalDevice.GetSurfacePresentModesKHR_Count(surface);
		Span<VkPresentModeKHR> span = stackalloc VkPresentModeKHR[surfacePresentModesKHR_Count];
		physicalDevice.GetSurfacePresentModesKHR_Write(surface, span);
		List<string> list = new List<string>(surfacePresentModesKHR_Count);
		for (int i = 0; i < span.Length; i++)
		{
			list.Add(span[i].ToString());
		}
		list.Sort(StringComparer.Ordinal);
		return list;
	}

	private static List<string> GetToolLines(PhysicalDevice physicalDevice)
	{
		int toolProperties_Count = physicalDevice.GetToolProperties_Count();
		Span<VkPhysicalDeviceToolProperties> span = stackalloc VkPhysicalDeviceToolProperties[toolProperties_Count];
		physicalDevice.GetToolProperties_Write(span);
		List<string> list = new List<string>(toolProperties_Count);
		for (int i = 0; i < span.Length; i++)
		{
			VkPhysicalDeviceToolProperties properties = span[i];
			list.Add($"{GetToolName(properties)} {GetToolVersion(properties)}: purposes={properties.Purposes}, layer={GetToolLayer(properties)}, description={GetToolDescription(properties)}");
		}
		list.Sort(StringComparer.Ordinal);
		return list;
	}

	private static List<string> FormatExtensionLines(ReadOnlySpan<VkExtensionProperties> extensions)
	{
		List<string> list = new List<string>(extensions.Length);
		for (int i = 0; i < extensions.Length; i++)
		{
			VkExtensionProperties extension = extensions[i];
			list.Add(GetExtensionName(extension) + " (spec=" + FormatVersionWithHex(extension.SpecVersion) + ")");
		}
		list.Sort(StringComparer.Ordinal);
		return list;
	}

	private static bool SupportsToolProperties(PhysicalDevice physicalDevice, int apiVersion)
	{
		if (!IsApiVersionAtLeast(apiVersion, 1, 3, 0))
		{
			return physicalDevice.SupportsExtension("VK_EXT_tooling_info");
		}
		return true;
	}

	private static bool IsApiVersionAtLeast(int apiVersion, int major, int minor, int patch)
	{
		DeconstructApiVersion(apiVersion, out var major2, out var minor2, out var patch2, out var _);
		if (major2 != major)
		{
			return major2 > major;
		}
		if (minor2 != minor)
		{
			return minor2 > minor;
		}
		return patch2 >= patch;
	}

	private static void DeconstructApiVersion(int apiVersion, out int major, out int minor, out int patch, out int variant)
	{
		variant = (apiVersion >> 29) & 7;
		major = (apiVersion >> 22) & 0x7F;
		minor = (apiVersion >> 12) & 0x3FF;
		patch = apiVersion & 0xFFF;
	}

	private static string FormatApiVersion(int apiVersion)
	{
		DeconstructApiVersion(apiVersion, out var major, out var minor, out var patch, out var variant);
		return $"{major}.{minor}.{patch} (variant={variant}, raw=0x{apiVersion:X8})";
	}

	private static string FormatVersionWithHex(int version)
	{
		return $"{version} (0x{version:X8})";
	}

	private static string FormatConformanceVersion(VkConformanceVersion version)
	{
		return $"{version.Major}.{version.Minor}.{version.Subminor}.{version.Patch}";
	}

	private static string FormatExtent(VkExtent2D extent)
	{
		return $"{extent.Width}x{extent.Height}";
	}

	private static string FormatUnboundedCount(int count)
	{
		if (count != 0)
		{
			return count.ToString();
		}
		return "unbounded";
	}

	private static string FormatHandle(nint handle)
	{
		return $"0x{((IntPtr)handle).ToInt64():X}";
	}

	private static string FormatByteSize64(ByteSize64 size)
	{
		return size.ToString();
	}

	private unsafe static string FormatUuid(byte* bytes, int length)
	{
		string[] array = new string[length];
		for (int i = 0; i < length; i++)
		{
			array[i] = bytes[i].ToString("X2");
		}
		return string.Join("-", array);
	}

	private unsafe static string Utf8String(byte* value)
	{
		return Utf8.GetString(value);
	}

	private unsafe static string GetExtensionName(VkExtensionProperties extension)
	{
		return Utf8String(extension.ExtensionName);
	}

	private unsafe static string GetLayerName(VkLayerProperties layer)
	{
		return Utf8String(layer.LayerName);
	}

	private unsafe static string GetLayerDescription(VkLayerProperties layer)
	{
		return Utf8String(layer.Description);
	}

	private unsafe static string GetDeviceName(VkPhysicalDeviceProperties properties)
	{
		return Utf8String(properties.DeviceName);
	}

	private unsafe static string GetDriverName(VkPhysicalDeviceDriverProperties properties)
	{
		return Utf8String(properties.DriverName);
	}

	private unsafe static string GetDriverInfo(VkPhysicalDeviceDriverProperties properties)
	{
		return Utf8String(properties.DriverInfo);
	}

	private unsafe static string GetToolName(VkPhysicalDeviceToolProperties properties)
	{
		return Utf8String(properties.Name);
	}

	private unsafe static string GetToolVersion(VkPhysicalDeviceToolProperties properties)
	{
		return Utf8String(properties.Version);
	}

	private unsafe static string GetToolDescription(VkPhysicalDeviceToolProperties properties)
	{
		return Utf8String(properties.Description);
	}

	private unsafe static string GetToolLayer(VkPhysicalDeviceToolProperties properties)
	{
		return Utf8String(properties.Layer);
	}

	private unsafe static string GetPipelineCacheUuid(VkPhysicalDeviceProperties properties)
	{
		return FormatUuid(properties.PipelineCacheUUID, 16);
	}

	private unsafe static string GetDeviceUuid(VkPhysicalDeviceVulkan11Properties properties)
	{
		return FormatUuid(properties.DeviceUUID, 16);
	}

	private unsafe static string GetDriverUuid(VkPhysicalDeviceVulkan11Properties properties)
	{
		return FormatUuid(properties.DriverUUID, 16);
	}

	private unsafe static string GetDeviceLuid(VkPhysicalDeviceVulkan11Properties properties)
	{
		return FormatUuid(properties.DeviceLUID, 8);
	}

	private static void AppendCollection(StringBuilder report, string label, IEnumerable<string> values)
	{
		string[] array = values.Where((string value) => !string.IsNullOrWhiteSpace(value)).ToArray();
		StringBuilder stringBuilder = report;
		StringBuilder stringBuilder2 = stringBuilder;
		StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(2, 2, stringBuilder);
		handler.AppendFormatted(label);
		handler.AppendLiteral(": ");
		handler.AppendFormatted(array.Length);
		stringBuilder2.AppendLine(ref handler);
		if (array.Length == 0)
		{
			report.AppendLine("  <none>");
			return;
		}
		for (int i = 0; i < array.Length; i++)
		{
			stringBuilder = report;
			StringBuilder stringBuilder3 = stringBuilder;
			handler = new StringBuilder.AppendInterpolatedStringHandler(2, 1, stringBuilder);
			handler.AppendLiteral("  ");
			handler.AppendFormatted(array[i]);
			stringBuilder3.AppendLine(ref handler);
		}
	}

	private static void SafeAppend(StringBuilder report, Action action)
	{
		try
		{
			action();
		}
		catch (Exception ex)
		{
			StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(8, 2, report);
			handler.AppendLiteral("N/A (");
			handler.AppendFormatted(ex.GetType().Name);
			handler.AppendLiteral(": ");
			handler.AppendFormatted(ex.Message);
			handler.AppendLiteral(")");
			report.AppendLine(ref handler);
		}
	}

	private static string TryFormat(Func<string> valueFactory)
	{
		try
		{
			return valueFactory();
		}
		catch (Exception ex)
		{
			return $"N/A ({ex.GetType().Name}: {ex.Message})";
		}
	}

	private static string? SafeGet(Func<string?> valueFactory)
	{
		try
		{
			return valueFactory();
		}
		catch
		{
			return null;
		}
	}

	private static string FormatBytes(long bytes)
	{
		string[] array = new string[5] { "B", "KiB", "MiB", "GiB", "TiB" };
		double num = bytes;
		int num2 = 0;
		while (num >= 1024.0 && num2 < array.Length - 1)
		{
			num /= 1024.0;
			num2++;
		}
		return $"{num:0.##} {array[num2]} ({bytes} bytes)";
	}
}
