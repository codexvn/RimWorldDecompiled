using System.Diagnostics;

namespace Waylith;

public static class Time
{
	private static readonly long StartTimestamp = Stopwatch.GetTimestamp();

	public static float DeltaTime { get; private set; }

	public static int FrameCount { get; private set; }

	public static float Elapsed { get; private set; }

	public static float RealtimeSinceStartup => (float)Stopwatch.GetElapsedTime(StartTimestamp).TotalSeconds;

	public static double RealtimeSinceStartupAsDouble => Stopwatch.GetElapsedTime(StartTimestamp).TotalSeconds;

	internal static void BeginFrame(float deltaTime, float elapsedTime)
	{
		DeltaTime = deltaTime;
		Elapsed = elapsedTime;
	}

	internal static void EndFrame()
	{
		FrameCount++;
	}
}
