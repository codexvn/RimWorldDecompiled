using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02003177 RID: 12663
	public class Dialog_FormCaravan : Window
	{
		// Token: 0x17002E39 RID: 11833
		// (get) Token: 0x06011CD4 RID: 72916 RVA: 0x00527DE9 File Offset: 0x00525FE9
		public PlanetTile CurrentTile
		{
			get
			{
				return this.map.Tile;
			}
		}

		// Token: 0x17002E3A RID: 11834
		// (get) Token: 0x06011CD5 RID: 72917 RVA: 0x00526A3B File Offset: 0x00524C3B
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(1024f, (float)UI.screenHeight);
			}
		}

		// Token: 0x17002E3B RID: 11835
		// (get) Token: 0x06011CD6 RID: 72918 RVA: 0x00006AAC File Offset: 0x00004CAC
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17002E3C RID: 11836
		// (get) Token: 0x06011CD7 RID: 72919 RVA: 0x00527DF6 File Offset: 0x00525FF6
		private bool AutoStripSpawnedCorpses
		{
			get
			{
				return this.reform;
			}
		}

		// Token: 0x17002E3D RID: 11837
		// (get) Token: 0x06011CD8 RID: 72920 RVA: 0x00527DF6 File Offset: 0x00525FF6
		private bool ListPlayerPawnsInventorySeparately
		{
			get
			{
				return this.reform;
			}
		}

		// Token: 0x17002E3E RID: 11838
		// (get) Token: 0x06011CD9 RID: 72921 RVA: 0x00527DFE File Offset: 0x00525FFE
		private BiomeDef Biome
		{
			get
			{
				return this.map.Biome;
			}
		}

		// Token: 0x17002E3F RID: 11839
		// (get) Token: 0x06011CDA RID: 72922 RVA: 0x00527E0B File Offset: 0x0052600B
		private bool MustChooseRoute
		{
			get
			{
				return this.canChooseRoute && (!this.reform || this.map.Parent is Settlement);
			}
		}

		// Token: 0x17002E40 RID: 11840
		// (get) Token: 0x06011CDB RID: 72923 RVA: 0x00527E34 File Offset: 0x00526034
		private bool ShowCancelButton
		{
			get
			{
				if (!this.mapAboutToBeRemoved)
				{
					return true;
				}
				bool flag = false;
				for (int i = 0; i < this.transferables.Count; i++)
				{
					Pawn pawn = this.transferables[i].AnyThing as Pawn;
					if (pawn != null && pawn.IsColonist && !pawn.Downed)
					{
						flag = true;
						break;
					}
				}
				return !flag;
			}
		}

		// Token: 0x17002E41 RID: 11841
		// (get) Token: 0x06011CDC RID: 72924 RVA: 0x00527E97 File Offset: 0x00526097
		private IgnorePawnsInventoryMode IgnoreInventoryMode
		{
			get
			{
				if (!this.ListPlayerPawnsInventorySeparately)
				{
					return IgnorePawnsInventoryMode.IgnoreIfAssignedToUnload;
				}
				return IgnorePawnsInventoryMode.IgnoreIfAssignedToUnloadOrPlayerPawn;
			}
		}

		// Token: 0x17002E42 RID: 11842
		// (get) Token: 0x06011CDD RID: 72925 RVA: 0x00527EA4 File Offset: 0x005260A4
		public float MassUsage
		{
			get
			{
				if (this.massUsageDirty)
				{
					this.massUsageDirty = false;
					this.cachedMassUsage = CollectionsMassCalculator.MassUsageTransferables(this.transferables, this.IgnoreInventoryMode, false, this.AutoStripSpawnedCorpses);
				}
				return this.cachedMassUsage;
			}
		}

		// Token: 0x17002E43 RID: 11843
		// (get) Token: 0x06011CDE RID: 72926 RVA: 0x00527EDC File Offset: 0x005260DC
		public float MassCapacity
		{
			get
			{
				if (this.massCapacityDirty)
				{
					this.massCapacityDirty = false;
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedMassCapacity = CollectionsMassCalculator.CapacityTransferables(this.transferables, stringBuilder);
					this.cachedMassCapacityExplanation = stringBuilder.ToString();
				}
				return this.cachedMassCapacity;
			}
		}

		// Token: 0x17002E44 RID: 11844
		// (get) Token: 0x06011CDF RID: 72927 RVA: 0x00527F24 File Offset: 0x00526124
		private float TilesPerDay
		{
			get
			{
				if (this.tilesPerDayDirty)
				{
					this.tilesPerDayDirty = false;
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedTilesPerDay = TilesPerDayCalculator.ApproxTilesPerDay(this.transferables, this.MassUsage, this.MassCapacity, this.CurrentTile, this.startingTile, false, stringBuilder);
					this.cachedTilesPerDayExplanation = stringBuilder.ToString();
				}
				return this.cachedTilesPerDay;
			}
		}

		// Token: 0x17002E45 RID: 11845
		// (get) Token: 0x06011CE0 RID: 72928 RVA: 0x00527F84 File Offset: 0x00526184
		[TupleElementNames(new string[]
		{
			"days",
			"tillRot"
		})]
		private ValueTuple<float, float> DaysWorthOfFood
		{
			[return: TupleElementNames(new string[]
			{
				"days",
				"tillRot"
			})]
			get
			{
				if (this.daysWorthOfFoodDirty)
				{
					this.daysWorthOfFoodDirty = false;
					float item;
					float item2;
					if (this.destinationTile.Valid)
					{
						using (WorldPath worldPath = this.CurrentTile.Layer.Pather.FindPath(this.CurrentTile, this.destinationTile, null, null))
						{
							int ticksPerMove = CaravanTicksPerMoveUtility.GetTicksPerMove(new CaravanTicksPerMoveUtility.CaravanInfo(this), null);
							item = DaysWorthOfFoodCalculator.ApproxDaysWorthOfFood(this.transferables, this.CurrentTile, this.IgnoreInventoryMode, Faction.OfPlayer, worldPath, 0f, ticksPerMove);
							item2 = DaysUntilRotCalculator.ApproxDaysUntilRot(this.transferables, this.CurrentTile, this.IgnoreInventoryMode, worldPath, 0f, ticksPerMove);
							goto IL_F1;
						}
					}
					item = DaysWorthOfFoodCalculator.ApproxDaysWorthOfFood(this.transferables, this.CurrentTile, this.IgnoreInventoryMode, Faction.OfPlayer, null, 0f, 3300);
					item2 = DaysUntilRotCalculator.ApproxDaysUntilRot(this.transferables, this.CurrentTile, this.IgnoreInventoryMode, null, 0f, 3300);
					IL_F1:
					this.cachedDaysWorthOfFood = new ValueTuple<float, float>(item, item2);
				}
				return this.cachedDaysWorthOfFood;
			}
		}

		// Token: 0x17002E46 RID: 11846
		// (get) Token: 0x06011CE1 RID: 72929 RVA: 0x005280A8 File Offset: 0x005262A8
		[TupleElementNames(new string[]
		{
			"food",
			"perDay"
		})]
		private ValueTuple<ThingDef, float> ForagedFoodPerDay
		{
			[return: TupleElementNames(new string[]
			{
				"food",
				"perDay"
			})]
			get
			{
				if (this.foragedFoodPerDayDirty)
				{
					this.foragedFoodPerDayDirty = false;
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedForagedFoodPerDay = ForagedFoodPerDayCalculator.ForagedFoodPerDay(this.transferables, this.Biome, Faction.OfPlayer, stringBuilder);
					this.cachedForagedFoodPerDayExplanation = stringBuilder.ToString();
				}
				return this.cachedForagedFoodPerDay;
			}
		}

		// Token: 0x17002E47 RID: 11847
		// (get) Token: 0x06011CE2 RID: 72930 RVA: 0x005280FC File Offset: 0x005262FC
		private float Visibility
		{
			get
			{
				if (this.visibilityDirty)
				{
					this.visibilityDirty = false;
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedVisibility = CaravanVisibilityCalculator.Visibility(this.transferables, stringBuilder);
					this.cachedVisibilityExplanation = stringBuilder.ToString();
				}
				return this.cachedVisibility;
			}
		}

		// Token: 0x17002E48 RID: 11848
		// (get) Token: 0x06011CE3 RID: 72931 RVA: 0x00528144 File Offset: 0x00526344
		private int TicksToArrive
		{
			get
			{
				if (!this.destinationTile.Valid)
				{
					return 0;
				}
				if (this.ticksToArriveDirty)
				{
					this.ticksToArriveDirty = false;
					using (WorldPath worldPath = this.CurrentTile.Layer.Pather.FindPath(this.CurrentTile, this.destinationTile, null, null))
					{
						this.cachedTicksToArrive = CaravanArrivalTimeEstimator.EstimatedTicksToArrive(this.CurrentTile, this.destinationTile, worldPath, 0f, CaravanTicksPerMoveUtility.GetTicksPerMove(new CaravanTicksPerMoveUtility.CaravanInfo(this), null), Find.TickManager.TicksAbs);
					}
				}
				return this.cachedTicksToArrive;
			}
		}

		// Token: 0x17002E49 RID: 11849
		// (get) Token: 0x06011CE4 RID: 72932 RVA: 0x005281EC File Offset: 0x005263EC
		private bool MostFoodWillRotSoon
		{
			get
			{
				float num = 0f;
				float num2 = 0f;
				for (int i = 0; i < this.transferables.Count; i++)
				{
					TransferableOneWay transferableOneWay = this.transferables[i];
					if (transferableOneWay.HasAnyThing && transferableOneWay.CountToTransfer > 0 && transferableOneWay.ThingDef.IsNutritionGivingIngestible && !(transferableOneWay.AnyThing is Corpse))
					{
						float num3 = 600f;
						CompRottable compRottable = transferableOneWay.AnyThing.TryGetComp<CompRottable>();
						if (compRottable != null)
						{
							num3 = (float)DaysUntilRotCalculator.ApproxTicksUntilRot_AssumeTimePassesBy(compRottable, this.CurrentTile, null) / 60000f;
						}
						float num4 = transferableOneWay.ThingDef.GetStatValueAbstract(StatDefOf.Nutrition, null) * (float)transferableOneWay.CountToTransfer;
						if (num3 < 5f)
						{
							num += num4;
						}
						else
						{
							num2 += num4;
						}
					}
				}
				return (num != 0f || num2 != 0f) && num / (num + num2) >= 0.75f;
			}
		}

		// Token: 0x06011CE5 RID: 72933 RVA: 0x005282DC File Offset: 0x005264DC
		public Dialog_FormCaravan(Map map, bool reform = false, Action onClosed = null, bool mapAboutToBeRemoved = false, IntVec3? designatedMeetingPoint = null) : base(null)
		{
			this.map = map;
			this.reform = reform;
			this.onClosed = onClosed;
			this.mapAboutToBeRemoved = mapAboutToBeRemoved;
			this.designatedMeetingPoint = (designatedMeetingPoint ?? IntVec3.Invalid);
			this.canChooseRoute = (!reform || !map.retainedCaravanData.HasDestinationTile);
			this.closeOnAccept = true;
			this.closeOnCancel = !reform;
			this.autoSelectTravelSupplies = !reform;
			this.forcePause = true;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x06011CE6 RID: 72934 RVA: 0x005283CD File Offset: 0x005265CD
		public override void PostOpen()
		{
			base.PostOpen();
			this.choosingRoute = false;
			if (!this.thisWindowInstanceEverOpened)
			{
				this.thisWindowInstanceEverOpened = true;
				this.CalculateAndRecacheTransferables();
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.FormCaravan, KnowledgeAmount.Total);
				Find.WorldRoutePlanner.Start(this);
			}
		}

		// Token: 0x06011CE7 RID: 72935 RVA: 0x00528407 File Offset: 0x00526607
		public override void PostClose()
		{
			base.PostClose();
			if (this.onClosed != null && !this.choosingRoute)
			{
				this.onClosed();
			}
		}

		// Token: 0x06011CE8 RID: 72936 RVA: 0x0052842A File Offset: 0x0052662A
		public void Notify_NoLongerChoosingRoute()
		{
			this.choosingRoute = false;
			if (!Find.WindowStack.IsOpen(this) && this.onClosed != null)
			{
				this.onClosed();
			}
		}

		// Token: 0x06011CE9 RID: 72937 RVA: 0x00528454 File Offset: 0x00526654
		public override void DoWindowContents(Rect inRect)
		{
			Rect rect = new Rect(0f, 0f, inRect.width, 35f);
			Text.Font = GameFont.Medium;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect, (this.reform ? "ReformCaravan" : "FormCaravan").Translate());
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.UpperLeft;
			CaravanUIUtility.DrawCaravanInfo(new CaravanUIUtility.CaravanInfo(this.MassUsage, this.MassCapacity, this.cachedMassCapacityExplanation, this.TilesPerDay, this.cachedTilesPerDayExplanation, this.DaysWorthOfFood, this.ForagedFoodPerDay, this.cachedForagedFoodPerDayExplanation, this.Visibility, this.cachedVisibilityExplanation, -1f, -1f, null), null, this.CurrentTile, (!this.destinationTile.Valid) ? null : new int?(this.TicksToArrive), this.lastMassFlashTime, new Rect(12f, 35f, inRect.width - 24f, 40f), true, (!this.destinationTile.Valid) ? null : ("\n" + "DaysWorthOfFoodTooltip_OnlyFirstWaypoint".Translate()), false, true);
			Dialog_FormCaravan.tabsList.Clear();
			Dialog_FormCaravan.tabsList.Add(new TabRecord("PawnsTab".Translate(), delegate()
			{
				this.tab = Dialog_FormCaravan.Tab.Pawns;
			}, this.tab == Dialog_FormCaravan.Tab.Pawns));
			Dialog_FormCaravan.tabsList.Add(new TabRecord("ItemsTab".Translate(), delegate()
			{
				this.tab = Dialog_FormCaravan.Tab.Items;
			}, this.tab == Dialog_FormCaravan.Tab.Items));
			Dialog_FormCaravan.tabsList.Add(new TabRecord("TravelSupplies".Translate(), delegate()
			{
				this.tab = Dialog_FormCaravan.Tab.TravelSupplies;
			}, this.tab == Dialog_FormCaravan.Tab.TravelSupplies));
			inRect.yMin += 119f;
			Widgets.DrawMenuSection(inRect);
			TabDrawer.DrawTabs<TabRecord>(inRect, Dialog_FormCaravan.tabsList, 200f);
			Dialog_FormCaravan.tabsList.Clear();
			inRect = inRect.ContractedBy(17f);
			inRect.height += 17f;
			Widgets.BeginGroup(inRect);
			Rect rect2 = inRect.AtZero();
			this.DoBottomButtons(rect2);
			Rect rect3 = rect2;
			rect3.yMax -= 76f;
			bool flag = false;
			switch (this.tab)
			{
			case Dialog_FormCaravan.Tab.Pawns:
				this.pawnsTransfer.OnGUI(rect3, out flag);
				break;
			case Dialog_FormCaravan.Tab.Items:
				this.itemsTransfer.OnGUI(rect3, out flag);
				break;
			case Dialog_FormCaravan.Tab.TravelSupplies:
				this.travelSuppliesTransfer.extraHeaderSpace = 35f;
				this.travelSuppliesTransfer.OnGUI(rect3, out flag);
				this.DrawAutoSelectCheckbox(rect3, ref flag);
				break;
			}
			if (flag)
			{
				this.Notify_TransferablesChanged();
			}
			Widgets.EndGroup();
		}

		// Token: 0x06011CEA RID: 72938 RVA: 0x00528724 File Offset: 0x00526924
		public void DrawAutoSelectCheckbox(Rect rect, ref bool anythingChanged)
		{
			rect.yMin += 37f;
			rect.height = 35f;
			bool flag = this.autoSelectTravelSupplies;
			Widgets.CheckboxLabeled(rect, "AutomaticallySelectTravelSupplies".Translate(), ref this.autoSelectTravelSupplies, false, null, null, true, false);
			this.travelSuppliesTransfer.readOnly = this.autoSelectTravelSupplies;
			if (flag != this.autoSelectTravelSupplies)
			{
				anythingChanged = true;
			}
		}

		// Token: 0x06011CEB RID: 72939 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool CausesMessageBackground()
		{
			return true;
		}

		// Token: 0x06011CEC RID: 72940 RVA: 0x00528794 File Offset: 0x00526994
		public void Notify_ChoseRoute(PlanetTile destinationTile)
		{
			this.destinationTile = destinationTile;
			this.startingTile = CaravanExitMapUtility.BestExitTileToGoTo(destinationTile, this.map);
			this.ticksToArriveDirty = true;
			this.daysWorthOfFoodDirty = true;
			this.soundAppear.PlayOneShotOnCamera(null);
			if (this.autoSelectTravelSupplies)
			{
				this.SelectApproximateBestTravelSupplies();
			}
		}

		// Token: 0x06011CED RID: 72941 RVA: 0x005287E4 File Offset: 0x005269E4
		private void AddToTransferables(Thing t, bool setToTransferMax = false)
		{
			TransferableOneWay transferableOneWay = TransferableUtility.TransferableMatching<TransferableOneWay>(t, this.transferables, TransferAsOneMode.PodsOrCaravanPacking);
			if (transferableOneWay == null)
			{
				transferableOneWay = new TransferableOneWay();
				this.transferables.Add(transferableOneWay);
			}
			if (transferableOneWay.things.Contains(t))
			{
				Log.Error("Tried to add the same thing twice to TransferableOneWay: " + ((t != null) ? t.ToString() : null));
				return;
			}
			transferableOneWay.things.Add(t);
			if (setToTransferMax)
			{
				transferableOneWay.AdjustTo(transferableOneWay.CountToTransfer + t.stackCount);
			}
		}

		// Token: 0x06011CEE RID: 72942 RVA: 0x00528864 File Offset: 0x00526A64
		private void DoBottomButtons(Rect rect)
		{
			Rect rect2 = new Rect(rect.width - Dialog_FormCaravan.BottomButtonSize.x, rect.height - 55f - 17f, Dialog_FormCaravan.BottomButtonSize.x, Dialog_FormCaravan.BottomButtonSize.y);
			if (Widgets.ButtonText(rect2, "Send".Translate(), true, true, true, null))
			{
				this.TrySend();
			}
			if (this.ShowCancelButton && Widgets.ButtonText(new Rect(0f, rect2.y, Dialog_FormCaravan.BottomButtonSize.x, Dialog_FormCaravan.BottomButtonSize.y), "CancelButton".Translate(), true, true, true, null))
			{
				this.Close(true);
			}
			if (Widgets.ButtonText(new Rect(rect.width / 2f - Dialog_FormCaravan.BottomButtonSize.x - 8.5f, rect2.y, Dialog_FormCaravan.BottomButtonSize.x, Dialog_FormCaravan.BottomButtonSize.y), "ResetButton".Translate(), true, true, true, null))
			{
				SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
				this.CalculateAndRecacheTransferables();
			}
			if (this.canChooseRoute)
			{
				if (Widgets.ButtonText(new Rect(rect.width / 2f + 8.5f, rect2.y, Dialog_FormCaravan.BottomButtonSize.x, Dialog_FormCaravan.BottomButtonSize.y), "ChangeRouteButton".Translate(), true, true, true, null))
				{
					this.soundClose.PlayOneShotOnCamera(null);
					Find.WorldRoutePlanner.Start(this);
				}
				if (this.destinationTile.Valid)
				{
					Rect rect3 = rect2;
					rect3.y += rect2.height + 4f;
					rect3.height = 200f;
					rect3.xMin -= 200f;
					Text.Anchor = TextAnchor.UpperRight;
					Widgets.Label(rect3, "CaravanEstimatedDaysToDestination".Translate(((float)this.TicksToArrive / 60000f).ToString("0.#")));
					Text.Anchor = TextAnchor.UpperLeft;
				}
			}
			if (Prefs.DevMode)
			{
				float width = 200f;
				float height = Dialog_FormCaravan.BottomButtonSize.y / 2f;
				if (Widgets.ButtonText(new Rect(0f, rect2.yMax + 4f, width, height), "DEV: Send instantly", true, true, true, null) && this.DebugTryFormCaravanInstantly())
				{
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
					this.Close(false);
				}
				if (Widgets.ButtonText(new Rect(204f, rect2.yMax + 4f, width, height), "DEV: Select everything", true, true, true, null))
				{
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
					this.SetToSendEverything();
				}
			}
		}

		// Token: 0x06011CEF RID: 72943 RVA: 0x00528B50 File Offset: 0x00526D50
		private void TrySend()
		{
			if (this.reform)
			{
				if (this.TryReformCaravan())
				{
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
					this.Close(false);
					return;
				}
			}
			else
			{
				List<string> list = new List<string>();
				List<Pawn> pawns = TransferableUtility.GetPawnsFromTransferables(this.transferables);
				ValueTuple<float, float> daysWorthOfFood = this.DaysWorthOfFood;
				if (daysWorthOfFood.Item1 < 5f)
				{
					list.Add((daysWorthOfFood.Item1 < 0.1f) ? "DaysWorthOfFoodWarningDialog_NoFood".Translate().ToString() : "DaysWorthOfFoodWarningDialog".Translate(daysWorthOfFood.Item1.ToString("0.#")).Resolve());
				}
				else if (this.MostFoodWillRotSoon)
				{
					list.Add("CaravanFoodWillRotSoonWarningDialog".Translate());
				}
				if (!pawns.Any((Pawn pawn) => CaravanUtility.IsOwner(pawn, Faction.OfPlayer) && !pawn.skills.GetSkill(SkillDefOf.Social).TotallyDisabled))
				{
					list.Add("CaravanIncapableOfSocial".Translate());
				}
				if (this.ShouldShowWarningForUndesirableFood())
				{
					list.Add("DaysWorthOfFoodDietWarningDialog".Translate());
				}
				if (this.ShouldShowWarningForMechWithoutMechanitor())
				{
					list.Add("CaravanLacksMechMechanitorWarning".Translate());
				}
				if (ModsConfig.BiotechActive)
				{
					IEnumerable<Pawn> source = pawns.Where(delegate(Pawn x)
					{
						Hediff_PsychicBond hediff_PsychicBond = x.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PsychicBond, false) as Hediff_PsychicBond;
						return hediff_PsychicBond != null && ThoughtWorker_PsychicBondProximity.NearPsychicBondedPerson(x, hediff_PsychicBond) && !pawns.Contains(hediff_PsychicBond.target);
					});
					if (source.Any<Pawn>())
					{
						list.Add("PsychicBondDistanceWillBeActive_Caravan".Translate().ToString() + ":\n" + (from x in source
						select x.NameFullColored.Resolve() + " (" + "Partner".Translate((x.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PsychicBond, false) as Hediff_PsychicBond).target).Resolve() + ")").ToLineList("  - ", true));
					}
				}
				if (list.Count > 0)
				{
					if (this.CheckForErrors(pawns))
					{
						string str2 = string.Concat((from str in list
						select str + "\n\n").ToArray<string>()) + "CaravanAreYouSure".Translate().ToString();
						Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(str2, delegate()
						{
							if (this.TryFormAndSendCaravan())
							{
								this.Close(false);
							}
						}, false, null, WindowLayer.Dialog));
						return;
					}
				}
				else if (this.TryFormAndSendCaravan())
				{
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
					this.Close(false);
				}
			}
		}

		// Token: 0x06011CF0 RID: 72944 RVA: 0x00528DD0 File Offset: 0x00526FD0
		private void CalculateAndRecacheTransferables()
		{
			this.transferables = new List<TransferableOneWay>();
			this.AddPawnsToTransferables();
			this.AddItemsToTransferables();
			CaravanUIUtility.CreateCaravanTransferableWidgets(this.transferables, out this.pawnsTransfer, out this.itemsTransfer, out this.travelSuppliesTransfer, "FormCaravanColonyThingCountTip".Translate(), this.IgnoreInventoryMode, () => this.MassCapacity - this.MassUsage, this.AutoStripSpawnedCorpses, this.CurrentTile, this.mapAboutToBeRemoved);
			this.Notify_TransferablesChanged();
		}

		// Token: 0x06011CF1 RID: 72945 RVA: 0x00528E4C File Offset: 0x0052704C
		private bool DebugTryFormCaravanInstantly()
		{
			List<Pawn> pawnsFromTransferables = TransferableUtility.GetPawnsFromTransferables(this.transferables);
			if (!pawnsFromTransferables.Any((Pawn x) => CaravanUtility.IsOwner(x, Faction.OfPlayer)))
			{
				if (ModsConfig.IdeologyActive)
				{
					Messages.Message("CaravanMustHaveAtLeastOneNonSlaveColonist".Translate(), MessageTypeDefOf.RejectInput, false);
				}
				else
				{
					Messages.Message("CaravanMustHaveAtLeastOneColonist".Translate(), MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			this.AddItemsFromTransferablesToRandomInventories(pawnsFromTransferables);
			PlanetTile directionTile = this.startingTile;
			if (!directionTile.Valid)
			{
				directionTile = CaravanExitMapUtility.RandomBestExitTileFrom(this.map);
			}
			if (!directionTile.Valid)
			{
				directionTile = this.CurrentTile;
			}
			CaravanFormingUtility.FormAndCreateCaravan(pawnsFromTransferables, Faction.OfPlayer, this.CurrentTile, directionTile, this.destinationTile);
			return true;
		}

		// Token: 0x06011CF2 RID: 72946 RVA: 0x00528F18 File Offset: 0x00527118
		private bool TryFormAndSendCaravan()
		{
			List<Pawn> pawns = TransferableUtility.GetPawnsFromTransferables(this.transferables);
			if (!this.CheckForErrors(pawns))
			{
				return false;
			}
			IntVec3 root = pawns.Aggregate(IntVec3.Zero, (IntVec3 acc, Pawn p) => acc + p.Position) / pawns.Count;
			Direction8Way direction8WayFromTo = Find.WorldGrid.GetDirection8WayFromTo(this.CurrentTile, this.startingTile);
			IntVec3 exitSpot;
			if (!RCellFinder.TryFindClosestEdgeCellTo(root, this.map, out exitSpot) && !this.TryFindExitSpot(pawns, true, out exitSpot))
			{
				if (!this.TryFindExitSpot(pawns, false, out exitSpot))
				{
					Messages.Message("CaravanCouldNotFindExitSpot".Translate(direction8WayFromTo.LabelShort()), MessageTypeDefOf.RejectInput, false);
					return false;
				}
				Messages.Message("CaravanCouldNotFindReachableExitSpot".Translate(direction8WayFromTo.LabelShort()), new GlobalTargetInfo(exitSpot, this.map, false), MessageTypeDefOf.CautionInput, false);
			}
			IntVec3 meetingPoint = this.designatedMeetingPoint;
			if (meetingPoint == IntVec3.Invalid && !this.TryFindRandomPackingSpot(exitSpot, out meetingPoint))
			{
				Messages.Message("CaravanCouldNotFindPackingSpot".Translate(direction8WayFromTo.LabelShort()), new GlobalTargetInfo(exitSpot, this.map, false), MessageTypeDefOf.RejectInput, false);
				return false;
			}
			Pawn pawn = pawns.Find((Pawn ropee) => AnimalPenUtility.NeedsToBeManagedByRope(ropee) && !pawns.Any((Pawn roper) => roper.IsColonist && GatherAnimalsAndSlavesForCaravanUtility.CanRoperTakeAnimalToDest(roper, ropee, exitSpot) && GatherAnimalsAndSlavesForCaravanUtility.CanRoperTakeAnimalToDest(roper, ropee, meetingPoint)));
			if (pawn != null)
			{
				Messages.Message("CaravanRoamerCannotReachSpots".Translate(pawn.LabelShort, pawn), pawn, MessageTypeDefOf.CautionInput, false);
				return false;
			}
			CaravanFormingUtility.StartFormingCaravan((from x in pawns
			where !x.Downed
			select x).ToList<Pawn>(), (from x in pawns
			where x.Downed
			select x).ToList<Pawn>(), Faction.OfPlayer, this.transferables, meetingPoint, exitSpot, this.startingTile, this.destinationTile);
			Messages.Message("CaravanFormationProcessStarted".Translate(), pawns[0], MessageTypeDefOf.PositiveEvent, false);
			if (ModsConfig.BiotechActive)
			{
				if (pawns.Any((Pawn p) => p.RaceProps.IsMechanoid))
				{
					LessonAutoActivator.TeachOpportunity(ConceptDefOf.MechsInCaravans, OpportunityType.GoodToKnow);
				}
			}
			return true;
		}

		// Token: 0x06011CF3 RID: 72947 RVA: 0x005291F4 File Offset: 0x005273F4
		private bool TryReformCaravan()
		{
			List<Pawn> pawnsFromTransferables = TransferableUtility.GetPawnsFromTransferables(this.transferables);
			if (!this.CheckForErrors(pawnsFromTransferables))
			{
				return false;
			}
			this.AddItemsFromTransferablesToRandomInventories(pawnsFromTransferables);
			Caravan caravan = CaravanExitMapUtility.ExitMapAndCreateCaravan(pawnsFromTransferables, Faction.OfPlayer, this.CurrentTile, this.CurrentTile, this.destinationTile, false);
			this.map.Parent.CheckRemoveMapNow();
			TaggedString taggedString = "MessageReformedCaravan".Translate();
			if (caravan.pather.Moving && caravan.pather.ArrivalAction != null)
			{
				taggedString += " " + "MessageFormedCaravan_Orders".Translate() + ": " + caravan.pather.ArrivalAction.Label + ".";
			}
			Messages.Message(taggedString, caravan, MessageTypeDefOf.TaskCompletion, false);
			return true;
		}

		// Token: 0x06011CF4 RID: 72948 RVA: 0x005292D0 File Offset: 0x005274D0
		private void AddItemsFromTransferablesToRandomInventories(List<Pawn> pawns)
		{
			this.transferables.RemoveAll((TransferableOneWay x) => x.AnyThing is Pawn);
			if (this.ListPlayerPawnsInventorySeparately)
			{
				for (int i = 0; i < pawns.Count; i++)
				{
					if (Dialog_FormCaravan.CanListInventorySeparately(pawns[i]))
					{
						ThingOwner<Thing> innerContainer = pawns[i].inventory.innerContainer;
						for (int j = innerContainer.Count - 1; j >= 0; j--)
						{
							this.RemoveCarriedItemFromTransferablesOrDrop(innerContainer[j], pawns[i], this.transferables);
						}
					}
				}
				for (int k = 0; k < this.transferables.Count; k++)
				{
					if (this.transferables[k].things.Any((Thing x) => !x.Spawned))
					{
						this.transferables[k].things.SortBy((Thing x) => x.Spawned);
					}
				}
			}
			Action<Thing, IThingHolder> <>9__3;
			for (int l = 0; l < this.transferables.Count; l++)
			{
				if (!(this.transferables[l].AnyThing is Corpse))
				{
					List<Thing> things = this.transferables[l].things;
					int countToTransfer = this.transferables[l].CountToTransfer;
					Action<Thing, IThingHolder> transferred;
					if ((transferred = <>9__3) == null)
					{
						transferred = (<>9__3 = delegate(Thing splitPiece, IThingHolder originalHolder)
						{
							if (splitPiece.Faction != null && splitPiece.Faction != Faction.OfPlayer)
							{
								splitPiece.SetFactionDirect(Faction.OfPlayer);
							}
							Thing item = splitPiece.TryMakeMinified();
							CaravanInventoryUtility.FindPawnToMoveInventoryTo(item, pawns, null, null).inventory.TryAddAndUnforbid(item);
						});
					}
					TransferableUtility.Transfer(things, countToTransfer, transferred);
				}
			}
			Action<Thing, int> <>9__4;
			for (int m = 0; m < this.transferables.Count; m++)
			{
				if (this.transferables[m].AnyThing is Corpse)
				{
					List<Thing> things2 = this.transferables[m].things;
					int countToTransfer2 = this.transferables[m].CountToTransfer;
					Action<Thing, int> transfer;
					if ((transfer = <>9__4) == null)
					{
						transfer = (<>9__4 = delegate(Thing originalThing, int numToTake)
						{
							if (this.AutoStripSpawnedCorpses)
							{
								Corpse corpse = originalThing as Corpse;
								if (corpse != null && corpse.Spawned)
								{
									corpse.Strip(true);
								}
							}
							Thing item = originalThing.SplitOff(numToTake);
							CaravanInventoryUtility.FindPawnToMoveInventoryTo(item, pawns, null, null).inventory.TryAddAndUnforbid(item);
						});
					}
					TransferableUtility.TransferNoSplit(things2, countToTransfer2, transfer, true, true);
				}
			}
		}

		// Token: 0x06011CF5 RID: 72949 RVA: 0x00529520 File Offset: 0x00527720
		private bool CheckForErrors(List<Pawn> pawns)
		{
			if (this.MustChooseRoute && !this.destinationTile.Valid)
			{
				Messages.Message("MessageMustChooseRouteFirst".Translate(), MessageTypeDefOf.RejectInput, false);
				return false;
			}
			if (!this.reform && !this.startingTile.Valid)
			{
				Messages.Message("MessageNoValidExitTile".Translate(), MessageTypeDefOf.RejectInput, false);
				return false;
			}
			if (!pawns.Any((Pawn x) => CaravanUtility.IsOwner(x, Faction.OfPlayer) && !x.Downed))
			{
				if (ModsConfig.IdeologyActive)
				{
					Messages.Message("CaravanMustHaveAtLeastOneNonSlaveColonist".Translate(), MessageTypeDefOf.RejectInput, false);
				}
				else
				{
					Messages.Message("CaravanMustHaveAtLeastOneColonist".Translate(), MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			if (!this.reform && this.MassUsage > this.MassCapacity)
			{
				this.FlashMass();
				Messages.Message("TooBigCaravanMassUsage".Translate(), MessageTypeDefOf.RejectInput, false);
				return false;
			}
			for (int i = 0; i < this.transferables.Count; i++)
			{
				if (this.transferables[i].ThingDef.category == ThingCategory.Item)
				{
					int countToTransfer = this.transferables[i].CountToTransfer;
					int num = 0;
					if (countToTransfer > 0)
					{
						for (int j = 0; j < this.transferables[i].things.Count; j++)
						{
							Thing t = this.transferables[i].things[j];
							if (!t.Spawned || pawns.Any((Pawn x) => x.IsColonist && x.CanReach(t, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn)))
							{
								num += t.stackCount;
								if (num >= countToTransfer)
								{
									break;
								}
							}
						}
						if (num < countToTransfer)
						{
							if (countToTransfer == 1)
							{
								Messages.Message("CaravanItemIsUnreachableSingle".Translate(this.transferables[i].ThingDef.label), MessageTypeDefOf.RejectInput, false);
							}
							else
							{
								Messages.Message("CaravanItemIsUnreachableMulti".Translate(countToTransfer, this.transferables[i].ThingDef.label), MessageTypeDefOf.RejectInput, false);
							}
							return false;
						}
					}
				}
			}
			return true;
		}

		// Token: 0x06011CF6 RID: 72950 RVA: 0x0052977C File Offset: 0x0052797C
		private bool ShouldShowWarningForUndesirableFood()
		{
			Dialog_FormCaravan.tmpFood.Clear();
			Dialog_FormCaravan.tmpPawns.Clear();
			int num = 0;
			foreach (TransferableOneWay transferableOneWay in this.transferables)
			{
				if (transferableOneWay.HasAnyThing)
				{
					if (transferableOneWay.AnyThing is Pawn)
					{
						for (int i = 0; i < transferableOneWay.CountToTransfer; i++)
						{
							Pawn pawn = (Pawn)transferableOneWay.things[i];
							if (!InventoryCalculatorsUtility.ShouldIgnoreInventoryOf(pawn, this.IgnoreInventoryMode))
							{
								ThingOwner<Thing> innerContainer = pawn.inventory.innerContainer;
								int j = 0;
								int count = innerContainer.Count;
								while (j < count)
								{
									Thing thing = innerContainer[j];
									if (thing.def.IsNutritionGivingIngestible && ThingDefOf.Human.race.CanEverEat(thing))
									{
										Dialog_FormCaravan.tmpFood.Add(new ThingCount(thing, thing.stackCount, false));
										num += thing.stackCount;
									}
									j++;
								}
							}
							if (pawn.RaceProps.Humanlike)
							{
								Pawn_NeedsTracker needs = pawn.needs;
								if (((needs != null) ? needs.food : null) != null)
								{
									Dialog_FormCaravan.tmpPawns.Add(pawn);
								}
							}
						}
					}
					else if (transferableOneWay.ThingDef.IsNutritionGivingIngestible && ThingDefOf.Human.race.CanEverEat(transferableOneWay.AnyThing) && transferableOneWay.CountToTransfer > 0)
					{
						Dialog_FormCaravan.tmpFood.Add(new ThingCount(transferableOneWay.AnyThing, transferableOneWay.CountToTransfer, true));
						num += transferableOneWay.CountToTransfer;
					}
				}
			}
			int num2 = 0;
			foreach (ThingCount thingCount in Dialog_FormCaravan.tmpFood)
			{
				foreach (Pawn pawn2 in Dialog_FormCaravan.tmpPawns)
				{
					if (FoodUtility.WillGiveNegativeThoughts(thingCount.Thing, pawn2))
					{
						num2 += thingCount.Count;
						break;
					}
				}
			}
			return (double)((float)num2 / (float)num) >= 0.5;
		}

		// Token: 0x06011CF7 RID: 72951 RVA: 0x00529A08 File Offset: 0x00527C08
		private bool ShouldShowWarningForMechWithoutMechanitor()
		{
			if (!ModsConfig.BiotechActive)
			{
				return false;
			}
			Dialog_FormCaravan.tmpPawnsToTransfer.Clear();
			foreach (TransferableOneWay transferableOneWay in this.transferables)
			{
				if (transferableOneWay.HasAnyThing && transferableOneWay.AnyThing is Pawn)
				{
					for (int i = 0; i < transferableOneWay.CountToTransfer; i++)
					{
						Dialog_FormCaravan.tmpPawnsToTransfer.Add((Pawn)transferableOneWay.things[i]);
					}
				}
			}
			for (int j = 0; j < Dialog_FormCaravan.tmpPawnsToTransfer.Count; j++)
			{
				Pawn pawn = Dialog_FormCaravan.tmpPawnsToTransfer[j];
				if (pawn.IsColonyMech && MechanitorUtility.EverControllable(pawn))
				{
					Pawn overseer = pawn.GetOverseer();
					if (overseer != null && !Dialog_FormCaravan.tmpPawnsToTransfer.Contains(overseer))
					{
						Dialog_FormCaravan.tmpPawnsToTransfer.Clear();
						return true;
					}
				}
			}
			Dialog_FormCaravan.tmpPawnsToTransfer.Clear();
			return false;
		}

		// Token: 0x06011CF8 RID: 72952 RVA: 0x00529B10 File Offset: 0x00527D10
		private bool TryFindExitSpot(List<Pawn> pawns, bool reachableForEveryColonist, out IntVec3 spot)
		{
			Rot4 rot;
			Rot4 rot2;
			CaravanExitMapUtility.GetExitMapEdges(Find.WorldGrid, this.CurrentTile, this.startingTile, out rot, out rot2);
			return (rot != Rot4.Invalid && this.TryFindExitSpot(pawns, reachableForEveryColonist, rot, out spot)) || (rot2 != Rot4.Invalid && this.TryFindExitSpot(pawns, reachableForEveryColonist, rot2, out spot)) || this.TryFindExitSpot(pawns, reachableForEveryColonist, rot.Rotated(RotationDirection.Clockwise), out spot) || this.TryFindExitSpot(pawns, reachableForEveryColonist, rot.Rotated(RotationDirection.Counterclockwise), out spot);
		}

		// Token: 0x06011CF9 RID: 72953 RVA: 0x00529B90 File Offset: 0x00527D90
		private bool TryFindExitSpot(List<Pawn> pawns, bool reachableForEveryColonist, Rot4 exitDirection, out IntVec3 spot)
		{
			Dialog_FormCaravan.<>c__DisplayClass102_0 CS$<>8__locals1 = new Dialog_FormCaravan.<>c__DisplayClass102_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.pawns = pawns;
			if (!this.startingTile.Valid)
			{
				Log.Error("Can't find exit spot because startingTile is not set.");
				spot = IntVec3.Invalid;
				return false;
			}
			if (reachableForEveryColonist)
			{
				return CellFinder.TryFindRandomEdgeCellWith(delegate(IntVec3 x)
				{
					if (!base.<TryFindExitSpot>g__Validator|0(x))
					{
						return false;
					}
					for (int j = 0; j < CS$<>8__locals1.pawns.Count; j++)
					{
						if (CS$<>8__locals1.pawns[j].IsColonist && !CS$<>8__locals1.pawns[j].Downed && !CS$<>8__locals1.pawns[j].CanReach(x, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
						{
							return false;
						}
					}
					return true;
				}, this.map, exitDirection, CellFinder.EdgeRoadChance_Always, out spot);
			}
			IntVec3 intVec = IntVec3.Invalid;
			int num = -1;
			foreach (IntVec3 intVec2 in CellRect.WholeMap(this.map).GetEdgeCells(exitDirection).InRandomOrder(null))
			{
				if (CS$<>8__locals1.<TryFindExitSpot>g__Validator|0(intVec2))
				{
					int num2 = 0;
					for (int i = 0; i < CS$<>8__locals1.pawns.Count; i++)
					{
						if (CS$<>8__locals1.pawns[i].IsColonist && !CS$<>8__locals1.pawns[i].Downed && CS$<>8__locals1.pawns[i].CanReach(intVec2, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
						{
							num2++;
						}
					}
					if (num2 > num)
					{
						num = num2;
						intVec = intVec2;
					}
				}
			}
			spot = intVec;
			return intVec.IsValid;
		}

		// Token: 0x06011CFA RID: 72954 RVA: 0x00529CE8 File Offset: 0x00527EE8
		private bool TryFindRandomPackingSpot(IntVec3 exitSpot, out IntVec3 packingSpot)
		{
			Dialog_FormCaravan.tmpPackingSpots.Clear();
			List<Thing> list = this.map.listerThings.ThingsOfDef(ThingDefOf.CaravanPackingSpot);
			TraverseParms traverseParams = TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false, false, false, true, false);
			for (int i = 0; i < list.Count; i++)
			{
				if (this.map.reachability.CanReach(exitSpot, list[i], PathEndMode.OnCell, traverseParams))
				{
					Dialog_FormCaravan.tmpPackingSpots.Add(list[i]);
				}
			}
			if (Dialog_FormCaravan.tmpPackingSpots.Any<Thing>())
			{
				Thing thing = Dialog_FormCaravan.tmpPackingSpots.RandomElement<Thing>();
				Dialog_FormCaravan.tmpPackingSpots.Clear();
				packingSpot = thing.Position;
				return true;
			}
			return RCellFinder.TryFindRandomSpotJustOutsideColony(exitSpot, this.map, out packingSpot);
		}

		// Token: 0x06011CFB RID: 72955 RVA: 0x00529DA4 File Offset: 0x00527FA4
		private void AddPawnsToTransferables()
		{
			List<Pawn> list = Dialog_FormCaravan.AllSendablePawns(this.map, this.reform);
			for (int i = 0; i < list.Count; i++)
			{
				bool setToTransferMax = (this.reform || this.mapAboutToBeRemoved) && !CaravanUtility.ShouldAutoCapture(list[i], Faction.OfPlayer);
				this.AddToTransferables(list[i], setToTransferMax);
				Pawn_CarryTracker carryTracker = list[i].carryTracker;
				Thing thing = (carryTracker != null) ? carryTracker.CarriedThing : null;
				if (thing != null)
				{
					this.AddToTransferables(thing, setToTransferMax);
				}
			}
		}

		// Token: 0x06011CFC RID: 72956 RVA: 0x00529E30 File Offset: 0x00528030
		private void AddItemsToTransferables()
		{
			List<Thing> list = CaravanFormingUtility.AllReachableColonyItems(this.map, this.reform, this.reform, this.reform);
			for (int i = 0; i < list.Count; i++)
			{
				this.AddToTransferables(list[i], false);
			}
			if (this.AutoStripSpawnedCorpses)
			{
				for (int j = 0; j < list.Count; j++)
				{
					if (list[j].Spawned)
					{
						this.TryAddCorpseInventoryAndGearToTransferables(list[j]);
					}
				}
			}
			if (this.ListPlayerPawnsInventorySeparately)
			{
				List<Pawn> list2 = Dialog_FormCaravan.AllSendablePawns(this.map, this.reform);
				for (int k = 0; k < list2.Count; k++)
				{
					if (Dialog_FormCaravan.CanListInventorySeparately(list2[k]))
					{
						ThingOwner<Thing> innerContainer = list2[k].inventory.innerContainer;
						for (int l = 0; l < innerContainer.Count; l++)
						{
							this.AddToTransferables(innerContainer[l], true);
							if (this.AutoStripSpawnedCorpses && innerContainer[l].Spawned)
							{
								this.TryAddCorpseInventoryAndGearToTransferables(innerContainer[l]);
							}
						}
					}
				}
			}
		}

		// Token: 0x06011CFD RID: 72957 RVA: 0x00529F50 File Offset: 0x00528150
		private void TryAddCorpseInventoryAndGearToTransferables(Thing potentiallyCorpse)
		{
			Corpse corpse = potentiallyCorpse as Corpse;
			if (corpse != null)
			{
				this.AddCorpseInventoryAndGearToTransferables(corpse);
			}
		}

		// Token: 0x06011CFE RID: 72958 RVA: 0x00529F70 File Offset: 0x00528170
		private void AddCorpseInventoryAndGearToTransferables(Corpse corpse)
		{
			Pawn_InventoryTracker inventory = corpse.InnerPawn.inventory;
			Pawn_ApparelTracker apparel = corpse.InnerPawn.apparel;
			Pawn_EquipmentTracker equipment = corpse.InnerPawn.equipment;
			for (int i = 0; i < inventory.innerContainer.Count; i++)
			{
				if (Dialog_FormCaravan.<AddCorpseInventoryAndGearToTransferables>g__CanAdd|108_0(inventory.innerContainer[i]))
				{
					this.AddToTransferables(inventory.innerContainer[i], false);
				}
			}
			if (apparel != null)
			{
				List<Apparel> wornApparel = apparel.WornApparel;
				for (int j = 0; j < wornApparel.Count; j++)
				{
					if (Dialog_FormCaravan.<AddCorpseInventoryAndGearToTransferables>g__CanAdd|108_0(wornApparel[j]))
					{
						this.AddToTransferables(wornApparel[j], false);
					}
				}
			}
			if (equipment != null)
			{
				List<ThingWithComps> allEquipmentListForReading = equipment.AllEquipmentListForReading;
				for (int k = 0; k < allEquipmentListForReading.Count; k++)
				{
					if (Dialog_FormCaravan.<AddCorpseInventoryAndGearToTransferables>g__CanAdd|108_0(allEquipmentListForReading[k]))
					{
						this.AddToTransferables(allEquipmentListForReading[k], false);
					}
				}
			}
		}

		// Token: 0x06011CFF RID: 72959 RVA: 0x0052A060 File Offset: 0x00528260
		private void RemoveCarriedItemFromTransferablesOrDrop(Thing carried, Pawn carrier, List<TransferableOneWay> transferables)
		{
			TransferableOneWay transferableOneWay = TransferableUtility.TransferableMatchingDesperate(carried, transferables, TransferAsOneMode.PodsOrCaravanPacking);
			int num;
			if (transferableOneWay == null)
			{
				num = carried.stackCount;
			}
			else if (transferableOneWay.CountToTransfer >= carried.stackCount)
			{
				transferableOneWay.AdjustBy(-carried.stackCount);
				transferableOneWay.things.Remove(carried);
				num = 0;
			}
			else
			{
				num = carried.stackCount - transferableOneWay.CountToTransfer;
				transferableOneWay.AdjustTo(0);
			}
			if (num > 0)
			{
				Thing thing = carried.SplitOff(num);
				if (carrier.SpawnedOrAnyParentSpawned)
				{
					GenPlace.TryPlaceThing(thing, carrier.PositionHeld, carrier.MapHeld, ThingPlaceMode.Near, null, null, null, 1);
					return;
				}
				thing.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x06011D00 RID: 72960 RVA: 0x0052A0FF File Offset: 0x005282FF
		private void FlashMass()
		{
			this.lastMassFlashTime = Time.time;
		}

		// Token: 0x06011D01 RID: 72961 RVA: 0x0052A10C File Offset: 0x0052830C
		public static bool CanListInventorySeparately(Pawn p)
		{
			return p.Faction == Faction.OfPlayer || p.HostFaction == Faction.OfPlayer;
		}

		// Token: 0x06011D02 RID: 72962 RVA: 0x0052A12C File Offset: 0x0052832C
		private void SetToSendEverything()
		{
			for (int i = 0; i < this.transferables.Count; i++)
			{
				this.transferables[i].AdjustTo(this.transferables[i].GetMaximumToTransfer());
			}
			this.Notify_TransferablesChanged();
		}

		// Token: 0x06011D03 RID: 72963 RVA: 0x0052A178 File Offset: 0x00528378
		private void Notify_TransferablesChanged()
		{
			if (this.autoSelectTravelSupplies)
			{
				this.SelectApproximateBestTravelSupplies();
			}
			if (ModsConfig.BiotechActive)
			{
				foreach (TransferableOneWay transferableOneWay in this.transferables)
				{
					for (int i = 0; i < transferableOneWay.things.Count; i++)
					{
						if (transferableOneWay.IsThing)
						{
							Pawn pawn = transferableOneWay.things[i] as Pawn;
							if (pawn != null && pawn.IsColonyMech)
							{
								Pawn overseer = pawn.GetOverseer();
								if (overseer != null && pawn.GetMechControlGroup().WorkMode == MechWorkModeDefOf.Escort)
								{
									bool flag = false;
									foreach (TransferableOneWay transferableOneWay2 in this.transferables)
									{
										if (transferableOneWay2.IsThing && transferableOneWay2.things.Contains(overseer) && transferableOneWay2.CountToTransferToDestination > 0)
										{
											flag = true;
											break;
										}
									}
									if (flag && transferableOneWay.CountToTransferToDestination <= 0)
									{
										Messages.Message("MessageCaravanAddingEscortingMech".Translate(pawn.Named("MECH"), overseer.Named("OVERSEER")), pawn, MessageTypeDefOf.RejectInput, false);
										transferableOneWay.ForceToDestination(1);
									}
									if (!flag && transferableOneWay.CountToTransferToDestination > 0)
									{
										Messages.Message("MessageCaravanRemovingEscortingMech".Translate(pawn.Named("MECH"), overseer.Named("OVERSEER")), pawn, MessageTypeDefOf.RejectInput, false);
										transferableOneWay.ForceToDestination(0);
									}
								}
							}
						}
					}
				}
			}
			this.massUsageDirty = true;
			this.massCapacityDirty = true;
			this.tilesPerDayDirty = true;
			this.daysWorthOfFoodDirty = true;
			this.foragedFoodPerDayDirty = true;
			this.visibilityDirty = true;
			this.ticksToArriveDirty = true;
		}

		// Token: 0x06011D04 RID: 72964 RVA: 0x0052A398 File Offset: 0x00528598
		private void SelectApproximateBestTravelSupplies()
		{
			this.daysWorthOfFoodDirty = true;
			this.massUsageDirty = true;
			this.massCapacityDirty = true;
			List<TransferableOneWay> list = (from x in this.transferables
			where x.HasAnyThing && x.ThingDef.category != ThingCategory.Pawn && !x.ThingDef.thingCategories.NullOrEmpty<ThingCategoryDef>() && x.ThingDef.thingCategories.Contains(ThingCategoryDefOf.Medicine)
			select x).ToList<TransferableOneWay>();
			List<TransferableOneWay> list2 = (from x in this.transferables
			where x.HasAnyThing && x.ThingDef.IsIngestible && !x.ThingDef.IsDrug && !x.ThingDef.IsCorpse
			select x).ToList<TransferableOneWay>();
			IEnumerable<TransferableOneWay> enumerable = Enumerable.Empty<TransferableOneWay>();
			if (ModsConfig.BiotechActive)
			{
				enumerable = from x in this.transferables
				where x.ThingDef == ThingDefOf.HemogenPack
				select x;
			}
			Dialog_FormCaravan.tmpBeds.Clear();
			for (int i = 0; i < this.transferables.Count; i++)
			{
				for (int j = 0; j < this.transferables[i].things.Count; j++)
				{
					Thing thing = this.transferables[i].things[j];
					for (int k = 0; k < thing.stackCount; k++)
					{
						Building_Bed building_Bed = thing.GetInnerIfMinified() as Building_Bed;
						if (building_Bed != null && building_Bed.def.building.bed_caravansCanUse)
						{
							for (int l = 0; l < building_Bed.SleepingSlotsCount; l++)
							{
								Dialog_FormCaravan.tmpBeds.Add(this.transferables[i]);
							}
						}
					}
				}
			}
			Dialog_FormCaravan.tmpBeds.SortByDescending((TransferableOneWay x) => x.AnyThing.GetInnerIfMinified().GetStatValue(StatDefOf.BedRestEffectiveness, true, -1));
			foreach (TransferableOneWay transferableOneWay in list)
			{
				transferableOneWay.AdjustTo(0);
			}
			foreach (TransferableOneWay transferableOneWay2 in list2)
			{
				transferableOneWay2.AdjustTo(0);
			}
			foreach (TransferableOneWay transferableOneWay3 in Dialog_FormCaravan.tmpBeds)
			{
				transferableOneWay3.AdjustTo(0);
			}
			foreach (TransferableOneWay transferableOneWay4 in enumerable)
			{
				transferableOneWay4.AdjustTo(0);
			}
			List<Pawn> pawnsFromTransferables = TransferableUtility.GetPawnsFromTransferables(this.transferables);
			if (!pawnsFromTransferables.Any<Pawn>())
			{
				return;
			}
			pawnsFromTransferables.SortByDescending((Pawn x) => x.RaceProps.Humanlike);
			foreach (Pawn pawn in pawnsFromTransferables)
			{
				TransferableOneWay transferableOneWay5 = this.BestBedFor(pawn);
				if (transferableOneWay5 != null)
				{
					Dialog_FormCaravan.tmpBeds.Remove(transferableOneWay5);
					if (transferableOneWay5.CanAdjustBy(1).Accepted)
					{
						this.AddOneIfMassAllows(transferableOneWay5);
					}
				}
				if (!pawn.AnimalOrWildMan() && (pawn.guest == null || !pawn.guest.IsPrisoner) && (!ModsConfig.BiotechActive || !pawn.IsColonyMech))
				{
					for (int m = 0; m < 2; m++)
					{
						Transferable transferable = this.BestMedicineItemFor(pawn, list);
						if (transferable != null)
						{
							this.AddOneIfMassAllows(transferable);
						}
					}
				}
			}
			if (!this.destinationTile.Valid || !DaysWorthOfFoodCalculator.AnyFoodEatingPawn(pawnsFromTransferables) || !list2.Any<TransferableOneWay>())
			{
				return;
			}
			try
			{
				Dialog_FormCaravan.<>c__DisplayClass117_0 CS$<>8__locals1;
				CS$<>8__locals1.path = this.CurrentTile.Layer.Pather.FindPath(this.CurrentTile, this.destinationTile, null, null);
				try
				{
					CS$<>8__locals1.ticksPerMove = CaravanTicksPerMoveUtility.GetTicksPerMove(new CaravanTicksPerMoveUtility.CaravanInfo(this), null);
					CaravanArrivalTimeEstimator.EstimatedTicksToArriveToEvery(this.CurrentTile, this.destinationTile, CS$<>8__locals1.path, 0f, CS$<>8__locals1.ticksPerMove, Find.TickManager.TicksAbs, Dialog_FormCaravan.tmpTicksToArrive);
					float num = (float)Dialog_FormCaravan.tmpTicksToArrive.Last<ValueTuple<PlanetTile, int>>().Item2 / 60000f;
					float num2 = num + Dialog_FormCaravan.ExtraFoodDaysRange.ClampToRange(num * 0.35f);
					foreach (Pawn pawn2 in pawnsFromTransferables)
					{
						if (ModsConfig.BiotechActive && !pawn2.AnimalOrWildMan() && !pawn2.IsPrisoner && pawn2.genes != null && num >= 0.5f && enumerable.Any<TransferableOneWay>() && pawn2.genes.GetFirstGeneOfType<Gene_Hemogen>() != null)
						{
							int num3 = Mathf.CeilToInt(num / 2f);
							for (int n = 0; n < num3; n++)
							{
								Transferable hemogenPack = this.GetHemogenPack(enumerable);
								if (hemogenPack != null)
								{
									this.AddOneIfMassAllows(hemogenPack);
								}
							}
						}
						if (VirtualPlantsUtility.CanEverEatVirtualPlants(pawn2))
						{
							for (float num4 = 0f; num4 < num; num4 += 0.25f)
							{
								int ticksAbs = Find.TickManager.TicksGame + (int)(60000f * num4);
								if (VirtualPlantsUtility.EnvironmentAllowsEatingVirtualPlantsAt(CaravanArrivalTimeEstimator.TileIllBeInAt(ticksAbs, Dialog_FormCaravan.tmpTicksToArrive, Find.TickManager.TicksGame), ticksAbs))
								{
									Dialog_FormCaravan.tmpPawnNutritionDays.SetOrAdd(pawn2, 0.25f);
								}
							}
						}
					}
					float num5 = this.<SelectApproximateBestTravelSupplies>g__DaysOfFood|117_5(ref CS$<>8__locals1);
					Dialog_FormCaravan.cachedTicksToRot.Clear();
					while (num5 < num2 && this.MassUsage < this.MassCapacity)
					{
						bool flag = false;
						foreach (Pawn pawn3 in pawnsFromTransferables)
						{
							if (pawn3.RaceProps.EatsFood && pawn3.needs.food != null && pawn3.needs.food.MaxLevel > 0f && pawn3.needs.food.FoodFallPerTick != 0f && Dialog_FormCaravan.tmpPawnNutritionDays.TryGetValue(pawn3, 0f) < num2)
							{
								Transferable transferable2 = this.BestFoodItemFor(pawn3, list2, Dialog_FormCaravan.tmpTicksToArrive);
								if (transferable2 != null && this.AddFoodItem(pawn3, transferable2, num2))
								{
									flag = true;
								}
							}
						}
						if (!flag)
						{
							break;
						}
						num5 = this.<SelectApproximateBestTravelSupplies>g__DaysOfFood|117_5(ref CS$<>8__locals1);
					}
				}
				finally
				{
					if (CS$<>8__locals1.path != null)
					{
						((IDisposable)CS$<>8__locals1.path).Dispose();
					}
				}
			}
			finally
			{
				Dialog_FormCaravan.tmpTicksToArrive.Clear();
				Dialog_FormCaravan.tmpPawnNutritionDays.Clear();
				this.daysWorthOfFoodDirty = true;
				this.massUsageDirty = true;
				this.massCapacityDirty = true;
			}
		}

		// Token: 0x06011D05 RID: 72965 RVA: 0x0052AB00 File Offset: 0x00528D00
		private bool AddFoodItem(Pawn pawn, Transferable transferable, float tripDays)
		{
			int num = transferable.GetMaximumToTransfer() - transferable.CountToTransfer;
			if (num <= 0)
			{
				return false;
			}
			float num2 = Dialog_FormCaravan.tmpPawnNutritionDays.TryGetValue(pawn, 0f);
			float num3 = pawn.needs.food.FoodFallPerTickAssumingCategory(HungerCategory.Fed, true) * 60000f;
			float statValueAbstract = transferable.ThingDef.GetStatValueAbstract(StatDefOf.Nutrition, null);
			int num4 = Mathf.Min(num, Mathf.CeilToInt((tripDays - num2) * num3 / statValueAbstract));
			if (num4 <= 0 || !transferable.CanAdjustBy(num4).Accepted)
			{
				return false;
			}
			int destination = transferable.CountToTransfer + Mathf.Min(num4, Mathf.FloorToInt((this.MassCapacity - this.MassUsage) / transferable.ThingDef.BaseMass));
			transferable.AdjustTo(destination);
			Dialog_FormCaravan.tmpPawnNutritionDays.SetOrAdd(pawn, num2 + num3 / (statValueAbstract * (float)num4));
			this.daysWorthOfFoodDirty = true;
			this.massUsageDirty = true;
			return true;
		}

		// Token: 0x06011D06 RID: 72966 RVA: 0x0052ABE4 File Offset: 0x00528DE4
		private bool AddOneIfMassAllows(Transferable transferable)
		{
			if (transferable.CanAdjustBy(1).Accepted && this.MassUsage + transferable.ThingDef.BaseMass < this.MassCapacity)
			{
				transferable.AdjustBy(1);
				this.massUsageDirty = true;
				return true;
			}
			return false;
		}

		// Token: 0x06011D07 RID: 72967 RVA: 0x0052AC30 File Offset: 0x00528E30
		private TransferableOneWay BestBedFor(Pawn pawn)
		{
			if (ModsConfig.BiotechActive && pawn.IsColonyMech)
			{
				return null;
			}
			Pawn_NeedsTracker needs = pawn.needs;
			if (((needs != null) ? needs.rest : null) == null)
			{
				return null;
			}
			for (int i = 0; i < Dialog_FormCaravan.tmpBeds.Count; i++)
			{
				Thing innerIfMinified = Dialog_FormCaravan.tmpBeds[i].AnyThing.GetInnerIfMinified();
				if (RestUtility.CanUseBedEver(pawn, innerIfMinified.def))
				{
					return Dialog_FormCaravan.tmpBeds[i];
				}
			}
			return null;
		}

		// Token: 0x06011D08 RID: 72968 RVA: 0x0052ACAC File Offset: 0x00528EAC
		private Transferable BestFoodItemFor(Pawn pawn, List<TransferableOneWay> food, [TupleElementNames(new string[]
		{
			"tile",
			"ticks"
		})] List<ValueTuple<PlanetTile, int>> ticksToArrive)
		{
			Transferable result = null;
			float num = 0f;
			for (int i = 0; i < food.Count; i++)
			{
				TransferableOneWay transferableOneWay = food[i];
				if (transferableOneWay.CanAdjustBy(1).Accepted)
				{
					float foodScore = this.GetFoodScore(pawn, transferableOneWay.AnyThing, ticksToArrive);
					if (foodScore > num)
					{
						result = transferableOneWay;
						num = foodScore;
					}
				}
			}
			return result;
		}

		// Token: 0x06011D09 RID: 72969 RVA: 0x0052AD08 File Offset: 0x00528F08
		private float GetFoodScore(Pawn pawn, Thing food, [TupleElementNames(new string[]
		{
			"tile",
			"ticks"
		})] List<ValueTuple<PlanetTile, int>> ticksToArrive)
		{
			if (!CaravanPawnsNeedsUtility.CanEatForNutritionEver(food.def, pawn))
			{
				return 0f;
			}
			float num = CaravanPawnsNeedsUtility.GetFoodScore(food.def, pawn, food.GetStatValue(StatDefOf.Nutrition, true, 1));
			if (FoodUtility.WillGiveNegativeThoughts(food, pawn))
			{
				num *= 0.4f;
			}
			if (ModsConfig.BiotechActive && food.def == ThingDefOf.BabyFood && !pawn.DevelopmentalStage.Baby())
			{
				num *= 0.1f;
			}
			int num2;
			if (!Dialog_FormCaravan.cachedTicksToRot.TryGetValue(food, out num2))
			{
				if (food.TryGetComp<CompRottable>() != null)
				{
					num2 = DaysUntilRotCalculator.ApproxTicksUntilRot_AssumeTimePassesBy(food.TryGetComp<CompRottable>(), this.CurrentTile, ticksToArrive);
				}
				else
				{
					num2 = -1;
				}
				Dialog_FormCaravan.cachedTicksToRot[food] = num2;
			}
			if (num2 >= 0 && num2 < ticksToArrive.Last<ValueTuple<PlanetTile, int>>().Item2)
			{
				num *= 0.1f;
			}
			return num;
		}

		// Token: 0x06011D0A RID: 72970 RVA: 0x0052ADD4 File Offset: 0x00528FD4
		private Transferable BestMedicineItemFor(Pawn pawn, IEnumerable<TransferableOneWay> medicine)
		{
			Transferable transferable = null;
			float num = 0f;
			foreach (TransferableOneWay transferableOneWay in medicine)
			{
				Thing anyThing = transferableOneWay.AnyThing;
				if (transferableOneWay.CanAdjustBy(1).Accepted && pawn.playerSettings.medCare.AllowsMedicine(anyThing.def))
				{
					float statValue = anyThing.GetStatValue(StatDefOf.MedicalPotency, true, -1);
					if (transferable == null || statValue > num)
					{
						transferable = transferableOneWay;
						num = statValue;
					}
				}
			}
			return transferable;
		}

		// Token: 0x06011D0B RID: 72971 RVA: 0x0052AE70 File Offset: 0x00529070
		private Transferable GetHemogenPack(IEnumerable<TransferableOneWay> hemogen)
		{
			foreach (TransferableOneWay transferableOneWay in hemogen)
			{
				if (transferableOneWay.CanAdjustBy(1).Accepted)
				{
					return transferableOneWay;
				}
			}
			return null;
		}

		// Token: 0x06011D0C RID: 72972 RVA: 0x0052AECC File Offset: 0x005290CC
		public static List<Pawn> AllSendablePawns(Map map, bool reform)
		{
			return CaravanFormingUtility.AllSendablePawns(map, true, reform, reform, reform, false, -1);
		}

		// Token: 0x06011D0D RID: 72973 RVA: 0x0052AEDA File Offset: 0x005290DA
		public override void OnAcceptKeyPressed()
		{
			this.TrySend();
		}

		// Token: 0x06011D14 RID: 72980 RVA: 0x0052AFAE File Offset: 0x005291AE
		[CompilerGenerated]
		internal static bool <AddCorpseInventoryAndGearToTransferables>g__CanAdd|108_0(Thing thing)
		{
			return !thing.def.destroyOnDrop && thing.GetInnerIfMinified().def.canLoadIntoCaravan;
		}

		// Token: 0x06011D15 RID: 72981 RVA: 0x0052AFCF File Offset: 0x005291CF
		[CompilerGenerated]
		private float <SelectApproximateBestTravelSupplies>g__DaysOfFood|117_5(ref Dialog_FormCaravan.<>c__DisplayClass117_0 A_1)
		{
			return DaysWorthOfFoodCalculator.ApproxDaysWorthOfFood(this.transferables, this.CurrentTile, this.IgnoreInventoryMode, Faction.OfPlayer, A_1.path, 0f, A_1.ticksPerMove);
		}

		// Token: 0x0400B7FB RID: 47099
		private readonly Map map;

		// Token: 0x0400B7FC RID: 47100
		private readonly bool reform;

		// Token: 0x0400B7FD RID: 47101
		private readonly Action onClosed;

		// Token: 0x0400B7FE RID: 47102
		private readonly bool canChooseRoute;

		// Token: 0x0400B7FF RID: 47103
		private readonly bool mapAboutToBeRemoved;

		// Token: 0x0400B800 RID: 47104
		public bool choosingRoute;

		// Token: 0x0400B801 RID: 47105
		private bool thisWindowInstanceEverOpened;

		// Token: 0x0400B802 RID: 47106
		public List<TransferableOneWay> transferables;

		// Token: 0x0400B803 RID: 47107
		private TransferableOneWayWidget pawnsTransfer;

		// Token: 0x0400B804 RID: 47108
		private TransferableOneWayWidget itemsTransfer;

		// Token: 0x0400B805 RID: 47109
		private TransferableOneWayWidget travelSuppliesTransfer;

		// Token: 0x0400B806 RID: 47110
		private Dialog_FormCaravan.Tab tab;

		// Token: 0x0400B807 RID: 47111
		private float lastMassFlashTime = -9999f;

		// Token: 0x0400B808 RID: 47112
		private PlanetTile startingTile = PlanetTile.Invalid;

		// Token: 0x0400B809 RID: 47113
		private PlanetTile destinationTile = PlanetTile.Invalid;

		// Token: 0x0400B80A RID: 47114
		private readonly IntVec3 designatedMeetingPoint = IntVec3.Invalid;

		// Token: 0x0400B80B RID: 47115
		private bool massUsageDirty = true;

		// Token: 0x0400B80C RID: 47116
		private float cachedMassUsage;

		// Token: 0x0400B80D RID: 47117
		private bool massCapacityDirty = true;

		// Token: 0x0400B80E RID: 47118
		private float cachedMassCapacity;

		// Token: 0x0400B80F RID: 47119
		private string cachedMassCapacityExplanation;

		// Token: 0x0400B810 RID: 47120
		private bool tilesPerDayDirty = true;

		// Token: 0x0400B811 RID: 47121
		private float cachedTilesPerDay;

		// Token: 0x0400B812 RID: 47122
		private string cachedTilesPerDayExplanation;

		// Token: 0x0400B813 RID: 47123
		private bool daysWorthOfFoodDirty = true;

		// Token: 0x0400B814 RID: 47124
		[TupleElementNames(new string[]
		{
			"days",
			"tillRot"
		})]
		private ValueTuple<float, float> cachedDaysWorthOfFood;

		// Token: 0x0400B815 RID: 47125
		private bool foragedFoodPerDayDirty = true;

		// Token: 0x0400B816 RID: 47126
		[TupleElementNames(new string[]
		{
			"food",
			"perDay"
		})]
		private ValueTuple<ThingDef, float> cachedForagedFoodPerDay;

		// Token: 0x0400B817 RID: 47127
		private string cachedForagedFoodPerDayExplanation;

		// Token: 0x0400B818 RID: 47128
		private bool visibilityDirty = true;

		// Token: 0x0400B819 RID: 47129
		private float cachedVisibility;

		// Token: 0x0400B81A RID: 47130
		private string cachedVisibilityExplanation;

		// Token: 0x0400B81B RID: 47131
		private bool ticksToArriveDirty = true;

		// Token: 0x0400B81C RID: 47132
		private int cachedTicksToArrive;

		// Token: 0x0400B81D RID: 47133
		private bool autoSelectTravelSupplies;

		// Token: 0x0400B81E RID: 47134
		private const int AutoMedicinePerColonist = 2;

		// Token: 0x0400B81F RID: 47135
		private const float TitleRectHeight = 35f;

		// Token: 0x0400B820 RID: 47136
		private const float BottomAreaHeight = 55f;

		// Token: 0x0400B821 RID: 47137
		private const float AutoSelectCheckBoxHeight = 35f;

		// Token: 0x0400B822 RID: 47138
		private const float AdjustedTravelTimeFactor = 0.35f;

		// Token: 0x0400B823 RID: 47139
		private const float MaxDaysWorthOfFoodToShowWarningDialog = 5f;

		// Token: 0x0400B824 RID: 47140
		private static readonly Vector2 BottomButtonSize = new Vector2(160f, 40f);

		// Token: 0x0400B825 RID: 47141
		private static FloatRange ExtraFoodDaysRange = new FloatRange(1f, 5f);

		// Token: 0x0400B826 RID: 47142
		private static readonly List<TabRecord> tabsList = new List<TabRecord>();

		// Token: 0x0400B827 RID: 47143
		private static readonly List<ThingCount> tmpFood = new List<ThingCount>();

		// Token: 0x0400B828 RID: 47144
		private static readonly List<Pawn> tmpPawns = new List<Pawn>();

		// Token: 0x0400B829 RID: 47145
		private static readonly List<Pawn> tmpPawnsToTransfer = new List<Pawn>();

		// Token: 0x0400B82A RID: 47146
		private static readonly List<Thing> tmpPackingSpots = new List<Thing>();

		// Token: 0x0400B82B RID: 47147
		[TupleElementNames(new string[]
		{
			"tile",
			"ticks"
		})]
		private static readonly List<ValueTuple<PlanetTile, int>> tmpTicksToArrive = new List<ValueTuple<PlanetTile, int>>();

		// Token: 0x0400B82C RID: 47148
		private static readonly Dictionary<Pawn, float> tmpPawnNutritionDays = new Dictionary<Pawn, float>();

		// Token: 0x0400B82D RID: 47149
		private static readonly List<TransferableOneWay> tmpBeds = new List<TransferableOneWay>();

		// Token: 0x0400B82E RID: 47150
		private static Dictionary<Thing, int> cachedTicksToRot = new Dictionary<Thing, int>();

		// Token: 0x02003178 RID: 12664
		private enum Tab
		{
			// Token: 0x0400B830 RID: 47152
			Pawns,
			// Token: 0x0400B831 RID: 47153
			Items,
			// Token: 0x0400B832 RID: 47154
			TravelSupplies
		}
	}
}
