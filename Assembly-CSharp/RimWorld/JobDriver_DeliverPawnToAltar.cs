using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_DeliverPawnToAltar : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__8 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_DeliverPawnToAltar _003C_003E4__this;

		private Toil _003CstartCarrying_003E5__2;

		private Toil _003CgoToAltar_003E5__3;

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
		public _003CMakeNewToils_003Ed__8(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003CstartCarrying_003E5__2 = null;
			_003CgoToAltar_003E5__3 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			JobDriver_DeliverPawnToAltar CS_0024_003C_003E8__locals16 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				if (!ModLister.CheckIdeology("Deliver to altar job"))
				{
					return false;
				}
				CS_0024_003C_003E8__locals16.FailOnDestroyedOrNull(TargetIndex.A);
				CS_0024_003C_003E8__locals16.FailOnDestroyedOrNull(TargetIndex.C);
				CS_0024_003C_003E8__locals16.FailOnAggroMentalStateAndHostile(TargetIndex.A);
				Toil toil = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOnDespawnedNullOrForbidden(TargetIndex.A).FailOnDespawnedNullOrForbidden(TargetIndex.C)
					.FailOn(() => !CS_0024_003C_003E8__locals16.pawn.CanReach(CS_0024_003C_003E8__locals16.DropAltar, PathEndMode.OnCell, Danger.Deadly))
					.FailOnSomeonePhysicallyInteracting(TargetIndex.A);
				_003C_003E2__current = toil;
				_003C_003E1__state = 1;
				return true;
			}
			case 1:
				_003C_003E1__state = -1;
				_003CstartCarrying_003E5__2 = Toils_Haul.StartCarryThing(TargetIndex.A);
				_003CgoToAltar_003E5__3 = Toils_Goto.GotoThing(TargetIndex.C, PathEndMode.ClosestTouch);
				_003C_003E2__current = Toils_Jump.JumpIf(_003CgoToAltar_003E5__3, () => CS_0024_003C_003E8__locals16.pawn.IsCarryingPawn(CS_0024_003C_003E8__locals16.Takee));
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = _003CstartCarrying_003E5__2;
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				_003C_003E2__current = _003CgoToAltar_003E5__3;
				_003C_003E1__state = 4;
				return true;
			case 4:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_General.Do(delegate
				{
					IntVec3 intVec = CS_0024_003C_003E8__locals16.DropAltar.Position;
					if (CS_0024_003C_003E8__locals16.DropAltar.def.hasInteractionCell)
					{
						IntVec3 interactionCell = CS_0024_003C_003E8__locals16.DropAltar.InteractionCell;
						IntVec3 intVec2 = (CS_0024_003C_003E8__locals16.DropAltar.Position - interactionCell).ClampInsideRect(new CellRect(-1, -1, 3, 3));
						intVec = interactionCell + intVec2;
					}
					else if (CS_0024_003C_003E8__locals16.DropAltar.def.Size.z % 2 != 0)
					{
						intVec = CS_0024_003C_003E8__locals16.DropAltar.Position + new IntVec3(0, 0, -CS_0024_003C_003E8__locals16.DropAltar.def.Size.z / 2).RotatedBy(CS_0024_003C_003E8__locals16.DropAltar.Rotation);
					}
					CS_0024_003C_003E8__locals16.job.SetTarget(TargetIndex.B, intVec);
				});
				_003C_003E1__state = 5;
				return true;
			case 5:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Reserve.Release(TargetIndex.C);
				_003C_003E1__state = 6;
				return true;
			case 6:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Haul.PlaceHauledThingInCell(TargetIndex.B, null, storageMode: false);
				_003C_003E1__state = 7;
				return true;
			case 7:
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
			_003CMakeNewToils_003Ed__8 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__8(0)
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

	private const TargetIndex AltarIndex = TargetIndex.C;

	protected Pawn Takee => (Pawn)job.GetTarget(TargetIndex.A).Thing;

	protected Building DropAltar => (Building)job.GetTarget(TargetIndex.C).Thing;

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		base.Map.reservationManager.ReleaseAllForTarget(Takee);
		if (pawn.Reserve(Takee, job, 1, -1, null, errorOnFailed))
		{
			return pawn.Reserve(DropAltar, job, 1, 0, null, errorOnFailed);
		}
		return false;
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__8))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__8(-2)
		{
			_003C_003E4__this = this
		};
	}
}
