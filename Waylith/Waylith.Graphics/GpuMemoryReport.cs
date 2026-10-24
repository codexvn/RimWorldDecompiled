using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Brutal.Logging;
using Brutal.VulkanApi;
using Brutal.VulkanApi.Vma;
using Waylith.Rendering;

namespace Waylith.Graphics;

public static class GpuMemoryReport
{
	private static readonly LogCategory Logger = LogCategory.Make("GpuMemory");

	private static GraphicsDevice? _graphicsDevice;

	private static bool _capTripped;

	private static float _nextCapCheckTime;

	public static bool IsConfigured => _graphicsDevice != null;

	public static void Configure(GraphicsDevice graphicsDevice)
	{
		ArgumentNullException.ThrowIfNull(graphicsDevice, "graphicsDevice");
		_graphicsDevice = graphicsDevice;
	}

	public static bool TryGetSummary(out GpuMemorySummary summary)
	{
		GraphicsDevice graphicsDevice = _graphicsDevice;
		if (graphicsDevice == null)
		{
			summary = default(GpuMemorySummary);
			return false;
		}
		VmaAllocator allocator = graphicsDevice.Allocator;
		VkPhysicalDeviceMemoryProperties memoryProperties = graphicsDevice.PhysicalDevice.GetMemoryProperties();
		int memoryHeapCount = memoryProperties.MemoryHeapCount;
		if (memoryHeapCount <= 0)
		{
			summary = default(GpuMemorySummary);
			return false;
		}
		Span<VmaBudget> pBudgets = stackalloc VmaBudget[memoryHeapCount];
		allocator.GetHeapBudgets(pBudgets);
		GpuHeapInfo[] array = new GpuHeapInfo[memoryHeapCount];
		long vramLimitBytes = graphicsDevice.VramLimitBytes;
		long num = 0L;
		long num2 = 0L;
		long num3 = 0L;
		long num4 = 0L;
		long num5 = 0L;
		long num6 = 0L;
		int num7 = 0;
		int num8 = 0;
		for (int i = 0; i < memoryHeapCount; i++)
		{
			VkMemoryHeap vkMemoryHeap = global::_003CPrivateImplementationDetails_003E.InlineArrayAsSpan<VkMemoryHeap_16, VkMemoryHeap>(ref memoryProperties.MemoryHeaps, 16)[i];
			VmaBudget vmaBudget = pBudgets[i];
			bool flag = (vkMemoryHeap.Flags & VkMemoryHeapFlags.DeviceLocalBit) != 0;
			long num9 = ((flag && vramLimitBytes > 0) ? System.Math.Min((long)vkMemoryHeap.Size.Value, vramLimitBytes) : ((long)vkMemoryHeap.Size.Value));
			long num10 = (long)vmaBudget.Statistics.AllocationBytes;
			long num11 = (long)vmaBudget.Statistics.BlockBytes;
			int allocationCount = (int)vmaBudget.Statistics.AllocationCount;
			array[i] = new GpuHeapInfo(i, flag, num9, (long)vmaBudget.Usage, (long)vmaBudget.Budget, num11, num10, (int)vmaBudget.Statistics.BlockCount, allocationCount);
			num5 += num10;
			num6 += num11;
			num7 += allocationCount;
			num8 += (int)vmaBudget.Statistics.BlockCount;
			if (flag)
			{
				num += (long)vmaBudget.Usage;
				num2 += (long)vmaBudget.Budget;
				num3 += num9;
				num4 += num10;
			}
		}
		summary = new GpuMemorySummary(num, num2, num3, num4, num5, num6, num7, array)
		{
			DriverReported = graphicsDevice.MemoryBudgetEnabled,
			VmaBlockCount = num8,
			MaxDeviceMemoryCount = graphicsDevice.MaxDeviceMemoryCount
		};
		return true;
	}

	public static long GetDeviceLocalAllocatedBytes()
	{
		if (!TryGetSummary(out var summary))
		{
			return 0L;
		}
		return summary.DeviceLocalVmaAllocationBytes;
	}

	public static bool TryGetDeviceLocalDriverUsage(out long usageBytes)
	{
		usageBytes = 0L;
		GraphicsDevice graphicsDevice = _graphicsDevice;
		if (graphicsDevice == null)
		{
			return false;
		}
		VkPhysicalDeviceMemoryProperties memoryProperties = graphicsDevice.PhysicalDevice.GetMemoryProperties();
		int memoryHeapCount = memoryProperties.MemoryHeapCount;
		if (memoryHeapCount <= 0)
		{
			return false;
		}
		Span<VmaBudget> pBudgets = stackalloc VmaBudget[memoryHeapCount];
		graphicsDevice.Allocator.GetHeapBudgets(pBudgets);
		for (int i = 0; i < memoryHeapCount; i++)
		{
			if ((global::_003CPrivateImplementationDetails_003E.InlineArrayAsSpan<VkMemoryHeap_16, VkMemoryHeap>(ref memoryProperties.MemoryHeaps, 16)[i].Flags & VkMemoryHeapFlags.DeviceLocalBit) != 0)
			{
				usageBytes += (long)pBudgets[i].Usage;
			}
		}
		return true;
	}

