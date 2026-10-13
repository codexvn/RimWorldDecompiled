using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200239A RID: 9114
	public class PawnsArrivalModeWorker_EdgeWalkInDistributedGroups : PawnsArrivalModeWorker
	{
		// Token: 0x0600C566 RID: 50534 RVA: 0x00396E00 File Offset: 0x00395000
		public override void Arrive(List<Pawn> pawns, IncidentParms parms)
		{
			Map map = (Map)parms.target;
			List<Pair<List<Pawn>, IntVec3>> list = this.SplitIntoRandomGroupsNearMapEdge(pawns, map);
			PawnsArrivalModeWorkerUtility.SetPawnGroupsInfo(parms, list);
			for (int i = 0; i < list.Count; i++)
			{
				for (int j = 0; j < list[i].First.Count; j++)
				{
					IntVec3 loc = CellFinder.RandomClosewalkCellNear(list[i].Second, map, 8, null);
					GenSpawn.Spawn(list[i].First[j], loc, map, parms.spawnRotation, WipeMode.Vanish, false, false);
				}
			}
		}

		// Token: 0x0600C567 RID: 50535 RVA: 0x00396A22 File Offset: 0x00394C22
		public override bool TryResolveRaidSpawnCenter(IncidentParms parms)
		{
			parms.spawnRotation = Rot4.Random;
			return true;
		}

		// Token: 0x0600C568 RID: 50536 RVA: 0x00396E9C File Offset: 0x0039509C
		private List<Pair<List<Pawn>, IntVec3>> SplitIntoRandomGroupsNearMapEdge(List<Pawn> pawns, Map map)
		{
			List<Pair<List<Pawn>, IntVec3>> list = new List<Pair<List<Pawn>, IntVec3>>();
			if (!pawns.Any<Pawn>())
			{
				return list;
			}
			int num = Mathf.Min(PawnsArrivalModeWorker_EdgeWalkInDistributedGroups.NumGroupsRange.RandomInRange, pawns.Count);
			int[] array = new int[num];
			for (int i = 0; i < num; i++)
			{
				IntVec3 second = PawnsArrivalModeWorkerUtility.FindNewMapEdgeGroupCenter(map, list, false);
				Pair<List<Pawn>, IntVec3> item = new Pair<List<Pawn>, IntVec3>(new List<Pawn>(), second);
				list.Add(item);
				array[i] = Mathf.CeilToInt((float)pawns.Count / (float)num);
			}
			for (int j = 0; j < 20; j++)
			{
				int num2 = Rand.Range(0, num);
				int num3 = (num2 + 1) % num;
				int num4 = Rand.Range(0, Mathf.Min(array[num2], array[num3]) / 5);
				array[num2] += num4;
				array[num3] -= num4;
			}
			int num5 = 0;
			int num6 = 0;
			foreach (Pawn item2 in pawns)
			{
				if (num6 >= array[num5])
				{
					num5++;
					num6 = 0;
				}
				list[num5].First.Add(item2);
				num6++;
			}
			return list;
		}

		// Token: 0x040086A9 RID: 34473
		private static readonly IntRange NumGroupsRange = new IntRange(1, 4);
	}
}
