using Brutal.VulkanApi;

namespace Waylith.Graphics;

public interface IRenderTarget
{
	GraphicsDevice GraphicsDevice { get; }

	VkSampleCountFlags SampleCount { get; }

	VkFormat ColorFormat { get; }

	VkFormat DepthFormat { get; }

	VkRenderPass GetRenderPass();

	VkExtent2D GetExtent();
}
