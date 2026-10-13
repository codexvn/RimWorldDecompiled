using Brutal.VulkanApi;
using Brutal.VulkanApi.Abstractions;
using Waylith.Graphics;

namespace Waylith.Rendering;

public readonly struct FrameContext
{
	private readonly bool hasOverrides;

	private readonly VkRenderPass overrideRenderPass;

	private readonly VkFramebuffer overrideFramebuffer;

	private readonly VkExtent2D overrideExtent;

	private readonly VkSampleCountFlags overrideSampleCount;

	private readonly VkFormat overrideColorFormat;

	private readonly VkFormat overrideDepthFormat;

	public IRenderContext RenderContext { get; }

	public GraphicsDevice GraphicsDevice => RenderContext.GraphicsDevice;

	public IRenderTarget Target => RenderContext;

	public FrameResources Resources { get; }

	public CommandBuffer CommandBuffer { get; }

	public DeviceEx Device => RenderContext.Device;

	public VkRenderPass RenderPass
	{
		get
		{
			if (!hasOverrides)
			{
				return RenderContext.GetRenderPass();
			}
			return overrideRenderPass;
		}
	}

	public VkFramebuffer Framebuffer
	{
		get
		{
			if (!hasOverrides)
			{
				return Resources.Framebuffer;
			}
			return overrideFramebuffer;
		}
	}

	public VkExtent2D Extent
	{
		get
		{
			if (!hasOverrides)
			{
				return RenderContext.GetExtent();
			}
			return overrideExtent;
		}
	}

	public int FrameIndex => RenderContext.FrameIndex;

	public int SubmissionFrameIndex => RenderContext.SubmissionFrameIndex;

	public VkSampleCountFlags SampleCount
	{
		get
		{
			if (!hasOverrides)
			{
				return RenderContext.SampleCount;
			}
			return overrideSampleCount;
		}
	}

	public VkFormat ColorFormat
	{
		get
		{
			if (!hasOverrides)
			{
				return RenderContext.ColorFormat;
			}
			return overrideColorFormat;
		}
	}

	public VkFormat DepthFormat
	{
		get
		{
			if (!hasOverrides)
			{
				return RenderContext.DepthFormat;
			}
			return overrideDepthFormat;
		}
	}

	public FrameContext(IRenderContext renderContext, FrameResources resources, CommandBuffer commandBuffer)
	{
		RenderContext = renderContext;
		Resources = resources;
		CommandBuffer = commandBuffer;
		hasOverrides = false;
		overrideRenderPass = default(VkRenderPass);
		overrideFramebuffer = default(VkFramebuffer);
		overrideExtent = default(VkExtent2D);
		overrideSampleCount = VkSampleCountFlags.None;
		overrideColorFormat = VkFormat.Undefined;
		overrideDepthFormat = VkFormat.Undefined;
	}

	public FrameContext(IRenderContext renderContext, FrameResources resources, CommandBuffer commandBuffer, VkRenderPass renderPass, VkFramebuffer framebuffer, VkExtent2D extent, VkSampleCountFlags sampleCount, VkFormat colorFormat, VkFormat depthFormat)
	{
		RenderContext = renderContext;
		Resources = resources;
		CommandBuffer = commandBuffer;
		hasOverrides = true;
		overrideRenderPass = renderPass;
		overrideFramebuffer = framebuffer;
		overrideExtent = extent;
		overrideSampleCount = sampleCount;
		overrideColorFormat = colorFormat;
		overrideDepthFormat = depthFormat;
	}
}
