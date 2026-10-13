using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x0200025D RID: 605
	public class RecipeWorkerCounter_ButcherAnimals : RecipeWorkerCounter
	{
		// Token: 0x06000F70 RID: 3952 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool CanCountProducts(Bill_Production bill)
		{
			return true;
		}

		// Token: 0x06000F71 RID: 3953 RVA: 0x000490C4 File Offset: 0x000472C4
		public override int CountProducts(Bill_Production bill)
		{
			int num = 0;
			List<ThingDef> childThingDefs = ThingCategoryDefOf.MeatRaw.childThingDefs;
			for (int i = 0; i < childThingDefs.Count; i++)
			{
				num += bill.Map.resourceCounter.GetCount(childThingDefs[i]);
			}
			return num;
		}

		// Token: 0x06000F72 RID: 3954 RVA: 0x0004910A File Offset: 0x0004730A
		public override string ProductsDescription(Bill_Production bill)
		{
			return ThingCategoryDefOf.MeatRaw.label;
		}

		// Token: 0x06000F73 RID: 3955 RVA: 0x00049118 File Offset: 0x00047318
		public override bool CanPossiblyStore(Bill_Production bill, ISlotGroup slotGroup)
		{
			foreach (ThingDef thingDef in bill.ingredientFilter.AllowedThingDefs)
			{
				if (thingDef.ingestible != null && thingDef.ingestible.sourceDef != null)
				{
					RaceProperties race = thingDef.ingestible.sourceDef.race;
					if (race != null && race.meatDef != null && !slotGroup.Settings.AllowedToAccept(race.meatDef))
					{
						return false;
					}
				}
			}
			return true;
		}
	}
}
