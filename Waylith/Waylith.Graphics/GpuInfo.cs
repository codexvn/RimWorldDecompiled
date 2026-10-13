using System;
using Brutal.Strings;
using Brutal.VulkanApi;

namespace Waylith.Graphics;

public static class GpuInfo
{
	public static string DeviceName { get; private set; } = "Unknown";

	public static int VendorId { get; private set; }

	public static int MaxTextureSize { get; private set; } = 16384;

	public static long DeviceLocalMemoryBytes { get; private set; }

	public static VkPhysicalDeviceType DeviceType { get; private set; }

	public static bool HostVisibleVram { get; private set; }

	public static bool StageUploadsInVram { get; set; }

	internal unsafe static void Configure(PhysicalDevice physicalDevice, long deviceLocalMemoryBytes)
	{
		VkPhysicalDeviceProperties properties = physicalDevice.GetProperties();
		DeviceName = Utf8.GetString(properties.DeviceName);
		VendorId = properties.VendorID;
		MaxTextureSize = properties.Limits.MaxImageDimension2D;
		DeviceLocalMemoryBytes = deviceLocalMemoryBytes;
		DeviceType = properties.DeviceType;
		VkPhysicalDeviceMemoryProperties memoryProperties = physicalDevice.GetMemoryProperties();
		HostVisibleVram = false;
		for (int i = 0; i < memoryProperties.MemoryTypeCount; i++)
		{
			VkMemoryType vkMemoryType = global::_003CPrivateImplementationDetails_003E.InlineArrayAsSpan<VkMemoryType_32, VkMemoryType>(ref memoryProperties.MemoryTypes, 32)[i];
			if ((vkMemoryType.PropertyFlags & (VkMemoryPropertyFlags.DeviceLocalBit | VkMemoryPropertyFlags.HostVisibleBit | VkMemoryPropertyFlags.HostCoherentBit)) == (VkMemoryPropertyFlags.DeviceLocalBit | VkMemoryPropertyFlags.HostVisibleBit | VkMemoryPropertyFlags.HostCoherentBit) && (long)global::_003CPrivateImplementationDetails_003E.InlineArrayAsSpan<VkMemoryHeap_16, VkMemoryHeap>(ref memoryProperties.MemoryHeaps, 16)[vkMemoryType.HeapIndex].Size.Value >= 1073741824L)
			{
				HostVisibleVram = true;
			}
		}
		StageUploadsInVram = HostVisibleVram && Environment.GetEnvironmentVariable("WAYLITH_STAGE_UPLOADS_IN_RAM") != "1";
	}
}
