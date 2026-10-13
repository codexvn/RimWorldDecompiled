using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Brutal.VulkanApi;
using UnityEngine;
using UnityStubs;
using UnityStubs.Rendering;
using Waylith;
using Waylith.Graphics;
using Waylith.Rendering;

namespace RimWorldWaylith.Rendering;

public sealed class ScreenCaptureFeature : IRenderFeature, IPostBackbufferPass, IRenderPass, IDisposable, IScreenCaptureBackend
{
	private sealed class Capture(UnityEngine.RenderTexture target, GpuTextureAllocation gpu)
	{
		public readonly UnityEngine.RenderTexture Target = target;

		public readonly GpuTextureAllocation Gpu = gpu;

		public volatile bool Recorded;

		public bool Settled;
	}

	private static readonly VkImageSubresourceRange ColorRange = new VkImageSubresourceRange
	{
		AspectMask = VkImageAspectFlags.ColorBit,
		BaseMipLevel = 0,
		LevelCount = 1,
		BaseArrayLayer = 0,
		LayerCount = 1
	};

	private readonly EngineApplication _app;

	private readonly CameraTextureBaker _baker;

	private readonly IRenderPass[] _passes;

	private UnityEngine.RenderTexture? _screen;

	private bool _captureRequested;

	private Capture? _armed;

	private Capture? _readable;

	private bool _warnedUnsupported;

	public bool IsActive => true;

	public IReadOnlyList<IRenderPass> Passes => _passes;

	public byte PassId => 0;

	public PassAttachment? Attachment => null;

	public bool PreBackbuffer => false;

	private bool CanCapture
	{
		get
		{
			WindowRenderContext renderContext = _app.RenderContext;
			if (renderContext != null)
			{
				return (renderContext.SwapchainImageUsage & VkImageUsageFlags.TransferSrcBit) != 0;
			}
			return false;
		}
	}

	public ScreenCaptureFeature(EngineApplication app, CameraTextureBaker baker)
	{
		_app = app;
		_baker = baker;
		_passes = new IRenderPass[1] { this };
	}

	public object? CaptureFrame(in FrameTiming timing)
	{
		_armed = null;
		if (!_captureRequested && !EndOfFrame.HasWaiters)
		{
			return null;
		}
		_captureRequested = false;
		if (!CanCapture)
		{
			return null;
		}
		VkExtent2D extent = _app.RenderContext.Extent;
		if (_screen == null || _screen.width != extent.Width || _screen.height != extent.Height)
		{
			_screen?.Release();
			_screen = new UnityEngine.RenderTexture(extent.Width, extent.Height, 0)
			{
				name = "ScreenCapture"
			};
		}
		if (_screen.Create())
		{
			GpuTextureAllocation gpuTexture = _screen.GpuTexture;
			if (gpuTexture != null)
			{
				return _armed = new Capture(_screen, gpuTexture);
			}
		}
		return null;
	}

	public void RunEndOfFrame()
	{
		_readable = _armed;
		_armed = null;
		try
		{
			EndOfFrame.Run();
		}
		finally
		{
			_readable = null;
		}
	}

	public void Record(in FrameContext ctx, object? featurePayload)
	{
		if (featurePayload is Capture capture)
		{
			VkExtent2D extent = ctx.Extent;
			if (capture.target.width == extent.Width && capture.target.height == extent.Height)
			{
				CommandBuffer commandBuffer = ctx.CommandBuffer;
				VkImage colorImage = ctx.Resources.ColorImage;
				VkImageLayout vkImageLayout = ((ctx.RenderContext is RenderContext { FinalLayout: var finalLayout }) ? finalLayout : VkImageLayout.PresentSrcKHR);
				InlineArray2<VkImageMemoryBarrier> buffer = default(InlineArray2<VkImageMemoryBarrier>);
				global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray2<VkImageMemoryBarrier>, VkImageMemoryBarrier>(ref buffer, 0) = Barrier(colorImage, vkImageLayout, VkImageLayout.TransferSrcOptimal, VkAccessFlags.ColorAttachmentWriteBit, VkAccessFlags.TransferReadBit, null);
				global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray2<VkImageMemoryBarrier>, VkImageMemoryBarrier>(ref buffer, 1) = Barrier(capture.gpu.Image, VkImageLayout.Undefined, VkImageLayout.TransferDstOptimal, VkAccessFlags.TransferWriteBit, VkAccessFlags.TransferWriteBit, null);
				commandBuffer.PipelineBarrier(VkPipelineStageFlags.ColorAttachmentOutputBit | VkPipelineStageFlags.TransferBit, VkPipelineStageFlags.TransferBit, VkDependencyFlags.None, default(ReadOnlySpan<VkMemoryBarrier>), default(ReadOnlySpan<VkBufferMemoryBarrier>), global::_003CPrivateImplementationDetails_003E.InlineArrayAsReadOnlySpan<InlineArray2<VkImageMemoryBarrier>, VkImageMemoryBarrier>(in buffer, 2));
				VkImage image = capture.gpu.Image;
				VkImageBlit reference = FullBlit(extent.Width, extent.Height, extent.Width, extent.Height);
				commandBuffer.BlitImage(colorImage, VkImageLayout.TransferSrcOptimal, image, VkImageLayout.TransferDstOptimal, new ReadOnlySpan<VkImageBlit>(in reference), VkFilter.Nearest);
				InlineArray2<VkImageMemoryBarrier> buffer2 = default(InlineArray2<VkImageMemoryBarrier>);
				global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray2<VkImageMemoryBarrier>, VkImageMemoryBarrier>(ref buffer2, 0) = Barrier(colorImage, VkImageLayout.TransferSrcOptimal, vkImageLayout, VkAccessFlags.TransferReadBit, VkAccessFlags.None, null);
				global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray2<VkImageMemoryBarrier>, VkImageMemoryBarrier>(ref buffer2, 1) = Barrier(capture.gpu.Image, VkImageLayout.TransferDstOptimal, VkImageLayout.ShaderReadOnlyOptimal, VkAccessFlags.TransferWriteBit, VkAccessFlags.ShaderReadBit, null);
				commandBuffer.PipelineBarrier(VkPipelineStageFlags.TransferBit, VkPipelineStageFlags.FragmentShaderBit | VkPipelineStageFlags.BottomOfPipeBit, VkDependencyFlags.None, default(ReadOnlySpan<VkMemoryBarrier>), default(ReadOnlySpan<VkBufferMemoryBarrier>), global::_003CPrivateImplementationDetails_003E.InlineArrayAsReadOnlySpan<InlineArray2<VkImageMemoryBarrier>, VkImageMemoryBarrier>(in buffer2, 2));
				capture.gpu.CurrentLayout = VkImageLayout.ShaderReadOnlyOptimal;
				capture.Recorded = true;
			}
		}
	}

