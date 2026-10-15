using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using RimWorld.QuestGen;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld;

public static class TradeUtility
{
	[CompilerGenerated]
	private sealed class _003CAllLaunchableThingsForTrade_003Ed__7 : IEnumerable<Thing>, IEnumerable, IEnumerator<Thing>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Thing _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Map map;

		public Map _003C_003E3__map;

		private ITrader trader;

		public ITrader _003C_003E3__trader;

		private HashSet<Thing> _003CyieldedThings_003E5__2;

		private IEnumerator<Building_OrbitalTradeBeacon> _003C_003E7__wrap2;

		private IEnumerator<IntVec3> _003C_003E7__wrap3;

		private List<Thing> _003CthingList_003E5__5;

		private int _003Ci_003E5__6;

		private Thing _003Ct_003E5__7;

		private List<Genepack>.Enumerator _003C_003E7__wrap7;

		private IEnumerator<Book> _003C_003E7__wrap8;

		private IEnumerator<Thing> _003C_003E7__wrap9;

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
		public _003CAllLaunchableThingsForTrade_003Ed__7(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if ((uint)(num - -7) <= 4u || (uint)(num - 1) <= 3u)
			{
				try
				{
					if ((uint)(num - -7) <= 3u || (uint)(num - 1) <= 3u)
					{
						try
						{
							switch (num)
							{
							case -5:
							case 1:
								try
								{
								}
								finally
								{
									_003C_003Em__Finally3();
								}
								break;
							case -6:
							case 2:
								try
								{
								}
								finally
								{
									_003C_003Em__Finally4();
								}
								break;
							case -7:
							case 3:
								try
								{
								}
								finally
								{
									_003C_003Em__Finally5();
								}
								break;
							case -4:
							case -3:
							case -2:
							case -1:
							case 0:
								break;
							}
						}
						finally
						{
							_003C_003Em__Finally2();
						}
					}
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003CyieldedThings_003E5__2 = null;
			_003C_003E7__wrap2 = null;
			_003C_003E7__wrap3 = null;
			_003CthingList_003E5__5 = null;
			_003Ct_003E5__7 = null;
			_003C_003E7__wrap7 = default(List<Genepack>.Enumerator);
			_003C_003E7__wrap8 = null;
			_003C_003E7__wrap9 = null;
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
					_003CyieldedThings_003E5__2 = new HashSet<Thing>();
					_003C_003E7__wrap2 = Building_OrbitalTradeBeacon.AllPowered(map).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_03ab;
				case 1:
					_003C_003E1__state = -5;
					goto IL_0176;
				case 2:
					_003C_003E1__state = -6;
					goto IL_0225;
				case 3:
					_003C_003E1__state = -7;
					goto IL_02cf;
				case 4:
					{
						_003C_003E1__state = -4;
						goto IL_0358;
					}
					IL_03ab:
					if (_003C_003E7__wrap2.MoveNext())
					{
						Building_OrbitalTradeBeacon current = _003C_003E7__wrap2.Current;
						_003C_003E7__wrap3 = current.TradeableCells.GetEnumerator();
						_003C_003E1__state = -4;
						goto IL_038e;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap2 = null;
					return false;
					IL_02cf:
					while (_003C_003E7__wrap9.MoveNext())
					{
						Thing current2 = _003C_003E7__wrap9.Current;
						if (PlayerSellableNow(_003Ct_003E5__7, trader) && !_003CyieldedThings_003E5__2.Contains(current2))
						{
							_003CyieldedThings_003E5__2.Add(current2);
							_003C_003E2__current = current2;
							_003C_003E1__state = 3;
							return true;
						}
					}
					_003C_003Em__Finally5();
					_003C_003E7__wrap9 = null;
					goto IL_0358;
					IL_0358:
					_003Ct_003E5__7 = null;
					_003Ci_003E5__6++;
					goto IL_0371;
					IL_038e:
					if (_003C_003E7__wrap3.MoveNext())
					{
						IntVec3 current3 = _003C_003E7__wrap3.Current;
						_003CthingList_003E5__5 = current3.GetThingList(map);
						_003Ci_003E5__6 = 0;
						goto IL_0371;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap3 = null;
					goto IL_03ab;
					IL_0225:
					while (_003C_003E7__wrap8.MoveNext())
					{
						Book current4 = _003C_003E7__wrap8.Current;
						if (PlayerSellableNow(_003Ct_003E5__7, trader) && !_003CyieldedThings_003E5__2.Contains(current4))
						{
							_003CyieldedThings_003E5__2.Add(current4);
							_003C_003E2__current = current4;
							_003C_003E1__state = 2;
							return true;
						}
					}
					_003C_003Em__Finally4();
					_003C_003E7__wrap8 = null;
					goto IL_0358;
					IL_0371:
					if (_003Ci_003E5__6 < _003CthingList_003E5__5.Count)
					{
						_003Ct_003E5__7 = _003CthingList_003E5__5[_003Ci_003E5__6];
						if (ModsConfig.BiotechActive && _003Ct_003E5__7.def == ThingDefOf.GeneBank)
						{
							CompGenepackContainer compGenepackContainer = _003Ct_003E5__7.TryGetComp<CompGenepackContainer>();
							if (compGenepackContainer != null)
							{
								List<Genepack> containedGenepacks = compGenepackContainer.ContainedGenepacks;
								_003C_003E7__wrap7 = containedGenepacks.GetEnumerator();
								_003C_003E1__state = -5;
								goto IL_0176;
							}
						}
						else
						{
							if (_003Ct_003E5__7 is Building_Bookcase building_Bookcase)
							{
								_003C_003E7__wrap8 = building_Bookcase.HeldBooks.GetEnumerator();
								_003C_003E1__state = -6;
								goto IL_0225;
							}
							if (_003Ct_003E5__7 is Building_OutfitStand building_OutfitStand)
							{
								_003C_003E7__wrap9 = building_OutfitStand.HeldItems.GetEnumerator();
								_003C_003E1__state = -7;
								goto IL_02cf;
							}
							if (_003Ct_003E5__7.def.category == ThingCategory.Item && PlayerSellableNow(_003Ct_003E5__7, trader) && !_003CyieldedThings_003E5__2.Contains(_003Ct_003E5__7))
							{
								_003CyieldedThings_003E5__2.Add(_003Ct_003E5__7);
								_003C_003E2__current = _003Ct_003E5__7;
								_003C_003E1__state = 4;
								return true;
							}
						}
						goto IL_0358;
					}
					_003CthingList_003E5__5 = null;
					goto IL_038e;
					IL_0176:
					while (_003C_003E7__wrap7.MoveNext())
					{
						Genepack current5 = _003C_003E7__wrap7.Current;
						if (PlayerSellableNow(_003Ct_003E5__7, trader) && !_003CyieldedThings_003E5__2.Contains(current5))
						{
							_003CyieldedThings_003E5__2.Add(current5);
							_003C_003E2__current = current5;
							_003C_003E1__state = 1;
							return true;
						}
					}
					_003C_003Em__Finally3();
					_003C_003E7__wrap7 = default(List<Genepack>.Enumerator);
					goto IL_0358;
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
			if (_003C_003E7__wrap2 != null)
			{
				_003C_003E7__wrap2.Dispose();
			}
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -3;
			if (_003C_003E7__wrap3 != null)
			{
				_003C_003E7__wrap3.Dispose();
			}
		}

		private void _003C_003Em__Finally3()
		{
			_003C_003E1__state = -4;
			((IDisposable)_003C_003E7__wrap7/*cast due to .constrained prefix*/).Dispose();
		}

		private void _003C_003Em__Finally4()
		{
			_003C_003E1__state = -4;
			if (_003C_003E7__wrap8 != null)
			{
				_003C_003E7__wrap8.Dispose();
			}
		}

		private void _003C_003Em__Finally5()
		{
			_003C_003E1__state = -4;
			if (_003C_003E7__wrap9 != null)
			{
				_003C_003E7__wrap9.Dispose();
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
			_003CAllLaunchableThingsForTrade_003Ed__7 _003CAllLaunchableThingsForTrade_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CAllLaunchableThingsForTrade_003Ed__ = this;
			}
			else
			{
				_003CAllLaunchableThingsForTrade_003Ed__ = new _003CAllLaunchableThingsForTrade_003Ed__7(0);
			}
			_003CAllLaunchableThingsForTrade_003Ed__.map = _003C_003E3__map;
			_003CAllLaunchableThingsForTrade_003Ed__.trader = _003C_003E3__trader;
			return _003CAllLaunchableThingsForTrade_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Thing>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CAllSellableColonyPawns_003Ed__8 : IEnumerable<Pawn>, IEnumerable, IEnumerator<Pawn>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Pawn _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Map map;

		public Map _003C_003E3__map;

		private bool checkAcceptableTemperatureOfAnimals;

		public bool _003C_003E3__checkAcceptableTemperatureOfAnimals;

		private List<Pawn>.Enumerator _003C_003E7__wrap1;

		Pawn IEnumerator<Pawn>.Current
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
		public _003CAllSellableColonyPawns_003Ed__8(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			switch (_003C_003E1__state)
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
			case -4:
			case 2:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			case -5:
			case 3:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally3();
				}
				break;
			}
			_003C_003E7__wrap1 = default(List<Pawn>.Enumerator);
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
					_003C_003E7__wrap1 = map.mapPawns.PrisonersOfColonySpawned.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0086;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0086;
				case 2:
					_003C_003E1__state = -4;
					goto IL_0100;
				case 3:
					{
						_003C_003E1__state = -5;
						break;
					}
					IL_0086:
					while (_003C_003E7__wrap1.MoveNext())
					{
						Pawn current = _003C_003E7__wrap1.Current;
						if (current.guest.PrisonerIsSecure)
						{
							_003C_003E2__current = current;
							_003C_003E1__state = 1;
							return true;
						}
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = default(List<Pawn>.Enumerator);
					_003C_003E7__wrap1 = map.mapPawns.SlavesOfColonySpawned.GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_0100;
					IL_0100:
					while (_003C_003E7__wrap1.MoveNext())
					{
						Pawn current2 = _003C_003E7__wrap1.Current;
						if (current2.guest.SlaveIsSecure)
						{
							_003C_003E2__current = current2;
							_003C_003E1__state = 2;
							return true;
						}
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap1 = default(List<Pawn>.Enumerator);
					_003C_003E7__wrap1 = map.mapPawns.SpawnedPawnsInFaction(Faction.OfPlayer).GetEnumerator();
					_003C_003E1__state = -5;
					break;
				}
				while (_003C_003E7__wrap1.MoveNext())
				{
					Pawn current3 = _003C_003E7__wrap1.Current;
					if (current3.IsAnimal && current3.HostFaction == null && !current3.InMentalState && !current3.Downed && (!checkAcceptableTemperatureOfAnimals || map.mapTemperature.SeasonAndOutdoorTemperatureAcceptableFor(current3.def)))
					{
						_003C_003E2__current = current3;
						_003C_003E1__state = 3;
						return true;
					}
				}
				_003C_003Em__Finally3();
				_003C_003E7__wrap1 = default(List<Pawn>.Enumerator);
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
			((IDisposable)_003C_003E7__wrap1/*cast due to .constrained prefix*/).Dispose();
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -1;
			((IDisposable)_003C_003E7__wrap1/*cast due to .constrained prefix*/).Dispose();
		}

		private void _003C_003Em__Finally3()
		{
			_003C_003E1__state = -1;
			((IDisposable)_003C_003E7__wrap1/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Pawn> IEnumerable<Pawn>.GetEnumerator()
		{
			_003CAllSellableColonyPawns_003Ed__8 _003CAllSellableColonyPawns_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CAllSellableColonyPawns_003Ed__ = this;
			}
			else
			{
				_003CAllSellableColonyPawns_003Ed__ = new _003CAllSellableColonyPawns_003Ed__8(0);
			}
			_003CAllSellableColonyPawns_003Ed__.map = _003C_003E3__map;
			_003CAllSellableColonyPawns_003Ed__.checkAcceptableTemperatureOfAnimals = _003C_003E3__checkAcceptableTemperatureOfAnimals;
			return _003CAllSellableColonyPawns_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Pawn>)this).GetEnumerator();
		}
	}

	public const float MinimumBuyPrice = 0.5f;

	public const float MinimumSellPrice = 0.01f;

	public const float PriceFactorBuy_Global = 1.4f;

	public const float PriceFactorSell_Global = 0.6f;

	public static bool EverPlayerSellable(ThingDef def)
	{
		if (!def.tradeability.PlayerCanSell())
		{
			return false;
		}
		if (def.GetStatValueAbstract(StatDefOf.MarketValue) <= 0f)
		{
			return false;
		}
		if (def.category != ThingCategory.Item && def.category != ThingCategory.Pawn && def.category != ThingCategory.Building)
		{
			return false;
		}
		if (def.category == ThingCategory.Building && !def.Minifiable)
		{
			return false;
		}
		return true;
	}

	public static bool PlayerSellableNow(Thing t, ITrader trader)
	{
		t = t.GetInnerIfMinified();
		if (!EverPlayerSellable(t.def))
		{
			return false;
		}
		if (t.IsNotFresh())
		{
			return false;
		}
		if (t is Apparel { WornByCorpse: not false })
		{
			return false;
		}
		if (CompBiocodable.IsBiocoded(t))
		{
			return false;
		}
		if (t is Pawn pawn)
		{
			if ((pawn.GetExtraHostFaction() != null && pawn.GetExtraHostFaction() == trader.Faction) || (pawn.IsQuestLodger() && pawn.GetExtraHomeFaction() == trader.Faction) || (pawn.HomeFaction != null && pawn.HomeFaction == trader.Faction && !pawn.IsSlave && !pawn.IsPrisoner))
			{
				return false;
			}
			if (pawn.RaceProps.Animal && pawn.IsFormingCaravan())
			{
				return false;
			}
		}
		return true;
	}

	public static void SpawnDropPod(IntVec3 dropSpot, Map map, Thing t)
	{
		ActiveTransporterInfo activeTransporterInfo = new ActiveTransporterInfo();
		activeTransporterInfo.SingleContainedThing = t;
		activeTransporterInfo.leaveSlag = false;
		DropPodUtility.MakeDropPodAt(dropSpot, map, activeTransporterInfo);
	}

	[IteratorStateMachine(typeof(_003CAllLaunchableThingsForTrade_003Ed__7))]
	public static IEnumerable<Thing> AllLaunchableThingsForTrade(Map map, ITrader trader = null)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CAllLaunchableThingsForTrade_003Ed__7(-2)
		{
			_003C_003E3__map = map,
			_003C_003E3__trader = trader
		};
	}

	[IteratorStateMachine(typeof(_003CAllSellableColonyPawns_003Ed__8))]
	public static IEnumerable<Pawn> AllSellableColonyPawns(Map map, bool checkAcceptableTemperatureOfAnimals = true)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CAllSellableColonyPawns_003Ed__8(-2)
		{
			_003C_003E3__map = map,
			_003C_003E3__checkAcceptableTemperatureOfAnimals = checkAcceptableTemperatureOfAnimals
		};
	}

