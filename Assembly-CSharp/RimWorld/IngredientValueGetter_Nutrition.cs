using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020019B6 RID: 6582
	public class IngredientValueGetter_Nutrition : IngredientValueGetter
	{
		// Token: 0x060094D7 RID: 38103 RVA: 0x002BA864 File Offset: 0x002B8A64
		public override float ValuePerUnitOf(ThingDef t)
		{
			if (!t.IsNutritionGivingIngestible)
			{
				return 0f;
			}
			return t.GetStatValueAbstract(StatDefOf.Nutrition, null);
		}

		// Token: 0x060094D8 RID: 38104 RVA: 0x002BA880 File Offset: 0x002B8A80
		public override string BillRequirementsDescription(RecipeDef r, IngredientCount ing)
		{
			return ing.GetBaseCount().ToString() + "x " + "BillNutrition".Translate() + " (" + ing.filter.Summary + ")";
		}
	}
}
