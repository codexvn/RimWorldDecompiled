using System.Collections.Generic;
using Brutal.VulkanApi;
using Brutal.VulkanApi.Abstractions;

namespace Waylith.Rendering;

public sealed class PendingTextureBatch(CommandBuffer commandBuffer)
{
	public CommandBuffer CommandBuffer { get; } = commandBuffer;

	public List<BufferAllocationInfo> StagingBuffers { get; } = new List<BufferAllocationInfo>();

	public HashSet<(GpuTextureAllocation texture, int mipLevel)> TouchedSubresources { get; } = new HashSet<(GpuTextureAllocation, int)>();

	public long StagingBytes { get; set; }
}
