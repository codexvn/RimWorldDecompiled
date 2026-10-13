using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001621 RID: 5665
	public class WorkGiver_TendOther_Animal : WorkGiver_TendOther
	{
		// Token: 0x0600859B RID: 34203 RVA: 0x0027E9A7 File Offset: 0x0027CBA7
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.mapPawns.SpawnedAnimalsWithAnyHediff;
		}

		// Token: 0x0600859C RID: 34204 RVA: 0x0027E9BC File Offset: 0x0027CBBC
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			return (pawn2 == null || pawn2.IsAnimal) && base.HasJobOnThing(pawn, t, forced);
		}
	}
}
