using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002399 RID: 9113
	public class PawnsArrivalModeWorker_EdgeWalkInDistributed : PawnsArrivalModeWorker
	{
		// Token: 0x0600C562 RID: 50530 RVA: 0x00396D54 File Offset: 0x00394F54
		public override void Arrive(List<Pawn> pawns, IncidentParms parms)
		{
			PawnsArrivalModeWorker_EdgeWalkInDistributed.EntryCells.Clear();
			Map map = (Map)parms.target;
			int i;
			for (i = 0; i < pawns.Count; i++)
			{
				bool flag = false;
				for (int j = 0; j < 100; j++)
				{
					IntVec3 intVec;
					if (RCellFinder.TryFindRandomPawnEntryCell(out intVec, map, CellFinder.EdgeRoadChance_Ignore, false, null))
					{
						pawns[i].Position = intVec;
						PawnsArrivalModeWorker_EdgeWalkInDistributed.EntryCells.Add(intVec);
						flag = true;
						GenSpawn.Spawn(pawns[i], intVec, map, parms.spawnRotation, WipeMode.Vanish, false, false);
						break;
					}
				}
				if (!flag)
				{
					break;
				}
			}
			pawns.RemoveRange(i, pawns.Count - i);
		}

		// Token: 0x0600C563 RID: 50531 RVA: 0x00396A22 File Offset: 0x00394C22
		public override bool TryResolveRaidSpawnCenter(IncidentParms parms)
		{
			parms.spawnRotation = Rot4.Random;
			return true;
		}

		// Token: 0x040086A8 RID: 34472
		private static readonly HashSet<IntVec3> EntryCells = new HashSet<IntVec3>();
	}
}
