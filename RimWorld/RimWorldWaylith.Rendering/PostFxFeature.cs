using System;
using System.Collections.Generic;
using Waylith.Graphics;
using Waylith.Rendering;
using Waylith.Rendering.PostFx;

namespace RimWorldWaylith.Rendering;

public sealed class PostFxFeature : IRenderFeature, IDisposable
{
	private readonly SceneColorTargets targets;

	private readonly CompositePass composite;

	private readonly Func<bool> sceneActive;

	private readonly IRenderPass[] passes;

	private static readonly object SceneRenderedPayload = new object();

	public bool IsActive => sceneActive();

	public IReadOnlyList<IRenderPass> Passes => passes;

	public PostFxFeature(SceneColorTargets targets, CompositePass composite, Func<bool> sceneActive)
	{
		this.targets = targets;
		this.composite = composite;
		this.sceneActive = sceneActive;
		passes = new IRenderPass[1] { composite };
	}

	public object? CaptureFrame(in FrameTiming timing)
	{
		if (!sceneActive())
		{
			return null;
		}
		return SceneRenderedPayload;
	}

	public void OnRenderTargetRebuilt(IRenderTarget renderTarget)
	{
		if (renderTarget is IRenderContext renderContext)
		{
			targets.EnsureSized(renderContext.FramesInFlight, renderContext.GetExtent().Width, renderContext.GetExtent().Height);
		}
		composite.OnRenderTargetRebuilt(renderTarget);
	}

	public void Dispose()
	{
		composite.Dispose();
		targets.Dispose();
	}

	object? IRenderFeature.CaptureFrame(in FrameTiming timing)
	{
		return CaptureFrame(in timing);
	}
}
