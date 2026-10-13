using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x0200025C RID: 604
	public class RecipeWorkerCounter_MakeStoneBlocks : RecipeWorkerCounter
	{
		// Token: 0x06000F6B RID: 3947 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool CanCountProducts(Bill_Production bill)
		{
			return true;
		}

		// Token: 0x06000F6C RID: 3948 RVA: 0x00048FE8 File Offset: 0x000471E8
		public override int CountProducts(Bill_Production bill)
		{
			int num = 0;
			List<ThingDef> childThingDefs = ThingCategoryDefOf.StoneBlocks.childThingDefs;
			for (int i = 0; i < childThingDefs.Count; i++)
			{
				num += bill.Map.resourceCounter.GetCount(childThingDefs[i]);
			}
			return num;
		}

		// Token: 0x06000F6D RID: 3949 RVA: 0x0004902E File Offset: 0x0004722E
		public override string ProductsDescription(Bill_Production bill)
		{
			return ThingCategoryDefOf.StoneBlocks.label;
		}

		// Token: 0x06000F6E RID: 3950 RVA: 0x0004903C File Offset: 0x0004723C
		public override bool CanPossiblyStore(Bill_Production bill, ISlotGroup slotGroup)
		{
			foreach (ThingDef thingDef in bill.ingredientFilter.AllowedThingDefs)
			{
				if (!thingDef.butcherProducts.NullOrEmpty<ThingDefCountClass>())
				{
					ThingDef thingDef2 = thingDef.butcherProducts[0].thingDef;
					if (!slotGroup.Settings.AllowedToAccept(thingDef2))
					{
						return false;
					}
				}
			}
			return true;
		}
	}
}
