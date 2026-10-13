using System;
using System.Runtime.CompilerServices;
using Brutal;
using Brutal.Numerics;
using Brutal.VulkanApi;
using Brutal.VulkanApi.Abstractions;
using Waylith.Graphics;
using Waylith.Rendering;
using Waylith.Rendering.PostFx;

namespace RimWorldWaylith.Rendering;

public sealed class SceneColorTargets : IDisposable
{
	private readonly DeviceEx device;

	private readonly VkFormat colorFormat;

	private readonly VkFormat depthFormat;

	private readonly VkRenderPass renderPass;

	private readonly VkRenderPass loadRenderPass;

	private readonly VkRenderPass backdropRenderPass;

	private readonly string name;

	private RenderTexture[] targets = Array.Empty<RenderTexture>();

	private PassAttachment? attachment;

	private PassAttachment? loadAttachment;

	private PassAttachment? backdropAttachment;

	private int width;

	private int height;

	public VkRenderPass RenderPass => renderPass;

	public PassAttachment? Attachment => attachment;

	public PassAttachment? LoadAttachment => loadAttachment;

	public PassAttachment? BackdropAttachment => backdropAttachment;

	public SceneColorTargets(GraphicsDevice graphicsDevice, VkFormat colorFormat, VkFormat depthFormat, string name = "SceneColor")
	{
		device = graphicsDevice.Device;
		this.colorFormat = colorFormat;
		this.depthFormat = depthFormat;
		this.name = name;
		renderPass = PostFxPassHelpers.CreateColorRenderPass(device, colorFormat, depthFormat, VkAttachmentLoadOp.Clear);
		loadRenderPass = PostFxPassHelpers.CreateLoadColorDepthRenderPass(device, colorFormat, depthFormat);
		backdropRenderPass = PostFxPassHelpers.CreateColorRenderPass(device, colorFormat, depthFormat, VkAttachmentLoadOp.Load);
	}

	public RenderTexture Current(int frame)
	{
		return targets[frame];
	}

	public void RecordSampleBarrier(CommandBuffer cmd, int frame)
	{
		if (Platform.IsMacOS && (uint)frame < (uint)targets.Length)
		{
			VkImageMemoryBarrier vkImageMemoryBarrier = new VkImageMemoryBarrier();
			vkImageMemoryBarrier.SrcAccessMask = VkAccessFlags.ColorAttachmentWriteBit;
			vkImageMemoryBarrier.DstAccessMask = VkAccessFlags.ShaderReadBit;
			vkImageMemoryBarrier.OldLayout = VkImageLayout.ShaderReadOnlyOptimal;
			vkImageMemoryBarrier.NewLayout = VkImageLayout.ShaderReadOnlyOptimal;
			vkImageMemoryBarrier.SrcQueueFamilyIndex = -1;
			vkImageMemoryBarrier.DstQueueFamilyIndex = -1;
			vkImageMemoryBarrier.Image = targets[frame].Color.Image;
			vkImageMemoryBarrier.SubresourceRange = new VkImageSubresourceRange
			{
				AspectMask = VkImageAspectFlags.ColorBit,
				BaseMipLevel = 0,
				LevelCount = 1,
				BaseArrayLayer = 0,
				LayerCount = 1
			};
			VkImageMemoryBarrier vkImageMemoryBarrier2 = vkImageMemoryBarrier;
			VkImageMemoryBarrier reference = vkImageMemoryBarrier2;
			cmd.PipelineBarrier(VkPipelineStageFlags.ColorAttachmentOutputBit, VkPipelineStageFlags.FragmentShaderBit, VkDependencyFlags.None, default(ReadOnlySpan<VkMemoryBarrier>), default(ReadOnlySpan<VkBufferMemoryBarrier>), new ReadOnlySpan<VkImageMemoryBarrier>(in reference));
		}
	}

