using System;
using System.Collections.Generic;
using Brutal.VulkanApi.Abstractions;
using Waylith.Graphics;

namespace Waylith.Rendering.Compute;

public sealed class ComputeRenderFeature : IRenderFeature, IDisposable
{
	private readonly ComputeRenderPass pass;

	private readonly IRenderPass[] passes;

	private bool isActive;

	public bool IsActive => isActive;

	public IReadOnlyList<IRenderPass> Passes => passes;

	public ComputeRenderFeature(DeviceEx device, int framesInFlight)
	{
		pass = new ComputeRenderPass(device, framesInFlight);
		passes = new IRenderPass[1] { pass };
	}

	public object? CaptureFrame(in FrameTiming timing)
	{
		List<ComputeDispatch> list = ComputeDispatchQueue.DrainPending();
		isActive = list != null;
		return list;
	}

	public void OnRenderTargetRebuilt(IRenderTarget renderTarget)
	{
	}

	public void Dispose()
	{
		pass.Dispose();
	}

	object? IRenderFeature.CaptureFrame(in FrameTiming timing)
	{
		return CaptureFrame(in timing);
	}
}