	public static void CheckUsageCap(long capBytes, float elapsedTime)
	{
		if (capBytes > 0 && !_capTripped && !(elapsedTime < _nextCapCheckTime))
		{
			_nextCapCheckTime = elapsedTime + 2f;
			if (TryGetDeviceLocalDriverUsage(out var usageBytes) && usageBytes > capBytes)
			{
				_capTripped = true;
				StringBuilder stringBuilder = new StringBuilder(8192);
				stringBuilder.Append("VRAM usage ").Append(FormatBytes(usageBytes)).Append(" exceeded the ")
					.Append(FormatBytes(capBytes))
					.Append(" diagnostic cap. This suggests a VRAM leak. One-time breakdown follows.")
					.AppendLine();
				GpuMemoryHistory.AppendGrowthSummary(stringBuilder, 12, 1048576L);
				stringBuilder.Append(BuildReport(includeVmaJson: false));
				Logger.Error(stringBuilder.ToString(), "CheckUsageCap", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Graphics\\GpuMemoryReport.cs", 239);
			}
		}
	}

	public static string BuildCompactSummary()
	{
		StringBuilder stringBuilder = new StringBuilder(1024);
		AppendCompactSummary(stringBuilder);
		return stringBuilder.ToString();
	}

	public static void AppendCompactSummary(StringBuilder sb)
	{
		if (TryGetSummary(out var summary))
		{
			sb.Append("Device-local VRAM: ").Append(UsageSource(in summary)).Append(' ')
				.Append(FormatBytes(summary.DeviceLocalDriverUsageBytes))
				.Append(" used / ")
				.Append(FormatBytes(summary.DeviceLocalBudgetBytes))
				.Append(" budget (free ")
				.Append(FormatBytes(summary.DeviceLocalFreeBytes))
				.Append("), VMA ")
				.Append(FormatBytes(summary.DeviceLocalVmaAllocationBytes))
				.Append(" allocated in ")
				.Append(FormatBytes(summary.DeviceLocalVmaBlockBytes))
				.Append(" of blocks")
				.Append(LimitSuffix())
				.Append('.')
				.AppendLine();
			sb.Append("System memory via VMA: ").Append(FormatBytes(summary.HostVmaAllocationBytes)).Append(", of which ")
				.Append(TextureResidency.OffloadedCount)
				.Append(" offloaded textures take ")
				.Append(FormatBytes(TextureResidency.OffloadedBytes))
				.Append('.')
				.AppendLine();
			sb.Append("VMA all heaps: ").Append(FormatBytes(summary.VmaAllocationBytes)).Append(" in ")
				.Append(summary.VmaAllocationCount)
				.Append(" allocations, ")
				.Append(FormatBytes(summary.VmaBlockBytes))
				.Append(" reserved in blocks");
			if (summary.VmaBlockBytes > 0)
			{
				sb.Append(" (").Append(((double)summary.VmaAllocationBytes / (double)summary.VmaBlockBytes * 100.0).ToString("0")).Append("% of reserved in use)");
			}
			sb.Append(", ").Append(summary.VmaBlockCount).Append(" device memory objects of ")
				.Append(summary.MaxDeviceMemoryCount)
				.Append(" allowed.")
				.AppendLine();
			sb.Append("Awaiting deferred disposal: ").Append(FormatBytes(TextureResourceManager.PendingDisposalBytes)).Append(" in ")
				.Append(TextureResourceManager.PendingDisposalCount)
				.Append(" textures.")
				.AppendLine();
		}
		else
		{
			sb.AppendLine("Device-local VRAM: unavailable (allocator not configured).");
		}
		List<GpuMemoryTracker.TopLevelStat> list = new List<GpuMemoryTracker.TopLevelStat>();
		GpuMemoryTracker.CollectTopLevel(list);
		list.Sort((GpuMemoryTracker.TopLevelStat a, GpuMemoryTracker.TopLevelStat b) => b.Bytes.CompareTo(a.Bytes));
		sb.Append("Tracked: ").Append(FormatBytes(GpuMemoryTracker.TotalBytes())).Append(" by label: ");
		for (int i = 0; i < list.Count; i++)
		{
			if (i > 0)
			{
				sb.Append(", ");
			}
			sb.Append(list[i].Label).Append(' ').Append(FormatBytes(list[i].Bytes))
				.Append(" (")
				.Append(list[i].Count)
				.Append(')');
		}
		sb.AppendLine();
	}

	public static string BuildReport(bool includeVmaJson = true)
	{
		GraphicsDevice graphicsDevice = _graphicsDevice;
		if (graphicsDevice == null)
		{
			return "GPU memory report unavailable: GpuMemoryReport has not been configured.";
		}
		VmaAllocator allocator = graphicsDevice.Allocator;
		VkPhysicalDeviceMemoryProperties memoryProperties = graphicsDevice.PhysicalDevice.GetMemoryProperties();
		StringBuilder stringBuilder = new StringBuilder(4096);
		if (TryGetSummary(out var summary))
		{
			stringBuilder.AppendLine("=== GPU Memory (VRAM) ===");
			stringBuilder.Append("Device-local usage (").Append(UsageSource(in summary)).Append("): ")
				.Append(FormatBytes(summary.DeviceLocalDriverUsageBytes))
				.Append(" / budget ")
				.Append(FormatBytes(summary.DeviceLocalBudgetBytes))
				.Append(" (free ")
				.Append(FormatBytes(summary.DeviceLocalFreeBytes))
				.Append(')')
				.AppendLine();
			stringBuilder.Append("Device-local physical size: ").Append(FormatBytes(summary.DeviceLocalTotalBytes)).Append(LimitSuffix())
				.AppendLine();
			stringBuilder.Append("System memory via VMA: ").Append(FormatBytes(summary.HostVmaAllocationBytes)).Append(", of which ")
				.Append(TextureResidency.OffloadedCount)
				.Append(" offloaded textures take ")
				.Append(FormatBytes(TextureResidency.OffloadedBytes))
				.AppendLine();
			stringBuilder.Append("Allocated by us via VMA (device-local): ").Append(FormatBytes(summary.DeviceLocalVmaAllocationBytes)).AppendLine();
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Per-heap:");
			GpuHeapInfo[] heaps = summary.Heaps;
			for (int i = 0; i < heaps.Length; i++)
			{
				GpuHeapInfo gpuHeapInfo = heaps[i];
				stringBuilder.Append("  Heap ").Append(gpuHeapInfo.HeapIndex).Append(gpuHeapInfo.DeviceLocal ? " [device-local]" : " [host]")
					.Append(": size ")
					.Append(FormatBytes(gpuHeapInfo.TotalBytes))
					.Append(", driver usage ")
					.Append(FormatBytes(gpuHeapInfo.DriverUsageBytes))
					.Append(", budget ")
					.Append(FormatBytes(gpuHeapInfo.BudgetBytes))
					.Append(" | VMA blocks ")
					.Append(gpuHeapInfo.VmaBlockCount)
					.Append(' ')
					.Append(FormatBytes(gpuHeapInfo.VmaBlockBytes))
					.Append(", allocations ")
					.Append(gpuHeapInfo.VmaAllocationCount)
					.Append(' ')
					.Append(FormatBytes(gpuHeapInfo.VmaAllocationBytes))
					.AppendLine();
			}
			stringBuilder.AppendLine();
		}
		stringBuilder.AppendLine("By category (Waylith allocations):");
		long num = 0L;
		GpuMemoryTracker.CategoryStat[] array = GpuMemoryTracker.Snapshot();
		for (int i = 0; i < array.Length; i++)
		{
			GpuMemoryTracker.CategoryStat categoryStat = array[i];
			if (categoryStat.Count != 0L || categoryStat.Bytes != 0L)
			{
				num += categoryStat.Bytes;
				stringBuilder.Append("  ").Append(categoryStat.Category).Append(": ")
					.Append(FormatBytes(categoryStat.Bytes))
					.Append(" (")
					.Append(categoryStat.Count)
					.Append(" allocations)")
					.AppendLine();
			}
		}
		stringBuilder.Append("  Tracked total: ").Append(FormatBytes(num)).AppendLine();
		stringBuilder.AppendLine();
		stringBuilder.AppendLine("By top-level label:");
		List<GpuMemoryTracker.TopLevelStat> list = new List<GpuMemoryTracker.TopLevelStat>();
		GpuMemoryTracker.CollectTopLevel(list);
		list.Sort((GpuMemoryTracker.TopLevelStat a, GpuMemoryTracker.TopLevelStat b) => b.Bytes.CompareTo(a.Bytes));
		foreach (GpuMemoryTracker.TopLevelStat item in list)
		{
			stringBuilder.Append("  ").Append(item.Label).Append(": ")
				.Append(FormatBytes(item.Bytes))
				.Append(" (")
				.Append(item.Count)
				.Append(" allocations)")
				.AppendLine();
		}
		stringBuilder.AppendLine();
		stringBuilder.AppendLine("Largest allocations:");
		List<GpuMemoryTracker.TrackedAllocation> list2 = new List<GpuMemoryTracker.TrackedAllocation>();
		GpuMemoryTracker.CollectAll(list2);
		list2.Sort((GpuMemoryTracker.TrackedAllocation a, GpuMemoryTracker.TrackedAllocation b) => b.Bytes.CompareTo(a.Bytes));
		for (int j = 0; j < System.Math.Min(40, list2.Count); j++)
		{
			GpuMemoryTracker.TrackedAllocation trackedAllocation = list2[j];
			stringBuilder.Append("  ").Append(FormatBytes(trackedAllocation.Bytes)).Append("  ")
				.Append(trackedAllocation.Category)
				.Append("  ")
				.Append(trackedAllocation.Name ?? "<unnamed>");
			if (trackedAllocation.Detail != null)
			{
				stringBuilder.Append("  (").Append(trackedAllocation.Detail).Append(')');
			}
			stringBuilder.AppendLine();
		}
		stringBuilder.AppendLine();
		allocator.CalculateStatistics(out var pStats);
		stringBuilder.AppendLine("VMA totals (vmaCalculateStatistics):");
		AppendDetailed(stringBuilder, "  Total", pStats.Total);
		stringBuilder.AppendLine();
		stringBuilder.AppendLine("By Vulkan memory type:");
		for (int k = 0; k < memoryProperties.MemoryTypeCount; k++)
		{
			VkMemoryType vkMemoryType = global::_003CPrivateImplementationDetails_003E.InlineArrayAsSpan<VkMemoryType_32, VkMemoryType>(ref memoryProperties.MemoryTypes, 32)[k];
			VmaDetailedStatistics vmaDetailedStatistics = global::_003CPrivateImplementationDetails_003E.InlineArrayAsSpan<VmaDetailedStatistics_32, VmaDetailedStatistics>(ref pStats.MemoryType, 32)[k];
			if (vmaDetailedStatistics.Statistics.AllocationCount != 0 || vmaDetailedStatistics.Statistics.BlockCount != 0)
			{
				stringBuilder.Append("  Type ").Append(k).Append(" (heap ")
					.Append(vkMemoryType.HeapIndex)
					.Append(", ")
					.Append(vkMemoryType.PropertyFlags)
					.Append("): ")
					.Append(FormatBytes((long)vmaDetailedStatistics.Statistics.AllocationBytes))
					.Append(" in ")
					.Append((int)vmaDetailedStatistics.Statistics.AllocationCount)
					.Append(" allocations, ")
					.Append(FormatBytes((long)vmaDetailedStatistics.Statistics.BlockBytes))
					.Append(" reserved")
					.AppendLine();
			}
		}
		if (includeVmaJson)
		{
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("=== VMA detailed JSON dump ===");
			stringBuilder.AppendLine(BuildVmaJson(allocator));
		}
		return stringBuilder.ToString();
	}

	private static void AppendDetailed(StringBuilder sb, string label, VmaDetailedStatistics detailed)
	{
		sb.Append(label).Append(": ").Append(FormatBytes((long)detailed.Statistics.AllocationBytes))
			.Append(" in ")
			.Append((int)detailed.Statistics.AllocationCount)
			.Append(" allocations across ")
			.Append((int)detailed.Statistics.BlockCount)
			.Append(" blocks (")
			.Append(FormatBytes((long)detailed.Statistics.BlockBytes))
			.Append(" reserved)")
			.AppendLine();
	}

	private unsafe static string BuildVmaJson(VmaAllocator allocator)
	{
		allocator.BuildStatsString(out var ppStatsString, true);
		try
		{
			return Marshal.PtrToStringUTF8((nint)ppStatsString.Pointer) ?? string.Empty;
		}
		finally
		{
			allocator.FreeStatsString(new ReadOnlySpan<byte>(ppStatsString.Pointer, 1));
		}
	}

	private static string UsageSource(in GpuMemorySummary summary)
	{
		if (!summary.DriverReported)
		{
			return "VMA estimate";
		}
		return "driver";
	}

	private static string LimitSuffix()
	{
		long num = _graphicsDevice?.VramLimitBytes ?? 0;
		if (num <= 0)
		{
			return string.Empty;
		}
		return " (capped to " + FormatBytes(num) + " by WAYLITH_VRAM_BUDGET_MB)";
	}

	public static string FormatBytes(long bytes)
	{
		if (bytes < 1073741824)
		{
			if (bytes < 1048576)
			{
				if (bytes < 1024)
				{
					return $"{bytes} B";
				}
				return $"{(double)bytes / 1024.0:0.0} KiB";
			}
			return $"{(double)bytes / 1048576.0:0.0} MiB";
		}
		return $"{(double)bytes / 1073741824.0:0.00} GiB";
	}
}
