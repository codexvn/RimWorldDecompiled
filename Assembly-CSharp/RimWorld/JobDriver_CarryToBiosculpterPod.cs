using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_CarryToBiosculpterPod : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__10 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_CarryToBiosculpterPod _003C_003E4__this;

		private Toil _003CgoToTakee_003E5__2;

		private Toil _003CstartCarryingTakee_003E5__3;

		private Toil _003CgoToThing_003E5__4;

		private Toil _003CjumpIfGoToTakee_003E5__5;

		private IEnumerator<Toil> _003C_003E7__wrap5;

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
			int num = _003C_003E1__state;
			if (num == -3 || num == 4)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003CgoToTakee_003E5__2 = null;
			_003CstartCarryingTakee_003E5__3 = null;
			_003CgoToThing_003E5__4 = null;
			_003CjumpIfGoToTakee_003E5__5 = null;
			_003C_003E7__wrap5 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				JobDriver_CarryToBiosculpterPod CS_0024_003C_003E8__locals35 = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
				{
					_003C_003E1__state = -1;
					if (!ModLister.CheckIdeology("Biosculpting"))
					{
						return false;
					}
					CS_0024_003C_003E8__locals35.AddFinishAction(delegate
					{
						if (CS_0024_003C_003E8__locals35.Pod != null)
						{
							if (CS_0024_003C_003E8__locals35.Pod.queuedEnterJob == CS_0024_003C_003E8__locals35.job)
							{
								CS_0024_003C_003E8__locals35.Pod.ClearQueuedInformation();
							}
							if (CS_0024_003C_003E8__locals35.Pod.Occupant != CS_0024_003C_003E8__locals35.Takee)
							{
								foreach (Thing pickedUpIngredient in CS_0024_003C_003E8__locals35.pickedUpIngredients)
								{
									Thing lastResultingThing;
									if (CS_0024_003C_003E8__locals35.pawn.inventory.Contains(pickedUpIngredient))
									{
										CS_0024_003C_003E8__locals35.pawn.inventory.innerContainer.TryDrop(pickedUpIngredient, ThingPlaceMode.Near, out lastResultingThing);
									}
									else if (CS_0024_003C_003E8__locals35.Takee.inventory.Contains(pickedUpIngredient))
									{
										CS_0024_003C_003E8__locals35.Takee.inventory.innerContainer.TryDrop(pickedUpIngredient, ThingPlaceMode.Near, out lastResultingThing);
									}
								}
							}
						}
					});
					CS_0024_003C_003E8__locals35.FailOnDestroyedOrNull(TargetIndex.A);
					CS_0024_003C_003E8__locals35.FailOnDestroyedOrNull(TargetIndex.C);
					CS_0024_003C_003E8__locals35.FailOnAggroMentalState(TargetIndex.A);
					CS_0024_003C_003E8__locals35.FailOn(() => CS_0024_003C_003E8__locals35.job.biosculpterCycleKey == null || !CS_0024_003C_003E8__locals35.Pod.CanAcceptOnceCycleChosen(CS_0024_003C_003E8__locals35.Takee));
					_003CgoToTakee_003E5__2 = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.OnCell).FailOnDestroyedNullOrForbidden(TargetIndex.A).FailOnDespawnedNullOrForbidden(TargetIndex.C)
						.FailOn(() => CS_0024_003C_003E8__locals35.Takee.IsFreeColonist && !CS_0024_003C_003E8__locals35.Takee.Downed)
						.FailOnSomeonePhysicallyInteracting(TargetIndex.A);
					_003CstartCarryingTakee_003E5__3 = Toils_Haul.StartCarryThing(TargetIndex.A);
					_003CgoToThing_003E5__4 = Toils_Goto.GotoThing(TargetIndex.C, PathEndMode.InteractionCell);
					Toil toil2 = Toils_Jump.JumpIf(_003CgoToThing_003E5__4, () => CS_0024_003C_003E8__locals35.pawn.IsCarryingPawn(CS_0024_003C_003E8__locals35.Takee) && CS_0024_003C_003E8__locals35.job.GetTargetQueue(TargetIndex.B).NullOrEmpty());
					_003CjumpIfGoToTakee_003E5__5 = Toils_Jump.JumpIf(_003CgoToTakee_003E5__2, () => CS_0024_003C_003E8__locals35.job.GetTargetQueue(TargetIndex.B).NullOrEmpty());
					_003C_003E2__current = toil2;
					_003C_003E1__state = 1;
					return true;
				}
				case 1:
					_003C_003E1__state = -1;
					_003C_003E2__current = _003CjumpIfGoToTakee_003E5__5;
					_003C_003E1__state = 2;
					return true;
				case 2:
					_003C_003E1__state = -1;
					_003C_003E2__current = CS_0024_003C_003E8__locals35.DropCarryToGrabIngredients();
					_003C_003E1__state = 3;
					return true;
				case 3:
					_003C_003E1__state = -1;
					_003C_003E7__wrap5 = JobDriver_EnterBiosculpterPod.CollectIngredientsToilsHelper(TargetIndex.B, CS_0024_003C_003E8__locals35.pawn, CS_0024_003C_003E8__locals35.pickedUpIngredients).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_01d3;
				case 4:
					_003C_003E1__state = -3;
					goto IL_01d3;
				case 5:
					_003C_003E1__state = -1;
					_003C_003E2__current = CS_0024_003C_003E8__locals35.TransferIngredientsAndPrepareCarryDownedPawn();
					_003C_003E1__state = 6;
					return true;
				case 6:
					_003C_003E1__state = -1;
					_003C_003E2__current = _003CstartCarryingTakee_003E5__3;
					_003C_003E1__state = 7;
					return true;
				case 7:
					_003C_003E1__state = -1;
					_003C_003E2__current = _003CgoToThing_003E5__4.FailOn(() => !CS_0024_003C_003E8__locals35.Pod.PawnCarryingExtraCycleIngredients(CS_0024_003C_003E8__locals35.Takee, CS_0024_003C_003E8__locals35.job.biosculpterCycleKey));
					_003C_003E1__state = 8;
					return true;
				case 8:
					_003C_003E1__state = -1;
					_003C_003E2__current = JobDriver_EnterBiosculpterPod.PrepareToEnterToil(TargetIndex.C);
					_003C_003E1__state = 9;
					return true;
				case 9:
				{
					_003C_003E1__state = -1;
					Toil toil = ToilMaker.MakeToil("MakeNewToils");
					toil.initAction = delegate
					{
						CS_0024_003C_003E8__locals35.Pod.TryAcceptPawn(CS_0024_003C_003E8__locals35.Takee, CS_0024_003C_003E8__locals35.job.biosculpterCycleKey);
					};
					toil.defaultCompleteMode = ToilCompleteMode.Instant;
					_003C_003E2__current = toil;
					_003C_003E1__state = 10;
					return true;
				}
				case 10:
					{
						_003C_003E1__state = -1;
						return false;
					}
					IL_01d3:
					if (_003C_003E7__wrap5.MoveNext())
					{
						Toil current = _003C_003E7__wrap5.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 4;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap5 = null;
					_003C_003E2__current = _003CgoToTakee_003E5__2;
					_003C_003E1__state = 5;
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
			if (_003C_003E7__wrap5 != null)
			{
				_003C_003E7__wrap5.Dispose();
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

	private const TargetIndex TakeeInd = TargetIndex.A;

	private const TargetIndex IngredientInd = TargetIndex.B;

	private const TargetIndex PodInd = TargetIndex.C;

	private List<Thing> pickedUpIngredients = new List<Thing>();

	private List<ThingCount> thingsToTransfer = new List<ThingCount>();

	private Pawn Takee => (Pawn)job.GetTarget(TargetIndex.A).Thing;

	private CompBiosculpterPod Pod => job.GetTarget(TargetIndex.C).Thing.TryGetComp<CompBiosculpterPod>();

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		if (!pawn.Reserve(Takee, job, 1, -1, null, errorOnFailed) || !pawn.Reserve(Pod.parent, job, 1, -1, null, errorOnFailed))
		{
			return false;
		}
		List<LocalTargetInfo> targetQueue = job.GetTargetQueue(TargetIndex.B);
		for (int i = 0; i < targetQueue.Count; i++)
		{
			if (!pawn.Reserve(targetQueue[i], job, 1, -1, null, errorOnFailed))
			{
				return false;
			}
		}
		return true;
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

	private Toil TransferIngredientsAndPrepareCarryDownedPawn()
	{
		Toil toil = ToilMaker.MakeToil("TransferIngredientsAndPrepareCarryDownedPawn");
		toil.initAction = delegate
		{
			List<ThingDefCountClass> extraRequiredIngredients = Pod.GetCycle(job.biosculpterCycleKey).Props.extraRequiredIngredients;
			if (extraRequiredIngredients != null && !Pod.devFillPodLatch)
			{
				ThingOwner<Thing> innerContainer = pawn.inventory.innerContainer;
				foreach (ThingDefCountClass item in extraRequiredIngredients)
				{
					if (Takee.inventory.Count(item.thingDef) < item.count)
					{
						if (pawn.inventory.Count(item.thingDef) < item.count)
						{
							EndJobWith(JobCondition.Incompletable);
							return;
						}
						thingsToTransfer.Clear();
						int num = 0;
						foreach (Thing item2 in innerContainer)
						{
							if (num >= item.count)
							{
								break;
							}
							if (item2.def == item.thingDef)
							{
								int num2 = Mathf.Min(item.count - num, item2.stackCount);
								thingsToTransfer.Add(new ThingCount(item2, Mathf.Min(item.count - num, item2.stackCount)));
								num += num2;
							}
						}
						foreach (ThingCount item3 in thingsToTransfer)
						{
							int num3 = Takee.inventory.innerContainer.TryAddOrTransfer(item3.Thing, item3.Count);
							if (num3 != item3.Count)
							{
								Log.Warning($"Only able to transfer x{num3} of the expected x{item3.Count} of {item3.Thing.Label} while CarryToBiosculpter");
								EndJobWith(JobCondition.Incompletable);
								return;
							}
						}
					}
				}
			}
			job.count = 1;
		};
		toil.defaultCompleteMode = ToilCompleteMode.Instant;
		return toil;
	}

	private Toil DropCarryToGrabIngredients()
	{
		Toil toil = ToilMaker.MakeToil("DropCarryToGrabIngredients");
		toil.initAction = delegate
		{
			if (pawn.carryTracker.CarriedThing != null)
			{
				pawn.carryTracker.TryDropCarriedThing(pawn.Position, ThingPlaceMode.Near, out var _);
			}
		};
		toil.defaultCompleteMode = ToilCompleteMode.Instant;
		return toil;
	}

	public override string GetReport()
	{
		if (!Pod.PawnCarryingExtraCycleIngredients(Takee, job.biosculpterCycleKey) && !Pod.PawnCarryingExtraCycleIngredients(pawn, job.biosculpterCycleKey))
		{
			return "BiosculpterJobReportCollectIngredients".Translate();
		}
		return base.GetReport();
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Collections.Look(ref pickedUpIngredients, "pickedUpIngredients", LookMode.Reference);
	}
}
