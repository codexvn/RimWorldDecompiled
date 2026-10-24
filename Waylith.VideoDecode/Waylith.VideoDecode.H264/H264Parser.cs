using System;
using System.Collections.Generic;

namespace Waylith.VideoDecode.H264;

internal sealed class H264Parser
{
	public const int NalSlice = 1;

	public const int NalIdrSlice = 5;

	public const int NalSps = 7;

	public const int NalPps = 8;

	private static readonly byte[] Default4x4Intra = new byte[16]
	{
		6, 13, 13, 20, 20, 20, 28, 28, 28, 28,
		32, 32, 32, 37, 37, 42
	};

	private static readonly byte[] Default4x4Inter = new byte[16]
	{
		10, 14, 14, 20, 20, 20, 24, 24, 24, 24,
		27, 27, 27, 30, 30, 34
	};

	private static readonly byte[] Default8x8Intra = new byte[64]
	{
		6, 10, 10, 13, 11, 13, 16, 16, 16, 16,
		18, 18, 18, 18, 18, 23, 23, 23, 23, 23,
		23, 25, 25, 25, 25, 25, 25, 25, 27, 27,
		27, 27, 27, 27, 27, 27, 29, 29, 29, 29,
		29, 29, 29, 31, 31, 31, 31, 31, 31, 33,
		33, 33, 33, 33, 36, 36, 36, 36, 38, 38,
		38, 40, 40, 42
	};

	private static readonly byte[] Default8x8Inter = new byte[64]
	{
		9, 13, 13, 15, 13, 15, 17, 17, 17, 17,
		19, 19, 19, 19, 19, 21, 21, 21, 21, 21,
		21, 22, 22, 22, 22, 22, 22, 22, 24, 24,
		24, 24, 24, 24, 24, 24, 25, 25, 25, 25,
		25, 25, 25, 27, 27, 27, 27, 27, 27, 28,
		28, 28, 28, 28, 30, 30, 30, 30, 32, 32,
		32, 33, 33, 35
	};

	private static readonly int[] TableSarWidth = new int[17]
	{
		0, 1, 12, 10, 16, 40, 24, 20, 32, 80,
		18, 15, 64, 160, 4, 3, 2
	};

	private static readonly int[] TableSarHeight = new int[17]
	{
		0, 1, 11, 11, 11, 33, 11, 11, 11, 33,
		11, 11, 33, 99, 3, 2, 1
	};

	private const int ExtendedSar = 255;

	public readonly Dictionary<int, H264Sps> ActiveSps = new Dictionary<int, H264Sps>();

	public readonly Dictionary<int, H264Pps> ActivePps = new Dictionary<int, H264Pps>();

	private static void Require(bool condition, string what)
	{
		if (!condition)
		{
			throw new H264Exception("h264: invalid stream (" + what + ")");
		}
	}

	private static void InRange(int value, int min, int max, string what)
	{
		Require(value >= min && value <= max, what);
	}

	private static void FallbackScalingList4x4(int i, byte[] defaultIntra, byte[] defaultInter, byte[][] lists)
	{
		(i switch
		{
			0 => defaultIntra, 
			3 => defaultInter, 
			_ => lists[i - 1], 
		}).CopyTo(lists[i], 0);
	}

	private static void FallbackScalingList8x8(int i, byte[] defaultIntra, byte[] defaultInter, byte[][] lists)
	{
		(i switch
		{
			0 => defaultIntra, 
			1 => defaultInter, 
			_ => lists[i - 2], 
		}).CopyTo(lists[i], 0);
	}

	private static bool ParseScalingList(ref H264BitReader br, byte[] list)
	{
		int num = 8;
		int num2 = 8;
		for (int i = 0; i < list.Length; i++)
		{
			if (num2 != 0)
			{
				int num3 = br.ReadSE();
				InRange(num3, -128, 127, "delta_scale");
				num2 = (num + num3 + 256) & 0xFF;
				if (i == 0 && num2 == 0)
				{
					return true;
				}
			}
			list[i] = (byte)((num2 == 0) ? num : num2);
			num = list[i];
		}
		return false;
	}

