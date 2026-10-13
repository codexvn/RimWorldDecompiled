using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02002F35 RID: 12085
	public class CompTargetEffect_InstallImplantInOtherPawn : CompTargetEffect
	{
		// Token: 0x17002C12 RID: 11282
		// (get) Token: 0x060110D9 RID: 69849 RVA: 0x004E69B6 File Offset: 0x004E4BB6
		public CompProperties_TargetEffectInstallImplantInOtherPawn Props
		{
			get
			{
				return (CompProperties_TargetEffectInstallImplantInOtherPawn)this.props;
			}
		}

		// Token: 0x060110DA RID: 69850 RVA: 0x004E69C4 File Offset: 0x004E4BC4
		public override void DoEffectOn(Pawn user, Thing target)
		{
			if (!user.IsColonistPlayerControlled)
			{
				return;
			}
			Job job = JobMaker.MakeJob(JobDefOf.InstallImplant, target, this.parent);
			job.count = 1;
			job.playerForced = true;
			user.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
		}
	}
}
