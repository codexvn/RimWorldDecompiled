using System.Collections.Generic;
using Waylith.Graphics;

namespace Waylith.Rendering;

public interface IRenderFeature
{
	bool IsActive { get; }

	IReadOnlyList<IRenderPass> Passes { get; }

	object? CaptureFrame(in FrameTiming timing);

	void OnRenderTargetRebuilt(IRenderTarget renderTarget);
}
