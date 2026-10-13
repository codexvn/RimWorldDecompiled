using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A06 RID: 6662
	public abstract class BiomeWorker
	{
		// Token: 0x06009650 RID: 38480
		public abstract float GetScore(BiomeDef biome, Tile tile, PlanetTile planetTile);

		// Token: 0x06009651 RID: 38481 RVA: 0x002C00D8 File Offset: 0x002BE2D8
		public virtual bool CanPlaceOnLayer(BiomeDef biome, PlanetLayer layer)
		{
			return (biome.layerWhitelist.NullOrEmpty<PlanetLayerDef>() || biome.layerWhitelist.Contains(layer.Def)) && (biome.layerBlacklist.NullOrEmpty<PlanetLayerDef>() || !biome.layerBlacklist.Contains(layer.Def)) && (!layer.Def.onlyAllowWhitelistedBiomes || (!biome.layerWhitelist.NullOrEmpty<PlanetLayerDef>() && biome.layerWhitelist.Contains(layer.Def)));
		}
	}
}
