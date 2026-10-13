using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200141C RID: 5148
	public class JobGiver_AIJumpToJobTarget : ThinkNode_JobGiver
	{
		// Token: 0x06007D41 RID: 32065 RVA: 0x0025B8A8 File Offset: 0x00259AA8
		protected override Job TryGiveJob(Pawn pawn)
		{
			Pawn_AbilityTracker abilities = pawn.abilities;
			Ability ability = (abilities != null) ? abilities.GetAbility(this.ability, false) : null;
			if (ability == null || !ability.CanCast)
			{
				return null;
			}
			Job curJob = pawn.CurJob;
			if (curJob == null || curJob.def == this.ability.jobDef)
			{
				return null;
			}
			LocalTargetInfo target = curJob.GetTarget(this.targetIndex);
			if (!this.CanJumpToTarget(pawn, target))
			{
				return null;
			}
			IntVec3 cell = target.Cell;
			float num = pawn.Position.DistanceTo(cell);
			VerbProperties verbProps = ability.verb.verbProps;
			if (num < verbProps.minRange || num > ability.verb.EffectiveRange || !GenSight.LineOfSight(pawn.Position, cell, pawn.Map))
			{
				return null;
			}
			if (target.HasThing && !RCellFinder.TryFindGoodAdjacentSpotToTouch(pawn, target.Thing, out cell))
			{
				return null;
			}
			if (ability.verb.ValidateTarget(target, false))
			{
				Job job = ability.GetJob(cell, cell);
				pawn.jobs.StartJob(job, JobCondition.InterruptForced, null, true, true, null, null, false, false, null, false, true, false);
				FleckMaker.Static(cell, pawn.Map, FleckDefOf.FeedbackGoto, 1f);
			}
			return null;
		}

		// Token: 0x06007D42 RID: 32066 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool CanJumpToTarget(Pawn pawn, LocalTargetInfo target)
		{
			return true;
		}

		// Token: 0x04005445 RID: 21573
		public AbilityDef ability;

		// Token: 0x04005446 RID: 21574
		public TargetIndex targetIndex = TargetIndex.A;
	}
}
