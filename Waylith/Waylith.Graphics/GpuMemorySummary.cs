using System;
using System.Runtime.CompilerServices;

namespace Waylith.Graphics;

public readonly record struct GpuMemorySummary
{
	public long DeviceLocalDriverUsageBytes { get; init; }

	public long DeviceLocalBudgetBytes { get; init; }

	public long DeviceLocalTotalBytes { get; init; }

	public long DeviceLocalVmaAllocationBytes { get; init; }

	public long VmaAllocationBytes { get; init; }

	public long VmaBlockBytes { get; init; }

	public int VmaAllocationCount { get; init; }

	public GpuHeapInfo[] Heaps { get; init; }

	public long DeviceLocalFreeBytes => System.Math.Max(0L, DeviceLocalBudgetBytes - DeviceLocalDriverUsageBytes);

	public bool DriverReported { get; init; }

	public long DeviceLocalVmaBlockBytes
	{
		get
		{
			long num = 0L;
			GpuHeapInfo[] array = Heaps ?? Array.Empty<GpuHeapInfo>();
			for (int i = 0; i < array.Length; i++)
			{
				GpuHeapInfo gpuHeapInfo = array[i];
				if (gpuHeapInfo.DeviceLocal)
				{
					num += gpuHeapInfo.VmaBlockBytes;
				}
			}
			return num;
		}
	}

	public long HostVmaAllocationBytes
	{
		get
		{
			long num = 0L;
			GpuHeapInfo[] array = Heaps ?? Array.Empty<GpuHeapInfo>();
			for (int i = 0; i < array.Length; i++)
			{
				GpuHeapInfo gpuHeapInfo = array[i];
				if (!gpuHeapInfo.DeviceLocal)
				{
					num += gpuHeapInfo.VmaAllocationBytes;
				}
			}
			return num;
		}
	}

	public GpuMemorySummary(long DeviceLocalDriverUsageBytes, long DeviceLocalBudgetBytes, long DeviceLocalTotalBytes, long DeviceLocalVmaAllocationBytes, long VmaAllocationBytes, long VmaBlockBytes, int VmaAllocationCount, GpuHeapInfo[] Heaps)
	{
		DriverReported = false;
		this.DeviceLocalDriverUsageBytes = DeviceLocalDriverUsageBytes;
		this.DeviceLocalBudgetBytes = DeviceLocalBudgetBytes;
		this.DeviceLocalTotalBytes = DeviceLocalTotalBytes;
		this.DeviceLocalVmaAllocationBytes = DeviceLocalVmaAllocationBytes;
		this.VmaAllocationBytes = VmaAllocationBytes;
		this.VmaBlockBytes = VmaBlockBytes;
		this.VmaAllocationCount = VmaAllocationCount;
		this.Heaps = Heaps;
	}

	[CompilerGenerated]
	public void Deconstruct(out long DeviceLocalDriverUsageBytes, out long DeviceLocalBudgetBytes, out long DeviceLocalTotalBytes, out long DeviceLocalVmaAllocationBytes, out long VmaAllocationBytes, out long VmaBlockBytes, out int VmaAllocationCount, out GpuHeapInfo[] Heaps)
	{
		DeviceLocalDriverUsageBytes = this.DeviceLocalDriverUsageBytes;
		DeviceLocalBudgetBytes = this.DeviceLocalBudgetBytes;
		DeviceLocalTotalBytes = this.DeviceLocalTotalBytes;
		DeviceLocalVmaAllocationBytes = this.DeviceLocalVmaAllocationBytes;
		VmaAllocationBytes = this.VmaAllocationBytes;
		VmaBlockBytes = this.VmaBlockBytes;
		VmaAllocationCount = this.VmaAllocationCount;
		Heaps = this.Heaps;
	}
}
