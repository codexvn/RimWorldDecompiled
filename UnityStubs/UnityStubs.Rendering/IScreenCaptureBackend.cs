using UnityEngine;

namespace UnityStubs.Rendering;

public interface IScreenCaptureBackend
{
	bool RequestCapture();

	bool TryReadScreen(out byte[] rgbaTopDown, out int width, out int height);

	bool TryCopyScreenInto(RenderTexture target);
}
