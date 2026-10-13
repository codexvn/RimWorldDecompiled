using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200265E RID: 9822
	public class PreceptWorker_Animal : PreceptWorker
	{
		// Token: 0x0600D766 RID: 55142 RVA: 0x003FE460 File Offset: 0x003FC660
		public override AcceptanceReport CanUse(ThingDef def, Ideo ideo, FactionDef generatingFor)
		{
			foreach (Precept precept in ideo.PreceptsListForReading)
			{
				Precept_Animal precept_Animal = precept as Precept_Animal;
				if (precept_Animal != null && precept_Animal.ThingDef == def)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x170020F9 RID: 8441
		// (get) Token: 0x0600D767 RID: 55143 RVA: 0x003FE4D0 File Offset: 0x003FC6D0
		public override IEnumerable<PreceptThingChance> ThingDefs
		{
			get
			{
				foreach (ThingDef def in from x in DefDatabase<ThingDef>.AllDefs
				where x.thingCategories != null && x.thingCategories.Contains(ThingCategoryDefOf.Animals) && !x.race.Insect && !x.race.Dryad
				select x)
				{
					yield return new PreceptThingChance
					{
						def = def,
						chance = 1f
					};
				}
				IEnumerator<ThingDef> enumerator = null;
				yield break;
				yield break;
			}
		}
	}
}
