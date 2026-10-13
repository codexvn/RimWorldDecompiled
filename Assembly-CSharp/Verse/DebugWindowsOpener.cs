using System;
using LudeonTK;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000B30 RID: 2864
	public class DebugWindowsOpener
	{
		// Token: 0x06004A46 RID: 19014 RVA: 0x0017A0CA File Offset: 0x001782CA
		public DebugWindowsOpener()
		{
			this.drawButtonsCached = new Action(this.DrawButtons);
		}

		// Token: 0x06004A47 RID: 19015 RVA: 0x0017A0F0 File Offset: 0x001782F0
		public void DevToolStarterOnGUI()
		{
			if (!Prefs.DevMode)
			{
				return;
			}
			Vector2 vector = new Vector2((float)UI.screenWidth * 0.5f - this.widgetRowFinalX * 0.5f, 3f);
			float height = 25f;
			Find.WindowStack.ImmediateWindow(1593759361, new Rect(vector.x, vector.y, this.widgetRowFinalX, height).Rounded(), WindowLayer.GameUI, this.drawButtonsCached, false, false, 0f, null, false);
			if (KeyBindingDefOf.Dev_ToggleDebugLog.KeyDownEvent)
			{
				this.ToggleLogWindow();
				Event.current.Use();
			}
			if (KeyBindingDefOf.Dev_ToggleDebugActionsMenu.KeyDownEvent)
			{
				this.ToggleDebugActionsMenu();
				Event.current.Use();
			}
			if (KeyBindingDefOf.Dev_ToggleDebugLogMenu.KeyDownEvent)
			{
				this.ToggleDebugLogMenu();
				Event.current.Use();
			}
			if (KeyBindingDefOf.Dev_ToggleDebugSettingsMenu.KeyDownEvent)
			{
				this.ToggleDebugSettingsMenu();
				Event.current.Use();
			}
			if (KeyBindingDefOf.Dev_ToggleDevPalette.KeyDownEvent && Current.ProgramState == ProgramState.Playing)
			{
				DebugSettings.devPalette = !DebugSettings.devPalette;
				this.TryOpenOrClosePalette();
				Event.current.Use();
			}
			if (KeyBindingDefOf.Dev_ToggleDebugInspector.KeyDownEvent)
			{
				this.ToggleDebugInspector();
				Event.current.Use();
			}
			if (Current.ProgramState == ProgramState.Playing && KeyBindingDefOf.Dev_ToggleGodMode.KeyDownEvent)
			{
				this.ToggleGodMode();
				Event.current.Use();
			}
		}

		// Token: 0x06004A48 RID: 19016 RVA: 0x0017A250 File Offset: 0x00178450
		private void DrawButtons()
		{
			this.widgetRow.Init(0f, 0f, UIDirection.RightThenUp, 99999f, 4f);
			if (this.widgetRow.ButtonIcon(TexButton.ToggleLog, "Open the debug log.", null, null, null, true, -1f))
			{
				this.ToggleLogWindow();
			}
			if (this.widgetRow.ButtonIcon(TexButton.ToggleTweak, "Open tweakvalues menu.\n\nThis lets you change internal values.", null, null, null, true, -1f))
			{
				this.ToggleTweakValuesMenu();
			}
			if (this.widgetRow.ButtonIcon(TexButton.OpenDebugActionsMenu, "Open debug actions menu.\n\nThis lets you spawn items and force various events.", null, null, null, true, -1f))
			{
				this.ToggleDebugActionsMenu();
			}
			if (this.widgetRow.ButtonIcon(TexButton.OpenInspectSettings, "Open the view settings.\n\nThis lets you see special debug visuals.", null, null, null, true, -1f))
			{
				this.ToggleDebugSettingsMenu();
			}
			if (this.widgetRow.ButtonIcon(TexButton.OpenDebugActionsMenu, "Open debug logging menu.", null, null, null, true, -1f))
			{
				this.ToggleDebugLogMenu();
			}
			if (this.widgetRow.ButtonIcon(TexButton.OpenInspector, "Open the inspector.\n\nThis lets you inspect what's happening in the game, down to individual variables.", null, null, null, true, -1f))
			{
				this.ToggleDebugInspector();
			}
			if (Current.ProgramState == ProgramState.Playing)
			{
				bool devPalette = DebugSettings.devPalette;
				this.widgetRow.ToggleableIcon(ref devPalette, TexButton.ToggleDevPalette, "Toggle the dev palette.\n\nAllows you to setup a palette of debug actions for ease of use.", null, null);
				if (devPalette != DebugSettings.devPalette)
				{
					DebugSettings.devPalette = devPalette;
					this.TryOpenOrClosePalette();
				}
				if (this.widgetRow.ButtonIcon(DebugSettings.godMode ? TexButton.GodModeEnabled : TexButton.GodModeDisabled, "Toggle god mode.\n\nWhen god mode is on, you can build stuff instantly, for free, and sell things that aren't yours.", null, null, null, true, -1f))
				{
					this.ToggleGodMode();
				}
			}
			this.widgetRowFinalX = this.widgetRow.FinalX;
		}

		// Token: 0x06004A49 RID: 19017 RVA: 0x0017A489 File Offset: 0x00178689
		private void ToggleLogWindow()
		{
			if (!Find.WindowStack.TryRemove(typeof(EditWindow_Log), true))
			{
				Find.WindowStack.Add(new EditWindow_Log());
			}
		}

		// Token: 0x06004A4A RID: 19018 RVA: 0x0017A4B4 File Offset: 0x001786B4
		private void ToggleDebugSettingsMenu()
		{
			Dialog_Debug dialog_Debug = Find.WindowStack.WindowOfType<Dialog_Debug>();
			if (dialog_Debug == null)
			{
				Find.WindowStack.Add(new Dialog_Debug(DebugTabMenuDefOf.Settings));
				return;
			}
			dialog_Debug.SwitchTab(DebugTabMenuDefOf.Settings);
		}

		// Token: 0x06004A4B RID: 19019 RVA: 0x0017A4F0 File Offset: 0x001786F0
		private void ToggleDebugActionsMenu()
		{
			Dialog_Debug dialog_Debug = Find.WindowStack.WindowOfType<Dialog_Debug>();
			if (dialog_Debug == null)
			{
				Find.WindowStack.Add(new Dialog_Debug(DebugTabMenuDefOf.Actions));
				return;
			}
			dialog_Debug.SwitchTab(DebugTabMenuDefOf.Actions);
		}

		// Token: 0x06004A4C RID: 19020 RVA: 0x0017A52B File Offset: 0x0017872B
		private void ToggleTweakValuesMenu()
		{
			if (!Find.WindowStack.TryRemove(typeof(EditWindow_TweakValues), true))
			{
				Find.WindowStack.Add(new EditWindow_TweakValues());
			}
		}

		// Token: 0x06004A4D RID: 19021 RVA: 0x0017A554 File Offset: 0x00178754
		private void ToggleDebugLogMenu()
		{
			Dialog_Debug dialog_Debug = Find.WindowStack.WindowOfType<Dialog_Debug>();
			if (dialog_Debug == null)
			{
				Find.WindowStack.Add(new Dialog_Debug(DebugTabMenuDefOf.Output));
				return;
			}
			dialog_Debug.SwitchTab(DebugTabMenuDefOf.Output);
		}

		// Token: 0x06004A4E RID: 19022 RVA: 0x0017A58F File Offset: 0x0017878F
		private void ToggleDebugInspector()
		{
			if (!Find.WindowStack.TryRemove(typeof(EditWindow_DebugInspector), true))
			{
				Find.WindowStack.Add(new EditWindow_DebugInspector());
			}
		}

		// Token: 0x06004A4F RID: 19023 RVA: 0x0017A5B7 File Offset: 0x001787B7
		private void ToggleGodMode()
		{
			DebugSettings.godMode = !DebugSettings.godMode;
			if (DebugSettings.godMode)
			{
				SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera(null);
				return;
			}
			SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera(null);
		}

		// Token: 0x06004A50 RID: 19024 RVA: 0x0017A5E4 File Offset: 0x001787E4
		public void TryOpenOrClosePalette()
		{
			if (DebugSettings.devPalette)
			{
				Find.WindowStack.Add(new Dialog_DevPalette());
				return;
			}
			Find.WindowStack.TryRemove(typeof(Dialog_DevPalette), true);
		}

		// Token: 0x040034C1 RID: 13505
		private Action drawButtonsCached;

		// Token: 0x040034C2 RID: 13506
		private WidgetRow widgetRow = new WidgetRow();

		// Token: 0x040034C3 RID: 13507
		private float widgetRowFinalX;
	}
}
