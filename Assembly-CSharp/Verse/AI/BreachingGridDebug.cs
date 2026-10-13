using System;
using System.Collections.Generic;
using LudeonTK;
using RimWorld;
using Verse.AI.Group;

namespace Verse.AI
{
	// Token: 0x0200104C RID: 4172
	public static class BreachingGridDebug
	{
		// Token: 0x060065BA RID: 26042 RVA: 0x00203A10 File Offset: 0x00201C10
		private static void DebugBreachPickTwoPoints(int breachRadius, int walkMargin, bool useAvoidGrid)
		{
			if (!BreachingGridDebug.debugStartCell.IsValid)
			{
				BreachingGridDebug.debugStartCell = UI.MouseCell();
				return;
			}
			IntVec3 destCall = UI.MouseCell();
			BreachingGridDebug.DebugCreateBreachPath(breachRadius, walkMargin, useAvoidGrid, BreachingGridDebug.debugStartCell, destCall);
		}

		// Token: 0x060065BB RID: 26043 RVA: 0x00203A48 File Offset: 0x00201C48
		private static void DebugBreachPickOnePoint(int breachRadius, int walkMargin, bool useAvoidGrid)
		{
			IntVec3 intVec = UI.MouseCell();
			IntVec3 destCall = GenAI.RandomRaidDest(intVec, Find.CurrentMap);
			if (!destCall.IsValid)
			{
				Messages.Message("Could not find a destination for breach path", MessageTypeDefOf.RejectInput, false);
				return;
			}
			BreachingGridDebug.DebugCreateBreachPath(breachRadius, walkMargin, useAvoidGrid, intVec, destCall);
		}

		// Token: 0x060065BC RID: 26044 RVA: 0x00203A8B File Offset: 0x00201C8B
		private static void DebugCreateBreachPath(int breachRadius, int walkMargin, bool useAvoidGrid, IntVec3 startCell, IntVec3 destCall)
		{
			DebugViewSettings.drawBreachingGrid = true;
			BreachingGridDebug.debugBeachGridForDrawing = new BreachingGrid(Find.CurrentMap, null);
			BreachingGridDebug.debugBeachGridForDrawing.CreateBreachPath(startCell, destCall, breachRadius, walkMargin, useAvoidGrid);
			BreachingGridDebug.debugStartCell = IntVec3.Invalid;
		}

		// Token: 0x060065BD RID: 26045 RVA: 0x00203ABD File Offset: 0x00201CBD
		public static void ClearDebugPath()
		{
			BreachingGridDebug.debugBeachGridForDrawing = null;
		}

		// Token: 0x060065BE RID: 26046 RVA: 0x00203AC8 File Offset: 0x00201CC8
		public static void DebugDrawAllOnMap(Map map)
		{
			if (!DebugViewSettings.drawBreachingGrid && !DebugViewSettings.drawBreachingNoise)
			{
				return;
			}
			BreachingGrid breachingGrid = BreachingGridDebug.debugBeachGridForDrawing;
			if (((breachingGrid != null) ? breachingGrid.Map : null) == map)
			{
				BreachingGridDebug.DebugDrawBreachingGrid(BreachingGridDebug.debugBeachGridForDrawing);
			}
			List<Lord> lords = map.lordManager.lords;
			for (int i = 0; i < lords.Count; i++)
			{
				LordToilData_AssaultColonyBreaching lordToilData_AssaultColonyBreaching = BreachingUtility.LordDataFor(lords[i]);
				if (lordToilData_AssaultColonyBreaching != null)
				{
					BreachingGridDebug.DebugDrawBreachingGrid(lordToilData_AssaultColonyBreaching.breachingGrid);
					if (lordToilData_AssaultColonyBreaching.currentTarget != null)
					{
						CellRenderer.RenderSpot(lordToilData_AssaultColonyBreaching.currentTarget.Position, 0.9f, 0.4f);
					}
				}
			}
		}

		// Token: 0x060065BF RID: 26047 RVA: 0x00203B5C File Offset: 0x00201D5C
		private static void DebugDrawMarkerGrid(BreachingGrid grid, Map map)
		{
			for (int i = 0; i < map.Size.x; i++)
			{
				for (int j = 0; j < map.Size.z; j++)
				{
					IntVec3 c = new IntVec3(i, 0, j);
					byte b = grid.MarkerGrid[c];
					if (b == 180)
					{
						CellRenderer.RenderSpot(c, 0.1f, 0.15f);
					}
					else if (b == 10)
					{
						CellRenderer.RenderCell(c, 0.1f);
					}
					if (grid.ReachableGrid[c])
					{
						CellRenderer.RenderSpot(c, 0.5f, 0.03f);
					}
				}
			}
		}

