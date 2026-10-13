using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Brutal;
using Brutal.ImGuiApi;
using Brutal.Pointers.Extensions;
using Brutal.VulkanApi;
using Brutal.VulkanApi.Abstractions;
using Brutal.VulkanApi.Vma;
using UnityStubs;
using Waylith.Graphics;
using Waylith.Graphics.Meshes;
using Waylith.Rendering;

namespace RimWorldWaylith.Imgui;

internal sealed class ImGuiMaterialRenderer : IDisposable
{
	private readonly struct ShaderPipeline
	{
		public readonly VkPipeline Pipeline;

		public readonly VkPipelineLayout Layout;

		public readonly VkDescriptorSetLayout MaterialSetLayout;

		public ShaderPipeline(VkPipeline pipeline, VkPipelineLayout layout, VkDescriptorSetLayout materialSetLayout)
		{
			Pipeline = pipeline;
			Layout = layout;
			MaterialSetLayout = materialSetLayout;
		}
	}

	private struct FrameScratch
	{
		public DescriptorPoolEx DescriptorPool;

		public BufferAllocationInfo UboBuffer;

		public ByteSize UboSize;

		public int UboWritten;

		public BufferAllocationInfo StorageBuffer;

		public unsafe byte* StoragePointer;

		public int StorageWritten;
	}

	private readonly DeviceEx device;

	private readonly VmaAllocator allocator;

	private readonly VulkanShaderLayoutCache layoutCache;

	private readonly ShaderModuleCache shaderModules;

	private readonly VkShaderModule vertModule;

	private readonly int subPass;

	private readonly VkSampleCountFlags sampleCount;

	private readonly VkSampler sampler;

	private readonly VkSampler mippedSampler;

	private readonly VkDescriptorSetLayout emptySetLayout;

	private readonly VkDescriptorSetLayout frameSetLayout;

	private ByteSize uboAlignment;

	private BufferAllocationInfo vertexDefaults;

	private const int NormalDefaultOffset = 0;

	private const int TangentDefaultOffset = 16;

	private const int ZeroDefaultOffset = 32;

	private const int FrameBlockBytes = 144;

	private const int FrameWorldToCameraOffset = 16;

	private const int FrameViewProjOffset = 80;

	private readonly Dictionary<Shader, ShaderPipeline> pipelines = new Dictionary<Shader, ShaderPipeline>(ReferenceEqualityComparer.Instance);

	private readonly Dictionary<(bool mipped, TextureWrapMode wrap), VkSampler> wrapSamplers = new Dictionary<(bool, TextureWrapMode), VkSampler>();

	private readonly FrameScratch[] frames;

	private const int MaterialDrawsPerFrame = 256;

	private const int UboSlotBytes = 512;

	private const int StorageBytesPerFrame = 1048576;

	private const int ZeroStorageBytes = 4096;

	private readonly int storageAlignment;

	public ImGuiMaterialRenderer(GraphicsDevice graphicsDevice, VulkanShaderLayoutCache layoutCache, ShaderModuleCache shaderModules, VkShaderModule vertModule, int subPass, VkSampleCountFlags sampleCount, VkSampler sampler, VkSampler mippedSampler, int framesInFlight)
	{
		device = graphicsDevice.Device;
		allocator = graphicsDevice.Allocator;
		this.layoutCache = layoutCache;
		this.shaderModules = shaderModules;
		this.vertModule = vertModule;
		this.subPass = subPass;
		this.sampleCount = sampleCount;
		this.sampler = sampler;
		this.mippedSampler = mippedSampler;
		emptySetLayout = device.CreateDescriptorSetLayout(new DescriptorSetLayoutEx.CreateInfo
		{
			Bindings = default(Span<VkDescriptorSetLayoutBinding>)
		}, null);
		DeviceEx deviceEx = device;
		DescriptorSetLayoutEx.CreateInfo createInfo = default(DescriptorSetLayoutEx.CreateInfo);
		VkDescriptorSetLayoutBinding reference = new VkDescriptorSetLayoutBinding
		{
			Binding = 1,
			DescriptorType = VkDescriptorType.UniformBuffer,
			DescriptorCount = 1,
			StageFlags = (VkShaderStageFlags.VertexBit | VkShaderStageFlags.FragmentBit)
		};
		createInfo.Bindings = new Span<VkDescriptorSetLayoutBinding>(ref reference);
		frameSetLayout = deviceEx.CreateDescriptorSetLayout(createInfo, null);
		VkPhysicalDeviceLimits limits = device.PhysicalDevice.GetProperties().Limits;
		uboAlignment = limits.MinUniformBufferOffsetAlignment;
		storageAlignment = Math.Max(16, limits.MinStorageBufferOffsetAlignment);
		frames = new FrameScratch[Math.Max(1, framesInFlight)];
	}

