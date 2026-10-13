using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020034F7 RID: 13559
	public class WorldInterface
	{
		// Token: 0x170031A1 RID: 12705
		// (get) Token: 0x060130D2 RID: 78034 RVA: 0x005A6094 File Offset: 0x005A4294
		// (set) Token: 0x060130D3 RID: 78035 RVA: 0x005A60A1 File Offset: 0x005A42A1
		public PlanetTile SelectedTile
		{
			get
			{
				return this.selector.SelectedTile;
			}
			set
			{
				this.selector.SelectedTile = value;
			}
		}

		// Token: 0x060130D4 RID: 78036 RVA: 0x005A60B0 File Offset: 0x005A42B0
		public void Reset()
		{
			this.everReset = true;
			this.inspectPane.Reset();
			if (Current.ProgramState == ProgramState.Playing)
			{
				this.SelectedTile = ((Find.CurrentMap != null) ? Find.CurrentMap.Tile : PlanetTile.Invalid);
			}
			else if (Find.GameInitData != null)
			{
				if (Find.GameInitData.startingTile.Valid && Find.World != null && !Find.WorldGrid.InBounds(Find.GameInitData.startingTile))
				{
					Log.Error("Map world tile was out of bounds.");
					Find.GameInitData.startingTile = PlanetTile.Invalid;
				}
				this.SelectedTile = Find.GameInitData.startingTile;
				this.inspectPane.OpenTabType = typeof(WITab_Terrain);
			}
			else
			{
				this.SelectedTile = PlanetTile.Invalid;
			}
			if (this.SelectedTile.Valid)
			{
				Find.WorldCameraDriver.JumpTo(this.SelectedTile);
			}
			else
			{
				Find.WorldCameraDriver.JumpTo(Find.WorldGrid.SurfaceViewCenter);
			}
			Find.WorldCameraDriver.ResetAltitude();
		}

		// Token: 0x060130D5 RID: 78037 RVA: 0x005A61BC File Offset: 0x005A43BC
		public void WorldInterfaceUpdate()
		{
			if (WorldRendererUtility.WorldSelected)
			{
				this.targeter.TargeterUpdate();
				WorldSelectionDrawer.DrawSelectionOverlays();
				if (this.tilePicker.Active)
				{
					this.tilePicker.TileSelectorUpdate();
				}
				Find.WorldDebugDrawer.WorldDebugDrawerUpdate();
			}
			else
			{
				this.targeter.StopTargeting();
				this.tilePicker.StopTargeting();
			}
			this.routePlanner.WorldRoutePlannerUpdate();
			WorldGizmoUtility.WorldUIUpdate();
		}

		// Token: 0x060130D6 RID: 78038 RVA: 0x005A622C File Offset: 0x005A442C
		public void WorldInterfaceOnGUI()
		{
			this.CheckOpenOrCloseInspectPane();
			if (WorldRendererUtility.WorldSelected && Find.WorldCamera.gameObject.activeInHierarchy)
			{
				ScreenshotModeHandler screenshotMode = Find.UIRoot.screenshotMode;
				ExpandableWorldObjectsUtility.ExpandableWorldObjectsOnGUI();
				if (ModsConfig.OdysseyActive)
				{
					ExpandableLandmarksUtility.ExpandableLandmarksOnGUI();
				}
				WorldSelectionDrawer.SelectionOverlaysOnGUI();
				this.routePlanner.WorldRoutePlannerOnGUI();
				if (!screenshotMode.FiltersCurrentEvent && Current.ProgramState == ProgramState.Playing)
				{
					Find.ColonistBar.ColonistBarOnGUI();
				}
				this.selector.dragBox.DragBoxOnGUI();
				this.targeter.TargeterOnGUI();
				if (this.tilePicker.Active)
				{
					this.tilePicker.TileSelectorOnGUI();
				}
				if (!screenshotMode.FiltersCurrentEvent)
				{
					this.globalControls.WorldGlobalControlsOnGUI();
				}
				WorldGizmoUtility.WorldUIOnGUI();
				Find.WorldDebugDrawer.WorldDebugDrawerOnGUI();
			}
		}

		// Token: 0x060130D7 RID: 78039 RVA: 0x005A62F6 File Offset: 0x005A44F6
		public void HandleLowPriorityInput()
		{
			if (WorldRendererUtility.WorldSelected)
			{
				this.targeter.ProcessInputEvents();
				this.selector.WorldSelectorOnGUI();
			}
		}

		// Token: 0x060130D8 RID: 78040 RVA: 0x005A6318 File Offset: 0x005A4518
		private void CheckOpenOrCloseInspectPane()
		{
			if (this.selector.AnyObjectOrTileSelected && WorldRendererUtility.WorldSelected && (Current.ProgramState != ProgramState.Playing || Find.MainTabsRoot.OpenTab == null))
			{
				if (!Find.WindowStack.IsOpen<WorldInspectPane>())
				{
					Find.WindowStack.Add(this.inspectPane);
					return;
				}
			}
			else if (Find.WindowStack.IsOpen<WorldInspectPane>())
			{
				Find.WindowStack.TryRemove(this.inspectPane, false);
			}
		}

		// Token: 0x0400C579 RID: 50553
		public WorldSelector selector = new WorldSelector();

		// Token: 0x0400C57A RID: 50554
		public WorldTargeter targeter = new WorldTargeter();

		// Token: 0x0400C57B RID: 50555
		public WorldInspectPane inspectPane = new WorldInspectPane();

		// Token: 0x0400C57C RID: 50556
		public WorldGlobalControls globalControls = new WorldGlobalControls();

		// Token: 0x0400C57D RID: 50557
		public WorldRoutePlanner routePlanner = new WorldRoutePlanner();

		// Token: 0x0400C57E RID: 50558
		public TilePicker tilePicker = new TilePicker();

		// Token: 0x0400C57F RID: 50559
		public bool everReset;
	}
}
