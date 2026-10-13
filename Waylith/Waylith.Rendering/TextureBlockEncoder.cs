using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using Brutal;
using Brutal.Logging;
using Brutal.Logging.Internal;
using Brutal.VulkanApi;
using Brutal.VulkanApi.Abstractions;
using Brutal.VulkanApi.Vma;
using Waylith.Assets;
using Waylith.Graphics;
using Waylith.Profiling;
using Waylith.Rendering.Compute;

namespace Waylith.Rendering;

public static class TextureBlockEncoder
{
	internal sealed class BuildItem
	{
		public required GpuTextureAllocation Image;

		public required int Width;

		public required int Height;

		public required int MipCount;

		public required long StagingOffset;

		public required bool Encode;

		public bool Premultiplied;

		public byte[][]? Blocks;

		public byte[][]? MipLevels;

		internal long[] LevelOffsets = Array.Empty<long>();

		internal long[] OutOffsets = Array.Empty<long>();

		internal VkImageView Mip0View;
	}

	internal sealed class InFlightBuild
	{
		public required List<BuildItem> Items;

		public BcEncodeTarget EncodeTarget;

		public int BlockBytes;

		public GpuComputeBuffer? Mips;

		public GpuComputeBuffer? Guidance;

		public GpuComputeBuffer? Partials;

		public GpuComputeBuffer? MipsReadback;

		public GpuComputeBuffer? Output;

		public VkDescriptorPool DescriptorPool;

		public CommandBuffer CommandBuffer;

		public VkFence Fence;
	}

	private struct KaiserPushConstants
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

	internal sealed class PixelEncodeContext(VkCommandPool commandPool, VkDescriptorPool descriptorPool, VkFence fence)
	{
		public readonly VkCommandPool CommandPool = commandPool;

		public readonly VkDescriptorPool DescriptorPool = descriptorPool;

		public readonly VkFence Fence = fence;

		public GpuComputeBuffer? Staging;

		public GpuComputeBuffer.Mapping StagingMapping;

		public GpuComputeBuffer? Out;

		public GpuComputeBuffer.Mapping OutMapping;

		public GpuComputeBuffer? Levels;

		public void Destroy(DeviceEx device)
		{
			Release(ref Staging, StagingMapping);
			Release(ref Out, OutMapping);
			Levels?.DisposeNow();
			device.DestroyFence(fence, device.VkAllocator);
			device.DestroyDescriptorPool(descriptorPool, device.VkAllocator);
			device.DestroyCommandPool(commandPool, device.VkAllocator);
		}

		public unsafe static byte* Ensure(ref GpuComputeBuffer? buffer, ref GpuComputeBuffer.Mapping mapping, DeviceEx device, VmaAllocator allocator, string label, long bytes, bool readback)
		{
			if (buffer != null && (ulong)buffer.Size.Bytes >= (ulong)bytes)
			{
				return mapping.Pointer;
			}
			Release(ref buffer, mapping);
			long sizeBytes = (bytes + 4194304 - 1) / 4194304 * 4194304;
			buffer = GpuComputeBuffer.Create(device, allocator, label, sizeBytes, VkBufferUsageFlags.TransferSrcBit, deviceLocal: false, readback, !readback);
			mapping = buffer.Map();
			return mapping.Pointer;
		}

		public static GpuComputeBuffer EnsureDeviceLocal(ref GpuComputeBuffer? buffer, DeviceEx device, VmaAllocator allocator, string label, long bytes)
		{
			if (buffer != null && (ulong)buffer.Size.Bytes >= (ulong)bytes)
			{
				return buffer;
			}
			buffer?.DisposeNow();
			long sizeBytes = (bytes + 4194304 - 1) / 4194304 * 4194304;
			buffer = GpuComputeBuffer.Create(device, allocator, label, sizeBytes, VkBufferUsageFlags.TransferSrcBit, deviceLocal: true);
			return buffer;
		}

		private static void Release(ref GpuComputeBuffer? buffer, GpuComputeBuffer.Mapping mapping)
		{
			if (buffer != null)
			{
				mapping.Dispose();
				buffer.DisposeNow();
				buffer = null;
			}
		}
	}

	public sealed class PendingPixelEncode
	{
		internal DeviceEx Device { get; }

		internal PixelEncodeContext Context { get; }

		internal GpuTextureAllocation Source { get; }

		internal VkImageView Mip0View { get; }

		internal CommandBuffer CommandBuffer { get; }

		internal BcEncodeTarget Target { get; }

		internal unsafe byte* OutMapped { get; }

		internal int[] BlocksX { get; }

		internal int[] BlocksY { get; }

		internal int[] BlockOffset { get; }

		internal int BlockBytes { get; }

		internal unsafe PendingPixelEncode(DeviceEx device, PixelEncodeContext context, GpuTextureAllocation source, VkImageView mip0View, CommandBuffer commandBuffer, BcEncodeTarget target, byte* outMapped, int[] blocksX, int[] blocksY, int[] blockOffset, int blockBytes)
		{
			Device = device;
			Context = context;
			Source = source;
			Mip0View = mip0View;
			CommandBuffer = commandBuffer;
			Target = target;
			OutMapped = outMapped;
			BlocksX = blocksX;
			BlocksY = blocksY;
			BlockOffset = blockOffset;
			BlockBytes = blockBytes;
		}
	}

	private struct DownsamplePushConstants
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

	private struct EncodePushConstants
	{
		public uint NumBlocksX;

		public uint NumBlocksY;

		public uint OutBlockOffset;

		public uint SrcMip;

		public uint MipWidth;

		public uint MipHeight;

		public float Quality;

		public uint Flags;
	}

	private const string KaiserKernelPath = "Compute/MipmapKaiser.comp.hlsl";

	private const int KaiserPushConstantSize = 32;

	private const long RegionAlignment = 256L;

	private static readonly ComputeBinding[] KaiserBindings = new ComputeBinding[2]
	{
		new ComputeBinding(0, ComputeBindingType.SampledImage),
		new ComputeBinding(1, ComputeBindingType.StorageBuffer)
	};

	private const int MaxMipsPerEncode = 16;

	private const int EncodePushConstantSize = 32;

	private const long FenceTimeoutNs = 60000000000L;

	private const string Bc7FastKernelPath = "Compute/EncodeBC7Fast.comp.hlsl";

	internal const Bc7PackFlags Bc7FastFlags = Bc7PackFlags.WaylithDefault;

	private const string Bc1FastKernelPath = "Compute/EncodeBC1Fast.comp.hlsl";

	private const string Bc4FastKernelPath = "Compute/EncodeBC4Fast.comp.hlsl";

	private const string Bc5FastKernelPath = "Compute/EncodeBC5Fast.comp.hlsl";

	private static readonly Lock _sync = new Lock();

	private static readonly HashSet<BcEncodeTarget> _failedTargets = new HashSet<BcEncodeTarget>();

	private static bool _bc7FastFailed;

	private static GraphicsDevice _graphicsDevice = null;

	private static DeviceEx? _device;

	private static VmaAllocator _allocator = null;

	private static VkCommandPool _commandPool;

	private static VkDescriptorPool _descriptorPool;

	private const long PixelEncodeBufferGranularity = 4194304L;

	public const int MaxPixelEncodesInFlight = 4;

	private static readonly SemaphoreSlim _pixelEncodeSlots = new SemaphoreSlim(4);

	private static readonly ConcurrentBag<PixelEncodeContext> _pixelEncodeContexts = new ConcurrentBag<PixelEncodeContext>();

	public static bool IsConfigured => _device != null;

	public static bool CanEncodeBc7Fast
	{
		get
		{
			if (Bc7FastPermitted)
			{
				return !_bc7FastFailed;
			}
			return false;
		}
	}

	public static bool Bc7FastPermitted
	{
		get
		{
			if (_device != null && CpuBlockEncoder.Bc7Flags == Bc7PackFlags.WaylithDefault)
			{
				return GpuInfo.DeviceType != VkPhysicalDeviceType.Cpu;
			}
			return false;
		}
	}

	internal static GpuComputeBuffer CreateUploadBuffer(string label, long sizeBytes)
	{
		using (_sync.EnterScope())
		{
			if (_device == null)
			{
				throw new InvalidOperationException("TextureBlockEncoder is not configured");
			}
			return GpuComputeBuffer.Create(_device, _allocator, label, sizeBytes, VkBufferUsageFlags.TransferSrcBit);
		}
	}

	internal static bool CanBatchEncode(BcEncodeTarget target)
	{
		using (_sync.EnterScope())
		{
			return CanBatchEncodeLocked(target);
		}
	}

	private static bool CanBatchEncodeLocked(BcEncodeTarget target)
	{
		switch (target)
		{
		case BcEncodeTarget.Bc7:
			return CanEncodeBc7Fast;
		case BcEncodeTarget.Bc1:
		case BcEncodeTarget.Bc5:
		case BcEncodeTarget.Bc4:
			return _device != null && !_failedTargets.Contains(target);
		default:
			return false;
		}
	}

