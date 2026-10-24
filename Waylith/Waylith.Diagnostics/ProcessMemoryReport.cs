using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Brutal.Logging;
using Brutal.Logging.Internal;
using Waylith.Assets;
using Waylith.Graphics;
using Waylith.Profiling;
using Waylith.Rendering;
using Waylith.Runtime;

namespace Waylith.Diagnostics;

public static class ProcessMemoryReport
{
	private static class LinuxMallinfo
	{
		private struct Mallinfo2
		{
			public nuint arena;

			public nuint ordblks;

			public nuint smblks;

			public nuint hblks;

			public nuint hblkhd;

			public nuint usmblks;

			public nuint fsmblks;

			public nuint uordblks;

			public nuint fordblks;

			public nuint keepcost;
		}

		[DllImport("libc")]
		private static extern Mallinfo2 mallinfo2();

		public static bool TryRead(out long inUse, out long arena, out long mmapped, out long free)
		{
			try
			{
				Mallinfo2 mallinfo = mallinfo2();
				arena = (long)mallinfo.arena;
				mmapped = (long)mallinfo.hblkhd;
				free = (long)mallinfo.fordblks;
				inUse = (long)mallinfo.uordblks + mmapped;
				return true;
			}
			catch (Exception)
			{
				inUse = (arena = (mmapped = (free = 0L)));
				return false;
			}
		}
	}

	private sealed class LinuxMappings
	{
		public long Rss;

		public long Anonymous;

		public long Shared;

		public long DriverBytes;

		public long JitBytes;

		private readonly Dictionary<string, long> fileRssByPath = new Dictionary<string, long>(StringComparer.Ordinal);

		public long FileBacked => System.Math.Max(0L, Rss - Anonymous - Shared);

		public long ResidentBytesForPath(string path)
		{
			if (!fileRssByPath.TryGetValue(path, out var value))
			{
				return 0L;
			}
			return value;
		}

		public IEnumerable<(string Path, long Rss)> FileBackedByPath()
		{
			List<(string, long)> list = new List<(string, long)>(fileRssByPath.Count);
			foreach (KeyValuePair<string, long> item in fileRssByPath)
			{
				list.Add((item.Key, item.Value));
			}
			list.Sort(((string, long) a, (string, long) b) => b.Item2.CompareTo(a.Item2));
			return list;
		}

		public static LinuxMappings? Read()
		{
			try
			{
				LinuxMappings linuxMappings = new LinuxMappings();
				using StreamReader streamReader = new StreamReader("/proc/self/smaps", Encoding.ASCII, detectEncodingFromByteOrderMarks: false, 65536);
				string path = null;
				long rss = 0L;
				long anon = 0L;
				while (true)
				{
					string text = streamReader.ReadLine();
					if (text == null)
					{
						break;
					}
					if (text.Length > 0 && IsHex(text[0]) && text.Contains('-'))
					{
						Flush(linuxMappings, path, rss, anon);
						path = MappingPath(text);
						rss = (anon = 0L);
					}
					else if (text.StartsWith("Rss:", StringComparison.Ordinal))
					{
						rss = KbValue(text);
					}
					else if (text.StartsWith("Anonymous:", StringComparison.Ordinal))
					{
						anon = KbValue(text);
					}
					else if (text.StartsWith("Shared_Clean:", StringComparison.Ordinal) || text.StartsWith("Shared_Dirty:", StringComparison.Ordinal))
					{
						linuxMappings.Shared += KbValue(text);
					}
				}
				Flush(linuxMappings, path, rss, anon);
				return linuxMappings;
			}
			catch (Exception)
			{
				return null;
			}
		}

		private static void Flush(LinuxMappings result, string? path, long rss, long anon)
		{
			if (path != null)
			{
				result.Rss += rss;
				result.Anonymous += anon;
				if (path.StartsWith("/dev/nvidia", StringComparison.Ordinal) || path.StartsWith("/dev/dri", StringComparison.Ordinal))
				{
					result.DriverBytes += rss;
				}
				else if (path.Contains("doublemapper", StringComparison.Ordinal))
				{
					result.JitBytes += rss;
				}
				else if (path.StartsWith('/') && rss > anon)
				{
					result.fileRssByPath[path] = result.fileRssByPath.GetValueOrDefault(path) + rss - anon;
				}
			}
		}

