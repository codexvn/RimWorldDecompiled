using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using Brutal;
using Brutal.Logging;
using Brutal.Logging.Internal;
using Brutal.VulkanApi;
using Brutal.VulkanApi.Abstractions;
using Waylith.Graphics;
using Waylith.Profiling;
using Waylith.Rendering.Compute;

namespace Waylith.Rendering;

public static class Mipmapper
{
	public readonly record struct MipmapRequest(GpuTextureAllocation Texture, int Width, int Height, int MipCount, int MinMipDimension, bool PreserveAlphaCoverage = false, float AlphaCoverageCutoff = 0.5f, bool Premultiplied = false);

	private struct MipmapPushConstants
	{
		public uint ReadFromImage;

		public uint SrcByteOffset;

		public uint DstByteOffset;

		public uint SrcWidth;

		public uint SrcHeight;

		public uint DstWidth;

		public uint DstHeight;

		public uint Premultiplied;
	}

	private struct HistogramPushConstants
	{
		public uint ReadFromImage;

		public uint SrcByteOffset;

		public uint Width;

		public uint Height;

		public uint HistUintOffset;
	}

	private struct SolvePushConstants
	{
		public uint BaseWidth;

		public uint BaseHeight;

		public uint MipCount;

		public float Cutoff;
	}

	private struct ApplyPushConstants
	{
		public uint ByteOffset;

		public uint Width;

		public uint Height;

		public uint MipIndex;
	}

	private sealed class ComputeMipPlan
	{
		public ComputeKernel DownsampleKernel { get; init; }

		public GpuComputeBuffer Ssbo { get; init; }

		public int[] MipWidth { get; init; }

		public int[] MipHeight { get; init; }

		public long[] ByteOffset { get; init; }

		public int GeneratedMipCount { get; init; }

		public bool Premultiplied { get; init; }

		public int BaseWidth { get; init; }

		public int BaseHeight { get; init; }

		public bool PreserveCoverage { get; set; }

		public float AlphaCutoff { get; set; }

		public ComputeKernel? HistogramKernel { get; set; }

		public ComputeKernel? SolveKernel { get; set; }

		public ComputeKernel? ApplyKernel { get; set; }

		public GpuComputeBuffer? Hist { get; set; }

		public GpuComputeBuffer? Scales { get; set; }

		public void DisposeBuffers()
		{
			Ssbo.Dispose();
			Hist?.Dispose();
			Scales?.Dispose();
		}

		public void DisposeBuffersNow()
		{
			Ssbo.DisposeNow();
			Hist?.DisposeNow();
			Scales?.DisposeNow();
		}
	}

	private readonly record struct SubmittedMipmapCommands(DeviceEx Device, VkCommandPool CommandPool, CommandBuffer CommandBuffer, VkFence Fence, ComputeMipPlan? Plan);

	private const int MaxComputeMips = 16;

	private const int AlphaHistogramBins = 256;

	private const int DownsamplePushConstantSize = 32;

	private const int HistogramPushConstantSize = 20;

	private const int SolvePushConstantSize = 16;

	private const int ApplyPushConstantSize = 16;

	private const string DownsampleKernelPath = "Compute/MipmapDownsample.comp.hlsl";

	private const string HistogramKernelPath = "Compute/MipmapAlphaHistogram.comp.hlsl";

	private const string SolveKernelPath = "Compute/MipmapSolveCoverage.comp.hlsl";

	private const string ApplyKernelPath = "Compute/MipmapApplyAlphaScale.comp.hlsl";

	private static readonly ComputeBinding[] DownsampleBindings = new ComputeBinding[2]
	{
		new ComputeBinding(0, ComputeBindingType.SampledImage),
		new ComputeBinding(1, ComputeBindingType.StorageBuffer)
	};

	private static readonly ComputeBinding[] HistogramBindings = new ComputeBinding[3]
	{
		new ComputeBinding(0, ComputeBindingType.SampledImage),
		new ComputeBinding(1, ComputeBindingType.StorageBuffer),
		new ComputeBinding(2, ComputeBindingType.StorageBuffer)
	};

	private static readonly ComputeBinding[] SolveBindings = new ComputeBinding[2]
	{
		new ComputeBinding(0, ComputeBindingType.StorageBuffer),
		new ComputeBinding(1, ComputeBindingType.StorageBuffer)
	};

	private static readonly ComputeBinding[] ApplyBindings = new ComputeBinding[2]
	{
		new ComputeBinding(0, ComputeBindingType.StorageBuffer),
		new ComputeBinding(1, ComputeBindingType.StorageBuffer)
	};

	private static readonly Lock _sync = new Lock();

	private static readonly SemaphoreSlim _mipmapCommandGate = new SemaphoreSlim(1, 1);

	private static GraphicsDevice? _graphicsDevice;

	private static DeviceEx? _device;

	private static VkCommandPool _commandPool;

	private static VkDescriptorPool _descriptorPool;

	private static bool _computeMipmapsDisabled;

	private static bool _coveragePreservationDisabled;

	private const int MaxTexturesPerBatch = 64;

	private const long MaxScratchBytesPerBatch = 268435456L;

