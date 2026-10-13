using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001535 RID: 5429
	public class JobGiver_SocialFighting : ThinkNode_JobGiver
	{
		// Token: 0x06008101 RID: 33025 RVA: 0x0026D838 File Offset: 0x0026BA38
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.RaceProps.Humanlike && pawn.WorkTagIsDisabled(WorkTags.Violent))
			{
				return null;
			}
			Pawn otherPawn = ((MentalState_SocialFighting)pawn.MentalState).otherPawn;
			if (!otherPawn.Spawned || otherPawn.Map != pawn.Map)
			{
				return null;
			}
			Verb verbToUse;
			if (!SocialInteractionUtility.TryGetRandomVerbForSocialFight(pawn, out verbToUse))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.SocialFight, otherPawn);
			job.maxNumMeleeAttacks = 1;
			job.verbToUse = verbToUse;
			return job;
		}
	}
}
