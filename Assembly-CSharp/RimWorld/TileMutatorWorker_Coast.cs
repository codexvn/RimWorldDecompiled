using System;
using RimWorld.Planet;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x020020DF RID: 8415
	public class TileMutatorWorker_Coast : TileMutatorWorker
	{
		// Token: 0x17001BF1 RID: 7153
		// (get) Token: 0x0600B7DE RID: 47070 RVA: 0x0034FBF4 File Offset: 0x0034DDF4
		protected virtual float MaxForDeepWater
		{
			get
			{
				return 0.4f;
			}
		}

		// Token: 0x17001BF2 RID: 7154
		// (get) Token: 0x0600B7DF RID: 47071 RVA: 0x0031CA56 File Offset: 0x0031AC56
		protected virtual float MaxForShallowWater
		{
			get
			{
				return 0.5f;
			}
		}

		// Token: 0x17001BF3 RID: 7155
		// (get) Token: 0x0600B7E0 RID: 47072 RVA: 0x0034FBFB File Offset: 0x0034DDFB
		protected virtual float MaxForSand
		{
			get
			{
				return 0.6f;
			}
		}

		// Token: 0x17001BF4 RID: 7156
		// (get) Token: 0x0600B7E1 RID: 47073 RVA: 0x0034FC02 File Offset: 0x0034DE02
		protected virtual FloatRange CoastOffset
		{
			get
			{
				return new FloatRange(0.1f, 0.2f);
			}
		}

		// Token: 0x0600B7E2 RID: 47074 RVA: 0x0034CD7F File Offset: 0x0034AF7F
		public TileMutatorWorker_Coast(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x0600B7E3 RID: 47075 RVA: 0x0034FC14 File Offset: 0x0034DE14
		public override void Init(Map map)
		{
			float coastAngle = this.GetCoastAngle(map.Tile);
			this.coastNoise = MapNoiseUtility.FalloffAtAngle(coastAngle, this.CoastOffset.RandomInRange, map);
			this.coastNoise = MapNoiseUtility.AddDisplacementNoise(this.coastNoise, 0.006f, 30f, 2, -1);
			this.coastNoise = MapNoiseUtility.AddDisplacementNoise(this.coastNoise, 0.015f, 25f, 4, -1);
			NoiseDebugUI.StoreNoiseRender(this.coastNoise, "coast");
		}

		// Token: 0x0600B7E4 RID: 47076 RVA: 0x0034FC94 File Offset: 0x0034DE94
		protected virtual float GetCoastAngle(PlanetTile tile)
		{
			return Find.World.CoastAngleAt(tile, BiomeDefOf.Ocean).GetValueOrDefault();
		}

		// Token: 0x0600B7E5 RID: 47077 RVA: 0x0034FCBC File Offset: 0x0034DEBC
		public override void GeneratePostElevationFertility(Map map)
		{
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			foreach (IntVec3 intVec in map.AllCells)
			{
				if (this.GetNoiseValue(intVec) < this.MaxForDeepWater)
				{
					elevation[intVec] = 0f;
				}
			}
		}

		// Token: 0x0600B7E6 RID: 47078 RVA: 0x0034FD24 File Offset: 0x0034DF24
		public override void GeneratePostTerrain(Map map)
		{
			foreach (IntVec3 intVec in map.AllCells)
			{
				TerrainDef terrainDef = this.CoastTerrainAt(intVec, map);
				if (terrainDef != null && (intVec.GetTerrain(map).categoryType != TerrainDef.TerrainCategoryType.Stone || (intVec.GetEdifice(map) == null && terrainDef.IsWater)))
				{
					map.terrainGrid.SetTerrain(intVec, terrainDef);
				}
			}
		}

		// Token: 0x0600B7E7 RID: 47079 RVA: 0x0034FDA4 File Offset: 0x0034DFA4
		protected virtual float GetNoiseValue(IntVec3 cell)
		{
			return this.coastNoise.GetValue(cell);
		}

		// Token: 0x0600B7E8 RID: 47080 RVA: 0x0034FDB2 File Offset: 0x0034DFB2
		protected virtual TerrainDef DeepWaterTerrainAt(IntVec3 cell, Map map)
		{
			return MapGenUtility.DeepOceanWaterTerrainAt(cell, map);
		}

		// Token: 0x0600B7E9 RID: 47081 RVA: 0x0034FDBB File Offset: 0x0034DFBB
		protected virtual TerrainDef ShallowWaterTerrainAt(IntVec3 cell, Map map)
		{
			return MapGenUtility.ShallowOceanWaterTerrainAt(cell, map);
		}

		// Token: 0x0600B7EA RID: 47082 RVA: 0x0034FDC4 File Offset: 0x0034DFC4
		protected virtual TerrainDef BeachTerrainAt(IntVec3 cell, Map map)
		{
			return MapGenUtility.BeachTerrainAt(cell, map);
		}

		// Token: 0x0600B7EB RID: 47083 RVA: 0x0034FDD0 File Offset: 0x0034DFD0
		protected virtual TerrainDef CoastTerrainAt(IntVec3 cell, Map map)
		{
			if (this.coastNoise == null)
			{
				return null;
			}
			float noiseValue = this.GetNoiseValue(cell);
			if (noiseValue < this.MaxForDeepWater)
			{
				return this.DeepWaterTerrainAt(cell, map);
			}
			if (noiseValue < this.MaxForShallowWater)
			{
				return this.ShallowWaterTerrainAt(cell, map);
			}
			if (noiseValue < this.MaxForSand && MapGenUtility.ShouldGenerateBeachSand(cell, map))
			{
				return this.BeachTerrainAt(cell, map);
			}
			return null;
		}

		// Token: 0x04007E79 RID: 32377
		protected ModuleBase coastNoise;

		// Token: 0x04007E7A RID: 32378
		protected const float CoastMacroNoiseFrequency = 0.006f;

		// Token: 0x04007E7B RID: 32379
		protected const float CoastMacroNoiseStrength = 30f;

		// Token: 0x04007E7C RID: 32380
		protected const float CoastNoiseFrequency = 0.015f;

		// Token: 0x04007E7D RID: 32381
		protected const float CoastNoiseStrength = 25f;
	}
}
