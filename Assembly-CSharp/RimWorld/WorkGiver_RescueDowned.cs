using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200160F RID: 5647
	public class WorkGiver_RescueDowned : WorkGiver_TakeToBed
	{
		// Token: 0x1700146F RID: 5231
		// (get) Token: 0x06008539 RID: 34105 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.OnCell;
			}
		}

		// Token: 0x0600853A RID: 34106 RVA: 0x00075162 File Offset: 0x00073362
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x17001470 RID: 5232
		// (get) Token: 0x0600853B RID: 34107 RVA: 0x002735F9 File Offset: 0x002717F9
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Pawn);
			}
		}

		// Token: 0x0600853C RID: 34108 RVA: 0x0027DBBE File Offset: 0x0027BDBE
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.mapPawns.SpawnedDownedPawns;
		}

		// Token: 0x0600853D RID: 34109 RVA: 0x0027DBD0 File Offset: 0x0027BDD0
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			List<Pawn> list = pawn.Map.mapPawns.SpawnedPawnsInFaction(pawn.Faction);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].Downed && !list[i].InBed())
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600853E RID: 34110 RVA: 0x0027DC24 File Offset: 0x0027BE24
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			if (pawn2 == null || !HealthAIUtility.CanRescueNow(pawn, pawn2, forced))
			{
				return false;
			}
			Thing thing = null;
			ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
			if (ChildcareUtility.CanSuckle(pawn2, out breastfeedFailReason))
			{
				if (!HealthAIUtility.ShouldSeekMedicalRest(pawn2))
				{
					return false;
				}
				Building_Bed building_Bed = ChildcareUtility.SafePlaceForBaby(pawn2, pawn, false).Thing as Building_Bed;
				if (building_Bed != null)
				{
					thing = building_Bed;
				}
			}
			else
			{
				thing = base.FindBed(pawn, pawn2);
			}
			return thing != null && pawn2.CanReserve(thing, 1, -1, null, false);
		}

		// Token: 0x0600853F RID: 34111 RVA: 0x0027DC9C File Offset: 0x0027BE9C
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			Thing t2 = base.FindBed(pawn, pawn2);
			Job job = JobMaker.MakeJob(JobDefOf.Rescue, pawn2, t2);
			job.count = 1;
			return job;
		}

		// Token: 0x0400575C RID: 22364
		private const float MinDistFromEnemy = 40f;
	}
}
