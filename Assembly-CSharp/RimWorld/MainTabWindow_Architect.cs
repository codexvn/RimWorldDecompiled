using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;
using Verse.Steam;

namespace RimWorld
{
	// Token: 0x020033F4 RID: 13300
	public class MainTabWindow_Architect : MainTabWindow
	{
		// Token: 0x170030FD RID: 12541
		// (get) Token: 0x06012B3A RID: 76602 RVA: 0x005895BB File Offset: 0x005877BB
		public float WinHeight
		{
			get
			{
				return (float)Mathf.CeilToInt((float)this.desPanelsCached.Count / 2f) * 32f + 28f;
			}
		}

		// Token: 0x170030FE RID: 12542
		// (get) Token: 0x06012B3B RID: 76603 RVA: 0x005895E1 File Offset: 0x005877E1
		public override Vector2 RequestedTabSize
		{
			get
			{
				return new Vector2(200f, this.WinHeight);
			}
		}

		// Token: 0x170030FF RID: 12543
		// (get) Token: 0x06012B3C RID: 76604 RVA: 0x00006AAC File Offset: 0x00004CAC
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17003100 RID: 12544
		// (get) Token: 0x06012B3D RID: 76605 RVA: 0x005895F3 File Offset: 0x005877F3
		public bool QuickSearchWidgetFocused
		{
			get
			{
				return this.quickSearchWidget.CurrentlyFocused();
			}
		}

		// Token: 0x17003101 RID: 12545
		// (get) Token: 0x06012B3E RID: 76606 RVA: 0x00589600 File Offset: 0x00587800
		public float PaneTopY
		{
			get
			{
				return (float)UI.screenHeight - this.WinHeight - 35f;
			}
		}

		// Token: 0x06012B3F RID: 76607 RVA: 0x00589615 File Offset: 0x00587815
		public MainTabWindow_Architect()
		{
			this.closeOnAccept = false;
			this.quickSearchWidget.inactiveTextColor = MainTabWindow_Architect.InactiveSearchColor;
			this.CacheDesPanels();
		}

		// Token: 0x06012B40 RID: 76608 RVA: 0x00589650 File Offset: 0x00587850
		public override void PreOpen()
		{
			base.PreOpen();
			this.ResetAndUnfocusQuickSearch();
			this.forceActivatedCommand = null;
			this.didInitialUnfocus = false;
		}

		// Token: 0x06012B41 RID: 76609 RVA: 0x0058966C File Offset: 0x0058786C
		public override void WindowUpdate()
		{
			base.WindowUpdate();
			ArchitectCategoryTab architectCategoryTab = this.OpenTab();
			if (architectCategoryTab != null && architectCategoryTab.def.showPowerGrid)
			{
				OverlayDrawHandler.DrawPowerGridOverlayThisFrame();
			}
		}

		// Token: 0x06012B42 RID: 76610 RVA: 0x0058969B File Offset: 0x0058789B
		public override void ExtraOnGUI()
		{
			base.ExtraOnGUI();
			ArchitectCategoryTab architectCategoryTab = this.OpenTab();
			if (architectCategoryTab != null)
			{
				architectCategoryTab.DesignationTabOnGUI(this.forceActivatedCommand);
			}
			this.forceActivatedCommand = null;
		}

		// Token: 0x06012B43 RID: 76611 RVA: 0x005896C4 File Offset: 0x005878C4
		private ArchitectCategoryTab OpenTab()
		{
			if (!this.quickSearchWidget.filter.Active || this.userForcedSelectionDuringSearch)
			{
				return this.selectedDesPanel;
			}
			if (this.selectedDesPanel != null && this.selectedDesPanel.AnySearchMatches)
			{
				return this.selectedDesPanel;
			}
			foreach (ArchitectCategoryTab architectCategoryTab in this.desPanelsCached)
			{
				if (architectCategoryTab.AnySearchMatches)
				{
					return architectCategoryTab;
				}
			}
			return null;
		}

		// Token: 0x06012B44 RID: 76612 RVA: 0x0058975C File Offset: 0x0058795C
		private void DoCategoryButton(ArchitectCategoryTab panel, float butWidth, float curXInd, float curYInd, ArchitectCategoryTab openTab, bool enabled)
		{
			Rect rect = new Rect(curXInd * butWidth, curYInd * 32f, butWidth, 32f);
			float height = rect.height;
			rect.height = height + 1f;
			if (curXInd == 0f)
			{
				rect.width += 1f;
			}
			Color? labelColor = panel.AnySearchMatches ? null : new Color?(MainTabWindow_Architect.NoMatchColor);
			string label = panel.def.LabelCap;
			if (!enabled)
			{
				if (labelColor == null)
				{
					labelColor = new Color?(MainTabWindow_Architect.InactiveColor);
				}
				GUI.color = MainTabWindow_Architect.InactiveColor;
			}
			if (Widgets.ButtonTextSubtle(rect, label, 0f, 8f, SoundDefOf.Mouseover_Category, new Vector2(-1f, -1f), labelColor, this.quickSearchWidget.filter.Active && openTab == panel))
			{
				this.ClickedCategory(panel);
			}
			GUI.color = Color.white;
			if (this.selectedDesPanel != panel)
			{
				UIHighlighter.HighlightOpportunity(rect, panel.def.cachedHighlightClosedTag);
			}
		}

