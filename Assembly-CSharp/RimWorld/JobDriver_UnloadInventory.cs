using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_UnloadInventory : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__7 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_UnloadInventory _003C_003E4__this;

		private Toil _003CcarryToCell_003E5__2;

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
			_003CcarryToCell_003E5__2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			JobDriver_UnloadInventory CS_0024_003C_003E8__locals10 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				CS_0024_003C_003E8__locals10.FailOnDespawnedOrNull(TargetIndex.A);
				_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_General.Wait(10);
				_003C_003E1__state = 2;
				return true;
			case 2:
			{
				_003C_003E1__state = -1;
				Toil toil = ToilMaker.MakeToil("MakeNewToils");
				toil.initAction = delegate
				{
					Pawn otherPawn = CS_0024_003C_003E8__locals10.OtherPawn;
					if (!otherPawn.inventory.UnloadEverything)
					{
						CS_0024_003C_003E8__locals10.EndJobWith(JobCondition.Succeeded);
					}
					else
					{
						ThingCount firstUnloadableThing = otherPawn.inventory.FirstUnloadableThing;
						if (!firstUnloadableThing.Thing.def.EverStorable(willMinifyIfPossible: false) || !CS_0024_003C_003E8__locals10.pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation) || !StoreUtility.TryFindStoreCellNearColonyDesperate(firstUnloadableThing.Thing, CS_0024_003C_003E8__locals10.pawn, out var storeCell))
						{
							otherPawn.inventory.innerContainer.TryDrop(firstUnloadableThing.Thing, ThingPlaceMode.Near, firstUnloadableThing.Count, out var lastResultingThing);
							CS_0024_003C_003E8__locals10.EndJobWith(JobCondition.Succeeded);
							lastResultingThing?.SetForbidden(value: false, warnOnFail: false);
						}
						else
						{
							otherPawn.inventory.innerContainer.TryTransferToContainer(firstUnloadableThing.Thing, CS_0024_003C_003E8__locals10.pawn.carryTracker.innerContainer, firstUnloadableThing.Count, out var resultingTransferredItem);
							CS_0024_003C_003E8__locals10.job.count = resultingTransferredItem.stackCount;
							CS_0024_003C_003E8__locals10.job.SetTarget(TargetIndex.B, resultingTransferredItem);
							CS_0024_003C_003E8__locals10.job.SetTarget(TargetIndex.C, storeCell);
							firstUnloadableThing.Thing.SetForbidden(value: false, warnOnFail: false);
						}
						if (otherPawn.RaceProps.packAnimal && otherPawn.inventory.innerContainer.Count == 0)
						{
							otherPawn.Drawer.renderer.SetAllGraphicsDirty();
						}
					}
				};
				_003C_003E2__current = toil;
				_003C_003E1__state = 3;
				return true;
			}
			case 3:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Reserve.Reserve(TargetIndex.C);
				_003C_003E1__state = 4;
				return true;
			case 4:
				_003C_003E1__state = -1;
				_003CcarryToCell_003E5__2 = Toils_Haul.CarryHauledThingToCell(TargetIndex.C);
				_003C_003E2__current = _003CcarryToCell_003E5__2;
				_003C_003E1__state = 5;
				return true;
			case 5:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Haul.PlaceHauledThingInCell(TargetIndex.C, _003CcarryToCell_003E5__2, storageMode: true);
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

	private const TargetIndex OtherPawnInd = TargetIndex.A;

	private const TargetIndex ItemToHaulInd = TargetIndex.B;

	private const TargetIndex StoreCellInd = TargetIndex.C;

	private const int UnloadDuration = 10;

	private Pawn OtherPawn => job.GetTarget(TargetIndex.A).Pawn;

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		return pawn.Reserve(OtherPawn, job, 1, -1, null, errorOnFailed);
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
}
