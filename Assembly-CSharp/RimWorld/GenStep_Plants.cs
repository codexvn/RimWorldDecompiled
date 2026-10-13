using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FDD RID: 8157
	public class GenStep_Plants : GenStep
	{
		// Token: 0x17001B6A RID: 7018
		// (get) Token: 0x0600B3D4 RID: 46036 RVA: 0x00338B9C File Offset: 0x00336D9C
		public override int SeedPart
		{
			get
			{
				return 578415222;
			}
		}

		// Token: 0x0600B3D5 RID: 46037 RVA: 0x00338BA4 File Offset: 0x00336DA4
		public override void Generate(Map map, GenStepParams parms)
		{
			float currentPlantDensityFactor = map.wildPlantSpawner.CurrentPlantDensityFactor;
			float currentWholeMapNumDesiredPlants = map.wildPlantSpawner.CurrentWholeMapNumDesiredPlants;
			foreach (IntVec3 c in map.cellsInRandomOrder.GetAll())
			{
				if (!Rand.Chance(0.001f))
				{
					map.wildPlantSpawner.CheckSpawnWildPlantAt(c, currentPlantDensityFactor, currentWholeMapNumDesiredPlants, true);
				}
			}
		}

		// Token: 0x04007B51 RID: 31569
		private const float ChanceToSkip = 0.001f;
	}
}
