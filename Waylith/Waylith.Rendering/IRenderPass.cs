using System;
using Waylith.Graphics;

namespace Waylith.Rendering;

public interface IRenderPass : IDisposable
{
	byte PassId { get; }

	PassAttachment? Attachment { get; }

	bool PreBackbuffer { get; }

	void Record(in FrameContext ctx, object? featurePayload);

	void OnRenderTargetRebuilt(IRenderTarget target);
}
