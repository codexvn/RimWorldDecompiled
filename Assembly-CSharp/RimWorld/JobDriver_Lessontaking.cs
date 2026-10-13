using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001202 RID: 4610
	public class JobDriver_Lessontaking : JobDriver
	{
		// Token: 0x1700117A RID: 4474
		// (get) Token: 0x0600709F RID: 28831 RVA: 0x00229653 File Offset: 0x00227853
		private Thing Desk
		{
			get
			{
				return base.TargetThingA;
			}
		}

		// Token: 0x1700117B RID: 4475
		// (get) Token: 0x060070A0 RID: 28832 RVA: 0x000024F4 File Offset: 0x000006F4
		private Pawn Teacher
		{
			get
			{
				return (Pawn)base.TargetThingB;
			}
		}

		// Token: 0x060070A1 RID: 28833 RVA: 0x0022EDB3 File Offset: 0x0022CFB3
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.ReserveSittableOrSpot(SchoolUtility.DeskSpotStudent(this.Desk), this.job, errorOnFailed);
		}

		// Token: 0x060070A2 RID: 28834 RVA: 0x0022EDD2 File Offset: 0x0022CFD2
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOnChildLearningConditions<JobDriver_Lessontaking>();
			this.FailOn(delegate()
			{
				if (this.Teacher == null)
				{
					int num = this.waitingForTeacherTicks + 1;
					this.waitingForTeacherTicks = num;
					return num > 5000;
				}
				this.waitingForTeacherTicks = 0;
				Pawn_JobTracker jobs = this.Teacher.jobs;
				Job job = (jobs != null) ? jobs.curJob : null;
				return job == null || job.def != JobDefOf.Lessongiving || job.GetTarget(TargetIndex.B).Pawn != this.pawn || job.GetTarget(TargetIndex.A).Thing != this.Desk;
			});
			yield return Toils_Goto.GotoCell(SchoolUtility.DeskSpotStudent(this.Desk), PathEndMode.OnCell);
			Toil toil = ToilMaker.MakeToil("MakeNewToils");
			toil.tickIntervalAction = delegate(int delta)
			{
				this.pawn.GainComfortFromCellIfPossible(delta, false);
				this.isReadyToLearn = true;
				this.pawn.rotationTracker.FaceTarget(this.Desk);
				LearningUtility.LearningTickCheckEnd(this.pawn, delta, false);
			};
			toil.handlingFacing = true;
			toil.defaultCompleteMode = ToilCompleteMode.Never;
			toil.socialMode = RandomSocialMode.Off;
			yield return toil;
			yield break;
		}

		// Token: 0x060070A3 RID: 28835 RVA: 0x0022EDE4 File Offset: 0x0022CFE4
		public override string GetReport()
		{
			string text = this.ReportStringProcessed(this.job.def.reportString);
			if (this.Teacher != null)
			{
				text += " " + "ReportTakingLessonFrom".Translate(this.Teacher.Named("TEACHER"));
				Pawn_JobTracker jobs = this.Teacher.jobs;
				JobDriver_Lessongiving jobDriver_Lessongiving = ((jobs != null) ? jobs.curDriver : null) as JobDriver_Lessongiving;
				if (jobDriver_Lessongiving != null && jobDriver_Lessongiving.taughtSkill != null)
				{
					text = text + " (" + jobDriver_Lessongiving.taughtSkill.label + ")";
				}
			}
			return text;
		}

		// Token: 0x060070A4 RID: 28836 RVA: 0x0022EE85 File Offset: 0x0022D085
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.isReadyToLearn, "isReadyToLearn", false, false);
			Scribe_Values.Look<int>(ref this.waitingForTeacherTicks, "waitForTeacherTicks", 0, false);
		}

		// Token: 0x04004E39 RID: 20025
		public const int TicksWaitForTeacher = 5000;

		// Token: 0x04004E3A RID: 20026
		public bool isReadyToLearn;

		// Token: 0x04004E3B RID: 20027
		public int waitingForTeacherTicks;
	}
}
