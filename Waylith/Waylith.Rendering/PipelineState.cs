namespace Waylith.Rendering;

public readonly record struct PipelineState(BlendState Blend, DepthState Depth, FaceCull Cull, StencilState Stencil = default(StencilState), bool ColorWrite = true, DepthBiasState DepthBias = default(DepthBiasState))
{
	public static readonly PipelineState Opaque = new PipelineState(BlendState.Opaque, DepthState.Default, FaceCull.None);
}
