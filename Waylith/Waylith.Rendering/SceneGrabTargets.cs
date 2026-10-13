using System;
using System.Collections.Generic;
using Brutal.VulkanApi;

namespace Waylith.Rendering;

public sealed class SceneGrabTargets(VkFormat colorFormat) : IDisposable
{
	private readonly Dictionary<SceneGrabPoint, RenderTexture> targets = new Dictionary<SceneGrabPoint, RenderTexture>();

	private int width = 1;

	private int height = 1;

	public void SetExtent(int w, int h)
	{
		w = System.Math.Max(1, w);
		h = System.Math.Max(1, h);
		if (width == w && height == h)
		{
			return;
		}
		width = w;
		height = h;
		foreach (var (point, renderTexture2) in targets)
		{
			renderTexture2.Resize(w, h);
			AttachToPoint(point, renderTexture2);
		}
	}

	public void RecordGrab(CommandBuffer cmd, SceneGrabPoint point, VkImage sceneImage)
	{
		RenderTexture orCreateTarget = GetOrCreateTarget(point);
		if (point.Texture.GpuResource != orCreateTarget.Color)
		{
			AttachToPoint(point, orCreateTarget);
		}
		VkImage image = orCreateTarget.Color.Image;
		VkImageSubresourceRange vkImageSubresourceRange = new VkImageSubresourceRange();
		vkImageSubresourceRange.AspectMask = VkImageAspectFlags.ColorBit;
		vkImageSubresourceRange.BaseMipLevel = 0;
		vkImageSubresourceRange.LevelCount = 1;
		vkImageSubresourceRange.BaseArrayLayer = 0;
		vkImageSubresourceRange.LayerCount = 1;
		VkImageSubresourceRange subresourceRange = vkImageSubresourceRange;
		VkImageLayout currentLayout = orCreateTarget.Color.CurrentLayout;
		VkImageMemoryBarrier[] array = new VkImageMemoryBarrier[2]
		{
			new VkImageMemoryBarrier
			{
				SrcAccessMask = VkAccessFlags.ColorAttachmentWriteBit,
				DstAccessMask = VkAccessFlags.TransferReadBit,
				OldLayout = VkImageLayout.ShaderReadOnlyOptimal,
				NewLayout = VkImageLayout.TransferSrcOptimal,
				SrcQueueFamilyIndex = -1,
				DstQueueFamilyIndex = -1,
				Image = sceneImage,
				SubresourceRange = subresourceRange
			},
			new VkImageMemoryBarrier
			{
				SrcAccessMask = ((currentLayout != 0) ? VkAccessFlags.ShaderReadBit : VkAccessFlags.None),
				DstAccessMask = VkAccessFlags.TransferWriteBit,
				OldLayout = currentLayout,
				NewLayout = VkImageLayout.TransferDstOptimal,
				SrcQueueFamilyIndex = -1,
				DstQueueFamilyIndex = -1,
				Image = image,
				SubresourceRange = subresourceRange
			}
		};
		cmd.PipelineBarrier(VkPipelineStageFlags.FragmentShaderBit | VkPipelineStageFlags.ColorAttachmentOutputBit, VkPipelineStageFlags.TransferBit, VkDependencyFlags.None, default(ReadOnlySpan<VkMemoryBarrier>), default(ReadOnlySpan<VkBufferMemoryBarrier>), array);
		VkImageCopy reference = new VkImageCopy
		{
			SrcSubresource = new VkImageSubresourceLayers
			{
				AspectMask = VkImageAspectFlags.ColorBit,
				MipLevel = 0,
				BaseArrayLayer = 0,
				LayerCount = 1
			},
			SrcOffset = new VkOffset3D(0, 0, 0),
			DstSubresource = new VkImageSubresourceLayers
			{
				AspectMask = VkImageAspectFlags.ColorBit,
				MipLevel = 0,
				BaseArrayLayer = 0,
				LayerCount = 1
			},
			DstOffset = new VkOffset3D(0, 0, 0),
			Extent = new VkExtent3D(orCreateTarget.Width, orCreateTarget.Height, 1)
		};
		cmd.CopyImage(sceneImage, VkImageLayout.TransferSrcOptimal, image, VkImageLayout.TransferDstOptimal, new ReadOnlySpan<VkImageCopy>(in reference));
		VkImageMemoryBarrier[] array2 = new VkImageMemoryBarrier[2]
		{
			new VkImageMemoryBarrier
			{
				SrcAccessMask = VkAccessFlags.TransferReadBit,
				DstAccessMask = (VkAccessFlags.ColorAttachmentReadBit | VkAccessFlags.ColorAttachmentWriteBit),
				OldLayout = VkImageLayout.TransferSrcOptimal,
				NewLayout = VkImageLayout.ShaderReadOnlyOptimal,
				SrcQueueFamilyIndex = -1,
				DstQueueFamilyIndex = -1,
				Image = sceneImage,
				SubresourceRange = subresourceRange
			},
			new VkImageMemoryBarrier
			{
				SrcAccessMask = VkAccessFlags.TransferWriteBit,
				DstAccessMask = VkAccessFlags.ShaderReadBit,
				OldLayout = VkImageLayout.TransferDstOptimal,
				NewLayout = VkImageLayout.ShaderReadOnlyOptimal,
				SrcQueueFamilyIndex = -1,
				DstQueueFamilyIndex = -1,
				Image = image,
				SubresourceRange = subresourceRange
			}
		};
		cmd.PipelineBarrier(VkPipelineStageFlags.TransferBit, VkPipelineStageFlags.FragmentShaderBit | VkPipelineStageFlags.ColorAttachmentOutputBit, VkDependencyFlags.None, default(ReadOnlySpan<VkMemoryBarrier>), default(ReadOnlySpan<VkBufferMemoryBarrier>), array2);
		orCreateTarget.Color.CurrentLayout = VkImageLayout.ShaderReadOnlyOptimal;
	}

	private RenderTexture GetOrCreateTarget(SceneGrabPoint point)
	{
		if (targets.TryGetValue(point, out RenderTexture value))
		{
			return value;
		}
		RenderTexture renderTexture = new RenderTexture(width, height, colorFormat, VkFormat.Undefined, RenderTextureFilter.Bilinear, RenderTextureWrap.Clamp, randomWrite: false, "SceneGrab_" + point.Name);
		targets[point] = renderTexture;
		AttachToPoint(point, renderTexture);
		return renderTexture;
	}

	private static void AttachToPoint(SceneGrabPoint point, RenderTexture target)
	{
		point.Texture.width = target.Width;
		point.Texture.height = target.Height;
		point.Texture.GpuResource = target.Color;
	}

	public void Dispose()
	{
		foreach (var (sceneGrabPoint2, renderTexture2) in targets)
		{
			sceneGrabPoint2.Texture.GpuResource = null;
			renderTexture2.Dispose();
		}
		targets.Clear();
	}
}
