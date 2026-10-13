using System;
using Brutal.Memory;
using Brutal.VulkanApi;

namespace Waylith.Graphics;

public interface IRenderContext : IRenderTarget, IRenderFrameState, IDisposable
{
	void PollEvents();

	FrameAcquireResult TryAcquireNextFrame();

	FrameResult TrySubmitFrame();

	FrameResult TrySubmitFrame(Span<VkSemaphore> additionalWaitSemaphores, Span<VkPipelineStageFlags> additionalWaitStages, Span<VkSemaphore> additionalSignalSemaphores);

	PtrOwner<VkPipelineViewportStateCreateInfo> GetViewportState();

	void Rebuild();
}