	internal static InFlightBuild SubmitBuildBatch(GpuComputeBuffer staging, List<BuildItem> items, MipFilter filter, BcEncodeTarget target, float quality)
	{
		using (_sync.EnterScope())
		{
			if (_device == null)
			{
				throw new InvalidOperationException("TextureBlockEncoder is not configured");
			}
			if (filter != MipFilter.Kaiser)
			{
				throw new ArgumentOutOfRangeException("filter", filter, "the batch builds Kaiser chains");
			}
			DeviceEx device = _device;
			InFlightBuild inFlightBuild = new InFlightBuild
			{
				Items = items
			};
			inFlightBuild.EncodeTarget = (CanBatchEncodeLocked(target) ? target : BcEncodeTarget.None);
			inFlightBuild.BlockBytes = inFlightBuild.EncodeTarget.BlockBytes();
			long num = 0L;
			long num2 = 0L;
			int num3 = 1;
			int num4 = 0;
			foreach (BuildItem item in items)
			{
				num3 = System.Math.Max(num3, item.MipCount);
				item.LevelOffsets = new long[item.MipCount];
				for (int i = 1; i < item.MipCount; i++)
				{
					item.LevelOffsets[i] = num;
					num += 4L * (long)MipSize(item.Width, i) * MipSize(item.Height, i);
				}
				num4 += item.MipCount - 1;
				if (item.Encode && inFlightBuild.EncodeTarget != 0)
				{
					item.OutOffsets = new long[item.MipCount];
					for (int j = 0; j < item.MipCount; j++)
					{
						item.OutOffsets[j] = Align(num2);
						num2 = item.OutOffsets[j] + (long)BlocksAcross(item.Width, j) * (long)BlocksAcross(item.Height, j) * inFlightBuild.BlockBytes;
					}
					num4 += item.MipCount;
				}
			}
			bool flag = false;
			try
			{
				if (num > 0)
				{
					inFlightBuild.Mips = GpuComputeBuffer.Create(device, _allocator, "TextureBuild.Mips", num, VkBufferUsageFlags.TransferSrcBit, deviceLocal: true);
					inFlightBuild.MipsReadback = GpuComputeBuffer.Create(device, _allocator, "TextureBuild.MipsReadback", num, VkBufferUsageFlags.TransferDstBit, deviceLocal: false, readback: true);
				}
				if (num2 > 0)
				{
					inFlightBuild.Output = GpuComputeBuffer.Create(device, _allocator, "TextureBuild.Blocks", num2, VkBufferUsageFlags.None, deviceLocal: false, readback: true);
				}
				inFlightBuild.DescriptorPool = CreateDescriptorPool(System.Math.Max(1, num4));
				foreach (BuildItem item2 in items)
				{
					item2.Mip0View = CreateMipView(device, item2.Image, 0);
				}
				CommandBuffer commandBuffer = (inFlightBuild.CommandBuffer = device.AllocateCommandBuffer(new VkCommandBufferAllocateInfo
				{
					CommandPool = _commandPool,
					Level = VkCommandBufferLevel.Primary
				}));
				commandBuffer.Begin(VkCommandBufferUsageFlags.OneTimeSubmitBit);
				RecordUploads(commandBuffer, staging, items);
				if (inFlightBuild.Mips != null)
				{
					RecordKaiserChains(commandBuffer, device, inFlightBuild.DescriptorPool, inFlightBuild.Mips, items, num3);
					RecordChainCopies(commandBuffer, inFlightBuild.Mips, items);
					VkBuffer buffer = inFlightBuild.Mips.Buffer;
					VkBuffer buffer2 = inFlightBuild.MipsReadback.Buffer;
					VkBufferCopy reference = new VkBufferCopy
					{
						SrcOffset = ByteSize.Zero,
						DstOffset = ByteSize.Zero,
						Size = ByteSize.Of(num)
					};
					commandBuffer.CopyBuffer(buffer, buffer2, new ReadOnlySpan<VkBufferCopy>(in reference));
				}
				if (inFlightBuild.Output != null)
				{
					RecordEncodeBatch(commandBuffer, device, inFlightBuild.DescriptorPool, inFlightBuild.EncodeTarget, inFlightBuild.Output, items, inFlightBuild.BlockBytes, quality);
				}
				MemoryBarrier(commandBuffer, VkAccessFlags.ShaderWriteBit | VkAccessFlags.TransferWriteBit, VkAccessFlags.HostReadBit, VkPipelineStageFlags.ComputeShaderBit | VkPipelineStageFlags.TransferBit, VkPipelineStageFlags.HostBit);
				commandBuffer.End();
				inFlightBuild.Fence = device.CreateFence(new VkFenceCreateInfo(), device.VkAllocator);
				GraphicsDevice graphicsDevice = _graphicsDevice;
				CommandBuffer reference2 = commandBuffer;
				graphicsDevice.SubmitGraphics(default(Span<VkSemaphore>), default(Span<VkPipelineStageFlags>), new Span<CommandBuffer>(ref reference2), default(Span<VkSemaphore>), inFlightBuild.Fence);
				flag = true;
				return inFlightBuild;
			}
			finally
			{
				if (!flag)
				{
					ReleaseLocked(inFlightBuild, gpuDone: true);
				}
			}
		}
	}

	internal static void CompleteBuildBatch(InFlightBuild build)
	{
		bool gpuDone = false;
		try
		{
			WaitForEncodeFence(_device, build.Fence);
			gpuDone = true;
			GpuComputeBuffer.Mapping? mapping = build.Output?.Map();
			GpuComputeBuffer.Mapping mapping3;
			try
			{
				GpuComputeBuffer.Mapping? mapping2 = build.MipsReadback?.Map();
				try
				{
					foreach (BuildItem item in build.Items)
					{
						if (item.Encode && mapping.HasValue)
						{
							GpuComputeBuffer.Mapping valueOrDefault = mapping.GetValueOrDefault();
							byte[][] array = new byte[item.MipCount][];
							for (int i = 0; i < item.MipCount; i++)
							{
								array[i] = GC.AllocateUninitializedArray<byte>(BlocksAcross(item.Width, i) * BlocksAcross(item.Height, i) * build.BlockBytes);
								valueOrDefault.Slice(item.OutOffsets[i], array[i].Length).CopyTo(array[i]);
							}
							if (build.EncodeTarget != BcEncodeTarget.Bc7 || !IsAllZero(array))
							{
								item.Blocks = array;
								continue;
							}
							using (_sync.EnterScope())
							{
								_bc7FastFailed = true;
							}
							LogCategory log = RenderLog.Log;
							bool shouldAppend;
							LogString<Error> message = new LogString<Error>(119, 1, log, out shouldAppend);
							if (shouldAppend)
							{
								message.AppendLiteral("TextureBlockEncoder: ");
								message.AppendFormatted(build.EncodeTarget);
								message.AppendLiteral(" batch produced all-zero blocks (gpu reset mid-encode?), falling back to uncompressed for this run");
							}
							log.Error(message, "CompleteBuildBatch", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Rendering\\TextureBlockEncoder.Batch.cs", 228);
						}
						item.MipLevels = new byte[item.MipCount][];
						for (int j = 1; j < item.MipCount; j++)
						{
							item.MipLevels[j] = GC.AllocateUninitializedArray<byte>(4 * MipSize(item.Width, j) * MipSize(item.Height, j));
							mapping3 = mapping2.Value;
							mapping3.Slice(item.LevelOffsets[j], item.MipLevels[j].Length).CopyTo(item.MipLevels[j]);
						}
					}
				}
				finally
				{
					if (mapping2.HasValue)
					{
						mapping3 = mapping2.GetValueOrDefault();
						((IDisposable)mapping3/*cast due to .constrained prefix*/).Dispose();
					}
				}
			}
			finally
			{
				if (mapping.HasValue)
				{
					mapping3 = mapping.GetValueOrDefault();
					((IDisposable)mapping3/*cast due to .constrained prefix*/).Dispose();
				}
			}
		}
		finally
		{
			using (_sync.EnterScope())
			{
				ReleaseLocked(build, gpuDone);
			}
		}
	}

	private static void ReleaseLocked(InFlightBuild build, bool gpuDone)
	{
		DeviceEx device = _device;
		if (device == null)
		{
			return;
		}
		if (!build.Fence.IsNull())
		{
			device.DestroyFence(build.Fence, device.VkAllocator);
		}
		if (!build.CommandBuffer.Handle.IsNull())
		{
			VkCommandPool commandPool = _commandPool;
			CommandBuffer reference = build.CommandBuffer;
			device.FreeCommandBuffers(commandPool, new ReadOnlySpan<CommandBuffer>(in reference));
		}
		foreach (BuildItem item in build.Items)
		{
			if (gpuDone && !item.Mip0View.IsNull())
			{
				device.DestroyImageView(item.Mip0View, device.VkAllocator);
			}
			item.Mip0View = default(VkImageView);
		}
		if (!build.DescriptorPool.IsNull())
		{
			device.DestroyDescriptorPool(build.DescriptorPool, device.VkAllocator);
		}
		InlineArray5<GpuComputeBuffer> buffer = default(InlineArray5<GpuComputeBuffer>);
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray5<GpuComputeBuffer>, GpuComputeBuffer>(ref buffer, 0) = build.Mips;
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray5<GpuComputeBuffer>, GpuComputeBuffer>(ref buffer, 1) = build.Guidance;
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray5<GpuComputeBuffer>, GpuComputeBuffer>(ref buffer, 2) = build.Partials;
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray5<GpuComputeBuffer>, GpuComputeBuffer>(ref buffer, 3) = build.MipsReadback;
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray5<GpuComputeBuffer>, GpuComputeBuffer>(ref buffer, 4) = build.Output;
		ReadOnlySpan<GpuComputeBuffer> readOnlySpan = global::_003CPrivateImplementationDetails_003E.InlineArrayAsReadOnlySpan<InlineArray5<GpuComputeBuffer>, GpuComputeBuffer>(in buffer, 5);
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			GpuComputeBuffer gpuComputeBuffer = readOnlySpan[i];
			if (gpuDone)
			{
				gpuComputeBuffer?.DisposeNow();
			}
			else
			{
				gpuComputeBuffer?.Dispose();
			}
		}
	}

