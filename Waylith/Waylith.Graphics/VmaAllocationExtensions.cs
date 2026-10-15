using System;
using System.Runtime.CompilerServices;
using System.Threading;
using Brutal;
using Brutal.Logging;
using Brutal.Logging.Internal;
using Brutal.VulkanApi;
using Brutal.VulkanApi.Abstractions;
using Brutal.VulkanApi.Vma;
using Waylith.Rendering;

namespace Waylith.Graphics;

public static class VmaAllocationExtensions
{
	private enum Relax
	{
		None,
		NotDeviceLocal,
		AnyType,
		Dedicated
	}

	[SpecialName]
	public sealed class _003CG_003E_00246AF3411E052BE00719BCFA988CB70666
	{
		[SpecialName]
		public static class _003CM_003E_0024162DA75BC77B57F902F547955491F480
		{
		}

		[ExtensionMarker("<M>$162DA75BC77B57F902F547955491F480")]
		public BufferAllocationInfo AllocateBufferChecked(string name, in VkBufferCreateInfo bufferCreateInfo, in AllocationInfo.CreateInfo allocateCreateInfo, bool anyMemoryOnRefusal = true)
		{
			throw new NotSupportedException();
		}

		[ExtensionMarker("<M>$162DA75BC77B57F902F547955491F480")]
		public ImageAllocationInfo AllocateImageChecked(string name, in VkImageCreateInfo imageCreateInfo, in AllocationInfo.CreateInfo allocateCreateInfo, uint memoryTypeBits = 0u, bool anyMemoryOnRefusal = true)
		{
			throw new NotSupportedException();
		}
	}

	private const int MaxReclaimAttempts = 2;

	private static int _anyMemoryLogged;

	private const VmaAllocationCreateFlags HostAccessFlags = VmaAllocationCreateFlags.HostAccessSequentialWriteBit | VmaAllocationCreateFlags.HostAccessRandomBit;

	public static Func<long, int, bool, bool>? OutOfDeviceMemoryHandler { get; set; }

	private static bool WantsDeviceLocal(in AllocationInfo.CreateInfo info)
	{
		if (info.Preference != MemoryPreference.PreferGpu)
		{
			return (info.RequiredProperties & VkMemoryPropertyFlags.DeviceLocalBit) != 0;
		}
		return true;
	}

	private static bool IsOutOfMemory(VkResult result)
	{
		if ((uint)(result - -2) <= 1u)
		{
			return true;
		}
		return false;
	}

	private static bool TryReclaim(VkResult result, in AllocationInfo.CreateInfo info, long bytes, ref int attempt)
	{
		if (IsOutOfMemory(result) && attempt < 2)
		{
			return OutOfDeviceMemoryHandler?.Invoke(bytes, attempt++, WantsDeviceLocal(in info)) ?? false;
		}
		return false;
	}

	private static bool TryRelax(VkResult result, bool anyMemoryOnRefusal, ref VmaAllocationCreateInfo vmaInfo, ref Relax relax)
	{
		if (!anyMemoryOnRefusal || !IsOutOfMemory(result) || relax == Relax.Dedicated)
		{
			return false;
		}
		relax++;
		switch (relax)
		{
		case Relax.NotDeviceLocal:
			vmaInfo.RequiredFlags &= ~VkMemoryPropertyFlags.DeviceLocalBit;
			vmaInfo.Usage = VmaMemoryUsage.Auto;
			break;
		case Relax.AnyType:
			if ((vmaInfo.Flags & (VmaAllocationCreateFlags.HostAccessSequentialWriteBit | VmaAllocationCreateFlags.HostAccessRandomBit)) != 0)
			{
				vmaInfo.RequiredFlags |= VkMemoryPropertyFlags.HostVisibleBit;
			}
			vmaInfo.Flags &= ~(VmaAllocationCreateFlags.WithinBudgetBit | VmaAllocationCreateFlags.HostAccessSequentialWriteBit | VmaAllocationCreateFlags.HostAccessRandomBit);
			vmaInfo.PreferredFlags = VkMemoryPropertyFlags.None;
			vmaInfo.Usage = VmaMemoryUsage.Unknown;
			vmaInfo.MemoryTypeBits = 0u;
			break;
		case Relax.Dedicated:
			vmaInfo.Flags |= VmaAllocationCreateFlags.DedicatedMemoryBit;
			break;
		}
		return true;
	}

