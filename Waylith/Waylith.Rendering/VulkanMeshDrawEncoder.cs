using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;
using Brutal;
using Brutal.Logging;
using Brutal.Logging.Internal;
using Brutal.VulkanApi;
using Brutal.VulkanApi.Abstractions;
using Brutal.VulkanApi.Vma;
using Waylith.Graphics;
using Waylith.Graphics.Meshes;

namespace Waylith.Rendering;

internal sealed class VulkanMeshDrawEncoder : IDisposable
{
	private sealed class PerFrameState
	{
		private readonly List<VkDescriptorPool> descriptorPools;

		public readonly UploadBuffer UniformBuffer;

		public readonly UploadBuffer InstanceBuffer;

		public readonly UploadBuffer StorageBuffer;

		public readonly UploadBuffer IndirectBuffer;

		public VkPipeline CurrentPipeline;

		public VkDescriptorSet CurrentMaterialDescriptorSet;

		public VkDescriptorSet CurrentFrameDescriptorSet;

		public VkDescriptorSet CurrentGlobalTextureDescriptorSet;

		public MeshDrawGeometry CurrentMesh;

		public bool HasCurrentMesh;

		public VkBuffer CurrentFallbackBuffer;

		public int CurrentFallbackOffset = -1;

		public int CurrentFallbackBinding = -1;

		private int descriptorPoolIndex;

		public VkDescriptorPool CurrentDescriptorPool => descriptorPools[descriptorPoolIndex];

		public PerFrameState(List<VkDescriptorPool> descriptorPools, UploadBuffer uniformBuffer, UploadBuffer instanceBuffer, UploadBuffer storageBuffer, UploadBuffer indirectBuffer)
		{
			this.descriptorPools = descriptorPools;
			UniformBuffer = uniformBuffer;
			InstanceBuffer = instanceBuffer;
			StorageBuffer = storageBuffer;
			IndirectBuffer = indirectBuffer;
		}

		public bool TryAdvanceDescriptorPool()
		{
			if (descriptorPoolIndex + 1 >= descriptorPools.Count)
			{
				return false;
			}
			descriptorPoolIndex++;
			return true;
		}

		public void AddDescriptorPool(VkDescriptorPool descriptorPool)
		{
			descriptorPools.Add(descriptorPool);
			descriptorPoolIndex = descriptorPools.Count - 1;
		}

		public void Reset(DeviceEx device)
		{
			for (int i = 0; i < descriptorPools.Count; i++)
			{
				device.ResetDescriptorPool(descriptorPools[i], 0);
			}
			descriptorPoolIndex = 0;
			UniformBuffer.Reset();
			InstanceBuffer.Reset();
			StorageBuffer.Reset();
			IndirectBuffer.Reset();
			InvalidateBoundState();
		}

		public void InvalidateBoundState()
		{
			CurrentPipeline = default(VkPipeline);
			CurrentMaterialDescriptorSet = default(VkDescriptorSet);
			CurrentFrameDescriptorSet = default(VkDescriptorSet);
			CurrentGlobalTextureDescriptorSet = default(VkDescriptorSet);
			CurrentMesh = null;
			HasCurrentMesh = false;
			CurrentFallbackBuffer = default(VkBuffer);
			CurrentFallbackOffset = -1;
			CurrentFallbackBinding = -1;
		}

		public void Dispose(DeviceEx device)
		{
			UniformBuffer.Dispose();
			InstanceBuffer.Dispose();
			StorageBuffer.Dispose();
			IndirectBuffer.Dispose();
			for (int i = 0; i < descriptorPools.Count; i++)
			{
				device.DestroyDescriptorPool(descriptorPools[i], device.VkAllocator);
			}
		}
	}

	private sealed class UploadBuffer : IDisposable
	{
		private readonly VmaAllocator allocator;

		private readonly BufferAllocationInfo allocation;

		private readonly AllocationMapInfo mapInfo;

		private unsafe readonly byte* mappedPointer;

		private readonly int alignmentBytes;

		private readonly int capacityBytes;

		private int offsetBytes;

		public VkBuffer Buffer => allocation.VkBuffer;

