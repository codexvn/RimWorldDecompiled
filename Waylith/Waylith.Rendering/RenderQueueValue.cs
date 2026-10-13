using System.Globalization;

namespace Waylith.Rendering;

public readonly struct RenderQueueValue
{
	public readonly int Value;

	public RenderQueueValue(int value)
	{
		Value = value;
	}

	public static implicit operator int(RenderQueueValue queue)
	{
		return queue.Value;
	}

	public override string ToString()
	{
		return Value.ToString(CultureInfo.InvariantCulture);
	}
}
