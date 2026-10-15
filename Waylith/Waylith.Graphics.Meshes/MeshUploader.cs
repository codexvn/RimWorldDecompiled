using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Brutal;
using Brutal.Logging;
using Brutal.Logging.Internal;
using Brutal.Pointers;
using Brutal.VulkanApi;
using Brutal.VulkanApi.Abstractions;
using Brutal.VulkanApi.Vma;

namespace Waylith.Graphics.Meshes;

public sealed class MeshUploader : IGpuMeshResourceOwner
{
	private sealed class UploadCommandBufferLease : IDisposable
	{
		private readonly DeviceEx _device;

		private readonly VkCommandPool _commandPool;

		private bool _submitted;

		public CommandBuffer CommandBuffer { get; }

		public UploadCommandBufferLease(DeviceEx device, VkCommandPool commandPool, CommandBuffer commandBuffer)
		{
			_device = device;
			_commandPool = commandPool;
			CommandBuffer = commandBuffer;
		}

		public void MarkSubmitted()
		{
			_submitted = true;
		}

		public void Dispose()
		{
			if (!_submitted && !CommandBuffer.Handle.IsNull())
			{
				DeviceEx device = _device;
				VkCommandPool commandPool = _commandPool;
				CommandBuffer reference = CommandBuffer;
				device.FreeCommandBuffers(commandPool, new ReadOnlySpan<CommandBuffer>(in reference));
			}
		}
	}

	private sealed class GpuResourceRetirementQueue
	{
		private readonly List<GpuResourceRetirement> _retirements = new List<GpuResourceRetirement>();

		public void Enqueue(long frameToRelease, GpuMeshBuffer[] vertexStreams, GpuMeshBuffer? indexBuffer)
		{
			_retirements.Add(new GpuResourceRetirement(frameToRelease, vertexStreams, indexBuffer));
		}

		public void Flush(long currentFrame, Action<GpuMeshBuffer[], GpuMeshBuffer?> release)
		{
			for (int num = _retirements.Count - 1; num >= 0; num--)
			{
				if (_retirements[num].FrameToRelease <= currentFrame)
				{
					release(_retirements[num].VertexStreams, _retirements[num].IndexBuffer);
					_retirements.RemoveAt(num);
				}
			}
		}
	}

	private readonly record struct GpuResourceRetirement(long FrameToRelease, GpuMeshBuffer[] VertexStreams, GpuMeshBuffer? IndexBuffer);

	private sealed class UploadRetirementQueue
	{
		private readonly List<UploadRetirement> _retirements = new List<UploadRetirement>();

		public void Enqueue(VkFence fence, CommandBuffer commandBuffer, BufferAllocationInfo[] stagingAllocations)
		{
			_retirements.Add(new UploadRetirement(fence, commandBuffer, stagingAllocations));
		}

		public void FlushCompleted(DeviceEx device, IBufferAllocator allocator, VkCommandPool commandPool, bool wait)
		{
			for (int num = _retirements.Count - 1; num >= 0; num--)
			{
				UploadRetirement uploadRetirement = _retirements[num];
				if (wait || IsFenceSignaled(device, uploadRetirement.Fence))
				{
					if (wait)
					{
						device.WaitForFence(uploadRetirement.Fence, -1);
					}
					for (int i = 0; i < uploadRetirement.StagingAllocations.Length; i++)
					{
						GpuMemoryTracker.Remove(GpuMemoryCategory.Staging, in uploadRetirement.StagingAllocations[i]);
						allocator.FreeAllocation(in uploadRetirement.StagingAllocations[i]);
					}
					CommandBuffer reference = uploadRetirement.CommandBuffer;
					device.FreeCommandBuffers(commandPool, new ReadOnlySpan<CommandBuffer>(in reference));
					device.DestroyFence(uploadRetirement.Fence, device.VkAllocator);
					_retirements.RemoveAt(num);
				}
			}
		}

		private static bool IsFenceSignaled(DeviceEx device, VkFence fence)
		{
			VkResult fenceStatus = device.GetFenceStatus(fence);
			return fenceStatus switch
			{
				VkResult.Success => true, 
				VkResult.NotReady => false, 
				_ => throw new InvalidOperationException($"Unexpected upload fence status {fenceStatus}."), 
			};
		}
	}

	private readonly record struct UploadRetirement(VkFence Fence, CommandBuffer CommandBuffer, BufferAllocationInfo[] StagingAllocations);

