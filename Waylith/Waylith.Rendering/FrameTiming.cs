namespace Waylith.Rendering;

public readonly struct FrameTiming
{
	public int FrameNumber { get; }

	public float DeltaTime { get; }

	public double ElapsedTime { get; }

	public FrameTiming(int frameNumber, float deltaTime, double elapsedTime)
	{
		FrameNumber = frameNumber;
		DeltaTime = deltaTime;
		ElapsedTime = elapsedTime;
	}
}
