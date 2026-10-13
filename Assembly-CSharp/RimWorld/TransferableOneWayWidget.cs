using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200325D RID: 12893
	[StaticConstructorOnStartup]
	public class TransferableOneWayWidget
	{
		// Token: 0x17002EF4 RID: 12020
		// (get) Token: 0x06012238 RID: 74296 RVA: 0x0054ED9C File Offset: 0x0054CF9C
		public float TotalNumbersColumnsWidths
		{
			get
			{
				float num = 315f;
				if (this.drawMass)
				{
					num += 100f;
				}
				if (this.drawMarketValue)
				{
					num += 100f;
				}
				if (this.drawDaysUntilRot)
				{
					num += 75f;
				}
				if (this.drawItemNutrition)
				{
					num += 75f;
				}
				if (this.drawNutritionEatenPerDay || this.drawMechEnergy)
				{
					num += 75f;
				}
				if (this.drawForagedFoodPerDay)
				{
					num += 75f;
				}
				return num;
			}
		}

		// Token: 0x17002EF5 RID: 12021
		// (get) Token: 0x06012239 RID: 74297 RVA: 0x0054EE18 File Offset: 0x0054D018
		private bool AnyTransferable
		{
			get
			{
				if (!this.transferablesCached)
				{
					this.CacheTransferables();
				}
				for (int i = 0; i < this.sections.Count; i++)
				{
					if (this.sections[i].cachedTransferables.Any<TransferableOneWay>())
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x0601223A RID: 74298 RVA: 0x0054EE64 File Offset: 0x0054D064
		public TransferableOneWayWidget(IEnumerable<TransferableOneWay> transferables, string sourceLabel, string destinationLabel, string sourceCountDesc, bool drawMass = false, IgnorePawnsInventoryMode ignorePawnInventoryMass = IgnorePawnsInventoryMode.DontIgnore, bool includePawnsMassInMassUsage = false, Func<float> availableMassGetter = null, float extraHeaderSpace = 0f, bool ignoreSpawnedCorpseGearAndInventoryMass = false, PlanetTile? tile = null, bool drawMarketValue = false, bool drawEquippedWeapon = false, bool drawNutritionEatenPerDay = false, bool drawMechEnergy = false, bool drawItemNutrition = false, bool drawForagedFoodPerDay = false, bool drawDaysUntilRot = false, bool playerPawnsReadOnly = false, bool drawIdeo = false, bool drawXenotype = false)
		{
			if (transferables != null)
			{
				this.AddSection(null, transferables);
			}
			this.sourceLabel = sourceLabel;
			this.destinationLabel = destinationLabel;
			this.sourceCountDesc = sourceCountDesc;
			this.drawMass = drawMass;
			this.ignorePawnInventoryMass = ignorePawnInventoryMass;
			this.includePawnsMassInMassUsage = includePawnsMassInMassUsage;
			this.availableMassGetter = availableMassGetter;
			this.extraHeaderSpace = extraHeaderSpace;
			this.ignoreSpawnedCorpseGearAndInventoryMass = ignoreSpawnedCorpseGearAndInventoryMass;
			this.tile = (tile ?? PlanetTile.Invalid);
			this.drawMarketValue = drawMarketValue;
			this.drawEquippedWeapon = drawEquippedWeapon;
			this.drawNutritionEatenPerDay = drawNutritionEatenPerDay;
			this.drawMechEnergy = drawMechEnergy;
			this.drawItemNutrition = drawItemNutrition;
			this.drawForagedFoodPerDay = drawForagedFoodPerDay;
			this.drawDaysUntilRot = drawDaysUntilRot;
			this.playerPawnsReadOnly = playerPawnsReadOnly;
			this.drawIdeo = drawIdeo;
			this.drawXenotype = drawXenotype;
			if (drawIdeo && Find.IdeoManager.classicMode)
			{
				drawIdeo = false;
			}
			this.sorter1 = TransferableSorterDefOf.Category;
			this.sorter2 = TransferableSorterDefOf.MarketValue;
		}

		// Token: 0x0601223B RID: 74299 RVA: 0x0054EFB4 File Offset: 0x0054D1B4
		public void AddSection(string title, IEnumerable<TransferableOneWay> transferables)
		{
			TransferableOneWayWidget.Section item = default(TransferableOneWayWidget.Section);
			item.title = title;
			item.transferables = transferables;
			item.cachedTransferables = new List<TransferableOneWay>();
			this.sections.Add(item);
			this.transferablesCached = false;
		}

		// Token: 0x0601223C RID: 74300 RVA: 0x0054EFF8 File Offset: 0x0054D1F8
		private void CacheTransferables()
		{
			this.transferablesCached = true;
			for (int i = 0; i < this.sections.Count; i++)
			{
				List<TransferableOneWay> cachedTransferables = this.sections[i].cachedTransferables;
				cachedTransferables.Clear();
				cachedTransferables.AddRange((from tr in this.sections[i].transferables
				where this.quickSearchWidget.filter.Matches(tr.Label)
				select tr).OrderBy((TransferableOneWay tr) => tr, this.sorter1.Comparer).ThenBy((TransferableOneWay tr) => tr, this.sorter2.Comparer).ThenBy((TransferableOneWay tr) => TransferableUIUtility.DefaultListOrderPriority(tr)).ToList<TransferableOneWay>());
			}
		}

		// Token: 0x0601223D RID: 74301 RVA: 0x0054F0F0 File Offset: 0x0054D2F0
		public void OnGUI(Rect inRect)
		{
			bool flag;
			this.OnGUI(inRect, out flag);
		}

		// Token: 0x0601223E RID: 74302 RVA: 0x0054F108 File Offset: 0x0054D308
		public void OnGUI(Rect inRect, out bool anythingChanged)
		{
			if (!this.transferablesCached)
			{
				this.CacheTransferables();
			}
			TransferableUIUtility.DoTransferableSorters(this.sorter1, this.sorter2, delegate(TransferableSorterDef x)
			{
				this.sorter1 = x;
				this.CacheTransferables();
			}, delegate(TransferableSorterDef x)
			{
				this.sorter2 = x;
				this.CacheTransferables();
			});
			this.quickSearchWidget.noResultsMatched = !this.AnyTransferable;
			TransferableUIUtility.DoTransferableSearcher(this.quickSearchWidget, new Action(this.CacheTransferables));
			if (!this.sourceLabel.NullOrEmpty() || !this.destinationLabel.NullOrEmpty())
			{
				float num = inRect.width - 515f;
				Rect rect = new Rect(inRect.x + num, inRect.y, inRect.width - num, 37f);
				Widgets.BeginGroup(rect);
				Text.Font = GameFont.Medium;
				if (!this.sourceLabel.NullOrEmpty())
				{
					Rect rect2 = new Rect(0f, 0f, rect.width / 2f, rect.height);
					Text.Anchor = TextAnchor.UpperLeft;
					Widgets.Label(rect2, this.sourceLabel);
				}
				if (!this.destinationLabel.NullOrEmpty())
				{
					Rect rect3 = new Rect(rect.width / 2f, 0f, rect.width / 2f, rect.height);
					Text.Anchor = TextAnchor.UpperRight;
					Widgets.Label(rect3, this.destinationLabel);
				}
				Text.Font = GameFont.Small;
				Text.Anchor = TextAnchor.UpperLeft;
				Widgets.EndGroup();
			}
			Rect mainRect = new Rect(inRect.x, inRect.y + 37f + this.extraHeaderSpace, inRect.width, inRect.height - 37f - this.extraHeaderSpace);
			this.FillMainRect(mainRect, out anythingChanged);
		}

		// Token: 0x0601223F RID: 74303 RVA: 0x0054F2B4 File Offset: 0x0054D4B4
		private void FillMainRect(Rect mainRect, out bool anythingChanged)
		{
			anythingChanged = false;
			Text.Font = GameFont.Small;
			if (this.AnyTransferable)
			{
				float num = 6f;
				for (int i = 0; i < this.sections.Count; i++)
				{
					num += (float)this.sections[i].cachedTransferables.Count * 30f;
					if (this.sections[i].title != null)
					{
						num += 30f;
					}
				}
				float num2 = 6f;
				float availableMass = (this.availableMassGetter != null) ? this.availableMassGetter() : float.MaxValue;
				Rect viewRect = new Rect(0f, 0f, mainRect.width - 16f, num);
				Widgets.BeginScrollView(mainRect, ref this.scrollPosition, viewRect, true);
				float num3 = this.scrollPosition.y - 30f;
				float num4 = this.scrollPosition.y + mainRect.height;
				for (int j = 0; j < this.sections.Count; j++)
				{
					List<TransferableOneWay> cachedTransferables = this.sections[j].cachedTransferables;
					if (cachedTransferables.Any<TransferableOneWay>())
					{
						if (this.sections[j].title != null)
						{
							Widgets.ListSeparator(ref num2, viewRect.width, this.sections[j].title);
							num2 += 5f;
						}
						for (int k = 0; k < cachedTransferables.Count; k++)
						{
							if (num2 > num3 && num2 < num4)
							{
								Rect rect = new Rect(0f, num2, viewRect.width, 30f);
								int countToTransfer = cachedTransferables[k].CountToTransfer;
								this.DoRow(rect, cachedTransferables[k], k, availableMass);
								if (countToTransfer != cachedTransferables[k].CountToTransfer)
								{
									anythingChanged = true;
								}
							}
							num2 += 30f;
						}
					}
				}
				Widgets.EndScrollView();
				return;
			}
			GUI.color = Color.gray;
			Text.Anchor = TextAnchor.UpperCenter;
			Widgets.Label(mainRect, "NoneBrackets".Translate());
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.color = Color.white;
		}

		// Token: 0x06012240 RID: 74304 RVA: 0x0054F4D0 File Offset: 0x0054D6D0
		private void DoRow(Rect rect, TransferableOneWay trad, int index, float availableMass)
		{
			if (index % 2 == 1)
			{
				Widgets.DrawLightHighlight(rect);
			}
			Text.Font = GameFont.Small;
			Widgets.BeginGroup(rect);
			float num = rect.width;
			int maxCount = trad.MaxCount;
			Rect rect2 = new Rect(num - 240f, 0f, 240f, rect.height);
			TransferableOneWayWidget.stoppingPoints.Clear();
			if (this.availableMassGetter != null && (!(trad.AnyThing is Pawn) || this.includePawnsMassInMassUsage))
			{
				float num2 = availableMass + this.GetMass(trad.AnyThing) * (float)trad.CountToTransfer;
				int threshold = (num2 <= 0f) ? 0 : Mathf.FloorToInt(num2 / this.GetMass(trad.AnyThing));
				TransferableOneWayWidget.stoppingPoints.Add(new TransferableCountToTransferStoppingPoint(threshold, "M<", ">M"));
			}
			Pawn pawn = trad.AnyThing as Pawn;
			bool flag = pawn != null && (pawn.IsColonist || pawn.IsPrisonerOfColony);
			TransferableUIUtility.DoCountAdjustInterface(rect2, trad, index, 0, maxCount, false, TransferableOneWayWidget.stoppingPoints, (this.playerPawnsReadOnly && flag) || this.readOnly);
			num -= 240f;
			if (this.drawMarketValue)
			{
				Rect rect3 = new Rect(num - 100f, 0f, 100f, rect.height);
				Text.Anchor = TextAnchor.MiddleLeft;
				this.DrawMarketValue(rect3, trad);
				num -= 100f;
			}
			if (this.drawMass)
			{
				Rect rect4 = new Rect(num - 100f, 0f, 100f, rect.height);
				Text.Anchor = TextAnchor.MiddleLeft;
				this.DrawMass(rect4, trad, availableMass);
				num -= 100f;
			}
			if (this.drawDaysUntilRot)
			{
				Rect rect5 = new Rect(num - 75f, 0f, 75f, rect.height);
				Text.Anchor = TextAnchor.MiddleLeft;
				this.DrawDaysUntilRot(rect5, trad);
				num -= 75f;
			}
			if (this.drawItemNutrition)
			{
				Rect rect6 = new Rect(num - 75f, 0f, 75f, rect.height);
				Text.Anchor = TextAnchor.MiddleLeft;
				this.DrawItemNutrition(rect6, trad);
				num -= 75f;
			}
			if (this.drawForagedFoodPerDay)
			{
				Rect rect7 = new Rect(num - 75f, 0f, 75f, rect.height);
				Text.Anchor = TextAnchor.MiddleLeft;
				if (!this.DrawGrazeability(rect7, trad))
				{
					this.DrawForagedFoodPerDay(rect7, trad);
				}
				num -= 75f;
			}
			if (this.drawNutritionEatenPerDay || this.drawMechEnergy)
			{
				bool flag2 = false;
				if (this.drawNutritionEatenPerDay)
				{
					Rect rect8 = new Rect(num - 75f, 0f, 75f, rect.height);
					Text.Anchor = TextAnchor.MiddleLeft;
					flag2 = this.DrawNutritionEatenPerDay(rect8, trad);
				}
				if (ModsConfig.BiotechActive && this.drawMechEnergy && !flag2)
				{
					Rect rect9 = new Rect(num - 75f, 0f, 75f, rect.height);
					this.DrawMechEnergy(rect9, trad);
				}
				num -= 75f;
			}
			if (this.ShouldShowCount(trad))
			{
				Rect rect10 = new Rect(num - 75f, 0f, 75f, rect.height);
				Widgets.DrawHighlightIfMouseover(rect10);
				Text.Anchor = TextAnchor.MiddleLeft;
				Rect rect11 = rect10;
				rect11.xMin += 5f;
				rect11.xMax -= 5f;
				Widgets.Label(rect11, maxCount.ToStringCached());
				TooltipHandler.TipRegion(rect10, this.sourceCountDesc);
			}
			num -= 75f;
			if (this.drawIdeo)
			{
				if (pawn != null && pawn.Ideo != null)
				{
					Rect rect12 = new Rect(num - 30f, 0f, 30f, rect.height);
					Widgets.DrawHighlightIfMouseover(rect12);
					pawn.Ideo.DrawIcon(rect12);
					TooltipHandler.TipRegion(rect12, pawn.Ideo.name);
				}
				num -= 30f;
			}
			if (this.drawXenotype && pawn != null)
			{
				Pawn_GeneTracker genes = pawn.genes;
				if (((genes != null) ? genes.Xenotype : null) != null)
				{
					Rect rect13 = new Rect(num - 30f, 0f, 30f, rect.height);
					Widgets.DrawHighlightIfMouseover(rect13);
					GUI.color = XenotypeDef.IconColor;
					GUI.DrawTexture(rect13, pawn.genes.XenotypeIcon);
					GUI.color = Color.white;
					TooltipHandler.TipRegion(rect13, pawn.genes.XenotypeLabelCap);
				}
			}
			if (pawn != null && pawn.IsSlave)
			{
				Rect rect14 = new Rect(num - 30f, 0f, 30f, rect.height);
				Widgets.DrawHighlightIfMouseover(rect14);
				GUI.DrawTexture(rect14, pawn.guest.GetIcon());
				TooltipHandler.TipRegion(rect14, pawn.guest.GetLabel());
				num -= 30f;
			}
			if (this.drawEquippedWeapon)
			{
				Rect rect15 = new Rect(num - 30f, 0f, 30f, rect.height);
				Rect iconRect = new Rect(num - 30f, (rect.height - 30f) / 2f, 30f, 30f);
				this.DrawEquippedWeapon(rect15, iconRect, trad);
				num -= 30f;
			}
			TransferableUIUtility.DoExtraIcons(trad, rect, ref num);
			Rect idRect = new Rect(0f, 0f, num, rect.height);
			Color labelColor = (pawn != null && pawn.IsSlave) ? PawnNameColorUtility.PawnNameColorOf(pawn) : Color.white;
			TransferableUIUtility.DrawTransferableInfo(trad, idRect, labelColor);
			GenUI.ResetLabelAlign();
			Widgets.EndGroup();
		}

		// Token: 0x06012241 RID: 74305 RVA: 0x0054FA30 File Offset: 0x0054DC30
		private bool ShouldShowCount(TransferableOneWay trad)
		{
			if (!trad.HasAnyThing)
			{
				return true;
			}
			Pawn pawn = trad.AnyThing as Pawn;
			return pawn == null || !pawn.RaceProps.Humanlike || trad.MaxCount != 1;
		}

		// Token: 0x06012242 RID: 74306 RVA: 0x0054FA74 File Offset: 0x0054DC74
		private void DrawDaysUntilRot(Rect rect, TransferableOneWay trad)
		{
			if (!trad.HasAnyThing)
			{
				return;
			}
			if (!trad.ThingDef.IsNutritionGivingIngestible)
			{
				return;
			}
			int num;
			if (!this.cachedTicksUntilRot.TryGetValue(trad, out num))
			{
				num = int.MaxValue;
				for (int i = 0; i < trad.things.Count; i++)
				{
					CompRottable compRottable = trad.things[i].TryGetComp<CompRottable>();
					if (compRottable != null)
					{
						num = Mathf.Min(num, DaysUntilRotCalculator.ApproxTicksUntilRot_AssumeTimePassesBy(compRottable, this.tile, null));
					}
				}
				this.cachedTicksUntilRot.Add(trad, num);
			}
			if (num >= 36000000 || (float)num >= 36000000f)
			{
				return;
			}
			Widgets.DrawHighlightIfMouseover(rect);
			float num2 = (float)num / 60000f;
			GUI.color = Color.yellow;
			Widgets.Label(rect, num2.ToString("0.#"));
			GUI.color = Color.white;
			TooltipHandler.TipRegionByKey(rect, "DaysUntilRotTip");
		}

		// Token: 0x06012243 RID: 74307 RVA: 0x0054FB4C File Offset: 0x0054DD4C
		private void DrawItemNutrition(Rect rect, TransferableOneWay trad)
		{
			if (!trad.HasAnyThing)
			{
				return;
			}
			if (!trad.ThingDef.IsNutritionGivingIngestible)
			{
				return;
			}
			Widgets.DrawHighlightIfMouseover(rect);
			GUI.color = Color.green;
			Widgets.Label(rect, trad.AnyThing.GetStatValue(StatDefOf.Nutrition, true, -1).ToString("0.##"));
			GUI.color = Color.white;
			if (Mouse.IsOver(rect))
			{
				TooltipHandler.TipRegion(rect, "ItemNutritionTip".Translate((1.6f * ThingDefOf.Human.race.baseHungerRate).ToString("0.##")));
			}
		}

		// Token: 0x06012244 RID: 74308 RVA: 0x0054FBF4 File Offset: 0x0054DDF4
		private bool DrawGrazeability(Rect rect, TransferableOneWay trad)
		{
			if (!trad.HasAnyThing)
			{
				return false;
			}
			Pawn pawn = trad.AnyThing as Pawn;
			if (pawn == null || !VirtualPlantsUtility.CanEverEatVirtualPlants(pawn))
			{
				return false;
			}
			rect.width = 40f;
			Rect position = new Rect(rect.x + (float)((int)((rect.width - 28f) / 2f)), rect.y + (float)((int)((rect.height - 28f) / 2f)), 28f, 28f);
			Widgets.DrawHighlightIfMouseover(rect);
			GUI.DrawTexture(position, TransferableOneWayWidget.CanGrazeIcon);
			if (Mouse.IsOver(rect))
			{
				TooltipHandler.TipRegion(rect, delegate()
				{
					TaggedString taggedString = "AnimalCanGrazeTip".Translate();
					if (this.tile.Valid)
					{
						taggedString += "\n\n" + VirtualPlantsUtility.GetVirtualPlantsStatusExplanationAt(this.tile, Find.TickManager.TicksAbs);
					}
					return taggedString;
				}, trad.GetHashCode() ^ 1948571634);
			}
			return true;
		}

		// Token: 0x06012245 RID: 74309 RVA: 0x0054FCB0 File Offset: 0x0054DEB0
		private void DrawForagedFoodPerDay(Rect rect, TransferableOneWay trad)
		{
			if (!trad.HasAnyThing)
			{
				return;
			}
			Pawn p = trad.AnyThing as Pawn;
			if (p == null)
			{
				return;
			}
			bool flag;
			float foragedNutritionPerDay = ForagedFoodPerDayCalculator.GetBaseForagedNutritionPerDay(p, out flag);
			if (flag)
			{
				return;
			}
			Widgets.DrawHighlightIfMouseover(rect);
			GUI.color = ((foragedNutritionPerDay == 0f) ? Color.gray : Color.green);
			Widgets.Label(rect, "+" + foragedNutritionPerDay.ToString("0.##"));
			GUI.color = Color.white;
			if (Mouse.IsOver(rect))
			{
				TooltipHandler.TipRegion(rect, () => "NutritionForagedPerDayTip".Translate(StatDefOf.ForagedNutritionPerDay.Worker.GetExplanationFull(StatRequest.For(p), StatDefOf.ForagedNutritionPerDay.toStringNumberSense, foragedNutritionPerDay)), trad.GetHashCode() ^ 1958671422);
			}
		}

		// Token: 0x06012246 RID: 74310 RVA: 0x0054FD74 File Offset: 0x0054DF74
		private bool DrawNutritionEatenPerDay(Rect rect, TransferableOneWay trad)
		{
			if (!trad.HasAnyThing)
			{
				return false;
			}
			Pawn p = trad.AnyThing as Pawn;
			if (p == null || !p.RaceProps.EatsFood || p.Dead || p.needs.food == null)
			{
				return false;
			}
			Widgets.DrawHighlightIfMouseover(rect);
			string text = RaceProperties.NutritionEatenPerDay(p);
			DietCategory resolvedDietCategory = p.RaceProps.ResolvedDietCategory;
			if (resolvedDietCategory != DietCategory.Omnivorous)
			{
				text = text + " (" + resolvedDietCategory.ToStringHumanShort() + ")";
			}
			GUI.color = new Color(1f, 0.5f, 0f);
			Widgets.Label(rect, text);
			GUI.color = Color.white;
			if (Mouse.IsOver(rect))
			{
				TooltipHandler.TipRegion(rect, () => RaceProperties.NutritionEatenPerDayExplanation(p, true, true, false), trad.GetHashCode() ^ 385968958);
			}
			return true;
		}

		// Token: 0x06012247 RID: 74311 RVA: 0x0054FE6C File Offset: 0x0054E06C
		private void DrawMechEnergy(Rect rect, TransferableOneWay trad)
		{
			if (!ModsConfig.BiotechActive)
			{
				return;
			}
			if (!trad.HasAnyThing)
			{
				return;
			}
			Pawn pawn = trad.AnyThing as Pawn;
			if (pawn != null && !pawn.Dead && pawn.needs.energy != null)
			{
				Widgets.DrawHighlightIfMouseover(rect);
				GUI.color = new Color32(104, 190, byte.MaxValue, byte.MaxValue);
				Widgets.Label(rect, pawn.needs.energy.CurLevelPercentage.ToStringPercent());
				GUI.color = Color.white;
				if (Mouse.IsOver(rect))
				{
					TooltipHandler.TipRegion(rect, "MechEnergy".Translate());
				}
			}
		}

		// Token: 0x06012248 RID: 74312 RVA: 0x0054FF16 File Offset: 0x0054E116
		private void DrawMarketValue(Rect rect, TransferableOneWay trad)
		{
			if (!trad.HasAnyThing || trad.ThingDef.tradeability == Tradeability.None)
			{
				return;
			}
			Widgets.DrawHighlightIfMouseover(rect);
			Widgets.Label(rect, trad.AnyThing.MarketValue.ToStringMoney(null));
			TooltipHandler.TipRegionByKey(rect, "MarketValueTip");
		}

		// Token: 0x06012249 RID: 74313 RVA: 0x0054FF58 File Offset: 0x0054E158
		private void DrawMass(Rect rect, TransferableOneWay trad, float availableMass)
		{
			if (!trad.HasAnyThing)
			{
				return;
			}
			Thing anyThing = trad.AnyThing;
			Pawn pawn = anyThing as Pawn;
			if (pawn != null && !this.includePawnsMassInMassUsage && !MassUtility.CanEverCarryAnything(pawn))
			{
				return;
			}
			Widgets.DrawHighlightIfMouseover(rect);
			if (pawn == null || this.includePawnsMassInMassUsage)
			{
				float mass = this.GetMass(anyThing);
				if (Mouse.IsOver(rect))
				{
					if (pawn != null)
					{
						float gearMass = 0f;
						float invMass = 0f;
						gearMass = MassUtility.GearMass(pawn);
						if (!InventoryCalculatorsUtility.ShouldIgnoreInventoryOf(pawn, this.ignorePawnInventoryMass))
						{
							invMass = MassUtility.InventoryMass(pawn);
						}
						TooltipHandler.TipRegion(rect, () => this.GetPawnMassTip(trad, 0f, mass - gearMass - invMass, gearMass, invMass), trad.GetHashCode() * 59);
					}
					else
					{
						TooltipHandler.TipRegion(rect, "ItemWeightTip".Translate());
					}
				}
				if (mass > availableMass)
				{
					GUI.color = ColorLibrary.RedReadable;
				}
				else
				{
					GUI.color = TransferableOneWayWidget.ItemMassColor;
				}
				Widgets.Label(rect, mass.ToStringMass());
			}
			else
			{
				float cap = MassUtility.Capacity(pawn, null);
				float gearMass = MassUtility.GearMass(pawn);
				float invMass = InventoryCalculatorsUtility.ShouldIgnoreInventoryOf(pawn, this.ignorePawnInventoryMass) ? 0f : MassUtility.InventoryMass(pawn);
				float num = cap - gearMass - invMass;
				if (num > 0f)
				{
					GUI.color = Color.green;
				}
				else if (num < 0f)
				{
					GUI.color = ColorLibrary.RedReadable;
				}
				else
				{
					GUI.color = Color.gray;
				}
				Widgets.Label(rect, num.ToStringMassOffset());
				if (Mouse.IsOver(rect))
				{
					TooltipHandler.TipRegion(rect, () => this.GetPawnMassTip(trad, cap, 0f, gearMass, invMass), trad.GetHashCode() * 59);
				}
			}
			GUI.color = Color.white;
		}

		// Token: 0x0601224A RID: 74314 RVA: 0x00550174 File Offset: 0x0054E374
		private void DrawEquippedWeapon(Rect rect, Rect iconRect, TransferableOneWay trad)
		{
			if (!trad.HasAnyThing)
			{
				return;
			}
			Pawn pawn = trad.AnyThing as Pawn;
			if (pawn == null || pawn.equipment == null || pawn.equipment.Primary == null)
			{
				return;
			}
			ThingWithComps primary = pawn.equipment.Primary;
			Widgets.DrawHighlightIfMouseover(rect);
			Widgets.ThingIcon(iconRect, primary, 1f, null, false, 1f, false);
			if (Mouse.IsOver(rect))
			{
				TooltipHandler.TipRegion(rect, primary.LabelCap);
			}
		}

		// Token: 0x0601224B RID: 74315 RVA: 0x005501F8 File Offset: 0x0054E3F8
		private string GetPawnMassTip(TransferableOneWay trad, float capacity, float pawnMass, float gearMass, float invMass)
		{
			if (!trad.HasAnyThing)
			{
				return "";
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (capacity != 0f)
			{
				stringBuilder.Append("MassCapacity".Translate() + ": " + capacity.ToStringMass());
			}
			else
			{
				stringBuilder.Append("Mass".Translate() + ": " + pawnMass.ToStringMass());
			}
			if (gearMass != 0f)
			{
				stringBuilder.AppendLine();
				stringBuilder.Append("EquipmentAndApparelMass".Translate() + ": " + gearMass.ToStringMass());
			}
			if (invMass != 0f)
			{
				stringBuilder.AppendLine();
				stringBuilder.Append("InventoryMass".Translate() + ": " + invMass.ToStringMass());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0601224C RID: 74316 RVA: 0x005502F8 File Offset: 0x0054E4F8
		private float GetMass(Thing thing)
		{
			if (thing == null)
			{
				return 0f;
			}
			float num = thing.GetStatValue(StatDefOf.Mass, true, -1);
			Pawn pawn = thing as Pawn;
			if (pawn != null)
			{
				if (InventoryCalculatorsUtility.ShouldIgnoreInventoryOf(pawn, this.ignorePawnInventoryMass))
				{
					num -= MassUtility.InventoryMass(pawn);
				}
			}
			else if (this.ignoreSpawnedCorpseGearAndInventoryMass)
			{
				Corpse corpse = thing as Corpse;
				if (corpse != null && corpse.Spawned)
				{
					num -= MassUtility.GearAndInventoryMass(corpse.InnerPawn);
				}
			}
			return num;
		}

		// Token: 0x0400BC4A RID: 48202
		private List<TransferableOneWayWidget.Section> sections = new List<TransferableOneWayWidget.Section>();

		// Token: 0x0400BC4B RID: 48203
		private string sourceLabel;

		// Token: 0x0400BC4C RID: 48204
		private string destinationLabel;

		// Token: 0x0400BC4D RID: 48205
		private string sourceCountDesc;

		// Token: 0x0400BC4E RID: 48206
		private bool drawMass;

		// Token: 0x0400BC4F RID: 48207
		private IgnorePawnsInventoryMode ignorePawnInventoryMass = IgnorePawnsInventoryMode.DontIgnore;

		// Token: 0x0400BC50 RID: 48208
		private bool includePawnsMassInMassUsage;

		// Token: 0x0400BC51 RID: 48209
		private Func<float> availableMassGetter;

		// Token: 0x0400BC52 RID: 48210
		public float extraHeaderSpace;

		// Token: 0x0400BC53 RID: 48211
		private bool ignoreSpawnedCorpseGearAndInventoryMass;

		// Token: 0x0400BC54 RID: 48212
		private PlanetTile tile;

		// Token: 0x0400BC55 RID: 48213
		private bool drawMarketValue;

		// Token: 0x0400BC56 RID: 48214
		private bool drawEquippedWeapon;

		// Token: 0x0400BC57 RID: 48215
		private bool drawNutritionEatenPerDay;

		// Token: 0x0400BC58 RID: 48216
		private bool drawMechEnergy;

		// Token: 0x0400BC59 RID: 48217
		private bool drawItemNutrition;

		// Token: 0x0400BC5A RID: 48218
		private bool drawForagedFoodPerDay;

		// Token: 0x0400BC5B RID: 48219
		private bool drawDaysUntilRot;

		// Token: 0x0400BC5C RID: 48220
		private bool playerPawnsReadOnly;

		// Token: 0x0400BC5D RID: 48221
		public bool readOnly;

		// Token: 0x0400BC5E RID: 48222
		public bool drawIdeo;

		// Token: 0x0400BC5F RID: 48223
		public bool drawXenotype;

		// Token: 0x0400BC60 RID: 48224
		private bool transferablesCached;

		// Token: 0x0400BC61 RID: 48225
		private Vector2 scrollPosition;

		// Token: 0x0400BC62 RID: 48226
		private QuickSearchWidget quickSearchWidget = new QuickSearchWidget();

		// Token: 0x0400BC63 RID: 48227
		private TransferableSorterDef sorter1;

		// Token: 0x0400BC64 RID: 48228
		private TransferableSorterDef sorter2;

		// Token: 0x0400BC65 RID: 48229
		private Dictionary<TransferableOneWay, int> cachedTicksUntilRot = new Dictionary<TransferableOneWay, int>();

		// Token: 0x0400BC66 RID: 48230
		private static List<TransferableCountToTransferStoppingPoint> stoppingPoints = new List<TransferableCountToTransferStoppingPoint>();

		// Token: 0x0400BC67 RID: 48231
		public const float TopAreaHeight = 37f;

		// Token: 0x0400BC68 RID: 48232
		protected readonly Vector2 AcceptButtonSize = new Vector2(160f, 40f);

		// Token: 0x0400BC69 RID: 48233
		protected readonly Vector2 OtherBottomButtonSize = new Vector2(160f, 40f);

		// Token: 0x0400BC6A RID: 48234
		private const float ColumnWidth = 120f;

		// Token: 0x0400BC6B RID: 48235
		private const float FirstTransferableY = 6f;

		// Token: 0x0400BC6C RID: 48236
		private const float RowInterval = 30f;

		// Token: 0x0400BC6D RID: 48237
		public const float CountColumnWidth = 75f;

		// Token: 0x0400BC6E RID: 48238
		public const float AdjustColumnWidth = 240f;

		// Token: 0x0400BC6F RID: 48239
		public const float MassColumnWidth = 100f;

		// Token: 0x0400BC70 RID: 48240
		public static readonly Color ItemMassColor = new Color(0.7f, 0.7f, 0.7f);

		// Token: 0x0400BC71 RID: 48241
		private const float MarketValueColumnWidth = 100f;

		// Token: 0x0400BC72 RID: 48242
		private const float ExtraSpaceAfterSectionTitle = 5f;

		// Token: 0x0400BC73 RID: 48243
		private const float DaysUntilRotColumnWidth = 75f;

		// Token: 0x0400BC74 RID: 48244
		private const float NutritionEatenPerDayColumnWidth = 75f;

		// Token: 0x0400BC75 RID: 48245
		private const float ItemNutritionColumnWidth = 75f;

		// Token: 0x0400BC76 RID: 48246
		private const float ForagedFoodPerDayColumnWidth = 75f;

		// Token: 0x0400BC77 RID: 48247
		private const float GrazeabilityInnerColumnWidth = 40f;

		// Token: 0x0400BC78 RID: 48248
		private const float MiscIconSize = 30f;

		// Token: 0x0400BC79 RID: 48249
		public const float TopAreaWidth = 515f;

		// Token: 0x0400BC7A RID: 48250
		private static readonly Texture2D CanGrazeIcon = ContentFinder<Texture2D>.Get("UI/Icons/CanGraze", true);

		// Token: 0x0200325E RID: 12894
		private struct Section
		{
			// Token: 0x0400BC7B RID: 48251
			public string title;

			// Token: 0x0400BC7C RID: 48252
			public IEnumerable<TransferableOneWay> transferables;

			// Token: 0x0400BC7D RID: 48253
			public List<TransferableOneWay> cachedTransferables;
		}
	}
}
