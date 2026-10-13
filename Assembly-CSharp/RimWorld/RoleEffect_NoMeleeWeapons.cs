using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200281A RID: 10266
	public class RoleEffect_NoMeleeWeapons : RoleEffect
	{
		// Token: 0x17002248 RID: 8776
		// (get) Token: 0x0600E09E RID: 57502 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool IsBad
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600E09F RID: 57503 RVA: 0x0041F363 File Offset: 0x0041D563
		public RoleEffect_NoMeleeWeapons()
		{
			this.labelKey = "RoleEffectWontUseMeleeWeapons";
		}

		// Token: 0x0600E0A0 RID: 57504 RVA: 0x0041F376 File Offset: 0x0041D576
		public override bool CanEquip(Pawn pawn, Thing thing)
		{
			return !thing.def.IsMeleeWeapon;
		}
	}
}
