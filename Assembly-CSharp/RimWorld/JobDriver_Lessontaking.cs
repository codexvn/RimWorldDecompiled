using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_Lessontaking : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__8 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_Lessontaking _003C_003E4__this;

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
		public _003CMakeNewToils_003Ed__8(int _003C_003E1__state)
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
			JobDriver_Lessontaking CS_0024_003C_003E8__locals15 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				CS_0024_003C_003E8__locals15.FailOnDespawnedNullOrForbidden(TargetIndex.A);
				CS_0024_003C_003E8__locals15.FailOnChildLearningConditions();
				CS_0024_003C_003E8__locals15.FailOn(delegate
				{
					if (CS_0024_003C_003E8__locals15.Teacher == null)
					{
						if (++CS_0024_003C_003E8__locals15.waitingForTeacherTicks > 5000)
						{
							return true;
						}
						return false;
					}
					CS_0024_003C_003E8__locals15.waitingForTeacherTicks = 0;
					Job job = CS_0024_003C_003E8__locals15.Teacher.jobs?.curJob;
					return (job == null || job.def != JobDefOf.Lessongiving || job.GetTarget(TargetIndex.B).Pawn != CS_0024_003C_003E8__locals15.pawn || job.GetTarget(TargetIndex.A).Thing != CS_0024_003C_003E8__locals15.Desk) ? true : false;
				});
				_003C_003E2__current = Toils_Goto.GotoCell(SchoolUtility.DeskSpotStudent(CS_0024_003C_003E8__locals15.Desk), PathEndMode.OnCell);
				_003C_003E1__state = 1;
				return true;
			case 1:
			{
				_003C_003E1__state = -1;
				Toil toil = ToilMaker.MakeToil("MakeNewToils");
				toil.tickIntervalAction = delegate(int delta)
				{
					CS_0024_003C_003E8__locals15.pawn.GainComfortFromCellIfPossible(delta);
					CS_0024_003C_003E8__locals15.isReadyToLearn = true;
					CS_0024_003C_003E8__locals15.pawn.rotationTracker.FaceTarget(CS_0024_003C_003E8__locals15.Desk);
					LearningUtility.LearningTickCheckEnd(CS_0024_003C_003E8__locals15.pawn, delta);
				};
				toil.handlingFacing = true;
				toil.defaultCompleteMode = ToilCompleteMode.Never;
				toil.socialMode = RandomSocialMode.Off;
				_003C_003E2__current = toil;
				_003C_003E1__state = 2;
				return true;
			}
			case 2:
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
			_003CMakeNewToils_003Ed__8 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__8(0)
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

	public const int TicksWaitForTeacher = 5000;

	public bool isReadyToLearn;

	public int waitingForTeacherTicks;

	private Thing Desk => base.TargetThingA;

	private Pawn Teacher => (Pawn)base.TargetThingB;

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		return pawn.ReserveSittableOrSpot(SchoolUtility.DeskSpotStudent(Desk), job, errorOnFailed);
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__8))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__8(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override string GetReport()
	{
		string text = ReportStringProcessed(job.def.reportString);
		if (Teacher != null)
		{
			text += " " + "ReportTakingLessonFrom".Translate(Teacher.Named("TEACHER"));
			if (Teacher.jobs?.curDriver is JobDriver_Lessongiving { taughtSkill: not null } jobDriver_Lessongiving)
			{
				text = text + " (" + jobDriver_Lessongiving.taughtSkill.label + ")";
			}
		}
		return text;
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref isReadyToLearn, "isReadyToLearn", defaultValue: false);
		Scribe_Values.Look(ref waitingForTeacherTicks, "waitForTeacherTicks", 0);
	}
}
