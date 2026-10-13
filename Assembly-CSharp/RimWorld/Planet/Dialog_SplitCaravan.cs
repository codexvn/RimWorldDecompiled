using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld.Planet
{
	// Token: 0x02003E4E RID: 15950
	public class Dialog_SplitCaravan : Window
	{
		// Token: 0x1700367D RID: 13949
		// (get) Token: 0x06015D08 RID: 89352 RVA: 0x00526A3B File Offset: 0x00524C3B
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(1024f, (float)UI.screenHeight);
			}
		}

		// Token: 0x1700367E RID: 13950
		// (get) Token: 0x06015D09 RID: 89353 RVA: 0x00006AAC File Offset: 0x00004CAC
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x1700367F RID: 13951
		// (get) Token: 0x06015D0A RID: 89354 RVA: 0x00684D8E File Offset: 0x00682F8E
		private BiomeDef Biome
		{
			get
			{
				return this.caravan.Biome;
			}
		}

		// Token: 0x17003680 RID: 13952
		// (get) Token: 0x06015D0B RID: 89355 RVA: 0x00684D9B File Offset: 0x00682F9B
		private float SourceMassUsage
		{
			get
			{
				if (this.sourceMassUsageDirty)
				{
					this.sourceMassUsageDirty = false;
					this.cachedSourceMassUsage = CollectionsMassCalculator.MassUsageLeftAfterTransfer(this.transferables, IgnorePawnsInventoryMode.Ignore, false, false);
				}
				return this.cachedSourceMassUsage;
			}
		}

		// Token: 0x17003681 RID: 13953
		// (get) Token: 0x06015D0C RID: 89356 RVA: 0x00684DC8 File Offset: 0x00682FC8
		private float SourceMassCapacity
		{
			get
			{
				if (this.sourceMassCapacityDirty)
				{
					this.sourceMassCapacityDirty = false;
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedSourceMassCapacity = CollectionsMassCalculator.CapacityLeftAfterTransfer(this.transferables, stringBuilder);
					this.cachedSourceMassCapacityExplanation = stringBuilder.ToString();
				}
				return this.cachedSourceMassCapacity;
			}
		}

		// Token: 0x17003682 RID: 13954
		// (get) Token: 0x06015D0D RID: 89357 RVA: 0x00684E10 File Offset: 0x00683010
		private float SourceTilesPerDay
		{
			get
			{
				if (this.sourceTilesPerDayDirty)
				{
					this.sourceTilesPerDayDirty = false;
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedSourceTilesPerDay = TilesPerDayCalculator.ApproxTilesPerDayLeftAfterTransfer(this.transferables, this.SourceMassUsage, this.SourceMassCapacity, this.caravan.Tile, this.caravan.pather.Moving ? this.caravan.pather.nextTile : PlanetTile.Invalid, this.caravan.Shuttle != null, stringBuilder);
					this.cachedSourceTilesPerDayExplanation = stringBuilder.ToString();
				}
				return this.cachedSourceTilesPerDay;
			}
		}

		// Token: 0x17003683 RID: 13955
		// (get) Token: 0x06015D0E RID: 89358 RVA: 0x00684EA4 File Offset: 0x006830A4
		[TupleElementNames(new string[]
		{
			"days",
			"tillRot"
		})]
		private ValueTuple<float, float> SourceDaysWorthOfFood
		{
			[return: TupleElementNames(new string[]
			{
				"days",
				"tillRot"
			})]
			get
			{
				if (this.sourceDaysWorthOfFoodDirty)
				{
					this.sourceDaysWorthOfFoodDirty = false;
					float item;
					float item2;
					if (this.caravan.pather.Moving)
					{
						using (this.caravan.Tile.Layer.Pather.FindPath(this.caravan.Tile, this.caravan.pather.Destination, null, null))
						{
							item = DaysWorthOfFoodCalculator.ApproxDaysWorthOfFoodLeftAfterTransfer(this.transferables, this.caravan.Tile, IgnorePawnsInventoryMode.Ignore, this.caravan.Faction, this.caravan.pather.curPath, this.caravan.pather.nextTileCostLeft, this.caravan.TicksPerMove);
							item2 = DaysUntilRotCalculator.ApproxDaysUntilRotLeftAfterTransfer(this.transferables, this.caravan.Tile, IgnorePawnsInventoryMode.Ignore, this.caravan.pather.curPath, this.caravan.pather.nextTileCostLeft, this.caravan.TicksPerMove);
							goto IL_150;
						}
					}
					item = DaysWorthOfFoodCalculator.ApproxDaysWorthOfFoodLeftAfterTransfer(this.transferables, this.caravan.Tile, IgnorePawnsInventoryMode.Ignore, this.caravan.Faction, null, 0f, 3300);
					item2 = DaysUntilRotCalculator.ApproxDaysUntilRotLeftAfterTransfer(this.transferables, this.caravan.Tile, IgnorePawnsInventoryMode.Ignore, null, 0f, 3300);
					IL_150:
					this.cachedSourceDaysWorthOfFood = new ValueTuple<float, float>(item, item2);
				}
				return this.cachedSourceDaysWorthOfFood;
			}
		}

		// Token: 0x17003684 RID: 13956
		// (get) Token: 0x06015D0F RID: 89359 RVA: 0x00685024 File Offset: 0x00683224
		[TupleElementNames(new string[]
		{
			"food",
			"perDay"
		})]
		private ValueTuple<ThingDef, float> SourceForagedFoodPerDay
		{
			[return: TupleElementNames(new string[]
			{
				"food",
				"perDay"
			})]
			get
			{
				if (this.sourceForagedFoodPerDayDirty)
				{
					this.sourceForagedFoodPerDayDirty = false;
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedSourceForagedFoodPerDay = ForagedFoodPerDayCalculator.ForagedFoodPerDayLeftAfterTransfer(this.transferables, this.Biome, Faction.OfPlayer, stringBuilder);
					this.cachedSourceForagedFoodPerDayExplanation = stringBuilder.ToString();
				}
				return this.cachedSourceForagedFoodPerDay;
			}
		}

		// Token: 0x17003685 RID: 13957
		// (get) Token: 0x06015D10 RID: 89360 RVA: 0x00685078 File Offset: 0x00683278
		private float SourceVisibility
		{
			get
			{
				if (this.sourceVisibilityDirty)
				{
					this.sourceVisibilityDirty = false;
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedSourceVisibility = CaravanVisibilityCalculator.VisibilityLeftAfterTransfer(this.transferables, stringBuilder);
					this.cachedSourceVisibilityExplanation = stringBuilder.ToString();
				}
				return this.cachedSourceVisibility;
			}
		}

		// Token: 0x17003686 RID: 13958
		// (get) Token: 0x06015D11 RID: 89361 RVA: 0x006850BE File Offset: 0x006832BE
		private float DestMassUsage
		{
			get
			{
				if (this.destMassUsageDirty)
				{
					this.destMassUsageDirty = false;
					this.cachedDestMassUsage = CollectionsMassCalculator.MassUsageTransferables(this.transferables, IgnorePawnsInventoryMode.Ignore, false, false);
				}
				return this.cachedDestMassUsage;
			}
		}

		// Token: 0x17003687 RID: 13959
		// (get) Token: 0x06015D12 RID: 89362 RVA: 0x006850EC File Offset: 0x006832EC
		private float DestMassCapacity
		{
			get
			{
				if (this.destMassCapacityDirty)
				{
					this.destMassCapacityDirty = false;
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedDestMassCapacity = CollectionsMassCalculator.CapacityTransferables(this.transferables, stringBuilder);
					this.cachedDestMassCapacityExplanation = stringBuilder.ToString();
				}
				return this.cachedDestMassCapacity;
			}
		}

		// Token: 0x17003688 RID: 13960
		// (get) Token: 0x06015D13 RID: 89363 RVA: 0x00685134 File Offset: 0x00683334
		private float DestTilesPerDay
		{
			get
			{
				if (this.destTilesPerDayDirty)
				{
					this.destTilesPerDayDirty = false;
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedDestTilesPerDay = TilesPerDayCalculator.ApproxTilesPerDay(this.transferables, this.DestMassUsage, this.DestMassCapacity, this.caravan.Tile, this.caravan.pather.Moving ? this.caravan.pather.nextTile : PlanetTile.Invalid, this.caravan.Shuttle != null, stringBuilder);
					this.cachedDestTilesPerDayExplanation = stringBuilder.ToString();
				}
				return this.cachedDestTilesPerDay;
			}
		}

		// Token: 0x17003689 RID: 13961
		// (get) Token: 0x06015D14 RID: 89364 RVA: 0x006851C8 File Offset: 0x006833C8
		[TupleElementNames(new string[]
		{
			"days",
			"tillRot"
		})]
		private ValueTuple<float, float> DestDaysWorthOfFood
		{
			[return: TupleElementNames(new string[]
			{
				"days",
				"tillRot"
			})]
			get
			{
				if (this.destDaysWorthOfFoodDirty)
				{
					this.destDaysWorthOfFoodDirty = false;
					float item;
					float item2;
					if (this.caravan.pather.Moving)
					{
						item = DaysWorthOfFoodCalculator.ApproxDaysWorthOfFood(this.transferables, this.caravan.Tile, IgnorePawnsInventoryMode.Ignore, this.caravan.Faction, this.caravan.pather.curPath, this.caravan.pather.nextTileCostLeft, this.caravan.TicksPerMove);
						item2 = DaysUntilRotCalculator.ApproxDaysUntilRot(this.transferables, this.caravan.Tile, IgnorePawnsInventoryMode.Ignore, this.caravan.pather.curPath, this.caravan.pather.nextTileCostLeft, this.caravan.TicksPerMove);
					}
					else
					{
						item = DaysWorthOfFoodCalculator.ApproxDaysWorthOfFood(this.transferables, this.caravan.Tile, IgnorePawnsInventoryMode.Ignore, this.caravan.Faction, null, 0f, 3300);
						item2 = DaysUntilRotCalculator.ApproxDaysUntilRot(this.transferables, this.caravan.Tile, IgnorePawnsInventoryMode.Ignore, null, 0f, 3300);
					}
					this.cachedDestDaysWorthOfFood = new ValueTuple<float, float>(item, item2);
				}
				return this.cachedDestDaysWorthOfFood;
			}
		}

		// Token: 0x1700368A RID: 13962
		// (get) Token: 0x06015D15 RID: 89365 RVA: 0x006852F4 File Offset: 0x006834F4
		[TupleElementNames(new string[]
		{
			"food",
			"perDay"
		})]
		private ValueTuple<ThingDef, float> DestForagedFoodPerDay
		{
			[return: TupleElementNames(new string[]
			{
				"food",
				"perDay"
			})]
			get
			{
				if (this.destForagedFoodPerDayDirty)
				{
					this.destForagedFoodPerDayDirty = false;
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedDestForagedFoodPerDay = ForagedFoodPerDayCalculator.ForagedFoodPerDay(this.transferables, this.Biome, Faction.OfPlayer, stringBuilder);
					this.cachedDestForagedFoodPerDayExplanation = stringBuilder.ToString();
				}
				return this.cachedDestForagedFoodPerDay;
			}
		}

		// Token: 0x1700368B RID: 13963
		// (get) Token: 0x06015D16 RID: 89366 RVA: 0x00685348 File Offset: 0x00683548
		private float DestVisibility
		{
			get
			{
				if (this.destVisibilityDirty)
				{
					this.destVisibilityDirty = false;
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedDestVisibility = CaravanVisibilityCalculator.Visibility(this.transferables, stringBuilder);
					this.cachedDestVisibilityExplanation = stringBuilder.ToString();
				}
				return this.cachedDestVisibility;
			}
		}

		// Token: 0x1700368C RID: 13964
		// (get) Token: 0x06015D17 RID: 89367 RVA: 0x0068538E File Offset: 0x0068358E
		private int TicksToArrive
		{
			get
			{
				if (!this.caravan.pather.Moving)
				{
					return 0;
				}
				if (this.ticksToArriveDirty)
				{
					this.ticksToArriveDirty = false;
					this.cachedTicksToArrive = CaravanArrivalTimeEstimator.EstimatedTicksToArrive(this.caravan, false);
				}
				return this.cachedTicksToArrive;
			}
		}

		// Token: 0x06015D18 RID: 89368 RVA: 0x006853CC File Offset: 0x006835CC
		public Dialog_SplitCaravan(Caravan caravan) : base(null)
		{
			this.caravan = caravan;
			this.forcePause = true;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x06015D19 RID: 89369 RVA: 0x00685465 File Offset: 0x00683665
		public override void PostOpen()
		{
			base.PostOpen();
			this.CalculateAndRecacheTransferables();
		}

		// Token: 0x06015D1A RID: 89370 RVA: 0x00685474 File Offset: 0x00683674
		public override void DoWindowContents(Rect inRect)
		{
			Rect rect = new Rect(0f, 0f, inRect.width, 35f);
			Text.Font = GameFont.Medium;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect, "SplitCaravan".Translate());
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.UpperLeft;
			CaravanUIUtility.DrawCaravanInfo(new CaravanUIUtility.CaravanInfo(this.SourceMassUsage, this.SourceMassCapacity, this.cachedSourceMassCapacityExplanation, this.SourceTilesPerDay, this.cachedSourceTilesPerDayExplanation, this.SourceDaysWorthOfFood, this.SourceForagedFoodPerDay, this.cachedSourceForagedFoodPerDayExplanation, this.SourceVisibility, this.cachedSourceVisibilityExplanation, -1f, -1f, null), new CaravanUIUtility.CaravanInfo?(new CaravanUIUtility.CaravanInfo(this.DestMassUsage, this.DestMassCapacity, this.cachedDestMassCapacityExplanation, this.DestTilesPerDay, this.cachedDestTilesPerDayExplanation, this.DestDaysWorthOfFood, this.DestForagedFoodPerDay, this.cachedDestForagedFoodPerDayExplanation, this.DestVisibility, this.cachedDestVisibilityExplanation, -1f, -1f, null)), this.caravan.Tile, this.caravan.pather.Moving ? new int?(this.TicksToArrive) : null, -9999f, new Rect(12f, 35f, inRect.width - 24f, 40f), true, null, false, true);
			Dialog_SplitCaravan.tabsList.Clear();
			Dialog_SplitCaravan.tabsList.Add(new TabRecord("PawnsTab".Translate(), delegate()
			{
				this.tab = Dialog_SplitCaravan.Tab.Pawns;
			}, this.tab == Dialog_SplitCaravan.Tab.Pawns));
			Dialog_SplitCaravan.tabsList.Add(new TabRecord("ItemsTab".Translate(), delegate()
			{
				this.tab = Dialog_SplitCaravan.Tab.Items;
			}, this.tab == Dialog_SplitCaravan.Tab.Items));
			Dialog_SplitCaravan.tabsList.Add(new TabRecord("TravelSupplies".Translate(), delegate()
			{
				this.tab = Dialog_SplitCaravan.Tab.FoodAndMedicine;
			}, this.tab == Dialog_SplitCaravan.Tab.FoodAndMedicine));
			inRect.yMin += 119f;
			Widgets.DrawMenuSection(inRect);
			TabDrawer.DrawTabs<TabRecord>(inRect, Dialog_SplitCaravan.tabsList, 200f);
			inRect = inRect.ContractedBy(17f);
			Widgets.BeginGroup(inRect);
			Rect rect2 = inRect.AtZero();
			this.DoBottomButtons(rect2);
			Rect inRect2 = rect2;
			inRect2.yMax -= 59f;
			bool flag = false;
			switch (this.tab)
			{
			case Dialog_SplitCaravan.Tab.Pawns:
				this.pawnsTransfer.OnGUI(inRect2, out flag);
				break;
			case Dialog_SplitCaravan.Tab.Items:
				this.itemsTransfer.OnGUI(inRect2, out flag);
				break;
			case Dialog_SplitCaravan.Tab.FoodAndMedicine:
				this.foodAndMedicineTransfer.OnGUI(inRect2, out flag);
				break;
			}
			if (flag)
			{
				this.CountToTransferChanged();
			}
			Widgets.EndGroup();
		}

		// Token: 0x06015D1B RID: 89371 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool CausesMessageBackground()
		{
			return true;
		}

		// Token: 0x06015D1C RID: 89372 RVA: 0x00685720 File Offset: 0x00683920
		private void AddToTransferables(Thing t)
		{
			TransferableOneWay transferableOneWay = TransferableUtility.TransferableMatching<TransferableOneWay>(t, this.transferables, TransferAsOneMode.Normal);
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
		}

		// Token: 0x06015D1D RID: 89373 RVA: 0x00685788 File Offset: 0x00683988
		private void DoBottomButtons(Rect rect)
		{
			Rect rect2 = new Rect(rect.width / 2f - this.BottomButtonSize.x / 2f, rect.height - 55f, this.BottomButtonSize.x, this.BottomButtonSize.y);
			if (Widgets.ButtonText(rect2, "AcceptButton".Translate(), true, true, true, null) && this.TrySplitCaravan())
			{
				SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				this.Close(false);
			}
			if (Widgets.ButtonText(new Rect(rect2.x - 10f - this.BottomButtonSize.x, rect2.y, this.BottomButtonSize.x, this.BottomButtonSize.y), "ResetButton".Translate(), true, true, true, null))
			{
				SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
				this.CalculateAndRecacheTransferables();
			}
			if (Widgets.ButtonText(new Rect(rect2.xMax + 10f, rect2.y, this.BottomButtonSize.x, this.BottomButtonSize.y), "CancelButton".Translate(), true, true, true, null))
			{
				this.Close(true);
			}
		}

		// Token: 0x06015D1E RID: 89374 RVA: 0x006858E4 File Offset: 0x00683AE4
		private void CalculateAndRecacheTransferables()
		{
			this.transferables = new List<TransferableOneWay>();
			this.AddPawnsToTransferables();
			this.AddItemsToTransferables();
			CaravanUIUtility.CreateCaravanTransferableWidgets(this.transferables, out this.pawnsTransfer, out this.itemsTransfer, out this.foodAndMedicineTransfer, "SplitCaravanThingCountTip".Translate(), IgnorePawnsInventoryMode.Ignore, () => this.DestMassCapacity - this.DestMassUsage, false, this.caravan.Tile, false);
			this.CountToTransferChanged();
		}

		// Token: 0x06015D1F RID: 89375 RVA: 0x00685954 File Offset: 0x00683B54
		private bool TrySplitCaravan()
		{
			List<Pawn> pawnsFromTransferables = TransferableUtility.GetPawnsFromTransferables(this.transferables);
			if (!this.CheckForErrors(pawnsFromTransferables))
			{
				return false;
			}
			for (int i = 0; i < pawnsFromTransferables.Count; i++)
			{
				CaravanInventoryUtility.MoveAllInventoryToSomeoneElse(pawnsFromTransferables[i], this.caravan.PawnsListForReading, pawnsFromTransferables);
			}
			for (int j = 0; j < pawnsFromTransferables.Count; j++)
			{
				this.caravan.RemovePawn(pawnsFromTransferables[j]);
			}
			Caravan newCaravan = CaravanMaker.MakeCaravan(pawnsFromTransferables, this.caravan.Faction, this.caravan.Tile, true);
			this.transferables.RemoveAll((TransferableOneWay x) => x.AnyThing is Pawn);
			Action<Thing, int> <>9__1;
			for (int k = 0; k < this.transferables.Count; k++)
			{
				List<Thing> things = this.transferables[k].things;
				int countToTransfer = this.transferables[k].CountToTransfer;
				Action<Thing, int> transfer;
				if ((transfer = <>9__1) == null)
				{
					transfer = (<>9__1 = delegate(Thing thing, int numToTake)
					{
						Pawn ownerOf = CaravanInventoryUtility.GetOwnerOf(this.caravan, thing);
						if (ownerOf == null)
						{
							Log.Error("Error while splitting a caravan: Thing " + ((thing != null) ? thing.ToString() : null) + " has no owner. Where did it come from then?");
							return;
						}
						CaravanInventoryUtility.MoveInventoryToSomeoneElse(ownerOf, thing, newCaravan.PawnsListForReading, null, numToTake);
					});
				}
				TransferableUtility.TransferNoSplit(things, countToTransfer, transfer, true, true);
			}
			return true;
		}

		// Token: 0x06015D20 RID: 89376 RVA: 0x00685A88 File Offset: 0x00683C88
		private bool CheckForErrors(List<Pawn> pawns)
		{
			if (!pawns.Any((Pawn x) => CaravanUtility.IsOwner(x, Faction.OfPlayer) && !x.Downed))
			{
				if (ModsConfig.IdeologyActive)
				{
					Messages.Message("CaravanMustHaveAtLeastOneNonSlaveColonist".Translate(), MessageTypeDefOf.RejectInput, false);
				}
				else
				{
					Messages.Message("CaravanMustHaveAtLeastOneColonist".Translate(), this.caravan, MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			if (!this.AnyNonDownedColonistLeftInSourceCaravan(pawns))
			{
				if (ModsConfig.IdeologyActive)
				{
					Messages.Message("SourceCaravanMustHaveAtLeastOneNonSlaveColonist".Translate(), this.caravan, MessageTypeDefOf.RejectInput, false);
				}
				else
				{
					Messages.Message("SourceCaravanMustHaveAtLeastOneColonist".Translate(), this.caravan, MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			return true;
		}

		// Token: 0x06015D21 RID: 89377 RVA: 0x00685B68 File Offset: 0x00683D68
		private void AddPawnsToTransferables()
		{
			List<Pawn> pawnsListForReading = this.caravan.PawnsListForReading;
			for (int i = 0; i < pawnsListForReading.Count; i++)
			{
				this.AddToTransferables(pawnsListForReading[i]);
			}
		}

		// Token: 0x06015D22 RID: 89378 RVA: 0x00685BA0 File Offset: 0x00683DA0
		private void AddItemsToTransferables()
		{
			List<Thing> list = CaravanInventoryUtility.AllInventoryItems(this.caravan);
			for (int i = 0; i < list.Count; i++)
			{
				this.AddToTransferables(list[i]);
			}
		}

		// Token: 0x06015D23 RID: 89379 RVA: 0x00685BD8 File Offset: 0x00683DD8
		private bool AnyNonDownedColonistLeftInSourceCaravan(List<Pawn> pawnsToTransfer)
		{
			Predicate<Thing> <>9__1;
			return this.transferables.Any(delegate(TransferableOneWay x)
			{
				List<Thing> things = x.things;
				Predicate<Thing> predicate;
				if ((predicate = <>9__1) == null)
				{
					predicate = (<>9__1 = delegate(Thing y)
					{
						Pawn pawn = y as Pawn;
						return pawn != null && CaravanUtility.IsOwner(pawn, Faction.OfPlayer) && !pawn.Downed && !pawnsToTransfer.Contains(pawn);
					});
				}
				return things.Any(predicate);
			});
		}

		// Token: 0x06015D24 RID: 89380 RVA: 0x00685C0C File Offset: 0x00683E0C
		private void CountToTransferChanged()
		{
			this.sourceMassUsageDirty = true;
			this.sourceMassCapacityDirty = true;
			this.sourceTilesPerDayDirty = true;
			this.sourceDaysWorthOfFoodDirty = true;
			this.sourceForagedFoodPerDayDirty = true;
			this.sourceVisibilityDirty = true;
			this.destMassUsageDirty = true;
			this.destMassCapacityDirty = true;
			this.destTilesPerDayDirty = true;
			this.destDaysWorthOfFoodDirty = true;
			this.destForagedFoodPerDayDirty = true;
			this.destVisibilityDirty = true;
			this.ticksToArriveDirty = true;
		}

		// Token: 0x0400F1E5 RID: 61925
		private Caravan caravan;

		// Token: 0x0400F1E6 RID: 61926
		private List<TransferableOneWay> transferables;

		// Token: 0x0400F1E7 RID: 61927
		private TransferableOneWayWidget pawnsTransfer;

		// Token: 0x0400F1E8 RID: 61928
		private TransferableOneWayWidget itemsTransfer;

		// Token: 0x0400F1E9 RID: 61929
		private TransferableOneWayWidget foodAndMedicineTransfer;

		// Token: 0x0400F1EA RID: 61930
		private Dialog_SplitCaravan.Tab tab;

		// Token: 0x0400F1EB RID: 61931
		private bool sourceMassUsageDirty = true;

		// Token: 0x0400F1EC RID: 61932
		private float cachedSourceMassUsage;

		// Token: 0x0400F1ED RID: 61933
		private bool sourceMassCapacityDirty = true;

		// Token: 0x0400F1EE RID: 61934
		private float cachedSourceMassCapacity;

		// Token: 0x0400F1EF RID: 61935
		private string cachedSourceMassCapacityExplanation;

		// Token: 0x0400F1F0 RID: 61936
		private bool sourceTilesPerDayDirty = true;

		// Token: 0x0400F1F1 RID: 61937
		private float cachedSourceTilesPerDay;

		// Token: 0x0400F1F2 RID: 61938
		private string cachedSourceTilesPerDayExplanation;

		// Token: 0x0400F1F3 RID: 61939
		private bool sourceDaysWorthOfFoodDirty = true;

		// Token: 0x0400F1F4 RID: 61940
		[TupleElementNames(new string[]
		{
			"days",
			"tillRot"
		})]
		private ValueTuple<float, float> cachedSourceDaysWorthOfFood;

		// Token: 0x0400F1F5 RID: 61941
		private bool sourceForagedFoodPerDayDirty = true;

		// Token: 0x0400F1F6 RID: 61942
		[TupleElementNames(new string[]
		{
			"food",
			"perDay"
		})]
		private ValueTuple<ThingDef, float> cachedSourceForagedFoodPerDay;

		// Token: 0x0400F1F7 RID: 61943
		private string cachedSourceForagedFoodPerDayExplanation;

		// Token: 0x0400F1F8 RID: 61944
		private bool sourceVisibilityDirty = true;

		// Token: 0x0400F1F9 RID: 61945
		private float cachedSourceVisibility;

		// Token: 0x0400F1FA RID: 61946
		private string cachedSourceVisibilityExplanation;

		// Token: 0x0400F1FB RID: 61947
		private bool destMassUsageDirty = true;

		// Token: 0x0400F1FC RID: 61948
		private float cachedDestMassUsage;

		// Token: 0x0400F1FD RID: 61949
		private bool destMassCapacityDirty = true;

		// Token: 0x0400F1FE RID: 61950
		private float cachedDestMassCapacity;

		// Token: 0x0400F1FF RID: 61951
		private string cachedDestMassCapacityExplanation;

		// Token: 0x0400F200 RID: 61952
		private bool destTilesPerDayDirty = true;

		// Token: 0x0400F201 RID: 61953
		private float cachedDestTilesPerDay;

		// Token: 0x0400F202 RID: 61954
		private string cachedDestTilesPerDayExplanation;

		// Token: 0x0400F203 RID: 61955
		private bool destDaysWorthOfFoodDirty = true;

		// Token: 0x0400F204 RID: 61956
		[TupleElementNames(new string[]
		{
			"days",
			"tillRot"
		})]
		private ValueTuple<float, float> cachedDestDaysWorthOfFood;

		// Token: 0x0400F205 RID: 61957
		private bool destForagedFoodPerDayDirty = true;

		// Token: 0x0400F206 RID: 61958
		[TupleElementNames(new string[]
		{
			"food",
			"perDay"
		})]
		private ValueTuple<ThingDef, float> cachedDestForagedFoodPerDay;

		// Token: 0x0400F207 RID: 61959
		private string cachedDestForagedFoodPerDayExplanation;

		// Token: 0x0400F208 RID: 61960
		private bool destVisibilityDirty = true;

		// Token: 0x0400F209 RID: 61961
		private float cachedDestVisibility;

		// Token: 0x0400F20A RID: 61962
		private string cachedDestVisibilityExplanation;

		// Token: 0x0400F20B RID: 61963
		private bool ticksToArriveDirty = true;

		// Token: 0x0400F20C RID: 61964
		private int cachedTicksToArrive;

		// Token: 0x0400F20D RID: 61965
		private const float TitleRectHeight = 35f;

		// Token: 0x0400F20E RID: 61966
		private const float BottomAreaHeight = 55f;

		// Token: 0x0400F20F RID: 61967
		private readonly Vector2 BottomButtonSize = new Vector2(160f, 40f);

		// Token: 0x0400F210 RID: 61968
		private static List<TabRecord> tabsList = new List<TabRecord>();

		// Token: 0x02003E4F RID: 15951
		private enum Tab
		{
			// Token: 0x0400F212 RID: 61970
			Pawns,
			// Token: 0x0400F213 RID: 61971
			Items,
			// Token: 0x0400F214 RID: 61972
			FoodAndMedicine
		}
	}
}
