using System;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x020020F8 RID: 8440
	public class TileMutatorWorker_LakeWithIslands : TileMutatorWorker_Lake
	{
		// Token: 0x17001C00 RID: 7168
		// (get) Token: 0x0600B84A RID: 47178 RVA: 0x00352795 File Offset: 0x00350995
		protected override float LakeRadius
		{
			get
			{
				return 0.75f;
			}
		}

		// Token: 0x17001C01 RID: 7169
		// (get) Token: 0x0600B84B RID: 47179 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool GenerateDeepWater
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600B84C RID: 47180 RVA: 0x0034E5E7 File Offset: 0x0034C7E7
		public TileMutatorWorker_LakeWithIslands(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x0600B84D RID: 47181 RVA: 0x0035279C File Offset: 0x0035099C
		public override void Init(Map map)
		{
			base.Init(map);
			ModuleBase moduleBase = new Perlin(0.019999999552965164, 2.0, 0.5, 2, Rand.Int, QualityMode.Medium);
			moduleBase = new ScaleBias(0.6000000238418579, 0.699999988079071, moduleBase);
			moduleBase = MapNoiseUtility.AddDisplacementNoise(moduleBase, 0.015f, 15f, 4, -1);
			NoiseDebugUI.StoreNoiseRender(moduleBase, "island base");
			ModuleBase moduleBase2 = new ScaleBias(2.0, -1.0, this.lakeNoise);
			moduleBase2 = new Clamp(0.0, 1.0, moduleBase2);
			moduleBase2 = new Power(moduleBase2, new Const(0.44999998807907104));
			NoiseDebugUI.StoreNoiseRender(moduleBase2, "island falloff");
			moduleBase = new Multiply(moduleBase, moduleBase2);
			moduleBase = new ScaleBias(-1.0, 1.0, moduleBase);
			NoiseDebugUI.StoreNoiseRender(moduleBase, "island Noise");
			this.lakeNoise = new Min(this.lakeNoise, moduleBase);
			NoiseDebugUI.StoreNoiseRender(this.lakeNoise, "lake + island Noise");
		}

		// Token: 0x04007F00 RID: 32512
		private const float IslandsPerlinFreq = 0.02f;

		// Token: 0x04007F01 RID: 32513
		private const float IslandsPerlinLac = 2f;

		// Token: 0x04007F02 RID: 32514
		private const float IslandsPerlinPers = 0.5f;

		// Token: 0x04007F03 RID: 32515
		private const int IslandsPerlinOctaves = 2;

		// Token: 0x04007F04 RID: 32516
		private const float IslandsScale = 0.6f;

		// Token: 0x04007F05 RID: 32517
		private const float IslandsBias = 0.7f;

		// Token: 0x04007F06 RID: 32518
		private const float FalloffScale = 2f;

		// Token: 0x04007F07 RID: 32519
		private const float FalloffBias = -1f;

		// Token: 0x04007F08 RID: 32520
		private const float FalloffExp = 0.45f;
	}
}
