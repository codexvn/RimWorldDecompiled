using System;
using System.Collections.Generic;
using System.Threading;
using Brutal.VulkanApi;
using Brutal.VulkanApi.Abstractions;
using Brutal.VulkanApi.Vma;
using Waylith.Graphics;

namespace Waylith.Rendering;

public sealed class RenderTexture : IRenderTextureTarget, IDisposable
{
	private static GraphicsDevice device;

	private static long nextHandle;

	private readonly Dictionary<VkRenderPass, VkFramebuffer> framebuffers = new Dictionary<VkRenderPass, VkFramebuffer>();

	private readonly bool randomWrite;

	private GpuTextureAllocation? color;

	private GpuTextureAllocation? depth;

	private VkImageView depthSampleView;

	private VkSampler sampler;

	private bool created;

	private bool disposed;

	public int Width { get; private set; }

	public int Height { get; private set; }

	public VkFormat ColorFormat { get; }

	public VkFormat DepthFormat { get; }

	public RenderTextureFilter Filter { get; }

	public RenderTextureWrap Wrap { get; }

	public string Name { get; }

	public bool HasDepth => DepthFormat != VkFormat.Undefined;

	public bool IsCreated => created;

	public GpuTextureAllocation Color
	{
		get
		{
			EnsureCreated();
			return color;
		}
	}

	public VkImageView ColorView => Color.ImageView;

	public GpuTextureAllocation? Depth
	{
		get
		{
			EnsureCreated();
			return depth;
		}
	}

	public VkImageView DepthView => Depth?.ImageView ?? default(VkImageView);

	public VkImageView DepthSampleView
	{
		get
		{
			EnsureCreated();
			if (depth == null)
			{
				return default(VkImageView);
			}
			if (DepthAspect(DepthFormat) == VkImageAspectFlags.DepthBit)
			{
				return depth.ImageView;
			}
			if (depthSampleView.IsNull())
			{
				depthSampleView = CreateView(depth.Image, DepthFormat, VkImageAspectFlags.DepthBit);
			}
			return depthSampleView;
		}
	}

	public VkSampler Sampler
	{
		get
		{
			EnsureCreated();
			return sampler;
		}
	}

	public VkExtent2D Extent => new VkExtent2D(Width, Height);

	public RenderTexture(int width, int height, VkFormat colorFormat = VkFormat.R8G8B8A8UNorm, VkFormat depthFormat = VkFormat.Undefined, RenderTextureFilter filter = RenderTextureFilter.Bilinear, RenderTextureWrap wrap = RenderTextureWrap.Clamp, bool randomWrite = false, string? name = null)
	{
		Width = System.Math.Max(1, width);
		Height = System.Math.Max(1, height);
		ColorFormat = colorFormat;
		DepthFormat = depthFormat;
		Filter = filter;
		Wrap = wrap;
		this.randomWrite = randomWrite;
		Name = (string.IsNullOrWhiteSpace(name) ? $"RenderTexture_{Interlocked.Increment(ref nextHandle)}" : name);
	}

	public static void Configure(GraphicsDevice graphicsDevice)
	{
		device = graphicsDevice;
	}

	public void Create()
	{
		if (!created)
		{
			if (device == null)
			{
				throw new InvalidOperationException("RenderTexture.Configure must be called before creating render textures.");
			}
			if (sampler.IsNull())
			{
				sampler = CreateSampler();
			}
			AllocateImages();
			created = true;
		}
	}

	public void Resize(int width, int height)
	{
		width = System.Math.Max(1, width);
		height = System.Math.Max(1, height);
		if (width != Width || height != Height)
		{
			Width = width;
			Height = height;
			if (created)
			{
				ReleaseImages();
				AllocateImages();
			}
		}
	}

	private void AllocateImages()
	{
		VkImageUsageFlags vkImageUsageFlags = VkImageUsageFlags.TransferSrcBit | VkImageUsageFlags.TransferDstBit | VkImageUsageFlags.SampledBit | VkImageUsageFlags.ColorAttachmentBit;
		if (randomWrite)
		{
			vkImageUsageFlags |= VkImageUsageFlags.StorageBit;
		}
		color = Allocate(Name + ".Color", ColorFormat, vkImageUsageFlags, VkImageAspectFlags.ColorBit);
		if (HasDepth)
		{
			depth = Allocate(Name + ".Depth", DepthFormat, VkImageUsageFlags.SampledBit | VkImageUsageFlags.DepthStencilAttachmentBit, DepthAspect(DepthFormat));
		}
	}

	public VkFramebuffer GetFramebuffer(VkRenderPass renderPass)
	{
		EnsureCreated();
		if (framebuffers.TryGetValue(renderPass, out var value))
		{
			return value;
		}
		VkFramebuffer vkFramebuffer = CreateFramebuffer(renderPass);
		framebuffers[renderPass] = vkFramebuffer;
		return vkFramebuffer;
	}

