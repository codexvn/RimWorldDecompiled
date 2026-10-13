namespace Waylith.Rendering;

public readonly record struct StencilState(bool Enabled, StencilCompare Compare, StencilOp PassOp, StencilOp FailOp, StencilOp DepthFailOp, byte Reference, byte CompareMask, byte WriteMask)
{
	public static readonly StencilState Disabled;

	public static StencilState WriteOnce(byte reference)
	{
		return new StencilState(Enabled: true, StencilCompare.NotEqual, StencilOp.Replace, StencilOp.Keep, StencilOp.Keep, reference, byte.MaxValue, byte.MaxValue);
	}
}
