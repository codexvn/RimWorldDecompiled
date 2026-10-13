using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200160E RID: 5646
	public class WorkGiver_RepairMech : WorkGiver_Scanner
	{
		// Token: 0x1700146D RID: 5229
		// (get) Token: 0x06008531 RID: 34097 RVA: 0x002735F9 File Offset: 0x002717F9
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Pawn);
			}
		}

		// Token: 0x06008532 RID: 34098 RVA: 0x0026F7EF File Offset: 0x0026D9EF
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.mapPawns.SpawnedPawnsInFaction(pawn.Faction);
		}

		// Token: 0x1700146E RID: 5230
		// (get) Token: 0x06008533 RID: 34099 RVA: 0x000E96BB File Offset: 0x000E78BB
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x06008534 RID: 34100 RVA: 0x00075162 File Offset: 0x00073362
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x06008535 RID: 34101 RVA: 0x0027DB03 File Offset: 0x0027BD03
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !MechanitorUtility.IsMechanitor(pawn);
		}

		// Token: 0x06008536 RID: 34102 RVA: 0x0027DB10 File Offset: 0x0027BD10
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!ModLister.CheckBiotech("Repair mech"))
			{
				return false;
			}
			Pawn pawn2 = (Pawn)t;
			CompMechRepairable compMechRepairable = t.TryGetComp<CompMechRepairable>();
			return compMechRepairable != null && pawn2.RaceProps.IsMechanoid && !pawn2.InAggroMentalState && !pawn2.HostileTo(pawn) && pawn.CanReserve(t, 1, -1, null, forced) && !pawn2.IsBurning() && !pawn2.IsAttacking() && pawn2.needs.energy != null && MechRepairUtility.CanRepair(pawn2) && (forced || compMechRepairable.autoRepair);
		}

		// Token: 0x06008537 RID: 34103 RVA: 0x0027DBAC File Offset: 0x0027BDAC
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.RepairMech, t);
		}
	}
}