	private static void ParseSpsScalingLists(ref H264BitReader br, H264Sps sps)
	{
		for (int i = 0; i < 6; i++)
		{
			if (br.ReadBool())
			{
				if (ParseScalingList(ref br, sps.ScalingList4x4[i]))
				{
					((i < 3) ? Default4x4Intra : Default4x4Inter).CopyTo(sps.ScalingList4x4[i], 0);
				}
			}
			else
			{
				FallbackScalingList4x4(i, Default4x4Intra, Default4x4Inter, sps.ScalingList4x4);
			}
		}
		for (int j = 0; j < ((sps.ChromaFormatIdc != 3) ? 2 : 6); j++)
		{
			if (br.ReadBool())
			{
				if (ParseScalingList(ref br, sps.ScalingList8x8[j]))
				{
					((j % 2 == 0) ? Default8x8Intra : Default8x8Inter).CopyTo(sps.ScalingList8x8[j], 0);
				}
			}
			else
			{
				FallbackScalingList8x8(j, Default8x8Intra, Default8x8Inter, sps.ScalingList8x8);
			}
		}
	}

	private static void ParsePpsScalingLists(ref H264BitReader br, H264Sps sps, H264Pps pps)
	{
		for (int i = 0; i < 6; i++)
		{
			if (br.ReadBool())
			{
				if (ParseScalingList(ref br, pps.ScalingList4x4[i]))
				{
					((i < 3) ? Default4x4Intra : Default4x4Inter).CopyTo(pps.ScalingList4x4[i], 0);
				}
			}
			else if (!sps.SeqScalingMatrixPresent)
			{
				FallbackScalingList4x4(i, Default4x4Intra, Default4x4Inter, pps.ScalingList4x4);
			}
			else
			{
				FallbackScalingList4x4(i, sps.ScalingList4x4[0], sps.ScalingList4x4[3], pps.ScalingList4x4);
			}
		}
		if (!pps.Transform8x8Mode)
		{
			return;
		}
		for (int j = 0; j < ((sps.ChromaFormatIdc != 3) ? 2 : 6); j++)
		{
			if (br.ReadBool())
			{
				if (ParseScalingList(ref br, pps.ScalingList8x8[j]))
				{
					((j % 2 == 0) ? Default8x8Intra : Default8x8Inter).CopyTo(pps.ScalingList8x8[j], 0);
				}
			}
			else if (!sps.SeqScalingMatrixPresent)
			{
				FallbackScalingList8x8(j, Default8x8Intra, Default8x8Inter, pps.ScalingList8x8);
			}
			else
			{
				FallbackScalingList8x8(j, sps.ScalingList8x8[0], sps.ScalingList8x8[1], pps.ScalingList8x8);
			}
		}
	}

	private static bool ParseAndIgnoreHrdParameters(ref H264BitReader br)
	{
		if (!br.ReadBool())
		{
			return false;
		}
		int num = br.ReadUE();
		InRange(num, 0, 31, "cpb_cnt_minus1");
		br.ReadBits(8);
		for (int i = 0; i <= num; i++)
		{
			br.ReadUE();
			br.ReadUE();
			br.ReadBool();
		}
		br.ReadBits(20);
		return true;
	}