	private readonly object sync = new object();

	private readonly GraphicsDevice graphicsDevice;

	private readonly DeviceEx device;

	private readonly VmaAllocator allocator;

	private readonly VkCommandPool uploadCommandPool;

	private readonly HashSet<GpuMeshResource> trackedResources = new HashSet<GpuMeshResource>();

	private readonly HashSet<Mesh> pendingMeshSet = new HashSet<Mesh>();

	private readonly Queue<Mesh> pendingMeshes = new Queue<Mesh>();

	private readonly GpuResourceRetirementQueue retirementQueue = new GpuResourceRetirementQueue();

	private readonly UploadRetirementQueue uploadRetirementQueue = new UploadRetirementQueue();

	private long renderFrameSerial;

	private readonly int deferredReleaseFrames;

	private int uploadDeferralCount;

	private long hostFallbackUntilFrame = -1L;

	private const int HostFallbackFrames = 600;

	private static int hostFallbackLogged;

	private static readonly LogCategory Logger = LogCategory.Make("MeshUploader");

	public bool UploadsDeferred => Volatile.Read(in uploadDeferralCount) > 0;

	public MeshUploader(GraphicsDevice graphicsDevice, int framesInFlight = 2)
	{
		deferredReleaseFrames = framesInFlight * 2 + 2;
		this.graphicsDevice = graphicsDevice;
		DeviceEx deviceEx = graphicsDevice.Device;
		Queue graphicsQueue = graphicsDevice.GraphicsQueue;
		device = deviceEx;
		allocator = graphicsDevice.Allocator;
		VkCommandPoolCreateInfo pCreateInfo = new VkCommandPoolCreateInfo
		{
			QueueFamilyIndex = graphicsQueue.Family,
			Flags = VkCommandPoolCreateFlags.ResetCommandBufferBit
		};
		uploadCommandPool = deviceEx.CreateCommandPool(in pCreateInfo, deviceEx.VkAllocator);
	}

	public void BeginFrame(int frameIndex)
	{
		lock (sync)
		{
			uploadRetirementQueue.FlushCompleted(device, allocator, uploadCommandPool, wait: false);
		}
	}

	public void BeginRenderFrame()
	{
		lock (sync)
		{
			renderFrameSerial++;
			retirementQueue.Flush(renderFrameSerial, ReleaseBuffers);
		}
	}

	public void Enqueue(Mesh mesh)
	{
		ArgumentNullException.ThrowIfNull(mesh, "mesh");
		lock (sync)
		{
			if (pendingMeshSet.Add(mesh))
			{
				pendingMeshes.Enqueue(mesh);
			}
		}
	}

	public void Upload(Mesh mesh)
	{
		lock (sync)
		{
			UploadCore(mesh);
		}
	}

	public void BeginUploadDeferral()
	{
		Interlocked.Increment(ref uploadDeferralCount);
	}

	public void EndUploadDeferral()
	{
		Interlocked.Decrement(ref uploadDeferralCount);
	}

	public void DrainPendingUploads()
	{
		if (Volatile.Read(in uploadDeferralCount) > 0)
		{
			return;
		}
		lock (sync)
		{
			Mesh[] array = DrainPendingMeshQueue();
			if (array.Length != 0)
			{
				UploadBatch(array);
			}
		}
	}

	public void Release(Mesh? mesh)
	{
		if (mesh == null)
		{
			return;
		}
		lock (sync)
		{
			if (pendingMeshSet.Remove(mesh) && pendingMeshes.Count > 2 * pendingMeshSet.Count + 64)
			{
				DropReleasedFromQueue();
			}
			ReleaseCore(mesh.GpuResource);
		}
	}

	private void DropReleasedFromQueue()
	{
		for (int num = pendingMeshes.Count; num > 0; num--)
		{
			Mesh item = pendingMeshes.Dequeue();
			if (pendingMeshSet.Contains(item))
			{
				pendingMeshes.Enqueue(item);
			}
		}
	}

	public void Release(GpuMeshResource resource)
	{
		lock (sync)
		{
			ReleaseCore(resource);
		}
	}

	private void UploadCore(Mesh mesh)
	{
		ArgumentNullException.ThrowIfNull(mesh, "mesh");
		UploadBatch(new _003C_003Ez__ReadOnlySingleElementList<Mesh>(mesh));
	}

