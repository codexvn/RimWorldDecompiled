using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002FA9 RID: 12201
	public class Tradeable : Transferable
	{
		// Token: 0x17002C68 RID: 11368
		// (get) Token: 0x060112D4 RID: 70356 RVA: 0x004EC578 File Offset: 0x004EA778
		// (set) Token: 0x060112D5 RID: 70357 RVA: 0x004EC580 File Offset: 0x004EA780
		public override int CountToTransfer
		{
			get
			{
				return this.countToTransfer;
			}
			protected set
			{
				this.countToTransfer = value;
				base.EditBuffer = value.ToStringCached();
			}
		}

		// Token: 0x17002C69 RID: 11369
		// (get) Token: 0x060112D6 RID: 70358 RVA: 0x004EC595 File Offset: 0x004EA795
		public Thing FirstThingColony
		{
			get
			{
				if (this.thingsColony.Count == 0)
				{
					return null;
				}
				return this.thingsColony[0];
			}
		}

		// Token: 0x17002C6A RID: 11370
		// (get) Token: 0x060112D7 RID: 70359 RVA: 0x004EC5B2 File Offset: 0x004EA7B2
		public Thing FirstThingTrader
		{
			get
			{
				if (this.thingsTrader.Count == 0)
				{
					return null;
				}
				return this.thingsTrader[0];
			}
		}

		// Token: 0x17002C6B RID: 11371
		// (get) Token: 0x060112D8 RID: 70360 RVA: 0x004EC5CF File Offset: 0x004EA7CF
		public override string Label
		{
			get
			{
				return this.AnyThing.LabelNoCount;
			}
		}

		// Token: 0x17002C6C RID: 11372
		// (get) Token: 0x060112D9 RID: 70361 RVA: 0x004EC5DC File Offset: 0x004EA7DC
		public virtual float BaseMarketValue
		{
			get
			{
				return this.AnyThing.MarketValue;
			}
		}

		// Token: 0x17002C6D RID: 11373
		// (get) Token: 0x060112DA RID: 70362 RVA: 0x004EC5E9 File Offset: 0x004EA7E9
		public override bool Interactive
		{
			get
			{
				return !this.IsCurrency || (TradeSession.Active && TradeSession.giftMode);
			}
		}

		// Token: 0x17002C6E RID: 11374
		// (get) Token: 0x060112DB RID: 70363 RVA: 0x004EC603 File Offset: 0x004EA803
		public virtual bool TraderWillTrade
		{
			get
			{
				return TradeSession.trader.TraderKind.WillTrade(this.ThingDef);
			}
		}

		// Token: 0x17002C6F RID: 11375
		// (get) Token: 0x060112DC RID: 70364 RVA: 0x004EC61A File Offset: 0x004EA81A
		public override bool HasAnyThing
		{
			get
			{
				return this.AnyThing != null;
			}
		}

		// Token: 0x17002C70 RID: 11376
		// (get) Token: 0x060112DD RID: 70365 RVA: 0x004EC628 File Offset: 0x004EA828
		public override Thing AnyThing
		{
			get
			{
				if (this.FirstThingColony != null)
				{
					return this.FirstThingColony.GetInnerIfMinified();
				}
				if (this.FirstThingTrader != null)
				{
					return this.FirstThingTrader.GetInnerIfMinified();
				}
				Type type = base.GetType();
				Log.Error(((type != null) ? type.ToString() : null) + " lacks AnyThing.");
				return null;
			}
		}

		// Token: 0x17002C71 RID: 11377
		// (get) Token: 0x060112DE RID: 70366 RVA: 0x004EC67F File Offset: 0x004EA87F
		public override ThingDef ThingDef
		{
			get
			{
				Thing anyThing = this.AnyThing;
				if (anyThing == null)
				{
					return null;
				}
				return anyThing.def;
			}
		}

		// Token: 0x17002C72 RID: 11378
		// (get) Token: 0x060112DF RID: 70367 RVA: 0x004EC692 File Offset: 0x004EA892
		public ThingDef StuffDef
		{
			get
			{
				Thing anyThing = this.AnyThing;
				if (anyThing == null)
				{
					return null;
				}
				return anyThing.Stuff;
			}
		}

		// Token: 0x17002C73 RID: 11379
		// (get) Token: 0x060112E0 RID: 70368 RVA: 0x004EC6A5 File Offset: 0x004EA8A5
		public override string TipDescription
		{
			get
			{
				if (!this.HasAnyThing)
				{
					return "";
				}
				return this.AnyThing.DescriptionDetailed;
			}
		}

		// Token: 0x17002C74 RID: 11380
		// (get) Token: 0x060112E1 RID: 70369 RVA: 0x004EC6C0 File Offset: 0x004EA8C0
		public TradeAction ActionToDo
		{
			get
			{
				if (this.CountToTransfer == 0)
				{
					return TradeAction.None;
				}
				if (base.CountToTransferToDestination > 0)
				{
					return TradeAction.PlayerSells;
				}
				return TradeAction.PlayerBuys;
			}
		}

		// Token: 0x17002C75 RID: 11381
		// (get) Token: 0x060112E2 RID: 70370 RVA: 0x004EC6D8 File Offset: 0x004EA8D8
		public virtual bool IsCurrency
		{
			get
			{
				return !this.Bugged && this.ThingDef == ThingDefOf.Silver;
			}
		}

		// Token: 0x17002C76 RID: 11382
		// (get) Token: 0x060112E3 RID: 70371 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool IsFavor
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060112E4 RID: 70372 RVA: 0x004EC6F1 File Offset: 0x004EA8F1
		public virtual int CostToInt(float cost)
		{
			return Mathf.RoundToInt(cost);
		}

		// Token: 0x17002C77 RID: 11383
		// (get) Token: 0x060112E5 RID: 70373 RVA: 0x004EC6F9 File Offset: 0x004EA8F9
		public override TransferablePositiveCountDirection PositiveCountDirection
		{
			get
			{
				if (TradeSession.Active && TradeSession.giftMode)
				{
					return TransferablePositiveCountDirection.Destination;
				}
				return TransferablePositiveCountDirection.Source;
			}
		}

		// Token: 0x17002C78 RID: 11384
		// (get) Token: 0x060112E6 RID: 70374 RVA: 0x004EC70C File Offset: 0x004EA90C
		public float CurTotalCurrencyCostForSource
		{
			get
			{
				if (this.ActionToDo == TradeAction.None)
				{
					return 0f;
				}
				return (float)base.CountToTransferToSource * this.GetPriceFor(this.ActionToDo);
			}
		}

		// Token: 0x17002C79 RID: 11385
		// (get) Token: 0x060112E7 RID: 70375 RVA: 0x004EC730 File Offset: 0x004EA930
		public float CurTotalCurrencyCostForDestination
		{
			get
			{
				if (this.ActionToDo == TradeAction.None)
				{
					return 0f;
				}
				return (float)base.CountToTransferToDestination * this.GetPriceFor(this.ActionToDo);
			}
		}

		// Token: 0x17002C7A RID: 11386
		// (get) Token: 0x060112E8 RID: 70376 RVA: 0x004EC754 File Offset: 0x004EA954
		public virtual Window NewInfoDialog
		{
			get
			{
				return new Dialog_InfoCard(this.ThingDef, null);
			}
		}

		// Token: 0x17002C7B RID: 11387
		// (get) Token: 0x060112E9 RID: 70377 RVA: 0x004EC762 File Offset: 0x004EA962
		private bool Bugged
		{
			get
			{
				if (!this.HasAnyThing)
				{
					Log.ErrorOnce(this.ToString() + " is bugged. There will be no more logs about this.", 162112);
					return true;
				}
				return false;
			}
		}

		// Token: 0x060112EA RID: 70378 RVA: 0x004EC789 File Offset: 0x004EA989
		public Tradeable()
		{
		}

		// Token: 0x060112EB RID: 70379 RVA: 0x004EC7C0 File Offset: 0x004EA9C0
		public Tradeable(Thing thingColony, Thing thingTrader)
		{
			this.thingsColony.Add(thingColony);
			this.thingsTrader.Add(thingTrader);
		}

		// Token: 0x060112EC RID: 70380 RVA: 0x004EC817 File Offset: 0x004EAA17
		public void AddThing(Thing t, Transactor trans)
		{
			if (trans == Transactor.Colony)
			{
				this.thingsColony.Add(t);
			}
			if (trans == Transactor.Trader)
			{
				this.thingsTrader.Add(t);
			}
		}

		// Token: 0x060112ED RID: 70381 RVA: 0x004EC838 File Offset: 0x004EAA38
		public PriceType PriceTypeFor(TradeAction action)
		{
			return TradeSession.trader.TraderKind.PriceTypeFor(this.ThingDef, action);
		}

		// Token: 0x060112EE RID: 70382 RVA: 0x004EC850 File Offset: 0x004EAA50
		private void InitPriceDataIfNeeded()
		{
			if (this.pricePlayerBuy > 0f)
			{
				return;
			}
			if (this.IsCurrency)
			{
				this.pricePlayerBuy = this.BaseMarketValue;
				this.pricePlayerSell = this.BaseMarketValue;
				return;
			}
			this.priceFactorBuy_TraderPriceType = this.PriceTypeFor(TradeAction.PlayerBuys).PriceMultiplier();
			this.priceFactorSell_TraderPriceType = this.PriceTypeFor(TradeAction.PlayerSells).PriceMultiplier();
			this.priceGain_PlayerNegotiator = TradeSession.playerNegotiator.GetStatValue(StatDefOf.TradePriceImprovement, true, -1);
			this.priceGain_Leader = ((TradeSession.playerNegotiator == Faction.OfPlayer.leader) ? 0.02f : 0f);
			this.priceGain_Settlement = TradeSession.trader.TradePriceImprovementOffsetForPlayer;
			this.priceFactorSell_ItemSellPriceFactor = this.AnyThing.GetStatValue(StatDefOf.SellPriceFactor, true, -1);
			this.priceFactorBuy_JoinAs = 1f;
			this.priceFactorSell_HumanPawn = 1f;
			if (this.ThingDef.IsNonMedicalDrug)
			{
				this.priceGain_DrugBonus = TradeSession.playerNegotiator.GetStatValue(StatDefOf.DrugSellPriceImprovement, true, -1);
			}
			if (ModsConfig.IdeologyActive)
			{
				if (this.ThingDef.IsLeather || this.ThingDef.IsMeat || this.ThingDef.IsWool)
				{
					this.priceGain_AnimalProduceBonus = TradeSession.playerNegotiator.GetStatValue(StatDefOf.AnimalProductsSellImprovement, true, -1);
				}
				Pawn pawn = this.AnyThing as Pawn;
				if (pawn != null && pawn.RaceProps.Humanlike && pawn.guest != null)
				{
					if (pawn.guest.joinStatus == JoinStatus.JoinAsSlave)
					{
						this.priceFactorBuy_JoinAs = 0.6f;
					}
					this.priceFactorSell_HumanPawn = 0.6f;
				}
			}
			this.pricePlayerBuy = TradeUtility.GetPricePlayerBuy(this.AnyThing, this.priceFactorBuy_TraderPriceType, this.priceFactorBuy_JoinAs, this.priceGain_PlayerNegotiator, this.priceGain_Settlement);
			this.pricePlayerSell = TradeUtility.GetPricePlayerSell(this.AnyThing, this.priceFactorSell_TraderPriceType, this.priceFactorSell_HumanPawn, this.priceGain_PlayerNegotiator, this.priceGain_Settlement, this.priceGain_DrugBonus, this.priceGain_AnimalProduceBonus, TradeSession.TradeCurrency);
			if (this.pricePlayerSell >= this.pricePlayerBuy)
			{
				this.pricePlayerSell = this.pricePlayerBuy;
			}
		}

		// Token: 0x060112EF RID: 70383 RVA: 0x004ECA5C File Offset: 0x004EAC5C
		public string GetPriceTooltip(TradeAction action)
		{
			if (!this.HasAnyThing)
			{
				return "";
			}
			this.InitPriceDataIfNeeded();
			string text = (action == TradeAction.PlayerBuys) ? "BuyPriceDesc".Translate() : "SellPriceDesc".Translate();
			if (TradeSession.TradeCurrency != TradeCurrency.Silver)
			{
				return text;
			}
			text += "\n\n";
			text += StatDefOf.MarketValue.LabelCap + ": " + this.BaseMarketValue.ToStringMoney(null);
			if (action == TradeAction.PlayerBuys)
			{
				text += "\n  x " + 1.4f.ToString("F2") + " (" + "Buying".Translate() + ")";
				if (this.priceFactorBuy_TraderPriceType != 1f)
				{
					text += "\n  x " + this.priceFactorBuy_TraderPriceType.ToString("F2") + " (" + "TraderTypePrice".Translate() + ")";
				}
				if (Find.Storyteller.difficulty.tradePriceFactorLoss != 0f)
				{
					text += "\n  x " + (1f + Find.Storyteller.difficulty.tradePriceFactorLoss).ToString("F2") + " (" + "DifficultyLevel".Translate() + ")";
				}
				text += "\n";
				text += "\n" + "YourNegotiatorBonus".Translate() + ": -" + (this.priceGain_PlayerNegotiator - this.priceGain_Leader).ToStringPercent();
				if (ModsConfig.IdeologyActive)
				{
					if (this.priceGain_Leader != 0f)
					{
						text += "\n" + "YourLeaderTradeBonus".Translate() + ": -" + this.priceGain_Leader.ToStringPercent();
					}
					if (this.priceFactorBuy_JoinAs != 1f)
					{
						text += "\n" + "Slave".Translate().CapitalizeFirst() + ": x" + this.priceFactorBuy_JoinAs.ToStringPercent();
					}
				}
				if (this.priceGain_Settlement != 0f)
				{
					text += "\n" + "TradeWithFactionBaseBonus".Translate() + ": -" + this.priceGain_Settlement.ToStringPercent();
				}
			}
			else
			{
				text += "\n  x " + 0.6f.ToString("F2") + " (" + "Selling".Translate() + ")";
				if (this.priceFactorSell_TraderPriceType != 1f)
				{
					text += "\n  x " + this.priceFactorSell_TraderPriceType.ToString("F2") + " (" + "TraderTypePrice".Translate() + ")";
				}
				if (this.priceFactorSell_ItemSellPriceFactor != 1f)
				{
					text += "\n  x " + this.priceFactorSell_ItemSellPriceFactor.ToString("F2") + " (" + "ItemSellPriceFactor".Translate() + ")";
				}
				if (this.priceFactorSell_HumanPawn != 1f)
				{
					text += "\n  x " + this.priceFactorSell_HumanPawn.ToString("F2") + " (" + "Slave".Translate() + ")";
				}
				if (Find.Storyteller.difficulty.tradePriceFactorLoss != 0f)
				{
					text += "\n  x " + (1f - Find.Storyteller.difficulty.tradePriceFactorLoss).ToString("F2") + " (" + "DifficultyLevel".Translate() + ")";
				}
				text += "\n";
				text += "\n" + "YourNegotiatorBonus".Translate() + ": " + (this.priceGain_PlayerNegotiator - this.priceGain_Leader).ToStringPercent();
				if (ModsConfig.IdeologyActive && this.priceGain_Leader != 0f)
				{
					text += "\n" + "YourLeaderTradeBonus".Translate() + ": " + this.priceGain_Leader.ToStringPercent();
				}
				if (this.priceGain_Settlement != 0f)
				{
					text += "\n" + "TradeWithFactionBaseBonus".Translate() + ": " + this.priceGain_Settlement.ToStringPercent();
				}
				if (this.priceGain_DrugBonus != 0f)
				{
					text += "\n" + "TradingDrugsBonus".Translate() + ": " + this.priceGain_DrugBonus.ToStringPercent();
				}
				if (this.priceGain_AnimalProduceBonus != 0f)
				{
					text += "\n" + "TradingAnimalProduceBonus".Translate() + ": " + this.priceGain_AnimalProduceBonus.ToStringPercent();
				}
			}
			text += "\n\n";
			float priceFor = this.GetPriceFor(action);
			text += "FinalPrice".Translate() + ": " + priceFor.ToStringMoney(null);
			if ((action == TradeAction.PlayerBuys && priceFor <= 0.5f) || (action == TradeAction.PlayerBuys && priceFor <= 0.01f))
			{
				text += " (" + "minimum".Translate() + ")";
			}
			return text;
		}

		// Token: 0x060112F0 RID: 70384 RVA: 0x004ED0C1 File Offset: 0x004EB2C1
		public virtual float GetPriceFor(TradeAction action)
		{
			this.InitPriceDataIfNeeded();
			if (action == TradeAction.PlayerBuys)
			{
				return this.pricePlayerBuy;
			}
			return this.pricePlayerSell;
		}

		// Token: 0x060112F1 RID: 70385 RVA: 0x004ED0DA File Offset: 0x004EB2DA
		public override int GetMinimumToTransfer()
		{
			if (this.PositiveCountDirection == TransferablePositiveCountDirection.Destination)
			{
				return -this.CountHeldBy(Transactor.Trader);
			}
			return -this.CountHeldBy(Transactor.Colony);
		}

		// Token: 0x060112F2 RID: 70386 RVA: 0x004ED0F6 File Offset: 0x004EB2F6
		public override int GetMaximumToTransfer()
		{
			if (this.PositiveCountDirection == TransferablePositiveCountDirection.Destination)
			{
				return this.CountHeldBy(Transactor.Colony);
			}
			return this.CountHeldBy(Transactor.Trader);
		}

		// Token: 0x060112F3 RID: 70387 RVA: 0x004ED110 File Offset: 0x004EB310
		public override AcceptanceReport UnderflowReport()
		{
			if (this.PositiveCountDirection == TransferablePositiveCountDirection.Destination)
			{
				return new AcceptanceReport("TraderHasNoMore".Translate());
			}
			return new AcceptanceReport("ColonyHasNoMore".Translate());
		}

		// Token: 0x060112F4 RID: 70388 RVA: 0x004ED144 File Offset: 0x004EB344
		public override AcceptanceReport OverflowReport()
		{
			if (this.PositiveCountDirection == TransferablePositiveCountDirection.Destination)
			{
				return new AcceptanceReport("ColonyHasNoMore".Translate());
			}
			return new AcceptanceReport("TraderHasNoMore".Translate());
		}

		// Token: 0x060112F5 RID: 70389 RVA: 0x004ED178 File Offset: 0x004EB378
		private List<Thing> TransactorThings(Transactor trans)
		{
			if (trans == Transactor.Colony)
			{
				return this.thingsColony;
			}
			return this.thingsTrader;
		}

		// Token: 0x060112F6 RID: 70390 RVA: 0x004ED18C File Offset: 0x004EB38C
		public virtual int CountHeldBy(Transactor trans)
		{
			List<Thing> list = this.TransactorThings(trans);
			int num = 0;
			for (int i = 0; i < list.Count; i++)
			{
				num += list[i].stackCount;
			}
			return num;
		}

		// Token: 0x060112F7 RID: 70391 RVA: 0x004ED1C4 File Offset: 0x004EB3C4
		public int CountPostDealFor(Transactor trans)
		{
			if (trans == Transactor.Colony)
			{
				return this.CountHeldBy(trans) + base.CountToTransferToSource;
			}
			return this.CountHeldBy(trans) + base.CountToTransferToDestination;
		}

		// Token: 0x060112F8 RID: 70392 RVA: 0x004ED1E8 File Offset: 0x004EB3E8
		public virtual void ResolveTrade()
		{
			if (this.ActionToDo == TradeAction.PlayerSells)
			{
				TransferableUtility.TransferNoSplit(this.thingsColony, base.CountToTransferToDestination, delegate(Thing thing, int countToTransfer)
				{
					TradeSession.trader.GiveSoldThingToTrader(thing, countToTransfer, TradeSession.playerNegotiator);
				}, true, true);
				return;
			}
			if (this.ActionToDo == TradeAction.PlayerBuys)
			{
				TransferableUtility.TransferNoSplit(this.thingsTrader, base.CountToTransferToSource, delegate(Thing thing, int countToTransfer)
				{
					this.CheckTeachOpportunity(thing, countToTransfer);
					TradeSession.trader.GiveSoldThingToPlayer(thing, countToTransfer, TradeSession.playerNegotiator);
				}, true, true);
			}
		}

		// Token: 0x060112F9 RID: 70393 RVA: 0x004ED25C File Offset: 0x004EB45C
		private void CheckTeachOpportunity(Thing boughtThing, int boughtCount)
		{
			Building building = boughtThing as Building;
			if (building == null)
			{
				MinifiedThing minifiedThing = boughtThing as MinifiedThing;
				if (minifiedThing != null)
				{
					building = (minifiedThing.InnerThing as Building);
				}
			}
			bool flag;
			if (building == null)
			{
				flag = (null != null);
			}
			else
			{
				BuildingProperties building2 = building.def.building;
				flag = (((building2 != null) ? building2.boughtConceptLearnOpportunity : null) != null);
			}
			if (flag)
			{
				LessonAutoActivator.TeachOpportunity(building.def.building.boughtConceptLearnOpportunity, OpportunityType.GoodToKnow);
			}
		}

		// Token: 0x060112FA RID: 70394 RVA: 0x004ED2C0 File Offset: 0x004EB4C0
		public override string ToString()
		{
			string[] array = new string[6];
			int num = 0;
			Type type = base.GetType();
			array[num] = ((type != null) ? type.ToString() : null);
			array[1] = "(";
			int num2 = 2;
			ThingDef thingDef = this.ThingDef;
			array[num2] = ((thingDef != null) ? thingDef.ToString() : null);
			array[3] = ", countToTransfer=";
			array[4] = this.CountToTransfer.ToString();
			array[5] = ")";
			return string.Concat(array);
		}

		// Token: 0x060112FB RID: 70395 RVA: 0x004ED32B File Offset: 0x004EB52B
		public override int GetHashCode()
		{
			return this.AnyThing.GetHashCode();
		}

		// Token: 0x060112FC RID: 70396 RVA: 0x004ED338 File Offset: 0x004EB538
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.thingsColony.RemoveAll((Thing x) => x.Destroyed);
				this.thingsTrader.RemoveAll((Thing x) => x.Destroyed);
			}
			Scribe_Values.Look<int>(ref this.countToTransfer, "countToTransfer", 0, false);
			Scribe_Collections.Look<Thing>(ref this.thingsColony, "thingsColony", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<Thing>(ref this.thingsTrader, "thingsTrader", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				base.EditBuffer = this.countToTransfer.ToStringCached();
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.thingsColony.RemoveAll((Thing x) => x == null) == 0)
				{
					if (this.thingsTrader.RemoveAll((Thing x) => x == null) == 0)
					{
						return;
					}
				}
				Log.Warning("Some of the things were null after loading.");
			}
		}

		// Token: 0x0400B256 RID: 45654
		public List<Thing> thingsColony = new List<Thing>();

		// Token: 0x0400B257 RID: 45655
		public List<Thing> thingsTrader = new List<Thing>();

		// Token: 0x0400B258 RID: 45656
		private int countToTransfer;

		// Token: 0x0400B259 RID: 45657
		public const float NegotiatorLeaderOffset = 0.02f;

		// Token: 0x0400B25A RID: 45658
		private const float PriceFactor_JoinAsSlaveOrColonyPawn = 0.6f;

		// Token: 0x0400B25B RID: 45659
		private float pricePlayerBuy = -1f;

		// Token: 0x0400B25C RID: 45660
		private float pricePlayerSell = -1f;

		// Token: 0x0400B25D RID: 45661
		private float priceFactorBuy_TraderPriceType;

		// Token: 0x0400B25E RID: 45662
		private float priceFactorBuy_JoinAs;

		// Token: 0x0400B25F RID: 45663
		private float priceFactorSell_HumanPawn;

		// Token: 0x0400B260 RID: 45664
		private float priceFactorSell_TraderPriceType;

		// Token: 0x0400B261 RID: 45665
		private float priceFactorSell_ItemSellPriceFactor;

		// Token: 0x0400B262 RID: 45666
		private float priceGain_PlayerNegotiator;

		// Token: 0x0400B263 RID: 45667
		private float priceGain_Leader;

		// Token: 0x0400B264 RID: 45668
		private float priceGain_Settlement;

		// Token: 0x0400B265 RID: 45669
		private float priceGain_DrugBonus;

		// Token: 0x0400B266 RID: 45670
		private float priceGain_AnimalProduceBonus;
	}
}
