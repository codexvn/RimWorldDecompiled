using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200161A RID: 5658
	public class WorkGiver_TakeToBedToOperate : WorkGiver_TakeToBed
	{
		// Token: 0x1700147C RID: 5244
		// (get) Token: 0x06008577 RID: 34167 RVA: 0x002735F9 File Offset: 0x002717F9
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Pawn);
			}
		}

		// Token: 0x1700147D RID: 5245
		// (get) Token: 0x06008578 RID: 34168 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.OnCell;
			}
		}

		// Token: 0x06008579 RID: 34169 RVA: 0x00075162 File Offset: 0x00073362
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x0600857A RID: 34170 RVA: 0x0027E4C8 File Offset: 0x0027C6C8
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			IReadOnlyList<Pawn> allPawnsSpawned = pawn.Map.mapPawns.AllPawnsSpawned;
			for (int i = 0; i < allPawnsSpawned.Count; i++)
			{
				if (HealthAIUtility.ShouldHaveSurgeryDoneNow(allPawnsSpawned[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600857B RID: 34171 RVA: 0x0027E508 File Offset: 0x0027C708
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.mapPawns.SpawnedPawnsWhoShouldHaveSurgeryDoneNow;
		}

		// Token: 0x0600857C RID: 34172 RVA: 0x0027E51C File Offset: 0x0027C71C
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			if (pawn2 == null || pawn2 == pawn || pawn2.InBed() || !pawn2.RaceProps.IsFlesh || !HealthAIUtility.ShouldHaveSurgeryDoneNow(pawn2) || !pawn.CanReserve(pawn2, 1, -1, null, forced) || (pawn2.InMentalState && pawn2.MentalStateDef.IsAggro))
			{
				return false;
			}
			if (!pawn2.Downed)
			{
				if (pawn2.IsColonist || pawn2.CurJobDef == JobDefOf.LayDown)
				{
					return false;
				}
				if (!pawn2.IsPrisonerOfColony && pawn2.Faction != Faction.OfPlayer)
				{
					return false;
				}
				if (pawn2.guest != null && pawn2.guest.Released)
				{
					return false;
				}
			}
			Building_Bed building_Bed = base.FindBed(pawn, pawn2);
			return building_Bed != null && pawn2.CanReserve(building_Bed, building_Bed.SleepingSlotsCount, -1, null, false);
		}

		// Token: 0x0600857D RID: 34173 RVA: 0x0027E5F0 File Offset: 0x0027C7F0
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			Building_Bed t2 = base.FindBed(pawn, pawn2);
			Job job = JobMaker.MakeJob(JobDefOf.TakeToBedToOperate, pawn2, t2);
			job.count = 1;
			return job;
		}
	}
}
