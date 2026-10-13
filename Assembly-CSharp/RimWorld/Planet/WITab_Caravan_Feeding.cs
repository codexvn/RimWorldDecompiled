using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld.Planet
{
	// Token: 0x02003E5F RID: 15967
	public class WITab_Caravan_Feeding : WITab
	{
		// Token: 0x17003697 RID: 13975
		// (get) Token: 0x06015D6A RID: 89450 RVA: 0x0068711A File Offset: 0x0068531A
		private List<Pawn> Pawns
		{
			get
			{
				return base.SelCaravan.PawnsListForReading;
			}
		}

		// Token: 0x17003698 RID: 13976
		// (get) Token: 0x06015D6B RID: 89451 RVA: 0x00687127 File Offset: 0x00685327
		public override bool IsVisible
		{
			get
			{
				return this.Pawns.Any(delegate(Pawn pawn)
				{
					ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
					return ChildcareUtility.CanSuckle(pawn, out breastfeedFailReason);
				});
			}
		}

		// Token: 0x06015D6C RID: 89452 RVA: 0x00687153 File Offset: 0x00685353
		public WITab_Caravan_Feeding()
		{
			this.labelKey = "TabCaravanFeeding";
		}

		// Token: 0x06015D6D RID: 89453 RVA: 0x00687166 File Offset: 0x00685366
		protected override void UpdateSize()
		{
			this.size = new Vector2(250f, Mathf.Min(550f, this.PaneTopY - 30f));
			base.UpdateSize();
		}

		// Token: 0x06015D6E RID: 89454 RVA: 0x00687194 File Offset: 0x00685394
		protected override void FillTab()
		{
			Text.Font = GameFont.Small;
			RectDivider rect = new RectDivider(new Rect(0f, 15f, this.size.x, this.size.y - 15f).ContractedBy(10f), 1384745000, null);
			Widgets.ListSeparator(ref rect, "Babies".Translate().CapitalizeFirst());
			RectDivider rect2 = rect.CreateViewRect(this.Pawns.Count(delegate(Pawn pawn)
			{
				ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
				return ChildcareUtility.CanSuckle(pawn, out breastfeedFailReason);
			}), 34f);
			Widgets.BeginScrollView(rect, ref this.scrollPosition, rect2, true);
			this.DoRows(ref rect2);
			Widgets.EndScrollView();
		}

		// Token: 0x06015D6F RID: 89455 RVA: 0x0068726C File Offset: 0x0068546C
		private void DoRows(ref RectDivider scrollViewRect)
		{
			if (this.selectedPawn != null && (this.selectedPawn.Destroyed || !base.SelCaravan.ContainsPawn(this.selectedPawn)))
			{
				this.selectedPawn = null;
			}
			foreach (Pawn pawn in this.Pawns)
			{
				ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
				if (ChildcareUtility.CanSuckle(pawn, out breastfeedFailReason))
				{
					this.DoRow(scrollViewRect.NewRow(34f, VerticalJustification.Top, null), pawn);
				}
			}
		}

		// Token: 0x06015D70 RID: 89456 RVA: 0x00687314 File Offset: 0x00685514
		private void DoRow(Rect rect, Pawn pawn)
		{
			Widgets.BeginGroup(rect);
			Rect rect2 = rect.AtZero();
			CaravanThingsTabUtility.DoAbandonButton(rect2, pawn, base.SelCaravan);
			rect2.width -= 24f;
			Widgets.InfoCardButton(rect2.width - 24f, (rect.height - 24f) / 2f, pawn);
			rect2.width -= 24f;
			CaravanThingsTabUtility.DoOpenSpecificTabButton(rect2, pawn, ref this.selectedPawn);
			rect2.width -= 24f;
			CaravanThingsTabUtility.DoOpenSpecificTabButtonInvisible(rect2, pawn, ref this.selectedPawn);
			if (Mouse.IsOver(rect2))
			{
				Widgets.DrawHighlight(rect2);
			}
			Rect rect3 = new Rect(4f, (rect.height - 27f) / 2f, 27f, 27f);
			Widgets.ThingIcon(rect3, pawn, 1f, null, false, 1f, false);
			Rect bgRect = new Rect(rect3.xMax + 4f, 8f, 100f, 18f);
			GenMapUI.DrawPawnLabel(pawn, bgRect, 1f, 100f, null, GameFont.Small, false, false);
			if (pawn.Downed && !pawn.ageTracker.CurLifeStage.alwaysDowned)
			{
				GUI.color = new Color(1f, 0f, 0f, 0.5f);
				Widgets.DrawLineHorizontal(0f, rect.height / 2f, rect.width);
				GUI.color = Color.white;
			}
			Widgets.EndGroup();
		}

		// Token: 0x06015D71 RID: 89457 RVA: 0x006874A8 File Offset: 0x006856A8
		protected override void ExtraOnGUI()
		{
			base.ExtraOnGUI();
			if (this.selectedPawn != null && (this.selectedPawn.Destroyed || !base.SelCaravan.ContainsPawn(this.selectedPawn)))
			{
				this.selectedPawn = null;
			}
			Pawn captureSelectedPawn = this.selectedPawn;
			if (this.selectedPawn != null)
			{
				Rect tabRect = base.TabRect;
				float width = 500f;
				Rect rect = new Rect(tabRect.xMax - 1f, tabRect.yMin, width, tabRect.height);
				Find.WindowStack.ImmediateWindow(323826479, rect, WindowLayer.GameUI, delegate
				{
					if (captureSelectedPawn.DestroyedOrNull())
					{
						return;
					}
					if (this.SelCaravan == null)
					{
						return;
					}
					ITab_Pawn_Feeding.FillTab(captureSelectedPawn, rect.AtZero(), ref this.scrollPositionAutoBreastfeed, ref this.scrollPositionBabyConsumables, this.Pawns);
					if (Widgets.CloseButtonFor(rect.AtZero()))
					{
						captureSelectedPawn = null;
						SoundDefOf.TabClose.PlayOneShotOnCamera(null);
					}
				}, true, false, 1f, null, false);
			}
		}

		// Token: 0x06015D72 RID: 89458 RVA: 0x0068756A File Offset: 0x0068576A
		public override void Notify_ClearingAllMapsMemory()
		{
			base.Notify_ClearingAllMapsMemory();
			this.selectedPawn = null;
		}

		// Token: 0x06015D73 RID: 89459 RVA: 0x0068757C File Offset: 0x0068577C
		public override void OnOpen()
		{
			base.OnOpen();
			if (this.selectedPawn == null || !this.Pawns.Contains(this.selectedPawn))
			{
				this.selectedPawn = this.Pawns.FirstOrFallback(delegate(Pawn pawn)
				{
					ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
					return ChildcareUtility.CanSuckle(pawn, out breastfeedFailReason);
				}, null);
			}
		}

		// Token: 0x0400F23A RID: 62010
		private Vector2 scrollPosition;

		// Token: 0x0400F23B RID: 62011
		private Pawn selectedPawn;

		// Token: 0x0400F23C RID: 62012
		private Vector2 scrollPositionAutoBreastfeed;

		// Token: 0x0400F23D RID: 62013
		private Vector2 scrollPositionBabyConsumables;

		// Token: 0x0400F23E RID: 62014
		private const int contextHash = 1384745000;

		// Token: 0x0400F23F RID: 62015
		private const float RowHeight = 34f;

		// Token: 0x0400F240 RID: 62016
		private const float ScrollViewTopMargin = 15f;

		// Token: 0x0400F241 RID: 62017
		private const float PawnLabelHeight = 18f;

		// Token: 0x0400F242 RID: 62018
		private const float PawnLabelColumnWidth = 100f;

		// Token: 0x0400F243 RID: 62019
		private const float SpaceAroundIcon = 4f;

		// Token: 0x0400F244 RID: 62020
		private const float BabyPickWindowWidth = 250f;
	}
}
