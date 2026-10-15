using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_GiveToPawn : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__9 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_GiveToPawn _003C_003E4__this;

		private Toil _003CsetItemTarget_003E5__2;

		private Toil _003Creserve_003E5__3;

		private Toil _003CcheckForDuplicates_003E5__4;

		private Toil _003ChaulToContainer_003E5__5;

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
			_003CsetItemTarget_003E5__2 = null;
			_003Creserve_003E5__3 = null;
			_003CcheckForDuplicates_003E5__4 = null;
			_003ChaulToContainer_003E5__5 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			JobDriver_GiveToPawn CS_0024_003C_003E8__locals13 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				CS_0024_003C_003E8__locals13.FailOnDestroyedNullOrForbidden(TargetIndex.B);
				CS_0024_003C_003E8__locals13.FailOn(() => !GiveItemsToPawnUtility.IsWaitingForItems(CS_0024_003C_003E8__locals13.Receiver));
				_003CsetItemTarget_003E5__2 = CS_0024_003C_003E8__locals13.SetItemTarget();
				_003C_003E2__current = _003CsetItemTarget_003E5__2;
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003Creserve_003E5__3 = Toils_Reserve.Reserve(TargetIndex.A).FailOnDespawnedOrNull(TargetIndex.A);
				_003C_003E2__current = _003Creserve_003E5__3;
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = CS_0024_003C_003E8__locals13.DetermineNumToHaul();
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOnDespawnedNullOrForbidden(TargetIndex.A);
				_003C_003E1__state = 4;
				return true;
			case 4:
				_003C_003E1__state = -1;
				_003CcheckForDuplicates_003E5__4 = Toils_Haul.CheckForGetOpportunityDuplicate(_003Creserve_003E5__3, TargetIndex.A, TargetIndex.None, takeFromValidStorage: true, (Thing x) => x.def == CS_0024_003C_003E8__locals13.Item.def);
				_003ChaulToContainer_003E5__5 = Toils_Haul.CarryHauledThingToContainer();
				_003C_003E2__current = Toils_Jump.JumpIf(_003ChaulToContainer_003E5__5, () => GiveItemsToPawnUtility.ItemCountLeftToCollect(CS_0024_003C_003E8__locals13.Receiver) <= 0);
				_003C_003E1__state = 5;
				return true;
			case 5:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Haul.StartCarryThing(TargetIndex.A);
				_003C_003E1__state = 6;
				return true;
			case 6:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Jump.JumpIf(_003ChaulToContainer_003E5__5, () => GiveItemsToPawnUtility.ItemCountLeftToCollect(CS_0024_003C_003E8__locals13.Receiver) <= 0);
				_003C_003E1__state = 7;
				return true;
			case 7:
				_003C_003E1__state = -1;
				_003C_003E2__current = _003CcheckForDuplicates_003E5__4;
				_003C_003E1__state = 8;
				return true;
			case 8:
				_003C_003E1__state = -1;
				_003C_003E2__current = _003ChaulToContainer_003E5__5;
				_003C_003E1__state = 9;
				return true;
			case 9:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Interpersonal.WaitToBeAbleToInteract(CS_0024_003C_003E8__locals13.pawn);
				_003C_003E1__state = 10;
				return true;
			case 10:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Interpersonal.GotoInteractablePosition(TargetIndex.B);
				_003C_003E1__state = 11;
				return true;
			case 11:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Haul.DepositHauledThingInContainer(TargetIndex.B, TargetIndex.A);
				_003C_003E1__state = 12;
				return true;
			case 12:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Jump.JumpIf(_003CsetItemTarget_003E5__2, () => GiveItemsToPawnUtility.ItemCountLeftToCollect(CS_0024_003C_003E8__locals13.Receiver) > 0);
				_003C_003E1__state = 13;
				return true;
			case 13:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_General.Do(delegate
				{
					QuestUtility.SendQuestTargetSignals(CS_0024_003C_003E8__locals13.Receiver.questTags, "ReceivedItems", CS_0024_003C_003E8__locals13.pawn.Named("GIVER"), CS_0024_003C_003E8__locals13.Receiver.Named("RECEIVER"));
				});
				_003C_003E1__state = 14;
				return true;
			case 14:
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

	private const TargetIndex ItemInd = TargetIndex.A;

	private const TargetIndex PawnInd = TargetIndex.B;

	private Thing Item => job.GetTarget(TargetIndex.A).Thing;

	private Pawn Receiver => job.GetTarget(TargetIndex.B).Pawn;

	public int CountBeingHauled
	{
		get
		{
			if (pawn.carryTracker.CarriedThing == null)
			{
				return 0;
			}
			return pawn.carryTracker.CarriedThing.stackCount;
		}
	}

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		return pawn.Reserve(Item, job, 1, -1, null, errorOnFailed);
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

	private Toil DetermineNumToHaul()
	{
		Toil toil = ToilMaker.MakeToil("DetermineNumToHaul");
		toil.initAction = delegate
		{
			int num = GiveItemsToPawnUtility.ItemCountLeftToCollect(Receiver);
			if (num <= 0)
			{
				pawn.jobs.EndCurrentJob(JobCondition.Succeeded);
			}
			else
			{
				job.count = num;
			}
		};
		toil.defaultCompleteMode = ToilCompleteMode.Instant;
		toil.atomicWithPrevious = true;
		return toil;
	}

	private Toil SetItemTarget()
	{
		Toil toil = ToilMaker.MakeToil("SetItemTarget");
		toil.initAction = delegate
		{
			Thing thing = GiveItemsToPawnUtility.FindItemToGive(pawn, Item.def);
			if (thing == null)
			{
				pawn.jobs.EndCurrentJob(JobCondition.Incompletable);
			}
			else
			{
				job.SetTarget(TargetIndex.A, thing);
			}
		};
		return toil;
	}
}