		private static bool IsHex(char c)
		{
			if (c < '0' || c > '9')
			{
				if (c >= 'a')
				{
					return c <= 'f';
				}
				return false;
			}
			return true;
		}

		private static string MappingPath(string header)
		{
			int num = 0;
			for (int i = 0; i < header.Length; i++)
			{
				if (header[i] != ' ')
				{
					continue;
				}
				for (; i + 1 < header.Length && header[i + 1] == ' '; i++)
				{
				}
				if (++num == 5)
				{
					if (i + 1 >= header.Length)
					{
						return string.Empty;
					}
					return header.Substring(i + 1);
				}
			}
			return string.Empty;
		}

		private static long KbValue(string line)
		{
			int num = line.IndexOf(':');
			ReadOnlySpan<char> readOnlySpan = line.AsSpan(num + 1).Trim();
			int num2 = readOnlySpan.IndexOf(' ');
			if (num2 > 0)
			{
				readOnlySpan = readOnlySpan.Slice(0, num2);
			}
			if (!long.TryParse(readOnlySpan, out var result))
			{
				return 0L;
			}
			return result * 1024;
		}
	}

	private static readonly LogCategory Logger = LogCategory.Make("ProcessMemory");

	public const string ProcessSection = "Process";

	public const string ManagedSection = "Managed heap (GC)";

	public const string NativeSection = "Native heap (malloc)";

	public const string GpuHostSection = "GPU memory in system RAM";

	public const string PacksSection = "Content packs (mapped)";

	public const string ImagesSection = "Mapped files and images";

	public static ProcessMemorySnapshot Capture()
	{
		LinuxMappings linuxMappings = (OperatingSystem.IsLinux() ? LinuxMappings.Read() : null);
		using Process process = Process.GetCurrentProcess();
		long num = linuxMappings?.Rss ?? process.WorkingSet64;
		ProcessMemorySnapshot processMemorySnapshot = new ProcessMemorySnapshot
		{
			CapturedAt = DateTime.Now,
			ResidentBytes = num,
			AnonymousBytes = (linuxMappings?.Anonymous ?? 0),
			FileBackedBytes = (linuxMappings?.FileBacked ?? 0),
			SharedBytes = (linuxMappings?.Shared ?? 0),
			PrivateBytes = process.PrivateMemorySize64,
			HasMappingDetail = (linuxMappings != null)
		};
		MemoryReportSection memoryReportSection = new MemoryReportSection
		{
			Name = "Process",
			Bytes = num,
			Detail = "resident set"
		};
		if (linuxMappings != null)
		{
			memoryReportSection.Items.Add(new MemoryReportItem("anonymous (heaps, GC, driver)", linuxMappings.Anonymous, 0L));
			memoryReportSection.Items.Add(new MemoryReportItem("file backed (packs, dlls, libraries)", linuxMappings.FileBacked - linuxMappings.DriverBytes, 0L, "page cache, reclaimable"));
			memoryReportSection.Items.Add(new MemoryReportItem("shared", linuxMappings.Shared, 0L));
			if (linuxMappings.DriverBytes > 0)
			{
				memoryReportSection.Items.Add(new MemoryReportItem("GPU driver mappings", linuxMappings.DriverBytes, 0L, "/dev/nvidia*, /dev/dri"));
			}
			if (linuxMappings.JitBytes > 0)
			{
				memoryReportSection.Items.Add(new MemoryReportItem("JIT code (double mapped)", linuxMappings.JitBytes, 0L));
			}
		}
		else
		{
			memoryReportSection.Items.Add(new MemoryReportItem("private (committed)", process.PrivateMemorySize64, 0L));
		}
		processMemorySnapshot.Sections.Add(memoryReportSection);
		MemoryReportSection memoryReportSection2 = CaptureManaged();
		processMemorySnapshot.Sections.Add(memoryReportSection2);
		MemoryReportSection memoryReportSection3 = CaptureNative();
		processMemorySnapshot.Sections.Add(memoryReportSection3);
		processMemorySnapshot.Sections.Add(CaptureGpuHost(linuxMappings));
		processMemorySnapshot.Sections.Add(CapturePacks(linuxMappings));
		if (linuxMappings != null)
		{
			processMemorySnapshot.Sections.Add(CaptureImages(linuxMappings));
		}
		if (linuxMappings != null)
		{
			long num2 = linuxMappings.Anonymous - memoryReportSection2.Bytes - memoryReportSection3.Bytes;
			memoryReportSection.Items.Add(new MemoryReportItem("anonymous not attributed (runtime, stacks, driver)", num2, 0L, (num2 < 0) ? "negative: committed but untouched pages in the GC or malloc" : ""));
		}
		return processMemorySnapshot;
	}

