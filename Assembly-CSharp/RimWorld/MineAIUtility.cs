using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001768 RID: 5992
	public static class MineAIUtility
	{
		// Token: 0x06008C8C RID: 35980 RVA: 0x0029CB4C File Offset: 0x0029AD4C
		public static void ResetStaticData()
		{
			MineAIUtility.NoPathTrans = "NoPath".Translate();
		}

		// Token: 0x06008C8D RID: 35981 RVA: 0x0029CB62 File Offset: 0x0029AD62
		public static IEnumerable<Thing> PotentialMineables(Pawn pawn)
		{
			MineAIUtility.tmpDesignations.Clear();
			MineAIUtility.tmpDesignations.AddRange(pawn.Map.designationManager.SpawnedDesignationsOfDef(DesignationDefOf.Mine));
			MineAIUtility.tmpDesignations.AddRange(pawn.Map.designationManager.SpawnedDesignationsOfDef(DesignationDefOf.MineVein));
			foreach (Designation designation in MineAIUtility.tmpDesignations)
			{
				bool flag = false;
				for (int i = 0; i < 8; i++)
				{
					IntVec3 c = designation.target.Cell + GenAdj.AdjacentCells[i];
					if (c.InBounds(pawn.Map) && c.Walkable(pawn.Map))
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					Mineable firstMineable = designation.target.Cell.GetFirstMineable(pawn.Map);
					if (firstMineable != null)
					{
						yield return firstMineable;
					}
				}
			}
			List<Designation>.Enumerator enumerator = default(List<Designation>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06008C8E RID: 35982 RVA: 0x0029CB74 File Offset: 0x0029AD74
		public static Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!t.def.mineable)
			{
				return null;
			}
			if (pawn.Map.designationManager.DesignationAt(t.Position, DesignationDefOf.Mine) == null && pawn.Map.designationManager.DesignationAt(t.Position, DesignationDefOf.MineVein) == null)
			{
				return null;
			}
			if (!pawn.CanReserve(t, 1, -1, null, forced))
			{
				return null;
			}
			if (!new HistoryEvent(HistoryEventDefOf.Mined, pawn.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo_Job())
			{
				return null;
			}
			bool flag = false;
			for (int i = 0; i < 8; i++)
			{
				IntVec3 intVec = t.Position + GenAdj.AdjacentCells[i];
				if (intVec.InBounds(pawn.Map) && intVec.Standable(pawn.Map) && ReachabilityImmediate.CanReachImmediate(intVec, t, pawn.Map, PathEndMode.Touch, pawn))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				for (int j = 0; j < 8; j++)
				{
					IntVec3 intVec2 = t.Position + GenAdj.AdjacentCells[j];
					if (intVec2.InBounds(t.Map) && ReachabilityImmediate.CanReachImmediate(intVec2, t, pawn.Map, PathEndMode.Touch, pawn) && intVec2.WalkableBy(t.Map, pawn) && !intVec2.Standable(t.Map))
					{
						List<Thing> thingList = intVec2.GetThingList(t.Map);
						for (int k = 0; k < thingList.Count; k++)
						{
							if (thingList[k].def.designateHaulable && thingList[k].def.passability == Traversability.PassThroughOnly)
							{
								Job job = HaulAIUtility.HaulAsideJobFor(pawn, thingList[k]);
								if (job != null)
								{
									return job;
								}
							}
						}
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					JobFailReason.Is(MineAIUtility.NoPathTrans, null);
					return null;
				}
			}
			return JobMaker.MakeJob(JobDefOf.Mine, t, 20000, true);
		}

		// Token: 0x04005B5B RID: 23387
		private static string NoPathTrans;

		// Token: 0x04005B5C RID: 23388
		private const int MiningJobTicks = 20000;

		// Token: 0x04005B5D RID: 23389
		private static List<Designation> tmpDesignations = new List<Designation>();
	}
}
