using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020018B0 RID: 6320
	public class ThoughtWorker_WeaponTraitKillNeed : ThoughtWorker_WeaponTrait
	{
		// Token: 0x0600906F RID: 36975 RVA: 0x002A94F8 File Offset: 0x002A76F8
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!base.CurrentStateInternal(p).Active)
			{
				return ThoughtState.Inactive;
			}
			CompBladelinkWeapon compBladelinkWeapon = p.equipment.bondedWeapon.TryGetComp<CompBladelinkWeapon>();
			List<WeaponTraitDef> traitsListForReading = compBladelinkWeapon.TraitsListForReading;
			for (int i = 0; i < traitsListForReading.Count; i++)
			{
				if (traitsListForReading[i] == WeaponTraitDefOf.NeedKill)
				{
					return compBladelinkWeapon.TicksSinceLastKill > 1200000;
				}
			}
			return ThoughtState.Inactive;
		}

		// Token: 0x04005C44 RID: 23620
		public const int TicksToGiveMemory = 1200000;
	}
}