		public unsafe UploadBuffer(DeviceEx device, VmaAllocator allocator, string label, int capacityBytes, int alignmentBytes, VkBufferUsageFlags usage)
		{
			this.allocator = allocator;
			this.capacityBytes = capacityBytes;
			this.alignmentBytes = System.Math.Max(alignmentBytes, 16);
			VkBufferCreateInfo bufferCreateInfo = new VkBufferCreateInfo
			{
				Size = ByteSize.Of(capacityBytes),
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
			allocation = allocator.AllocateBufferChecked(label, in bufferCreateInfo, in allocateCreateInfo);
			mapInfo = new AllocationMapInfo(allocation.AllocationInfo)
			{
				Region = new MapRegion
				{
					Offset = ByteSize.Zero,
					Size = allocation.BufferSize
				}
			};
			mappedPointer = (byte*)(void*)allocator.MapMemory(in mapInfo);
			GpuMemoryTracker.Add(GpuMemoryCategory.DynamicDraw, in allocation, null, label);
		}

		public void Reset()
		{
			offsetBytes = 0;
		}

		public unsafe bool TryAllocate(int sizeBytes, out void* pointer, out ByteSize64 offset)
		{
			if (!TryAllocateRaw(sizeBytes, out pointer, out var byteOffset))
			{
				offset = default(ByteSize64);
				return false;
			}
			offset = ByteSize.Of(byteOffset);
			return true;
		}

		public unsafe bool TryAllocateRaw(int sizeBytes, out void* pointer, out int byteOffset)
		{
			int num = Align(offsetBytes, alignmentBytes);
			if (sizeBytes <= 0 || num + sizeBytes > capacityBytes)
			{
				pointer = null;
				byteOffset = 0;
				return false;
			}
			pointer = mappedPointer + num;
			byteOffset = num;
			offsetBytes = num + sizeBytes;
			return true;
		}

		public void Dispose()
		{
			GpuMemoryTracker.Remove(GpuMemoryCategory.DynamicDraw, in allocation);
			allocator.UnmapMemory(in mapInfo);
			allocator.FreeAllocation(in allocation);
		}

		private static int Align(int value, int alignment)
		{
			int num = alignment - 1;
			return (value + num) & ~num;
		}
	}

	private const int DefaultDescriptorSetCapacity = 8192;

	private const int DefaultUniformBufferCapacity = 4194304;

	private const int DefaultInstanceBufferCapacity = 8388608;

	private const int DefaultStorageBufferCapacity = 16777216;

	private const int DefaultIndirectBufferCapacity = 65536;

	private const int FallbackBufferCapacity = 4096;

	private const int IndirectCommandBytes = 20;

	private readonly DeviceEx device;

	private readonly VmaAllocator allocator;

	private readonly int descriptorSetCapacity;

	private readonly int instanceBufferCapacity;

	private readonly int storageBufferCapacity;

	private readonly int indirectBufferCapacity;

	private readonly int topologyUniformAlignment;

	private readonly int storageBufferAlignment;

	private readonly int uniformBufferCapacity;

	private readonly List<UploadBuffer> fallbackBuffers = new List<UploadBuffer>();

	private UploadBuffer fallbackBufferCur;

	private int fallbackBytesUsed;

	private readonly Dictionary<ShaderVertexInputLayout, (VkBuffer buffer, int offset)> fallbackSlots = new Dictionary<ShaderVertexInputLayout, (VkBuffer, int)>(ReferenceEqualityComparer.Instance);

	private PerFrameState[] frameStates;

	private PerFrameState frameStateCur;

	private const int MaxVertexStreams = 8;

	private static readonly ByteSize64[] ZeroVertexOffsets = new ByteSize64[8];

	public VulkanMeshDrawEncoder(DeviceEx device, VmaAllocator allocator, int uniformBufferAlignment, int descriptorSetCapacity = 8192, int uniformBufferCapacity = 4194304, int instanceBufferCapacity = 8388608, int storageBufferAlignment = 256, int storageBufferCapacity = 16777216, int indirectBufferCapacity = 65536)
	{
		this.device = device;
		this.allocator = allocator;
		this.descriptorSetCapacity = System.Math.Max(1, descriptorSetCapacity);
		topologyUniformAlignment = System.Math.Max(uniformBufferAlignment, 16);
		this.storageBufferAlignment = System.Math.Max(storageBufferAlignment, 16);
		this.uniformBufferCapacity = uniformBufferCapacity;
		this.instanceBufferCapacity = instanceBufferCapacity;
		this.storageBufferCapacity = storageBufferCapacity;
		this.indirectBufferCapacity = indirectBufferCapacity;
		fallbackBufferCur = GrowFallbackBuffer(4096);
		frameStates = CreateFrameStates(1);
		frameStateCur = frameStates[0];
	}

