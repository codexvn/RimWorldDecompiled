using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;
using Verse.Steam;

namespace RimWorld
{
	// Token: 0x020033F2 RID: 13298
	public class MainTabsRoot
	{
		// Token: 0x170030FA RID: 12538
		// (get) Token: 0x06012B2E RID: 76590 RVA: 0x00589330 File Offset: 0x00587530
		public MainButtonDef OpenTab
		{
			get
			{
				MainTabWindow mainTabWindow = Find.WindowStack.WindowOfType<MainTabWindow>();
				if (mainTabWindow == null)
				{
					return null;
				}
				return mainTabWindow.def;
			}
		}

		// Token: 0x06012B2F RID: 76591 RVA: 0x00589354 File Offset: 0x00587554
		public void HandleLowPriorityShortcuts()
		{
			this.AutoCloseInspectionTabIfNothingSelected(true);
			if (Find.Selector.NumSelected == 0 && Event.current.type == EventType.MouseDown && Event.current.button == 1 && !WorldRendererUtility.WorldSelected && (!SteamDeck.IsSteamDeck || !Input.GetMouseButton(2)))
			{
				Event.current.Use();
				MainButtonDefOf.Architect.Worker.InterfaceTryActivate();
			}
			if (this.OpenTab != null && this.OpenTab != MainButtonDefOf.Inspect && Event.current.type == EventType.MouseDown && Event.current.button != 2)
			{
				this.EscapeCurrentTab(true);
				if (Event.current.button == 0)
				{
					Find.Selector.ClearSelection();
					Find.WorldSelector.ClearSelection();
				}
			}
		}

		// Token: 0x06012B30 RID: 76592 RVA: 0x00589411 File Offset: 0x00587611
		public void EscapeCurrentTab(bool playSound = true)
		{
			this.SetCurrentTab(null, playSound);
		}

		// Token: 0x06012B31 RID: 76593 RVA: 0x0058941B File Offset: 0x0058761B
		public void SetCurrentTab(MainButtonDef tab, bool playSound = true)
		{
			if (tab == this.OpenTab)
			{
				return;
			}
			this.ToggleTab(tab, playSound);
		}

		// Token: 0x06012B32 RID: 76594 RVA: 0x00589430 File Offset: 0x00587630
		public void ToggleTab(MainButtonDef newTab, bool playSound = true)
		{
			if (this.OpenTab == null && newTab == null)
			{
				return;
			}
			if (this.OpenTab == newTab)
			{
				Find.WindowStack.TryRemove(this.OpenTab.TabWindow, true);
				if (playSound)
				{
					SoundDefOf.TabClose.PlayOneShotOnCamera(null);
					return;
				}
			}
			else
			{
				if (this.OpenTab != null)
				{
					Find.WindowStack.TryRemove(this.OpenTab.TabWindow, false);
				}
				if (newTab != null)
				{
					Find.WindowStack.Add(newTab.TabWindow);
				}
				if (playSound)
				{
					if (newTab == null)
					{
						SoundDefOf.TabClose.PlayOneShotOnCamera(null);
					}
					else
					{
						SoundDefOf.TabOpen.PlayOneShotOnCamera(null);
					}
				}
				if (TutorSystem.TutorialMode && newTab != null)
				{
					TutorSystem.Notify_Event("MainTab-" + newTab.defName + "-Open");
				}
			}
		}

		// Token: 0x06012B33 RID: 76595 RVA: 0x005894F4 File Offset: 0x005876F4
		public void Notify_SelectedObjectDespawned()
		{
			this.AutoCloseInspectionTabIfNothingSelected(false);
		}

		// Token: 0x06012B34 RID: 76596 RVA: 0x005894FD File Offset: 0x005876FD
		private void AutoCloseInspectionTabIfNothingSelected(bool playSound)
		{
			if (this.OpenTab == MainButtonDefOf.Inspect && (Find.Selector.NumSelected == 0 || WorldRendererUtility.WorldSelected))
			{
				this.EscapeCurrentTab(playSound);
			}
		}
	}
}
