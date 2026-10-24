using Brutal.VulkanApi;

namespace Waylith.Rendering;

public readonly record struct RenderTextureDesc(string Name, VkFormat ColorFormat, VkFormat DepthFormat = VkFormat.Undefined, RenderTextureFilter Filter = RenderTextureFilter.Bilinear);