	private static void RecordUploads(CommandBuffer commandBuffer, GpuComputeBuffer staging, List<BuildItem> items)
	{
		VkImageMemoryBarrier[] array = new VkImageMemoryBarrier[items.Count];
		for (int i = 0; i < items.Count; i++)
		{
			array[i] = ImageBarrier(items[i].Image, 0, items[i].MipCount, VkImageLayout.Undefined, VkImageLayout.TransferDstOptimal, VkAccessFlags.None, VkAccessFlags.TransferWriteBit);
		}
		commandBuffer.PipelineBarrier(VkPipelineStageFlags.TopOfPipeBit, VkPipelineStageFlags.TransferBit, VkDependencyFlags.None, default(ReadOnlySpan<VkMemoryBarrier>), default(ReadOnlySpan<VkBufferMemoryBarrier>), array);
		foreach (BuildItem item in items)
		{
			for (int j = 0; j < item.MipCount; j++)
			{
				item.Image.SetMipLayout(j, VkImageLayout.TransferDstOptimal);
			}
			VkBuffer buffer = staging.Buffer;
			VkImage image = item.Image.Image;
			VkBufferImageCopy reference = new VkBufferImageCopy
			{
				BufferOffset = ByteSize.Of(item.StagingOffset),
				ImageSubresource = new VkImageSubresourceLayers
				{
					AspectMask = VkImageAspectFlags.ColorBit,
					MipLevel = 0,
					BaseArrayLayer = 0,
					LayerCount = 1
				},
				ImageExtent = new VkExtent3D(item.Width, item.Height, 1)
			};
			commandBuffer.CopyBufferToImage(buffer, image, VkImageLayout.TransferDstOptimal, new ReadOnlySpan<VkBufferImageCopy>(in reference));
		}
		VkImageMemoryBarrier[] array2 = new VkImageMemoryBarrier[items.Count];
		for (int k = 0; k < items.Count; k++)
		{
			array2[k] = ImageBarrier(items[k].Image, 0, 1, VkImageLayout.TransferDstOptimal, VkImageLayout.ShaderReadOnlyOptimal, VkAccessFlags.TransferWriteBit, VkAccessFlags.ShaderReadBit);
			items[k].Image.SetMipLayout(0, VkImageLayout.ShaderReadOnlyOptimal);
		}
		commandBuffer.PipelineBarrier(VkPipelineStageFlags.TransferBit, VkPipelineStageFlags.FragmentShaderBit | VkPipelineStageFlags.ComputeShaderBit, VkDependencyFlags.None, default(ReadOnlySpan<VkMemoryBarrier>), default(ReadOnlySpan<VkBufferMemoryBarrier>), array2);
	}

	private unsafe static void RecordKaiserChains(CommandBuffer commandBuffer, DeviceEx device, VkDescriptorPool descriptorPool, GpuComputeBuffer mips, List<BuildItem> items, int maxMips)
	{
		ComputeKernel computeKernel = ComputeKernelCache.Load("Compute/MipmapKaiser.comp.hlsl", "main", KaiserBindings, 32, ShaderSourceLanguage.Hlsl);
		Span<byte> span = stackalloc byte[32];
		VkWriteDescriptorSet* ptr = stackalloc VkWriteDescriptorSet[2];
		for (int i = 1; i < maxMips; i++)
		{
			commandBuffer.BindPipeline(VkPipelineBindPoint.Compute, computeKernel.Pipeline);
			foreach (BuildItem item in items)
			{
				if (i < item.MipCount)
				{
					VkDescriptorSet vkDescriptorSet = DescriptorSetWriter.AllocateDescriptorSet(device, descriptorPool, computeKernel.DescriptorSetLayout);
					VkDescriptorImageInfo vkDescriptorImageInfo = new VkDescriptorImageInfo();
					vkDescriptorImageInfo.ImageView = item.Mip0View;
					vkDescriptorImageInfo.ImageLayout = VkImageLayout.ShaderReadOnlyOptimal;
					VkDescriptorImageInfo vkDescriptorImageInfo2 = vkDescriptorImageInfo;
					VkDescriptorBufferInfo vkDescriptorBufferInfo = new VkDescriptorBufferInfo();
					vkDescriptorBufferInfo.Buffer = mips.Buffer;
					vkDescriptorBufferInfo.Offset = ByteSize.Zero;
					vkDescriptorBufferInfo.Range = mips.Size;
					VkDescriptorBufferInfo vkDescriptorBufferInfo2 = vkDescriptorBufferInfo;
					*ptr = new VkWriteDescriptorSet
					{
						DstSet = vkDescriptorSet,
						DstBinding = 0,
						DescriptorCount = 1,
						DescriptorType = VkDescriptorType.SampledImage,
						ImageInfo = &vkDescriptorImageInfo2
					};
					ptr[1] = new VkWriteDescriptorSet
					{
						DstSet = vkDescriptorSet,
						DstBinding = 1,
						DescriptorCount = 1,
						DescriptorType = VkDescriptorType.StorageBuffer,
						BufferInfo = &vkDescriptorBufferInfo2
					};
					device.UpdateDescriptorSets(new ReadOnlySpan<VkWriteDescriptorSet>(ptr, 2), default(ReadOnlySpan<VkCopyDescriptorSet>));
					commandBuffer.BindDescriptorSet(VkPipelineBindPoint.Compute, computeKernel.PipelineLayout, 0, vkDescriptorSet, default(Span<ByteSize32>));
					int num = MipSize(item.Width, i);
					int num2 = MipSize(item.Height, i);
					KaiserPushConstants value = new KaiserPushConstants
					{
						ReadFromImage = ((i == 1) ? 1u : 0u),
						SrcByteOffset = (uint)((i != 1) ? item.LevelOffsets[i - 1] : 0u),
						DstByteOffset = (uint)item.LevelOffsets[i],
						SrcWidth = (uint)MipSize(item.Width, i - 1),
						SrcHeight = (uint)MipSize(item.Height, i - 1),
						DstWidth = (uint)num,
						DstHeight = (uint)num2,
						Premultiplied = (item.Premultiplied ? 1u : 0u)
					};
					MemoryMarshal.Write(span, in value);
					commandBuffer.PushConstants<byte>(computeKernel.PipelineLayout, VkShaderStageFlags.ComputeBit, ByteSize.Zero, span);
					commandBuffer.Dispatch((num + 7) / 8, (num2 + 7) / 8, 1);
				}
			}
			bool flag = i == maxMips - 1;
			MemoryBarrier(commandBuffer, VkAccessFlags.ShaderWriteBit, flag ? VkAccessFlags.TransferReadBit : VkAccessFlags.ShaderReadBit, VkPipelineStageFlags.ComputeShaderBit, flag ? VkPipelineStageFlags.TransferBit : VkPipelineStageFlags.ComputeShaderBit);
		}
	}

