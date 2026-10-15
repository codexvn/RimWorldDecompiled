using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld.Planet;

public class Caravan_TraderTracker : IExposable
{
	[CompilerGenerated]
	private sealed class _003CColonyThingsWillingToBuy_003Ed__14 : IEnumerable<Thing>, IEnumerable, IEnumerator<Thing>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Thing _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Pawn playerNegotiator;

		public Pawn _003C_003E3__playerNegotiator;

		private Caravan _003CplayerCaravan_003E5__2;

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
		public _003CColonyThingsWillingToBuy_003Ed__14(int _003C_003E1__state)
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
			_003CplayerCaravan_003E5__2 = null;
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
					_003CplayerCaravan_003E5__2 = playerNegotiator.GetCaravan();
					_003C_003E7__wrap3 = CaravanInventoryUtility.AllInventoryItems(_003CplayerCaravan_003E5__2).GetEnumerator();
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
					_003Cpawns_003E5__3 = _003CplayerCaravan_003E5__2.PawnsListForReading;
					_003Ci_003E5__5 = 0;
					goto IL_0111;
					IL_0111:
					if (_003Ci_003E5__5 < _003Cpawns_003E5__3.Count)
					{
						if (!_003CplayerCaravan_003E5__2.IsOwner(_003Cpawns_003E5__3[_003Ci_003E5__5]))
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
			_003CColonyThingsWillingToBuy_003Ed__14 _003CColonyThingsWillingToBuy_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CColonyThingsWillingToBuy_003Ed__ = this;
			}
			else
			{
				_003CColonyThingsWillingToBuy_003Ed__ = new _003CColonyThingsWillingToBuy_003Ed__14(0);
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

	[CompilerGenerated]
	private sealed class _003Cget_Goods_003Ed__5 : IEnumerable<Thing>, IEnumerable, IEnumerator<Thing>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Thing _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Caravan_TraderTracker _003C_003E4__this;

		private List<Thing> _003Cinv_003E5__2;

		private List<Pawn> _003Cpawns_003E5__3;

		private int _003Ci_003E5__4;

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
		public _003Cget_Goods_003Ed__5(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003Cinv_003E5__2 = null;
			_003Cpawns_003E5__3 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			Caravan_TraderTracker caravan_TraderTracker = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003Cinv_003E5__2 = CaravanInventoryUtility.AllInventoryItems(caravan_TraderTracker.caravan);
				_003Ci_003E5__4 = 0;
				goto IL_007a;
			case 1:
				_003C_003E1__state = -1;
				_003Ci_003E5__4++;
				goto IL_007a;
			case 2:
				{
					_003C_003E1__state = -1;
					goto IL_0114;
				}
				IL_007a:
				if (_003Ci_003E5__4 < _003Cinv_003E5__2.Count)
				{
					_003C_003E2__current = _003Cinv_003E5__2[_003Ci_003E5__4];
					_003C_003E1__state = 1;
					return true;
				}
				_003Cpawns_003E5__3 = caravan_TraderTracker.caravan.PawnsListForReading;
				_003Ci_003E5__4 = 0;
				goto IL_0124;
				IL_0124:
				if (_003Ci_003E5__4 < _003Cpawns_003E5__3.Count)
				{
					Pawn pawn = _003Cpawns_003E5__3[_003Ci_003E5__4];
					if (!caravan_TraderTracker.caravan.IsOwner(pawn) && (!pawn.RaceProps.packAnimal || pawn.inventory == null || pawn.inventory.innerContainer.Count <= 0) && !caravan_TraderTracker.soldPrisoners.Contains(pawn))
					{
						_003C_003E2__current = pawn;
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_0114;
				}
				return false;
				IL_0114:
				_003Ci_003E5__4++;
				goto IL_0124;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Thing> IEnumerable<Thing>.GetEnumerator()
		{
			_003Cget_Goods_003Ed__5 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_Goods_003Ed__5(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Thing>)this).GetEnumerator();
		}
	}

	private Caravan caravan;

	private List<Pawn> soldPrisoners = new List<Pawn>();

	public TraderKindDef TraderKind
	{
		get
		{
			List<Pawn> pawnsListForReading = caravan.PawnsListForReading;
			for (int i = 0; i < pawnsListForReading.Count; i++)
			{
				Pawn pawn = pawnsListForReading[i];
				if (caravan.IsOwner(pawn) && pawn.TraderKind != null)
				{
					return pawn.TraderKind;
				}
			}
			return null;
		}
	}

	public IEnumerable<Thing> Goods
	{
		[IteratorStateMachine(typeof(_003Cget_Goods_003Ed__5))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_Goods_003Ed__5(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public int RandomPriceFactorSeed => Gen.HashCombineInt(caravan.ID, 1048142365);

	public string TraderName => caravan.LabelCap;

	public bool CanTradeNow
	{
		get
		{
			if (TraderKind != null && !caravan.AllOwnersDowned && caravan.Faction != Faction.OfPlayer)
			{
				return Goods.Any((Thing x) => TraderKind.WillTrade(x.def));
			}
			return false;
		}
	}

	public Caravan_TraderTracker(Caravan caravan)
	{
		this.caravan = caravan;
	}

	public void ExposeData()
	{
		Scribe_Collections.Look(ref soldPrisoners, "soldPrisoners", LookMode.Reference);
		if (Scribe.mode == LoadSaveMode.PostLoadInit)
		{
			soldPrisoners.RemoveAll((Pawn x) => x == null);
		}
	}

	[IteratorStateMachine(typeof(_003CColonyThingsWillingToBuy_003Ed__14))]
	public IEnumerable<Thing> ColonyThingsWillingToBuy(Pawn playerNegotiator)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CColonyThingsWillingToBuy_003Ed__14(-2)
		{
			_003C_003E3__playerNegotiator = playerNegotiator
		};
	}

	public void GiveSoldThingToTrader(Thing toGive, int countToGive, Pawn playerNegotiator)
	{
		if (Goods.Contains(toGive))
		{
			Log.Error("Tried to add " + toGive?.ToString() + " to stock (pawn's trader tracker), but it's already here.");
			return;
		}
		Caravan caravan = playerNegotiator.GetCaravan();
		Thing thing = toGive.SplitOff(countToGive);
		thing.PreTraded(TradeAction.PlayerSells, playerNegotiator, this.caravan);
		if (thing is Pawn pawn)
		{
			CaravanInventoryUtility.MoveAllInventoryToSomeoneElse(pawn, caravan.PawnsListForReading);
			this.caravan.AddPawn(pawn, addCarriedPawnToWorldPawnsIfAny: false);
			if (pawn.IsWorldPawn() && !this.caravan.Spawned)
			{
				Find.WorldPawns.RemovePawn(pawn);
			}
			if (pawn.RaceProps.Humanlike)
			{
				soldPrisoners.Add(pawn);
			}
		}
		else
		{
			Pawn pawn2 = CaravanInventoryUtility.FindPawnToMoveInventoryTo(thing, this.caravan.PawnsListForReading, null);
			if (pawn2 == null)
			{
				Log.Error("Could not find pawn to move sold thing to (sold by player). thing=" + thing);
				thing.Destroy();
			}
			else if (!pawn2.inventory.innerContainer.TryAdd(thing))
			{
				Log.Error("Could not add item to inventory.");
				thing.Destroy();
			}
		}
	}

	public void GiveSoldThingToPlayer(Thing toGive, int countToGive, Pawn playerNegotiator)
	{
		Caravan caravan = playerNegotiator.GetCaravan();
		Thing thing = toGive.SplitOff(countToGive);
		thing.PreTraded(TradeAction.PlayerBuys, playerNegotiator, this.caravan);
		if (thing is Pawn pawn)
		{
			CaravanInventoryUtility.MoveAllInventoryToSomeoneElse(pawn, this.caravan.PawnsListForReading);
			caravan.AddPawn(pawn, addCarriedPawnToWorldPawnsIfAny: true);
			if (!pawn.IsWorldPawn() && caravan.Spawned)
			{
				Find.WorldPawns.PassToWorld(pawn);
			}
			soldPrisoners.Remove(pawn);
			return;
		}
		Pawn pawn2 = CaravanInventoryUtility.FindPawnToMoveInventoryTo(thing, caravan.PawnsListForReading, null);
		if (pawn2 == null)
		{
			Log.Error("Could not find pawn to move bought thing to (bought by player). thing=" + thing);
			thing.Destroy();
		}
		else if (!pawn2.inventory.innerContainer.TryAdd(thing))
		{
			Log.Error("Could not add item to inventory.");
			thing.Destroy();
		}
	}
}
