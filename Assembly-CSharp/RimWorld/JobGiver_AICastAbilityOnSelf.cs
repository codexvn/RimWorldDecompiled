using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020013FA RID: 5114
	public class JobGiver_AICastAbilityOnSelf : JobGiver_AICastAbility
	{
		// Token: 0x06007CB8 RID: 31928 RVA: 0x00259190 File Offset: 0x00257390
		protected override LocalTargetInfo GetTarget(Pawn caster, Ability ability)
		{
			LocalTargetInfo localTargetInfo = new LocalTargetInfo(caster);
			if (!ability.def.targetRequired || ability.CanApplyOn(localTargetInfo))
			{
				return localTargetInfo;
			}
			return LocalTargetInfo.Invalid;
		}
	}
}
