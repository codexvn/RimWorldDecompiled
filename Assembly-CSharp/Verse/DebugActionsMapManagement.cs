using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LudeonTK;
using RimWorld;
using RimWorld.BaseGen;
using RimWorld.Planet;
using RimWorld.SketchGen;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200094F RID: 2383
	public static class DebugActionsMapManagement
	{
		// Token: 0x06003D21 RID: 15649 RVA: 0x001406DF File Offset: 0x0013E8DF
		[DebugAction("Map", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 1000)]
		private static List<DebugActionNode> UseScatterer()
		{
			return DebugTools_MapGen.Options_Scatterers();
		}

		// Token: 0x06003D22 RID: 15650 RVA: 0x001406E8 File Offset: 0x0013E8E8
		[DebugAction("Map", "BaseGen", false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 1000)]
		private static List<DebugActionNode> BaseGen()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (string localSymbol2 in (from x in DefDatabase<RuleDef>.AllDefs
			select x.symbol).Distinct<string>())
			{
				string localSymbol = localSymbol2;
				list.Add(new DebugActionNode(localSymbol, DebugActionType.Action, null, null)
				{
					action = delegate()
					{
						DebugTool tool = null;
						IntVec3 firstCorner;
						Action <>9__3;
						tool = new DebugTool("first corner...", delegate()
						{
							firstCorner = UI.MouseCell();
							string label = "second corner...";
							Action clickAction;
							if ((clickAction = <>9__3) == null)
							{
								clickAction = (<>9__3 = delegate()
								{
									IntVec3 second = UI.MouseCell();
									CellRect rect = CellRect.FromLimits(firstCorner, second).ClipInsideMap(Find.CurrentMap);
									RimWorld.BaseGen.BaseGen.globalSettings.map = Find.CurrentMap;
									RimWorld.BaseGen.BaseGen.symbolStack.Push(localSymbol, rect, null);
									RimWorld.BaseGen.BaseGen.Generate();
									DebugTools.curTool = tool;
								});
							}
							DebugTools.curTool = new DebugTool(label, clickAction, firstCorner);
						}, null);
						DebugTools.curTool = tool;
					}
				});
			}
			return list;
		}

		// Token: 0x06003D23 RID: 15651 RVA: 0x00140790 File Offset: 0x0013E990
		[DebugAction("Map", "SketchGen", false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 1000)]
		private static List<DebugActionNode> SketchGen()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (SketchResolverDef localResolver2 in from x in DefDatabase<SketchResolverDef>.AllDefs
			where x.isRoot
			select x)
			{
				SketchResolverDef localResolver = localResolver2;
				DebugActionNode debugActionNode = new DebugActionNode(localResolver.defName, DebugActionType.Action, null, null);
				if (localResolver == SketchResolverDefOf.Monument || localResolver == SketchResolverDefOf.MonumentRuin)
				{
					new List<DebugMenuOption>();
					for (int i = 1; i <= 60; i++)
					{
						int localIndex = i;
						debugActionNode.AddChild(new DebugActionNode(localIndex.ToString(), DebugActionType.ToolMap, null, null)
						{
							action = delegate()
							{
								SketchResolveParams parms = default(SketchResolveParams);
								parms.sketch = new Sketch();
								parms.monumentSize = new IntVec2?(new IntVec2(localIndex, localIndex));
								RimWorld.SketchGen.SketchGen.Generate(localResolver, parms).Spawn(Find.CurrentMap, UI.MouseCell(), null, Sketch.SpawnPosType.Unchanged, Sketch.SpawnMode.Normal, false, false, false, null, false, true, null, null, null);
							}
						});
					}
				}
				else
				{
					debugActionNode.actionType = DebugActionType.ToolMap;
					debugActionNode.action = delegate()
					{
						SketchResolveParams parms = default(SketchResolveParams);
						parms.sketch = new Sketch();
						RimWorld.SketchGen.SketchGen.Generate(localResolver, parms).Spawn(Find.CurrentMap, UI.MouseCell(), null, Sketch.SpawnPosType.Unchanged, Sketch.SpawnMode.Normal, false, false, false, null, false, true, null, null, null);
					};
				}
				list.Add(debugActionNode);
			}
			return list;
		}

		// Token: 0x06003D24 RID: 15652 RVA: 0x001408C4 File Offset: 0x0013EAC4
		[DebugAction("Map", "Set terrain (rect)", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 100)]
		private static List<DebugActionNode> SetTerrainRect()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (TerrainDef localDef2 in from terr in DefDatabase<TerrainDef>.AllDefs
			where !terr.temporary
			select terr)
			{
				TerrainDef localDef = localDef2;
				Action<CellRect> <>9__2;
				list.Add(new DebugActionNode(localDef.defName, DebugActionType.Action, null, null)
				{
					action = delegate()
					{
						string defName = localDef.defName;
						Action<CellRect> rectAction;
						if ((rectAction = <>9__2) == null)
						{
							rectAction = (<>9__2 = delegate(CellRect rect)
							{
								foreach (IntVec3 c in rect)
								{
									Find.CurrentMap.terrainGrid.SetTerrain(c, localDef);
								}
							});
						}
						DebugToolsGeneral.GenericRectTool(defName, rectAction, false);
					}
				});
			}
			return list;
		}

		// Token: 0x06003D25 RID: 15653 RVA: 0x0014096C File Offset: 0x0013EB6C
		[DebugAction("Map", "Set temp terrain (rect)", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 100)]
		private static List<DebugActionNode> SetTempTerrainRect()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (TerrainDef localDef2 in from terr in DefDatabase<TerrainDef>.AllDefs
			where terr.temporary
			select terr)
			{
				TerrainDef localDef = localDef2;
				Action<CellRect> <>9__2;
				list.Add(new DebugActionNode(localDef.defName, DebugActionType.Action, null, null)
				{
					action = delegate()
					{
						string defName = localDef.defName;
						Action<CellRect> rectAction;
						if ((rectAction = <>9__2) == null)
						{
							rectAction = (<>9__2 = delegate(CellRect rect)
							{
								foreach (IntVec3 c in rect)
								{
									Find.CurrentMap.terrainGrid.SetTempTerrain(c, localDef);
								}
							});
						}
						DebugToolsGeneral.GenericRectTool(defName, rectAction, false);
					}
				});
			}
			return list;
		}

		// Token: 0x06003D26 RID: 15654 RVA: 0x00140A14 File Offset: 0x0013EC14
		[DebugAction("Map", "Clear temp terrain (rect)", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 100)]
		private static void ClearTempTerrainRect()
		{
			DebugToolsGeneral.GenericRectTool("Clear temp terrain", delegate(CellRect rect)
			{
				foreach (IntVec3 c in rect)
				{
					Find.CurrentMap.terrainGrid.RemoveTempTerrain(c, false, false);
				}
			}, false);
		}

		// Token: 0x06003D27 RID: 15655 RVA: 0x00140A40 File Offset: 0x0013EC40
		[DebugAction("Lighting", "Log lights affecting cell", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, actionType = DebugActionType.ToolMap)]
		private static void LogLightsAffectinCell()
		{
			Find.CurrentMap.glowGrid.DevPrintLightIdsAffectingCell(UI.MouseCell());
		}

		// Token: 0x06003D28 RID: 15656 RVA: 0x00140A56 File Offset: 0x0013EC56
		[DebugAction("Lighting", "Regen glow grid cells (rect)", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 100)]
		private static void RegenGlowGridCells()
		{
			DebugToolsGeneral.GenericRectTool("Regen glow grid cells", delegate(CellRect rect)
			{
				Find.CurrentMap.glowGrid.DevDirtyRect(rect);
			}, false);
		}

		// Token: 0x06003D29 RID: 15657 RVA: 0x00140A82 File Offset: 0x0013EC82
		[DebugAction("Map", "Pollute (rect)", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 100, requiresBiotech = true)]
		private static void PolluteRect()
		{
			DebugToolsGeneral.GenericRectTool("Pollute", delegate(CellRect rect)
			{
				foreach (IntVec3 cell in rect)
				{
					Find.CurrentMap.pollutionGrid.SetPolluted(cell, true, false);
				}
			}, false);
		}

		// Token: 0x06003D2A RID: 15658 RVA: 0x00140AAE File Offset: 0x0013ECAE
		[DebugAction("Map", "Unpollute (rect)", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 100, requiresBiotech = true)]
		private static void UnpolluteRect()
		{
			DebugToolsGeneral.GenericRectTool("Unpollute", delegate(CellRect rect)
			{
				foreach (IntVec3 cell in rect)
				{
					Find.CurrentMap.pollutionGrid.SetPolluted(cell, false, false);
				}
			}, false);
		}

		// Token: 0x06003D2B RID: 15659 RVA: 0x00140ADA File Offset: 0x0013ECDA
		[DebugAction("Map", "Make rock (rect)", false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 100)]
		private static void MakeRock()
		{
			DebugToolsGeneral.GenericRectTool("Make rock", delegate(CellRect rect)
			{
				foreach (IntVec3 loc in rect)
				{
					GenSpawn.Spawn(ThingDefOf.Granite, loc, Find.CurrentMap, WipeMode.Vanish);
				}
			}, false);
		}

		// Token: 0x06003D2C RID: 15660 RVA: 0x00140B06 File Offset: 0x0013ED06
		[DebugAction("Map", "Grow pollution (x10 cell)", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = -200, requiresBiotech = true)]
		private static void PolluteCellTen()
		{
			PollutionUtility.GrowPollutionAt(UI.MouseCell(), Find.CurrentMap, 10, null, false, null);
		}

		// Token: 0x06003D2D RID: 15661 RVA: 0x00140B1C File Offset: 0x0013ED1C
		[DebugAction("Map", "Grow pollution (x100 cell)", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = -200, requiresBiotech = true)]
		private static void PolluteCellHundred()
		{
			PollutionUtility.GrowPollutionAt(UI.MouseCell(), Find.CurrentMap, 100, null, false, null);
		}

		// Token: 0x06003D2E RID: 15662 RVA: 0x00140B32 File Offset: 0x0013ED32
		[DebugAction("Map", "Grow pollution (x1000 cell)", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = -200, requiresBiotech = true)]
		private static void PolluteCellThousand()
		{
			PollutionUtility.GrowPollutionAt(UI.MouseCell(), Find.CurrentMap, 1000, null, false, null);
		}

		// Token: 0x06003D2F RID: 15663 RVA: 0x00140B4C File Offset: 0x0013ED4C
		[DebugAction("Map", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.Playing)]
		private static List<DebugActionNode> AddGameCondition()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (GameConditionDef localDef2 in DefDatabase<GameConditionDef>.AllDefs)
			{
				GameConditionDef localDef = localDef2;
				DebugActionNode debugActionNode = new DebugActionNode(localDef.LabelCap, DebugActionType.Action, null, null);
				debugActionNode.AddChild(new DebugActionNode("Permanent", DebugActionType.Action, null, null)
				{
					action = delegate()
					{
						GameCondition gameCondition = GameConditionMaker.MakeCondition(localDef, -1);
						gameCondition.Permanent = true;
						if (Find.CurrentMap != null)
						{
							Find.CurrentMap.GameConditionManager.RegisterCondition(gameCondition);
							return;
						}
						Find.World.GameConditionManager.RegisterCondition(gameCondition);
					}
				});
				for (int i = 2500; i <= 60000; i += 2500)
				{
					int localTicks = i;
					debugActionNode.AddChild(new DebugActionNode(localTicks.ToStringTicksToPeriod(true, false, true, true, false) ?? "", DebugActionType.Action, null, null)
					{
						action = delegate()
						{
							GameCondition gameCondition = GameConditionMaker.MakeCondition(localDef, -1);
							gameCondition.Duration = localTicks;
							if (Find.CurrentMap != null)
							{
								Find.CurrentMap.GameConditionManager.RegisterCondition(gameCondition);
								return;
							}
							Find.World.GameConditionManager.RegisterCondition(gameCondition);
						}
					});
				}
				list.Add(debugActionNode);
			}
			return list;
		}

		// Token: 0x06003D30 RID: 15664 RVA: 0x00140C64 File Offset: 0x0013EE64
		[DebugAction("Map", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.Playing)]
		private static List<DebugActionNode> RemoveGameCondition()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (GameConditionDef localDef2 in DefDatabase<GameConditionDef>.AllDefs)
			{
				GameConditionDef localDef = localDef2;
				list.Add(new DebugActionNode(localDef.LabelCap, DebugActionType.Action, null, null)
				{
					action = delegate()
					{
						GameCondition activeCondition = Find.CurrentMap.gameConditionManager.GetActiveCondition(localDef);
						if (activeCondition != null)
						{
							activeCondition.Duration = 0;
						}
					},
					visibilityGetter = (() => Find.CurrentMap != null && Find.CurrentMap.gameConditionManager.ConditionIsActive(localDef))
				});
			}
			return list;
		}

		// Token: 0x06003D31 RID: 15665 RVA: 0x00140D00 File Offset: 0x0013EF00
		[DebugAction("Map", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void RefogMap()
		{
			FloodFillerFog.DebugRefogMap(Find.CurrentMap);
		}

		// Token: 0x06003D32 RID: 15666 RVA: 0x00140D0C File Offset: 0x0013EF0C
		[DebugAction("Map", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> UseGenStep()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (Type localGenStep2 in typeof(GenStep).AllSubclassesNonAbstract())
			{
				Type localGenStep = localGenStep2;
				list.Add(new DebugActionNode(localGenStep.Name, DebugActionType.Action, null, null)
				{
					action = delegate()
					{
						((GenStep)Activator.CreateInstance(localGenStep)).Generate(Find.CurrentMap, default(GenStepParams));
					}
				});
			}
			return list;
		}

		// Token: 0x06003D33 RID: 15667 RVA: 0x00140DA0 File Offset: 0x0013EFA0
		[DebugAction("Map", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void RegenSection()
		{
			Find.CurrentMap.mapDrawer.SectionAt(UI.MouseCell()).RegenerateAllLayers();
		}

		// Token: 0x06003D34 RID: 15668 RVA: 0x00140DBB File Offset: 0x0013EFBB
		[DebugAction("Map", null, false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void RegenAllMapMeshSections()
		{
			Find.CurrentMap.mapDrawer.RegenerateEverythingNow();
		}

		// Token: 0x06003D35 RID: 15669 RVA: 0x00140DCC File Offset: 0x0013EFCC
		[DebugAction("Map", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void AddSnow()
		{
			WeatherBuildupUtility.AddSnowRadial(UI.MouseCell(), Find.CurrentMap, 5f, 1f);
		}

		// Token: 0x06003D36 RID: 15670 RVA: 0x00140DE7 File Offset: 0x0013EFE7
		[DebugAction("Map", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void AddSand()
		{
			WeatherBuildupUtility.AddSandRadial(UI.MouseCell(), Find.CurrentMap, 5f, 1f);
		}

		// Token: 0x06003D37 RID: 15671 RVA: 0x00140E02 File Offset: 0x0013F002
		[DebugAction("Map", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void RemoveSnow()
		{
			WeatherBuildupUtility.AddSnowRadial(UI.MouseCell(), Find.CurrentMap, 5f, -1f);
		}

		// Token: 0x06003D38 RID: 15672 RVA: 0x00140E1D File Offset: 0x0013F01D
		[DebugAction("Map", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void RemoveSand()
		{
			WeatherBuildupUtility.AddSandRadial(UI.MouseCell(), Find.CurrentMap, 5f, -1f);
		}

		// Token: 0x06003D39 RID: 15673 RVA: 0x00140E38 File Offset: 0x0013F038
		[DebugAction("Map", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ClearAllSnow()
		{
			foreach (IntVec3 c in Find.CurrentMap.AllCells)
			{
				Find.CurrentMap.snowGrid.SetDepth(c, 0f);
			}
		}

		// Token: 0x06003D3A RID: 15674 RVA: 0x00140E98 File Offset: 0x0013F098
		[DebugAction("Map", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresOdyssey = true)]
		private static void ClearAllSand()
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			foreach (IntVec3 c in Find.CurrentMap.AllCells)
			{
				Find.CurrentMap.sandGrid.SetDepth(c, 0f);
			}
		}

		// Token: 0x06003D3B RID: 15675 RVA: 0x00140F00 File Offset: 0x0013F100
		[DebugAction("Map", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.Playing, hideInSubMenu = true)]
		private static void GenerateMap()
		{
			MapParent mapParent = (MapParent)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.Settlement);
			mapParent.Tile = TileFinder.RandomStartingTile();
			mapParent.SetFaction(Faction.OfPlayer);
			Find.WorldObjects.Add(mapParent);
			GetOrGenerateMapUtility.GetOrGenerateMap(mapParent.Tile, new IntVec3(50, 1, 50), null, null, false);
		}

		// Token: 0x06003D3C RID: 15676 RVA: 0x00140F58 File Offset: 0x0013F158
		[DebugAction("Map", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.Playing)]
		private static void DestroyMap()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				Map map = maps[i];
				list.Add(new DebugMenuOption(map.ToString(), DebugMenuOptionMode.Action, delegate()
				{
					Current.Game.DeinitAndRemoveMap(map, true);
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x06003D3D RID: 15677 RVA: 0x00140FCC File Offset: 0x0013F1CC
		[DebugAction("Map", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.Playing, hideInSubMenu = true)]
		private static void LeakMap()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				Map map = maps[i];
				list.Add(new DebugMenuOption(map.ToString(), DebugMenuOptionMode.Action, delegate()
				{
					DebugActionsMapManagement.mapLeak = map;
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x06003D3E RID: 15678 RVA: 0x0014103D File Offset: 0x0013F23D
		[DebugAction("Map", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.Playing, hideInSubMenu = true)]
		private static void PrintLeakedMap()
		{
			Log.Message(string.Format("Leaked map {0}", DebugActionsMapManagement.mapLeak));
		}

		// Token: 0x06003D3F RID: 15679 RVA: 0x00141054 File Offset: 0x0013F254
		[DebugAction("Map", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.Playing, actionType = DebugActionType.ToolMap)]
		private static void Transfer()
		{
			List<Thing> toTransfer = Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>();
			if (!toTransfer.Any<Thing>())
			{
				return;
			}
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				Map map = maps[i];
				if (map != Find.CurrentMap)
				{
					Predicate<IntVec3> <>9__1;
					list.Add(new DebugMenuOption(map.ToString(), DebugMenuOptionMode.Action, delegate()
					{
						for (int j = 0; j < toTransfer.Count; j++)
						{
							Map map;
							IntVec3 center = map.Center;
							map = map;
							int squareRadius = Mathf.Max(map.Size.x, map.Size.z);
							Predicate<IntVec3> validator;
							if ((validator = <>9__1) == null)
							{
								validator = (<>9__1 = ((IntVec3 x) => !x.Fogged(map) && x.Standable(map)));
							}
							IntVec3 center2;
							if (CellFinder.TryFindRandomCellNear(center, map, squareRadius, validator, out center2, -1))
							{
								toTransfer[j].DeSpawn(DestroyMode.Vanish);
								GenPlace.TryPlaceThing(toTransfer[j], center2, map, ThingPlaceMode.Near, null, null, null, 1);
							}
							else
							{
								Log.Error("Could not find spawn cell.");
							}
						}
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x06003D40 RID: 15680 RVA: 0x00141114 File Offset: 0x0013F314
		[DebugAction("Map", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.Playing)]
		private static void ChangeMap()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				Map map = maps[i];
				if (map != Find.CurrentMap)
				{
					list.Add(new DebugMenuOption(map.ToString(), DebugMenuOptionMode.Action, delegate()
					{
						Current.Game.CurrentMap = map;
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x06003D41 RID: 15681 RVA: 0x00141194 File Offset: 0x0013F394
		[DebugAction("Map", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.Playing)]
		private static void RegenerateCurrentMap()
		{
			RememberedCameraPos rememberedCameraPos = Find.CurrentMap.rememberedCameraPos;
			PlanetTile tile = Find.CurrentMap.Tile;
			MapParent parent = Find.CurrentMap.Parent;
			IntVec3 size = Find.CurrentMap.Size;
			bool isPocketMap = Find.CurrentMap.IsPocketMap;
			Current.Game.DeinitAndRemoveMap(Find.CurrentMap, true);
			if (isPocketMap)
			{
				Map currentMap = PocketMapUtility.GeneratePocketMap(size, parent.MapGeneratorDef, null, Find.AnyPlayerHomeMap);
				Current.Game.CurrentMap = currentMap;
			}
			else
			{
				Map orGenerateMap = GetOrGenerateMapUtility.GetOrGenerateMap(tile, size, parent.def, null, false);
				Current.Game.CurrentMap = orGenerateMap;
			}
			Find.World.renderer.wantedMode = WorldRenderMode.None;
			Find.CameraDriver.SetRootPosAndSize(rememberedCameraPos.rootPos, rememberedCameraPos.rootSize);
		}

		// Token: 0x06003D42 RID: 15682 RVA: 0x00141250 File Offset: 0x0013F450
		[DebugAction("Map", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.Playing)]
		private static void RegenerateCurrentMapStepped()
		{
			RememberedCameraPos rememberedCameraPos = Find.CurrentMap.rememberedCameraPos;
			PlanetTile tile = Find.CurrentMap.Tile;
			MapParent parent = Find.CurrentMap.Parent;
			IntVec3 size = Find.CurrentMap.Size;
			bool isPocketMap = Find.CurrentMap.IsPocketMap;
			Current.Game.DeinitAndRemoveMap(Find.CurrentMap, true);
			if (isPocketMap)
			{
				Log.Error("Pocket maps are not supported for stepped generation (yet)");
			}
			else
			{
				Map orGenerateMap = GetOrGenerateMapUtility.GetOrGenerateMap(tile, size, parent.def, null, true);
				Current.Game.CurrentMap = orGenerateMap;
			}
			Find.World.renderer.wantedMode = WorldRenderMode.None;
			Find.CameraDriver.SetRootPosAndSize(rememberedCameraPos.rootPos, rememberedCameraPos.rootSize);
		}

		// Token: 0x06003D43 RID: 15683 RVA: 0x001412F4 File Offset: 0x0013F4F4
		[DebugAction("Map", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.Playing)]
		private static void DoNextGenStep()
		{
			if (!MapGenerator.DebugDoNextGenStep(Current.Game.CurrentMap))
			{
				Log.Message("No more gen steps to do.");
			}
		}

		// Token: 0x06003D44 RID: 15684 RVA: 0x00141314 File Offset: 0x0013F514
		[DebugAction("Map", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.Playing)]
		private static void GenerateMapWithCaves()
		{
			PlanetTile tile = TileFinder.RandomSettlementTileFor(Faction.OfPlayer, false, (PlanetTile x) => Find.World.HasCaves(x));
			if (Find.CurrentMap != null)
			{
				Find.CurrentMap.Parent.Destroy();
			}
			MapParent mapParent = (MapParent)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.Settlement);
			mapParent.Tile = tile;
			mapParent.SetFaction(Faction.OfPlayer);
			Find.WorldObjects.Add(mapParent);
			Map orGenerateMap = GetOrGenerateMapUtility.GetOrGenerateMap(tile, Find.World.info.initialMapSize, null, null, false);
			Current.Game.CurrentMap = orGenerateMap;
			Find.World.renderer.wantedMode = WorldRenderMode.None;
		}

		// Token: 0x06003D45 RID: 15685 RVA: 0x001413C4 File Offset: 0x0013F5C4
		[DebugAction("Map", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.Playing)]
		private static List<DebugActionNode> RunMapGenerator()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (MapGeneratorDef defLocal2 in DefDatabase<MapGeneratorDef>.AllDefsListForReading)
			{
				MapGeneratorDef defLocal = defLocal2;
				list.Add(new DebugActionNode(defLocal.defName, DebugActionType.Action, null, null)
				{
					action = delegate()
					{
						MapParent mapParent = (MapParent)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.Settlement);
						mapParent.Tile = (from tile in Find.WorldGrid.Surface.Tiles
						where tile.PrimaryBiome.canBuildBase
						select tile).RandomElement<Tile>().tile;
						mapParent.SetFaction(Faction.OfPlayer);
						Find.WorldObjects.Add(mapParent);
						Map currentMap = MapGenerator.GenerateMap(Find.World.info.initialMapSize, mapParent, defLocal, null, null, false, false);
						Current.Game.CurrentMap = currentMap;
					}
				});
			}
			return list;
		}

		// Token: 0x06003D46 RID: 15686 RVA: 0x00141450 File Offset: 0x0013F650
		[DebugAction("Map", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.Playing)]
		private static List<DebugActionNode> GeneratePocketMap()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (MapGeneratorDef defLocal2 in DefDatabase<MapGeneratorDef>.AllDefsListForReading)
			{
				MapGeneratorDef defLocal = defLocal2;
				list.Add(new DebugActionNode(defLocal.defName, DebugActionType.Action, null, null)
				{
					action = delegate()
					{
						Map currentMap = PocketMapUtility.GeneratePocketMap(new IntVec3(100, 1, 100), defLocal, null, Find.CurrentMap);
						Current.Game.CurrentMap = currentMap;
					}
				});
			}
			return list;
		}

		// Token: 0x06003D47 RID: 15687 RVA: 0x001414DC File Offset: 0x0013F6DC
		[DebugAction("Map", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.Playing)]
		private static void ForceReformInCurrentMap()
		{
			if (Find.CurrentMap != null)
			{
				MapParent mapParent = Find.CurrentMap.Parent;
				List<Pawn> list = new List<Pawn>();
				if (Dialog_FormCaravan.AllSendablePawns(mapParent.Map, true).Any((Pawn x) => x.IsColonist))
				{
					Messages.Message("MessageYouHaveToReformCaravanNow".Translate(), new GlobalTargetInfo(mapParent.Tile), MessageTypeDefOf.NeutralEvent, true);
					Current.Game.CurrentMap = mapParent.Map;
					Dialog_FormCaravan window = new Dialog_FormCaravan(mapParent.Map, true, delegate()
					{
						if (mapParent.HasMap)
						{
							mapParent.Destroy();
						}
					}, true, null);
					Find.WindowStack.Add(window);
					return;
				}
				list.Clear();
				list.AddRange(from x in mapParent.Map.mapPawns.AllPawns
				where x.Faction == Faction.OfPlayer || x.HostFaction == Faction.OfPlayer
				select x);
				if (list.Any((Pawn x) => CaravanUtility.IsOwner(x, Faction.OfPlayer)))
				{
					CaravanExitMapUtility.ExitMapAndCreateCaravan(list, Faction.OfPlayer, mapParent.Tile, mapParent.Tile, PlanetTile.Invalid, true);
				}
				list.Clear();
				mapParent.Destroy();
			}
		}

		// Token: 0x06003D48 RID: 15688 RVA: 0x00141664 File Offset: 0x0013F864
		[DebugAction("Map", null, false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void FillMapWithTrees()
		{
			Map currentMap = Find.CurrentMap;
			foreach (IntVec3 intVec in currentMap.AllCells)
			{
				if (intVec.Standable(currentMap))
				{
					GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.Plant_TreeOak, null), intVec, currentMap, WipeMode.Vanish);
				}
			}
		}

		// Token: 0x06003D49 RID: 15689 RVA: 0x001416D0 File Offset: 0x0013F8D0
		[DebugAction("Map", null, false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresBiotech = true)]
		private static void LogMapPollution()
		{
			Log.Message("Polluted (of all possible pollutable cells): " + Find.CurrentMap.pollutionGrid.TotalPollutionPercent.ToStringPercent());
		}

		// Token: 0x06003D4A RID: 15690 RVA: 0x001416F8 File Offset: 0x0013F8F8
		[DebugAction("Map", null, false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> RegenerateMapWithLandmark()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (LandmarkDef defLocal2 in DefDatabase<LandmarkDef>.AllDefsListForReading)
			{
				LandmarkDef defLocal = defLocal2;
				list.Add(new DebugActionNode(defLocal.defName, DebugActionType.Action, null, null)
				{
					action = delegate()
					{
						RememberedCameraPos rememberedCameraPos = Find.CurrentMap.rememberedCameraPos;
						PlanetTile tile = Find.CurrentMap.Tile;
						MapParent parent = Find.CurrentMap.Parent;
						IntVec3 size = Find.CurrentMap.Size;
						Current.Game.DeinitAndRemoveMap(Find.CurrentMap, true);
						Find.WorldGrid[tile].mutatorsNullable = new List<TileMutatorDef>();
						Find.World.landmarks.AddLandmark(defLocal, tile, null, true);
						Map orGenerateMap = GetOrGenerateMapUtility.GetOrGenerateMap(tile, size, parent.def, null, false);
						Current.Game.CurrentMap = orGenerateMap;
						Find.World.renderer.wantedMode = WorldRenderMode.None;
						Find.CameraDriver.SetRootPosAndSize(rememberedCameraPos.rootPos, rememberedCameraPos.rootSize);
					}
				});
			}
			return list;
		}

		// Token: 0x06003D4B RID: 15691 RVA: 0x00141784 File Offset: 0x0013F984
		[DebugAction("Map", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void GenerateLandmarkScreenshots()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (LandmarkDef landmarkDef2 in DefDatabase<LandmarkDef>.AllDefsListForReading)
			{
				LandmarkDef landmarkDef = landmarkDef2;
				list.Add(new DebugMenuOption(landmarkDef.defName, DebugMenuOptionMode.Action, delegate()
				{
					List<DebugMenuOption> list2 = new List<DebugMenuOption>();
					foreach (int num in Enumerable.Range(0, 10))
					{
						float localCount = (float)(num * 5);
						if (num == 0)
						{
							localCount = 1f;
						}
						list2.Add(new DebugMenuOption(localCount.ToString() + " screenshots", DebugMenuOptionMode.Action, delegate()
						{
							List<DebugMenuOption> list3 = new List<DebugMenuOption>();
							int[] mapSizes = Dialog_AdvancedGameConfig.MapSizes;
							for (int i = 0; i < mapSizes.Length; i++)
							{
								int localSize2 = mapSizes[i];
								int localSize = localSize2;
								list3.Add(new DebugMenuOption(string.Format("{0} x {1}", localSize, localSize), DebugMenuOptionMode.Action, delegate()
								{
									Find.UIRoot.screenshotMode.Active = true;
									Enumerable.Range(0, Find.WorldGrid.TilesCount - 1).ToList<int>().Shuffle<int>();
									WorldObject worldObject = null;
									StringBuilder log = new StringBuilder();
									log.AppendLine("Landmark info:");
									log.AppendLine("Seed: " + Find.World.info.Seed.ToString());
									int num2 = 0;
									foreach (SurfaceTile surfaceTile in Find.WorldGrid.Tiles.InRandomOrder(null))
									{
										PlanetTile tile = surfaceTile.tile;
										if (!Find.WorldObjects.AnyWorldObjectAt(tile) && landmarkDef.IsValidTile(tile, tile.Layer, false))
										{
											if ((float)num2 >= localCount)
											{
												break;
											}
											num2++;
											LongEventHandler.QueueLongEvent(delegate()
											{
												Find.World.landmarks.AddLandmark(landmarkDef, tile, null, false);
												Current.Game.DeinitAndRemoveMap(Find.CurrentMap, false);
												WorldObject worldObject = worldObject;
												if (worldObject != null)
												{
													worldObject.Destroy();
												}
												Find.World.renderer.wantedMode = WorldRenderMode.None;
												worldObject = (MapParent)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.Settlement);
												worldObject.Tile = tile;
												worldObject.SetFaction(Faction.OfPlayer);
												Find.WorldObjects.Add(worldObject);
												Map orGenerateMap = GetOrGenerateMapUtility.GetOrGenerateMap(worldObject.Tile, new IntVec3(localSize, 1, localSize), null, null, false);
												Current.Game.CurrentMap = orGenerateMap;
												orGenerateMap.fogGrid.ClearAllFog();
											}, null, false, null, true, true, null);
											int ssIndex = num2;
											LongEventHandler.QueueLongEvent(delegate()
											{
												Find.CameraDriver.config.sizeRange.max = 150f;
												Find.CameraDriver.SetRootPosAndSize(Find.CurrentMap.Center.ToVector3(), 150f);
												string text = string.Format("{0}_{1}_{2}", landmarkDef.defName, Find.CurrentMap.Biome.defName, ssIndex);
												log.AppendLine(text);
												log.AppendLine(string.Format("   tile ID: {0}, mutators: {1}", tile, (from x in Find.CurrentMap.TileInfo.Mutators
												select x.defName).ToCommaList(false, false)));
												ScreenshotTaker.TakeNonSteamShot(text);
											}, null, false, null, true, true, null);
										}
									}
									LongEventHandler.QueueLongEvent(delegate()
									{
										Log.Message(log.ToString());
										Find.UIRoot.screenshotMode.Active = false;
									}, null, false, null, true, false, null);
								}));
							}
							Find.WindowStack.Add(new Dialog_DebugOptionListLister(list3, null));
						}));
					}
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2, null));
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x06003D4C RID: 15692 RVA: 0x00141818 File Offset: 0x0013FA18
		[DebugAction("Map", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void AddMutatorToCurrentMapTile()
		{
			Tile tile = Find.CurrentMap.TileInfo;
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			using (List<TileMutatorDef>.Enumerator enumerator = DefDatabase<TileMutatorDef>.AllDefsListForReading.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					TileMutatorDef mutatorDef = enumerator.Current;
					list.Add(new DebugMenuOption(mutatorDef.defName, DebugMenuOptionMode.Action, delegate()
					{
						tile.AddMutator(mutatorDef);
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x06003D4D RID: 15693 RVA: 0x001418C8 File Offset: 0x0013FAC8
		[DebugAction("Map", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void RemoveMutatorFromCurrentMapTile()
		{
			Tile tile = Find.CurrentMap.TileInfo;
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			using (IEnumerator<TileMutatorDef> enumerator = tile.Mutators.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					TileMutatorDef mutatorDef = enumerator.Current;
					list.Add(new DebugMenuOption(mutatorDef.defName, DebugMenuOptionMode.Action, delegate()
					{
						tile.RemoveMutator(mutatorDef);
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x04002C07 RID: 11271
		private static Map mapLeak;
	}
}
