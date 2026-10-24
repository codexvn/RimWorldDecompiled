using System;
using Brutal.VulkanApi;
using Brutal.VulkanApi.Abstractions;
using Waylith.Math;

namespace Waylith.Rendering;

public static class RenderTextureMaker
{
	private const VkPipelineStageFlags UseStages = VkPipelineStageFlags.FragmentShaderBit | VkPipelineStageFlags.EarlyFragmentTestsBit | VkPipelineStageFlags.LateFragmentTestsBit | VkPipelineStageFlags.ColorAttachmentOutputBit | VkPipelineStageFlags.TransferBit;

	private const VkPipelineStageFlags FirstWriteStages = VkPipelineStageFlags.FragmentShaderBit | VkPipelineStageFlags.EarlyFragmentTestsBit | VkPipelineStageFlags.LateFragmentTestsBit | VkPipelineStageFlags.ColorAttachmentOutputBit | VkPipelineStageFlags.TransferBit;

	private const VkAccessFlags ColorWrites = VkAccessFlags.ColorAttachmentWriteBit | VkAccessFlags.TransferWriteBit;

	private const VkAccessFlags DepthWrites = VkAccessFlags.DepthStencilAttachmentWriteBit | VkAccessFlags.TransferWriteBit;

	public static void RecordReuseBarrier(CommandBuffer cmd, ReadOnlySpan<RenderTexture> targets)
	{
		Span<VkImageMemoryBarrier> span = stackalloc VkImageMemoryBarrier[targets.Length * 2];
		int length = 0;
		ReadOnlySpan<RenderTexture> readOnlySpan = targets;
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			RenderTexture renderTexture = readOnlySpan[i];
			if (renderTexture.ColorFormat != 0)
			{
				span[length++] = ColorBarrier(renderTexture, VkAccessFlags.ColorAttachmentWriteBit | VkAccessFlags.TransferWriteBit, VkAccessFlags.ColorAttachmentWriteBit | VkAccessFlags.TransferWriteBit, VkImageLayout.Undefined, VkImageLayout.ColorAttachmentOptimal);
			}
			if (renderTexture.HasDepth)
			{
				span[length++] = DepthBarrier(renderTexture, VkAccessFlags.DepthStencilAttachmentWriteBit | VkAccessFlags.TransferWriteBit, VkAccessFlags.DepthStencilAttachmentWriteBit | VkAccessFlags.TransferWriteBit, VkImageLayout.Undefined, VkImageLayout.DepthStencilAttachmentOptimal);
			}
		}
		cmd.PipelineBarrier(VkPipelineStageFlags.FragmentShaderBit | VkPipelineStageFlags.EarlyFragmentTestsBit | VkPipelineStageFlags.LateFragmentTestsBit | VkPipelineStageFlags.ColorAttachmentOutputBit | VkPipelineStageFlags.TransferBit, VkPipelineStageFlags.FragmentShaderBit | VkPipelineStageFlags.EarlyFragmentTestsBit | VkPipelineStageFlags.LateFragmentTestsBit | VkPipelineStageFlags.ColorAttachmentOutputBit | VkPipelineStageFlags.TransferBit, VkDependencyFlags.None, default(ReadOnlySpan<VkMemoryBarrier>), default(ReadOnlySpan<VkBufferMemoryBarrier>), span.Slice(0, length));
	}

	public static bool EnsureSized(ref RenderTexture? texture, in RenderTextureDesc desc, int width, int height)
	{
		width = math.max(1, width);
		height = math.max(1, height);
		if (texture == null)
		{
			texture = new RenderTexture(width, height, desc.ColorFormat, desc.DepthFormat, desc.Filter, RenderTextureWrap.Clamp, randomWrite: false, desc.Name);
			return true;
		}
		if (texture.Width == width && texture.Height == height)
		{
			return false;
		}
		texture.Resize(width, height);
		return true;
	}

	public static void Dispose(ref RenderTexture? texture)
	{
		texture?.Dispose();
		texture = null;
	}

	public static PassAttachment Attachment(VkRenderPass renderPass, RenderTexture texture, VkClearValue[] clearValues)
	{
		return new PassAttachment(renderPass, new VkFramebuffer[1] { texture.GetFramebuffer(renderPass) }, new VkImageView[1] { texture.DepthView }, texture.Extent, clearValues, VkSampleCountFlags._1Bit, texture.ColorFormat, texture.DepthFormat);
	}

	public unsafe static VkFramebuffer CreateFramebuffer(DeviceEx device, VkRenderPass renderPass, VkExtent2D extent, ReadOnlySpan<VkImageView> attachments)
	{
		fixed (VkImageView* attachments2 = attachments)
		{
			VkFramebufferCreateInfo pCreateInfo = new VkFramebufferCreateInfo
			{
				RenderPass = renderPass,
				AttachmentCount = attachments.Length,
				Attachments = attachments2,
				Width = extent.Width,
				Height = extent.Height,
				Layers = 1
			};
			return device.CreateFramebuffer(in pCreateInfo, device.VkAllocator);
		}
	}

	public static void DestroyFramebuffer(DeviceEx device, ref VkFramebuffer framebuffer)
	{
		if (!framebuffer.IsNull())
		{
			device.DestroyFramebuffer(framebuffer, device.VkAllocator);
			framebuffer = default(VkFramebuffer);
		}
	}

	public static VkImageMemoryBarrier ColorBarrier(RenderTexture texture, VkAccessFlags srcAccess, VkAccessFlags dstAccess, VkImageLayout oldLayout, VkImageLayout newLayout)
	{
		return ImageBarrier(texture.Color.Image, VkImageAspectFlags.ColorBit, srcAccess, dstAccess, oldLayout, newLayout);
	}

	public static VkImageMemoryBarrier DepthBarrier(RenderTexture texture, VkAccessFlags srcAccess, VkAccessFlags dstAccess, VkImageLayout oldLayout, VkImageLayout newLayout)
	{
		return ImageBarrier(texture.Depth.Image, RenderTexture.DepthAspect(texture.DepthFormat), srcAccess, dstAccess, oldLayout, newLayout);
	}

	public static VkImageMemoryBarrier ImageBarrier(VkImage image, VkImageAspectFlags aspect, VkAccessFlags srcAccess, VkAccessFlags dstAccess, VkImageLayout oldLayout, VkImageLayout newLayout)
	{
		VkImageMemoryBarrier result = new VkImageMemoryBarrier();
		result.SrcAccessMask = srcAccess;
		result.DstAccessMask = dstAccess;
		result.OldLayout = oldLayout;
		result.NewLayout = newLayout;
		result.SrcQueueFamilyIndex = -1;
		result.DstQueueFamilyIndex = -1;
		result.Image = image;
		result.SubresourceRange = new VkImageSubresourceRange
		{
			AspectMask = aspect,
			BaseMipLevel = 0,
			LevelCount = 1,
			BaseArrayLayer = 0,
			LayerCount = 1
		};
		return result;
	}
}
