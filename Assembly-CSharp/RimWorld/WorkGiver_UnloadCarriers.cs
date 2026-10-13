using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001629 RID: 5673
	public class WorkGiver_UnloadCarriers : WorkGiver_Scanner
	{
		// Token: 0x17001488 RID: 5256
		// (get) Token: 0x060085C5 RID: 34245 RVA: 0x002735F9 File Offset: 0x002717F9
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Pawn);
			}
		}

		// Token: 0x17001489 RID: 5257
		// (get) Token: 0x060085C6 RID: 34246 RVA: 0x000E96BB File Offset: 0x000E78BB
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x060085C7 RID: 34247 RVA: 0x0027EEB4 File Offset: 0x0027D0B4
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			IReadOnlyList<Pawn> allPawnsSpawned = pawn.Map.mapPawns.AllPawnsSpawned;
			for (int i = 0; i < allPawnsSpawned.Count; i++)
			{
				if (allPawnsSpawned[i].inventory.UnloadEverything)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060085C8 RID: 34248 RVA: 0x0027EEF9 File Offset: 0x0027D0F9
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.mapPawns.SpawnedPawnsWhoShouldHaveInventoryUnloaded;
		}

		// Token: 0x060085C9 RID: 34249 RVA: 0x0027EF0B File Offset: 0x0027D10B
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return UnloadCarriersJobGiverUtility.HasJobOnThing(pawn, t, forced);
		}

		// Token: 0x060085CA RID: 34250 RVA: 0x0027EF15 File Offset: 0x0027D115
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.UnloadInventory, t);
		}
	}
}
