using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld.Planet
{
	// Token: 0x02003E6D RID: 15981
	public class WITab_Caravan_Social : WITab
	{
		// Token: 0x1700369E RID: 13982
		// (get) Token: 0x06015DC6 RID: 89542 RVA: 0x0068711A File Offset: 0x0068531A
		private List<Pawn> Pawns
		{
			get
			{
				return base.SelCaravan.PawnsListForReading;
			}
		}

		// Token: 0x1700369F RID: 13983
		// (get) Token: 0x06015DC7 RID: 89543 RVA: 0x00689A11 File Offset: 0x00687C11
		private float SpecificSocialTabWidth
		{
			get
			{
				this.EnsureSpecificSocialTabForPawnValid();
				if (this.specificSocialTabForPawn.DestroyedOrNull())
				{
					return 0f;
				}
				return 540f;
			}
		}

		// Token: 0x06015DC8 RID: 89544 RVA: 0x00689A31 File Offset: 0x00687C31
		public WITab_Caravan_Social()
		{
			this.labelKey = "TabCaravanSocial";
		}

		// Token: 0x06015DC9 RID: 89545 RVA: 0x00689A44 File Offset: 0x00687C44
		protected override void FillTab()
		{
			this.EnsureSpecificSocialTabForPawnValid();
			Text.Font = GameFont.Small;
			Rect rect = new Rect(0f, 15f, this.size.x, this.size.y - 15f).ContractedBy(10f);
			Rect rect2 = new Rect(0f, 0f, rect.width - 16f, this.scrollViewHeight);
			float num = 0f;
			Widgets.BeginScrollView(rect, ref this.scrollPosition, rect2, true);
			this.DoRows(ref num, rect2, rect);
			if (Event.current.type == EventType.Layout)
			{
				this.scrollViewHeight = num + 30f;
			}
			Widgets.EndScrollView();
		}

		// Token: 0x06015DCA RID: 89546 RVA: 0x00689AF4 File Offset: 0x00687CF4
		protected override void UpdateSize()
		{
			this.EnsureSpecificSocialTabForPawnValid();
			base.UpdateSize();
			this.size.x = 243f;
			this.size.y = Mathf.Min(550f, this.PaneTopY - 30f);
		}

		// Token: 0x06015DCB RID: 89547 RVA: 0x00689B34 File Offset: 0x00687D34
		protected override void ExtraOnGUI()
		{
			this.EnsureSpecificSocialTabForPawnValid();
			base.ExtraOnGUI();
			Pawn localSpecificSocialTabForPawn = this.specificSocialTabForPawn;
			if (localSpecificSocialTabForPawn != null)
			{
				Rect tabRect = base.TabRect;
				float specificSocialTabWidth = this.SpecificSocialTabWidth;
				Rect rect = new Rect(tabRect.xMax - 1f, tabRect.yMin, specificSocialTabWidth, tabRect.height);
				Find.WindowStack.ImmediateWindow(1439870015, rect, WindowLayer.GameUI, delegate
				{
					if (localSpecificSocialTabForPawn.DestroyedOrNull())
					{
						return;
					}
					SocialCardUtility.DrawSocialCard(rect.AtZero(), localSpecificSocialTabForPawn);
					if (Widgets.CloseButtonFor(rect.AtZero()))
					{
						this.specificSocialTabForPawn = null;
						SoundDefOf.TabClose.PlayOneShotOnCamera(null);
					}
				}, true, false, 1f, null, false);
			}
		}

		// Token: 0x06015DCC RID: 89548 RVA: 0x00689BD0 File Offset: 0x00687DD0
		public override void OnOpen()
		{
			base.OnOpen();
			if ((this.specificSocialTabForPawn == null || !this.Pawns.Contains(this.specificSocialTabForPawn)) && this.Pawns.Any<Pawn>())
			{
				this.specificSocialTabForPawn = this.Pawns[0];
			}
		}

		// Token: 0x06015DCD RID: 89549 RVA: 0x00689C20 File Offset: 0x00687E20
		private void DoRows(ref float curY, Rect scrollViewRect, Rect scrollOutRect)
		{
			List<Pawn> pawns = this.Pawns;
			Pawn pawn = BestCaravanPawnUtility.FindBestNegotiator(base.SelCaravan, null, null);
			GUI.color = new Color(0.8f, 0.8f, 0.8f, 1f);
			Widgets.Label(new Rect(0f, curY, scrollViewRect.width, 24f), string.Format("{0}: {1}", "Negotiator".TranslateSimple(), (pawn != null) ? pawn.LabelShort : "NoneCapable".Translate().ToString()));
			curY += 24f;
			if (this.specificSocialTabForPawn != null && !pawns.Contains(this.specificSocialTabForPawn))
			{
				this.specificSocialTabForPawn = null;
			}
			bool flag = false;
			for (int i = 0; i < pawns.Count; i++)
			{
				Pawn pawn2 = pawns[i];
				if (pawn2.RaceProps.IsFlesh && pawn2.IsColonist)
				{
					if (!flag)
					{
						Widgets.ListSeparator(ref curY, scrollViewRect.width, "CaravanColonists".Translate());
						flag = true;
					}
					this.DoRow(ref curY, scrollViewRect, scrollOutRect, pawn2);
				}
			}
			bool flag2 = false;
			for (int j = 0; j < pawns.Count; j++)
			{
				Pawn pawn3 = pawns[j];
				if (pawn3.RaceProps.IsFlesh && !pawn3.IsColonist)
				{
					if (!flag2)
					{
						Widgets.ListSeparator(ref curY, scrollViewRect.width, "CaravanPrisonersAndAnimals".Translate());
						flag2 = true;
					}
					this.DoRow(ref curY, scrollViewRect, scrollOutRect, pawn3);
				}
			}
		}

		// Token: 0x06015DCE RID: 89550 RVA: 0x00689DA4 File Offset: 0x00687FA4
		private void DoRow(ref float curY, Rect viewRect, Rect scrollOutRect, Pawn p)
		{
			float num = this.scrollPosition.y - 34f;
			float num2 = this.scrollPosition.y + scrollOutRect.height;
			if (curY > num && curY < num2)
			{
				this.DoRow(new Rect(0f, curY, viewRect.width, 34f), p);
			}
			curY += 34f;
		}

		// Token: 0x06015DCF RID: 89551 RVA: 0x00689E0C File Offset: 0x0068800C
		private void DoRow(Rect rect, Pawn p)
		{
			Widgets.BeginGroup(rect);
			Rect rect2 = rect.AtZero();
			CaravanThingsTabUtility.DoAbandonButton(rect2, p, base.SelCaravan);
			rect2.width -= 24f;
			Widgets.InfoCardButton(rect2.width - 24f, (rect.height - 24f) / 2f, p);
			rect2.width -= 24f;
			CaravanThingsTabUtility.DoOpenSpecificTabButton(rect2, p, ref this.specificSocialTabForPawn);
			rect2.width -= 24f;
			CaravanThingsTabUtility.DoOpenSpecificTabButtonInvisible(rect2, p, ref this.specificSocialTabForPawn);
			if (Mouse.IsOver(rect2))
			{
				Widgets.DrawHighlight(rect2);
			}
			Rect rect3 = new Rect(4f, (rect.height - 27f) / 2f, 27f, 27f);
			Widgets.ThingIcon(rect3, p, 1f, null, false, 1f, false);
			Rect bgRect = new Rect(rect3.xMax + 4f, 8f, 100f, 18f);
			GenMapUI.DrawPawnLabel(p, bgRect, 1f, 100f, null, GameFont.Small, false, false);
			if (p.Downed && !p.ageTracker.CurLifeStage.alwaysDowned)
			{
				GUI.color = new Color(1f, 0f, 0f, 0.5f);
				Widgets.DrawLineHorizontal(0f, rect.height / 2f, rect.width);
				GUI.color = Color.white;
			}
			Widgets.EndGroup();
		}

		// Token: 0x06015DD0 RID: 89552 RVA: 0x00689F9D File Offset: 0x0068819D
		public override void Notify_ClearingAllMapsMemory()
		{
			base.Notify_ClearingAllMapsMemory();
			this.specificSocialTabForPawn = null;
		}

		// Token: 0x06015DD1 RID: 89553 RVA: 0x00689FAC File Offset: 0x006881AC
		private void EnsureSpecificSocialTabForPawnValid()
		{
			if (this.specificSocialTabForPawn != null && (this.specificSocialTabForPawn.Destroyed || !base.SelCaravan.ContainsPawn(this.specificSocialTabForPawn)))
			{
				this.specificSocialTabForPawn = null;
			}
		}

		// Token: 0x0400F28F RID: 62095
		private Vector2 scrollPosition;

		// Token: 0x0400F290 RID: 62096
		private float scrollViewHeight;

		// Token: 0x0400F291 RID: 62097
		private Pawn specificSocialTabForPawn;

		// Token: 0x0400F292 RID: 62098
		private const float RowHeight = 34f;

		// Token: 0x0400F293 RID: 62099
		private const float ScrollViewTopMargin = 15f;

		// Token: 0x0400F294 RID: 62100
		private const float PawnLabelHeight = 18f;

		// Token: 0x0400F295 RID: 62101
		private const float PawnLabelColumnWidth = 100f;

		// Token: 0x0400F296 RID: 62102
		private const float SpaceAroundIcon = 4f;
	}
}
