using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020036C8 RID: 14024
	public class SpecialThingFilterWorker_NonBurnableWeapons : SpecialThingFilterWorker
	{
		// Token: 0x06013785 RID: 79749 RVA: 0x005C8ED2 File Offset: 0x005C70D2
		public override bool Matches(Thing t)
		{
			return this.CanEverMatch(t.def) && !t.BurnableByRecipe;
		}

		// Token: 0x06013786 RID: 79750 RVA: 0x005C8F14 File Offset: 0x005C7114
		public override bool CanEverMatch(ThingDef def)
		{
			if (!def.IsWeapon)
			{
				return false;
			}
			if (!def.thingCategories.NullOrEmpty<ThingCategoryDef>())
			{
				for (int i = 0; i < def.thingCategories.Count; i++)
				{
					for (ThingCategoryDef thingCategoryDef = def.thingCategories[i]; thingCategoryDef != null; thingCategoryDef = thingCategoryDef.parent)
					{
						if (thingCategoryDef == ThingCategoryDefOf.Weapons)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06013787 RID: 79751 RVA: 0x005C8F72 File Offset: 0x005C7172
		public override bool AlwaysMatches(ThingDef def)
		{
			return this.CanEverMatch(def) && !def.burnableByRecipe && !def.MadeFromStuff;
		}
	}
}
