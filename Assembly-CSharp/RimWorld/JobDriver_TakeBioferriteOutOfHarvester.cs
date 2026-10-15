using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_TakeBioferriteOutOfHarvester : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__9 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_TakeBioferriteOutOfHarvester _003C_003E4__this;

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
		public _003CMakeNewToils_003Ed__9(int _003C_003E1__state)
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
			JobDriver_TakeBioferriteOutOfHarvester CS_0024_003C_003E8__locals13 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				CS_0024_003C_003E8__locals13.FailOnDespawnedNullOrForbidden(TargetIndex.A);
				CS_0024_003C_003E8__locals13.FailOnBurningImmobile(TargetIndex.A);
				_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_General.Wait(200).FailOnDestroyedNullOrForbidden(TargetIndex.A).FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch)
					.FailOn(() => CS_0024_003C_003E8__locals13.Harvester.containedBioferrite < 1f)
					.WithProgressBarToilDelay(TargetIndex.A);
				_003C_003E1__state = 2;
				return true;
			case 2:
			{
				_003C_003E1__state = -1;
				Toil toil = ToilMaker.MakeToil("MakeNewToils");
				toil.initAction = delegate
				{
					Thing thing = CS_0024_003C_003E8__locals13.Harvester.TakeOutBioferrite();
					GenPlace.TryPlaceThing(thing, CS_0024_003C_003E8__locals13.pawn.Position, CS_0024_003C_003E8__locals13.Map, ThingPlaceMode.Near, null, null, null);
					StoragePriority currentPriority = StoreUtility.CurrentStoragePriorityOf(thing);
					if (StoreUtility.TryFindBestBetterStoreCellFor(thing, CS_0024_003C_003E8__locals13.pawn, CS_0024_003C_003E8__locals13.Map, currentPriority, CS_0024_003C_003E8__locals13.pawn.Faction, out var foundCell))
					{
						CS_0024_003C_003E8__locals13.job.SetTarget(TargetIndex.C, foundCell);
						CS_0024_003C_003E8__locals13.job.SetTarget(TargetIndex.B, thing);
						CS_0024_003C_003E8__locals13.job.count = thing.stackCount;
					}
					else
					{
						CS_0024_003C_003E8__locals13.EndJobWith(JobCondition.Incompletable);
					}
				};
				toil.defaultCompleteMode = ToilCompleteMode.Instant;
				_003C_003E2__current = toil;
				_003C_003E1__state = 3;
				return true;
			}
			case 3:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Reserve.Reserve(TargetIndex.B);
				_003C_003E1__state = 4;
				return true;
			case 4:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Reserve.Reserve(TargetIndex.C);
				_003C_003E1__state = 5;
				return true;
			case 5:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.ClosestTouch);
				_003C_003E1__state = 6;
				return true;
			case 6:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Haul.StartCarryThing(TargetIndex.B);
				_003C_003E1__state = 7;
				return true;
			case 7:
				_003C_003E1__state = -1;
				_003CcarryToCell_003E5__2 = Toils_Haul.CarryHauledThingToCell(TargetIndex.C);
				_003C_003E2__current = _003CcarryToCell_003E5__2;
				_003C_003E1__state = 8;
				return true;
			case 8:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Haul.PlaceHauledThingInCell(TargetIndex.C, _003CcarryToCell_003E5__2, storageMode: true);
				_003C_003E1__state = 9;
				return true;
			case 9:
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
			_003CMakeNewToils_003Ed__9 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__9(0)
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

	private const TargetIndex HarvesterInd = TargetIndex.A;

	private const TargetIndex BioferriteToHaulInd = TargetIndex.B;

	private const TargetIndex StorageCellInd = TargetIndex.C;

	private const int Duration = 200;

	protected Building_BioferriteHarvester Harvester => (Building_BioferriteHarvester)job.GetTarget(TargetIndex.A).Thing;

	protected Thing Bioferrite => job.GetTarget(TargetIndex.B).Thing;

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		return pawn.Reserve(Harvester, job, 1, -1, null, errorOnFailed);
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__9))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__9(-2)
		{
			_003C_003E4__this = this
		};
	}
}