	public void BeginFrame(in FrameContext frameContext)
	{
		EnsureFrameStateCount(System.Math.Max(frameContext.RenderContext.FramesInFlight, 1));
		frameStateCur = frameStates[GetFrameStateIndex(frameContext.SubmissionFrameIndex)];
		frameStateCur.Reset(device);
	}

	public void InvalidateBoundState()
	{
		frameStateCur.InvalidateBoundState();
	}

	public unsafe bool TryUploadUniforms<T>(in T value, out VkDescriptorBufferInfo bufferInfo) where T : unmanaged
	{
		if (!frameStateCur.UniformBuffer.TryAllocate(sizeof(T), out var pointer, out var offset))
		{
			bufferInfo = default(VkDescriptorBufferInfo);
			return false;
		}
		*(T*)pointer = value;
		bufferInfo = new VkDescriptorBufferInfo
		{
			Buffer = frameStateCur.UniformBuffer.Buffer,
			Offset = offset,
			Range = ByteSize.Of(sizeof(T))
		};
		return true;
	}

	public VkDescriptorSet AllocateDescriptorSet(VkDescriptorSetLayout descriptorSetLayout)
	{
		while (true)
		{
			try
			{
				return DescriptorSetWriter.AllocateDescriptorSet(device, frameStateCur.CurrentDescriptorPool, descriptorSetLayout);
			}
			catch (VkResultError vkResultError) when (vkResultError.Result == VkResult.ErrorOutOfPoolMemory || vkResultError.Result == VkResult.ErrorFragmentedPool)
			{
				if (!frameStateCur.TryAdvanceDescriptorPool())
				{
					frameStateCur.AddDescriptorPool(CreateDescriptorPool(descriptorSetCapacity));
				}
			}
		}
	}

	public void SubmitDraw(in FrameContext frameContext, VkPipeline pipeline, VkPipelineLayout pipelineLayout, ShaderVertexInputLayout vertexInputLayout, MeshDrawGeometry meshDraw, VkDescriptorSet frameDescriptorSet, VkDescriptorSet globalTextureDescriptorSet, VkDescriptorSet materialDescriptorSet, in Matrix4x4 matrix)
	{
		if (!pipeline.IsNull() && meshDraw.IndexCount > 0 && BindMeshAndFallback(frameContext.CommandBuffer, frameStateCur, vertexInputLayout, meshDraw))
		{
			BindPipelineAndSets(in frameContext, pipeline, pipelineLayout, frameDescriptorSet, globalTextureDescriptorSet, materialDescriptorSet);
			frameContext.CommandBuffer.PushConstants(pipelineLayout, VkShaderStageFlags.VertexBit, ByteSize.Zero, matrix);
			frameContext.CommandBuffer.DrawIndexed(meshDraw.IndexCount, 1, meshDraw.FirstIndex, meshDraw.BaseVertex, 0);
		}
	}

	public void SubmitDraw(in FrameContext frameContext, VkPipeline pipeline, VkPipelineLayout pipelineLayout, ShaderVertexInputLayout vertexInputLayout, MeshDrawGeometry meshDraw, VkDescriptorSet frameDescriptorSet, VkDescriptorSet globalTextureDescriptorSet, VkDescriptorSet materialDescriptorSet, ReadOnlySpan<Matrix4x4> instances)
	{
		if (!pipeline.IsNull() && instances.Length != 0 && meshDraw.IndexCount > 0 && BindMeshAndInstances(frameContext.CommandBuffer, frameStateCur, vertexInputLayout, meshDraw, instances, out var firstInstance))
		{
			BindPipelineAndSets(in frameContext, pipeline, pipelineLayout, frameDescriptorSet, globalTextureDescriptorSet, materialDescriptorSet);
			frameContext.CommandBuffer.PushConstants(pipelineLayout, VkShaderStageFlags.VertexBit, ByteSize.Zero, Matrix4x4.Identity);
			frameContext.CommandBuffer.DrawIndexed(meshDraw.IndexCount, instances.Length, meshDraw.FirstIndex, meshDraw.BaseVertex, firstInstance);
		}
	}

