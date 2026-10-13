using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001899 RID: 6297
	public class ThoughtWorker_IsCarryingIncendiaryWeapon : ThoughtWorker
	{
		// Token: 0x06009023 RID: 36899 RVA: 0x002A82E4 File Offset: 0x002A64E4
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (p.equipment.Primary == null)
			{
				return false;
			}
			List<Verb> allVerbs = p.equipment.Primary.GetComp<CompEquippable>().AllVerbs;
			for (int i = 0; i < allVerbs.Count; i++)
			{
				if (allVerbs[i].IsIncendiary_Ranged() || allVerbs[i].IsIncendiary_Melee())
				{
					return true;
				}
			}
			CompEquippableAbility comp = p.equipment.Primary.GetComp<CompEquippableAbility>();
			Ability ability = (comp != null) ? comp.AbilityForReading : null;
			if (ability != null && ability.IsIncendiary())
			{
				return true;
			}
			return false;
		}
	}
}
