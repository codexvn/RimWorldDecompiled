using System;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x020020D5 RID: 8405
	public class TileMutatorWorker_Bay : TileMutatorWorker_Coast
	{
		// Token: 0x17001BEA RID: 7146
		// (get) Token: 0x0600B7B4 RID: 47028 RVA: 0x0034E970 File Offset: 0x0034CB70
		protected override FloatRange CoastOffset
		{
			get
			{
				return new FloatRange(0.2f, 0.3f);
			}
		}

		// Token: 0x0600B7B5 RID: 47029 RVA: 0x0034E49C File Offset: 0x0034C69C
		public TileMutatorWorker_Bay(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x0600B7B6 RID: 47030 RVA: 0x0034E984 File Offset: 0x0034CB84
		public override void Init(Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			base.Init(map);
			float coastAngle = this.GetCoastAngle(map.Tile);
			this.bayNoise = new DistFromPoint((float)map.Size.x * 0.6f);
			this.bayNoise = new Clamp((double)this.MaxForDeepWater, 1.0, this.bayNoise);
			this.bayNoise = new Scale(1.0, 1.0, 0.75, this.bayNoise);
			this.bayNoise = new Translate(0.0, 0.0, (double)((float)(-(float)map.Size.x) * 0.1f), this.bayNoise);
			this.bayNoise = new Rotate(0.0, (double)(coastAngle + 90f), 0.0, this.bayNoise);
			this.bayNoise = new Translate((double)(-(double)map.Center.x), 0.0, (double)(-(double)map.Center.z), this.bayNoise);
			NoiseDebugUI.StoreNoiseRender(this.bayNoise, "bay shape");
			this.bayNoise = MapNoiseUtility.AddDisplacementNoise(this.bayNoise, 0.003f, 70f, 2, -1);
			this.bayNoise = MapNoiseUtility.AddDisplacementNoise(this.bayNoise, 0.015f, 25f, 4, -1);
			NoiseDebugUI.StoreNoiseRender(this.bayNoise, "bay noise");
			this.coastNoise = new SmoothMin(this.bayNoise, this.coastNoise, 0.5);
			NoiseDebugUI.StoreNoiseRender(this.coastNoise, "bay & coast");
		}

		// Token: 0x0600B7B7 RID: 47031 RVA: 0x0034EB38 File Offset: 0x0034CD38
		public override void GeneratePostElevationFertility(Map map)
		{
			base.GeneratePostElevationFertility(map);
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			foreach (IntVec3 intVec in map.AllCells)
			{
				if (this.bayNoise.GetValue(intVec) < 0.4f)
				{
					elevation[intVec] = 0f;
				}
			}
		}

		// Token: 0x04007E36 RID: 32310
		private const float BaySpan = 0.6f;

		// Token: 0x04007E37 RID: 32311
		private const float BayOffset = 0.1f;

		// Token: 0x04007E38 RID: 32312
		private const float BaySquash = 0.75f;

		// Token: 0x04007E39 RID: 32313
		private const float BayMacroNoiseFrequency = 0.003f;

		// Token: 0x04007E3A RID: 32314
		private const float BayMacroNoiseStrength = 70f;

		// Token: 0x04007E3B RID: 32315
		private const float ElevationThreshold = 0.4f;

		// Token: 0x04007E3C RID: 32316
		private ModuleBase bayNoise;
	}
}
