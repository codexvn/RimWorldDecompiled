using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003C39 RID: 15417
	public class FeatureWorker_Bay : FeatureWorker_Protrusion
	{
		// Token: 0x06014EA5 RID: 85669 RVA: 0x00643834 File Offset: 0x00641A34
		protected override bool IsRoot(PlanetTile tile)
		{
			BiomeDef primaryBiome = Find.WorldGrid[tile].PrimaryBiome;
			return primaryBiome == BiomeDefOf.Ocean || primaryBiome == BiomeDefOf.Lake;
		}
	}
}
