using System;
using System.Collections.Generic;
using System.Linq;

namespace Waylith.VideoDecode.H264;

internal sealed class H264PictureManager
{
	public const int DpbMaxSize = 16;

	private readonly List<H264Picture> dpb = new List<H264Picture>();

	private int maxDpbSize = 16;

	private int maxNumReorderFrames;

	private int maxNumRefFrames = 1;

	private int maxFrameNum;

	private int maxLongTermFrameIdx = -1;

	private int refPicOrderCntMsb;

	private int refPicOrderCntLsb;

	private int prevFrameNum;

	private int prevFrameNumOffset;

	private int prevRefFrameNum;

	public IReadOnlyList<H264Picture> Dpb => dpb;

	public int MaxDpbSize => maxDpbSize;

	public int MaxNumReorderFrames => maxNumReorderFrames;

	public static int LevelToMaxDpbMbs(int level)
	{
		switch (level)
		{
		case 9:
		case 10:
			return 396;
		case 11:
			return 900;
		case 12:
		case 13:
		case 20:
			return 2376;
		case 21:
			return 4752;
		case 22:
		case 30:
			return 8100;
		case 31:
			return 18000;
		case 32:
			return 20480;
		case 40:
		case 41:
			return 32768;
		case 42:
			return 34816;
		case 50:
			return 110400;
		case 51:
		case 52:
			return 184320;
		case 60:
		case 61:
		case 62:
			return 696320;
		default:
			return 0;
		}
	}

	public static int ComputeDpbSize(H264Sps sps)
	{
		int num = sps.LevelIdc;
		int profileIdc = sps.ProfileIdc;
		bool flag = ((profileIdc == 66 || profileIdc == 77) ? true : false);
		if (flag && num == 11 && sps.ConstraintSet3)
		{
			num = 9;
		}
		int num2 = LevelToMaxDpbMbs(num);
		if (num2 == 0)
		{
			throw new H264Exception("h264: unknown level " + sps.LevelIdc);
		}
		int num3 = sps.CodedWidth / 16 * (sps.CodedHeight / 16);
		int num4 = System.Math.Max(System.Math.Min(num2 / num3, 16), System.Math.Max(sps.MaxNumRefFrames, sps.MaxDecFrameBuffering));
		if (num4 == 0 || num4 > 16)
		{
			throw new H264Exception("h264: invalid dpb size " + num4);
		}
		return num4;
	}

	public void Configure(H264Sps sps)
	{
		maxDpbSize = ComputeDpbSize(sps);
		maxNumRefFrames = System.Math.Max(sps.MaxNumRefFrames, 1);
		maxFrameNum = 1 << sps.Log2MaxFrameNumMinus4 + 4;
		if (sps.VuiParametersPresent && sps.BitstreamRestriction)
		{
			maxNumReorderFrames = System.Math.Min(sps.MaxNumReorderFrames, maxDpbSize);
			return;
		}
		bool flag = sps.ConstraintSet3;
		if (flag)
		{
			bool flag2;
			switch (sps.ProfileIdc)
			{
			case 44:
			case 86:
			case 100:
			case 110:
			case 122:
			case 244:
				flag2 = true;
				break;
			default:
				flag2 = false;
				break;
			}
			flag = flag2;
		}
		if (flag)
		{
			maxNumReorderFrames = 0;
		}
		else
		{
			maxNumReorderFrames = maxDpbSize;
		}
	}

	public void Reset()
	{
		dpb.Clear();
		maxLongTermFrameIdx = -1;
		refPicOrderCntMsb = 0;
		refPicOrderCntLsb = 0;
		prevFrameNum = 0;
		prevFrameNumOffset = 0;
		prevRefFrameNum = 0;
	}

	public H264Picture BeginPicture(H264Sps sps, H264SliceHeader hdr, List<H264Picture> outputs)
	{
		if (hdr.IdrPic)
		{
			if (!hdr.NoOutputOfPriorPics)
			{
				OutputAllRemaining(outputs);
			}
			dpb.Clear();
			prevRefFrameNum = 0;
		}
		if (hdr.FrameNum != prevRefFrameNum && hdr.FrameNum != (prevRefFrameNum + 1) % maxFrameNum && sps.GapsInFrameNumValueAllowed)
		{
			throw new H264Exception("h264: gaps in frame_num are not supported");
		}
		H264Picture h264Picture = new H264Picture
		{
			Idr = hdr.IdrPic,
			IdrPicId = hdr.IdrPicId,
			NalRefIdc = hdr.NalRefIdc,
			Ref = (hdr.NalRefIdc != 0),
			LongTermReferenceFlag = hdr.LongTermReference,
			AdaptiveRefPicMarkingMode = hdr.AdaptiveRefPicMarkingMode,
			RefPicMarking = hdr.RefPicMarking,
			FrameNum = hdr.FrameNum,
			PicNum = hdr.FrameNum
		};
		ComputePicOrderCnt(sps, hdr, h264Picture);
		UpdatePicNums(hdr.FrameNum);
		return h264Picture;
	}

