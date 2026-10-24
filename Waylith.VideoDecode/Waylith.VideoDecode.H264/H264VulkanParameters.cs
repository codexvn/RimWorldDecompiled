using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Brutal.Numerics;
using Brutal.VulkanApi;
using Brutal.VulkanApi.Abstractions;

namespace Waylith.VideoDecode.H264;

internal static class H264VulkanParameters
{
	public static StdVideoDecodeH264ReferenceInfo ReferenceInfo(H264Picture pic)
	{
		StdVideoDecodeH264ReferenceInfo result = new StdVideoDecodeH264ReferenceInfo();
		result.Flags = new StdVideoDecodeH264ReferenceInfoFlags
		{
			UsedForLongTermReference = pic.LongTerm
		};
		result.FrameNum = (ushort)(pic.LongTerm ? pic.LongTermFrameIdx : pic.FrameNum);
		result.PicOrderCnt = new int2(pic.TopFieldOrderCnt, pic.BottomFieldOrderCnt);
		return result;
	}

	private static StdVideoH264LevelIdc StdLevel(int levelIdc)
	{
		switch (levelIdc)
		{
		case 9:
		case 10:
			return StdVideoH264LevelIdc._10;
		case 11:
			return StdVideoH264LevelIdc._11;
		case 12:
			return StdVideoH264LevelIdc._12;
		case 13:
			return StdVideoH264LevelIdc._13;
		case 20:
			return StdVideoH264LevelIdc._20;
		case 21:
			return StdVideoH264LevelIdc._21;
		case 22:
			return StdVideoH264LevelIdc._22;
		case 30:
			return StdVideoH264LevelIdc._30;
		case 31:
			return StdVideoH264LevelIdc._31;
		case 32:
			return StdVideoH264LevelIdc._32;
		case 40:
			return StdVideoH264LevelIdc._40;
		case 41:
			return StdVideoH264LevelIdc._41;
		case 42:
			return StdVideoH264LevelIdc._42;
		case 50:
			return StdVideoH264LevelIdc._50;
		case 51:
			return StdVideoH264LevelIdc._51;
		case 52:
			return StdVideoH264LevelIdc._52;
		case 60:
			return StdVideoH264LevelIdc._60;
		case 61:
			return StdVideoH264LevelIdc._61;
		default:
			return StdVideoH264LevelIdc._62;
		}
	}

	private unsafe static void FillScalingLists(StdVideoH264ScalingLists* target, byte[][] lists4x4, byte[][] lists8x8)
	{
		target->ScalingListPresentMask = 255;
		target->UseDefaultScalingMatrixMask = 0;
		Span<byte> span = new Span<byte>(&target->ScalingList4x4, 96);
		Span<byte> span2 = new Span<byte>(&target->ScalingList8x8, 384);
		for (int i = 0; i < 6; i++)
		{
			lists4x4[i].CopyTo(span.Slice(i * 16));
			lists8x8[i].CopyTo(span2.Slice(i * 64));
		}
	}

