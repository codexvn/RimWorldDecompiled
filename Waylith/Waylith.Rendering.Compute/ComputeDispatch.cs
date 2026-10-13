using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Brutal.VulkanApi;

namespace Waylith.Rendering.Compute;

public class ComputeDispatch
{
	private const int MaxBindings = 16;

	private static readonly ConcurrentStack<ComputeDispatch> pool = new ConcurrentStack<ComputeDispatch>();

	private GpuTextureAllocation?[] sampledTextures = new GpuTextureAllocation[16];

	private VkImageView[] sampledViews = new VkImageView[16];

	private VkImageLayout[] sampledLayouts = new VkImageLayout[16];

	private VkSampler[] samplers = new VkSampler[16];

	private GpuTextureAllocation?[] storageImages = new GpuTextureAllocation[16];

	private GpuComputeBuffer?[] storageBuffers = new GpuComputeBuffer[16];

	private byte[] pushConstantData = new byte[128];

	private int pushConstantSize;

	public ComputeKernel Kernel { get; private set; }

	public int ThreadGroupsX { get; private set; }

	public int ThreadGroupsY { get; private set; }

	public int ThreadGroupsZ { get; private set; }

	internal object? Owner { get; set; }

	internal int Key { get; set; } = -1;

	public void SetSampledTexture(int binding, GpuTextureAllocation texture, VkSampler sampler, VkImageView view = default(VkImageView))
	{
		texture.MarkUsed();
		sampledTextures[binding] = texture;
		sampledViews[binding] = view;
		samplers[binding] = sampler;
	}

	public void SetSampledView(int binding, VkImageView view, VkSampler sampler, VkImageLayout layout = VkImageLayout.ShaderReadOnlyOptimal)
	{
		sampledViews[binding] = view;
		samplers[binding] = sampler;
		sampledLayouts[binding] = layout;
	}

	public void SetStorageImage(int binding, GpuTextureAllocation texture)
	{
		texture.MarkWritten();
		storageImages[binding] = texture;
	}

	public void SetSampledImage(int binding, GpuTextureAllocation texture)
	{
		texture.MarkUsed();
		sampledTextures[binding] = texture;
	}

	public void SetStorageBuffer(int binding, GpuComputeBuffer buffer)
	{
		storageBuffers[binding] = buffer;
	}

	public void SetPushConstants<T>(in T data) where T : unmanaged
	{
		pushConstantSize = Unsafe.SizeOf<T>();
		MemoryMarshal.Write(pushConstantData, in data);
	}

	internal VkImageView GetSampledView(int binding)
	{
		if (!sampledViews[binding].IsNull())
		{
			return sampledViews[binding];
		}
		return sampledTextures[binding]?.ImageView ?? default(VkImageView);
	}

	internal VkSampler GetSampler(int binding)
	{
		return samplers[binding];
	}

	internal VkImageLayout GetSampledLayout(int binding)
	{
		if (sampledLayouts[binding] != 0)
		{
			return sampledLayouts[binding];
		}
		return VkImageLayout.ShaderReadOnlyOptimal;
	}

	internal GpuTextureAllocation? GetStorageImage(int binding)
	{
		return storageImages[binding];
	}

	internal GpuComputeBuffer? GetStorageBuffer(int binding)
	{
		return storageBuffers[binding];
	}

	internal ReadOnlySpan<byte> GetPushConstantData()
	{
		return new ReadOnlySpan<byte>(pushConstantData, 0, pushConstantSize);
	}

	public static ComputeDispatch Rent(ComputeKernel kernel, int x, int y, int z = 1)
	{
		if (!pool.TryPop(out ComputeDispatch result))
		{
			result = new ComputeDispatch();
		}
		result.Kernel = kernel;
		result.ThreadGroupsX = x;
		result.ThreadGroupsY = y;
		result.ThreadGroupsZ = z;
		return result;
	}

	internal static void Return(ComputeDispatch dispatch)
	{
		Array.Clear(dispatch.sampledTextures);
		Array.Clear(dispatch.sampledViews);
		Array.Clear(dispatch.sampledLayouts);
		Array.Clear(dispatch.samplers);
		Array.Clear(dispatch.storageImages);
		Array.Clear(dispatch.storageBuffers);
		dispatch.pushConstantSize = 0;
		dispatch.Kernel = null;
		dispatch.Owner = null;
		dispatch.Key = -1;
		pool.Push(dispatch);
	}
}
