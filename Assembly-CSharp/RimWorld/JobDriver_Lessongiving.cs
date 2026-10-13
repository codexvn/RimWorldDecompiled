using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020011FE RID: 4606
	public class JobDriver_Lessongiving : JobDriver
	{
		// Token: 0x17001176 RID: 4470
		// (get) Token: 0x06007087 RID: 28807 RVA: 0x00229653 File Offset: 0x00227853
		private Thing Desk
		{
			get
			{
				return base.TargetThingA;
			}
		}

		// Token: 0x17001177 RID: 4471
		// (get) Token: 0x06007088 RID: 28808 RVA: 0x000024F4 File Offset: 0x000006F4
		private Pawn Student
		{
			get
			{
				return (Pawn)base.TargetThingB;
			}
		}

		// Token: 0x06007089 RID: 28809 RVA: 0x0022E8A3 File Offset: 0x0022CAA3
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return ModsConfig.BiotechActive && this.pawn.ReserveSittableOrSpot(SchoolUtility.DeskSpotTeacher(this.Desk), this.job, errorOnFailed);
		}

		// Token: 0x0600708A RID: 28810 RVA: 0x0022E8CE File Offset: 0x0022CACE
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOnDespawnedNullOrForbidden(TargetIndex.B);
			this.FailOn(delegate()
			{
				if (PawnUtility.WillSoonHaveBasicNeed(this.pawn, -0.05f))
				{
					return true;
				}
				Pawn student = this.Student;
				Job job;
				if (student == null)
				{
					job = null;
				}
				else
				{
					Pawn_JobTracker jobs = student.jobs;
					job = ((jobs != null) ? jobs.curJob : null);
				}
				Job job2 = job;
				return job2 == null || job2.def != JobDefOf.Lessontaking || job2.GetTarget(TargetIndex.B).Pawn != this.pawn || job2.GetTarget(TargetIndex.A).Thing != this.Desk;
			});
			yield return Toils_Goto.GotoCell(SchoolUtility.DeskSpotTeacher(this.Desk), PathEndMode.OnCell);
			Toil toil = ToilMaker.MakeToil("MakeNewToils");
			toil.tickIntervalAction = delegate(int delta)
			{
				this.pawn.GainComfortFromCellIfPossible(delta, false);
				this.isReadyToTeach = true;
				this.pawn.rotationTracker.FaceTarget(this.Desk);
				if (this.taughtSkill == null)
				{
					this.taughtSkill = this.ChooseSkill(this.Student);
				}
				if (this.taughtSkill == null)
				{
					base.EndJobWith(JobCondition.Incompletable);
					return;
				}
				if (this.isReadyToTeach)
				{
					Pawn student = this.Student;
					object obj;
					if (student == null)
					{
						obj = null;
					}
					else
					{
						Pawn_JobTracker jobs = student.jobs;
						obj = ((jobs != null) ? jobs.curDriver : null);
					}
					JobDriver_Lessontaking jobDriver_Lessontaking = obj as JobDriver_Lessontaking;
					if (jobDriver_Lessontaking != null && jobDriver_Lessontaking.isReadyToLearn)
					{
						if (this.pawn.IsHashIntervalTick(900, delta))
						{
							this.pawn.interactions.TryInteractWith(this.Student, this.taughtSkill.lessonInteraction);
							Pawn_NeedsTracker needs = this.pawn.needs;
							if (needs != null)
							{
								Need_Mood mood = needs.mood;
								if (mood != null)
								{
									ThoughtHandler thoughts = mood.thoughts;
									if (thoughts != null)
									{
										MemoryThoughtHandler memories = thoughts.memories;
										if (memories != null)
										{
											memories.TryGainMemory(ThoughtDefOf.GaveLesson, this.Student, null);
										}
									}
								}
							}
							Pawn_NeedsTracker needs2 = this.Student.needs;
							if (needs2 != null)
							{
								Need_Mood mood2 = needs2.mood;
								if (mood2 != null)
								{
									ThoughtHandler thoughts2 = mood2.thoughts;
									if (thoughts2 != null)
									{
										MemoryThoughtHandler memories2 = thoughts2.memories;
										if (memories2 != null)
										{
											memories2.TryGainMemory(ThoughtDefOf.WasTaught, this.pawn, null);
										}
									}
								}
							}
						}
						float num = LearningDesireDefOf.Lessontaking.xpPerTick * LearningUtility.LearningRateFactor(this.Student);
						this.Student.skills.Learn(this.taughtSkill, num * (float)delta, false, false);
						this.pawn.skills.Learn(SkillDefOf.Social, 0.1f * (float)delta, false, false);
					}
				}
			};
			toil.handlingFacing = true;
			toil.defaultCompleteMode = ToilCompleteMode.Never;
			toil.socialMode = RandomSocialMode.Off;
			toil.activeSkill = (() => SkillDefOf.Social);
			yield return toil;
			yield break;
		}

		// Token: 0x0600708B RID: 28811 RVA: 0x0022E8E0 File Offset: 0x0022CAE0
		private SkillDef ChooseSkill(Pawn student)
		{
			return (from s in this.pawn.skills.skills
			where !s.TotallyDisabled && s.def.lessonInteraction != null && !student.skills.GetSkill(s.def).TotallyDisabled
			orderby s descending
			select s).Take(4).RandomElement<SkillRecord>().def;
		}

		// Token: 0x0600708C RID: 28812 RVA: 0x0022E950 File Offset: 0x0022CB50
		public override string GetReport()
		{
			string str = (this.taughtSkill != null) ? (" (" + this.taughtSkill.label + ")") : "";
			return this.ReportStringProcessed(this.job.def.reportString) + str;
		}

		// Token: 0x0600708D RID: 28813 RVA: 0x0022E9A3 File Offset: 0x0022CBA3
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.isReadyToTeach, "isReadyToTeach", false, false);
			Scribe_Defs.Look<SkillDef>(ref this.taughtSkill, "taughtSkill");
		}

		// Token: 0x04004E2D RID: 20013
		private const int InteractionTicks = 900;

		// Token: 0x04004E2E RID: 20014
		private const int TopTeachableSkillCount = 4;

		// Token: 0x04004E2F RID: 20015
		public bool isReadyToTeach;

		// Token: 0x04004E30 RID: 20016
		public SkillDef taughtSkill;
	}
}
