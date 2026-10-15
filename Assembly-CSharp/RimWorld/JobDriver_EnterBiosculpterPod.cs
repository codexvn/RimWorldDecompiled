using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_EnterBiosculpterPod : JobDriver
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass7_0
	{
		public Toil enter;

		public JobDriver_EnterBiosculpterPod _003C_003E4__this;

		internal void _003CMakeNewToils_003Eb__4()
		{
			_003C_003E4__this.BiosculpterPod.TryAcceptPawn(enter.actor, _003C_003E4__this.job.biosculpterCycleKey);
		}
	}

	[CompilerGenerated]
	private sealed class _003CCollectIngredientsToilsHelper_003Ed__9 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private TargetIndex ingredientIndex;

		public TargetIndex _003C_003E3__ingredientIndex;

		private Pawn carrier;

		public Pawn _003C_003E3__carrier;

		private List<Thing> pickedUpIngredients;

		public List<Thing> _003C_003E3__pickedUpIngredients;

		private Toil _003Cextract_003E5__2;

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
		public _003CCollectIngredientsToilsHelper_003Ed__9(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003Cextract_003E5__2 = null;
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
				_003Cextract_003E5__2 = Toils_JobTransforms.ExtractNextTargetFromQueue(ingredientIndex);
				_003C_003E2__current = _003Cextract_003E5__2;
				_003C_003E1__state = 1;
				return true;
			case 1:
			{
				_003C_003E1__state = -1;
				Toil toil = Toils_Goto.GotoThing(ingredientIndex, PathEndMode.ClosestTouch).FailOnDespawnedNullOrForbidden(ingredientIndex).FailOnSomeonePhysicallyInteracting(ingredientIndex);
				_003C_003E2__current = toil;
				_003C_003E1__state = 2;
				return true;
			}
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Haul.StartCarryThing(ingredientIndex, putRemainderInQueue: true, subtractNumTakenFromJobCount: false, failIfStackCountLessThanJobCount: true);
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				_003C_003E2__current = HaulBiosculpterIngredientInInventory(carrier, pickedUpIngredients);
				_003C_003E1__state = 4;
				return true;
			case 4:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Jump.JumpIfHaveTargetInQueue(ingredientIndex, _003Cextract_003E5__2);
				_003C_003E1__state = 5;
				return true;
			case 5:
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
			_003CCollectIngredientsToilsHelper_003Ed__9 _003CCollectIngredientsToilsHelper_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CCollectIngredientsToilsHelper_003Ed__ = this;
			}
			else
			{
				_003CCollectIngredientsToilsHelper_003Ed__ = new _003CCollectIngredientsToilsHelper_003Ed__9(0);
			}
			_003CCollectIngredientsToilsHelper_003Ed__.ingredientIndex = _003C_003E3__ingredientIndex;
			_003CCollectIngredientsToilsHelper_003Ed__.carrier = _003C_003E3__carrier;
			_003CCollectIngredientsToilsHelper_003Ed__.pickedUpIngredients = _003C_003E3__pickedUpIngredients;
			return _003CCollectIngredientsToilsHelper_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Toil>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__7 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_EnterBiosculpterPod _003C_003E4__this;

		private Toil _003CgoToPod_003E5__2;

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
		public _003CMakeNewToils_003Ed__7(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 2)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003CgoToPod_003E5__2 = null;
			_003C_003E7__wrap2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				JobDriver_EnterBiosculpterPod CS_0024_003C_003E8__locals31 = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (!ModLister.CheckIdeology("Biosculpting"))
					{
						return false;
					}
					CS_0024_003C_003E8__locals31.AddFinishAction(delegate
					{
						if (CS_0024_003C_003E8__locals31.BiosculpterPod != null)
						{
							if (CS_0024_003C_003E8__locals31.BiosculpterPod.queuedEnterJob == CS_0024_003C_003E8__locals31.job)
							{
								CS_0024_003C_003E8__locals31.BiosculpterPod.ClearQueuedInformation();
							}
							if (CS_0024_003C_003E8__locals31.BiosculpterPod.Occupant != CS_0024_003C_003E8__locals31.GetActor())
							{
								foreach (Thing pickedUpIngredient in CS_0024_003C_003E8__locals31.pickedUpIngredients)
								{
									if (CS_0024_003C_003E8__locals31.pawn.inventory.Contains(pickedUpIngredient))
									{
										CS_0024_003C_003E8__locals31.pawn.inventory.innerContainer.TryDrop(pickedUpIngredient, ThingPlaceMode.Near, out var _);
									}
								}
							}
						}
					});
					CS_0024_003C_003E8__locals31.FailOnDespawnedOrNull(TargetIndex.A);
					CS_0024_003C_003E8__locals31.FailOn(() => CS_0024_003C_003E8__locals31.job.biosculpterCycleKey == null || !CS_0024_003C_003E8__locals31.BiosculpterPod.CanAcceptOnceCycleChosen(CS_0024_003C_003E8__locals31.GetActor()));
					_003CgoToPod_003E5__2 = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell);
					_003C_003E2__current = Toils_Jump.JumpIf(_003CgoToPod_003E5__2, () => CS_0024_003C_003E8__locals31.job.GetTargetQueue(TargetIndex.B).NullOrEmpty());
					_003C_003E1__state = 1;
					return true;
				case 1:
					_003C_003E1__state = -1;
					_003C_003E7__wrap2 = CollectIngredientsToilsHelper(TargetIndex.B, CS_0024_003C_003E8__locals31.pawn, CS_0024_003C_003E8__locals31.pickedUpIngredients).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_010a;
				case 2:
					_003C_003E1__state = -3;
					goto IL_010a;
				case 3:
					_003C_003E1__state = -1;
					_003C_003E2__current = PrepareToEnterToil(TargetIndex.A);
					_003C_003E1__state = 4;
					return true;
				case 4:
				{
					_003C_003E1__state = -1;
					_003C_003Ec__DisplayClass7_0 CS_0024_003C_003E8__locals10 = new _003C_003Ec__DisplayClass7_0();
					CS_0024_003C_003E8__locals10._003C_003E4__this = CS_0024_003C_003E8__locals31;
					CS_0024_003C_003E8__locals10.enter = ToilMaker.MakeToil("MakeNewToils");
					CS_0024_003C_003E8__locals10.enter.initAction = delegate
					{
						CS_0024_003C_003E8__locals10._003C_003E4__this.BiosculpterPod.TryAcceptPawn(CS_0024_003C_003E8__locals10.enter.actor, CS_0024_003C_003E8__locals10._003C_003E4__this.job.biosculpterCycleKey);
					};
					CS_0024_003C_003E8__locals10.enter.defaultCompleteMode = ToilCompleteMode.Instant;
					_003C_003E2__current = CS_0024_003C_003E8__locals10.enter;
					_003C_003E1__state = 5;
					return true;
				}
				case 5:
					{
						_003C_003E1__state = -1;
						return false;
					}
					IL_010a:
					if (_003C_003E7__wrap2.MoveNext())
					{
						Toil current = _003C_003E7__wrap2.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap2 = null;
					_003C_003E2__current = _003CgoToPod_003E5__2.FailOn(() => !CS_0024_003C_003E8__locals31.BiosculpterPod.PawnCarryingExtraCycleIngredients(CS_0024_003C_003E8__locals31.pawn, CS_0024_003C_003E8__locals31.job.biosculpterCycleKey));
					_003C_003E1__state = 3;
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
			_003CMakeNewToils_003Ed__7 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__7(0)
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

	public const int EnterPodDelay = 70;

	private List<Thing> pickedUpIngredients = new List<Thing>();

	private const TargetIndex BiosculpterIndex = TargetIndex.A;

	private const TargetIndex IngredientInd = TargetIndex.B;

	private CompBiosculpterPod BiosculpterPod => job.targetA.Thing.TryGetComp<CompBiosculpterPod>();

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		if (!pawn.Reserve(job.targetA, job, 1, -1, null, errorOnFailed))
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

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__7))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__7(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override string GetReport()
	{
		if (!BiosculpterPod.PawnCarryingExtraCycleIngredients(pawn, job.biosculpterCycleKey))
		{
			return "BiosculpterJobReportCollectIngredients".Translate();
		}
		return base.GetReport();
	}

	[IteratorStateMachine(typeof(_003CCollectIngredientsToilsHelper_003Ed__9))]
	public static IEnumerable<Toil> CollectIngredientsToilsHelper(TargetIndex ingredientIndex, Pawn carrier, List<Thing> pickedUpIngredients)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCollectIngredientsToilsHelper_003Ed__9(-2)
		{
			_003C_003E3__ingredientIndex = ingredientIndex,
			_003C_003E3__carrier = carrier,
			_003C_003E3__pickedUpIngredients = pickedUpIngredients
		};
	}

	private static Toil HaulBiosculpterIngredientInInventory(Pawn carrier, List<Thing> pickedUpIngredients)
	{
		Toil toil = ToilMaker.MakeToil("HaulBiosculpterIngredientInInventory");
		toil.initAction = delegate
		{
			Thing carriedThing = carrier.carryTracker.CarriedThing;
			carrier.carryTracker.innerContainer.TryTransferToContainer(carriedThing, carrier.inventory.innerContainer, carriedThing.stackCount, out var resultingTransferredItem);
			if (resultingTransferredItem != null)
			{
				pickedUpIngredients.Add(resultingTransferredItem);
			}
		};
		return toil;
	}

	public static Toil PrepareToEnterToil(TargetIndex podIndex)
	{
		Toil prepare = Toils_General.Wait(70);
		prepare.FailOnCannotTouch(podIndex, PathEndMode.InteractionCell);
		prepare.WithProgressBarToilDelay(podIndex);
		prepare.PlaySustainerOrSound(() => prepare.actor.CurJob.GetTarget(podIndex).Thing?.TryGetComp<CompBiosculpterPod>()?.Props.enterSound);
		return prepare;
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Collections.Look(ref pickedUpIngredients, "pickedUpIngredients", LookMode.Reference);
	}
}
