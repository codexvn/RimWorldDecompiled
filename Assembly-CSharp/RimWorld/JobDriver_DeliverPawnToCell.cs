using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld;

public class JobDriver_DeliverPawnToCell : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__5 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_DeliverPawnToCell _003C_003E4__this;

		private Toil _003CstartCarrying_003E5__2;

		private Toil _003CgotoCell_003E5__3;

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
		public _003CMakeNewToils_003Ed__5(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003CstartCarrying_003E5__2 = null;
			_003CgotoCell_003E5__3 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			JobDriver_DeliverPawnToCell CS_0024_003C_003E8__locals13 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				CS_0024_003C_003E8__locals13.FailOnDestroyedOrNull(TargetIndex.A);
				CS_0024_003C_003E8__locals13.FailOnAggroMentalStateAndHostile(TargetIndex.A);
				Toil toil = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.OnCell).FailOnDespawnedOrNull(TargetIndex.A).FailOn(() => !CS_0024_003C_003E8__locals13.pawn.CanReach(CS_0024_003C_003E8__locals13.job.GetTarget(TargetIndex.B), PathEndMode.Touch, Danger.Deadly))
					.FailOnSomeonePhysicallyInteracting(TargetIndex.A);
				_003C_003E2__current = toil;
				_003C_003E1__state = 1;
				return true;
			}
			case 1:
				_003C_003E1__state = -1;
				_003CstartCarrying_003E5__2 = Toils_Haul.StartCarryThing(TargetIndex.A);
				_003CgotoCell_003E5__3 = Toils_Goto.GotoCell(TargetIndex.B, PathEndMode.OnCell);
				_003C_003E2__current = Toils_Jump.JumpIf(_003CgotoCell_003E5__3, () => CS_0024_003C_003E8__locals13.pawn.IsCarryingPawn(CS_0024_003C_003E8__locals13.Takee));
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = _003CstartCarrying_003E5__2;
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				_003C_003E2__current = _003CgotoCell_003E5__3;
				_003C_003E1__state = 4;
				return true;
			case 4:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_General.Do(delegate
				{
					if (!CS_0024_003C_003E8__locals13.job.ritualTag.NullOrEmpty())
					{
						if (CS_0024_003C_003E8__locals13.Takee.GetLord()?.LordJob is LordJob_Ritual lordJob_Ritual)
						{
							lordJob_Ritual.AddTagForPawn(CS_0024_003C_003E8__locals13.Takee, CS_0024_003C_003E8__locals13.job.ritualTag);
						}
						if (CS_0024_003C_003E8__locals13.pawn.GetLord()?.LordJob is LordJob_Ritual lordJob_Ritual2)
						{
							lordJob_Ritual2.AddTagForPawn(CS_0024_003C_003E8__locals13.pawn, CS_0024_003C_003E8__locals13.job.ritualTag);
						}
					}
				});
				_003C_003E1__state = 5;
				return true;
			case 5:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Haul.PlaceHauledThingInCell(TargetIndex.B, null, storageMode: false);
				_003C_003E1__state = 6;
				return true;
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
			_003CMakeNewToils_003Ed__5 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__5(0)
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

	private const TargetIndex TakeeIndex = TargetIndex.A;

	private const TargetIndex TargetCellIndex = TargetIndex.B;

	protected Pawn Takee => (Pawn)job.GetTarget(TargetIndex.A).Thing;

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		base.Map.reservationManager.ReleaseAllForTarget(Takee);
		return pawn.Reserve(Takee, job, 1, -1, null, errorOnFailed);
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__5))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__5(-2)
		{
			_003C_003E4__this = this
		};
	}
}
