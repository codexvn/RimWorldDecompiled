using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200141E RID: 5150
	public class JobGiver_AISkipToJobTarget : ThinkNode_JobGiver
	{
		// Token: 0x06007D46 RID: 32070 RVA: 0x0025BA64 File Offset: 0x00259C64
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
			if (!this.CanSkipToTarget(pawn, target))
			{
				return null;
			}
			IntVec3 cell = target.Cell;
			if (target.HasThing && !RCellFinder.TryFindGoodAdjacentSpotToTouch(pawn, target.Thing, out cell))
			{
				return null;
			}
			if (ability.verb.ValidateTarget(target, false))
			{
				Job job = ability.GetJob(pawn, cell);
				pawn.jobs.StartJob(job, JobCondition.InterruptForced, null, true, true, null, null, false, false, null, false, true, false);
			}
			return null;
		}

		// Token: 0x06007D47 RID: 32071 RVA: 0x0025BB49 File Offset: 0x00259D49
		protected virtual bool CanSkipToTarget(Pawn pawn, LocalTargetInfo target)
		{
			return target.Cell.Fogged(pawn.Map);
		}

		// Token: 0x04005447 RID: 21575
		public AbilityDef ability;

		// Token: 0x04005448 RID: 21576
		public TargetIndex targetIndex = TargetIndex.A;
	}
}
