using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001603 RID: 5635
	public class WorkGiver_PruneGauranlenTree : WorkGiver_Scanner
	{
		// Token: 0x17001463 RID: 5219
		// (get) Token: 0x060084FD RID: 34045 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool Prioritized
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060084FE RID: 34046 RVA: 0x0027D278 File Offset: 0x0027B478
		public override float GetPriority(Pawn pawn, TargetInfo t)
		{
			if (!t.HasThing)
			{
				return 0f;
			}
			CompTreeConnection compTreeConnection = t.Thing.TryGetComp<CompTreeConnection>();
			if (compTreeConnection == null)
			{
				return 0f;
			}
			return compTreeConnection.DesiredConnectionStrength - compTreeConnection.ConnectionStrength;
		}

		// Token: 0x060084FF RID: 34047 RVA: 0x00275146 File Offset: 0x00273346
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.listerThings.ThingsMatching(ThingRequest.ForGroup(ThingRequestGroup.DryadSpawner));
		}

		// Token: 0x06008500 RID: 34048 RVA: 0x0027D2B8 File Offset: 0x0027B4B8
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!ModsConfig.IdeologyActive)
			{
				return false;
			}
			CompTreeConnection compTreeConnection = t.TryGetComp<CompTreeConnection>();
			return compTreeConnection != null && compTreeConnection.ConnectedPawn == pawn && compTreeConnection.ShouldBePrunedNow(forced) && pawn.CanReserve(t, 1, -1, null, forced);
		}

		// Token: 0x06008501 RID: 34049 RVA: 0x0027D305 File Offset: 0x0027B505
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Job job = JobMaker.MakeJob(JobDefOf.PruneGauranlenTree, t);
			job.playerForced = forced;
			return job;
		}

		// Token: 0x04005750 RID: 22352
		public const float MaxConnectionStrengthForAutoPruning = 0.99f;
	}
}
