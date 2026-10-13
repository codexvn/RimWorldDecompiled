using System.Collections.Generic;

namespace Waylith.Rendering;

public sealed class ShaderWarmupPlan
{
	public List<GraphicsShaderJob> Graphics { get; } = new List<GraphicsShaderJob>();

	public List<ComputeShaderJob> Compute { get; } = new List<ComputeShaderJob>();

	public List<FixedGraphicsShaderJob> FixedGraphics { get; } = new List<FixedGraphicsShaderJob>();

	public List<InlineShaderJob> Inline { get; } = new List<InlineShaderJob>();

	public int Count => Graphics.Count + Compute.Count + FixedGraphics.Count + Inline.Count;
}