	private void ComputePicOrderCnt(H264Sps sps, H264SliceHeader hdr, H264Picture pic)
	{
		bool hasMmco = hdr.HasMmco5;
		int num2;
		int num3;
		switch (sps.PicOrderCntType)
		{
		case 0:
		{
			int num4 = 1 << sps.Log2MaxPicOrderCntLsbMinus4 + 4;
			int num5 = ((!hdr.IdrPic) ? refPicOrderCntMsb : 0);
			int num6 = ((!hdr.IdrPic) ? refPicOrderCntLsb : 0);
			int num7 = ((hdr.PicOrderCntLsb < num6 && num6 - hdr.PicOrderCntLsb >= num4 / 2) ? (num5 + num4) : ((hdr.PicOrderCntLsb <= num6 || hdr.PicOrderCntLsb - num6 <= num4 / 2) ? num5 : (num5 - num4)));
			num2 = num7 + hdr.PicOrderCntLsb;
			num3 = num2 + hdr.DeltaPicOrderCntBottom;
			if (hdr.NalRefIdc != 0)
			{
				refPicOrderCntMsb = ((!hasMmco) ? num7 : 0);
				refPicOrderCntLsb = (hasMmco ? (num2 - System.Math.Min(num2, num3)) : hdr.PicOrderCntLsb);
			}
			break;
		}
		case 1:
		{
			int num8 = FrameNumOffset(hdr);
			int num9 = ((sps.NumRefFramesInPicOrderCntCycle != 0) ? (num8 + hdr.FrameNum) : 0);
			if (hdr.NalRefIdc == 0 && num9 > 0)
			{
				num9--;
			}
			int num10 = 0;
			if (num9 > 0)
			{
				int num11 = (num9 - 1) / sps.NumRefFramesInPicOrderCntCycle;
				int num12 = (num9 - 1) % sps.NumRefFramesInPicOrderCntCycle;
				num10 = num11 * sps.ExpectedDeltaPerPicOrderCntCycle;
				for (int i = 0; i <= num12; i++)
				{
					num10 += sps.OffsetForRefFrame[i];
				}
			}
			if (hdr.NalRefIdc == 0)
			{
				num10 += sps.OffsetForNonRefPic;
			}
			num2 = num10 + hdr.DeltaPicOrderCnt0;
			num3 = num2 + sps.OffsetForTopToBottomField + hdr.DeltaPicOrderCnt1;
			prevFrameNumOffset = ((!hasMmco) ? num8 : 0);
			break;
		}
		default:
		{
			int num = FrameNumOffset(hdr);
			num2 = ((!hdr.IdrPic) ? ((hdr.NalRefIdc != 0) ? (2 * (num + hdr.FrameNum)) : (2 * (num + hdr.FrameNum) - 1)) : 0);
			num3 = num2;
			prevFrameNumOffset = ((!hasMmco) ? num : 0);
			break;
		}
		}
		prevFrameNum = ((!hasMmco) ? hdr.FrameNum : 0);
		pic.TopFieldOrderCnt = num2;
		pic.BottomFieldOrderCnt = num3;
		pic.PicOrderCnt = System.Math.Min(num2, num3);
	}

	private int FrameNumOffset(H264SliceHeader hdr)
	{
		if (hdr.IdrPic)
		{
			return 0;
		}
		if (prevFrameNum <= hdr.FrameNum)
		{
			return prevFrameNumOffset;
		}
		return prevFrameNumOffset + maxFrameNum;
	}

	private void UpdatePicNums(int frameNum)
	{
		foreach (H264Picture item in dpb)
		{
			if (item.Ref)
			{
				if (item.LongTerm)
				{
					item.LongTermPicNum = item.LongTermFrameIdx;
					continue;
				}
				item.FrameNumWrap = ((item.FrameNum > frameNum) ? (item.FrameNum - maxFrameNum) : item.FrameNum);
				item.PicNum = item.FrameNumWrap;
			}
		}
	}

	private H264Picture ShortRefByPicNum(int picNum)
	{
		foreach (H264Picture item in dpb)
		{
			if (item.Ref && !item.LongTerm && item.PicNum == picNum)
			{
				return item;
			}
		}
		return null;
	}

	private void MarkAllUnusedForRef()
	{
		foreach (H264Picture item in dpb)
		{
			item.Ref = false;
		}
	}

