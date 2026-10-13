using System;
using Brutal.Memory;
using Brutal.VulkanApi;
using Brutal.VulkanApi.Abstractions;
using Brutal.VulkanApi.Vma;

namespace Waylith.Graphics;

public abstract class RenderContext : IRenderContext, IRenderTarget, IRenderFrameState, IDisposable
{
	public VkExtent2D Extent;

	public int FrameIndex = -1;

	public VkRenderPass RenderPass;

	protected SyncObjects[] AllSyncObjects = Array.Empty<SyncObjects>();

	private int SyncObjectsIndex;

	private bool _loggedAcquireNotReady;

	public VkFormat DepthFormat;

	public VkFormat ColorFormat;

	public VkImageLayout FinalLayout;

	public VkSampleCountFlags SampleCount;

	public readonly Queue Graphics;

	protected PtrOwner<VkViewport> Viewport = new PtrOwner<VkViewport>(new VkViewport());

	protected PtrOwner<VkRect2D> ScissorRect = new PtrOwner<VkRect2D>(new VkRect2D());

	protected PtrOwner<VkPipelineViewportStateCreateInfo> ViewportState;

	protected VkCommandPool GraphicsCommandPool;

	protected readonly VkAllocator? _allocator;

	private ImageAllocationInfo[] _msaaImageAllocations = Array.Empty<ImageAllocationInfo>();

	private ImageAllocationInfo[] _depthImageAllocations = Array.Empty<ImageAllocationInfo>();

	public Instance Instance { get; }

	public PhysicalDevice PhysicalDevice { get; }

	public DeviceEx Device { get; }

	public GraphicsDevice GraphicsDevice { get; }

	public int ImageCount { get; protected set; }

	public int FramesInFlight { get; }

	protected SyncObjects SyncObjects => AllSyncObjects[SyncObjectsIndex];

	public FrameResources[] FrameResources { get; protected set; }

	DeviceEx IRenderFrameState.Device => Device;

	int IRenderFrameState.FrameIndex => FrameIndex;

	int IRenderFrameState.SubmissionFrameIndex => SyncObjectsIndex;

	VkSampleCountFlags IRenderTarget.SampleCount => SampleCount;

	VkFormat IRenderTarget.ColorFormat => ColorFormat;

	VkFormat IRenderTarget.DepthFormat => DepthFormat;

	public RenderContext(GraphicsDevice graphicsDevice, Queue graphics, VkFormat colorFormat, VkFormat depthFormat, VkSampleCountFlags sampleCount, int numFramesInFlight, VkAllocator? allocator)
	{
		_allocator = allocator;
		GraphicsDevice = graphicsDevice;
		Instance = graphicsDevice.Instance;
		Device = graphicsDevice.Device;
		PhysicalDevice = graphicsDevice.PhysicalDevice;
		ColorFormat = colorFormat;
		DepthFormat = depthFormat;
		SampleCount = sampleCount;
		Graphics = graphics;
		FrameResources = Array.Empty<FrameResources>();
		FramesInFlight = numFramesInFlight;
		DeviceEx device = Device;
		VkCommandPoolCreateInfo pCreateInfo = new VkCommandPoolCreateInfo
		{
			QueueFamilyIndex = Graphics.Family,
			Flags = VkCommandPoolCreateFlags.ResetCommandBufferBit
		};
		GraphicsCommandPool = device.CreateCommandPool(in pCreateInfo, _allocator);
		ViewportState = new PtrOwner<VkPipelineViewportStateCreateInfo>(new VkPipelineViewportStateCreateInfo
		{
			ViewportCount = 1,
			Viewports = Viewport,
			ScissorCount = 1,
			Scissors = ScissorRect
		});
	}

	public abstract void Rebuild();

	public abstract void PollEvents();

	public abstract bool IsRunning();

	public virtual void Dispose()
	{
		Device.DestroyCommandPool(GraphicsCommandPool, _allocator);
		ReleaseFrameResources();
		AllSyncObjects.Dispose();
		Device.DestroyRenderPass(RenderPass, _allocator);
		GC.SuppressFinalize(this);
	}

