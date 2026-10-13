using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x020020DC RID: 8412
	public class TileMutatorWorker_Chasm : TileMutatorWorker
	{
		// Token: 0x0600B7D5 RID: 47061 RVA: 0x0034CD7F File Offset: 0x0034AF7F
		public TileMutatorWorker_Chasm(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x0600B7D6 RID: 47062 RVA: 0x0034F624 File Offset: 0x0034D824
		public override void Init(Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			this.chasmNoise = new DistFromPoint((float)map.Size.x * 0.4f);
			this.chasmNoise = new Scale((double)TileMutatorWorker_Chasm.OpeningSquashRange.RandomInRange, 1.0, 1.0, this.chasmNoise);
			this.chasmNoise = new Rotate(0.0, (double)Rand.Range(0f, 360f), 0.0, this.chasmNoise);
			this.chasmNoise = new Translate((double)(-(double)map.Center.x), 0.0, (double)(-(double)map.Center.z), this.chasmNoise);
			float? num = Find.World.CoastAngleAt(map.Tile, BiomeDefOf.Ocean);
			float? num2 = (num != null) ? num : Find.World.CoastAngleAt(map.Tile, BiomeDefOf.Lake);
			if (num2 != null)
			{
				ModuleBase rhs = MapNoiseUtility.FalloffAtAngle(num2.Value, 0.1f, map);
				this.chasmNoise = new SmoothMin(this.chasmNoise, rhs, 0.2);
			}
			NoiseDebugUI.StoreNoiseRender(this.chasmNoise, "opening shape");
			this.chasmNoise = MapNoiseUtility.AddDisplacementNoise(this.chasmNoise, 0.015f, 40f, 4, -1);
			NoiseDebugUI.StoreNoiseRender(this.chasmNoise, "opening");
			List<float> list = new List<float>();
			ModuleBase moduleBase = new Const(1.0);
			int randomInRange = TileMutatorWorker_Chasm.EntranceCountRange.RandomInRange;
			for (int i = 0; i < randomInRange; i++)
			{
				float entranceAngle = Rand.Range(0f, 360f);
				int num3 = 100;
				while (list.Any((float ua) => Mathf.Abs(Mathf.DeltaAngle(ua, entranceAngle)) < 40f) && num3-- > 0)
				{
					entranceAngle = Rand.Range(0f, 360f);
				}
				list.Add(entranceAngle);
				ModuleBase moduleBase2 = new DistFromAxis((float)map.Size.x * 0.06f);
				moduleBase2 = new ScaleBias(-1.0, 1.0, moduleBase2);
				moduleBase2 = new Multiply(moduleBase2, new CutOff(false, true));
				moduleBase2 = new ScaleBias(-1.0, 1.0, moduleBase2);
				moduleBase2 = new Rotate(0.0, (double)entranceAngle, 0.0, moduleBase2);
				moduleBase2 = new Translate((double)(-(double)map.Center.x), 0.0, (double)(-(double)map.Center.z), moduleBase2);
				moduleBase = new SmoothMin(moduleBase, moduleBase2, 0.2);
			}
			moduleBase = MapNoiseUtility.AddDisplacementNoise(moduleBase, 0.007f, 50f, 6, -1);
			NoiseDebugUI.StoreNoiseRender(moduleBase, "entrances");
			this.chasmNoise = new SmoothMin(this.chasmNoise, moduleBase, 0.2);
			NoiseDebugUI.StoreNoiseRender(this.chasmNoise, "chasm");
		}

		// Token: 0x0600B7D7 RID: 47063 RVA: 0x0034F948 File Offset: 0x0034DB48
		public override void GeneratePostElevationFertility(Map map)
		{
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			foreach (IntVec3 intVec in map.AllCells)
			{
				float value = this.chasmNoise.GetValue(intVec);
				if (value > 0.5f)
				{
					elevation[intVec] = value;
				}
			}
		}

		// Token: 0x04007E63 RID: 32355
		private const float OpeningNoiseFreq = 0.015f;

		// Token: 0x04007E64 RID: 32356
		private const float OpeningNoiseStrength = 40f;

		// Token: 0x04007E65 RID: 32357
		private const float OpeningRadius = 0.4f;

		// Token: 0x04007E66 RID: 32358
		private const float ChasmThreshold = 0.5f;

		// Token: 0x04007E67 RID: 32359
		private static readonly FloatRange OpeningSquashRange = new FloatRange(1f, 1.3f);

		// Token: 0x04007E68 RID: 32360
		private static readonly IntRange EntranceCountRange = new IntRange(3, 6);

		// Token: 0x04007E69 RID: 32361
		private const float EntranceSpan = 0.06f;

		// Token: 0x04007E6A RID: 32362
		private const float EntranceNoiseFreq = 0.007f;

		// Token: 0x04007E6B RID: 32363
		private const float EntranceNoiseStrength = 50f;

		// Token: 0x04007E6C RID: 32364
		private const int EntranceNoiseOctaves = 6;

		// Token: 0x04007E6D RID: 32365
		private const float MinAngleBetweenEntrances = 40f;

		// Token: 0x04007E6E RID: 32366
		private const float CoastFalloff = 0.1f;

		// Token: 0x04007E6F RID: 32367
		private ModuleBase chasmNoise;
	}
}