	private void HandleMemoryManagementOps(H264Picture pic)
	{
		H264DecRefPicMarking[] refPicMarking = pic.RefPicMarking;
		for (int i = 0; i < refPicMarking.Length; i++)
		{
			H264DecRefPicMarking marking = refPicMarking[i];
			switch (marking.Operation)
			{
			case 0:
				return;
			case 1:
			{
				H264Picture h264Picture2 = ShortRefByPicNum(pic.PicNum - (marking.DifferenceOfPicNumsMinus1 + 1));
				if (h264Picture2 != null)
				{
					h264Picture2.Ref = false;
				}
				break;
			}
			case 2:
				(dpb.Find((H264Picture p) => p.Ref && p.LongTerm && p.LongTermPicNum == marking.LongTermPicNum) ?? throw new H264Exception("h264: mmco 2 names a missing long term picture")).Ref = false;
				break;
			case 3:
			{
				H264Picture obj = ShortRefByPicNum(pic.PicNum - (marking.DifferenceOfPicNumsMinus1 + 1)) ?? throw new H264Exception("h264: mmco 3 names a missing short term picture");
				H264Picture h264Picture = dpb.Find((H264Picture p) => p.Ref && p.LongTerm && p.LongTermFrameIdx == marking.LongTermFrameIdx);
				if (h264Picture != null)
				{
					h264Picture.Ref = false;
				}
				obj.LongTerm = true;
				obj.LongTermFrameIdx = marking.LongTermFrameIdx;
				break;
			}
			case 4:
				maxLongTermFrameIdx = marking.MaxLongTermFrameIdxPlus1 - 1;
				foreach (H264Picture item in dpb)
				{
					if (item.Ref && item.LongTerm && item.LongTermFrameIdx > maxLongTermFrameIdx)
					{
						item.Ref = false;
					}
				}
				break;
			case 5:
				MarkAllUnusedForRef();
				maxLongTermFrameIdx = -1;
				pic.MemMgmt5 = true;
				break;
			case 6:
				foreach (H264Picture item2 in dpb)
				{
					if (item2.Ref && item2.LongTerm && item2.LongTermFrameIdx == marking.LongTermFrameIdx)
					{
						item2.Ref = false;
					}
				}
				pic.Ref = true;
				pic.LongTerm = true;
				pic.LongTermFrameIdx = marking.LongTermFrameIdx;
				break;
			}
		}
	}

	private void SlidingWindowPictureMarking()
	{
		int num = dpb.Count((H264Picture p) => p.Ref);
		if (num < maxNumRefFrames)
		{
			return;
		}
		if (num > maxNumRefFrames)
		{
			throw new H264Exception("h264: more reference pictures than max_num_ref_frames");
		}
		H264Picture h264Picture = null;
		foreach (H264Picture item in dpb)
		{
			if (item.Ref && !item.LongTerm && (h264Picture == null || item.FrameNumWrap < h264Picture.FrameNumWrap))
			{
				h264Picture = item;
			}
		}
		if (h264Picture == null)
		{
			throw new H264Exception("h264: sliding window found no short term picture to drop");
		}
		h264Picture.Ref = false;
	}

	private void ReferencePictureMarking(H264Picture pic)
	{
		if (pic.Idr)
		{
			MarkAllUnusedForRef();
			pic.LongTerm = pic.LongTermReferenceFlag;
			pic.LongTermFrameIdx = 0;
			maxLongTermFrameIdx = ((!pic.LongTermReferenceFlag) ? (-1) : 0);
		}
		else if (pic.AdaptiveRefPicMarkingMode)
		{
			HandleMemoryManagementOps(pic);
		}
		else
		{
			SlidingWindowPictureMarking();
		}
	}

	public void FinishPicture(H264Picture pic, List<H264Picture> outputs)
	{
		if (pic.Ref)
		{
			ReferencePictureMarking(pic);
			prevRefFrameNum = ((!pic.MemMgmt5) ? pic.FrameNum : 0);
		}
		if (pic.MemMgmt5)
		{
			OutputAllRemaining(outputs);
			int num = System.Math.Min(pic.TopFieldOrderCnt, pic.BottomFieldOrderCnt);
			pic.TopFieldOrderCnt -= num;
			pic.BottomFieldOrderCnt -= num;
			pic.PicOrderCnt = 0;
			pic.FrameNum = 0;
		}
		dpb.RemoveAll((H264Picture p) => p.Outputted && !p.Ref);
		List<H264Picture> list = dpb.Where((H264Picture p) => !p.Outputted).ToList();
		list.Add(pic);
		list.Sort((H264Picture a, H264Picture b) => a.PicOrderCnt.CompareTo(b.PicOrderCnt));
		int num2 = 0;
		int num3 = list.Count;
		while (num3 > maxNumReorderFrames || (dpb.Count >= maxDpbSize && (!pic.Outputted || pic.Ref) && num3 > 0))
		{
			H264Picture h264Picture = list[num2];
			h264Picture.Outputted = true;
			outputs.Add(h264Picture);
			if (!h264Picture.Ref && h264Picture != pic)
			{
				dpb.Remove(h264Picture);
			}
			num2++;
			num3--;
		}
		if (!pic.Outputted || pic.Ref)
		{
			if (dpb.Count >= maxDpbSize)
			{
				throw new H264Exception("h264: dpb overflow");
			}
			dpb.Add(pic);
		}
	}

	private void OutputAllRemaining(List<H264Picture> outputs)
	{
		int count = outputs.Count;
		foreach (H264Picture item in dpb)
		{
			if (!item.Outputted)
			{
				item.Outputted = true;
				outputs.Add(item);
			}
		}
		outputs.Sort(count, outputs.Count - count, Comparer<H264Picture>.Create((H264Picture a, H264Picture b) => a.PicOrderCnt.CompareTo(b.PicOrderCnt)));
	}

	public void Flush(List<H264Picture> outputs)
	{
		OutputAllRemaining(outputs);
		dpb.Clear();
	}
}
