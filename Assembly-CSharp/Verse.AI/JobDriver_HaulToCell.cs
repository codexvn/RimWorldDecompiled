using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld;

namespace Verse.AI;

public class JobDriver_HaulToCell : JobDriver
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass13_0
	{
		public JobDriver_HaulToCell _003C_003E4__this;

		public Toil toilGoto;

		internal void _003CMakeNewToils_003Eb__0()
		{
			_003C_003E4__this.startTick = Find.TickManager.TicksGame;
		}

		internal bool _003CMakeNewToils_003Eb__1()
		{
			Thing carriedThing = _003C_003E4__this.pawn.carryTracker.CarriedThing;
			if (carriedThing == null)
			{
				return false;
			}
			if (_003C_003E4__this.pawn.carryTracker.AvailableStackSpace(_003C_003E4__this.ToHaul.def) > 0)
			{
				return carriedThing == _003C_003E4__this.ToHaul;
			}
			return true;
		}

		internal void _003CMakeNewToils_003Eb__2()
		{
			if (_003C_003E4__this.DropCarriedThingIfNotTarget && _003C_003E4__this.pawn.IsCarrying())
			{
				if (DebugViewSettings.logCarriedBetweenJobs)
				{
					Log.Message($"Dropping {_003C_003E4__this.pawn.carryTracker.CarriedThing} because it is not the designated Thing to haul.");
				}
				_003C_003E4__this.pawn.carryTracker.TryDropCarriedThing(_003C_003E4__this.pawn.Position, ThingPlaceMode.Near, out var _);
			}
		}

		internal bool _003CMakeNewToils_003Eb__3()
		{
			Pawn actor = toilGoto.actor;
			Job curJob = actor.jobs.curJob;
			if (curJob.haulMode == HaulMode.ToCellStorage)
			{
				Thing thing = curJob.GetTarget(TargetIndex.A).Thing;
				if (!actor.jobs.curJob.GetTarget(TargetIndex.B).Cell.IsValidStorageFor(_003C_003E4__this.Map, thing))
				{
					return true;
				}
			}
			return false;
		}
	}

	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__13 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_HaulToCell _003C_003E4__this;

		private _003C_003Ec__DisplayClass13_0 _003C_003E8__1;

		private Toil _003CreserveTargetA_003E5__2;

		private Toil _003CpostCarry_003E5__3;

		private Toil _003CcheckJumpPostCarry_003E5__4;

		private Toil _003CcarryToCell_003E5__5;

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
		public _003CMakeNewToils_003Ed__13(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E8__1 = null;
			_003CreserveTargetA_003E5__2 = null;
			_003CpostCarry_003E5__3 = null;
			_003CcheckJumpPostCarry_003E5__4 = null;
			_003CcarryToCell_003E5__5 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			JobDriver_HaulToCell jobDriver_HaulToCell = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E8__1 = new _003C_003Ec__DisplayClass13_0();
				_003C_003E8__1._003C_003E4__this = _003C_003E4__this;
				jobDriver_HaulToCell.FailOnDestroyedOrNull(TargetIndex.A);
				jobDriver_HaulToCell.FailOnBurningImmobile(TargetIndex.B);
				jobDriver_HaulToCell.FailOnForbidden(TargetIndex.B);
				if (!jobDriver_HaulToCell.forbiddenInitially)
				{
					jobDriver_HaulToCell.FailOnForbidden(TargetIndex.A);
				}
				_003C_003E2__current = Toils_General.DoAtomic(delegate
				{
					_003C_003E8__1._003C_003E4__this.startTick = Find.TickManager.TicksGame;
				});
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003CreserveTargetA_003E5__2 = Toils_Reserve.Reserve(TargetIndex.A);
				_003C_003E2__current = _003CreserveTargetA_003E5__2;
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003CpostCarry_003E5__3 = Toils_General.Label();
				_003CcheckJumpPostCarry_003E5__4 = Toils_Jump.JumpIf(_003CpostCarry_003E5__3, delegate
				{
					Thing carriedThing = _003C_003E8__1._003C_003E4__this.pawn.carryTracker.CarriedThing;
					if (carriedThing == null)
					{
						return false;
					}
					return _003C_003E8__1._003C_003E4__this.pawn.carryTracker.AvailableStackSpace(_003C_003E8__1._003C_003E4__this.ToHaul.def) <= 0 || carriedThing == _003C_003E8__1._003C_003E4__this.ToHaul;
				});
				_003C_003E2__current = _003CcheckJumpPostCarry_003E5__4;
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_General.DoAtomic(delegate
				{
					if (_003C_003E8__1._003C_003E4__this.DropCarriedThingIfNotTarget && _003C_003E8__1._003C_003E4__this.pawn.IsCarrying())
					{
						if (DebugViewSettings.logCarriedBetweenJobs)
						{
							Log.Message($"Dropping {_003C_003E8__1._003C_003E4__this.pawn.carryTracker.CarriedThing} because it is not the designated Thing to haul.");
						}
						_003C_003E8__1._003C_003E4__this.pawn.carryTracker.TryDropCarriedThing(_003C_003E8__1._003C_003E4__this.pawn.Position, ThingPlaceMode.Near, out var _);
					}
				});
				_003C_003E1__state = 4;
				return true;
			case 4:
				_003C_003E1__state = -1;
				_003C_003E8__1.toilGoto = null;
				_003C_003E8__1.toilGoto = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch, canGotoSpawnedParent: true).FailOnSomeonePhysicallyInteracting(TargetIndex.A).FailOn((Func<bool>)delegate
				{
					Pawn actor = _003C_003E8__1.toilGoto.actor;
					Job curJob = actor.jobs.curJob;
					if (curJob.haulMode == HaulMode.ToCellStorage)
					{
						Thing thing = curJob.GetTarget(TargetIndex.A).Thing;
						if (!actor.jobs.curJob.GetTarget(TargetIndex.B).Cell.IsValidStorageFor(_003C_003E8__1._003C_003E4__this.Map, thing))
						{
							return true;
						}
					}
					return false;
				});
				_003C_003E2__current = _003C_003E8__1.toilGoto;
				_003C_003E1__state = 5;
				return true;
			case 5:
				_003C_003E1__state = -1;
				_003C_003E2__current = _003CcheckJumpPostCarry_003E5__4;
				_003C_003E1__state = 6;
				return true;
			case 6:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Haul.StartCarryThing(TargetIndex.A, putRemainderInQueue: false, subtractNumTakenFromJobCount: true, failIfStackCountLessThanJobCount: false, reserve: true, HaulAIUtility.IsInHaulableInventory(jobDriver_HaulToCell.ToHaul));
				_003C_003E1__state = 7;
				return true;
			case 7:
				_003C_003E1__state = -1;
				_003C_003E2__current = _003CpostCarry_003E5__3;
				_003C_003E1__state = 8;
				return true;
			case 8:
				_003C_003E1__state = -1;
				if (jobDriver_HaulToCell.job.haulOpportunisticDuplicates)
				{
					_003C_003E2__current = Toils_Haul.CheckForGetOpportunityDuplicate(_003CreserveTargetA_003E5__2, TargetIndex.A, TargetIndex.B);
					_003C_003E1__state = 9;
					return true;
				}
				goto IL_0258;
			case 9:
				_003C_003E1__state = -1;
				goto IL_0258;
			case 10:
				_003C_003E1__state = -1;
				_003C_003E2__current = jobDriver_HaulToCell.PossiblyDelay();
				_003C_003E1__state = 11;
				return true;
			case 11:
				_003C_003E1__state = -1;
				_003C_003E2__current = jobDriver_HaulToCell.BeforeDrop();
				_003C_003E1__state = 12;
				return true;
			case 12:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Haul.PlaceHauledThingInCell(TargetIndex.B, _003CcarryToCell_003E5__5, storageMode: true);
				_003C_003E1__state = 13;
				return true;
			case 13:
				{
					_003C_003E1__state = -1;
					return false;
				}
				IL_0258:
				_003CcarryToCell_003E5__5 = Toils_Haul.CarryHauledThingToCell(TargetIndex.B);
				_003C_003E2__current = _003CcarryToCell_003E5__5;
				_003C_003E1__state = 10;
				return true;
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
			_003CMakeNewToils_003Ed__13 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__13(0)
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

	private bool forbiddenInitially;

	private const TargetIndex HaulableInd = TargetIndex.A;

	private const TargetIndex StoreCellInd = TargetIndex.B;

	private const int MinimumHaulingJobTicks = 30;

	public Thing ToHaul => job.GetTarget(TargetIndex.A).Thing;

	protected virtual bool DropCarriedThingIfNotTarget => false;

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref forbiddenInitially, "forbiddenInitially", defaultValue: false);
	}

	public override string GetReport()
	{
		IntVec3 cell = job.targetB.Cell;
		Thing thing = null;
		if (pawn.CurJob == job && pawn.carryTracker.CarriedThing != null)
		{
			thing = pawn.carryTracker.CarriedThing;
		}
		else if (base.TargetThingA != null && base.TargetThingA.Spawned)
		{
			thing = base.TargetThingA;
		}
		if (thing == null)
		{
			return "ReportHaulingUnknown".Translate();
		}
		string text = null;
		SlotGroup slotGroup = cell.GetSlotGroup(base.Map);
		if (slotGroup != null)
		{
			text = slotGroup.parent.SlotYielderLabel();
		}
		if (text != null)
		{
			return "ReportHaulingTo".Translate(thing.Label, text.Named("DESTINATION"), thing.Named("THING"));
		}
		return "ReportHauling".Translate(thing.Label, thing);
	}

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		if (pawn.Reserve(job.GetTarget(TargetIndex.B), job, 1, -1, null, errorOnFailed))
		{
			return pawn.Reserve(job.GetTarget(TargetIndex.A), job, 1, -1, null, errorOnFailed);
		}
		return false;
	}

	public override void Notify_Starting()
	{
		base.Notify_Starting();
		forbiddenInitially = base.TargetThingA != null && base.TargetThingA.IsForbidden(pawn);
	}

	protected virtual Toil BeforeDrop()
	{
		return Toils_General.Label();
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__13))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__13(-2)
		{
			_003C_003E4__this = this
		};
	}

	private Toil PossiblyDelay()
	{
		Toil toil = ToilMaker.MakeToil("PossiblyDelay");
		toil.atomicWithPrevious = true;
		toil.tickIntervalAction = delegate
		{
			if (Find.TickManager.TicksGame >= startTick + 30)
			{
				ReadyForNextToil();
			}
		};
		toil.defaultCompleteMode = ToilCompleteMode.Never;
		return toil;
	}
}