	private static void ParseVuiParameters(ref H264BitReader br, H264Sps sps)
	{
		sps.AspectRatioInfoPresent = br.ReadBool();
		if (sps.AspectRatioInfoPresent)
		{
			sps.AspectRatioIdc = br.ReadBits(8);
			if (sps.AspectRatioIdc == 255)
			{
				sps.SarWidth = br.ReadBits(16);
				sps.SarHeight = br.ReadBits(16);
			}
			else
			{
				InRange(sps.AspectRatioIdc, 0, TableSarWidth.Length - 1, "aspect_ratio_idc");
				sps.SarWidth = TableSarWidth[sps.AspectRatioIdc];
				sps.SarHeight = TableSarHeight[sps.AspectRatioIdc];
			}
		}
		if (br.ReadBool())
		{
			br.ReadBool();
		}
		sps.VideoSignalTypePresent = br.ReadBool();
		if (sps.VideoSignalTypePresent)
		{
			sps.VideoFormat = br.ReadBits(3);
			sps.VideoFullRange = br.ReadBool();
			sps.ColourDescriptionPresent = br.ReadBool();
			if (sps.ColourDescriptionPresent)
			{
				sps.ColourPrimaries = br.ReadBits(8);
				sps.TransferCharacteristics = br.ReadBits(8);
				sps.MatrixCoefficients = br.ReadBits(8);
			}
		}
		if (br.ReadBool())
		{
			br.ReadUE();
			br.ReadUE();
		}
		sps.TimingInfoPresent = br.ReadBool();
		if (sps.TimingInfoPresent)
		{
			sps.NumUnitsInTick = (uint)((br.ReadBits(16) << 16) | br.ReadBits(16));
			sps.TimeScale = (uint)((br.ReadBits(16) << 16) | br.ReadBits(16));
			sps.FixedFrameRate = br.ReadBool();
		}
		bool num = ParseAndIgnoreHrdParameters(ref br);
		bool flag = ParseAndIgnoreHrdParameters(ref br);
		if (num || flag)
		{
			br.ReadBool();
		}
		br.ReadBool();
		sps.BitstreamRestriction = br.ReadBool();
		if (sps.BitstreamRestriction)
		{
			br.ReadBool();
			br.ReadUE();
			br.ReadUE();
			br.ReadUE();
			br.ReadUE();
			sps.MaxNumReorderFrames = br.ReadUE();
			sps.MaxDecFrameBuffering = br.ReadUE();
			Require(sps.MaxDecFrameBuffering >= sps.MaxNumRefFrames, "max_dec_frame_buffering");
			InRange(sps.MaxNumReorderFrames, 0, sps.MaxDecFrameBuffering, "max_num_reorder_frames");
		}
	}

	private static void FillFlatScalingLists(H264Sps sps)
	{
		byte[][] scalingList4x = sps.ScalingList4x4;
		for (int i = 0; i < scalingList4x.Length; i++)
		{
			Array.Fill(scalingList4x[i], (byte)16);
		}
		scalingList4x = sps.ScalingList8x8;
		for (int i = 0; i < scalingList4x.Length; i++)
		{
			Array.Fill(scalingList4x[i], (byte)16);
		}
	}