	private void UploadBatch(IReadOnlyList<Mesh> meshes)
	{
		int i = 0;
		while (i < meshes.Count)
		{
			UploadCommandBufferLease uploadCommandBufferLease = BeginUploadCommandBuffer();
			List<BufferAllocationInfo> list = new List<BufferAllocationInfo>();
			List<VkBufferMemoryBarrier> barriers = new List<VkBufferMemoryBarrier>();
			List<(Mesh, bool)> list2 = new List<(Mesh, bool)>();
			long num = 0L;
			try
			{
				for (; i < meshes.Count; i++)
				{
					Mesh mesh = meshes[i];
					if (mesh != null && !mesh.IsDisposed)
					{
						num += RecordMeshUpload(mesh, uploadCommandBufferLease.CommandBuffer, list, barriers, list2);
						if (num >= 134217728)
						{
							i++;
							break;
						}
					}
				}
			}
			catch
			{
				FreeAllocations(list);
				uploadCommandBufferLease.Dispose();
				foreach (var item2 in list2)
				{
					Mesh item = item2.Item1;
					if (!item.IsDisposed)
					{
						item.GpuResource.MarkDirty();
						Enqueue(item);
					}
				}
				throw;
			}
			if (num > 0)
			{
				RecordBarriers(uploadCommandBufferLease.CommandBuffer, barriers);
				SubmitUploadCommandBuffer(uploadCommandBufferLease, list);
				foreach (var (mesh2, discardCpuCopy) in list2)
				{
					mesh2.OnUploadSucceeded(discardCpuCopy);
				}
			}
			else
			{
				FreeAllocations(list);
				uploadCommandBufferLease.Dispose();
			}
		}
	}

