using System;
using System.Runtime.InteropServices;
using Brutal.Extensions;
using Brutal.GlfwApi;
using Brutal.Logging;
using Brutal.Logging.Internal;
using Brutal.Numerics;
using Brutal.VulkanApi;
using Brutal.VulkanApi.Abstractions;

namespace Waylith.Graphics;

public class WindowRenderContext : RenderContext
{
	[StructLayout(LayoutKind.Explicit, Size = 24)]
	private struct VkSurfaceFullScreenExclusiveInfoEXT
	{
		[FieldOffset(0)]
		private readonly VkStructureType sType;

		[FieldOffset(8)]
		public unsafe void* Next;

		[FieldOffset(16)]
		public int FullScreenExclusive;

		public unsafe VkSurfaceFullScreenExclusiveInfoEXT()
		{
			Next = default(void*);
			FullScreenExclusive = 0;
			sType = VkStructureType.SurfaceFullScreenExclusiveInfoEXT;
		}
	}

	private readonly GlfwWindow window;

	private VkSwapchainKHR swapchain;

	private VkPresentModeKHR presentMode;

	private VkPresentModeKHR? _pendingPresentMode;

	private readonly Queue queue;

	private VkSurfaceKHR surface;

	private VkSurfaceFormatKHR surfaceFormat;

	private VkSemaphore[] imageRenderingFinishedSemaphores = Array.Empty<VkSemaphore>();

	private const int FullScreenExclusiveDisallowed = 2;

	public VkSwapchainKHR Swapchain => swapchain;

	public VkPresentModeKHR PresentMode => presentMode;

	public VkSurfaceKHR Surface => surface;

	public VkSurfaceFormatKHR SurfaceFormat => surfaceFormat;

	public VkImageUsageFlags SwapchainImageUsage { get; private set; }

	public Queue PresentQueue => queue;

	public WindowRenderContext(GraphicsDevice graphicsDevice, GlfwWindow window, VkSurfaceKHR surface, VkFormat colorFormat, VkFormat depthFormat, VkPresentModeKHR requestedPresentMode, VkSampleCountFlags sampleCount, int numFramesInFlight, VkAllocator? allocator = null)
		: base(graphicsDevice, graphicsDevice.GraphicsQueue, colorFormat, depthFormat, sampleCount, numFramesInFlight, allocator)
	{
		this.window = window;
		this.surface = surface;
		presentMode = requestedPresentMode;
		queue = graphicsDevice.PresentQueue;
		FinalLayout = VkImageLayout.PresentSrcKHR;
		Rebuild();
	}

	public WindowRenderContext(GraphicsDevice graphicsDevice, GlfwWindow window, VkSurfaceKHR surface, VkFormat colorFormat, VkFormat depthFormat, VkPresentModeKHR requestedPresentMode, VkSampleCountFlags sampleCount, int numFramesInFlight)
		: this(graphicsDevice, window, surface, colorFormat, depthFormat, requestedPresentMode, sampleCount, numFramesInFlight, null)
	{
	}

