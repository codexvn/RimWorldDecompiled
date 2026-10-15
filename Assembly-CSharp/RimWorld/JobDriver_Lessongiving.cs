using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_Lessongiving : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__9 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_Lessongiving _003C_003E4__this;

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
		public _003CMakeNewToils_003Ed__9(int _003C_003E1__state)
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
			JobDriver_Lessongiving CS_0024_003C_003E8__locals32 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				CS_0024_003C_003E8__locals32.FailOnDespawnedNullOrForbidden(TargetIndex.A);
				CS_0024_003C_003E8__locals32.FailOnDespawnedNullOrForbidden(TargetIndex.B);
				CS_0024_003C_003E8__locals32.FailOn(delegate
				{
					if (PawnUtility.WillSoonHaveBasicNeed(CS_0024_003C_003E8__locals32.pawn, -0.05f))
					{
						return true;
					}
					Job job = CS_0024_003C_003E8__locals32.Student?.jobs?.curJob;
					return (job == null || job.def != JobDefOf.Lessontaking || job.GetTarget(TargetIndex.B).Pawn != CS_0024_003C_003E8__locals32.pawn || job.GetTarget(TargetIndex.A).Thing != CS_0024_003C_003E8__locals32.Desk) ? true : false;
				});
				_003C_003E2__current = Toils_Goto.GotoCell(SchoolUtility.DeskSpotTeacher(CS_0024_003C_003E8__locals32.Desk), PathEndMode.OnCell);
				_003C_003E1__state = 1;
				return true;
			case 1:
			{
				_003C_003E1__state = -1;
				Toil toil = ToilMaker.MakeToil("MakeNewToils");
				toil.tickIntervalAction = delegate(int delta)
				{
					CS_0024_003C_003E8__locals32.pawn.GainComfortFromCellIfPossible(delta);
					CS_0024_003C_003E8__locals32.isReadyToTeach = true;
					CS_0024_003C_003E8__locals32.pawn.rotationTracker.FaceTarget(CS_0024_003C_003E8__locals32.Desk);
					if (CS_0024_003C_003E8__locals32.taughtSkill == null)
					{
						CS_0024_003C_003E8__locals32.taughtSkill = CS_0024_003C_003E8__locals32.ChooseSkill(CS_0024_003C_003E8__locals32.Student);
					}
					if (CS_0024_003C_003E8__locals32.taughtSkill == null)
					{
						CS_0024_003C_003E8__locals32.EndJobWith(JobCondition.Incompletable);
					}
					else if (CS_0024_003C_003E8__locals32.isReadyToTeach && CS_0024_003C_003E8__locals32.Student?.jobs?.curDriver is JobDriver_Lessontaking { isReadyToLearn: not false })
					{
						if (CS_0024_003C_003E8__locals32.pawn.IsHashIntervalTick(900, delta))
						{
							CS_0024_003C_003E8__locals32.pawn.interactions.TryInteractWith(CS_0024_003C_003E8__locals32.Student, CS_0024_003C_003E8__locals32.taughtSkill.lessonInteraction);
							CS_0024_003C_003E8__locals32.pawn.needs?.mood?.thoughts?.memories?.TryGainMemory(ThoughtDefOf.GaveLesson, CS_0024_003C_003E8__locals32.Student);
							CS_0024_003C_003E8__locals32.Student.needs?.mood?.thoughts?.memories?.TryGainMemory(ThoughtDefOf.WasTaught, CS_0024_003C_003E8__locals32.pawn);
						}
						float num2 = LearningDesireDefOf.Lessontaking.xpPerTick * LearningUtility.LearningRateFactor(CS_0024_003C_003E8__locals32.Student);
						CS_0024_003C_003E8__locals32.Student.skills.Learn(CS_0024_003C_003E8__locals32.taughtSkill, num2 * (float)delta);
						CS_0024_003C_003E8__locals32.pawn.skills.Learn(SkillDefOf.Social, 0.1f * (float)delta);
					}
				};
				toil.handlingFacing = true;
				toil.defaultCompleteMode = ToilCompleteMode.Never;
				toil.socialMode = RandomSocialMode.Off;
				toil.activeSkill = () => SkillDefOf.Social;
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
			_003CMakeNewToils_003Ed__9 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__9(0)
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

	private const int InteractionTicks = 900;

	private const int TopTeachableSkillCount = 4;

	public bool isReadyToTeach;

	public SkillDef taughtSkill;

	private Thing Desk => base.TargetThingA;

	private Pawn Student => (Pawn)base.TargetThingB;

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		if (!ModsConfig.BiotechActive || !pawn.ReserveSittableOrSpot(SchoolUtility.DeskSpotTeacher(Desk), job, errorOnFailed))
		{
			return false;
		}
		return true;
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__9))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__9(-2)
		{
			_003C_003E4__this = this
		};
	}

	private SkillDef ChooseSkill(Pawn student)
	{
		return (from s in pawn.skills.skills
			where !s.TotallyDisabled && s.def.lessonInteraction != null && !student.skills.GetSkill(s.def).TotallyDisabled
			orderby s descending
			select s).Take(4).RandomElement().def;
	}

	public override string GetReport()
	{
		string text = ((taughtSkill != null) ? (" (" + taughtSkill.label + ")") : "");
		return ReportStringProcessed(job.def.reportString) + text;
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref isReadyToTeach, "isReadyToTeach", defaultValue: false);
		Scribe_Defs.Look(ref taughtSkill, "taughtSkill");
	}
}
