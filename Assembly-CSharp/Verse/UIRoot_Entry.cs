using System;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.Steam;

namespace Verse
{
	// Token: 0x02000B71 RID: 2929
	public class UIRoot_Entry : UIRoot
	{
		// Token: 0x17000B6E RID: 2926
		// (get) Token: 0x06004B1E RID: 19230 RVA: 0x0017E0F0 File Offset: 0x0017C2F0
		private bool ShouldDoMainMenu
		{
			get
			{
				if (LongEventHandler.AnyEventNowOrWaiting)
				{
					return false;
				}
				for (int i = 0; i < Find.WindowStack.Count; i++)
				{
					if (this.windows[i].layer == WindowLayer.Dialog && !Find.WindowStack[i].IsDebug)
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x06004B1F RID: 19231 RVA: 0x0017E144 File Offset: 0x0017C344
		public override void Init()
		{
			base.Init();
			UIMenuBackgroundManager.background = new UI_BackgroundMain();
			MainMenuDrawer.Init();
			QuickStarter.CheckQuickStart();
			VersionUpdateDialogMaker.CreateVersionUpdateDialogIfNecessary();
			if (!SteamManager.Initialized)
			{
				Dialog_MessageBox window = new Dialog_MessageBox("SteamClientMissing".Translate(), "Quit".Translate(), delegate()
				{
					Application.Quit();
				}, "Ignore".Translate(), null, null, false, null, null, WindowLayer.Dialog);
				Find.WindowStack.Add(window);
			}
		}

		// Token: 0x06004B20 RID: 19232 RVA: 0x0017E1E0 File Offset: 0x0017C3E0
		public override void UIRootOnGUI()
		{
			base.UIRootOnGUI();
			if (Find.World != null)
			{
				Find.World.UI.WorldInterfaceOnGUI();
			}
			this.DoMainMenu();
			if (Current.Game != null)
			{
				Find.Tutor.TutorOnGUI();
			}
			ReorderableWidget.ReorderableWidgetOnGUI_BeforeWindowStack();
			DragAndDropWidget.DragAndDropWidgetOnGUI_BeforeWindowStack();
			this.windows.WindowStackOnGUI();
			DragAndDropWidget.DragAndDropWidgetOnGUI_AfterWindowStack();
			ReorderableWidget.ReorderableWidgetOnGUI_AfterWindowStack();
			Widgets.WidgetsOnGUI();
			if (Find.World != null)
			{
				Find.World.UI.HandleLowPriorityInput();
			}
		}

		// Token: 0x06004B21 RID: 19233 RVA: 0x0017E25A File Offset: 0x0017C45A
		public override void UIRootUpdate()
		{
			base.UIRootUpdate();
			if (Find.World != null)
			{
				Find.World.UI.WorldInterfaceUpdate();
			}
			if (Current.Game != null)
			{
				LessonAutoActivator.LessonAutoActivatorUpdate();
				Find.Tutor.TutorUpdate();
			}
		}

		// Token: 0x06004B22 RID: 19234 RVA: 0x0017E28E File Offset: 0x0017C48E
		private void DoMainMenu()
		{
			if (!WorldRendererUtility.WorldSelected)
			{
				UIMenuBackgroundManager.background.BackgroundOnGUI();
				if (this.ShouldDoMainMenu)
				{
					Current.Game = null;
					MainMenuDrawer.MainMenuOnGUI();
				}
			}
		}
	}
}
