using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003C30 RID: 15408
	public class FeatureWorker_MountainRange : FeatureWorker_Cluster
	{
		// Token: 0x06014E7A RID: 85626 RVA: 0x00642DD6 File Offset: 0x00640FD6
		protected override bool IsRoot(PlanetTile tile)
		{
			return Find.WorldGrid[tile].hilliness != Hilliness.Flat;
		}

		// Token: 0x06014E7B RID: 85627 RVA: 0x00642DEE File Offset: 0x00640FEE
		protected override bool CanTraverse(PlanetTile tile, out bool ifRootThenRootGroupSizeMustMatch)
		{
			ifRootThenRootGroupSizeMustMatch = false;
			return Find.WorldGrid[tile].PrimaryBiome != BiomeDefOf.Ocean;
		}
	}
}
