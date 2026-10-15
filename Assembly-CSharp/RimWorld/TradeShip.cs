using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class TradeShip : PassingShip, ITrader, IThingHolder
{
	[CompilerGenerated]
	private sealed class _003CColonyThingsWillingToBuy_003Ed__31 : IEnumerable<Thing>, IEnumerable, IEnumerator<Thing>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Thing _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public TradeShip _003C_003E4__this;

		private IEnumerator<Thing> _003C_003E7__wrap1;

		private IEnumerator<Pawn> _003C_003E7__wrap2;

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
		public _003CColonyThingsWillingToBuy_003Ed__31(int _003C_003E1__state)
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
			}
			_003C_003E7__wrap1 = null;
			_003C_003E7__wrap2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				TradeShip tradeShip = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = TradeUtility.AllLaunchableThingsForTrade(tradeShip.Map, tradeShip).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0078;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0078;
				case 2:
					{
						_003C_003E1__state = -4;
						break;
					}
					IL_0078:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Thing current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					_003C_003E7__wrap2 = TradeUtility.AllSellableColonyPawns(tradeShip.Map, checkAcceptableTemperatureOfAnimals: false).GetEnumerator();
					_003C_003E1__state = -4;
					break;
				}
				if (_003C_003E7__wrap2.MoveNext())
				{
					Pawn current2 = _003C_003E7__wrap2.Current;
					_003C_003E2__current = current2;
					_003C_003E1__state = 2;
					return true;
				}
				_003C_003Em__Finally2();
				_003C_003E7__wrap2 = null;
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
			if (_003C_003E7__wrap2 != null)
			{
				_003C_003E7__wrap2.Dispose();
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
			_003CColonyThingsWillingToBuy_003Ed__31 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CColonyThingsWillingToBuy_003Ed__31(0)
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
	private sealed class _003Cget_Goods_003Ed__27 : IEnumerable<Thing>, IEnumerable, IEnumerator<Thing>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Thing _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public TradeShip _003C_003E4__this;

		private int _003Ci_003E5__2;

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
		public _003Cget_Goods_003Ed__27(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			TradeShip tradeShip = _003C_003E4__this;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				goto IL_0076;
			}
			_003C_003E1__state = -1;
			_003Ci_003E5__2 = 0;
			goto IL_0086;
			IL_0076:
			_003Ci_003E5__2++;
			goto IL_0086;
			IL_0086:
			if (_003Ci_003E5__2 < tradeShip.things.Count)
			{
				if (!(tradeShip.things[_003Ci_003E5__2] is Pawn item) || !tradeShip.soldPrisoners.Contains(item))
				{
					_003C_003E2__current = tradeShip.things[_003Ci_003E5__2];
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_0076;
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
			_003Cget_Goods_003Ed__27 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_Goods_003Ed__27(0)
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

	public TraderKindDef def;

	private ThingOwner things;

	private List<Pawn> soldPrisoners = new List<Pawn>();

	private int randomPriceFactorSeed = -1;

	private bool wasAnnounced;

	private static List<string> tmpExtantNames = new List<string>();

	public override string FullTitle => name + " (" + def.label + ")";

	public int Silver => CountHeldOf(ThingDefOf.Silver);

	public TradeCurrency TradeCurrency => def.tradeCurrency;

	public IThingHolder ParentHolder => base.Map;

	public TraderKindDef TraderKind => def;

	public int RandomPriceFactorSeed => randomPriceFactorSeed;

	public string TraderName => name;

	public bool CanTradeNow => !base.Departed;

	public float TradePriceImprovementOffsetForPlayer => 0f;

	public bool WasAnnounced
	{
		get
		{
			return wasAnnounced;
		}
		set
		{
			wasAnnounced = value;
		}
	}

	public IEnumerable<Thing> Goods
	{
		[IteratorStateMachine(typeof(_003Cget_Goods_003Ed__27))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_Goods_003Ed__27(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public TradeShip()
	{
	}

	public TradeShip(TraderKindDef def, Faction faction = null)
		: base(faction)
	{
		this.def = def;
		things = new ThingOwner<Thing>(this);
		tmpExtantNames.Clear();
		List<Map> maps = Find.Maps;
		for (int i = 0; i < maps.Count; i++)
		{
			tmpExtantNames.AddRange(maps[i].passingShipManager.passingShips.Select((PassingShip x) => x.name));
		}
		name = NameGenerator.GenerateName(RulePackDefOf.NamerTraderGeneral, tmpExtantNames);
		if (faction != null)
		{
			name = "GuildTradeShipName".Translate(name, faction.Name);
		}
		randomPriceFactorSeed = Rand.RangeInclusive(1, 10000000);
		loadID = Find.UniqueIDsManager.GetNextPassingShipID();
	}

	[IteratorStateMachine(typeof(_003CColonyThingsWillingToBuy_003Ed__31))]
	public IEnumerable<Thing> ColonyThingsWillingToBuy(Pawn playerNegotiator)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CColonyThingsWillingToBuy_003Ed__31(-2)
		{
			_003C_003E4__this = this
		};
	}

	public void GenerateThings()
	{
		ThingSetMakerParams parms = default(ThingSetMakerParams);
		parms.traderDef = def;
		parms.tile = base.Map.Tile;
		things.TryAddRangeOrTransfer(ThingSetMakerDefOf.TraderStock.root.Generate(parms));
	}

	public override void PassingShipTick()
	{
		base.PassingShipTick();
		for (int num = things.Count - 1; num >= 0; num--)
		{
			if (things[num] is Pawn pawn)
			{
				pawn.DoTick();
				if (pawn.Dead)
				{
					things.Remove(pawn);
				}
			}
		}
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Defs.Look(ref def, "def");
		Scribe_Deep.Look(ref things, "things", this);
		Scribe_Collections.Look(ref soldPrisoners, "soldPrisoners", LookMode.Reference);
		Scribe_Values.Look(ref randomPriceFactorSeed, "randomPriceFactorSeed", 0);
		Scribe_Values.Look(ref wasAnnounced, "wasAnnounced", defaultValue: true);
		if (Scribe.mode == LoadSaveMode.PostLoadInit)
		{
			soldPrisoners.RemoveAll((Pawn x) => x == null);
		}
	}

	public override void TryOpenComms(Pawn negotiator)
	{
		if (CanTradeNow)
		{
			Find.WindowStack.Add(new Dialog_Trade(negotiator, this));
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.BuildOrbitalTradeBeacon, OpportunityType.Critical);
			PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter_Send(Goods.OfType<Pawn>(), "LetterRelatedPawnsTradeShip".Translate(Faction.OfPlayer.def.pawnsPlural), LetterDefOf.NeutralEvent);
			TutorUtility.DoModalDialogIfNotKnown(ConceptDefOf.TradeGoodsMustBeNearBeacon);
		}
	}

	public override void Depart()
	{
		base.Depart();
		things.ClearAndDestroyContentsOrPassToWorld();
		soldPrisoners.Clear();
	}

	public override string GetCallLabel()
	{
		return name + " (" + def.label + ")";
	}

	protected override AcceptanceReport CanCommunicateWith(Pawn negotiator)
	{
		AcceptanceReport result = base.CanCommunicateWith(negotiator);
		if (!result.Accepted)
		{
			return result;
		}
		return negotiator.CanTradeWith(base.Faction, TraderKind);
	}

	public int CountHeldOf(ThingDef thingDef, ThingDef stuffDef = null)
	{
		return HeldThingMatching(thingDef, stuffDef)?.stackCount ?? 0;
	}

	public void GiveSoldThingToTrader(Thing toGive, int countToGive, Pawn playerNegotiator)
	{
		Thing thing = toGive.SplitOff(countToGive);
		thing.PreTraded(TradeAction.PlayerSells, playerNegotiator, this);
		Thing thing2 = TradeUtility.ThingFromStockToMergeWith(this, thing);
		if (thing2 != null)
		{
			if (!thing2.TryAbsorbStack(thing, respectStackLimit: false))
			{
				thing.Destroy();
			}
			return;
		}
		if (thing is Pawn pawn && pawn.RaceProps.Humanlike)
		{
			soldPrisoners.Add(pawn);
		}
		things.TryAdd(thing, canMergeWithExistingStacks: false);
	}

	public void GiveSoldThingToPlayer(Thing toGive, int countToGive, Pawn playerNegotiator)
	{
		Thing thing = toGive.SplitOff(countToGive);
		thing.PreTraded(TradeAction.PlayerBuys, playerNegotiator, this);
		if (thing is Pawn item)
		{
			soldPrisoners.Remove(item);
		}
		TradeUtility.SpawnDropPod(DropCellFinder.TradeDropSpot(base.Map), base.Map, thing);
	}

	private Thing HeldThingMatching(ThingDef thingDef, ThingDef stuffDef)
	{
		for (int i = 0; i < things.Count; i++)
		{
			if (things[i].def == thingDef && things[i].Stuff == stuffDef)
			{
				return things[i];
			}
		}
		return null;
	}

	public void ChangeCountHeldOf(ThingDef thingDef, ThingDef stuffDef, int count)
	{
		Thing thing = HeldThingMatching(thingDef, stuffDef);
		if (thing == null)
		{
			Log.Error("Changing count of thing trader doesn't have: " + thingDef);
		}
		thing.stackCount += count;
	}

	public override string ToString()
	{
		return FullTitle;
	}

	public ThingOwner GetDirectlyHeldThings()
	{
		return things;
	}

	public void GetChildHolders(List<IThingHolder> outChildren)
	{
		ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, GetDirectlyHeldThings());
	}
}
