using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_BottleFeedBaby : JobDriver_FeedBaby
{
	[CompilerGenerated]
	private sealed class _003CFeedBaby_003Ed__14 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_BottleFeedBaby _003C_003E4__this;

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
		public _003CFeedBaby_003Ed__14(int _003C_003E1__state)
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
			JobDriver_BottleFeedBaby jobDriver_BottleFeedBaby = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E2__current = jobDriver_BottleFeedBaby.FeedBabyFoodFromInventory();
				_003C_003E1__state = 1;
				return true;
			case 1:
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
			_003CFeedBaby_003Ed__14 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CFeedBaby_003Ed__14(0)
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

	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__12 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_BottleFeedBaby _003C_003E4__this;

		private Toil _003CfailIfNoBabyFoodInInventory_003E5__2;

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
		public _003CMakeNewToils_003Ed__12(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 6)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003CfailIfNoBabyFoodInInventory_003E5__2 = null;
			_003C_003E7__wrap2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				JobDriver_BottleFeedBaby CS_0024_003C_003E8__locals10 = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					CS_0024_003C_003E8__locals10.AddFailCondition(() => !CS_0024_003C_003E8__locals10.pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation));
					_003CfailIfNoBabyFoodInInventory_003E5__2 = CS_0024_003C_003E8__locals10.FailIfNoBabyFoodInInventory();
					_003C_003E2__current = Toils_Jump.JumpIf(_003CfailIfNoBabyFoodInInventory_003E5__2, () => !CS_0024_003C_003E8__locals10.BabyFoodTarget.IsValid || CS_0024_003C_003E8__locals10.pawn.inventory.Contains(CS_0024_003C_003E8__locals10.BabyFood));
					_003C_003E1__state = 1;
					return true;
				case 1:
					_003C_003E1__state = -1;
					_003C_003E2__current = Toils_Ingest.ReserveFoodFromStackForIngesting(TargetIndex.B, CS_0024_003C_003E8__locals10.Baby).FailOnDestroyedNullOrForbidden(TargetIndex.B);
					_003C_003E1__state = 2;
					return true;
				case 2:
					_003C_003E1__state = -1;
					_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.ClosestTouch).FailOnDestroyedNullOrForbidden(TargetIndex.B);
					_003C_003E1__state = 3;
					return true;
				case 3:
					_003C_003E1__state = -1;
					_003C_003E2__current = Toils_Haul.TakeToInventory(TargetIndex.B, delegate(Thing babyFood)
					{
						int b = FoodUtility.WillIngestStackCountOf(CS_0024_003C_003E8__locals10.Baby, babyFood.def, FoodUtility.NutritionForEater(CS_0024_003C_003E8__locals10.Baby, babyFood));
						return Mathf.Min(babyFood.stackCount, b);
					}).FailOnDestroyedNullOrForbidden(TargetIndex.B);
					_003C_003E1__state = 4;
					return true;
				case 4:
					_003C_003E1__state = -1;
					_003C_003E2__current = _003CfailIfNoBabyFoodInInventory_003E5__2;
					_003C_003E1__state = 5;
					return true;
				case 5:
					_003C_003E1__state = -1;
					_003C_003E7__wrap2 = ((JobDriver_FeedBaby)CS_0024_003C_003E8__locals10).MakeNewToils().GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 6:
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap2.MoveNext())
				{
					Toil current = _003C_003E7__wrap2.Current;
					_003C_003E2__current = current;
					_003C_003E1__state = 6;
					return true;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap2 = null;
				return false;
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
			_003CMakeNewToils_003Ed__12 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__12(0)
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

	protected const TargetIndex BabyFoodInd = TargetIndex.B;

	private float bottleNutrition;

	private float totalBottleNutrition;

	private float initialNutritionNeeded;

	protected Thing BabyFood => base.TargetThingB;

	protected LocalTargetInfo BabyFoodTarget => base.TargetB;

	protected override Toil FeedingToil { get; set; }

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__12))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__12(-2)
		{
			_003C_003E4__this = this
		};
	}

	private Toil FailIfNoBabyFoodInInventory()
	{
		Toil toil = ToilMaker.MakeToil("FailIfNoBabyFoodInInventory");
		toil.FailOn(() => FoodUtility.BestFoodInInventory(pawn, base.Baby) == null);
		return toil;
	}

	[IteratorStateMachine(typeof(_003CFeedBaby_003Ed__14))]
	protected override IEnumerable<Toil> FeedBaby()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CFeedBaby_003Ed__14(-2)
		{
			_003C_003E4__this = this
		};
	}

	private Toil FeedBabyFoodFromInventory()
	{
		FeedingToil = ToilMaker.MakeToil("FeedBabyFoodFromInventory");
		FeedingToil.initAction = delegate
		{
			initialNutritionNeeded = base.Baby.needs.food.NutritionWanted;
			initialFoodPercentage = base.Baby.needs.food.CurLevelPercentage;
			base.Baby.jobs.StartJob(ChildcareUtility.MakeBabySuckleJob(pawn), JobCondition.InterruptForced, null, resumeCurJobAfterwards: false, cancelBusyStances: true, null, null, fromQueue: false, canReturnCurJobToPool: false, null);
		};
		FeedingToil.tickIntervalAction = delegate(int delta)
		{
			float num = base.Baby.needs.food.MaxLevel / 5000f;
			float num2 = Mathf.Min(base.Baby.needs.food.NutritionWanted, num * (float)delta);
			bottleNutrition += num2;
			totalBottleNutrition += num2;
			pawn.GainComfortFromCellIfPossible(delta);
			base.Baby.ideo?.IncreaseIdeoExposureIfBabyTick(pawn.Ideo);
			if (!pawn.Downed && pawn.Rotation == Rot4.North)
			{
				pawn.Rotation = Rot4.East;
			}
			while (true)
			{
				Thing thing = FoodUtility.BestFoodInInventory(pawn, base.Baby);
				if (thing == null)
				{
					ReadyForNextToil();
					break;
				}
				if (bottleNutrition >= base.Baby.needs.food.NutritionWanted)
				{
					float num3 = thing.Ingested(base.Baby, bottleNutrition);
					base.Baby.records.AddTo(RecordDefOf.NutritionEaten, num3);
					bottleNutrition -= num3;
					base.Baby.needs.food.CurLevel = Mathf.Clamp(base.Baby.needs.food.CurLevel + num3, 0f, base.Baby.needs.food.MaxLevel);
					if (base.Baby.needs.food.CurLevel >= base.Baby.needs.food.MaxLevel)
					{
						ReadyForNextToil();
						break;
					}
				}
				else
				{
					float num4 = FoodUtility.NutritionForEater(base.Baby, thing);
					if (!(bottleNutrition >= num4))
					{
						break;
					}
					float num5 = thing.Ingested(base.Baby, num4);
					base.Baby.records.AddTo(RecordDefOf.NutritionEaten, num5);
					bottleNutrition -= num5;
					base.Baby.needs.food.CurLevel = Mathf.Clamp(base.Baby.needs.food.CurLevel + num5, 0f, base.Baby.needs.food.MaxLevel);
				}
			}
		};
		FeedingToil.AddFinishAction(delegate
		{
			if (base.Baby.needs != null && base.Baby.needs.food.CurLevelPercentage - initialFoodPercentage > 0.6f)
			{
				base.Baby.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.FedMe, pawn);
				pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.FedBaby, base.Baby);
			}
			if (base.Baby.CurJobDef == JobDefOf.BabySuckle)
			{
				base.Baby.jobs.EndCurrentJob(JobCondition.Succeeded);
			}
		});
		FeedingToil.handlingFacing = true;
		FeedingToil.WithProgressBar(TargetIndex.A, () => totalBottleNutrition / initialNutritionNeeded);
		FeedingToil.defaultCompleteMode = ToilCompleteMode.Never;
		FeedingToil.WithEffect(EffecterDefOf.Breastfeeding, TargetIndex.A, null);
		return FeedingToil;
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref bottleNutrition, "bottleNutrition", 0f);
		Scribe_Values.Look(ref totalBottleNutrition, "totalBottleNutrition", 0f);
		Scribe_Values.Look(ref initialNutritionNeeded, "initialNutritionNeeded", 0f);
	}
}