	internal static void Configure(GraphicsDevice graphicsDevice)
	{
		ArgumentNullException.ThrowIfNull(graphicsDevice, "graphicsDevice");
		using (_sync.EnterScope())
		{
			DeviceEx device = graphicsDevice.Device;
			if (_device != device && _device != null)
			{
				if (!_commandPool.Equals(default(VkCommandPool)))
				{
					_device.DestroyCommandPool(_commandPool, _device.VkAllocator);
					_commandPool = default(VkCommandPool);
				}
				if (!_descriptorPool.IsNull())
				{
					_device.DestroyDescriptorPool(_descriptorPool, _device.VkAllocator);
					_descriptorPool = default(VkDescriptorPool);
				}
			}
			_graphicsDevice = graphicsDevice;
			_device = device;
			if (_commandPool.Equals(default(VkCommandPool)))
			{
				VkCommandPoolCreateInfo pCreateInfo = new VkCommandPoolCreateInfo
				{
					QueueFamilyIndex = graphicsDevice.GraphicsFamilyIndex,
					Flags = VkCommandPoolCreateFlags.ResetCommandBufferBit
				};
				_commandPool = device.CreateCommandPool(in pCreateInfo, device.VkAllocator);
			}
			if (_descriptorPool.IsNull())
			{
				_descriptorPool = CreateDescriptorPool(device);
			}
		}
	}

	internal static void Shutdown()
	{
		using (_sync.EnterScope())
		{
			if (_device != null)
			{
				if (!_commandPool.Equals(default(VkCommandPool)))
				{
					_device.DestroyCommandPool(_commandPool, _device.VkAllocator);
					_commandPool = default(VkCommandPool);
				}
				if (!_descriptorPool.IsNull())
				{
					_device.DestroyDescriptorPool(_descriptorPool, _device.VkAllocator);
					_descriptorPool = default(VkDescriptorPool);
				}
			}
			_graphicsDevice = null;
			_device = null;
		}
	}

	public static void GenerateMipmaps(GpuTextureAllocation texture, int width, int height, int mipCount, int minMipDimension, bool preserveAlphaCoverage = false, float alphaCoverageCutoff = 0.5f, bool premultiplied = false)
	{
		if (texture == null || texture.IsDisposed || texture.Image.IsNull())
		{
			return;
		}
		GpuTextureService.FlushPendingTextureCommands();
		int num = CalculateGeneratedMipCount(width, height, System.Math.Min(mipCount, texture.MipCount), minMipDimension);
		if (num <= 0 || IsDepthOrStencilFormat(texture.Format))
		{
			return;
		}
		_mipmapCommandGate.Wait();
		try
		{
			if (!TrySubmitGenerateMipmaps(texture, width, height, num, preserveAlphaCoverage, alphaCoverageCutoff, premultiplied, out var submitted))
			{
				return;
			}
			try
			{
				submitted.Device.WaitForFence(submitted.Fence, -1);
			}
			finally
			{
				ReleaseSubmittedMipmapCommands(submitted);
			}
		}
		finally
		{
			_mipmapCommandGate.Release();
		}
	}

	public static void GenerateMipmaps(IReadOnlyList<MipmapRequest> requests)
	{
		GpuTextureService.FlushPendingTextureCommands();
		for (int num = 0; num < requests.Count; num = SubmitBatch(requests, num))
		{
		}
	}