		// Token: 0x06012B45 RID: 76613 RVA: 0x00589871 File Offset: 0x00587A71
		private void DoCategoryRow(ArchitectCategoryTab left, ArchitectCategoryTab right, float butWidth, float curYInd, ArchitectCategoryTab openTab)
		{
			if (left != null)
			{
				this.DoCategoryButton(left, butWidth, 0f, curYInd, openTab, left.Visible);
			}
			if (right != null)
			{
				this.DoCategoryButton(right, butWidth, 1f, curYInd, openTab, right.Visible);
			}
		}

		// Token: 0x06012B46 RID: 76614 RVA: 0x005898A8 File Offset: 0x00587AA8
		public override void DoWindowContents(Rect inRect)
		{
			Text.Font = GameFont.Small;
			float butWidth = inRect.width / 2f;
			float num = 0f;
			bool flag = ModsConfig.IdeologyActive && Find.IdeoManager.classicMode;
			ArchitectCategoryTab architectCategoryTab = this.OpenTab();
			if (KeyBindingDefOf.Accept.KeyDownEvent)
			{
				if (this.quickSearchWidget.filter.Active && ((architectCategoryTab != null) ? architectCategoryTab.UniqueSearchMatch : null) != null)
				{
					this.forceActivatedCommand = architectCategoryTab.UniqueSearchMatch;
					Event.current.Use();
				}
				else if (!SteamDeck.IsSteamDeck)
				{
					this.Close(true);
					Event.current.Use();
				}
			}
			int i = 0;
			while (i < this.desPanelsCached.Count)
			{
				ArchitectCategoryTab architectCategoryTab2 = this.desPanelsCached[i];
				ArchitectCategoryTab architectCategoryTab3 = (i + 1 < this.desPanelsCached.Count) ? this.desPanelsCached[i + 1] : null;
				int? preferredColumn = architectCategoryTab2.PreferredColumn;
				int num2 = 1;
				if (preferredColumn.GetValueOrDefault() == num2 & preferredColumn != null)
				{
					goto IL_114;
				}
				if (architectCategoryTab3 == null)
				{
					goto IL_16F;
				}
				preferredColumn = architectCategoryTab3.PreferredColumn;
				num2 = 0;
				if (preferredColumn.GetValueOrDefault() == num2 & preferredColumn != null)
				{
					goto IL_114;
				}
				goto IL_16F;
				IL_17C:
				num += 1f;
				i += 2;
				continue;
				IL_114:
				preferredColumn = architectCategoryTab2.PreferredColumn;
				num2 = 0;
				if (!(preferredColumn.GetValueOrDefault() == num2 & preferredColumn != null))
				{
					bool flag2;
					if (architectCategoryTab3 == null)
					{
						flag2 = true;
					}
					else
					{
						preferredColumn = architectCategoryTab3.PreferredColumn;
						num2 = 1;
						flag2 = !(preferredColumn.GetValueOrDefault() == num2 & preferredColumn != null);
					}
					if (flag2)
					{
						this.DoCategoryRow(architectCategoryTab3, architectCategoryTab2, butWidth, num, architectCategoryTab);
						goto IL_17C;
					}
				}
				IL_16F:
				this.DoCategoryRow(architectCategoryTab2, architectCategoryTab3, butWidth, num, architectCategoryTab);
				goto IL_17C;
			}
			float num3 = inRect.width;
			if (flag)
			{
				num3 -= 32f;
			}
			Rect rect = new Rect(0f, num * 32f + 1f, num3, 24f);
			this.quickSearchWidget.OnGUI(rect, new Action(this.CacheSearchState), null);
			if (!this.didInitialUnfocus)
			{
				UI.UnfocusCurrentControl();
				this.didInitialUnfocus = true;
			}
			if (flag && Widgets.ButtonImage(new Rect(rect.xMax + 4f, rect.y, 24f, 24f).ContractedBy(2f), MainTabWindow_Architect.ChangeStyleIcon.Texture, true, null))
			{
				if (Find.WindowStack.IsOpen<Dialog_StyleSelection>())
				{
					Find.WindowStack.TryRemove(typeof(Dialog_StyleSelection), true);
					return;
				}
				Find.WindowStack.Add(new Dialog_StyleSelection());
			}
		}

