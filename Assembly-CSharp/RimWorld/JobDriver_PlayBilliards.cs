using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld;

public class JobDriver_PlayBilliards : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__2 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_PlayBilliards _003C_003E4__this;

		private Toil _003CchooseCell_003E5__2;

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
		public _003CMakeNewToils_003Ed__2(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003CchooseCell_003E5__2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			JobDriver_PlayBilliards CS_0024_003C_003E8__locals15 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				CS_0024_003C_003E8__locals15.EndOnDespawnedOrNull(TargetIndex.A);
				_003CchooseCell_003E5__2 = Toils_Misc.FindRandomAdjacentReachableCell(TargetIndex.A, TargetIndex.B);
				_003C_003E2__current = _003CchooseCell_003E5__2;
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Reserve.Reserve(TargetIndex.B);
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Goto.GotoCell(TargetIndex.B, PathEndMode.OnCell);
				_003C_003E1__state = 3;
				return true;
			case 3:
			{
				_003C_003E1__state = -1;
				Toil toil = ToilMaker.MakeToil("MakeNewToils");
				toil.initAction = delegate
				{
					CS_0024_003C_003E8__locals15.job.locomotionUrgency = LocomotionUrgency.Walk;
				};
				toil.tickIntervalAction = delegate(int delta)
				{
					CS_0024_003C_003E8__locals15.pawn.rotationTracker.FaceCell(CS_0024_003C_003E8__locals15.TargetA.Thing.OccupiedRect().ClosestCellTo(CS_0024_003C_003E8__locals15.pawn.Position));
					int num2 = 300;
					if (CS_0024_003C_003E8__locals15.ticksLeftThisToil + delta > num2 && CS_0024_003C_003E8__locals15.ticksLeftThisToil <= num2)
					{
						SoundDefOf.PlayBilliards.PlayOneShot(new TargetInfo(CS_0024_003C_003E8__locals15.pawn.Position, CS_0024_003C_003E8__locals15.pawn.Map));
					}
					if (Find.TickManager.TicksGame > CS_0024_003C_003E8__locals15.startTick + CS_0024_003C_003E8__locals15.job.def.joyDuration)
					{
						CS_0024_003C_003E8__locals15.EndJobWith(JobCondition.Succeeded);
					}
					else
					{
						JoyUtility.JoyTickCheckEnd(CS_0024_003C_003E8__locals15.pawn, delta, JoyTickFullJoyAction.EndJob, 1f, (Building)CS_0024_003C_003E8__locals15.TargetThingA);
					}
				};
				toil.handlingFacing = true;
				toil.socialMode = RandomSocialMode.SuperActive;
				toil.defaultCompleteMode = ToilCompleteMode.Delay;
				toil.defaultDuration = 600;
				toil.AddFinishAction(delegate
				{
					JoyUtility.TryGainRecRoomThought(CS_0024_003C_003E8__locals15.pawn);
				});
				_003C_003E2__current = toil;
				_003C_003E1__state = 4;
				return true;
			}
			case 4:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Reserve.Release(TargetIndex.B);
				_003C_003E1__state = 5;
				return true;
			case 5:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Jump.Jump(_003CchooseCell_003E5__2);
				_003C_003E1__state = 6;
				return true;
			case 6:
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
			_003CMakeNewToils_003Ed__2 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__2(0)
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

	private const int ShotDuration = 600;

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		return pawn.Reserve(job.targetA, job, job.def.joyMaxParticipants, 0, null, errorOnFailed);
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__2))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__2(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override object[] TaleParameters()
	{
		return new object[2]
		{
			pawn,
			base.TargetA.Thing.def
		};
	}
}
