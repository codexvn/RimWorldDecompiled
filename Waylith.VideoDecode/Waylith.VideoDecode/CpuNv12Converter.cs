using System;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using Brutal;
using Brutal.VulkanApi;
using Brutal.VulkanApi.Abstractions;
using Waylith.Graphics;
using Waylith.Rendering;
using Waylith.Rendering.Compute;

namespace Waylith.VideoDecode;

internal sealed class CpuNv12Converter : IDisposable
{
	private const long FenceTimeoutNs = 5000000000L;

	private const string ConvertKernelPath = "Compute/Nv12ToRgba.comp.glsl";

	private static readonly ComputeBinding[] ConvertBindings = new ComputeBinding[3]
	{
		new ComputeBinding(0, ComputeBindingType.CombinedImageSampler),
		new ComputeBinding(1, ComputeBindingType.CombinedImageSampler),
		new ComputeBinding(2, ComputeBindingType.StorageImage)
	};

	private readonly GraphicsDevice graphicsDevice;

	private readonly DeviceEx device;

	private readonly ComputeKernel kernel;

	private readonly VkDescriptorPool descriptorPool;

	private readonly VkCommandPool commandPool;

	private readonly CommandBuffer commands;

	private readonly VkFence fence;

	private GpuTextureAllocation luma;

	private GpuTextureAllocation chroma;

	private GpuComputeBuffer staging;

	private int stride;

	private int rows;

	public unsafe CpuNv12Converter(GraphicsDevice graphicsDevice)
	{
		this.graphicsDevice = graphicsDevice;
		device = graphicsDevice.Device;
		kernel = ComputeKernelCache.Load("Compute/Nv12ToRgba.comp.glsl", "main", ConvertBindings, sizeof(Nv12ToRgbaPush));
		descriptorPool = ComputeDispatchQueue.CreateDescriptorPool(1);
		DeviceEx handle = device;
		VkCommandPoolCreateInfo pCreateInfo = new VkCommandPoolCreateInfo
		{
			QueueFamilyIndex = graphicsDevice.GraphicsFamilyIndex,
			Flags = VkCommandPoolCreateFlags.ResetCommandBufferBit
		};
		commandPool = handle.CreateCommandPool(in pCreateInfo, device.VkAllocator);
		commands = device.AllocateCommandBuffer(new VkCommandBufferAllocateInfo
		{
			CommandPool = commandPool,
			Level = VkCommandBufferLevel.Primary
		});
		fence = device.CreateFence(new VkFenceCreateInfo(), device.VkAllocator);
	}

	public void Convert(ReadOnlySpan<byte> nv12, int frameStride, int frameRows, in Nv12ToRgbaPush push, GpuTextureAllocation target)
	{
		if (frameStride != stride || frameRows != rows)
		{
			Resize(frameStride, frameRows);
		}
		int length = stride * rows * 3 / 2;
		using (GpuComputeBuffer.Mapping mapping = staging.Map())
		{
			nv12.Slice(0, length).CopyTo(mapping.Slice(0L, length));
		}
		Submit(in push, target);
	}

	public unsafe void ConvertPlanar(byte* y, int yStride, byte* u, byte* v, int uvStride, int width, int height, in Nv12ToRgbaPush push, GpuTextureAllocation target)
	{
		if (width != stride || height != rows)
		{
			Resize(width, height);
		}
		using (GpuComputeBuffer.Mapping mapping = staging.Map())
		{
			fixed (byte* ptr = mapping.Slice(0L, stride * rows * 3 / 2))
			{
				for (int i = 0; i < rows; i++)
				{
					Buffer.MemoryCopy(y + i * yStride, ptr + i * stride, stride, stride);
				}
				byte* ptr2 = ptr + stride * rows;
				for (int j = 0; j < rows / 2; j++)
				{
					Interleave(u + j * uvStride, v + j * uvStride, ptr2 + j * stride, stride / 2);
				}
			}
		}
		Submit(in push, target);
	}