	public static Thing ThingFromStockToMergeWith(ITrader trader, Thing thing)
	{
		if (thing is Pawn)
		{
			return null;
		}
		foreach (Thing good in trader.Goods)
		{
			if (TransferableUtility.TransferAsOne(good, thing, TransferAsOneMode.Normal) && good.CanStackWith(thing) && good.def.stackLimit != 1)
			{
				return good;
			}
		}
		return null;
	}

	public static void LaunchThingsOfType(ThingDef resDef, int debt, Map map, TradeShip trader)
	{
		while (debt > 0)
		{
			Thing thing = null;
			foreach (Building_OrbitalTradeBeacon item in Building_OrbitalTradeBeacon.AllPowered(map))
			{
				foreach (IntVec3 tradeableCell in item.TradeableCells)
				{
					foreach (Thing item2 in map.thingGrid.ThingsAt(tradeableCell))
					{
						if (item2.def == resDef)
						{
							thing = item2;
							goto end_IL_0089;
						}
					}
				}
				continue;
				end_IL_0089:
				break;
			}
			if (thing == null)
			{
				Log.Error("Could not find any " + resDef?.ToString() + " to transfer to trader.");
				break;
			}
			int num = Math.Min(debt, thing.stackCount);
			if (trader != null)
			{
				trader.GiveSoldThingToTrader(thing, num, TradeSession.playerNegotiator);
			}
			else
			{
				thing.SplitOff(num).Destroy();
			}
			debt -= num;
		}
	}