	private static int SubmitBatch(IReadOnlyList<MipmapRequest> requests, int first)
	{
		List<ComputeMipPlan> list = new List<ComputeMipPlan>();
		int i = first;
		DeviceEx deviceEx = null;
		CommandBuffer commandBuffer = default(CommandBuffer);
		VkFence vkFence = default(VkFence);
		VkDescriptorPool descriptorPool = default(VkDescriptorPool);
		bool flag = false;
		bool flag2 = false;
		_mipmapCommandGate.Wait();
		try
		{
			using (_sync.EnterScope())
			{
				if (_device == null || _graphicsDevice == null)
				{
					return requests.Count;
				}
				deviceEx = _device;
				descriptorPool = CreateDescriptorPool(deviceEx, 64);
				commandBuffer = deviceEx.AllocateCommandBuffer(new VkCommandBufferAllocateInfo
				{
					CommandPool = _commandPool,
					Level = VkCommandBufferLevel.Primary
				});
				commandBuffer.Begin(VkCommandBufferUsageFlags.OneTimeSubmitBit);
				int num = 0;
				long num2 = 0L;
				for (; i < requests.Count; i++)
				{
					if (num >= 64)
					{
						break;
					}
					if (num2 >= 268435456)
					{
						break;
					}
					MipmapRequest mipmapRequest = requests[i];
					GpuTextureAllocation texture = mipmapRequest.Texture;
					if (texture == null || texture.IsDisposed || texture.Image.IsNull())
					{
						continue;
					}
					int num3 = CalculateGeneratedMipCount(mipmapRequest.Width, mipmapRequest.Height, System.Math.Min(mipmapRequest.MipCount, texture.MipCount), mipmapRequest.MinMipDimension);
					if (num3 > 0 && !IsDepthOrStencilFormat(texture.Format))
					{
						ComputeMipPlan computeMipPlan = TryPrepareComputeMipmaps(texture, mipmapRequest.Width, mipmapRequest.Height, num3, mipmapRequest.PreserveAlphaCoverage, mipmapRequest.AlphaCoverageCutoff, mipmapRequest.Premultiplied);
						if (computeMipPlan != null)
						{
							list.Add(computeMipPlan);
							num2 += (long)computeMipPlan.Ssbo.Size.Bytes;
							RecordGenerateMipmapsCompute(commandBuffer, texture, computeMipPlan, descriptorPool);
						}
						else
						{
							RecordGenerateMipmaps(commandBuffer, texture, mipmapRequest.Width, mipmapRequest.Height, num3);
						}
						num++;
					}
				}
				commandBuffer.End();
				if (num == 0)
				{
					return i;
				}
				vkFence = deviceEx.CreateFence(new VkFenceCreateInfo(), deviceEx.VkAllocator);
				GraphicsDevice? graphicsDevice = _graphicsDevice;
				CommandBuffer reference = commandBuffer;
				graphicsDevice.SubmitGraphics(default(Span<VkSemaphore>), default(Span<VkPipelineStageFlags>), new Span<CommandBuffer>(ref reference), default(Span<VkSemaphore>), vkFence);
				flag = true;
			}
			using (FrameProfiler.Scope("MipmapGpuWait"))
			{
				deviceEx.WaitForFence(vkFence, -1);
			}
			flag2 = true;
			return i;
		}
		finally
		{
			if (flag && !flag2)
			{
				deviceEx.WaitForFence(vkFence, -1);
			}
			using (_sync.EnterScope())
			{
				if (deviceEx != null)
				{
					if (!commandBuffer.Handle.IsNull())
					{
						DeviceEx handle = deviceEx;
						VkCommandPool commandPool = _commandPool;
						CommandBuffer reference2 = commandBuffer;
						handle.FreeCommandBuffers(commandPool, new ReadOnlySpan<CommandBuffer>(in reference2));
					}
					if (!vkFence.IsNull())
					{
						deviceEx.DestroyFence(vkFence, deviceEx.VkAllocator);
					}
					if (!descriptorPool.IsNull())
					{
						deviceEx.DestroyDescriptorPool(descriptorPool, deviceEx.VkAllocator);
					}
				}
				foreach (ComputeMipPlan item in list)
				{
					item.DisposeBuffersNow();
				}
			}
			_mipmapCommandGate.Release();
		}
	}

	public static ValueTask GenerateMipmapsAsync(GpuTextureAllocation texture, int width, int height, int mipCount, int minMipDimension, bool preserveAlphaCoverage = false, float alphaCoverageCutoff = 0.5f, CancellationToken cancellationToken = default(CancellationToken))
	{
		if (texture == null || texture.IsDisposed || texture.Image.IsNull())
		{
			return ValueTask.CompletedTask;
		}
		GpuTextureService.FlushPendingTextureCommands();
		int num = CalculateGeneratedMipCount(width, height, System.Math.Min(mipCount, texture.MipCount), minMipDimension);
		if (num <= 0 || IsDepthOrStencilFormat(texture.Format))
		{
			return ValueTask.CompletedTask;
		}
		return new ValueTask(GenerateMipmapsAsyncCore(texture, width, height, num, preserveAlphaCoverage, alphaCoverageCutoff, cancellationToken));
	}

	private static async Task GenerateMipmapsAsyncCore(GpuTextureAllocation texture, int width, int height, int generatedMipCount, bool preserveAlphaCoverage, float alphaCoverageCutoff, CancellationToken cancellationToken)
	{
		await _mipmapCommandGate.WaitAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		try
		{
			cancellationToken.ThrowIfCancellationRequested();
			if (TrySubmitGenerateMipmaps(texture, width, height, generatedMipCount, preserveAlphaCoverage, alphaCoverageCutoff, premultiplied: false, out var submitted))
			{
				try
				{
					await WaitForFenceAsync(submitted).ConfigureAwait(continueOnCapturedContext: false);
				}
				finally
				{
					ReleaseSubmittedMipmapCommands(submitted);
				}
			}
		}
		finally
		{
			_mipmapCommandGate.Release();
		}
	}

