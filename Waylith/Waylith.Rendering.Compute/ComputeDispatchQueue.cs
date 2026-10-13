using System;
using System.Collections.Generic;
using Brutal;
using Brutal.VulkanApi;
using Brutal.VulkanApi.Abstractions;
using Waylith.Graphics;

namespace Waylith.Rendering.Compute;

public static class ComputeDispatchQueue
{
	private const int ImmediateMaxSets = 16;

	private static GraphicsDevice? graphicsDevice;

	private static DeviceEx device = null;

	private static VkCommandPool immediateCommandPool;

	private static VkDescriptorPool immediateDescriptorPool;

	private static CommandBuffer immediateCommandBuffer;

	private static VkFence immediateFence;

	private static bool immediateSubmitted;

	private static List<ComputeDispatch> pending = new List<ComputeDispatch>();

	private static readonly object pendingLock = new object();

	public static VkSampler BilinearClampSampler { get; private set; }

	public static VkSampler PointClampSampler { get; private set; }

	public static void Configure(GraphicsDevice gfx)
	{
		graphicsDevice = gfx;
		device = gfx.Device;
		DeviceEx handle = device;
		VkCommandPoolCreateInfo pCreateInfo = new VkCommandPoolCreateInfo
		{
			QueueFamilyIndex = gfx.GraphicsQueue.Family,
			Flags = (VkCommandPoolCreateFlags.TransientBit | VkCommandPoolCreateFlags.ResetCommandBufferBit)
		};
		immediateCommandPool = handle.CreateCommandPool(in pCreateInfo, device.VkAllocator);
		immediateDescriptorPool = CreateDescriptorPool(16);
		immediateCommandBuffer = device.AllocateCommandBuffer(new VkCommandBufferAllocateInfo
		{
			CommandPool = immediateCommandPool,
			Level = VkCommandBufferLevel.Primary
		});
		immediateFence = device.CreateFence(new VkFenceCreateInfo(), device.VkAllocator);
		DeviceEx handle2 = device;
		VkSamplerCreateInfo pCreateInfo2 = new VkSamplerCreateInfo
		{
			MagFilter = VkFilter.Linear,
			MinFilter = VkFilter.Linear,
			MipmapMode = VkSamplerMipmapMode.Nearest,
			AddressModeU = VkSamplerAddressMode.ClampToEdge,
			AddressModeV = VkSamplerAddressMode.ClampToEdge,
			AddressModeW = VkSamplerAddressMode.ClampToEdge,
			MinLod = 0f,
			MaxLod = float.MaxValue,
			BorderColor = VkBorderColor.FloatOpaqueBlack
		};
		BilinearClampSampler = handle2.CreateSampler(in pCreateInfo2, device.VkAllocator);
		DeviceEx handle3 = device;
		pCreateInfo2 = new VkSamplerCreateInfo
		{
			MagFilter = VkFilter.Nearest,
			MinFilter = VkFilter.Nearest,
			MipmapMode = VkSamplerMipmapMode.Nearest,
			AddressModeU = VkSamplerAddressMode.ClampToEdge,
			AddressModeV = VkSamplerAddressMode.ClampToEdge,
			AddressModeW = VkSamplerAddressMode.ClampToEdge,
			MinLod = 0f,
			MaxLod = float.MaxValue,
			BorderColor = VkBorderColor.FloatOpaqueBlack
		};
		PointClampSampler = handle3.CreateSampler(in pCreateInfo2, device.VkAllocator);
	}

	public static void Shutdown()
	{
		if (graphicsDevice != null)
		{
			WaitImmediate();
			device.DestroyFence(immediateFence, device.VkAllocator);
			device.DestroyCommandPool(immediateCommandPool, device.VkAllocator);
			device.DestroyDescriptorPool(immediateDescriptorPool, device.VkAllocator);
			device.DestroySampler(BilinearClampSampler, device.VkAllocator);
			device.DestroySampler(PointClampSampler, device.VkAllocator);
			graphicsDevice = null;
		}
	}

	public static void Enqueue(ComputeDispatch dispatch, object? owner = null, int key = -1)
	{
		lock (pendingLock)
		{
			dispatch.Owner = owner;
			dispatch.Key = key;
			if (key >= 0)
			{
				for (int i = 0; i < pending.Count; i++)
				{
					ComputeDispatch computeDispatch = pending[i];
					if (computeDispatch.Key == key && computeDispatch.Owner == owner)
					{
						ComputeDispatch.Return(computeDispatch);
						pending[i] = dispatch;
						return;
					}
				}
			}
			pending.Add(dispatch);
		}
	}

