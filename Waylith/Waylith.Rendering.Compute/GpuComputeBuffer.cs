using System;
using System.Runtime.InteropServices;
using Brutal;
using Brutal.Pointers;
using Brutal.VulkanApi;
using Brutal.VulkanApi.Abstractions;
using Brutal.VulkanApi.Vma;
using Waylith.Graphics;

namespace Waylith.Rendering.Compute;

public sealed class GpuComputeBuffer : IDisposable
{
	public readonly struct Mapping : IDisposable
	{
		private readonly VmaAllocator _allocator;

		private readonly AllocationMapInfo _mapInfo;

		private unsafe readonly byte* _pointer;

		private readonly long _length;

		public unsafe byte* Pointer => _pointer;

		internal unsafe Mapping(VmaAllocator allocator, AllocationMapInfo mapInfo, byte* pointer, long length)
		{
			_allocator = allocator;
			_mapInfo = mapInfo;
			_pointer = pointer;
			_length = length;
		}

		public unsafe Span<byte> Slice(long offset, int length)
		{
			if (offset < 0 || offset + length > _length)
			{
				throw new ArgumentOutOfRangeException("offset", $"{length} bytes at {offset} exceed the {_length} byte buffer.");
			}
			return new Span<byte>(_pointer + offset, length);
		}

		public void Dispose()
		{
			_allocator.UnmapMemory(in _mapInfo);
		}
	}

	private readonly DeviceEx device;

	private readonly VmaAllocator allocator;

	private BufferAllocationInfo allocation;

	private bool disposed;

	public VkBuffer Buffer => allocation.VkBuffer;

	public static AllocationInfo.CreateInfo UploadStaging
	{
		get
		{
			AllocationInfo.CreateInfo result = default(AllocationInfo.CreateInfo);
			result.Preference = MemoryPreference.PreferGpu;
			result.RequiredProperties = VkMemoryPropertyFlags.HostVisibleBit | VkMemoryPropertyFlags.HostCoherentBit;
			result.PreferredProperties = VkMemoryPropertyFlags.DeviceLocalBit | VkMemoryPropertyFlags.HostVisibleBit | VkMemoryPropertyFlags.HostCoherentBit;
			result.Priority = 1f;
			return result;
		}
	}

	public ByteSize Size { get; }

	private GpuComputeBuffer(DeviceEx device, VmaAllocator allocator, BufferAllocationInfo allocation)
	{
		this.device = device;
		this.allocator = allocator;
		this.allocation = allocation;
		Size = allocation.BufferSize;
	}

	public static GpuComputeBuffer Create(DeviceEx device, VmaAllocator allocator, string label, long sizeBytes, VkBufferUsageFlags extraUsage = VkBufferUsageFlags.None, bool deviceLocal = false, bool readback = false, bool upload = false)
	{
		AllocationInfo.CreateInfo createInfo2;
		if (!deviceLocal)
		{
			if (!upload || !GpuInfo.StageUploadsInVram)
			{
				AllocationInfo.CreateInfo createInfo = default(AllocationInfo.CreateInfo);
				createInfo.Preference = MemoryPreference.PreferCpu;
				createInfo.RequiredProperties = VkMemoryPropertyFlags.HostVisibleBit | VkMemoryPropertyFlags.HostCoherentBit;
				createInfo.PreferredProperties = (readback ? (VkMemoryPropertyFlags.HostVisibleBit | VkMemoryPropertyFlags.HostCoherentBit | VkMemoryPropertyFlags.HostCachedBit) : (VkMemoryPropertyFlags.HostVisibleBit | VkMemoryPropertyFlags.HostCoherentBit));
				createInfo.Priority = 1f;
				createInfo2 = createInfo;
			}
			else
			{
				createInfo2 = UploadStaging;
			}
		}
		else
		{
			AllocationInfo.CreateInfo createInfo = default(AllocationInfo.CreateInfo);
			createInfo.Preference = MemoryPreference.PreferGpu;
			createInfo.RequiredProperties = VkMemoryPropertyFlags.DeviceLocalBit;
			createInfo.PreferredProperties = VkMemoryPropertyFlags.DeviceLocalBit;
			createInfo.Priority = 1f;
			createInfo2 = createInfo;
		}
		AllocationInfo.CreateInfo allocateCreateInfo = createInfo2;
		BufferAllocationInfo bufferAllocationInfo = allocator.AllocateBufferChecked(label, new VkBufferCreateInfo
		{
			Size = ByteSize.Of(sizeBytes),
			Usage = (VkBufferUsageFlags.TransferDstBit | VkBufferUsageFlags.StorageBufferBit | extraUsage),
			SharingMode = VkSharingMode.Exclusive
		}, in allocateCreateInfo);
		GpuMemoryTracker.Add(GpuMemoryCategory.ComputeBuffer, in bufferAllocationInfo, null, label);
		return new GpuComputeBuffer(device, allocator, bufferAllocationInfo);
	}

	public GpuComputeBuffer StageUpload(CommandBuffer commandBuffer, ReadOnlySpan<byte> data, long byteOffset)
	{
		GpuComputeBuffer gpuComputeBuffer = Create(device, allocator, "ComputeStaging", data.Length, VkBufferUsageFlags.TransferSrcBit);
		gpuComputeBuffer.Upload(data, 0L);
		VkBuffer buffer = gpuComputeBuffer.Buffer;
		VkBuffer vkBuffer = allocation.VkBuffer;
		VkBufferCopy reference = new VkBufferCopy
		{
			SrcOffset = ByteSize.Zero,
			DstOffset = ByteSize.Of(byteOffset),
			Size = ByteSize.Of(data.Length)
		};
		commandBuffer.CopyBuffer(buffer, vkBuffer, new ReadOnlySpan<VkBufferCopy>(in reference));
		return gpuComputeBuffer;
	}

