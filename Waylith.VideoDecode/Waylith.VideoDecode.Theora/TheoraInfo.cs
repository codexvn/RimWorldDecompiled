namespace Waylith.VideoDecode.Theora;

internal struct TheoraInfo
{
	public int VersionMajor;

	public int VersionMinor;

	public int VersionSubminor;

	public int FrameWidth;

	public int FrameHeight;

	public int PictureWidth;

	public int PictureHeight;

	public int PictureX;

	public int PictureY;

	public uint FpsNumerator;

	public uint FpsDenominator;

	public int ColorSpace;

	public int PixelFormat;

	public int KeyframeGranuleShift;
}
