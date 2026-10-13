using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003C38 RID: 15416
	public class FeatureWorker_Island : FeatureWorker_FloodFill
	{
		// Token: 0x06014EA2 RID: 85666 RVA: 0x006437E8 File Offset: 0x006419E8
		protected override bool IsRoot(PlanetTile tile)
		{
			BiomeDef primaryBiome = Find.WorldGrid[tile].PrimaryBiome;
			return primaryBiome != BiomeDefOf.Ocean && primaryBiome != BiomeDefOf.Lake;
		}

		// Token: 0x06014EA3 RID: 85667 RVA: 0x0064381B File Offset: 0x00641A1B
		protected override bool IsPossiblyAllowed(PlanetTile tile)
		{
			return Find.WorldGrid[tile].PrimaryBiome == BiomeDefOf.Lake;
		}
	}
}
