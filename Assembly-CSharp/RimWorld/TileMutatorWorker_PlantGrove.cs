using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200210E RID: 8462
	public class TileMutatorWorker_PlantGrove : TileMutatorWorker
	{
		// Token: 0x0600B89D RID: 47261 RVA: 0x0034CD7F File Offset: 0x0034AF7F
		public TileMutatorWorker_PlantGrove(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x0600B89E RID: 47262 RVA: 0x003540DA File Offset: 0x003522DA
		public override bool IsValidTile(PlanetTile tile, PlanetLayer layer)
		{
			return layer[tile].PrimaryBiome.AllWildPlants.Any(new Predicate<ThingDef>(this.def.plantKinds.Contains));
		}

		// Token: 0x0600B89F RID: 47263 RVA: 0x00354109 File Offset: 0x00352309
		public override string GetLabel(PlanetTile tile)
		{
			return "PlantGrove".Translate(this.GetPlantKind(tile).label);
		}

		// Token: 0x0600B8A0 RID: 47264 RVA: 0x0035412B File Offset: 0x0035232B
		public override string GetDescription(PlanetTile tile)
		{
			return this.def.description.Formatted(this.GetPlantKind(tile).label);
		}

		// Token: 0x0600B8A1 RID: 47265 RVA: 0x00354153 File Offset: 0x00352353
		public override float PlantCommonalityFactorFor(ThingDef plant, PlanetTile tile)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return 1f;
			}
			if (plant != this.GetPlantKind(tile))
			{
				return 1f;
			}
			return 10f;
		}

		// Token: 0x0600B8A2 RID: 47266 RVA: 0x00354178 File Offset: 0x00352378
		private ThingDef GetPlantKind(PlanetTile tile)
		{
			BiomeDef primaryBiome = Find.WorldGrid[tile].PrimaryBiome;
			List<ThingDef> list;
			if (!TileMutatorWorker_PlantGrove.cachedPlantKinds.TryGetValue(primaryBiome, out list))
			{
				list = new List<ThingDef>();
				foreach (ThingDef item in primaryBiome.AllWildPlants)
				{
					if (this.def.plantKinds.Contains(item))
					{
						list.Add(item);
					}
				}
				TileMutatorWorker_PlantGrove.cachedPlantKinds.Add(primaryBiome, list);
			}
			Rand.PushState();
			Rand.Seed = tile.GetHashCode();
			ThingDef result = list.RandomElement<ThingDef>();
			Rand.PopState();
			return result;
		}

		// Token: 0x04007F42 RID: 32578
		private static Dictionary<BiomeDef, List<ThingDef>> cachedPlantKinds = new Dictionary<BiomeDef, List<ThingDef>>();

		// Token: 0x04007F43 RID: 32579
		private const float PlantCommonalityFactor = 10f;
	}
}
