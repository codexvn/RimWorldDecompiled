using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse.AI;

namespace Verse;

public class JobDriver_CastAbilityWorld : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__1 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_CastAbilityWorld _003C_003E4__this;

		Toil IEnumerator<Toil>.Current
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
		public _003CMakeNewToils_003Ed__1(int _003C_003E1__state)
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
			JobDriver_CastAbilityWorld CS_0024_003C_003E8__locals5 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_General.Do(delegate
				{
					CS_0024_003C_003E8__locals5.pawn.stances.SetStance(new Stance_WarmupAbilityWorld(CS_0024_003C_003E8__locals5.job.ability.def.verbProperties.warmupTime.SecondsToTicks(), null, CS_0024_003C_003E8__locals5.job.ability.verb));
				});
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_General.Do(delegate
				{
					CS_0024_003C_003E8__locals5.job.ability.Activate(CS_0024_003C_003E8__locals5.job.globalTarget);
				});
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				return false;
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
		IEnumerator<Toil> IEnumerable<Toil>.GetEnumerator()
		{
			_003CMakeNewToils_003Ed__1 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__1(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Toil>)this).GetEnumerator();
		}
	}

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		return true;
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__1))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__1(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override string GetReport()
	{
		return "UsingVerb".Translate(job.ability.def.label, job.globalTarget.Label);
	}

	public override void Notify_Starting()
	{
		base.Notify_Starting();
		job.ability?.Notify_StartedCasting();
	}
}