	public static void LaunchSilver(Map map, int fee)
	{
		LaunchThingsOfType(ThingDefOf.Silver, fee, map, null);
	}

	public static Map PlayerHomeMapWithMostLaunchableSilver()
	{
		return Find.Maps.Where((Map x) => x.IsPlayerHome).MaxBy((Map x) => (from t in AllLaunchableThingsForTrade(x)
			where t.def == ThingDefOf.Silver
			select t).Sum((Thing t) => t.stackCount));
	}

	public static bool ColonyHasEnoughSilver(Map map, int fee)
	{
		return (from t in AllLaunchableThingsForTrade(map)
			where t.def == ThingDefOf.Silver
			select t).Sum((Thing t) => t.stackCount) >= fee;
	}

	public static void CheckInteractWithTradersTeachOpportunity(Pawn pawn)
	{
		if (!pawn.Dead)
		{
			Lord lord = pawn.GetLord();
			if (lord != null && lord.CurLordToil is LordToil_DefendTraderCaravan)
			{
				LessonAutoActivator.TeachOpportunity(ConceptDefOf.InteractingWithTraders, pawn, OpportunityType.Important);
			}
		}
	}

	public static float GetPricePlayerSell(Thing thing, float priceFactorSell_TraderPriceType, float priceFactorSell_HumanPawn, float priceGain_PlayerNegotiator, float priceGain_FactionBase, float priceGain_DrugBonus, float priceGain_AnimalProduceBonus, TradeCurrency currency = TradeCurrency.Silver)
	{
		if (currency == TradeCurrency.Favor)
		{
			return thing.RoyalFavorValue;
		}
		float statValue = thing.GetStatValue(StatDefOf.SellPriceFactor);
		float num = thing.MarketValue * 0.6f * priceFactorSell_TraderPriceType * statValue * priceFactorSell_HumanPawn * (1f - Find.Storyteller.difficulty.tradePriceFactorLoss);
		num *= 1f + priceGain_PlayerNegotiator + priceGain_DrugBonus + priceGain_FactionBase + priceGain_AnimalProduceBonus;
		num = Mathf.Max(num, 0.01f);
		if (num > 99.5f)
		{
			num = Mathf.Round(num);
		}
		return num;
	}

