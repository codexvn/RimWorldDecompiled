using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_NociosphereDepart : JobDriver
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass2_0
	{
		public Toil toil;

		internal void _003CMakeNewToils_003Eb__1()
		{
			Messages.Message(toil.actor.GetComp<CompNociosphere>().Props.departingMessage, toil.actor, MessageTypeDefOf.NeutralEvent);
		}

		internal void _003CMakeNewToils_003Eb__2()
		{
			toil.actor.GetComp<CompActivity>()?.EnterPassiveState();
		}

		internal void _003CMakeNewToils_003Eb__0()
		{
			EffecterDefOf.Skip_Entry.SpawnMaintained(toil.actor.PositionHeld, toil.actor.MapHeld, 2f);
			EffecterDefOf.NociosphereDepartComplete.Spawn(toil.actor.PositionHeld, toil.actor.MapHeld).Cleanup();
		}
	}

	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__2 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private _003C_003Ec__DisplayClass2_0 _003C_003E8__1;

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
		public _003CMakeNewToils_003Ed__2(int _003C_003E1__state)
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
			{
				_003C_003E1__state = -1;
				_003C_003E8__1 = new _003C_003Ec__DisplayClass2_0();
				_003C_003E8__1.toil = Toils_General.Wait(LeaveTimeTicks);
				_003C_003E8__1.toil.WithEffect(EffecterDefOf.NociosphereDeparting, TargetIndex.A, null);
				_003C_003E8__1.toil.PlaySustainerOrSound(SoundDefOf.Pawn_Nociosphere_Departing);
				Toil toil2 = _003C_003E8__1.toil;
				toil2.initAction = (Action)Delegate.Combine(toil2.initAction, (Action)delegate
				{
					Messages.Message(_003C_003E8__1.toil.actor.GetComp<CompNociosphere>().Props.departingMessage, _003C_003E8__1.toil.actor, MessageTypeDefOf.NeutralEvent);
				});
				_003C_003E2__current = _003C_003E8__1.toil;
				_003C_003E1__state = 1;
				return true;
			}
			case 1:
			{
				_003C_003E1__state = -1;
				Toil toil = ToilMaker.MakeToil("MakeNewToils");
				toil.initAction = (Action)Delegate.Combine(toil.initAction, (Action)delegate
				{
					_003C_003E8__1.toil.actor.GetComp<CompActivity>()?.EnterPassiveState();
				});
				toil.AddFinishAction(delegate
				{
					EffecterDefOf.Skip_Entry.SpawnMaintained(_003C_003E8__1.toil.actor.PositionHeld, _003C_003E8__1.toil.actor.MapHeld, 2f);
					EffecterDefOf.NociosphereDepartComplete.Spawn(_003C_003E8__1.toil.actor.PositionHeld, _003C_003E8__1.toil.actor.MapHeld).Cleanup();
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
			return new _003CMakeNewToils_003Ed__2(0);
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Toil>)this).GetEnumerator();
		}
	}

	private static readonly int LeaveTimeTicks = 720;

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		return true;
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__2))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__2(-2);
	}
}
