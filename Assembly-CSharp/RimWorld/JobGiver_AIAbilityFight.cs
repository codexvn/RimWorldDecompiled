using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001478 RID: 5240
	public class JobGiver_AIAbilityFight : JobGiver_AIFightEnemy
	{
		// Token: 0x1700139D RID: 5021
		// (get) Token: 0x06007E49 RID: 32329 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool OnlyUseAbilityVerbs
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700139E RID: 5022
		// (get) Token: 0x06007E4A RID: 32330 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool OnlyUseRangedSearch
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06007E4B RID: 32331 RVA: 0x00260509 File Offset: 0x0025E709
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_AIAbilityFight jobGiver_AIAbilityFight = (JobGiver_AIAbilityFight)base.DeepCopy(resolve);
			jobGiver_AIAbilityFight.ability = this.ability;
			jobGiver_AIAbilityFight.skipIfCantTargetNow = this.skipIfCantTargetNow;
			return jobGiver_AIAbilityFight;
		}

		// Token: 0x06007E4C RID: 32332 RVA: 0x00260530 File Offset: 0x0025E730
		protected override bool TryFindShootingPosition(Pawn pawn, out IntVec3 dest, Verb verbToUse = null)
		{
			dest = IntVec3.Invalid;
			Thing enemyTarget = pawn.mindState.enemyTarget;
			Ability ability = pawn.abilities.GetAbility(this.ability, false);
			return CastPositionFinder.TryFindCastPosition(new CastPositionRequest
			{
				caster = pawn,
				target = enemyTarget,
				verb = ability.verb,
				maxRangeFromTarget = ability.verb.EffectiveRange,
				wantCoverFromTarget = false,
				preferredCastPosition = new IntVec3?(pawn.Position)
			}, out dest);
		}

		// Token: 0x06007E4D RID: 32333 RVA: 0x002605BF File Offset: 0x0025E7BF
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.abilities.GetAbility(this.ability, false).OnCooldown && this.skipIfCantTargetNow)
			{
				return null;
			}
			return base.TryGiveJob(pawn);
		}

		// Token: 0x06007E4E RID: 32334 RVA: 0x002605EB File Offset: 0x0025E7EB
		protected override bool ShouldLoseTarget(Pawn pawn)
		{
			return base.ShouldLoseTarget(pawn) || !this.CanTarget(pawn, pawn.mindState.enemyTarget);
		}

		// Token: 0x06007E4F RID: 32335 RVA: 0x0026060D File Offset: 0x0025E80D
		protected override bool ExtraTargetValidator(Pawn pawn, Thing target)
		{
			return base.ExtraTargetValidator(pawn, target) && this.CanTarget(pawn, target);
		}

		// Token: 0x06007E50 RID: 32336 RVA: 0x00260624 File Offset: 0x0025E824
		private bool CanTarget(Pawn pawn, Thing target)
		{
			if (!this.ability.verbProperties.targetParams.CanTarget(target, null))
			{
				return false;
			}
			Ability ability = pawn.abilities.GetAbility(this.ability, false);
			return ability.CanApplyOn(target) && (!this.skipIfCantTargetNow || ability.AICanTargetNow(target));
		}

		// Token: 0x040054BD RID: 21693
		private AbilityDef ability;

		// Token: 0x040054BE RID: 21694
		private bool skipIfCantTargetNow = true;
	}
}
