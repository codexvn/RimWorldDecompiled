using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002554 RID: 9556
	public static class TraitUtility
	{
		// Token: 0x0600D141 RID: 53569 RVA: 0x003D8E64 File Offset: 0x003D7064
		public static void ApplySkillGainFromTrait(Pawn pawn, Trait trait)
		{
			List<SkillGain> skillGains = trait.CurrentData.skillGains;
			if (!trait.Suppressed && skillGains != null && pawn.skills != null)
			{
				foreach (SkillGain skillGain in skillGains)
				{
					SkillRecord skill = pawn.skills.GetSkill(skillGain.skill);
					if (skill != null && !skill.PermanentlyDisabled)
					{
						skill.Level = skill.GetLevel(false) + skillGain.amount;
					}
				}
			}
		}
	}
}
