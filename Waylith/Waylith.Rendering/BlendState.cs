namespace Waylith.Rendering;

public readonly record struct BlendState(bool Enabled, BlendFactor SrcColor, BlendFactor DstColor, BlendOp ColorOp, BlendFactor SrcAlpha, BlendFactor DstAlpha, BlendOp AlphaOp)
{
	public static readonly BlendState Opaque = new BlendState(Enabled: false, BlendFactor.One, BlendFactor.Zero, BlendOp.Add, BlendFactor.One, BlendFactor.Zero, BlendOp.Add);

	public static readonly BlendState Alpha = new BlendState(Enabled: true, BlendFactor.SrcAlpha, BlendFactor.OneMinusSrcAlpha, BlendOp.Add, BlendFactor.One, BlendFactor.OneMinusSrcAlpha, BlendOp.Add);

	public static readonly BlendState MultiplyDestination = new BlendState(Enabled: true, BlendFactor.Zero, BlendFactor.SrcColor, BlendOp.Add, BlendFactor.Zero, BlendFactor.SrcColor, BlendOp.Add);

	public static readonly BlendState Additive = new BlendState(Enabled: true, BlendFactor.SrcAlpha, BlendFactor.One, BlendOp.Add, BlendFactor.One, BlendFactor.One, BlendOp.Add);
}
