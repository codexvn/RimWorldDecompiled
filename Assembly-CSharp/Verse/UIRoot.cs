using System;
using LudeonTK;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.Noise;
using Verse.Sound;
using Verse.Steam;

namespace Verse
{
	// Token: 0x02000BE2 RID: 3042
	public abstract class UIRoot
	{
		// Token: 0x17000C00 RID: 3072
		// (get) Token: 0x06004DD8 RID: 19928 RVA: 0x0018BB3E File Offset: 0x00189D3E
		public bool HideMotes
		{
			get
			{
				return WorldComponent_GravshipController.CutsceneInProgress;
			}
		}

		// Token: 0x06004DD9 RID: 19929 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Init()
		{
		}

		// Token: 0x06004DDA RID: 19930 RVA: 0x0018BB4C File Offset: 0x00189D4C
		public virtual void UIRootOnGUI()
		{
			DebugInputLogger.InputLogOnGUI();
			UnityGUIBugsFixer.OnGUI();
			SteamDeck.OnGUI();
			SteamDeck.RootOnGUI();
			OriginalEventUtility.RecordOriginalEvent(Event.current);
			Text.StartOfOnGUI();
			this.CheckOpenLogWindow();
			DelayedErrorWindowRequest.DelayedErrorWindowRequestOnGUI();
			if (!this.screenshotMode.FiltersCurrentEvent)
			{
				this.debugWindowOpener.DevToolStarterOnGUI();
			}
			this.windows.HandleEventsHighPriority();
			this.screenshotMode.ScreenshotModesOnGUI();
			if (!this.screenshotMode.FiltersCurrentEvent)
			{
				TooltipHandler.DoTooltipGUI();
				this.feedbackFloaters.FeedbackOnGUI();
				DragSliderManager.DragSlidersOnGUI();
				Messages.MessagesDoGUI();
			}
			this.shortcutKeys.ShortcutKeysOnGUI();
			NoiseDebugUI.NoiseDebugOnGUI();
			Debug.developerConsoleVisible = false;
			if (Current.Game != null)
			{
				GameComponentUtility.GameComponentOnGUI();
				CellInspectorDrawer.OnGUI();
			}
			World world = Find.World;
			if (world != null)
			{
				world.WorldOnGUI();
			}
			OriginalEventUtility.Reset();
		}

		// Token: 0x06004DDB RID: 19931 RVA: 0x0018BC18 File Offset: 0x00189E18
		public virtual void UIRootUpdate()
		{
			ScreenshotTaker.Update();
			DragSliderManager.DragSlidersUpdate();
			this.windows.WindowsUpdate();
			MouseoverSounds.ResolveFrame();
			UIHighlighter.UIHighlighterUpdate();
			Messages.Update();
			CellInspectorDrawer.Update();
		}

		// Token: 0x06004DDC RID: 19932 RVA: 0x0018BC43 File Offset: 0x00189E43
		private void CheckOpenLogWindow()
		{
			if (EditWindow_Log.wantsToOpen && !Find.WindowStack.IsOpen(typeof(EditWindow_Log)))
			{
				Find.WindowStack.Add(new EditWindow_Log());
				EditWindow_Log.wantsToOpen = false;
			}
		}

		// Token: 0x04003743 RID: 14147
		public WindowStack windows = new WindowStack();

		// Token: 0x04003744 RID: 14148
		public DebugWindowsOpener debugWindowOpener = new DebugWindowsOpener();

		// Token: 0x04003745 RID: 14149
		public ScreenshotModeHandler screenshotMode = new ScreenshotModeHandler();

		// Token: 0x04003746 RID: 14150
		private ShortcutKeys shortcutKeys = new ShortcutKeys();

		// Token: 0x04003747 RID: 14151
		public FeedbackFloaters feedbackFloaters = new FeedbackFloaters();
	}
}
