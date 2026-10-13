using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020033FE RID: 13310
	[StaticConstructorOnStartup]
	public class MainTabWindow_Inspect : MainTabWindow, IInspectPane
	{
		// Token: 0x17003106 RID: 12550
		// (get) Token: 0x06012B74 RID: 76660 RVA: 0x0058B09A File Offset: 0x0058929A
		// (set) Token: 0x06012B75 RID: 76661 RVA: 0x0058B0A2 File Offset: 0x005892A2
		public Type OpenTabType
		{
			get
			{
				return this.openTabType;
			}
			set
			{
				this.openTabType = value;
			}
		}

		// Token: 0x17003107 RID: 12551
		// (get) Token: 0x06012B76 RID: 76662 RVA: 0x0058B0AB File Offset: 0x005892AB
		// (set) Token: 0x06012B77 RID: 76663 RVA: 0x0058B0B3 File Offset: 0x005892B3
		public float RecentHeight
		{
			get
			{
				return this.recentHeight;
			}
			set
			{
				this.recentHeight = value;
			}
		}

		// Token: 0x17003108 RID: 12552
		// (get) Token: 0x06012B78 RID: 76664 RVA: 0x00006AAC File Offset: 0x00004CAC
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17003109 RID: 12553
		// (get) Token: 0x06012B79 RID: 76665 RVA: 0x0058B0BC File Offset: 0x005892BC
		public override Vector2 RequestedTabSize
		{
			get
			{
				return InspectPaneUtility.PaneSizeFor(this);
			}
		}

		// Token: 0x1700310A RID: 12554
		// (get) Token: 0x06012B7A RID: 76666 RVA: 0x00578FBC File Offset: 0x005771BC
		private List<object> Selected
		{
			get
			{
				return Find.Selector.SelectedObjects;
			}
		}

		// Token: 0x1700310B RID: 12555
		// (get) Token: 0x06012B7B RID: 76667 RVA: 0x00578FC8 File Offset: 0x005771C8
		private Thing SelThing
		{
			get
			{
				return Find.Selector.SingleSelectedThing;
			}
		}

		// Token: 0x1700310C RID: 12556
		// (get) Token: 0x06012B7C RID: 76668 RVA: 0x0050E7D0 File Offset: 0x0050C9D0
		private Zone SelZone
		{
			get
			{
				return Find.Selector.SelectedZone;
			}
		}

		// Token: 0x1700310D RID: 12557
		// (get) Token: 0x06012B7D RID: 76669 RVA: 0x0058B0C4 File Offset: 0x005892C4
		private int NumSelected
		{
			get
			{
				return Find.Selector.NumSelected;
			}
		}

		// Token: 0x1700310E RID: 12558
		// (get) Token: 0x06012B7E RID: 76670 RVA: 0x0058B0D0 File Offset: 0x005892D0
		public float PaneTopY
		{
			get
			{
				return (float)UI.screenHeight - 165f - 35f;
			}
		}

		// Token: 0x1700310F RID: 12559
		// (get) Token: 0x06012B7F RID: 76671 RVA: 0x0058B0E4 File Offset: 0x005892E4
		public bool AnythingSelected
		{
			get
			{
				return this.NumSelected > 0;
			}
		}

		// Token: 0x17003110 RID: 12560
		// (get) Token: 0x06012B80 RID: 76672 RVA: 0x0058B0F0 File Offset: 0x005892F0
		public bool ShouldShowSelectNextInCellButton
		{
			get
			{
				return this.NumSelected == 1 && (Find.Selector.SelectedZone == null || Find.Selector.SelectedZone.ContainsCell(MainTabWindow_Inspect.lastSelectCell)) && (Find.Selector.SelectedPlan == null || Find.Selector.SelectedPlan.ContainsCell(MainTabWindow_Inspect.lastSelectCell));
			}
		}

		// Token: 0x17003111 RID: 12561
		// (get) Token: 0x06012B81 RID: 76673 RVA: 0x0058B14C File Offset: 0x0058934C
		public bool ShouldShowPaneContents
		{
			get
			{
				StorageGroup storageGroup;
				return (this.SelThing == null || !this.SelThing.def.hideInspect || DebugSettings.showHiddenInfo) && (this.TryGetSelectedStorageGroup(out storageGroup) || this.NumSelected == 1);
			}
		}

		// Token: 0x17003112 RID: 12562
		// (get) Token: 0x06012B82 RID: 76674 RVA: 0x0058B194 File Offset: 0x00589394
		public IEnumerable<InspectTabBase> CurTabs
		{
			get
			{
				if (Find.ScreenshotModeHandler.Active)
				{
					return null;
				}
				if (this.NumSelected == 1)
				{
					if (this.SelThing != null)
					{
						if (this.SelThing.def.inspectorTabsResolved == null)
						{
							IStorageGroupMember storageGroupMember = this.SelThing as IStorageGroupMember;
							if (storageGroupMember == null || !storageGroupMember.DrawStorageTab)
							{
								goto IL_54;
							}
						}
						return this.SelThing.GetInspectTabs();
					}
					IL_54:
					if (this.SelZone != null)
					{
						return this.SelZone.GetInspectTabs();
					}
				}
				else if (this.Selected.Count > 1)
				{
					if (this.Selected.All((object s) => s is IStorageGroupMember))
					{
						Thing thing = this.Selected.First<object>() as Thing;
						if (thing == null)
						{
							return null;
						}
						return thing.GetInspectTabs();
					}
				}
				return null;
			}
		}

		// Token: 0x06012B83 RID: 76675 RVA: 0x0058B260 File Offset: 0x00589460
		public MainTabWindow_Inspect()
		{
			this.closeOnAccept = false;
			this.closeOnCancel = false;
			this.drawInScreenshotMode = false;
			this.soundClose = SoundDefOf.TabClose;
		}

		// Token: 0x06012B84 RID: 76676 RVA: 0x0058B288 File Offset: 0x00589488
		public override void ExtraOnGUI()
		{
			base.ExtraOnGUI();
			InspectPaneUtility.ExtraOnGUI(this);
			Designator selectedDesignator = Find.DesignatorManager.SelectedDesignator;
			if (selectedDesignator != null && (this.AnythingSelected || selectedDesignator.AlwaysDoGuiControls))
			{
				Find.DesignatorManager.SelectedDesignator.DoExtraGuiControls(0f, this.PaneTopY);
			}
		}

		// Token: 0x06012B85 RID: 76677 RVA: 0x0058B2DC File Offset: 0x005894DC
		public override void DoWindowContents(Rect inRect)
		{
			InspectPaneUtility.InspectPaneOnGUI(inRect, this);
			if (this.lastSelectedThing != this.SelThing)
			{
				this.SetInitialSizeAndPosition();
				this.lastSelectedThing = this.SelThing;
				return;
			}
			if (this.RequestedTabSize != this.lastTabSize)
			{
				this.SetInitialSizeAndPosition();
				this.lastTabSize = this.RequestedTabSize;
			}
		}

		// Token: 0x06012B86 RID: 76678 RVA: 0x0058B336 File Offset: 0x00589536
		public string GetLabel(Rect rect)
		{
			return InspectPaneUtility.AdjustedLabelFor(this.Selected, rect);
		}

		// Token: 0x06012B87 RID: 76679 RVA: 0x0058B344 File Offset: 0x00589544
		public void DoPaneContents(Rect rect)
		{
			StorageGroup group;
			if (this.TryGetSelectedStorageGroup(out group))
			{
				InspectPaneFiller.DoPaneContentsForStorageGroup(group, rect);
				return;
			}
			InspectPaneFiller.DoPaneContentsFor((ISelectable)Find.Selector.FirstSelectedObject, rect);
		}

		// Token: 0x06012B88 RID: 76680 RVA: 0x0058B378 File Offset: 0x00589578
		public void DoInspectPaneButtons(Rect rect, ref float lineEndWidth)
		{
			StorageGroup renamable;
			if (this.NumSelected == 1)
			{
				Thing singleSelectedThing = Find.Selector.SingleSelectedThing;
				if (singleSelectedThing != null)
				{
					float num = rect.width - 48f;
					Widgets.InfoCardButton(num, 0f, Find.Selector.SingleSelectedThing);
					lineEndWidth += 24f;
					Pawn p = singleSelectedThing as Pawn;
					if (p != null)
					{
						if (p.playerSettings != null && p.playerSettings.UsesConfigurableHostilityResponse)
						{
							num -= 24f;
							HostilityResponseModeUtility.DrawResponseButton(new Rect(num, 0f, 24f, 24f), p, false);
							lineEndWidth += 24f;
						}
						if ((p.Faction == Faction.OfPlayer && p.RaceProps.Animal && p.RaceProps.hideTrainingTab) || (ModsConfig.BiotechActive && p.IsColonyMech))
						{
							num -= 30f;
							RenameUIUtility.DrawRenameButton(new Rect(num, 0f, 30f, 30f), p);
							lineEndWidth += 30f;
						}
						if (p.guilt != null && p.guilt.IsGuilty)
						{
							num -= 26f;
							Rect rect2 = new Rect(num, 0f, 26f, 26f);
							GUI.DrawTexture(rect2, TexUI.GuiltyTex);
							TooltipHandler.TipRegion(rect2, () => p.guilt.Tip, 6321223);
							lineEndWidth += 26f;
							return;
						}
					}
					else
					{
						IStorageGroupMember storageGroupMember = singleSelectedThing as IStorageGroupMember;
						if (storageGroupMember != null && storageGroupMember.ShowRenameButton)
						{
							num -= 30f;
							Rect rect3 = new Rect(num, 0f, 30f, 30f);
							if (storageGroupMember.Group != null)
							{
								RenameUIUtility.DrawRenameButton(rect3, storageGroupMember.Group);
							}
							else if (singleSelectedThing.Spawned)
							{
								RenameUIUtility.DrawRenameButton(rect3, storageGroupMember);
							}
							lineEndWidth += 30f;
							return;
						}
						CompAnimalPenMarker marker;
						if (singleSelectedThing.Spawned && singleSelectedThing.Faction == Faction.OfPlayer && singleSelectedThing.TryGetComp(out marker))
						{
							num -= 30f;
							RenameUIUtility.DrawRenameButton(new Rect(num, 0f, 30f, 30f), marker);
							lineEndWidth += 30f;
							return;
						}
						CompPilotConsole compPilotConsole;
						if (singleSelectedThing.Spawned && singleSelectedThing.Faction == Faction.OfPlayer && singleSelectedThing.TryGetComp(out compPilotConsole) && compPilotConsole.engine != null)
						{
							num -= 30f;
							RenameUIUtility.DrawRenameButton(new Rect(num, 0f, 30f, 30f), compPilotConsole.engine);
							lineEndWidth += 30f;
							return;
						}
						IRenameable renameable = singleSelectedThing as IRenameable;
						if (renameable != null)
						{
							num -= 30f;
							RenameUIUtility.DrawRenameButton(new Rect(num, 0f, 30f, 30f), renameable);
							lineEndWidth += 30f;
							return;
						}
					}
				}
				else
				{
					if (Find.Selector.SelectedZone != null)
					{
						Rect rect4 = new Rect(rect.width - 30f, 0f, 30f, 30f);
						if (this.ShouldShowSelectNextInCellButton)
						{
							rect4.x -= 24f;
						}
						RenameUIUtility.DrawRenameButton(rect4, Find.Selector.SelectedZone);
						lineEndWidth += 30f;
						return;
					}
					if (Find.Selector.SelectedPlan != null)
					{
						Rect rect5 = new Rect(rect.width - 30f, 0f, 30f, 30f);
						if (this.ShouldShowSelectNextInCellButton)
						{
							rect5.x -= 24f;
						}
						RenameUIUtility.DrawRenameButton(rect5, Find.Selector.SelectedPlan);
						lineEndWidth += 30f;
						return;
					}
				}
			}
			else if (this.TryGetSelectedStorageGroup(out renamable))
			{
				float x = rect.width - 30f;
				RenameUIUtility.DrawRenameButton(new Rect(x, 0f, 30f, 30f), renamable);
				lineEndWidth += 30f;
			}
		}

		// Token: 0x06012B89 RID: 76681 RVA: 0x0058B784 File Offset: 0x00589984
		private bool TryGetSelectedStorageGroup(out StorageGroup group)
		{
			bool flag = true;
			group = null;
			if (Find.Selector.SelectedObjects.Count <= 1)
			{
				group = null;
				return false;
			}
			foreach (object obj in Find.Selector.SelectedObjects)
			{
				IStorageGroupMember storageGroupMember = obj as IStorageGroupMember;
				if (storageGroupMember == null)
				{
					flag = false;
					break;
				}
				if (group == null)
				{
					group = storageGroupMember.Group;
				}
				if (storageGroupMember.Group != group || storageGroupMember.Group == null)
				{
					flag = false;
					break;
				}
			}
			return flag && group != null;
		}

		// Token: 0x06012B8A RID: 76682 RVA: 0x0058B828 File Offset: 0x00589A28
		public void SelectNextInCell()
		{
			if (this.NumSelected != 1)
			{
				return;
			}
			Selector selector = Find.Selector;
			if ((selector.SelectedZone == null || selector.SelectedZone.ContainsCell(MainTabWindow_Inspect.lastSelectCell)) && (selector.SelectedPlan == null || selector.SelectedPlan.ContainsCell(MainTabWindow_Inspect.lastSelectCell)))
			{
				if (selector.SelectedZone == null && selector.SelectedPlan == null)
				{
					MainTabWindow_Inspect.lastSelectCell = selector.SingleSelectedThing.PositionHeld;
				}
				Map map;
				if (selector.SingleSelectedThing != null)
				{
					map = selector.SingleSelectedThing.MapHeld;
				}
				else if (selector.SelectedZone != null)
				{
					map = selector.SelectedZone.Map;
				}
				else
				{
					if (selector.SelectedPlan == null)
					{
						Log.Error("Attempted to selected next thing in cell but case was not handled.");
						return;
					}
					map = selector.SelectedPlan.Map;
				}
				selector.SelectNextAt(MainTabWindow_Inspect.lastSelectCell, map);
			}
		}

		// Token: 0x06012B8B RID: 76683 RVA: 0x0058B8F5 File Offset: 0x00589AF5
		public override void WindowUpdate()
		{
			base.WindowUpdate();
			InspectPaneUtility.UpdateTabs(this);
		}

		// Token: 0x06012B8C RID: 76684 RVA: 0x0058B903 File Offset: 0x00589B03
		public void CloseOpenTab()
		{
			this.openTabType = null;
		}

		// Token: 0x06012B8D RID: 76685 RVA: 0x0058B903 File Offset: 0x00589B03
		public void Reset()
		{
			this.openTabType = null;
		}

		// Token: 0x0400C268 RID: 49768
		private Type openTabType;

		// Token: 0x0400C269 RID: 49769
		private float recentHeight;

		// Token: 0x0400C26A RID: 49770
		private static IntVec3 lastSelectCell;

		// Token: 0x0400C26B RID: 49771
		private Thing lastSelectedThing;

		// Token: 0x0400C26C RID: 49772
		private Vector2 lastTabSize;

		// Token: 0x0400C26D RID: 49773
		private const float GuiltyTexSize = 26f;
	}
}
