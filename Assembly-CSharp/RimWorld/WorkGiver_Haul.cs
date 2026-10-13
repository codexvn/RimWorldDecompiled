using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020015D2 RID: 5586
	public abstract class WorkGiver_Haul : WorkGiver_Scanner
	{
		// Token: 0x060083FB RID: 33787 RVA: 0x00075162 File Offset: 0x00073362
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x060083FC RID: 33788 RVA: 0x002798B0 File Offset: 0x00277AB0
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.listerHaulables.ThingsPotentiallyNeedingHauling();
		}

		// Token: 0x060083FD RID: 33789 RVA: 0x002798C2 File Offset: 0x00277AC2
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return pawn.Map.listerHaulables.ThingsPotentiallyNeedingHauling().Count == 0;
		}

		// Token: 0x060083FE RID: 33790 RVA: 0x002798DC File Offset: 0x00277ADC
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!HaulAIUtility.PawnCanAutomaticallyHaulFast(pawn, t, forced))
			{
				return null;
			}
			return HaulAIUtility.HaulToStorageJob(pawn, t, forced);
		}
	}
}