	public void RecordResolveFrom(CommandBuffer cmd, int frame, SceneColorTargets source)
	{
		if ((uint)frame < (uint)targets.Length && (uint)frame < (uint)source.targets.Length)
		{
			VkImage image = source.targets[frame].Color.Image;
			VkImage image2 = targets[frame].Color.Image;
			VkMemoryBarrier reference = new VkMemoryBarrier
			{
				SrcAccessMask = VkAccessFlags.ColorAttachmentWriteBit,
				DstAccessMask = VkAccessFlags.TransferReadBit
			};
			ReadOnlySpan<VkMemoryBarrier> pMemoryBarriers = new ReadOnlySpan<VkMemoryBarrier>(in reference);
			InlineArray2<VkImageMemoryBarrier> buffer = default(InlineArray2<VkImageMemoryBarrier>);
			global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray2<VkImageMemoryBarrier>, VkImageMemoryBarrier>(ref buffer, 0) = ColorBarrier(image, VkImageLayout.ShaderReadOnlyOptimal, VkImageLayout.TransferSrcOptimal, VkAccessFlags.None, VkAccessFlags.TransferReadBit);
			global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray2<VkImageMemoryBarrier>, VkImageMemoryBarrier>(ref buffer, 1) = ColorBarrier(image2, VkImageLayout.Undefined, VkImageLayout.TransferDstOptimal, VkAccessFlags.None, VkAccessFlags.TransferWriteBit);
			cmd.PipelineBarrier(VkPipelineStageFlags.FragmentShaderBit | VkPipelineStageFlags.ColorAttachmentOutputBit, VkPipelineStageFlags.TransferBit, VkDependencyFlags.None, pMemoryBarriers, default(ReadOnlySpan<VkBufferMemoryBarrier>), global::_003CPrivateImplementationDetails_003E.InlineArrayAsReadOnlySpan<InlineArray2<VkImageMemoryBarrier>, VkImageMemoryBarrier>(in buffer, 2));
			VkImageSubresourceLayers vkImageSubresourceLayers = new VkImageSubresourceLayers();
			vkImageSubresourceLayers.AspectMask = VkImageAspectFlags.ColorBit;
			vkImageSubresourceLayers.MipLevel = 0;
			vkImageSubresourceLayers.BaseArrayLayer = 0;
			vkImageSubresourceLayers.LayerCount = 1;
			VkImageSubresourceLayers vkImageSubresourceLayers2 = vkImageSubresourceLayers;
			VkImageBlit vkImageBlit = new VkImageBlit();
			vkImageBlit.SrcSubresource = vkImageSubresourceLayers2;
			vkImageBlit.DstSubresource = vkImageSubresourceLayers2;
			VkImageBlit vkImageBlit2 = vkImageBlit;
			global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<VkOffset3D_2, VkOffset3D>(ref vkImageBlit2.SrcOffsets, 1) = new VkOffset3D(width, height, 1);
			global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<VkOffset3D_2, VkOffset3D>(ref vkImageBlit2.DstOffsets, 1) = new VkOffset3D(width, height, 1);
			VkImageBlit reference2 = vkImageBlit2;
			cmd.BlitImage(image, VkImageLayout.TransferSrcOptimal, image2, VkImageLayout.TransferDstOptimal, new ReadOnlySpan<VkImageBlit>(in reference2), VkFilter.Nearest);
			VkMemoryBarrier reference3 = new VkMemoryBarrier
			{
				SrcAccessMask = VkAccessFlags.TransferWriteBit,
				DstAccessMask = (VkAccessFlags.ColorAttachmentReadBit | VkAccessFlags.ColorAttachmentWriteBit)
			};
			ReadOnlySpan<VkMemoryBarrier> pMemoryBarriers2 = new ReadOnlySpan<VkMemoryBarrier>(in reference3);
			InlineArray2<VkImageMemoryBarrier> buffer2 = default(InlineArray2<VkImageMemoryBarrier>);
			global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray2<VkImageMemoryBarrier>, VkImageMemoryBarrier>(ref buffer2, 0) = ColorBarrier(image, VkImageLayout.TransferSrcOptimal, VkImageLayout.ShaderReadOnlyOptimal, VkAccessFlags.TransferReadBit, VkAccessFlags.None);
			global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray2<VkImageMemoryBarrier>, VkImageMemoryBarrier>(ref buffer2, 1) = ColorBarrier(image2, VkImageLayout.TransferDstOptimal, VkImageLayout.ShaderReadOnlyOptimal, VkAccessFlags.TransferWriteBit, VkAccessFlags.ShaderReadBit);
			cmd.PipelineBarrier(VkPipelineStageFlags.TransferBit, VkPipelineStageFlags.FragmentShaderBit | VkPipelineStageFlags.ColorAttachmentOutputBit, VkDependencyFlags.None, pMemoryBarriers2, default(ReadOnlySpan<VkBufferMemoryBarrier>), global::_003CPrivateImplementationDetails_003E.InlineArrayAsReadOnlySpan<InlineArray2<VkImageMemoryBarrier>, VkImageMemoryBarrier>(in buffer2, 2));
		}
	}