	private static bool TrySubmitGenerateMipmaps(GpuTextureAllocation texture, int width, int height, int generatedMipCount, bool preserveAlphaCoverage, float alphaCoverageCutoff, bool premultiplied, out SubmittedMipmapCommands submitted)
	{
		submitted = default(SubmittedMipmapCommands);
		using (_sync.EnterScope())
		{
			if (_device == null || _graphicsDevice == null)
			{
				return false;
			}
			DeviceEx device = _device;
			GraphicsDevice graphicsDevice = _graphicsDevice;
			VkCommandPool commandPool = _commandPool;
			CommandBuffer commandBuffer = default(CommandBuffer);
			VkFence fence = default(VkFence);
			ComputeMipPlan computeMipPlan = TryPrepareComputeMipmaps(texture, width, height, generatedMipCount, preserveAlphaCoverage, alphaCoverageCutoff, premultiplied);
			try
			{
				commandBuffer = device.AllocateCommandBuffer(new VkCommandBufferAllocateInfo
				{
					CommandPool = commandPool,
					Level = VkCommandBufferLevel.Primary
				});
				fence = device.CreateFence(new VkFenceCreateInfo(), device.VkAllocator);
				commandBuffer.Begin(VkCommandBufferUsageFlags.OneTimeSubmitBit);
				if (computeMipPlan != null)
				{
					device.ResetDescriptorPool(_descriptorPool, 0);
					RecordGenerateMipmapsCompute(commandBuffer, texture, computeMipPlan, _descriptorPool);
				}
				else
				{
					RecordGenerateMipmaps(commandBuffer, texture, width, height, generatedMipCount);
				}
				commandBuffer.End();
				CommandBuffer reference = commandBuffer;
				graphicsDevice.SubmitGraphics(default(Span<VkSemaphore>), default(Span<VkPipelineStageFlags>), new Span<CommandBuffer>(ref reference), default(Span<VkSemaphore>), fence);
				submitted = new SubmittedMipmapCommands(device, commandPool, commandBuffer, fence, computeMipPlan);
				return true;
			}
			catch
			{
				computeMipPlan?.DisposeBuffers();
				if (!commandBuffer.Handle.IsNull())
				{
					CommandBuffer reference2 = commandBuffer;
					device.FreeCommandBuffers(commandPool, new ReadOnlySpan<CommandBuffer>(in reference2));
				}
				if (!fence.IsNull())
				{
					device.DestroyFence(fence, device.VkAllocator);
				}
				throw;
			}
		}
	}

	private static async Task WaitForFenceAsync(SubmittedMipmapCommands submitted)
	{
		while (true)
		{
			VkResult fenceStatus = submitted.Device.GetFenceStatus(submitted.Fence);
			switch (fenceStatus)
			{
			case VkResult.Success:
				return;
			case VkResult.NotReady:
				await Task.Delay(1).ConfigureAwait(continueOnCapturedContext: false);
				continue;
			}
			throw new InvalidOperationException($"Unexpected mipmap fence status {fenceStatus}.");
		}
	}

	private static void ReleaseSubmittedMipmapCommands(SubmittedMipmapCommands submitted)
	{
		using (_sync.EnterScope())
		{
			if (!submitted.CommandBuffer.Handle.IsNull())
			{
				DeviceEx device = submitted.Device;
				VkCommandPool commandPool = submitted.CommandPool;
				CommandBuffer reference = submitted.CommandBuffer;
				device.FreeCommandBuffers(commandPool, new ReadOnlySpan<CommandBuffer>(in reference));
			}
			if (!submitted.Fence.IsNull())
			{
				submitted.Device.DestroyFence(submitted.Fence, submitted.Device.VkAllocator);
			}
			submitted.Plan?.DisposeBuffersNow();
		}
	}

	private static void TransitionSubresourceLayout(CommandBuffer commandBuffer, GpuTextureAllocation texture, int mipLevel, VkImageLayout newLayout, VkAccessFlags sourceAccessMask, VkAccessFlags destinationAccessMask, VkPipelineStageFlags sourceStageMask, VkPipelineStageFlags destinationStageMask)
	{
		if (texture.GetMipLayout(mipLevel) != newLayout || sourceAccessMask != destinationAccessMask || sourceStageMask != destinationStageMask)
		{
			VkImageMemoryBarrier reference = new VkImageMemoryBarrier
			{
				SrcAccessMask = sourceAccessMask,
				DstAccessMask = destinationAccessMask,
				OldLayout = texture.GetMipLayout(mipLevel),
				NewLayout = newLayout,
				SrcQueueFamilyIndex = -1,
				DstQueueFamilyIndex = -1,
				Image = texture.Image,
				SubresourceRange = new VkImageSubresourceRange
				{
					AspectMask = VkImageAspectFlags.ColorBit,
					BaseMipLevel = mipLevel,
					LevelCount = 1,
					BaseArrayLayer = 0,
					LayerCount = 1
				}
			};
			commandBuffer.PipelineBarrier(sourceStageMask, destinationStageMask, VkDependencyFlags.None, default(ReadOnlySpan<VkMemoryBarrier>), default(ReadOnlySpan<VkBufferMemoryBarrier>), new ReadOnlySpan<VkImageMemoryBarrier>(in reference));
			texture.SetMipLayout(mipLevel, newLayout);
		}
	}

	private static int CalculateGeneratedMipCount(int width, int height, int mipCount, int minMipDimension)
	{
		int num = System.Math.Max(1, mipCount);
		if (num <= 1)
		{
			return 0;
		}
		int num2 = System.Math.Max(System.Math.Max(1, width), System.Math.Max(1, height));
		int num3 = System.Math.Max(1, minMipDimension);
		uint num4 = (uint)(num2 / num3);
		return System.Math.Clamp((num4 > 1) ? BitOperations.Log2(num4) : 0, 0, num - 1);
	}

	private static bool IsDepthOrStencilFormat(VkFormat format)
	{
		switch (format)
		{
		case VkFormat.D16UNorm:
		case VkFormat.D32SFloat:
		case VkFormat.D16UNormS8UInt:
		case VkFormat.D24UNormS8UInt:
		case VkFormat.D32SFloatS8UInt:
			return true;
		default:
			return false;
		}
	}