	public unsafe bool TryUploadStorage(ReadOnlySpan<byte> bytes, out VkDescriptorBufferInfo bufferInfo)
	{
		if (bytes.IsEmpty || !frameStateCur.StorageBuffer.TryAllocate(bytes.Length, out var pointer, out var offset))
		{
			bufferInfo = default(VkDescriptorBufferInfo);
			return false;
		}
		fixed (byte* source = bytes)
		{
			Buffer.MemoryCopy(source, pointer, bytes.Length, bytes.Length);
		}
		bufferInfo = new VkDescriptorBufferInfo
		{
			Buffer = frameStateCur.StorageBuffer.Buffer,
			Offset = offset,
			Range = ByteSize.Of(bytes.Length)
		};
		return true;
	}

	public unsafe bool TryUploadIndirectArgs(ReadOnlySpan<byte> args, int argsOffset, out VkBuffer buffer, out ByteSize64 offset)
	{
		buffer = default(VkBuffer);
		offset = default(ByteSize64);
		if ((uint)argsOffset > (uint)args.Length || args.Length - argsOffset < 20)
		{
			return false;
		}
		args = args.Slice(argsOffset, 20);
		if (!frameStateCur.IndirectBuffer.TryAllocate(20, out var pointer, out offset))
		{
			return false;
		}
		fixed (byte* source = args)
		{
			Buffer.MemoryCopy(source, pointer, 20L, 20L);
		}
		buffer = frameStateCur.IndirectBuffer.Buffer;
		return true;
	}

	public void SubmitDrawInstancedDirect(in FrameContext frameContext, VkPipeline pipeline, VkPipelineLayout pipelineLayout, ShaderVertexInputLayout vertexInputLayout, MeshDrawGeometry meshDraw, VkDescriptorSet frameDescriptorSet, VkDescriptorSet globalTextureDescriptorSet, VkDescriptorSet materialDescriptorSet, in Matrix4x4 matrix, int instanceCount)
	{
		if (!pipeline.IsNull() && instanceCount > 0 && meshDraw.IndexCount > 0 && BindMeshAndInstancedFallback(frameContext.CommandBuffer, frameStateCur, vertexInputLayout, meshDraw, instanceCount))
		{
			BindPipelineAndSets(in frameContext, pipeline, pipelineLayout, frameDescriptorSet, globalTextureDescriptorSet, materialDescriptorSet);
			frameContext.CommandBuffer.PushConstants(pipelineLayout, VkShaderStageFlags.VertexBit, ByteSize.Zero, matrix);
			frameContext.CommandBuffer.DrawIndexed(meshDraw.IndexCount, instanceCount, meshDraw.FirstIndex, meshDraw.BaseVertex, 0);
		}
	}

	public void SubmitDrawInstancedDirectRanges(in FrameContext frameContext, VkPipeline pipeline, VkPipelineLayout pipelineLayout, ShaderVertexInputLayout vertexInputLayout, MeshDrawGeometry meshDraw, VkDescriptorSet frameDescriptorSet, VkDescriptorSet globalTextureDescriptorSet, VkDescriptorSet materialDescriptorSet, in Matrix4x4 matrix, ReadOnlySpan<InstanceRange> ranges)
	{
		if (pipeline.IsNull() || ranges.IsEmpty || meshDraw.IndexCount <= 0 || !BindMeshAndFallback(frameContext.CommandBuffer, frameStateCur, vertexInputLayout, meshDraw))
		{
			return;
		}
		BindPipelineAndSets(in frameContext, pipeline, pipelineLayout, frameDescriptorSet, globalTextureDescriptorSet, materialDescriptorSet);
		frameContext.CommandBuffer.PushConstants(pipelineLayout, VkShaderStageFlags.VertexBit, ByteSize.Zero, matrix);
		ReadOnlySpan<InstanceRange> readOnlySpan = ranges;
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			InstanceRange instanceRange = readOnlySpan[i];
			if (instanceRange.InstanceCount > 0)
			{
				frameContext.CommandBuffer.DrawIndexed(meshDraw.IndexCount, instanceRange.InstanceCount, meshDraw.FirstIndex, meshDraw.BaseVertex, instanceRange.FirstInstance);
			}
		}
	}