	public bool RequestCapture()
	{
		if (!CanCapture)
		{
			if (!_warnedUnsupported)
			{
				Debug.LogError("Screen capture unavailable, the swapchain images can't be read back on this device.");
			}
			_warnedUnsupported = true;
			return false;
		}
		_captureRequested = true;
		return true;
	}

	public bool TryReadScreen(out byte[] rgbaTopDown, out int width, out int height)
	{
		rgbaTopDown = Array.Empty<byte>();
		width = (height = 0);
		if (!TrySettle(out Capture capture) || !_baker.TryReadPixels(capture.target, out rgbaTopDown))
		{
			return false;
		}
		for (int i = 3; i < rgbaTopDown.Length; i += 4)
		{
			rgbaTopDown[i] = byte.MaxValue;
		}
		width = capture.target.width;
		height = capture.target.height;
		return true;
	}

	public bool TryCopyScreenInto(UnityEngine.RenderTexture target)
	{
		if (TrySettle(out Capture capture) && target.Create())
		{
			GpuTextureAllocation destination = target.GpuTexture;
			if (destination != null)
			{
				GpuTextureAllocation source = capture.gpu;
				bool sameSize = target.width == capture.target.width && target.height == capture.target.height;
				VkImageSubresourceRange colorRange = ColorRange;
				colorRange.LevelCount = destination.MipCount;
				VkImageSubresourceRange destinationRange = colorRange;
				_app.GraphicsDevice.ExecuteOneShot(delegate(CommandBuffer commandBuffer)
				{
					InlineArray2<VkImageMemoryBarrier> buffer = default(InlineArray2<VkImageMemoryBarrier>);
					global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray2<VkImageMemoryBarrier>, VkImageMemoryBarrier>(ref buffer, 0) = Barrier(source.Image, source.CurrentLayout, VkImageLayout.TransferSrcOptimal, VkAccessFlags.ShaderReadBit, VkAccessFlags.TransferReadBit, null);
					global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray2<VkImageMemoryBarrier>, VkImageMemoryBarrier>(ref buffer, 1) = Barrier(destination.Image, destination.CurrentLayout, VkImageLayout.TransferDstOptimal, VkAccessFlags.ShaderReadBit | VkAccessFlags.ColorAttachmentWriteBit, VkAccessFlags.TransferWriteBit, destinationRange);
					commandBuffer.PipelineBarrier(VkPipelineStageFlags.AllCommandsBit, VkPipelineStageFlags.TransferBit, VkDependencyFlags.None, default(ReadOnlySpan<VkMemoryBarrier>), default(ReadOnlySpan<VkBufferMemoryBarrier>), global::_003CPrivateImplementationDetails_003E.InlineArrayAsReadOnlySpan<InlineArray2<VkImageMemoryBarrier>, VkImageMemoryBarrier>(in buffer, 2));
					VkImage image = source.Image;
					VkImage image2 = destination.Image;
					VkImageBlit reference = FullBlit(capture.target.width, capture.target.height, target.width, target.height);
					commandBuffer.BlitImage(image, VkImageLayout.TransferSrcOptimal, image2, VkImageLayout.TransferDstOptimal, new ReadOnlySpan<VkImageBlit>(in reference), (!sameSize) ? VkFilter.Linear : VkFilter.Nearest);
					InlineArray2<VkImageMemoryBarrier> buffer2 = default(InlineArray2<VkImageMemoryBarrier>);
					global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray2<VkImageMemoryBarrier>, VkImageMemoryBarrier>(ref buffer2, 0) = Barrier(source.Image, VkImageLayout.TransferSrcOptimal, VkImageLayout.ShaderReadOnlyOptimal, VkAccessFlags.TransferReadBit, VkAccessFlags.ShaderReadBit, null);
					global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray2<VkImageMemoryBarrier>, VkImageMemoryBarrier>(ref buffer2, 1) = Barrier(destination.Image, VkImageLayout.TransferDstOptimal, VkImageLayout.ShaderReadOnlyOptimal, VkAccessFlags.TransferWriteBit, VkAccessFlags.ShaderReadBit, destinationRange);
					commandBuffer.PipelineBarrier(VkPipelineStageFlags.TransferBit, VkPipelineStageFlags.FragmentShaderBit, VkDependencyFlags.None, default(ReadOnlySpan<VkMemoryBarrier>), default(ReadOnlySpan<VkBufferMemoryBarrier>), global::_003CPrivateImplementationDetails_003E.InlineArrayAsReadOnlySpan<InlineArray2<VkImageMemoryBarrier>, VkImageMemoryBarrier>(in buffer2, 2));
				});
				source.CurrentLayout = VkImageLayout.ShaderReadOnlyOptimal;
				for (int i = 0; i < destination.MipCount; i++)
				{
					destination.SetMipLayout(i, VkImageLayout.ShaderReadOnlyOptimal);
				}
				return true;
			}
		}
		return false;
	}

