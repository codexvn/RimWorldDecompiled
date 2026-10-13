using System;
using System.Numerics;
using Brutal.VulkanApi;
using Waylith.Graphics.Meshes;

namespace Waylith.Rendering;

internal static class OptionalVertexInputDefaults
{
	public const int MinimumStrideBytes = 16;

	public static int GetStrideBytes(ShaderVertexInputLayout layout)
	{
		return System.Math.Max(16, layout.FallbackStrideBytes);
	}

	public static bool TryGetFormat(VertexAttributeSemantic semantic, out VkFormat format)
	{
		VkFormat vkFormat = (((uint)(semantic - 1) <= 11u) ? VkFormat.R32G32B32A32SFloat : VkFormat.Undefined);
		format = vkFormat;
		return format != VkFormat.Undefined;
	}

	public unsafe static void Fill(void* destination, int count, ShaderVertexInputLayout layout)
	{
		int strideBytes = GetStrideBytes(layout);
		for (int i = 0; i < count; i++)
		{
			byte* ptr = (byte*)destination + i * strideBytes;
			foreach (ShaderVertexInputSlot slot in layout.Slots)
			{
				if (!slot.required)
				{
					*(Vector4*)(ptr + slot.defaultOffsetBytes) = slot.defaultValue;
				}
			}
		}
	}
}
