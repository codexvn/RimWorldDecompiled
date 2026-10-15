using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_VisitSickPawn : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__7 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_VisitSickPawn _003C_003E4__this;

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
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			JobDriver_VisitSickPawn CS_0024_003C_003E8__locals25 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				CS_0024_003C_003E8__locals25.FailOnDespawnedNullOrForbidden(TargetIndex.A);
				CS_0024_003C_003E8__locals25.FailOn(() => !CS_0024_003C_003E8__locals25.Patient.InBed() || !CS_0024_003C_003E8__locals25.Patient.Awake());
				if (CS_0024_003C_003E8__locals25.Chair != null)
				{
					CS_0024_003C_003E8__locals25.FailOnDespawnedNullOrForbidden(TargetIndex.B);
				}
				if (CS_0024_003C_003E8__locals25.Chair != null)
				{
					_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.OnCell);
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell);
				_003C_003E1__state = 2;
				return true;
			case 1:
				_003C_003E1__state = -1;
				goto IL_00a2;
			case 2:
				_003C_003E1__state = -1;
				goto IL_00a2;
			case 3:
			{
				_003C_003E1__state = -1;
				Toil toil = ToilMaker.MakeToil("MakeNewToils");
				toil.tickIntervalAction = delegate(int delta)
				{
					CS_0024_003C_003E8__locals25.Patient.needs.joy?.GainJoy(CS_0024_003C_003E8__locals25.job.def.joyGainRate * 0.000144f * (float)delta, CS_0024_003C_003E8__locals25.job.def.joyKind);
					if (CS_0024_003C_003E8__locals25.pawn.IsHashIntervalTick(320, delta) && CS_0024_003C_003E8__locals25.pawn.interactions.CanInteractNowWith(CS_0024_003C_003E8__locals25.Patient))
					{
						InteractionDef intDef = ((Rand.Value < 0.8f) ? InteractionDefOf.Chitchat : InteractionDefOf.DeepTalk);
						CS_0024_003C_003E8__locals25.pawn.interactions.TryInteractWith(CS_0024_003C_003E8__locals25.Patient, intDef);
					}
					CS_0024_003C_003E8__locals25.pawn.rotationTracker.FaceCell(CS_0024_003C_003E8__locals25.Patient.Position);
					CS_0024_003C_003E8__locals25.pawn.GainComfortFromCellIfPossible(delta);
					if (CS_0024_003C_003E8__locals25.pawn.needs.joy == null || !JoyUtility.JoyTickCheckEnd(CS_0024_003C_003E8__locals25.pawn, delta, JoyTickFullJoyAction.None))
					{
						Need_Joy joy = CS_0024_003C_003E8__locals25.pawn.needs.joy;
						if (joy != null && joy.CurLevelPercentage > 0.9999f)
						{
							Need_Joy joy2 = CS_0024_003C_003E8__locals25.Patient.needs.joy;
							if (joy2 != null && joy2.CurLevelPercentage > 0.9999f)
							{
								CS_0024_003C_003E8__locals25.EndJobWith(JobCondition.Succeeded);
							}
						}
					}
				};
				toil.handlingFacing = true;
				toil.socialMode = RandomSocialMode.Off;
				toil.defaultCompleteMode = ToilCompleteMode.Delay;
				toil.defaultDuration = CS_0024_003C_003E8__locals25.job.def.joyDuration;
				_003C_003E2__current = toil;
				_003C_003E1__state = 4;
				return true;
			}
			case 4:
				{
					_003C_003E1__state = -1;
					return false;
				}
				IL_00a2:
				_003C_003E2__current = Toils_Interpersonal.WaitToBeAbleToInteract(CS_0024_003C_003E8__locals25.pawn);
				_003C_003E1__state = 3;
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

	private const TargetIndex PatientInd = TargetIndex.A;

	private const TargetIndex ChairInd = TargetIndex.B;

	private Pawn Patient => (Pawn)job.GetTarget(TargetIndex.A).Thing;

	private Thing Chair => job.GetTarget(TargetIndex.B).Thing;

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		if (!pawn.Reserve(Patient, job, 1, -1, null, errorOnFailed))
		{
			return false;
		}
		if (Chair != null && !pawn.Reserve(Chair, job, 1, -1, null, errorOnFailed))
		{
			return false;
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
}