	public H264Sps ParseSps(ReadOnlySpan<byte> nal)
	{
		H264BitReader br = new H264BitReader(nal.Slice(1));
		H264Sps h264Sps = new H264Sps();
		h264Sps.ProfileIdc = br.ReadBits(8);
		h264Sps.ConstraintSet0 = br.ReadBool();
		h264Sps.ConstraintSet1 = br.ReadBool();
		h264Sps.ConstraintSet2 = br.ReadBool();
		h264Sps.ConstraintSet3 = br.ReadBool();
		h264Sps.ConstraintSet4 = br.ReadBool();
		h264Sps.ConstraintSet5 = br.ReadBool();
		br.ReadBits(2);
		h264Sps.LevelIdc = br.ReadBits(8);
		h264Sps.SeqParameterSetId = br.ReadUE();
		Require(h264Sps.SeqParameterSetId < 32, "seq_parameter_set_id");
		bool flag;
		switch (h264Sps.ProfileIdc)
		{
		case 44:
		case 83:
		case 86:
		case 100:
		case 110:
		case 118:
		case 122:
		case 128:
		case 244:
			flag = true;
			break;
		default:
			flag = false;
			break;
		}
		if (flag)
		{
			h264Sps.ChromaFormatIdc = br.ReadUE();
			Require(h264Sps.ChromaFormatIdc < 4, "chroma_format_idc");
			if (h264Sps.ChromaFormatIdc == 3)
			{
				h264Sps.SeparateColourPlane = br.ReadBool();
			}
			h264Sps.BitDepthLumaMinus8 = br.ReadUE();
			Require(h264Sps.BitDepthLumaMinus8 < 7, "bit_depth_luma_minus8");
			h264Sps.BitDepthChromaMinus8 = br.ReadUE();
			Require(h264Sps.BitDepthChromaMinus8 < 7, "bit_depth_chroma_minus8");
			h264Sps.QpprimeYZeroTransformBypass = br.ReadBool();
			h264Sps.SeqScalingMatrixPresent = br.ReadBool();
			if (h264Sps.SeqScalingMatrixPresent)
			{
				ParseSpsScalingLists(ref br, h264Sps);
			}
			else
			{
				FillFlatScalingLists(h264Sps);
			}
		}
		else
		{
			h264Sps.ChromaFormatIdc = 1;
			FillFlatScalingLists(h264Sps);
		}
		h264Sps.Log2MaxFrameNumMinus4 = br.ReadUE();
		Require(h264Sps.Log2MaxFrameNumMinus4 < 13, "log2_max_frame_num_minus4");
		h264Sps.PicOrderCntType = br.ReadUE();
		Require(h264Sps.PicOrderCntType < 3, "pic_order_cnt_type");
		if (h264Sps.PicOrderCntType == 0)
		{
			h264Sps.Log2MaxPicOrderCntLsbMinus4 = br.ReadUE();
			Require(h264Sps.Log2MaxPicOrderCntLsbMinus4 < 13, "log2_max_pic_order_cnt_lsb_minus4");
		}
		else if (h264Sps.PicOrderCntType == 1)
		{
			h264Sps.DeltaPicOrderAlwaysZero = br.ReadBool();
			h264Sps.OffsetForNonRefPic = br.ReadSE();
			h264Sps.OffsetForTopToBottomField = br.ReadSE();
			h264Sps.NumRefFramesInPicOrderCntCycle = br.ReadUE();
			Require(h264Sps.NumRefFramesInPicOrderCntCycle < 255, "num_ref_frames_in_pic_order_cnt_cycle");
			long num = 0L;
			for (int i = 0; i < h264Sps.NumRefFramesInPicOrderCntCycle; i++)
			{
				h264Sps.OffsetForRefFrame[i] = br.ReadSE();
				num += h264Sps.OffsetForRefFrame[i];
			}
			Require(num >= int.MinValue && num <= int.MaxValue, "offset_for_ref_frame");
			h264Sps.ExpectedDeltaPerPicOrderCntCycle = (int)num;
		}
		h264Sps.MaxNumRefFrames = br.ReadUE();
		InRange(h264Sps.MaxNumRefFrames, 0, 16, "max_num_ref_frames");
		h264Sps.GapsInFrameNumValueAllowed = br.ReadBool();
		h264Sps.PicWidthInMbsMinus1 = br.ReadUE();
		InRange(h264Sps.PicWidthInMbsMinus1, 0, 543, "pic_width_in_mbs_minus1");
		h264Sps.PicHeightInMapUnitsMinus1 = br.ReadUE();
		InRange(h264Sps.PicHeightInMapUnitsMinus1, 0, 543, "pic_height_in_map_units_minus1");
		h264Sps.FrameMbsOnly = br.ReadBool();
		if (!h264Sps.FrameMbsOnly)
		{
			h264Sps.MbAdaptiveFrameField = br.ReadBool();
		}
		h264Sps.Direct8x8Inference = br.ReadBool();
		h264Sps.FrameCropping = br.ReadBool();
		if (h264Sps.FrameCropping)
		{
			h264Sps.FrameCropLeftOffset = br.ReadUE();
			h264Sps.FrameCropRightOffset = br.ReadUE();
			h264Sps.FrameCropTopOffset = br.ReadUE();
			h264Sps.FrameCropBottomOffset = br.ReadUE();
		}
		h264Sps.VuiParametersPresent = br.ReadBool();
		if (h264Sps.VuiParametersPresent)
		{
			ParseVuiParameters(ref br, h264Sps);
		}
		ActiveSps[h264Sps.SeqParameterSetId] = h264Sps;
		return h264Sps;
	}