	public static void CancelPending(object owner)
	{
		lock (pendingLock)
		{
			int num = 0;
			for (int i = 0; i < pending.Count; i++)
			{
				ComputeDispatch computeDispatch = pending[i];
				if (computeDispatch.Owner == owner)
				{
					ComputeDispatch.Return(computeDispatch);
				}
				else
				{
					pending[num++] = computeDispatch;
				}
			}
			pending.RemoveRange(num, pending.Count - num);
		}
	}

	public static void DispatchNow(ComputeDispatch dispatch, Action<CommandBuffer>? prologue = null)
	{
		ComputeDispatch reference = dispatch;
		DispatchNow(new ReadOnlySpan<ComputeDispatch>(in reference), prologue);
	}

	public static void DispatchNow(ReadOnlySpan<ComputeDispatch> dispatches, Action<CommandBuffer>? prologue = null)
	{
		Dispatch(dispatches, prologue);
		WaitImmediate();
	}

	public static void Dispatch(ReadOnlySpan<ComputeDispatch> dispatches, Action<CommandBuffer>? prologue = null)
	{
		if (dispatches.Length > 16)
		{
			throw new ArgumentOutOfRangeException("dispatches", $"Immediate dispatch batch of {dispatches.Length} exceeds the pool capacity of {16}.");
		}
		WaitImmediate();
		CommandBuffer commandBuffer = immediateCommandBuffer;
		device.ResetDescriptorPool(immediateDescriptorPool, 0);
		commandBuffer.Begin(VkCommandBufferUsageFlags.OneTimeSubmitBit);
		prologue?.Invoke(commandBuffer);
		for (int i = 0; i < dispatches.Length; i++)
		{
			if (i > 0)
			{
				RecordComputeChainBarrier(commandBuffer);
			}
			RecordDispatch(commandBuffer, device, immediateDescriptorPool, dispatches[i]);
		}
		commandBuffer.End();
		GraphicsDevice? obj = graphicsDevice;
		CommandBuffer reference = commandBuffer;
		obj.SubmitGraphics(default(Span<VkSemaphore>), default(Span<VkPipelineStageFlags>), new Span<CommandBuffer>(ref reference), default(Span<VkSemaphore>), immediateFence);
		immediateSubmitted = true;
		for (int j = 0; j < dispatches.Length; j++)
		{
			ComputeDispatch.Return(dispatches[j]);
		}
	}

	public static void WaitImmediate()
	{
		if (immediateSubmitted)
		{
			device.WaitForFence(immediateFence, -1);
			DeviceEx handle = device;
			VkFence reference = immediateFence;
			handle.ResetFences(new ReadOnlySpan<VkFence>(in reference));
			immediateSubmitted = false;
		}
	}

	internal static void RecordComputeChainBarrier(CommandBuffer cmdBuf)
	{
		VkMemoryBarrier vkMemoryBarrier = new VkMemoryBarrier();
		vkMemoryBarrier.SrcAccessMask = VkAccessFlags.ShaderWriteBit;
		vkMemoryBarrier.DstAccessMask = VkAccessFlags.ShaderReadBit;
		VkMemoryBarrier reference = vkMemoryBarrier;
		Span<VkMemoryBarrier> span = new Span<VkMemoryBarrier>(ref reference);
		cmdBuf.PipelineBarrier(VkPipelineStageFlags.ComputeShaderBit, VkPipelineStageFlags.ComputeShaderBit, VkDependencyFlags.None, span, Array.Empty<VkBufferMemoryBarrier>(), default(Span<VkImageMemoryBarrier>));
	}

	public static List<ComputeDispatch>? DrainPending()
	{
		lock (pendingLock)
		{
			if (pending.Count == 0)
			{
				return null;
			}
			List<ComputeDispatch> result = pending;
			pending = new List<ComputeDispatch>();
			return result;
		}
	}

