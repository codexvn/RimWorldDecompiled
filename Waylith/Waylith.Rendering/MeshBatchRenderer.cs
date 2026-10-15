using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Threading;
using Brutal;
using Brutal.Logging;
using Brutal.Logging.Internal;
using Brutal.Numerics;
using Brutal.VulkanApi;
using Brutal.VulkanApi.Abstractions;
using Brutal.VulkanApi.Vma;
using Waylith.Collections;
using Waylith.Graphics;
using Waylith.Rendering.Compute;

namespace Waylith.Rendering;

public sealed class MeshBatchRenderer : IDisposable
{
	private struct GlobalVertexUniforms
	{
		public Matrix4x4 ViewProj;
	}

	private struct GlobalFragmentUniforms
	{
		public Vector4 SunDirection;

		public Matrix4x4 WorldToCamera;

		public Matrix4x4 ViewProj;
	}

	private readonly struct PerFrameMaterialSetKey : IRefEquatable<PerFrameMaterialSetKey>
	{
		public readonly Shader Shader;

		public readonly MaterialUniformBytes Uniforms;

		public readonly MaterialTextureResourceSet<Texture?> Resources;

		public readonly TextureWrapMode? WrapOverride;

		public readonly MaterialStorageArray[]? StorageArrays;

		public PerFrameMaterialSetKey(Shader shader, in MaterialUniformBytes uniforms, in MaterialTextureResourceSet<Texture?> resources, TextureWrapMode? wrapOverride, MaterialStorageArray[]? storageArrays)
		{
			Shader = shader;
			Uniforms = uniforms;
			Resources = resources;
			WrapOverride = wrapOverride;
			StorageArrays = storageArrays;
		}

		public bool Equals(in PerFrameMaterialSetKey other)
		{
			if (Shader == other.Shader && WrapOverride == other.WrapOverride && StorageArrays == other.StorageArrays && Uniforms.Equals(in other.Uniforms))
			{
				return Resources.Equals(in other.Resources);
			}
			return false;
		}

		public override bool Equals(object? obj)
		{
			if (obj is PerFrameMaterialSetKey other)
			{
				return Equals(in other);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return Hash(Shader, in Uniforms, in Resources, WrapOverride);
		}

		public static int Hash(Shader shader, in MaterialUniformBytes uniforms, in MaterialTextureResourceSet<Texture?> resources, TextureWrapMode? wrapOverride)
		{
			return HashCode.Combine(ReferenceEqualityComparer.Instance.GetHashCode(shader), uniforms.GetHashCode(), resources.GetHashCode(), wrapOverride);
		}

		bool IRefEquatable<PerFrameMaterialSetKey>.Equals(in PerFrameMaterialSetKey other)
		{
			return Equals(in other);
		}
	}

	private readonly ref struct PerFrameMaterialSetLookup : IRefLookup<PerFrameMaterialSetKey>
	{
		private readonly Shader shader;

		private readonly ref readonly MaterialUniformBytes uniforms;

		private readonly ref readonly MaterialTextureResourceSet<Texture?> resources;

		private readonly TextureWrapMode? wrapOverride;

		private readonly MaterialStorageArray[]? storageArrays;

		public PerFrameMaterialSetLookup(Shader shader, in MaterialUniformBytes uniforms, in MaterialTextureResourceSet<Texture?> resources, TextureWrapMode? wrapOverride, MaterialStorageArray[]? storageArrays)
		{
			this.shader = shader;
			this.uniforms = ref uniforms;
			this.resources = ref resources;
			this.wrapOverride = wrapOverride;
			this.storageArrays = storageArrays;
		}

		public int KeyHash()
		{
			return PerFrameMaterialSetKey.Hash(shader, in uniforms, in resources, wrapOverride);
		}

		public bool KeyEquals(in PerFrameMaterialSetKey key)
		{
			if (shader == key.Shader && wrapOverride == key.WrapOverride && storageArrays == key.StorageArrays && uniforms.Equals(in key.Uniforms))
			{
				return resources.Equals(in key.Resources);
			}
			return false;
		}

		public PerFrameMaterialSetKey ToKey()
		{
			return new PerFrameMaterialSetKey(shader, in uniforms, in resources, wrapOverride, storageArrays);
		}

		bool IRefLookup<PerFrameMaterialSetKey>.KeyEquals(in PerFrameMaterialSetKey key)
		{
			return KeyEquals(in key);
		}
	}

	private readonly struct PendingMaterialSetRetirement
	{
		public VkDescriptorSet Set { get; }

		public int RetireAfterFrame { get; }

		public PendingMaterialSetRetirement(VkDescriptorSet set, int retireAfterFrame)
		{
			Set = set;
			RetireAfterFrame = retireAfterFrame;
		}
	}

	private sealed class PersistentBufferCache : IDisposable
	{
		private readonly struct CacheEntry
		{
			public readonly long version;

			public readonly VkDescriptorBufferInfo bufferInfo;

			public readonly FreeRange range;

			public CacheEntry(long version, VkDescriptorBufferInfo bufferInfo, FreeRange range)
			{
				this.version = version;
				this.bufferInfo = bufferInfo;
				this.range = range;
			}
		}

		private readonly struct PendingRetirement
		{
			public FreeRange Range { get; }

			public int RetireAfterFrame { get; }

			public PendingRetirement(FreeRange range, int retireAfterFrame)
			{
				Range = range;
				RetireAfterFrame = retireAfterFrame;
			}
		}

		private readonly struct FreeRange
		{
			public int Offset { get; }

			public int Size { get; }

			public FreeRange(int offset, int size)
			{
				Offset = offset;
				Size = size;
			}
		}

		private readonly VmaAllocator allocator;

		private readonly GpuMemoryCategory category;

		private readonly BufferAllocationInfo allocation;

		private readonly AllocationMapInfo mapInfo;

		private unsafe readonly byte* mappedPointer;

		private readonly int alignment;

		private readonly int capacity;

		private readonly List<FreeRange> freeRanges = new List<FreeRange>();

		private readonly List<PendingRetirement> pendingRetirements = new List<PendingRetirement>();

		private int usedBytes;

		private readonly Dictionary<long, CacheEntry> cache = new Dictionary<long, CacheEntry>();

		public unsafe PersistentBufferCache(DeviceEx device, VmaAllocator allocator, string label, int capacityBytes, int alignment, VkBufferUsageFlags usage, GpuMemoryCategory category)
		{
			this.allocator = allocator;
			this.category = category;
			this.alignment = System.Math.Max(alignment, 16);
			capacity = capacityBytes;
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
			GpuMemoryTracker.Add(category, in allocation, null, label);
		}

		public bool TryGetOrUpload<T>(long id, long version, in T data, int currentFrame, int framesInFlight, out VkDescriptorBufferInfo bufferInfo) where T : unmanaged
		{
			return TryGetOrUpload(id, version, MemoryMarshal.AsBytes(new ReadOnlySpan<T>(in data)), currentFrame, framesInFlight, out bufferInfo);
		}

		public unsafe bool TryGetOrUpload(long id, long version, ReadOnlySpan<byte> data, int currentFrame, int framesInFlight, out VkDescriptorBufferInfo bufferInfo)
		{
			if (cache.TryGetValue(id, out var value) && value.version == version)
			{
				bufferInfo = value.bufferInfo;
				return true;
			}
			if (data.IsEmpty || !TryAllocate(data.Length, out var range))
			{
				bufferInfo = default(VkDescriptorBufferInfo);
				return false;
			}
			data.CopyTo(new Span<byte>(mappedPointer + range.Offset, data.Length));
			bufferInfo = new VkDescriptorBufferInfo
			{
				Buffer = allocation.VkBuffer,
				Offset = ByteSize.Of(range.Offset),
				Range = ByteSize.Of(data.Length)
			};
			if (cache.TryGetValue(id, out value))
			{
				pendingRetirements.Add(new PendingRetirement(value.range, currentFrame + framesInFlight));
			}
			cache[id] = new CacheEntry(version, bufferInfo, range);
			return true;
		}

		public void ScheduleRemove(long id, int currentFrame, int framesInFlight)
		{
			if (cache.Remove(id, out var value))
			{
				pendingRetirements.Add(new PendingRetirement(value.range, currentFrame + framesInFlight));
			}
		}

		public void CollectRetired(int currentFrame)
		{
			for (int num = pendingRetirements.Count - 1; num >= 0; num--)
			{
				if (pendingRetirements[num].RetireAfterFrame <= currentFrame)
				{
					ReleaseRange(pendingRetirements[num].Range);
					pendingRetirements.RemoveAt(num);
				}
			}
		}

		public void Dispose()
		{
			GpuMemoryTracker.Remove(category, in allocation);
			allocator.UnmapMemory(in mapInfo);
			allocator.FreeAllocation(in allocation);
		}

		private bool TryAllocate(int sizeBytes, out FreeRange range)
		{
			sizeBytes = Align(sizeBytes, alignment);
			for (int i = 0; i < freeRanges.Count; i++)
			{
				FreeRange freeRange = freeRanges[i];
				if (freeRange.Size >= sizeBytes)
				{
					range = new FreeRange(freeRange.Offset, sizeBytes);
					if (freeRange.Size == sizeBytes)
					{
						freeRanges.RemoveAt(i);
					}
					else
					{
						freeRanges[i] = new FreeRange(freeRange.Offset + sizeBytes, freeRange.Size - sizeBytes);
					}
					return true;
				}
			}
			int num = Align(usedBytes, alignment);
			if (num + sizeBytes > capacity)
			{
				range = default(FreeRange);
				return false;
			}
			usedBytes = num + sizeBytes;
			range = new FreeRange(num, sizeBytes);
			return true;
		}

		private void ReleaseRange(FreeRange range)
		{
			int i;
			for (i = 0; i < freeRanges.Count && freeRanges[i].Offset < range.Offset; i++)
			{
			}
			freeRanges.Insert(i, range);
			if (i > 0 && TryMerge(i - 1, i))
			{
				i--;
			}
			if (i + 1 < freeRanges.Count)
			{
				TryMerge(i, i + 1);
			}
		}

		private bool TryMerge(int leftIndex, int rightIndex)
		{
			FreeRange freeRange = freeRanges[leftIndex];
			FreeRange freeRange2 = freeRanges[rightIndex];
			if (freeRange.Offset + freeRange.Size != freeRange2.Offset)
			{
				return false;
			}
			freeRanges[leftIndex] = new FreeRange(freeRange.Offset, freeRange.Size + freeRange2.Size);
			freeRanges.RemoveAt(rightIndex);
			return true;
		}

		private static int Align(int value, int alignment)
		{
			int num = alignment - 1;
			return (value + num) & ~num;
		}
	}

	private const int DescriptorSetCapacity = 2048;

	private const int PerFrameUniformCapacity = 4194304;

	private const int PerFrameInstanceCapacity = 8388608;

	private readonly DeviceEx device;

	private readonly VmaAllocator allocator;

	private readonly VkDescriptorSetLayout frameDescriptorSetLayout;

	private readonly VkDescriptorSetLayout globalTextureDescriptorSetLayout;

	private readonly VulkanShaderLayoutCache shaderLayoutCache;

	private readonly ShaderModuleCache shaderModules;

	private readonly VulkanMeshDrawEncoder vkDrawEncoder;

	private readonly MeshPipelineCache pipelines;

	private readonly List<FinalDrawItem> drawScratch = new List<FinalDrawItem>();

	private readonly Dictionary<ShaderModuleCache.MeshSamplerKey, VkSampler> samplers = new Dictionary<ShaderModuleCache.MeshSamplerKey, VkSampler>();

	private Texture? fallbackWhiteTexture;

	private Texture? fallbackWhiteArrayTexture;

	private float deviceMaxSamplerAnisotropy = -1f;

	private readonly Dictionary<Texture, VkDescriptorImageInfo> frameImageInfoCache = new Dictionary<Texture, VkDescriptorImageInfo>(ReferenceEqualityComparer.Instance);

	private readonly VkDescriptorImageInfo?[] frameNullImageInfoCache = new VkDescriptorImageInfo?[3];

	private VkDescriptorImageInfo? frameNullArrayImageInfoCache;

	private readonly DenseDictionary<PerFrameMaterialSetKey, VkDescriptorSet> perFrameMaterialSetCache = new DenseDictionary<PerFrameMaterialSetKey, VkDescriptorSet>();

	private readonly PersistentBufferCache persistentMaterialUbos;

	private readonly PersistentBufferCache persistentMaterialStorage;

	private readonly Dictionary<int, HashSet<int>> storagePropertiesByMaterial = new Dictionary<int, HashSet<int>>();

	private const long ZeroStorageKey = -1L;

	private static readonly byte[] ZeroStorageBytes = new byte[4096];

	private readonly VkDescriptorPool materialSetPool;

	private readonly Dictionary<int, (long version, int resourceGeneration, int epoch, VkDescriptorSet set)> materialSetCache = new Dictionary<int, (long, int, int, VkDescriptorSet)>();

	private readonly List<PendingMaterialSetRetirement> pendingMaterialSetRetirements = new List<PendingMaterialSetRetirement>();

	private readonly MaterialReleaseListener materialReleaseListener = new MaterialReleaseListener();

	private readonly Dictionary<int, HashSet<int>> bindingsByMaterial = new Dictionary<int, HashSet<int>>();

	private IRenderContext renderContext;

	private int currentFrameNumber;

	private int currentFramesInFlight = 1;

	private int pipelineVersion;

	private VkDescriptorSet sceneFrameDescriptorSet;

	private int sceneSetupFrame = -1;

	private bool sceneReady;

	private int currentColorAttachmentCount = -1;

	private static readonly object LiveRenderersSync = new object();

	private static readonly List<MeshBatchRenderer> LiveRenderers = new List<MeshBatchRenderer>();

	private static readonly HashSet<string> LoggedFallbackTextures = new HashSet<string>();

	public static void ReloadAllShaders()
	{
		lock (LiveRenderersSync)
		{
			foreach (MeshBatchRenderer liveRenderer in LiveRenderers)
			{
				liveRenderer.ReloadShaders();
			}
		}
	}

	private void ReloadShaders()
	{
		shaderModules.Clear();
		pipelines.InvalidateAll();
		pipelineVersion++;
	}

	public MeshBatchRenderer(GraphicsDevice graphicsDevice, IRenderContext renderContext)
	{
		device = graphicsDevice.Device;
		allocator = graphicsDevice.Allocator;
		this.renderContext = renderContext;
		frameDescriptorSetLayout = CreateFrameDescriptorSetLayout();
		globalTextureDescriptorSetLayout = CreateReservedDescriptorSetLayout();
		shaderLayoutCache = graphicsDevice.ShaderLayoutCache;
		shaderModules = new ShaderModuleCache(device);
		int num = ResolveUniformBufferAlignment(renderContext);
		vkDrawEncoder = new VulkanMeshDrawEncoder(device, allocator, num, 2048);
		persistentMaterialUbos = new PersistentBufferCache(device, allocator, "PersistentMaterialUniforms", 2097152, num, VkBufferUsageFlags.UniformBufferBit, GpuMemoryCategory.UniformBuffer);
		persistentMaterialStorage = new PersistentBufferCache(device, allocator, "PersistentMaterialStorage", 8388608, ResolveStorageBufferAlignment(renderContext), VkBufferUsageFlags.StorageBufferBit, GpuMemoryCategory.ComputeBuffer);
		pipelines = new MeshPipelineCache(device, shaderLayoutCache, shaderModules);
		materialSetPool = CreateMaterialDescriptorPool();
		lock (LiveRenderersSync)
		{
			LiveRenderers.Add(this);
		}
	}

	public void OnRenderContextRebuilt(IRenderContext renderContext)
	{
		this.renderContext = renderContext;
		deviceMaxSamplerAnisotropy = -1f;
		pipelines.InvalidateAll();
		pipelineVersion++;
	}

	public void Dispose()
	{
		lock (LiveRenderersSync)
		{
			LiveRenderers.Remove(this);
		}
		materialReleaseListener.Dispose();
		bindingsByMaterial.Clear();
		vkDrawEncoder.Dispose();
		persistentMaterialUbos.Dispose();
		persistentMaterialStorage.Dispose();
		pipelines.Dispose();
		shaderModules.Dispose();
		foreach (VkSampler value in samplers.Values)
		{
			device.DestroySampler(value, device.VkAllocator);
		}
		samplers.Clear();
		fallbackWhiteTexture?.Dispose();
		fallbackWhiteTexture = null;
		fallbackWhiteArrayTexture?.Dispose();
		fallbackWhiteArrayTexture = null;
		if (!materialSetPool.Equals(default(VkDescriptorPool)))
		{
			materialSetCache.Clear();
			device.DestroyDescriptorPool(materialSetPool, device.VkAllocator);
		}
		if (!globalTextureDescriptorSetLayout.IsNull())
		{
			device.DestroyDescriptorSetLayout(globalTextureDescriptorSetLayout, device.VkAllocator);
		}
		if (!frameDescriptorSetLayout.IsNull())
		{
			device.DestroyDescriptorSetLayout(frameDescriptorSetLayout, device.VkAllocator);
		}
	}

	public void BeginFrame(in FrameContext frameContext, int frameNumber)
	{
		BeginFrameCore(in frameContext, frameNumber);
	}

	public void InvalidateCommandBufferBindings()
	{
		vkDrawEncoder.InvalidateBoundState();
	}

	private void BeginFrameCore(in FrameContext frameContext, int frameNumber)
	{
		currentFrameNumber = frameNumber;
		currentFramesInFlight = System.Math.Max(frameContext.RenderContext.FramesInFlight, 1);
		currentColorAttachmentCount = -1;
		CollectRetiredMaterialResources();
		vkDrawEncoder.BeginFrame(in frameContext);
		frameImageInfoCache.Clear();
		Array.Clear(frameNullImageInfoCache);
		frameNullArrayImageInfoCache = null;
		perFrameMaterialSetCache.Clear();
	}

	public void RecordBatch(in FrameContext frameContext, IReadOnlyList<FinalDrawItem> draws, CameraView cameraView, Vector3? sunDirection = null)
	{
		if (draws.Count == 0)
		{
			return;
		}
		VkDescriptorSet frameDescriptorSet = CreateFrameDescriptorSet(in cameraView, sunDirection ?? new Vector3(0f, 1f, 0f));
		if (!frameDescriptorSet.IsNull())
		{
			VkDescriptorSet globalTextureDescriptorSet = default(VkDescriptorSet);
			for (int i = 0; i < draws.Count; i++)
			{
				DrawBatch(in frameContext, frameDescriptorSet, globalTextureDescriptorSet, draws[i]);
			}
		}
	}

	public void RecordPass(in FrameContext frameContext, MeshDrawFrame meshFrame, byte passId = 0, CameraView? viewOverride = null, string? diagnosticPrefix = null, int queueMin = int.MinValue, int queueMax = int.MaxValue, int colorAttachmentCount = -1, bool mergeBySortKey = false)
	{
		if (sceneSetupFrame != meshFrame.FrameNumber)
		{
			sceneSetupFrame = meshFrame.FrameNumber;
			sceneReady = false;
			BeginFrameCore(in frameContext, meshFrame.FrameNumber);
			if (meshFrame.Draws.Count == 0 && meshFrame.PersistentDraws.Count == 0)
			{
				return;
			}
			sceneFrameDescriptorSet = CreateFrameDescriptorSet(viewOverride ?? meshFrame.View, meshFrame.SunDirection);
			if (sceneFrameDescriptorSet.IsNull())
			{
				LogDiagnostic(diagnosticPrefix, $"RecordMainPass aborted: frame descriptor set is null (pass {passId}).");
				return;
			}
			drawScratch.Clear();
			for (int i = 0; i < meshFrame.Draws.Count; i++)
			{
				FinalDrawItem finalDrawItem = meshFrame.Draws[i];
				if (finalDrawItem.PassId == 0 || finalDrawItem.PassId == passId)
				{
					drawScratch.Add(finalDrawItem);
				}
			}
			drawScratch.Sort(DrawSortKeyComparer.Instance);
			sceneReady = true;
		}
		if (!sceneReady)
		{
			return;
		}
		VkDescriptorSet globalTextureDescriptorSet = default(VkDescriptorSet);
		currentColorAttachmentCount = colorAttachmentCount;
		List<FinalDrawItem> persistentDraws = meshFrame.PersistentDraws;
		if (mergeBySortKey)
		{
			int count = persistentDraws.Count;
			int count2 = drawScratch.Count;
			int num = 0;
			int j = 0;
			while (true)
			{
				if (num < count)
				{
					FinalDrawItem finalDrawItem2 = persistentDraws[num];
					if ((finalDrawItem2.PassId != 0 && finalDrawItem2.PassId != passId) || finalDrawItem2.RenderQueue < queueMin || finalDrawItem2.RenderQueue >= queueMax)
					{
						num++;
						continue;
					}
				}
				for (; j < count2 && (drawScratch[j].RenderQueue < queueMin || drawScratch[j].RenderQueue >= queueMax); j++)
				{
				}
				bool flag = num < count;
				bool flag2 = j < count2;
				if (!flag && !flag2)
				{
					break;
				}
				FinalDrawItem finalDrawItem3 = ((flag && (!flag2 || persistentDraws[num].SortKey.CompareTo(drawScratch[j].SortKey) <= 0)) ? persistentDraws[num++] : drawScratch[j++]);
				if (finalDrawItem3.Instancing != null)
				{
					DrawBatchInstanced(in frameContext, sceneFrameDescriptorSet, globalTextureDescriptorSet, finalDrawItem3, diagnosticPrefix);
				}
				else
				{
					DrawBatch(in frameContext, sceneFrameDescriptorSet, globalTextureDescriptorSet, finalDrawItem3, diagnosticPrefix);
				}
			}
			currentColorAttachmentCount = -1;
			return;
		}
		for (int k = 0; k < persistentDraws.Count; k++)
		{
			FinalDrawItem finalDrawItem4 = persistentDraws[k];
			if (finalDrawItem4.PassId != 0 && finalDrawItem4.PassId != passId)
			{
				continue;
			}
			int renderQueue = finalDrawItem4.RenderQueue;
			if (renderQueue >= queueMin && renderQueue < queueMax)
			{
				if (finalDrawItem4.Instancing != null)
				{
					DrawBatchInstanced(in frameContext, sceneFrameDescriptorSet, globalTextureDescriptorSet, finalDrawItem4, diagnosticPrefix);
				}
				else
				{
					DrawBatch(in frameContext, sceneFrameDescriptorSet, globalTextureDescriptorSet, finalDrawItem4, diagnosticPrefix);
				}
			}
		}
		for (int l = 0; l < drawScratch.Count; l++)
		{
			FinalDrawItem finalDrawItem5 = drawScratch[l];
			int renderQueue2 = finalDrawItem5.RenderQueue;
			if (renderQueue2 >= queueMin && renderQueue2 < queueMax)
			{
				if (finalDrawItem5.Instancing != null)
				{
					DrawBatchInstanced(in frameContext, sceneFrameDescriptorSet, globalTextureDescriptorSet, finalDrawItem5, diagnosticPrefix);
				}
				else
				{
					DrawBatch(in frameContext, sceneFrameDescriptorSet, globalTextureDescriptorSet, finalDrawItem5, diagnosticPrefix);
				}
			}
		}
		currentColorAttachmentCount = -1;
	}

	public void BumpPipelineVersion()
	{
		pipelineVersion++;
	}

	private int ResolveColorAttachmentCount(in FrameContext frameContext)
	{
		if (currentColorAttachmentCount < 0)
		{
			return (frameContext.ColorFormat != VkFormat.Undefined) ? 1 : 0;
		}
		return currentColorAttachmentCount;
	}

	private void DrawBatch(in FrameContext frameContext, VkDescriptorSet frameDescriptorSet, VkDescriptorSet globalTextureDescriptorSet, FinalDrawItem draw, string? diagnosticPrefix = null)
	{
		MaterialBinding binding = draw.Binding;
		MeshDrawGeometry mesh = draw.Mesh;
		if (draw.cachedPipelineVersion != pipelineVersion)
		{
			draw.cachedPipelineLayout = shaderLayoutCache.GetOrCreatePipelineLayout(binding.Shader, frameDescriptorSetLayout, globalTextureDescriptorSetLayout);
			draw.cachedPipeline = pipelines.GetOrCreatePipeline(in frameDescriptorSetLayout, in globalTextureDescriptorSetLayout, binding, mesh.Layout, mesh.Topology, binding.PipelineState, frameContext.RenderPass, frameContext.SampleCount, ResolveColorAttachmentCount(in frameContext), draw.PassId, diagnosticPrefix);
			draw.cachedPipelineVersion = pipelineVersion;
		}
		if (draw.cachedPipeline.IsNull())
		{
			LogDiagnostic(diagnosticPrefix, "draw skipped: pipeline is null; " + FormatDraw(draw) + ".");
			return;
		}
		VkDescriptorSet materialDescriptorSet = ((draw.HasOverride || draw.Storage != null) ? CreateOverrideMaterialDescriptorSet(draw) : GetOrCreateCachedMaterialSet(binding));
		if (materialDescriptorSet.IsNull())
		{
			LogDiagnostic(diagnosticPrefix, "draw skipped: material descriptor set is null; " + FormatDraw(draw) + ".");
		}
		else
		{
			vkDrawEncoder.SubmitDraw(in frameContext, draw.cachedPipeline, draw.cachedPipelineLayout, binding.Shader.VertexInputLayout, mesh, frameDescriptorSet, globalTextureDescriptorSet, materialDescriptorSet, draw.Matrix);
		}
	}

	private void DrawBatch(in FrameContext frameContext, VkDescriptorSet frameDescriptorSet, VkDescriptorSet globalTextureDescriptorSet, FinalDrawItem draw, ReadOnlySpan<Matrix4x4> instances)
	{
		if (instances.Length == 0)
		{
			return;
		}
		MaterialBinding binding = draw.Binding;
		MeshDrawGeometry mesh = draw.Mesh;
		if (draw.cachedPipelineVersion != pipelineVersion)
		{
			draw.cachedPipelineLayout = shaderLayoutCache.GetOrCreatePipelineLayout(binding.Shader, frameDescriptorSetLayout, globalTextureDescriptorSetLayout);
			draw.cachedPipeline = pipelines.GetOrCreatePipeline(in frameDescriptorSetLayout, in globalTextureDescriptorSetLayout, binding, mesh.Layout, mesh.Topology, binding.PipelineState, frameContext.RenderPass, frameContext.SampleCount, ResolveColorAttachmentCount(in frameContext), draw.PassId);
			draw.cachedPipelineVersion = pipelineVersion;
		}
		if (!draw.cachedPipeline.IsNull())
		{
			VkDescriptorSet materialDescriptorSet = ((draw.HasOverride || draw.Storage != null) ? CreateOverrideMaterialDescriptorSet(draw) : GetOrCreateCachedMaterialSet(binding));
			if (!materialDescriptorSet.IsNull())
			{
				string label = CreateDrawDebugLabel(draw, instances.Length);
				BeginDebugLabel(frameContext.CommandBuffer, label, new float4(0.25f, 0.72f, 0.34f, 1f));
				vkDrawEncoder.SubmitDraw(in frameContext, draw.cachedPipeline, draw.cachedPipelineLayout, binding.Shader.VertexInputLayout, mesh, frameDescriptorSet, globalTextureDescriptorSet, materialDescriptorSet, instances);
				EndDebugLabel(frameContext.CommandBuffer);
			}
		}
	}

	private void DrawBatchInstanced(in FrameContext frameContext, VkDescriptorSet frameDescriptorSet, VkDescriptorSet globalTextureDescriptorSet, FinalDrawItem draw, string? diagnosticPrefix = null)
	{
		InstancedDrawInfo instancing = draw.Instancing;
		MaterialBinding binding = draw.Binding;
		MeshDrawGeometry mesh = draw.Mesh;
		if (draw.cachedPipelineVersion != pipelineVersion)
		{
			draw.cachedPipelineLayout = shaderLayoutCache.GetOrCreatePipelineLayout(binding.Shader, frameDescriptorSetLayout, globalTextureDescriptorSetLayout);
			draw.cachedPipeline = pipelines.GetOrCreatePipeline(in frameDescriptorSetLayout, in globalTextureDescriptorSetLayout, binding, mesh.Layout, mesh.Topology, binding.PipelineState, frameContext.RenderPass, frameContext.SampleCount, ResolveColorAttachmentCount(in frameContext), draw.PassId, diagnosticPrefix, instanced: true);
			draw.cachedPipelineVersion = pipelineVersion;
		}
		if (draw.cachedPipeline.IsNull())
		{
			LogDiagnostic(diagnosticPrefix, "instanced draw skipped: pipeline is null; " + FormatDraw(draw) + ".");
			return;
		}
		VkDescriptorSet orCreateInstancedMaterialSet = GetOrCreateInstancedMaterialSet(draw, instancing);
		if (orCreateInstancedMaterialSet.IsNull())
		{
			LogDiagnostic(diagnosticPrefix, "instanced draw skipped: material descriptor set is null; " + FormatDraw(draw) + ".");
			return;
		}
		Matrix4x4 matrix = draw.Matrix;
		if (instancing.Mode == InstanceDrawMode.Indirect)
		{
			if (!TryResolveIndirectArgs(instancing, out var buffer, out var offset))
			{
				LogDiagnostic(diagnosticPrefix, "instanced draw skipped: indirect args unavailable; " + FormatDraw(draw) + ".");
				return;
			}
			string label = CreateDrawDebugLabel(draw, 0);
			BeginDebugLabel(frameContext.CommandBuffer, label, new float4(0.25f, 0.72f, 0.34f, 1f));
			vkDrawEncoder.SubmitDrawInstancedIndirect(in frameContext, draw.cachedPipeline, draw.cachedPipelineLayout, binding.Shader.VertexInputLayout, mesh, frameDescriptorSet, globalTextureDescriptorSet, orCreateInstancedMaterialSet, in matrix, buffer, offset);
			EndDebugLabel(frameContext.CommandBuffer);
			return;
		}
		InstanceRange[] ranges = instancing.Ranges;
		if (ranges != null)
		{
			int num = 0;
			InstanceRange[] array = ranges;
			foreach (InstanceRange instanceRange in array)
			{
				num += instanceRange.InstanceCount;
			}
			string label2 = CreateDrawDebugLabel(draw, num);
			BeginDebugLabel(frameContext.CommandBuffer, label2, new float4(0.25f, 0.72f, 0.34f, 1f));
			vkDrawEncoder.SubmitDrawInstancedDirectRanges(in frameContext, draw.cachedPipeline, draw.cachedPipelineLayout, binding.Shader.VertexInputLayout, mesh, frameDescriptorSet, globalTextureDescriptorSet, orCreateInstancedMaterialSet, in matrix, ranges);
			EndDebugLabel(frameContext.CommandBuffer);
		}
		else
		{
			string label3 = CreateDrawDebugLabel(draw, instancing.InstanceCount);
			BeginDebugLabel(frameContext.CommandBuffer, label3, new float4(0.25f, 0.72f, 0.34f, 1f));
			vkDrawEncoder.SubmitDrawInstancedDirect(in frameContext, draw.cachedPipeline, draw.cachedPipelineLayout, binding.Shader.VertexInputLayout, mesh, frameDescriptorSet, globalTextureDescriptorSet, orCreateInstancedMaterialSet, in matrix, instancing.InstanceCount);
			EndDebugLabel(frameContext.CommandBuffer);
		}
	}

	private VkDescriptorSet GetOrCreateInstancedMaterialSet(FinalDrawItem draw, InstancedDrawInfo instancing)
	{
		MaterialBinding binding = draw.Binding;
		Shader shader = binding.Shader;
		bool flag = instancing.IsFullyPersistent && instancing.Ranges == null && !draw.HasOverride;
		int num = ComputeResourceResidencyGeneration(in draw.Resources);
		long resolvedDataVersion = binding.ResolvedDataVersion;
		if (flag && draw.cachedInstancedResolvedDataVersion == resolvedDataVersion && draw.cachedInstancedResourceGeneration == num && !draw.cachedInstancedMaterialSet.IsNull())
		{
			return draw.cachedInstancedMaterialSet;
		}
		VkDescriptorSetLayout orCreateMaterialSetLayout = shaderLayoutCache.GetOrCreateMaterialSetLayout(shader);
		VkDescriptorSet vkDescriptorSet;
		if (flag && persistentMaterialUbos.TryGetOrUpload(binding.Id, resolvedDataVersion, in draw.Uniforms, currentFrameNumber, currentFramesInFlight, out var bufferInfo))
		{
			try
			{
				vkDescriptorSet = DescriptorSetWriter.AllocateDescriptorSet(device, materialSetPool, orCreateMaterialSetLayout);
			}
			catch (VkResultError)
			{
				vkDescriptorSet = default(VkDescriptorSet);
			}
			if (vkDescriptorSet.IsNull())
			{
				flag = false;
				vkDescriptorSet = vkDrawEncoder.AllocateDescriptorSet(orCreateMaterialSetLayout);
				if (vkDescriptorSet.IsNull() || !vkDrawEncoder.TryUploadUniforms(in draw.Uniforms, out bufferInfo))
				{
					return default(VkDescriptorSet);
				}
			}
		}
		else
		{
			flag = false;
			vkDescriptorSet = vkDrawEncoder.AllocateDescriptorSet(orCreateMaterialSetLayout);
			if (vkDescriptorSet.IsNull() || !vkDrawEncoder.TryUploadUniforms(in draw.Uniforms, out bufferInfo))
			{
				return default(VkDescriptorSet);
			}
		}
		WriteMaterialBindingToSet(vkDescriptorSet, shader, in bufferInfo, in draw.Resources, draw.Binding.DebugName, draw.Binding.WrapModeOverride);
		if (!TryWriteMaterialStorage(vkDescriptorSet, shader, binding, instancing.StorageBindings, instanced: true, out var transient))
		{
			return default(VkDescriptorSet);
		}
		if (flag && transient)
		{
			ScheduleMaterialSetRetirement(vkDescriptorSet);
			flag = false;
		}
		if (flag)
		{
			draw.cachedInstancedMaterialSet = vkDescriptorSet;
			draw.cachedInstancedResolvedDataVersion = resolvedDataVersion;
			draw.cachedInstancedResourceGeneration = num;
		}
		return vkDescriptorSet;
	}

	private bool TryWriteMaterialStorage(VkDescriptorSet set, Shader shader, MaterialBinding binding, InstanceStorageBinding[]? drawStorage, bool instanced, out bool transient)
	{
		transient = false;
		MaterialStorageBufferSlot[] storageBufferSlots = shader.MaterialLayout.StorageBufferSlots;
		if (storageBufferSlots.Length == 0)
		{
			return true;
		}
		Span<int> span = stackalloc int[storageBufferSlots.Length];
		Span<VkDescriptorBufferInfo> span2 = stackalloc VkDescriptorBufferInfo[storageBufferSlots.Length];
		Span<bool> span3 = stackalloc bool[storageBufferSlots.Length];
		VkDescriptorBufferInfo bufferInfo = default(VkDescriptorBufferInfo);
		bool flag = false;
		for (int i = 0; i < storageBufferSlots.Length; i++)
		{
			span[i] = storageBufferSlots[i].Binding;
			if (TryResolveStorageBinding(drawStorage, binding, storageBufferSlots[i].PropertyId, ref transient, out var info))
			{
				span2[i] = info;
				span3[i] = true;
				if (!flag)
				{
					bufferInfo = info;
					flag = true;
				}
			}
		}
		if (!flag && (instanced || !persistentMaterialStorage.TryGetOrUpload(-1L, 0L, ZeroStorageBytes, currentFrameNumber, currentFramesInFlight, out bufferInfo)))
		{
			return false;
		}
		for (int j = 0; j < storageBufferSlots.Length; j++)
		{
			if (!span3[j])
			{
				span2[j] = bufferInfo;
			}
		}
		DescriptorSetWriter.WriteMaterialStorageBuffers(device, set, span, span2);
		return true;
	}

	private bool TryResolveStorageBinding(InstanceStorageBinding[]? drawStorage, MaterialBinding binding, int propertyId, ref bool transient, out VkDescriptorBufferInfo info)
	{
		if (drawStorage != null)
		{
			for (int i = 0; i < drawStorage.Length; i++)
			{
				InstanceStorageBinding instanceStorageBinding = drawStorage[i];
				if (instanceStorageBinding.PropertyId == propertyId)
				{
					if (instanceStorageBinding.IsPersistent)
					{
						info = new VkDescriptorBufferInfo
						{
							Buffer = instanceStorageBinding.Persistent.Buffer,
							Offset = ByteSize.Of(0),
							Range = instanceStorageBinding.Persistent.Size
						};
						return true;
					}
					transient = true;
					ReadOnlySpan<byte> bytes = instanceStorageBinding.Dynamic.GetBytes(instanceStorageBinding.Generation);
					if (!bytes.IsEmpty && vkDrawEncoder.TryUploadStorage(bytes, out info))
					{
						return true;
					}
					info = default(VkDescriptorBufferInfo);
					return false;
				}
			}
		}
		MaterialStorageArray[] storageArrays = binding.StorageArrays;
		for (int i = 0; i < storageArrays.Length; i++)
		{
			MaterialStorageArray materialStorageArray = storageArrays[i];
			if (materialStorageArray.PropertyId == propertyId)
			{
				if (persistentMaterialStorage.TryGetOrUpload(MaterialStorageKey(binding.MaterialId, propertyId), materialStorageArray.Generation, materialStorageArray.Bytes, currentFrameNumber, currentFramesInFlight, out info))
				{
					TrackStorageForRelease(binding.MaterialId, propertyId);
					return true;
				}
				transient = true;
				return vkDrawEncoder.TryUploadStorage(materialStorageArray.Bytes, out info);
			}
		}
		info = default(VkDescriptorBufferInfo);
		return false;
	}

	private void TrackStorageForRelease(int materialId, int propertyId)
	{
		if (!storagePropertiesByMaterial.TryGetValue(materialId, out HashSet<int> value))
		{
			value = (storagePropertiesByMaterial[materialId] = new HashSet<int>());
		}
		value.Add(propertyId);
	}

	private bool TryResolveIndirectArgs(InstancedDrawInfo instancing, out VkBuffer buffer, out ByteSize64 offset)
	{
		GpuComputeBuffer indirectArgsPersistent = instancing.IndirectArgsPersistent;
		if (indirectArgsPersistent != null)
		{
			buffer = indirectArgsPersistent.Buffer;
			offset = ByteSize.Of(instancing.IndirectArgsOffset);
			return true;
		}
		InstanceDataBuffer indirectArgsDynamic = instancing.IndirectArgsDynamic;
		if (indirectArgsDynamic != null)
		{
			ReadOnlySpan<byte> bytes = indirectArgsDynamic.GetBytes(instancing.IndirectArgsGeneration);
			return vkDrawEncoder.TryUploadIndirectArgs(bytes, instancing.IndirectArgsOffset, out buffer, out offset);
		}
		buffer = default(VkBuffer);
		offset = default(ByteSize64);
		return false;
	}

	private VkDescriptorSet GetOrCreateCachedMaterialSet(MaterialBinding binding)
	{
		int num = Volatile.Read(in Texture.GlobalResidencyEpoch);
		long resolvedDataVersion = binding.ResolvedDataVersion;
		(long, int, int, VkDescriptorSet) value;
		bool flag = materialSetCache.TryGetValue(binding.Id, out value) && !value.Item4.IsNull();
		bool flag2 = flag && value.Item1 == resolvedDataVersion;
		if (flag2 && value.Item3 == num)
		{
			return value.Item4;
		}
		int num2 = ComputeResourceResidencyGeneration(in binding.Resources);
		if (flag2 && value.Item2 == num2)
		{
			materialSetCache[binding.Id] = (value.Item1, num2, num, value.Item4);
			return value.Item4;
		}
		if (flag)
		{
			materialSetCache.Remove(binding.Id);
			ScheduleMaterialSetRetirement(value.Item4);
		}
		if (!persistentMaterialUbos.TryGetOrUpload(binding.Id, resolvedDataVersion, in binding.Uniforms, currentFrameNumber, currentFramesInFlight, out var bufferInfo))
		{
			return GetOrCreatePerFrameMaterialSet(binding, in binding.Uniforms, in binding.Resources);
		}
		TrackBindingForRelease(binding.MaterialId, binding.Id);
		VkDescriptorSetLayout orCreateMaterialSetLayout = shaderLayoutCache.GetOrCreateMaterialSetLayout(binding.Shader);
		VkDescriptorSet vkDescriptorSet;
		try
		{
			vkDescriptorSet = DescriptorSetWriter.AllocateDescriptorSet(device, materialSetPool, orCreateMaterialSetLayout);
		}
		catch (VkResultError)
		{
			vkDescriptorSet = default(VkDescriptorSet);
		}
		if (vkDescriptorSet.IsNull())
		{
			return GetOrCreatePerFrameMaterialSet(binding, in binding.Uniforms, in binding.Resources);
		}
		WriteMaterialBindingToSet(vkDescriptorSet, binding.Shader, in bufferInfo, in binding.Resources, binding.DebugName, binding.WrapModeOverride);
		if (!TryWriteMaterialStorage(vkDescriptorSet, binding.Shader, binding, null, instanced: false, out var transient))
		{
			ScheduleMaterialSetRetirement(vkDescriptorSet);
			return default(VkDescriptorSet);
		}
		if (transient)
		{
			ScheduleMaterialSetRetirement(vkDescriptorSet);
			return vkDescriptorSet;
		}
		materialSetCache[binding.Id] = (resolvedDataVersion, num2, num, vkDescriptorSet);
		return vkDescriptorSet;
	}

	private static int ComputeResourceResidencyGeneration(in MaterialTextureResourceSet<Texture?> resources)
	{
		int num = 0;
		for (int i = 0; i < 16; i++)
		{
			num += resources[i]?.ResidencyGeneration ?? 0;
		}
		return num;
	}

	private VkDescriptorSet CreateOverrideMaterialDescriptorSet(FinalDrawItem draw)
	{
		return GetOrCreatePerFrameMaterialSet(draw.Binding, in draw.Uniforms, in draw.Resources, draw.Storage);
	}

	private VkDescriptorSet GetOrCreatePerFrameMaterialSet(MaterialBinding binding, in MaterialUniformBytes uniforms, in MaterialTextureResourceSet<Texture?> resources, InstanceStorageBinding[]? drawStorage = null)
	{
		Shader shader = binding.Shader;
		TextureWrapMode? wrapModeOverride = binding.WrapModeOverride;
		MaterialStorageArray[] storageArrays = (shader.MaterialLayout.HasStorageBuffers ? binding.StorageArrays : null);
		bool flag = drawStorage == null || !shader.MaterialLayout.HasStorageBuffers;
		PerFrameMaterialSetLookup lookup = new PerFrameMaterialSetLookup(shader, in uniforms, in resources, wrapModeOverride, storageArrays);
		if (flag && perFrameMaterialSetCache.TryGetValueBy(in lookup, out var value))
		{
			return value;
		}
		VkDescriptorSetLayout orCreateMaterialSetLayout = shaderLayoutCache.GetOrCreateMaterialSetLayout(shader);
		VkDescriptorSet vkDescriptorSet = vkDrawEncoder.AllocateDescriptorSet(orCreateMaterialSetLayout);
		if (vkDescriptorSet.IsNull())
		{
			return default(VkDescriptorSet);
		}
		if (!vkDrawEncoder.TryUploadUniforms(in uniforms, out var bufferInfo))
		{
			return default(VkDescriptorSet);
		}
		WriteMaterialBindingToSet(vkDescriptorSet, shader, in bufferInfo, in resources, null, wrapModeOverride);
		if (!TryWriteMaterialStorage(vkDescriptorSet, shader, binding, drawStorage, instanced: false, out var _))
		{
			return default(VkDescriptorSet);
		}
		if (flag)
		{
			perFrameMaterialSetCache.SetBy(in lookup, vkDescriptorSet);
		}
		return vkDescriptorSet;
	}

	private void CollectRetiredMaterialResources()
	{
		int materialId;
		while (materialReleaseListener.TryDequeueReleased(out materialId))
		{
			RetireMaterialResources(materialId);
		}
		persistentMaterialUbos.CollectRetired(currentFrameNumber);
		persistentMaterialStorage.CollectRetired(currentFrameNumber);
		for (int num = pendingMaterialSetRetirements.Count - 1; num >= 0; num--)
		{
			if (pendingMaterialSetRetirements[num].RetireAfterFrame <= currentFrameNumber)
			{
				FreeDescriptorSet(materialSetPool, pendingMaterialSetRetirements[num].Set);
				pendingMaterialSetRetirements.RemoveAt(num);
			}
		}
	}

	private void TrackBindingForRelease(int materialId, int bindingId)
	{
		if (!bindingsByMaterial.TryGetValue(materialId, out HashSet<int> value))
		{
			value = new HashSet<int>();
			bindingsByMaterial[materialId] = value;
		}
		value.Add(bindingId);
	}

	private void RetireMaterialResources(int materialId)
	{
		if (storagePropertiesByMaterial.Remove(materialId, out HashSet<int> value))
		{
			foreach (int item in value)
			{
				persistentMaterialStorage.ScheduleRemove(MaterialStorageKey(materialId, item), currentFrameNumber, currentFramesInFlight);
			}
		}
		if (!bindingsByMaterial.Remove(materialId, out HashSet<int> value2))
		{
			return;
		}
		foreach (int item2 in value2)
		{
			if (materialSetCache.Remove(item2, out (long, int, int, VkDescriptorSet) value3) && !value3.Item4.IsNull())
			{
				ScheduleMaterialSetRetirement(value3.Item4);
			}
			persistentMaterialUbos.ScheduleRemove(item2, currentFrameNumber, currentFramesInFlight);
		}
	}

	private static long MaterialStorageKey(int materialId, int propertyId)
	{
		return ((long)materialId << 32) | (uint)propertyId;
	}

	private void ScheduleMaterialSetRetirement(VkDescriptorSet set)
	{
		if (!set.IsNull())
		{
			pendingMaterialSetRetirements.Add(new PendingMaterialSetRetirement(set, currentFrameNumber + currentFramesInFlight));
		}
	}

	private unsafe void FreeDescriptorSet(VkDescriptorPool descriptorPool, VkDescriptorSet descriptorSet)
	{
		VkDescriptorSet* ptr = stackalloc VkDescriptorSet[1];
		*ptr = descriptorSet;
		VkResult vkResult = device.PInvoke.vkFreeDescriptorSets.Invoke(device.Handle, descriptorPool, 1, ptr);
		if (vkResult != 0)
		{
			throw new VkResultError(vkResult, $"Vulkan call to vkFreeDescriptorSets failed with error: {vkResult}");
		}
	}

	private void WriteMaterialBindingToSet(VkDescriptorSet set, Shader shader, in VkDescriptorBufferInfo bufferInfo, in MaterialTextureResourceSet<Texture?> resources, string? bindingDebugName = null, TextureWrapMode? wrapOverride = null)
	{
		MaterialTextureSlot[] textureSlots = shader.MaterialLayout.TextureSlots;
		Dictionary<string, TextureWrapMode> textureWrapOverrides = shader.Program.textureWrapOverrides;
		Span<VkDescriptorImageInfo> span = stackalloc VkDescriptorImageInfo[textureSlots.Length];
		for (int i = 0; i < textureSlots.Length; i++)
		{
			TextureWrapMode? wrapOverride2 = wrapOverride;
			if (textureWrapOverrides != null && textureWrapOverrides.TryGetValue(textureSlots[i].PropertyName, out var value))
			{
				wrapOverride2 = value;
			}
			if (textureSlots[i].Kind == MaterialTextureKind.Sampler)
			{
				span[i] = new VkDescriptorImageInfo
				{
					Sampler = ResolveTextureBinding(resources[0], isArray: false, wrapOverride2).Sampler
				};
			}
			else
			{
				span[i] = ResolveTextureBinding(resources[textureSlots[i].ResourceSlot], isArray: false, wrapOverride2, textureSlots[i].Default);
			}
		}
		DescriptorSetWriter.WriteMaterialDescriptorSet(device, set, textureSlots, in bufferInfo, span);
	}

	private VkDescriptorSet CreateFrameDescriptorSet(in CameraView view, Vector3 sunDirection)
	{
		GlobalVertexUniforms globalVertexUniforms = default(GlobalVertexUniforms);
		globalVertexUniforms.ViewProj = view.ViewProjection;
		GlobalVertexUniforms value = globalVertexUniforms;
		if (!vkDrawEncoder.TryUploadUniforms(in value, out var bufferInfo))
		{
			return default(VkDescriptorSet);
		}
		GlobalFragmentUniforms globalFragmentUniforms = default(GlobalFragmentUniforms);
		globalFragmentUniforms.SunDirection = new Vector4(sunDirection.X, sunDirection.Y, sunDirection.Z, 0f);
		globalFragmentUniforms.WorldToCamera = view.View;
		globalFragmentUniforms.ViewProj = view.ViewProjection;
		GlobalFragmentUniforms value2 = globalFragmentUniforms;
		if (!vkDrawEncoder.TryUploadUniforms(in value2, out var bufferInfo2))
		{
			return default(VkDescriptorSet);
		}
		VkDescriptorSet vkDescriptorSet = vkDrawEncoder.AllocateDescriptorSet(frameDescriptorSetLayout);
		if (vkDescriptorSet.IsNull())
		{
			return default(VkDescriptorSet);
		}
		DescriptorSetWriter.WriteFrameDescriptorSet(device, vkDescriptorSet, in bufferInfo, in bufferInfo2);
		return vkDescriptorSet;
	}

	private VkDescriptorImageInfo ResolveTextureBinding(Texture? texture, bool isArray = false, TextureWrapMode? wrapOverride = null, MaterialTextureDefault fallback = MaterialTextureDefault.White)
	{
		bool flag = !wrapOverride.HasValue;
		if (flag)
		{
			VkDescriptorImageInfo value;
			if (texture == null)
			{
				ref VkDescriptorImageInfo? reference = ref isArray ? ref frameNullArrayImageInfoCache : ref frameNullImageInfoCache[(uint)fallback];
				if (reference.HasValue)
				{
					return reference.Value;
				}
			}
			else if (frameImageInfoCache.TryGetValue(texture, out value))
			{
				return value;
			}
		}
		Texture texture2 = texture ?? GetFallbackTexture(isArray, fallback);
		if (!GpuTextureService.TryGetSampledImageInfo(texture2, out var imageView, out var imageLayout) || imageLayout == VkImageLayout.Undefined)
		{
			texture2 = GetFallbackTexture(isArray, fallback);
			if (!GpuTextureService.TryGetSampledImageInfo(texture2, out imageView, out imageLayout))
			{
				throw new InvalidOperationException("Texture '" + texture2.Name + "' does not have a sampled GPU image.");
			}
		}
		VkDescriptorImageInfo vkDescriptorImageInfo = new VkDescriptorImageInfo();
		vkDescriptorImageInfo.Sampler = GetOrCreateSampler(texture2, wrapOverride ?? texture2.wrapMode);
		vkDescriptorImageInfo.ImageView = imageView;
		vkDescriptorImageInfo.ImageLayout = ((imageLayout == VkImageLayout.Undefined) ? VkImageLayout.ShaderReadOnlyOptimal : imageLayout);
		VkDescriptorImageInfo vkDescriptorImageInfo2 = vkDescriptorImageInfo;
		if (flag)
		{
			if (texture == null)
			{
				if (isArray)
				{
					frameNullArrayImageInfoCache = vkDescriptorImageInfo2;
				}
				else
				{
					frameNullImageInfoCache[(uint)fallback] = vkDescriptorImageInfo2;
				}
			}
			else
			{
				frameImageInfoCache[texture] = vkDescriptorImageInfo2;
			}
		}
		return vkDescriptorImageInfo2;
	}

	private Texture GetFallbackTexture(bool isArray, MaterialTextureDefault fallback)
	{
		if (isArray)
		{
			return fallbackWhiteArrayTexture ?? (fallbackWhiteArrayTexture = GpuTextureService.GetOrCreateWhiteArrayFallbackTexture());
		}
		if (fallback != 0)
		{
			return GpuTextureService.GetOrCreateFallbackTexture(fallback);
		}
		return fallbackWhiteTexture ?? (fallbackWhiteTexture = GpuTextureService.GetOrCreateWhiteFallbackTexture());
	}

	private VkSampler GetOrCreateSampler(Texture? texture, TextureWrapMode wrapMode)
	{
		FilterMode filterMode = texture?.filterMode ?? FilterMode.Bilinear;
		int num = ((texture is Texture2D texture2D) ? System.Math.Max(0, texture2D.minimumMipmapLevel) : 0);
		float num2 = (Platform.IsMacOS ? 0f : (texture?.mipMapBias ?? 0f));
		float num3 = ResolveMaxSamplerAnisotropy(System.Math.Max(1, texture?.anisoLevel ?? 1));
		ShaderModuleCache.MeshSamplerKey key = new ShaderModuleCache.MeshSamplerKey(filterMode, wrapMode, num, BitConverter.SingleToInt32Bits(num2), BitConverter.SingleToInt32Bits(num3));
		if (samplers.TryGetValue(key, out var value))
		{
			return value;
		}
		VkSampler vkSampler = device.CreateSampler(new VkSamplerCreateInfo
		{
			MagFilter = ((key.FilterMode != 0) ? VkFilter.Linear : VkFilter.Nearest),
			MinFilter = ((key.FilterMode != 0) ? VkFilter.Linear : VkFilter.Nearest),
			MipmapMode = ((key.FilterMode == FilterMode.Trilinear) ? VkSamplerMipmapMode.Linear : VkSamplerMipmapMode.Nearest),
			AddressModeU = ToAddressMode(key.WrapMode),
			AddressModeV = ToAddressMode(key.WrapMode),
			AddressModeW = VkSamplerAddressMode.ClampToEdge,
			MinLod = num,
			MaxLod = float.MaxValue,
			MaxAnisotropy = num3,
			AnisotropyEnable = (num3 > 1f),
			MipLodBias = num2,
			BorderColor = VkBorderColor.FloatOpaqueBlack
		}, device.VkAllocator);
		samplers.Add(key, vkSampler);
		return vkSampler;
	}

	private float GetDeviceMaxSamplerAnisotropy()
	{
		if (deviceMaxSamplerAnisotropy >= 0f)
		{
			return deviceMaxSamplerAnisotropy;
		}
		if (!(this.renderContext is RenderContext renderContext))
		{
			return deviceMaxSamplerAnisotropy = 1f;
		}
		if (!renderContext.PhysicalDevice.GetFeatures().SamplerAnisotropy)
		{
			return deviceMaxSamplerAnisotropy = 1f;
		}
		float maxSamplerAnisotropy = renderContext.PhysicalDevice.GetProperties().Limits.MaxSamplerAnisotropy;
		return deviceMaxSamplerAnisotropy = ((maxSamplerAnisotropy <= 1f) ? 1f : maxSamplerAnisotropy);
	}

	private float ResolveMaxSamplerAnisotropy(int requestedAnisotropy)
	{
		if (requestedAnisotropy <= 1)
		{
			return 1f;
		}
		return System.Math.Min(requestedAnisotropy, GetDeviceMaxSamplerAnisotropy());
	}

	private unsafe VkDescriptorSetLayout CreateFrameDescriptorSetLayout()
	{
		VkDescriptorSetLayoutBinding* ptr = stackalloc VkDescriptorSetLayoutBinding[2];
		*ptr = CreateDescriptorSetLayoutBinding(0u, VkDescriptorType.UniformBuffer, VkShaderStageFlags.VertexBit);
		ptr[1] = CreateDescriptorSetLayoutBinding(1u, VkDescriptorType.UniformBuffer, VkShaderStageFlags.VertexBit | VkShaderStageFlags.FragmentBit);
		DeviceEx handle = device;
		VkDescriptorSetLayoutCreateInfo pCreateInfo = new VkDescriptorSetLayoutCreateInfo
		{
			BindingCount = 2,
			Bindings = ptr
		};
		return handle.CreateDescriptorSetLayout(in pCreateInfo, device.VkAllocator);
	}

	private VkDescriptorSetLayout CreateReservedDescriptorSetLayout()
	{
		return device.CreateDescriptorSetLayout(new VkDescriptorSetLayoutCreateInfo(), device.VkAllocator);
	}

	private static VkDescriptorSetLayoutBinding CreateDescriptorSetLayoutBinding(uint binding, VkDescriptorType descriptorType, VkShaderStageFlags stageFlags)
	{
		VkDescriptorSetLayoutBinding result = new VkDescriptorSetLayoutBinding();
		result.Binding = (int)binding;
		result.DescriptorType = descriptorType;
		result.DescriptorCount = 1;
		result.StageFlags = stageFlags;
		return result;
	}

	private static void LogDiagnostic(string? diagnosticPrefix, string message)
	{
		if (diagnosticPrefix != null)
		{
			LogCategory log = RenderLog.Log;
			bool shouldAppend;
			LogString<Warning> message2 = new LogString<Warning>(1, 2, log, out shouldAppend);
			if (shouldAppend)
			{
				message2.AppendFormatted(diagnosticPrefix);
				message2.AppendLiteral(" ");
				message2.AppendFormatted(message);
			}
			log.Warning(message2, "LogDiagnostic", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Rendering\\DrawCallCore\\MeshBatchRenderer.cs", 1512);
		}
	}

	private static string FormatDraw(FinalDrawItem draw)
	{
		return $"mesh='{draw.Mesh.DebugName}' material='{draw.Binding.DebugName}' shader='{draw.Binding.Shader.Name}' passId={draw.PassId} sortKey={draw.SortKey}";
	}

	private static string CreateDrawDebugLabel(FinalDrawItem draw, int instanceCount)
	{
		return string.Empty;
	}

	private static void BeginDebugLabel(CommandBuffer commandBuffer, string label, float4 color)
	{
	}

	private static void EndDebugLabel(CommandBuffer commandBuffer)
	{
	}

	private static VkSamplerAddressMode ToAddressMode(TextureWrapMode wrapMode)
	{
		return wrapMode switch
		{
			TextureWrapMode.Clamp => VkSamplerAddressMode.ClampToEdge, 
			TextureWrapMode.Mirror => VkSamplerAddressMode.MirroredRepeat, 
			_ => VkSamplerAddressMode.Repeat, 
		};
	}

	private static int ResolveUniformBufferAlignment(IRenderContext renderContext)
	{
		return ResolveBufferAlignment(renderContext, storage: false);
	}

	private static int ResolveStorageBufferAlignment(IRenderContext renderContext)
	{
		return ResolveBufferAlignment(renderContext, storage: true);
	}

	private static int ResolveBufferAlignment(IRenderContext renderContext, bool storage)
	{
		if (!(renderContext is RenderContext renderContext2))
		{
			return 256;
		}
		VkPhysicalDeviceLimits limits = renderContext2.PhysicalDevice.GetProperties().Limits;
		int num = (storage ? limits.MinStorageBufferOffsetAlignment : limits.MinUniformBufferOffsetAlignment);
		if (num <= 0)
		{
			return 256;
		}
		return System.Math.Max(num, 16);
	}

	private unsafe VkDescriptorPool CreateMaterialDescriptorPool()
	{
		VkDescriptorPoolSize* ptr = stackalloc VkDescriptorPoolSize[5];
		*ptr = new VkDescriptorPoolSize
		{
			Type = VkDescriptorType.UniformBuffer,
			DescriptorCount = 16384
		};
		ptr[1] = new VkDescriptorPoolSize
		{
			Type = VkDescriptorType.CombinedImageSampler,
			DescriptorCount = 262144
		};
		ptr[2] = new VkDescriptorPoolSize
		{
			Type = VkDescriptorType.StorageBuffer,
			DescriptorCount = 131072
		};
		ptr[3] = new VkDescriptorPoolSize
		{
			Type = VkDescriptorType.SampledImage,
			DescriptorCount = 262144
		};
		ptr[4] = new VkDescriptorPoolSize
		{
			Type = VkDescriptorType.Sampler,
			DescriptorCount = 16384
		};
		DeviceEx handle = device;
		VkDescriptorPoolCreateInfo pCreateInfo = new VkDescriptorPoolCreateInfo
		{
			Flags = VkDescriptorPoolCreateFlags.FreeDescriptorSetBit,
			MaxSets = 16384,
			PoolSizeCount = 5,
			PoolSizes = ptr
		};
		return handle.CreateDescriptorPool(in pCreateInfo, device.VkAllocator);
	}
}
