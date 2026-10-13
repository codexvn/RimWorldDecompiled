using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200281F RID: 10271
	public class RoleEffect_ProductionQualityOffset : RoleEffect
	{
		// Token: 0x0600E0A9 RID: 57513 RVA: 0x0041F421 File Offset: 0x0041D621
		public RoleEffect_ProductionQualityOffset()
		{
			this.labelKey = "RoleEffectProductionQualityOffset";
		}

		// Token: 0x0600E0AA RID: 57514 RVA: 0x0041F434 File Offset: 0x0041D634
		public override string Label(Pawn pawn, Precept_Role role)
		{
			return this.labelKey.Translate(this.offset.ToStringWithSign());
		}

		// Token: 0x040096FF RID: 38655
		public int offset;
	}
}