	private bool TrySettle([NotNullWhen(true)] out Capture? capture)
	{
		capture = _readable;
		if (capture == null)
		{
			return false;
		}
		if (!capture.Settled)
		{
			_app.WaitForRenderThreadIdle();
			_app.GraphicsDevice.WaitIdleAndRun(delegate
			{
			});
			capture.Settled = true;
		}
		return capture.Recorded;
	}

	private static VkImageMemoryBarrier Barrier(VkImage image, VkImageLayout oldLayout, VkImageLayout newLayout, VkAccessFlags srcAccess, VkAccessFlags dstAccess, VkImageSubresourceRange? range = null)
	{
		VkImageMemoryBarrier result = new VkImageMemoryBarrier();
		result.SrcAccessMask = srcAccess;
		result.DstAccessMask = dstAccess;
		result.OldLayout = oldLayout;
		result.NewLayout = newLayout;
		result.SrcQueueFamilyIndex = -1;
		result.DstQueueFamilyIndex = -1;
		result.Image = image;
		result.SubresourceRange = range ?? ColorRange;
		return result;
	}

	private static VkImageBlit FullBlit(int srcWidth, int srcHeight, int dstWidth, int dstHeight)
	{
		VkImageSubresourceLayers vkImageSubresourceLayers = new VkImageSubresourceLayers();
		vkImageSubresourceLayers.AspectMask = VkImageAspectFlags.ColorBit;
		vkImageSubresourceLayers.MipLevel = 0;
		vkImageSubresourceLayers.BaseArrayLayer = 0;
		vkImageSubresourceLayers.LayerCount = 1;
		VkImageSubresourceLayers vkImageSubresourceLayers2 = vkImageSubresourceLayers;
		VkImageBlit vkImageBlit = new VkImageBlit();
		vkImageBlit.SrcSubresource = vkImageSubresourceLayers2;
		vkImageBlit.DstSubresource = vkImageSubresourceLayers2;
		VkImageBlit result = vkImageBlit;
		global::_003CPrivateImplementationDetails_003E.InlineArrayFirstElementRef<VkOffset3D_2, VkOffset3D>(ref result.SrcOffsets) = new VkOffset3D(0, 0, 0);
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<VkOffset3D_2, VkOffset3D>(ref result.SrcOffsets, 1) = new VkOffset3D(srcWidth, srcHeight, 1);
		global::_003CPrivateImplementationDetails_003E.InlineArrayFirstElementRef<VkOffset3D_2, VkOffset3D>(ref result.DstOffsets) = new VkOffset3D(0, 0, 0);
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<VkOffset3D_2, VkOffset3D>(ref result.DstOffsets, 1) = new VkOffset3D(dstWidth, dstHeight, 1);
		return result;
	}

	public void OnRenderTargetRebuilt(IRenderTarget renderTarget)
	{
	}

	public void Dispose()
	{
		_screen?.Release();
		_screen = null;
	}

	object? IRenderFeature.CaptureFrame(in FrameTiming timing)
	{
		return CaptureFrame(in timing);
	}

	void IRenderPass.Record(in FrameContext ctx, object? featurePayload)
	{
		Record(in ctx, featurePayload);
	}
}
