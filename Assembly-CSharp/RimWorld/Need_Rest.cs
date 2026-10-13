using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200248F RID: 9359
	public class Need_Rest : Need
	{
		// Token: 0x17001EE3 RID: 7907
		// (get) Token: 0x0600CAF2 RID: 51954 RVA: 0x003B7352 File Offset: 0x003B5552
		public RestCategory CurCategory
		{
			get
			{
				if (this.CurLevel < 0.01f)
				{
					return RestCategory.Exhausted;
				}
				if (this.CurLevel < 0.14f)
				{
					return RestCategory.VeryTired;
				}
				if (this.CurLevel < 0.28f)
				{
					return RestCategory.Tired;
				}
				return RestCategory.Rested;
			}
		}

		// Token: 0x17001EE4 RID: 7908
		// (get) Token: 0x0600CAF3 RID: 51955 RVA: 0x003B7384 File Offset: 0x003B5584
		public float RestFallPerTick
		{
			get
			{
				switch (this.CurCategory)
				{
				case RestCategory.Rested:
					return 1.5833333E-05f * this.RestFallFactor;
				case RestCategory.Tired:
					return 1.5833333E-05f * this.RestFallFactor * 0.7f;
				case RestCategory.VeryTired:
					return 1.5833333E-05f * this.RestFallFactor * 0.3f;
				case RestCategory.Exhausted:
					return 1.5833333E-05f * this.RestFallFactor * 0.6f;
				default:
					return 999f;
				}
			}
		}

		// Token: 0x17001EE5 RID: 7909
		// (get) Token: 0x0600CAF4 RID: 51956 RVA: 0x003B73FB File Offset: 0x003B55FB
		private float RestFallFactor
		{
			get
			{
				return this.pawn.health.hediffSet.RestFallFactor;
			}
		}

		// Token: 0x17001EE6 RID: 7910
		// (get) Token: 0x0600CAF5 RID: 51957 RVA: 0x003B7412 File Offset: 0x003B5612
		public override int GUIChangeArrow
		{
			get
			{
				if (!this.Resting)
				{
					return -1;
				}
				return 1;
			}
		}

		// Token: 0x17001EE7 RID: 7911
		// (get) Token: 0x0600CAF6 RID: 51958 RVA: 0x003B741F File Offset: 0x003B561F
		public int TicksAtZero
		{
			get
			{
				return this.ticksAtZero;
			}
		}

		// Token: 0x17001EE8 RID: 7912
		// (get) Token: 0x0600CAF7 RID: 51959 RVA: 0x003B7427 File Offset: 0x003B5627
		public bool Resting
		{
			get
			{
				return Find.TickManager.TicksGame < this.lastRestTick + this.pawn.UpdateRateTicks;
			}
		}

		// Token: 0x0600CAF8 RID: 51960 RVA: 0x003B7448 File Offset: 0x003B5648
		public Need_Rest(Pawn pawn) : base(pawn)
		{
			this.threshPercents = new List<float>();
			this.threshPercents.Add(0.28f);
			this.threshPercents.Add(0.14f);
		}

		// Token: 0x0600CAF9 RID: 51961 RVA: 0x003B749D File Offset: 0x003B569D
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksAtZero, "ticksAtZero", 0, false);
		}

		// Token: 0x0600CAFA RID: 51962 RVA: 0x003B74B7 File Offset: 0x003B56B7
		public override void SetInitialLevel()
		{
			this.CurLevel = Rand.Range(0.9f, 1f);
		}

		// Token: 0x0600CAFB RID: 51963 RVA: 0x003B74D0 File Offset: 0x003B56D0
		public override void NeedInterval()
		{
			if (!this.IsFrozen)
			{
				if (this.Resting)
				{
					float num = this.lastRestEffectiveness;
					num *= this.pawn.GetStatValue(StatDefOf.RestRateMultiplier, true, -1);
					if (num > 0f)
					{
						this.CurLevel += 0.005714286f * num;
					}
				}
				else
				{
					float num2 = this.RestFallPerTick * 150f * this.pawn.GetStatValue(StatDefOf.RestFallRateFactor, true, -1);
					this.CurLevel -= num2;
				}
			}
			if (this.CurLevel < 0.0001f)
			{
				this.ticksAtZero += 150;
			}
			else
			{
				this.ticksAtZero = 0;
			}
			if (Need_Rest.CanInvoluntarilySleep(this.pawn) && this.ShouldInvoluntarySleepFromMTB())
			{
				Building_Bed building_Bed = this.pawn.CurrentBed();
				LocalTargetInfo targetA;
				if (building_Bed != null)
				{
					targetA = building_Bed;
				}
				else
				{
					Thing spawnedParentOrMe = this.pawn.SpawnedParentOrMe;
					if (spawnedParentOrMe != null && spawnedParentOrMe != this.pawn)
					{
						targetA = spawnedParentOrMe;
					}
					else
					{
						targetA = this.pawn.Position;
					}
				}
				Job job = JobMaker.MakeJob(JobDefOf.LayDown, targetA);
				job.startInvoluntarySleep = true;
				this.pawn.jobs.StartJob(job, JobCondition.InterruptForced, null, false, true, null, new JobTag?(JobTag.SatisfyingNeeds), false, false, null, false, true, true);
				if (this.pawn.InMentalState && this.pawn.MentalStateDef.recoverFromCollapsingExhausted)
				{
					this.pawn.mindState.mentalStateHandler.CurState.RecoverFromState();
				}
				LifeStageDef curLifeStage = this.pawn.ageTracker.CurLifeStage;
				if (curLifeStage == null || curLifeStage.involuntarySleepIsNegativeEvent)
				{
					if (PawnUtility.ShouldSendNotificationAbout(this.pawn))
					{
						Messages.Message("MessageInvoluntarySleep".Translate(this.pawn.LabelShort, this.pawn), this.pawn, MessageTypeDefOf.NegativeEvent, true);
					}
					TaleRecorder.RecordTale(TaleDefOf.Exhausted, new object[]
					{
						this.pawn
					});
				}
			}
		}

		// Token: 0x0600CAFC RID: 51964 RVA: 0x003B76E6 File Offset: 0x003B58E6
		public void TickResting(float restEffectiveness)
		{
			if (restEffectiveness <= 0f)
			{
				return;
			}
			this.lastRestTick = Find.TickManager.TicksGame;
			this.lastRestEffectiveness = restEffectiveness;
		}

		// Token: 0x0600CAFD RID: 51965 RVA: 0x003B7708 File Offset: 0x003B5908
		private bool ShouldInvoluntarySleepFromMTB()
		{
			float num = float.PositiveInfinity;
			if (this.ticksAtZero > 1000)
			{
				if (this.ticksAtZero < 15000)
				{
					num = 0.25f;
				}
				else if (this.ticksAtZero < 30000)
				{
					num = 0.125f;
				}
				else if (this.ticksAtZero < 45000)
				{
					num = 0.083333336f;
				}
				else
				{
					num = 0.0625f;
				}
			}
			LifeStageDef curLifeStage = this.pawn.ageTracker.CurLifeStage;
			SimpleCurve simpleCurve = (curLifeStage != null) ? curLifeStage.involuntarySleepMTBDaysFromRest : null;
			if (simpleCurve != null)
			{
				num = Rand.CombineMTBs(num, simpleCurve.Evaluate(base.CurLevelPercentage));
			}
			return Rand.MTBEventOccurs(num, 60000f, 150f);
		}

		// Token: 0x0600CAFE RID: 51966 RVA: 0x003B77B0 File Offset: 0x003B59B0
		private static bool CanInvoluntarilySleep(Pawn pawn)
		{
			Pawn_JobTracker jobs = pawn.jobs;
			bool flag;
			if (jobs == null)
			{
				flag = false;
			}
			else
			{
				JobDriver curDriver = jobs.curDriver;
				bool? flag2 = (curDriver != null) ? new bool?(curDriver.asleep) : null;
				bool flag3 = true;
				flag = (flag2.GetValueOrDefault() == flag3 & flag2 != null);
			}
			if (flag)
			{
				return false;
			}
			if (!RestUtility.CanFallAsleep(pawn))
			{
				return false;
			}
			if (!pawn.Spawned)
			{
				if (!pawn.ageTracker.CurLifeStage.canSleepWhileHeld)
				{
					return false;
				}
				if (!(pawn.SpawnedParentOrMe is Pawn))
				{
					return false;
				}
				if (pawn.IsWorldPawn())
				{
					return false;
				}
				if (pawn.IsCaravanMember())
				{
					return false;
				}
			}
			return !pawn.ageTracker.CurLifeStage.canVoluntarilySleep || pawn.CurJobDef != JobDefOf.LayDown;
		}

		// Token: 0x04008A6D RID: 35437
		private int lastRestTick = -999;

		// Token: 0x04008A6E RID: 35438
		private float lastRestEffectiveness = 1f;

		// Token: 0x04008A6F RID: 35439
		private int ticksAtZero;

		// Token: 0x04008A70 RID: 35440
		private const float FullSleepHours = 10.5f;

		// Token: 0x04008A71 RID: 35441
		public const float BaseRestGainPerTick = 3.809524E-05f;

		// Token: 0x04008A72 RID: 35442
		private const float BaseRestFallPerTick = 1.5833333E-05f;

		// Token: 0x04008A73 RID: 35443
		public const float ThreshTired = 0.28f;

		// Token: 0x04008A74 RID: 35444
		public const float ThreshVeryTired = 0.14f;

		// Token: 0x04008A75 RID: 35445
		public const float DefaultFallAsleepMaxLevel = 0.75f;

		// Token: 0x04008A76 RID: 35446
		public const float DefaultNaturalWakeThreshold = 1f;

		// Token: 0x04008A77 RID: 35447
		public const float CanWakeThreshold = 0.2f;

		// Token: 0x04008A78 RID: 35448
		private const float BaseInvoluntarySleepMTBDays = 0.25f;
	}
}