	private unsafe static void Interleave(byte* u, byte* v, byte* destination, int count)
	{
		int i;
		for (i = 0; i + 16 <= count; i += 16)
		{
			Vector128<byte> source = Vector128.Load(u + i);
			Vector128<byte> source2 = Vector128.Load(v + i);
			(Vector128.WidenLower(source) | Vector128.ShiftLeft(Vector128.WidenLower(source2), 8)).AsByte().Store(destination + i * 2);
			(Vector128.WidenUpper(source) | Vector128.ShiftLeft(Vector128.WidenUpper(source2), 8)).AsByte().Store(destination + i * 2 + 16);
		}
		for (; i < count; i++)
		{
			destination[i * 2] = u[i];
			destination[i * 2 + 1] = v[i];
		}
	}

	private void Submit(in Nv12ToRgbaPush push, GpuTextureAllocation target)
	{
		int num = stride * rows;
		device.ResetDescriptorPool(descriptorPool, 0);
		commands.Reset();
		commands.Begin();
		InlineArray2<VkImageMemoryBarrier> buffer = default(InlineArray2<VkImageMemoryBarrier>);
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray2<VkImageMemoryBarrier>, VkImageMemoryBarrier>(ref buffer, 0) = PlaneBarrier(luma, VkImageLayout.Undefined, VkImageLayout.TransferDstOptimal, VkAccessFlags.None, VkAccessFlags.TransferWriteBit);
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray2<VkImageMemoryBarrier>, VkImageMemoryBarrier>(ref buffer, 1) = PlaneBarrier(chroma, VkImageLayout.Undefined, VkImageLayout.TransferDstOptimal, VkAccessFlags.None, VkAccessFlags.TransferWriteBit);
		Span<VkImageMemoryBarrier> span = global::_003CPrivateImplementationDetails_003E.InlineArrayAsSpan<InlineArray2<VkImageMemoryBarrier>, VkImageMemoryBarrier>(ref buffer, 2);
		commands.PipelineBarrier(VkPipelineStageFlags.TopOfPipeBit, VkPipelineStageFlags.TransferBit, VkDependencyFlags.None, default(ReadOnlySpan<VkMemoryBarrier>), default(ReadOnlySpan<VkBufferMemoryBarrier>), span);
		CommandBuffer handle = commands;
		VkBuffer buffer2 = staging.Buffer;
		VkImage image = luma.Image;
		VkBufferImageCopy reference = PlaneCopy(0L, stride, rows);
		handle.CopyBufferToImage(buffer2, image, VkImageLayout.TransferDstOptimal, new ReadOnlySpan<VkBufferImageCopy>(in reference));
		CommandBuffer handle2 = commands;
		VkBuffer buffer3 = staging.Buffer;
		VkImage image2 = chroma.Image;
		VkBufferImageCopy reference2 = PlaneCopy(num, stride / 2, rows / 2);
		handle2.CopyBufferToImage(buffer3, image2, VkImageLayout.TransferDstOptimal, new ReadOnlySpan<VkBufferImageCopy>(in reference2));
		InlineArray2<VkImageMemoryBarrier> buffer4 = default(InlineArray2<VkImageMemoryBarrier>);
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray2<VkImageMemoryBarrier>, VkImageMemoryBarrier>(ref buffer4, 0) = PlaneBarrier(luma, VkImageLayout.TransferDstOptimal, VkImageLayout.ShaderReadOnlyOptimal, VkAccessFlags.TransferWriteBit, VkAccessFlags.ShaderReadBit);
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray2<VkImageMemoryBarrier>, VkImageMemoryBarrier>(ref buffer4, 1) = PlaneBarrier(chroma, VkImageLayout.TransferDstOptimal, VkImageLayout.ShaderReadOnlyOptimal, VkAccessFlags.TransferWriteBit, VkAccessFlags.ShaderReadBit);
		Span<VkImageMemoryBarrier> span2 = global::_003CPrivateImplementationDetails_003E.InlineArrayAsSpan<InlineArray2<VkImageMemoryBarrier>, VkImageMemoryBarrier>(ref buffer4, 2);
		commands.PipelineBarrier(VkPipelineStageFlags.TransferBit, VkPipelineStageFlags.ComputeShaderBit, VkDependencyFlags.None, default(ReadOnlySpan<VkMemoryBarrier>), default(ReadOnlySpan<VkBufferMemoryBarrier>), span2);
		ComputeDispatch computeDispatch = ComputeDispatch.Rent(kernel, (push.Width + 7) / 8, (push.Height + 7) / 8);
		computeDispatch.SetSampledView(0, luma.ImageView, ComputeDispatchQueue.PointClampSampler);
		computeDispatch.SetSampledView(1, chroma.ImageView, ComputeDispatchQueue.BilinearClampSampler);
		computeDispatch.SetStorageImage(2, target);
		computeDispatch.SetPushConstants(in push);
		ComputeDispatchQueue.RecordDispatch(commands, device, descriptorPool, computeDispatch);
		ComputeDispatch.Return(computeDispatch);
		commands.End();
		GraphicsDevice obj = graphicsDevice;
		CommandBuffer reference3 = commands;
		obj.SubmitGraphics(default(Span<VkSemaphore>), default(Span<VkPipelineStageFlags>), new Span<CommandBuffer>(ref reference3), default(Span<VkSemaphore>), fence);
		VkResult vkResult = device.WaitForFence(fence, unchecked((nint)5000000000L));
		if (vkResult != 0)
		{
			throw new InvalidOperationException($"video conversion fence wait returned {vkResult}");
		}
		DeviceEx handle3 = device;
		VkFence reference4 = fence;
		handle3.ResetFences(new ReadOnlySpan<VkFence>(in reference4));
	}

