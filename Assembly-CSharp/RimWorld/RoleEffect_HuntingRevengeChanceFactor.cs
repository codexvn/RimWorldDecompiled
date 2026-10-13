using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002820 RID: 10272
	public class RoleEffect_HuntingRevengeChanceFactor : RoleEffect
	{
		// Token: 0x0600E0AB RID: 57515 RVA: 0x0041F456 File Offset: 0x0041D656
		public RoleEffect_HuntingRevengeChanceFactor()
		{
			this.labelKey = "RoleEffectHuntingRevengeChance";
		}

		// Token: 0x0600E0AC RID: 57516 RVA: 0x0041F469 File Offset: 0x0041D669
		public override string Label(Pawn pawn, Precept_Role role)
		{
			return this.labelKey.Translate("x" + this.factor.ToStringPercent());
		}

		// Token: 0x04009700 RID: 38656
		public float factor;
	}
}
