using System;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x020020E4 RID: 8420
	public class TileMutatorWorker_Dunes : TileMutatorWorker
	{
		// Token: 0x0600B7FC RID: 47100 RVA: 0x0034CD7F File Offset: 0x0034AF7F
		public TileMutatorWorker_Dunes(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x0600B7FD RID: 47101 RVA: 0x0035082C File Offset: 0x0034EA2C
		public override void Init(Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			this.dunesNoise = new Voronoi2D(0.0024999999441206455, Rand.Int, 0.15f, 0.5f, true);
			this.dunesNoise = MapNoiseUtility.AddDisplacementNoise(this.dunesNoise, 0.015f, 20f, 2, -1);
			this.dunesNoise = new Scale(1.0, 1.0, 15.0, this.dunesNoise);
			this.dunesNoise = new Scale(1.0, 1.0, 1.0, this.dunesNoise);
			ModuleBase moduleBase = new Perlin(0.019999999552965164, 2.0, 0.5, 2, Rand.Int, QualityMode.Medium);
			moduleBase = new Multiply(moduleBase, new Const(7.5));
			this.dunesNoise = new Displace(this.dunesNoise, new Const(0.0), new Const(0.0), moduleBase);
			this.dunesNoise = new Rotate(0.0, (double)Rand.Range(0f, 180f), 0.0, this.dunesNoise);
		}

		// Token: 0x0600B7FE RID: 47102 RVA: 0x00350978 File Offset: 0x0034EB78
		public override void GeneratePostTerrain(Map map)
		{
			foreach (IntVec3 intVec in map.AllCells)
			{
				if (this.dunesNoise.GetValue(intVec) <= 0.2f && intVec.GetTerrain(map) == TerrainDefOf.Sand)
				{
					map.terrainGrid.SetTerrain(intVec, TerrainDefOf.SoftSand);
				}
			}
		}

		// Token: 0x04007E9E RID: 32414
		private ModuleBase dunesNoise;

		// Token: 0x04007E9F RID: 32415
		private const float VoronoiFrequency = 0.0025f;

		// Token: 0x04007EA0 RID: 32416
		private const float VoronoiRandomnessX = 0.15f;

		// Token: 0x04007EA1 RID: 32417
		private const float VoronoiRandomnessZ = 0.5f;

		// Token: 0x04007EA2 RID: 32418
		private const float NoiseFrequency = 0.015f;

		// Token: 0x04007EA3 RID: 32419
		private const float NoiseStrength = 20f;

		// Token: 0x04007EA4 RID: 32420
		private const int NoiseOctaves = 2;

		// Token: 0x04007EA5 RID: 32421
		private const float StretchFactor = 15f;

		// Token: 0x04007EA6 RID: 32422
		private const float ScaleFactor = 1f;

		// Token: 0x04007EA7 RID: 32423
		private const float Threshold = 0.2f;

		// Token: 0x04007EA8 RID: 32424
		private const float DisplaceFrequency = 0.02f;

		// Token: 0x04007EA9 RID: 32425
		private const float DisplaceMagnitude = 7.5f;
	}
}