	private static VkImageMemoryBarrier PlaneBarrier(GpuTextureAllocation plane, VkImageLayout from, VkImageLayout to, VkAccessFlags sourceAccess, VkAccessFlags destinationAccess)
	{
		VkImageMemoryBarrier result = new VkImageMemoryBarrier();
		result.SrcAccessMask = sourceAccess;
		result.DstAccessMask = destinationAccess;
		result.OldLayout = from;
		result.NewLayout = to;
		result.SrcQueueFamilyIndex = -1;
		result.DstQueueFamilyIndex = -1;
		result.Image = plane.Image;
		result.SubresourceRange = new VkImageSubresourceRange
		{
			AspectMask = VkImageAspectFlags.ColorBit,
			LevelCount = 1,
			LayerCount = 1
		};
		return result;
	}

	private static VkBufferImageCopy PlaneCopy(long offset, int width, int height)
	{
		VkBufferImageCopy result = new VkBufferImageCopy();
		result.BufferOffset = ByteSize.Of(offset);
		result.ImageSubresource = new VkImageSubresourceLayers
		{
			AspectMask = VkImageAspectFlags.ColorBit,
			LayerCount = 1
		};
		result.ImageExtent = new VkExtent3D(width, height, 1);
		return result;
	}

	private void Resize(int frameStride, int frameRows)
	{
		FreePlanes();
		stride = frameStride;
		rows = frameRows;
		luma = GpuTextureService.AllocateTexture2D("VideoLuma", stride, rows, 1, VkFormat.R8UNorm, GpuMemoryCategory.Other);
		chroma = GpuTextureService.AllocateTexture2D("VideoChroma", stride / 2, rows / 2, 1, VkFormat.R8G8UNorm, GpuMemoryCategory.Other);
		if (luma == null || chroma == null)
		{
			throw new InvalidOperationException("could not allocate video plane textures");
		}
		staging = GpuComputeBuffer.Create(device, graphicsDevice.Allocator, "VideoStaging", stride * rows * 3 / 2, VkBufferUsageFlags.TransferSrcBit);
	}

	private void FreePlanes()
	{
		luma?.DisposeNow();
		chroma?.DisposeNow();
		staging?.DisposeNow();
		luma = (chroma = null);
		staging = null;
	}

	public void Dispose()
	{
		FreePlanes();
		device.DestroyFence(fence, device.VkAllocator);
		device.DestroyCommandPool(commandPool, device.VkAllocator);
		device.DestroyDescriptorPool(descriptorPool, device.VkAllocator);
	}
}
