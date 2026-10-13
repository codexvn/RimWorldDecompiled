using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200162A RID: 5674
	public class WorkGiver_VisitSickPawn : WorkGiver_Scanner
	{
		// Token: 0x1700148A RID: 5258
		// (get) Token: 0x060085CC RID: 34252 RVA: 0x000E9647 File Offset: 0x000E7847
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.InteractionCell;
			}
		}

		// Token: 0x1700148B RID: 5259
		// (get) Token: 0x060085CD RID: 34253 RVA: 0x002735F9 File Offset: 0x002717F9
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Pawn);
			}
		}

		// Token: 0x060085CE RID: 34254 RVA: 0x0027E62A File Offset: 0x0027C82A
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.mapPawns.SpawnedPawnsInFaction(Faction.OfPlayer);
		}

		// Token: 0x060085CF RID: 34255 RVA: 0x0027EF28 File Offset: 0x0027D128
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			if (!SocialInteractionUtility.CanInitiateInteraction(pawn, null))
			{
				return true;
			}
			List<Pawn> list = pawn.Map.mapPawns.SpawnedPawnsInFaction(Faction.OfPlayer);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].InBed())
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060085D0 RID: 34256 RVA: 0x0027EF78 File Offset: 0x0027D178
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			return pawn2 != null && SickPawnVisitUtility.CanVisit(pawn, pawn2, JoyCategory.VeryLow);
		}

		// Token: 0x060085D1 RID: 34257 RVA: 0x0027EF9C File Offset: 0x0027D19C
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = (Pawn)t;
			Thing thing = SickPawnVisitUtility.FindChair(pawn, pawn2);
			if (thing != null && !pawn.CanReserveAndReach(thing, PathEndMode.OnCell, Danger.Some, 1, -1, null, false))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.VisitSickPawn, pawn2, thing);
			job.ignoreJoyTimeAssignment = true;
			return job;
		}
	}
}
