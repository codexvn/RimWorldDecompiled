using Brutal.VulkanApi;

namespace Waylith.Rendering;

public sealed class PassAttachment
{
	public VkRenderPass RenderPass { get; }

	public VkFramebuffer[] Framebuffers { get; }

	public VkImageView[] DepthViews { get; }

	public VkExtent2D Extent { get; }

	public VkClearValue[] ClearValues { get; }

	public VkSampleCountFlags SampleCount { get; }

	public VkFormat ColorFormat { get; }

	public VkFormat DepthFormat { get; }

	public PassAttachment(VkRenderPass renderPass, VkFramebuffer[] framebuffers, VkImageView[] depthViews, VkExtent2D extent, VkClearValue[] clearValues, VkSampleCountFlags sampleCount, VkFormat colorFormat, VkFormat depthFormat)
	{
		RenderPass = renderPass;
		Framebuffers = framebuffers;
		DepthViews = depthViews;
		Extent = extent;
		ClearValues = clearValues;
		SampleCount = sampleCount;
		ColorFormat = colorFormat;
		DepthFormat = depthFormat;
	}

	public FrameContext MakeFrameContext(in FrameContext main)
	{
		return new FrameContext(main.RenderContext, main.Resources, main.CommandBuffer, RenderPass, Framebuffers[main.SubmissionFrameIndex % Framebuffers.Length], Extent, SampleCount, ColorFormat, DepthFormat);
	}
}
