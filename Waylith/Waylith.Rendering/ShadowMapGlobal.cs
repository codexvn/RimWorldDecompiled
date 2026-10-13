using System;
using Brutal.VulkanApi;

namespace Waylith.Rendering;

public static class ShadowMapGlobal
{
	public static ShadowQuality Quality { get; set; }

	public static Func<int, VkImageView>? GetDepthView { get; set; }

	public static int Size { get; set; }

	public static Func<int>? GetDepthSize { get; set; }

	public static CameraView View { get; set; }
}