	private static void RecordGenerateMipmaps(CommandBuffer commandBuffer, GpuTextureAllocation texture, int width, int height, int generatedMipCount)
	{
		if (generatedMipCount <= 0)
		{
			return;
		}
		TransitionSubresourceLayout(commandBuffer, texture, 0, VkImageLayout.TransferSrcOptimal, VkAccessFlags.ShaderReadBit, VkAccessFlags.TransferReadBit, VkPipelineStageFlags.FragmentShaderBit, VkPipelineStageFlags.TransferBit);
		int num = System.Math.Max(1, width);
		int num2 = System.Math.Max(1, height);
		for (int i = 1; i <= generatedMipCount; i++)
		{
			if (i > 1)
			{
				TransitionSubresourceLayout(commandBuffer, texture, i - 1, VkImageLayout.TransferSrcOptimal, VkAccessFlags.TransferWriteBit, VkAccessFlags.TransferReadBit, VkPipelineStageFlags.TransferBit, VkPipelineStageFlags.TransferBit);
			}
			TransitionSubresourceLayout(commandBuffer, texture, i, VkImageLayout.TransferDstOptimal, VkAccessFlags.None, VkAccessFlags.TransferWriteBit, VkPipelineStageFlags.TopOfPipeBit, VkPipelineStageFlags.TransferBit);
			int num3 = System.Math.Max(1, num / 2);
			int num4 = System.Math.Max(1, num2 / 2);
			VkImageBlit vkImageBlit = new VkImageBlit();
			vkImageBlit.SrcSubresource = new VkImageSubresourceLayers
			{
				AspectMask = VkImageAspectFlags.ColorBit,
				MipLevel = i - 1,
				BaseArrayLayer = 0,
				LayerCount = 1
			};
			vkImageBlit.DstSubresource = new VkImageSubresourceLayers
			{
				AspectMask = VkImageAspectFlags.ColorBit,
				MipLevel = i,
				BaseArrayLayer = 0,
				LayerCount = 1
			};
			VkImageBlit vkImageBlit2 = vkImageBlit;
			global::_003CPrivateImplementationDetails_003E.InlineArrayFirstElementRef<VkOffset3D_2, VkOffset3D>(ref vkImageBlit2.SrcOffsets) = new VkOffset3D(0, 0, 0);
			global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<VkOffset3D_2, VkOffset3D>(ref vkImageBlit2.SrcOffsets, 1) = new VkOffset3D(num, num2, 1);
			global::_003CPrivateImplementationDetails_003E.InlineArrayFirstElementRef<VkOffset3D_2, VkOffset3D>(ref vkImageBlit2.DstOffsets) = new VkOffset3D(0, 0, 0);
			global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<VkOffset3D_2, VkOffset3D>(ref vkImageBlit2.DstOffsets, 1) = new VkOffset3D(num3, num4, 1);
			VkImage image = texture.Image;
			VkImage image2 = texture.Image;
			VkImageBlit reference = vkImageBlit2;
			commandBuffer.BlitImage(image, VkImageLayout.TransferSrcOptimal, image2, VkImageLayout.TransferDstOptimal, new ReadOnlySpan<VkImageBlit>(in reference), VkFilter.Linear);
			num = num3;
			num2 = num4;
		}
		for (int j = 0; j <= generatedMipCount; j++)
		{
			VkAccessFlags sourceAccessMask = ((j == generatedMipCount) ? VkAccessFlags.TransferWriteBit : VkAccessFlags.TransferReadBit);
			TransitionSubresourceLayout(commandBuffer, texture, j, VkImageLayout.ShaderReadOnlyOptimal, sourceAccessMask, VkAccessFlags.ShaderReadBit, VkPipelineStageFlags.TransferBit, VkPipelineStageFlags.FragmentShaderBit);
		}
	}

	private static bool SupportsComputeMipmaps(VkFormat format)
	{
		return format == VkFormat.R8G8B8A8UNorm;
	}

