using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200198F RID: 6543
	public class CompProperties_Ingredients : CompProperties
	{
		// Token: 0x06009450 RID: 37968 RVA: 0x002B911A File Offset: 0x002B731A
		public CompProperties_Ingredients()
		{
			this.compClass = typeof(CompIngredients);
		}

		// Token: 0x04005FEE RID: 24558
		public bool performMergeCompatibilityChecks = true;

		// Token: 0x04005FEF RID: 24559
		public bool splitTransferableFoodKind;

		// Token: 0x04005FF0 RID: 24560
		public FoodKind noIngredientsFoodKind = FoodKind.Any;
	}
}
