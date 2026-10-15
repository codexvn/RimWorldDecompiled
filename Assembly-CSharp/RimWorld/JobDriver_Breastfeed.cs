using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_Breastfeed : JobDriver_FeedBaby
{
	[CompilerGenerated]
	private sealed class _003CFeedBaby_003Ed__7 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_Breastfeed _003C_003E4__this;

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
		public _003CFeedBaby_003Ed__7(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			JobDriver_Breastfeed CS_0024_003C_003E8__locals4 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				CS_0024_003C_003E8__locals4.AddFailCondition(() => !ChildcareUtility.CanBreastfeed(CS_0024_003C_003E8__locals4.pawn, out var _));
				_003C_003E2__current = CS_0024_003C_003E8__locals4.Breastfeed();
				_003C_003E1__state = 1;
				return true;
			}
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = CS_0024_003C_003E8__locals4.TuckMomInIfDowned();
				_003C_003E1__state = 2;
				return true;
			case 2:
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
			_003CFeedBaby_003Ed__7 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CFeedBaby_003Ed__7(0)
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

	protected override Toil FeedingToil { get; set; }

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		if (base.TryMakePreToilReservations(errorOnFailed))
		{
			return pawn.Reserve(pawn, job, 1, -1, null, errorOnFailed);
		}
		return false;
	}

	public override void SetInitialPosture()
	{
	}

	public override bool CanBeginNowWhileLyingDown()
	{
		return pawn.Downed;
	}

	[IteratorStateMachine(typeof(_003CFeedBaby_003Ed__7))]
	protected override IEnumerable<Toil> FeedBaby()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CFeedBaby_003Ed__7(-2)
		{
			_003C_003E4__this = this
		};
	}

	private Toil Breastfeed()
	{
		FeedingToil = ToilMaker.MakeToil("Breastfeed");
		FeedingToil.initAction = delegate
		{
			base.Baby.jobs.StartJob(ChildcareUtility.MakeBabySuckleJob(pawn), JobCondition.InterruptForced, null, resumeCurJobAfterwards: false, cancelBusyStances: true, null, null, fromQueue: false, canReturnCurJobToPool: false, null);
			initialFoodPercentage = base.Baby.needs.food.CurLevelPercentage;
		};
		FeedingToil.tickIntervalAction = delegate(int delta)
		{
			bool num = ChildcareUtility.SuckleFromLactatingPawn(base.Baby, pawn, delta);
			pawn.GainComfortFromCellIfPossible(delta);
			if (!pawn.Downed && pawn.Rotation == Rot4.North)
			{
				pawn.Rotation = Rot4.East;
			}
			if (!num)
			{
				ReadyForNextToil();
			}
		};
		FeedingToil.AddFinishAction(delegate
		{
			if (!base.Baby.Dead)
			{
				if (base.Baby.needs.food.CurLevelPercentage - initialFoodPercentage > 0.6f)
				{
					base.Baby.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.BreastfedMe, pawn);
					pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.BreastfedBaby, base.Baby);
				}
				if (base.Baby.CurJobDef == JobDefOf.BabySuckle)
				{
					base.Baby.jobs.EndCurrentJob(JobCondition.Succeeded);
				}
				if (pawn.Downed)
				{
					pawn.carryTracker.TryDropCarriedThing(pawn.Position, ThingPlaceMode.Near, out var _);
				}
			}
		});
		FeedingToil.AddFailCondition(() => !ChildcareUtility.CanBreastfeed(pawn, out var _));
		FeedingToil.handlingFacing = true;
		FeedingToil.WithProgressBar(TargetIndex.A, () => base.Baby.needs.food.CurLevelPercentage);
		FeedingToil.defaultCompleteMode = ToilCompleteMode.Never;
		FeedingToil.WithEffect(EffecterDefOf.Breastfeeding, TargetIndex.A, null);
		return FeedingToil;
	}

	private Toil TuckMomInIfDowned()
	{
		Toil toil = ToilMaker.MakeToil("TuckMomInIfDowned");
		toil.initAction = delegate
		{
			if (HealthAIUtility.ShouldSeekMedicalRest(pawn))
			{
				Building_Bed building_Bed = pawn.CurrentBed();
				if (building_Bed != null && building_Bed == RestUtility.FindPatientBedFor(pawn))
				{
					pawn.jobs.StartJob(JobMaker.MakeJob(JobDefOf.LayDown, building_Bed), JobCondition.Succeeded, null, resumeCurJobAfterwards: false, cancelBusyStances: true, null, JobTag.RestingForMedicalReasons, fromQueue: false, canReturnCurJobToPool: false, null);
				}
			}
		};
		toil.defaultCompleteMode = ToilCompleteMode.Instant;
		return toil;
	}
}
