using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class Building_CommsConsole : Building
{
	[CompilerGenerated]
	private sealed class _003CGetFloatMenuOptions_003Ed__7 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Building_CommsConsole _003C_003E4__this;

		private Pawn myPawn;

		public Pawn _003C_003E3__myPawn;

		private IEnumerator<ICommunicable> _003C_003E7__wrap1;

		private IEnumerator<FloatMenuOption> _003C_003E7__wrap2;

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
		public _003CGetFloatMenuOptions_003Ed__7(int _003C_003E1__state)
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
			case 2:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
				break;
			case -4:
			case 3:
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
				Building_CommsConsole building_CommsConsole = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
				{
					_003C_003E1__state = -1;
					FloatMenuOption failureReason = building_CommsConsole.GetFailureReason(myPawn);
					if (failureReason != null)
					{
						_003C_003E2__current = failureReason;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003E7__wrap1 = building_CommsConsole.GetCommTargets(myPawn).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_00c1;
				}
				case 1:
					_003C_003E1__state = -1;
					return false;
				case 2:
					_003C_003E1__state = -3;
					goto IL_00c1;
				case 3:
					{
						_003C_003E1__state = -4;
						break;
					}
					IL_00c1:
					while (_003C_003E7__wrap1.MoveNext())
					{
						FloatMenuOption floatMenuOption = _003C_003E7__wrap1.Current.CommFloatMenuOption(building_CommsConsole, myPawn);
						if (floatMenuOption != null)
						{
							_003C_003E2__current = floatMenuOption;
							_003C_003E1__state = 2;
							return true;
						}
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					_003C_003E7__wrap2 = ((ThingWithComps)building_CommsConsole).GetFloatMenuOptions(myPawn).GetEnumerator();
					_003C_003E1__state = -4;
					break;
				}
				if (_003C_003E7__wrap2.MoveNext())
				{
					FloatMenuOption current = _003C_003E7__wrap2.Current;
					_003C_003E2__current = current;
					_003C_003E1__state = 3;
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
		IEnumerator<FloatMenuOption> IEnumerable<FloatMenuOption>.GetEnumerator()
		{
			_003CGetFloatMenuOptions_003Ed__7 _003CGetFloatMenuOptions_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetFloatMenuOptions_003Ed__ = this;
			}
			else
			{
				_003CGetFloatMenuOptions_003Ed__ = new _003CGetFloatMenuOptions_003Ed__7(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGetFloatMenuOptions_003Ed__.myPawn = _003C_003E3__myPawn;
			return _003CGetFloatMenuOptions_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FloatMenuOption>)this).GetEnumerator();
		}
	}

	private CompPowerTrader powerComp;

	public bool CanUseCommsNow
	{
		get
		{
			if (base.Spawned && base.Map.gameConditionManager.ElectricityDisabled(base.Map))
			{
				return false;
			}
			if (powerComp != null)
			{
				return powerComp.PowerOn;
			}
			return true;
		}
	}

	public override void SpawnSetup(Map map, bool respawningAfterLoad)
	{
		base.SpawnSetup(map, respawningAfterLoad);
		powerComp = GetComp<CompPowerTrader>();
		LessonAutoActivator.TeachOpportunity(ConceptDefOf.BuildOrbitalTradeBeacon, OpportunityType.GoodToKnow);
		LessonAutoActivator.TeachOpportunity(ConceptDefOf.OpeningComms, OpportunityType.GoodToKnow);
		if (CanUseCommsNow)
		{
			LongEventHandler.ExecuteWhenFinished(AnnounceTradeShips);
		}
	}

	private void UseAct(Pawn myPawn, ICommunicable commTarget)
	{
		Job job = JobMaker.MakeJob(JobDefOf.UseCommsConsole, this);
		job.commTarget = commTarget;
		myPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
		PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.OpeningComms, KnowledgeAmount.Total);
	}

	private FloatMenuOption GetFailureReason(Pawn myPawn)
	{
		if (!myPawn.CanReach(this, PathEndMode.InteractionCell, Danger.Some))
		{
			return new FloatMenuOption("CannotUseNoPath".Translate(), null);
		}
		if (base.Spawned && base.Map.gameConditionManager.ElectricityDisabled(base.Map))
		{
			return new FloatMenuOption("CannotUseSolarFlare".Translate(), null);
		}
		if (powerComp != null && !powerComp.PowerOn)
		{
			return new FloatMenuOption("CannotUseNoPower".Translate(), null);
		}
		if (!myPawn.health.capacities.CapableOf(PawnCapacityDefOf.Talking))
		{
			return new FloatMenuOption("CannotUseReason".Translate("IncapableOfCapacity".Translate(PawnCapacityDefOf.Talking.label, myPawn.Named("PAWN"))), null);
		}
		if (!GetCommTargets(myPawn).Any())
		{
			return new FloatMenuOption("CannotUseReason".Translate("NoCommsTarget".Translate()), null);
		}
		if (!CanUseCommsNow)
		{
			Log.Error(myPawn?.ToString() + " could not use comm console for unknown reason.");
			return new FloatMenuOption("Cannot use now", null);
		}
		return null;
	}

	public IEnumerable<ICommunicable> GetCommTargets(Pawn myPawn)
	{
		return myPawn.Map.passingShipManager.passingShips.Cast<ICommunicable>().Concat(Find.FactionManager.AllFactionsVisibleInViewOrder.Where((Faction f) => !f.temporary && !f.IsPlayer).Cast<ICommunicable>());
	}

	[IteratorStateMachine(typeof(_003CGetFloatMenuOptions_003Ed__7))]
	public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn myPawn)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetFloatMenuOptions_003Ed__7(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__myPawn = myPawn
		};
	}

	public void GiveUseCommsJob(Pawn negotiator, ICommunicable target)
	{
		Job job = JobMaker.MakeJob(JobDefOf.UseCommsConsole, this);
		job.commTarget = target;
		negotiator.jobs.TryTakeOrderedJob(job, JobTag.Misc);
		PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.OpeningComms, KnowledgeAmount.Total);
	}

	private void AnnounceTradeShips()
	{
		foreach (TradeShip item in from s in base.Map.passingShipManager.passingShips.OfType<TradeShip>()
			where !s.WasAnnounced
			select s)
		{
			TaggedString baseLetterText = "TraderArrival".Translate(item.name, item.def.label, (item.Faction == null) ? "TraderArrivalNoFaction".Translate() : "TraderArrivalFromFaction".Translate(item.Faction.Named("FACTION")));
			IncidentParms incidentParms = new IncidentParms();
			incidentParms.target = base.Map;
			incidentParms.traderKind = item.TraderKind;
			IncidentWorker.SendIncidentLetter(item.def.LabelCap, baseLetterText, LetterDefOf.PositiveEvent, incidentParms, LookTargets.Invalid, null);
			item.WasAnnounced = true;
		}
	}
}
