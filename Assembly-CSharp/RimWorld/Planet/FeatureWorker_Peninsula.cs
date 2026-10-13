using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003C3A RID: 15418
	public class FeatureWorker_Peninsula : FeatureWorker_Protrusion
	{
		// Token: 0x06014EA7 RID: 85671 RVA: 0x0064386C File Offset: 0x00641A6C
		protected override bool IsRoot(PlanetTile tile)
		{
			BiomeDef primaryBiome = Find.WorldGrid[tile].PrimaryBiome;
			return primaryBiome != BiomeDefOf.Ocean && primaryBiome != BiomeDefOf.Lake;
		}
	}
}
