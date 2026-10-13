using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020034B4 RID: 13492
	[StaticConstructorOnStartup]
	public class Dialog_Trade : Window
	{
		// Token: 0x17003164 RID: 12644
		// (get) Token: 0x06012F68 RID: 77672 RVA: 0x00526A3B File Offset: 0x00524C3B
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(1024f, (float)UI.screenHeight);
			}
		}

		// Token: 0x17003165 RID: 12645
		// (get) Token: 0x06012F69 RID: 77673 RVA: 0x0059FCFF File Offset: 0x0059DEFF
		private PlanetTile Tile
		{
			get
			{
				return TradeSession.playerNegotiator.Tile;
			}
		}

		// Token: 0x17003166 RID: 12646
		// (get) Token: 0x06012F6A RID: 77674 RVA: 0x0059FD0B File Offset: 0x0059DF0B
		private BiomeDef Biome
		{
			get
			{
				return Find.WorldGrid[this.Tile].PrimaryBiome;
			}
		}

		// Token: 0x17003167 RID: 12647
		// (get) Token: 0x06012F6B RID: 77675 RVA: 0x0059FD24 File Offset: 0x0059DF24
		private float MassUsage
		{
			get
			{
				if (this.massUsageDirty)
				{
					this.massUsageDirty = false;
					TradeSession.deal.UpdateCurrencyCount();
					if (this.cachedCurrencyTradeable != null)
					{
						this.cachedTradeables.Add(this.cachedCurrencyTradeable);
					}
					Building_PassengerShuttle shuttle = TradeSession.playerNegotiator.GetCaravan().Shuttle;
					if (shuttle != null)
					{
						this.cachedMassUsage = CollectionsMassCalculator.MassUsageLeftAfterTradeableTransfer(this.playerCaravanAllPawnsAndItems, this.cachedTradeables, IgnorePawnsInventoryMode.Ignore, true, false);
						this.cachedMassUsage -= shuttle.GetStatValue(StatDefOf.Mass, true, -1);
					}
					else
					{
						this.cachedMassUsage = CollectionsMassCalculator.MassUsageLeftAfterTradeableTransfer(this.playerCaravanAllPawnsAndItems, this.cachedTradeables, IgnorePawnsInventoryMode.Ignore, false, false);
					}
					if (this.cachedCurrencyTradeable != null)
					{
						this.cachedTradeables.RemoveLast<Tradeable>();
					}
				}
				return this.cachedMassUsage;
			}
		}

		// Token: 0x17003168 RID: 12648
		// (get) Token: 0x06012F6C RID: 77676 RVA: 0x0059FDE4 File Offset: 0x0059DFE4
		private float MassCapacity
		{
			get
			{
				if (this.massCapacityDirty)
				{
					this.massCapacityDirty = false;
					Building_PassengerShuttle shuttle = TradeSession.playerNegotiator.GetCaravan().Shuttle;
					if (shuttle != null)
					{
						this.cachedMassCapacity = shuttle.TransporterComp.MassCapacity;
						return this.cachedMassCapacity;
					}
					TradeSession.deal.UpdateCurrencyCount();
					if (this.cachedCurrencyTradeable != null)
					{
						this.cachedTradeables.Add(this.cachedCurrencyTradeable);
					}
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedMassCapacity = CollectionsMassCalculator.CapacityLeftAfterTradeableTransfer(this.playerCaravanAllPawnsAndItems, this.cachedTradeables, stringBuilder);
					this.cachedMassCapacityExplanation = stringBuilder.ToString();
					if (this.cachedCurrencyTradeable != null)
					{
						this.cachedTradeables.RemoveLast<Tradeable>();
					}
				}
				return this.cachedMassCapacity;
			}
		}

		// Token: 0x17003169 RID: 12649
		// (get) Token: 0x06012F6D RID: 77677 RVA: 0x0059FE94 File Offset: 0x0059E094
		private float TilesPerDay
		{
			get
			{
				if (this.tilesPerDayDirty)
				{
					this.tilesPerDayDirty = false;
					TradeSession.deal.UpdateCurrencyCount();
					Caravan caravan = TradeSession.playerNegotiator.GetCaravan();
					if (caravan.Shuttle != null)
					{
						this.cachedTilesPerDayExplanation = "CaravanMovementSpeedShuttle".Translate();
						return 0f;
					}
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedTilesPerDay = TilesPerDayCalculator.ApproxTilesPerDayLeftAfterTradeableTransfer(this.playerCaravanAllPawnsAndItems, this.cachedTradeables, this.MassUsage, this.MassCapacity, this.Tile, caravan.pather.Moving ? caravan.pather.nextTile : PlanetTile.Invalid, TradeSession.playerNegotiator.GetCaravan().Shuttle != null, stringBuilder);
					this.cachedTilesPerDayExplanation = stringBuilder.ToString();
				}
				return this.cachedTilesPerDay;
			}
		}

		// Token: 0x1700316A RID: 12650
		// (get) Token: 0x06012F6E RID: 77678 RVA: 0x0059FF60 File Offset: 0x0059E160
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
					TradeSession.deal.UpdateCurrencyCount();
					float item = DaysWorthOfFoodCalculator.ApproxDaysWorthOfFoodLeftAfterTradeableTransfer(this.playerCaravanAllPawnsAndItems, this.cachedTradeables, this.Tile, IgnorePawnsInventoryMode.Ignore, Faction.OfPlayer);
					this.cachedDaysWorthOfFood = new ValueTuple<float, float>(item, DaysUntilRotCalculator.ApproxDaysUntilRotLeftAfterTradeableTransfer(this.playerCaravanAllPawnsAndItems, this.cachedTradeables, this.Tile, IgnorePawnsInventoryMode.Ignore));
				}
				return this.cachedDaysWorthOfFood;
			}
		}

		// Token: 0x1700316B RID: 12651
		// (get) Token: 0x06012F6F RID: 77679 RVA: 0x0059FFD0 File Offset: 0x0059E1D0
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
					TradeSession.deal.UpdateCurrencyCount();
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedForagedFoodPerDay = ForagedFoodPerDayCalculator.ForagedFoodPerDayLeftAfterTradeableTransfer(this.playerCaravanAllPawnsAndItems, this.cachedTradeables, this.Biome, Faction.OfPlayer, stringBuilder);
					this.cachedForagedFoodPerDayExplanation = stringBuilder.ToString();
				}
				return this.cachedForagedFoodPerDay;
			}
		}

		// Token: 0x1700316C RID: 12652
		// (get) Token: 0x06012F70 RID: 77680 RVA: 0x005A0034 File Offset: 0x0059E234
		private float Visibility
		{
			get
			{
				if (this.visibilityDirty)
				{
					this.visibilityDirty = false;
					TradeSession.deal.UpdateCurrencyCount();
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedVisibility = CaravanVisibilityCalculator.VisibilityLeftAfterTradeableTransfer(this.playerCaravanAllPawnsAndItems, this.cachedTradeables, stringBuilder);
					this.cachedVisibilityExplanation = stringBuilder.ToString();
				}
				return this.cachedVisibility;
			}
		}

		// Token: 0x1700316D RID: 12653
		// (get) Token: 0x06012F71 RID: 77681 RVA: 0x005A008A File Offset: 0x0059E28A
		public override QuickSearchWidget CommonSearchWidget
		{
			get
			{
				return this.quickSearchWidget;
			}
		}

		// Token: 0x06012F72 RID: 77682 RVA: 0x005A0094 File Offset: 0x0059E294
		public Dialog_Trade(Pawn playerNegotiator, ITrader trader, bool giftsOnly = false) : base(null)
		{
			this.giftsOnly = giftsOnly;
			TradeSession.SetupWith(trader, playerNegotiator, giftsOnly);
			this.SetupPlayerCaravanVariables();
			this.forcePause = true;
			this.absorbInputAroundWindow = true;
			this.soundAppear = SoundDefOf.CommsWindow_Open;
			this.soundClose = SoundDefOf.CommsWindow_Close;
			if (trader is PassingShip)
			{
				this.soundAmbient = SoundDefOf.RadioComms_Ambience;
			}
			this.commonSearchWidgetOffset.x = this.commonSearchWidgetOffset.x + 18f;
			this.commonSearchWidgetOffset.y = this.commonSearchWidgetOffset.y - 18f;
			this.sorter1 = TransferableSorterDefOf.Category;
			this.sorter2 = TransferableSorterDefOf.MarketValue;
		}

		// Token: 0x06012F73 RID: 77683 RVA: 0x005A0172 File Offset: 0x0059E372
		public override void PreOpen()
		{
			base.PreOpen();
			this.quickSearchWidget.Reset();
		}

		// Token: 0x06012F74 RID: 77684 RVA: 0x005A0188 File Offset: 0x0059E388
		public override void PostOpen()
		{
			base.PostOpen();
			if (!this.giftsOnly && !this.playerIsCaravan)
			{
				Pawn playerNegotiator = TradeSession.playerNegotiator;
				float level = playerNegotiator.health.capacities.GetLevel(PawnCapacityDefOf.Talking);
				float level2 = playerNegotiator.health.capacities.GetLevel(PawnCapacityDefOf.Hearing);
				if (level < 0.95f || level2 < 0.95f)
				{
					TaggedString taggedString;
					if (level < 0.95f)
					{
						taggedString = "NegotiatorTalkingImpaired".Translate(playerNegotiator.LabelShort, playerNegotiator);
					}
					else
					{
						taggedString = "NegotiatorHearingImpaired".Translate(playerNegotiator.LabelShort, playerNegotiator);
					}
					taggedString += "\n\n" + "NegotiatorCapacityImpaired".Translate();
					Find.WindowStack.Add(new Dialog_MessageBox(taggedString, null, null, null, null, null, false, null, null, WindowLayer.Dialog));
				}
			}
			this.CacheTradeables();
		}

		// Token: 0x06012F75 RID: 77685 RVA: 0x005A0270 File Offset: 0x0059E470
		private void CacheTradeables()
		{
			this.cachedCurrencyTradeable = TradeSession.deal.AllTradeables.FirstOrDefault((Tradeable x) => x.IsCurrency && (TradeSession.TradeCurrency != TradeCurrency.Favor || x.IsFavor));
			this.cachedTradeables = (from tr in TradeSession.deal.AllTradeables
			where !tr.IsCurrency && (tr.TraderWillTrade || !TradeSession.trader.TraderKind.hideThingsNotWillingToTrade)
			where this.quickSearchWidget.filter.Matches(tr.Label)
			select tr).OrderByDescending(delegate(Tradeable tr)
			{
				if (!tr.TraderWillTrade)
				{
					return -1;
				}
				return 0;
			}).ThenBy((Tradeable tr) => tr, this.sorter1.Comparer).ThenBy((Tradeable tr) => tr, this.sorter2.Comparer).ThenBy((Tradeable tr) => TransferableUIUtility.DefaultListOrderPriority(tr)).ThenBy((Tradeable tr) => tr.ThingDef.label).ThenBy(delegate(Tradeable tr)
			{
				QualityCategory result;
				if (tr.AnyThing.TryGetQuality(out result))
				{
					return (int)result;
				}
				return -1;
			}).ThenBy((Tradeable tr) => tr.AnyThing.HitPoints).ToList<Tradeable>();
			this.quickSearchWidget.noResultsMatched = !this.cachedTradeables.Any<Tradeable>();
		}

		// Token: 0x06012F76 RID: 77686 RVA: 0x005A0428 File Offset: 0x0059E628
		public override void DoWindowContents(Rect inRect)
		{
			if (this.playerIsCaravan)
			{
				CaravanUIUtility.DrawCaravanInfo(new CaravanUIUtility.CaravanInfo(this.MassUsage, this.MassCapacity, this.cachedMassCapacityExplanation, this.TilesPerDay, this.cachedTilesPerDayExplanation, this.DaysWorthOfFood, this.ForagedFoodPerDay, this.cachedForagedFoodPerDayExplanation, this.Visibility, this.cachedVisibilityExplanation, -1f, -1f, null), null, this.Tile, null, -9999f, new Rect(12f, 0f, inRect.width - 24f, 40f), true, null, false, true);
				inRect.yMin += 52f;
			}
			TradeSession.deal.UpdateCurrencyCount();
			Widgets.BeginGroup(inRect);
			inRect = inRect.AtZero();
			TransferableUIUtility.DoTransferableSorters(this.sorter1, this.sorter2, delegate(TransferableSorterDef x)
			{
				this.sorter1 = x;
				this.CacheTradeables();
			}, delegate(TransferableSorterDef x)
			{
				this.sorter2 = x;
				this.CacheTradeables();
			});
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.UpperLeft;
			Widgets.Label(new Rect(0f, 27f, inRect.width / 2f, inRect.height / 2f), "NegotiatorTradeDialogInfo".Translate(TradeSession.playerNegotiator.Name.ToStringFull, TradeSession.playerNegotiator.GetStatValue(StatDefOf.TradePriceImprovement, true, -1).ToStringPercent()));
			float num = inRect.width - 590f;
			Rect rect = new Rect(num, 0f, inRect.width - num, 58f);
			Widgets.BeginGroup(rect);
			Text.Font = GameFont.Medium;
			Rect rect2 = new Rect(0f, 0f, rect.width / 2f, rect.height);
			Text.Anchor = TextAnchor.UpperLeft;
			Widgets.Label(rect2, Faction.OfPlayer.Name.Truncate(rect2.width, null));
			Rect rect3 = new Rect(rect.width / 2f, 0f, rect.width / 2f, rect.height);
			Text.Anchor = TextAnchor.UpperRight;
			string text = TradeSession.trader.TraderName;
			if (Text.CalcSize(text).x > rect3.width)
			{
				Text.Font = GameFont.Small;
				text = text.Truncate(rect3.width, null);
			}
			Widgets.Label(rect3, text);
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.UpperRight;
			Widgets.Label(new Rect(rect.width / 2f, 27f, rect.width / 2f, rect.height / 2f), TradeSession.trader.TraderKind.LabelCap);
			Text.Anchor = TextAnchor.UpperLeft;
			if (!TradeSession.giftMode)
			{
				GUI.color = new Color(1f, 1f, 1f, 0.6f);
				Text.Font = GameFont.Tiny;
				Rect rect4 = new Rect(rect.width / 2f - 100f - 30f, 0f, 200f, rect.height);
				Text.Anchor = TextAnchor.LowerCenter;
				Widgets.Label(rect4, "PositiveBuysNegativeSells".Translate());
				Text.Anchor = TextAnchor.UpperLeft;
				GUI.color = Color.white;
			}
			Widgets.EndGroup();
			float num2 = 0f;
			if (this.cachedCurrencyTradeable != null)
			{
				float num3 = inRect.width - 16f;
				Rect rect5 = new Rect(0f, 58f, num3, 30f);
				int countToTransfer = this.cachedCurrencyTradeable.CountToTransfer;
				TradeUI.DrawTradeableRow(rect5, this.cachedCurrencyTradeable, 1);
				if (countToTransfer != this.cachedCurrencyTradeable.CountToTransfer)
				{
					this.CountToTransferChanged();
				}
				GUI.color = Color.gray;
				Widgets.DrawLineHorizontal(0f, 87f, num3);
				GUI.color = Color.white;
				num2 = 30f;
			}
			Rect mainRect = new Rect(0f, 58f + num2, inRect.width, inRect.height - 58f - 38f - num2 - 20f);
			this.FillMainRect(mainRect);
			Text.Font = GameFont.Small;
			Rect rect6 = new Rect(inRect.width / 2f - Dialog_Trade.AcceptButtonSize.x / 2f, inRect.height - 55f, Dialog_Trade.AcceptButtonSize.x, Dialog_Trade.AcceptButtonSize.y);
			if (Widgets.ButtonText(rect6, TradeSession.giftMode ? ("OfferGifts".Translate() + " (" + FactionGiftUtility.GetGoodwillChange(TradeSession.deal.AllTradeables, TradeSession.trader.Faction).ToStringWithSign() + ")") : "AcceptButton".Translate(), true, true, true, null))
			{
				Action action = delegate()
				{
					bool flag;
					if (TradeSession.deal.TryExecute(out flag))
					{
						if (flag)
						{
							SoundDefOf.ExecuteTrade.PlayOneShotOnCamera(null);
							Caravan caravan = TradeSession.playerNegotiator.GetCaravan();
							if (caravan != null)
							{
								caravan.RecacheInventory();
							}
							this.Close(false);
							return;
						}
						this.Close(true);
					}
				};
				if (TradeSession.deal.DoesTraderHaveEnoughSilver())
				{
					action();
				}
				else
				{
					this.FlashSilver();
					SoundDefOf.ClickReject.PlayOneShotOnCamera(null);
					Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmTraderShortFunds".Translate(), action, false, null, WindowLayer.Dialog));
				}
				Event.current.Use();
			}
			if (Widgets.ButtonText(new Rect(rect6.x - 10f - Dialog_Trade.OtherBottomButtonSize.x, rect6.y, Dialog_Trade.OtherBottomButtonSize.x, Dialog_Trade.OtherBottomButtonSize.y), "ResetButton".Translate(), true, true, true, null))
			{
				SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
				TradeSession.deal.Reset();
				this.CacheTradeables();
				this.CountToTransferChanged();
			}
			if (Widgets.ButtonText(new Rect(rect6.xMax + 10f, rect6.y, Dialog_Trade.OtherBottomButtonSize.x, Dialog_Trade.OtherBottomButtonSize.y), "CancelButton".Translate(), true, true, true, null))
			{
				this.Close(true);
				Event.current.Use();
			}
			float y = Dialog_Trade.OtherBottomButtonSize.y;
			Rect rect7 = new Rect(inRect.width - y, rect6.y, y, y);
			if (Widgets.ButtonImageWithBG(rect7, Dialog_Trade.ShowSellableItemsIcon, new Vector2?(new Vector2(32f, 32f))))
			{
				Find.WindowStack.Add(new Dialog_SellableItems(TradeSession.trader));
			}
			TooltipHandler.TipRegionByKey(rect7, "CommandShowSellableItemsDesc");
			Faction faction = TradeSession.trader.Faction;
			if (faction != null && !this.giftsOnly && !faction.def.permanentEnemy && TradeSession.trader.TradeCurrency != TradeCurrency.Favor)
			{
				Rect rect8 = new Rect(rect7.x - y - 4f, rect6.y, y, y);
				if (TradeSession.giftMode)
				{
					if (Widgets.ButtonImageWithBG(rect8, Dialog_Trade.TradeModeIcon, new Vector2?(new Vector2(32f, 32f))))
					{
						TradeSession.giftMode = false;
						TradeSession.deal.Reset();
						this.CacheTradeables();
						this.CountToTransferChanged();
						SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
					}
					TooltipHandler.TipRegionByKey(rect8, "TradeModeTip");
				}
				else
				{
					if (Widgets.ButtonImageWithBG(rect8, Dialog_Trade.GiftModeIcon, new Vector2?(new Vector2(32f, 32f))))
					{
						TradeSession.giftMode = true;
						TradeSession.deal.Reset();
						this.CacheTradeables();
						this.CountToTransferChanged();
						SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
					}
					TooltipHandler.TipRegionByKey(rect8, "GiftModeTip", faction.Name);
				}
			}
			Widgets.EndGroup();
		}

		// Token: 0x06012F77 RID: 77687 RVA: 0x005A0BB8 File Offset: 0x0059EDB8
		public override void Close(bool doCloseSound = true)
		{
			DragSliderManager.ForceStop();
			base.Close(doCloseSound);
			Pawn pawn = TradeSession.trader as Pawn;
			if (pawn != null && pawn.mindState.hasQuest)
			{
				TradeUtility.ReceiveQuestFromTrader(pawn, TradeSession.playerNegotiator);
			}
		}

		// Token: 0x06012F78 RID: 77688 RVA: 0x005A0BF8 File Offset: 0x0059EDF8
		private void FillMainRect(Rect mainRect)
		{
			Text.Font = GameFont.Small;
			float height = 6f + (float)this.cachedTradeables.Count * 30f;
			Rect viewRect = new Rect(0f, 0f, mainRect.width - 16f, height);
			Widgets.BeginScrollView(mainRect, ref this.scrollPosition, viewRect, true);
			float num = 6f;
			float num2 = this.scrollPosition.y - 30f;
			float num3 = this.scrollPosition.y + mainRect.height;
			int num4 = 0;
			for (int i = 0; i < this.cachedTradeables.Count; i++)
			{
				if (num > num2 && num < num3)
				{
					Rect rect = new Rect(0f, num, viewRect.width, 30f);
					int countToTransfer = this.cachedTradeables[i].CountToTransfer;
					TradeUI.DrawTradeableRow(rect, this.cachedTradeables[i], num4);
					if (countToTransfer != this.cachedTradeables[i].CountToTransfer)
					{
						this.CountToTransferChanged();
					}
				}
				num += 30f;
				num4++;
			}
			Widgets.EndScrollView();
		}

		// Token: 0x06012F79 RID: 77689 RVA: 0x005A0D15 File Offset: 0x0059EF15
		public void FlashSilver()
		{
			Dialog_Trade.lastCurrencyFlashTime = Time.time;
		}

		// Token: 0x06012F7A RID: 77690 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool CausesMessageBackground()
		{
			return true;
		}

		// Token: 0x06012F7B RID: 77691 RVA: 0x005A0D24 File Offset: 0x0059EF24
		private void SetupPlayerCaravanVariables()
		{
			Caravan caravan = TradeSession.playerNegotiator.GetCaravan();
			if (caravan != null)
			{
				this.playerIsCaravan = true;
				this.playerCaravanAllPawnsAndItems = new List<Thing>();
				List<Pawn> pawnsListForReading = caravan.PawnsListForReading;
				for (int i = 0; i < pawnsListForReading.Count; i++)
				{
					this.playerCaravanAllPawnsAndItems.Add(pawnsListForReading[i]);
				}
				this.playerCaravanAllPawnsAndItems.AddRange(CaravanInventoryUtility.AllInventoryItems(caravan));
				caravan.Notify_StartedTrading();
				return;
			}
			this.playerIsCaravan = false;
		}

		// Token: 0x06012F7C RID: 77692 RVA: 0x005A0D9A File Offset: 0x0059EF9A
		private void CountToTransferChanged()
		{
			this.massUsageDirty = true;
			this.massCapacityDirty = true;
			this.tilesPerDayDirty = true;
			this.daysWorthOfFoodDirty = true;
			this.foragedFoodPerDayDirty = true;
			this.visibilityDirty = true;
		}

		// Token: 0x06012F7D RID: 77693 RVA: 0x005A0DC6 File Offset: 0x0059EFC6
		public override void Notify_CommonSearchChanged()
		{
			this.CacheTradeables();
		}

		// Token: 0x0400C4AF RID: 50351
		private bool giftsOnly;

		// Token: 0x0400C4B0 RID: 50352
		private Vector2 scrollPosition = Vector2.zero;

		// Token: 0x0400C4B1 RID: 50353
		private QuickSearchWidget quickSearchWidget = new QuickSearchWidget();

		// Token: 0x0400C4B2 RID: 50354
		public static float lastCurrencyFlashTime = -100f;

		// Token: 0x0400C4B3 RID: 50355
		private List<Tradeable> cachedTradeables;

		// Token: 0x0400C4B4 RID: 50356
		private Tradeable cachedCurrencyTradeable;

		// Token: 0x0400C4B5 RID: 50357
		private TransferableSorterDef sorter1;

		// Token: 0x0400C4B6 RID: 50358
		private TransferableSorterDef sorter2;

		// Token: 0x0400C4B7 RID: 50359
		private bool playerIsCaravan;

		// Token: 0x0400C4B8 RID: 50360
		private bool isShuttleCaravan;

		// Token: 0x0400C4B9 RID: 50361
		private List<Thing> playerCaravanAllPawnsAndItems;

		// Token: 0x0400C4BA RID: 50362
		private bool massUsageDirty = true;

		// Token: 0x0400C4BB RID: 50363
		private float cachedMassUsage;

		// Token: 0x0400C4BC RID: 50364
		private bool massCapacityDirty = true;

		// Token: 0x0400C4BD RID: 50365
		private float cachedMassCapacity;

		// Token: 0x0400C4BE RID: 50366
		private string cachedMassCapacityExplanation;

		// Token: 0x0400C4BF RID: 50367
		private bool tilesPerDayDirty = true;

		// Token: 0x0400C4C0 RID: 50368
		private float cachedTilesPerDay;

		// Token: 0x0400C4C1 RID: 50369
		private string cachedTilesPerDayExplanation;

		// Token: 0x0400C4C2 RID: 50370
		private bool daysWorthOfFoodDirty = true;

		// Token: 0x0400C4C3 RID: 50371
		[TupleElementNames(new string[]
		{
			"days",
			"tillRot"
		})]
		private ValueTuple<float, float> cachedDaysWorthOfFood;

		// Token: 0x0400C4C4 RID: 50372
		private bool foragedFoodPerDayDirty = true;

		// Token: 0x0400C4C5 RID: 50373
		[TupleElementNames(new string[]
		{
			"food",
			"perDay"
		})]
		private ValueTuple<ThingDef, float> cachedForagedFoodPerDay;

		// Token: 0x0400C4C6 RID: 50374
		private string cachedForagedFoodPerDayExplanation;

		// Token: 0x0400C4C7 RID: 50375
		private bool visibilityDirty = true;

		// Token: 0x0400C4C8 RID: 50376
		private float cachedVisibility;

		// Token: 0x0400C4C9 RID: 50377
		private string cachedVisibilityExplanation;

		// Token: 0x0400C4CA RID: 50378
		private const float TitleAreaHeight = 45f;

		// Token: 0x0400C4CB RID: 50379
		private const float TopAreaHeight = 58f;

		// Token: 0x0400C4CC RID: 50380
		private const float ColumnWidth = 120f;

		// Token: 0x0400C4CD RID: 50381
		private const float FirstCommodityY = 6f;

		// Token: 0x0400C4CE RID: 50382
		private const float RowInterval = 30f;

		// Token: 0x0400C4CF RID: 50383
		private const float SpaceBetweenTraderNameAndTraderKind = 27f;

		// Token: 0x0400C4D0 RID: 50384
		private const float ShowSellableItemsIconSize = 32f;

		// Token: 0x0400C4D1 RID: 50385
		private const float GiftModeIconSize = 32f;

		// Token: 0x0400C4D2 RID: 50386
		private const float TradeModeIconSize = 32f;

		// Token: 0x0400C4D3 RID: 50387
		protected static readonly Vector2 AcceptButtonSize = new Vector2(160f, 40f);

		// Token: 0x0400C4D4 RID: 50388
		protected static readonly Vector2 OtherBottomButtonSize = new Vector2(160f, 40f);

		// Token: 0x0400C4D5 RID: 50389
		private static readonly Texture2D ShowSellableItemsIcon = ContentFinder<Texture2D>.Get("UI/Commands/SellableItems", true);

		// Token: 0x0400C4D6 RID: 50390
		private static readonly Texture2D GiftModeIcon = ContentFinder<Texture2D>.Get("UI/Buttons/GiftMode", true);

		// Token: 0x0400C4D7 RID: 50391
		private static readonly Texture2D TradeModeIcon = ContentFinder<Texture2D>.Get("UI/Buttons/TradeMode", true);
	}
}