		// Token: 0x060065C0 RID: 26048 RVA: 0x00203BF8 File Offset: 0x00201DF8
		private static void DebugDrawBreachingGrid(BreachingGrid grid)
		{
			if (DebugViewSettings.drawBreachingNoise)
			{
				BreachingNoiseDebugDrawer.DebugDrawNoise(grid);
			}
			if (DebugViewSettings.drawBreachingGrid)
			{
				BreachingGridDebug.DebugDrawMarkerGrid(grid, grid.Map);
				foreach (IntVec3 c in grid.WalkGrid.ActiveCells)
				{
					Building firstBuilding = c.GetFirstBuilding(grid.Map);
					float colorPct = 0.3f;
					if (grid.BreachGrid[c])
					{
						colorPct = 0.4f;
						if (firstBuilding != null && BreachingUtility.ShouldBreachBuilding(firstBuilding))
						{
							colorPct = 0.1f;
							if (BreachingUtility.IsWorthBreachingBuilding(grid, firstBuilding))
							{
								colorPct = 0.8f;
								if (BreachingUtility.CountReachableAdjacentCells(grid, firstBuilding) > 0)
								{
									CellRenderer.RenderSpot(c, colorPct, 0.15f);
								}
							}
						}
					}
					CellRenderer.RenderCell(c, colorPct);
				}
			}
		}

		// Token: 0x060065C1 RID: 26049 RVA: 0x00203CCC File Offset: 0x00201ECC
		[DebugAction("Pawns", "Draw breach path...", false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void DebugDrawBreachPath()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			for (int i = 1; i <= 5; i++)
			{
				int widthLocal = i;
				list.Add(new DebugMenuOption("width: " + i.ToString(), DebugMenuOptionMode.Action, delegate()
				{
					List<DebugMenuOption> list2 = new List<DebugMenuOption>();
					for (int j = 1; j < 5; j++)
					{
						int marginLocal = j;
						Action <>9__2;
						Action <>9__3;
						Action <>9__4;
						Action <>9__5;
						list2.Add(new DebugMenuOption("margin: " + j.ToString(), DebugMenuOptionMode.Action, delegate()
						{
							List<DebugMenuOption> list3 = new List<DebugMenuOption>();
							List<DebugMenuOption> list4 = list3;
							string label = "Draw from...";
							DebugMenuOptionMode mode = DebugMenuOptionMode.Tool;
							Action method;
							if ((method = <>9__2) == null)
							{
								method = (<>9__2 = delegate()
								{
									BreachingGridDebug.DebugBreachPickOnePoint(widthLocal, marginLocal, false);
								});
							}
							list4.Add(new DebugMenuOption(label, mode, method));
							List<DebugMenuOption> list5 = list3;
							string label2 = "Draw from (with avoid grid)...";
							DebugMenuOptionMode mode2 = DebugMenuOptionMode.Tool;
							Action method2;
							if ((method2 = <>9__3) == null)
							{
								method2 = (<>9__3 = delegate()
								{
									BreachingGridDebug.DebugBreachPickOnePoint(widthLocal, marginLocal, true);
								});
							}
							list5.Add(new DebugMenuOption(label2, mode2, method2));
							List<DebugMenuOption> list6 = list3;
							string label3 = "Draw between...";
							DebugMenuOptionMode mode3 = DebugMenuOptionMode.Tool;
							Action method3;
							if ((method3 = <>9__4) == null)
							{
								method3 = (<>9__4 = delegate()
								{
									BreachingGridDebug.DebugBreachPickTwoPoints(widthLocal, marginLocal, false);
								});
							}
							list6.Add(new DebugMenuOption(label3, mode3, method3));
							List<DebugMenuOption> list7 = list3;
							string label4 = "Draw between (with avoid grid)...";
							DebugMenuOptionMode mode4 = DebugMenuOptionMode.Tool;
							Action method4;
							if ((method4 = <>9__5) == null)
							{
								method4 = (<>9__5 = delegate()
								{
									BreachingGridDebug.DebugBreachPickTwoPoints(widthLocal, marginLocal, true);
								});
							}
							list7.Add(new DebugMenuOption(label4, mode4, method4));
							Find.WindowStack.Add(new Dialog_DebugOptionListLister(list3, null));
						}));
					}
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2, null));
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x060065C2 RID: 26050 RVA: 0x00203D32 File Offset: 0x00201F32
		public static void Notify_BuildingStateChanged(Building b)
		{
			BreachingGrid breachingGrid = BreachingGridDebug.debugBeachGridForDrawing;
			if (breachingGrid == null)
			{
				return;
			}
			breachingGrid.Notify_BuildingStateChanged(b);
		}

		// Token: 0x040048D9 RID: 18649
		private static IntVec3 debugStartCell = IntVec3.Invalid;

		// Token: 0x040048DA RID: 18650
		private static BreachingGrid debugBeachGridForDrawing = null;
	}
}