		// Token: 0x06012B47 RID: 76615 RVA: 0x00589B2C File Offset: 0x00587D2C
		private void CacheDesPanels()
		{
			this.desPanelsCached.Clear();
			foreach (DesignationCategoryDef def in from dc in DefDatabase<DesignationCategoryDef>.AllDefs
			orderby dc.order descending
			select dc)
			{
				this.desPanelsCached.Add(new ArchitectCategoryTab(def, this.quickSearchWidget.filter));
			}
		}

		// Token: 0x06012B48 RID: 76616 RVA: 0x00589BBC File Offset: 0x00587DBC
		private void CacheSearchState()
		{
			bool flag = false;
			foreach (ArchitectCategoryTab architectCategoryTab in this.desPanelsCached)
			{
				architectCategoryTab.CacheSearchState();
				flag |= architectCategoryTab.AnySearchMatches;
			}
			this.quickSearchWidget.noResultsMatched = !flag;
			if (!this.quickSearchWidget.filter.Active)
			{
				this.userForcedSelectionDuringSearch = false;
			}
		}

		// Token: 0x06012B49 RID: 76617 RVA: 0x00589C44 File Offset: 0x00587E44
		protected void ClickedCategory(ArchitectCategoryTab Pan)
		{
			if (!Pan.Visible)
			{
				Messages.Message("NothingAvailableInCategory".Translate() + ": " + Pan.def.LabelCap, MessageTypeDefOf.RejectInput, false);
				return;
			}
			if (this.selectedDesPanel == Pan && !this.quickSearchWidget.filter.Active)
			{
				this.selectedDesPanel = null;
			}
			else
			{
				this.selectedDesPanel = Pan;
			}
			if (this.quickSearchWidget.filter.Active)
			{
				this.userForcedSelectionDuringSearch = true;
			}
			SoundDefOf.ArchitectCategorySelect.PlayOneShotOnCamera(null);
		}

		// Token: 0x06012B4A RID: 76618 RVA: 0x00589CDD File Offset: 0x00587EDD
		public override void Notify_ClickOutsideWindow()
		{
			base.Notify_ClickOutsideWindow();
			this.quickSearchWidget.Unfocus();
		}

		// Token: 0x06012B4B RID: 76619 RVA: 0x00589CF0 File Offset: 0x00587EF0
		public override void OnCancelKeyPressed()
		{
			if (this.quickSearchWidget.CurrentlyFocused())
			{
				this.ResetAndUnfocusQuickSearch();
				return;
			}
			base.OnCancelKeyPressed();
		}

		// Token: 0x06012B4C RID: 76620 RVA: 0x00589D0C File Offset: 0x00587F0C
		private void ResetAndUnfocusQuickSearch()
		{
			this.quickSearchWidget.Reset();
			this.quickSearchWidget.Unfocus();
			this.CacheSearchState();
		}

		// Token: 0x0400C22D RID: 49709
		private List<ArchitectCategoryTab> desPanelsCached = new List<ArchitectCategoryTab>();

		// Token: 0x0400C22E RID: 49710
		public ArchitectCategoryTab selectedDesPanel;

		// Token: 0x0400C22F RID: 49711
		private QuickSearchWidget quickSearchWidget = new QuickSearchWidget();

		// Token: 0x0400C230 RID: 49712
		private bool userForcedSelectionDuringSearch;

		// Token: 0x0400C231 RID: 49713
		private Designator forceActivatedCommand;

		// Token: 0x0400C232 RID: 49714
		private bool didInitialUnfocus;

		// Token: 0x0400C233 RID: 49715
		public const float WinWidth = 200f;

		// Token: 0x0400C234 RID: 49716
		private const float ButHeight = 32f;

		// Token: 0x0400C235 RID: 49717
		private static readonly Color InactiveSearchColor = Color.white.ToTransparent(0.4f);

		// Token: 0x0400C236 RID: 49718
		private static readonly Color NoMatchColor = Color.grey;

		// Token: 0x0400C237 RID: 49719
		public const int ColumnCount = 2;

		// Token: 0x0400C238 RID: 49720
		private static readonly Color InactiveColor = new Color(1f, 1f, 1f, 0.1f);

		// Token: 0x0400C239 RID: 49721
		private static readonly CachedTexture ChangeStyleIcon = new CachedTexture("UI/Icons/ChangeSelectedStyles");
	}
}
