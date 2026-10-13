using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x02002567 RID: 9575
	public class BiomeWorker_Space : BiomeWorker
	{
		// Token: 0x0600D191 RID: 53649 RVA: 0x003DA059 File Offset: 0x003D8259
		public override bool CanPlaceOnLayer(BiomeDef biome, PlanetLayer layer)
		{
			return layer.Def == PlanetLayerDefOf.Orbit;
		}

		// Token: 0x0600D192 RID: 53650 RVA: 0x00182697 File Offset: 0x00180897
		public override float GetScore(BiomeDef biome, Tile tile, PlanetTile planetTile)
		{
			return -100f;
		}
	}
}
