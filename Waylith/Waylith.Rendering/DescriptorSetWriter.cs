using System;
using Brutal.VulkanApi;
using Brutal.VulkanApi.Abstractions;

namespace Waylith.Rendering;

public static class DescriptorSetWriter
{
	public unsafe static VkDescriptorSet AllocateDescriptorSet(DeviceEx device, VkDescriptorPool descriptorPool, VkDescriptorSetLayout descriptorSetLayout)
	{
		VkDescriptorSetLayout vkDescriptorSetLayout = descriptorSetLayout;
		VkDescriptorSet* ptr = stackalloc VkDescriptorSet[1];
		VkDescriptorSetAllocateInfo pAllocateInfo = new VkDescriptorSetAllocateInfo
		{
			DescriptorPool = descriptorPool,
			DescriptorSetCount = 1,
			SetLayouts = &vkDescriptorSetLayout
		};
		device.AllocateDescriptorSets(in pAllocateInfo, new ReadOnlySpan<VkDescriptorSet>(ptr, 1));
		return *ptr;
	}

	public unsafe static void WriteFrameDescriptorSet(DeviceEx device, VkDescriptorSet descriptorSet, in VkDescriptorBufferInfo vertexUniforms, in VkDescriptorBufferInfo fragmentUniforms)
	{
		VkWriteDescriptorSet* ptr = stackalloc VkWriteDescriptorSet[2];
		VkDescriptorBufferInfo* ptr2 = stackalloc VkDescriptorBufferInfo[2];
		*ptr2 = vertexUniforms;
		ptr2[1] = fragmentUniforms;
		*ptr = CreateBufferWrite(descriptorSet, 0u, ptr2);
		ptr[1] = CreateBufferWrite(descriptorSet, 1u, ptr2 + 1);
		device.UpdateDescriptorSets(new ReadOnlySpan<VkWriteDescriptorSet>(ptr, 2), default(ReadOnlySpan<VkCopyDescriptorSet>));
	}

	public unsafe static void WriteMaterialDescriptorSet(DeviceEx device, VkDescriptorSet descriptorSet, ReadOnlySpan<MaterialTextureSlot> textureSlots, in VkDescriptorBufferInfo materialUniforms, ReadOnlySpan<VkDescriptorImageInfo> textures)
	{
		if (textures.Length < textureSlots.Length)
		{
			throw new ArgumentException("Texture span must cover all material texture slots.", "textures");
		}
		int num = 1 + textureSlots.Length;
		VkWriteDescriptorSet* ptr = stackalloc VkWriteDescriptorSet[num];
		VkDescriptorBufferInfo* ptr2 = stackalloc VkDescriptorBufferInfo[1];
		VkDescriptorImageInfo* ptr3 = stackalloc VkDescriptorImageInfo[textureSlots.Length];
		*ptr2 = materialUniforms;
		*ptr = CreateBufferWrite(descriptorSet, 0u, ptr2);
		for (int i = 0; i < textureSlots.Length; i++)
		{
			ptr3[i] = textures[i];
			MaterialTextureKind kind = textureSlots[i].Kind;
			ptr[i + 1] = CreateImageWrite(descriptorSet, (uint)textureSlots[i].Binding, ptr3 + i, kind switch
			{
				MaterialTextureKind.SampledImage => VkDescriptorType.SampledImage, 
				MaterialTextureKind.Sampler => VkDescriptorType.Sampler, 
				_ => VkDescriptorType.CombinedImageSampler, 
			});
		}
		device.UpdateDescriptorSets(new ReadOnlySpan<VkWriteDescriptorSet>(ptr, num), default(ReadOnlySpan<VkCopyDescriptorSet>));
	}

	public unsafe static void WriteMaterialStorageBuffers(DeviceEx device, VkDescriptorSet descriptorSet, ReadOnlySpan<int> bindings, ReadOnlySpan<VkDescriptorBufferInfo> buffers)
	{
		int length = bindings.Length;
		if (length != 0)
		{
			if (buffers.Length < length)
			{
				throw new ArgumentException("Buffer span must cover all storage bindings.", "buffers");
			}
			VkWriteDescriptorSet* ptr = stackalloc VkWriteDescriptorSet[length];
			VkDescriptorBufferInfo* ptr2 = stackalloc VkDescriptorBufferInfo[length];
			for (int i = 0; i < length; i++)
			{
				ptr2[i] = buffers[i];
				ptr[i] = CreateStorageBufferWrite(descriptorSet, (uint)bindings[i], ptr2 + i);
			}
			device.UpdateDescriptorSets(new ReadOnlySpan<VkWriteDescriptorSet>(ptr, length), default(ReadOnlySpan<VkCopyDescriptorSet>));
		}
	}

	private unsafe static VkWriteDescriptorSet CreateStorageBufferWrite(VkDescriptorSet descriptorSet, uint binding, VkDescriptorBufferInfo* bufferInfo)
	{
		VkWriteDescriptorSet result = new VkWriteDescriptorSet();
		result.DstSet = descriptorSet;
		result.DstBinding = (int)binding;
		result.DescriptorCount = 1;
		result.DescriptorType = VkDescriptorType.StorageBuffer;
		result.BufferInfo = bufferInfo;
		return result;
	}

	private unsafe static VkWriteDescriptorSet CreateBufferWrite(VkDescriptorSet descriptorSet, uint binding, VkDescriptorBufferInfo* bufferInfo)
	{
		VkWriteDescriptorSet result = new VkWriteDescriptorSet();
		result.DstSet = descriptorSet;
		result.DstBinding = (int)binding;
		result.DescriptorCount = 1;
		result.DescriptorType = VkDescriptorType.UniformBuffer;
		result.BufferInfo = bufferInfo;
		return result;
	}

	private unsafe static VkWriteDescriptorSet CreateImageWrite(VkDescriptorSet descriptorSet, uint binding, VkDescriptorImageInfo* imageInfo, VkDescriptorType descriptorType = VkDescriptorType.CombinedImageSampler)
	{
		VkWriteDescriptorSet result = new VkWriteDescriptorSet();
		result.DstSet = descriptorSet;
		result.DstBinding = (int)binding;
		result.DescriptorCount = 1;
		result.DescriptorType = descriptorType;
		result.ImageInfo = imageInfo;
		return result;
	}
}
