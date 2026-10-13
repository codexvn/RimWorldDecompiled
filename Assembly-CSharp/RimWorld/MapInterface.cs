using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02003428 RID: 13352
	public class MapInterface
	{
		// Token: 0x06012C90 RID: 76944 RVA: 0x00593D88 File Offset: 0x00591F88
		public void MapInterfaceOnGUI_BeforeMainTabs()
		{
			if (Find.CurrentMap == null)
			{
				return;
			}
			if (WorldRendererUtility.DrawingMap)
			{
				ScreenshotModeHandler screenshotMode = Find.UIRoot.screenshotMode;
				this.thingOverlays.ThingOverlaysOnGUI();
				Find.CurrentMap.MapOnGUI();
				MapComponentUtility.MapComponentOnGUI(Find.CurrentMap);
				BeautyDrawer.BeautyDrawerOnGUI();
				if (!screenshotMode.FiltersCurrentEvent)
				{
					this.colonistBar.ColonistBarOnGUI();
				}
				this.selector.dragBox.DragBoxOnGUI();
				this.designatorManager.DesignationManagerOnGUI();
				this.targeter.TargeterOnGUI();
				this.selector.SelectorOnGUI_BeforeMainTabs();
				Find.CurrentMap.tooltipGiverList.DispenseAllThingTooltips();
				Find.CurrentMap.flecks.FleckManagerOnGUI();
				if (DebugViewSettings.drawFoodSearchFromMouse)
				{
					FoodUtility.DebugFoodSearchFromMouse_OnGUI();
				}
				if (DebugViewSettings.drawNonCombatantTimer)
				{
					AttackTargetFinder.DebugDrawNonCombatantTimer_OnGUI();
				}
				if (DebugViewSettings.drawAttackTargetScores)
				{
					AttackTargetFinder.DebugDrawAttackTargetScores_OnGUI();
				}
				if (ModsConfig.OdysseyActive && GravshipUtility.ShowConnectedSubstructure)
				{
					using (new ProfilerBlock("DrawSubstructureCountOnGUI()"))
					{
						SubstructureGrid.DrawSubstructureCountOnGUI();
					}
				}
				if (!screenshotMode.FiltersCurrentEvent)
				{
					this.mouseoverReadout.MouseoverReadoutOnGUI();
					this.globalControls.GlobalControlsOnGUI();
					this.resourceReadout.ResourceReadoutOnGUI();
					MapGizmoUtility.MapUIOnGUI();
					return;
				}
			}
			else
			{
				this.targeter.StopTargeting();
			}
		}

		// Token: 0x06012C91 RID: 76945 RVA: 0x00593ED8 File Offset: 0x005920D8
		public void MapInterfaceOnGUI_AfterMainTabs()
		{
			if (Find.CurrentMap == null)
			{
				return;
			}
			if (!WorldRendererUtility.DrawingMap)
			{
				return;
			}
			if (!Find.UIRoot.screenshotMode.FiltersCurrentEvent)
			{
				EnvironmentStatsDrawer.EnvironmentStatsOnGUI();
				Find.CurrentMap.deepResourceGrid.DeepResourcesOnGUI();
				Find.CurrentMap.debugDrawer.DebugDrawerOnGUI();
			}
		}

		// Token: 0x06012C92 RID: 76946 RVA: 0x00593F29 File Offset: 0x00592129
		public void HandleMapClicks()
		{
			if (Find.CurrentMap == null)
			{
				return;
			}
			if (!WorldRendererUtility.DrawingMap)
			{
				return;
			}
			this.designatorManager.ProcessInputEvents();
			this.targeter.ProcessInputEvents();
		}

		// Token: 0x06012C93 RID: 76947 RVA: 0x00593F51 File Offset: 0x00592151
		public void HandleLowPriorityInput()
		{
			if (Find.CurrentMap == null)
			{
				return;
			}
			if (!WorldRendererUtility.DrawingMap)
			{
				return;
			}
			this.selector.SelectorOnGUI();
			Find.CurrentMap.lordManager.LordManagerOnGUI();
		}

		// Token: 0x06012C94 RID: 76948 RVA: 0x00593F80 File Offset: 0x00592180
		public void MapInterfaceUpdate()
		{
			if (Find.CurrentMap == null)
			{
				return;
			}
			if (!WorldRendererUtility.DrawingMap)
			{
				return;
			}
			this.targeter.TargeterUpdate();
			SelectionDrawer.DrawSelectionOverlays();
			EnvironmentStatsDrawer.DrawRoomOverlays();
			this.designatorManager.DesignatorManagerUpdate();
			this.selector.gotoController.Draw();
			Find.CurrentMap.roofGrid.RoofGridUpdate();
			Find.CurrentMap.fertilityGrid.FertilityGridUpdate();
			if (ModsConfig.BiotechActive)
			{
				Find.CurrentMap.pollutionGrid.PollutionGridUpdate();
			}
			using (new ProfilerBlock("MapComponentOnDraw()"))
			{
				MapComponentUtility.MapComponentOnDraw(Find.CurrentMap);
			}
			Find.CurrentMap.pathFinder.OnDraw();
			if (ModsConfig.OdysseyActive)
			{
				using (new ProfilerBlock("SubstructureGridUpdate()"))
				{
					SubstructureGrid substructureGrid = Find.CurrentMap.substructureGrid;
					if (substructureGrid != null)
					{
						substructureGrid.DrawSubstructureGrid();
					}
				}
				if (GravshipUtility.ShowConnectedSubstructure)
				{
					using (new ProfilerBlock("SubstructureGridUpdate()"))
					{
						SubstructureGrid.DrawSubstructureFootprint(null, null);
					}
				}
			}
			Find.CurrentMap.terrainGrid.TerrainGridUpdate();
			Find.CurrentMap.exitMapGrid.ExitMapGridUpdate();
			Find.CurrentMap.deepResourceGrid.DeepResourceGridUpdate();
			Find.CurrentMap.mapTemperature.TemperatureUpdate();
			MapGizmoUtility.MapUIUpdate();
			if (DebugViewSettings.drawPawnDebug)
			{
				Find.CurrentMap.pawnDestinationReservationManager.DebugDrawDestinations();
				Find.CurrentMap.reservationManager.DebugDrawReservations();
			}
			if (DebugViewSettings.drawDestReservations)
			{
				Find.CurrentMap.pawnDestinationReservationManager.DebugDrawReservations();
			}
			if (DebugViewSettings.drawFoodSearchFromMouse)
			{
				FoodUtility.DebugFoodSearchFromMouse_Update();
			}
			if (DebugViewSettings.drawPreyInfo)
			{
				FoodUtility.DebugDrawPredatorFoodSource();
			}
			if (DebugViewSettings.drawAttackTargetScores)
			{
				AttackTargetFinder.DebugDrawAttackTargetScores_Update();
			}
			if (DebugViewSettings.drawFOVSymmetry)
			{
				GenSight.DebugDrawFOVSymmetry_Update();
			}
			MiscDebugDrawer.DebugDrawInteractionCells();
			Find.CurrentMap.debugDrawer.DebugDrawerUpdate();
			Find.CurrentMap.regionGrid.DebugDraw();
			InfestationCellFinder.DebugDraw();
			LargeBuildingCellFinder.DebugDraw();
			StealAIDebugDrawer.DebugDraw();
			MapGenerator.DebugDraw();
			Find.CurrentMap.waterInfo.DebugDrawRiver();
			BuildingsDamageSectionLayerUtility.DebugDraw();
			WaterBodyTracker waterBodyTracker = Find.CurrentMap.waterBodyTracker;
			if (waterBodyTracker == null)
			{
				return;
			}
			waterBodyTracker.DebugDraw();
		}

		// Token: 0x06012C95 RID: 76949 RVA: 0x005941CC File Offset: 0x005923CC
		public void Notify_SwitchedMap()
		{
			this.designatorManager.Deselect();
			this.reverseDesignatorDatabase.Reinit();
			this.selector.ClearSelection();
			this.selector.dragBox.active = false;
			this.selector.gotoController.Deactivate();
			this.targeter.StopTargeting();
			Designator_AreaAllowed.ClearSelectedArea();
			MainButtonDef openTab = Find.MainTabsRoot.OpenTab;
			List<MainButtonDef> allDefsListForReading = DefDatabase<MainButtonDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				allDefsListForReading[i].Notify_SwitchedMap();
			}
			if (openTab != null && openTab != MainButtonDefOf.Inspect)
			{
				Find.MainTabsRoot.SetCurrentTab(openTab, false);
			}
			if (Find.CurrentMap != null)
			{
				RememberedCameraPos rememberedCameraPos = Find.CurrentMap.rememberedCameraPos;
				Find.CameraDriver.SetRootPosAndSize(rememberedCameraPos.rootPos, rememberedCameraPos.rootSize);
			}
		}

		// Token: 0x0400C33B RID: 49979
		public ThingOverlays thingOverlays = new ThingOverlays();

		// Token: 0x0400C33C RID: 49980
		public Selector selector = new Selector();

		// Token: 0x0400C33D RID: 49981
		public Targeter targeter = new Targeter();

		// Token: 0x0400C33E RID: 49982
		public DesignatorManager designatorManager = new DesignatorManager();

		// Token: 0x0400C33F RID: 49983
		public ReverseDesignatorDatabase reverseDesignatorDatabase = new ReverseDesignatorDatabase();

		// Token: 0x0400C340 RID: 49984
		private MouseoverReadout mouseoverReadout = new MouseoverReadout();

		// Token: 0x0400C341 RID: 49985
		public GlobalControls globalControls = new GlobalControls();

		// Token: 0x0400C342 RID: 49986
		protected ResourceReadout resourceReadout = new ResourceReadout();

		// Token: 0x0400C343 RID: 49987
		public ColonistBar colonistBar = new ColonistBar();
	}
}
