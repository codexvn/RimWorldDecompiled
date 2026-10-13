using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001403 RID: 5123
	public class JobGiver_AIDefendPoint : JobGiver_AIFightEnemy
	{
		// Token: 0x06007CD5 RID: 31957 RVA: 0x00259690 File Offset: 0x00257890
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.mindState.duty.radius > 0f)
			{
				this.targetAcquireRadius = pawn.mindState.duty.radius;
				this.targetKeepRadius = pawn.mindState.duty.radius * 1.5f;
			}
			return base.TryGiveJob(pawn);
		}

		// Token: 0x06007CD6 RID: 31958 RVA: 0x002596F0 File Offset: 0x002578F0
		protected override bool TryFindShootingPosition(Pawn pawn, out IntVec3 dest, Verb verbToUse = null)
		{
			Thing enemyTarget = pawn.mindState.enemyTarget;
			Verb verb = verbToUse ?? pawn.TryGetAttackVerb(enemyTarget, !pawn.IsColonist, false);
			if (verb == null)
			{
				dest = IntVec3.Invalid;
				return false;
			}
			return CastPositionFinder.TryFindCastPosition(new CastPositionRequest
			{
				caster = pawn,
				target = enemyTarget,
				verb = verb,
				maxRangeFromTarget = 9999f,
				locus = (IntVec3)pawn.mindState.duty.focus,
				maxRangeFromLocus = pawn.mindState.duty.radius,
				wantCoverFromTarget = (verb.EffectiveRange > 7f)
			}, out dest);
		}
	}
}
