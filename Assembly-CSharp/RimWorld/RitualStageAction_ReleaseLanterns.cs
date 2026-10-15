using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class RitualStageAction_ReleaseLanterns : RitualStageTickActionMaker
{
	[CompilerGenerated]
	private sealed class _003CGenerateTimedActions_003Ed__2 : IEnumerable<ActionOnTick>, IEnumerable, IEnumerator<ActionOnTick>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private ActionOnTick _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private RitualStage stage;

		public RitualStage _003C_003E3__stage;

		private LordJob_Ritual ritual;

		public LordJob_Ritual _003C_003E3__ritual;

		public RitualStageAction_ReleaseLanterns _003C_003E4__this;

		private int _003CdurationTicks_003E5__2;

		private List<Pawn>.Enumerator _003C_003E7__wrap2;

		ActionOnTick IEnumerator<ActionOnTick>.Current
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
		public _003CGenerateTimedActions_003Ed__2(int _003C_003E1__state)
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
			_003C_003E7__wrap2 = default(List<Pawn>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				RitualStageAction_ReleaseLanterns ritualStageAction_ReleaseLanterns = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
				{
					_003C_003E1__state = -1;
					StageEndTrigger_DurationPercentage stageEndTrigger_DurationPercentage = (StageEndTrigger_DurationPercentage)stage.endTriggers.FirstOrFallback((StageEndTrigger e) => e is StageEndTrigger_DurationPercentage);
					if (stageEndTrigger_DurationPercentage == null)
					{
						return false;
					}
					_003CdurationTicks_003E5__2 = (int)(stageEndTrigger_DurationPercentage.percentage * (float)ritual.DurationTicks);
					_003C_003E7__wrap2 = ritual.assignments.Participants.GetEnumerator();
					_003C_003E1__state = -3;
					break;
				}
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap2.MoveNext())
				{
					Pawn current = _003C_003E7__wrap2.Current;
					_003C_003E2__current = new ActionOnTick_ReleaseLantern
					{
						tick = _003CdurationTicks_003E5__2 - ritualStageAction_ReleaseLanterns.preliminaryTicks.RandomInRange,
						pawn = current,
						woodCost = ritualStageAction_ReleaseLanterns.woodCost
					};
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally1();
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
			((IDisposable)_003C_003E7__wrap2/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<ActionOnTick> IEnumerable<ActionOnTick>.GetEnumerator()
		{
			_003CGenerateTimedActions_003Ed__2 _003CGenerateTimedActions_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGenerateTimedActions_003Ed__ = this;
			}
			else
			{
				_003CGenerateTimedActions_003Ed__ = new _003CGenerateTimedActions_003Ed__2(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGenerateTimedActions_003Ed__.ritual = _003C_003E3__ritual;
			_003CGenerateTimedActions_003Ed__.stage = _003C_003E3__stage;
			return _003CGenerateTimedActions_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<ActionOnTick>)this).GetEnumerator();
		}
	}

	public int woodCost = 4;

	public IntRange preliminaryTicks;

	[IteratorStateMachine(typeof(_003CGenerateTimedActions_003Ed__2))]
	public override IEnumerable<ActionOnTick> GenerateTimedActions(LordJob_Ritual ritual, RitualStage stage)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGenerateTimedActions_003Ed__2(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__ritual = ritual,
			_003C_003E3__stage = stage
		};
	}
}