	public void BeginFrame(int frameSlot)
	{
		ref FrameScratch reference = ref frames[frameSlot % frames.Length];
		if (reference.DescriptorPool != null)
		{
			device.ResetDescriptorPool(reference.DescriptorPool, 0);
		}
		reference.UboWritten = 0;
		reference.StorageWritten = AlignStorage(4096);
	}

	public unsafe bool TryRecord(CommandBuffer commandBuffer, VkRenderPass renderPass, int frameSlot, in ImGuiMaterialDraw draw, ReadOnlySpan<float> scale, ReadOnlySpan<float> translate, int indexCount, int indexOffset, int vertexOffset)
	{
		Shader shader = draw.Material.Shader;
		if (shader?.Program == null)
		{
			return false;
		}
		if (!TryGetPipeline(shader, renderPass, out var sp))
		{
			return false;
		}
		int frameSlot2 = frameSlot % frames.Length;
		if (!TryBuildMaterialSet(frameSlot2, shader, in draw, sp.MaterialSetLayout, out var set))
		{
			return false;
		}
		if (shader.PrecompiledStages != null)
		{
			if (!TryBuildFrameSet(frameSlot2, scale, translate, out var set2))
			{
				return false;
			}
			commandBuffer.BindPipeline(VkPipelineBindPoint.Graphics, sp.Pipeline);
			Matrix4x4 identity = Matrix4x4.Identity;
			commandBuffer.PushConstants(sp.Layout, VkShaderStageFlags.VertexBit, 0, new ReadOnlySpan<byte>(&identity, sizeof(Matrix4x4)));
			VkDescriptorSet reference = set2;
			Span<VkDescriptorSet> span = new Span<VkDescriptorSet>(ref reference);
			commandBuffer.BindDescriptorSets(VkPipelineBindPoint.Graphics, sp.Layout, 0, span, default(Span<ByteSize32>));
			commandBuffer.BindVertexBuffer(1, vertexDefaults.VkBuffer);
		}
		else
		{
			commandBuffer.BindPipeline(VkPipelineBindPoint.Graphics, sp.Pipeline);
			commandBuffer.PushConstants(sp.Layout, VkShaderStageFlags.VertexBit, 0, MemoryMarshal.AsBytes(scale));
			commandBuffer.PushConstants(sp.Layout, VkShaderStageFlags.VertexBit, 8, MemoryMarshal.AsBytes(translate));
		}
		VkDescriptorSet reference2 = set;
		Span<VkDescriptorSet> span2 = new Span<VkDescriptorSet>(ref reference2);
		commandBuffer.BindDescriptorSets(VkPipelineBindPoint.Graphics, sp.Layout, 2, span2, default(Span<ByteSize32>));
		commandBuffer.DrawIndexed(indexCount, 1, indexOffset, vertexOffset, 0);
		return true;
	}

	private bool TryGetPipeline(Shader shader, VkRenderPass renderPass, out ShaderPipeline sp)
	{
		if (pipelines.TryGetValue(shader, out sp))
		{
			return !sp.Pipeline.IsNull();
		}
		ShaderVariantKey variant = new ShaderVariantKey(shader, 0L, "", useTexture: true);
		bool flag = shader.PrecompiledStages != null;
		bool usedFallback;
		VkShaderModule vert = (flag ? shaderModules.GetOrCreateVertexShader(variant, out usedFallback) : vertModule);
		VkShaderModule frag = (shader.HasErrors ? shaderModules.GetOrCreateMagentaFragmentShader() : shaderModules.GetOrCreateFragmentShader(variant));
		VkDescriptorSetLayout orCreateMaterialSetLayout = layoutCache.GetOrCreateMaterialSetLayout(shader);
		VkPipelineLayout layout = CreatePipelineLayout(orCreateMaterialSetLayout, flag);
		if (flag)
		{
			EnsureVertexDefaults();
		}
		VkPipeline pipeline = ((frag.IsNull() || vert.IsNull()) ? default(VkPipeline) : CreatePipeline(vert, frag, flag ? shader.VertexInputLayout : null, layout, renderPass, flag ? new PipelineState?(shader.PipelineState) : ((PipelineState?)null)));
		sp = new ShaderPipeline(pipeline, layout, orCreateMaterialSetLayout);
		pipelines[shader] = sp;
		return !pipeline.IsNull();
	}

