using System;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x020020FC RID: 8444
	public class TileMutatorWorker_LavaCaves : TileMutatorWorker_Caves
	{
		// Token: 0x0600B85D RID: 47197 RVA: 0x0034EBAC File Offset: 0x0034CDAC
		public TileMutatorWorker_LavaCaves(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x0600B85E RID: 47198 RVA: 0x003529D4 File Offset: 0x00350BD4
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
					ModuleBase moduleBase = new DistFromPoint(TileMutatorWorker_LavaCaves.LakeRadius.RandomInRange);
					moduleBase = new ScaleBias(-1.0, 1.0, moduleBase);
					moduleBase = new Scale((double)TileMutatorWorker_LavaCaves.LakeSquash.RandomInRange, 1.0, 1.0, moduleBase);
					moduleBase = new Rotate(0.0, (double)Rand.Range(0f, 360f), 0.0, moduleBase);
					moduleBase = new Translate((double)(-(double)intVec.x), 0.0, (double)(-(double)intVec.z), moduleBase);
					moduleBase = MapNoiseUtility.AddDisplacementNoise(moduleBase, 0.03f, 5f, 4, map.Tile.tileId);
					CellRect cellRect = intVec.RectAbout(20, 20);
					cellRect.ClipInsideMap(map);
					foreach (IntVec3 intVec2 in cellRect)
					{
						float num = (float)moduleBase.GetValue((double)intVec2.x, 0.0, (double)intVec2.z);
						if (num >= 0.2f)
						{
							Thing edifice = intVec2.GetEdifice(map);
							if (edifice != null)
							{
								edifice.Destroy(DestroyMode.Vanish);
							}
							if (num > 0.5f)
							{
								map.terrainGrid.SetTerrain(intVec2, TerrainDefOf.LavaDeep);
							}
							else if (num > 0.2f)
							{
								map.terrainGrid.SetTerrain(intVec2, TerrainDefOf.CooledLava);
							}
						}
					}
				}
			}
		}

		// Token: 0x04007F09 RID: 32521
		private static readonly FloatRange LakeRadius = new FloatRange(4f, 6f);

		// Token: 0x04007F0A RID: 32522
		private static readonly FloatRange LakeSquash = new FloatRange(1f, 1.25f);

		// Token: 0x04007F0B RID: 32523
		private const float LakeNoiseFrequency = 0.03f;

		// Token: 0x04007F0C RID: 32524
		private const float LakeNoiseStrength = 5f;

		// Token: 0x04007F0D RID: 32525
		private const float LavaPocketChancePerCell = 0.005f;

		// Token: 0x04007F0E RID: 32526
		private const float LavaThreshold = 0.5f;

		// Token: 0x04007F0F RID: 32527
		private const float CooledLavaThreshold = 0.2f;
	}
}
