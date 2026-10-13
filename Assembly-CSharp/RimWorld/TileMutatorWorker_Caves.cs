using System;
using System.Collections.Generic;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x020020DA RID: 8410
	public class TileMutatorWorker_Caves : TileMutatorWorker
	{
		// Token: 0x17001BEE RID: 7150
		// (get) Token: 0x0600B7C9 RID: 47049 RVA: 0x0034EE1A File Offset: 0x0034D01A
		protected virtual float BranchChance
		{
			get
			{
				return 0.02f;
			}
		}

		// Token: 0x17001BEF RID: 7151
		// (get) Token: 0x0600B7CA RID: 47050 RVA: 0x0034F3AE File Offset: 0x0034D5AE
		protected virtual float WidthOffsetPerCell
		{
			get
			{
				return 0.034f;
			}
		}

		// Token: 0x17001BF0 RID: 7152
		// (get) Token: 0x0600B7CB RID: 47051 RVA: 0x0034F3B5 File Offset: 0x0034D5B5
		protected virtual float MinTunnelWidth
		{
			get
			{
				return 1.4f;
			}
		}

		// Token: 0x0600B7CC RID: 47052 RVA: 0x0034CD7F File Offset: 0x0034AF7F
		public TileMutatorWorker_Caves(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x0600B7CD RID: 47053 RVA: 0x0034F3BC File Offset: 0x0034D5BC
		public override void Init(Map map)
		{
			this.directionNoise = new Perlin(0.002050000010058284, 2.0, 0.5, 4, Rand.Int, QualityMode.Medium);
		}

		// Token: 0x0600B7CE RID: 47054 RVA: 0x0034F3EB File Offset: 0x0034D5EB
		public override void GeneratePostElevationFertility(Map map)
		{
			this.GenerateCaves(map);
		}

		// Token: 0x0600B7CF RID: 47055 RVA: 0x0034F3F4 File Offset: 0x0034D5F4
		protected void GenerateCaves(Map map)
		{
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			BoolGrid visited = new BoolGrid(map);
			List<IntVec3> group = new List<IntVec3>();
			MapGenCavesUtility.CaveGenParms @default = MapGenCavesUtility.CaveGenParms.Default;
			@default.widthOffsetPerCell = this.WidthOffsetPerCell;
			@default.minTunnelWidth = this.MinTunnelWidth;
			@default.branchChance = this.BranchChance;
			@default.allowBranchingAfterThisManyCells = 10;
			MapGenCavesUtility.GenerateCaves(map, visited, group, this.directionNoise, @default, (IntVec3 cell) => this.ShouldCarve(cell, elevation, map));
		}

		// Token: 0x0600B7D0 RID: 47056 RVA: 0x0034F488 File Offset: 0x0034D688
		public override void GeneratePostTerrain(Map map)
		{
			Perlin perlin = new Perlin(0.07999999821186066, 2.0, 0.5, 6, Rand.Int, QualityMode.Medium);
			Perlin perlin2 = new Perlin(0.1599999964237213, 2.0, 0.5, 6, Rand.Int, QualityMode.Medium);
			MapGenFloatGrid caves = MapGenerator.Caves;
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			foreach (IntVec3 intVec in map.AllCells)
			{
				if (caves[intVec] > 0f && elevation[intVec] > 0f && !intVec.GetTerrain(map).IsRiver)
				{
					float num = (float)perlin.GetValue((double)intVec.x, 0.0, (double)intVec.z);
					float num2 = (float)perlin2.GetValue((double)intVec.x, 0.0, (double)intVec.z);
					if (num > 0.93f)
					{
						map.terrainGrid.SetTerrain(intVec, MapGenUtility.ShallowFreshWaterTerrainAt(intVec, map));
					}
					else if (num2 > 0.55f)
					{
						map.terrainGrid.SetTerrain(intVec, TerrainDefOf.Gravel);
					}
				}
			}
		}

		// Token: 0x0600B7D1 RID: 47057 RVA: 0x0034F5E4 File Offset: 0x0034D7E4
		protected virtual bool ShouldCarve(IntVec3 c, MapGenFloatGrid elevation, Map map)
		{
			return TileMutatorWorker_Caves.IsRock(c, elevation, map);
		}

		// Token: 0x0600B7D2 RID: 47058 RVA: 0x0034F5EE File Offset: 0x0034D7EE
		public static bool IsRock(IntVec3 c, MapGenFloatGrid elevation, Map map)
		{
			return c.InBounds(map) && elevation[c] > 0.7f;
		}

		// Token: 0x04007E59 RID: 32345
		protected ModuleBase directionNoise;

		// Token: 0x04007E5A RID: 32346
		public const float DirectionNoiseFrequency = 0.00205f;

		// Token: 0x04007E5B RID: 32347
		private const int AllowBranchingAfterThisManyCells = 10;

		// Token: 0x04007E5C RID: 32348
		private const float WaterFrequency = 0.08f;

		// Token: 0x04007E5D RID: 32349
		private const float GravelFrequency = 0.16f;

		// Token: 0x04007E5E RID: 32350
		private const float WaterThreshold = 0.93f;

		// Token: 0x04007E5F RID: 32351
		private const float GravelThreshold = 0.55f;
	}
}
