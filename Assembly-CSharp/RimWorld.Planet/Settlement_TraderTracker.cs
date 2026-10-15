using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld.Planet;

public class Settlement_TraderTracker : IThingHolderTickable, IThingHolder, IExposable
{
	[CompilerGenerated]
	private sealed class _003CColonyThingsWillingToBuy_003Ed__32 : IEnumerable<Thing>, IEnumerable, IEnumerator<Thing>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Thing _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Pawn playerNegotiator;

		public Pawn _003C_003E3__playerNegotiator;

		private Caravan _003Ccaravan_003E5__2;

		private List<Pawn> _003Cpawns_003E5__3;

		private List<Thing>.Enumerator _003C_003E7__wrap3;

		private int _003Ci_003E5__5;

		Thing IEnumerator<Thing>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CColonyThingsWillingToBuy_003Ed__32(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 1)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003Ccaravan_003E5__2 = null;
			_003Cpawns_003E5__3 = null;
			_003C_003E7__wrap3 = default(List<Thing>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003Ccaravan_003E5__2 = playerNegotiator.GetCaravan();
					_003C_003E7__wrap3 = CaravanInventoryUtility.AllInventoryItems(_003Ccaravan_003E5__2).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0081;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0081;
				case 2:
					{
						_003C_003E1__state = -1;
						goto IL_0101;
					}
					IL_0081:
					if (_003C_003E7__wrap3.MoveNext())
					{
						Thing current = _003C_003E7__wrap3.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap3 = default(List<Thing>.Enumerator);
					_003Cpawns_003E5__3 = _003Ccaravan_003E5__2.PawnsListForReading;
					_003Ci_003E5__5 = 0;
					goto IL_0111;
					IL_0111:
					if (_003Ci_003E5__5 < _003Cpawns_003E5__3.Count)
					{
						if (!_003Ccaravan_003E5__2.IsOwner(_003Cpawns_003E5__3[_003Ci_003E5__5]))
						{
							_003C_003E2__current = _003Cpawns_003E5__3[_003Ci_003E5__5];
							_003C_003E1__state = 2;
							return true;
						}
						goto IL_0101;
					}
					return false;
					IL_0101:
					_003Ci_003E5__5++;
					goto IL_0111;
				}
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			((IDisposable)_003C_003E7__wrap3/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Thing> IEnumerable<Thing>.GetEnumerator()
		{
			_003CColonyThingsWillingToBuy_003Ed__32 _003CColonyThingsWillingToBuy_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CColonyThingsWillingToBuy_003Ed__ = this;
			}
			else
			{
				_003CColonyThingsWillingToBuy_003Ed__ = new _003CColonyThingsWillingToBuy_003Ed__32(0);
			}
			_003CColonyThingsWillingToBuy_003Ed__.playerNegotiator = _003C_003E3__playerNegotiator;
			return _003CColonyThingsWillingToBuy_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Thing>)this).GetEnumerator();
		}
	}

	public Settlement settlement;

	private ThingOwner<Thing> stock;

	private int lastStockGenerationTicks = -1;

	private bool everGeneratedStock;

	private const float DefaultTradePriceImprovement = 0.02f;

	private List<Pawn> tmpSavedPawns = new List<Pawn>();

	protected virtual int RegenerateStockEveryDays => 30;

	public IThingHolder ParentHolder => settlement;

	public bool ShouldTickContents => false;

	public List<Thing> StockListForReading
	{
		get
		{
			if (stock == null || stock.InnerListForReading.Empty())
			{
				RegenerateStock();
			}
			return stock.InnerListForReading;
		}
	}

	public TraderKindDef TraderKind
	{
		get
		{
			List<TraderKindDef> baseTraderKinds = settlement.Faction.def.baseTraderKinds;
			if (baseTraderKinds.NullOrEmpty())
			{
				return null;
			}
			int index = Mathf.Abs(settlement.HashOffset()) % baseTraderKinds.Count;
			return baseTraderKinds[index];
		}
	}

	public int RandomPriceFactorSeed => Gen.HashCombineInt(settlement.ID, 1933327354);

	public bool EverVisited => everGeneratedStock;

	public bool RestockedSinceLastVisit
	{
		get
		{
			if (everGeneratedStock)
			{
				return stock == null;
			}
			return false;
		}
	}

	public int NextRestockTick
	{
		get
		{
			if (stock == null || !everGeneratedStock)
			{
				return -1;
			}
			return ((lastStockGenerationTicks != -1) ? lastStockGenerationTicks : 0) + RegenerateStockEveryDays * 60000;
		}
	}

	public virtual string TraderName
	{
		get
		{
			if (settlement.Faction == null)
			{
				return settlement.LabelCap;
			}
			return "SettlementTrader".Translate(settlement.LabelCap, settlement.Faction.Name);
		}
	}

	public virtual bool CanTradeNow
	{
		get
		{
			if (TraderKind != null)
			{
				if (stock != null && !stock.InnerListForReading.Empty())
				{
					return stock.InnerListForReading.Any((Thing x) => TraderKind.WillTrade(x.def));
				}
				return true;
			}
			return false;
		}
	}

	public virtual float TradePriceImprovementOffsetForPlayer => 0.02f;

	public Settlement_TraderTracker(Settlement settlement)
	{
		this.settlement = settlement;
	}

	public virtual void ExposeData()
	{
		if (Scribe.mode == LoadSaveMode.Saving)
		{
			tmpSavedPawns.Clear();
			if (stock != null)
			{
				for (int num = stock.Count - 1; num >= 0; num--)
				{
					if (stock[num] is Pawn item)
					{
						stock.Remove(item);
						tmpSavedPawns.Add(item);
					}
				}
			}
		}
		Scribe_Collections.Look(ref tmpSavedPawns, "tmpSavedPawns", LookMode.Reference);
		Scribe_Deep.Look(ref stock, "stock");
		Scribe_Values.Look(ref lastStockGenerationTicks, "lastStockGenerationTicks", 0);
		Scribe_Values.Look(ref everGeneratedStock, "wasStockGeneratedYet", defaultValue: false);
		if (Scribe.mode == LoadSaveMode.PostLoadInit || Scribe.mode == LoadSaveMode.Saving)
		{
			for (int i = 0; i < tmpSavedPawns.Count; i++)
			{
				stock.TryAdd(tmpSavedPawns[i], canMergeWithExistingStacks: false);
			}
			tmpSavedPawns.Clear();
		}
	}

	[IteratorStateMachine(typeof(_003CColonyThingsWillingToBuy_003Ed__32))]
	public virtual IEnumerable<Thing> ColonyThingsWillingToBuy(Pawn playerNegotiator)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CColonyThingsWillingToBuy_003Ed__32(-2)
		{
			_003C_003E3__playerNegotiator = playerNegotiator
		};
	}

	public virtual void GiveSoldThingToTrader(Thing toGive, int countToGive, Pawn playerNegotiator)
	{
		if (stock == null)
		{
			RegenerateStock();
		}
		Caravan caravan = playerNegotiator.GetCaravan();
		Thing thing = toGive.SplitOff(countToGive);
		thing.PreTraded(TradeAction.PlayerSells, playerNegotiator, settlement);
		if (toGive is Pawn pawn)
		{
			CaravanInventoryUtility.MoveAllInventoryToSomeoneElse(pawn, caravan.PawnsListForReading);
			if (!pawn.RaceProps.Humanlike && !stock.TryAdd(pawn, canMergeWithExistingStacks: false))
			{
				pawn.Destroy();
			}
		}
		else if (!stock.TryAdd(thing, canMergeWithExistingStacks: false))
		{
			thing.Destroy();
		}
	}

	public virtual void GiveSoldThingToPlayer(Thing toGive, int countToGive, Pawn playerNegotiator)
	{
		Caravan caravan = playerNegotiator.GetCaravan();
		Thing thing = toGive.SplitOff(countToGive);
		thing.PreTraded(TradeAction.PlayerBuys, playerNegotiator, settlement);
		if (thing is Pawn p)
		{
			caravan.AddPawn(p, addCarriedPawnToWorldPawnsIfAny: true);
			return;
		}
		Pawn pawn = CaravanInventoryUtility.FindPawnToMoveInventoryTo(thing, caravan.PawnsListForReading, null);
		if (pawn == null)
		{
			Log.Error("Could not find any pawn to give sold thing to.");
			thing.Destroy();
		}
		else if (!pawn.inventory.innerContainer.TryAdd(thing))
		{
			Log.Error("Could not add sold thing to inventory.");
			thing.Destroy();
		}
	}

	public virtual void TraderTrackerTick()
	{
		if (stock == null)
		{
			return;
		}
		if (Find.TickManager.TicksGame - lastStockGenerationTicks > RegenerateStockEveryDays * 60000)
		{
			TryDestroyStock();
			return;
		}
		for (int num = stock.Count - 1; num >= 0; num--)
		{
			if (stock[num] is Pawn { Destroyed: not false } pawn)
			{
				stock.Remove(pawn);
			}
		}
		for (int num2 = stock.Count - 1; num2 >= 0; num2--)
		{
			if (stock[num2] is Pawn pawn2 && !pawn2.IsWorldPawn())
			{
				Log.Error("Faction base has non-world-pawns in its stock. Removing...");
				stock.Remove(pawn2);
			}
		}
	}

	public void TryDestroyStock()
	{
		if (stock == null)
		{
			return;
		}
		for (int num = stock.Count - 1; num >= 0; num--)
		{
			Thing thing = stock[num];
			stock.Remove(thing);
			if (!(thing is Pawn) && !thing.Destroyed)
			{
				thing.Destroy();
			}
		}
		stock = null;
	}

	public bool ContainsPawn(Pawn p)
	{
		if (stock != null)
		{
			return stock.Contains(p);
		}
		return false;
	}

	protected virtual void RegenerateStock()
	{
		TryDestroyStock();
		stock = new ThingOwner<Thing>(this)
		{
			dontTickContents = true
		};
		everGeneratedStock = true;
		if (settlement.Faction == null || !settlement.Faction.IsPlayer)
		{
			ThingSetMakerParams parms = default(ThingSetMakerParams);
			parms.traderDef = TraderKind;
			parms.tile = settlement.Tile;
			parms.makingFaction = settlement.Faction;
			stock.TryAddRangeOrTransfer(ThingSetMakerDefOf.TraderStock.root.Generate(parms));
		}
		for (int i = 0; i < stock.Count; i++)
		{
			if (stock[i] is Pawn pawn)
			{
				Find.WorldPawns.PassToWorld(pawn);
			}
		}
		lastStockGenerationTicks = Find.TickManager.TicksGame;
	}

	public ThingOwner GetDirectlyHeldThings()
	{
		return stock;
	}

	public void GetChildHolders(List<IThingHolder> outChildren)
	{
		ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, GetDirectlyHeldThings());
	}
}
