using System;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x0200210F RID: 8463
	public class TileMutatorWorker_Plateau : TileMutatorWorker
	{
		// Token: 0x0600B8A4 RID: 47268 RVA: 0x0034CD7F File Offset: 0x0034AF7F
		public TileMutatorWorker_Plateau(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x0600B8A5 RID: 47269 RVA: 0x00354240 File Offset: 0x00352440
		public override void Init(Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			Vector2 offset = map.Center.ToVector2() + new Vector2((float)map.Size.x * TileMutatorWorker_Plateau.CenterOffsetRange.RandomInRange, (float)map.Size.z * TileMutatorWorker_Plateau.CenterOffsetRange.RandomInRange);
			float? num = Find.World.CoastAngleAt(map.Tile, BiomeDefOf.Ocean);
			float? num2 = (num != null) ? num : Find.World.CoastAngleAt(map.Tile, BiomeDefOf.Lake);
			if (num2 != null)
			{
				offset = map.Center.ToVector2() + Vector2Utility.FromAngle(num2.Value) * (float)map.Size.x * TileMutatorWorker_Plateau.CenterOffsetRange.max;
			}
			this.plateauNoise = MapNoiseUtility.CreateFalloffRadius((float)map.Size.x * 0.4f, offset, 0.2f, true);
			this.plateauNoise = MapNoiseUtility.AddDisplacementNoise(this.plateauNoise, 0.015f, 35f, 4, -1);
			NoiseDebugUI.StoreNoiseRender(this.plateauNoise, "plateau");
		}

		// Token: 0x0600B8A6 RID: 47270 RVA: 0x00354378 File Offset: 0x00352578
		public override void GeneratePostElevationFertility(Map map)
		{
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			foreach (IntVec3 intVec in map.AllCells)
			{
				float value = this.plateauNoise.GetValue(intVec);
				if (value > 0f)
				{
					elevation[intVec] = value;
				}
			}
		}

		// Token: 0x04007F44 RID: 32580
		private ModuleBase plateauNoise;

		// Token: 0x04007F45 RID: 32581
		private const float PlateauRadius = 0.4f;

		// Token: 0x04007F46 RID: 32582
		private const float PlateauExp = 0.2f;

		// Token: 0x04007F47 RID: 32583
		private const float PlateauNoiseFrequency = 0.015f;

		// Token: 0x04007F48 RID: 32584
		private const float PlateauNoiseStrength = 35f;

		// Token: 0x04007F49 RID: 32585
		private const float PlateauThreshold = 0f;

		// Token: 0x04007F4A RID: 32586
		private static readonly FloatRange CenterOffsetRange = new FloatRange(-0.2f, 0.2f);
	}
}