	private unsafe bool TryBuildFrameSet(int frameSlot, ReadOnlySpan<float> scale, ReadOnlySpan<float> translate, out VkDescriptorSet set)
	{
		set = default(VkDescriptorSet);
		ref FrameScratch reference = ref frames[frameSlot];
		Matrix4x4 value = new Matrix4x4(scale[0], 0f, 0f, translate[0], 0f, scale[1], 0f, translate[1], 0f, 0f, 0f, 0f, 0f, 0f, 0f, 1f);
		Matrix4x4 value2 = Matrix4x4.Identity;
		Span<byte> span = stackalloc byte[144];
		span.Clear();
		MemoryMarshal.Write(span.Slice(16), in value2);
		MemoryMarshal.Write(span.Slice(80), in value);
		if (!TryUpload(ref reference, span, out var bufferInfo))
		{
			return false;
		}
		set = DescriptorSetWriter.AllocateDescriptorSet(device, reference.DescriptorPool, frameSetLayout);
		if (set.IsNull())
		{
			return false;
		}
		VkWriteDescriptorSet vkWriteDescriptorSet = new VkWriteDescriptorSet();
		vkWriteDescriptorSet.DstSet = set;
		vkWriteDescriptorSet.DstBinding = 1;
		vkWriteDescriptorSet.DescriptorCount = 1;
		vkWriteDescriptorSet.DescriptorType = VkDescriptorType.UniformBuffer;
		vkWriteDescriptorSet.BufferInfo = &bufferInfo;
		VkWriteDescriptorSet vkWriteDescriptorSet2 = vkWriteDescriptorSet;
		device.UpdateDescriptorSets(new ReadOnlySpan<VkWriteDescriptorSet>(&vkWriteDescriptorSet2, 1), default(ReadOnlySpan<VkCopyDescriptorSet>));
		return true;
	}

	private unsafe bool TryBuildMaterialSet(int frameSlot, Shader shader, in ImGuiMaterialDraw draw, VkDescriptorSetLayout materialSetLayout, out VkDescriptorSet set)
	{
		set = default(VkDescriptorSet);
		ref FrameScratch reference = ref frames[frameSlot];
		MaterialUniformBytes uniforms = draw.Uniforms;
		MaterialTextureResourceSet<Texture> resources = draw.Resources;
		Texture mainTexture = draw.MainTexture;
		int length = Math.Max(16, shader.MaterialLayout.UniformSize);
		if (!TryUpload(ref reference, new ReadOnlySpan<byte>(&uniforms, length), out var bufferInfo))
		{
			return false;
		}
		MaterialTextureSlot[] textureSlots = shader.MaterialLayout.TextureSlots;
		Span<VkDescriptorImageInfo> span = stackalloc VkDescriptorImageInfo[Math.Max(1, textureSlots.Length)];
		for (int i = 0; i < textureSlots.Length; i++)
		{
			if (textureSlots[i].Kind == MaterialTextureKind.Sampler)
			{
				span[i] = new VkDescriptorImageInfo
				{
					Sampler = mippedSampler
				};
				continue;
			}
			Texture texture = ((mainTexture != null && textureSlots[i].PropertyName == "_MainTex") ? mainTexture : resources[textureSlots[i].ResourceSlot]);
			if (!TryResolveTextureBinding(texture, textureSlots[i].Default, out span[i]))
			{
				return false;
			}
		}
		set = DescriptorSetWriter.AllocateDescriptorSet(device, reference.DescriptorPool, materialSetLayout);
		if (set.IsNull())
		{
			return false;
		}
		DescriptorSetWriter.WriteMaterialDescriptorSet(device, set, textureSlots, in bufferInfo, span.Slice(0, textureSlots.Length));
		return TryWriteStorage(ref reference, set, shader.MaterialLayout.StorageBufferSlots, draw.StorageArrays);
	}

	private unsafe bool TryWriteStorage(ref FrameScratch f, VkDescriptorSet set, ReadOnlySpan<MaterialStorageBufferSlot> slots, MaterialStorageArray[] arrays)
	{
		if (slots.Length == 0)
		{
			return true;
		}
		EnsureStorage(ref f);
		Span<int> span = stackalloc int[slots.Length];
		Span<VkDescriptorBufferInfo> span2 = stackalloc VkDescriptorBufferInfo[slots.Length];
		for (int i = 0; i < slots.Length; i++)
		{
			span[i] = slots[i].Binding;
			span2[i] = new VkDescriptorBufferInfo
			{
				Buffer = f.StorageBuffer.VkBuffer,
				Offset = ByteSize.Zero,
				Range = ByteSize.Of(4096)
			};
			for (int j = 0; j < arrays.Length; j++)
			{
				MaterialStorageArray materialStorageArray = arrays[j];
				if (materialStorageArray.PropertyId == slots[i].PropertyId)
				{
					int storageWritten = f.StorageWritten;
					if (storageWritten + materialStorageArray.Bytes.Length > 1048576)
					{
						return false;
					}
					materialStorageArray.Bytes.CopyTo(new Span<byte>(f.StoragePointer + storageWritten, materialStorageArray.Bytes.Length));
					span2[i] = new VkDescriptorBufferInfo
					{
						Buffer = f.StorageBuffer.VkBuffer,
						Offset = ByteSize.Of(storageWritten),
						Range = ByteSize.Of(materialStorageArray.Bytes.Length)
					};
					f.StorageWritten = AlignStorage(storageWritten + materialStorageArray.Bytes.Length);
					break;
				}
			}
		}
		DescriptorSetWriter.WriteMaterialStorageBuffers(device, set, span, span2);
		return true;
	}

