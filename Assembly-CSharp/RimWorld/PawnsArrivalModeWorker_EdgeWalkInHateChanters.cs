using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200239C RID: 9116
	public class PawnsArrivalModeWorker_EdgeWalkInHateChanters : PawnsArrivalModeWorker
	{
		// Token: 0x0600C56E RID: 50542 RVA: 0x0039708C File Offset: 0x0039528C
		public override void Arrive(List<Pawn> pawns, IncidentParms parms)
		{
			PawnsArrivalModeWorker_EdgeWalkInHateChanters.<>c__DisplayClass1_0 CS$<>8__locals1 = new PawnsArrivalModeWorker_EdgeWalkInHateChanters.<>c__DisplayClass1_0();
			CS$<>8__locals1.pawns = pawns;
			PawnsArrivalModeWorker_EdgeWalkInHateChanters.EntryCells.Clear();
			Map map = (Map)parms.target;
			int i;
			for (i = 0; i < CS$<>8__locals1.pawns.Count; i++)
			{
				bool flag = false;
				for (int j = 0; j < 100; j++)
				{
					IntVec3 intVec;
					if (RCellFinder.TryFindRandomPawnEntryCell(out intVec, map, CellFinder.EdgeRoadChance_Ignore, false, new Predicate<IntVec3>(CS$<>8__locals1.<Arrive>g__SmallGroupValidator|0)))
					{
						CS$<>8__locals1.pawns[i].Position = intVec;
						PawnsArrivalModeWorker_EdgeWalkInHateChanters.EntryCells.Add(intVec);
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					break;
				}
			}
			CS$<>8__locals1.pawns.RemoveRange(i, CS$<>8__locals1.pawns.Count - i);
		}

		// Token: 0x0600C56F RID: 50543 RVA: 0x00396A22 File Offset: 0x00394C22
		public override bool TryResolveRaidSpawnCenter(IncidentParms parms)
		{
			parms.spawnRotation = Rot4.Random;
			return true;
		}

		// Token: 0x040086AA RID: 34474
		private static readonly HashSet<IntVec3> EntryCells = new HashSet<IntVec3>();
	}
}
