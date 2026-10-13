using System;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x020020DE RID: 8414
	public class TileMutatorWorker_Cliffs : TileMutatorWorker
	{
		// Token: 0x0600B7DB RID: 47067 RVA: 0x0034CD7F File Offset: 0x0034AF7F
		public TileMutatorWorker_Cliffs(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x0600B7DC RID: 47068 RVA: 0x0034F9F0 File Offset: 0x0034DBF0
		public override void Init(Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			base.Init(map);
			float? num = Find.World.CoastAngleAt(map.Tile, BiomeDefOf.Ocean);
			float num2 = num ?? ((float)Rand.Range(0, 360));
			this.cliffNoise = new DistFromPoint((float)map.Size.x * 0.5f);
			this.cliffNoise = new Scale(1.0, 1.0, 0.5, this.cliffNoise);
			this.cliffNoise = new Translate(0.0, 0.0, (double)((float)(-(float)map.Size.x) * 0.5f), this.cliffNoise);
			this.cliffNoise = new Rotate(0.0, (double)(num2 + 90f), 0.0, this.cliffNoise);
			this.cliffNoise = new Translate((double)(-(double)map.Center.x), 0.0, (double)(-(double)map.Center.z), this.cliffNoise);
			if (num != null)
			{
				ModuleBase rhs = MapNoiseUtility.FalloffAtAngle(num.Value, 0.1f, map);
				this.cliffNoise = new SmoothMin(this.cliffNoise, rhs, 0.1);
			}
			this.cliffNoise = MapNoiseUtility.AddDisplacementNoise(this.cliffNoise, 0.015f, 40f, 4, -1);
			NoiseDebugUI.StoreNoiseRender(this.cliffNoise, "cliff");
		}

		// Token: 0x0600B7DD RID: 47069 RVA: 0x0034FB88 File Offset: 0x0034DD88
		public override void GeneratePostElevationFertility(Map map)
		{
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			foreach (IntVec3 intVec in map.AllCells)
			{
				float value = this.cliffNoise.GetValue(intVec);
				if (value > 0.75f)
				{
					elevation[intVec] = value;
				}
			}
		}

		// Token: 0x04007E71 RID: 32369
		private ModuleBase cliffNoise;

		// Token: 0x04007E72 RID: 32370
		private const float CliffSpan = 0.5f;

		// Token: 0x04007E73 RID: 32371
		private const float CliffOffset = 0.5f;

		// Token: 0x04007E74 RID: 32372
		private const float CliffSquash = 0.5f;

		// Token: 0x04007E75 RID: 32373
		private const float CliffNoiseFreq = 0.015f;

		// Token: 0x04007E76 RID: 32374
		private const float CliffNoiseStrength = 40f;

		// Token: 0x04007E77 RID: 32375
		private const float CliffThreshold = 0.75f;

		// Token: 0x04007E78 RID: 32376
		private const float CoastFalloff = 0.1f;
	}
}
