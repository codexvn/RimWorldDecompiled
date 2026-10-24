using System;
using System.Runtime.Versioning;
using Brutal.Logging;
using Brutal.Logging.Internal;
using Microsoft.Win32;

namespace Waylith.Graphics;

internal static class WindowsGpuPreference
{
	private static readonly LogCategory Logger = LogCategory.Make("GraphicsDevice");

	public static bool IsExplicit()
	{
		if (!OperatingSystem.IsWindows())
		{
			return false;
		}
		try
		{
			int? num = ReadPreference();
			return num.HasValue && num.GetValueOrDefault() > 0;
		}
		catch (Exception ex)
		{
			LogCategory logger = Logger;
			bool shouldAppend;
			LogString<Information> message = new LogString<Information>(43, 1, logger, out shouldAppend);
			if (shouldAppend)
			{
				message.AppendLiteral("Could not read the Windows GPU preference: ");
				message.AppendFormatted(ex.Message);
			}
			logger.Info(message, "IsExplicit", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Graphics\\WindowsGpuPreference.cs", 23);
			return false;
		}
	}

	[SupportedOSPlatform("windows")]
	private static int? ReadPreference()
	{
		string processPath = Environment.ProcessPath;
		if (string.IsNullOrEmpty(processPath))
		{
			return null;
		}
		using RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\DirectX\\UserGpuPreferences");
		if (!(registryKey?.GetValue(processPath) is string text))
		{
			return null;
		}
		string[] array = text.Split(';', StringSplitOptions.RemoveEmptyEntries);
		foreach (string text2 in array)
		{
			int num = text2.IndexOf('=');
			if (num > 0 && MemoryExtensions.Equals(text2.AsSpan(0, num).Trim(), "GpuPreference".AsSpan(), StringComparison.OrdinalIgnoreCase) && int.TryParse(text2.AsSpan(num + 1).Trim(), out var result))
			{
				return result;
			}
		}
		return null;
	}
}