	public H264Pps ParsePps(ReadOnlySpan<byte> nal)
	{
		H264BitReader br = new H264BitReader(nal.Slice(1));
		H264Pps h264Pps = new H264Pps();
		h264Pps.PicParameterSetId = br.ReadUE();
		InRange(h264Pps.PicParameterSetId, 0, 255, "pic_parameter_set_id");
		h264Pps.SeqParameterSetId = br.ReadUE();
		Require(h264Pps.SeqParameterSetId < 32, "seq_parameter_set_id");
		Require(ActiveSps.TryGetValue(h264Pps.SeqParameterSetId, out var value), "pps without sps");
		h264Pps.EntropyCodingMode = br.ReadBool();
		h264Pps.BottomFieldPicOrderInFramePresent = br.ReadBool();
		h264Pps.NumSliceGroupsMinus1 = br.ReadUE();
		if (h264Pps.NumSliceGroupsMinus1 > 0)
		{
			throw new H264Exception("h264: slice groups are not supported");
		}
		h264Pps.NumRefIdxL0DefaultActiveMinus1 = br.ReadUE();
		Require(h264Pps.NumRefIdxL0DefaultActiveMinus1 < 32, "num_ref_idx_l0_default_active_minus1");
		h264Pps.NumRefIdxL1DefaultActiveMinus1 = br.ReadUE();
		Require(h264Pps.NumRefIdxL1DefaultActiveMinus1 < 32, "num_ref_idx_l1_default_active_minus1");
		h264Pps.WeightedPred = br.ReadBool();
		h264Pps.WeightedBipredIdc = br.ReadBits(2);
		Require(h264Pps.WeightedBipredIdc < 3, "weighted_bipred_idc");
		h264Pps.PicInitQpMinus26 = br.ReadSE();
		InRange(h264Pps.PicInitQpMinus26, -26, 25, "pic_init_qp_minus26");
		h264Pps.PicInitQsMinus26 = br.ReadSE();
		InRange(h264Pps.PicInitQsMinus26, -26, 25, "pic_init_qs_minus26");
		h264Pps.ChromaQpIndexOffset = br.ReadSE();
		InRange(h264Pps.ChromaQpIndexOffset, -12, 12, "chroma_qp_index_offset");
		h264Pps.SecondChromaQpIndexOffset = h264Pps.ChromaQpIndexOffset;
		h264Pps.DeblockingFilterControlPresent = br.ReadBool();
		h264Pps.ConstrainedIntraPred = br.ReadBool();
		h264Pps.RedundantPicCntPresent = br.ReadBool();
		bool flag = br.HasMoreRbspData();
		if (flag)
		{
			int profileIdc = value.ProfileIdc;
			bool flag2 = ((profileIdc == 66 || profileIdc == 77) ? true : false);
			flag = !flag2;
		}
		if (flag)
		{
			h264Pps.Transform8x8Mode = br.ReadBool();
			h264Pps.PicScalingMatrixPresent = br.ReadBool();
			if (h264Pps.PicScalingMatrixPresent)
			{
				ParsePpsScalingLists(ref br, value, h264Pps);
			}
			h264Pps.SecondChromaQpIndexOffset = br.ReadSE();
			InRange(h264Pps.SecondChromaQpIndexOffset, -12, 12, "second_chroma_qp_index_offset");
		}
		ActivePps[h264Pps.PicParameterSetId] = h264Pps;
		return h264Pps;
	}

