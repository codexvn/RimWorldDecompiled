using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020012CA RID: 4810
	public class JobDriver_OperateDeepDrill : JobDriver
	{
		// Token: 0x060075D4 RID: 30164 RVA: 0x001E06E3 File Offset: 0x001DE8E3
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x060075D5 RID: 30165 RVA: 0x0023F52B File Offset: 0x0023D72B
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOnBurningImmobile(TargetIndex.A);
			this.FailOnThingHavingDesignation(TargetIndex.A, DesignationDefOf.Uninstall);
			this.FailOn(() => !this.job.targetA.Thing.TryGetComp<CompDeepDrill>().CanDrillNow());
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell, false);
			Toil work = ToilMaker.MakeToil("MakeNewToils");
			work.tickIntervalAction = delegate(int delta)
			{
				Pawn actor = work.actor;
				((Building)actor.CurJob.targetA.Thing).GetComp<CompDeepDrill>().DrillWorkDone(actor, delta);
				Pawn_SkillTracker skills = actor.skills;
				if (skills == null)
				{
					return;
				}
				skills.Learn(SkillDefOf.Mining, 0.065f * (float)delta, false, false);
			};
			work.defaultCompleteMode = ToilCompleteMode.Never;
			work.WithEffect(EffecterDefOf.Drill, TargetIndex.A, null);
			work.FailOnCannotTouch(TargetIndex.A, PathEndMode.InteractionCell);
			work.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			work.activeSkill = (() => SkillDefOf.Mining);
			yield return work;
			yield break;
		}
	}
}
