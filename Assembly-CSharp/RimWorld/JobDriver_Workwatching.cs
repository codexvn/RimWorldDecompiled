using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200120E RID: 4622
	public class JobDriver_Workwatching : JobDriver
	{
		// Token: 0x17001185 RID: 4485
		// (get) Token: 0x060070E8 RID: 28904 RVA: 0x000024E7 File Offset: 0x000006E7
		private Pawn AdultToFollow
		{
			get
			{
				return (Pawn)base.TargetThingA;
			}
		}

		// Token: 0x060070E9 RID: 28905 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x060070EA RID: 28906 RVA: 0x0022F9CB File Offset: 0x0022DBCB
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			this.FailOnChildLearningConditions<JobDriver_Workwatching>();
			Toil toil = ToilMaker.MakeToil("MakeNewToils");
			toil.tickIntervalAction = delegate(int delta)
			{
				Pawn adultToFollow = this.AdultToFollow;
				if (!LearningGiver_Workwatching.ChildCanLearnFromAdultJob(this.pawn, adultToFollow))
				{
					this.consecutiveTicksNoSkillJob += delta;
					if (this.consecutiveTicksNoSkillJob >= 5)
					{
						Pawn pawn = LearningGiver_Workwatching.AdultToWorkwatch(this.pawn);
						if (pawn == null)
						{
							base.EndJobWith(JobCondition.Incompletable);
							return;
						}
						this.consecutiveTicksNoSkillJob = 0;
						this.job.SetTarget(TargetIndex.A, pawn);
						return;
					}
				}
				else
				{
					this.consecutiveTicksNoSkillJob = 0;
					if (this.pawn.Spawned && this.pawn.Map == adultToFollow.MapHeld && this.pawn.Position.InHorDistOf(adultToFollow.Position, 12f))
					{
						float num = LearningUtility.LearningRateFactor(this.pawn) * LearningDesireDefOf.Workwatching.xpPerTick * (float)delta;
						Job curJob = adultToFollow.CurJob;
						SkillDef skillDef;
						if (curJob == null)
						{
							skillDef = null;
						}
						else
						{
							RecipeDef recipeDef = curJob.RecipeDef;
							skillDef = ((recipeDef != null) ? recipeDef.workSkill : null);
						}
						SkillDef skillDef2 = skillDef;
						if (skillDef2 != null)
						{
							this.pawn.skills.Learn(skillDef2, num, false, false);
						}
						else
						{
							Job curJob2 = adultToFollow.CurJob;
							List<SkillDef> list;
							if (curJob2 == null)
							{
								list = null;
							}
							else
							{
								WorkGiverDef workGiverDef = curJob2.workGiverDef;
								if (workGiverDef == null)
								{
									list = null;
								}
								else
								{
									WorkTypeDef workType = workGiverDef.workType;
									list = ((workType != null) ? workType.relevantSkills : null);
								}
							}
							List<SkillDef> list2 = list;
							num /= (float)list2.Count;
							foreach (SkillDef sDef in list2)
							{
								this.pawn.skills.Learn(sDef, num, false, false);
							}
						}
					}
				}
				if (LearningUtility.LearningTickCheckEnd(this.pawn, delta, false))
				{
					return;
				}
				if ((adultToFollow.Position - this.pawn.Position).LengthHorizontal <= 3f && this.pawn.Position.WithinRegions(adultToFollow.Position, this.pawn.Map, 2, TraverseParms.For(this.pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), RegionType.Set_Passable))
				{
					return;
				}
				if (!this.pawn.CanReach(adultToFollow, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn) || adultToFollow.IsForbidden(this.pawn))
				{
					base.EndJobWith(JobCondition.Incompletable);
					return;
				}
				if (!this.pawn.pather.Moving || this.pawn.pather.Destination != adultToFollow)
				{
					this.pawn.pather.StartPath(adultToFollow, PathEndMode.Touch);
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Never;
			yield return toil;
			yield break;
		}

		// Token: 0x060070EB RID: 28907 RVA: 0x0022F9DB File Offset: 0x0022DBDB
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.consecutiveTicksNoSkillJob, "consecutiveTicksNoSkillJob", 0, false);
		}

		// Token: 0x04004E5C RID: 20060
		private const TargetIndex AdultInd = TargetIndex.A;

		// Token: 0x04004E5D RID: 20061
		private const int FollowDistance = 3;

		// Token: 0x04004E5E RID: 20062
		private const int StopWatchingNoSkillJobTicks = 5;

		// Token: 0x04004E5F RID: 20063
		private const float LearningObservationDistance = 12f;

		// Token: 0x04004E60 RID: 20064
		private int consecutiveTicksNoSkillJob;
	}
}