	private static void SkipRefPicListModification(ref H264BitReader br, int numRefIdxActiveMinus1)
	{
		Require(numRefIdxActiveMinus1 < 32, "num_ref_idx_active_minus1");
		for (int i = 0; i < 32; i++)
		{
			int num = br.ReadUE();
			Require(num < 4, "modification_of_pic_nums_idc");
			if (num == 3)
			{
				Require(i != 0, "empty ref pic list modification");
				return;
			}
			br.ReadUE();
		}
		Require(br.ReadUE() == 3, "unterminated ref pic list modification");
	}

	private static void SkipWeightingFactors(ref H264BitReader br, int numRefIdxActiveMinus1, int chromaArrayType)
	{
		for (int i = 0; i < numRefIdxActiveMinus1 + 1; i++)
		{
			if (br.ReadBool())
			{
				br.ReadSE();
				br.ReadSE();
			}
			if (chromaArrayType != 0 && br.ReadBool())
			{
				for (int j = 0; j < 2; j++)
				{
					br.ReadSE();
					br.ReadSE();
				}
			}
		}
	}

	private static void ParseDecRefPicMarking(ref H264BitReader br, H264SliceHeader shdr)
	{
		if (shdr.IdrPic)
		{
			shdr.NoOutputOfPriorPics = br.ReadBool();
			shdr.LongTermReference = br.ReadBool();
			return;
		}
		shdr.AdaptiveRefPicMarkingMode = br.ReadBool();
		if (!shdr.AdaptiveRefPicMarkingMode)
		{
			return;
		}
		int i;
		for (i = 0; i < shdr.RefPicMarking.Length; i++)
		{
			ref H264DecRefPicMarking reference = ref shdr.RefPicMarking[i];
			reference.Operation = br.ReadUE();
			if (reference.Operation == 0)
			{
				break;
			}
			int operation = reference.Operation;
			if ((operation == 1 || operation == 3) ? true : false)
			{
				reference.DifferenceOfPicNumsMinus1 = br.ReadUE();
			}
			if (reference.Operation == 2)
			{
				reference.LongTermPicNum = br.ReadUE();
			}
			operation = reference.Operation;
			if ((operation == 3 || operation == 6) ? true : false)
			{
				reference.LongTermFrameIdx = br.ReadUE();
			}
			if (reference.Operation == 4)
			{
				reference.MaxLongTermFrameIdxPlus1 = br.ReadUE();
			}
			Require(reference.Operation <= 6, "memory_management_control_operation");
		}
		if (i != shdr.RefPicMarking.Length)
		{
			return;
		}
		throw new H264Exception("h264: too many memory management control operations");
	}

