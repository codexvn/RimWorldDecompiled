using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld;

namespace Verse.AI;

public class JobDriver_PickupToHold : JobDriver
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass2_0
	{
		public JobDriver driver;

		public TargetIndex HeldItem;

		internal bool _003CToils_003Eb__0()
		{
			return !driver.GetActor().health.capacities.CapableOf(PawnCapacityDefOf.Manipulation);
		}

		internal bool _003CToils_003Eb__1()
		{
			return driver.GetActor().IsCarryingThing(driver.GetActor().CurJob.GetTarget(HeldItem).Thing);
		}
	}

	[CompilerGenerated]
	private sealed class _003CToils_003Ed__2 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private JobDriver driver;

		public JobDriver _003C_003E3__driver;

		private TargetIndex HeldItem;

		public TargetIndex _003C_003E3__HeldItem;

		private _003C_003Ec__DisplayClass2_0 _003C_003E8__1;

		private bool subtractNumTakenFromJobCount;

		public bool _003C_003E3__subtractNumTakenFromJobCount;

		private Toil _003Cend_003E5__2;

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
		public _003CToils_003Ed__2(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E8__1 = null;
			_003Cend_003E5__2 = null;
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
				_003C_003E8__1 = new _003C_003Ec__DisplayClass2_0();
				_003C_003E8__1.driver = driver;
				_003C_003E8__1.HeldItem = HeldItem;
				_003C_003E8__1.driver.FailOn(() => !_003C_003E8__1.driver.GetActor().health.capacities.CapableOf(PawnCapacityDefOf.Manipulation));
				_003C_003E8__1.driver.FailOnDestroyedOrNull(_003C_003E8__1.HeldItem);
				_003C_003E8__1.driver.FailOnForbidden(_003C_003E8__1.HeldItem);
				_003Cend_003E5__2 = Toils_General.Label();
				_003C_003E2__current = Toils_Jump.JumpIf(_003Cend_003E5__2, () => _003C_003E8__1.driver.GetActor().IsCarryingThing(_003C_003E8__1.driver.GetActor().CurJob.GetTarget(_003C_003E8__1.HeldItem).Thing));
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Goto.GotoThing(_003C_003E8__1.HeldItem, PathEndMode.ClosestTouch).FailOnSomeonePhysicallyInteracting(_003C_003E8__1.HeldItem);
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Haul.StartCarryThing(_003C_003E8__1.HeldItem, putRemainderInQueue: false, subtractNumTakenFromJobCount);
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				_003C_003E2__current = _003Cend_003E5__2;
				_003C_003E1__state = 4;
				return true;
			case 4:
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
			_003CToils_003Ed__2 _003CToils_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CToils_003Ed__ = this;
			}
			else
			{
				_003CToils_003Ed__ = new _003CToils_003Ed__2(0);
			}
			_003CToils_003Ed__.driver = _003C_003E3__driver;
			_003CToils_003Ed__.HeldItem = _003C_003E3__HeldItem;
			_003CToils_003Ed__.subtractNumTakenFromJobCount = _003C_003E3__subtractNumTakenFromJobCount;
			return _003CToils_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Toil>)this).GetEnumerator();
		}
	}

	private const TargetIndex HeldItemInd = TargetIndex.A;

	public static bool TryMakePreToilReservations(JobDriver driver, bool errorOnFailed)
	{
		driver.pawn.Map.pawnDestinationReservationManager.Reserve(driver.pawn, driver.job, driver.job.GetTarget(TargetIndex.A).Cell);
		return driver.pawn.Reserve(driver.job.GetTarget(TargetIndex.A), driver.job, 1, -1, null, errorOnFailed);
	}

	[IteratorStateMachine(typeof(_003CToils_003Ed__2))]
	public static IEnumerable<Toil> Toils(JobDriver driver, TargetIndex HeldItem = TargetIndex.A, bool subtractNumTakenFromJobCount = true)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CToils_003Ed__2(-2)
		{
			_003C_003E3__driver = driver,
			_003C_003E3__HeldItem = HeldItem,
			_003C_003E3__subtractNumTakenFromJobCount = subtractNumTakenFromJobCount
		};
	}

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		return TryMakePreToilReservations(this, errorOnFailed);
	}

	protected override IEnumerable<Toil> MakeNewToils()
	{
		return Toils(this);
	}
}
