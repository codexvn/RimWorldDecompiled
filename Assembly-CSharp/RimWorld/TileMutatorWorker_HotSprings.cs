using System;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x020020EB RID: 8427
	public class TileMutatorWorker_HotSprings : TileMutatorWorker
	{
		// Token: 0x0600B816 RID: 47126 RVA: 0x0034CD7F File Offset: 0x0034AF7F
		public TileMutatorWorker_HotSprings(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x0600B817 RID: 47127 RVA: 0x0035182C File Offset: 0x0034FA2C
		public override void Init(Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			this.springNoise = new Perlin(0.05000000074505806, 1.5, 0.5, 4, Rand.Int, QualityMode.Medium);
			this.springNoise = new ScaleBias(0.5, 0.5, this.springNoise);
			ModuleBase moduleBase = MapNoiseUtility.CreateFalloffRadius((float)map.Size.x * 0.35f, map.Center.ToVector2(), 0.05f, true);
			moduleBase = MapNoiseUtility.AddDisplacementNoise(moduleBase, 0.015f, 25f, 1, -1);
			NoiseDebugUI.StoreNoiseRender(moduleBase, "hot springs area");
			this.springNoise = new Multiply(this.springNoise, moduleBase);
			NoiseDebugUI.StoreNoiseRender(this.springNoise, "hot springs");
		}

		// Token: 0x0600B818 RID: 47128 RVA: 0x00351900 File Offset: 0x0034FB00
		public override void GeneratePostElevationFertility(Map map)
		{
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			foreach (IntVec3 intVec in map.AllCells)
			{
				if (this.springNoise.GetValue(intVec) > 0.65f)
				{
					elevation[intVec] = 0f;
				}
			}
		}

		// Token: 0x0600B819 RID: 47129 RVA: 0x0035196C File Offset: 0x0034FB6C
		public override void GeneratePostTerrain(Map map)
		{
			foreach (IntVec3 intVec in map.AllCells)
			{
				float value = this.springNoise.GetValue(intVec);
				if (value > 0.85f)
				{
					map.terrainGrid.SetTerrain(intVec, TerrainDefOf.HotSpring);
				}
				else if (value > 0.65f)
				{
					map.terrainGrid.SetTerrain(intVec, GenStep_RocksFromGrid.RockDefAt(intVec).building.naturalTerrain);
				}
			}
		}

		// Token: 0x04007ECA RID: 32458
		private const float ShapeNoiseFrequency = 0.015f;

		// Token: 0x04007ECB RID: 32459
		private const float ShapeNoiseStrength = 25f;

		// Token: 0x04007ECC RID: 32460
		private const float NoiseFrequency = 0.05f;

		// Token: 0x04007ECD RID: 32461
		private const float NoiseLacunarity = 1.5f;

		// Token: 0x04007ECE RID: 32462
		private const float NoisePersistence = 0.5f;

		// Token: 0x04007ECF RID: 32463
		private const int NoiseOctaves = 4;

		// Token: 0x04007ED0 RID: 32464
		private const float PoolsRadius = 0.35f;

		// Token: 0x04007ED1 RID: 32465
		private const float SpringThreshold = 0.85f;

		// Token: 0x04007ED2 RID: 32466
		private const float StoneThreshold = 0.65f;

		// Token: 0x04007ED3 RID: 32467
		private const float FalloffExp = 0.05f;

		// Token: 0x04007ED4 RID: 32468
		private ModuleBase springNoise;
	}
}
