using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200281B RID: 10267
	public class RoleEffect_NoRangedWeapons : RoleEffect
	{
		// Token: 0x17002249 RID: 8777
		// (get) Token: 0x0600E0A1 RID: 57505 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool IsBad
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600E0A2 RID: 57506 RVA: 0x0041F386 File Offset: 0x0041D586
		public RoleEffect_NoRangedWeapons()
		{
			this.labelKey = "RoleEffectWontUseRangedWeapons";
		}

		// Token: 0x0600E0A3 RID: 57507 RVA: 0x0041F399 File Offset: 0x0041D599
		public override bool CanEquip(Pawn pawn, Thing thing)
		{
			return !thing.def.IsRangedWeapon;
		}
	}
}
