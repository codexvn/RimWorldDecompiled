using System;

namespace Waylith.Rendering;

public static class RenderTextureDraws
{
	public static RenderTextureDrawFeature? Feature { get; set; }

	public static RenderTextureBatch CreateBatch(IRenderTextureTarget target)
	{
		return Require().CreateBatch(target);
	}

	private static RenderTextureDrawFeature Require()
	{
		return Feature ?? throw new InvalidOperationException("RenderTextureDraws.Feature is not configured; the render-texture draw feature was not registered.");
	}
}
