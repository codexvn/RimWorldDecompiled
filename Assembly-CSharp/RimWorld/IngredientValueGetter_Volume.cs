using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020019B2 RID: 6578
	public class IngredientValueGetter_Volume : IngredientValueGetter
	{
		// Token: 0x060094CB RID: 38091 RVA: 0x002BA6AC File Offset: 0x002B88AC
		public override float ValuePerUnitOf(ThingDef t)
		{
			if (t.IsStuff)
			{
				return t.VolumePerUnit;
			}
			return 1f;
		}

		// Token: 0x060094CC RID: 38092 RVA: 0x002BA6C4 File Offset: 0x002B88C4
		public override string BillRequirementsDescription(RecipeDef r, IngredientCount ing)
		{
			if (!ing.filter.AllowedThingDefs.Any((ThingDef td) => td.smallVolume) || ing.filter.AllowedThingDefs.Any((ThingDef td) => td.smallVolume && !r.GetPremultipliedSmallIngredients().Contains(td)))
			{
				return ing.GetBaseCount().ToString() + "x " + ing.filter.Summary;
			}
			return (ing.GetBaseCount() * 10f).ToString() + "x " + ing.filter.Summary;
		}

		// Token: 0x060094CD RID: 38093 RVA: 0x002BA77C File Offset: 0x002B897C
		public override string ExtraDescriptionLine(RecipeDef r)
		{
			Func<ThingDef, bool> <>9__1;
			if (r.ingredients.Any(delegate(IngredientCount ing)
			{
				IEnumerable<ThingDef> allowedThingDefs = ing.filter.AllowedThingDefs;
				Func<ThingDef, bool> predicate;
				if ((predicate = <>9__1) == null)
				{
					predicate = (<>9__1 = ((ThingDef td) => td.smallVolume && !r.GetPremultipliedSmallIngredients().Contains(td)));
				}
				return allowedThingDefs.Any(predicate);
			}))
			{
				return "BillRequiresMayVary".Translate(10.ToStringCached());
			}
			return null;
		}
	}
}
