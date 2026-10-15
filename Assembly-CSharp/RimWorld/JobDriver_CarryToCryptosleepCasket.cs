using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_CarryToCryptosleepCasket : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__7 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_CarryToCryptosleepCasket _003C_003E4__this;

		private Toil _003CgoToTakee_003E5__2;

		private Toil _003CstartCarryingTakee_003E5__3;

		private Toil _003CgoToThing_003E5__4;

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
		public _003CMakeNewToils_003Ed__7(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003CgoToTakee_003E5__2 = null;
			_003CstartCarryingTakee_003E5__3 = null;
			_003CgoToThing_003E5__4 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			JobDriver_CarryToCryptosleepCasket CS_0024_003C_003E8__locals13 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				CS_0024_003C_003E8__locals13.FailOnDestroyedOrNull(TargetIndex.A);
				CS_0024_003C_003E8__locals13.FailOnDestroyedOrNull(TargetIndex.B);
				CS_0024_003C_003E8__locals13.FailOnAggroMentalState(TargetIndex.A);
				CS_0024_003C_003E8__locals13.FailOn(() => !CS_0024_003C_003E8__locals13.DropPod.Accepts(CS_0024_003C_003E8__locals13.Takee));
				_003CgoToTakee_003E5__2 = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.OnCell).FailOnDestroyedNullOrForbidden(TargetIndex.A).FailOnDespawnedNullOrForbidden(TargetIndex.B)
					.FailOn(() => CS_0024_003C_003E8__locals13.DropPod.GetDirectlyHeldThings().Count > 0)
					.FailOn(() => !CS_0024_003C_003E8__locals13.pawn.CanReach(CS_0024_003C_003E8__locals13.Takee, PathEndMode.OnCell, Danger.Deadly))
					.FailOnSomeonePhysicallyInteracting(TargetIndex.A);
				_003CstartCarryingTakee_003E5__3 = Toils_Haul.StartCarryThing(TargetIndex.A);
				_003CgoToThing_003E5__4 = Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.InteractionCell);
				_003C_003E2__current = Toils_Jump.JumpIf(_003CgoToThing_003E5__4, () => CS_0024_003C_003E8__locals13.pawn.IsCarryingPawn(CS_0024_003C_003E8__locals13.Takee));
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = _003CgoToTakee_003E5__2;
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = _003CstartCarryingTakee_003E5__3;
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				_003C_003E2__current = _003CgoToThing_003E5__4;
				_003C_003E1__state = 4;
				return true;
			case 4:
			{
				_003C_003E1__state = -1;
				Toil toil2 = Toils_General.Wait(500, TargetIndex.B);
				toil2.FailOnCannotTouch(TargetIndex.B, PathEndMode.InteractionCell);
				toil2.WithProgressBarToilDelay(TargetIndex.B);
				_003C_003E2__current = toil2;
				_003C_003E1__state = 5;
				return true;
			}
			case 5:
			{
				_003C_003E1__state = -1;
				Toil toil = ToilMaker.MakeToil("MakeNewToils");
				toil.initAction = delegate
				{
					CS_0024_003C_003E8__locals13.DropPod.TryAcceptThing(CS_0024_003C_003E8__locals13.Takee);
				};
				toil.defaultCompleteMode = ToilCompleteMode.Instant;
				_003C_003E2__current = toil;
				_003C_003E1__state = 6;
				return true;
			}
			case 6:
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
			_003CMakeNewToils_003Ed__7 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__7(0)
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

	private const TargetIndex TakeeInd = TargetIndex.A;

	private const TargetIndex DropPodInd = TargetIndex.B;

	protected Pawn Takee => (Pawn)job.GetTarget(TargetIndex.A).Thing;

	protected Building_CryptosleepCasket DropPod => (Building_CryptosleepCasket)job.GetTarget(TargetIndex.B).Thing;

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		if (pawn.Reserve(Takee, job, 1, -1, null, errorOnFailed))
		{
			return pawn.Reserve(DropPod, job, 1, -1, null, errorOnFailed);
		}
		return false;
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__7))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__7(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override object[] TaleParameters()
	{
		return new object[2] { pawn, Takee };
	}
}