	private int AlignStorage(int value)
	{
		return (value + storageAlignment - 1) / storageAlignment * storageAlignment;
	}

	private unsafe void EnsureStorage(ref FrameScratch f)
	{
		if (f.StorageBuffer.VkBuffer.IsNull())
		{
			f.StorageBuffer = ImGuiGpuMemory.CreateHostBuffer(allocator, "ImGui.MaterialStorage", ByteSize.Of(1048576), VkBufferUsageFlags.StorageBufferBit, GpuMemoryCategory.DynamicDraw);
			f.StoragePointer = ImGuiGpuMemory.Map(allocator, in f.StorageBuffer);
			new Span<byte>(f.StoragePointer, 4096).Clear();
			f.StorageWritten = AlignStorage(4096);
		}
	}

	private bool TryResolveTextureBinding(Texture? texture, MaterialTextureDefault fallback, out VkDescriptorImageInfo info)
	{
		info = default(VkDescriptorImageInfo);
		if (texture == null)
		{
			texture = GpuTextureService.GetOrCreateFallbackTexture(fallback);
		}
		if (!GpuTextureService.TryGetSampledImageInfo(texture, out var imageView, out var imageLayout) || imageLayout == VkImageLayout.Undefined)
		{
			return false;
		}
		bool flag = texture is Texture2D texture2D && ImportDefaults.IsOfficialUiTexture(texture2D.SourcePath);
		info = new VkDescriptorImageInfo
		{
			Sampler = SamplerFor(!flag, texture.wrapMode),
			ImageView = imageView,
			ImageLayout = imageLayout
		};
		return true;
	}

	private VkSampler SamplerFor(bool mipped, TextureWrapMode wrap)
	{
		if (wrap == TextureWrapMode.Clamp)
		{
			if (!mipped)
			{
				return sampler;
			}
			return mippedSampler;
		}
		if (wrapSamplers.TryGetValue((mipped, wrap), out var value))
		{
			return value;
		}
		VkSamplerAddressMode vkSamplerAddressMode = ((wrap == TextureWrapMode.Mirror) ? VkSamplerAddressMode.MirroredRepeat : VkSamplerAddressMode.Repeat);
		VkSampler vkSampler = device.CreateSampler(new VkSamplerCreateInfo
		{
			MagFilter = VkFilter.Linear,
			MinFilter = VkFilter.Linear,
			MipmapMode = VkSamplerMipmapMode.Linear,
			AddressModeU = vkSamplerAddressMode,
			AddressModeV = vkSamplerAddressMode,
			AddressModeW = VkSamplerAddressMode.ClampToEdge,
			MaxLod = (mipped ? 1000f : 0f),
			MaxAnisotropy = 1f
		}, null);
		wrapSamplers[(mipped, wrap)] = vkSampler;
		return vkSampler;
	}

	private unsafe bool TryUpload(ref FrameScratch f, ReadOnlySpan<byte> bytes, out VkDescriptorBufferInfo bufferInfo)
	{
		bufferInfo = default(VkDescriptorBufferInfo);
		EnsureUbo(ref f);
		int num = (int)uboAlignment.Bytes;
		num = (512 + num - 1) / num * num;
		int uboWritten = f.UboWritten;
		if ((ulong)(uboWritten + num) > (ulong)f.UboSize.Bytes)
		{
			return false;
		}
		byte* ptr = ImGuiGpuMemory.Map(allocator, in f.UboBuffer);
		bytes.CopyTo(new Span<byte>(ptr + uboWritten, 512));
		ImGuiGpuMemory.Unmap(allocator, in f.UboBuffer);
		bufferInfo = new VkDescriptorBufferInfo
		{
			Buffer = f.UboBuffer.VkBuffer,
			Offset = ByteSize.Of(uboWritten),
			Range = ByteSize.Of(bytes.Length)
		};
		f.UboWritten = uboWritten + num;
		return true;
	}

	private void EnsureUbo(ref FrameScratch f)
	{
		int num = (int)uboAlignment.Bytes;
		num = (512 + num - 1) / num * num;
		ByteSize byteSize = ByteSize.Of(num * 256 * 2);
		if (f.UboBuffer.VkBuffer.IsNull() || f.UboSize.Bytes < byteSize.Bytes)
		{
			if (f.DescriptorPool == null)
			{
				f.DescriptorPool = CreateDescriptorPool();
			}
			ImGuiGpuMemory.Free(allocator, ref f.UboBuffer, GpuMemoryCategory.UniformBuffer);
			f.UboBuffer = ImGuiGpuMemory.CreateHostBuffer(allocator, "ImGui.MaterialUniforms", byteSize, VkBufferUsageFlags.UniformBufferBit, GpuMemoryCategory.UniformBuffer);
			f.UboSize = byteSize;
		}
	}

