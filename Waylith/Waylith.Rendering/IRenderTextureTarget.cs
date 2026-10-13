using Brutal.VulkanApi;

namespace Waylith.Rendering;

public interface IRenderTextureTarget
{
	VkExtent2D Extent { get; }

	GpuTextureAllocation Color { get; }

	VkFramebuffer GetFramebuffer(VkRenderPass renderPass);
}