	protected unsafe VkSwapchainKHR CreateSwapchain(Device inDevice, PhysicalDevice physicalDevice, int graphicsQueueFamilyIndex, int presentQueueFamilyIndex, VkPresentModeKHR requestedPresentMode, int requestedImageCount, GlfwWindow window, VkSurfaceKHR surface, VkSwapchainKHR oldSwapchain)
	{
		VkSurfaceFullScreenExclusiveInfoEXT vkSurfaceFullScreenExclusiveInfoEXT = new VkSurfaceFullScreenExclusiveInfoEXT();
		vkSurfaceFullScreenExclusiveInfoEXT.FullScreenExclusive = 2;
		VkSurfaceFullScreenExclusiveInfoEXT vkSurfaceFullScreenExclusiveInfoEXT2 = vkSurfaceFullScreenExclusiveInfoEXT;
		VkSurfaceCapabilitiesKHR vkSurfaceCapabilitiesKHR;
		if (base.GraphicsDevice.FullscreenExclusiveDisallowed)
		{
			VkPhysicalDeviceSurfaceInfo2KHR vkPhysicalDeviceSurfaceInfo2KHR = new VkPhysicalDeviceSurfaceInfo2KHR();
			vkPhysicalDeviceSurfaceInfo2KHR.Next = &vkSurfaceFullScreenExclusiveInfoEXT2;
			vkPhysicalDeviceSurfaceInfo2KHR.Surface = surface;
			VkPhysicalDeviceSurfaceInfo2KHR pSurfaceInfo = vkPhysicalDeviceSurfaceInfo2KHR;
			VkSurfaceCapabilities2KHR pSurfaceCapabilities = new VkSurfaceCapabilities2KHR();
			physicalDevice.GetSurfaceCapabilities2KHR(in pSurfaceInfo, ref pSurfaceCapabilities);
			vkSurfaceCapabilitiesKHR = pSurfaceCapabilities.SurfaceCapabilities;
		}
		else
		{
			vkSurfaceCapabilitiesKHR = physicalDevice.GetSurfaceCapabilitiesKHR(surface);
		}
		Span<VkSurfaceFormatKHR> span = stackalloc VkSurfaceFormatKHR[physicalDevice.GetSurfaceFormatsKHR_Count(surface)];
		physicalDevice.GetSurfaceFormatsKHR_Write(surface, span);
		Span<VkPresentModeKHR> span2 = stackalloc VkPresentModeKHR[physicalDevice.GetSurfacePresentModesKHR_Count(surface)];
		physicalDevice.GetSurfacePresentModesKHR_Write(surface, span2);
		VkPresentModeKHR vkPresentModeKHR = requestedPresentMode;
		if (!SpanExtensions.Contains(span2, requestedPresentMode))
		{
			vkPresentModeKHR = VkPresentModeKHR.FifoKHR;
		}
		presentMode = vkPresentModeKHR;
		if (span.Length <= 0)
		{
			throw new InvalidOperationException("WindowRenderContext: Vulkan surface exposes no supported formats.");
		}
		surfaceFormat = span[0];
		Span<VkSurfaceFormatKHR> span3 = span;
		int i;
		for (i = 0; i < span3.Length; i++)
		{
			VkSurfaceFormatKHR vkSurfaceFormatKHR = span3[i];
			if (vkSurfaceFormatKHR.Format == ColorFormat && vkSurfaceFormatKHR.ColorSpace == VkColorSpaceKHR.SRGBNonlinearKHR)
			{
				surfaceFormat = vkSurfaceFormatKHR;
				break;
			}
		}
		if (ColorFormat != surfaceFormat.Format)
		{
			LogCategory log = RenderLog.Log;
			bool shouldAppend;
			LogString<Warning> message = new LogString<Warning>(103, 2, log, out shouldAppend);
			if (shouldAppend)
			{
				message.AppendLiteral("WindowRenderContext: Preferred surface format ");
				message.AppendFormatted(ColorFormat);
				message.AppendLiteral(" not available, ");
				message.AppendLiteral("falling back to available surface format ");
				message.AppendFormatted(surfaceFormat.Format);
			}
			log.Warning(message, "CreateSwapchain", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Graphics\\WindowRenderContext.cs", 175);
		}
		ColorFormat = surfaceFormat.Format;
		Extent = vkSurfaceCapabilitiesKHR.CurrentExtent;
		if (Extent.Width == -1)
		{
			window.FramebufferSize.Deconstruct(out i, out var y);
			int width = i;
			int height = y;
			Extent = new VkExtent2D(width, height);
		}
		int num = System.Math.Max((vkPresentModeKHR == VkPresentModeKHR.MailboxKHR) ? System.Math.Max(requestedImageCount, 3) : requestedImageCount, vkSurfaceCapabilitiesKHR.MinImageCount);
		if (vkSurfaceCapabilitiesKHR.MaxImageCount != 0)
		{
			num = System.Math.Min(num, vkSurfaceCapabilitiesKHR.MaxImageCount);
		}
		VkImageUsageFlags imageUsage = (SwapchainImageUsage = VkImageUsageFlags.ColorAttachmentBit | ((VkImageUsageFlags.TransferSrcBit | VkImageUsageFlags.TransferDstBit) & vkSurfaceCapabilitiesKHR.SupportedUsageFlags));
		VkCompositeAlphaFlagsKHR vkCompositeAlphaFlagsKHR = VkCompositeAlphaFlagsKHR.OpaqueBitKHR;
		if ((vkSurfaceCapabilitiesKHR.SupportedCompositeAlpha & vkCompositeAlphaFlagsKHR) == 0)
		{
			VkCompositeAlphaFlagsKHR supportedCompositeAlpha = vkSurfaceCapabilitiesKHR.SupportedCompositeAlpha;
			vkCompositeAlphaFlagsKHR = (((supportedCompositeAlpha & VkCompositeAlphaFlagsKHR.InheritBitKHR) != 0) ? VkCompositeAlphaFlagsKHR.InheritBitKHR : (((supportedCompositeAlpha & VkCompositeAlphaFlagsKHR.PreMultipliedBitKHR) != 0) ? VkCompositeAlphaFlagsKHR.PreMultipliedBitKHR : VkCompositeAlphaFlagsKHR.PostMultipliedBitKHR));
		}
		VkSwapchainCreateInfoKHR vkSwapchainCreateInfoKHR = new VkSwapchainCreateInfoKHR();
		vkSwapchainCreateInfoKHR.Surface = surface;
		vkSwapchainCreateInfoKHR.MinImageCount = num;
		vkSwapchainCreateInfoKHR.ImageFormat = surfaceFormat.Format;
		vkSwapchainCreateInfoKHR.ImageColorSpace = surfaceFormat.ColorSpace;
		vkSwapchainCreateInfoKHR.ImageExtent = Extent;
		vkSwapchainCreateInfoKHR.ImageArrayLayers = 1;
		vkSwapchainCreateInfoKHR.ImageUsage = imageUsage;
		vkSwapchainCreateInfoKHR.PreTransform = vkSurfaceCapabilitiesKHR.CurrentTransform;
		vkSwapchainCreateInfoKHR.CompositeAlpha = vkCompositeAlphaFlagsKHR;
		vkSwapchainCreateInfoKHR.PresentMode = vkPresentModeKHR;
		vkSwapchainCreateInfoKHR.Clipped = true;
		vkSwapchainCreateInfoKHR.OldSwapchain = oldSwapchain;
		vkSwapchainCreateInfoKHR.Next = (base.GraphicsDevice.FullscreenExclusiveDisallowed ? (&vkSurfaceFullScreenExclusiveInfoEXT2) : null);
		VkSwapchainCreateInfoKHR pCreateInfo = vkSwapchainCreateInfoKHR;
		int* queueFamilyIndices = stackalloc int[2] { graphicsQueueFamilyIndex, presentQueueFamilyIndex };
		if (graphicsQueueFamilyIndex != presentQueueFamilyIndex)
		{
			pCreateInfo.ImageSharingMode = VkSharingMode.Concurrent;
			pCreateInfo.QueueFamilyIndexCount = 2;
			pCreateInfo.QueueFamilyIndices = queueFamilyIndices;
		}
		else
		{
			pCreateInfo.ImageSharingMode = VkSharingMode.Exclusive;
			pCreateInfo.QueueFamilyIndexCount = 0;
			pCreateInfo.QueueFamilyIndices = null;
		}
		VkSwapchainKHR result = inDevice.CreateSwapchainKHR(in pCreateInfo, _allocator);
		base.ImageCount = inDevice.GetSwapchainImagesKHR_Count(result);
		return result;
	}

	public sealed override void Rebuild()
	{
		DestroySwapchainDependents();
		VkSwapchainKHR oldSwapchain = swapchain;
		swapchain = CreateSwapchain(base.Device, base.PhysicalDevice, Graphics.Family, queue.Family, presentMode, 2, window, surface, oldSwapchain);
		if (!oldSwapchain.IsNull())
		{
			base.Device.DestroySwapchainKHR(oldSwapchain, _allocator);
		}
		RebuildSwapchainDependents();
	}

	public void RecreateSurfaceAndRebuild()
	{
		DestroySwapchainDependents();
		if (!swapchain.IsNull())
		{
			base.Device.DestroySwapchainKHR(swapchain, _allocator);
		}
		swapchain = default(VkSwapchainKHR);
		ReplaceSurface();
		int num = 1;
		while (true)
		{
			try
			{
				swapchain = CreateSwapchain(base.Device, base.PhysicalDevice, Graphics.Family, queue.Family, presentMode, 2, window, surface, default(VkSwapchainKHR));
			}
			catch (VkResultError vkResultError) when (vkResultError.Result == VkResult.ErrorSurfaceLostKHR && num < 10)
			{
				RenderLog.Log.Warning($"Surface lost again while rebuilding the swapchain (attempt {num}/{10}); " + "pumping window events, recreating the surface and retrying.", "RecreateSurfaceAndRebuild", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Graphics\\WindowRenderContext.cs", 338);
				Glfw.WaitEventsTimeout(0.05);
				ReplaceSurface();
				goto IL_0138;
			}
			break;
			IL_0138:
			num++;
		}
		RebuildSwapchainDependents();
	}

	private void DestroySwapchainDependents()
	{
		base.Device.WaitIdle();
		ReleaseFrameResources();
		AllSyncObjects.Dispose();
		DestroyImageRenderingFinishedSemaphores();
		base.Device.DestroyRenderPass(RenderPass, _allocator);
	}

	private void RebuildSwapchainDependents()
	{
		RenderPass = CreateMainRenderPass();
		Viewport.Value = new VkViewport
		{
			X = 0f,
			Y = 0f,
			Width = Extent.Width,
			Height = Extent.Height,
			MinDepth = 0f,
			MaxDepth = 1f
		};
		ScissorRect.Value = new VkRect2D(Extent);
		base.FrameResources = CreateFrameResources();
		InitializeFrameResources();
		imageRenderingFinishedSemaphores = CreateImageRenderingFinishedSemaphores();
		AllSyncObjects = CreateSyncObjects();
	}

	private void ReplaceSurface()
	{
		VkSurfaceKHR vkSurfaceKHR = surface;
		surface = CreateWindowSurface(base.Instance, window);
		base.Instance.DestroySurfaceKHR(vkSurfaceKHR, _allocator);
	}

	private static VkSurfaceKHR CreateWindowSurface(Instance instance, GlfwWindow window)
	{
		Glfw.CreateWindowSurface(instance.VkHandle, window, out var num);
		if (num == 0L)
		{
			string description;
			GlfwErrorCode error = Glfw.GetError(out description);
			throw new InvalidOperationException($"Failed to recreate window surface: {error} {description}");
		}
		return new VkSurfaceKHR((nint)num);
	}

	public override void PollEvents()
	{
		Glfw.PollEvents();
	}

	public override bool IsRunning()
	{
		return !window.ShouldClose;
	}

	public bool HasFramebufferSizeMismatch()
	{
		var (num3, num4) = (int2)(ref window.FramebufferSize);
		if (num3 <= 0 || num4 <= 0)
		{
			return false;
		}
		if (Extent.Width == num3)
		{
			return Extent.Height != num4;
		}
		return true;
	}

	public bool IsFramebufferMinimized()
	{
		var (num3, num4) = (int2)(ref window.FramebufferSize);
		if (num3 > 0)
		{
			return num4 <= 0;
		}
		return true;
	}

	protected FrameResources[] CreateFrameResources()
	{
		Span<VkImage> span = stackalloc VkImage[base.Device.GetSwapchainImagesKHR_Count(swapchain)];
		base.Device.GetSwapchainImagesKHR_Write(swapchain, span);
		FrameResources[] array = new FrameResources[base.ImageCount];
		VkImageSubresourceRange vkImageSubresourceRange = new VkImageSubresourceRange();
		vkImageSubresourceRange.AspectMask = VkImageAspectFlags.ColorBit;
		vkImageSubresourceRange.LevelCount = 1;
		vkImageSubresourceRange.LayerCount = 1;
		VkImageSubresourceRange subresourceRange = vkImageSubresourceRange;
		for (int i = 0; i < base.ImageCount; i++)
		{
			VkImage vkImage = span[i];
			VkImageView colorImageView = base.Device.CreateImageView(new VkImageViewCreateInfo
			{
				Image = vkImage,
				ViewType = VkImageViewType._2D,
				Format = ColorFormat,
				SubresourceRange = subresourceRange
			}, _allocator);
			array[i] = new FrameResources(base.Device)
			{
				FrameIndex = i,
				OwnsColorImage = false,
				ColorImage = vkImage,
				ColorImageView = colorImageView,
				Allocator = _allocator
			};
		}
		return array;
	}

	private unsafe VkResult PresentFrame()
	{
		VkSemaphore imageRenderingFinishedSemaphore = GetImageRenderingFinishedSemaphore();
		VkSwapchainKHR vkSwapchainKHR = swapchain;
		int frameIndex = FrameIndex;
		VkPresentInfoKHR vkPresentInfoKHR = new VkPresentInfoKHR();
		vkPresentInfoKHR.WaitSemaphoreCount = 1;
		vkPresentInfoKHR.WaitSemaphores = &imageRenderingFinishedSemaphore;
		vkPresentInfoKHR.SwapchainCount = 1;
		vkPresentInfoKHR.Swapchains = &vkSwapchainKHR;
		vkPresentInfoKHR.ImageIndices = &frameIndex;
		vkPresentInfoKHR.Next = null;
		VkPresentInfoKHR inPresentInfo = vkPresentInfoKHR;
		try
		{
			return queue.PresentKHR(inPresentInfo);
		}
		catch (VkResultError vkResultError)
		{
			VkResult result = vkResultError.Result;
			if ((result != VkResult.ErrorOutOfDateKHR && result != VkResult.ErrorSurfaceLostKHR) || 1 == 0)
			{
				throw;
			}
			return vkResultError.Result;
		}
	}

	public override FrameResult TrySubmitFrame(Span<VkSemaphore> additionalWaitSemaphores, Span<VkPipelineStageFlags> additionalWaitStages, Span<VkSemaphore> additionalSignalSemaphores)
	{
		Span<VkSemaphore> span = stackalloc VkSemaphore[additionalWaitSemaphores.Length + 1];
		span[0] = base.SyncObjects.ImageReadySemaphore;
		additionalWaitSemaphores.CopyTo(span.Slice(1));
		Span<VkPipelineStageFlags> span2 = stackalloc VkPipelineStageFlags[additionalWaitStages.Length + 1];
		span2[0] = VkPipelineStageFlags.ColorAttachmentOutputBit;
		additionalWaitStages.CopyTo(span2.Slice(1));
		Span<VkSemaphore> signalSemaphores = stackalloc VkSemaphore[additionalSignalSemaphores.Length + 1];
		signalSemaphores[0] = GetImageRenderingFinishedSemaphore();
		additionalSignalSemaphores.CopyTo(signalSemaphores.Slice(1));
		GraphicsDevice graphicsDevice = base.GraphicsDevice;
		Span<VkSemaphore> waitSemaphores = span;
		Span<VkPipelineStageFlags> waitStages = span2;
		CommandBuffer reference = base.SyncObjects.CommandBuffer;
		VkResult vkResult = graphicsDevice.SubmitGraphicsAndPresent(waitSemaphores, waitStages, new Span<CommandBuffer>(ref reference), signalSemaphores, base.SyncObjects.Fence, PresentFrame);
		switch (vkResult)
		{
		case VkResult.ErrorOutOfDateKHR:
		case VkResult.SuboptimalKHR:
			return FrameResult.NeedsResize;
		case VkResult.ErrorSurfaceLostKHR:
			return FrameResult.SurfaceLost;
		case VkResult.Success:
			return FrameResult.Success;
		default:
			throw new NotImplementedException($"Unexpected result {vkResult} from TrySubmitFrame(), expected Vulkan API to throw for this result.");
		}
	}

	protected override VkResult GetNextFrameIndex(out int imageIndex)
	{
		try
		{
			return base.Device.AcquireNextImageKHR(swapchain, -1, base.SyncObjects.ImageReadySemaphore, default(VkFence), out imageIndex);
		}
		catch (VkResultError vkResultError)
		{
			VkResult result = vkResultError.Result;
			if ((result == VkResult.ErrorOutOfDateKHR || result == VkResult.ErrorSurfaceLostKHR || result == VkResult.SuboptimalKHR) ? true : false)
			{
				imageIndex = -1;
				return vkResultError.Result;
			}
			throw;
		}
	}

	public void SetVSyncMode(VSyncMode mode)
	{
		_pendingPresentMode = VSyncModeToPreferredPresentMode(mode);
	}

	public bool ConsumePresentModeChange()
	{
		VkPresentModeKHR? pendingPresentMode = _pendingPresentMode;
		if (pendingPresentMode.HasValue)
		{
			VkPresentModeKHR valueOrDefault = pendingPresentMode.GetValueOrDefault();
			if (valueOrDefault != presentMode)
			{
				presentMode = valueOrDefault;
				_pendingPresentMode = null;
				return true;
			}
		}
		_pendingPresentMode = null;
		return false;
	}

	public static VkPresentModeKHR VSyncModeToPreferredPresentMode(VSyncMode mode)
	{
		return mode switch
		{
			VSyncMode.Off => VkPresentModeKHR.ImmediateKHR, 
			VSyncMode.TripleBuffer => VkPresentModeKHR.MailboxKHR, 
			VSyncMode.VSync => VkPresentModeKHR.FifoKHR, 
			_ => VkPresentModeKHR.MailboxKHR, 
		};
	}

	public override void Dispose()
	{
		DestroyImageRenderingFinishedSemaphores();
		base.Dispose();
		if (!swapchain.IsNull())
		{
			base.Device.DestroySwapchainKHR(swapchain, _allocator);
		}
		base.Instance.DestroySurfaceKHR(surface, _allocator);
		GC.SuppressFinalize(this);
	}

	private VkSemaphore[] CreateImageRenderingFinishedSemaphores()
	{
		VkSemaphore[] array = new VkSemaphore[base.ImageCount];
		for (int i = 0; i < base.ImageCount; i++)
		{
			array[i] = base.Device.CreateSemaphore(new VkSemaphoreCreateInfo(), _allocator);
		}
		return array;
	}

	private VkSemaphore GetImageRenderingFinishedSemaphore()
	{
		if (FrameIndex < 0 || FrameIndex >= imageRenderingFinishedSemaphores.Length)
		{
			throw new InvalidOperationException($"Cannot get rendering-finished semaphore for swapchain image index {FrameIndex}.");
		}
		return imageRenderingFinishedSemaphores[FrameIndex];
	}

	private void DestroyImageRenderingFinishedSemaphores()
	{
		for (int i = 0; i < imageRenderingFinishedSemaphores.Length; i++)
		{
			base.Device.DestroySemaphore(imageRenderingFinishedSemaphores[i], _allocator);
		}
		imageRenderingFinishedSemaphores = Array.Empty<VkSemaphore>();
	}
}