	public unsafe static VkVideoSessionParametersKHR Create(DeviceEx device, VkVideoSessionKHR session, H264Parser parser)
	{
		H264Sps[] array = parser.ActiveSps.Values.ToArray();
		H264Pps[] array2 = parser.ActivePps.Values.ToArray();
		List<nint> allocations = new List<nint>();
		StdVideoH264SequenceParameterSet* ptr = Alloc<StdVideoH264SequenceParameterSet>(array.Length);
		for (int i = 0; i < array.Length; i++)
		{
			H264Sps h264Sps = array[i];
			int* ptr2 = Alloc<int>(255);
			h264Sps.OffsetForRefFrame.CopyTo(new Span<int>(ptr2, 255));
			ptr[i] = new StdVideoH264SequenceParameterSet
			{
				Flags = new StdVideoH264SpsFlags
				{
					ConstraintSet0Flag = h264Sps.ConstraintSet0,
					ConstraintSet1Flag = h264Sps.ConstraintSet1,
					ConstraintSet2Flag = h264Sps.ConstraintSet2,
					ConstraintSet3Flag = h264Sps.ConstraintSet3,
					ConstraintSet4Flag = h264Sps.ConstraintSet4,
					ConstraintSet5Flag = h264Sps.ConstraintSet5,
					Direct_8x8InferenceFlag = h264Sps.Direct8x8Inference,
					MbAdaptiveFrameFieldFlag = h264Sps.MbAdaptiveFrameField,
					FrameMbsOnlyFlag = h264Sps.FrameMbsOnly,
					DeltaPicOrderAlwaysZeroFlag = h264Sps.DeltaPicOrderAlwaysZero,
					SeparateColourPlaneFlag = h264Sps.SeparateColourPlane,
					GapsInFrameNumValueAllowedFlag = h264Sps.GapsInFrameNumValueAllowed,
					QpprimeYZeroTransformBypassFlag = h264Sps.QpprimeYZeroTransformBypass,
					FrameCroppingFlag = h264Sps.FrameCropping,
					SeqScalingMatrixPresentFlag = h264Sps.SeqScalingMatrixPresent,
					VuiParametersPresentFlag = h264Sps.VuiParametersPresent
				},
				ProfileIdc = (StdVideoH264ProfileIdc)h264Sps.ProfileIdc,
				LevelIdc = StdLevel(h264Sps.LevelIdc),
				ChromaFormatIdc = (StdVideoH264ChromaFormatIdc)h264Sps.ChromaFormatIdc,
				SeqParameterSetId = (byte)h264Sps.SeqParameterSetId,
				BitDepthLumaMinus8 = (byte)h264Sps.BitDepthLumaMinus8,
				BitDepthChromaMinus8 = (byte)h264Sps.BitDepthChromaMinus8,
				Log2MaxFrameNumMinus4 = (byte)h264Sps.Log2MaxFrameNumMinus4,
				PicOrderCntType = (StdVideoH264PocType)h264Sps.PicOrderCntType,
				OffsetForNonRefPic = h264Sps.OffsetForNonRefPic,
				OffsetForTopToBottomField = h264Sps.OffsetForTopToBottomField,
				Log2MaxPicOrderCntLsbMinus4 = (byte)h264Sps.Log2MaxPicOrderCntLsbMinus4,
				NumRefFramesInPicOrderCntCycle = (byte)h264Sps.NumRefFramesInPicOrderCntCycle,
				MaxNumRefFrames = (byte)h264Sps.MaxNumRefFrames,
				PicWidthInMbsMinus1 = h264Sps.PicWidthInMbsMinus1,
				PicHeightInMapUnitsMinus1 = h264Sps.PicHeightInMapUnitsMinus1,
				FrameCropLeftOffset = h264Sps.FrameCropLeftOffset,
				FrameCropRightOffset = h264Sps.FrameCropRightOffset,
				FrameCropTopOffset = h264Sps.FrameCropTopOffset,
				FrameCropBottomOffset = h264Sps.FrameCropBottomOffset,
				OffsetForRefFrame = ptr2
			};
			if (h264Sps.SeqScalingMatrixPresent)
			{
				ptr[i].ScalingLists = Alloc<StdVideoH264ScalingLists>();
				FillScalingLists(ptr[i].ScalingLists, h264Sps.ScalingList4x4, h264Sps.ScalingList8x8);
			}
			if (h264Sps.VuiParametersPresent)
			{
				StdVideoH264SequenceParameterSetVui* ptr3 = Alloc<StdVideoH264SequenceParameterSetVui>();
				*ptr3 = new StdVideoH264SequenceParameterSetVui
				{
					Flags = new StdVideoH264SpsVuiFlags
					{
						AspectRatioInfoPresentFlag = h264Sps.AspectRatioInfoPresent,
						VideoSignalTypePresentFlag = h264Sps.VideoSignalTypePresent,
						VideoFullRangeFlag = h264Sps.VideoFullRange,
						ColorDescriptionPresentFlag = h264Sps.ColourDescriptionPresent,
						TimingInfoPresentFlag = h264Sps.TimingInfoPresent,
						FixedFrameRateFlag = h264Sps.FixedFrameRate,
						BitstreamRestrictionFlag = h264Sps.BitstreamRestriction
					},
					AspectRatioIdc = (StdVideoH264AspectRatioIdc)h264Sps.AspectRatioIdc,
					SarWidth = (ushort)h264Sps.SarWidth,
					SarHeight = (ushort)h264Sps.SarHeight,
					VideoFormat = (byte)h264Sps.VideoFormat,
					ColourPrimaries = (byte)h264Sps.ColourPrimaries,
					TransferCharacteristics = (byte)h264Sps.TransferCharacteristics,
					MatrixCoefficients = (byte)h264Sps.MatrixCoefficients,
					NumUnitsInTick = (int)h264Sps.NumUnitsInTick,
					TimeScale = (int)h264Sps.TimeScale,
					MaxNumReorderFrames = (byte)h264Sps.MaxNumReorderFrames,
					MaxDecFrameBuffering = (byte)h264Sps.MaxDecFrameBuffering
				};
				ptr[i].SequenceParameterSetVui = ptr3;
			}
		}
		StdVideoH264PictureParameterSet* ptr4 = Alloc<StdVideoH264PictureParameterSet>(array2.Length);
		for (int j = 0; j < array2.Length; j++)
		{
			H264Pps h264Pps = array2[j];
			ptr4[j] = new StdVideoH264PictureParameterSet
			{
				Flags = new StdVideoH264PpsFlags
				{
					Transform_8x8ModeFlag = h264Pps.Transform8x8Mode,
					RedundantPicCntPresentFlag = h264Pps.RedundantPicCntPresent,
					ConstrainedIntraPredFlag = h264Pps.ConstrainedIntraPred,
					DeblockingFilterControlPresentFlag = h264Pps.DeblockingFilterControlPresent,
					WeightedPredFlag = h264Pps.WeightedPred,
					BottomFieldPicOrderInFramePresentFlag = h264Pps.BottomFieldPicOrderInFramePresent,
					EntropyCodingModeFlag = h264Pps.EntropyCodingMode,
					PicScalingMatrixPresentFlag = h264Pps.PicScalingMatrixPresent
				},
				SeqParameterSetId = (byte)h264Pps.SeqParameterSetId,
				PicParameterSetId = (byte)h264Pps.PicParameterSetId,
				NumRefIdxL0DefaultActiveMinus1 = (byte)h264Pps.NumRefIdxL0DefaultActiveMinus1,
				NumRefIdxL1DefaultActiveMinus1 = (byte)h264Pps.NumRefIdxL1DefaultActiveMinus1,
				WeightedBipredIdc = (StdVideoH264WeightedBipredIdc)h264Pps.WeightedBipredIdc,
				PicInitQpMinus26 = (byte)(sbyte)h264Pps.PicInitQpMinus26,
				PicInitQsMinus26 = (byte)(sbyte)h264Pps.PicInitQsMinus26,
				ChromaQpIndexOffset = (byte)(sbyte)h264Pps.ChromaQpIndexOffset,
				SecondChromaQpIndexOffset = (byte)(sbyte)h264Pps.SecondChromaQpIndexOffset
			};
			if (h264Pps.PicScalingMatrixPresent)
			{
				ptr4[j].ScalingLists = Alloc<StdVideoH264ScalingLists>();
				FillScalingLists(ptr4[j].ScalingLists, h264Pps.ScalingList4x4, h264Pps.ScalingList8x8);
			}
		}
		VkVideoDecodeH264SessionParametersAddInfoKHR vkVideoDecodeH264SessionParametersAddInfoKHR = new VkVideoDecodeH264SessionParametersAddInfoKHR();
		vkVideoDecodeH264SessionParametersAddInfoKHR.StdSPSCount = array.Length;
		vkVideoDecodeH264SessionParametersAddInfoKHR.StdSPSs = ptr;
		vkVideoDecodeH264SessionParametersAddInfoKHR.StdPPSCount = array2.Length;
		vkVideoDecodeH264SessionParametersAddInfoKHR.StdPPSs = ptr4;
		VkVideoDecodeH264SessionParametersAddInfoKHR vkVideoDecodeH264SessionParametersAddInfoKHR2 = vkVideoDecodeH264SessionParametersAddInfoKHR;
		VkVideoDecodeH264SessionParametersCreateInfoKHR vkVideoDecodeH264SessionParametersCreateInfoKHR = new VkVideoDecodeH264SessionParametersCreateInfoKHR();
		vkVideoDecodeH264SessionParametersCreateInfoKHR.MaxStdSPSCount = array.Length;
		vkVideoDecodeH264SessionParametersCreateInfoKHR.MaxStdPPSCount = array2.Length;
		vkVideoDecodeH264SessionParametersCreateInfoKHR.ParametersAddInfo = &vkVideoDecodeH264SessionParametersAddInfoKHR2;
		VkVideoDecodeH264SessionParametersCreateInfoKHR vkVideoDecodeH264SessionParametersCreateInfoKHR2 = vkVideoDecodeH264SessionParametersCreateInfoKHR;
		VkVideoSessionParametersCreateInfoKHR pCreateInfo = new VkVideoSessionParametersCreateInfoKHR
		{
			VideoSession = session,
			Next = &vkVideoDecodeH264SessionParametersCreateInfoKHR2
		};
		VkVideoSessionParametersKHR result = device.CreateVideoSessionParametersKHR(in pCreateInfo, device.VkAllocator);
		foreach (nint item in allocations)
		{
			NativeMemory.Free((void*)item);
		}
		return result;
		unsafe T* Alloc<T>(int count = 1) where T : unmanaged
		{
			T* ptr5 = (T*)NativeMemory.AllocZeroed((nuint)(sizeof(T) * count));
			allocations.Add((nint)ptr5);
			return ptr5;
		}
	}
}
