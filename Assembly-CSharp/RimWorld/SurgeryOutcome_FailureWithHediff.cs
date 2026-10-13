using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020037CD RID: 14285
	public class SurgeryOutcome_FailureWithHediff : SurgeryOutcome_Failure
	{
		// Token: 0x06013C8B RID: 81035 RVA: 0x005DE9FF File Offset: 0x005DCBFF
		protected override bool CanApply(RecipeDef recipe)
		{
			return this.failedHediff != null && base.CanApply(recipe) && (this.applyToRecipes.NullOrEmpty<RecipeDef>() || this.applyToRecipes.Contains(recipe));
		}

		// Token: 0x06013C8C RID: 81036 RVA: 0x005DEA34 File Offset: 0x005DCC34
		protected override void PostDamagedApplied(Pawn patient)
		{
			patient.health.AddHediff(this.failedHediff, null, null, null);
		}

		// Token: 0x0400DB08 RID: 56072
		public List<RecipeDef> applyToRecipes;

		// Token: 0x0400DB09 RID: 56073
		public HediffDef failedHediff;
	}
}
