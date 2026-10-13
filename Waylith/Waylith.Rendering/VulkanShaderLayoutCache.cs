using System;
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

	private readonly Dictionary<Shader, VkDescriptorSetLayout> _materialSetLayouts = new Dictionary<Shader, VkDescriptorSetLayout>(ReferenceEqualityComparer.Instance);

	private readonly Dictionary<PipelineLayoutKey, VkPipelineLayout> _pipelineLayouts = new Dictionary<PipelineLayoutKey, VkPipelineLayout>();

	private bool _disposed;

	public VulkanShaderLayoutCache(DeviceEx device)
	{
		_device = device;
	}

	public unsafe VkDescriptorSetLayout GetOrCreateMaterialSetLayout(Shader shader)
	{
		ObjectDisposedException.ThrowIf(_disposed, this);
		ArgumentNullException.ThrowIfNull(shader, "shader");
		if (_materialSetLayouts.TryGetValue(shader, out var value))
		{
			return value;
		}
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
		VkDescriptorSetLayout vkDescriptorSetLayout = _device.CreateDescriptorSetLayout(new VkDescriptorSetLayoutCreateInfo
		{
			BindingCount = num,
			Bindings = ptr
		}, _device.VkAllocator);
		_materialSetLayouts.Add(shader, vkDescriptorSetLayout);
		return vkDescriptorSetLayout;
	}

	public unsafe VkPipelineLayout GetOrCreatePipelineLayout(Shader shader, VkDescriptorSetLayout frameSetLayout, VkDescriptorSetLayout globalTextureSetLayout)
	{
		ObjectDisposedException.ThrowIf(_disposed, this);
		ArgumentNullException.ThrowIfNull(shader, "shader");
		PipelineLayoutKey key = new PipelineLayoutKey(shader, frameSetLayout, globalTextureSetLayout);
		if (_pipelineLayouts.TryGetValue(key, out var value))
		{
			return value;
		}
		VkDescriptorSetLayout orCreateMaterialSetLayout = GetOrCreateMaterialSetLayout(shader);
		VkDescriptorSetLayout* ptr = stackalloc VkDescriptorSetLayout[3];
		*ptr = frameSetLayout;
		ptr[1] = globalTextureSetLayout;
		ptr[2] = orCreateMaterialSetLayout;
		VkPushConstantRange vkPushConstantRange = new VkPushConstantRange();
		vkPushConstantRange.StageFlags = VkShaderStageFlags.VertexBit;
		vkPushConstantRange.Offset = ByteSize.Zero;
		vkPushConstantRange.Size = ByteSize.Of<Matrix4x4>();
		VkPushConstantRange vkPushConstantRange2 = vkPushConstantRange;
		VkPipelineLayout vkPipelineLayout = _device.CreatePipelineLayout(new VkPipelineLayoutCreateInfo
		{
			SetLayoutCount = 3,
			SetLayouts = ptr,
			PushConstantRangeCount = 1,
			PushConstantRanges = &vkPushConstantRange2
		}, _device.VkAllocator);
		_pipelineLayouts.Add(key, vkPipelineLayout);
		return vkPipelineLayout;
	}

	public void Invalidate(Shader shader)
	{
		if (_disposed)
		{
			return;
		}
		if (_materialSetLayouts.Remove(shader, out var value))
		{
			_device.DestroyDescriptorSetLayout(value, _device.VkAllocator);
		}
		List<PipelineLayoutKey> list = null;
		foreach (var (item, pipelineLayout) in _pipelineLayouts)
		{
			if (item.IsFor(shader))
			{
				_device.DestroyPipelineLayout(pipelineLayout, _device.VkAllocator);
				(list ?? (list = new List<PipelineLayoutKey>())).Add(item);
			}
		}
		if (list == null)
		{
			return;
		}
		foreach (PipelineLayoutKey item2 in list)
		{
			_pipelineLayouts.Remove(item2);
		}
	}

	public void Dispose()
	{
		if (_disposed)
		{
			return;
		}
		_disposed = true;
		foreach (VkPipelineLayout value in _pipelineLayouts.Values)
		{
			_device.DestroyPipelineLayout(value, _device.VkAllocator);
		}
		foreach (VkDescriptorSetLayout value2 in _materialSetLayouts.Values)
		{
			_device.DestroyDescriptorSetLayout(value2, _device.VkAllocator);
		}
		_pipelineLayouts.Clear();
		_materialSetLayouts.Clear();
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
