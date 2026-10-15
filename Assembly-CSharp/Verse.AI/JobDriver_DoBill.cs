using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse.AI;

public class JobDriver_DoBill : JobDriver
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass16_0
	{
		public TargetIndex ingredientInd;

		public Toil physReserveToil;

		internal void _003CCollectIngredientsToils_003Eb__0()
		{
			physReserveToil.actor.Map.physicalInteractionReservationManager.Reserve(physReserveToil.actor, physReserveToil.actor.CurJob, physReserveToil.actor.CurJob.GetTarget(ingredientInd));
		}
	}

	[CompilerGenerated]
	private sealed class _003CCollectIngredientsToils_003Ed__16 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private TargetIndex ingredientInd;

		public TargetIndex _003C_003E3__ingredientInd;

		private _003C_003Ec__DisplayClass16_0 _003C_003E8__1;

		private TargetIndex billGiverInd;

		public TargetIndex _003C_003E3__billGiverInd;

		private bool subtractNumTakenFromJobCount;

		public bool _003C_003E3__subtractNumTakenFromJobCount;

		private bool failIfStackCountLessThanJobCount;

		public bool _003C_003E3__failIfStackCountLessThanJobCount;

		private bool placeInBillGiver;

		public bool _003C_003E3__placeInBillGiver;

		private TargetIndex ingredientPlaceCellInd;

		public TargetIndex _003C_003E3__ingredientPlaceCellInd;

		private Toil _003Cextract_003E5__2;

		private Toil _003CjumpIfHaveTargetInQueue_003E5__3;

		private Toil _003CgetToHaulTarget_003E5__4;

		private Toil _003CfindPlaceTarget_003E5__5;

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
		public _003CCollectIngredientsToils_003Ed__16(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E8__1 = null;
			_003Cextract_003E5__2 = null;
			_003CjumpIfHaveTargetInQueue_003E5__3 = null;
			_003CgetToHaulTarget_003E5__4 = null;
			_003CfindPlaceTarget_003E5__5 = null;
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
				_003C_003E8__1 = new _003C_003Ec__DisplayClass16_0();
				_003C_003E8__1.ingredientInd = ingredientInd;
				_003Cextract_003E5__2 = Toils_JobTransforms.ExtractNextTargetFromQueue(_003C_003E8__1.ingredientInd);
				_003C_003E2__current = _003Cextract_003E5__2;
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003CjumpIfHaveTargetInQueue_003E5__3 = Toils_Jump.JumpIfHaveTargetInQueue(_003C_003E8__1.ingredientInd, _003Cextract_003E5__2);
				_003C_003E2__current = JumpIfTargetInsideBillGiver(_003CjumpIfHaveTargetInQueue_003E5__3, _003C_003E8__1.ingredientInd, billGiverInd);
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003CgetToHaulTarget_003E5__4 = Toils_Goto.GotoThing(_003C_003E8__1.ingredientInd, PathEndMode.ClosestTouch, canGotoSpawnedParent: true).FailOnForbidden(_003C_003E8__1.ingredientInd).FailOnSomeonePhysicallyInteracting(_003C_003E8__1.ingredientInd);
				_003C_003E2__current = _003CgetToHaulTarget_003E5__4;
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Haul.StartCarryThing(_003C_003E8__1.ingredientInd, putRemainderInQueue: true, subtractNumTakenFromJobCount, failIfStackCountLessThanJobCount, reserve: false, canTakeFromInventory: true);
				_003C_003E1__state = 4;
				return true;
			case 4:
				_003C_003E1__state = -1;
				_003C_003E2__current = JumpToCollectNextIntoHandsForBill(_003CgetToHaulTarget_003E5__4, TargetIndex.B);
				_003C_003E1__state = 5;
				return true;
			case 5:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Goto.GotoThing(billGiverInd, PathEndMode.InteractionCell).FailOnDestroyedOrNull(_003C_003E8__1.ingredientInd);
				_003C_003E1__state = 6;
				return true;
			case 6:
				_003C_003E1__state = -1;
				if (!placeInBillGiver)
				{
					_003CfindPlaceTarget_003E5__5 = Toils_JobTransforms.SetTargetToIngredientPlaceCell(billGiverInd, _003C_003E8__1.ingredientInd, ingredientPlaceCellInd);
					_003C_003E2__current = _003CfindPlaceTarget_003E5__5;
					_003C_003E1__state = 7;
					return true;
				}
				_003C_003E2__current = Toils_Haul.DepositHauledThingInContainer(billGiverInd, _003C_003E8__1.ingredientInd);
				_003C_003E1__state = 10;
				return true;
			case 7:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Haul.PlaceHauledThingInCell(ingredientPlaceCellInd, _003CfindPlaceTarget_003E5__5, storageMode: false);
				_003C_003E1__state = 8;
				return true;
			case 8:
				_003C_003E1__state = -1;
				_003C_003E8__1.physReserveToil = ToilMaker.MakeToil("CollectIngredientsToils");
				_003C_003E8__1.physReserveToil.initAction = delegate
				{
					_003C_003E8__1.physReserveToil.actor.Map.physicalInteractionReservationManager.Reserve(_003C_003E8__1.physReserveToil.actor, _003C_003E8__1.physReserveToil.actor.CurJob, _003C_003E8__1.physReserveToil.actor.CurJob.GetTarget(_003C_003E8__1.ingredientInd));
				};
				_003C_003E2__current = _003C_003E8__1.physReserveToil;
				_003C_003E1__state = 9;
				return true;
			case 9:
				_003C_003E1__state = -1;
				_003CfindPlaceTarget_003E5__5 = null;
				goto IL_02c2;
			case 10:
				_003C_003E1__state = -1;
				goto IL_02c2;
			case 11:
				{
					_003C_003E1__state = -1;
					return false;
				}
				IL_02c2:
				_003C_003E2__current = _003CjumpIfHaveTargetInQueue_003E5__3;
				_003C_003E1__state = 11;
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
			_003CCollectIngredientsToils_003Ed__16 _003CCollectIngredientsToils_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CCollectIngredientsToils_003Ed__ = this;
			}
			else
			{
				_003CCollectIngredientsToils_003Ed__ = new _003CCollectIngredientsToils_003Ed__16(0);
			}
			_003CCollectIngredientsToils_003Ed__.ingredientInd = _003C_003E3__ingredientInd;
			_003CCollectIngredientsToils_003Ed__.billGiverInd = _003C_003E3__billGiverInd;
			_003CCollectIngredientsToils_003Ed__.ingredientPlaceCellInd = _003C_003E3__ingredientPlaceCellInd;
			_003CCollectIngredientsToils_003Ed__.subtractNumTakenFromJobCount = _003C_003E3__subtractNumTakenFromJobCount;
			_003CCollectIngredientsToils_003Ed__.failIfStackCountLessThanJobCount = _003C_003E3__failIfStackCountLessThanJobCount;
			_003CCollectIngredientsToils_003Ed__.placeInBillGiver = _003C_003E3__placeInBillGiver;
			return _003CCollectIngredientsToils_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Toil>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__15 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_DoBill _003C_003E4__this;

		private Toil _003CgotoBillGiver_003E5__2;

		private IEnumerator<Toil> _003C_003E7__wrap2;

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
		public _003CMakeNewToils_003Ed__15(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 3)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003CgotoBillGiver_003E5__2 = null;
			_003C_003E7__wrap2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				JobDriver_DoBill CS_0024_003C_003E8__locals14 = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
				{
					_003C_003E1__state = -1;
					CS_0024_003C_003E8__locals14.AddEndCondition(delegate
					{
						Thing thing = CS_0024_003C_003E8__locals14.GetActor().jobs.curJob.GetTarget(TargetIndex.A).Thing;
						return (!(thing is Building) || thing.Spawned) ? JobCondition.Ongoing : JobCondition.Incompletable;
					});
					CS_0024_003C_003E8__locals14.FailOnBurningImmobile(TargetIndex.A);
					CS_0024_003C_003E8__locals14.FailOn(delegate
					{
						if (CS_0024_003C_003E8__locals14.job.GetTarget(TargetIndex.A).Thing is IBillGiver billGiver)
						{
							if (CS_0024_003C_003E8__locals14.job.bill.DeletedOrDereferenced)
							{
								return true;
							}
							if (!billGiver.CurrentlyUsableForBills())
							{
								return true;
							}
						}
						return false;
					});
					_003CgotoBillGiver_003E5__2 = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell);
					Toil toil = ToilMaker.MakeToil("MakeNewToils");
					toil.initAction = delegate
					{
						if (CS_0024_003C_003E8__locals14.job.targetQueueB != null && CS_0024_003C_003E8__locals14.job.targetQueueB.Count == 1 && CS_0024_003C_003E8__locals14.job.targetQueueB[0].Thing is UnfinishedThing { Destroyed: false } unfinishedThing)
						{
							unfinishedThing.BoundBill = (Bill_ProductionWithUft)CS_0024_003C_003E8__locals14.job.bill;
						}
						CS_0024_003C_003E8__locals14.job.bill.Notify_DoBillStarted(CS_0024_003C_003E8__locals14.pawn);
					};
					_003C_003E2__current = toil;
					_003C_003E1__state = 1;
					return true;
				}
				case 1:
					_003C_003E1__state = -1;
					_003C_003E2__current = Toils_Jump.JumpIf(_003CgotoBillGiver_003E5__2, () => CS_0024_003C_003E8__locals14.job.GetTargetQueue(TargetIndex.B).NullOrEmpty());
					_003C_003E1__state = 2;
					return true;
				case 2:
					_003C_003E1__state = -1;
					_003C_003E7__wrap2 = CollectIngredientsToils(TargetIndex.B, TargetIndex.A, TargetIndex.C, subtractNumTakenFromJobCount: false, failIfStackCountLessThanJobCount: true, CS_0024_003C_003E8__locals14.BillGiver is Building_WorkTableAutonomous).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0144;
				case 3:
					_003C_003E1__state = -3;
					goto IL_0144;
				case 4:
					_003C_003E1__state = -1;
					_003C_003E2__current = Toils_Recipe.MakeUnfinishedThingIfNeeded();
					_003C_003E1__state = 5;
					return true;
				case 5:
					_003C_003E1__state = -1;
					_003C_003E2__current = Toils_Recipe.DoRecipeWork().FailOnDespawnedNullOrForbiddenPlacedThings(TargetIndex.A).FailOnCannotTouch(TargetIndex.A, PathEndMode.InteractionCell);
					_003C_003E1__state = 6;
					return true;
				case 6:
					_003C_003E1__state = -1;
					_003C_003E2__current = Toils_Recipe.CheckIfRecipeCanFinishNow();
					_003C_003E1__state = 7;
					return true;
				case 7:
					_003C_003E1__state = -1;
					_003C_003E2__current = Toils_Recipe.FinishRecipeAndStartStoringProduct(TargetIndex.None);
					_003C_003E1__state = 8;
					return true;
				case 8:
					{
						_003C_003E1__state = -1;
						return false;
					}
					IL_0144:
					if (_003C_003E7__wrap2.MoveNext())
					{
						Toil current = _003C_003E7__wrap2.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 3;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap2 = null;
					_003C_003E2__current = _003CgotoBillGiver_003E5__2;
					_003C_003E1__state = 4;
					return true;
				}
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap2 != null)
			{
				_003C_003E7__wrap2.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Toil> IEnumerable<Toil>.GetEnumerator()
		{
			_003CMakeNewToils_003Ed__15 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__15(0)
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

	public float workLeft;

	public int billStartTick;

	public int ticksSpentDoingRecipeWork;

	public const PathEndMode GotoIngredientPathEndMode = PathEndMode.ClosestTouch;

	public const TargetIndex BillGiverInd = TargetIndex.A;

	public const TargetIndex IngredientInd = TargetIndex.B;

	public const TargetIndex IngredientPlaceCellInd = TargetIndex.C;

	public IBillGiver BillGiver => (job.GetTarget(TargetIndex.A).Thing as IBillGiver) ?? throw new InvalidOperationException("DoBill on non-Billgiver.");

	public bool AnyIngredientsQueued => !job.GetTargetQueue(TargetIndex.B).NullOrEmpty();

	public override string GetReport()
	{
		if (ModsConfig.BiotechActive && job.bill is Bill_Mech bill)
		{
			return MechanitorUtility.GetMechGestationJobString(this, pawn, bill);
		}
		if (job.RecipeDef != null)
		{
			return ReportStringProcessed(job.RecipeDef.jobString);
		}
		return base.GetReport();
	}

	public override bool IsContinuation(Job j)
	{
		return j.bill == job.bill;
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref workLeft, "workLeft", 0f);
		Scribe_Values.Look(ref billStartTick, "billStartTick", 0);
		Scribe_Values.Look(ref ticksSpentDoingRecipeWork, "ticksSpentDoingRecipeWork", 0);
	}

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		Thing thing = job.GetTarget(TargetIndex.A).Thing;
		if (!pawn.Reserve(job.GetTarget(TargetIndex.A), job, 1, -1, null, errorOnFailed))
		{
			return false;
		}
		if (thing != null && thing.def.hasInteractionCell && !pawn.ReserveSittableOrSpot(thing.InteractionCell, job, errorOnFailed))
		{
			return false;
		}
		pawn.ReserveAsManyAsPossible(job.GetTargetQueue(TargetIndex.B), job);
		return true;
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__15))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__15(-2)
		{
			_003C_003E4__this = this
		};
	}

	[IteratorStateMachine(typeof(_003CCollectIngredientsToils_003Ed__16))]
	public static IEnumerable<Toil> CollectIngredientsToils(TargetIndex ingredientInd, TargetIndex billGiverInd, TargetIndex ingredientPlaceCellInd, bool subtractNumTakenFromJobCount = false, bool failIfStackCountLessThanJobCount = true, bool placeInBillGiver = false)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCollectIngredientsToils_003Ed__16(-2)
		{
			_003C_003E3__ingredientInd = ingredientInd,
			_003C_003E3__billGiverInd = billGiverInd,
			_003C_003E3__ingredientPlaceCellInd = ingredientPlaceCellInd,
			_003C_003E3__subtractNumTakenFromJobCount = subtractNumTakenFromJobCount,
			_003C_003E3__failIfStackCountLessThanJobCount = failIfStackCountLessThanJobCount,
			_003C_003E3__placeInBillGiver = placeInBillGiver
		};
	}

	private static Toil JumpIfTargetInsideBillGiver(Toil jumpToil, TargetIndex ingredient, TargetIndex billGiver)
	{
		Toil toil = ToilMaker.MakeToil("JumpIfTargetInsideBillGiver");
		toil.initAction = delegate
		{
			Thing thing = toil.actor.CurJob.GetTarget(billGiver).Thing;
			if (thing != null && thing.Spawned)
			{
				Thing thing2 = toil.actor.jobs.curJob.GetTarget(ingredient).Thing;
				if (thing2 != null)
				{
					ThingOwner thingOwner = thing.TryGetInnerInteractableThingOwner();
					if (thingOwner != null && thingOwner.Contains(thing2))
					{
						HaulAIUtility.UpdateJobWithPlacedThings(toil.actor.jobs.curJob, thing2, thing2.stackCount);
						toil.actor.jobs.curDriver.JumpToToil(jumpToil);
					}
				}
			}
		};
		return toil;
	}

	public static Toil JumpToCollectNextIntoHandsForBill(Toil gotoGetTargetToil, TargetIndex ind)
	{
		Toil toil = ToilMaker.MakeToil("JumpToCollectNextIntoHandsForBill");
		toil.initAction = delegate
		{
			Pawn actor = toil.actor;
			if (actor.carryTracker.CarriedThing == null)
			{
				Log.Error("JumpToAlsoCollectTargetInQueue run on " + actor?.ToString() + " who is not carrying something.");
			}
			else if (!actor.carryTracker.Full)
			{
				Job curJob = actor.jobs.curJob;
				List<LocalTargetInfo> targetQueue = curJob.GetTargetQueue(ind);
				if (!targetQueue.NullOrEmpty())
				{
					for (int i = 0; i < targetQueue.Count; i++)
					{
						if (GenAI.CanUseItemForWork(actor, targetQueue[i].Thing) && targetQueue[i].Thing.CanStackWith(actor.carryTracker.CarriedThing) && !((float)(actor.Position - targetQueue[i].Thing.Position).LengthHorizontalSquared > 64f))
						{
							int num = ((actor.carryTracker.CarriedThing != null) ? actor.carryTracker.CarriedThing.stackCount : 0);
							int a = curJob.countQueue[i];
							a = Mathf.Min(a, targetQueue[i].Thing.def.stackLimit - num);
							a = Mathf.Min(a, actor.carryTracker.AvailableStackSpace(targetQueue[i].Thing.def));
							if (a > 0)
							{
								curJob.count = a;
								curJob.SetTarget(ind, targetQueue[i].Thing);
								curJob.countQueue[i] -= a;
								if (curJob.countQueue[i] <= 0)
								{
									curJob.countQueue.RemoveAt(i);
									targetQueue.RemoveAt(i);
								}
								actor.jobs.curDriver.JumpToToil(gotoGetTargetToil);
								break;
							}
						}
					}
				}
			}
		};
		return toil;
	}
}
