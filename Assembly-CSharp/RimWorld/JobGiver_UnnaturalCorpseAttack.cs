using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200146F RID: 5231
	public class JobGiver_UnnaturalCorpseAttack : ThinkNode_JobGiver
	{
		// Token: 0x06007E30 RID: 32304 RVA: 0x0025FEE4 File Offset: 0x0025E0E4
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.Downed)
			{
				return null;
			}
			UnnaturalCorpseTracker unnaturalCorpseTracker;
			if (!Find.Anomaly.TryGetUnnaturalCorpseTrackerForAwoken(pawn, out unnaturalCorpseTracker))
			{
				return null;
			}
			Pawn haunted = unnaturalCorpseTracker.Haunted;
			if (haunted.DestroyedOrNull())
			{
				return null;
			}
			if (!haunted.Spawned)
			{
				if (haunted.SpawnedOrAnyParentSpawned && pawn.CanReach(haunted.SpawnedParentOrMe, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					Job job = JobMaker.MakeJob(JobDefOf.AttackMelee, haunted.SpawnedParentOrMe);
					job.expiryInterval = 30;
					return job;
				}
				return null;
			}
			else
			{
				if (pawn.CanReach(haunted, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					Job job2 = JobMaker.MakeJob(JobDefOf.UnnaturalCorpseAttack, haunted);
					job2.expiryInterval = 120;
					job2.checkOverrideOnExpire = true;
					return job2;
				}
				return null;
			}
		}
	}
}
