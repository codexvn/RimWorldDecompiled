using System.Collections.Generic;
using UnityEngine;
using Waylith.Rendering;

namespace UnityStubs.Rendering;

public interface ICameraTextureRenderer
{
	void Render(UnityEngine.RenderTexture target, IReadOnlyList<MeshDrawCommand> commands, CameraView view, Rect viewportRect, Color? clearColor);

	bool TryReadPixels(UnityEngine.RenderTexture source, out byte[] rgbaTopDown);

	void ReleaseTarget(UnityEngine.RenderTexture target);
}