	public static float GetPricePlayerBuy(Thing thing, float priceFactorBuy_TraderPriceType, float priceFactorBuy_JoinAs, float priceGain_PlayerNegotiator, float priceGain_FactionBase)
	{
		float num = thing.MarketValue * 1.4f * priceFactorBuy_TraderPriceType * priceFactorBuy_JoinAs * (1f + Find.Storyteller.difficulty.tradePriceFactorLoss);
		num *= 1f - priceGain_PlayerNegotiator - priceGain_FactionBase;
		num = Mathf.Max(num, 0.5f);
		if (num > 99.5f)
		{
			num = Mathf.Round(num);
		}
		return num;
	}

	public static float TotalMarketValue(IEnumerable<Thing> things)
	{
		if (things == null)
		{
			return 0f;
		}
		float num = 0f;
		foreach (Thing thing in things)
		{
			num += thing.MarketValue * (float)thing.stackCount;
		}
		return num;
	}

	public static void CheckGiveTraderQuest(Pawn trader)
	{
		if (ModsConfig.OdysseyActive && Rand.Chance(Find.Storyteller.QuestGiverTraderChanceNow))
		{
			trader.mindState.hasQuest = true;
			Find.Storyteller.Notify_QuestGiverTraderSpawned();
		}
	}

	public static void ReceiveQuestFromTrader(Pawn trader, Pawn negotiator)
	{
		if (!ModsConfig.OdysseyActive)
		{
			return;
		}
		Slate slate = new Slate();
		slate.Set("points", StorytellerUtility.DefaultThreatPointsNow(negotiator.Map));
		slate.Set("discoveryMethod", "QuestDiscoveredFromTrader".Translate(trader.Named("TRADER"), negotiator.Named("NEGOTIATOR")));
		List<QuestScriptDef> list = QuestUtility.GetGiverQuests(QuestGiverTag.Traders).ToList();
		if (!list.NullOrEmpty())
		{
			Quest quest = QuestUtility.GenerateQuestAndMakeAvailable(list.RandomElementByWeight((QuestScriptDef q) => q.rootSelectionWeight), slate);
			Messages.Message("MessageTraderGaveQuest".Translate(quest.name, trader.Named("TRADER"), negotiator.Named("NEGOTIATOR")), MessageTypeDefOf.PositiveEvent);
			if (!quest.hidden && quest.root.sendAvailableLetter)
			{
				QuestUtility.SendLetterQuestAvailable(quest, slate.Get<string>("discoveryMethod"));
			}
			trader.mindState.hasQuest = false;
		}
	}
}