	private static MemoryReportSection CaptureManaged()
	{
		GCMemoryInfo gCMemoryInfo = GC.GetGCMemoryInfo();
		long totalMemory = GC.GetTotalMemory(forceFullCollection: false);
		MemoryReportSection memoryReportSection = new MemoryReportSection
		{
			Name = "Managed heap (GC)",
			Bytes = gCMemoryInfo.TotalCommittedBytes,
			Detail = "committed, " + VramProfilerFormat(totalMemory) + " allocated"
		};
		memoryReportSection.Items.Add(new MemoryReportItem("allocated objects", totalMemory, 0L, "includes garbage not yet collected"));
		memoryReportSection.Items.Add(new MemoryReportItem("heap size (live + free blocks)", gCMemoryInfo.HeapSizeBytes, 0L));
		memoryReportSection.Items.Add(new MemoryReportItem("fragmented (free blocks)", gCMemoryInfo.FragmentedBytes, 0L));
		ReadOnlySpan<GCGenerationInfo> generationInfo = gCMemoryInfo.GenerationInfo;
		if (generationInfo.Length >= 5)
		{
			memoryReportSection.Items.Add(new MemoryReportItem("gen0", generationInfo[0].SizeAfterBytes, 0L));
			memoryReportSection.Items.Add(new MemoryReportItem("gen1", generationInfo[1].SizeAfterBytes, 0L));
			memoryReportSection.Items.Add(new MemoryReportItem("gen2", generationInfo[2].SizeAfterBytes, 0L));
			memoryReportSection.Items.Add(new MemoryReportItem("large object heap", generationInfo[3].SizeAfterBytes, 0L, "after the last GC of any generation, dead large arrays stay until a gen2"));
			memoryReportSection.Items.Add(new MemoryReportItem("pinned object heap", generationInfo[4].SizeAfterBytes, 0L));
		}
		memoryReportSection.Items.Add(new MemoryReportItem("texture CPU copies (byte[] levels)", (long)TexturePixelData.CurrentCpuBytes, 0L, "inside the managed heap"));
		memoryReportSection.Detail += $", {GC.CollectionCount(0)}/{GC.CollectionCount(1)}/{GC.CollectionCount(2)} gen0/1/2 collections";
		return memoryReportSection;
	}

