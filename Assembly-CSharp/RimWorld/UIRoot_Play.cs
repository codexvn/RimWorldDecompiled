using System;
using LudeonTK;
using UnityEngine;
using Verse;
using Verse.Steam;

namespace RimWorld
{
	// Token: 0x020034F4 RID: 13556
	public class UIRoot_Play : UIRoot
	{
		// Token: 0x060130BC RID: 78012 RVA: 0x005A509D File Offset: 0x005A329D
		public override void Init()
		{
			base.Init();
			Messages.Clear();
			this.debugWindowOpener.TryOpenOrClosePalette();
		}

		// Token: 0x060130BD RID: 78013 RVA: 0x005A50B8 File Offset: 0x005A32B8
		public override void UIRootOnGUI()
		{
			base.UIRootOnGUI();
			Find.GameInfo.GameInfoOnGUI();
			Find.World.UI.WorldInterfaceOnGUI();
			this.mapUI.MapInterfaceOnGUI_BeforeMainTabs();
			if (!this.screenshotMode.FiltersCurrentEvent)
			{
				this.mainButtonsRoot.MainButtonsOnGUI();
				this.alerts.AlertsReadoutOnGUI();
			}
			this.mapUI.MapInterfaceOnGUI_AfterMainTabs();
			if (!this.screenshotMode.FiltersCurrentEvent)
			{
				Find.Tutor.TutorOnGUI();
			}
			Find.World.WorldOnGUI();
			ReorderableWidget.ReorderableWidgetOnGUI_BeforeWindowStack();
			DragAndDropWidget.DragAndDropWidgetOnGUI_BeforeWindowStack();
			this.windows.WindowStackOnGUI();
			DragAndDropWidget.DragAndDropWidgetOnGUI_AfterWindowStack();
			ReorderableWidget.ReorderableWidgetOnGUI_AfterWindowStack();
			Widgets.WidgetsOnGUI();
			this.mapUI.HandleMapClicks();
			if (Find.DesignatorManager.SelectedDesignator != null)
			{
				Find.DesignatorManager.SelectedDesignator.SelectedProcessInput(Event.current);
			}
			DebugTools.DebugToolsOnGUI();
			this.mainButtonsRoot.HandleLowPriorityShortcuts();
			Find.World.UI.HandleLowPriorityInput();
			this.mapUI.HandleLowPriorityInput();
			this.OpenMainMenuShortcut();
			SteamDeck.ShowSteamDeckGameControlsIfNotKnown();
		}

		// Token: 0x060130BE RID: 78014 RVA: 0x005A51C4 File Offset: 0x005A33C4
		public override void UIRootUpdate()
		{
			base.UIRootUpdate();
			try
			{
				Find.World.UI.WorldInterfaceUpdate();
				this.mapUI.MapInterfaceUpdate();
				this.alerts.AlertsReadoutUpdate();
				LessonAutoActivator.LessonAutoActivatorUpdate();
				Find.Tutor.TutorUpdate();
			}
			catch (Exception ex)
			{
				Log.Error("Exception in UIRootUpdate: " + ex.ToString());
			}
		}

		// Token: 0x060130BF RID: 78015 RVA: 0x005A5238 File Offset: 0x005A3438
		private void OpenMainMenuShortcut()
		{
			if ((Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape) || KeyBindingDefOf.Cancel.KeyDownEvent)
			{
				Event.current.Use();
				Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Menu, true);
			}
		}

		// Token: 0x0400C556 RID: 50518
		public MapInterface mapUI = new MapInterface();

		// Token: 0x0400C557 RID: 50519
		public MainButtonsRoot mainButtonsRoot = new MainButtonsRoot();

		// Token: 0x0400C558 RID: 50520
		public AlertsReadout alerts = new AlertsReadout();
	}
}
