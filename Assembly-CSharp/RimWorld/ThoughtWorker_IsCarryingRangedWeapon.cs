using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001898 RID: 6296
	public class ThoughtWorker_IsCarryingRangedWeapon : ThoughtWorker
	{
		// Token: 0x06009021 RID: 36897 RVA: 0x002A82B7 File Offset: 0x002A64B7
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return p.equipment.Primary != null && p.equipment.Primary.def.IsRangedWeapon;
		}
	}
}
