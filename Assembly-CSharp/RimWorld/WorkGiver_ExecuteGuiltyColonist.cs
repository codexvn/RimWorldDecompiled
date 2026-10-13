using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001583 RID: 5507
	public class WorkGiver_ExecuteGuiltyColonist : WorkGiver_Scanner
	{
		// Token: 0x170013FD RID: 5117
		// (get) Token: 0x0600828F RID: 33423 RVA: 0x000E96BB File Offset: 0x000E78BB
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x170013FE RID: 5118
		// (get) Token: 0x06008290 RID: 33424 RVA: 0x002735F9 File Offset: 0x002717F9
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Pawn);
			}
		}

		// Token: 0x06008291 RID: 33425 RVA: 0x00273602 File Offset: 0x00271802
		public static void ResetStaticData()
		{
			WorkGiver_ExecuteGuiltyColonist.IncapableOfViolenceLowerTrans = "IncapableOfViolenceLower".Translate();
		}

		// Token: 0x06008292 RID: 33426 RVA: 0x00273618 File Offset: 0x00271818
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.mapPawns.FreeColonistsSpawned;
		}

		// Token: 0x06008293 RID: 33427 RVA: 0x0027362A File Offset: 0x0027182A
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return pawn.Map.mapPawns.FreeColonistsSpawnedCount == 0;
		}

		// Token: 0x06008294 RID: 33428 RVA: 0x00273640 File Offset: 0x00271840
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = (Pawn)t;
			if (((pawn2 != null) ? pawn2.guilt : null) == null || !pawn2.guilt.IsGuilty || !pawn2.guilt.awaitingExecution || !pawn2.IsColonist || !pawn2.Spawned || pawn2.InAggroMentalState || pawn2.IsForbidden(pawn) || pawn2.IsFormingCaravan() || !pawn.CanReserveAndReach(pawn2, PathEndMode.Touch, pawn.NormalMaxDanger(), 1, -1, null, forced))
			{
				return null;
			}
			if (pawn.WorkTagIsDisabled(WorkTags.Violent))
			{
				JobFailReason.Is(WorkGiver_ExecuteGuiltyColonist.IncapableOfViolenceLowerTrans, null);
				return null;
			}
			if (!new HistoryEvent(HistoryEventDefOf.ExecutedColonist, pawn.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo_Job())
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.GuiltyColonistExecution, t);
		}

		// Token: 0x040056A0 RID: 22176
		private static string IncapableOfViolenceLowerTrans;
	}
}
