using System;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x020020EC RID: 8428
	public class TileMutatorWorker_Iceberg : TileMutatorWorker_Coast
	{
		// Token: 0x17001BF8 RID: 7160
		// (get) Token: 0x0600B81A RID: 47130 RVA: 0x0034FE2F File Offset: 0x0034E02F
		protected override FloatRange CoastOffset
		{
			get
			{
				return new FloatRange(0.6f);
			}
		}

		// Token: 0x0600B81B RID: 47131 RVA: 0x0034E49C File Offset: 0x0034C69C
		public TileMutatorWorker_Iceberg(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x0600B81C RID: 47132 RVA: 0x00351A00 File Offset: 0x0034FC00
		public override void Init(Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			base.Init(map);
			ModuleBase moduleBase = new DistFromPoint((float)map.Size.x * 0.95f);
			moduleBase = new Clamp((double)this.MaxForDeepWater, 1.0, moduleBase);
			moduleBase = new Scale(1.0, 1.0, 0.75, moduleBase);
			moduleBase = new Rotate(0.0, (double)this.GetCoastAngle(map.Tile), 0.0, moduleBase);
			moduleBase = new Translate((double)((float)(-(float)map.Size.x) / 2f), 0.0, (double)((float)(-(float)map.Size.z) / 2f), moduleBase);
			moduleBase = MapNoiseUtility.AddDisplacementNoise(moduleBase, 0.015f, 25f, 4, -1);
			this.coastNoise = new SmoothMin(this.coastNoise, moduleBase, 0.25);
			NoiseDebugUI.StoreNoiseRender(this.coastNoise, "coast with space for iceberg");
			this.icebergNoise = new DistFromPoint((float)map.Size.x * 0.25f);
			this.icebergNoise = new ScaleBias(-1.0, 1.0, this.icebergNoise);
			this.icebergNoise = new Scale((double)TileMutatorWorker_Iceberg.BergSquashRange.RandomInRange, 1.0, 1.0, this.icebergNoise);
			this.icebergNoise = new Rotate(0.0, (double)Rand.Range(0f, 360f), 0.0, this.icebergNoise);
			this.icebergNoise = new Translate((double)(-(double)map.Center.x), 0.0, (double)(-(double)map.Center.z), this.icebergNoise);
			this.icebergNoise = MapNoiseUtility.AddDisplacementNoise(this.icebergNoise, 0.015f, 20f, 4, -1);
			this.icebergNoise = new Clamp(0.0, 1.0, this.icebergNoise);
			this.icebergNoise = new Power(this.icebergNoise, new Const(0.4000000059604645));
			NoiseDebugUI.StoreNoiseRender(this.icebergNoise, "iceberg");
		}

		// Token: 0x0600B81D RID: 47133 RVA: 0x00351C50 File Offset: 0x0034FE50
		public override void GeneratePostTerrain(Map map)
		{
			base.GeneratePostTerrain(map);
			foreach (IntVec3 intVec in map.AllCells)
			{
				float value = this.icebergNoise.GetValue(intVec);
				if (value > 0.35f)
				{
					map.terrainGrid.SetTerrain(intVec, TerrainDefOf.Ice);
				}
				else if (value > 0f)
				{
					map.terrainGrid.SetTerrain(intVec, MapGenUtility.ShallowOceanWaterTerrainAt(intVec, map));
				}
				if (value > 0.55f)
				{
					GenSpawn.Spawn(ThingDefOf.SolidIce, intVec, map, WipeMode.Vanish);
				}
			}
		}

		// Token: 0x04007ED5 RID: 32469
		private ModuleBase icebergNoise;

		// Token: 0x04007ED6 RID: 32470
		private const float Radius = 0.25f;

		// Token: 0x04007ED7 RID: 32471
		private const float BergNoiseFrequency = 0.015f;

		// Token: 0x04007ED8 RID: 32472
		private const float BergNoiseStrength = 20f;

		// Token: 0x04007ED9 RID: 32473
		private const float Exponent = 0.4f;

		// Token: 0x04007EDA RID: 32474
		private const float BergDeepWaterThreshold = 0f;

		// Token: 0x04007EDB RID: 32475
		private const float BergThreshold = 0.35f;

		// Token: 0x04007EDC RID: 32476
		private const float BergWallThreshold = 0.55f;

		// Token: 0x04007EDD RID: 32477
		private const float WaterSmoothMinStrength = 0.25f;

		// Token: 0x04007EDE RID: 32478
		private const float WaterRadius = 0.95f;

		// Token: 0x04007EDF RID: 32479
		private const float WaterSquash = 0.75f;

		// Token: 0x04007EE0 RID: 32480
		private static readonly FloatRange BergSquashRange = new FloatRange(0.65f, 0.9f);
	}
}
