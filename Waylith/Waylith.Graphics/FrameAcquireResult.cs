namespace Waylith.Graphics;

public struct FrameAcquireResult
{
	public FrameResult result;

	public AcquiredFrame Value;

	public void Deconstruct(out FrameResult result, out AcquiredFrame value)
	{
		result = this.result;
		value = Value;
	}
}
