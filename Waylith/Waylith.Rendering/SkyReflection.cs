using System;
using Brutal.VulkanApi;
using Waylith.Graphics;

namespace Waylith.Rendering;

public static class SkyReflection
{
	private static readonly int TextureId = Shader.PropertyToID("_SkyReflection");

	private static readonly Texture2D Texture = new Texture2D(1, 1)
	{
		Name = "SkyReflection",
		filterMode = FilterMode.Bilinear,
		wrapMode = TextureWrapMode.Clamp
	};

	public static RenderTexture? Target { get; private set; }

	public static void SetSize(GraphicsDevice graphicsDevice, int width, int height)
	{
		if (Target == null)
		{
			Target = new RenderTexture(1, 1, VkFormat.R8G8B8A8UNorm, VkFormat.D32SFloat, RenderTextureFilter.Bilinear, RenderTextureWrap.Clamp, randomWrite: false, "SkyReflection");
		}
		Target.Resize(System.Math.Max(1, width), System.Math.Max(1, height));
		Texture.width = Target.Width;
		Texture.height = Target.Height;
		Texture.GpuResource = Target.Color;
		GlobalShaderState.SetTexture(TextureId, Texture);
		MakeSampleable(graphicsDevice);
	}

	private unsafe static void MakeSampleable(GraphicsDevice graphicsDevice)
	{
		GpuTextureAllocation color = Target.Color;
		if (color.CurrentLayout != VkImageLayout.ShaderReadOnlyOptimal)
		{
			graphicsDevice.ExecuteOneShot(delegate(CommandBuffer cmd)
			{
				VkImageSubresourceRange vkImageSubresourceRange = new VkImageSubresourceRange
				{
					AspectMask = VkImageAspectFlags.ColorBit,
					BaseMipLevel = 0,
					LevelCount = 1,
					BaseArrayLayer = 0,
					LayerCount = 1
				};
				VkImageMemoryBarrier reference = new VkImageMemoryBarrier
				{
					DstAccessMask = VkAccessFlags.TransferWriteBit,
					OldLayout = VkImageLayout.Undefined,
					NewLayout = VkImageLayout.TransferDstOptimal,
					SrcQueueFamilyIndex = -1,
					DstQueueFamilyIndex = -1,
					Image = color.Image,
					SubresourceRange = vkImageSubresourceRange
				};
				cmd.PipelineBarrier(VkPipelineStageFlags.TopOfPipeBit, VkPipelineStageFlags.TransferBit, VkDependencyFlags.None, default(ReadOnlySpan<VkMemoryBarrier>), default(ReadOnlySpan<VkBufferMemoryBarrier>), new ReadOnlySpan<VkImageMemoryBarrier>(in reference));
				VkClearColorValue vkClearColorValue = new VkClearColorValue();
				VkImage image = color.Image;
				VkClearColorValue* pColor = &vkClearColorValue;
				VkImageSubresourceRange reference2 = vkImageSubresourceRange;
				cmd.ClearColorImage(image, VkImageLayout.TransferDstOptimal, pColor, new ReadOnlySpan<VkImageSubresourceRange>(in reference2));
				VkImageMemoryBarrier reference3 = new VkImageMemoryBarrier
				{
					SrcAccessMask = VkAccessFlags.TransferWriteBit,
					DstAccessMask = VkAccessFlags.ShaderReadBit,
					OldLayout = VkImageLayout.TransferDstOptimal,
					NewLayout = VkImageLayout.ShaderReadOnlyOptimal,
					SrcQueueFamilyIndex = -1,
					DstQueueFamilyIndex = -1,
					Image = color.Image,
					SubresourceRange = vkImageSubresourceRange
				};
				cmd.PipelineBarrier(VkPipelineStageFlags.TransferBit, VkPipelineStageFlags.FragmentShaderBit, VkDependencyFlags.None, default(ReadOnlySpan<VkMemoryBarrier>), default(ReadOnlySpan<VkBufferMemoryBarrier>), new ReadOnlySpan<VkImageMemoryBarrier>(in reference3));
			});
			color.CurrentLayout = VkImageLayout.ShaderReadOnlyOptimal;
		}
	}

	public static void Dispose()
	{
		Texture.GpuResource = null;
		Target?.Dispose();
		Target = null;
	}
}