	private static VkImageMemoryBarrier ColorBarrier(VkImage image, VkImageLayout oldLayout, VkImageLayout newLayout, VkAccessFlags srcAccess, VkAccessFlags dstAccess)
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
			AspectMask = VkImageAspectFlags.ColorBit,
			BaseMipLevel = 0,
			LevelCount = 1,
			BaseArrayLayer = 0,
			LayerCount = 1
		};
		return result;
	}

	public void EnsureSized(int framesInFlight, int w, int h)
	{
		w = Math.Max(1, w);
		h = Math.Max(1, h);
		if (targets.Length == framesInFlight && width == w && height == h)
		{
			return;
		}
		width = w;
		height = h;
		if (targets.Length != framesInFlight)
		{
			DisposeTargets();
			targets = new RenderTexture[framesInFlight];
			for (int i = 0; i < framesInFlight; i++)
			{
				targets[i] = new RenderTexture(w, h, colorFormat, depthFormat, RenderTextureFilter.Bilinear, RenderTextureWrap.Clamp, randomWrite: false, $"{name}{i}");
			}
		}
		else
		{
			for (int j = 0; j < framesInFlight; j++)
			{
				targets[j].Resize(w, h);
			}
		}
		RebuildAttachment();
	}

	private void RebuildAttachment()
	{
		VkFramebuffer[] array = new VkFramebuffer[targets.Length];
		VkFramebuffer[] array2 = new VkFramebuffer[targets.Length];
		VkFramebuffer[] array3 = new VkFramebuffer[targets.Length];
		VkImageView[] array4 = new VkImageView[targets.Length];
		for (int i = 0; i < targets.Length; i++)
		{
			array[i] = targets[i].GetFramebuffer(renderPass);
			array2[i] = targets[i].GetFramebuffer(loadRenderPass);
			array3[i] = targets[i].GetFramebuffer(backdropRenderPass);
			array4[i] = targets[i].DepthView;
		}
		VkClearValue[] clearValues = new VkClearValue[2]
		{
			new VkClearColorValue
			{
				Float32 = new float4(0f, 0f, 0f, 1f)
			},
			new VkClearDepthStencilValue
			{
				Depth = 0f,
				Stencil = 0
			}
		};
		attachment = new PassAttachment(renderPass, array, array4, new VkExtent2D(width, height), clearValues, VkSampleCountFlags._1Bit, colorFormat, depthFormat);
		loadAttachment = new PassAttachment(loadRenderPass, array2, array4, new VkExtent2D(width, height), Array.Empty<VkClearValue>(), VkSampleCountFlags._1Bit, colorFormat, depthFormat);
		backdropAttachment = new PassAttachment(backdropRenderPass, array3, array4, new VkExtent2D(width, height), clearValues, VkSampleCountFlags._1Bit, colorFormat, depthFormat);
	}

	private void DisposeTargets()
	{
		for (int i = 0; i < targets.Length; i++)
		{
			targets[i].Dispose();
		}
		targets = Array.Empty<RenderTexture>();
	}

	public void Dispose()
	{
		DisposeTargets();
		device.DestroyRenderPass(renderPass, device.VkAllocator);
		device.DestroyRenderPass(loadRenderPass, device.VkAllocator);
		device.DestroyRenderPass(backdropRenderPass, device.VkAllocator);
	}
}
