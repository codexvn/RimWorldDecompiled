namespace Waylith.VideoDecode.H264;

internal struct H264DecRefPicMarking
{
	public int Operation;

	public int DifferenceOfPicNumsMinus1;

	public int LongTermPicNum;

	public int LongTermFrameIdx;

	public int MaxLongTermFrameIdxPlus1;
}