	private DescriptorPoolEx CreateDescriptorPool()
	{
		DeviceEx deviceEx = device;
		DescriptorPoolEx.CreateInfo createInfo = new DescriptorPoolEx.CreateInfo
		{
			MaxSets = 512
		};
		InlineArray3<VkDescriptorPoolSize> buffer = default(InlineArray3<VkDescriptorPoolSize>);
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray3<VkDescriptorPoolSize>, VkDescriptorPoolSize>(ref buffer, 0) = new VkDescriptorPoolSize
		{
			Type = VkDescriptorType.UniformBuffer,
			DescriptorCount = 512
		};
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray3<VkDescriptorPoolSize>, VkDescriptorPoolSize>(ref buffer, 1) = new VkDescriptorPoolSize
		{
			Type = VkDescriptorType.CombinedImageSampler,
			DescriptorCount = 4096
		};
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray3<VkDescriptorPoolSize>, VkDescriptorPoolSize>(ref buffer, 2) = new VkDescriptorPoolSize
		{
			Type = VkDescriptorType.StorageBuffer,
			DescriptorCount = 2048
		};
		createInfo.PoolSizes = global::_003CPrivateImplementationDetails_003E.InlineArrayAsSpan<InlineArray3<VkDescriptorPoolSize>, VkDescriptorPoolSize>(ref buffer, 3);
		return deviceEx.CreateDescriptorPool(createInfo, null);
	}

	private VkPipelineLayout CreatePipelineLayout(VkDescriptorSetLayout materialSetLayout, bool precompiled)
	{
		VkPushConstantRange vkPushConstantRange = new VkPushConstantRange();
		vkPushConstantRange.StageFlags = VkShaderStageFlags.VertexBit;
		vkPushConstantRange.Offset = ByteSize.Zero;
		vkPushConstantRange.Size = (precompiled ? ByteSize.Of<Matrix4x4>() : ByteSize.Of<float>(4));
		VkPushConstantRange vkPushConstantRange2 = vkPushConstantRange;
		DeviceEx inDevice = device;
		InlineArray3<VkDescriptorSetLayout> buffer = default(InlineArray3<VkDescriptorSetLayout>);
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray3<VkDescriptorSetLayout>, VkDescriptorSetLayout>(ref buffer, 0) = (precompiled ? frameSetLayout : emptySetLayout);
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray3<VkDescriptorSetLayout>, VkDescriptorSetLayout>(ref buffer, 1) = emptySetLayout;
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray3<VkDescriptorSetLayout>, VkDescriptorSetLayout>(ref buffer, 2) = materialSetLayout;
		ReadOnlySpan<VkDescriptorSetLayout> setLayouts = global::_003CPrivateImplementationDetails_003E.InlineArrayAsReadOnlySpan<InlineArray3<VkDescriptorSetLayout>, VkDescriptorSetLayout>(in buffer, 3);
		VkPushConstantRange reference = vkPushConstantRange2;
		return inDevice.CreatePipelineLayout(setLayouts, new ReadOnlySpan<VkPushConstantRange>(in reference), null);
	}

