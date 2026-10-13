using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02002F3F RID: 12095
	public class CompTargetEffect_Resurrect : CompTargetEffect
	{
		// Token: 0x17002C15 RID: 11285
		// (get) Token: 0x060110EF RID: 69871 RVA: 0x004E6CC6 File Offset: 0x004E4EC6
		public CompProperties_TargetEffectResurrect Props
		{
			get
			{
				return (CompProperties_TargetEffectResurrect)this.props;
			}
		}

		// Token: 0x060110F0 RID: 69872 RVA: 0x004E6CD4 File Offset: 0x004E4ED4
		public override void DoEffectOn(Pawn user, Thing target)
		{
			if (!user.IsColonistPlayerControlled)
			{
				return;
			}
			Job job = JobMaker.MakeJob(JobDefOf.Resurrect, target, this.parent);
			job.count = 1;
			job.playerForced = true;
			user.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
		}
	}
}
