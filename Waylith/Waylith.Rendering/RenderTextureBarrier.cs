using Brutal.VulkanApi;

namespace Waylith.Rendering;

public readonly struct RenderTextureBarrier
{
	public VkImageLayout Layout { get; }

	public VkAccessFlags Access { get; }

	public VkPipelineStageFlags Stage { get; }

	public static RenderTextureBarrier Sampleable => new RenderTextureBarrier(VkImageLayout.ShaderReadOnlyOptimal, VkAccessFlags.ShaderReadBit, VkPipelineStageFlags.FragmentShaderBit);

	public static RenderTextureBarrier TransferSource => new RenderTextureBarrier(VkImageLayout.TransferSrcOptimal, VkAccessFlags.TransferReadBit, VkPipelineStageFlags.TransferBit);

	public RenderTextureBarrier(VkImageLayout layout, VkAccessFlags access, VkPipelineStageFlags stage)
	{
		Layout = layout;
		Access = access;
		Stage = stage;
	}
}
