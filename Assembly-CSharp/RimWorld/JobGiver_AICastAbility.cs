using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020013F9 RID: 5113
	public abstract class JobGiver_AICastAbility : ThinkNode_JobGiver
	{
		// Token: 0x06007CB4 RID: 31924 RVA: 0x002590F8 File Offset: 0x002572F8
		protected override Job TryGiveJob(Pawn pawn)
		{
			Job curJob = pawn.CurJob;
			AbilityDef abilityDef;
			if (curJob == null)
			{
				abilityDef = null;
			}
			else
			{
				Ability ability = curJob.ability;
				abilityDef = ((ability != null) ? ability.def : null);
			}
			if (abilityDef == this.ability)
			{
				return null;
			}
			Pawn_AbilityTracker abilities = pawn.abilities;
			Ability ability2 = (abilities != null) ? abilities.GetAbility(this.ability, false) : null;
			if (ability2 == null || !ability2.CanCast)
			{
				return null;
			}
			LocalTargetInfo target = this.GetTarget(pawn, ability2);
			if (!target.IsValid)
			{
				return null;
			}
			return ability2.GetJob(target, target);
		}

		// Token: 0x06007CB5 RID: 31925
		protected abstract LocalTargetInfo GetTarget(Pawn caster, Ability ability);

		// Token: 0x06007CB6 RID: 31926 RVA: 0x00259175 File Offset: 0x00257375
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_AICastAbility jobGiver_AICastAbility = (JobGiver_AICastAbility)base.DeepCopy(resolve);
			jobGiver_AICastAbility.ability = this.ability;
			return jobGiver_AICastAbility;
		}

		// Token: 0x04005409 RID: 21513
		protected AbilityDef ability;
	}
}
