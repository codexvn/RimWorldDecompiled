namespace Waylith.VideoDecode.H264;

internal sealed class H264Pps
{
	public int PicParameterSetId;

	public int SeqParameterSetId;

	public bool EntropyCodingMode;

	public bool BottomFieldPicOrderInFramePresent;

	public int NumSliceGroupsMinus1;

	public int NumRefIdxL0DefaultActiveMinus1;

	public int NumRefIdxL1DefaultActiveMinus1;

	public bool WeightedPred;

	public int WeightedBipredIdc;

	public int PicInitQpMinus26;

	public int PicInitQsMinus26;

	public int ChromaQpIndexOffset;

	public bool DeblockingFilterControlPresent;

	public bool ConstrainedIntraPred;

	public bool RedundantPicCntPresent;

	public bool Transform8x8Mode;

	public bool PicScalingMatrixPresent;

	public readonly byte[][] ScalingList4x4 = H264Sps.NewLists(16);

	public readonly byte[][] ScalingList8x8 = H264Sps.NewLists(64);

	public int SecondChromaQpIndexOffset;
}
