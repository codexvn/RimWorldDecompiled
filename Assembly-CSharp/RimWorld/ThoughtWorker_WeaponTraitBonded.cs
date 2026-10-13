using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020018AF RID: 6319
	public class ThoughtWorker_WeaponTraitBonded : ThoughtWorker_WeaponTrait
	{
		// Token: 0x0600906D RID: 36973 RVA: 0x002A9484 File Offset: 0x002A7684
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!base.CurrentStateInternal(p).Active)
			{
				return ThoughtState.Inactive;
			}
			List<WeaponTraitDef> traitsListForReading = p.equipment.bondedWeapon.TryGetComp<CompBladelinkWeapon>().TraitsListForReading;
			for (int i = 0; i < traitsListForReading.Count; i++)
			{
				if (traitsListForReading[i].bondedThought == this.def)
				{
					return true;
				}
			}
			return ThoughtState.Inactive;
		}
	}
}
