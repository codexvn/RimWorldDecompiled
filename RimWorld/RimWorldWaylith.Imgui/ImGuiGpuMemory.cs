using Brutal;
using Brutal.VulkanApi;
using Brutal.VulkanApi.Abstractions;
using Brutal.VulkanApi.Vma;
using Waylith.Graphics;

namespace RimWorldWaylith.Imgui;

internal static class ImGuiGpuMemory
{
	public const string TrackerLabel = "UI/ImGui";

	public static BufferAllocationInfo CreateHostBuffer(VmaAllocator allocator, string name, ByteSize size, VkBufferUsageFlags usage, GpuMemoryCategory category)
	{
		VkBufferCreateInfo bufferCreateInfo = new VkBufferCreateInfo
		{
			Size = size,
			Usage = usage,
			SharingMode = VkSharingMode.Exclusive
		};
		AllocationInfo.CreateInfo allocateCreateInfo = new AllocationInfo.CreateInfo
		{
			Preference = MemoryPreference.PreferCpu,
			RequiredProperties = (VkMemoryPropertyFlags.HostVisibleBit | VkMemoryPropertyFlags.HostCoherentBit),
			PreferredProperties = (VkMemoryPropertyFlags.HostVisibleBit | VkMemoryPropertyFlags.HostCoherentBit),
			Priority = 1f
		};
		BufferAllocationInfo allocation = allocator.AllocateBufferChecked(name, in bufferCreateInfo, in allocateCreateInfo);
		GpuMemoryTracker.Add(category, in allocation, "UI/ImGui", name);
		return allocation;
	}

	public static void Free(VmaAllocator allocator, ref BufferAllocationInfo buffer, GpuMemoryCategory category)
	{
		if (!buffer.VkBuffer.IsNull())
		{
			GpuMemoryTracker.Remove(category, in buffer);
			allocator.FreeAllocation(in buffer);
			buffer = default(BufferAllocationInfo);
		}
	}

	public unsafe static byte* Map(VmaAllocator allocator, in BufferAllocationInfo buffer)
	{
		return (byte*)(nint)allocator.MapMemory(new AllocationMapInfo(buffer.AllocationInfo));
	}

	public static void Unmap(VmaAllocator allocator, in BufferAllocationInfo buffer)
	{
		allocator.UnmapMemory(new AllocationMapInfo(buffer.AllocationInfo));
	}
}
