using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F8E RID: 12174
	public class StockGenerator_Category : StockGenerator
	{
		// Token: 0x06011258 RID: 70232 RVA: 0x004EAC3F File Offset: 0x004E8E3F
		public override IEnumerable<Thing> GenerateThings(PlanetTile forTile, Faction faction = null)
		{
			List<ThingDef> generatedDefs = new List<ThingDef>();
			int numThingDefsToUse = this.thingDefCountRange.RandomInRange;
			Func<ThingDef, bool> <>9__0;
			int num;
			for (int i = 0; i < numThingDefsToUse; i = num + 1)
			{
				IEnumerable<ThingDef> descendantThingDefs = this.categoryDef.DescendantThingDefs;
				Func<ThingDef, bool> predicate;
				if ((predicate = <>9__0) == null)
				{
					predicate = (<>9__0 = ((ThingDef t) => t.tradeability.TraderCanSell() && t.techLevel <= this.maxTechLevelGenerate && !generatedDefs.Contains(t) && (this.excludedThingDefs == null || !this.excludedThingDefs.Contains(t)) && (this.excludedCategories == null || !this.excludedCategories.Any((ThingCategoryDef c) => c.DescendantThingDefs.Contains(t)))));
				}
				ThingDef chosenThingDef;
				if (!descendantThingDefs.Where(predicate).TryRandomElement(out chosenThingDef))
				{
					break;
				}
				foreach (Thing thing in StockGeneratorUtility.TryMakeForStock(chosenThingDef, base.RandomCountOf(chosenThingDef), faction))
				{
					yield return thing;
				}
				IEnumerator<Thing> enumerator = null;
				generatedDefs.Add(chosenThingDef);
				chosenThingDef = null;
				num = i;
			}
			yield break;
			yield break;
		}

		// Token: 0x06011259 RID: 70233 RVA: 0x004EAC58 File Offset: 0x004E8E58
		public override bool HandlesThingDef(ThingDef t)
		{
			return this.categoryDef.DescendantThingDefs.Contains(t) && t.tradeability != Tradeability.None && t.techLevel <= this.maxTechLevelBuy && (this.excludedThingDefs == null || !this.excludedThingDefs.Contains(t)) && (this.excludedCategories == null || !this.excludedCategories.Any((ThingCategoryDef c) => c.DescendantThingDefs.Contains(t)));
		}

		// Token: 0x0400B1E8 RID: 45544
		private ThingCategoryDef categoryDef;

		// Token: 0x0400B1E9 RID: 45545
		private IntRange thingDefCountRange = IntRange.One;

		// Token: 0x0400B1EA RID: 45546
		private List<ThingDef> excludedThingDefs;

		// Token: 0x0400B1EB RID: 45547
		private List<ThingCategoryDef> excludedCategories;
	}
}
