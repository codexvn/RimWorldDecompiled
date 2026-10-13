using System;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x020020E1 RID: 8417
	public class TileMutatorWorker_CoastalIsland : TileMutatorWorker_Coast
	{
		// Token: 0x17001BF7 RID: 7159
		// (get) Token: 0x0600B7F1 RID: 47089 RVA: 0x0034FE2F File Offset: 0x0034E02F
		protected override FloatRange CoastOffset
		{
			get
			{
				return new FloatRange(0.6f);
			}
		}

		// Token: 0x0600B7F2 RID: 47090 RVA: 0x0034E49C File Offset: 0x0034C69C
		public TileMutatorWorker_CoastalIsland(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x0600B7F3 RID: 47091 RVA: 0x0035014C File Offset: 0x0034E34C
		public override void Init(Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			base.Init(map);
			float randomInRange = TileMutatorWorker_CoastalIsland.IslandSquashRange.RandomInRange;
			float num = Rand.Range(0f, 360f);
			ModuleBase moduleBase = new DistFromPoint((float)map.Size.x * 0.95f);
			moduleBase = new Clamp((double)this.MaxForDeepWater, 1.0, moduleBase);
			moduleBase = new Scale((double)randomInRange, 1.0, 1.0, moduleBase);
			moduleBase = new Rotate(0.0, (double)num, 0.0, moduleBase);
			moduleBase = new Translate((double)((float)(-(float)map.Size.x) / 2f), 0.0, (double)((float)(-(float)map.Size.z) / 2f), moduleBase);
			moduleBase = MapNoiseUtility.AddDisplacementNoise(moduleBase, 0.015f, 35f, 4, map.Tile.tileId);
			ModuleBase moduleBase2 = new DistFromPoint((float)map.Size.x * 0.65f);
			moduleBase2 = new ScaleBias(-1.0, 1.0, moduleBase2);
			moduleBase2 = new Scale((double)randomInRange, 1.0, 1.0, moduleBase2);
			moduleBase2 = new Rotate(0.0, (double)num, 0.0, moduleBase2);
			moduleBase2 = new Translate((double)((float)(-(float)map.Size.x) / 2f), 0.0, (double)((float)(-(float)map.Size.z) / 2f), moduleBase2);
			moduleBase2 = MapNoiseUtility.AddDisplacementNoise(moduleBase2, 0.015f, 35f, 4, map.Tile.tileId);
			this.coastNoise = new SmoothMin(this.coastNoise, moduleBase, 0.25);
			this.coastNoise = new Max(this.coastNoise, moduleBase2);
			NoiseDebugUI.StoreNoiseRender(this.coastNoise, "island & coast");
		}

		// Token: 0x04007E88 RID: 32392
		private const float IslandRadius = 0.65f;

		// Token: 0x04007E89 RID: 32393
		private const float WaterRadius = 0.95f;

		// Token: 0x04007E8A RID: 32394
		private const float IslandNoiseFrequency = 0.015f;

		// Token: 0x04007E8B RID: 32395
		private const float IslandNoiseStrength = 35f;

		// Token: 0x04007E8C RID: 32396
		private const float WaterSmoothMinStrength = 0.25f;

		// Token: 0x04007E8D RID: 32397
		private static readonly FloatRange IslandSquashRange = new FloatRange(0.65f, 1f);
	}
}
