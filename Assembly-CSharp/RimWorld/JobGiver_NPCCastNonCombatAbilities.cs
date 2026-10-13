using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020014B1 RID: 5297
	public class JobGiver_NPCCastNonCombatAbilities : ThinkNode_JobGiver
	{
		// Token: 0x06007F02 RID: 32514 RVA: 0x002634B8 File Offset: 0x002616B8
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.abilities == null)
			{
				return null;
			}
			List<Ability> list = pawn.abilities.AICastableAbilities(pawn, false);
			if (list.NullOrEmpty<Ability>())
			{
				return null;
			}
			Ability ability = list.RandomElement<Ability>();
			VerbProperties verbProperties = ability.def.verbProperties;
			TargetingParameters targetingParameters = (verbProperties != null) ? verbProperties.targetParams : null;
			if (!ability.def.targetRequired || (targetingParameters != null && targetingParameters.canTargetSelf))
			{
				return ability.GetJob(pawn, pawn);
			}
			return null;
		}
	}
}
