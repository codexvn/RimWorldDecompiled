using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200051B RID: 1307
	public static class RoofCollapseCellsFinder
	{
		// Token: 0x0600221D RID: 8733 RVA: 0x000B1AB4 File Offset: 0x000AFCB4
		public static void Notify_RoofHolderDespawned(Thing t, Map map)
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return;
			}
			RoofCollapseCellsFinder.ProcessRoofHolderDespawned(t.OccupiedRect(), t.Position, map, false, false);
		}

		// Token: 0x0600221E RID: 8734 RVA: 0x000B1AD4 File Offset: 0x000AFCD4
		public static void ProcessRoofHolderDespawned(CellRect rect, IntVec3 position, Map map, bool removalMode = false, bool canRemoveThickRoof = false)
		{
			RoofCollapseCellsFinder.CheckCollapseFlyingRoofs(rect, map, removalMode, canRemoveThickRoof);
			RoofGrid roofGrid = map.roofGrid;
			RoofCollapseCellsFinder.roofsCollapsingBecauseTooFar.Clear();
			for (int i = 0; i < RoofCollapseUtility.RoofSupportRadialCellsCount; i++)
			{
				IntVec3 intVec = position + GenRadial.RadialPattern[i];
				if (intVec.InBounds(map) && roofGrid.Roofed(intVec.x, intVec.z) && roofGrid.RoofAt(intVec).canCollapse && !map.roofCollapseBuffer.IsMarkedToCollapse(intVec) && !RoofCollapseUtility.WithinRangeOfRoofHolder(intVec, map, false))
				{
					if (removalMode && (canRemoveThickRoof || intVec.GetRoof(map).VanishOnCollapse))
					{
						map.roofGrid.SetRoof(intVec, null);
					}
					else
					{
						map.roofCollapseBuffer.MarkToCollapse(intVec);
					}
					RoofCollapseCellsFinder.roofsCollapsingBecauseTooFar.Add(intVec);
				}
			}
			RoofCollapseCellsFinder.CheckCollapseFlyingRoofs(RoofCollapseCellsFinder.roofsCollapsingBecauseTooFar, map, removalMode, canRemoveThickRoof);
			RoofCollapseCellsFinder.roofsCollapsingBecauseTooFar.Clear();
		}

		// Token: 0x0600221F RID: 8735 RVA: 0x000B1BBC File Offset: 0x000AFDBC
		public static void CheckAndRemoveCollpsingRoofs(Map map)
		{
			foreach (IntVec3 intVec in map.AllCells)
			{
				if (intVec.Roofed(map))
				{
					Building edifice = intVec.GetEdifice(map);
					if (edifice != null && edifice.def.holdsRoof)
					{
						RoofCollapseCellsFinder.ProcessRoofHolderDespawned(new CellRect(intVec.x, intVec.z, 1, 1), intVec, map, true, true);
					}
				}
			}
		}

		// Token: 0x06002220 RID: 8736 RVA: 0x000B1C44 File Offset: 0x000AFE44
		public static void RemoveBulkCollapsingRoofs(List<IntVec3> nearCells, Map map)
		{
			for (int i = 0; i < nearCells.Count; i++)
			{
				RoofCollapseCellsFinder.ProcessRoofHolderDespawned(new CellRect(nearCells[i].x, nearCells[i].z, 1, 1), nearCells[i], map, true, true);
			}
		}

		// Token: 0x06002221 RID: 8737 RVA: 0x000B1C90 File Offset: 0x000AFE90
		public static void CheckCollapseFlyingRoofs(List<IntVec3> nearCells, Map map, bool removalMode = false, bool canRemoveThickRoof = false)
		{
			RoofCollapseCellsFinder.visitedCells.Clear();
			for (int i = 0; i < nearCells.Count; i++)
			{
				RoofCollapseCellsFinder.CheckCollapseFlyingRoofAtAndAdjInternal(nearCells[i], map, removalMode, canRemoveThickRoof);
			}
			RoofCollapseCellsFinder.visitedCells.Clear();
		}

		// Token: 0x06002222 RID: 8738 RVA: 0x000B1CD4 File Offset: 0x000AFED4
		public static void CheckCollapseFlyingRoofs(CellRect nearRect, Map map, bool removalMode = false, bool canRemoveThickRoof = false)
		{
			RoofCollapseCellsFinder.visitedCells.Clear();
			foreach (IntVec3 root in nearRect)
			{
				RoofCollapseCellsFinder.CheckCollapseFlyingRoofAtAndAdjInternal(root, map, removalMode, canRemoveThickRoof);
			}
			RoofCollapseCellsFinder.visitedCells.Clear();
		}

		// Token: 0x06002223 RID: 8739 RVA: 0x000B1D38 File Offset: 0x000AFF38
		private static bool CheckCollapseFlyingRoofAtAndAdjInternal(IntVec3 root, Map map, bool removalMode, bool canRemoveThickRoof)
		{
			RoofCollapseBuffer roofCollapseBuffer = map.roofCollapseBuffer;
			if (removalMode && roofCollapseBuffer.CellsMarkedToCollapse.Count > 0)
			{
				map.roofCollapseBufferResolver.CollapseRoofsMarkedToCollapse();
			}
			Predicate<IntVec3> <>9__0;
			Action<IntVec3> <>9__1;
			for (int i = 0; i < 5; i++)
			{
				IntVec3 intVec = root + GenAdj.CardinalDirectionsAndInside[i];
				if (intVec.InBounds(map) && intVec.Roofed(map) && !RoofCollapseCellsFinder.visitedCells.Contains(intVec) && !roofCollapseBuffer.IsMarkedToCollapse(intVec) && !RoofCollapseCellsFinder.ConnectsToRoofHolder(intVec, map, RoofCollapseCellsFinder.visitedCells))
				{
					FloodFiller floodFiller = map.floodFiller;
					IntVec3 root2 = intVec;
					Predicate<IntVec3> passCheck;
					if ((passCheck = <>9__0) == null)
					{
						passCheck = (<>9__0 = ((IntVec3 x) => x.Roofed(map)));
					}
					Action<IntVec3> processor;
					if ((processor = <>9__1) == null)
					{
						processor = (<>9__1 = delegate(IntVec3 x)
						{
							roofCollapseBuffer.MarkToCollapse(x);
						});
					}
					floodFiller.FloodFill(root2, passCheck, processor, int.MaxValue, false, null);
					if (removalMode)
					{
						List<IntVec3> cellsMarkedToCollapse = roofCollapseBuffer.CellsMarkedToCollapse;
						for (int j = cellsMarkedToCollapse.Count - 1; j >= 0; j--)
						{
							RoofDef roofDef = map.roofGrid.RoofAt(cellsMarkedToCollapse[j]);
							if (roofDef != null && (canRemoveThickRoof || roofDef.VanishOnCollapse))
							{
								map.roofGrid.SetRoof(cellsMarkedToCollapse[j], null);
								cellsMarkedToCollapse.RemoveAt(j);
							}
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06002224 RID: 8740 RVA: 0x000B1ED8 File Offset: 0x000B00D8
		public static bool ConnectsToRoofHolder(IntVec3 c, Map map, HashSet<IntVec3> visitedCells)
		{
			bool connected = false;
			map.floodFiller.FloodFill(c, (IntVec3 x) => x.Roofed(map) && !connected, delegate(IntVec3 x)
			{
				if (visitedCells.Contains(x))
				{
					connected = true;
					return;
				}
				visitedCells.Add(x);
				for (int i = 0; i < 5; i++)
				{
					IntVec3 c2 = x + GenAdj.CardinalDirectionsAndInside[i];
					if (c2.InBounds(map))
					{
						Building edifice = c2.GetEdifice(map);
						if (edifice != null && edifice.def.holdsRoof)
						{
							connected = true;
							return;
						}
					}
				}
			}, int.MaxValue, false, null);
			return connected;
		}

		// Token: 0x04001CC2 RID: 7362
		private static List<IntVec3> roofsCollapsingBecauseTooFar = new List<IntVec3>();

		// Token: 0x04001CC3 RID: 7363
		private static HashSet<IntVec3> visitedCells = new HashSet<IntVec3>();
	}
}
