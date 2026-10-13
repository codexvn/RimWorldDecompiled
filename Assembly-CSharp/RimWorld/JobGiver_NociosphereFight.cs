using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020014AC RID: 5292
	public class JobGiver_NociosphereFight : ThinkNode_JobGiver
	{
		// Token: 0x06007EED RID: 32493 RVA: 0x00262E00 File Offset: 0x00261000
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (this.IsAnyAttackOnCooldown(pawn))
			{
				return null;
			}
			Thing thing = NociosphereUtility.FindTarget(pawn);
			if (thing == null)
			{
				return null;
			}
			Ability randomPossibleAbility = this.GetRandomPossibleAbility(pawn, thing);
			if (randomPossibleAbility == null)
			{
				return null;
			}
			return randomPossibleAbility.GetJob(thing, null);
		}

		// Token: 0x06007EEE RID: 32494 RVA: 0x00262E44 File Offset: 0x00261044
		private bool IsAnyAttackOnCooldown(Pawn pawn)
		{
			List<CompProperties_Nociosphere.AttackDetails> attacks = pawn.TryGetComp<CompNociosphere>().Props.attacks;
			for (int i = 0; i < attacks.Count; i++)
			{
				Ability ability = pawn.abilities.GetAbility(attacks[i].ability, false);
				if (!ability.CanCast && (float)ability.CooldownTicksRemaining <= attacks[i].maxCooldownTicks)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06007EEF RID: 32495 RVA: 0x00262EB4 File Offset: 0x002610B4
		private Ability GetRandomPossibleAbility(Pawn pawn, Thing target)
		{
			List<CompProperties_Nociosphere.AttackDetails> attacks = pawn.TryGetComp<CompNociosphere>().Props.attacks;
			bool flag = false;
			for (int i = 0; i < attacks.Count; i++)
			{
				Ability ability = pawn.abilities.GetAbility(attacks[i].ability, false);
				Verb verb = ability.verb;
				if (ability.CanCast && verb.CanHitTarget(target) && (attacks[i].requiresPawn || target is Pawn))
				{
					if (attacks[i].aiPreferArtificial)
					{
						flag = true;
					}
					this.tmpAttacks.Add(attacks[i]);
				}
			}
			if (this.tmpAttacks.Empty<CompProperties_Nociosphere.AttackDetails>())
			{
				return null;
			}
			bool artificialTarget = target == null || target is Building || target is Plant;
			CompProperties_Nociosphere.AttackDetails attackDetails;
			if (flag)
			{
				attackDetails = this.tmpAttacks.RandomElementByWeight(delegate(CompProperties_Nociosphere.AttackDetails a)
				{
					if (a.aiPreferArtificial)
					{
						return (float)(artificialTarget ? 2 : 1);
					}
					return (float)(artificialTarget ? 1 : 2);
				});
			}
			else
			{
				attackDetails = this.tmpAttacks.RandomElement<CompProperties_Nociosphere.AttackDetails>();
			}
			this.tmpAttacks.Clear();
			return pawn.abilities.GetAbility(attackDetails.ability, false);
		}

		// Token: 0x04005506 RID: 21766
		private readonly List<CompProperties_Nociosphere.AttackDetails> tmpAttacks = new List<CompProperties_Nociosphere.AttackDetails>();
	}
}
