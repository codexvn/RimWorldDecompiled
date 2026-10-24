namespace Waylith.VideoDecode;

internal struct Mp4Sample
{
	public long Offset;

	public int Size;

	public long Dts;

	public long Pts;

	public bool Sync;
}
