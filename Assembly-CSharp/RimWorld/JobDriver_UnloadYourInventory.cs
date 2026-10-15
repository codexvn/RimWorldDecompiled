using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_UnloadYourInventory : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__6 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_UnloadYourInventory _003C_003E4__this;

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
		public _003CMakeNewToils_003Ed__6(int _003C_003E1__state)
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
			JobDriver_UnloadYourInventory CS_0024_003C_003E8__locals22 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_General.Wait(10);
				_003C_003E1__state = 1;
				return true;
			case 1:
			{
				_003C_003E1__state = -1;
				Toil toil2 = ToilMaker.MakeToil("MakeNewToils");
				toil2.initAction = delegate
				{
					if (!CS_0024_003C_003E8__locals22.pawn.inventory.UnloadEverything)
					{
						CS_0024_003C_003E8__locals22.EndJobWith(JobCondition.Succeeded);
					}
					else
					{
						ThingCount firstUnloadableThing = CS_0024_003C_003E8__locals22.pawn.inventory.FirstUnloadableThing;
						if (!StoreUtility.TryFindStoreCellNearColonyDesperate(firstUnloadableThing.Thing, CS_0024_003C_003E8__locals22.pawn, out var storeCell))
						{
							CS_0024_003C_003E8__locals22.pawn.inventory.innerContainer.TryDrop(firstUnloadableThing.Thing, ThingPlaceMode.Near, firstUnloadableThing.Count, out var _);
							CS_0024_003C_003E8__locals22.EndJobWith(JobCondition.Succeeded);
						}
						else
						{
							CS_0024_003C_003E8__locals22.job.SetTarget(TargetIndex.A, firstUnloadableThing.Thing);
							CS_0024_003C_003E8__locals22.job.SetTarget(TargetIndex.B, storeCell);
							CS_0024_003C_003E8__locals22.countToDrop = firstUnloadableThing.Count;
						}
					}
				};
				_003C_003E2__current = toil2;
				_003C_003E1__state = 2;
				return true;
			}
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Reserve.Reserve(TargetIndex.B);
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Goto.GotoCell(TargetIndex.B, PathEndMode.Touch);
				_003C_003E1__state = 4;
				return true;
			case 4:
			{
				_003C_003E1__state = -1;
				Toil toil = ToilMaker.MakeToil("MakeNewToils");
				toil.initAction = delegate
				{
					Thing resultingTransferredItem = CS_0024_003C_003E8__locals22.job.GetTarget(TargetIndex.A).Thing;
					if (resultingTransferredItem == null || !CS_0024_003C_003E8__locals22.pawn.inventory.innerContainer.Contains(resultingTransferredItem))
					{
						CS_0024_003C_003E8__locals22.EndJobWith(JobCondition.Incompletable);
					}
					else
					{
						if (!CS_0024_003C_003E8__locals22.pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation) || !resultingTransferredItem.def.EverStorable(willMinifyIfPossible: false))
						{
							CS_0024_003C_003E8__locals22.pawn.inventory.innerContainer.TryDrop(resultingTransferredItem, ThingPlaceMode.Near, CS_0024_003C_003E8__locals22.countToDrop, out resultingTransferredItem);
							CS_0024_003C_003E8__locals22.EndJobWith(JobCondition.Succeeded);
						}
						else
						{
							CS_0024_003C_003E8__locals22.pawn.inventory.innerContainer.TryTransferToContainer(resultingTransferredItem, CS_0024_003C_003E8__locals22.pawn.carryTracker.innerContainer, CS_0024_003C_003E8__locals22.countToDrop, out resultingTransferredItem);
							CS_0024_003C_003E8__locals22.job.count = CS_0024_003C_003E8__locals22.countToDrop;
							CS_0024_003C_003E8__locals22.job.SetTarget(TargetIndex.A, resultingTransferredItem);
						}
						resultingTransferredItem.SetForbidden(value: false, warnOnFail: false);
					}
				};
				_003C_003E2__current = toil;
				_003C_003E1__state = 5;
				return true;
			}
			case 5:
				_003C_003E1__state = -1;
				_003CcarryToCell_003E5__2 = Toils_Haul.CarryHauledThingToCell(TargetIndex.B);
				_003C_003E2__current = _003CcarryToCell_003E5__2;
				_003C_003E1__state = 6;
				return true;
			case 6:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Haul.PlaceHauledThingInCell(TargetIndex.B, _003CcarryToCell_003E5__2, storageMode: true);
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
			_003CMakeNewToils_003Ed__6 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__6(0)
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

	private int countToDrop = -1;

	private const TargetIndex ItemToHaulInd = TargetIndex.A;

	private const TargetIndex StoreCellInd = TargetIndex.B;

	private const int UnloadDuration = 10;

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref countToDrop, "countToDrop", -1);
	}

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		return true;
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__6))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__6(-2)
		{
			_003C_003E4__this = this
		};
	}
}