	private long RecordMeshUpload(Mesh mesh, CommandBuffer commandBuffer, List<BufferAllocationInfo> stagingAllocations, List<VkBufferMemoryBarrier> barriers, List<(Mesh mesh, bool discardCpuCopy)> stagedMeshes)
	{
		if (mesh.IsDisposed)
		{
			return 0L;
		}
		if (!mesh.GpuResource.NeedsUpload(mesh.Version))
		{
			return 0L;
		}
		if (mesh.Residency == MeshResidency.CpuOnly || mesh.VertexCount == 0)
		{
			bool discardCpuCopy = mesh.GpuResource.RequestedDiscardCpuCopy || mesh.Residency == MeshResidency.StaticUploaded;
			ReleaseCore(mesh.GpuResource);
			mesh.GpuResource.SetBuffers(Array.Empty<GpuMeshBuffer>(), null, mesh.CpuStorage.IndexFormat, mesh.CpuStorage.Layout, mesh.CpuStorage.SubMeshes.ToArray(), mesh.CpuStorage.Bounds, mesh.Version);
			mesh.OnUploadSucceeded(discardCpuCopy);
			return 0L;
		}
		GpuMeshBuffer[] array = new GpuMeshBuffer[mesh.CpuStorage.Layout.StreamCount];
		GpuMeshBuffer indexBuffer = null;
		bool flag = mesh.GpuResource.RequestedDiscardCpuCopy || mesh.Residency == MeshResidency.StaticUploaded;
		long num = 0L;
		try
		{
			if (mesh.Residency == MeshResidency.DynamicMapped)
			{
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = CreateMappedBuffer(GetVertexBufferLabel(mesh, i), mesh.CpuStorage.GetVertexStream(i), VkBufferUsageFlags.VertexBufferBit, TrackingLabel(mesh));
				}
				if (mesh.IndexCount > 0)
				{
					indexBuffer = CreateMappedBuffer(GetIndexBufferLabel(mesh), mesh.CpuStorage.GetIndexBuffer(), VkBufferUsageFlags.IndexBufferBit, TrackingLabel(mesh));
				}
			}
			else
			{
				for (int j = 0; j < array.Length; j++)
				{
					NativeByteBuffer vertexStream = mesh.CpuStorage.GetVertexStream(j);
					array[j] = CreateDeviceLocalBuffer(GetVertexBufferLabel(mesh, j), vertexStream, VkBufferUsageFlags.VertexBufferBit, commandBuffer, stagingAllocations, TrackingLabel(mesh));
					num += vertexStream.LengthBytes;
				}
				if (mesh.IndexCount > 0)
				{
					NativeByteBuffer indexBuffer2 = mesh.CpuStorage.GetIndexBuffer();
					indexBuffer = CreateDeviceLocalBuffer(GetIndexBufferLabel(mesh), indexBuffer2, VkBufferUsageFlags.IndexBufferBit, commandBuffer, stagingAllocations, TrackingLabel(mesh));
					num += indexBuffer2.LengthBytes;
				}
				CollectUploadCompletionBarriers(array, indexBuffer, barriers);
			}
			ReleaseCore(mesh.GpuResource);
			mesh.GpuResource.SetBuffers(array, indexBuffer, mesh.CpuStorage.IndexFormat, mesh.CpuStorage.Layout, mesh.CpuStorage.SubMeshes.ToArray(), mesh.CpuStorage.Bounds, mesh.Version);
			if (num > 0)
			{
				stagedMeshes.Add((mesh, flag));
			}
			else
			{
				mesh.OnUploadSucceeded(flag);
			}
			mesh.GpuResource.SetOwner(this);
			trackedResources.Add(mesh.GpuResource);
			return num;
		}
		catch (VmaAllocationException inner)
		{
			ReleaseBuffers(array, indexBuffer);
			throw VmaAllocationException.WithContext($"uploading mesh '{mesh.DebugName}' ({mesh.VertexCount} verts, {mesh.IndexCount} indices, {mesh.CpuStorage.Layout.StreamCount} streams, {mesh.Residency})", inner);
		}
		catch
		{
			ReleaseBuffers(array, indexBuffer);
			throw;
		}
	}

	private void ReleaseCore(GpuMeshResource resource)
	{
		if (resource != null)
		{
			EnqueueRelease(resource, renderFrameSerial);
			resource.ClearBuffers(-1L);
			resource.ClearOwner(this);
			trackedResources.Remove(resource);
		}
	}

	private static string TrackingLabel(Mesh mesh)
	{
		if (!(mesh.DebugName == "Mesh"))
		{
			return "Mesh/" + mesh.DebugName;
		}
		return "Mesh/Unnamed";
	}

	public void Rename(Mesh? mesh)
	{
		if (mesh != null)
		{
			Rename(mesh.GpuResource, mesh.DebugName);
		}
	}

	public void Shutdown()
	{
		lock (sync)
		{
			uploadRetirementQueue.FlushCompleted(device, allocator, uploadCommandPool, wait: true);
			retirementQueue.Flush(long.MaxValue, ReleaseBuffers);
			foreach (GpuMeshResource trackedResource in trackedResources)
			{
				ReadOnlySpan<GpuMeshBuffer> vertexStreams = trackedResource.VertexStreams;
				for (int i = 0; i < vertexStreams.Length; i++)
				{
					vertexStreams[i]?.Release(allocator);
				}
				trackedResource.IndexBuffer?.Release(allocator);
				trackedResource.ClearBuffers(-1L);
				trackedResource.ClearOwner(this);
			}
			trackedResources.Clear();
			device.DestroyCommandPool(uploadCommandPool, device.VkAllocator);
		}
	}

	private unsafe GpuMeshBuffer CreateMappedBuffer(string label, NativeByteBuffer source, VkBufferUsageFlags usage, string? trackingLabel = null)
	{
		if (source.LengthBytes == 0)
		{
			return new GpuMeshBuffer();
		}
		VmaAllocator vmaAllocator = allocator;
		VkBufferCreateInfo bufferCreateInfo = new VkBufferCreateInfo
		{
			Size = ByteSize.Of(source.LengthBytes),
			Usage = usage,
			SharingMode = VkSharingMode.Exclusive
		};
		AllocationInfo.CreateInfo allocateCreateInfo = new AllocationInfo.CreateInfo
		{
			Preference = MemoryPreference.PreferCpu,
			RequiredProperties = (VkMemoryPropertyFlags.HostVisibleBit | VkMemoryPropertyFlags.HostCoherentBit),
			PreferredProperties = (VkMemoryPropertyFlags.HostVisibleBit | VkMemoryPropertyFlags.HostCoherentBit),
			Priority = 1f
		};
		BufferAllocationInfo allocation = vmaAllocator.AllocateBufferChecked(label, in bufferCreateInfo, in allocateCreateInfo);
		AllocationMapInfo allocationMapInfo = new AllocationMapInfo(allocation.AllocationInfo);
		allocationMapInfo.Region = new MapRegion
		{
			Offset = ByteSize.Zero,
			Size = allocation.BufferSize
		};
		AllocationMapInfo allocationMapInfo2 = allocationMapInfo;
		Ptr ptr = allocator.MapMemory(in allocationMapInfo2);
		Buffer.MemoryCopy((void*)source.Pointer, ptr, source.LengthBytes, source.LengthBytes);
		GpuMeshBuffer gpuMeshBuffer = new GpuMeshBuffer();
		gpuMeshBuffer.Initialize(allocation, (uint)source.LengthBytes, usage, allocationMapInfo2, (nint)ptr, trackingLabel);
		return gpuMeshBuffer;
	}

	public void Rename(GpuMeshResource resource, string debugName)
	{
		lock (sync)
		{
			if (resource != null && resource.HasBuffers)
			{
				ReadOnlySpan<GpuMeshBuffer> vertexStreams = resource.VertexStreams;
				for (int i = 0; i < vertexStreams.Length; i++)
				{
					_ = vertexStreams[i]?.IsAllocated;
				}
				_ = resource.IndexBuffer?.IsAllocated;
			}
		}
	}

	private unsafe GpuMeshBuffer CreateDeviceLocalBuffer(string label, NativeByteBuffer source, VkBufferUsageFlags usage, CommandBuffer commandBuffer, List<BufferAllocationInfo> stagingAllocations, string? trackingLabel = null)
	{
		if (source.LengthBytes == 0)
		{
			return new GpuMeshBuffer();
		}
		bool hostResident;
		BufferAllocationInfo bufferAllocationInfo = AllocateMeshBuffer(label, source.LengthBytes, usage, out hostResident);
		if (hostResident)
		{
			trackingLabel = (trackingLabel ?? "Mesh") + "/Host";
		}
		BufferAllocationInfo allocation = allocator.AllocateBufferChecked(label + ".Staging", new VkBufferCreateInfo
		{
			Size = ByteSize.Of(source.LengthBytes),
			Usage = VkBufferUsageFlags.TransferSrcBit,
			SharingMode = VkSharingMode.Exclusive
		}, new AllocationInfo.CreateInfo
		{
			Preference = MemoryPreference.PreferCpu,
			RequiredProperties = (VkMemoryPropertyFlags.HostVisibleBit | VkMemoryPropertyFlags.HostCoherentBit),
			PreferredProperties = (VkMemoryPropertyFlags.HostVisibleBit | VkMemoryPropertyFlags.HostCoherentBit),
			Priority = 1f
		});
		GpuMemoryTracker.Add(GpuMemoryCategory.Staging, in allocation, "Staging/Mesh");
		AllocationMapInfo allocationMapInfo = new AllocationMapInfo(allocation.AllocationInfo);
		allocationMapInfo.Region = new MapRegion
		{
			Offset = ByteSize.Zero,
			Size = allocation.BufferSize
		};
		AllocationMapInfo allocationMapInfo2 = allocationMapInfo;
		Ptr ptr = allocator.MapMemory(in allocationMapInfo2);
		Buffer.MemoryCopy((void*)source.Pointer, ptr, source.LengthBytes, source.LengthBytes);
		allocator.UnmapMemory(in allocationMapInfo2);
		VkBuffer vkBuffer = allocation.VkBuffer;
		VkBuffer vkBuffer2 = bufferAllocationInfo.VkBuffer;
		VkBufferCopy reference = new VkBufferCopy
		{
			SrcOffset = ByteSize.Zero,
			DstOffset = ByteSize.Zero,
			Size = bufferAllocationInfo.BufferSize
		};
		commandBuffer.CopyBuffer(vkBuffer, vkBuffer2, new ReadOnlySpan<VkBufferCopy>(in reference));
		stagingAllocations.Add(allocation);
		GpuMeshBuffer gpuMeshBuffer = new GpuMeshBuffer();
		BufferAllocationInfo allocation2 = bufferAllocationInfo;
		int lengthBytes = source.LengthBytes;
		string trackingLabel2 = trackingLabel;
		gpuMeshBuffer.Initialize(allocation2, (uint)lengthBytes, usage, null, 0, trackingLabel2);
		return gpuMeshBuffer;
	}

	private BufferAllocationInfo AllocateMeshBuffer(string label, long bytes, VkBufferUsageFlags usage, out bool hostResident)
	{
		VkBufferCreateInfo vkBufferCreateInfo = new VkBufferCreateInfo();
		vkBufferCreateInfo.Size = ByteSize.Of(bytes);
		vkBufferCreateInfo.Usage = usage | VkBufferUsageFlags.TransferDstBit;
		vkBufferCreateInfo.SharingMode = VkSharingMode.Exclusive;
		VkBufferCreateInfo bufferCreateInfo = vkBufferCreateInfo;
		hostResident = renderFrameSerial < hostFallbackUntilFrame;
		if (!hostResident && hostFallbackUntilFrame >= 0 && !DeviceLocalHasRoom(bytes))
		{
			hostResident = true;
			hostFallbackUntilFrame = renderFrameSerial + 600;
		}
		if (!hostResident)
		{
			try
			{
				VmaAllocator vmaAllocator = allocator;
				AllocationInfo.CreateInfo allocateCreateInfo = new AllocationInfo.CreateInfo
				{
					Preference = MemoryPreference.PreferGpu,
					RequiredProperties = VkMemoryPropertyFlags.DeviceLocalBit,
					PreferredProperties = VkMemoryPropertyFlags.DeviceLocalBit,
					Priority = 1f
				};
				return vmaAllocator.AllocateBufferChecked(label, in bufferCreateInfo, in allocateCreateInfo, anyMemoryOnRefusal: false);
			}
			catch (VmaAllocationException ex) when (ex.Result == VkResult.ErrorOutOfDeviceMemory)
			{
				hostResident = true;
				hostFallbackUntilFrame = renderFrameSerial + 600;
				if (Interlocked.Exchange(ref hostFallbackLogged, 1) == 0)
				{
					LogCategory logger = Logger;
					bool shouldAppend;
					LogString<Warning> message = new LogString<Warning>(156, 4, logger, out shouldAppend);
					if (shouldAppend)
					{
						message.AppendLiteral("Device-local memory refused a ");
						message.AppendFormatted(GpuMemoryReport.FormatBytes(bytes));
						message.AppendLiteral(" mesh buffer, mesh uploads go to system memory for the next ");
						message.AppendFormatted(600);
						message.AppendLiteral(" frames after each refusal. This message will only be logged once.");
						message.AppendFormatted(Environment.NewLine);
						message.AppendFormatted(GpuMemoryReport.BuildCompactSummary().TrimEnd());
					}
					logger.Warning(message, "AllocateMeshBuffer", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Graphics\\Meshes\\MeshUploader.cs", 581);
				}
			}
		}
		return allocator.AllocateBufferChecked(label, in bufferCreateInfo, new AllocationInfo.CreateInfo
		{
			Preference = MemoryPreference.PreferCpu,
			RequiredProperties = (VkMemoryPropertyFlags.HostVisibleBit | VkMemoryPropertyFlags.HostCoherentBit),
			PreferredProperties = (VkMemoryPropertyFlags.HostVisibleBit | VkMemoryPropertyFlags.HostCoherentBit),
			Priority = 0.5f
		});
	}

	private static bool DeviceLocalHasRoom(long bytes)
	{
		if (GpuMemoryReport.TryGetSummary(out var summary))
		{
			return summary.DeviceLocalFreeBytes >= bytes + 67108864;
		}
		return true;
	}

	private static void CollectUploadCompletionBarriers(GpuMeshBuffer[] vertexStreams, GpuMeshBuffer? indexBuffer, List<VkBufferMemoryBarrier> barriers)
	{
		for (int i = 0; i < vertexStreams.Length; i++)
		{
			if (vertexStreams[i].IsAllocated)
			{
				barriers.Add(new VkBufferMemoryBarrier
				{
					SrcAccessMask = VkAccessFlags.TransferWriteBit,
					DstAccessMask = VkAccessFlags.VertexAttributeReadBit,
					SrcQueueFamilyIndex = -1,
					DstQueueFamilyIndex = -1,
					Buffer = vertexStreams[i].Buffer,
					Offset = ByteSize.Zero,
					Size = ByteSize.Of(vertexStreams[i].SizeBytes)
				});
			}
		}
		if (indexBuffer != null && indexBuffer.IsAllocated)
		{
			barriers.Add(new VkBufferMemoryBarrier
			{
				SrcAccessMask = VkAccessFlags.TransferWriteBit,
				DstAccessMask = VkAccessFlags.IndexReadBit,
				SrcQueueFamilyIndex = -1,
				DstQueueFamilyIndex = -1,
				Buffer = indexBuffer.Buffer,
				Offset = ByteSize.Zero,
				Size = ByteSize.Of(indexBuffer.SizeBytes)
			});
		}
	}

	private static void RecordBarriers(CommandBuffer commandBuffer, List<VkBufferMemoryBarrier> barriers)
	{
		if (barriers.Count != 0)
		{
			commandBuffer.PipelineBarrier(VkPipelineStageFlags.TransferBit, VkPipelineStageFlags.VertexInputBit, VkDependencyFlags.None, default(Span<VkMemoryBarrier>), barriers.ToArray(), default(Span<VkImageMemoryBarrier>));
		}
	}

	private void FreeAllocations(List<BufferAllocationInfo> allocations)
	{
		for (int i = 0; i < allocations.Count; i++)
		{
			GpuMemoryTracker.Remove(GpuMemoryCategory.Staging, allocations[i]);
			allocator.FreeAllocation(allocations[i]);
		}
		allocations.Clear();
	}

	private void ReleaseBuffers(GpuMeshBuffer[] vertexStreams, GpuMeshBuffer? indexBuffer)
	{
		for (int i = 0; i < vertexStreams.Length; i++)
		{
			vertexStreams[i]?.Release(allocator);
		}
		indexBuffer?.Release(allocator);
	}

	private UploadCommandBufferLease BeginUploadCommandBuffer()
	{
		CommandBuffer commandBuffer = device.AllocateCommandBuffer(new VkCommandBufferAllocateInfo
		{
			CommandPool = uploadCommandPool,
			Level = VkCommandBufferLevel.Primary
		});
		commandBuffer.Begin(VkCommandBufferUsageFlags.OneTimeSubmitBit);
		return new UploadCommandBufferLease(device, uploadCommandPool, commandBuffer);
	}

	private void SubmitUploadCommandBuffer(UploadCommandBufferLease lease, List<BufferAllocationInfo> stagingAllocations)
	{
		lease.CommandBuffer.End();
		VkFence fence = device.CreateFence(new VkFenceCreateInfo(), device.VkAllocator);
		try
		{
			GraphicsDevice obj = graphicsDevice;
			CommandBuffer reference = lease.CommandBuffer;
			obj.SubmitGraphics(default(Span<VkSemaphore>), default(Span<VkPipelineStageFlags>), new Span<CommandBuffer>(ref reference), default(Span<VkSemaphore>), fence);
			uploadRetirementQueue.Enqueue(fence, lease.CommandBuffer, stagingAllocations.ToArray());
			lease.MarkSubmitted();
			stagingAllocations.Clear();
		}
		catch
		{
			device.DestroyFence(fence, device.VkAllocator);
			throw;
		}
	}

	private static string GetVertexBufferLabel(Mesh mesh, int stream)
	{
		return $"{mesh.DebugName}.Vertex[{stream}]";
	}

	private static string GetIndexBufferLabel(Mesh mesh)
	{
		return mesh.DebugName + ".Index";
	}

	private Mesh[] DrainPendingMeshQueue()
	{
		if (pendingMeshes.Count == 0)
		{
			return Array.Empty<Mesh>();
		}
		Mesh[] array = new Mesh[pendingMeshes.Count];
		int num = 0;
		while (pendingMeshes.Count > 0)
		{
			Mesh mesh = pendingMeshes.Dequeue();
			if (pendingMeshSet.Remove(mesh))
			{
				array[num++] = mesh;
			}
		}
		if (num == array.Length)
		{
			return array;
		}
		Array.Resize(ref array, num);
		return array;
	}

	private void EnqueueRelease(GpuMeshResource resource, long currentFrame)
	{
		ReadOnlySpan<GpuMeshBuffer> vertexStreams = resource.VertexStreams;
		GpuMeshBuffer[] array = new GpuMeshBuffer[vertexStreams.Length];
		for (int i = 0; i < vertexStreams.Length; i++)
		{
			array[i] = vertexStreams[i];
		}
		GpuMeshBuffer indexBuffer = resource.IndexBuffer;
		if (array.Length != 0 || indexBuffer != null)
		{
			retirementQueue.Enqueue(currentFrame + deferredReleaseFrames, array, indexBuffer);
		}
	}
}
