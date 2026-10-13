using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003C33 RID: 15411
	public class FeatureWorker_Biome : FeatureWorker_FloodFill
	{
		// Token: 0x06014E84 RID: 85636 RVA: 0x00642FCC File Offset: 0x006411CC
		protected override bool IsRoot(PlanetTile tile)
		{
			return this.def.rootBiomes.Contains(Find.WorldGrid[tile].PrimaryBiome);
		}

		// Token: 0x06014E85 RID: 85637 RVA: 0x00642FEE File Offset: 0x006411EE
		protected override bool IsPossiblyAllowed(PlanetTile tile)
		{
			return this.def.acceptableBiomes.Contains(Find.WorldGrid[tile].PrimaryBiome);
		}
	}
}