	private static void RecordChainCopies(CommandBuffer commandBuffer, GpuComputeBuffer mips, List<BuildItem> items)
	{
		List<VkImageMemoryBarrier> list = new List<VkImageMemoryBarrier>(items.Count);
		foreach (BuildItem item in items)
		{
			if (item.MipCount > 1)
			{
				VkBufferImageCopy[] array = new VkBufferImageCopy[item.MipCount - 1];
				for (int i = 1; i < item.MipCount; i++)
				{
					array[i - 1] = new VkBufferImageCopy
					{
						BufferOffset = ByteSize.Of(item.LevelOffsets[i]),
						ImageSubresource = new VkImageSubresourceLayers
						{
							AspectMask = VkImageAspectFlags.ColorBit,
							MipLevel = i,
							BaseArrayLayer = 0,
							LayerCount = 1
						},
						ImageExtent = new VkExtent3D(MipSize(item.Width, i), MipSize(item.Height, i), 1)
					};
				}
				commandBuffer.CopyBufferToImage(mips.Buffer, item.Image.Image, VkImageLayout.TransferDstOptimal, array);
				list.Add(ImageBarrier(item.Image, 1, item.MipCount - 1, VkImageLayout.TransferDstOptimal, VkImageLayout.ShaderReadOnlyOptimal, VkAccessFlags.TransferWriteBit, VkAccessFlags.ShaderReadBit));
				for (int j = 1; j < item.MipCount; j++)
				{
					item.Image.SetMipLayout(j, VkImageLayout.ShaderReadOnlyOptimal);
				}
			}
		}
		if (list.Count > 0)
		{
			commandBuffer.PipelineBarrier(VkPipelineStageFlags.TransferBit, VkPipelineStageFlags.FragmentShaderBit | VkPipelineStageFlags.ComputeShaderBit, VkDependencyFlags.None, default(ReadOnlySpan<VkMemoryBarrier>), default(ReadOnlySpan<VkBufferMemoryBarrier>), list.ToArray());
		}
	}

	private static void RecordEncodeBatch(CommandBuffer commandBuffer, DeviceEx device, VkDescriptorPool descriptorPool, BcEncodeTarget target, GpuComputeBuffer output, List<BuildItem> items, int blockBytes, float quality)
	{
		ComputeKernel kernel = GetKernel(target);
		foreach (BuildItem item in items)
		{
			if (item.Encode)
			{
				for (int i = 0; i < item.MipCount; i++)
				{
					int num = BlocksAcross(item.Width, i);
					int num2 = BlocksAcross(item.Height, i);
					ComputeDispatch computeDispatch = ComputeDispatch.Rent(kernel, (num + 7) / 8, (num2 + 7) / 8);
					computeDispatch.SetSampledImage(0, item.Image);
					computeDispatch.SetStorageBuffer(1, output);
					computeDispatch.SetPushConstants<EncodePushConstants>(new EncodePushConstants
					{
						NumBlocksX = (uint)num,
						NumBlocksY = (uint)num2,
						OutBlockOffset = (uint)(item.OutOffsets[i] / blockBytes),
						SrcMip = (uint)i,
						MipWidth = (uint)MipSize(item.Width, i),
						MipHeight = (uint)MipSize(item.Height, i),
						Quality = quality,
						Flags = (uint)CpuBlockEncoder.Bc7Flags
					});
					ComputeDispatchQueue.RecordDispatch(commandBuffer, device, descriptorPool, computeDispatch);
					ComputeDispatch.Return(computeDispatch);
				}
			}
		}
	}

	private static VkImageView CreateMipView(DeviceEx device, GpuTextureAllocation image, int mip)
	{
		VkImageViewCreateInfo pCreateInfo = new VkImageViewCreateInfo
		{
			Image = image.Image,
			ViewType = VkImageViewType._2D,
			Format = ((image.Format == VkFormat.Undefined) ? VkFormat.R8G8B8A8UNorm : image.Format),
			SubresourceRange = new VkImageSubresourceRange
			{
				AspectMask = VkImageAspectFlags.ColorBit,
				BaseMipLevel = mip,
				LevelCount = 1,
				BaseArrayLayer = 0,
				LayerCount = 1
			}
		};
		return device.CreateImageView(in pCreateInfo, device.VkAllocator);
	}

	private static VkImageMemoryBarrier ImageBarrier(GpuTextureAllocation image, int baseMip, int mipCount, VkImageLayout from, VkImageLayout to, VkAccessFlags srcAccess, VkAccessFlags dstAccess)
	{
		VkImageMemoryBarrier result = new VkImageMemoryBarrier();
		result.SrcAccessMask = srcAccess;
		result.DstAccessMask = dstAccess;
		result.OldLayout = from;
		result.NewLayout = to;
		result.SrcQueueFamilyIndex = -1;
		result.DstQueueFamilyIndex = -1;
		result.Image = image.Image;
		result.SubresourceRange = new VkImageSubresourceRange
		{
			AspectMask = VkImageAspectFlags.ColorBit,
			BaseMipLevel = baseMip,
			LevelCount = mipCount,
			BaseArrayLayer = 0,
			LayerCount = 1
		};
		return result;
	}

	private static void MemoryBarrier(CommandBuffer commandBuffer, VkAccessFlags srcAccess, VkAccessFlags dstAccess, VkPipelineStageFlags srcStage, VkPipelineStageFlags dstStage)
	{
		VkMemoryBarrier reference = new VkMemoryBarrier
		{
			SrcAccessMask = srcAccess,
			DstAccessMask = dstAccess
		};
		commandBuffer.PipelineBarrier(srcStage, dstStage, VkDependencyFlags.None, new ReadOnlySpan<VkMemoryBarrier>(in reference), default(ReadOnlySpan<VkBufferMemoryBarrier>), default(ReadOnlySpan<VkImageMemoryBarrier>));
	}

	private static int MipSize(int size, int mip)
	{
		return System.Math.Max(1, size >> mip);
	}

	private static int BlocksAcross(int size, int mip)
	{
		return (MipSize(size, mip) + 3) / 4;
	}

	private static long Align(long offset)
	{
		return (offset + 256 - 1) / 256 * 256;
	}

	public static void Configure(GraphicsDevice graphicsDevice)
	{
		ArgumentNullException.ThrowIfNull(graphicsDevice, "graphicsDevice");
		AssetBakeHooks.BootComplete -= ReleasePixelEncodeContexts;
		AssetBakeHooks.BootComplete += ReleasePixelEncodeContexts;
		using (_sync.EnterScope())
		{
			ShutdownLocked();
			_graphicsDevice = graphicsDevice;
			_device = graphicsDevice.Device;
			_allocator = graphicsDevice.Allocator;
			DeviceEx? device = _device;
			VkCommandPoolCreateInfo pCreateInfo = new VkCommandPoolCreateInfo
			{
				QueueFamilyIndex = graphicsDevice.GraphicsFamilyIndex,
				Flags = (VkCommandPoolCreateFlags.TransientBit | VkCommandPoolCreateFlags.ResetCommandBufferBit)
			};
			_commandPool = device.CreateCommandPool(in pCreateInfo, _device.VkAllocator);
			_descriptorPool = CreateDescriptorPool(16);
			_failedTargets.Clear();
			_bc7FastFailed = false;
		}
	}

	public static void Shutdown()
	{
		using (_sync.EnterScope())
		{
			ShutdownLocked();
		}
	}

	private static void ShutdownLocked()
	{
		if (_device != null)
		{
			ReleasePixelEncodeContextsLocked();
			if (!_descriptorPool.IsNull())
			{
				_device.DestroyDescriptorPool(_descriptorPool, _device.VkAllocator);
			}
			if (!_commandPool.IsNull())
			{
				_device.DestroyCommandPool(_commandPool, _device.VkAllocator);
			}
			_descriptorPool = default(VkDescriptorPool);
			_commandPool = default(VkCommandPool);
			_device = null;
			_allocator = null;
			_graphicsDevice = null;
		}
	}

	public static byte[][]? TryEncodeFromPixels(byte[][]? levels, int width, int height, int mipCount, BcEncodeTarget target, bool premultiplied = false)
	{
		if (levels == null || levels.Length <= 0 || levels[0] == null || target == BcEncodeTarget.None)
		{
			return null;
		}
		mipCount = System.Math.Clamp(mipCount, 1, 16);
		for (int i = 0; i < mipCount; i++)
		{
			if (i >= levels.Length || levels[i] == null)
			{
				return CpuBlockEncoder.TryEncodeFromPixels(levels[0], width, height, mipCount, target, parallel: true, premultiplied);
			}
		}
		return CpuBlockEncoder.EncodeLevels(levels, width, height, mipCount, target);
	}

