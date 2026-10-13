using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003C2C RID: 15404
	public class FeatureWorker_Archipelago : FeatureWorker_Cluster
	{
		// Token: 0x06014E5E RID: 85598 RVA: 0x0064264C File Offset: 0x0064084C
		protected override bool IsRoot(PlanetTile tile)
		{
			BiomeDef primaryBiome = Find.WorldGrid[tile].PrimaryBiome;
			return primaryBiome != BiomeDefOf.Ocean && primaryBiome != BiomeDefOf.Lake;
		}

		// Token: 0x06014E5F RID: 85599 RVA: 0x0064267F File Offset: 0x0064087F
		protected override bool CanTraverse(PlanetTile tile, out bool ifRootThenRootGroupSizeMustMatch)
		{
			ifRootThenRootGroupSizeMustMatch = true;
			return true;
		}

		// Token: 0x06014E60 RID: 85600 RVA: 0x00642688 File Offset: 0x00640888
		protected override bool IsMember(PlanetTile tile, out bool ifRootThenRootGroupSizeMustMatch)
		{
			ifRootThenRootGroupSizeMustMatch = true;
			bool flag;
			return base.IsMember(tile, out flag);
		}
	}
}
