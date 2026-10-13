using System.Diagnostics;

namespace Waylith.UnityCompat;

public sealed class UnityTimeState
{
	private readonly long _startTimestamp = Stopwatch.GetTimestamp();

	public float DeltaTime { get; private set; }

	public int FrameCount { get; private set; }

	public float Time { get; private set; }

	public float UnscaledDeltaTime { get; private set; }

	public float UnscaledTime { get; private set; }

	public float RealtimeSinceStartup => (float)Stopwatch.GetElapsedTime(_startTimestamp).TotalSeconds;

	public double RealtimeSinceStartupAsDouble => Stopwatch.GetElapsedTime(_startTimestamp).TotalSeconds;

	internal void BeginFrame(float deltaTime, float elapsedTime)
	{
		FrameCount++;
		DeltaTime = deltaTime;
		UnscaledDeltaTime = deltaTime;
		Time = elapsedTime;
		UnscaledTime = elapsedTime;
	}
}
