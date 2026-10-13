using UnityEngine;

namespace UnityStubs;

public static class FrameStart
{
	public static void Run()
	{
		RenderTexture.active = null;
		RenderTexture.FreeReleasedTemporaries();
	}
}
