using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse.AI;

public class JobDriver_PlayWalking : JobDriver_BabyPlay
{
	[CompilerGenerated]
	private sealed class _003CPlay_003Ed__10 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_PlayWalking _003C_003E4__this;

		private Toil _003CplayWalking_003E5__2;

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
		public _003CPlay_003Ed__10(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003CplayWalking_003E5__2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			JobDriver_PlayWalking CS_0024_003C_003E8__locals5 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003CplayWalking_003E5__2 = CS_0024_003C_003E8__locals5.PlayWalking();
				_003C_003E2__current = _003CplayWalking_003E5__2;
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = CS_0024_003C_003E8__locals5.Interact();
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Jump.JumpIf(_003CplayWalking_003E5__2, delegate
				{
					IntVec3 intVec = TryFindWanderCell(CS_0024_003C_003E8__locals5.pawn, CS_0024_003C_003E8__locals5.pawn.Position);
					if (intVec.IsValid)
					{
						CS_0024_003C_003E8__locals5.job.SetTarget(TargetIndex.B, intVec);
						return true;
					}
					return false;
				});
				_003C_003E1__state = 3;
				return true;
			case 3:
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
			_003CPlay_003Ed__10 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CPlay_003Ed__10(0)
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

	private const TargetIndex WanderCellInd = TargetIndex.B;

	private const int InteractionTicks = 600;

	private const int InteractionIntervalTicks = 300;

	private const float WanderRadius = 8f;

	protected override StartingConditions StartingCondition => StartingConditions.PickupBaby;

	public override Vector3 ForcedBodyOffset
	{
		get
		{
			if (!pawn.IsCarryingPawn(base.Baby))
			{
				return Vector3.zero;
			}
			float f = (float)Find.TickManager.TicksGame / 60f * 2f;
			if (pawn.pather.Moving)
			{
				float f2 = Mathf.Sin(f);
				float num = Mathf.Sign(f2);
				return new Vector3(EasingFunctions.EaseInOutQuad(Mathf.Abs(f2) * 0.6f) * num * 0.12f, 0f, 0f);
			}
			float z = EasingFunctions.EaseInOutQuint(Mathf.Abs(Mathf.Sin(f) * 0.6f)) * 0.12f;
			return new Vector3(0f, 0f, z);
		}
	}

	public static IntVec3 TryFindWanderCell(Pawn pawn, IntVec3 root)
	{
		return RCellFinder.RandomWanderDestFor(pawn, root, 8f, null, PawnUtility.ResolveMaxDanger(pawn, Danger.None));
	}

	private void CheckEndJobSuccess(int delta)
	{
		if (roomPlayGainFactor < 0f || pawn.IsHashIntervalTick(300, delta))
		{
			roomPlayGainFactor = BabyPlayUtility.GetRoomPlayGainFactors(base.Baby);
		}
		if (BabyPlayUtility.PlayTickCheckEnd(base.Baby, pawn, roomPlayGainFactor, delta))
		{
			pawn.jobs.curDriver.EndJobWith(JobCondition.Succeeded);
		}
	}

	[IteratorStateMachine(typeof(_003CPlay_003Ed__10))]
	protected override IEnumerable<Toil> Play()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CPlay_003Ed__10(-2)
		{
			_003C_003E4__this = this
		};
	}

	private Toil PlayWalking()
	{
		Toil toil = Toils_Goto.Goto(TargetIndex.B, PathEndMode.OnCell);
		toil.initAction = (Action)Delegate.Combine(toil.initAction, (Action)delegate
		{
			job.locomotionUrgency = LocomotionUrgency.Amble;
		});
		toil.tickIntervalAction = (Action<int>)Delegate.Combine(toil.tickIntervalAction, new Action<int>(CheckEndJobSuccess));
		ChildcareUtility.MakeBabyPlayAsLongAsToilIsActive(toil, TargetIndex.A);
		return toil;
	}

	private Toil Interact()
	{
		Toil toil = Toils_General.Wait(600);
		toil.WithEffect(EffecterDefOf.PlayStatic, TargetIndex.A, null);
		toil.initAction = (Action)Delegate.Combine(toil.initAction, (Action)delegate
		{
			pawn.Rotation = Rot4.Random;
		});
		toil.tickIntervalAction = (Action<int>)Delegate.Combine(toil.tickIntervalAction, (Action<int>)delegate(int delta)
		{
			if (pawn.IsHashIntervalTick(300, delta))
			{
				pawn.interactions.TryInteractWith(base.Baby, InteractionDefOf.BabyPlay);
			}
			CheckEndJobSuccess(delta);
		});
		ChildcareUtility.MakeBabyPlayAsLongAsToilIsActive(toil, TargetIndex.A);
		return toil;
	}
}
