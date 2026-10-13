using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02002237 RID: 8759
	public class CompAbilityEffect_ForceJob : CompAbilityEffect_WithDest
	{
		// Token: 0x17001D38 RID: 7480
		// (get) Token: 0x0600BF61 RID: 48993 RVA: 0x00377DF1 File Offset: 0x00375FF1
		public new CompProperties_AbilityForceJob Props
		{
			get
			{
				return (CompProperties_AbilityForceJob)this.props;
			}
		}

		// Token: 0x0600BF62 RID: 48994 RVA: 0x00377E00 File Offset: 0x00376000
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			Pawn pawn = target.Thing as Pawn;
			if (pawn != null)
			{
				Job job = JobMaker.MakeJob(this.Props.jobDef, new LocalTargetInfo(base.GetDestination(target).Cell));
				float num = 1f;
				if (this.Props.durationMultiplier != null)
				{
					num = pawn.GetStatValue(this.Props.durationMultiplier, true, -1);
				}
				job.expiryInterval = (this.parent.def.GetStatValueAbstract(StatDefOf.Ability_Duration, this.parent.pawn) * num).SecondsToTicks();
				job.mote = MoteMaker.MakeThoughtBubble(pawn, this.parent.def.iconPath, true);
				RestUtility.WakeUp(pawn, true);
				pawn.jobs.StopAll(false, true);
				pawn.jobs.StartJob(job, JobCondition.InterruptForced, null, false, true, null, null, false, false, null, false, true, false);
			}
		}
	}
}
