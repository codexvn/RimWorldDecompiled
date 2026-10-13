using System;
using System.Collections.Generic;
using Waylith.Graphics;
using Waylith.Rendering;

namespace RimWorldWaylith.Imgui;

internal sealed class ImGuiRenderFeature : IRenderFeature, IRenderPass, IDisposable
{
	private readonly ImGuiHost host;

	private readonly Action<ImGuiHost> buildUi;

	private readonly IRenderPass[] passes;

	public bool IsActive => true;

	public IReadOnlyList<IRenderPass> Passes => passes;

	public byte PassId => 0;

	public PassAttachment? Attachment => null;

	public bool PreBackbuffer => false;

	public ImGuiRenderFeature(ImGuiHost host, Action<ImGuiHost> buildUi)
	{
		this.host = host;
		this.buildUi = buildUi;
		passes = new IRenderPass[1] { this };
	}

	public object? CaptureFrame(in FrameTiming timing)
	{
		ImGuiDrawSnapshot imGuiDrawSnapshot = host.BeginFrame(timing.FrameNumber, timing.DeltaTime);
		buildUi(host);
		host.EndFrame(imGuiDrawSnapshot);
		return imGuiDrawSnapshot;
	}

	public void OnRenderTargetRebuilt(IRenderTarget renderTarget)
	{
	}

	public void Record(in FrameContext ctx, object? featurePayload)
	{
		if (featurePayload is ImGuiDrawSnapshot snapshot)
		{
			host.Render(snapshot, in ctx);
		}
	}

	public void Dispose()
	{
		host.Dispose();
	}

	object? IRenderFeature.CaptureFrame(in FrameTiming timing)
	{
		return CaptureFrame(in timing);
	}

	void IRenderPass.Record(in FrameContext ctx, object? featurePayload)
	{
		Record(in ctx, featurePayload);
	}
}
