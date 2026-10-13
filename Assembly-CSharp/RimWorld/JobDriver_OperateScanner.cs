using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020012CE RID: 4814
	public class JobDriver_OperateScanner : JobDriver
	{
		// Token: 0x060075E5 RID: 30181 RVA: 0x001E06E3 File Offset: 0x001DE8E3
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x060075E6 RID: 30182 RVA: 0x0023F7B7 File Offset: 0x0023D9B7
		protected override IEnumerable<Toil> MakeNewToils()
		{
			CompScanner scannerComp = this.job.targetA.Thing.TryGetComp<CompScanner>();
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOnBurningImmobile(TargetIndex.A);
			this.FailOn(() => !scannerComp.CanUseNow);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell, false);
			Toil work = ToilMaker.MakeToil("MakeNewToils");
			work.tickAction = delegate()
			{
				Pawn actor = work.actor;
				scannerComp.Used(actor);
				actor.skills.Learn(SkillDefOf.Intellectual, 0.035f, false, false);
				actor.GainComfortFromCellIfPossible(1, true);
			};
			work.PlaySustainerOrSound(scannerComp.Props.soundWorking, 1f);
			work.AddFailCondition(() => !scannerComp.CanUseNow);
			work.defaultCompleteMode = ToilCompleteMode.Never;
			work.FailOnCannotTouch(TargetIndex.A, PathEndMode.InteractionCell);
			work.activeSkill = (() => SkillDefOf.Intellectual);
			yield return work;
			yield break;
		}
	}
}
