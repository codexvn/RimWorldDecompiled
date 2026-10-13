using System;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x02002108 RID: 8456
	public class TileMutatorWorker_Mountain : TileMutatorWorker
	{
		// Token: 0x0600B88B RID: 47243 RVA: 0x0034CD7F File Offset: 0x0034AF7F
		public TileMutatorWorker_Mountain(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x0600B88C RID: 47244 RVA: 0x00353A5C File Offset: 0x00351C5C
		public override void GeneratePostElevationFertility(Map map)
		{
			float? num = Find.World.CoastAngleAt(map.Tile, BiomeDefOf.Ocean);
			float num2 = ((num != null) ? num : Find.World.CoastAngleAt(map.Tile, BiomeDefOf.Lake)) ?? ((float)Rand.Range(0, 360));
			ModuleBase moduleBase = new DistFromAxis_Directional((float)map.Size.x * 0.15f);
			moduleBase = new Translate((double)((float)(-(float)map.Size.x) * 0.2f), 0.0, 0.0, moduleBase);
			moduleBase = new ScaleBias(0.5, 0.5, moduleBase);
			moduleBase = new Clamp(0.0, 1.0, moduleBase);
			moduleBase = new Rotate(0.0, (double)num2, 0.0, moduleBase);
			moduleBase = new Translate((double)((float)(-(float)map.Size.x) / 2f), 0.0, (double)((float)(-(float)map.Size.z) / 2f), moduleBase);
			moduleBase = MapNoiseUtility.AddDisplacementNoise(moduleBase, 0.003f, 20f, 2, -1);
			NoiseDebugUI.StoreNoiseRender(moduleBase, "mountain");
			float b = map.TileInfo.WaterCovered ? 0f : float.MaxValue;
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			foreach (IntVec3 intVec in map.AllCells)
			{
				elevation[intVec] = Mathf.Min(elevation[intVec] + moduleBase.GetValue(intVec), b);
			}
		}

		// Token: 0x04007F32 RID: 32562
		private const float MountainNoiseFrequency = 0.003f;

		// Token: 0x04007F33 RID: 32563
		private const float MountainNoiseStrength = 20f;

		// Token: 0x04007F34 RID: 32564
		private const float EdgeMountainSpan = 0.15f;

		// Token: 0x04007F35 RID: 32565
		private const float EdgeMountainOffset = 0.2f;
	}
}
