using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F91 RID: 8081
	public class GenStep_Animals : GenStep
	{
		// Token: 0x17001B28 RID: 6952
		// (get) Token: 0x0600B289 RID: 45705 RVA: 0x0033203A File Offset: 0x0033023A
		public override int SeedPart
		{
			get
			{
				return 1298760307;
			}
		}

		// Token: 0x0600B28A RID: 45706 RVA: 0x00332044 File Offset: 0x00330244
		public override void Generate(Map map, GenStepParams parms)
		{
			int num = 0;
			while (!map.wildAnimalSpawner.AnimalEcosystemFull)
			{
				num++;
				if (num >= 10000)
				{
					Log.Error("Too many iterations.");
					return;
				}
				IntVec3 loc = RCellFinder.RandomAnimalSpawnCell_MapGen(map);
				if (!map.wildAnimalSpawner.SpawnRandomWildAnimalAt(loc, false, null))
				{
					break;
				}
			}
		}
	}
}
