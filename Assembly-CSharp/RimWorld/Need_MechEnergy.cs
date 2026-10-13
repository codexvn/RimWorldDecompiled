using System;
using System.Text;
using RimWorld.Planet;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02002487 RID: 9351
	public class Need_MechEnergy : Need
	{
		// Token: 0x17001ED5 RID: 7893
		// (get) Token: 0x0600CAD4 RID: 51924 RVA: 0x003B67CE File Offset: 0x003B49CE
		public override float MaxLevel
		{
			get
			{
				return (float)this.pawn.RaceProps.maxMechEnergy;
			}
		}

		// Token: 0x17001ED6 RID: 7894
		// (get) Token: 0x0600CAD5 RID: 51925 RVA: 0x003B67E1 File Offset: 0x003B49E1
		public bool IsSelfShutdown
		{
			get
			{
				return this.pawn.CurJobDef == JobDefOf.SelfShutdown && this.pawn.GetPosture().Laying();
			}
		}

		// Token: 0x17001ED7 RID: 7895
		// (get) Token: 0x0600CAD6 RID: 51926 RVA: 0x003B6807 File Offset: 0x003B4A07
		public bool IsLowEnergySelfShutdown
		{
			get
			{
				return this.IsSelfShutdown && this.selfShutdown;
			}
		}

		// Token: 0x17001ED8 RID: 7896
		// (get) Token: 0x0600CAD7 RID: 51927 RVA: 0x003B6819 File Offset: 0x003B4A19
		public override int GUIChangeArrow
		{
			get
			{
				if (this.FallPerDay > 0f)
				{
					return -1;
				}
				if (this.IsSelfShutdown || this.pawn.IsCharging())
				{
					return 1;
				}
				return 0;
			}
		}

		// Token: 0x17001ED9 RID: 7897
		// (get) Token: 0x0600CAD8 RID: 51928 RVA: 0x003B6842 File Offset: 0x003B4A42
		private float BaseFallPerDay
		{
			get
			{
				if (this.pawn.mindState != null && !this.pawn.mindState.IsIdle && !this.pawn.IsGestating())
				{
					return 10f;
				}
				return 3f;
			}
		}

		// Token: 0x17001EDA RID: 7898
		// (get) Token: 0x0600CAD9 RID: 51929 RVA: 0x003B687C File Offset: 0x003B4A7C
		public float FallPerDay
		{
			get
			{
				if (this.pawn.Downed)
				{
					return 0f;
				}
				if (!this.pawn.Awake())
				{
					return 0f;
				}
				if (this.IsSelfShutdown)
				{
					return 0f;
				}
				if (this.currentCharger != null)
				{
					return 0f;
				}
				if (this.pawn.IsCaravanMember())
				{
					return 0f;
				}
				return this.BaseFallPerDay * this.pawn.GetStatValue(StatDefOf.MechEnergyUsageFactor, true, -1);
			}
		}

		// Token: 0x0600CADA RID: 51930 RVA: 0x003B5049 File Offset: 0x003B3249
		public Need_MechEnergy(Pawn pawn) : base(pawn)
		{
		}

		// Token: 0x0600CADB RID: 51931 RVA: 0x003B68F8 File Offset: 0x003B4AF8
		public override void NeedInterval()
		{
			float num = 400f;
			if (!this.IsSelfShutdown)
			{
				this.CurLevel -= this.FallPerDay / num;
			}
			else
			{
				this.CurLevel += 1f / num;
			}
			if (this.CurLevel <= 0f)
			{
				this.selfShutdown = true;
			}
			else if (this.CurLevel >= 15f || this.pawn.CurJobDef == JobDefOf.MechCharge)
			{
				this.selfShutdown = false;
			}
			Hediff firstHediffOfDef = this.pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.SelfShutdown, false);
			if (firstHediffOfDef != null && !this.selfShutdown)
			{
				this.pawn.health.RemoveHediff(firstHediffOfDef);
				Pawn_JobTracker jobs = this.pawn.jobs;
				if (jobs != null)
				{
					jobs.CheckForJobOverride(0f, true);
				}
			}
			else if (firstHediffOfDef == null && this.selfShutdown)
			{
				this.pawn.health.AddHediff(HediffDefOf.SelfShutdown, null, null, null);
			}
			if (this.selfShutdown && this.pawn.Spawned && !this.pawn.Downed && this.pawn.CurJobDef != JobDefOf.SelfShutdown && this.pawn.jobs != null)
			{
				IntVec3 position = this.pawn.Position;
				RCellFinder.TryFindNearbyMechSelfShutdownSpot(this.pawn.Position, this.pawn, this.pawn.Map, out position, true);
				if (this.pawn.Drafted)
				{
					this.pawn.drafter.Drafted = false;
				}
				this.pawn.jobs.StartJob(JobMaker.MakeJob(JobDefOf.SelfShutdown, position), JobCondition.InterruptForced, null, false, true, null, new JobTag?(JobTag.SatisfyingNeeds), false, false, null, false, true, false);
			}
		}

		// Token: 0x0600CADC RID: 51932 RVA: 0x003B6AD6 File Offset: 0x003B4CD6
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.selfShutdown, "selfShutdown", false, false);
			Scribe_References.Look<Building_MechCharger>(ref this.currentCharger, "currentCharger", false);
		}

		// Token: 0x0600CADD RID: 51933 RVA: 0x003B6B04 File Offset: 0x003B4D04
		public override string GetTipString()
		{
			StringBuilder stringBuilder = new StringBuilder(base.GetTipString());
			stringBuilder.AppendInNewLine("CurrentMechEnergyFallPerDay".Translate() + ": " + (this.FallPerDay / 100f).ToStringPercent());
			return stringBuilder.ToString();
		}

		// Token: 0x04008A40 RID: 35392
		private bool selfShutdown;

		// Token: 0x04008A41 RID: 35393
		public Building_MechCharger currentCharger;

		// Token: 0x04008A42 RID: 35394
		public const float BaseFallPerDayActive = 10f;

		// Token: 0x04008A43 RID: 35395
		public const float BaseFallPerDayIdle = 3f;

		// Token: 0x04008A44 RID: 35396
		public const float BaseGainPerDaySelfShutdown = 1f;

		// Token: 0x04008A45 RID: 35397
		public const float ShutdownUntil = 15f;
	}
}
