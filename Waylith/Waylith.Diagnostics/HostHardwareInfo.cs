using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;
using Microsoft.Win32;

namespace Waylith.Diagnostics;

public static class HostHardwareInfo
{
	public readonly record struct DisplayDriver(string Description, string Version, string Date, string Provider, string MatchingDeviceId)
	{
		public bool Matches(long vendorId, long deviceId)
		{
			return MatchingDeviceId.Contains($"ven_{vendorId:x4}&dev_{deviceId:x4}", StringComparison.OrdinalIgnoreCase);
		}

		public override string ToString()
		{
			return $"{Description}: version {Version}, date {Date}, provider {Provider} ({MatchingDeviceId})";
		}
	}

	private struct MemoryStatusEx
	{
		public uint Length;

		public uint MemoryLoad;

		public ulong TotalPhys;

		public ulong AvailPhys;

		public ulong TotalPageFile;

		public ulong AvailPageFile;

		public ulong TotalVirtual;

		public ulong AvailVirtual;

		public ulong AvailExtendedVirtual;
	}

	private static string? _cpuName;

	private static long? _totalPhysicalMemory;

	public static string CpuName
	{
		get
		{
			object obj = _cpuName;
			if (obj == null)
			{
				obj = ReadCpuName() ?? "Unknown";
				_cpuName = (string?)obj;
			}
			return (string)obj;
		}
	}

	public static long TotalPhysicalMemoryBytes
	{
		get
		{
			long valueOrDefault = _totalPhysicalMemory.GetValueOrDefault();
			if (!_totalPhysicalMemory.HasValue)
			{
				valueOrDefault = ReadTotalPhysicalMemory();
				_totalPhysicalMemory = valueOrDefault;
				return valueOrDefault;
			}
			return valueOrDefault;
		}
	}

	private static string? ReadCpuName()
	{
		try
		{
			if (OperatingSystem.IsWindows())
			{
				return WindowsCpuName();
			}
			if (OperatingSystem.IsMacOS())
			{
				return SysctlString(new byte[25]
				{
					109, 97, 99, 104, 100, 101, 112, 46, 99, 112,
					117, 46, 98, 114, 97, 110, 100, 95, 115, 116,
					114, 105, 110, 103, 0
				});
			}
			if (OperatingSystem.IsLinux())
			{
				return ProcValue("/proc/cpuinfo", "model name") ?? ProcValue("/proc/cpuinfo", "Hardware") ?? ProcValue("/proc/cpuinfo", "Model");
			}
		}
		catch
		{
		}
		return null;
	}

	private unsafe static long ReadTotalPhysicalMemory()
	{
		try
		{
			if (OperatingSystem.IsWindows())
			{
				MemoryStatusEx memoryStatusEx = default(MemoryStatusEx);
				memoryStatusEx.Length = (uint)sizeof(MemoryStatusEx);
				MemoryStatusEx memoryStatusEx2 = memoryStatusEx;
				if (GlobalMemoryStatusEx(&memoryStatusEx2) != 0)
				{
					return (long)memoryStatusEx2.TotalPhys;
				}
			}
			else if (OperatingSystem.IsMacOS())
			{
				long result = 0L;
				nuint num = 8u;
				fixed (byte* name = (ReadOnlySpan<byte>)new byte[11]
				{
					104, 119, 46, 109, 101, 109, 115, 105, 122, 101,
					0
				})
				{
					if (sysctlbyname(name, &result, &num, null, 0u) == 0)
					{
						return result;
					}
				}
			}
			else if (OperatingSystem.IsLinux())
			{
				string text = ProcValue("/proc/meminfo", "MemTotal");
				if (text != null)
				{
					string text2;
					if (!text.EndsWith(" kB", StringComparison.Ordinal))
					{
						text2 = text;
					}
					else
					{
						string text3 = text;
						text2 = text3.Substring(0, text3.Length - 3);
					}
					if (long.TryParse(text2.Trim(), out var result2))
					{
						return result2 * 1024;
					}
				}
			}
		}
		catch
		{
		}
		return GC.GetGCMemoryInfo().TotalAvailableMemoryBytes;
	}

