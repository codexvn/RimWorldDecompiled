using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld.Planet
{
	// Token: 0x02003E66 RID: 15974
	[StaticConstructorOnStartup]
	public class WITab_Caravan_Health : WITab
	{
		// Token: 0x1700369A RID: 13978
		// (get) Token: 0x06015D9A RID: 89498 RVA: 0x0068711A File Offset: 0x0068531A
		private List<Pawn> Pawns
		{
			get
			{
				return base.SelCaravan.PawnsListForReading;
			}
		}

		// Token: 0x1700369B RID: 13979
		// (get) Token: 0x06015D9B RID: 89499 RVA: 0x00688A48 File Offset: 0x00686C48
		private List<PawnCapacityDef> CapacitiesToDisplay
		{
			get
			{
				WITab_Caravan_Health.capacitiesToDisplay.Clear();
				List<PawnCapacityDef> allDefsListForReading = DefDatabase<PawnCapacityDef>.AllDefsListForReading;
				for (int i = 0; i < allDefsListForReading.Count; i++)
				{
					if (allDefsListForReading[i].showOnCaravanHealthTab)
					{
						WITab_Caravan_Health.capacitiesToDisplay.Add(allDefsListForReading[i]);
					}
				}
				WITab_Caravan_Health.capacitiesToDisplay.SortBy((PawnCapacityDef x) => x.listOrder);
				return WITab_Caravan_Health.capacitiesToDisplay;
			}
		}

		// Token: 0x1700369C RID: 13980
		// (get) Token: 0x06015D9C RID: 89500 RVA: 0x00688AC3 File Offset: 0x00686CC3
		private float SpecificHealthTabWidth
		{
			get
			{
				this.EnsureSpecificHealthTabForPawnValid();
				if (this.specificHealthTabForPawn.DestroyedOrNull())
				{
					return 0f;
				}
				return 630f;
			}
		}

		// Token: 0x06015D9D RID: 89501 RVA: 0x00688AE3 File Offset: 0x00686CE3
		public WITab_Caravan_Health()
		{
			this.labelKey = "TabCaravanHealth";
		}

		// Token: 0x06015D9E RID: 89502 RVA: 0x00688AF8 File Offset: 0x00686CF8
		protected override void FillTab()
		{
			this.EnsureSpecificHealthTabForPawnValid();
			Text.Font = GameFont.Small;
			Rect rect = new Rect(0f, 0f, this.size.x, this.size.y).ContractedBy(10f);
			Rect rect2 = new Rect(0f, 0f, rect.width - 16f, this.scrollViewHeight);
			float num = 0f;
			Widgets.BeginScrollView(rect, ref this.scrollPosition, rect2, true);
			this.DoColumnHeaders(ref num);
			this.DoRows(ref num, rect2, rect);
			if (Event.current.type == EventType.Layout)
			{
				this.scrollViewHeight = num + 30f;
			}
			Widgets.EndScrollView();
		}

		// Token: 0x06015D9F RID: 89503 RVA: 0x00688BAC File Offset: 0x00686DAC
		protected override void UpdateSize()
		{
			this.EnsureSpecificHealthTabForPawnValid();
			base.UpdateSize();
			this.size = this.GetRawSize(false);
			if (this.size.x + this.SpecificHealthTabWidth > (float)UI.screenWidth)
			{
				this.compactMode = true;
				this.size = this.GetRawSize(true);
				return;
			}
			this.compactMode = false;
		}

		// Token: 0x06015DA0 RID: 89504 RVA: 0x00688C08 File Offset: 0x00686E08
		protected override void ExtraOnGUI()
		{
			this.EnsureSpecificHealthTabForPawnValid();
			base.ExtraOnGUI();
			Pawn localSpecificHealthTabForPawn = this.specificHealthTabForPawn;
			if (localSpecificHealthTabForPawn != null)
			{
				Rect tabRect = base.TabRect;
				float specificHealthTabWidth = this.SpecificHealthTabWidth;
				Rect rect = new Rect(tabRect.xMax - 1f, tabRect.yMin, specificHealthTabWidth, tabRect.height);
				Find.WindowStack.ImmediateWindow(1439870015, rect, WindowLayer.GameUI, delegate
				{
					if (localSpecificHealthTabForPawn.DestroyedOrNull())
					{
						return;
					}
					HealthCardUtility.DrawPawnHealthCard(new Rect(Vector2.zero, rect.size), localSpecificHealthTabForPawn, false, true, localSpecificHealthTabForPawn);
					if (Widgets.CloseButtonFor(rect.AtZero()))
					{
						this.specificHealthTabForPawn = null;
						SoundDefOf.TabClose.PlayOneShotOnCamera(null);
					}
				}, true, false, 1f, null, false);
			}
		}

		// Token: 0x06015DA1 RID: 89505 RVA: 0x00688CA4 File Offset: 0x00686EA4
		private void DoColumnHeaders(ref float curY)
		{
			if (!this.compactMode)
			{
				float num = 135f;
				Text.Anchor = TextAnchor.UpperCenter;
				GUI.color = Widgets.SeparatorLabelColor;
				Widgets.Label(new Rect(num, 3f, 100f, 100f), "Pain".Translate());
				num += 100f;
				List<PawnCapacityDef> list = this.CapacitiesToDisplay;
				for (int i = 0; i < list.Count; i++)
				{
					Widgets.Label(new Rect(num, 3f, 100f, 100f), list[i].LabelCap.Truncate(100f, null));
					num += 100f;
				}
				Rect rect = new Rect(num + 8f, 0f, 24f, 24f);
				GUI.DrawTexture(rect, WITab_Caravan_Health.BeCarriedIfSickIcon);
				TooltipHandler.TipRegionByKey(rect, "BeCarriedIfSickTip");
				num += 40f;
				Text.Anchor = TextAnchor.UpperLeft;
				GUI.color = Color.white;
			}
		}

		// Token: 0x06015DA2 RID: 89506 RVA: 0x00688D98 File Offset: 0x00686F98
		private void DoRows(ref float curY, Rect scrollViewRect, Rect scrollOutRect)
		{
			List<Pawn> pawns = this.Pawns;
			if (this.specificHealthTabForPawn != null && !pawns.Contains(this.specificHealthTabForPawn))
			{
				this.specificHealthTabForPawn = null;
			}
			bool flag = false;
			for (int i = 0; i < pawns.Count; i++)
			{
				Pawn pawn = pawns[i];
				if (pawn.IsColonist)
				{
					if (!flag)
					{
						Widgets.ListSeparator(ref curY, scrollViewRect.width, "CaravanColonists".Translate());
						flag = true;
					}
					this.DoRow(ref curY, scrollViewRect, scrollOutRect, pawn);
				}
			}
			bool flag2 = false;
			for (int j = 0; j < pawns.Count; j++)
			{
				Pawn pawn2 = pawns[j];
				if (!pawn2.IsColonist)
				{
					if (!flag2)
					{
						Widgets.ListSeparator(ref curY, scrollViewRect.width, ModsConfig.BiotechActive ? "CaravanPrisonersAnimalsAndMechs".Translate() : "CaravanPrisonersAndAnimals".Translate());
						flag2 = true;
					}
					this.DoRow(ref curY, scrollViewRect, scrollOutRect, pawn2);
				}
			}
		}

		// Token: 0x06015DA3 RID: 89507 RVA: 0x00688E84 File Offset: 0x00687084
		private Vector2 GetRawSize(bool compactMode)
		{
			float num = 100f;
			if (!compactMode)
			{
				num += 100f;
				num += (float)this.CapacitiesToDisplay.Count * 100f;
				num += 40f;
			}
			Vector2 result;
			result.x = 127f + num + 16f;
			result.y = Mathf.Min(550f, this.PaneTopY - 30f);
			return result;
		}

		// Token: 0x06015DA4 RID: 89508 RVA: 0x00688EF4 File Offset: 0x006870F4
		private void DoRow(ref float curY, Rect viewRect, Rect scrollOutRect, Pawn p)
		{
			float num = this.scrollPosition.y - 40f;
			float num2 = this.scrollPosition.y + scrollOutRect.height;
			if (curY > num && curY < num2)
			{
				this.DoRow(new Rect(0f, curY, viewRect.width, 40f), p);
			}
			curY += 40f;
		}

		// Token: 0x06015DA5 RID: 89509 RVA: 0x00688F5C File Offset: 0x0068715C
		private void DoRow(Rect rect, Pawn p)
		{
			Widgets.BeginGroup(rect);
			Rect rect2 = rect.AtZero();
			CaravanThingsTabUtility.DoAbandonButton(rect2, p, base.SelCaravan);
			rect2.width -= 24f;
			Widgets.InfoCardButton(rect2.width - 24f, (rect.height - 24f) / 2f, p);
			rect2.width -= 24f;
			CaravanThingsTabUtility.DoOpenSpecificTabButton(rect2, p, ref this.specificHealthTabForPawn);
			rect2.width -= 24f;
			if (Mouse.IsOver(rect2))
			{
				Widgets.DrawHighlight(rect2);
			}
			Rect rect3 = new Rect(4f, (rect.height - 27f) / 2f, 27f, 27f);
			Widgets.ThingIcon(rect3, p, 1f, null, false, 1f, false);
			Rect bgRect = new Rect(rect3.xMax + 4f, 11f, 100f, 18f);
			GenMapUI.DrawPawnLabel(p, bgRect, 1f, 100f, null, GameFont.Small, false, false);
			float num = bgRect.xMax;
			if (!this.compactMode)
			{
				if (p.RaceProps.IsFlesh)
				{
					Rect rect4 = new Rect(num, 0f, 100f, 40f);
					this.DoPain(rect4, p);
				}
				num += 100f;
				List<PawnCapacityDef> list = this.CapacitiesToDisplay;
				for (int i = 0; i < list.Count; i++)
				{
					Rect rect5 = new Rect(num, 0f, 100f, 40f);
					if ((p.RaceProps.Humanlike && !list[i].showOnHumanlikes) || (p.RaceProps.Animal && !list[i].showOnAnimals) || (p.RaceProps.IsAnomalyEntity && !list[i].showOnAnomalyEntities) || (p.RaceProps.IsMechanoid && !list[i].showOnMechanoids) || (p.RaceProps.IsDrone && !list[i].showOnDrones) || !PawnCapacityUtility.BodyCanEverDoCapacity(p.RaceProps.body, list[i]))
					{
						num += 100f;
					}
					else
					{
						this.DoCapacity(rect5, p, list[i]);
						num += 100f;
					}
				}
			}
			if (!this.compactMode)
			{
				Vector2 vector = new Vector2(num + 8f, 8f);
				Widgets.Checkbox(vector, ref p.health.beCarriedByCaravanIfSick, 24f, false, true, null, null);
				TooltipHandler.TipRegionByKey(new Rect(vector, new Vector2(24f, 24f)), "BeCarriedIfSickTip");
				num += 40f;
			}
			if (p.Downed && !p.ageTracker.CurLifeStage.alwaysDowned)
			{
				GUI.color = new Color(1f, 0f, 0f, 0.5f);
				Widgets.DrawLineHorizontal(0f, rect.height / 2f, rect.width);
				GUI.color = Color.white;
			}
			Widgets.EndGroup();
		}

		// Token: 0x06015DA6 RID: 89510 RVA: 0x00689298 File Offset: 0x00687498
		private void DoPain(Rect rect, Pawn pawn)
		{
			Pair<string, Color> painLabel = HealthCardUtility.GetPainLabel(pawn);
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawHighlight(rect);
			}
			GUI.color = painLabel.Second;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect, painLabel.First);
			GUI.color = Color.white;
			Text.Anchor = TextAnchor.UpperLeft;
			if (Mouse.IsOver(rect))
			{
				string painTip = HealthCardUtility.GetPainTip(pawn);
				TooltipHandler.TipRegion(rect, painTip);
			}
		}

		// Token: 0x06015DA7 RID: 89511 RVA: 0x00689304 File Offset: 0x00687504
		private void DoCapacity(Rect rect, Pawn pawn, PawnCapacityDef capacity)
		{
			Pair<string, Color> efficiencyLabel = HealthCardUtility.GetEfficiencyLabel(pawn, capacity);
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawHighlight(rect);
			}
			GUI.color = efficiencyLabel.Second;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect, efficiencyLabel.First);
			GUI.color = Color.white;
			Text.Anchor = TextAnchor.UpperLeft;
			if (Mouse.IsOver(rect))
			{
				string pawnCapacityTip = HealthCardUtility.GetPawnCapacityTip(pawn, capacity);
				TooltipHandler.TipRegion(rect, pawnCapacityTip);
			}
		}

		// Token: 0x06015DA8 RID: 89512 RVA: 0x00689372 File Offset: 0x00687572
		public override void Notify_ClearingAllMapsMemory()
		{
			base.Notify_ClearingAllMapsMemory();
			this.specificHealthTabForPawn = null;
		}

		// Token: 0x06015DA9 RID: 89513 RVA: 0x00689381 File Offset: 0x00687581
		private void EnsureSpecificHealthTabForPawnValid()
		{
			if (this.specificHealthTabForPawn != null && (this.specificHealthTabForPawn.Destroyed || !base.SelCaravan.ContainsPawn(this.specificHealthTabForPawn)))
			{
				this.specificHealthTabForPawn = null;
			}
		}

		// Token: 0x0400F268 RID: 62056
		private Vector2 scrollPosition;

		// Token: 0x0400F269 RID: 62057
		private float scrollViewHeight;

		// Token: 0x0400F26A RID: 62058
		private Pawn specificHealthTabForPawn;

		// Token: 0x0400F26B RID: 62059
		private bool compactMode;

		// Token: 0x0400F26C RID: 62060
		private static List<PawnCapacityDef> capacitiesToDisplay = new List<PawnCapacityDef>();

		// Token: 0x0400F26D RID: 62061
		private const float RowHeight = 40f;

		// Token: 0x0400F26E RID: 62062
		private const float PawnLabelHeight = 18f;

		// Token: 0x0400F26F RID: 62063
		private const float PawnLabelColumnWidth = 100f;

		// Token: 0x0400F270 RID: 62064
		private const float SpaceAroundIcon = 4f;

		// Token: 0x0400F271 RID: 62065
		private const float PawnCapacityColumnWidth = 100f;

		// Token: 0x0400F272 RID: 62066
		private const float BeCarriedIfSickColumnWidth = 40f;

		// Token: 0x0400F273 RID: 62067
		private const float BeCarriedIfSickIconSize = 24f;

		// Token: 0x0400F274 RID: 62068
		private static readonly Texture2D BeCarriedIfSickIcon = ContentFinder<Texture2D>.Get("UI/Icons/CarrySick", true);
	}
}
