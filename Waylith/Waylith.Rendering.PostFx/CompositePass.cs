using System;
using System.Runtime.CompilerServices;
using Brutal.VulkanApi;
using Waylith.Graphics;

namespace Waylith.Rendering.PostFx;

public sealed class CompositePass : PostEffectPass
{
	public VkSampler Sampler { get; set; }

	public Func<int, RenderTexture?>? Source { get; set; }

	public float Saturation { get; set; } = 1f;

	public override bool PreBackbuffer => false;

	public CompositePass(VkSampler sampler)
		: base("PostFxComposite", "PostFxComposite.frag.glsl", RuntimeHelpers.CreateSpan<int>((RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/), default(BlendState), 0, 1, 1)
	{
		Sampler = sampler;
	}

	public override void Record(in FrameContext ctx, object? featurePayload)
	{
		if (featurePayload != null)
		{
			RenderTexture renderTexture = Source?.Invoke(ctx.SubmissionFrameIndex);
			if (renderTexture != null)
			{
				SetInput(0, renderTexture.Color, Sampler);
				SetUniforms<float>(Saturation);
				RecordInline(in ctx, ctx.RenderPass, ctx.SampleCount);
			}
		}
	}

	public override void OnRenderTargetRebuilt(IRenderTarget target)
	{
		Invalidate();
	}
}
