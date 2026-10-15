using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public abstract class JobDriver_FeedBaby : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__13 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_FeedBaby _003C_003E4__this;

		private IEnumerator<Toil> _003CfeedBabyEnumerator_003E5__2;

		private Toil _003CfirstFeedingToil_003E5__3;

		private Toil _003CjumpIfDownedOrDrafted_003E5__4;

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
			_003CfeedBabyEnumerator_003E5__2 = null;
			_003CfirstFeedingToil_003E5__3 = null;
			_003CjumpIfDownedOrDrafted_003E5__4 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			JobDriver_FeedBaby CS_0024_003C_003E8__locals21 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				CS_0024_003C_003E8__locals21.FailOnDestroyedOrNull(TargetIndex.A);
				CS_0024_003C_003E8__locals21.AddFailCondition(() => !ChildcareUtility.CanSuckle(CS_0024_003C_003E8__locals21.Baby, out var _));
				CS_0024_003C_003E8__locals21.SetFinalizerJob((JobCondition _) => (!CS_0024_003C_003E8__locals21.pawn.IsCarryingPawn(CS_0024_003C_003E8__locals21.Baby)) ? null : ChildcareUtility.MakeBringBabyToSafetyJob(CS_0024_003C_003E8__locals21.pawn, CS_0024_003C_003E8__locals21.Baby));
				_003CfeedBabyEnumerator_003E5__2 = CS_0024_003C_003E8__locals21.FeedBaby().GetEnumerator();
				if (!_003CfeedBabyEnumerator_003E5__2.MoveNext())
				{
					throw new InvalidOperationException("There must be at least one toil in FeedBaby.");
				}
				_003CfirstFeedingToil_003E5__3 = _003CfeedBabyEnumerator_003E5__2.Current;
				_003CjumpIfDownedOrDrafted_003E5__4 = Toils_Jump.JumpIf(_003CfirstFeedingToil_003E5__3, () => CS_0024_003C_003E8__locals21.pawn.Downed || CS_0024_003C_003E8__locals21.pawn.Drafted).FailOn(() => !CS_0024_003C_003E8__locals21.pawn.IsCarryingPawn(CS_0024_003C_003E8__locals21.Baby));
				_003C_003E2__current = Toils_Jump.JumpIf(_003CjumpIfDownedOrDrafted_003E5__4, () => CS_0024_003C_003E8__locals21.pawn.IsCarryingPawn(CS_0024_003C_003E8__locals21.Baby)).FailOn(() => !CS_0024_003C_003E8__locals21.pawn.IsCarryingPawn(CS_0024_003C_003E8__locals21.Baby) && (CS_0024_003C_003E8__locals21.pawn.Downed || CS_0024_003C_003E8__locals21.pawn.Drafted));
				_003C_003E1__state = 1;
				return true;
			}
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.OnCell).FailOnDestroyedNullOrForbidden(TargetIndex.A).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Haul.StartCarryThing(TargetIndex.A);
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				_003C_003E2__current = _003CjumpIfDownedOrDrafted_003E5__4;
				_003C_003E1__state = 4;
				return true;
			case 4:
				_003C_003E1__state = -1;
				_003C_003E2__current = CS_0024_003C_003E8__locals21.GoToChair();
				_003C_003E1__state = 5;
				return true;
			case 5:
				_003C_003E1__state = -1;
				_003C_003E2__current = CS_0024_003C_003E8__locals21.SitInChair();
				_003C_003E1__state = 6;
				return true;
			case 6:
				_003C_003E1__state = -1;
				_003C_003E2__current = _003CfirstFeedingToil_003E5__3;
				_003C_003E1__state = 7;
				return true;
			case 7:
				_003C_003E1__state = -1;
				break;
			case 8:
				_003C_003E1__state = -1;
				break;
			}
			if (_003CfeedBabyEnumerator_003E5__2.MoveNext())
			{
				_003C_003E2__current = _003CfeedBabyEnumerator_003E5__2.Current;
				_003C_003E1__state = 8;
				return true;
			}
			_003CfeedBabyEnumerator_003E5__2.Dispose();
			return false;
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

	public const float BabyFedMemoryFoodNeedIncrease = 0.6f;

	protected const TargetIndex BabyInd = TargetIndex.A;

	private const int ChairSearchRadius = 32;

	protected float initialFoodPercentage;

	protected Pawn Baby => (Pawn)base.TargetThingA;

	protected abstract Toil FeedingToil { get; set; }

	public bool Feeding => base.CurToil == FeedingToil;

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		return pawn.Reserve(Baby, job, 1, -1, null, errorOnFailed);
	}

	protected abstract IEnumerable<Toil> FeedBaby();

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__13))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__13(-2)
		{
			_003C_003E4__this = this
		};
	}

	private Toil GoToChair()
	{
		Toil toil = ToilMaker.MakeToil("GoToChair");
		toil.initAction = delegate
		{
			IntVec3 breastfeedSpot = RCellFinder.SpotToStandDuringJob(pawn);
			Thing thing = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.BuildingArtificial), PathEndMode.OnCell, TraverseParms.For(pawn), 32f, (Thing t) => IsValidBreastfeedChair(t) && (int)t.Position.GetDangerFor(pawn, t.Map) <= (int)breastfeedSpot.GetDangerFor(pawn, pawn.Map));
			if (thing != null)
			{
				Toils_Ingest.TryFindFreeSittingSpotOnThing(thing, pawn, out breastfeedSpot);
			}
			if (!breastfeedSpot.IsValid)
			{
				pawn.pather.StartPath(pawn.Position, PathEndMode.OnCell);
			}
			else
			{
				pawn.ReserveSittableOrSpot(breastfeedSpot, toil.actor.CurJob);
				pawn.Map.pawnDestinationReservationManager.Reserve(pawn, pawn.CurJob, breastfeedSpot);
				pawn.pather.StartPath(breastfeedSpot, PathEndMode.OnCell);
			}
		};
		toil.defaultCompleteMode = ToilCompleteMode.PatherArrival;
		return toil;
	}

	private Toil SitInChair()
	{
		Toil toil = ToilMaker.MakeToil("SitInChair");
		toil.initAction = delegate
		{
			if (pawn.Spawned)
			{
				Thing thing = pawn.Position.GetThingList(pawn.Map).FirstOrDefault(IsValidBreastfeedChair);
				if (thing != null)
				{
					pawn.Rotation = thing.Rotation;
				}
			}
		};
		toil.defaultCompleteMode = ToilCompleteMode.Instant;
		return toil;
	}

	private bool IsValidBreastfeedChair(Thing t)
	{
		if (t.def.building == null || !t.def.building.isSittable)
		{
			return false;
		}
		if (!Toils_Ingest.TryFindFreeSittingSpotOnThing(t, pawn, out var _))
		{
			return false;
		}
		if (t.IsForbidden(pawn))
		{
			return false;
		}
		if (!pawn.CanReserve(t))
		{
			return false;
		}
		if (!t.IsSociallyProper(pawn))
		{
			return false;
		}
		if (t.IsBurning())
		{
			return false;
		}
		if (t.HostileTo(pawn))
		{
			return false;
		}
		return true;
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref initialFoodPercentage, "initialFoodPercentage", 0f);
	}
}
