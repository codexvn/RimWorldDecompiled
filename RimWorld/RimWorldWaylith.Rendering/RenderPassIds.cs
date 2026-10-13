using Waylith.Rendering;

namespace RimWorldWaylith.Rendering;

public static class RenderPassIds
{
	public static readonly byte MapMain = RenderPassRegistry.Register("MapMain");

	public static readonly byte WorldMain = RenderPassRegistry.Register("WorldMain");

	public static readonly byte WaterDepth = RenderPassRegistry.Register("WaterDepth");

	public static readonly byte CaptureOnly = RenderPassRegistry.Register("CaptureOnly");
}