	internal unsafe static void RecordDispatch(CommandBuffer cmdBuf, DeviceEx device, VkDescriptorPool descriptorPool, ComputeDispatch dispatch)
	{
		ComputeKernel kernel = dispatch.Kernel;
		ReadOnlySpan<ComputeBinding> bindings = kernel.Bindings;
		int length = bindings.Length;
		VkImageMemoryBarrier* ptr = stackalloc VkImageMemoryBarrier[length];
		int num = 0;
		for (int i = 0; i < length; i++)
		{
			if (bindings[i].Type == ComputeBindingType.StorageImage)
			{
				GpuTextureAllocation storageImage = dispatch.GetStorageImage(bindings[i].Binding);
				if (storageImage != null)
				{
					ptr[num++] = MakeImageBarrier(storageImage, VkAccessFlags.ShaderReadBit, VkAccessFlags.ShaderReadBit | VkAccessFlags.ShaderWriteBit, storageImage.CurrentLayout, VkImageLayout.General);
					SetTextureLayout(storageImage, VkImageLayout.General);
				}
			}
		}
		if (num > 0)
		{
			cmdBuf.PipelineBarrier(VkPipelineStageFlags.FragmentShaderBit | VkPipelineStageFlags.ComputeShaderBit | VkPipelineStageFlags.TransferBit, VkPipelineStageFlags.ComputeShaderBit, VkDependencyFlags.None, default(ReadOnlySpan<VkMemoryBarrier>), default(ReadOnlySpan<VkBufferMemoryBarrier>), new ReadOnlySpan<VkImageMemoryBarrier>(ptr, num));
		}
		cmdBuf.BindPipeline(VkPipelineBindPoint.Compute, kernel.Pipeline);
		VkDescriptorSet vkDescriptorSet = DescriptorSetWriter.AllocateDescriptorSet(device, descriptorPool, kernel.DescriptorSetLayout);
		VkDescriptorImageInfo* ptr2 = stackalloc VkDescriptorImageInfo[length];
		VkDescriptorBufferInfo* ptr3 = stackalloc VkDescriptorBufferInfo[length];
		VkWriteDescriptorSet* ptr4 = stackalloc VkWriteDescriptorSet[length];
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		for (int j = 0; j < length; j++)
		{
			ComputeBinding computeBinding = bindings[j];
			switch (computeBinding.Type)
			{
			case ComputeBindingType.CombinedImageSampler:
			{
				VkImageView sampledView2 = dispatch.GetSampledView(computeBinding.Binding);
				if (!sampledView2.IsNull())
				{
					ptr2[num3] = new VkDescriptorImageInfo
					{
						Sampler = dispatch.GetSampler(computeBinding.Binding),
						ImageView = sampledView2,
						ImageLayout = dispatch.GetSampledLayout(computeBinding.Binding)
					};
					ptr4[num2++] = new VkWriteDescriptorSet
					{
						DstSet = vkDescriptorSet,
						DstBinding = computeBinding.Binding,
						DescriptorCount = 1,
						DescriptorType = VkDescriptorType.CombinedImageSampler,
						ImageInfo = ptr2 + num3++
					};
				}
				break;
			}
			case ComputeBindingType.SampledImage:
			{
				VkImageView sampledView = dispatch.GetSampledView(computeBinding.Binding);
				if (!sampledView.IsNull())
				{
					ptr2[num3] = new VkDescriptorImageInfo
					{
						ImageView = sampledView,
						ImageLayout = dispatch.GetSampledLayout(computeBinding.Binding)
					};
					ptr4[num2++] = new VkWriteDescriptorSet
					{
						DstSet = vkDescriptorSet,
						DstBinding = computeBinding.Binding,
						DescriptorCount = 1,
						DescriptorType = VkDescriptorType.SampledImage,
						ImageInfo = ptr2 + num3++
					};
				}
				break;
			}
			case ComputeBindingType.StorageImage:
			{
				GpuTextureAllocation storageImage2 = dispatch.GetStorageImage(computeBinding.Binding);
				if (storageImage2 != null)
				{
					ptr2[num3] = new VkDescriptorImageInfo
					{
						ImageView = storageImage2.ImageView,
						ImageLayout = VkImageLayout.General
					};
					ptr4[num2++] = new VkWriteDescriptorSet
					{
						DstSet = vkDescriptorSet,
						DstBinding = computeBinding.Binding,
						DescriptorCount = 1,
						DescriptorType = VkDescriptorType.StorageImage,
						ImageInfo = ptr2 + num3++
					};
				}
				break;
			}
			case ComputeBindingType.StorageBuffer:
			{
				GpuComputeBuffer storageBuffer = dispatch.GetStorageBuffer(computeBinding.Binding);
				if (storageBuffer != null)
				{
					ptr3[num4] = new VkDescriptorBufferInfo
					{
						Buffer = storageBuffer.Buffer,
						Offset = ByteSize.Zero,
						Range = storageBuffer.Size
					};
					ptr4[num2++] = new VkWriteDescriptorSet
					{
						DstSet = vkDescriptorSet,
						DstBinding = computeBinding.Binding,
						DescriptorCount = 1,
						DescriptorType = VkDescriptorType.StorageBuffer,
						BufferInfo = ptr3 + num4++
					};
				}
				break;
			}
			}
		}
		if (num2 > 0)
		{
			device.UpdateDescriptorSets(new ReadOnlySpan<VkWriteDescriptorSet>(ptr4, num2), default(ReadOnlySpan<VkCopyDescriptorSet>));
		}
		cmdBuf.BindDescriptorSet(VkPipelineBindPoint.Compute, kernel.PipelineLayout, 0, vkDescriptorSet, default(Span<ByteSize32>));
		ReadOnlySpan<byte> pushConstantData = dispatch.GetPushConstantData();
		if (!pushConstantData.IsEmpty)
		{
			cmdBuf.PushConstants(kernel.PipelineLayout, VkShaderStageFlags.ComputeBit, ByteSize.Zero, pushConstantData);
		}
		cmdBuf.Dispatch(dispatch.ThreadGroupsX, dispatch.ThreadGroupsY, dispatch.ThreadGroupsZ);
		VkImageMemoryBarrier* ptr5 = stackalloc VkImageMemoryBarrier[length];
		int num5 = 0;
		for (int k = 0; k < length; k++)
		{
			if (bindings[k].Type == ComputeBindingType.StorageImage)
			{
				GpuTextureAllocation storageImage3 = dispatch.GetStorageImage(bindings[k].Binding);
				if (storageImage3 != null)
				{
					ptr5[num5++] = MakeImageBarrier(storageImage3, VkAccessFlags.ShaderWriteBit, VkAccessFlags.ShaderReadBit, VkImageLayout.General, VkImageLayout.ShaderReadOnlyOptimal);
					SetTextureLayout(storageImage3, VkImageLayout.ShaderReadOnlyOptimal);
				}
			}
		}
		if (num5 > 0)
		{
			cmdBuf.PipelineBarrier(VkPipelineStageFlags.ComputeShaderBit, VkPipelineStageFlags.FragmentShaderBit | VkPipelineStageFlags.ComputeShaderBit, VkDependencyFlags.None, default(ReadOnlySpan<VkMemoryBarrier>), default(ReadOnlySpan<VkBufferMemoryBarrier>), new ReadOnlySpan<VkImageMemoryBarrier>(ptr5, num5));
		}
	}