	private static MemoryReportSection CaptureNative()
	{
		MemoryReportSection memoryReportSection = new MemoryReportSection
		{
			Name = "Native heap (malloc)"
		};
		long num = 0L;
		UnityAllocationBucketDiagnostics[] buckets = NativeAllocationRuntime.GetDiagnosticsSnapshot().Buckets;
		for (int i = 0; i < buckets.Length; i++)
		{
			UnityAllocationBucketDiagnostics unityAllocationBucketDiagnostics = buckets[i];
			if (unityAllocationBucketDiagnostics.Bytes != 0L || unityAllocationBucketDiagnostics.Count != 0)
			{
				num += unityAllocationBucketDiagnostics.Bytes;
				memoryReportSection.Items.Add(new MemoryReportItem("allocation runtime: " + unityAllocationBucketDiagnostics.Kind, unityAllocationBucketDiagnostics.Bytes, unityAllocationBucketDiagnostics.Count, KindDetail(unityAllocationBucketDiagnostics.Kind)));
			}
		}
		if (OperatingSystem.IsLinux() && LinuxMallinfo.TryRead(out var inUse, out var arena, out var mmapped, out var free))
		{
			memoryReportSection.Bytes = inUse;
			memoryReportSection.Detail = "glibc mallinfo2, in use";
			memoryReportSection.Items.Add(new MemoryReportItem("other native (driver, FreeType, runtime, libraries)", System.Math.Max(0L, inUse - num), 0L));
			memoryReportSection.Items.Add(new MemoryReportItem("main arena (brk heap)", arena, 0L));
			memoryReportSection.Items.Add(new MemoryReportItem("mmapped chunks (large blocks)", mmapped, 0L));
			memoryReportSection.Items.Add(new MemoryReportItem("free inside arenas", free, 0L, "fragmentation, held by glibc"));
		}
		else
		{
			memoryReportSection.Bytes = num;
			memoryReportSection.Detail = "sum of tracked allocators, no malloc total on this platform";
		}
		return memoryReportSection;
	}

	private static string KindDetail(AllocationKind kind)
	{
		return kind switch
		{
			AllocationKind.AudioKernel => "decoded clips and miniaudio", 
			AllocationKind.Persistent => "NativeArray/NativeList Allocator.Persistent", 
			AllocationKind.MeshBacking => "CPU copies of mesh vertex and index data", 
			AllocationKind.UI => "ImGui: fonts, FreeType, atlas, draw lists", 
			AllocationKind.Temp => "Allocator.Temp, freed per frame", 
			AllocationKind.TempJob => "Allocator.TempJob", 
			AllocationKind.Domain => "Allocator.Domain", 
			_ => "", 
		};
	}

	private static MemoryReportSection CaptureGpuHost(LinuxMappings? mappings)
	{
		MemoryReportSection memoryReportSection = new MemoryReportSection
		{
			Name = "GPU memory in system RAM",
			Detail = "VMA allocations placed in host heaps"
		};
		if (GpuMemoryReport.TryGetSummary(out var summary))
		{
			memoryReportSection.Bytes = summary.HostVmaAllocationBytes;
			memoryReportSection.Items.Add(new MemoryReportItem("offloaded textures", TextureResidency.OffloadedBytes, TextureResidency.OffloadedCount, "evicted from VRAM, kept in system memory"));
			memoryReportSection.Items.Add(new MemoryReportItem("staging, uniform and host visible buffers", System.Math.Max(0L, summary.HostVmaAllocationBytes - TextureResidency.OffloadedBytes), 0L));
			memoryReportSection.Items.Add(new MemoryReportItem("device local VMA blocks (VRAM, not RAM)", summary.DeviceLocalVmaBlockBytes, 0L, "for reference only"));
		}
		else
		{
			memoryReportSection.Detail = "no graphics device";
		}
		if (mappings != null && mappings.DriverBytes > 0)
		{
			memoryReportSection.Items.Add(new MemoryReportItem("driver mappings in the process (counted under Process)", mappings.DriverBytes, 0L));
		}
		return memoryReportSection;
	}

	private static MemoryReportSection CapturePacks(LinuxMappings? mappings)
	{
		MemoryReportSection memoryReportSection = new MemoryReportSection
		{
			Name = "Content packs (mapped)",
			Detail = ((mappings != null) ? "resident pages of the mapped pack files" : "mapped size, resident unknown on this platform")
		};
		foreach (ContentPack.MountPoint mount in ContentPack.Mounts)
		{
			AssetPack pack = mount.Pack;
			long num = mappings?.ResidentBytesForPath(pack.Path) ?? (-1);
			long num2 = ((num >= 0) ? num : pack.MappedBytes);
			memoryReportSection.Bytes += num2;
			memoryReportSection.Count++;
			memoryReportSection.Items.Add(new MemoryReportItem(mount.Root, num2, 0L, VramProfilerFormat(pack.MappedBytes) + " mapped"));
		}
		return memoryReportSection;
	}

