using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x02002105 RID: 8453
	public class TileMutatorWorker_MixedBiome : TileMutatorWorker
	{
		// Token: 0x0600B87E RID: 47230 RVA: 0x0034CD7F File Offset: 0x0034AF7F
		public TileMutatorWorker_MixedBiome(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x0600B87F RID: 47231 RVA: 0x003536A8 File Offset: 0x003518A8
		public override bool IsValidTile(PlanetTile tile, PlanetLayer layer)
		{
			return this.def.biomeWhitelist.Contains(layer[tile].PrimaryBiome) && this.GetNeighbourTile(tile, layer) != null;
		}

		// Token: 0x0600B880 RID: 47232 RVA: 0x003536E8 File Offset: 0x003518E8
		public override void Init(Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			base.Init(map);
			PlanetTile? neighbourTile = this.GetNeighbourTile(map.Tile, map.Tile.Layer);
			if (neighbourTile == null)
			{
				Log.Error("No neighbour tile found for mixed biome tile");
				return;
			}
			ModuleBase moduleBase = new DistFromAxis_Directional((float)map.Size.x / 2f);
			float angle = this.GetAngle(map.Tile, neighbourTile.Value.Tile.PrimaryBiome);
			moduleBase = new Rotate(0.0, (double)angle, 0.0, moduleBase);
			moduleBase = new Translate((double)((float)(-(float)map.Size.x) / 2f), 0.0, (double)((float)(-(float)map.Size.z) / 2f), moduleBase);
			moduleBase = MapNoiseUtility.AddDisplacementNoise(moduleBase, 0.006f, 40f, 2, -1);
			moduleBase = MapNoiseUtility.AddDisplacementNoise(moduleBase, 0.015f, 20f, 6, -1);
			NoiseDebugUI.StoreNoiseRender(moduleBase, "biomeVal");
			bool[] array = new bool[map.Size.x * map.Size.z];
			for (int i = 0; i < array.Length; i++)
			{
				IntVec3 coordinate = map.cellIndices.IndexToCell(i);
				array[i] = (moduleBase.GetValue(coordinate) < 0f);
			}
			MixedBiomeMapComponent mixedBiomeMapComponent = map.components.First((MapComponent c) => c is MixedBiomeMapComponent) as MixedBiomeMapComponent;
			mixedBiomeMapComponent.biomeGrid = array;
			mixedBiomeMapComponent.secondaryBiome = neighbourTile.Value.Tile.PrimaryBiome;
		}

		// Token: 0x0600B881 RID: 47233 RVA: 0x00353894 File Offset: 0x00351A94
		private float GetAngle(PlanetTile tile, BiomeDef otherBiome)
		{
			TileMutatorWorker_MixedBiome.tmpNeighbors.Clear();
			Find.World.grid.GetTileNeighbors(tile, TileMutatorWorker_MixedBiome.tmpNeighbors);
			float num = GenMath.MeanAngle((from t in TileMutatorWorker_MixedBiome.tmpNeighbors
			where t.Tile.PrimaryBiome == otherBiome
			select Find.WorldGrid.GetHeadingFromTo(t, tile)).ToList<float>());
			return (450f - num) % 360f;
		}

		// Token: 0x0600B882 RID: 47234 RVA: 0x00353918 File Offset: 0x00351B18
		private PlanetTile? GetNeighbourTile(PlanetTile tile, PlanetLayer layer)
		{
			BiomeDef primaryBiome = layer[tile].PrimaryBiome;
			TileMutatorWorker_MixedBiome.tmpNeighbors.Clear();
			Rand.PushState(tile.tileId);
			layer.GetTileNeighbors(tile, TileMutatorWorker_MixedBiome.tmpNeighbors);
			TileMutatorWorker_MixedBiome.tmpNeighbors.Shuffle<PlanetTile>();
			Rand.PopState();
			foreach (PlanetTile planetTile in TileMutatorWorker_MixedBiome.tmpNeighbors)
			{
				BiomeDef primaryBiome2 = layer[planetTile].PrimaryBiome;
				if (this.def.biomeWhitelist.Contains(primaryBiome2) && primaryBiome2 != primaryBiome)
				{
					return new PlanetTile?(planetTile);
				}
			}
			return null;
		}

		// Token: 0x0600B883 RID: 47235 RVA: 0x003539DC File Offset: 0x00351BDC
		public BiomeDef SecondaryBiome(PlanetTile tile, PlanetLayer layer)
		{
			PlanetTile? neighbourTile = this.GetNeighbourTile(tile, layer);
			if (neighbourTile == null)
			{
				return null;
			}
			return layer[neighbourTile.Value].PrimaryBiome;
		}

		// Token: 0x04007F29 RID: 32553
		private const float DisplaceMacroFrequency = 0.006f;

		// Token: 0x04007F2A RID: 32554
		private const float DisplaceMacroStrength = 40f;

		// Token: 0x04007F2B RID: 32555
		private const float DisplaceFrequency = 0.015f;

		// Token: 0x04007F2C RID: 32556
		private const float DisplaceStrength = 20f;

		// Token: 0x04007F2D RID: 32557
		private static readonly List<PlanetTile> tmpNeighbors = new List<PlanetTile>();
	}
}
