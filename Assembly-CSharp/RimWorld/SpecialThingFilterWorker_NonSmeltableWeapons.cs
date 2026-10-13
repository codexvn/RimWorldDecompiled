using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020036C4 RID: 14020
	public class SpecialThingFilterWorker_NonSmeltableWeapons : SpecialThingFilterWorker
	{
		// Token: 0x06013775 RID: 79733 RVA: 0x005C8E04 File Offset: 0x005C7004
		public override bool Matches(Thing t)
		{
			return !t.Smeltable;
		}

		// Token: 0x06013776 RID: 79734 RVA: 0x005C8E1C File Offset: 0x005C701C
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

		// Token: 0x06013777 RID: 79735 RVA: 0x005C8E7A File Offset: 0x005C707A
		public override bool AlwaysMatches(ThingDef def)
		{
			return this.CanEverMatch(def) && !def.PotentiallySmeltable;
		}
	}
}
