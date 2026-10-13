using System;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020015C9 RID: 5577
	public class WorkGiver_Fish : WorkGiver_Scanner
	{
		// Token: 0x060083CD RID: 33741 RVA: 0x00278740 File Offset: 0x00276940
		public override Job NonScanJob(Pawn pawn)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return null;
			}
			if (pawn.Ideo != null && !new HistoryEvent(HistoryEventDefOf.SlaughteredFish, pawn.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo_Job())
			{
				return null;
			}
			if (pawn.CanReserve(pawn.Position, 1, -1, ReservationLayerDefOf.Floor, false) && !pawn.Position.GetTerrain(pawn.Map).IsWater)
			{
				for (int i = 0; i < 4; i++)
				{
					IntVec3 intVec = pawn.Position + GenAdj.CardinalDirections[(i + pawn.thingIDNumber) % 4];
					if (intVec.InBounds(pawn.Map))
					{
						Zone_Fishing zone_Fishing = intVec.GetZone(pawn.Map) as Zone_Fishing;
						if (zone_Fishing != null && zone_Fishing.ShouldFishNow && zone_Fishing.IsFishable(intVec) && !intVec.IsForbidden(pawn) && pawn.CanReserve(intVec, 1, -1, ReservationLayerDefOf.Floor, false))
						{
							return JobMaker.MakeJob(JobDefOf.Fish, intVec, pawn.Position);
						}
					}
				}
			}
			Zone_Fishing zone_Fishing2 = null;
			float num = float.MaxValue;
			foreach (Zone zone in pawn.Map.zoneManager.AllZones)
			{
				Zone_Fishing zone_Fishing3 = zone as Zone_Fishing;
				if (zone_Fishing3 != null && zone_Fishing3.ShouldFishNow && zone_Fishing3.HasAnyFishableCells && (zone_Fishing2 == null || (float)pawn.Position.DistanceToSquared(zone_Fishing3.Cells[0]) < num))
				{
					zone_Fishing2 = zone_Fishing3;
					num = (float)pawn.Position.DistanceToSquared(zone_Fishing3.Cells[0]);
				}
			}
			if (zone_Fishing2 != null)
			{
				int j = 0;
				bool flag = false;
				while (j < 60)
				{
					IntVec3 randomFishableCell = zone_Fishing2.RandomFishableCell;
					if (randomFishableCell.IsValid && pawn.CanReserveAndReach(randomFishableCell, PathEndMode.Touch, Danger.Some, 1, -1, ReservationLayerDefOf.Floor, false))
					{
						IntVec3 c = WorkGiver_Fish.BestStandSpotFor(pawn, randomFishableCell, true);
						if (c.IsValid && (flag || !c.GetTerrain(pawn.Map).IsWater))
						{
							return JobMaker.MakeJob(JobDefOf.Fish, randomFishableCell, c);
						}
					}
					j++;
					if (j >= 60 && !flag)
					{
						flag = true;
						j = 0;
					}
				}
			}
			return null;
		}

		// Token: 0x060083CE RID: 33742 RVA: 0x002789A4 File Offset: 0x00276BA4
		public override bool HasJobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			return (pawn.Ideo == null || new HistoryEvent(HistoryEventDefOf.SlaughteredFish, pawn.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo_Job()) && base.HasJobOnCell(pawn, c, forced);
		}

		// Token: 0x060083CF RID: 33743 RVA: 0x002789D8 File Offset: 0x00276BD8
		public static IntVec3 BestStandSpotFor(Pawn pawn, IntVec3 fishSpot, bool avoidStandingInWater = true)
		{
			IntVec3 result = IntVec3.Invalid;
			float num = float.MinValue;
			for (int i = 0; i < 4; i++)
			{
				IntVec3 intVec = fishSpot + GenAdj.CardinalDirections[i];
				if (intVec.InBounds(pawn.Map) && intVec.Standable(pawn.Map) && !intVec.IsForbidden(pawn) && pawn.CanReserveAndReach(intVec, PathEndMode.Touch, Danger.Some, 1, -1, ReservationLayerDefOf.Floor, false))
				{
					if (!avoidStandingInWater)
					{
						return intVec;
					}
					float num2 = intVec.GetTerrain(pawn.Map).avoidWander ? 0.5f : 1f;
					if (num2 > num)
					{
						num = num2;
						result = intVec;
					}
					else if (Mathf.Approximately(num2, num) && Rand.Bool)
					{
						result = intVec;
					}
				}
			}
			return result;
		}

		// Token: 0x040056F6 RID: 22262
		public const int TriesToFindSpotThatIsntInWater = 60;
	}
}
