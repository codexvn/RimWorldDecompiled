using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld;

public class Pawn_TraderTracker : IExposable
{
	[CompilerGenerated]
	private sealed class _003CColonyThingsWillingToBuy_003Ed__13 : IEnumerable<Thing>, IEnumerable, IEnumerator<Thing>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Thing _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Pawn_TraderTracker _003C_003E4__this;

		private IEnumerator<Thing> _003C_003E7__wrap1;

		private List<Building>.Enumerator _003C_003E7__wrap2;

		private List<Genepack>.Enumerator _003C_003E7__wrap3;

		private IEnumerator<IHaulSource> _003C_003E7__wrap4;

		private IEnumerator<Pawn> _003C_003E7__wrap5;

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
		public _003CColonyThingsWillingToBuy_003Ed__13(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			switch (num)
			{
			case -3:
			case 1:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
				break;
			case -5:
			case -4:
			case 2:
				try
				{
					if (num == -5 || num == 2)
					{
						try
						{
						}
						finally
						{
							_003C_003Em__Finally3();
						}
					}
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			case -7:
			case -6:
			case 3:
				try
				{
					if (num == -7 || num == 3)
					{
						try
						{
						}
						finally
						{
							_003C_003Em__Finally5();
						}
					}
				}
				finally
				{
					_003C_003Em__Finally4();
				}
				break;
			case -8:
			case 4:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally6();
				}
				break;
			}
			_003C_003E7__wrap1 = null;
			_003C_003E7__wrap2 = default(List<Building>.Enumerator);
			_003C_003E7__wrap3 = default(List<Genepack>.Enumerator);
			_003C_003E7__wrap4 = null;
			_003C_003E7__wrap5 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				Pawn_TraderTracker CS_0024_003C_003E8__locals11 = _003C_003E4__this;
				IHaulSource current2;
				CompGenepackContainer compGenepackContainer;
				List<Genepack> containedGenepacks;
				IEnumerable<IHaulSource> enumerable2;
				switch (num)
				{
				default:
					return false;
				case 0:
				{
					_003C_003E1__state = -1;
					IEnumerable<Thing> enumerable = CS_0024_003C_003E8__locals11.pawn.Map.listerThings.AllThings.Where((Thing x) => x.def.category == ThingCategory.Item && TradeUtility.PlayerSellableNow(x, CS_0024_003C_003E8__locals11.pawn) && !x.Position.Fogged(x.Map) && (CS_0024_003C_003E8__locals11.pawn.Map.areaManager.Home[x.Position] || x.IsInAnyStorage()) && CS_0024_003C_003E8__locals11.ReachableForTrade(x));
					_003C_003E7__wrap1 = enumerable.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_009e;
				}
				case 1:
					_003C_003E1__state = -3;
					goto IL_009e;
				case 2:
					_003C_003E1__state = -5;
					goto IL_0167;
				case 3:
					_003C_003E1__state = -7;
					goto IL_0240;
				case 4:
					{
						_003C_003E1__state = -8;
						goto IL_02e6;
					}
					IL_009e:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Thing current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (ModsConfig.BiotechActive)
					{
						List<Building> list = CS_0024_003C_003E8__locals11.pawn.Map.listerBuildings.AllBuildingsColonistOfDef(ThingDefOf.GeneBank);
						_003C_003E7__wrap2 = list.GetEnumerator();
						_003C_003E1__state = -4;
						goto IL_0186;
					}
					goto IL_01a8;
					IL_025a:
					while (_003C_003E7__wrap4.MoveNext())
					{
						current2 = _003C_003E7__wrap4.Current;
						Building thing = (Building)current2;
						if (!CS_0024_003C_003E8__locals11.ReachableForTrade(thing))
						{
							continue;
						}
						goto IL_01f9;
					}
					_003C_003Em__Finally4();
					_003C_003E7__wrap4 = null;
					if (CS_0024_003C_003E8__locals11.pawn.GetLord() == null)
					{
						break;
					}
					_003C_003E7__wrap5 = (from x in TradeUtility.AllSellableColonyPawns(CS_0024_003C_003E8__locals11.pawn.Map)
						where !x.Downed && CS_0024_003C_003E8__locals11.ReachableForTrade(x)
						select x).GetEnumerator();
					_003C_003E1__state = -8;
					goto IL_02e6;
					IL_0240:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Thing current3 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current3;
						_003C_003E1__state = 3;
						return true;
					}
					_003C_003Em__Finally5();
					_003C_003E7__wrap1 = null;
					goto IL_025a;
					IL_0186:
					while (_003C_003E7__wrap2.MoveNext())
					{
						Building current4 = _003C_003E7__wrap2.Current;
						if (!CS_0024_003C_003E8__locals11.ReachableForTrade(current4))
						{
							continue;
						}
						compGenepackContainer = current4.TryGetComp<CompGenepackContainer>();
						if (compGenepackContainer == null)
						{
							continue;
						}
						goto IL_011c;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap2 = default(List<Building>.Enumerator);
					goto IL_01a8;
					IL_02e6:
					if (_003C_003E7__wrap5.MoveNext())
					{
						Pawn current5 = _003C_003E7__wrap5.Current;
						_003C_003E2__current = current5;
						_003C_003E1__state = 4;
						return true;
					}
					_003C_003Em__Finally6();
					_003C_003E7__wrap5 = null;
					break;
					IL_011c:
					containedGenepacks = compGenepackContainer.ContainedGenepacks;
					_003C_003E7__wrap3 = containedGenepacks.GetEnumerator();
					_003C_003E1__state = -5;
					goto IL_0167;
					IL_0167:
					if (_003C_003E7__wrap3.MoveNext())
					{
						Genepack current6 = _003C_003E7__wrap3.Current;
						_003C_003E2__current = current6;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally3();
					_003C_003E7__wrap3 = default(List<Genepack>.Enumerator);
					goto IL_0186;
					IL_01f9:
					_003C_003E7__wrap1 = ((IEnumerable<Thing>)current2.GetDirectlyHeldThings()).GetEnumerator();
					_003C_003E1__state = -7;
					goto IL_0240;
					IL_01a8:
					enumerable2 = CS_0024_003C_003E8__locals11.pawn.Map.listerBuildings.AllColonistBuildingsOfType<IHaulSource>();
					_003C_003E7__wrap4 = enumerable2.GetEnumerator();
					_003C_003E1__state = -6;
					goto IL_025a;
				}
				return false;
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
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -1;
			((IDisposable)_003C_003E7__wrap2/*cast due to .constrained prefix*/).Dispose();
		}

		private void _003C_003Em__Finally3()
		{
			_003C_003E1__state = -4;
			((IDisposable)_003C_003E7__wrap3/*cast due to .constrained prefix*/).Dispose();
		}

		private void _003C_003Em__Finally4()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap4 != null)
			{
				_003C_003E7__wrap4.Dispose();
			}
		}

		private void _003C_003Em__Finally5()
		{
			_003C_003E1__state = -6;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		private void _003C_003Em__Finally6()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap5 != null)
			{
				_003C_003E7__wrap5.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Thing> IEnumerable<Thing>.GetEnumerator()
		{
			_003CColonyThingsWillingToBuy_003Ed__13 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CColonyThingsWillingToBuy_003Ed__13(0)
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

	[CompilerGenerated]
	private sealed class _003Cget_Goods_003Ed__4 : IEnumerable<Thing>, IEnumerable, IEnumerator<Thing>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Thing _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Pawn_TraderTracker _003C_003E4__this;

		private Lord _003Clord_003E5__2;

		private int _003Ci_003E5__3;

		private Pawn _003Cp_003E5__4;

		private int _003Cj_003E5__5;

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
		public _003Cget_Goods_003Ed__4(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003Clord_003E5__2 = null;
			_003Cp_003E5__4 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			Pawn_TraderTracker pawn_TraderTracker = _003C_003E4__this;
			TraderCaravanRole traderCaravanRole;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003Clord_003E5__2 = pawn_TraderTracker.pawn.GetLord();
				if (_003Clord_003E5__2 == null || !(_003Clord_003E5__2.LordJob is LordJob_TradeWithColony))
				{
					_003Ci_003E5__3 = 0;
					goto IL_00bd;
				}
				goto IL_00da;
			case 1:
				_003C_003E1__state = -1;
				goto IL_00ad;
			case 2:
				_003C_003E1__state = -1;
				_003Cj_003E5__5++;
				goto IL_0169;
			case 3:
				{
					_003C_003E1__state = -1;
					goto IL_01bc;
				}
				IL_0169:
				if (_003Cj_003E5__5 < _003Cp_003E5__4.inventory.innerContainer.Count)
				{
					_003C_003E2__current = _003Cp_003E5__4.inventory.innerContainer[_003Cj_003E5__5];
					_003C_003E1__state = 2;
					return true;
				}
				goto IL_01bc;
				IL_01bc:
				_003Cp_003E5__4 = null;
				_003Ci_003E5__3++;
				goto IL_01d3;
				IL_00bd:
				if (_003Ci_003E5__3 < pawn_TraderTracker.pawn.inventory.innerContainer.Count)
				{
					Thing item = pawn_TraderTracker.pawn.inventory.innerContainer[_003Ci_003E5__3];
					if (!pawn_TraderTracker.pawn.inventory.NotForSale(item))
					{
						_003C_003E2__current = item;
						_003C_003E1__state = 1;
						return true;
					}
					goto IL_00ad;
				}
				goto IL_00da;
				IL_00da:
				if (_003Clord_003E5__2 == null)
				{
					break;
				}
				_003Ci_003E5__3 = 0;
				goto IL_01d3;
				IL_01d3:
				if (_003Ci_003E5__3 >= _003Clord_003E5__2.ownedPawns.Count)
				{
					break;
				}
				_003Cp_003E5__4 = _003Clord_003E5__2.ownedPawns[_003Ci_003E5__3];
				traderCaravanRole = _003Cp_003E5__4.GetTraderCaravanRole();
				if (traderCaravanRole == TraderCaravanRole.Carrier)
				{
					_003Cj_003E5__5 = 0;
					goto IL_0169;
				}
				if (traderCaravanRole == TraderCaravanRole.Chattel && !pawn_TraderTracker.soldPrisoners.Contains(_003Cp_003E5__4))
				{
					_003C_003E2__current = _003Cp_003E5__4;
					_003C_003E1__state = 3;
					return true;
				}
				goto IL_01bc;
				IL_00ad:
				_003Ci_003E5__3++;
				goto IL_00bd;
			}
			return false;
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
			_003Cget_Goods_003Ed__4 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_Goods_003Ed__4(0)
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

	private Pawn pawn;

	public TraderKindDef traderKind;

	private List<Pawn> soldPrisoners = new List<Pawn>();

	public IEnumerable<Thing> Goods
	{
		[IteratorStateMachine(typeof(_003Cget_Goods_003Ed__4))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_Goods_003Ed__4(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public int RandomPriceFactorSeed => Gen.HashCombineInt(pawn.thingIDNumber, 1149275593);

	public string TraderName => pawn.LabelShort;

	public bool CanTradeNow
	{
		get
		{
			if (!pawn.Dead && pawn.Spawned && pawn.mindState.wantsToTradeWithColony && pawn.CanCasuallyInteractNow() && !pawn.Downed && !pawn.IsPrisoner && pawn.Faction != Faction.OfPlayer && (pawn.Faction == null || !pawn.Faction.HostileTo(Faction.OfPlayer)))
			{
				if (!Goods.Any((Thing x) => traderKind.WillTrade(x.def)))
				{
					return traderKind.tradeCurrency == TradeCurrency.Favor;
				}
				return true;
			}
			return false;
		}
	}

	public Pawn_TraderTracker(Pawn pawn)
	{
		this.pawn = pawn;
	}

	public void ExposeData()
	{
		Scribe_Defs.Look(ref traderKind, "traderKind");
		Scribe_Collections.Look(ref soldPrisoners, "soldPrisoners", LookMode.Reference);
		if (Scribe.mode == LoadSaveMode.PostLoadInit)
		{
			soldPrisoners.RemoveAll((Pawn x) => x == null);
		}
	}

	[IteratorStateMachine(typeof(_003CColonyThingsWillingToBuy_003Ed__13))]
	public IEnumerable<Thing> ColonyThingsWillingToBuy(Pawn playerNegotiator)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CColonyThingsWillingToBuy_003Ed__13(-2)
		{
			_003C_003E4__this = this
		};
	}

	public void GiveSoldThingToTrader(Thing toGive, int countToGive, Pawn playerNegotiator)
	{
		if (Goods.Contains(toGive))
		{
			Log.Error("Tried to add " + toGive?.ToString() + " to stock (pawn's trader tracker), but it's already here.");
			return;
		}
		if (toGive is Pawn pawn)
		{
			pawn.PreTraded(TradeAction.PlayerSells, playerNegotiator, this.pawn);
			AddPawnToStock(pawn);
			return;
		}
		Thing thing = toGive.SplitOff(countToGive);
		thing.PreTraded(TradeAction.PlayerSells, playerNegotiator, this.pawn);
		Thing thing2 = TradeUtility.ThingFromStockToMergeWith(this.pawn, thing);
		if (thing2 != null)
		{
			if (!thing2.TryAbsorbStack(thing, respectStackLimit: false))
			{
				thing.Destroy();
			}
		}
		else
		{
			AddThingToRandomInventory(thing);
		}
	}

	public void GiveSoldThingToPlayer(Thing toGive, int countToGive, Pawn playerNegotiator)
	{
		if (toGive is Pawn pawn)
		{
			pawn.PreTraded(TradeAction.PlayerBuys, playerNegotiator, this.pawn);
			pawn.GetLord()?.Notify_PawnLost(pawn, PawnLostCondition.Undefined, null);
			if (soldPrisoners.Contains(pawn))
			{
				soldPrisoners.Remove(pawn);
			}
			return;
		}
		IntVec3 positionHeld = toGive.PositionHeld;
		Map mapHeld = toGive.MapHeld;
		Thing thing = toGive.SplitOff(countToGive);
		thing.PreTraded(TradeAction.PlayerBuys, playerNegotiator, this.pawn);
		if (GenPlace.TryPlaceThing(thing, positionHeld, mapHeld, ThingPlaceMode.Near, null, null, null))
		{
			this.pawn.GetLord()?.extraForbiddenThings.Add(thing);
			return;
		}
		string obj = thing?.ToString();
		IntVec3 intVec = positionHeld;
		Log.Error("Could not place bought thing " + obj + " at " + intVec.ToString());
		thing.Destroy();
	}

	private void AddPawnToStock(Pawn newPawn)
	{
		if (!newPawn.Spawned)
		{
			GenSpawn.Spawn(newPawn, pawn.Position, pawn.Map);
		}
		if (newPawn.Faction != pawn.Faction)
		{
			newPawn.SetFaction(pawn.Faction);
		}
		if (newPawn.RaceProps.Humanlike)
		{
			newPawn.kindDef = PawnKindDefOf.Slave;
		}
		Lord lord = pawn.GetLord();
		if (lord == null)
		{
			newPawn.Destroy();
			Log.Error("Tried to sell pawn " + newPawn?.ToString() + " to " + pawn?.ToString() + ", but " + pawn?.ToString() + " has no lord. Traders without lord can't buy pawns.");
		}
		else
		{
			if (newPawn.RaceProps.Humanlike)
			{
				soldPrisoners.Add(newPawn);
			}
			lord.AddPawn(newPawn);
		}
	}

	private void AddThingToRandomInventory(Thing thing)
	{
		Lord lord = pawn.GetLord();
		IEnumerable<Pawn> source = Enumerable.Empty<Pawn>();
		if (lord != null)
		{
			source = lord.ownedPawns.Where((Pawn x) => x.GetTraderCaravanRole() == TraderCaravanRole.Carrier);
		}
		if (source.Any())
		{
			if (!source.RandomElement().inventory.innerContainer.TryAdd(thing))
			{
				thing.Destroy();
			}
		}
		else if (!pawn.inventory.innerContainer.TryAdd(thing))
		{
			thing.Destroy();
		}
	}

	private bool ReachableForTrade(Thing thing)
	{
		Thing thing2 = thing;
		if (HaulAIUtility.IsInHaulableInventory(thing))
		{
			thing2 = thing.SpawnedParentOrMe;
		}
		if (pawn.Map != thing2.MapHeld)
		{
			return false;
		}
		return pawn.Map.reachability.CanReach(pawn.Position, thing2, PathEndMode.Touch, TraverseMode.PassDoors, Danger.Some);
	}
}
