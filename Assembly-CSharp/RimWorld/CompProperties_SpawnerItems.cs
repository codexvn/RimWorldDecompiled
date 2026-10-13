using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02002E32 RID: 11826
	public class CompProperties_SpawnerItems : CompProperties
	{
		// Token: 0x17002AA8 RID: 10920
		// (get) Token: 0x06010A68 RID: 68200 RVA: 0x004CD26D File Offset: 0x004CB46D
		public IEnumerable<ThingDef> MatchingItems
		{
			get
			{
				return from def in DefDatabase<ThingDef>.AllDefsListForReading
				where def.BaseMarketValue <= this.approxMarketValuePerDay && ((def.IsStuff && this.stuffCategories.Any((StuffCategoryDef c) => def.stuffProps.categories.Contains(c))) || this.categories.Any((ThingCategoryDef c) => def.IsWithinCategory(c)))
				select def;
			}
		}

		// Token: 0x06010A69 RID: 68201 RVA: 0x004CD285 File Offset: 0x004CB485
		public CompProperties_SpawnerItems()
		{
			this.compClass = typeof(CompSpawnerItems);
		}

		// Token: 0x06010A6A RID: 68202 RVA: 0x004CD2BE File Offset: 0x004CB4BE
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			if (this.MatchingItems.Count<ThingDef>() <= 0)
			{
				yield return "Could not find any item that would be spawnable by " + parentDef.defName + " (CompSpawnerItems)!";
			}
			yield break;
		}

		// Token: 0x0400AD86 RID: 44422
		public float approxMarketValuePerDay;

		// Token: 0x0400AD87 RID: 44423
		public int spawnInterval = 60000;

		// Token: 0x0400AD88 RID: 44424
		public List<StuffCategoryDef> stuffCategories = new List<StuffCategoryDef>();

		// Token: 0x0400AD89 RID: 44425
		public List<ThingCategoryDef> categories = new List<ThingCategoryDef>();
	}
}
