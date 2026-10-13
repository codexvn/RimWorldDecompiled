using System;
using Brutal.VulkanApi;
using Brutal.VulkanApi.Abstractions;

namespace Waylith.Graphics;

public readonly struct AcquiredFrame
{
	public FrameResources Resources => _003CinFrameResources_003EP ?? throw new NullReferenceException("inFrameResources");

	public CommandBuffer CommandBuffer => _003CinCommandBuffer_003EP;

	public AcquiredFrame(FrameResources? inFrameResources, CommandBuffer inCommandBuffer)
	{
		_003CinFrameResources_003EP = inFrameResources;
		_003CinCommandBuffer_003EP = inCommandBuffer;
	}

	public void Deconstruct(out FrameResources resources, out CommandBuffer commandBuffer)
	{
		resources = Resources;
		commandBuffer = CommandBuffer;
	}
}
