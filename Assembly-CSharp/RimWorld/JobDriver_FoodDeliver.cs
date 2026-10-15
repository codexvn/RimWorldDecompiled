using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_FoodDeliver : JobDriver
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass10_0
	{
		public Toil toil;

		public JobDriver_FoodDeliver _003C_003E4__this;

		internal void _003CMakeNewToils_003Eb__0()
		{
			Pawn actor = toil.actor;
			Job curJob = actor.jobs.curJob;
			actor.pather.StartPath(curJob.targetC, PathEndMode.OnCell);
		}

		internal bool _003CMakeNewToils_003Eb__1()
		{
			if (!_003C_003E4__this.pawn.IsCarryingThing(_003C_003E4__this.job.GetTarget(TargetIndex.A).Thing))
			{
				return true;
			}
			Pawn pawn = (Pawn)toil.actor.jobs.curJob.targetB.Thing;
			if (!pawn.IsPrisonerOfColony)
			{
				return true;
			}
			if (!pawn.guest.CanBeBroughtFood)
			{
				return true;
			}
			return false;
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass10_1
	{
		public Toil toil;

		public JobDriver_FoodDeliver _003C_003E4__this;

		internal void _003CMakeNewToils_003Eb__2()
		{
			_003C_003E4__this.pawn.carryTracker.TryDropCarriedThing(toil.actor.jobs.curJob.targetC.Cell, ThingPlaceMode.Direct, out var _);
		}
	}

	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__10 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_FoodDeliver _003C_003E4__this;

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
		public _003CMakeNewToils_003Ed__10(int _003C_003E1__state)
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
			JobDriver_FoodDeliver jobDriver_FoodDeliver = _003C_003E4__this;
			_003C_003Ec__DisplayClass10_0 CS_0024_003C_003E8__locals19;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				jobDriver_FoodDeliver.FailOnDespawnedOrNull(TargetIndex.B);
				if (jobDriver_FoodDeliver.eatingFromInventory)
				{
					_003C_003E2__current = Toils_Misc.TakeItemFromInventoryToCarrier(jobDriver_FoodDeliver.pawn, TargetIndex.A);
					_003C_003E1__state = 1;
					return true;
				}
				if (jobDriver_FoodDeliver.usingNutrientPasteDispenser)
				{
					_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell).FailOnForbidden(TargetIndex.A);
					_003C_003E1__state = 2;
					return true;
				}
				_003C_003E2__current = Toils_Ingest.ReserveFoodFromStackForIngesting(TargetIndex.A, jobDriver_FoodDeliver.Deliveree);
				_003C_003E1__state = 4;
				return true;
			case 1:
				_003C_003E1__state = -1;
				goto IL_0130;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Ingest.TakeMealFromDispenser(TargetIndex.A, jobDriver_FoodDeliver.pawn);
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				goto IL_0130;
			case 4:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOnForbidden(TargetIndex.A);
				_003C_003E1__state = 5;
				return true;
			case 5:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Ingest.PickupIngestible(TargetIndex.A, jobDriver_FoodDeliver.Deliveree);
				_003C_003E1__state = 6;
				return true;
			case 6:
				_003C_003E1__state = -1;
				goto IL_0130;
			case 7:
			{
				_003C_003E1__state = -1;
				_003C_003Ec__DisplayClass10_1 CS_0024_003C_003E8__locals7 = new _003C_003Ec__DisplayClass10_1();
				CS_0024_003C_003E8__locals7._003C_003E4__this = jobDriver_FoodDeliver;
				CS_0024_003C_003E8__locals7.toil = ToilMaker.MakeToil("MakeNewToils");
				CS_0024_003C_003E8__locals7.toil.initAction = delegate
				{
					CS_0024_003C_003E8__locals7._003C_003E4__this.pawn.carryTracker.TryDropCarriedThing(CS_0024_003C_003E8__locals7.toil.actor.jobs.curJob.targetC.Cell, ThingPlaceMode.Direct, out var _);
				};
				CS_0024_003C_003E8__locals7.toil.defaultCompleteMode = ToilCompleteMode.Instant;
				_003C_003E2__current = CS_0024_003C_003E8__locals7.toil;
				_003C_003E1__state = 8;
				return true;
			}
			case 8:
				{
					_003C_003E1__state = -1;
					return false;
				}
				IL_0130:
				CS_0024_003C_003E8__locals19 = new _003C_003Ec__DisplayClass10_0();
				CS_0024_003C_003E8__locals19._003C_003E4__this = jobDriver_FoodDeliver;
				CS_0024_003C_003E8__locals19.toil = ToilMaker.MakeToil("MakeNewToils");
				CS_0024_003C_003E8__locals19.toil.initAction = delegate
				{
					Pawn actor = CS_0024_003C_003E8__locals19.toil.actor;
					Job curJob = actor.jobs.curJob;
					actor.pather.StartPath(curJob.targetC, PathEndMode.OnCell);
				};
				CS_0024_003C_003E8__locals19.toil.defaultCompleteMode = ToilCompleteMode.PatherArrival;
				CS_0024_003C_003E8__locals19.toil.FailOnDestroyedNullOrForbidden(TargetIndex.B);
				CS_0024_003C_003E8__locals19.toil.AddFailCondition(delegate
				{
					if (!CS_0024_003C_003E8__locals19._003C_003E4__this.pawn.IsCarryingThing(CS_0024_003C_003E8__locals19._003C_003E4__this.job.GetTarget(TargetIndex.A).Thing))
					{
						return true;
					}
					Pawn pawn = (Pawn)CS_0024_003C_003E8__locals19.toil.actor.jobs.curJob.targetB.Thing;
					if (!pawn.IsPrisonerOfColony)
					{
						return true;
					}
					return !pawn.guest.CanBeBroughtFood;
				});
				_003C_003E2__current = CS_0024_003C_003E8__locals19.toil;
				_003C_003E1__state = 7;
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
			_003CMakeNewToils_003Ed__10 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__10(0)
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

	private bool usingNutrientPasteDispenser;

	private bool eatingFromInventory;

	private const TargetIndex FoodSourceInd = TargetIndex.A;

	private const TargetIndex DelivereeInd = TargetIndex.B;

	private Pawn Deliveree => (Pawn)job.targetB.Thing;

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref usingNutrientPasteDispenser, "usingNutrientPasteDispenser", defaultValue: false);
		Scribe_Values.Look(ref eatingFromInventory, "eatingFromInventory", defaultValue: false);
	}

	public override string GetReport()
	{
		if (job.GetTarget(TargetIndex.A).Thing is Building_NutrientPasteDispenser && Deliveree != null)
		{
			return JobUtility.GetResolvedJobReportRaw(job.def.reportString, ThingDefOf.MealNutrientPaste.label, ThingDefOf.MealNutrientPaste, Deliveree.LabelShort, Deliveree, "", "");
		}
		return base.GetReport();
	}

	public override void Notify_Starting()
	{
		base.Notify_Starting();
		usingNutrientPasteDispenser = base.TargetThingA is Building_NutrientPasteDispenser;
		eatingFromInventory = pawn.inventory != null && pawn.inventory.Contains(base.TargetThingA);
	}

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		return pawn.Reserve(Deliveree, job, 1, -1, null, errorOnFailed);
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__10))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__10(-2)
		{
			_003C_003E4__this = this
		};
	}
}
