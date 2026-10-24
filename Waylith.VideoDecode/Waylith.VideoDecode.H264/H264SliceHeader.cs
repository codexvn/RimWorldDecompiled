namespace Waylith.VideoDecode.H264;

internal sealed class H264SliceHeader
{
	public const int MaxMarkingOperations = 32;

	public bool IdrPic;

	public int NalRefIdc;

	public int FirstMbInSlice;

	public int SliceType;

	public int PicParameterSetId;

	public int FrameNum;

	public int IdrPicId;

	public int PicOrderCntLsb;

	public int DeltaPicOrderCntBottom;

	public int DeltaPicOrderCnt0;

	public int DeltaPicOrderCnt1;

	public int RedundantPicCnt;

	public bool DirectSpatialMvPred;

	public int NumRefIdxL0ActiveMinus1;

	public int NumRefIdxL1ActiveMinus1;

	public bool NoOutputOfPriorPics;

	public bool LongTermReference;

	public bool AdaptiveRefPicMarkingMode;

	public readonly H264DecRefPicMarking[] RefPicMarking = new H264DecRefPicMarking[32];

	public bool IsPSlice => SliceType % 5 == 0;

	public bool IsBSlice => SliceType % 5 == 1;

	public bool IsISlice => SliceType % 5 == 2;

	public bool IsSPSlice => SliceType % 5 == 3;

	public bool IsSISlice => SliceType % 5 == 4;

	public bool HasMmco5
	{
		get
		{
			if (NalRefIdc == 0 || IdrPic || !AdaptiveRefPicMarkingMode)
			{
				return false;
			}
			H264DecRefPicMarking[] refPicMarking = RefPicMarking;
			for (int i = 0; i < refPicMarking.Length; i++)
			{
				H264DecRefPicMarking h264DecRefPicMarking = refPicMarking[i];
				if (h264DecRefPicMarking.Operation == 5)
				{
					return true;
				}
				if (h264DecRefPicMarking.Operation == 0)
				{
					return false;
				}
			}
			return false;
		}
	}
}