	public static void RecordUploadBarrier(CommandBuffer commandBuffer)
	{
		VkMemoryBarrier vkMemoryBarrier = new VkMemoryBarrier();
		vkMemoryBarrier.SrcAccessMask = VkAccessFlags.TransferWriteBit;
		vkMemoryBarrier.DstAccessMask = VkAccessFlags.ShaderReadBit;
		VkMemoryBarrier reference = vkMemoryBarrier;
		Span<VkMemoryBarrier> span = new Span<VkMemoryBarrier>(ref reference);
		commandBuffer.PipelineBarrier(VkPipelineStageFlags.TransferBit, VkPipelineStageFlags.VertexShaderBit | VkPipelineStageFlags.FragmentShaderBit, VkDependencyFlags.None, span, Array.Empty<VkBufferMemoryBarrier>(), default(Span<VkImageMemoryBarrier>));
	}

	public unsafe void Upload<T>(ReadOnlySpan<T> data, long byteOffset = 0L) where T : unmanaged
	{
		ReadOnlySpan<byte> readOnlySpan = MemoryMarshal.AsBytes(data);
		if (byteOffset < 0 || (ulong)(byteOffset + readOnlySpan.Length) > (ulong)allocation.BufferSize.Bytes)
		{
			throw new ArgumentOutOfRangeException("data", $"Upload at offset {byteOffset} with size {readOnlySpan.Length} exceeds compute buffer size {allocation.BufferSize.Bytes}.");
		}
		AllocationMapInfo allocationMapInfo = new AllocationMapInfo(allocation.AllocationInfo);
		allocationMapInfo.Region = new MapRegion
		{
			Offset = ByteSize.Zero,
			Size = allocation.BufferSize
		};
		AllocationMapInfo allocationMapInfo2 = allocationMapInfo;
		Ptr ptr = allocator.MapMemory(in allocationMapInfo2);
		try
		{
			readOnlySpan.CopyTo(new Span<byte>((void*)((nint)ptr + (nint)byteOffset), readOnlySpan.Length));
		}
		finally
		{
			allocator.UnmapMemory(in allocationMapInfo2);
		}
	}

	public unsafe void Read(Span<byte> dest, long byteOffset = 0L)
	{
		if (byteOffset < 0 || (ulong)(byteOffset + dest.Length) > (ulong)allocation.BufferSize.Bytes)
		{
			throw new ArgumentOutOfRangeException("dest", $"Read at offset {byteOffset} with size {dest.Length} exceeds compute buffer size {allocation.BufferSize.Bytes}.");
		}
		AllocationMapInfo allocationMapInfo = new AllocationMapInfo(allocation.AllocationInfo);
		allocationMapInfo.Region = new MapRegion
		{
			Offset = ByteSize.Zero,
			Size = allocation.BufferSize
		};
		AllocationMapInfo allocationMapInfo2 = allocationMapInfo;
		Ptr ptr = allocator.MapMemory(in allocationMapInfo2);
		try
		{
			new ReadOnlySpan<byte>((void*)((nint)ptr + (nint)byteOffset), dest.Length).CopyTo(dest);
		}
		finally
		{
			allocator.UnmapMemory(in allocationMapInfo2);
		}
	}

	public unsafe Mapping Map()
	{
		AllocationMapInfo allocationMapInfo = new AllocationMapInfo(allocation.AllocationInfo);
		allocationMapInfo.Region = new MapRegion
		{
			Offset = ByteSize.Zero,
			Size = allocation.BufferSize
		};
		AllocationMapInfo allocationMapInfo2 = allocationMapInfo;
		return new Mapping(allocator, allocationMapInfo2, (byte*)(nint)allocator.MapMemory(in allocationMapInfo2), (long)allocation.BufferSize.Bytes);
	}

	public unsafe void ReadRanges(byte[][] destinations, ReadOnlySpan<long> byteOffsets, int count)
	{
		AllocationMapInfo allocationMapInfo = new AllocationMapInfo(allocation.AllocationInfo);
		allocationMapInfo.Region = new MapRegion
		{
			Offset = ByteSize.Zero,
			Size = allocation.BufferSize
		};
		AllocationMapInfo allocationMapInfo2 = allocationMapInfo;
		Ptr ptr = allocator.MapMemory(in allocationMapInfo2);
		try
		{
			for (int i = 0; i < count; i++)
			{
				byte[] array = destinations[i];
				long num = byteOffsets[i];
				if (num < 0 || (ulong)(num + array.Length) > (ulong)allocation.BufferSize.Bytes)
				{
					throw new ArgumentOutOfRangeException("byteOffsets", $"Read at offset {num} with size {array.Length} exceeds compute buffer size {allocation.BufferSize.Bytes}.");
				}
				new ReadOnlySpan<byte>((void*)((nint)ptr + (nint)num), array.Length).CopyTo(array);
			}
		}
		finally
		{
			allocator.UnmapMemory(in allocationMapInfo2);
		}
	}

	public void Dispose()
	{
		if (!disposed)
		{
			disposed = true;
			ComputeBufferResourceManager.DeferDispose(this);
		}
	}

	public void DisposeNow()
	{
		if (!disposed)
		{
			disposed = true;
			FreeAllocationNow();
		}
	}

	internal void FreeAllocationNow()
	{
		if (!allocation.VkBuffer.IsNull())
		{
			GpuMemoryTracker.Remove(GpuMemoryCategory.ComputeBuffer, in allocation);
			allocator.FreeAllocation(in allocation);
			allocation = default(BufferAllocationInfo);
		}
	}
}
