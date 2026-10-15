using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class WorkGiver_Slaughter : WorkGiver_Scanner
{
	[CompilerGenerated]
	private sealed class _003CPotentialWorkThingsGlobal_003Ed__0 : IEnumerable<Thing>, IEnumerable, IEnumerator<Thing>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Thing _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Pawn pawn;

		public Pawn _003C_003E3__pawn;

		private IEnumerator<Designation> _003C_003E7__wrap1;

		private List<Pawn>.Enumerator _003C_003E7__wrap2;

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
		public _003CPotentialWorkThingsGlobal_003Ed__0(int _003C_003E1__state)
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
			_003C_003E7__wrap2 = default(List<Pawn>.Enumerator);
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
					_003C_003E7__wrap1 = pawn.Map.designationManager.SpawnedDesignationsOfDef(DesignationDefOf.Slaughter).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0089;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0089;
				case 2:
					{
						_003C_003E1__state = -4;
						break;
					}
					IL_0089:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Designation current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current.target.Thing;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					_003C_003E7__wrap2 = pawn.Map.autoSlaughterManager.AnimalsToSlaughter.GetEnumerator();
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
				_003C_003E7__wrap2 = default(List<Pawn>.Enumerator);
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

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Thing> IEnumerable<Thing>.GetEnumerator()
		{
			_003CPotentialWorkThingsGlobal_003Ed__0 _003CPotentialWorkThingsGlobal_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CPotentialWorkThingsGlobal_003Ed__ = this;
			}
			else
			{
				_003CPotentialWorkThingsGlobal_003Ed__ = new _003CPotentialWorkThingsGlobal_003Ed__0(0);
			}
			_003CPotentialWorkThingsGlobal_003Ed__.pawn = _003C_003E3__pawn;
			return _003CPotentialWorkThingsGlobal_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Thing>)this).GetEnumerator();
		}
	}

	public override PathEndMode PathEndMode => PathEndMode.OnCell;

	[IteratorStateMachine(typeof(_003CPotentialWorkThingsGlobal_003Ed__0))]
	public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CPotentialWorkThingsGlobal_003Ed__0(-2)
		{
			_003C_003E3__pawn = pawn
		};
	}

	public override bool ShouldSkip(Pawn pawn, bool forced = false)
	{
		if (!pawn.Map.designationManager.AnySpawnedDesignationOfDef(DesignationDefOf.Slaughter))
		{
			return pawn.Map.autoSlaughterManager.AnimalsToSlaughter.Count == 0;
		}
		return false;
	}

	public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
	{
		if (!(t is Pawn { IsAnimal: not false } pawn2))
		{
			return false;
		}
		if (!pawn2.ShouldBeSlaughtered())
		{
			return false;
		}
		if (pawn.Faction != t.Faction)
		{
			return false;
		}
		if (pawn2.InAggroMentalState)
		{
			return false;
		}
		if (!pawn.CanReserve(t, 1, -1, null, forced))
		{
			return false;
		}
		if (pawn.WorkTagIsDisabled(WorkTags.Violent))
		{
			JobFailReason.Is("IsIncapableOfViolenceShort".Translate(pawn));
			return false;
		}
		if (ModsConfig.IdeologyActive && !new HistoryEvent(HistoryEventDefOf.SlaughteredAnimal, pawn.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo_Job())
		{
			return false;
		}
		if (HistoryEventUtility.IsKillingInnocentAnimal(pawn, pawn2) && !new HistoryEvent(HistoryEventDefOf.KilledInnocentAnimal, pawn.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo_Job())
		{
			return false;
		}
		if (pawn.Ideo != null && pawn.Ideo.IsVeneratedAnimal(pawn2) && !new HistoryEvent(HistoryEventDefOf.SlaughteredVeneratedAnimal, pawn.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo_Job())
		{
			return false;
		}
		return true;
	}

	public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
	{
		return JobMaker.MakeJob(JobDefOf.Slaughter, t);
	}
}
