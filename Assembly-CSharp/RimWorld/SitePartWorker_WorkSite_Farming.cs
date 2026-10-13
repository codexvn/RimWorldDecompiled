using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02002852 RID: 10322
	public class SitePartWorker_WorkSite_Farming : SitePartWorker_WorkSite
	{
		// Token: 0x17002267 RID: 8807
		// (get) Token: 0x0600E1BA RID: 57786 RVA: 0x004228BB File Offset: 0x00420ABB
		public override IEnumerable<PreceptDef> DisallowedPrecepts
		{
			get
			{
				return from p in DefDatabase<PreceptDef>.AllDefs
				where p.disallowFarmingCamps
				select p;
			}
		}

		// Token: 0x17002268 RID: 8808
		// (get) Token: 0x0600E1BB RID: 57787 RVA: 0x004228E6 File Offset: 0x00420AE6
		public override PawnGroupKindDef WorkerGroupKind
		{
			get
			{
				return PawnGroupKindDefOf.Farmers;
			}
		}

		// Token: 0x0600E1BC RID: 57788 RVA: 0x004228ED File Offset: 0x00420AED
		public override bool CanSpawnOn(PlanetTile tile)
		{
			return Find.WorldGrid[tile].PrimaryBiome.allowFarmingCamps && this.LootThings(tile).Any<SitePartWorker_WorkSite.CampLootThingStruct>();
		}

		// Token: 0x0600E1BD RID: 57789 RVA: 0x00422914 File Offset: 0x00420B14
		public override IEnumerable<SitePartWorker_WorkSite.CampLootThingStruct> LootThings(PlanetTile tile)
		{
			SitePartWorker_WorkSite_Farming.<>c__DisplayClass5_0 CS$<>8__locals1 = new SitePartWorker_WorkSite_Farming.<>c__DisplayClass5_0();
			CS$<>8__locals1.temp = Find.World.tileTemperatures.GetSeasonalTemp(tile);
			IEnumerable<ThingDef> enumerable = from t in DefDatabase<ThingDef>.AllDefsListForReading.Where(new Func<ThingDef, bool>(CS$<>8__locals1.<LootThings>g__IsFoodCrop|0))
			select t.plant.harvestedThingDef;
			float cropWeight = 1f / (float)enumerable.Count<ThingDef>();
			foreach (ThingDef thing in enumerable)
			{
				yield return new SitePartWorker_WorkSite.CampLootThingStruct
				{
					thing = thing,
					weight = cropWeight
				};
			}
			IEnumerator<ThingDef> enumerator = null;
			yield break;
			yield break;
		}
	}
}
