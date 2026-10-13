using System;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x020020D6 RID: 8406
	public class TileMutatorWorker_CaveLakes : TileMutatorWorker_Caves
	{
		// Token: 0x0600B7B8 RID: 47032 RVA: 0x0034EBAC File Offset: 0x0034CDAC
		public TileMutatorWorker_CaveLakes(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x0600B7B9 RID: 47033 RVA: 0x0034EBB8 File Offset: 0x0034CDB8
		public override void GeneratePostTerrain(Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			MapGenFloatGrid caves = MapGenerator.Caves;
			base.GeneratePostTerrain(map);
			foreach (IntVec3 intVec in map.AllCells)
			{
				if (caves[intVec] > 0f && !intVec.GetTerrain(map).IsWater && Rand.Chance(0.005f))
				{
					ModuleBase moduleBase = new DistFromPoint(TileMutatorWorker_CaveLakes.LakeRadius.RandomInRange);
					moduleBase = new ScaleBias(-1.0, 1.0, moduleBase);
					moduleBase = new Scale((double)TileMutatorWorker_CaveLakes.LakeSquash.RandomInRange, 1.0, 1.0, moduleBase);
					moduleBase = new Rotate(0.0, (double)Rand.Range(0f, 360f), 0.0, moduleBase);
					moduleBase = new Translate((double)(-(double)intVec.x), 0.0, (double)(-(double)intVec.z), moduleBase);
					moduleBase = MapNoiseUtility.AddDisplacementNoise(moduleBase, 0.03f, 5f, 4, map.Tile.tileId);
					CellRect cellRect = intVec.RectAbout(30, 30);
					cellRect.ClipInsideMap(map);
					foreach (IntVec3 intVec2 in cellRect)
					{
						float num = (float)moduleBase.GetValue((double)intVec2.x, 0.0, (double)intVec2.z);
						if (num >= 0.5f)
						{
							Thing edifice = intVec2.GetEdifice(map);
							if (edifice != null)
							{
								edifice.Destroy(DestroyMode.Vanish);
							}
							if (num > 0.8f)
							{
								map.terrainGrid.SetTerrain(intVec2, MapGenUtility.DeepFreshWaterTerrainAt(intVec2, map));
							}
							else if (num > 0.5f)
							{
								map.terrainGrid.SetTerrain(intVec2, MapGenUtility.ShallowFreshWaterTerrainAt(intVec2, map));
							}
						}
					}
				}
			}
		}

		// Token: 0x04007E3D RID: 32317
		private static readonly FloatRange LakeRadius = new FloatRange(10f, 15f);

		// Token: 0x04007E3E RID: 32318
		private static readonly FloatRange LakeSquash = new FloatRange(1f, 1.25f);

		// Token: 0x04007E3F RID: 32319
		private const float LakeNoiseFrequency = 0.03f;

		// Token: 0x04007E40 RID: 32320
		private const float LakeNoiseStrength = 5f;

		// Token: 0x04007E41 RID: 32321
		private const float LakeChancePerCell = 0.005f;

		// Token: 0x04007E42 RID: 32322
		private const float DeepWaterThreshold = 0.8f;

		// Token: 0x04007E43 RID: 32323
		private const float WaterThreshold = 0.5f;
	}
}
