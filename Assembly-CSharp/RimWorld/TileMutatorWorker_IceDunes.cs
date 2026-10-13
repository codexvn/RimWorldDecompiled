using System;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x020020EE RID: 8430
	public class TileMutatorWorker_IceDunes : TileMutatorWorker
	{
		// Token: 0x0600B823 RID: 47139 RVA: 0x0034CD7F File Offset: 0x0034AF7F
		public TileMutatorWorker_IceDunes(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x0600B824 RID: 47140 RVA: 0x00351DD4 File Offset: 0x0034FFD4
		public override void Init(Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			this.iceDunesNoise = new Voronoi2D(0.007000000216066837, Rand.Int, 0.5f, true);
			this.iceDunesNoise = MapNoiseUtility.AddDisplacementNoise(this.iceDunesNoise, 0.03f, 10f, 3, -1);
			this.iceDunesNoise = new Scale(3.0, 1.0, 1.0, this.iceDunesNoise);
			ModuleBase moduleBase = new Perlin(0.019999999552965164, 2.0, 0.5, 2, Rand.Int, QualityMode.Medium);
			moduleBase = new Multiply(moduleBase, new Const(6.5));
			this.iceDunesNoise = new Displace(this.iceDunesNoise, moduleBase, new Const(0.0), new Const(0.0));
			float num = Rand.Range(0f, 180f);
			this.iceDunesNoise = new Rotate(0.0, (double)num, 0.0, this.iceDunesNoise);
		}

		// Token: 0x0600B825 RID: 47141 RVA: 0x00351EF4 File Offset: 0x003500F4
		public override void GeneratePostTerrain(Map map)
		{
			foreach (IntVec3 intVec in map.AllCells)
			{
				if (this.iceDunesNoise.GetValue(intVec) <= 0.2f && !intVec.GetTerrain(map).IsWater)
				{
					GenSpawn.Spawn(ThingDefOf.SolidIce, intVec, map, WipeMode.Vanish);
				}
			}
		}

		// Token: 0x04007EE1 RID: 32481
		private ModuleBase iceDunesNoise;

		// Token: 0x04007EE2 RID: 32482
		private const float VoronoiFrequency = 0.007f;

		// Token: 0x04007EE3 RID: 32483
		private const float VoronoiRandomness = 0.5f;

		// Token: 0x04007EE4 RID: 32484
		private const float NoiseFrequency = 0.03f;

		// Token: 0x04007EE5 RID: 32485
		private const float NoiseStrength = 10f;

		// Token: 0x04007EE6 RID: 32486
		private const float ScaleFactor = 3f;

		// Token: 0x04007EE7 RID: 32487
		private const float Threshold = 0.2f;

		// Token: 0x04007EE8 RID: 32488
		private const float DisplaceFrequency = 0.02f;

		// Token: 0x04007EE9 RID: 32489
		private const float DisplaceMagnitude = 6.5f;
	}
}
