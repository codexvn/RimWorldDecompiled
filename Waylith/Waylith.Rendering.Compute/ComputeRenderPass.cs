using System;
using System.Collections.Generic;
using Brutal.VulkanApi;
using Brutal.VulkanApi.Abstractions;
using Waylith.Graphics;

namespace Waylith.Rendering.Compute;

internal sealed class ComputeRenderPass : IRenderPass, IDisposable
{
	private readonly DeviceEx device;

	private readonly VkDescriptorPool[] pools;

	private readonly int[] poolCapacities;

	private const int InitialPoolCapacity = 64;

	public byte PassId => 0;

	public PassAttachment? Attachment => null;

	public bool PreBackbuffer => true;

	internal ComputeRenderPass(DeviceEx device, int framesInFlight)
	{
		this.device = device;
		pools = new VkDescriptorPool[framesInFlight];
		poolCapacities = new int[framesInFlight];
		for (int i = 0; i < framesInFlight; i++)
		{
			pools[i] = CreateDescriptorPool(device, 64);
			poolCapacities[i] = 64;
		}
	}

	public void Record(in FrameContext ctx, object? featurePayload)
	{
		if (!(featurePayload is List<ComputeDispatch> { Count: not 0 } list))
		{
			return;
		}
		int submissionFrameIndex = ctx.SubmissionFrameIndex;
		if (list.Count > poolCapacities[submissionFrameIndex])
		{
			device.DestroyDescriptorPool(pools[submissionFrameIndex], device.VkAllocator);
			poolCapacities[submissionFrameIndex] = System.Math.Max(list.Count, poolCapacities[submissionFrameIndex] * 2);
			pools[submissionFrameIndex] = CreateDescriptorPool(device, poolCapacities[submissionFrameIndex]);
		}
		else
		{
			device.ResetDescriptorPool(pools[submissionFrameIndex], 0);
		}
		CommandBuffer commandBuffer = ctx.CommandBuffer;
		foreach (ComputeDispatch item in list)
		{
			ComputeDispatchQueue.RecordDispatch(commandBuffer, device, pools[submissionFrameIndex], item);
			ComputeDispatch.Return(item);
		}
	}

	public void OnRenderTargetRebuilt(IRenderTarget target)
	{
	}

	public void Dispose()
	{
		VkDescriptorPool[] array = pools;
		foreach (VkDescriptorPool descriptorPool in array)
		{
			device.DestroyDescriptorPool(descriptorPool, device.VkAllocator);
		}
	}

	private unsafe static VkDescriptorPool CreateDescriptorPool(DeviceEx device, int maxSets)
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
		VkDescriptorPoolCreateInfo pCreateInfo = new VkDescriptorPoolCreateInfo
		{
			MaxSets = maxSets,
			PoolSizeCount = 4,
			PoolSizes = ptr
		};
		return device.CreateDescriptorPool(in pCreateInfo, device.VkAllocator);
	}

	void IRenderPass.Record(in FrameContext ctx, object? featurePayload)
	{
		Record(in ctx, featurePayload);
	}
}
