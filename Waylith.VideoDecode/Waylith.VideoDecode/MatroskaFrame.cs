namespace Waylith.VideoDecode;

internal struct MatroskaFrame
{
	public long Offset;

	public int Size;

	public double Time;

	public bool Key;
}
