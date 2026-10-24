namespace Waylith.VideoDecode.H264;

internal sealed class H264Picture
{
	public int FrameNum;

	public int FrameNumWrap;

	public int PicNum;

	public int LongTermPicNum;

	public int LongTermFrameIdx;

	public int TopFieldOrderCnt;

	public int BottomFieldOrderCnt;

	public int PicOrderCnt;

	public int IdrPicId;

	public int NalRefIdc;

	public bool Idr;

	public bool Ref;

	public bool LongTerm;

	public bool Outputted;

	public bool MemMgmt5;

	public bool LongTermReferenceFlag;

	public bool AdaptiveRefPicMarkingMode;

	public H264DecRefPicMarking[] RefPicMarking;

	public int Slot = -1;

	public long Pts;

	public object Payload;
}
