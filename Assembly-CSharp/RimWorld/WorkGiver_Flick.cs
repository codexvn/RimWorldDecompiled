using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class WorkGiver_Flick : WorkGiver_Scanner
{
	[CompilerGenerated]
	private sealed class _003CPotentialWorkThingsGlobal_003Ed__3 : IEnumerable<Thing>, IEnumerable, IEnumerator<Thing>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Thing _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Pawn pawn;

		public Pawn _003C_003E3__pawn;

		private List<Designation>.Enumerator _003C_003E7__wrap1;

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
		public _003CPotentialWorkThingsGlobal_003Ed__3(int _003C_003E1__state)
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
			_003C_003E7__wrap1 = default(List<Designation>.Enumerator);
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
					_003C_003E7__wrap1 = pawn.Map.designationManager.designationsByDef[DesignationDefOf.Flick].GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					Designation current = _003C_003E7__wrap1.Current;
					_003C_003E2__current = current.target.Thing;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = default(List<Designation>.Enumerator);
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

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Thing> IEnumerable<Thing>.GetEnumerator()
		{
			_003CPotentialWorkThingsGlobal_003Ed__3 _003CPotentialWorkThingsGlobal_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CPotentialWorkThingsGlobal_003Ed__ = this;
			}
			else
			{
				_003CPotentialWorkThingsGlobal_003Ed__ = new _003CPotentialWorkThingsGlobal_003Ed__3(0);
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

	public override PathEndMode PathEndMode => PathEndMode.Touch;

	public override Danger MaxPathDanger(Pawn pawn)
	{
		return Danger.Deadly;
	}

	[IteratorStateMachine(typeof(_003CPotentialWorkThingsGlobal_003Ed__3))]
	public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CPotentialWorkThingsGlobal_003Ed__3(-2)
		{
			_003C_003E3__pawn = pawn
		};
	}

	public override bool ShouldSkip(Pawn pawn, bool forced = false)
	{
		return !pawn.Map.designationManager.AnySpawnedDesignationOfDef(DesignationDefOf.Flick);
	}

	public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
	{
		if (pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.Flick) == null)
		{
			return false;
		}
		if (!pawn.CanReserve(t, 1, -1, null, forced))
		{
			return false;
		}
		return true;
	}

	public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
	{
		return JobMaker.MakeJob(JobDefOf.Flick, t);
	}
}
