using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001620 RID: 5664
	public class WorkGiver_TendOther_Humanlike : WorkGiver_TendOther
	{
		// Token: 0x06008598 RID: 34200 RVA: 0x0027E95E File Offset: 0x0027CB5E
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.mapPawns.SpawnedHumanlikesWithAnyHediff;
		}

		// Token: 0x06008599 RID: 34201 RVA: 0x0027E970 File Offset: 0x0027CB70
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			return (pawn2 == null || pawn2.RaceProps.Humanlike) && base.HasJobOnThing(pawn, t, forced);
		}
	}
}
