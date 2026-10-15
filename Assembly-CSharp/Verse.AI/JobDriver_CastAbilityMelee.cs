using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld;

namespace Verse.AI;

public class JobDriver_CastAbilityMelee : JobDriver_CastAbility
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass0_0
	{
		public JobDriver_CastAbilityMelee _003C_003E4__this;

		public Ability ability;

		internal bool _003CMakeNewToils_003Eb__0()
		{
			if (!_003C_003E4__this.job.ability.CanCast)
			{
				return !_003C_003E4__this.job.ability.Casting;
			}
			return false;
		}

		internal bool _003CMakeNewToils_003Eb__1()
		{
			return !ability.CanApplyOn(_003C_003E4__this.job.targetA);
		}
	}

	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__0 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_CastAbilityMelee _003C_003E4__this;

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
		public _003CMakeNewToils_003Ed__0(int _003C_003E1__state)
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
			JobDriver_CastAbilityMelee jobDriver_CastAbilityMelee = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				_003C_003Ec__DisplayClass0_0 CS_0024_003C_003E8__locals5 = new _003C_003Ec__DisplayClass0_0
				{
					_003C_003E4__this = _003C_003E4__this
				};
				jobDriver_CastAbilityMelee.FailOnDespawnedOrNull(TargetIndex.A);
				jobDriver_CastAbilityMelee.FailOn(() => !CS_0024_003C_003E8__locals5._003C_003E4__this.job.ability.CanCast && !CS_0024_003C_003E8__locals5._003C_003E4__this.job.ability.Casting);
				CS_0024_003C_003E8__locals5.ability = ((Verb_CastAbility)jobDriver_CastAbilityMelee.job.verbToUse).ability;
				_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOn(() => !CS_0024_003C_003E8__locals5.ability.CanApplyOn(CS_0024_003C_003E8__locals5._003C_003E4__this.job.targetA));
				_003C_003E1__state = 1;
				return true;
			}
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Combat.CastVerb(TargetIndex.A, TargetIndex.B, canHitNonTargetPawns: false);
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
			_003CMakeNewToils_003Ed__0 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__0(0)
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

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__0))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__0(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void Notify_Starting()
	{
		base.Notify_Starting();
		job.ability?.Notify_StartedCasting();
	}
}
