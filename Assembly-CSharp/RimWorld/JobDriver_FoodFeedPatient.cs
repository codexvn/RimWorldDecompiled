using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_FoodFeedPatient : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__15 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_FoodFeedPatient _003C_003E4__this;

		private Toil _003CcarryFoodFromInventory_003E5__2;

		private Toil _003CgoToNutrientDispenser_003E5__3;

		private Toil _003CgoToFoodHolder_003E5__4;

		private Toil _003CcarryFoodToPatient_003E5__5;

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
			_003CcarryFoodFromInventory_003E5__2 = null;
			_003CgoToNutrientDispenser_003E5__3 = null;
			_003CgoToFoodHolder_003E5__4 = null;
			_003CcarryFoodToPatient_003E5__5 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			JobDriver_FoodFeedPatient CS_0024_003C_003E8__locals24 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				CS_0024_003C_003E8__locals24.FailOnDespawnedNullOrForbidden(TargetIndex.B);
				CS_0024_003C_003E8__locals24.FailOn(() => !FoodUtility.ShouldBeFedBySomeone(CS_0024_003C_003E8__locals24.Deliveree));
				_003CcarryFoodFromInventory_003E5__2 = Toils_Misc.TakeItemFromInventoryToCarrier(CS_0024_003C_003E8__locals24.pawn, TargetIndex.A);
				_003CgoToNutrientDispenser_003E5__3 = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell).FailOnForbidden(TargetIndex.A);
				_003CgoToFoodHolder_003E5__4 = Toils_Goto.GotoThing(TargetIndex.C, PathEndMode.Touch).FailOn(() => CS_0024_003C_003E8__locals24.FoodHolder != CS_0024_003C_003E8__locals24.FoodHolderInventory?.pawn || CS_0024_003C_003E8__locals24.FoodHolder.IsForbidden(CS_0024_003C_003E8__locals24.pawn));
				_003CcarryFoodToPatient_003E5__5 = Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.Touch);
				_003C_003E2__current = Toils_Jump.JumpIf(_003CcarryFoodFromInventory_003E5__2, () => CS_0024_003C_003E8__locals24.pawn.inventory != null && CS_0024_003C_003E8__locals24.pawn.inventory.Contains(CS_0024_003C_003E8__locals24.TargetThingA));
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Haul.CheckItemCarriedByOtherPawn(CS_0024_003C_003E8__locals24.Food, TargetIndex.C, _003CgoToFoodHolder_003E5__4);
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Jump.JumpIf(_003CgoToNutrientDispenser_003E5__3, () => CS_0024_003C_003E8__locals24.TargetThingA is Building_NutrientPasteDispenser);
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOnForbidden(TargetIndex.A);
				_003C_003E1__state = 4;
				return true;
			case 4:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Ingest.PickupIngestible(TargetIndex.A, CS_0024_003C_003E8__locals24.Deliveree);
				_003C_003E1__state = 5;
				return true;
			case 5:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Jump.Jump(_003CcarryFoodToPatient_003E5__5);
				_003C_003E1__state = 6;
				return true;
			case 6:
				_003C_003E1__state = -1;
				_003C_003E2__current = _003CgoToFoodHolder_003E5__4;
				_003C_003E1__state = 7;
				return true;
			case 7:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_General.Wait(25).WithProgressBarToilDelay(TargetIndex.C);
				_003C_003E1__state = 8;
				return true;
			case 8:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Haul.TakeFromOtherInventory(CS_0024_003C_003E8__locals24.Food, CS_0024_003C_003E8__locals24.pawn.inventory.innerContainer, CS_0024_003C_003E8__locals24.FoodHolderInventory?.innerContainer, CS_0024_003C_003E8__locals24.job.count, TargetIndex.A);
				_003C_003E1__state = 9;
				return true;
			case 9:
				_003C_003E1__state = -1;
				_003C_003E2__current = _003CcarryFoodFromInventory_003E5__2;
				_003C_003E1__state = 10;
				return true;
			case 10:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Jump.Jump(_003CcarryFoodToPatient_003E5__5);
				_003C_003E1__state = 11;
				return true;
			case 11:
				_003C_003E1__state = -1;
				_003C_003E2__current = _003CgoToNutrientDispenser_003E5__3;
				_003C_003E1__state = 12;
				return true;
			case 12:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Ingest.TakeMealFromDispenser(TargetIndex.A, CS_0024_003C_003E8__locals24.pawn);
				_003C_003E1__state = 13;
				return true;
			case 13:
				_003C_003E1__state = -1;
				_003C_003E2__current = _003CcarryFoodToPatient_003E5__5;
				_003C_003E1__state = 14;
				return true;
			case 14:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Ingest.ChewIngestible(CS_0024_003C_003E8__locals24.Deliveree, 1.5f, TargetIndex.A).FailOnCannotTouch(TargetIndex.B, PathEndMode.Touch);
				_003C_003E1__state = 15;
				return true;
			case 15:
			{
				_003C_003E1__state = -1;
				Toil toil = Toils_Ingest.FinalizeIngest(CS_0024_003C_003E8__locals24.Deliveree, TargetIndex.A);
				toil.finishActions = new List<Action>
				{
					delegate
					{
						if (ModsConfig.AnomalyActive && Rand.Chance(0.3f) && MetalhorrorUtility.IsInfected(CS_0024_003C_003E8__locals24.pawn))
						{
							MetalhorrorUtility.Infect(CS_0024_003C_003E8__locals24.Deliveree, CS_0024_003C_003E8__locals24.pawn, "FeedingImplant");
						}
					}
				};
				_003C_003E2__current = toil;
				_003C_003E1__state = 16;
				return true;
			}
			case 16:
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

	private const TargetIndex FoodSourceInd = TargetIndex.A;

	private const TargetIndex DelivereeInd = TargetIndex.B;

	private const TargetIndex FoodHolderInd = TargetIndex.C;

	private const float FeedDurationMultiplier = 1.5f;

	private const float MetalhorrorInfectionChance = 0.3f;

	protected Thing Food => job.targetA.Thing;

	protected Pawn Deliveree => job.targetB.Pawn;

	protected Pawn_InventoryTracker FoodHolderInventory => Food?.ParentHolder as Pawn_InventoryTracker;

	protected Pawn FoodHolder => job.targetC.Pawn;

	public override string GetReport()
	{
		if (job.GetTarget(TargetIndex.A).Thing is Building_NutrientPasteDispenser && Deliveree != null)
		{
			return JobUtility.GetResolvedJobReportRaw(job.def.reportString, ThingDefOf.MealNutrientPaste.label, ThingDefOf.MealNutrientPaste, Deliveree.LabelShort, Deliveree, "", "");
		}
		return base.GetReport();
	}

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		if (!pawn.Reserve(Deliveree, job, 1, -1, null, errorOnFailed))
		{
			return false;
		}
		if (!(base.TargetThingA is Building_NutrientPasteDispenser) && (pawn.inventory == null || !pawn.inventory.Contains(base.TargetThingA)))
		{
			int maxAmountToPickup = FoodUtility.GetMaxAmountToPickup(Food, pawn, job.count);
			if (!pawn.Reserve(Food, job, 10, maxAmountToPickup, null, errorOnFailed))
			{
				return false;
			}
			job.count = maxAmountToPickup;
		}
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
}