	public void SubmitDrawInstancedIndirect(in FrameContext frameContext, VkPipeline pipeline, VkPipelineLayout pipelineLayout, ShaderVertexInputLayout vertexInputLayout, MeshDrawGeometry meshDraw, VkDescriptorSet frameDescriptorSet, VkDescriptorSet globalTextureDescriptorSet, VkDescriptorSet materialDescriptorSet, in Matrix4x4 matrix, VkBuffer argsBuffer, ByteSize64 argsOffset)
	{
		if (!pipeline.IsNull() && !argsBuffer.IsNull() && meshDraw.IndexCount > 0 && BindMeshAndFallback(frameContext.CommandBuffer, frameStateCur, vertexInputLayout, meshDraw))
		{
			BindPipelineAndSets(in frameContext, pipeline, pipelineLayout, frameDescriptorSet, globalTextureDescriptorSet, materialDescriptorSet);
			frameContext.CommandBuffer.PushConstants(pipelineLayout, VkShaderStageFlags.VertexBit, ByteSize.Zero, matrix);
			frameContext.CommandBuffer.DrawIndexedIndirect(argsBuffer, argsOffset, 1, ByteSize.Of(20));
		}
	}

	private unsafe void BindPipelineAndSets(in FrameContext frameContext, VkPipeline pipeline, VkPipelineLayout pipelineLayout, VkDescriptorSet frameDescriptorSet, VkDescriptorSet globalTextureDescriptorSet, VkDescriptorSet materialDescriptorSet)
	{
		int num;
		if (pipeline.VkHandle == frameStateCur.CurrentPipeline.VkHandle)
		{
			num = ((pipeline.VkObjectType != frameStateCur.CurrentPipeline.VkObjectType) ? 1 : 0);
			if (num == 0)
			{
				goto IL_0065;
			}
		}
		else
		{
			num = 1;
		}
		frameContext.CommandBuffer.BindPipeline(VkPipelineBindPoint.Graphics, pipeline);
		frameStateCur.CurrentPipeline = pipeline;
		frameStateCur.CurrentMaterialDescriptorSet = default(VkDescriptorSet);
		goto IL_0065;
		IL_0065:
		if (num != 0 || frameDescriptorSet.VkHandle != frameStateCur.CurrentFrameDescriptorSet.VkHandle || frameDescriptorSet.VkObjectType != frameStateCur.CurrentFrameDescriptorSet.VkObjectType || globalTextureDescriptorSet.VkHandle != frameStateCur.CurrentGlobalTextureDescriptorSet.VkHandle || globalTextureDescriptorSet.VkObjectType != frameStateCur.CurrentGlobalTextureDescriptorSet.VkObjectType)
		{
			VkDescriptorSet* ptr = stackalloc VkDescriptorSet[2];
			*ptr = frameDescriptorSet;
			ptr[1] = globalTextureDescriptorSet;
			int length = (globalTextureDescriptorSet.IsNull() ? 1 : 2);
			frameContext.CommandBuffer.BindDescriptorSets(VkPipelineBindPoint.Graphics, pipelineLayout, 0, new ReadOnlySpan<VkDescriptorSet>(ptr, length), default(Span<ByteSize32>));
			frameStateCur.CurrentFrameDescriptorSet = frameDescriptorSet;
			frameStateCur.CurrentGlobalTextureDescriptorSet = globalTextureDescriptorSet;
		}
		if (materialDescriptorSet.VkHandle != frameStateCur.CurrentMaterialDescriptorSet.VkHandle || materialDescriptorSet.VkObjectType != frameStateCur.CurrentMaterialDescriptorSet.VkObjectType)
		{
			VkDescriptorSet* ptr2 = stackalloc VkDescriptorSet[1];
			*ptr2 = materialDescriptorSet;
			frameContext.CommandBuffer.BindDescriptorSets(VkPipelineBindPoint.Graphics, pipelineLayout, 2, new ReadOnlySpan<VkDescriptorSet>(ptr2, 1), default(Span<ByteSize32>));
			frameStateCur.CurrentMaterialDescriptorSet = materialDescriptorSet;
		}
	}

