using System;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x020020F5 RID: 8437
	public class TileMutatorWorker_Lake : TileMutatorWorker
	{
		// Token: 0x17001BF9 RID: 7161
		// (get) Token: 0x0600B838 RID: 47160 RVA: 0x0034FBFB File Offset: 0x0034DDFB
		protected virtual float LakeRadius
		{
			get
			{
				return 0.6f;
			}
		}

		// Token: 0x17001BFA RID: 7162
		// (get) Token: 0x0600B839 RID: 47161 RVA: 0x0004FC72 File Offset: 0x0004DE72
		protected virtual float LakeFalloffDecay
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x17001BFB RID: 7163
		// (get) Token: 0x0600B83A RID: 47162 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected virtual bool GenerateDeepWater
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600B83B RID: 47163 RVA: 0x0034CD7F File Offset: 0x0034AF7F
		public TileMutatorWorker_Lake(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x0600B83C RID: 47164 RVA: 0x003522F4 File Offset: 0x003504F4
		public override void Init(Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			map.waterInfo.lakeCenter = this.GetLakeCenter(map);
			this.lakeNoise = new DistFromPoint((float)map.Size.x * this.LakeRadius);
			this.lakeNoise = new ScaleBias(-1.0, 1.0, this.lakeNoise);
			this.lakeNoise = new Scale((double)TileMutatorWorker_Lake.LakeSquashRange.RandomInRange, 1.0, 1.0, this.lakeNoise);
			this.lakeNoise = new Rotate(0.0, (double)Rand.Range(0f, 360f), 0.0, this.lakeNoise);
			this.lakeNoise = new Translate((double)(-(double)map.waterInfo.lakeCenter.x), 0.0, (double)(-(double)map.waterInfo.lakeCenter.z), this.lakeNoise);
			this.lakeNoise = new Clamp(0.0, 1.0, this.lakeNoise);
			this.lakeNoise = new Power(this.lakeNoise, new Const((double)this.LakeFalloffDecay));
			this.lakeNoise = MapNoiseUtility.AddDisplacementNoise(this.lakeNoise, 0.006f, 40f, 2, map.Tile.tileId);
			this.lakeNoise = MapNoiseUtility.AddDisplacementNoise(this.lakeNoise, 0.015f, 15f, 4, map.Tile.tileId);
			NoiseDebugUI.StoreNoiseRender(this.lakeNoise, "lake");
		}

		// Token: 0x0600B83D RID: 47165 RVA: 0x0035249C File Offset: 0x0035069C
		protected virtual IntVec3 GetLakeCenter(Map map)
		{
			Landmark landmark;
			if (Find.World.landmarks.landmarks.TryGetValue(map.Tile, out landmark) && landmark.isComboLandmark)
			{
				return map.Center;
			}
			Rand.PushState(map.Tile.tileId);
			IntVec3 result = new IntVec3(map.Center.x + Mathf.RoundToInt((float)map.Size.x * Rand.Range(-0.4f, 0.4f)), 0, map.Center.z + Mathf.RoundToInt((float)map.Size.z * Rand.Range(-0.4f, 0.4f)));
			Rand.PopState();
			return result;
		}

		// Token: 0x0600B83E RID: 47166 RVA: 0x0035254C File Offset: 0x0035074C
		public override void GeneratePostElevationFertility(Map map)
		{
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			foreach (IntVec3 intVec in map.AllCells)
			{
				if (this.GetValAt(intVec, map) > 0.45f)
				{
					elevation[intVec] = 0f;
				}
			}
		}

		// Token: 0x0600B83F RID: 47167 RVA: 0x003525B4 File Offset: 0x003507B4
		public override void GeneratePostTerrain(Map map)
		{
			foreach (IntVec3 cell in map.AllCells)
			{
				this.ProcessCell(cell, map);
			}
		}

		// Token: 0x0600B840 RID: 47168 RVA: 0x00352604 File Offset: 0x00350804
		protected virtual void ProcessCell(IntVec3 cell, Map map)
		{
			float valAt = this.GetValAt(cell, map);
			if (this.GenerateDeepWater && valAt > 0.75f)
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

		// Token: 0x0600B841 RID: 47169 RVA: 0x0035267E File Offset: 0x0035087E
		protected virtual float GetValAt(IntVec3 cell, Map map)
		{
			return (float)this.lakeNoise.GetValue((double)cell.x, 0.0, (double)cell.z);
		}

		// Token: 0x04007EF1 RID: 32497
		protected ModuleBase lakeNoise;

		// Token: 0x04007EF2 RID: 32498
		public const float LakeNoiseFrequency = 0.015f;

		// Token: 0x04007EF3 RID: 32499
		public const float LakeNoiseStrength = 15f;

		// Token: 0x04007EF4 RID: 32500
		public const float LakeMacroNoiseFrequency = 0.006f;

		// Token: 0x04007EF5 RID: 32501
		public const float LakeMacroNoiseStrength = 40f;

		// Token: 0x04007EF6 RID: 32502
		public const float DeepWaterThreshold = 0.75f;

		// Token: 0x04007EF7 RID: 32503
		public const float WaterThreshold = 0.5f;

		// Token: 0x04007EF8 RID: 32504
		public const float BeachThreshold = 0.45f;

		// Token: 0x04007EF9 RID: 32505
		private const float LakeMaxOffset = 0.4f;

		// Token: 0x04007EFA RID: 32506
		public static readonly FloatRange LakeSquashRange = new FloatRange(1f, 1.3f);
	}
}
