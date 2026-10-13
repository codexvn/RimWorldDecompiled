using UnityEngine;

namespace UnityStubs;

public static class EndOfFrame
{
	public static bool DrivenByHost { get; set; }

	public static bool HasWaiters => CoroutineScheduler.HasEndOfFrameWaiters();

	public static void Run()
	{
		ScreenCapture.WritePendingScreenshots();
		CoroutineScheduler.RunPendingEndOfFrame();
	}
}
