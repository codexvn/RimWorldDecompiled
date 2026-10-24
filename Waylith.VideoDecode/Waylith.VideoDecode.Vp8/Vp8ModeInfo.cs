namespace Waylith.VideoDecode.Vp8;

internal struct Vp8ModeInfo
{
	public byte Mode;

	public byte UvMode;

	public byte RefFrame;

	public byte Is4x4;

	public byte Partitioning;

	public byte Skip;

	public byte NeedToClampMvs;

	public byte SegmentId;

	public int Mv;

	public unsafe fixed int Blocks[16];
}
