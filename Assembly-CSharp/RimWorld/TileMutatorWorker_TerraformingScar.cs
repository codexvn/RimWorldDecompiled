using System;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x02002121 RID: 8481
	public class TileMutatorWorker_TerraformingScar : TileMutatorWorker
	{
		// Token: 0x0600B8FF RID: 47359 RVA: 0x0034CD7F File Offset: 0x0034AF7F
		public TileMutatorWorker_TerraformingScar(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x0600B900 RID: 47360 RVA: 0x00356744 File Offset: 0x00354944
		public override void GeneratePostElevationFertility(Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			ModuleBase moduleBase = new Perlin((double)GenStep_ElevationFertility.ElevationFreqRange.RandomInRange, 2.0, 0.5, 3, Rand.Range(0, int.MaxValue), QualityMode.High);
			NoiseDebugUI.StoreNoiseRender(moduleBase, "elev shape");
			moduleBase = MapNoiseUtility.AddDisplacementNoise(moduleBase, GenStep_ElevationFertility.DetailFreqRange.RandomInRange, GenStep_ElevationFertility.DetailStrengthRange.RandomInRange, GenStep_ElevationFertility.DetailOctavesRange.RandomInRange, -1);
			NoiseDebugUI.StoreNoiseRender(moduleBase, "elev shape + detail");
			moduleBase = new Scale(2.0, 1.0, 1.0, moduleBase);
			moduleBase = new Rotate(0.0, (double)Rand.Range(0f, 180f), 0.0, moduleBase);
			moduleBase = MapNoiseUtility.AddDisplacementNoise(moduleBase, 0.005f, 75f, 4, -1);
			moduleBase = new Scale(1.0, 1.0, 1.0, moduleBase);
			moduleBase = new Rotate(0.0, (double)Rand.Range(0f, 180f), 0.0, moduleBase);
			NoiseDebugUI.StoreNoiseRender(moduleBase, "elev shape + detail + warp");
			moduleBase = new ScaleBias(0.5, 0.5, moduleBase);
			float elevationFactorMountains = MapGenTuning.ElevationFactorMountains;
			moduleBase = new Multiply(moduleBase, new Const((double)elevationFactorMountains));
			NoiseDebugUI.StoreNoiseRender(moduleBase, "elev world-factored");
			float b = map.TileInfo.WaterCovered ? 0f : float.MaxValue;
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			foreach (IntVec3 intVec in map.AllCells)
			{
				elevation[intVec] = Mathf.Min(moduleBase.GetValue(intVec), b);
			}
		}

		// Token: 0x04007F99 RID: 32665
		private const float WarpFreq = 0.005f;

		// Token: 0x04007F9A RID: 32666
		private const float WarpStrength = 75f;

		// Token: 0x04007F9B RID: 32667
		private const int WarpOctaves = 4;

		// Token: 0x04007F9C RID: 32668
		private const float PreTurbulenceStretch = 2f;

		// Token: 0x04007F9D RID: 32669
		private const float PostTurbulenceStretch = 1f;
	}
}
