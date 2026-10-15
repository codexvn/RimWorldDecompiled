using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class CompTargetable_SingleCorpse : CompTargetable
{
	[CompilerGenerated]
	private sealed class _003CGetTargets_003Ed__3 : IEnumerable<Thing>, IEnumerable, IEnumerator<Thing>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Thing _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Thing targetChosenByPlayer;

		public Thing _003C_003E3__targetChosenByPlayer;

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
		public _003CGetTargets_003Ed__3(int _003C_003E1__state)
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
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E2__current = targetChosenByPlayer;
				_003C_003E1__state = 1;
				return true;
			case 1:
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
		IEnumerator<Thing> IEnumerable<Thing>.GetEnumerator()
		{
			_003CGetTargets_003Ed__3 _003CGetTargets_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetTargets_003Ed__ = this;
			}
			else
			{
				_003CGetTargets_003Ed__ = new _003CGetTargets_003Ed__3(0);
			}
			_003CGetTargets_003Ed__.targetChosenByPlayer = _003C_003E3__targetChosenByPlayer;
			return _003CGetTargets_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Thing>)this).GetEnumerator();
		}
	}

	protected override bool PlayerChoosesTarget => true;

	protected override TargetingParameters GetTargetingParameters()
	{
		return new TargetingParameters
		{
			canTargetPawns = false,
			canTargetBuildings = false,
			canTargetItems = false,
			canTargetCorpses = true,
			mapObjectTargetsMustBeAutoAttackable = false
		};
	}

	[IteratorStateMachine(typeof(_003CGetTargets_003Ed__3))]
	public override IEnumerable<Thing> GetTargets(Thing targetChosenByPlayer = null)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetTargets_003Ed__3(-2)
		{
			_003C_003E3__targetChosenByPlayer = targetChosenByPlayer
		};
	}

	public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
	{
		if (target.Thing is Corpse)
		{
			return base.ValidateTarget(target.Thing, showMessages);
		}
		return false;
	}
}