	private static string? ProcValue(string path, string key)
	{
		if (!File.Exists(path))
		{
			return null;
		}
		foreach (string item in File.ReadLines(path))
		{
			if (item.StartsWith(key, StringComparison.Ordinal))
			{
				int num = item.IndexOf(':');
				if (num > 0 && item.AsSpan(0, num).Trim().SequenceEqual(key.AsSpan()))
				{
					return item.Substring(num + 1).Trim();
				}
			}
		}
		return null;
	}

	public static string? LinuxKernelGpuDriver()
	{
		try
		{
			if (File.Exists("/proc/driver/nvidia/version"))
			{
				foreach (string item in File.ReadLines("/proc/driver/nvidia/version"))
				{
					if (item.Length > 0)
					{
						return item.Trim();
					}
				}
			}
		}
		catch
		{
		}
		return null;
	}

	[SupportedOSPlatform("windows")]
	private static string? WindowsCpuName()
	{
		using RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("HARDWARE\\DESCRIPTION\\System\\CentralProcessor\\0");
		return (registryKey?.GetValue("ProcessorNameString") as string)?.Trim();
	}

	[SupportedOSPlatform("windows")]
	public static IReadOnlyList<DisplayDriver> WindowsDisplayDrivers()
	{
		List<DisplayDriver> list = new List<DisplayDriver>();
		using RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Control\\Class\\{4d36e968-e325-11ce-bfc1-08002be10318}");
		if (registryKey == null)
		{
			return list;
		}
		string[] subKeyNames = registryKey.GetSubKeyNames();
		foreach (string name in subKeyNames)
		{
			try
			{
				using RegistryKey registryKey2 = registryKey.OpenSubKey(name);
				if (registryKey2?.GetValue("DriverDesc") is string description)
				{
					list.Add(new DisplayDriver(description, (registryKey2.GetValue("DriverVersion") as string) ?? "N/A", (registryKey2.GetValue("DriverDate") as string) ?? "N/A", (registryKey2.GetValue("ProviderName") as string) ?? "N/A", (registryKey2.GetValue("MatchingDeviceId") as string) ?? ""));
				}
			}
			catch
			{
			}
		}
		return list;
	}

	private unsafe static string? SysctlString(ReadOnlySpan<byte> name)
	{
		fixed (byte* name2 = name)
		{
			nuint num = 0u;
			if (sysctlbyname(name2, null, &num, null, 0u) != 0 || num == 0)
			{
				return null;
			}
			byte[] array = new byte[num];
			fixed (byte* oldp = array)
			{
				if (sysctlbyname(name2, oldp, &num, null, 0u) != 0)
				{
					return null;
				}
			}
			Span<byte> span = array.AsSpan(0, (int)num);
			int num2 = ((ReadOnlySpan<byte>)span).IndexOf((byte)0);
			return Encoding.UTF8.GetString((num2 >= 0) ? span.Slice(0, num2) : span).Trim();
		}
	}

	[LibraryImport("kernel32", SetLastError = true)]
	[GeneratedCode("Microsoft.Interop.LibraryImportGenerator", "42.42.42.42")]
	[SkipLocalsInit]
	private unsafe static int GlobalMemoryStatusEx(MemoryStatusEx* status)
	{
		Marshal.SetLastSystemError(0);
		int result = __PInvoke(status);
		Marshal.SetLastPInvokeError(Marshal.GetLastSystemError());
		return result;
		[DllImport("kernel32", EntryPoint = "GlobalMemoryStatusEx", ExactSpelling = true)]
		static extern unsafe int __PInvoke(MemoryStatusEx* __status_native);
	}

	[DllImport("libc", ExactSpelling = true)]
	[LibraryImport("libc")]
	private unsafe static extern int sysctlbyname(byte* name, void* oldp, nuint* oldlenp, void* newp, nuint newlen);
}
