using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld.Planet;

public class TransportersArrivalAction_AttackSettlement : TransportersArrivalAction
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass11_0
	{
		public IEnumerable<IThingHolder> pods;

		public Settlement settlement;

		public Func<FloatMenuAcceptanceReport> _003C_003E9__0;

		public Func<TransportersArrivalAction_AttackSettlement> _003C_003E9__1;

		public Func<FloatMenuAcceptanceReport> _003C_003E9__2;

		public Func<TransportersArrivalAction_AttackSettlement> _003C_003E9__3;

		internal FloatMenuAcceptanceReport _003CGetFloatMenuOptions_003Eb__0()
		{
			return CanAttack(pods, settlement);
		}

		internal TransportersArrivalAction_AttackSettlement _003CGetFloatMenuOptions_003Eb__1()
		{
			return new TransportersArrivalAction_AttackSettlement(settlement, PawnsArrivalModeDefOf.EdgeDrop);
		}

		internal FloatMenuAcceptanceReport _003CGetFloatMenuOptions_003Eb__2()
		{
			return CanAttack(pods, settlement);
		}

		internal TransportersArrivalAction_AttackSettlement _003CGetFloatMenuOptions_003Eb__3()
		{
			return new TransportersArrivalAction_AttackSettlement(settlement, PawnsArrivalModeDefOf.CenterDrop);
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetFloatMenuOptions_003Ed__11 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private IEnumerable<IThingHolder> pods;

		public IEnumerable<IThingHolder> _003C_003E3__pods;

		private Settlement settlement;

		public Settlement _003C_003E3__settlement;

		private Action<PlanetTile, TransportersArrivalAction> launchAction;

		public Action<PlanetTile, TransportersArrivalAction> _003C_003E3__launchAction;

		private _003C_003Ec__DisplayClass11_0 _003C_003E8__1;

		private IEnumerator<FloatMenuOption> _003C_003E7__wrap1;

		FloatMenuOption IEnumerator<FloatMenuOption>.Current
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
		public _003CGetFloatMenuOptions_003Ed__11(int _003C_003E1__state)
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
			_003C_003E8__1 = null;
			_003C_003E7__wrap1 = null;
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
					_003C_003E8__1 = new _003C_003Ec__DisplayClass11_0();
					_003C_003E8__1.pods = pods;
					_003C_003E8__1.settlement = settlement;
					_003C_003E7__wrap1 = TransportersArrivalActionUtility.GetFloatMenuOptions(() => CanAttack(_003C_003E8__1.pods, _003C_003E8__1.settlement), () => new TransportersArrivalAction_AttackSettlement(_003C_003E8__1.settlement, PawnsArrivalModeDefOf.EdgeDrop), "AttackAndDropAtEdge".Translate(_003C_003E8__1.settlement.Label), launchAction, _003C_003E8__1.settlement.Tile).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0130;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0130;
				case 2:
					{
						_003C_003E1__state = -4;
						break;
					}
					IL_0130:
					if (_003C_003E7__wrap1.MoveNext())
					{
						FloatMenuOption current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					_003C_003E7__wrap1 = TransportersArrivalActionUtility.GetFloatMenuOptions(() => CanAttack(_003C_003E8__1.pods, _003C_003E8__1.settlement), () => new TransportersArrivalAction_AttackSettlement(_003C_003E8__1.settlement, PawnsArrivalModeDefOf.CenterDrop), "AttackAndDropInCenter".Translate(_003C_003E8__1.settlement.Label), launchAction, _003C_003E8__1.settlement.Tile).GetEnumerator();
					_003C_003E1__state = -4;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					FloatMenuOption current2 = _003C_003E7__wrap1.Current;
					_003C_003E2__current = current2;
					_003C_003E1__state = 2;
					return true;
				}
				_003C_003Em__Finally2();
				_003C_003E7__wrap1 = null;
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
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<FloatMenuOption> IEnumerable<FloatMenuOption>.GetEnumerator()
		{
			_003CGetFloatMenuOptions_003Ed__11 _003CGetFloatMenuOptions_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetFloatMenuOptions_003Ed__ = this;
			}
			else
			{
				_003CGetFloatMenuOptions_003Ed__ = new _003CGetFloatMenuOptions_003Ed__11(0);
			}
			_003CGetFloatMenuOptions_003Ed__.launchAction = _003C_003E3__launchAction;
			_003CGetFloatMenuOptions_003Ed__.pods = _003C_003E3__pods;
			_003CGetFloatMenuOptions_003Ed__.settlement = _003C_003E3__settlement;
			return _003CGetFloatMenuOptions_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FloatMenuOption>)this).GetEnumerator();
		}
	}

	private Settlement settlement;

	private PawnsArrivalModeDef arrivalMode;

	public override bool GeneratesMap => true;

	public TransportersArrivalAction_AttackSettlement()
	{
	}

	public TransportersArrivalAction_AttackSettlement(Settlement settlement, PawnsArrivalModeDef arrivalMode)
	{
		this.settlement = settlement;
		this.arrivalMode = arrivalMode;
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_References.Look(ref settlement, "settlement");
		Scribe_Defs.Look(ref arrivalMode, "arrivalMode");
	}

	public override FloatMenuAcceptanceReport StillValid(IEnumerable<IThingHolder> pods, PlanetTile destinationTile)
	{
		FloatMenuAcceptanceReport floatMenuAcceptanceReport = base.StillValid(pods, destinationTile);
		if (!floatMenuAcceptanceReport)
		{
			return floatMenuAcceptanceReport;
		}
		if (settlement != null && settlement.Tile != destinationTile)
		{
			return false;
		}
		return CanAttack(pods, settlement);
	}

	public override bool ShouldUseLongEvent(List<ActiveTransporterInfo> pods, PlanetTile tile)
	{
		return !settlement.HasMap;
	}

	public override void Arrived(List<ActiveTransporterInfo> transporters, PlanetTile tile)
	{
		Thing lookTarget = TransportersArrivalActionUtility.GetLookTarget(transporters);
		bool num = !settlement.HasMap;
		Map orGenerateMap = GetOrGenerateMapUtility.GetOrGenerateMap(settlement.Tile, null);
		TaggedString letterLabel = "LetterLabelCaravanEnteredEnemyBase".Translate();
		TaggedString letterText = "LetterTransportPodsLandedInEnemyBase".Translate(settlement.Label).CapitalizeFirst();
		SettlementUtility.AffectRelationsOnAttacked(settlement, ref letterText);
		if (num)
		{
			Find.TickManager.Notify_GeneratedPotentiallyHostileMap();
			PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter(orGenerateMap.mapPawns.AllPawns, ref letterLabel, ref letterText, "LetterRelatedPawnsInMapWherePlayerLanded".Translate(Faction.OfPlayer.def.pawnsPlural), informEvenIfSeenBefore: true);
		}
		Find.LetterStack.ReceiveLetter(letterLabel, letterText, LetterDefOf.NeutralEvent, lookTarget, settlement.Faction);
		arrivalMode.Worker.TravellingTransportersArrived(transporters, orGenerateMap);
	}

	public static FloatMenuAcceptanceReport CanAttack(IEnumerable<IThingHolder> pods, Settlement settlement)
	{
		if (settlement == null || !settlement.Spawned || !settlement.Attackable)
		{
			return false;
		}
		if (!TransportersArrivalActionUtility.AnyNonDownedColonist(pods))
		{
			return false;
		}
		if (settlement.EnterCooldownBlocksEntering())
		{
			return FloatMenuAcceptanceReport.WithFailReasonAndMessage("EnterCooldownBlocksEntering".Translate(), "MessageEnterCooldownBlocksEntering".Translate(settlement.EnterCooldownTicksLeft().ToStringTicksToPeriod()));
		}
		return true;
	}

	[IteratorStateMachine(typeof(_003CGetFloatMenuOptions_003Ed__11))]
	public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(Action<PlanetTile, TransportersArrivalAction> launchAction, IEnumerable<IThingHolder> pods, Settlement settlement)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetFloatMenuOptions_003Ed__11(-2)
		{
			_003C_003E3__launchAction = launchAction,
			_003C_003E3__pods = pods,
			_003C_003E3__settlement = settlement
		};
	}
}