	private unsafe bool BindMeshAndInstancedFallback(CommandBuffer commandBuffer, PerFrameState state, ShaderVertexInputLayout vertexInputLayout, MeshDrawGeometry meshDraw, int instanceCount)
	{
		if (meshDraw.VertexStreams.Length != meshDraw.Layout.StreamCount)
		{
			return false;
		}
		int num = System.Math.Max(1, instanceCount);
		int sizeBytes = num * OptionalVertexInputDefaults.GetStrideBytes(vertexInputLayout);
		if (!state.InstanceBuffer.TryAllocateRaw(sizeBytes, out var pointer, out var byteOffset))
		{
			return false;
		}
		OptionalVertexInputDefaults.Fill(pointer, num, vertexInputLayout);
		if (!state.HasCurrentMesh || !SameBoundMesh(state.CurrentMesh, meshDraw))
		{
			BindMesh(commandBuffer, meshDraw);
			state.CurrentMesh = meshDraw;
			state.HasCurrentMesh = true;
		}
		BindVertexBuffer(commandBuffer, state.InstanceBuffer.Buffer, MeshPipelineCache.GetOptionalFallbackBinding(meshDraw.Layout), byteOffset);
		state.CurrentFallbackBuffer = default(VkBuffer);
		state.CurrentFallbackOffset = -1;
		state.CurrentFallbackBinding = -1;
		return true;
	}

	public void Dispose()
	{
		for (int i = 0; i < frameStates.Length; i++)
		{
			frameStates[i].Dispose(device);
		}
		for (int j = 0; j < fallbackBuffers.Count; j++)
		{
			fallbackBuffers[j].Dispose();
		}
	}

	private bool BindMeshAndFallback(CommandBuffer commandBuffer, PerFrameState state, ShaderVertexInputLayout vertexInputLayout, MeshDrawGeometry meshDraw)
	{
		if (meshDraw.VertexStreams.Length != meshDraw.Layout.StreamCount)
		{
			return false;
		}
		(VkBuffer buffer, int offset) orCreateFallbackSlot = GetOrCreateFallbackSlot(vertexInputLayout);
		VkBuffer item = orCreateFallbackSlot.buffer;
		int item2 = orCreateFallbackSlot.offset;
		int optionalFallbackBinding = MeshPipelineCache.GetOptionalFallbackBinding(meshDraw.Layout);
		if (!state.HasCurrentMesh || !SameBoundMesh(state.CurrentMesh, meshDraw))
		{
			BindMesh(commandBuffer, meshDraw);
			state.CurrentMesh = meshDraw;
			state.HasCurrentMesh = true;
		}
		if (item.VkHandle != state.CurrentFallbackBuffer.VkHandle || item2 != state.CurrentFallbackOffset || optionalFallbackBinding != state.CurrentFallbackBinding)
		{
			BindVertexBuffer(commandBuffer, item, optionalFallbackBinding, item2);
			state.CurrentFallbackBuffer = item;
			state.CurrentFallbackOffset = item2;
			state.CurrentFallbackBinding = optionalFallbackBinding;
		}
		return true;
	}