	protected void ReleaseFrameResources()
	{
		FrameResources[] frameResources = FrameResources;
		foreach (FrameResources obj in frameResources)
		{
			obj.MultisampledImage = default(VkImage);
			obj.MultisampledImageMemory = default(VkDeviceMemory);
			obj.DepthImage = default(VkImage);
			obj.DepthImageMemory = default(VkDeviceMemory);
		}
		FrameResources.Dispose();
		VmaAllocator allocator = GraphicsDevice.Allocator;
		ImageAllocationInfo[] msaaImageAllocations = _msaaImageAllocations;
		for (int i = 0; i < msaaImageAllocations.Length; i++)
		{
			ImageAllocationInfo allocation = msaaImageAllocations[i];
			if (!allocation.Image.IsNull())
			{
				GpuMemoryTracker.Remove(GpuMemoryCategory.RenderTarget, in allocation);
				allocator.FreeAllocation(in allocation);
			}
		}
		msaaImageAllocations = _depthImageAllocations;
		for (int i = 0; i < msaaImageAllocations.Length; i++)
		{
			ImageAllocationInfo allocation2 = msaaImageAllocations[i];
			if (!allocation2.Image.IsNull())
			{
				GpuMemoryTracker.Remove(GpuMemoryCategory.RenderTarget, in allocation2);
				allocator.FreeAllocation(in allocation2);
			}
		}
		_msaaImageAllocations = Array.Empty<ImageAllocationInfo>();
		_depthImageAllocations = Array.Empty<ImageAllocationInfo>();
	}

	public Device GetDevice()
	{
		return Device;
	}

	public VkRenderPass GetRenderPass()
	{
		return RenderPass;
	}

	public VkCommandPool GetCommandPool()
	{
		return GraphicsCommandPool;
	}

	public FrameResult TrySubmitFrame()
	{
		return TrySubmitFrame(default(Span<VkSemaphore>), default(Span<VkPipelineStageFlags>), default(Span<VkSemaphore>));
	}

	public abstract FrameResult TrySubmitFrame(Span<VkSemaphore> additionalWaitSemaphores, Span<VkPipelineStageFlags> additionalWaitStages, Span<VkSemaphore> additionalSignalSemaphores);

	public VkExtent2D GetExtent()
	{
		return Extent;
	}

	public VkFramebuffer GetFramebuffer(int frameIndex)
	{
		return FrameResources[frameIndex].Framebuffer;
	}

	protected abstract VkResult GetNextFrameIndex(out int imageIndex);

	public FrameResources GetFrame(int frameIndex)
	{
		return FrameResources[frameIndex];
	}