	private static MemoryReportSection CaptureImages(LinuxMappings mappings)
	{
		MemoryReportSection memoryReportSection = new MemoryReportSection
		{
			Name = "Mapped files and images",
			Detail = "largest file backed mappings, excluding packs"
		};
		HashSet<string> hashSet = new HashSet<string>(StringComparer.Ordinal);
		foreach (ContentPack.MountPoint mount in ContentPack.Mounts)
		{
			hashSet.Add(mount.Pack.Path);
		}
		int num = 0;
		foreach (var (text, num2) in mappings.FileBackedByPath())
		{
			if (!hashSet.Contains(text))
			{
				memoryReportSection.Bytes += num2;
				memoryReportSection.Count++;
				if (num++ < 16)
				{
					memoryReportSection.Items.Add(new MemoryReportItem(Path.GetFileName(text), num2, 0L, Path.GetDirectoryName(text)));
				}
			}
		}
		return memoryReportSection;
	}

	public static string BuildReport(ProcessMemorySnapshot? snapshot = null)
	{
		if (snapshot == null)
		{
			snapshot = Capture();
		}
		StringBuilder stringBuilder = new StringBuilder(4096);
		stringBuilder.Append("Process memory ").Append(snapshot.CapturedAt.ToString("yyyy-MM-dd HH:mm:ss")).Append(": resident ")
			.Append(VramProfilerFormat(snapshot.ResidentBytes));
		if (snapshot.HasMappingDetail)
		{
			stringBuilder.Append(" (anonymous ").Append(VramProfilerFormat(snapshot.AnonymousBytes)).Append(", file backed ")
				.Append(VramProfilerFormat(snapshot.FileBackedBytes))
				.Append(" incl. driver)");
		}
		else
		{
			stringBuilder.Append(", private ").Append(VramProfilerFormat(snapshot.PrivateBytes));
		}
		stringBuilder.AppendLine();
		foreach (MemoryReportSection section in snapshot.Sections)
		{
			stringBuilder.Append("  ").Append(section.Name).Append(": ")
				.Append(VramProfilerFormat(section.Bytes));
			if (section.Count > 0)
			{
				stringBuilder.Append(" (").Append(section.Count).Append(')');
			}
			if (!string.IsNullOrEmpty(section.Detail))
			{
				stringBuilder.Append("  ").Append(section.Detail);
			}
			stringBuilder.AppendLine();
			foreach (MemoryReportItem item in section.Items)
			{
				stringBuilder.Append("      ").Append(item.Name).Append(": ")
					.Append(VramProfilerFormat(item.Bytes));
				if (item.Count > 0)
				{
					stringBuilder.Append(" (").Append(item.Count).Append(')');
				}
				if (!string.IsNullOrEmpty(item.Detail))
				{
					stringBuilder.Append("  ").Append(item.Detail);
				}
				stringBuilder.AppendLine();
			}
		}
		return stringBuilder.ToString();
	}

	public static void Log()
	{
		try
		{
			Logger.Info(BuildReport(), "Log", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Diagnostics\\ProcessMemoryReport.cs", 273);
		}
		catch (Exception ex)
		{
			LogCategory logger = Logger;
			bool shouldAppend;
			LogString<Warning> message = new LogString<Warning>(30, 1, logger, out shouldAppend);
			if (shouldAppend)
			{
				message.AppendLiteral("Process memory report failed: ");
				message.AppendFormatted(ex.Message);
			}
			logger.Warning(message, "Log", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Diagnostics\\ProcessMemoryReport.cs", 277);
		}
	}

	private static string VramProfilerFormat(long bytes)
	{
		if (bytes >= 0)
		{
			return VramProfiler.FormatBytes(bytes);
		}
		return "-" + VramProfiler.FormatBytes(-bytes);
	}
}
