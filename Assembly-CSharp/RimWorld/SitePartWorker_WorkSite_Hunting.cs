using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200284F RID: 10319
	public class SitePartWorker_WorkSite_Hunting : SitePartWorker_WorkSite
	{
		// Token: 0x17002263 RID: 8803
		// (get) Token: 0x0600E1A7 RID: 57767 RVA: 0x004225FE File Offset: 0x004207FE
		public override IEnumerable<PreceptDef> DisallowedPrecepts
		{
			get
			{
				return from p in DefDatabase<PreceptDef>.AllDefs
				where p.disallowHuntingCamps
				select p;
			}
		}

		// Token: 0x17002264 RID: 8804
		// (get) Token: 0x0600E1A8 RID: 57768 RVA: 0x00422629 File Offset: 0x00420829
		public override PawnGroupKindDef WorkerGroupKind
		{
			get
			{
				return PawnGroupKindDefOf.Hunters;
			}
		}

		// Token: 0x0600E1A9 RID: 57769 RVA: 0x00422630 File Offset: 0x00420830
		public override bool CanSpawnOn(PlanetTile tile)
		{
			return Find.WorldGrid[tile].AnimalDensity > BiomeDefOf.Desert.animalDensity && base.CanSpawnOn(tile);
		}

		// Token: 0x0600E1AA RID: 57770 RVA: 0x00422657 File Offset: 0x00420857
		public override IEnumerable<SitePartWorker_WorkSite.CampLootThingStruct> LootThings(PlanetTile tile)
		{
			IEnumerable<ThingDef> enumerable = from a in Find.WorldGrid[tile].PrimaryBiome.AllWildAnimals
			select a.RaceProps.leatherDef;
			int num = enumerable.Count((ThingDef x) => x != null);
			if (num <= 0)
			{
				yield break;
			}
			float leatherWeight = 1f / (float)num;
			foreach (ThingDef thingDef in enumerable)
			{
				if (thingDef != null)
				{
					yield return new SitePartWorker_WorkSite.CampLootThingStruct
					{
						thing = thingDef,
						thing2 = ThingDefOf.Pemmican,
						weight = leatherWeight
					};
				}
			}
			IEnumerator<ThingDef> enumerator = null;
			yield break;
			yield break;
		}
	}
}
