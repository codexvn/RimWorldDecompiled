using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_InteractThing : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__12 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_InteractThing _003C_003E4__this;

		private Toil _003Copt_003E5__2;

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
			_003Copt_003E5__2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			JobDriver_InteractThing CS_0024_003C_003E8__locals24 = _003C_003E4__this;
			Toil toil;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (CS_0024_003C_003E8__locals24.job.GetTarget(TargetIndex.B).HasThing)
				{
					_003Copt_003E5__2 = Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.ClosestTouch, canGotoSpawnedParent: true).FailOnDespawnedNullOrForbidden(TargetIndex.B).FailOnSomeonePhysicallyInteracting(TargetIndex.B)
						.FailOn(() => !CS_0024_003C_003E8__locals24.Interactable.CanInteract(CS_0024_003C_003E8__locals24.pawn));
					_003C_003E2__current = _003Copt_003E5__2;
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_00e3;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Haul.StartCarryThing(TargetIndex.B, putRemainderInQueue: false, subtractNumTakenFromJobCount: true, failIfStackCountLessThanJobCount: false, reserve: true, canTakeFromInventory: true);
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Haul.JumpIfAlsoCollectingNextTargetInQueue(_003Copt_003E5__2, TargetIndex.B);
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				_003Copt_003E5__2 = null;
				goto IL_00e3;
			case 4:
				_003C_003E1__state = -1;
				if (CS_0024_003C_003E8__locals24.Interactable.TicksToActivate != 0)
				{
					int num2 = CS_0024_003C_003E8__locals24.Interactable.TicksToActivate;
					if (CS_0024_003C_003E8__locals24.Interactable.Props.activateStat != null && CS_0024_003C_003E8__locals24.pawn.GetStatValue(CS_0024_003C_003E8__locals24.Interactable.Props.activateStat) > 0f)
					{
						num2 = Mathf.RoundToInt((float)num2 / CS_0024_003C_003E8__locals24.pawn.GetStatValue(CS_0024_003C_003E8__locals24.Interactable.Props.activateStat));
					}
					int remainingTicks = Mathf.RoundToInt((float)num2 * (1f - CS_0024_003C_003E8__locals24.Interactable.progress));
					_003C_003E2__current = CS_0024_003C_003E8__locals24.WaitForActivate(remainingTicks, num2);
					_003C_003E1__state = 5;
					return true;
				}
				goto IL_021c;
			case 5:
				_003C_003E1__state = -1;
				goto IL_021c;
			case 6:
				{
					_003C_003E1__state = -1;
					return false;
				}
				IL_00e3:
				if (CS_0024_003C_003E8__locals24.job.GetTarget(TargetIndex.A).Thing.SpawnedParentOrMe != null)
				{
					CS_0024_003C_003E8__locals24.job.SetTarget(TargetIndex.C, CS_0024_003C_003E8__locals24.job.GetTarget(TargetIndex.A).Thing.SpawnedParentOrMe);
				}
				toil = Toils_Goto.GotoThing(TargetIndex.C, PathEndMode.Touch).FailOnDespawnedNullOrForbidden(TargetIndex.C).FailOnSomeonePhysicallyInteracting(TargetIndex.C)
					.FailOn(() => !CS_0024_003C_003E8__locals24.Interactable.CanInteract(CS_0024_003C_003E8__locals24.pawn));
				_003C_003E2__current = toil;
				_003C_003E1__state = 4;
				return true;
				IL_021c:
				_003C_003E2__current = Toils_General.Do(delegate
				{
					if (CS_0024_003C_003E8__locals24.HasOptionalThing)
					{
						if (!CS_0024_003C_003E8__locals24.pawn.IsCarryingThing(CS_0024_003C_003E8__locals24.OptionalThing))
						{
							Log.ErrorOnce("Interact driver required item (" + CS_0024_003C_003E8__locals24.OptionalThing.Label + ") but was not carrying it.", 77979685);
							return;
						}
						CS_0024_003C_003E8__locals24.pawn.carryTracker.DestroyCarriedThing();
					}
					CS_0024_003C_003E8__locals24.Interactable.Interact(CS_0024_003C_003E8__locals24.pawn);
				});
				_003C_003E1__state = 6;
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

	private const TargetIndex InteractableIndex = TargetIndex.A;

	private const TargetIndex OptionalItemIndex = TargetIndex.B;

	private const TargetIndex SpawnedOrParentIndex = TargetIndex.C;

	private Thing InteractableThing => job.GetTarget(TargetIndex.A).Thing;

	private Thing OptionalThing => job.GetTarget(TargetIndex.B).Thing;

	private bool HasOptionalThing => OptionalThing != null;

	private CompInteractable Interactable
	{
		get
		{
			if (job.interactableIndex == -1)
			{
				return job.GetTarget(TargetIndex.A).Thing.TryGetComp<CompInteractable>();
			}
			if (!(job.GetTarget(TargetIndex.A).Thing is ThingWithComps thingWithComps))
			{
				return null;
			}
			return thingWithComps.GetComps<CompInteractable>().ToList()[job.interactableIndex];
		}
	}

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		if (base.TargetThingB != null)
		{
			pawn.ReserveAsManyAsPossible(job.GetTargetQueue(TargetIndex.B), job);
		}
		if (base.TargetThingB != null && !pawn.Reserve(job.GetTarget(TargetIndex.B), job, 1, -1, null, errorOnFailed))
		{
			return false;
		}
		return pawn.Reserve(job.GetTarget(TargetIndex.A), job, 1, -1, null, errorOnFailed);
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__12))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__12(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override string GetReport()
	{
		if (base.CurToilIndex != 1 || ticksLeftThisToil <= 0)
		{
			if (string.IsNullOrEmpty(Interactable.Props.activatingStringPending))
			{
				return base.GetReport();
			}
			return Interactable.Props.activatingStringPending.Formatted(Interactable.parent.LabelShort);
		}
		int ticksToActivate = Interactable.TicksToActivate;
		int num = ((ticksLeftThisToil < 0) ? ticksToActivate : ticksLeftThisToil);
		float num2 = (float)(ticksToActivate - (ticksToActivate - num)) / 60f;
		if (string.IsNullOrEmpty(Interactable.Props.activatingString))
		{
			return base.GetReport();
		}
		return Interactable.Props.activatingString.Formatted(Interactable.parent.LabelShort, num2);
	}

	private Toil WaitForActivate(int remainingTicks, int totalTicks)
	{
		Toil toil = ToilMaker.MakeToil("WaitForActivate").FailOn(() => !Interactable.CanInteract(pawn));
		if (Interactable.Props.maintainProgress)
		{
			toil.WithProgressBar(TargetIndex.A, () => Interactable.progress);
		}
		else
		{
			toil.WithProgressBarToilDelay(TargetIndex.A, remainingTicks);
		}
		Toil toil2 = toil;
		toil2.initAction = (Action)Delegate.Combine(toil2.initAction, (Action)delegate
		{
			Interactable.Notify_InteractionStarted();
			toil.actor.pather.StopDead();
		});
		Toil toil3 = toil;
		toil3.tickIntervalAction = (Action<int>)Delegate.Combine(toil3.tickIntervalAction, (Action<int>)delegate
		{
			pawn.rotationTracker.FaceTarget(base.TargetA);
		});
		if (Interactable.Props.maintainProgress)
		{
			Toil toil4 = toil;
			toil4.tickIntervalAction = (Action<int>)Delegate.Combine(toil4.tickIntervalAction, (Action<int>)delegate
			{
				Interactable.progress = 1f - (float)ticksLeftThisToil / (float)totalTicks;
			});
		}
		if (Interactable.Props.forceNormalSpeedOnInteracting)
		{
			Toil toil5 = toil;
			toil5.tickAction = (Action)Delegate.Combine(toil5.tickAction, (Action)delegate
			{
				Find.TickManager.slower.SignalForceNormalSpeed();
			});
		}
		toil.AddFinishAction(delegate
		{
			Interactable.Notify_InteractionEnded();
		});
		toil.handlingFacing = true;
		toil.defaultCompleteMode = ToilCompleteMode.Delay;
		toil.socialMode = RandomSocialMode.Off;
		toil.defaultDuration = remainingTicks;
		return toil;
	}
}
