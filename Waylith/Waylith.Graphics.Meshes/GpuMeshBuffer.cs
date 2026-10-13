using System;
using Brutal.VulkanApi;
using Brutal.VulkanApi.Abstractions;

namespace Waylith.Graphics.Meshes;

public sealed class GpuMeshBuffer
{
	public BufferAllocationInfo Allocation { get; private set; }

	public AllocationMapInfo? MapInfo { get; private set; }

	public nint MappedPointer { get; private set; }

	public uint SizeBytes { get; private set; }

	public VkBufferUsageFlags Usage { get; private set; }

	public VkBuffer Buffer => Allocation.VkBuffer;

	public bool IsAllocated => !Allocation.AllocationInfo.Memory.IsNull();

	public bool IsMapped
	{
		get
		{
			if (MappedPointer != IntPtr.Zero)
			{
				return MapInfo.HasValue;
			}
			return false;
		}
	}

	public void Initialize(BufferAllocationInfo allocation, uint sizeBytes, VkBufferUsageFlags usage, AllocationMapInfo? mapInfo = null, nint mappedPointer = 0, string? trackingLabel = null)
	{
		Allocation = allocation;
		SizeBytes = sizeBytes;
		Usage = usage;
		MapInfo = mapInfo;
		MappedPointer = mappedPointer;
		GpuMemoryTracker.Add(CategoryFor(usage), in allocation, trackingLabel);
	}

	private static GpuMemoryCategory CategoryFor(VkBufferUsageFlags usage)
	{
		if ((usage & VkBufferUsageFlags.IndexBufferBit) == 0)
		{
			return GpuMemoryCategory.MeshVertex;
		}
		return GpuMemoryCategory.MeshIndex;
	}

	public void Release(IBufferAllocator allocator)
	{
		if (IsAllocated)
		{
			BufferAllocationInfo allocation = Allocation;
			AllocationMapInfo? mapInfo = MapInfo;
			nint mappedPointer = MappedPointer;
			GpuMemoryTracker.Remove(CategoryFor(Usage), in allocation);
			Allocation = default(BufferAllocationInfo);
			MapInfo = null;
			MappedPointer = IntPtr.Zero;
			SizeBytes = 0u;
			Usage = VkBufferUsageFlags.None;
			if (mappedPointer != IntPtr.Zero && mapInfo.HasValue)
			{
				allocator.UnmapMemory(mapInfo.Value);
			}
			allocator.FreeAllocation(in allocation);
		}
	}
}