	private static ComputeMipPlan? TryPrepareComputeMipmaps(GpuTextureAllocation texture, int width, int height, int generatedMipCount, bool preserveAlphaCoverage, float alphaCoverageCutoff, bool premultiplied)
	{
		if (_computeMipmapsDisabled || _device == null || _graphicsDevice == null || _descriptorPool.IsNull() || generatedMipCount < 1 || generatedMipCount > 16 || !SupportsComputeMipmaps(texture.Format))
		{
			return null;
		}
		ComputeKernel downsampleKernel;
		bool shouldAppend;
		try
		{
			downsampleKernel = ComputeKernelCache.Load("Compute/MipmapDownsample.comp.hlsl", "main", DownsampleBindings, 32, ShaderSourceLanguage.Hlsl);
		}
		catch (Exception ex)
		{
			_computeMipmapsDisabled = true;
			LogCategory log = RenderLog.Log;
			LogCategory logCategory = log;
			LogString<Error> message = new LogString<Error>(72, 1, log, out shouldAppend);
			if (shouldAppend)
			{
				message.AppendLiteral("Mipmapper: compute downsample kernel unavailable, falling back to blit: ");
				message.AppendFormatted(ex.Message);
			}
			logCategory.Error(message, "TryPrepareComputeMipmaps", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Rendering\\Mipmapper.cs", 652);
			return null;
		}
		int[] array = new int[generatedMipCount + 1];
		int[] array2 = new int[generatedMipCount + 1];
		long[] array3 = new long[generatedMipCount + 1];
		long num = 0L;
		for (int i = 0; i <= generatedMipCount; i++)
		{
			array[i] = System.Math.Max(1, width >> i);
			array2[i] = System.Math.Max(1, height >> i);
			if (i >= 1)
			{
				array3[i] = num * 4;
				num += (long)array[i] * (long)array2[i];
			}
		}
		GpuComputeBuffer ssbo;
		try
		{
			ssbo = GpuComputeBuffer.Create(_device, _graphicsDevice.Allocator, "Mipmapper.Downsample", num * 4, VkBufferUsageFlags.TransferSrcBit, deviceLocal: true);
		}
		catch (Exception ex2)
		{
			LogCategory log = RenderLog.Log;
			LogCategory logCategory2 = log;
			LogString<Warning> message2 = new LogString<Warning>(71, 1, log, out shouldAppend);
			if (shouldAppend)
			{
				message2.AppendLiteral("Mipmapper: failed to allocate downsample buffer, falling back to blit: ");
				message2.AppendFormatted(ex2.Message);
			}
			logCategory2.Warning(message2, "TryPrepareComputeMipmaps", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Rendering\\Mipmapper.cs", 687);
			return null;
		}
		ComputeMipPlan computeMipPlan = new ComputeMipPlan
		{
			DownsampleKernel = downsampleKernel,
			Premultiplied = premultiplied,
			Ssbo = ssbo,
			MipWidth = array,
			MipHeight = array2,
			ByteOffset = array3,
			GeneratedMipCount = generatedMipCount,
			BaseWidth = System.Math.Max(1, width),
			BaseHeight = System.Math.Max(1, height)
		};
		if (preserveAlphaCoverage)
		{
			TryAddCoveragePreservation(computeMipPlan, System.Math.Clamp(alphaCoverageCutoff, 0f, 1f));
		}
		return computeMipPlan;
	}

	private static void TryAddCoveragePreservation(ComputeMipPlan plan, float cutoff)
	{
		if (_coveragePreservationDisabled || _device == null || _graphicsDevice == null)
		{
			return;
		}
		GpuComputeBuffer gpuComputeBuffer = null;
		GpuComputeBuffer gpuComputeBuffer2 = null;
		try
		{
			plan.HistogramKernel = ComputeKernelCache.Load("Compute/MipmapAlphaHistogram.comp.hlsl", "main", HistogramBindings, 20, ShaderSourceLanguage.Hlsl);
			plan.SolveKernel = ComputeKernelCache.Load("Compute/MipmapSolveCoverage.comp.hlsl", "main", SolveBindings, 16, ShaderSourceLanguage.Hlsl);
			plan.ApplyKernel = ComputeKernelCache.Load("Compute/MipmapApplyAlphaScale.comp.hlsl", "main", ApplyBindings, 16, ShaderSourceLanguage.Hlsl);
			long num = (long)(plan.GeneratedMipCount + 1) * 256L;
			gpuComputeBuffer = GpuComputeBuffer.Create(_device, _graphicsDevice.Allocator, "Mipmapper.AlphaHistogram", num * 4);
			gpuComputeBuffer.Upload<uint>(new uint[num], 0L);
			gpuComputeBuffer2 = GpuComputeBuffer.Create(_device, _graphicsDevice.Allocator, "Mipmapper.AlphaScales", (long)(plan.GeneratedMipCount + 1) * 4L);
			plan.Hist = gpuComputeBuffer;
			plan.Scales = gpuComputeBuffer2;
			plan.AlphaCutoff = cutoff;
			plan.PreserveCoverage = true;
		}
		catch (Exception ex)
		{
			_coveragePreservationDisabled = true;
			gpuComputeBuffer?.Dispose();
			gpuComputeBuffer2?.Dispose();
			plan.HistogramKernel = null;
			plan.SolveKernel = null;
			plan.ApplyKernel = null;
			plan.Hist = null;
			plan.Scales = null;
			plan.PreserveCoverage = false;
			LogCategory log = RenderLog.Log;
			bool shouldAppend;
			LogString<Error> message = new LogString<Error>(80, 1, log, out shouldAppend);
			if (shouldAppend)
			{
				message.AppendLiteral("Mipmapper: alpha-coverage preservation unavailable, generating mips without it: ");
				message.AppendFormatted(ex.Message);
			}
			log.Error(message, "TryAddCoveragePreservation", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Rendering\\Mipmapper.cs", 749);
		}
	}

	private static void RecordGenerateMipmapsCompute(CommandBuffer commandBuffer, GpuTextureAllocation texture, ComputeMipPlan plan, VkDescriptorPool descriptorPool)
	{
		DeviceEx device = _device;
		GpuComputeBuffer ssbo = plan.Ssbo;
		int generatedMipCount = plan.GeneratedMipCount;
		if (texture.GetMipLayout(0) == VkImageLayout.ShaderReadOnlyOptimal)
		{
			VkMemoryBarrier reference = new VkMemoryBarrier
			{
				SrcAccessMask = (VkAccessFlags.ShaderWriteBit | VkAccessFlags.ColorAttachmentWriteBit | VkAccessFlags.TransferWriteBit),
				DstAccessMask = VkAccessFlags.ShaderReadBit
			};
			commandBuffer.PipelineBarrier(VkPipelineStageFlags.AllCommandsBit, VkPipelineStageFlags.ComputeShaderBit, VkDependencyFlags.None, new ReadOnlySpan<VkMemoryBarrier>(in reference), default(ReadOnlySpan<VkBufferMemoryBarrier>), default(ReadOnlySpan<VkImageMemoryBarrier>));
		}
		else
		{
			TransitionSubresourceLayout(commandBuffer, texture, 0, VkImageLayout.ShaderReadOnlyOptimal, VkAccessFlags.ShaderWriteBit | VkAccessFlags.ColorAttachmentWriteBit | VkAccessFlags.TransferWriteBit, VkAccessFlags.ShaderReadBit, VkPipelineStageFlags.AllCommandsBit, VkPipelineStageFlags.ComputeShaderBit);
		}
		for (int i = 1; i <= generatedMipCount; i++)
		{
			ComputeDispatch computeDispatch = ComputeDispatch.Rent(plan.DownsampleKernel, (plan.MipWidth[i] + 7) / 8, (plan.MipHeight[i] + 7) / 8);
			computeDispatch.SetSampledImage(0, texture);
			computeDispatch.SetStorageBuffer(1, ssbo);
			computeDispatch.SetPushConstants<MipmapPushConstants>(new MipmapPushConstants
			{
				ReadFromImage = ((i == 1) ? 1u : 0u),
				SrcByteOffset = (uint)((i != 1) ? plan.ByteOffset[i - 1] : 0u),
				DstByteOffset = (uint)plan.ByteOffset[i],
				SrcWidth = (uint)plan.MipWidth[i - 1],
				SrcHeight = (uint)plan.MipHeight[i - 1],
				DstWidth = (uint)plan.MipWidth[i],
				DstHeight = (uint)plan.MipHeight[i],
				Premultiplied = (plan.Premultiplied ? 1u : 0u)
			});
			ComputeDispatchQueue.RecordDispatch(commandBuffer, device, descriptorPool, computeDispatch);
			ComputeDispatch.Return(computeDispatch);
			bool flag = i != generatedMipCount || plan.PreserveCoverage;
			BufferBarrier(commandBuffer, ssbo, VkAccessFlags.ShaderWriteBit, flag ? VkAccessFlags.ShaderReadBit : VkAccessFlags.TransferReadBit, VkPipelineStageFlags.ComputeShaderBit, flag ? VkPipelineStageFlags.ComputeShaderBit : VkPipelineStageFlags.TransferBit);
		}
		if (plan.PreserveCoverage)
		{
			RecordCoveragePreservation(commandBuffer, texture, plan, descriptorPool);
		}
		Span<VkBufferImageCopy> span = stackalloc VkBufferImageCopy[generatedMipCount];
		for (int j = 1; j <= generatedMipCount; j++)
		{
			TransitionSubresourceLayout(commandBuffer, texture, j, VkImageLayout.TransferDstOptimal, VkAccessFlags.None, VkAccessFlags.TransferWriteBit, VkPipelineStageFlags.TopOfPipeBit, VkPipelineStageFlags.TransferBit);
			span[j - 1] = new VkBufferImageCopy
			{
				BufferOffset = ByteSize.Of(plan.ByteOffset[j]),
				BufferRowLength = 0,
				BufferImageHeight = 0,
				ImageSubresource = new VkImageSubresourceLayers
				{
					AspectMask = VkImageAspectFlags.ColorBit,
					MipLevel = j,
					BaseArrayLayer = 0,
					LayerCount = 1
				},
				ImageOffset = new VkOffset3D(0, 0, 0),
				ImageExtent = new VkExtent3D(plan.MipWidth[j], plan.MipHeight[j], 1)
			};
		}
		commandBuffer.CopyBufferToImage(ssbo.Buffer, texture.Image, VkImageLayout.TransferDstOptimal, span);
		for (int k = 1; k <= generatedMipCount; k++)
		{
			TransitionSubresourceLayout(commandBuffer, texture, k, VkImageLayout.ShaderReadOnlyOptimal, VkAccessFlags.TransferWriteBit, VkAccessFlags.ShaderReadBit, VkPipelineStageFlags.TransferBit, VkPipelineStageFlags.FragmentShaderBit);
		}
	}

	private static void RecordCoveragePreservation(CommandBuffer commandBuffer, GpuTextureAllocation texture, ComputeMipPlan plan, VkDescriptorPool descriptorPool)
	{
		DeviceEx device = _device;
		GpuComputeBuffer ssbo = plan.Ssbo;
		GpuComputeBuffer hist = plan.Hist;
		GpuComputeBuffer scales = plan.Scales;
		int generatedMipCount = plan.GeneratedMipCount;
		for (int i = 0; i <= generatedMipCount; i++)
		{
			bool flag = i == 0;
			ComputeDispatch computeDispatch = ComputeDispatch.Rent(plan.HistogramKernel, (plan.MipWidth[i] + 7) / 8, (plan.MipHeight[i] + 7) / 8);
			computeDispatch.SetSampledImage(0, texture);
			computeDispatch.SetStorageBuffer(1, ssbo);
			computeDispatch.SetStorageBuffer(2, hist);
			computeDispatch.SetPushConstants<HistogramPushConstants>(new HistogramPushConstants
			{
				ReadFromImage = (flag ? 1u : 0u),
				SrcByteOffset = (uint)((!flag) ? plan.ByteOffset[i] : 0u),
				Width = (uint)plan.MipWidth[i],
				Height = (uint)plan.MipHeight[i],
				HistUintOffset = (uint)(i * 256)
			});
			ComputeDispatchQueue.RecordDispatch(commandBuffer, device, descriptorPool, computeDispatch);
			ComputeDispatch.Return(computeDispatch);
		}
		BufferBarrier(commandBuffer, hist, VkAccessFlags.ShaderWriteBit, VkAccessFlags.ShaderReadBit, VkPipelineStageFlags.ComputeShaderBit, VkPipelineStageFlags.ComputeShaderBit);
		ComputeDispatch computeDispatch2 = ComputeDispatch.Rent(plan.SolveKernel, 1, 1);
		computeDispatch2.SetStorageBuffer(0, hist);
		computeDispatch2.SetStorageBuffer(1, scales);
		SolvePushConstants data = new SolvePushConstants
		{
			BaseWidth = (uint)plan.BaseWidth,
			BaseHeight = (uint)plan.BaseHeight,
			MipCount = (uint)generatedMipCount,
			Cutoff = plan.AlphaCutoff
		};
		computeDispatch2.SetPushConstants(in data);
		ComputeDispatchQueue.RecordDispatch(commandBuffer, device, descriptorPool, computeDispatch2);
		ComputeDispatch.Return(computeDispatch2);
		BufferBarrier(commandBuffer, scales, VkAccessFlags.ShaderWriteBit, VkAccessFlags.ShaderReadBit, VkPipelineStageFlags.ComputeShaderBit, VkPipelineStageFlags.ComputeShaderBit);
		for (int j = 1; j <= generatedMipCount; j++)
		{
			ComputeDispatch computeDispatch3 = ComputeDispatch.Rent(plan.ApplyKernel, (plan.MipWidth[j] + 7) / 8, (plan.MipHeight[j] + 7) / 8);
			computeDispatch3.SetStorageBuffer(0, ssbo);
			computeDispatch3.SetStorageBuffer(1, scales);
			computeDispatch3.SetPushConstants<ApplyPushConstants>(new ApplyPushConstants
			{
				ByteOffset = (uint)plan.ByteOffset[j],
				Width = (uint)plan.MipWidth[j],
				Height = (uint)plan.MipHeight[j],
				MipIndex = (uint)j
			});
			ComputeDispatchQueue.RecordDispatch(commandBuffer, device, descriptorPool, computeDispatch3);
			ComputeDispatch.Return(computeDispatch3);
		}
		BufferBarrier(commandBuffer, ssbo, VkAccessFlags.ShaderWriteBit, VkAccessFlags.TransferReadBit, VkPipelineStageFlags.ComputeShaderBit, VkPipelineStageFlags.TransferBit);
	}

	private static void BufferBarrier(CommandBuffer commandBuffer, GpuComputeBuffer buffer, VkAccessFlags sourceAccessMask, VkAccessFlags destinationAccessMask, VkPipelineStageFlags sourceStageMask, VkPipelineStageFlags destinationStageMask)
	{
		ReadOnlySpan<VkMemoryBarrier> pMemoryBarriers = default(Span<VkMemoryBarrier>);
		VkBufferMemoryBarrier reference = new VkBufferMemoryBarrier
		{
			SrcAccessMask = sourceAccessMask,
			DstAccessMask = destinationAccessMask,
			SrcQueueFamilyIndex = -1,
			DstQueueFamilyIndex = -1,
			Buffer = buffer.Buffer,
			Offset = ByteSize.Zero,
			Size = buffer.Size
		};
		commandBuffer.PipelineBarrier(sourceStageMask, destinationStageMask, VkDependencyFlags.None, pMemoryBarriers, new ReadOnlySpan<VkBufferMemoryBarrier>(in reference), default(Span<VkImageMemoryBarrier>));
	}

	private unsafe static VkDescriptorPool CreateDescriptorPool(DeviceEx device, int textures = 1)
	{
		VkDescriptorPoolSize* ptr = stackalloc VkDescriptorPoolSize[2];
		*ptr = new VkDescriptorPoolSize
		{
			Type = VkDescriptorType.SampledImage,
			DescriptorCount = 48 * textures
		};
		ptr[1] = new VkDescriptorPoolSize
		{
			Type = VkDescriptorType.StorageBuffer,
			DescriptorCount = 96 * textures
		};
		VkDescriptorPoolCreateInfo pCreateInfo = new VkDescriptorPoolCreateInfo
		{
			MaxSets = 64 * textures,
			PoolSizeCount = 2,
			PoolSizes = ptr
		};
		return device.CreateDescriptorPool(in pCreateInfo, device.VkAllocator);
	}
}
