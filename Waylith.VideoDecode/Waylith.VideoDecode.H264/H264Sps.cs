namespace Waylith.VideoDecode.H264;

internal sealed class H264Sps
{
	public int ProfileIdc;

	public bool ConstraintSet0;

	public bool ConstraintSet1;

	public bool ConstraintSet2;

	public bool ConstraintSet3;

	public bool ConstraintSet4;

	public bool ConstraintSet5;

	public int LevelIdc;

	public int SeqParameterSetId;

	public int ChromaFormatIdc;

	public bool SeparateColourPlane;

	public int BitDepthLumaMinus8;

	public int BitDepthChromaMinus8;

	public bool QpprimeYZeroTransformBypass;

	public bool SeqScalingMatrixPresent;

	public readonly byte[][] ScalingList4x4 = NewLists(16);

	public readonly byte[][] ScalingList8x8 = NewLists(64);

	public int Log2MaxFrameNumMinus4;

	public int PicOrderCntType;

	public int Log2MaxPicOrderCntLsbMinus4;

	public bool DeltaPicOrderAlwaysZero;

	public int OffsetForNonRefPic;

	public int OffsetForTopToBottomField;

	public int NumRefFramesInPicOrderCntCycle;

	public int ExpectedDeltaPerPicOrderCntCycle;

	public readonly int[] OffsetForRefFrame = new int[255];

	public int MaxNumRefFrames;

	public bool GapsInFrameNumValueAllowed;

	public int PicWidthInMbsMinus1;

	public int PicHeightInMapUnitsMinus1;

	public bool FrameMbsOnly;

	public bool MbAdaptiveFrameField;

	public bool Direct8x8Inference;

	public bool FrameCropping;

	public int FrameCropLeftOffset;

	public int FrameCropRightOffset;

	public int FrameCropTopOffset;

	public int FrameCropBottomOffset;

	public bool VuiParametersPresent;

	public bool AspectRatioInfoPresent;

	public int AspectRatioIdc;

	public int SarWidth;

	public int SarHeight;

	public bool VideoSignalTypePresent;

	public int VideoFormat;

	public bool VideoFullRange;

	public bool ColourDescriptionPresent;

	public int ColourPrimaries;

	public int TransferCharacteristics;

	public int MatrixCoefficients;

	public bool TimingInfoPresent;

	public uint NumUnitsInTick;

	public uint TimeScale;

	public bool FixedFrameRate;

	public bool BitstreamRestriction;

	public int MaxNumReorderFrames;

	public int MaxDecFrameBuffering;

	public int ChromaArrayType
	{
		get
		{
			if (!SeparateColourPlane)
			{
				return ChromaFormatIdc;
			}
			return 0;
		}
	}

	public int CodedWidth => (PicWidthInMbsMinus1 + 1) * 16;

	public int CodedHeight => (PicHeightInMapUnitsMinus1 + 1) * 16 * (FrameMbsOnly ? 1 : 2);

	public int CropX => FrameCropLeftOffset * 2;

	public int CropY => FrameCropTopOffset * 2;

	public int VisibleWidth => CodedWidth - (FrameCropLeftOffset + FrameCropRightOffset) * 2;

	public int VisibleHeight => CodedHeight - (FrameCropTopOffset + FrameCropBottomOffset) * 2;

	internal static byte[][] NewLists(int length)
	{
		byte[][] array = new byte[6][];
		for (int i = 0; i < 6; i++)
		{
			array[i] = new byte[length];
		}
		return array;
	}
}
