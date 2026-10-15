using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_GiveToPackAnimal : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__7 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_GiveToPackAnimal _003C_003E4__this;

		private Toil _003CfindNearestCarrier_003E5__2;

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
			_003CfindNearestCarrier_003E5__2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			JobDriver_GiveToPackAnimal CS_0024_003C_003E8__locals5 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Haul.StartCarryThing(TargetIndex.A);
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003CfindNearestCarrier_003E5__2 = CS_0024_003C_003E8__locals5.FindCarrierToil();
				_003C_003E2__current = _003CfindNearestCarrier_003E5__2;
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.Touch).FailOnDespawnedNullOrForbidden(TargetIndex.B).JumpIf(() => !CS_0024_003C_003E8__locals5.CanCarryAtLeastOne(CS_0024_003C_003E8__locals5.Animal), _003CfindNearestCarrier_003E5__2);
				_003C_003E1__state = 4;
				return true;
			case 4:
				_003C_003E1__state = -1;
				_003C_003E2__current = CS_0024_003C_003E8__locals5.GiveToCarrierAsMuchAsPossibleToil();
				_003C_003E1__state = 5;
				return true;
			case 5:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Jump.JumpIf(_003CfindNearestCarrier_003E5__2, () => CS_0024_003C_003E8__locals5.pawn.carryTracker.CarriedThing != null);
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

	private const TargetIndex ItemInd = TargetIndex.A;

	private const TargetIndex AnimalInd = TargetIndex.B;

	private Thing Item => job.GetTarget(TargetIndex.A).Thing;

	private Pawn Animal => (Pawn)job.GetTarget(TargetIndex.B).Thing;

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		return pawn.Reserve(Item, job, 1, -1, null, errorOnFailed);
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

	private Toil FindCarrierToil()
	{
		Toil toil = ToilMaker.MakeToil("FindCarrierToil");
		toil.initAction = delegate
		{
			Pawn pawn = FindCarrier();
			if (pawn == null)
			{
				base.pawn.jobs.EndCurrentJob(JobCondition.Incompletable);
			}
			else
			{
				job.SetTarget(TargetIndex.B, pawn);
			}
		};
		return toil;
	}

	private Pawn FindCarrier()
	{
		IEnumerable<Pawn> enumerable = GiveToPackAnimalUtility.CarrierCandidatesFor(base.pawn);
		Pawn animal = Animal;
		if (animal != null && enumerable.Contains(animal) && animal.RaceProps.packAnimal && CanCarryAtLeastOne(animal))
		{
			return animal;
		}
		Pawn pawn = null;
		float num = -1f;
		foreach (Pawn item in enumerable)
		{
			if (item.RaceProps.packAnimal && CanCarryAtLeastOne(item))
			{
				float num2 = item.Position.DistanceToSquared(base.pawn.Position);
				if (pawn == null || num2 < num)
				{
					pawn = item;
					num = num2;
				}
			}
		}
		return pawn;
	}

	private bool CanCarryAtLeastOne(Pawn carrier)
	{
		return !MassUtility.WillBeOverEncumberedAfterPickingUp(carrier, Item, 1);
	}

	private Toil GiveToCarrierAsMuchAsPossibleToil()
	{
		Toil toil = ToilMaker.MakeToil("GiveToCarrierAsMuchAsPossibleToil");
		toil.initAction = delegate
		{
			if (Item == null)
			{
				pawn.jobs.EndCurrentJob(JobCondition.Incompletable);
			}
			else
			{
				int count = Mathf.Min(MassUtility.CountToPickUpUntilOverEncumbered(Animal, Item), Item.stackCount);
				pawn.carryTracker.innerContainer.TryTransferToContainer(Item, Animal.inventory.innerContainer, count);
			}
		};
		return toil;
	}
}
