using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001618 RID: 5656
	public class WorkGiver_TakeBioferriteOutOfHarvester : WorkGiver_Scanner
	{
		// Token: 0x1700147A RID: 5242
		// (get) Token: 0x0600856F RID: 34159 RVA: 0x0027E44E File Offset: 0x0027C64E
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForDef(ThingDefOf.BioferriteHarvester);
			}
		}

		// Token: 0x06008570 RID: 34160 RVA: 0x0027D860 File Offset: 0x0027BA60
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !ModsConfig.AnomalyActive;
		}

		// Token: 0x1700147B RID: 5243
		// (get) Token: 0x06008571 RID: 34161 RVA: 0x000E96BB File Offset: 0x000E78BB
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x06008572 RID: 34162 RVA: 0x0027E45C File Offset: 0x0027C65C
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Building_BioferriteHarvester building_BioferriteHarvester = t as Building_BioferriteHarvester;
			return building_BioferriteHarvester != null && building_BioferriteHarvester.unloadingEnabled && building_BioferriteHarvester.ReadyForHauling && !t.IsBurning() && pawn.CanReserve(t, 1, -1, null, forced);
		}

		// Token: 0x06008573 RID: 34163 RVA: 0x0027E4A4 File Offset: 0x0027C6A4
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.TakeBioferriteOutOfHarvester, t);
		}
	}
}