	public H264SliceHeader ParseSliceHeader(ReadOnlySpan<byte> nal)
	{
		int num = (nal[0] >> 5) & 3;
		int num2 = nal[0] & 0x1F;
		H264BitReader br = new H264BitReader(nal.Slice(1));
		H264SliceHeader h264SliceHeader = new H264SliceHeader
		{
			IdrPic = (num2 == 5),
			NalRefIdc = num
		};
		h264SliceHeader.FirstMbInSlice = br.ReadUE();
		h264SliceHeader.SliceType = br.ReadUE();
		Require(h264SliceHeader.SliceType < 10, "slice_type");
		h264SliceHeader.PicParameterSetId = br.ReadUE();
		Require(ActivePps.TryGetValue(h264SliceHeader.PicParameterSetId, out var value), "slice without pps");
		Require(ActiveSps.TryGetValue(value.SeqParameterSetId, out var value2), "slice without sps");
		if (value2.SeparateColourPlane)
		{
			throw new H264Exception("h264: separate colour planes are not supported");
		}
		h264SliceHeader.FrameNum = br.ReadBits(value2.Log2MaxFrameNumMinus4 + 4);
		if (!value2.FrameMbsOnly && br.ReadBool())
		{
			throw new H264Exception("h264: interlaced streams are not supported");
		}
		int num3 = (value2.PicWidthInMbsMinus1 + 1) * (value2.PicHeightInMapUnitsMinus1 + 1) * (value2.FrameMbsOnly ? 1 : 2);
		InRange(h264SliceHeader.FirstMbInSlice, 0, num3 - 1, "first_mb_in_slice");
		if (h264SliceHeader.IdrPic)
		{
			h264SliceHeader.IdrPicId = br.ReadUE();
			InRange(h264SliceHeader.IdrPicId, 0, 65535, "idr_pic_id");
		}
		if (value2.PicOrderCntType == 0)
		{
			h264SliceHeader.PicOrderCntLsb = br.ReadBits(value2.Log2MaxPicOrderCntLsbMinus4 + 4);
			if (value.BottomFieldPicOrderInFramePresent)
			{
				h264SliceHeader.DeltaPicOrderCntBottom = br.ReadSE();
			}
		}
		if (value2.PicOrderCntType == 1 && !value2.DeltaPicOrderAlwaysZero)
		{
			h264SliceHeader.DeltaPicOrderCnt0 = br.ReadSE();
			if (value.BottomFieldPicOrderInFramePresent)
			{
				h264SliceHeader.DeltaPicOrderCnt1 = br.ReadSE();
			}
		}
		if (value.RedundantPicCntPresent)
		{
			h264SliceHeader.RedundantPicCnt = br.ReadUE();
			Require(h264SliceHeader.RedundantPicCnt < 128, "redundant_pic_cnt");
		}
		if (h264SliceHeader.IsBSlice)
		{
			h264SliceHeader.DirectSpatialMvPred = br.ReadBool();
		}
		if (h264SliceHeader.IsPSlice || h264SliceHeader.IsSPSlice || h264SliceHeader.IsBSlice)
		{
			if (br.ReadBool())
			{
				h264SliceHeader.NumRefIdxL0ActiveMinus1 = br.ReadUE();
				if (h264SliceHeader.IsBSlice)
				{
					h264SliceHeader.NumRefIdxL1ActiveMinus1 = br.ReadUE();
				}
			}
			else
			{
				h264SliceHeader.NumRefIdxL0ActiveMinus1 = value.NumRefIdxL0DefaultActiveMinus1;
				if (h264SliceHeader.IsBSlice)
				{
					h264SliceHeader.NumRefIdxL1ActiveMinus1 = value.NumRefIdxL1DefaultActiveMinus1;
				}
			}
		}
		Require(h264SliceHeader.NumRefIdxL0ActiveMinus1 < 16, "num_ref_idx_l0_active_minus1");
		Require(h264SliceHeader.NumRefIdxL1ActiveMinus1 < 16, "num_ref_idx_l1_active_minus1");
		if (!h264SliceHeader.IsISlice && !h264SliceHeader.IsSISlice && br.ReadBool())
		{
			SkipRefPicListModification(ref br, h264SliceHeader.NumRefIdxL0ActiveMinus1);
		}
		if (h264SliceHeader.IsBSlice && br.ReadBool())
		{
			SkipRefPicListModification(ref br, h264SliceHeader.NumRefIdxL1ActiveMinus1);
		}
		if ((value.WeightedPred && (h264SliceHeader.IsPSlice || h264SliceHeader.IsSPSlice)) || (value.WeightedBipredIdc == 1 && h264SliceHeader.IsBSlice))
		{
			Require(br.ReadUE() < 8, "luma_log2_weight_denom");
			if (value2.ChromaArrayType != 0)
			{
				Require(br.ReadUE() < 8, "chroma_log2_weight_denom");
			}
			SkipWeightingFactors(ref br, h264SliceHeader.NumRefIdxL0ActiveMinus1, value2.ChromaArrayType);
			if (h264SliceHeader.IsBSlice)
			{
				SkipWeightingFactors(ref br, h264SliceHeader.NumRefIdxL1ActiveMinus1, value2.ChromaArrayType);
			}
		}
		if (num != 0)
		{
			ParseDecRefPicMarking(ref br, h264SliceHeader);
		}
		return h264SliceHeader;
	}
}
