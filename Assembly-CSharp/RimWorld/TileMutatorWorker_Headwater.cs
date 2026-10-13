using System;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x02002114 RID: 8468
	public class TileMutatorWorker_Headwater : TileMutatorWorker_River
	{
		// Token: 0x0600B8C3 RID: 47299 RVA: 0x003550D5 File Offset: 0x003532D5
		public TileMutatorWorker_Headwater(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x0600B8C4 RID: 47300 RVA: 0x0034CD58 File Offset: 0x0034AF58
		public override string GetLabel(PlanetTile tile)
		{
			return this.def.label;
		}

		// Token: 0x0600B8C5 RID: 47301 RVA: 0x003550E0 File Offset: 0x003532E0
		public override void Init(Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			base.Init(map);
			this.lakeNoise = new DistFromPoint((float)map.Size.x * 0.35f);
			this.lakeNoise = new ScaleBias(-1.0, 1.0, this.lakeNoise);
			this.lakeNoise = new Scale((double)TileMutatorWorker_Lake.LakeSquashRange.RandomInRange, 1.0, 1.0, this.lakeNoise);
			this.lakeNoise = new Rotate(0.0, (double)Rand.Range(0f, 360f), 0.0, this.lakeNoise);
			this.lakeNoise = new Translate((double)(-(double)map.Center.x), 0.0, (double)(-(double)map.Center.z), this.lakeNoise);
			this.lakeNoise = MapNoiseUtility.AddDisplacementNoise(this.lakeNoise, 0.006f, 15f, 2, -1);
			this.lakeNoise = MapNoiseUtility.AddDisplacementNoise(this.lakeNoise, 0.015f, 15f, 4, -1);
		}

		// Token: 0x0600B8C6 RID: 47302 RVA: 0x0034E5DF File Offset: 0x0034C7DF
		protected override IntVec3 GetRiverCenter(Map map)
		{
			return map.Center;
		}

		// Token: 0x0600B8C7 RID: 47303 RVA: 0x00355210 File Offset: 0x00353410
		protected override void GenerateRiverGraph(Map map)
		{
			SurfaceTile surfaceTile;
			if (map.TileInfo.Isnt(out surfaceTile))
			{
				return;
			}
			SurfaceTile.RiverLink riverLink = surfaceTile.Rivers.First<SurfaceTile.RiverLink>();
			float headingFromTo = Find.WorldGrid.GetHeadingFromTo(surfaceTile.tile, riverLink.neighbor.Tile.tile);
			ValueTuple<Vector3, Vector3> mapEdgeNodes = base.GetMapEdgeNodes(map, headingFromTo);
			Vector3 item = mapEdgeNodes.Item1;
			Vector3 item2 = mapEdgeNodes.Item2;
			if (base.IsFlowingAToB(item, item2, headingFromTo))
			{
				RiverNode item3 = new RiverNode
				{
					start = this.riverCenter.ToVector3Shifted(),
					end = item2,
					width = riverLink.river.widthOnMap
				};
				map.waterInfo.riverGraph.Add(item3);
				return;
			}
			RiverNode item4 = new RiverNode
			{
				start = this.riverCenter.ToVector3Shifted(),
				end = item,
				width = riverLink.river.widthOnMap
			};
			map.waterInfo.riverGraph.Add(item4);
		}

		// Token: 0x0600B8C8 RID: 47304 RVA: 0x00355304 File Offset: 0x00353504
		public override void GeneratePostElevationFertility(Map map)
		{
			base.GeneratePostElevationFertility(map);
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			foreach (IntVec3 intVec in map.AllCells)
			{
				if (this.GetValAt(intVec) > 0.45f)
				{
					elevation[intVec] = 0f;
				}
			}
		}

		// Token: 0x0600B8C9 RID: 47305 RVA: 0x00355374 File Offset: 0x00353574
		public override void GeneratePostTerrain(Map map)
		{
			base.GeneratePostTerrain(map);
			foreach (IntVec3 cell in map.AllCells)
			{
				this.ProcessCell(cell, map);
			}
		}

		// Token: 0x0600B8CA RID: 47306 RVA: 0x003553CC File Offset: 0x003535CC
		private void ProcessCell(IntVec3 cell, Map map)
		{
			float valAt = this.GetValAt(cell);
			if (valAt > 0.75f)
			{
				map.terrainGrid.SetTerrain(cell, MapGenUtility.DeepFreshWaterTerrainAt(cell, map));
				return;
			}
			if (valAt > 0.5f)
			{
				map.terrainGrid.SetTerrain(cell, MapGenUtility.ShallowFreshWaterTerrainAt(cell, map));
				return;
			}
			if (valAt > 0.45f && MapGenUtility.ShouldGenerateBeachSand(cell, map))
			{
				map.terrainGrid.SetTerrain(cell, MapGenUtility.LakeshoreTerrainAt(cell, map));
			}
		}

		// Token: 0x0600B8CB RID: 47307 RVA: 0x0035543D File Offset: 0x0035363D
		private float GetValAt(IntVec3 cell)
		{
			return (float)this.lakeNoise.GetValue((double)cell.x, 0.0, (double)cell.z);
		}

		// Token: 0x04007F66 RID: 32614
		private ModuleBase lakeNoise;

		// Token: 0x04007F67 RID: 32615
		private const float LakeRadius = 0.35f;

		// Token: 0x04007F68 RID: 32616
		private const float MacroNoiseStrength = 15f;
	}
}
