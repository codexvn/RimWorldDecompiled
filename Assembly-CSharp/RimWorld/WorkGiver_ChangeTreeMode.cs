using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020015A2 RID: 5538
	public class WorkGiver_ChangeTreeMode : WorkGiver_Scanner
	{
		// Token: 0x060082F4 RID: 33524 RVA: 0x00275146 File Offset: 0x00273346
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.listerThings.ThingsMatching(ThingRequest.ForGroup(ThingRequestGroup.DryadSpawner));
		}

		// Token: 0x060082F5 RID: 33525 RVA: 0x00275160 File Offset: 0x00273360
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!ModsConfig.IdeologyActive)
			{
				return false;
			}
			CompTreeConnection compTreeConnection = t.TryGetComp<CompTreeConnection>();
			return compTreeConnection != null && compTreeConnection.ConnectedPawn == pawn && compTreeConnection.Mode != compTreeConnection.desiredMode;
		}

		// Token: 0x060082F6 RID: 33526 RVA: 0x0027519A File Offset: 0x0027339A
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Job job = JobMaker.MakeJob(JobDefOf.ChangeTreeMode, t);
			job.playerForced = forced;
			return job;
		}
	}
}
