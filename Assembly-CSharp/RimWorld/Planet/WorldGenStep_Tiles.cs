using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003D0A RID: 15626
	public class WorldGenStep_Tiles : WorldGenStep
	{
		// Token: 0x170034F4 RID: 13556
		// (get) Token: 0x06015418 RID: 87064 RVA: 0x0065C1D3 File Offset: 0x0065A3D3
		public override int SeedPart
		{
			get
			{
				return 16315992;
			}
		}

		// Token: 0x06015419 RID: 87065 RVA: 0x0065C1DC File Offset: 0x0065A3DC
		public override void GenerateFresh(string seed, PlanetLayer layer)
		{
			layer.Tiles.Clear();
			for (int i = 0; i < layer.TilesCount; i++)
			{
				Tile item = this.GenerateTileFor(new PlanetTile(i, layer), layer);
				layer.Tiles.Add(item);
			}
		}

		// Token: 0x0601541A RID: 87066 RVA: 0x0065C220 File Offset: 0x0065A420
		protected virtual Tile GenerateTileFor(PlanetTile tile, PlanetLayer layer)
		{
			return new Tile(tile)
			{
				PrimaryBiome = tile.LayerDef.DefaultBiome,
				elevation = tile.Layer.Radius
			};
		}
	}
}
