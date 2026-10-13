namespace Waylith.Rendering;

public readonly record struct DepthState(bool Test, bool Write)
{
	public static readonly DepthState Default = new DepthState(Test: true, Write: true);
}
