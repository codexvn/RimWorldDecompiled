using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_ActivateMonolith : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__6 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_ActivateMonolith _003C_003E4__this;

		private Toil _003CgetToHaulTarget_003E5__2;

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
		public _003CMakeNewToils_003Ed__6(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003CgetToHaulTarget_003E5__2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			JobDriver_ActivateMonolith CS_0024_003C_003E8__locals12 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				CS_0024_003C_003E8__locals12.FailOn(() => !CS_0024_003C_003E8__locals12.VoidMonolith.CanActivate(out var _, out var _));
				if (Find.Anomaly.NextLevelDef == MonolithLevelDefOf.VoidAwakened)
				{
					_003C_003E2__current = Toils_General.Do(delegate
					{
						Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("VoidAwakeningConfirmationText".Translate(), delegate
						{
						}, delegate
						{
							CS_0024_003C_003E8__locals12.pawn.jobs.EndCurrentJob(JobCondition.Succeeded);
						}));
					});
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_008c;
			}
			case 1:
				_003C_003E1__state = -1;
				goto IL_008c;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Haul.StartCarryThing(TargetIndex.B, putRemainderInQueue: false, subtractNumTakenFromJobCount: true, failIfStackCountLessThanJobCount: false, reserve: true, canTakeFromInventory: true);
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Haul.JumpIfAlsoCollectingNextTargetInQueue(_003CgetToHaulTarget_003E5__2, TargetIndex.B);
				_003C_003E1__state = 4;
				return true;
			case 4:
				_003C_003E1__state = -1;
				_003CgetToHaulTarget_003E5__2 = null;
				goto IL_0117;
			case 5:
			{
				_003C_003E1__state = -1;
				int ticks = Mathf.RoundToInt(600f);
				Toil toil = Toils_General.WaitWith(TargetIndex.A, ticks, useProgressBar: true, maintainPosture: true, maintainSleep: false, TargetIndex.A);
				toil.WithEffect(EffecterDefOf.MonolithStage2, () => CS_0024_003C_003E8__locals12.TargetA, null);
				toil.tickAction = (Action)Delegate.Combine(toil.tickAction, (Action)delegate
				{
					Find.TickManager.slower.SignalForceNormalSpeed();
				});
				if (Find.Anomaly.LevelDef.activateSound != null)
				{
					toil.PlaySustainerOrSound(Find.Anomaly.LevelDef.activateSound);
				}
				_003C_003E2__current = toil;
				_003C_003E1__state = 6;
				return true;
			}
			case 6:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_General.Do(delegate
				{
					if (CS_0024_003C_003E8__locals12.TargetThingB != null)
					{
						CS_0024_003C_003E8__locals12.pawn.carryTracker.DestroyCarriedThing();
					}
					CS_0024_003C_003E8__locals12.VoidMonolith.Activate(CS_0024_003C_003E8__locals12.pawn);
				});
				_003C_003E1__state = 7;
				return true;
			case 7:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_General.Wait(360, TargetIndex.A);
				_003C_003E1__state = 8;
				return true;
			case 8:
				{
					_003C_003E1__state = -1;
					return false;
				}
				IL_0117:
				if (CS_0024_003C_003E8__locals12.job.targetC == null)
				{
					CS_0024_003C_003E8__locals12.job.targetC = CS_0024_003C_003E8__locals12.TargetThingA.SpawnedParentOrMe;
				}
				_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.C, PathEndMode.InteractionCell).FailOnDespawnedNullOrForbidden(TargetIndex.C).FailOnSomeonePhysicallyInteracting(TargetIndex.C);
				_003C_003E1__state = 5;
				return true;
				IL_008c:
				if (CS_0024_003C_003E8__locals12.TargetThingB != null)
				{
					_003CgetToHaulTarget_003E5__2 = Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.ClosestTouch, canGotoSpawnedParent: true).FailOnDespawnedNullOrForbidden(TargetIndex.B).FailOnSomeonePhysicallyInteracting(TargetIndex.B);
					_003C_003E2__current = _003CgetToHaulTarget_003E5__2;
					_003C_003E1__state = 2;
					return true;
				}
				goto IL_0117;
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
			_003CMakeNewToils_003Ed__6 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__6(0)
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

	private const int ActivateSeconds = 10;

	private const TargetIndex MonolithIndex = TargetIndex.A;

	private const TargetIndex ActivationItemIndex = TargetIndex.B;

	private Building_VoidMonolith VoidMonolith => job.GetTarget(TargetIndex.A).Thing as Building_VoidMonolith;

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		if (base.TargetThingC != null)
		{
			pawn.ReserveAsManyAsPossible(job.GetTargetQueue(TargetIndex.B), job);
		}
		if (base.TargetThingB != null && !pawn.Reserve(job.GetTarget(TargetIndex.B), job, 1, -1, null, errorOnFailed))
		{
			return false;
		}
		if (!pawn.Reserve(VoidMonolith, job, 1, -1, null, errorOnFailed))
		{
			return false;
		}
		if (!pawn.ReserveSittableOrSpot(VoidMonolith.InteractionCell, job, errorOnFailed))
		{
			return false;
		}
		return true;
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__6))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__6(-2)
		{
			_003C_003E4__this = this
		};
	}
}
