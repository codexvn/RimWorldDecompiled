using Waylith.Rendering;

namespace Waylith.VideoDecode;

public readonly record struct VideoTarget(GpuTextureAllocation Texture, int Width, int Height, VideoAspect Aspect);
