using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_RefuelAtomic : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__11 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_RefuelAtomic _003C_003E4__this;

		private Toil _003CgetNextIngredient_003E5__2;

		private Toil _003CfindPlaceTarget_003E5__3;

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
		public _003CMakeNewToils_003Ed__11(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003CgetNextIngredient_003E5__2 = null;
			_003CfindPlaceTarget_003E5__3 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			JobDriver_RefuelAtomic CS_0024_003C_003E8__locals13 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				CS_0024_003C_003E8__locals13.FailOnDespawnedNullOrForbidden(TargetIndex.A);
				CS_0024_003C_003E8__locals13.AddEndCondition(() => (!CS_0024_003C_003E8__locals13.RefuelableComp.IsFull) ? JobCondition.Ongoing : JobCondition.Succeeded);
				CS_0024_003C_003E8__locals13.AddFailCondition(() => (!CS_0024_003C_003E8__locals13.job.playerForced && !CS_0024_003C_003E8__locals13.RefuelableComp.ShouldAutoRefuelNowIgnoringFuelPct) || !CS_0024_003C_003E8__locals13.RefuelableComp.allowAutoRefuel);
				CS_0024_003C_003E8__locals13.AddFailCondition(() => !CS_0024_003C_003E8__locals13.RefuelableComp.allowAutoRefuel && !CS_0024_003C_003E8__locals13.job.playerForced);
				_003C_003E2__current = Toils_General.DoAtomic(delegate
				{
					CS_0024_003C_003E8__locals13.job.count = CS_0024_003C_003E8__locals13.RefuelableComp.GetFuelCountToFullyRefuel();
				});
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003CgetNextIngredient_003E5__2 = Toils_General.Label();
				_003C_003E2__current = _003CgetNextIngredient_003E5__2;
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_JobTransforms.ExtractNextTargetFromQueue(TargetIndex.B);
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.ClosestTouch).FailOnDespawnedNullOrForbidden(TargetIndex.B).FailOnSomeonePhysicallyInteracting(TargetIndex.B);
				_003C_003E1__state = 4;
				return true;
			case 4:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Haul.StartCarryThing(TargetIndex.B, putRemainderInQueue: false, subtractNumTakenFromJobCount: true).FailOnDestroyedNullOrForbidden(TargetIndex.B);
				_003C_003E1__state = 5;
				return true;
			case 5:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
				_003C_003E1__state = 6;
				return true;
			case 6:
				_003C_003E1__state = -1;
				_003CfindPlaceTarget_003E5__3 = Toils_JobTransforms.SetTargetToIngredientPlaceCell(TargetIndex.A, TargetIndex.B, TargetIndex.C);
				_003C_003E2__current = _003CfindPlaceTarget_003E5__3;
				_003C_003E1__state = 7;
				return true;
			case 7:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Haul.PlaceHauledThingInCell(TargetIndex.C, _003CfindPlaceTarget_003E5__3, storageMode: false);
				_003C_003E1__state = 8;
				return true;
			case 8:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Jump.JumpIf(_003CgetNextIngredient_003E5__2, () => !CS_0024_003C_003E8__locals13.job.GetTargetQueue(TargetIndex.B).NullOrEmpty());
				_003C_003E1__state = 9;
				return true;
			case 9:
				_003C_003E1__state = -1;
				_003CfindPlaceTarget_003E5__3 = null;
				_003C_003E2__current = Toils_General.Wait(240).FailOnDestroyedNullOrForbidden(TargetIndex.A).FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch)
					.WithProgressBarToilDelay(TargetIndex.A);
				_003C_003E1__state = 10;
				return true;
			case 10:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Refuel.FinalizeRefueling(TargetIndex.A, TargetIndex.None);
				_003C_003E1__state = 11;
				return true;
			case 11:
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
			_003CMakeNewToils_003Ed__11 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__11(0)
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

	private const TargetIndex RefuelableInd = TargetIndex.A;

	private const TargetIndex FuelInd = TargetIndex.B;

	private const TargetIndex FuelPlaceCellInd = TargetIndex.C;

	private const int RefuelingDuration = 240;

	protected Thing Refuelable => job.GetTarget(TargetIndex.A).Thing;

	protected CompRefuelable RefuelableComp => Refuelable.TryGetComp<CompRefuelable>();

	protected Thing Fuel => job.GetTarget(TargetIndex.B).Thing;

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		pawn.ReserveAsManyAsPossible(job.GetTargetQueue(TargetIndex.B), job);
		return pawn.Reserve(Refuelable, job, 1, -1, null, errorOnFailed);
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__11))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__11(-2)
		{
			_003C_003E4__this = this
		};
	}
}
