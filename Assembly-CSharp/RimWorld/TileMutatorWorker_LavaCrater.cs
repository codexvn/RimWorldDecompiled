using System;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x02002102 RID: 8450
	public class TileMutatorWorker_LavaCrater : TileMutatorWorker_LavaLake
	{
		// Token: 0x0600B871 RID: 47217 RVA: 0x0035328D File Offset: 0x0035148D
		public TileMutatorWorker_LavaCrater(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x0600B872 RID: 47218 RVA: 0x00353298 File Offset: 0x00351498
		public override void Init(Map map)
		{
			base.Init(map);
			ModuleBase lhs = new Perlin(0.014999999664723873, 2.0, 0.5, 6, Rand.Int, QualityMode.Medium);
			ModuleBase moduleBase = new DistFromPoint((float)map.Size.x * this.LakeRadius);
			moduleBase = new Clamp(0.0, 1.0, moduleBase);
			moduleBase = new Invert(moduleBase);
			moduleBase = new ScaleBias(1.0, 1.0, moduleBase);
			moduleBase = new Translate((double)(-(double)map.waterInfo.lakeCenter.x), 0.0, (double)(-(double)map.waterInfo.lakeCenter.z), moduleBase);
			this.craterNoise = new Blend(lhs, moduleBase, new Const(0.7));
			NoiseDebugUI.StoreNoiseRender(this.craterNoise, "crater");
		}

		// Token: 0x0600B873 RID: 47219 RVA: 0x00353388 File Offset: 0x00351588
		public override void GeneratePostElevationFertility(Map map)
		{
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			foreach (IntVec3 intVec in map.AllCells)
			{
				if ((float)this.craterNoise.GetValue((double)intVec.x, 0.0, (double)intVec.z) > 0.15f)
				{
					elevation[intVec] = 1f;
				}
			}
			base.GeneratePostElevationFertility(map);
		}

		// Token: 0x04007F21 RID: 32545
		private const float RockThreshold = 0.15f;

		// Token: 0x04007F22 RID: 32546
		private const float NoiseFrequency = 0.015f;

		// Token: 0x04007F23 RID: 32547
		private ModuleBase craterNoise;
	}
}
