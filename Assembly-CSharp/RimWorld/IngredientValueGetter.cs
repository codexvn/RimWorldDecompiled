using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020019B1 RID: 6577
	public abstract class IngredientValueGetter
	{
		// Token: 0x060094C7 RID: 38087
		public abstract float ValuePerUnitOf(ThingDef t);

		// Token: 0x060094C8 RID: 38088
		public abstract string BillRequirementsDescription(RecipeDef r, IngredientCount ing);

		// Token: 0x060094C9 RID: 38089 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual string ExtraDescriptionLine(RecipeDef r)
		{
			return null;
		}
	}
}
