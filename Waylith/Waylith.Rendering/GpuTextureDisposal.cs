using Brutal.VulkanApi;
using Brutal.VulkanApi.Abstractions;
using Brutal.VulkanApi.Vma;
using Waylith.Graphics;

namespace Waylith.Rendering;

public readonly record struct GpuTextureDisposal(DeviceEx Device, VmaAllocator Allocator, ImageAllocationInfo Allocation, VkImageView ImageView, GpuMemoryCategory Category)
{
	public void DisposeNow()
	{
		if (!ImageView.IsNull())
		{
			Device.DestroyImageView(ImageView, Device.VkAllocator);
		}
		if (!Allocation.MemAllocationInfo.Memory.IsNull())
		{
			GpuMemoryTracker.Remove(Category, Allocation);
			Allocator.FreeAllocation(Allocation);
		}
	}
}
