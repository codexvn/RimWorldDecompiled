using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Numerics;
using Brutal;
using Brutal.VulkanApi;
using Brutal.VulkanApi.Abstractions;

namespace Waylith.Rendering;

public sealed class VulkanShaderLayoutCache : IDisposable
{
	private readonly struct PipelineLayoutKey : IEquatable<PipelineLayoutKey>
	{
		private readonly Shader _shader;

		private readonly VkDescriptorSetLayout _frameSetLayout;

		private readonly VkDescriptorSetLayout _globalTextureSetLayout;

		public PipelineLayoutKey(Shader shader, VkDescriptorSetLayout frameSetLayout, VkDescriptorSetLayout globalTextureSetLayout)
		{
			_shader = shader;
			_frameSetLayout = frameSetLayout;
			_globalTextureSetLayout = globalTextureSetLayout;
		}

		public bool IsFor(Shader shader)
		{
			return _shader == shader;
		}

		public void Deconstruct(out Shader shader, out VkDescriptorSetLayout frameSetLayout, out VkDescriptorSetLayout globalTextureSetLayout)
		{
			shader = _shader;
			frameSetLayout = _frameSetLayout;
			globalTextureSetLayout = _globalTextureSetLayout;
		}

		public bool Equals(PipelineLayoutKey other)
		{
			if (_shader == other._shader && _frameSetLayout.VkHandle == other._frameSetLayout.VkHandle && _frameSetLayout.VkObjectType == other._frameSetLayout.VkObjectType && _globalTextureSetLayout.VkHandle == other._globalTextureSetLayout.VkHandle)
			{
				return _globalTextureSetLayout.VkObjectType == other._globalTextureSetLayout.VkObjectType;
			}
			return false;
		}

		public override bool Equals(object? obj)
		{
			if (obj is PipelineLayoutKey other)
			{
				return Equals(other);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(ReferenceEqualityComparer.Instance.GetHashCode(_shader), _frameSetLayout.VkHandle, _frameSetLayout.VkObjectType, _globalTextureSetLayout.VkHandle, _globalTextureSetLayout.VkObjectType);
		}
	}

	private readonly DeviceEx _device;

	private readonly ConcurrentDictionary<Shader, Lazy<VkDescriptorSetLayout>> _materialSetLayouts = new ConcurrentDictionary<Shader, Lazy<VkDescriptorSetLayout>>(ReferenceEqualityComparer.Instance);

	private readonly ConcurrentDictionary<PipelineLayoutKey, Lazy<VkPipelineLayout>> _pipelineLayouts = new ConcurrentDictionary<PipelineLayoutKey, Lazy<VkPipelineLayout>>();

	private bool _disposed;

	public VulkanShaderLayoutCache(DeviceEx device)
	{
		_device = device;
	}

	public VkDescriptorSetLayout GetOrCreateMaterialSetLayout(Shader shader)
	{
		ObjectDisposedException.ThrowIf(_disposed, this);
		ArgumentNullException.ThrowIfNull(shader, "shader");
		return _materialSetLayouts.GetOrAdd(shader, (Shader s, VulkanShaderLayoutCache self) => new Lazy<VkDescriptorSetLayout>(() => self.CreateMaterialSetLayout(s)), this).Value;
	}

	private unsafe VkDescriptorSetLayout CreateMaterialSetLayout(Shader shader)
	{
		MaterialLayout materialLayout = shader.MaterialLayout;
		int num = 1 + materialLayout.TextureSlots.Length + materialLayout.StorageBufferSlots.Length;
		VkDescriptorSetLayoutBinding* ptr = stackalloc VkDescriptorSetLayoutBinding[num];
		*ptr = CreateBinding(0u, VkDescriptorType.UniformBuffer, VkShaderStageFlags.VertexBit | VkShaderStageFlags.FragmentBit);
		int num2 = 1;
		for (int i = 0; i < materialLayout.TextureSlots.Length; i++)
		{
			VkShaderStageFlags stageFlags = (materialLayout.TextureSlots[i].VertexVisible ? (VkShaderStageFlags.VertexBit | VkShaderStageFlags.FragmentBit) : VkShaderStageFlags.FragmentBit);
			MaterialTextureKind kind = materialLayout.TextureSlots[i].Kind;
			ptr[num2++] = CreateBinding((uint)materialLayout.TextureSlots[i].Binding, kind switch
			{
				MaterialTextureKind.SampledImage => VkDescriptorType.SampledImage, 
				MaterialTextureKind.Sampler => VkDescriptorType.Sampler, 
				_ => VkDescriptorType.CombinedImageSampler, 
			}, stageFlags);
		}
		for (int j = 0; j < materialLayout.StorageBufferSlots.Length; j++)
		{
			ptr[num2++] = CreateBinding((uint)materialLayout.StorageBufferSlots[j].Binding, VkDescriptorType.StorageBuffer, VkShaderStageFlags.VertexBit | VkShaderStageFlags.FragmentBit);
		}
		DeviceEx device = _device;
		VkDescriptorSetLayoutCreateInfo pCreateInfo = new VkDescriptorSetLayoutCreateInfo
		{
			BindingCount = num,
			Bindings = ptr
		};
		return device.CreateDescriptorSetLayout(in pCreateInfo, _device.VkAllocator);
	}

	public VkPipelineLayout GetOrCreatePipelineLayout(Shader shader, VkDescriptorSetLayout frameSetLayout, VkDescriptorSetLayout globalTextureSetLayout)
	{
		ObjectDisposedException.ThrowIf(_disposed, this);
		ArgumentNullException.ThrowIfNull(shader, "shader");
		PipelineLayoutKey key = new PipelineLayoutKey(shader, frameSetLayout, globalTextureSetLayout);
		return _pipelineLayouts.GetOrAdd(key, (PipelineLayoutKey k, VulkanShaderLayoutCache self) => new Lazy<VkPipelineLayout>(() => self.CreatePipelineLayout(k)), this).Value;
	}

	private unsafe VkPipelineLayout CreatePipelineLayout(PipelineLayoutKey key)
	{
		PipelineLayoutKey pipelineLayoutKey = key;
		pipelineLayoutKey.Deconstruct(out Shader shader, out VkDescriptorSetLayout frameSetLayout, out VkDescriptorSetLayout globalTextureSetLayout);
		Shader shader2 = shader;
		VkDescriptorSetLayout vkDescriptorSetLayout = frameSetLayout;
		VkDescriptorSetLayout vkDescriptorSetLayout2 = globalTextureSetLayout;
		VkDescriptorSetLayout orCreateMaterialSetLayout = GetOrCreateMaterialSetLayout(shader2);
		VkDescriptorSetLayout* ptr = stackalloc VkDescriptorSetLayout[3];
		*ptr = vkDescriptorSetLayout;
		ptr[1] = vkDescriptorSetLayout2;
		ptr[2] = orCreateMaterialSetLayout;
		VkPushConstantRange vkPushConstantRange = new VkPushConstantRange();
		vkPushConstantRange.StageFlags = VkShaderStageFlags.VertexBit;
		vkPushConstantRange.Offset = ByteSize.Zero;
		vkPushConstantRange.Size = ByteSize.Of<Matrix4x4>();
		VkPushConstantRange vkPushConstantRange2 = vkPushConstantRange;
		DeviceEx device = _device;
		VkPipelineLayoutCreateInfo pCreateInfo = new VkPipelineLayoutCreateInfo
		{
			SetLayoutCount = 3,
			SetLayouts = ptr,
			PushConstantRangeCount = 1,
			PushConstantRanges = &vkPushConstantRange2
		};
		return device.CreatePipelineLayout(in pCreateInfo, _device.VkAllocator);
	}

	public void Invalidate(Shader shader)
	{
		if (_disposed)
		{
			return;
		}
		if (_materialSetLayouts.TryRemove(shader, out Lazy<VkDescriptorSetLayout> value))
		{
			DestroyIfCreated(value);
		}
		foreach (KeyValuePair<PipelineLayoutKey, Lazy<VkPipelineLayout>> pipelineLayout2 in _pipelineLayouts)
		{
			pipelineLayout2.Deconstruct(out var key, out var value2);
			PipelineLayoutKey key2 = key;
			Lazy<VkPipelineLayout> pipelineLayout = value2;
			if (key2.IsFor(shader) && _pipelineLayouts.TryRemove(key2, out value2))
			{
				DestroyIfCreated(pipelineLayout);
			}
		}
	}

	public void Dispose()
	{
		if (_disposed)
		{
			return;
		}
		_disposed = true;
		foreach (Lazy<VkPipelineLayout> value in _pipelineLayouts.Values)
		{
			DestroyIfCreated(value);
		}
		foreach (Lazy<VkDescriptorSetLayout> value2 in _materialSetLayouts.Values)
		{
			DestroyIfCreated(value2);
		}
		_pipelineLayouts.Clear();
		_materialSetLayouts.Clear();
	}

	private void DestroyIfCreated(Lazy<VkPipelineLayout> pipelineLayout)
	{
		if (pipelineLayout.IsValueCreated)
		{
			_device.DestroyPipelineLayout(pipelineLayout.Value, _device.VkAllocator);
		}
	}

	private void DestroyIfCreated(Lazy<VkDescriptorSetLayout> descriptorSetLayout)
	{
		if (descriptorSetLayout.IsValueCreated)
		{
			_device.DestroyDescriptorSetLayout(descriptorSetLayout.Value, _device.VkAllocator);
		}
	}

	private static VkDescriptorSetLayoutBinding CreateBinding(uint binding, VkDescriptorType descriptorType, VkShaderStageFlags stageFlags)
	{
		VkDescriptorSetLayoutBinding result = new VkDescriptorSetLayoutBinding();
		result.Binding = (int)binding;
		result.DescriptorType = descriptorType;
		result.DescriptorCount = 1;
		result.StageFlags = stageFlags;
		return result;
	}
}
