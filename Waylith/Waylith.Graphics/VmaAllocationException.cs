using System;
using Brutal.VulkanApi;

namespace Waylith.Graphics;

public sealed class VmaAllocationException : Exception
{
	public VkResult Result { get; }

	public string ResourceKind { get; }

	public string ResourceLabel { get; }

	private VmaAllocationException(string message, VkResult result, string resourceKind, string resourceLabel, Exception? inner)
		: base(message, inner)
	{
		Result = result;
		ResourceKind = resourceKind;
		ResourceLabel = resourceLabel;
	}

	public static VmaAllocationException Buffer(string label, in VkBufferCreateInfo createInfo, VkResult result)
	{
		return new VmaAllocationException($"VMA failed to allocate buffer '{Describe(label)}' ({GpuMemoryReport.FormatBytes((long)createInfo.Size.Value)}, usage {createInfo.Usage}): {result}.{VramSuffix()}", result, "buffer", label, null);
	}

	public static VmaAllocationException Image(string label, in VkImageCreateInfo createInfo, VkResult result)
	{
		VkExtent3D extent = createInfo.Extent;
		return new VmaAllocationException($"VMA failed to allocate image '{Describe(label)}' ({extent.Width}x{extent.Height}x{extent.Depth}, {createInfo.Format}, {createInfo.MipLevels} mips, {createInfo.ArrayLayers} layers, ~{GpuMemoryReport.FormatBytes(EstimateImageBytes(in createInfo))}): {result}.{VramSuffix()}", result, "image", label, null);
	}

	public static VmaAllocationException WithContext(string context, VmaAllocationException inner)
	{
		return new VmaAllocationException(context + ": " + inner.Message, inner.Result, inner.ResourceKind, inner.ResourceLabel, inner);
	}

	private static string Describe(string label)
	{
		if (!string.IsNullOrEmpty(label))
		{
			return label;
		}
		return "<unnamed>";
	}

	private static string VramSuffix()
	{
		if (!GpuMemoryReport.IsConfigured)
		{
			return string.Empty;
		}
		return Environment.NewLine + GpuMemoryReport.BuildCompactSummary().TrimEnd();
	}

	private static long EstimateImageBytes(in VkImageCreateInfo createInfo)
	{
		VkExtent3D extent = createInfo.Extent;
		long num = System.Math.Max(1L, extent.Width) * System.Math.Max(1L, extent.Height) * System.Math.Max(1L, extent.Depth) * System.Math.Max(1, createInfo.ArrayLayers) * BytesPerPixelGuess(createInfo.Format);
		if (createInfo.MipLevels <= 1)
		{
			return num;
		}
		return num * 4 / 3;
	}

	private static long BytesPerPixelGuess(VkFormat format)
	{
		string text = format.ToString();
		if (text.Contains("R32G32B32A32"))
		{
			return 16L;
		}
		if (text.Contains("R16G16B16A16"))
		{
			return 8L;
		}
		if (text.Contains("R32G32"))
		{
			return 8L;
		}
		if (text.Contains("R8G8B8A8") || text.Contains("B8G8R8A8") || text.Contains("R32_") || text.Contains("D32") || text.Contains("D24"))
		{
			return 4L;
		}
		if (text.Contains("R16") && !text.Contains("R16G16"))
		{
			return 2L;
		}
		if (text.Contains("R8") && !text.Contains("R8G8"))
		{
			return 1L;
		}
		return 4L;
	}
}
