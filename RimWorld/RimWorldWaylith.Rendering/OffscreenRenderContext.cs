using System;
using Brutal.Memory;
using Brutal.VulkanApi;
using Brutal.VulkanApi.Abstractions;
using Waylith.Graphics;

namespace RimWorldWaylith.Rendering;

internal sealed class OffscreenRenderContext : IRenderContext, IRenderTarget, IRenderFrameState, IDisposable
{
	public const VkFormat DepthAttachmentFormat = VkFormat.D32SFloatS8UInt;

	private readonly IRenderContext _real;

	private readonly VkRenderPass _renderPass;

	private readonly VkExtent2D _extent;

	public GraphicsDevice GraphicsDevice => _real.GraphicsDevice;

	public VkSampleCountFlags SampleCount => VkSampleCountFlags._1Bit;

	public VkFormat ColorFormat => VkFormat.R8G8B8A8UNorm;

	public VkFormat DepthFormat => VkFormat.D32SFloatS8UInt;

	public DeviceEx Device => _real.Device;

	public int FrameIndex => _real.FrameIndex;

	public int SubmissionFrameIndex => _real.SubmissionFrameIndex;

	public int FramesInFlight => _real.FramesInFlight;

	public int ImageCount => _real.ImageCount;

	public FrameResources[] FrameResources => _real.FrameResources;

	public OffscreenRenderContext(IRenderContext real, VkRenderPass renderPass, VkExtent2D extent)
	{
		_real = real;
		_renderPass = renderPass;
		_extent = extent;
	}

	public VkRenderPass GetRenderPass()
	{
		return _renderPass;
	}

	public VkExtent2D GetExtent()
	{
		return _extent;
	}

	public Device GetDevice()
	{
		return _real.GetDevice();
	}

	public VkCommandPool GetCommandPool()
	{
		return _real.GetCommandPool();
	}

	public void PollEvents()
	{
		throw new NotSupportedException("OffscreenRenderContext has no swapchain to poll.");
	}

	public FrameAcquireResult TryAcquireNextFrame()
	{
		throw new NotSupportedException("OffscreenRenderContext has no swapchain to acquire from.");
	}

	public FrameResult TrySubmitFrame()
	{
		throw new NotSupportedException("OffscreenRenderContext does not submit frames; the host context owns submission.");
	}

	public FrameResult TrySubmitFrame(Span<VkSemaphore> w, Span<VkPipelineStageFlags> ws, Span<VkSemaphore> s)
	{
		throw new NotSupportedException("OffscreenRenderContext does not submit frames; the host context owns submission.");
	}

	public PtrOwner<VkPipelineViewportStateCreateInfo> GetViewportState()
	{
		throw new NotSupportedException("OffscreenRenderContext uses dynamic viewport state set per draw.");
	}

	public void Rebuild()
	{
		throw new NotSupportedException("OffscreenRenderContext has no swapchain to rebuild.");
	}

	public void Dispose()
	{
	}
}
