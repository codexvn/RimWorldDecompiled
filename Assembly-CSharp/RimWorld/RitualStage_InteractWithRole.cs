using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class RitualStage_InteractWithRole : RitualStage
{
	public class PawnTarget : IExposable
	{
		[NoTranslate]
		public string pawnId;

		[NoTranslate]
		public string targetId;

		public void ExposeData()
		{
			Scribe_Values.Look(ref pawnId, "pawnId");
			Scribe_Values.Look(ref targetId, "targetId");
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetPawnSecondFoci_003Ed__4 : IEnumerable<RitualStagePawnSecondFocus>, IEnumerable, IEnumerator<RitualStagePawnSecondFocus>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private RitualStagePawnSecondFocus _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public RitualStage_InteractWithRole _003C_003E4__this;

		private LordJob_Ritual ritual;

		public LordJob_Ritual _003C_003E3__ritual;

		private List<PawnTarget>.Enumerator _003C_003E7__wrap1;

		RitualStagePawnSecondFocus IEnumerator<RitualStagePawnSecondFocus>.Current
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
		public _003CGetPawnSecondFoci_003Ed__4(int _003C_003E1__state)
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
			_003C_003E7__wrap1 = default(List<PawnTarget>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				RitualStage_InteractWithRole ritualStage_InteractWithRole = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (ritualStage_InteractWithRole.targets.NullOrEmpty())
					{
						return false;
					}
					_003C_003E7__wrap1 = ritualStage_InteractWithRole.targets.GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					PawnTarget current = _003C_003E7__wrap1.Current;
					Pawn pawn = ritual.assignments.FirstAssignedPawn(current.pawnId);
					Pawn pawn2 = ritual.assignments.FirstAssignedPawn(current.targetId);
					_003C_003E2__current = new RitualStagePawnSecondFocus
					{
						pawn = pawn,
						target = pawn2
					};
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = default(List<PawnTarget>.Enumerator);
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
		IEnumerator<RitualStagePawnSecondFocus> IEnumerable<RitualStagePawnSecondFocus>.GetEnumerator()
		{
			_003CGetPawnSecondFoci_003Ed__4 _003CGetPawnSecondFoci_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetPawnSecondFoci_003Ed__ = this;
			}
			else
			{
				_003CGetPawnSecondFoci_003Ed__ = new _003CGetPawnSecondFoci_003Ed__4(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGetPawnSecondFoci_003Ed__.ritual = _003C_003E3__ritual;
			return _003CGetPawnSecondFoci_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<RitualStagePawnSecondFocus>)this).GetEnumerator();
		}
	}

	[NoTranslate]
	public string targetId;

	public List<PawnTarget> targets;

	public override TargetInfo GetSecondFocus(LordJob_Ritual ritual)
	{
		if (targetId != null)
		{
			return ritual.PawnWithRole(targetId);
		}
		return base.GetSecondFocus(ritual);
	}

	[IteratorStateMachine(typeof(_003CGetPawnSecondFoci_003Ed__4))]
	public override IEnumerable<RitualStagePawnSecondFocus> GetPawnSecondFoci(LordJob_Ritual ritual)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetPawnSecondFoci_003Ed__4(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__ritual = ritual
		};
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref targetId, "targetId");
		Scribe_Collections.Look(ref targets, "targets", LookMode.Deep);
	}
}