	public FrameAcquireResult TryAcquireNextFrame()
	{
		SyncObjectsIndex = (SyncObjectsIndex + 1) % AllSyncObjects.Length;
		VkFence fence = SyncObjects.Fence;
		DeviceEx device = Device;
		VkFence reference = fence;
		device.WaitForFences(new ReadOnlySpan<VkFence>(in reference), true, -1);
		VkResult nextFrameIndex = GetNextFrameIndex(out FrameIndex);
		bool flag;
		switch (nextFrameIndex)
		{
		case VkResult.Success:
		{
			DeviceEx device2 = Device;
			VkFence reference2 = fence;
			device2.ResetFences(new ReadOnlySpan<VkFence>(in reference2));
			FrameAcquireResult result = default(FrameAcquireResult);
			result.result = FrameResult.Success;
			result.Value = new AcquiredFrame(FrameResources[FrameIndex], SyncObjects.CommandBuffer);
			return result;
		}
		case VkResult.ErrorOutOfDateKHR:
		case VkResult.SuboptimalKHR:
			flag = true;
			break;
		default:
			flag = false;
			break;
		}
		if (flag)
		{
			FrameAcquireResult result = default(FrameAcquireResult);
			result.result = FrameResult.NeedsResize;
			return result;
		}
		switch (nextFrameIndex)
		{
		case VkResult.ErrorSurfaceLostKHR:
		{
			FrameAcquireResult result = default(FrameAcquireResult);
			result.result = FrameResult.SurfaceLost;
			return result;
		}
		case VkResult.NotReady:
		case VkResult.Timeout:
			flag = true;
			break;
		default:
			flag = false;
			break;
		}
		if (flag)
		{
			if (!_loggedAcquireNotReady)
			{
				_loggedAcquireNotReady = true;
				RenderLog.Log.Warning($"vkAcquireNextImageKHR returned {nextFrameIndex} despite an infinite timeout (likely a driver quirk); " + "skipping the frame. This is logged once per render context.", "TryAcquireNextFrame", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Graphics\\RenderContext.cs", 269);
			}
			FrameAcquireResult result = default(FrameAcquireResult);
			result.result = FrameResult.NotReady;
			return result;
		}
		throw new NotImplementedException($"Unexpected result {nextFrameIndex} from TryAcquireNextFrame(), expected Vulkan API to throw for this result.");
	}

	protected unsafe VkRenderPass CreateMainRenderPass()
	{
		bool flag = SampleCount != VkSampleCountFlags._1Bit;
		VkAttachmentDescription vkAttachmentDescription = new VkAttachmentDescription();
		vkAttachmentDescription.Format = ColorFormat;
		vkAttachmentDescription.Samples = SampleCount;
		vkAttachmentDescription.LoadOp = VkAttachmentLoadOp.Clear;
		vkAttachmentDescription.StoreOp = VkAttachmentStoreOp.Store;
		vkAttachmentDescription.StencilLoadOp = VkAttachmentLoadOp.DontCare;
		vkAttachmentDescription.StencilStoreOp = VkAttachmentStoreOp.DontCare;
		vkAttachmentDescription.InitialLayout = VkImageLayout.Undefined;
		vkAttachmentDescription.FinalLayout = FinalLayout;
		VkAttachmentDescription vkAttachmentDescription2 = vkAttachmentDescription;
		if (flag)
		{
			vkAttachmentDescription2.StoreOp = VkAttachmentStoreOp.DontCare;
			vkAttachmentDescription2.FinalLayout = VkImageLayout.ColorAttachmentOptimal;
		}
		vkAttachmentDescription = new VkAttachmentDescription();
		vkAttachmentDescription.Format = DepthFormat;
		vkAttachmentDescription.Samples = SampleCount;
		vkAttachmentDescription.LoadOp = VkAttachmentLoadOp.Clear;
		vkAttachmentDescription.StoreOp = VkAttachmentStoreOp.Store;
		vkAttachmentDescription.StencilLoadOp = VkAttachmentLoadOp.Clear;
		vkAttachmentDescription.StencilStoreOp = VkAttachmentStoreOp.DontCare;
		vkAttachmentDescription.InitialLayout = VkImageLayout.Undefined;
		vkAttachmentDescription.FinalLayout = VkImageLayout.DepthStencilAttachmentOptimal;
		VkAttachmentDescription vkAttachmentDescription3 = vkAttachmentDescription;
		vkAttachmentDescription = new VkAttachmentDescription();
		vkAttachmentDescription.Format = ColorFormat;
		vkAttachmentDescription.Samples = VkSampleCountFlags._1Bit;
		vkAttachmentDescription.LoadOp = VkAttachmentLoadOp.DontCare;
		vkAttachmentDescription.StoreOp = VkAttachmentStoreOp.Store;
		vkAttachmentDescription.StencilLoadOp = VkAttachmentLoadOp.DontCare;
		vkAttachmentDescription.StencilStoreOp = VkAttachmentStoreOp.DontCare;
		vkAttachmentDescription.InitialLayout = VkImageLayout.Undefined;
		vkAttachmentDescription.FinalLayout = FinalLayout;
		VkAttachmentDescription vkAttachmentDescription4 = vkAttachmentDescription;
		VkSubpassDependency vkSubpassDependency = new VkSubpassDependency();
		vkSubpassDependency.SrcSubpass = -1;
		vkSubpassDependency.DstSubpass = 0;
		vkSubpassDependency.SrcStageMask = VkPipelineStageFlags.ColorAttachmentOutputBit;
		vkSubpassDependency.DstStageMask = VkPipelineStageFlags.ColorAttachmentOutputBit;
		vkSubpassDependency.SrcAccessMask = VkAccessFlags.None;
		vkSubpassDependency.DstAccessMask = VkAccessFlags.ColorAttachmentReadBit | VkAccessFlags.ColorAttachmentWriteBit;
		vkSubpassDependency.DependencyFlags = VkDependencyFlags.None;
		VkSubpassDependency vkSubpassDependency2 = vkSubpassDependency;
		vkSubpassDependency = new VkSubpassDependency();
		vkSubpassDependency.SrcSubpass = -1;
		vkSubpassDependency.DstSubpass = 0;
		vkSubpassDependency.SrcStageMask = VkPipelineStageFlags.EarlyFragmentTestsBit | VkPipelineStageFlags.LateFragmentTestsBit;
		vkSubpassDependency.DstStageMask = VkPipelineStageFlags.EarlyFragmentTestsBit | VkPipelineStageFlags.LateFragmentTestsBit;
		vkSubpassDependency.SrcAccessMask = VkAccessFlags.None;
		vkSubpassDependency.DstAccessMask = VkAccessFlags.DepthStencilAttachmentReadBit | VkAccessFlags.DepthStencilAttachmentWriteBit;
		vkSubpassDependency.DependencyFlags = VkDependencyFlags.None;
		VkSubpassDependency vkSubpassDependency3 = vkSubpassDependency;
		VkAttachmentDescription* ptr = stackalloc VkAttachmentDescription[3];
		VkAttachmentReference* ptr2 = stackalloc VkAttachmentReference[3];
		int num = 0;
		VkSubpassDescription vkSubpassDescription = new VkSubpassDescription();
		vkSubpassDescription.PipelineBindPoint = VkPipelineBindPoint.Graphics;
		vkSubpassDescription.ColorAttachmentCount = 1;
		VkSubpassDescription vkSubpassDescription2 = vkSubpassDescription;
		ptr[num] = vkAttachmentDescription2;
		ptr2[num] = new VkAttachmentReference
		{
			Attachment = num,
			Layout = VkImageLayout.ColorAttachmentOptimal
		};
		vkSubpassDescription2.ColorAttachments = ptr2 + num;
		num++;
		if (DepthFormat != 0)
		{
			ptr[num] = vkAttachmentDescription3;
			ptr2[num] = new VkAttachmentReference
			{
				Attachment = num,
				Layout = VkImageLayout.DepthStencilAttachmentOptimal
			};
			vkSubpassDescription2.DepthStencilAttachment = ptr2 + num;
			num++;
		}
		if (flag)
		{
			ptr[num] = vkAttachmentDescription4;
			ptr2[num] = new VkAttachmentReference
			{
				Attachment = num,
				Layout = VkImageLayout.ColorAttachmentOptimal
			};
			vkSubpassDescription2.ResolveAttachments = ptr2 + num;
			num++;
		}
		VkSubpassDependency* dependencies = stackalloc VkSubpassDependency[2] { vkSubpassDependency2, vkSubpassDependency3 };
		VkRenderPassCreateInfo vkRenderPassCreateInfo = new VkRenderPassCreateInfo();
		vkRenderPassCreateInfo.AttachmentCount = num;
		vkRenderPassCreateInfo.Attachments = ptr;
		vkRenderPassCreateInfo.SubpassCount = 1;
		vkRenderPassCreateInfo.Subpasses = &vkSubpassDescription2;
		vkRenderPassCreateInfo.DependencyCount = ((DepthFormat == VkFormat.Undefined) ? 1 : 2);
		vkRenderPassCreateInfo.Dependencies = dependencies;
		VkRenderPassCreateInfo pCreateInfo = vkRenderPassCreateInfo;
		return Device.CreateRenderPass(in pCreateInfo, _allocator);
	}

	protected unsafe void InitializeFrameResources()
	{
		VkExtent3D extent = new VkExtent3D(Extent.Width, Extent.Height, 1);
		VkImageSubresourceRange vkImageSubresourceRange = new VkImageSubresourceRange();
		vkImageSubresourceRange.AspectMask = VkImageAspectFlags.ColorBit;
		vkImageSubresourceRange.BaseMipLevel = 0;
		vkImageSubresourceRange.LevelCount = 1;
		vkImageSubresourceRange.BaseArrayLayer = 0;
		vkImageSubresourceRange.LayerCount = 1;
		VkImageSubresourceRange vkImageSubresourceRange2 = vkImageSubresourceRange;
		VkImageSubresourceRange subresourceRange = vkImageSubresourceRange2;
		subresourceRange.AspectMask = VkImageAspectFlags.DepthBit | VkImageAspectFlags.StencilBit;
		_msaaImageAllocations = new ImageAllocationInfo[ImageCount];
		_depthImageAllocations = new ImageAllocationInfo[ImageCount];
		AllocationInfo.CreateInfo createInfo = default(AllocationInfo.CreateInfo);
		createInfo.Preference = MemoryPreference.PreferGpu;
		createInfo.RequiredProperties = VkMemoryPropertyFlags.DeviceLocalBit;
		createInfo.PreferredProperties = VkMemoryPropertyFlags.DeviceLocalBit;
		createInfo.Priority = 1f;
		AllocationInfo.CreateInfo allocateCreateInfo = createInfo;
		VkImageView* ptr = stackalloc VkImageView[3];
		for (int i = 0; i < ImageCount; i++)
		{
			FrameResources frameResources = FrameResources[i];
			if (SampleCount > VkSampleCountFlags._1Bit)
			{
				VkImageCreateInfo vkImageCreateInfo = new VkImageCreateInfo();
				vkImageCreateInfo.Flags = VkImageCreateFlags.None;
				vkImageCreateInfo.ImageType = VkImageType._2D;
				vkImageCreateInfo.Format = ColorFormat;
				vkImageCreateInfo.Extent = extent;
				vkImageCreateInfo.MipLevels = 1;
				vkImageCreateInfo.ArrayLayers = 1;
				vkImageCreateInfo.Samples = SampleCount;
				vkImageCreateInfo.Tiling = VkImageTiling.Optimal;
				vkImageCreateInfo.Usage = VkImageUsageFlags.SampledBit | VkImageUsageFlags.ColorAttachmentBit;
				vkImageCreateInfo.SharingMode = VkSharingMode.Exclusive;
				vkImageCreateInfo.InitialLayout = VkImageLayout.Undefined;
				VkImageCreateInfo imageCreateInfo = vkImageCreateInfo;
				ImageAllocationInfo allocation = GraphicsDevice.Allocator.AllocateImageChecked($"RenderContext.MsaaColor[{i}]", in imageCreateInfo, in allocateCreateInfo);
				_msaaImageAllocations[i] = allocation;
				GpuMemoryTracker.Add(GpuMemoryCategory.RenderTarget, in allocation, "RenderTarget/Swapchain", $"RenderContext.MsaaColor[{i}]");
				GpuMemoryTracker.SetDetail(in allocation, $"{imageCreateInfo.Extent.Width}x{imageCreateInfo.Extent.Height} {imageCreateInfo.Format}, {SampleCount}");
				frameResources.MultisampledImage = allocation.Image;
				DeviceEx device = Device;
				VkImageViewCreateInfo pCreateInfo = new VkImageViewCreateInfo
				{
					Image = frameResources.MultisampledImage,
					ViewType = VkImageViewType._2D,
					Format = ColorFormat,
					SubresourceRange = vkImageSubresourceRange2
				};
				frameResources.MultisampledImageView = device.CreateImageView(in pCreateInfo, _allocator);
			}
			if (DepthFormat != 0)
			{
				VkImageCreateInfo vkImageCreateInfo = new VkImageCreateInfo();
				vkImageCreateInfo.Flags = VkImageCreateFlags.None;
				vkImageCreateInfo.ImageType = VkImageType._2D;
				vkImageCreateInfo.Format = DepthFormat;
				vkImageCreateInfo.Extent = extent;
				vkImageCreateInfo.MipLevels = 1;
				vkImageCreateInfo.ArrayLayers = 1;
				vkImageCreateInfo.Samples = SampleCount;
				vkImageCreateInfo.Tiling = VkImageTiling.Optimal;
				vkImageCreateInfo.Usage = VkImageUsageFlags.SampledBit | VkImageUsageFlags.DepthStencilAttachmentBit;
				vkImageCreateInfo.SharingMode = VkSharingMode.Exclusive;
				vkImageCreateInfo.InitialLayout = VkImageLayout.Undefined;
				VkImageCreateInfo imageCreateInfo2 = vkImageCreateInfo;
				ImageAllocationInfo allocation2 = GraphicsDevice.Allocator.AllocateImageChecked($"RenderContext.Depth[{i}]", in imageCreateInfo2, in allocateCreateInfo);
				_depthImageAllocations[i] = allocation2;
				GpuMemoryTracker.Add(GpuMemoryCategory.RenderTarget, in allocation2, "RenderTarget/Swapchain", $"RenderContext.Depth[{i}]");
				GpuMemoryTracker.SetDetail(in allocation2, $"{imageCreateInfo2.Extent.Width}x{imageCreateInfo2.Extent.Height} {imageCreateInfo2.Format}, {SampleCount}");
				frameResources.DepthImage = allocation2.Image;
				DeviceEx device2 = Device;
				VkImageViewCreateInfo pCreateInfo = new VkImageViewCreateInfo
				{
					Image = frameResources.DepthImage,
					ViewType = VkImageViewType._2D,
					Format = DepthFormat,
					SubresourceRange = subresourceRange
				};
				frameResources.DepthImageView = device2.CreateImageView(in pCreateInfo, _allocator);
			}
			int attachmentCount = 0;
			if (SampleCount > VkSampleCountFlags._1Bit)
			{
				ptr[attachmentCount++] = frameResources.MultisampledImageView;
				if (DepthFormat != 0)
				{
					ptr[attachmentCount++] = frameResources.DepthImageView;
				}
				ptr[attachmentCount++] = frameResources.ColorImageView;
			}
			else
			{
				ptr[attachmentCount++] = frameResources.ColorImageView;
				if (DepthFormat != 0)
				{
					ptr[attachmentCount++] = frameResources.DepthImageView;
				}
			}
			frameResources.Framebuffer = Device.CreateFramebuffer(new VkFramebufferCreateInfo
			{
				RenderPass = RenderPass,
				AttachmentCount = attachmentCount,
				Attachments = ptr,
				Width = Extent.Width,
				Height = Extent.Height,
				Layers = 1
			}, _allocator);
		}
	}

	protected SyncObjects[] CreateSyncObjects()
	{
		SyncObjects[] array = new SyncObjects[FramesInFlight];
		CommandBuffer[] array2 = new CommandBuffer[FramesInFlight];
		Device.AllocateCommandBuffers(new VkCommandBufferAllocateInfo
		{
			CommandPool = GraphicsCommandPool,
			Level = VkCommandBufferLevel.Primary
		}, array2);
		for (int i = 0; i < array2.Length; i++)
		{
		}
		for (int j = 0; j < FramesInFlight; j++)
		{
			SyncObjects obj = new SyncObjects(Device)
			{
				ImageReadySemaphore = Device.CreateSemaphore(new VkSemaphoreCreateInfo(), _allocator),
				RenderingFinishedSemaphore = Device.CreateSemaphore(new VkSemaphoreCreateInfo(), _allocator)
			};
			obj.Fence = Device.CreateFence(new VkFenceCreateInfo
			{
				Flags = VkFenceCreateFlags.SignaledBit
			}, _allocator);
			obj.CommandBuffer = array2[j];
			obj.Allocator = _allocator;
			SyncObjects syncObjects = obj;
			array[j] = syncObjects;
		}
		return array;
	}

	public PtrOwner<VkPipelineViewportStateCreateInfo> GetViewportState()
	{
		return ViewportState;
	}
}