	private static void LogRelaxed(string name, long bytes, Relax relax)
	{
		if (Interlocked.Exchange(ref _anyMemoryLogged, 1) == 0)
		{
			LogCategory log = RenderLog.Log;
			bool shouldAppend;
			LogString<Warning> message = new LogString<Warning>(179, 5, log, out shouldAppend);
			if (shouldAppend)
			{
				message.AppendLiteral("Memory refused '");
				message.AppendFormatted(name);
				message.AppendLiteral("' (");
				message.AppendFormatted(GpuMemoryReport.FormatBytes(bytes));
				message.AppendLiteral(") even after trying to free up memory. The allocation has been placed in suboptimal storage (");
				message.AppendFormatted(relax);
				message.AppendLiteral("). Expect lower performance. This message will only be logged once.");
				message.AppendFormatted(Environment.NewLine);
				message.AppendFormatted(GpuMemoryReport.BuildCompactSummary().TrimEnd());
			}
			log.Warning(message, "LogRelaxed", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Graphics\\VmaAllocationExtensions.cs", 65);
		}
	}

	public static BufferAllocationInfo AllocateBufferChecked(this VmaAllocator allocator, string name, in VkBufferCreateInfo bufferCreateInfo, in AllocationInfo.CreateInfo allocateCreateInfo, bool anyMemoryOnRefusal = true)
	{
		VmaAllocationCreateInfo vmaInfo = ConvertInfo(in allocateCreateInfo);
		long value = (long)bufferCreateInfo.Size.Value;
		int attempt = 0;
		Relax relax = Relax.None;
		VkResult vkResult;
		VkBuffer pBuffer;
		VmaAllocation pAllocation;
		VmaAllocationInfo pAllocationInfo;
		do
		{
			vkResult = allocator.CreateBuffer(bufferCreateInfo, vmaInfo, out pBuffer, out pAllocation, out pAllocationInfo);
		}
		while (TryReclaim(vkResult, in allocateCreateInfo, value, ref attempt) || TryRelax(vkResult, anyMemoryOnRefusal, ref vmaInfo, ref relax));
		if (vkResult != 0)
		{
			throw VmaAllocationException.Buffer(name, in bufferCreateInfo, vkResult);
		}
		if (relax != 0)
		{
			LogRelaxed(name, value, relax);
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

	public static ImageAllocationInfo AllocateImageChecked(this VmaAllocator allocator, string name, in VkImageCreateInfo imageCreateInfo, in AllocationInfo.CreateInfo allocateCreateInfo, uint memoryTypeBits = 0u, bool anyMemoryOnRefusal = true)
	{
		VmaAllocationCreateInfo vmaInfo = ConvertInfo(in allocateCreateInfo);
		vmaInfo.MemoryTypeBits = memoryTypeBits;
		long bytes = GpuTextureService.EstimateImageBytes(in imageCreateInfo);
		int attempt = 0;
		Relax relax = Relax.None;
		VkResult vkResult;
		VkImage pImage;
		VmaAllocation pAllocation;
		VmaAllocationInfo pAllocationInfo;
		do
		{
			vkResult = allocator.CreateImage(imageCreateInfo, vmaInfo, out pImage, out pAllocation, out pAllocationInfo);
		}
		while (TryReclaim(vkResult, in allocateCreateInfo, bytes, ref attempt) || TryRelax(vkResult, anyMemoryOnRefusal, ref vmaInfo, ref relax));
		if (vkResult != 0)
		{
			throw VmaAllocationException.Image(name, in imageCreateInfo, vkResult);
		}
		if (relax != 0)
		{
			LogRelaxed(name, bytes, relax);
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
			usage = VmaMemoryUsage.AutoPreferHost;
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
