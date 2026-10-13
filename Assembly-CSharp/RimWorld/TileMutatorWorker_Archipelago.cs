using System;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x020020D2 RID: 8402
	public class TileMutatorWorker_Archipelago : TileMutatorWorker_Coast
	{
		// Token: 0x17001BE7 RID: 7143
		// (get) Token: 0x0600B7A7 RID: 47015 RVA: 0x00129253 File Offset: 0x00127453
		protected override float MaxForDeepWater
		{
			get
			{
				return 0.2f;
			}
		}

		// Token: 0x17001BE8 RID: 7144
		// (get) Token: 0x0600B7A8 RID: 47016 RVA: 0x0034E490 File Offset: 0x0034C690
		protected override FloatRange CoastOffset
		{
			get
			{
				return new FloatRange(0.2f);
			}
		}

		// Token: 0x0600B7A9 RID: 47017 RVA: 0x0034E49C File Offset: 0x0034C69C
		public TileMutatorWorker_Archipelago(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x0600B7AA RID: 47018 RVA: 0x0034E4A8 File Offset: 0x0034C6A8
		public override void Init(Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			base.Init(map);
			this.archNoise = new Perlin(0.019999999552965164, 2.0, 0.5, 2, Rand.Int, QualityMode.Medium);
			this.archNoise = new ScaleBias(0.5, 0.5, this.archNoise);
			this.archNoise = MapNoiseUtility.AddDisplacementNoise(this.archNoise, 0.015f, 25f, 4, -1);
			NoiseDebugUI.StoreNoiseRender(this.archNoise, "archipelago");
			this.coastNoise = new SmoothMin(this.coastNoise, this.archNoise, 0.2);
		}

		// Token: 0x0600B7AB RID: 47019 RVA: 0x0034E564 File Offset: 0x0034C764
		public override void GeneratePostElevationFertility(Map map)
		{
			base.GeneratePostElevationFertility(map);
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			foreach (IntVec3 intVec in map.AllCells)
			{
				if (this.archNoise.GetValue(intVec) < this.MaxForSand)
				{
					elevation[intVec] = 0f;
				}
			}
		}

		// Token: 0x04007E27 RID: 32295
		private const float ArchNoiseFrequency = 0.02f;

		// Token: 0x04007E28 RID: 32296
		private ModuleBase archNoise;
	}
}
