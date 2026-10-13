namespace Waylith.Rendering;

public readonly record struct DrawPropertyRange(int Start, int Count)
{
	public bool IsEmpty => Count <= 0;
}