	public static byte[][]? EncodeFromImage(GpuTextureAllocation? source, int width, int height, int mipCount, BcEncodeTarget target, float quality, bool allowGpuKernel = false)
	{
		if (source == null || width <= 0 || height <= 0)
		{
			return null;
		}
		mipCount = System.Math.Clamp(mipCount, 1, 16);
		switch (target)
		{
		case BcEncodeTarget.Bc7:
			return (allowGpuKernel ? TryEncodeBc7Fast(source, width, height, mipCount, quality) : null) ?? CpuBlockEncoder.TryEncodeBc7FromImage(source, width, height, mipCount);
		case BcEncodeTarget.None:
			return null;
		default:
			using (_sync.EnterScope())
			{
				if (_device == null || _failedTargets.Contains(target))
				{
					return null;
				}
				try
				{
					return EncodeLocked(source, width, height, mipCount, target, quality);
				}
				catch (Exception ex)
				{
					_failedTargets.Add(target);
					LogCategory log = RenderLog.Log;
					bool shouldAppend;
					LogString<Error> message = new LogString<Error>(80, 2, log, out shouldAppend);
					if (shouldAppend)
					{
						message.AppendLiteral("TextureBlockEncoder: ");
						message.AppendFormatted(target);
						message.AppendLiteral(" encode failed, falling back to uncompressed for this run: ");
						message.AppendFormatted(ex.Message);
					}
					log.Error(message, "EncodeFromImage", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Rendering\\TextureBlockEncoder.cs", 177);
					return null;
				}
			}
		}
	}

	public static byte[][]? TryEncodeBc7Fast(GpuTextureAllocation? source, int width, int height, int mipCount, float quality = 1f)
	{
		return TryEncodeBc7Fast(source, null, width, height, mipCount, quality);
	}

	public static byte[][]? TryEncodeBc7Fast(GpuTextureAllocation? source, GpuTextureAllocation? destination, int width, int height, int mipCount, float quality = 1f)
	{
		if (source == null || width <= 0 || height <= 0)
		{
			return null;
		}
		using (_sync.EnterScope())
		{
			if (!CanEncodeBc7Fast)
			{
				return null;
			}
			try
			{
				return EncodeLocked(source, width, height, System.Math.Clamp(mipCount, 1, 16), BcEncodeTarget.Bc7, quality, destination);
			}
			catch (Exception ex)
			{
				_bc7FastFailed = true;
				LogCategory log = RenderLog.Log;
				bool shouldAppend;
				LogString<Error> message = new LogString<Error>(89, 1, log, out shouldAppend);
				if (shouldAppend)
				{
					message.AppendLiteral("TextureBlockEncoder: EncodeBC7Fast failed, falling back to the CPU encoder for this run: ");
					message.AppendFormatted(ex.Message);
				}
				log.Error(message, "TryEncodeBc7Fast", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Rendering\\TextureBlockEncoder.cs", 214);
				return null;
			}
		}
	}

	internal static byte[][]? EncodeWithKernel(GpuTextureAllocation source, int width, int height, int mipCount, BcEncodeTarget target, ComputeKernel kernel, float quality)
	{
		using (_sync.EnterScope())
		{
			return (_device == null) ? null : EncodeLocked(source, width, height, System.Math.Clamp(mipCount, 1, 16), target, quality, null, kernel);
		}
	}

	private static byte[][] EncodeLocked(GpuTextureAllocation source, int width, int height, int mipCount, BcEncodeTarget target, float quality, GpuTextureAllocation? destination = null, ComputeKernel? kernel = null)
	{
		if (kernel == null)
		{
			kernel = GetKernel(target);
		}
		int num = target.BlockBytes();
		(int[] blocksX, int[] blocksY, int[] blockOffset, int totalBlocks) tuple = LayoutBlocks(width, height, mipCount);
		int[] item = tuple.blocksX;
		int[] item2 = tuple.blocksY;
		int[] item3 = tuple.blockOffset;
		long num2 = (long)tuple.totalBlocks * (long)num;
		DeviceEx device = _device;
		GpuComputeBuffer gpuComputeBuffer = GpuComputeBuffer.Create(device, _allocator, "BCEncode.Out", num2, (destination != null) ? VkBufferUsageFlags.TransferSrcBit : VkBufferUsageFlags.None, deviceLocal: false, readback: true);
		bool flag = false;
		try
		{
			device.ResetDescriptorPool(_descriptorPool, 0);
			CommandBuffer commandBuffer = device.AllocateCommandBuffer(new VkCommandBufferAllocateInfo
			{
				CommandPool = _commandPool,
				Level = VkCommandBufferLevel.Primary
			});
			VkFence fence = default(VkFence);
			try
			{
				commandBuffer.Begin(VkCommandBufferUsageFlags.OneTimeSubmitBit);
				RecordSourceReadBarrier(commandBuffer);
				RecordEncodeDispatches(commandBuffer, device, _descriptorPool, kernel, source, gpuComputeBuffer, width, height, mipCount, item, item2, item3, quality);
				if (destination != null)
				{
					RecordBlockCopyToImage(commandBuffer, gpuComputeBuffer, destination, width, height, mipCount, item3, num);
				}
				VkBufferMemoryBarrier vkBufferMemoryBarrier = new VkBufferMemoryBarrier();
				vkBufferMemoryBarrier.SrcAccessMask = VkAccessFlags.ShaderWriteBit;
				vkBufferMemoryBarrier.DstAccessMask = VkAccessFlags.HostReadBit;
				vkBufferMemoryBarrier.SrcQueueFamilyIndex = -1;
				vkBufferMemoryBarrier.DstQueueFamilyIndex = -1;
				vkBufferMemoryBarrier.Buffer = gpuComputeBuffer.Buffer;
				vkBufferMemoryBarrier.Offset = ByteSize.Zero;
				vkBufferMemoryBarrier.Size = ByteSize.Of(num2);
				VkBufferMemoryBarrier vkBufferMemoryBarrier2 = vkBufferMemoryBarrier;
				ReadOnlySpan<VkMemoryBarrier> pMemoryBarriers = default(Span<VkMemoryBarrier>);
				VkBufferMemoryBarrier reference = vkBufferMemoryBarrier2;
				commandBuffer.PipelineBarrier(VkPipelineStageFlags.ComputeShaderBit, VkPipelineStageFlags.HostBit, VkDependencyFlags.None, pMemoryBarriers, new ReadOnlySpan<VkBufferMemoryBarrier>(in reference), default(Span<VkImageMemoryBarrier>));
				commandBuffer.End();
				fence = device.CreateFence(new VkFenceCreateInfo(), device.VkAllocator);
				GraphicsDevice graphicsDevice = _graphicsDevice;
				CommandBuffer reference2 = commandBuffer;
				graphicsDevice.SubmitGraphics(default(Span<VkSemaphore>), default(Span<VkPipelineStageFlags>), new Span<CommandBuffer>(ref reference2), default(Span<VkSemaphore>), fence);
				WaitForEncodeFence(device, fence);
				flag = true;
			}
			finally
			{
				if (!fence.IsNull())
				{
					device.DestroyFence(fence, device.VkAllocator);
				}
				VkCommandPool commandPool = _commandPool;
				CommandBuffer reference3 = commandBuffer;
				device.FreeCommandBuffers(commandPool, new ReadOnlySpan<CommandBuffer>(in reference3));
			}
			byte[][] array = new byte[mipCount][];
			long[] array2 = new long[mipCount];
			for (int i = 0; i < mipCount; i++)
			{
				array[i] = new byte[item[i] * item2[i] * num];
				array2[i] = (long)item3[i] * (long)num;
			}
			gpuComputeBuffer.ReadRanges(array, array2, mipCount);
			if (target == BcEncodeTarget.Bc7)
			{
				ThrowIfAllZero(array);
			}
			return array;
		}
		finally
		{
			if (flag)
			{
				gpuComputeBuffer.DisposeNow();
			}
			else
			{
				gpuComputeBuffer.Dispose();
			}
		}
	}

	private static void RecordBlockCopyToImage(CommandBuffer commandBuffer, GpuComputeBuffer blocks, GpuTextureAllocation destination, int width, int height, int mipCount, int[] blockOffset, int blockBytes)
	{
		VkMemoryBarrier vkMemoryBarrier = new VkMemoryBarrier();
		vkMemoryBarrier.SrcAccessMask = VkAccessFlags.ShaderWriteBit;
		vkMemoryBarrier.DstAccessMask = VkAccessFlags.TransferReadBit;
		VkMemoryBarrier vkMemoryBarrier2 = vkMemoryBarrier;
		VkMemoryBarrier reference = vkMemoryBarrier2;
		commandBuffer.PipelineBarrier(VkPipelineStageFlags.ComputeShaderBit, VkPipelineStageFlags.TransferBit, VkDependencyFlags.None, new ReadOnlySpan<VkMemoryBarrier>(in reference), default(Span<VkBufferMemoryBarrier>), default(Span<VkImageMemoryBarrier>));
		VkBufferImageCopy[] array = new VkBufferImageCopy[mipCount];
		for (int i = 0; i < mipCount; i++)
		{
			TransitionMip(commandBuffer, destination, i, VkImageLayout.TransferDstOptimal, VkAccessFlags.None, VkAccessFlags.TransferWriteBit, VkPipelineStageFlags.TopOfPipeBit, VkPipelineStageFlags.TransferBit);
			array[i] = new VkBufferImageCopy
			{
				BufferOffset = ByteSize.Of((long)blockOffset[i] * (long)blockBytes),
				BufferRowLength = 0,
				BufferImageHeight = 0,
				ImageSubresource = new VkImageSubresourceLayers
				{
					AspectMask = VkImageAspectFlags.ColorBit,
					MipLevel = i,
					BaseArrayLayer = 0,
					LayerCount = 1
				},
				ImageOffset = new VkOffset3D(0, 0, 0),
				ImageExtent = new VkExtent3D(System.Math.Max(1, width >> i), System.Math.Max(1, height >> i), 1)
			};
		}
		commandBuffer.CopyBufferToImage(blocks.Buffer, destination.Image, VkImageLayout.TransferDstOptimal, array);
		for (int j = 0; j < mipCount; j++)
		{
			TransitionMip(commandBuffer, destination, j, VkImageLayout.ShaderReadOnlyOptimal, VkAccessFlags.TransferWriteBit, VkAccessFlags.ShaderReadBit, VkPipelineStageFlags.TransferBit, VkPipelineStageFlags.FragmentShaderBit);
		}
	}

	private static void TransitionMip(CommandBuffer commandBuffer, GpuTextureAllocation texture, int mip, VkImageLayout newLayout, VkAccessFlags sourceAccess, VkAccessFlags destinationAccess, VkPipelineStageFlags sourceStage, VkPipelineStageFlags destinationStage)
	{
		VkImageMemoryBarrier vkImageMemoryBarrier = new VkImageMemoryBarrier();
		vkImageMemoryBarrier.SrcAccessMask = sourceAccess;
		vkImageMemoryBarrier.DstAccessMask = destinationAccess;
		vkImageMemoryBarrier.OldLayout = texture.GetMipLayout(mip);
		vkImageMemoryBarrier.NewLayout = newLayout;
		vkImageMemoryBarrier.SrcQueueFamilyIndex = -1;
		vkImageMemoryBarrier.DstQueueFamilyIndex = -1;
		vkImageMemoryBarrier.Image = texture.Image;
		vkImageMemoryBarrier.SubresourceRange = new VkImageSubresourceRange
		{
			AspectMask = VkImageAspectFlags.ColorBit,
			BaseMipLevel = mip,
			LevelCount = 1,
			BaseArrayLayer = 0,
			LayerCount = 1
		};
		VkImageMemoryBarrier vkImageMemoryBarrier2 = vkImageMemoryBarrier;
		VkImageMemoryBarrier reference = vkImageMemoryBarrier2;
		commandBuffer.PipelineBarrier(sourceStage, destinationStage, VkDependencyFlags.None, default(ReadOnlySpan<VkMemoryBarrier>), default(ReadOnlySpan<VkBufferMemoryBarrier>), new ReadOnlySpan<VkImageMemoryBarrier>(in reference));
		texture.SetMipLayout(mip, newLayout);
	}

	private static void WaitForEncodeFence(DeviceEx device, VkFence fence)
	{
		VkResult vkResult = device.WaitForFence(fence, unchecked((nint)60000000000L));
		if (vkResult != 0)
		{
			throw new InvalidOperationException($"encode fence wait returned {vkResult}");
		}
	}

	private static void ThrowIfAllZero(byte[][] levels)
	{
		if (IsAllZero(levels))
		{
			throw new InvalidOperationException("encoder produced all-zero blocks (gpu reset mid-encode?)");
		}
	}

	private static bool IsAllZero(byte[][] levels)
	{
		for (int i = 0; i < levels.Length; i++)
		{
			if (((ReadOnlySpan<byte>)levels[i].AsSpan()).ContainsAnyExcept((byte)0))
			{
				return false;
			}
		}
		return true;
	}

	private static (int[] blocksX, int[] blocksY, int[] blockOffset, int totalBlocks) LayoutBlocks(int width, int height, int mipCount)
	{
		int[] array = new int[mipCount];
		int[] array2 = new int[mipCount];
		int[] array3 = new int[mipCount];
		int num = 0;
		for (int i = 0; i < mipCount; i++)
		{
			array[i] = (System.Math.Max(1, width >> i) + 3) / 4;
			array2[i] = (System.Math.Max(1, height >> i) + 3) / 4;
			array3[i] = num;
			num += array[i] * array2[i];
		}
		return (blocksX: array, blocksY: array2, blockOffset: array3, totalBlocks: num);
	}

	private static void RecordEncodeDispatches(CommandBuffer commandBuffer, DeviceEx device, VkDescriptorPool descriptorPool, ComputeKernel kernel, GpuTextureAllocation source, GpuComputeBuffer outBuffer, int width, int height, int mipCount, int[] blocksX, int[] blocksY, int[] blockOffset, float quality)
	{
		for (int i = 0; i < mipCount; i++)
		{
			ComputeDispatch computeDispatch = ComputeDispatch.Rent(kernel, (blocksX[i] + 7) / 8, (blocksY[i] + 7) / 8);
			computeDispatch.SetSampledImage(0, source);
			computeDispatch.SetStorageBuffer(1, outBuffer);
			computeDispatch.SetPushConstants<EncodePushConstants>(new EncodePushConstants
			{
				NumBlocksX = (uint)blocksX[i],
				NumBlocksY = (uint)blocksY[i],
				OutBlockOffset = (uint)blockOffset[i],
				SrcMip = (uint)i,
				MipWidth = (uint)System.Math.Max(1, width >> i),
				MipHeight = (uint)System.Math.Max(1, height >> i),
				Quality = quality,
				Flags = (uint)CpuBlockEncoder.Bc7Flags
			});
			ComputeDispatchQueue.RecordDispatch(commandBuffer, device, descriptorPool, computeDispatch);
			ComputeDispatch.Return(computeDispatch);
		}
	}

	public static byte[][]? TryEncodeFromPixelsOnGpu(byte[][] levels, int width, int height, int mipCount, BcEncodeTarget target, float quality, bool premultiplied = false)
	{
		PendingPixelEncode pendingPixelEncode = TryBeginEncodeFromPixelsOnGpu(levels, width, height, mipCount, target, quality, premultiplied);
		if (pendingPixelEncode == null)
		{
			return null;
		}
		return CompleteEncodeFromPixelsOnGpu(pendingPixelEncode);
	}

	public unsafe static PendingPixelEncode? TryBeginEncodeFromPixelsOnGpu(byte[][] levels, int width, int height, int mipCount, BcEncodeTarget target, float quality, bool premultiplied = false)
	{
		bool flag = target == BcEncodeTarget.Bc7;
		DeviceEx device;
		VmaAllocator allocator;
		using (_sync.EnterScope())
		{
			if (_device == null || target == BcEncodeTarget.None || (flag ? (!CanEncodeBc7Fast) : _failedTargets.Contains(target)))
			{
				return null;
			}
			device = _device;
			allocator = _allocator;
		}
		mipCount = System.Math.Clamp(mipCount, 1, 16);
		int num = ((levels.Length < mipCount) ? 1 : mipCount);
		ComputeKernel kernel = GetKernel(target);
		ComputeKernel computeKernel = ((num < mipCount) ? LoadDownsampleKernel() : null);
		int num2 = target.BlockBytes();
		(int[] blocksX, int[] blocksY, int[] blockOffset, int totalBlocks) tuple = LayoutBlocks(width, height, mipCount);
		int[] item = tuple.blocksX;
		int[] item2 = tuple.blocksY;
		int[] item3 = tuple.blockOffset;
		int item4 = tuple.totalBlocks;
		long[] array = new long[mipCount];
		long num3 = 0L;
		long num4 = 0L;
		for (int i = 0; i < mipCount; i++)
		{
			long num5 = (long)System.Math.Max(1, width >> i) * (long)System.Math.Max(1, height >> i) * 4;
			if (i < num)
			{
				if (levels[i]?.Length != num5)
				{
					return null;
				}
				array[i] = num3;
				num3 += num5;
			}
			else
			{
				array[i] = num4;
				num4 += num5;
			}
		}
		_pixelEncodeSlots.Wait();
		PixelEncodeContext pixelEncodeContext = TakePixelEncodeContext(device);
		GpuTextureAllocation gpuTextureAllocation = null;
		VkImageView vkImageView = default(VkImageView);
		CommandBuffer commandBuffer = default(CommandBuffer);
		bool flag2 = false;
		try
		{
			using (FrameProfiler.Scope("BlockEncodeGpu/Stage"))
			{
				byte* ptr = PixelEncodeContext.Ensure(ref pixelEncodeContext.Staging, ref pixelEncodeContext.StagingMapping, device, allocator, "BCEncode.Staging", num3, readback: false);
				for (int j = 0; j < num; j++)
				{
					levels[j].AsSpan().CopyTo(new Span<byte>(ptr + array[j], levels[j].Length));
				}
			}
			GpuComputeBuffer staging = pixelEncodeContext.Staging;
			gpuTextureAllocation = GpuTextureService.AllocateTexture2D("BCEncode.Source", width, height, mipCount, VkFormat.R8G8B8A8UNorm) ?? throw new InvalidOperationException("could not allocate the source image");
			byte* outMapped = PixelEncodeContext.Ensure(ref pixelEncodeContext.Out, ref pixelEncodeContext.OutMapping, device, allocator, "BCEncode.Out", (long)item4 * (long)num2, readback: true);
			GpuComputeBuffer @out = pixelEncodeContext.Out;
			device.ResetDescriptorPool(pixelEncodeContext.descriptorPool, 0);
			commandBuffer = device.AllocateCommandBuffer(new VkCommandBufferAllocateInfo
			{
				CommandPool = pixelEncodeContext.commandPool,
				Level = VkCommandBufferLevel.Primary
			});
			commandBuffer.Begin(VkCommandBufferUsageFlags.OneTimeSubmitBit);
			VkBufferImageCopy[] array2 = new VkBufferImageCopy[mipCount];
			for (int k = 0; k < mipCount; k++)
			{
				array2[k] = new VkBufferImageCopy
				{
					BufferOffset = ByteSize.Of(array[k]),
					ImageSubresource = new VkImageSubresourceLayers
					{
						AspectMask = VkImageAspectFlags.ColorBit,
						MipLevel = k,
						LayerCount = 1
					},
					ImageExtent = new VkExtent3D(System.Math.Max(1, width >> k), System.Math.Max(1, height >> k), 1)
				};
			}
			RecordSourceLayout(commandBuffer, gpuTextureAllocation, 0, mipCount, VkImageLayout.Undefined, VkImageLayout.TransferDstOptimal, VkAccessFlags.None, VkAccessFlags.TransferWriteBit, VkPipelineStageFlags.TopOfPipeBit, VkPipelineStageFlags.TransferBit);
			commandBuffer.CopyBufferToImage(staging.Buffer, gpuTextureAllocation.Image, VkImageLayout.TransferDstOptimal, array2.AsSpan(0, num));
			RecordSourceLayout(commandBuffer, gpuTextureAllocation, 0, num, VkImageLayout.TransferDstOptimal, VkImageLayout.ShaderReadOnlyOptimal, VkAccessFlags.TransferWriteBit, VkAccessFlags.ShaderReadBit, VkPipelineStageFlags.TransferBit, VkPipelineStageFlags.ComputeShaderBit);
			if (computeKernel != null)
			{
				VkImageViewCreateInfo pCreateInfo = new VkImageViewCreateInfo
				{
					Image = gpuTextureAllocation.Image,
					ViewType = VkImageViewType._2D,
					Format = VkFormat.R8G8B8A8UNorm,
					SubresourceRange = new VkImageSubresourceRange
					{
						AspectMask = VkImageAspectFlags.ColorBit,
						LevelCount = 1,
						LayerCount = 1
					}
				};
				vkImageView = device.CreateImageView(in pCreateInfo, device.VkAllocator);
				GpuComputeBuffer gpuComputeBuffer = PixelEncodeContext.EnsureDeviceLocal(ref pixelEncodeContext.Levels, device, allocator, "BCEncode.Levels", num4);
				RecordDownsampleChain(commandBuffer, device, pixelEncodeContext.descriptorPool, computeKernel, vkImageView, gpuComputeBuffer, width, height, mipCount, array, premultiplied);
				commandBuffer.CopyBufferToImage(gpuComputeBuffer.Buffer, gpuTextureAllocation.Image, VkImageLayout.TransferDstOptimal, array2.AsSpan(1));
				RecordSourceLayout(commandBuffer, gpuTextureAllocation, 1, mipCount - 1, VkImageLayout.TransferDstOptimal, VkImageLayout.ShaderReadOnlyOptimal, VkAccessFlags.TransferWriteBit, VkAccessFlags.ShaderReadBit, VkPipelineStageFlags.TransferBit, VkPipelineStageFlags.ComputeShaderBit);
			}
			RecordEncodeDispatches(commandBuffer, device, pixelEncodeContext.descriptorPool, kernel, gpuTextureAllocation, @out, width, height, mipCount, item, item2, item3, quality);
			VkMemoryBarrier vkMemoryBarrier = new VkMemoryBarrier();
			vkMemoryBarrier.SrcAccessMask = VkAccessFlags.ShaderWriteBit;
			vkMemoryBarrier.DstAccessMask = VkAccessFlags.HostReadBit;
			VkMemoryBarrier vkMemoryBarrier2 = vkMemoryBarrier;
			CommandBuffer handle = commandBuffer;
			VkMemoryBarrier reference = vkMemoryBarrier2;
			handle.PipelineBarrier(VkPipelineStageFlags.ComputeShaderBit, VkPipelineStageFlags.HostBit, VkDependencyFlags.None, new ReadOnlySpan<VkMemoryBarrier>(in reference), default(Span<VkBufferMemoryBarrier>), default(Span<VkImageMemoryBarrier>));
			commandBuffer.End();
			flag2 = true;
			GraphicsDevice graphicsDevice = _graphicsDevice;
			CommandBuffer reference2 = commandBuffer;
			graphicsDevice.SubmitGraphics(default(Span<VkSemaphore>), default(Span<VkPipelineStageFlags>), new Span<CommandBuffer>(ref reference2), default(Span<VkSemaphore>), pixelEncodeContext.fence);
			return new PendingPixelEncode(device, pixelEncodeContext, gpuTextureAllocation, vkImageView, commandBuffer, target, outMapped, item, item2, item3, num2);
		}
		catch (Exception exception)
		{
			OnPixelEncodeFailed(target, exception);
			if (!flag2)
			{
				if (!vkImageView.IsNull())
				{
					device.DestroyImageView(vkImageView, device.VkAllocator);
				}
				if (!commandBuffer.Handle.IsNull())
				{
					VkCommandPool commandPool = pixelEncodeContext.commandPool;
					CommandBuffer reference3 = commandBuffer;
					device.FreeCommandBuffers(commandPool, new ReadOnlySpan<CommandBuffer>(in reference3));
				}
			}
			gpuTextureAllocation?.Dispose();
			_pixelEncodeSlots.Release();
			return null;
		}
	}

	public unsafe static byte[][]? CompleteEncodeFromPixelsOnGpu(PendingPixelEncode pending)
	{
		DeviceEx device = pending.Device;
		PixelEncodeContext context = pending.Context;
		bool flag = false;
		try
		{
			WaitForEncodeFence(device, context.fence);
			VkFence reference = context.fence;
			device.ResetFences(new ReadOnlySpan<VkFence>(in reference));
			flag = true;
			byte[][] array = new byte[pending.BlocksX.Length][];
			using (FrameProfiler.Scope("BlockEncodeGpu/Readback"))
			{
				for (int i = 0; i < array.Length; i++)
				{
					int length = pending.BlocksX[i] * pending.BlocksY[i] * pending.BlockBytes;
					array[i] = GC.AllocateUninitializedArray<byte>(length);
					new ReadOnlySpan<byte>(pending.OutMapped + (long)pending.BlockOffset[i] * (long)pending.BlockBytes, length).CopyTo(array[i]);
				}
			}
			if (pending.Target == BcEncodeTarget.Bc7)
			{
				ThrowIfAllZero(array);
			}
			return array;
		}
		catch (Exception exception)
		{
			OnPixelEncodeFailed(pending.Target, exception);
			return null;
		}
		finally
		{
			if (flag)
			{
				if (!pending.Mip0View.IsNull())
				{
					device.DestroyImageView(pending.Mip0View, device.VkAllocator);
				}
				pending.Source.DisposeNow();
				VkCommandPool commandPool = context.commandPool;
				CommandBuffer reference2 = pending.CommandBuffer;
				device.FreeCommandBuffers(commandPool, new ReadOnlySpan<CommandBuffer>(in reference2));
				_pixelEncodeContexts.Add(context);
			}
			else
			{
				pending.Source.Dispose();
			}
			_pixelEncodeSlots.Release();
		}
	}

	private static void OnPixelEncodeFailed(BcEncodeTarget target, Exception exception)
	{
		using (_sync.EnterScope())
		{
			if (target == BcEncodeTarget.Bc7)
			{
				_bc7FastFailed = true;
			}
			else
			{
				_failedTargets.Add(target);
			}
		}
		LogCategory log = RenderLog.Log;
		bool shouldAppend;
		LogString<Error> message = new LogString<Error>(95, 2, log, out shouldAppend);
		if (shouldAppend)
		{
			message.AppendLiteral("TextureBlockEncoder: ");
			message.AppendFormatted(target);
			message.AppendLiteral(" encode from pixels failed, falling back to the CPU encoder for this run: ");
			message.AppendFormatted(exception.Message);
		}
		log.Error(message, "OnPixelEncodeFailed", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Rendering\\TextureBlockEncoder.cs", 759);
	}

	public static void ReleasePixelEncodeContexts()
	{
		using (_sync.EnterScope())
		{
			ReleasePixelEncodeContextsLocked();
		}
	}

	private static void ReleasePixelEncodeContextsLocked()
	{
		if (_device != null)
		{
			PixelEncodeContext result;
			while (_pixelEncodeContexts.TryTake(out result))
			{
				result.Destroy(_device);
			}
		}
	}

	private static PixelEncodeContext TakePixelEncodeContext(DeviceEx device)
	{
		if (_pixelEncodeContexts.TryTake(out PixelEncodeContext result))
		{
			return result;
		}
		using (_sync.EnterScope())
		{
			VkCommandPoolCreateInfo pCreateInfo = new VkCommandPoolCreateInfo
			{
				QueueFamilyIndex = _graphicsDevice.GraphicsFamilyIndex,
				Flags = (VkCommandPoolCreateFlags.TransientBit | VkCommandPoolCreateFlags.ResetCommandBufferBit)
			};
			return new PixelEncodeContext(device.CreateCommandPool(in pCreateInfo, device.VkAllocator), CreateDescriptorPool(32), device.CreateFence(new VkFenceCreateInfo(), device.VkAllocator));
		}
	}

	private static void RecordDownsampleChain(CommandBuffer commandBuffer, DeviceEx device, VkDescriptorPool descriptorPool, ComputeKernel kernel, VkImageView mip0, GpuComputeBuffer levels, int width, int height, int mipCount, long[] levelOffsets, bool premultiplied)
	{
		for (int i = 1; i < mipCount; i++)
		{
			int num = System.Math.Max(1, width >> i);
			int num2 = System.Math.Max(1, height >> i);
			ComputeDispatch computeDispatch = ComputeDispatch.Rent(kernel, (num + 7) / 8, (num2 + 7) / 8);
			computeDispatch.SetSampledView(0, mip0, default(VkSampler));
			computeDispatch.SetStorageBuffer(1, levels);
			computeDispatch.SetPushConstants<DownsamplePushConstants>(new DownsamplePushConstants
			{
				ReadFromImage = ((i == 1) ? 1u : 0u),
				SrcByteOffset = (uint)levelOffsets[i - 1],
				DstByteOffset = (uint)levelOffsets[i],
				SrcWidth = (uint)System.Math.Max(1, width >> i - 1),
				SrcHeight = (uint)System.Math.Max(1, height >> i - 1),
				DstWidth = (uint)num,
				DstHeight = (uint)num2,
				Premultiplied = (premultiplied ? 1u : 0u)
			});
			ComputeDispatchQueue.RecordDispatch(commandBuffer, device, descriptorPool, computeDispatch);
			ComputeDispatch.Return(computeDispatch);
			bool flag = i == mipCount - 1;
			VkMemoryBarrier vkMemoryBarrier = new VkMemoryBarrier();
			vkMemoryBarrier.SrcAccessMask = VkAccessFlags.ShaderWriteBit;
			vkMemoryBarrier.DstAccessMask = (flag ? VkAccessFlags.TransferReadBit : VkAccessFlags.ShaderReadBit);
			VkMemoryBarrier vkMemoryBarrier2 = vkMemoryBarrier;
			int dstStageMask = (flag ? 4096 : 2048);
			VkMemoryBarrier reference = vkMemoryBarrier2;
			commandBuffer.PipelineBarrier(VkPipelineStageFlags.ComputeShaderBit, (VkPipelineStageFlags)dstStageMask, VkDependencyFlags.None, new ReadOnlySpan<VkMemoryBarrier>(in reference), default(Span<VkBufferMemoryBarrier>), default(Span<VkImageMemoryBarrier>));
		}
	}

	private static ComputeKernel LoadDownsampleKernel()
	{
		InlineArray2<ComputeBinding> buffer = default(InlineArray2<ComputeBinding>);
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray2<ComputeBinding>, ComputeBinding>(ref buffer, 0) = new ComputeBinding(0, ComputeBindingType.SampledImage);
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray2<ComputeBinding>, ComputeBinding>(ref buffer, 1) = new ComputeBinding(1, ComputeBindingType.StorageBuffer);
		ReadOnlySpan<ComputeBinding> bindings = global::_003CPrivateImplementationDetails_003E.InlineArrayAsReadOnlySpan<InlineArray2<ComputeBinding>, ComputeBinding>(in buffer, 2);
		return ComputeKernelCache.Load("Compute/MipmapDownsample.comp.hlsl", "main", bindings, Marshal.SizeOf<DownsamplePushConstants>(), ShaderSourceLanguage.Hlsl);
	}

	private static void RecordSourceLayout(CommandBuffer commandBuffer, GpuTextureAllocation source, int baseMip, int mipCount, VkImageLayout oldLayout, VkImageLayout newLayout, VkAccessFlags srcAccess, VkAccessFlags dstAccess, VkPipelineStageFlags srcStage, VkPipelineStageFlags dstStage)
	{
		VkImageMemoryBarrier vkImageMemoryBarrier = new VkImageMemoryBarrier();
		vkImageMemoryBarrier.SrcAccessMask = srcAccess;
		vkImageMemoryBarrier.DstAccessMask = dstAccess;
		vkImageMemoryBarrier.OldLayout = oldLayout;
		vkImageMemoryBarrier.NewLayout = newLayout;
		vkImageMemoryBarrier.SrcQueueFamilyIndex = -1;
		vkImageMemoryBarrier.DstQueueFamilyIndex = -1;
		vkImageMemoryBarrier.Image = source.Image;
		vkImageMemoryBarrier.SubresourceRange = new VkImageSubresourceRange
		{
			AspectMask = VkImageAspectFlags.ColorBit,
			BaseMipLevel = baseMip,
			LevelCount = mipCount,
			LayerCount = 1
		};
		VkImageMemoryBarrier vkImageMemoryBarrier2 = vkImageMemoryBarrier;
		ReadOnlySpan<VkMemoryBarrier> pMemoryBarriers = default(Span<VkMemoryBarrier>);
		ReadOnlySpan<VkBufferMemoryBarrier> pBufferMemoryBarriers = default(Span<VkBufferMemoryBarrier>);
		VkImageMemoryBarrier reference = vkImageMemoryBarrier2;
		commandBuffer.PipelineBarrier(srcStage, dstStage, VkDependencyFlags.None, pMemoryBarriers, pBufferMemoryBarriers, new ReadOnlySpan<VkImageMemoryBarrier>(in reference));
	}

	private static void RecordSourceReadBarrier(CommandBuffer commandBuffer)
	{
		VkMemoryBarrier vkMemoryBarrier = new VkMemoryBarrier();
		vkMemoryBarrier.SrcAccessMask = VkAccessFlags.ShaderWriteBit | VkAccessFlags.ColorAttachmentWriteBit | VkAccessFlags.TransferWriteBit;
		vkMemoryBarrier.DstAccessMask = VkAccessFlags.ShaderReadBit;
		VkMemoryBarrier vkMemoryBarrier2 = vkMemoryBarrier;
		VkMemoryBarrier reference = vkMemoryBarrier2;
		commandBuffer.PipelineBarrier(VkPipelineStageFlags.AllCommandsBit, VkPipelineStageFlags.ComputeShaderBit, VkDependencyFlags.None, new ReadOnlySpan<VkMemoryBarrier>(in reference), default(Span<VkBufferMemoryBarrier>), default(Span<VkImageMemoryBarrier>));
	}

	private static ComputeKernel GetKernel(BcEncodeTarget target)
	{
		return target switch
		{
			BcEncodeTarget.Bc7 => LoadSingleDispatchKernel("Compute/EncodeBC7Fast.comp.hlsl", "main"), 
			BcEncodeTarget.Bc4 => LoadSingleDispatchKernel("Compute/EncodeBC4Fast.comp.hlsl", "main"), 
			BcEncodeTarget.Bc5 => LoadSingleDispatchKernel("Compute/EncodeBC5Fast.comp.hlsl", "main"), 
			_ => LoadSingleDispatchKernel("Compute/EncodeBC1Fast.comp.hlsl", "main"), 
		};
	}

	private static ComputeKernel LoadSingleDispatchKernel(string path, string entryPoint)
	{
		InlineArray2<ComputeBinding> buffer = default(InlineArray2<ComputeBinding>);
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray2<ComputeBinding>, ComputeBinding>(ref buffer, 0) = new ComputeBinding(0, ComputeBindingType.SampledImage);
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray2<ComputeBinding>, ComputeBinding>(ref buffer, 1) = new ComputeBinding(1, ComputeBindingType.StorageBuffer);
		ReadOnlySpan<ComputeBinding> bindings = global::_003CPrivateImplementationDetails_003E.InlineArrayAsReadOnlySpan<InlineArray2<ComputeBinding>, ComputeBinding>(in buffer, 2);
		return ComputeKernelCache.Load(path, entryPoint, bindings, 32, ShaderSourceLanguage.Hlsl);
	}

	private unsafe static VkDescriptorPool CreateDescriptorPool(int maxSets)
	{
		ArgumentNullException.ThrowIfNull(_device, "_device");
		VkDescriptorPoolSize* ptr = stackalloc VkDescriptorPoolSize[2];
		*ptr = new VkDescriptorPoolSize
		{
			Type = VkDescriptorType.SampledImage,
			DescriptorCount = maxSets
		};
		ptr[1] = new VkDescriptorPoolSize
		{
			Type = VkDescriptorType.StorageBuffer,
			DescriptorCount = maxSets
		};
		DeviceEx? device = _device;
		VkDescriptorPoolCreateInfo pCreateInfo = new VkDescriptorPoolCreateInfo
		{
			MaxSets = maxSets,
			PoolSizeCount = 2,
			PoolSizes = ptr
		};
		return device.CreateDescriptorPool(in pCreateInfo, _device.VkAllocator);
	}
}
