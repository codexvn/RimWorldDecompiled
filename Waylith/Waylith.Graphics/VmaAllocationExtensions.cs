using System;
using System.Runtime.CompilerServices;
using Brutal;
using Brutal.VulkanApi;
using Brutal.VulkanApi.Abstractions;
using Brutal.VulkanApi.Vma;
using Waylith.Rendering;

namespace Waylith.Graphics;

public static class VmaAllocationExtensions
{
	[SpecialName]
	public sealed class _003CG_003E_00246AF3411E052BE00719BCFA988CB70666
	{
		[SpecialName]
		public static class _003CM_003E_0024162DA75BC77B57F902F547955491F480
		{
		}

		[ExtensionMarker("<M>$162DA75BC77B57F902F547955491F480")]
		public BufferAllocationInfo AllocateBufferChecked(string name, in VkBufferCreateInfo bufferCreateInfo, in AllocationInfo.CreateInfo allocateCreateInfo)
		{
			throw new NotSupportedException();
		}

		[ExtensionMarker("<M>$162DA75BC77B57F902F547955491F480")]
		public ImageAllocationInfo AllocateImageChecked(string name, in VkImageCreateInfo imageCreateInfo, in AllocationInfo.CreateInfo allocateCreateInfo, uint memoryTypeBits = 0u)
		{
			throw new NotSupportedException();
		}
	}

	private const int MaxReclaimAttempts = 2;

	public static Func<long, int, bool>? OutOfDeviceMemoryHandler { get; set; }

	private static bool WantsDeviceLocal(in AllocationInfo.CreateInfo info)
	{
		if (info.Preference != MemoryPreference.PreferGpu)
		{
			return (info.RequiredProperties & VkMemoryPropertyFlags.DeviceLocalBit) != 0;
		}
		return true;
	}

	private static bool TryReclaim(VkResult result, in AllocationInfo.CreateInfo info, long bytes, ref int attempt)
	{
		if (result == VkResult.ErrorOutOfDeviceMemory && WantsDeviceLocal(in info) && attempt < 2)
		{
			return OutOfDeviceMemoryHandler?.Invoke(bytes, attempt++) ?? false;
		}
		return false;
	}

	public static BufferAllocationInfo AllocateBufferChecked(this VmaAllocator allocator, string name, in VkBufferCreateInfo bufferCreateInfo, in AllocationInfo.CreateInfo allocateCreateInfo)
	{
		VmaAllocationCreateInfo pAllocationCreateInfo = ConvertInfo(in allocateCreateInfo);
		int attempt = 0;
		VkResult vkResult;
		VkBuffer pBuffer;
		VmaAllocation pAllocation;
		VmaAllocationInfo pAllocationInfo;
		do
		{
			vkResult = allocator.CreateBuffer(bufferCreateInfo, pAllocationCreateInfo, out pBuffer, out pAllocation, out pAllocationInfo);
		}
		while (TryReclaim(vkResult, in allocateCreateInfo, (long)bufferCreateInfo.Size.Value, ref attempt));
		if (vkResult != 0)
		{
			throw VmaAllocationException.Buffer(name, in bufferCreateInfo, vkResult);
		}
		BufferAllocationInfo result = new BufferAllocationInfo(pBuffer, default(ByteSize), bufferCreateInfo.Size, bufferCreateInfo.Usage, new AllocationInfo
		{
			Allocator = allocator,
			Memory = pAllocationInfo.DeviceMemory,
			MemoryTypeIndex = (int)pAllocationInfo.MemoryType,
			AllocOffset = pAllocationInfo.Offset,
			AllocSize = pAllocationInfo.Size,
			AllocHandle = pAllocation.VkHandle
		});
		_ = name.Length;
		_ = 0;
		return result;
	}

	public static ImageAllocationInfo AllocateImageChecked(this VmaAllocator allocator, string name, in VkImageCreateInfo imageCreateInfo, in AllocationInfo.CreateInfo allocateCreateInfo, uint memoryTypeBits = 0u)
	{
		VmaAllocationCreateInfo pAllocationCreateInfo = ConvertInfo(in allocateCreateInfo);
		pAllocationCreateInfo.MemoryTypeBits = memoryTypeBits;
		int attempt = 0;
		VkResult vkResult;
		VkImage pImage;
		VmaAllocation pAllocation;
		VmaAllocationInfo pAllocationInfo;
		do
		{
			vkResult = allocator.CreateImage(imageCreateInfo, pAllocationCreateInfo, out pImage, out pAllocation, out pAllocationInfo);
		}
		while (TryReclaim(vkResult, in allocateCreateInfo, GpuTextureService.EstimateImageBytes(in imageCreateInfo), ref attempt));
		if (vkResult != 0)
		{
			throw VmaAllocationException.Image(name, in imageCreateInfo, vkResult);
		}
		ImageAllocationInfo imageAllocationInfo = default(ImageAllocationInfo);
		imageAllocationInfo.Image = pImage;
		imageAllocationInfo.Extent = imageCreateInfo.Extent;
		imageAllocationInfo.Format = imageCreateInfo.Format;
		imageAllocationInfo.MipLevels = imageCreateInfo.MipLevels;
		imageAllocationInfo.ArrayLayers = imageCreateInfo.ArrayLayers;
		imageAllocationInfo.MemAllocationInfo = new AllocationInfo
		{
			Allocator = allocator,
			Memory = pAllocationInfo.DeviceMemory,
			MemoryTypeIndex = (int)pAllocationInfo.MemoryType,
			AllocOffset = pAllocationInfo.Offset,
			AllocSize = pAllocationInfo.Size,
			AllocHandle = pAllocation.VkHandle
		};
		ImageAllocationInfo result = imageAllocationInfo;
		_ = name.Length;
		_ = 0;
		return result;
	}

	private static VmaAllocationCreateInfo ConvertInfo(in AllocationInfo.CreateInfo allocationCreateInfo)
	{
		VmaAllocationCreateFlags vmaAllocationCreateFlags = VmaAllocationCreateFlags.None;
		VmaMemoryUsage usage = VmaMemoryUsage.Auto;
		switch (allocationCreateInfo.Preference)
		{
		case MemoryPreference.PreferGpu:
			usage = VmaMemoryUsage.AutoPreferDevice;
			break;
		case MemoryPreference.PreferCpu:
			usage = VmaMemoryUsage.Auto;
			vmaAllocationCreateFlags |= VmaAllocationCreateFlags.HostAccessRandomBit;
			break;
		}
		if ((allocationCreateInfo.RequiredProperties & (VkMemoryPropertyFlags.HostVisibleBit | VkMemoryPropertyFlags.HostCoherentBit)) != 0)
		{
			vmaAllocationCreateFlags |= VmaAllocationCreateFlags.HostAccessRandomBit;
		}
		VmaAllocationCreateInfo result = default(VmaAllocationCreateInfo);
		result.Usage = usage;
		result.RequiredFlags = allocationCreateInfo.RequiredProperties;
		result.Flags = vmaAllocationCreateFlags;
		result.PreferredFlags = allocationCreateInfo.PreferredProperties;
		result.Priority = allocationCreateInfo.Priority;
		result.MemoryTypeBits = 0u;
		return result;
	}
}
