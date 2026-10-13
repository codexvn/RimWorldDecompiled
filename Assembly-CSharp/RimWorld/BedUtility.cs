using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020029DE RID: 10718
	public static class BedUtility
	{
		// Token: 0x0600EC66 RID: 60518 RVA: 0x0044A72F File Offset: 0x0044892F
		public static int GetSleepingSlotsCount(IntVec2 bedSize)
		{
			return bedSize.x;
		}

		// Token: 0x0600EC67 RID: 60519 RVA: 0x0044A738 File Offset: 0x00448938
		private static IntVec3 GetSlotPos(int index, IntVec3 bedCenter, Rot4 bedRot, IntVec2 bedSize, bool head)
		{
			int sleepingSlotsCount = BedUtility.GetSleepingSlotsCount(bedSize);
			if (index < 0 || index >= sleepingSlotsCount)
			{
				Log.Error(string.Concat(new string[]
				{
					"Tried to get sleeping slot pos with index ",
					index.ToString(),
					", but there are only ",
					sleepingSlotsCount.ToString(),
					" sleeping slots available."
				}));
				return bedCenter;
			}
			CellRect cellRect = GenAdj.OccupiedRect(bedCenter, bedRot, bedSize);
			switch (bedRot.AsInt)
			{
			case 0:
				return new IntVec3(cellRect.maxX - index, bedCenter.y, head ? cellRect.minZ : cellRect.maxZ);
			case 1:
				return new IntVec3(head ? cellRect.minX : cellRect.maxX, bedCenter.y, cellRect.minZ + index);
			case 2:
				return new IntVec3(cellRect.minX + index, bedCenter.y, head ? cellRect.maxZ : cellRect.minZ);
			case 3:
				return new IntVec3(head ? cellRect.maxX : cellRect.minX, bedCenter.y, cellRect.maxZ - index);
			default:
				throw new NotImplementedException();
			}
		}

		// Token: 0x0600EC68 RID: 60520 RVA: 0x0044A85B File Offset: 0x00448A5B
		public static IntVec3 GetFeetSlotPos(int index, IntVec3 bedCenter, Rot4 bedRot, IntVec2 bedSize)
		{
			return BedUtility.GetSlotPos(index, bedCenter, bedRot, bedSize, false);
		}

		// Token: 0x0600EC69 RID: 60521 RVA: 0x0044A867 File Offset: 0x00448A67
		public static IntVec3 GetSleepingSlotPos(int index, IntVec3 bedCenter, Rot4 bedRot, IntVec2 bedSize)
		{
			return BedUtility.GetSlotPos(index, bedCenter, bedRot, bedSize, true);
		}

		// Token: 0x0600EC6A RID: 60522 RVA: 0x0044A874 File Offset: 0x00448A74
		public static int GetSlotFromPosition(IntVec3 pos, IntVec3 bedCenter, Rot4 bedRot, IntVec2 bedSize)
		{
			CellRect cellRect = GenAdj.OccupiedRect(bedCenter, bedRot, bedSize);
			if (!cellRect.Contains(pos))
			{
				Log.Error(string.Format("{0} was not inside bed at {1} with size {2} and rotation {3}", new object[]
				{
					pos,
					bedCenter,
					bedSize,
					bedRot
				}));
				return 0;
			}
			int num;
			switch (bedRot.AsInt)
			{
			case 0:
				num = cellRect.maxX - pos.x;
				break;
			case 1:
				num = pos.z - cellRect.minZ;
				break;
			case 2:
				num = pos.x - cellRect.minX;
				break;
			case 3:
				num = cellRect.maxZ - pos.z;
				break;
			default:
				throw new NotImplementedException();
			}
			if (num < 0 || num >= BedUtility.GetSleepingSlotsCount(bedSize))
			{
				Log.Error(string.Format("{0} was outside the valid slot range for this bed: [0, {1}).", num, BedUtility.GetSleepingSlotsCount(bedSize)));
				return 0;
			}
			return num;
		}

		// Token: 0x0600EC6B RID: 60523 RVA: 0x0044A968 File Offset: 0x00448B68
		public static bool WillingToShareBed(Pawn pawn1, Pawn pawn2)
		{
			if (pawn1 == pawn2)
			{
				return true;
			}
			if (!IdeoUtility.DoerWillingToDo(HistoryEventDefOf.SharedBed, pawn1) || !IdeoUtility.DoerWillingToDo(HistoryEventDefOf.SharedBed, pawn2))
			{
				return false;
			}
			if (pawn1.relations.GetDirectRelation(PawnRelationDefOf.Spouse, pawn2) != null)
			{
				if (!IdeoUtility.DoerWillingToDo(HistoryEventDefOf.SharedBed_Spouse, pawn1) || !IdeoUtility.DoerWillingToDo(HistoryEventDefOf.SharedBed_Spouse, pawn2))
				{
					return false;
				}
			}
			else if (!IdeoUtility.DoerWillingToDo(HistoryEventDefOf.SharedBed_NonSpouse, pawn1) || !IdeoUtility.DoerWillingToDo(HistoryEventDefOf.SharedBed_NonSpouse, pawn2))
			{
				return false;
			}
			return true;
		}
	}
}
