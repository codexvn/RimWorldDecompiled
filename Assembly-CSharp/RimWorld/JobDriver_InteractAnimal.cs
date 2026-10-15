using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld;

public abstract class JobDriver_InteractAnimal : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CFeedToils_003Ed__19 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_InteractAnimal _003C_003E4__this;

		private Toil _003CgotoAnimal_003E5__2;

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
		public _003CFeedToils_003Ed__19(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003CgotoAnimal_003E5__2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			JobDriver_InteractAnimal CS_0024_003C_003E8__locals5 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				Toil toil = ToilMaker.MakeToil("FeedToils");
				toil.initAction = delegate
				{
					CS_0024_003C_003E8__locals5.feedNutritionLeft = RequiredNutritionPerFeed(CS_0024_003C_003E8__locals5.Animal);
				};
				toil.defaultCompleteMode = ToilCompleteMode.Instant;
				_003C_003E2__current = toil;
				_003C_003E1__state = 1;
				return true;
			}
			case 1:
				_003C_003E1__state = -1;
				_003CgotoAnimal_003E5__2 = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
				_003C_003E2__current = _003CgotoAnimal_003E5__2;
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = CS_0024_003C_003E8__locals5.StartFeedAnimal(TargetIndex.A);
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Ingest.FinalizeIngest(CS_0024_003C_003E8__locals5.Animal, TargetIndex.B);
				_003C_003E1__state = 4;
				return true;
			case 4:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_General.PutCarriedThingInInventory();
				_003C_003E1__state = 5;
				return true;
			case 5:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_General.ClearTarget(TargetIndex.B);
				_003C_003E1__state = 6;
				return true;
			case 6:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Jump.JumpIf(_003CgotoAnimal_003E5__2, () => CS_0024_003C_003E8__locals5.feedNutritionLeft > 0f);
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
			_003CFeedToils_003Ed__19 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CFeedToils_003Ed__19(0)
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
	private sealed class _003CMakeNewToils_003Ed__17 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_InteractAnimal _003C_003E4__this;

		private IEnumerator<Toil> _003C_003E7__wrap1;

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
		public _003CMakeNewToils_003Ed__17(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			switch (_003C_003E1__state)
			{
			case -3:
			case 9:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
				break;
			case -4:
			case 14:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			}
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				JobDriver_InteractAnimal CS_0024_003C_003E8__locals15 = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					CS_0024_003C_003E8__locals15.FailOnDespawnedNullOrForbidden(TargetIndex.A);
					CS_0024_003C_003E8__locals15.FailOnDowned(TargetIndex.A);
					CS_0024_003C_003E8__locals15.FailOnNotCasualInterruptible(TargetIndex.A);
					_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
					_003C_003E1__state = 1;
					return true;
				case 1:
					_003C_003E1__state = -1;
					_003C_003E2__current = Toils_Interpersonal.WaitToBeAbleToInteract(CS_0024_003C_003E8__locals15.pawn);
					_003C_003E1__state = 2;
					return true;
				case 2:
					_003C_003E1__state = -1;
					_003C_003E2__current = Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
					_003C_003E1__state = 3;
					return true;
				case 3:
					_003C_003E1__state = -1;
					_003C_003E2__current = CS_0024_003C_003E8__locals15.TalkToAnimal(TargetIndex.A);
					_003C_003E1__state = 4;
					return true;
				case 4:
					_003C_003E1__state = -1;
					_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
					_003C_003E1__state = 5;
					return true;
				case 5:
					_003C_003E1__state = -1;
					_003C_003E2__current = Toils_Interpersonal.WaitToBeAbleToInteract(CS_0024_003C_003E8__locals15.pawn);
					_003C_003E1__state = 6;
					return true;
				case 6:
					_003C_003E1__state = -1;
					_003C_003E2__current = Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
					_003C_003E1__state = 7;
					return true;
				case 7:
					_003C_003E1__state = -1;
					_003C_003E2__current = CS_0024_003C_003E8__locals15.TalkToAnimal(TargetIndex.A);
					_003C_003E1__state = 8;
					return true;
				case 8:
					_003C_003E1__state = -1;
					if (CS_0024_003C_003E8__locals15.CanFeedEver)
					{
						_003C_003E7__wrap1 = CS_0024_003C_003E8__locals15.FeedToils().GetEnumerator();
						_003C_003E1__state = -3;
						goto IL_01eb;
					}
					goto IL_0205;
				case 9:
					_003C_003E1__state = -3;
					goto IL_01eb;
				case 10:
					_003C_003E1__state = -1;
					_003C_003E2__current = Toils_Interpersonal.WaitToBeAbleToInteract(CS_0024_003C_003E8__locals15.pawn);
					_003C_003E1__state = 11;
					return true;
				case 11:
					_003C_003E1__state = -1;
					_003C_003E2__current = Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
					_003C_003E1__state = 12;
					return true;
				case 12:
					_003C_003E1__state = -1;
					_003C_003E2__current = CS_0024_003C_003E8__locals15.TalkToAnimal(TargetIndex.A);
					_003C_003E1__state = 13;
					return true;
				case 13:
					_003C_003E1__state = -1;
					if (CS_0024_003C_003E8__locals15.CanFeedEver)
					{
						_003C_003E7__wrap1 = CS_0024_003C_003E8__locals15.FeedToils().GetEnumerator();
						_003C_003E1__state = -4;
						goto IL_02e4;
					}
					goto IL_02fe;
				case 14:
					_003C_003E1__state = -4;
					goto IL_02e4;
				case 15:
					_003C_003E1__state = -1;
					_003C_003E2__current = Toils_Interpersonal.SetLastInteractTime(TargetIndex.A);
					_003C_003E1__state = 16;
					return true;
				case 16:
					_003C_003E1__state = -1;
					_003C_003E2__current = Toils_Interpersonal.WaitToBeAbleToInteract(CS_0024_003C_003E8__locals15.pawn);
					_003C_003E1__state = 17;
					return true;
				case 17:
					_003C_003E1__state = -1;
					_003C_003E2__current = Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
					_003C_003E1__state = 18;
					return true;
				case 18:
					{
						_003C_003E1__state = -1;
						return false;
					}
					IL_02e4:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Toil current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 14;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap1 = null;
					goto IL_02fe;
					IL_02fe:
					_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOn(() => !CS_0024_003C_003E8__locals15.CanInteractNow);
					_003C_003E1__state = 15;
					return true;
					IL_01eb:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Toil current2 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current2;
						_003C_003E1__state = 9;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					goto IL_0205;
					IL_0205:
					_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
					_003C_003E1__state = 10;
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
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
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
			_003CMakeNewToils_003Ed__17 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__17(0)
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

	protected const TargetIndex AnimalInd = TargetIndex.A;

	private const TargetIndex FoodHandInd = TargetIndex.B;

	private const int FeedDuration = 270;

	private const int TalkDuration = 270;

	private const float NutritionPercentagePerFeed = 0.15f;

	private const float MaxMinNutritionPerFeed = 0.3f;

	public const int FeedCount = 2;

	public const FoodPreferability MaxFoodPreferability = FoodPreferability.RawTasty;

	private float feedNutritionLeft;

	protected Pawn Animal => (Pawn)job.targetA.Thing;

	protected virtual bool CanInteractNow => true;

	protected virtual bool CanFeedEver => Animal?.needs?.food != null;

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref feedNutritionLeft, "feedNutritionLeft", 0f);
	}

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		return pawn.Reserve(Animal, job, 1, -1, null, errorOnFailed);
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__17))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__17(-2)
		{
			_003C_003E4__this = this
		};
	}

	public static float RequiredNutritionPerFeed(Pawn animal)
	{
		if (animal.needs.food == null)
		{
			return 0f;
		}
		return Mathf.Min(animal.needs.food.MaxLevel * 0.15f, 0.3f);
	}

	[IteratorStateMachine(typeof(_003CFeedToils_003Ed__19))]
	private IEnumerable<Toil> FeedToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CFeedToils_003Ed__19(-2)
		{
			_003C_003E4__this = this
		};
	}

	private Toil TalkToAnimal(TargetIndex tameeInd)
	{
		Toil toil = ToilMaker.MakeToil("TalkToAnimal");
		toil.initAction = delegate
		{
			Pawn actor = toil.GetActor();
			Pawn recipient = (Pawn)(Thing)actor.CurJob.GetTarget(tameeInd);
			actor.interactions.TryInteractWith(recipient, InteractionDefOf.AnimalChat);
		};
		toil.FailOn(() => !CanInteractNow);
		toil.defaultCompleteMode = ToilCompleteMode.Delay;
		toil.defaultDuration = 270;
		toil.activeSkill = () => SkillDefOf.Animals;
		return toil;
	}

	private Toil StartFeedAnimal(TargetIndex tameeInd)
	{
		Toil toil = ToilMaker.MakeToil("StartFeedAnimal");
		toil.initAction = delegate
		{
			Pawn actor = toil.GetActor();
			Pawn pawn = (Pawn)(Thing)actor.CurJob.GetTarget(tameeInd);
			PawnUtility.ForceWait(pawn, 270, actor);
			Thing thing = FoodUtility.BestFoodInInventory(actor, pawn, FoodPreferability.NeverForNutrition, FoodPreferability.RawTasty);
			if (thing == null)
			{
				actor.jobs.EndCurrentJob(JobCondition.Incompletable);
			}
			else
			{
				actor.mindState.lastInventoryRawFoodUseTick = Find.TickManager.TicksGame;
				int num = FoodUtility.StackCountForNutrition(feedNutritionLeft, thing.GetStatValue(StatDefOf.Nutrition));
				int stackCount = thing.stackCount;
				Thing thing2 = actor.inventory.innerContainer.Take(thing, Mathf.Min(num, stackCount));
				actor.carryTracker.TryStartCarry(thing2);
				actor.CurJob.SetTarget(TargetIndex.B, thing2);
				float num2 = (float)thing2.stackCount * thing2.GetStatValue(StatDefOf.Nutrition);
				ticksLeftThisToil = Mathf.CeilToInt(270f * (num2 / RequiredNutritionPerFeed(pawn)));
				if (num <= stackCount)
				{
					feedNutritionLeft = 0f;
				}
				else
				{
					feedNutritionLeft -= num2;
					if (feedNutritionLeft < 0.001f)
					{
						feedNutritionLeft = 0f;
					}
				}
			}
		};
		toil.defaultCompleteMode = ToilCompleteMode.Delay;
		toil.activeSkill = () => SkillDefOf.Animals;
		return toil;
	}
}