	public void Release()
	{
		ReleaseImages();
		if (!sampler.IsNull())
		{
			device.Device.DestroySampler(sampler, device.Device.VkAllocator);
			sampler = default(VkSampler);
		}
		created = false;
	}

	private void ReleaseImages()
	{
		foreach (VkFramebuffer value in framebuffers.Values)
		{
			device.Device.DestroyFramebuffer(value, device.Device.VkAllocator);
		}
		framebuffers.Clear();
		color?.Dispose();
		color = null;
		if (!depthSampleView.IsNull())
		{
			device.Device.DestroyImageView(depthSampleView, device.Device.VkAllocator);
			depthSampleView = default(VkImageView);
		}
		depth?.Dispose();
		depth = null;
	}

	public void Dispose()
	{
		if (!disposed)
		{
			disposed = true;
			Release();
		}
	}

	private void EnsureCreated()
	{
		if (!created)
		{
			Create();
		}
	}

	private GpuTextureAllocation Allocate(string label, VkFormat format, VkImageUsageFlags usage, VkImageAspectFlags aspect)
	{
		VmaAllocator allocator = device.Allocator;
		VkImageCreateInfo createInfo = new VkImageCreateInfo
		{
			ImageType = VkImageType._2D,
			Format = format,
			Extent = new VkExtent3D(Width, Height, 1),
			MipLevels = 1,
			ArrayLayers = 1,
			Samples = VkSampleCountFlags._1Bit,
			Tiling = VkImageTiling.Optimal,
			Usage = usage,
			SharingMode = VkSharingMode.Exclusive,
			InitialLayout = VkImageLayout.Undefined
		};
		ImageAllocationInfo allocation = GpuTextureService.AllocateTextureImage(allocator, label, GpuMemoryCategory.RenderTarget, in createInfo);
		VkImageView imageView = CreateView(allocation.Image, format, aspect);
		nint handle = new IntPtr(Interlocked.Increment(ref nextHandle));
		GpuMemoryTracker.SetDetail(in allocation, GpuTextureService.TextureDetail(Width, Height, format, 1));
		return new GpuTextureAllocation(device.Device, device.Allocator, allocation, imageView, handle, 1, format, label, GpuMemoryCategory.RenderTarget);
	}

	private unsafe VkFramebuffer CreateFramebuffer(VkRenderPass renderPass)
	{
		VkImageView* ptr = stackalloc VkImageView[2];
		int attachmentCount = 0;
		ptr[attachmentCount++] = color.ImageView;
		if (depth != null)
		{
			ptr[attachmentCount++] = depth.ImageView;
		}
		return device.Device.CreateFramebuffer(new VkFramebufferCreateInfo
		{
			RenderPass = renderPass,
			AttachmentCount = attachmentCount,
			Attachments = ptr,
			Width = Width,
			Height = Height,
			Layers = 1
		}, device.Device.VkAllocator);
	}

	private VkSampler CreateSampler()
	{
		VkFilter vkFilter = ((Filter != 0) ? VkFilter.Linear : VkFilter.Nearest);
		VkSamplerAddressMode vkSamplerAddressMode = ((Wrap != RenderTextureWrap.Repeat) ? VkSamplerAddressMode.ClampToEdge : VkSamplerAddressMode.Repeat);
		return device.Device.CreateSampler(new VkSamplerCreateInfo
		{
			MagFilter = vkFilter,
			MinFilter = vkFilter,
			MipmapMode = VkSamplerMipmapMode.Nearest,
			AddressModeU = vkSamplerAddressMode,
			AddressModeV = vkSamplerAddressMode,
			AddressModeW = vkSamplerAddressMode,
			MinLod = 0f,
			MaxLod = 0f,
			BorderColor = VkBorderColor.FloatOpaqueBlack
		}, device.Device.VkAllocator);
	}

	private static VkImageView CreateView(VkImage image, VkFormat format, VkImageAspectFlags aspect)
	{
		DeviceEx handle = device.Device;
		VkImageViewCreateInfo pCreateInfo = new VkImageViewCreateInfo
		{
			Image = image,
			ViewType = VkImageViewType._2D,
			Format = format,
			SubresourceRange = new VkImageSubresourceRange
			{
				AspectMask = aspect,
				BaseMipLevel = 0,
				LevelCount = 1,
				BaseArrayLayer = 0,
				LayerCount = 1
			}
		};
		return handle.CreateImageView(in pCreateInfo, device.Device.VkAllocator);
	}

	private static VkImageAspectFlags DepthAspect(VkFormat format)
	{
		return format switch
		{
			VkFormat.D24UNormS8UInt => VkImageAspectFlags.DepthBit | VkImageAspectFlags.StencilBit, 
			VkFormat.D32SFloatS8UInt => VkImageAspectFlags.DepthBit | VkImageAspectFlags.StencilBit, 
			_ => VkImageAspectFlags.DepthBit, 
		};
	}
}
