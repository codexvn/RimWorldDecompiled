using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003DD4 RID: 15828
	public class Settlement_TraderTracker : IThingHolder, IExposable
	{
		// Token: 0x170035BC RID: 13756
		// (get) Token: 0x0601595B RID: 88411 RVA: 0x002C8DD2 File Offset: 0x002C6FD2
		protected virtual int RegenerateStockEveryDays
		{
			get
			{
				return 30;
			}
		}

		// Token: 0x170035BD RID: 13757
		// (get) Token: 0x0601595C RID: 88412 RVA: 0x00676637 File Offset: 0x00674837
		public IThingHolder ParentHolder
		{
			get
			{
				return this.settlement;
			}
		}

		// Token: 0x170035BE RID: 13758
		// (get) Token: 0x0601595D RID: 88413 RVA: 0x0067663F File Offset: 0x0067483F
		public List<Thing> StockListForReading
		{
			get
			{
				if (this.stock == null || this.stock.InnerListForReading.Empty<Thing>())
				{
					this.RegenerateStock();
				}
				return this.stock.InnerListForReading;
			}
		}

		// Token: 0x170035BF RID: 13759
		// (get) Token: 0x0601595E RID: 88414 RVA: 0x0067666C File Offset: 0x0067486C
		public TraderKindDef TraderKind
		{
			get
			{
				List<TraderKindDef> baseTraderKinds = this.settlement.Faction.def.baseTraderKinds;
				if (baseTraderKinds.NullOrEmpty<TraderKindDef>())
				{
					return null;
				}
				int index = Mathf.Abs(this.settlement.HashOffset()) % baseTraderKinds.Count;
				return baseTraderKinds[index];
			}
		}

		// Token: 0x170035C0 RID: 13760
		// (get) Token: 0x0601595F RID: 88415 RVA: 0x006766B8 File Offset: 0x006748B8
		public int RandomPriceFactorSeed
		{
			get
			{
				return Gen.HashCombineInt(this.settlement.ID, 1933327354);
			}
		}

		// Token: 0x170035C1 RID: 13761
		// (get) Token: 0x06015960 RID: 88416 RVA: 0x006766CF File Offset: 0x006748CF
		public bool EverVisited
		{
			get
			{
				return this.everGeneratedStock;
			}
		}

		// Token: 0x170035C2 RID: 13762
		// (get) Token: 0x06015961 RID: 88417 RVA: 0x006766D7 File Offset: 0x006748D7
		public bool RestockedSinceLastVisit
		{
			get
			{
				return this.everGeneratedStock && this.stock == null;
			}
		}

		// Token: 0x170035C3 RID: 13763
		// (get) Token: 0x06015962 RID: 88418 RVA: 0x006766EC File Offset: 0x006748EC
		public int NextRestockTick
		{
			get
			{
				if (this.stock == null || !this.everGeneratedStock)
				{
					return -1;
				}
				return ((this.lastStockGenerationTicks == -1) ? 0 : this.lastStockGenerationTicks) + this.RegenerateStockEveryDays * 60000;
			}
		}

		// Token: 0x170035C4 RID: 13764
		// (get) Token: 0x06015963 RID: 88419 RVA: 0x00676720 File Offset: 0x00674920
		public virtual string TraderName
		{
			get
			{
				if (this.settlement.Faction == null)
				{
					return this.settlement.LabelCap;
				}
				return "SettlementTrader".Translate(this.settlement.LabelCap, this.settlement.Faction.Name);
			}
		}

		// Token: 0x170035C5 RID: 13765
		// (get) Token: 0x06015964 RID: 88420 RVA: 0x0067677C File Offset: 0x0067497C
		public virtual bool CanTradeNow
		{
			get
			{
				return this.TraderKind != null && (this.stock == null || this.stock.InnerListForReading.Empty<Thing>() || this.stock.InnerListForReading.Any((Thing x) => this.TraderKind.WillTrade(x.def)));
			}
		}

		// Token: 0x170035C6 RID: 13766
		// (get) Token: 0x06015965 RID: 88421 RVA: 0x0034EE1A File Offset: 0x0034D01A
		public virtual float TradePriceImprovementOffsetForPlayer
		{
			get
			{
				return 0.02f;
			}
		}

		// Token: 0x06015966 RID: 88422 RVA: 0x006767CB File Offset: 0x006749CB
		public Settlement_TraderTracker(Settlement settlement)
		{
			this.settlement = settlement;
		}

		// Token: 0x06015967 RID: 88423 RVA: 0x006767EC File Offset: 0x006749EC
		public virtual void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.tmpSavedPawns.Clear();
				if (this.stock != null)
				{
					for (int i = this.stock.Count - 1; i >= 0; i--)
					{
						Pawn pawn = this.stock[i] as Pawn;
						if (pawn != null)
						{
							this.stock.Remove(pawn);
							this.tmpSavedPawns.Add(pawn);
						}
					}
				}
			}
			Scribe_Collections.Look<Pawn>(ref this.tmpSavedPawns, "tmpSavedPawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Deep.Look<ThingOwner<Thing>>(ref this.stock, "stock", Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.lastStockGenerationTicks, "lastStockGenerationTicks", 0, false);
			Scribe_Values.Look<bool>(ref this.everGeneratedStock, "wasStockGeneratedYet", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit || Scribe.mode == LoadSaveMode.Saving)
			{
				for (int j = 0; j < this.tmpSavedPawns.Count; j++)
				{
					this.stock.TryAdd(this.tmpSavedPawns[j], false);
				}
				this.tmpSavedPawns.Clear();
			}
		}

		// Token: 0x06015968 RID: 88424 RVA: 0x006768F3 File Offset: 0x00674AF3
		public virtual IEnumerable<Thing> ColonyThingsWillingToBuy(Pawn playerNegotiator)
		{
			Caravan caravan = playerNegotiator.GetCaravan();
			foreach (Thing thing in CaravanInventoryUtility.AllInventoryItems(caravan))
			{
				yield return thing;
			}
			List<Thing>.Enumerator enumerator = default(List<Thing>.Enumerator);
			List<Pawn> pawns = caravan.PawnsListForReading;
			int num;
			for (int i = 0; i < pawns.Count; i = num + 1)
			{
				if (!caravan.IsOwner(pawns[i]))
				{
					yield return pawns[i];
				}
				num = i;
			}
			yield break;
			yield break;
		}

		// Token: 0x06015969 RID: 88425 RVA: 0x00676904 File Offset: 0x00674B04
		public virtual void GiveSoldThingToTrader(Thing toGive, int countToGive, Pawn playerNegotiator)
		{
			if (this.stock == null)
			{
				this.RegenerateStock();
			}
			Caravan caravan = playerNegotiator.GetCaravan();
			Thing thing = toGive.SplitOff(countToGive);
			thing.PreTraded(TradeAction.PlayerSells, playerNegotiator, this.settlement);
			Pawn pawn = toGive as Pawn;
			if (pawn != null)
			{
				CaravanInventoryUtility.MoveAllInventoryToSomeoneElse(pawn, caravan.PawnsListForReading, null);
				if (pawn.RaceProps.Humanlike)
				{
					return;
				}
				if (!this.stock.TryAdd(pawn, false))
				{
					pawn.Destroy(DestroyMode.Vanish);
					return;
				}
			}
			else if (!this.stock.TryAdd(thing, false))
			{
				thing.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x0601596A RID: 88426 RVA: 0x00676990 File Offset: 0x00674B90
		public virtual void GiveSoldThingToPlayer(Thing toGive, int countToGive, Pawn playerNegotiator)
		{
			Caravan caravan = playerNegotiator.GetCaravan();
			Thing thing = toGive.SplitOff(countToGive);
			thing.PreTraded(TradeAction.PlayerBuys, playerNegotiator, this.settlement);
			Pawn pawn = thing as Pawn;
			if (pawn != null)
			{
				caravan.AddPawn(pawn, true);
				return;
			}
			Pawn pawn2 = CaravanInventoryUtility.FindPawnToMoveInventoryTo(thing, caravan.PawnsListForReading, null, null);
			if (pawn2 == null)
			{
				Log.Error("Could not find any pawn to give sold thing to.");
				thing.Destroy(DestroyMode.Vanish);
				return;
			}
			if (!pawn2.inventory.innerContainer.TryAdd(thing, true))
			{
				Log.Error("Could not add sold thing to inventory.");
				thing.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x0601596B RID: 88427 RVA: 0x00676A18 File Offset: 0x00674C18
		public virtual void TraderTrackerTick()
		{
			if (this.stock != null)
			{
				if (Find.TickManager.TicksGame - this.lastStockGenerationTicks > this.RegenerateStockEveryDays * 60000)
				{
					this.TryDestroyStock();
					return;
				}
				for (int i = this.stock.Count - 1; i >= 0; i--)
				{
					Pawn pawn = this.stock[i] as Pawn;
					if (pawn != null && pawn.Destroyed)
					{
						this.stock.Remove(pawn);
					}
				}
				for (int j = this.stock.Count - 1; j >= 0; j--)
				{
					Pawn pawn2 = this.stock[j] as Pawn;
					if (pawn2 != null && !pawn2.IsWorldPawn())
					{
						Log.Error("Faction base has non-world-pawns in its stock. Removing...");
						this.stock.Remove(pawn2);
					}
				}
			}
		}

		// Token: 0x0601596C RID: 88428 RVA: 0x00676AE4 File Offset: 0x00674CE4
		public void TryDestroyStock()
		{
			if (this.stock != null)
			{
				for (int i = this.stock.Count - 1; i >= 0; i--)
				{
					Thing thing = this.stock[i];
					this.stock.Remove(thing);
					if (!(thing is Pawn) && !thing.Destroyed)
					{
						thing.Destroy(DestroyMode.Vanish);
					}
				}
				this.stock = null;
			}
		}

		// Token: 0x0601596D RID: 88429 RVA: 0x00676B49 File Offset: 0x00674D49
		public bool ContainsPawn(Pawn p)
		{
			return this.stock != null && this.stock.Contains(p);
		}

		// Token: 0x0601596E RID: 88430 RVA: 0x00676B64 File Offset: 0x00674D64
		protected virtual void RegenerateStock()
		{
			this.TryDestroyStock();
			this.stock = new ThingOwner<Thing>(this);
			this.everGeneratedStock = true;
			if (this.settlement.Faction == null || !this.settlement.Faction.IsPlayer)
			{
				ThingSetMakerParams parms = default(ThingSetMakerParams);
				parms.traderDef = this.TraderKind;
				parms.tile = new PlanetTile?(this.settlement.Tile);
				parms.makingFaction = this.settlement.Faction;
				this.stock.TryAddRangeOrTransfer(ThingSetMakerDefOf.TraderStock.root.Generate(parms), true, false);
			}
			for (int i = 0; i < this.stock.Count; i++)
			{
				Pawn pawn = this.stock[i] as Pawn;
				if (pawn != null)
				{
					Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
				}
			}
			this.lastStockGenerationTicks = Find.TickManager.TicksGame;
		}

		// Token: 0x0601596F RID: 88431 RVA: 0x00676C4B File Offset: 0x00674E4B
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.stock;
		}

		// Token: 0x06015970 RID: 88432 RVA: 0x00676C53 File Offset: 0x00674E53
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x0400EFE4 RID: 61412
		public Settlement settlement;

		// Token: 0x0400EFE5 RID: 61413
		private ThingOwner<Thing> stock;

		// Token: 0x0400EFE6 RID: 61414
		private int lastStockGenerationTicks = -1;

		// Token: 0x0400EFE7 RID: 61415
		private bool everGeneratedStock;

		// Token: 0x0400EFE8 RID: 61416
		private const float DefaultTradePriceImprovement = 0.02f;

		// Token: 0x0400EFE9 RID: 61417
		private List<Pawn> tmpSavedPawns = new List<Pawn>();
	}
}