	private unsafe (VkBuffer buffer, int offset) GetOrCreateFallbackSlot(ShaderVertexInputLayout vertexInputLayout)
	{
		if (fallbackSlots.TryGetValue(vertexInputLayout, out (VkBuffer, int) value))
		{
			return value;
		}
		int strideBytes = OptionalVertexInputDefaults.GetStrideBytes(vertexInputLayout);
		if (!fallbackBufferCur.TryAllocateRaw(strideBytes, out var pointer, out var byteOffset))
		{
			int num = System.Math.Max(4096, strideBytes);
			fallbackBufferCur = GrowFallbackBuffer(num);
			LogCategory log = RenderLog.Log;
			bool shouldAppend;
			LogString<Information> message = new LogString<Information>(89, 3, log, out shouldAppend);
			if (shouldAppend)
			{
				message.AppendLiteral("VulkanMeshDrawEncoder: fallback vertex store grew to ");
				message.AppendFormatted(fallbackBuffers.Count);
				message.AppendLiteral(" blocks (");
				message.AppendFormatted(fallbackSlots.Count + 1);
				message.AppendLiteral(" distinct layouts, ");
				message.AppendFormatted(fallbackBytesUsed + strideBytes);
				message.AppendLiteral(" bytes).");
			}
			log.Info(message, "GetOrCreateFallbackSlot", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Rendering\\DrawCallCore\\VulkanMeshDrawEncoder.cs", 369);
			if (!fallbackBufferCur.TryAllocateRaw(strideBytes, out pointer, out byteOffset))
			{
				throw new InvalidOperationException($"Fallback vertex slot ({strideBytes} bytes) does not fit a fresh {num}-byte block.");
			}
		}
		OptionalVertexInputDefaults.Fill(pointer, 1, vertexInputLayout);
		(VkBuffer, int) tuple = (fallbackBufferCur.Buffer, byteOffset);
		fallbackSlots.Add(vertexInputLayout, tuple);
		fallbackBytesUsed += strideBytes;
		return tuple;
	}

	private UploadBuffer GrowFallbackBuffer(int capacityBytes)
	{
		UploadBuffer uploadBuffer = new UploadBuffer(device, allocator, $"DrawCallBuilder.Fallback[{fallbackBuffers.Count}]", capacityBytes, 16, VkBufferUsageFlags.VertexBufferBit);
		fallbackBuffers.Add(uploadBuffer);
		return uploadBuffer;
	}

	private unsafe bool BindMeshAndInstances(CommandBuffer commandBuffer, PerFrameState state, ShaderVertexInputLayout vertexInputLayout, MeshDrawGeometry meshDraw, ReadOnlySpan<Matrix4x4> instances, out int firstInstance)
	{
		firstInstance = 0;
		if (meshDraw.VertexStreams.Length != meshDraw.Layout.StreamCount)
		{
			return false;
		}
		int sizeBytes = instances.Length * OptionalVertexInputDefaults.GetStrideBytes(vertexInputLayout);
		if (!state.InstanceBuffer.TryAllocateRaw(sizeBytes, out var pointer, out var byteOffset))
		{
			return false;
		}
		OptionalVertexInputDefaults.Fill(pointer, instances.Length, vertexInputLayout);
		int num = instances.Length * sizeof(Matrix4x4);
		if (!state.InstanceBuffer.TryAllocateRaw(num, out var pointer2, out var byteOffset2))
		{
			return false;
		}
		fixed (Matrix4x4* source = instances)
		{
			Buffer.MemoryCopy(source, pointer2, num, num);
		}
		firstInstance = 0;
		if (!state.HasCurrentMesh || !SameBoundMesh(state.CurrentMesh, meshDraw))
		{
			BindMesh(commandBuffer, meshDraw);
			state.CurrentMesh = meshDraw;
			state.HasCurrentMesh = true;
		}
		BindVertexBuffer(commandBuffer, state.InstanceBuffer.Buffer, MeshPipelineCache.GetOptionalFallbackBinding(meshDraw.Layout), byteOffset);
		BindVertexBuffer(commandBuffer, state.InstanceBuffer.Buffer, MeshPipelineCache.GetInstanceBinding(meshDraw.Layout), byteOffset2);
		return true;
	}

	private unsafe static void BindMesh(CommandBuffer commandBuffer, MeshDrawGeometry meshDraw)
	{
		GpuMeshBuffer[] vertexStreams = meshDraw.VertexStreams;
		VkBuffer* ptr = stackalloc VkBuffer[8];
		for (int i = 0; i < vertexStreams.Length; i++)
		{
			ptr[i] = vertexStreams[i].Buffer;
		}
		commandBuffer.BindVertexBuffers(0, new ReadOnlySpan<VkBuffer>(ptr, vertexStreams.Length), new ReadOnlySpan<ByteSize64>(ZeroVertexOffsets, 0, vertexStreams.Length));
		VkDeviceExtensions.BindIndexBuffer(commandBuffer, meshDraw.IndexBuffer, ByteSize.Zero, meshDraw.IndexType);
	}

	private unsafe static void BindVertexBuffer(CommandBuffer commandBuffer, VkBuffer buffer, int binding, int offset)
	{
		VkBuffer* ptr = stackalloc VkBuffer[1];
		ByteSize64* ptr2 = stackalloc ByteSize64[1];
		*ptr = buffer;
		*ptr2 = ByteSize.Of(offset);
		commandBuffer.BindVertexBuffers(binding, new ReadOnlySpan<VkBuffer>(ptr, 1), new ReadOnlySpan<ByteSize64>(ptr2, 1));
	}

	private static bool SameBoundMesh(MeshDrawGeometry left, MeshDrawGeometry right)
	{
		if (left.IndexBuffer.VkHandle != right.IndexBuffer.VkHandle || left.IndexBuffer.VkObjectType != right.IndexBuffer.VkObjectType || left.IndexType != right.IndexType || left.VertexStreams.Length != right.VertexStreams.Length)
		{
			return false;
		}
		for (int i = 0; i < left.VertexStreams.Length; i++)
		{
			VkBuffer buffer = left.VertexStreams[i].Buffer;
			VkBuffer buffer2 = right.VertexStreams[i].Buffer;
			if (buffer.VkHandle != buffer2.VkHandle || buffer.VkObjectType != buffer2.VkObjectType)
			{
				return false;
			}
		}
		return true;
	}

	private void EnsureFrameStateCount(int count)
	{
		if (frameStates.Length != count)
		{
			for (int i = 0; i < frameStates.Length; i++)
			{
				frameStates[i].Dispose(device);
			}
			frameStates = CreateFrameStates(count);
			frameStateCur = frameStates[0];
		}
	}

	private PerFrameState[] CreateFrameStates(int count)
	{
		PerFrameState[] array = new PerFrameState[count];
		for (int i = 0; i < array.Length; i++)
		{
			int num = i;
			int num2 = 1;
			List<VkDescriptorPool> list = new List<VkDescriptorPool>(num2);
			CollectionsMarshal.SetCount(list, num2);
			CollectionsMarshal.AsSpan(list)[0] = CreateDescriptorPool(descriptorSetCapacity);
			array[num] = new PerFrameState(list, new UploadBuffer(device, allocator, $"DrawCallBuilder.Uniforms[{i}]", uniformBufferCapacity, topologyUniformAlignment, VkBufferUsageFlags.UniformBufferBit), new UploadBuffer(device, allocator, $"DrawCallBuilder.Instances[{i}]", instanceBufferCapacity, 16, VkBufferUsageFlags.VertexBufferBit), new UploadBuffer(device, allocator, $"DrawCallBuilder.Storage[{i}]", storageBufferCapacity, storageBufferAlignment, VkBufferUsageFlags.StorageBufferBit), new UploadBuffer(device, allocator, $"DrawCallBuilder.Indirect[{i}]", indirectBufferCapacity, 16, VkBufferUsageFlags.IndirectBufferBit));
		}
		return array;
	}

	private int GetFrameStateIndex(int frameIndex)
	{
		int num = frameIndex % frameStates.Length;
		if (num < 0)
		{
			num += frameStates.Length;
		}
		return num;
	}

	private unsafe VkDescriptorPool CreateDescriptorPool(int maxSets)
	{
		VkDescriptorPoolSize* ptr = stackalloc VkDescriptorPoolSize[5];
		*ptr = new VkDescriptorPoolSize
		{
			Type = VkDescriptorType.UniformBuffer,
			DescriptorCount = maxSets * 4
		};
		ptr[1] = new VkDescriptorPoolSize
		{
			Type = VkDescriptorType.CombinedImageSampler,
			DescriptorCount = maxSets * 16
		};
		ptr[2] = new VkDescriptorPoolSize
		{
			Type = VkDescriptorType.StorageBuffer,
			DescriptorCount = maxSets * 8
		};
		ptr[3] = new VkDescriptorPoolSize
		{
			Type = VkDescriptorType.SampledImage,
			DescriptorCount = maxSets * 16
		};
		ptr[4] = new VkDescriptorPoolSize
		{
			Type = VkDescriptorType.Sampler,
			DescriptorCount = maxSets
		};
		DeviceEx handle = device;
		VkDescriptorPoolCreateInfo pCreateInfo = new VkDescriptorPoolCreateInfo
		{
			MaxSets = maxSets,
			PoolSizeCount = 5,
			PoolSizes = ptr
		};
		return handle.CreateDescriptorPool(in pCreateInfo, device.VkAllocator);
	}
}
