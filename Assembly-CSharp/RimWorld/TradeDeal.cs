using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02002FAF RID: 12207
	public class TradeDeal
	{
		// Token: 0x17002C88 RID: 11400
		// (get) Token: 0x06011319 RID: 70425 RVA: 0x004ED759 File Offset: 0x004EB959
		public int TradeableCount
		{
			get
			{
				return this.tradeables.Count;
			}
		}

		// Token: 0x17002C89 RID: 11401
		// (get) Token: 0x0601131A RID: 70426 RVA: 0x004ED768 File Offset: 0x004EB968
		public Tradeable CurrencyTradeable
		{
			get
			{
				for (int i = 0; i < this.tradeables.Count; i++)
				{
					if ((TradeSession.TradeCurrency == TradeCurrency.Favor) ? this.tradeables[i].IsFavor : (this.tradeables[i].ThingDef == ThingDefOf.Silver))
					{
						return this.tradeables[i];
					}
				}
				return null;
			}
		}

		// Token: 0x17002C8A RID: 11402
		// (get) Token: 0x0601131B RID: 70427 RVA: 0x004ED7CE File Offset: 0x004EB9CE
		public List<Tradeable> AllTradeables
		{
			get
			{
				return this.tradeables;
			}
		}

		// Token: 0x0601131C RID: 70428 RVA: 0x004ED7D6 File Offset: 0x004EB9D6
		public TradeDeal()
		{
			this.Reset();
		}

		// Token: 0x0601131D RID: 70429 RVA: 0x004ED7FA File Offset: 0x004EB9FA
		public void Reset()
		{
			this.tradeables.Clear();
			this.cannotSellReasons.Clear();
			this.AddAllTradeables();
		}

		// Token: 0x0601131E RID: 70430 RVA: 0x004ED818 File Offset: 0x004EBA18
		private void AddAllTradeables()
		{
			foreach (Thing t in TradeSession.trader.ColonyThingsWillingToBuy(TradeSession.playerNegotiator))
			{
				if (TradeUtility.PlayerSellableNow(t, TradeSession.trader))
				{
					string text;
					if (!TradeSession.playerNegotiator.IsWorldPawn() && !this.InSellablePosition(t, out text))
					{
						if (text != null && !this.cannotSellReasons.Contains(text))
						{
							this.cannotSellReasons.Add(text);
						}
					}
					else
					{
						this.AddToTradeables(t, Transactor.Colony);
					}
				}
			}
			if (!TradeSession.giftMode)
			{
				foreach (Thing t2 in TradeSession.trader.Goods)
				{
					this.AddToTradeables(t2, Transactor.Trader);
				}
			}
			if (!TradeSession.giftMode)
			{
				if (this.tradeables.Find((Tradeable x) => x.IsCurrency) == null)
				{
					Thing thing = ThingMaker.MakeThing(ThingDefOf.Silver, null);
					thing.stackCount = 0;
					this.AddToTradeables(thing, Transactor.Trader);
				}
			}
			if (TradeSession.TradeCurrency == TradeCurrency.Favor)
			{
				this.tradeables.Add(new Tradeable_RoyalFavor());
			}
		}

		// Token: 0x0601131F RID: 70431 RVA: 0x004ED964 File Offset: 0x004EBB64
		private bool InSellablePosition(Thing t, out string reason)
		{
			if (!t.Spawned && (!ModsConfig.BiotechActive || t.def != ThingDefOf.Genepack || !(t.ParentHolder is CompGenepackContainer)) && (!(t is Book) || !(t.ParentHolder is Building_Bookcase)) && (!(t is Apparel) || !(t.ParentHolder is Building_OutfitStand)))
			{
				reason = null;
				return false;
			}
			IntVec3 positionHeld = t.PositionHeld;
			Room room = t.SpawnedParentOrMe.GetRoom(RegionType.Set_All);
			if (positionHeld.Fogged(t.MapHeld))
			{
				reason = null;
				return false;
			}
			if (room != null)
			{
				int num = GenRadial.NumCellsInRadius(6.9f);
				for (int i = 0; i < num; i++)
				{
					IntVec3 intVec = positionHeld + GenRadial.RadialPattern[i];
					if (intVec.InBounds(t.MapHeld) && intVec.GetRoom(t.MapHeld) == room)
					{
						List<Thing> thingList = intVec.GetThingList(t.MapHeld);
						for (int j = 0; j < thingList.Count; j++)
						{
							if (thingList[j].PreventPlayerSellingThingsNearby(out reason))
							{
								return false;
							}
						}
					}
				}
			}
			reason = null;
			return true;
		}

		// Token: 0x06011320 RID: 70432 RVA: 0x004EDA7C File Offset: 0x004EBC7C
		private void AddToTradeables(Thing t, Transactor trans)
		{
			Tradeable tradeable = TransferableUtility.TradeableMatching(t, this.tradeables);
			if (tradeable == null)
			{
				if (t is Pawn)
				{
					tradeable = new Tradeable_Pawn();
				}
				else
				{
					tradeable = new Tradeable();
				}
				this.tradeables.Add(tradeable);
			}
			tradeable.AddThing(t, trans);
		}

		// Token: 0x06011321 RID: 70433 RVA: 0x004EDAC4 File Offset: 0x004EBCC4
		public void UpdateCurrencyCount()
		{
			if (this.CurrencyTradeable == null || TradeSession.giftMode)
			{
				return;
			}
			float num = 0f;
			for (int i = 0; i < this.tradeables.Count; i++)
			{
				Tradeable tradeable = this.tradeables[i];
				if (!tradeable.IsCurrency)
				{
					num += tradeable.CurTotalCurrencyCostForSource;
				}
			}
			this.CurrencyTradeable.ForceToSource(-this.CurrencyTradeable.CostToInt(num));
		}

		// Token: 0x06011322 RID: 70434 RVA: 0x004EDB34 File Offset: 0x004EBD34
		public bool TryExecute(out bool actuallyTraded)
		{
			if (TradeSession.giftMode)
			{
				this.UpdateCurrencyCount();
				this.LimitCurrencyCountToFunds();
				int goodwillChange = FactionGiftUtility.GetGoodwillChange(this.tradeables, TradeSession.trader.Faction);
				FactionGiftUtility.GiveGift(this.tradeables, TradeSession.trader.Faction, TradeSession.playerNegotiator);
				actuallyTraded = ((float)goodwillChange > 0f);
				return true;
			}
			if (this.CurrencyTradeable == null || this.CurrencyTradeable.CountPostDealFor(Transactor.Colony) < 0)
			{
				Find.WindowStack.WindowOfType<Dialog_Trade>().FlashSilver();
				Messages.Message("MessageColonyCannotAfford".Translate(), MessageTypeDefOf.RejectInput, false);
				actuallyTraded = false;
				return false;
			}
			if (ModsConfig.IdeologyActive)
			{
				if (this.tradeables.Any((Tradeable x) => x.ActionToDo != TradeAction.None && x.ThingDef != null && x.ThingDef.IsNaturalOrgan))
				{
					HistoryEvent historyEvent = new HistoryEvent(HistoryEventDefOf.TradedOrgan, TradeSession.playerNegotiator.Named(HistoryEventArgsNames.Doer));
					if (!historyEvent.Notify_PawnAboutToDo("MessagePawnUnwillingToDoDueToIdeo"))
					{
						actuallyTraded = false;
						return false;
					}
					Find.HistoryEventsManager.RecordEvent(historyEvent, true);
				}
				if (this.tradeables.Any((Tradeable x) => x.ActionToDo == TradeAction.PlayerSells && x.ThingDef != null && x.ThingDef.IsNaturalOrgan))
				{
					HistoryEvent historyEvent2 = new HistoryEvent(HistoryEventDefOf.SoldOrgan, TradeSession.playerNegotiator.Named(HistoryEventArgsNames.Doer));
					if (!historyEvent2.Notify_PawnAboutToDo("MessagePawnUnwillingToDoDueToIdeo"))
					{
						actuallyTraded = false;
						return false;
					}
					Find.HistoryEventsManager.RecordEvent(historyEvent2, true);
				}
			}
			this.UpdateCurrencyCount();
			this.LimitCurrencyCountToFunds();
			actuallyTraded = false;
			float num = 0f;
			foreach (Tradeable tradeable in this.tradeables)
			{
				if (tradeable.ActionToDo != TradeAction.None)
				{
					actuallyTraded = true;
				}
				if (tradeable.ActionToDo == TradeAction.PlayerSells)
				{
					num += tradeable.CurTotalCurrencyCostForDestination;
				}
				tradeable.ResolveTrade();
			}
			this.Reset();
			if (TradeSession.trader.Faction != null)
			{
				TradeSession.trader.Faction.Notify_PlayerTraded(num, TradeSession.playerNegotiator);
			}
			Pawn pawn = TradeSession.trader as Pawn;
			if (pawn != null)
			{
				TaleRecorder.RecordTale(TaleDefOf.TradedWith, new object[]
				{
					TradeSession.playerNegotiator,
					pawn
				});
			}
			if (actuallyTraded)
			{
				TradeSession.playerNegotiator.mindState.inspirationHandler.EndInspiration(InspirationDefOf.Inspired_Trade);
			}
			return true;
		}

		// Token: 0x06011323 RID: 70435 RVA: 0x004EDDA0 File Offset: 0x004EBFA0
		public bool DoesTraderHaveEnoughSilver()
		{
			return TradeSession.giftMode || (this.CurrencyTradeable != null && this.CurrencyTradeable.CountPostDealFor(Transactor.Trader) >= 0);
		}

		// Token: 0x06011324 RID: 70436 RVA: 0x004EDDC8 File Offset: 0x004EBFC8
		private void LimitCurrencyCountToFunds()
		{
			if (this.CurrencyTradeable == null)
			{
				return;
			}
			if (this.CurrencyTradeable.CountToTransferToSource > this.CurrencyTradeable.CountHeldBy(Transactor.Trader))
			{
				this.CurrencyTradeable.ForceToSource(this.CurrencyTradeable.CountHeldBy(Transactor.Trader));
			}
			if (this.CurrencyTradeable.CountToTransferToDestination > this.CurrencyTradeable.CountHeldBy(Transactor.Colony))
			{
				this.CurrencyTradeable.ForceToDestination(this.CurrencyTradeable.CountHeldBy(Transactor.Colony));
			}
		}

		// Token: 0x0400B273 RID: 45683
		private List<Tradeable> tradeables = new List<Tradeable>();

		// Token: 0x0400B274 RID: 45684
		public List<string> cannotSellReasons = new List<string>();
	}
}
