using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020015D5 RID: 5589
	public class WorkGiver_HaulMechToCharger : WorkGiver_Scanner
	{
		// Token: 0x17001438 RID: 5176
		// (get) Token: 0x06008405 RID: 33797 RVA: 0x002735F9 File Offset: 0x002717F9
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Pawn);
			}
		}

		// Token: 0x06008406 RID: 33798 RVA: 0x0026F7EF File Offset: 0x0026D9EF
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.mapPawns.SpawnedPawnsInFaction(pawn.Faction);
		}

		// Token: 0x06008407 RID: 33799 RVA: 0x002799A4 File Offset: 0x00277BA4
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!ModLister.CheckBiotech("Haul mech to recharger"))
			{
				return false;
			}
			Pawn pawn2 = t as Pawn;
			if (pawn2 == null || !pawn2.Spawned || !pawn2.RaceProps.IsMechanoid || !pawn2.IsColonyMech)
			{
				return false;
			}
			if (pawn2.needs.energy != null)
			{
				if (!pawn2.Downed && !pawn2.needs.energy.IsLowEnergySelfShutdown)
				{
					return false;
				}
				MechanitorControlGroup mechControlGroup = pawn2.GetMechControlGroup();
				if (mechControlGroup != null && mechControlGroup.WorkMode == MechWorkModeDefOf.SelfShutdown)
				{
					return false;
				}
				if (pawn2.needs.energy.CurLevel >= JobGiver_GetEnergy.GetMaxRechargeLimit(pawn2))
				{
					return false;
				}
			}
			return pawn2.CurJobDef != JobDefOf.MechCharge && pawn.CanReserve(t, 1, -1, null, forced) && JobGiver_GetEnergy_Charger.GetClosestCharger(pawn2, pawn, forced) != null;
		}

		// Token: 0x06008408 RID: 33800 RVA: 0x00279A74 File Offset: 0x00277C74
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = (Pawn)t;
			Building_MechCharger closestCharger = JobGiver_GetEnergy_Charger.GetClosestCharger(pawn2, pawn, forced);
			Job job = JobMaker.MakeJob(JobDefOf.HaulMechToCharger, pawn2, closestCharger, closestCharger.InteractionCell);
			job.count = 1;
			return job;
		}
	}
}
