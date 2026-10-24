namespace Waylith.VideoDecode.Vp8;

internal struct Vp8FrameContext
{
	public unsafe fixed byte Coef[1056];

	public unsafe fixed byte YMode[4];

	public unsafe fixed byte UvMode[3];

	public unsafe fixed byte Mv[38];
}
