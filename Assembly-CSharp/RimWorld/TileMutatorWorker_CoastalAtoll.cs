using System;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x020020E0 RID: 8416
	public class TileMutatorWorker_CoastalAtoll : TileMutatorWorker_Coast
	{
		// Token: 0x17001BF5 RID: 7157
		// (get) Token: 0x0600B7EC RID: 47084 RVA: 0x0034FE2F File Offset: 0x0034E02F
		protected override FloatRange CoastOffset
		{
			get
			{
				return new FloatRange(0.6f);
			}
		}

		// Token: 0x17001BF6 RID: 7158
		// (get) Token: 0x0600B7ED RID: 47085 RVA: 0x0034FE3B File Offset: 0x0034E03B
		protected override float MaxForSand
		{
			get
			{
				return 0.53f;
			}
		}

		// Token: 0x0600B7EE RID: 47086 RVA: 0x0034E49C File Offset: 0x0034C69C
		public TileMutatorWorker_CoastalAtoll(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x0600B7EF RID: 47087 RVA: 0x0034FE44 File Offset: 0x0034E044
		public override void Init(Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			base.Init(map);
			ModuleBase moduleBase = new DistFromPoint((float)map.Size.x * 0.95f);
			moduleBase = new Clamp((double)this.MaxForDeepWater, 1.0, moduleBase);
			moduleBase = new Scale(1.0, 1.0, 0.800000011920929, moduleBase);
			moduleBase = new Rotate(0.0, (double)this.GetCoastAngle(map.Tile), 0.0, moduleBase);
			moduleBase = new Translate((double)((float)(-(float)map.Size.x) / 2f), 0.0, (double)((float)(-(float)map.Size.z) / 2f), moduleBase);
			moduleBase = MapNoiseUtility.AddDisplacementNoise(moduleBase, 0.015f, 25f, 4, -1);
			ModuleBase moduleBase2 = new DistFromPoint((float)map.Size.x * 0.65f);
			moduleBase2 = new ScaleBias(-1.0, 1.0, moduleBase2);
			moduleBase2 = new Translate((double)((float)(-(float)map.Size.x) / 2f), 0.0, (double)((float)(-(float)map.Size.z) / 2f), moduleBase2);
			moduleBase2 = MapNoiseUtility.AddDisplacementNoise(moduleBase2, 0.003f, 20f, 4, map.Tile.tileId);
			moduleBase2 = MapNoiseUtility.AddDisplacementNoise(moduleBase2, 0.015f, 5f, 6, map.Tile.tileId);
			ModuleBase moduleBase3 = new DistFromPoint((float)map.Size.x * 0.5f);
			moduleBase3 = new Clamp((double)this.MaxForDeepWater, 1.0, moduleBase3);
			moduleBase3 = new Translate((double)((float)(-(float)map.Size.x) * (0.5f + Rand.Range(-0.08f, 0.08f))), 0.0, (double)((float)(-(float)map.Size.z) * (0.5f + Rand.Range(-0.08f, 0.08f))), moduleBase3);
			moduleBase3 = MapNoiseUtility.AddDisplacementNoise(moduleBase3, 0.003f, 20f, 4, map.Tile.tileId);
			moduleBase3 = MapNoiseUtility.AddDisplacementNoise(moduleBase3, 0.015f, 5f, 6, map.Tile.tileId);
			this.coastNoise = new SmoothMin(this.coastNoise, moduleBase, 0.5);
			this.coastNoise = new Max(this.coastNoise, moduleBase2);
			this.coastNoise = new Min(this.coastNoise, moduleBase3);
			NoiseDebugUI.StoreNoiseRender(this.coastNoise, "atoll & coast");
		}

		// Token: 0x0600B7F0 RID: 47088 RVA: 0x003500D8 File Offset: 0x0034E2D8
		public override void GeneratePostElevationFertility(Map map)
		{
			base.GeneratePostElevationFertility(map);
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			foreach (IntVec3 intVec in map.AllCells)
			{
				if (this.coastNoise.GetValue(intVec) < this.MaxForSand)
				{
					elevation[intVec] = 0f;
				}
			}
		}

		// Token: 0x04007E7E RID: 32382
		private ModuleBase innerWaterNoise;

		// Token: 0x04007E7F RID: 32383
		private const float WaterRadius = 0.95f;

		// Token: 0x04007E80 RID: 32384
		private const float WaterSquash = 0.8f;

		// Token: 0x04007E81 RID: 32385
		private const float IslandRadius = 0.65f;

		// Token: 0x04007E82 RID: 32386
		private const float IslandInnerRadius = 0.5f;

		// Token: 0x04007E83 RID: 32387
		private const float IslandMacroNoiseFrequency = 0.003f;

		// Token: 0x04007E84 RID: 32388
		private const float IslandMacroNoiseStrength = 20f;

		// Token: 0x04007E85 RID: 32389
		private const float IslandNoiseFrequency = 0.015f;

		// Token: 0x04007E86 RID: 32390
		private const float IslandNoiseStrength = 5f;

		// Token: 0x04007E87 RID: 32391
		private const float IslandMaxInnerOffset = 0.08f;
	}
}