	private static void SetTextureLayout(GpuTextureAllocation texture, VkImageLayout layout)
	{
		for (int i = 0; i < texture.MipCount; i++)
		{
			texture.SetMipLayout(i, layout);
		}
	}

	private static VkImageMemoryBarrier MakeImageBarrier(GpuTextureAllocation texture, VkAccessFlags srcAccess, VkAccessFlags dstAccess, VkImageLayout oldLayout, VkImageLayout newLayout)
	{
		VkImageMemoryBarrier result = new VkImageMemoryBarrier();
		result.SrcAccessMask = srcAccess;
		result.DstAccessMask = dstAccess;
		result.OldLayout = oldLayout;
		result.NewLayout = newLayout;
		result.SrcQueueFamilyIndex = -1;
		result.DstQueueFamilyIndex = -1;
		result.Image = texture.Image;
		result.SubresourceRange = new VkImageSubresourceRange
		{
			AspectMask = VkImageAspectFlags.ColorBit,
			BaseMipLevel = 0,
			LevelCount = texture.MipCount,
			BaseArrayLayer = 0,
			LayerCount = -1
		};
		return result;
	}

	internal unsafe static VkDescriptorPool CreateDescriptorPool(int maxSets)
	{
		VkDescriptorPoolSize* ptr = stackalloc VkDescriptorPoolSize[4];
		*ptr = new VkDescriptorPoolSize
		{
			Type = VkDescriptorType.CombinedImageSampler,
			DescriptorCount = maxSets * 8
		};
		ptr[1] = new VkDescriptorPoolSize
		{
			Type = VkDescriptorType.StorageImage,
			DescriptorCount = maxSets * 8
		};
		ptr[2] = new VkDescriptorPoolSize
		{
			Type = VkDescriptorType.StorageBuffer,
			DescriptorCount = maxSets * 8
		};
		ptr[3] = new VkDescriptorPoolSize
		{
			Type = VkDescriptorType.SampledImage,
			DescriptorCount = maxSets * 8
		};
		DeviceEx handle = device;
		VkDescriptorPoolCreateInfo pCreateInfo = new VkDescriptorPoolCreateInfo
		{
			MaxSets = maxSets,
			PoolSizeCount = 4,
			PoolSizes = ptr
		};
		return handle.CreateDescriptorPool(in pCreateInfo, device.VkAllocator);
	}
}
