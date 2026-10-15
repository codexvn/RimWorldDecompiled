using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_SelfDetonate : JobDriver
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass1_0
	{
		public Toil warmupToil;

		public OverlayHandle? overlayBurningWick;

		internal void _003CMakeNewToils_003Eb__0()
		{
			Pawn actor = warmupToil.actor;
			actor.Map.overlayDrawer.Disable(actor, ref overlayBurningWick);
			overlayBurningWick = actor.Map.overlayDrawer.Enable(actor, OverlayTypes.BurningWick);
		}

		internal void _003CMakeNewToils_003Eb__1()
		{
			Pawn actor = warmupToil.actor;
			actor.Map.overlayDrawer.Disable(actor, ref overlayBurningWick);
		}

		internal void _003CMakeNewToils_003Eb__2()
		{
			warmupToil.actor.Kill(null);
		}
	}

	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__1 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private _003C_003Ec__DisplayClass1_0 _003C_003E8__1;

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
			_003C_003E8__1 = null;
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
				_003C_003E8__1 = new _003C_003Ec__DisplayClass1_0();
				_003C_003E8__1.overlayBurningWick = null;
				_003C_003E8__1.warmupToil = Toils_General.Wait(120);
				_003C_003E8__1.warmupToil.initAction = delegate
				{
					Pawn actor = _003C_003E8__1.warmupToil.actor;
					actor.Map.overlayDrawer.Disable(actor, ref _003C_003E8__1.overlayBurningWick);
					_003C_003E8__1.overlayBurningWick = actor.Map.overlayDrawer.Enable(actor, OverlayTypes.BurningWick);
				};
				_003C_003E8__1.warmupToil.AddFinishAction(delegate
				{
					Pawn actor2 = _003C_003E8__1.warmupToil.actor;
					actor2.Map.overlayDrawer.Disable(actor2, ref _003C_003E8__1.overlayBurningWick);
				});
				_003C_003E8__1.warmupToil.PlaySustainerOrSound(SoundDefOf.HissSmall);
				_003C_003E2__current = _003C_003E8__1.warmupToil;
				_003C_003E1__state = 1;
				return true;
			case 1:
			{
				_003C_003E1__state = -1;
				Toil toil = Toils_General.Do(delegate
				{
					_003C_003E8__1.warmupToil.actor.Kill(null);
				});
				_003C_003E2__current = toil;
				_003C_003E1__state = 2;
				return true;
			}
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
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				return this;
			}
			return new _003CMakeNewToils_003Ed__1(0);
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
		return new _003CMakeNewToils_003Ed__1(-2);
	}
}
