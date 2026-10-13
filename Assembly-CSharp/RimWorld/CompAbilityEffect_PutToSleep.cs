using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200225A RID: 8794
	public class CompAbilityEffect_PutToSleep : CompAbilityEffect
	{
		// Token: 0x0600BFD1 RID: 49105 RVA: 0x00379894 File Offset: 0x00377A94
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			Pawn pawn = target.Pawn;
			if (pawn != null)
			{
				pawn.needs.rest.CurLevel = 0f;
				Job job = JobMaker.MakeJob(JobDefOf.LayDown, pawn.Position);
				job.forceSleep = true;
				pawn.jobs.StartJob(job, JobCondition.InterruptForced, null, false, true, null, null, false, false, null, false, true, false);
			}
		}
	}
}