	private unsafe void EnsureVertexDefaults()
	{
		if (vertexDefaults.VkBuffer.IsNull())
		{
			ReadOnlySpan<float> span = RuntimeHelpers.CreateSpan<float>((RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
			ByteSize size = ByteSize.Of(span.Length * 4);
			vertexDefaults = ImGuiGpuMemory.CreateHostBuffer(allocator, "ImGui.VertexDefaults", size, VkBufferUsageFlags.VertexBufferBit, GpuMemoryCategory.DynamicDraw);
			byte* pointer = ImGuiGpuMemory.Map(allocator, in vertexDefaults);
			MemoryMarshal.AsBytes(span).CopyTo(new Span<byte>(pointer, (int)size.Bytes));
			ImGuiGpuMemory.Unmap(allocator, in vertexDefaults);
		}
	}

	private static int FillUnityAttributes(ShaderVertexInputLayout layout, Span<VkVertexInputAttributeDescription> attrs)
	{
		int result = 0;
		foreach (ShaderVertexInputSlot slot in layout.Slots)
		{
			var (binding, format, byteSize) = slot.semantic switch
			{
				VertexAttributeSemantic.Position => (0, VkFormat.R32G32SFloat, ByteSize.OffsetOf<ImDrawVert>("pos")), 
				VertexAttributeSemantic.TexCoord0 => (0, VkFormat.R32G32SFloat, ByteSize.OffsetOf<ImDrawVert>("uv")), 
				VertexAttributeSemantic.Color => (0, VkFormat.R8G8B8A8UNorm, ByteSize.OffsetOf<ImDrawVert>("col")), 
				VertexAttributeSemantic.Normal => (1, VkFormat.R32G32B32A32SFloat, ByteSize.Of(0)), 
				VertexAttributeSemantic.Tangent => (1, VkFormat.R32G32B32A32SFloat, ByteSize.Of(16)), 
				_ => (1, VkFormat.R32G32B32A32SFloat, ByteSize.Of(32)), 
			};
			attrs[result++] = new VkVertexInputAttributeDescription
			{
				Location = slot.location,
				Binding = binding,
				Format = format,
				Offset = byteSize
			};
		}
		return result;
	}

	private unsafe VkPipeline CreatePipeline(VkShaderModule vert, VkShaderModule frag, ShaderVertexInputLayout? unityLayout, VkPipelineLayout layout, VkRenderPass renderPass, PipelineState? unityState)
	{
		ReadOnlySpan<byte> span = "main"u8;
		InlineArray2<VkPipelineShaderStageCreateInfo> buffer = default(InlineArray2<VkPipelineShaderStageCreateInfo>);
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray2<VkPipelineShaderStageCreateInfo>, VkPipelineShaderStageCreateInfo>(ref buffer, 0) = new VkPipelineShaderStageCreateInfo
		{
			Stage = VkShaderStageFlags.VertexBit,
			Module = vert,
			Name = span.AsPointer()
		};
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray2<VkPipelineShaderStageCreateInfo>, VkPipelineShaderStageCreateInfo>(ref buffer, 1) = new VkPipelineShaderStageCreateInfo
		{
			Stage = VkShaderStageFlags.FragmentBit,
			Module = frag,
			Name = span.AsPointer()
		};
		Span<VkPipelineShaderStageCreateInfo> span2 = global::_003CPrivateImplementationDetails_003E.InlineArrayAsSpan<InlineArray2<VkPipelineShaderStageCreateInfo>, VkPipelineShaderStageCreateInfo>(ref buffer, 2);
		InlineArray2<VkVertexInputBindingDescription> buffer2 = default(InlineArray2<VkVertexInputBindingDescription>);
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray2<VkVertexInputBindingDescription>, VkVertexInputBindingDescription>(ref buffer2, 0) = new VkVertexInputBindingDescription
		{
			Binding = 0,
			Stride = ByteSize.Of(sizeof(ImDrawVert)),
			InputRate = VkVertexInputRate.Vertex
		};
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray2<VkVertexInputBindingDescription>, VkVertexInputBindingDescription>(ref buffer2, 1) = new VkVertexInputBindingDescription
		{
			Binding = 1,
			Stride = ByteSize.Zero,
			InputRate = VkVertexInputRate.Vertex
		};
		Span<VkVertexInputBindingDescription> span3 = global::_003CPrivateImplementationDetails_003E.InlineArrayAsSpan<InlineArray2<VkVertexInputBindingDescription>, VkVertexInputBindingDescription>(ref buffer2, 2);
		Span<VkVertexInputAttributeDescription> span4 = stackalloc VkVertexInputAttributeDescription[Math.Max(3, unityLayout?.Slots.Count ?? 0)];
		int vertexAttributeDescriptionCount;
		if (unityLayout != null)
		{
			vertexAttributeDescriptionCount = FillUnityAttributes(unityLayout, span4);
		}
		else
		{
			span4[0] = new VkVertexInputAttributeDescription
			{
				Location = 0,
				Binding = 0,
				Format = VkFormat.R32G32SFloat,
				Offset = ByteSize.OffsetOf<ImDrawVert>("pos")
			};
			span4[1] = new VkVertexInputAttributeDescription
			{
				Location = 1,
				Binding = 0,
				Format = VkFormat.R32G32SFloat,
				Offset = ByteSize.OffsetOf<ImDrawVert>("uv")
			};
			span4[2] = new VkVertexInputAttributeDescription
			{
				Location = 2,
				Binding = 0,
				Format = VkFormat.R8G8B8A8UNorm,
				Offset = ByteSize.OffsetOf<ImDrawVert>("col")
			};
			vertexAttributeDescriptionCount = 3;
		}
		VkPipelineVertexInputStateCreateInfo vkPipelineVertexInputStateCreateInfo = new VkPipelineVertexInputStateCreateInfo();
		vkPipelineVertexInputStateCreateInfo.VertexBindingDescriptionCount = ((unityLayout == null) ? 1 : 2);
		vkPipelineVertexInputStateCreateInfo.VertexBindingDescriptions = span3.AsPointer();
		vkPipelineVertexInputStateCreateInfo.VertexAttributeDescriptionCount = vertexAttributeDescriptionCount;
		vkPipelineVertexInputStateCreateInfo.VertexAttributeDescriptions = span4.AsPointer();
		VkPipelineVertexInputStateCreateInfo vkPipelineVertexInputStateCreateInfo2 = vkPipelineVertexInputStateCreateInfo;
		VkPipelineInputAssemblyStateCreateInfo vkPipelineInputAssemblyStateCreateInfo = new VkPipelineInputAssemblyStateCreateInfo();
		vkPipelineInputAssemblyStateCreateInfo.Topology = VkPrimitiveTopology.TriangleList;
		VkPipelineInputAssemblyStateCreateInfo vkPipelineInputAssemblyStateCreateInfo2 = vkPipelineInputAssemblyStateCreateInfo;
		VkPipelineViewportStateCreateInfo vkPipelineViewportStateCreateInfo = new VkPipelineViewportStateCreateInfo();
		vkPipelineViewportStateCreateInfo.ViewportCount = 1;
		vkPipelineViewportStateCreateInfo.ScissorCount = 1;
		VkPipelineViewportStateCreateInfo vkPipelineViewportStateCreateInfo2 = vkPipelineViewportStateCreateInfo;
		VkPipelineRasterizationStateCreateInfo vkPipelineRasterizationStateCreateInfo = new VkPipelineRasterizationStateCreateInfo();
		vkPipelineRasterizationStateCreateInfo.PolygonMode = VkPolygonMode.Fill;
		vkPipelineRasterizationStateCreateInfo.CullMode = VkCullModeFlags.None;
		vkPipelineRasterizationStateCreateInfo.FrontFace = VkFrontFace.CounterClockwise;
		vkPipelineRasterizationStateCreateInfo.LineWidth = 1f;
		VkPipelineRasterizationStateCreateInfo vkPipelineRasterizationStateCreateInfo2 = vkPipelineRasterizationStateCreateInfo;
		VkPipelineMultisampleStateCreateInfo vkPipelineMultisampleStateCreateInfo = new VkPipelineMultisampleStateCreateInfo();
		vkPipelineMultisampleStateCreateInfo.RasterizationSamples = sampleCount;
		VkPipelineMultisampleStateCreateInfo vkPipelineMultisampleStateCreateInfo2 = vkPipelineMultisampleStateCreateInfo;
		BlendState blendState = unityState?.Blend ?? BlendState.Alpha;
		VkPipelineColorBlendAttachmentState vkPipelineColorBlendAttachmentState = new VkPipelineColorBlendAttachmentState();
		vkPipelineColorBlendAttachmentState.BlendEnable = blendState.Enabled;
		vkPipelineColorBlendAttachmentState.SrcColorBlendFactor = ToVkBlendFactor(blendState.SrcColor);
		vkPipelineColorBlendAttachmentState.DstColorBlendFactor = ToVkBlendFactor(blendState.DstColor);
		vkPipelineColorBlendAttachmentState.ColorBlendOp = ToVkBlendOp(blendState.ColorOp);
		vkPipelineColorBlendAttachmentState.SrcAlphaBlendFactor = ToVkBlendFactor(blendState.SrcAlpha);
		vkPipelineColorBlendAttachmentState.DstAlphaBlendFactor = ToVkBlendFactor(blendState.DstAlpha);
		vkPipelineColorBlendAttachmentState.AlphaBlendOp = ToVkBlendOp(blendState.AlphaOp);
		vkPipelineColorBlendAttachmentState.ColorWriteMask = ((!unityState.HasValue || unityState.GetValueOrDefault().ColorWrite) ? (VkColorComponentFlags.RBit | VkColorComponentFlags.GBit | VkColorComponentFlags.BBit | VkColorComponentFlags.ABit) : VkColorComponentFlags.None);
		VkPipelineColorBlendAttachmentState vkPipelineColorBlendAttachmentState2 = vkPipelineColorBlendAttachmentState;
		VkPipelineDepthStencilStateCreateInfo vkPipelineDepthStencilStateCreateInfo = new VkPipelineDepthStencilStateCreateInfo();
		VkPipelineColorBlendStateCreateInfo vkPipelineColorBlendStateCreateInfo = new VkPipelineColorBlendStateCreateInfo();
		vkPipelineColorBlendStateCreateInfo.AttachmentCount = 1;
		vkPipelineColorBlendStateCreateInfo.Attachments = &vkPipelineColorBlendAttachmentState2;
		VkPipelineColorBlendStateCreateInfo vkPipelineColorBlendStateCreateInfo2 = vkPipelineColorBlendStateCreateInfo;
		InlineArray2<VkDynamicState> buffer3 = default(InlineArray2<VkDynamicState>);
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray2<VkDynamicState>, VkDynamicState>(ref buffer3, 0) = VkDynamicState.Viewport;
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray2<VkDynamicState>, VkDynamicState>(ref buffer3, 1) = VkDynamicState.Scissor;
		Span<VkDynamicState> span5 = global::_003CPrivateImplementationDetails_003E.InlineArrayAsSpan<InlineArray2<VkDynamicState>, VkDynamicState>(ref buffer3, 2);
		VkPipelineDynamicStateCreateInfo vkPipelineDynamicStateCreateInfo = new VkPipelineDynamicStateCreateInfo();
		vkPipelineDynamicStateCreateInfo.DynamicStateCount = span5.Length;
		vkPipelineDynamicStateCreateInfo.DynamicStates = span5.AsPointer();
		VkPipelineDynamicStateCreateInfo vkPipelineDynamicStateCreateInfo2 = vkPipelineDynamicStateCreateInfo;
		DeviceEx handle = device;
		VkGraphicsPipelineCreateInfo reference = new VkGraphicsPipelineCreateInfo
		{
			StageCount = 2,
			Stages = span2.AsPointer(),
			VertexInputState = &vkPipelineVertexInputStateCreateInfo2,
			InputAssemblyState = &vkPipelineInputAssemblyStateCreateInfo2,
			ViewportState = &vkPipelineViewportStateCreateInfo2,
			RasterizationState = &vkPipelineRasterizationStateCreateInfo2,
			MultisampleState = &vkPipelineMultisampleStateCreateInfo2,
			DepthStencilState = &vkPipelineDepthStencilStateCreateInfo,
			ColorBlendState = &vkPipelineColorBlendStateCreateInfo2,
			DynamicState = &vkPipelineDynamicStateCreateInfo2,
			Layout = layout,
			RenderPass = renderPass,
			Subpass = subPass
		};
		VkPipeline result = default(VkPipeline);
		handle.CreateGraphicsPipelines(default(VkPipelineCache), new ReadOnlySpan<VkGraphicsPipelineCreateInfo>(in reference), null, new ReadOnlySpan<VkPipeline>(&result, 1));
		return result;
	}

	private static VkBlendFactor ToVkBlendFactor(BlendFactor factor)
	{
		return factor switch
		{
			BlendFactor.Zero => VkBlendFactor.Zero, 
			BlendFactor.One => VkBlendFactor.One, 
			BlendFactor.SrcAlpha => VkBlendFactor.SrcAlpha, 
			BlendFactor.OneMinusSrcAlpha => VkBlendFactor.OneMinusSrcAlpha, 
			BlendFactor.SrcColor => VkBlendFactor.SrcColor, 
			BlendFactor.DstColor => VkBlendFactor.DstColor, 
			BlendFactor.OneMinusSrcColor => VkBlendFactor.OneMinusSrcColor, 
			BlendFactor.OneMinusDstColor => VkBlendFactor.OneMinusDstColor, 
			_ => VkBlendFactor.One, 
		};
	}

	private static VkBlendOp ToVkBlendOp(BlendOp op)
	{
		return op switch
		{
			BlendOp.Subtract => VkBlendOp.Subtract, 
			BlendOp.ReverseSubtract => VkBlendOp.ReverseSubtract, 
			BlendOp.Min => VkBlendOp.Min, 
			BlendOp.Max => VkBlendOp.Max, 
			_ => VkBlendOp.Add, 
		};
	}

	public void DestroyPipelines()
	{
		foreach (ShaderPipeline value in pipelines.Values)
		{
			if (!value.Pipeline.IsNull())
			{
				device.DestroyPipeline(value.Pipeline, null);
			}
			if (!value.Layout.IsNull())
			{
				device.DestroyPipelineLayout(value.Layout, null);
			}
		}
		pipelines.Clear();
	}

	public void Dispose()
	{
		DestroyPipelines();
		for (int i = 0; i < frames.Length; i++)
		{
			ref FrameScratch reference = ref frames[i];
			reference.DescriptorPool?.Dispose();
			ImGuiGpuMemory.Free(allocator, ref reference.UboBuffer, GpuMemoryCategory.UniformBuffer);
			if (!reference.StorageBuffer.VkBuffer.IsNull())
			{
				ImGuiGpuMemory.Unmap(allocator, in reference.StorageBuffer);
			}
			ImGuiGpuMemory.Free(allocator, ref reference.StorageBuffer, GpuMemoryCategory.DynamicDraw);
		}
		foreach (VkSampler value in wrapSamplers.Values)
		{
			device.DestroySampler(value, null);
		}
		wrapSamplers.Clear();
		ImGuiGpuMemory.Free(allocator, ref vertexDefaults, GpuMemoryCategory.DynamicDraw);
		if (!emptySetLayout.IsNull())
		{
			device.DestroyDescriptorSetLayout(emptySetLayout, null);
		}
		if (!frameSetLayout.IsNull())
		{
			device.DestroyDescriptorSetLayout(frameSetLayout, null);
		}
	}
}
