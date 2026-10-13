using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020007CA RID: 1994
	public class Pawn_AgeTracker : IExposable
	{
		// Token: 0x17000975 RID: 2421
		// (get) Token: 0x06003261 RID: 12897 RVA: 0x001078BB File Offset: 0x00105ABB
		// (set) Token: 0x06003262 RID: 12898 RVA: 0x001078C3 File Offset: 0x00105AC3
		public long BirthAbsTicks
		{
			get
			{
				return this.birthAbsTicksInt;
			}
			set
			{
				this.birthAbsTicksInt = value;
			}
		}

		// Token: 0x17000976 RID: 2422
		// (get) Token: 0x06003263 RID: 12899 RVA: 0x001078CC File Offset: 0x00105ACC
		public int AgeBiologicalYears
		{
			get
			{
				return (int)(this.ageBiologicalTicksInt / 3600000L);
			}
		}

		// Token: 0x17000977 RID: 2423
		// (get) Token: 0x06003264 RID: 12900 RVA: 0x001078DC File Offset: 0x00105ADC
		public float AgeBiologicalYearsFloat
		{
			get
			{
				return (float)this.ageBiologicalTicksInt / 3600000f;
			}
		}

		// Token: 0x17000978 RID: 2424
		// (get) Token: 0x06003265 RID: 12901 RVA: 0x001078EB File Offset: 0x00105AEB
		// (set) Token: 0x06003266 RID: 12902 RVA: 0x001078F3 File Offset: 0x00105AF3
		public long AgeBiologicalTicks
		{
			get
			{
				return this.ageBiologicalTicksInt;
			}
			set
			{
				this.ageBiologicalTicksInt = value;
				this.CalculateInitialGrowth();
				this.RecalculateLifeStageIndex();
			}
		}

		// Token: 0x17000979 RID: 2425
		// (get) Token: 0x06003267 RID: 12903 RVA: 0x00107908 File Offset: 0x00105B08
		// (set) Token: 0x06003268 RID: 12904 RVA: 0x00107917 File Offset: 0x00105B17
		public long AgeChronologicalTicks
		{
			get
			{
				return (long)GenTicks.TicksAbs - this.birthAbsTicksInt;
			}
			set
			{
				this.BirthAbsTicks = (long)GenTicks.TicksAbs - value;
			}
		}

		// Token: 0x1700097A RID: 2426
		// (get) Token: 0x06003269 RID: 12905 RVA: 0x00107927 File Offset: 0x00105B27
		public int AgeChronologicalYears
		{
			get
			{
				return (int)(this.AgeChronologicalTicks / 3600000L);
			}
		}

		// Token: 0x1700097B RID: 2427
		// (get) Token: 0x0600326A RID: 12906 RVA: 0x00107937 File Offset: 0x00105B37
		public float AgeChronologicalYearsFloat
		{
			get
			{
				return (float)this.AgeChronologicalTicks / 3600000f;
			}
		}

		// Token: 0x1700097C RID: 2428
		// (get) Token: 0x0600326B RID: 12907 RVA: 0x00107946 File Offset: 0x00105B46
		public int BirthYear
		{
			get
			{
				return GenDate.Year(this.birthAbsTicksInt, 0f);
			}
		}

		// Token: 0x1700097D RID: 2429
		// (get) Token: 0x0600326C RID: 12908 RVA: 0x00107958 File Offset: 0x00105B58
		public int BirthDayOfSeasonZeroBased
		{
			get
			{
				return GenDate.DayOfSeason(this.birthAbsTicksInt, 0f);
			}
		}

		// Token: 0x1700097E RID: 2430
		// (get) Token: 0x0600326D RID: 12909 RVA: 0x0010796A File Offset: 0x00105B6A
		public int BirthDayOfYear
		{
			get
			{
				return GenDate.DayOfYear(this.birthAbsTicksInt, 0f);
			}
		}

		// Token: 0x1700097F RID: 2431
		// (get) Token: 0x0600326E RID: 12910 RVA: 0x0010797C File Offset: 0x00105B7C
		public Quadrum BirthQuadrum
		{
			get
			{
				return GenDate.Quadrum(this.birthAbsTicksInt, 0f);
			}
		}

		// Token: 0x17000980 RID: 2432
		// (get) Token: 0x0600326F RID: 12911 RVA: 0x00107990 File Offset: 0x00105B90
		public string AgeNumberString
		{
			get
			{
				string text = this.AgeBiologicalYearsFloat.ToStringApproxAge();
				if (this.AgeChronologicalYears != this.AgeBiologicalYears)
				{
					text = text + " (" + this.AgeChronologicalYears.ToString() + ")";
				}
				return text;
			}
		}

		// Token: 0x17000981 RID: 2433
		// (get) Token: 0x06003270 RID: 12912 RVA: 0x001079D8 File Offset: 0x00105BD8
		public string AgeTooltipString
		{
			get
			{
				int value;
				int value2;
				int value3;
				float biologicalTicksPerTick;
				this.ageBiologicalTicksInt.TicksToPeriod(out value, out value2, out value3, out biologicalTicksPerTick);
				int value4;
				int value5;
				int value6;
				((long)GenTicks.TicksAbs - this.birthAbsTicksInt).TicksToPeriod(out value4, out value5, out value6, out biologicalTicksPerTick);
				string value7 = "FullDate".Translate(Find.ActiveLanguageWorker.OrdinalNumber(this.BirthDayOfSeasonZeroBased + 1, Gender.None), this.BirthQuadrum.Label(), this.BirthYear);
				string text = "Born".Translate(value7) + "\n" + "AgeChronological".Translate(value4, value5, value6) + "\n" + "AgeBiological".Translate(value, value2, value3);
				if (Prefs.DevMode)
				{
					text += "\n\nDev mode info:";
					text = text + "\nageBiologicalTicksInt: " + this.ageBiologicalTicksInt.ToString();
					text = text + "\nbirthAbsTicksInt: " + this.birthAbsTicksInt.ToString();
					string str = text;
					string str2 = "\nBiologicalTicksPerTick: ";
					biologicalTicksPerTick = this.BiologicalTicksPerTick;
					text = str + str2 + biologicalTicksPerTick.ToString();
					text = text + "\ngrowth: " + this.growth.ToString();
					text = string.Concat(new string[]
					{
						text,
						"\nage reversal demand deadline: ",
						((int)Math.Abs(this.AgeReversalDemandedDeadlineTicks)).ToStringTicksToPeriod(true, false, true, true, false),
						(this.AgeReversalDemandedDeadlineTicks < 0L) ? " past deadline" : " in future",
						"(",
						this.AgeReversalDemandedDeadlineTicks.ToString(),
						")"
					});
					string str3 = text;
					string str4 = "\nlife stage: ";
					LifeStageDef curLifeStage = this.CurLifeStage;
					text = str3 + str4 + ((curLifeStage != null) ? curLifeStage.ToString() : null);
					text = text + "\nsterile: " + this.pawn.Sterile().ToString();
				}
				return text;
			}
		}

		// Token: 0x17000982 RID: 2434
		// (get) Token: 0x06003271 RID: 12913 RVA: 0x00107BF8 File Offset: 0x00105DF8
		public int CurLifeStageIndex
		{
			get
			{
				if (this.cachedLifeStageIndex < 0)
				{
					this.RecalculateLifeStageIndex();
				}
				return this.cachedLifeStageIndex;
			}
		}

		// Token: 0x17000983 RID: 2435
		// (get) Token: 0x06003272 RID: 12914 RVA: 0x00107C0F File Offset: 0x00105E0F
		public LifeStageDef CurLifeStage
		{
			get
			{
				return this.CurLifeStageRace.def;
			}
		}

		// Token: 0x17000984 RID: 2436
		// (get) Token: 0x06003273 RID: 12915 RVA: 0x00107C1C File Offset: 0x00105E1C
		public LifeStageAge CurLifeStageRace
		{
			get
			{
				return this.GetLifeStageAge(this.CurLifeStageIndex);
			}
		}

		// Token: 0x17000985 RID: 2437
		// (get) Token: 0x06003274 RID: 12916 RVA: 0x00107C2C File Offset: 0x00105E2C
		public PawnKindLifeStage CurKindLifeStage
		{
			get
			{
				if (this.pawn.RaceProps.Humanlike)
				{
					string str = "Tried to get CurKindLifeStage from humanlike pawn ";
					Pawn pawn = this.pawn;
					Log.ErrorOnce(str + ((pawn != null) ? pawn.ToString() : null), 8888811);
					return null;
				}
				return this.pawn.kindDef.lifeStages[this.CurLifeStageIndex];
			}
		}

		// Token: 0x17000986 RID: 2438
		// (get) Token: 0x06003275 RID: 12917 RVA: 0x00107C8E File Offset: 0x00105E8E
		public int MaxRaceLifeStageIndex
		{
			get
			{
				return this.pawn.RaceProps.lifeStageAges.Count - 1;
			}
		}

		// Token: 0x17000987 RID: 2439
		// (get) Token: 0x06003276 RID: 12918 RVA: 0x00107CA7 File Offset: 0x00105EA7
		public float Growth
		{
			get
			{
				return this.growth;
			}
		}

		// Token: 0x17000988 RID: 2440
		// (get) Token: 0x06003277 RID: 12919 RVA: 0x00107CB0 File Offset: 0x00105EB0
		public float AdultMinAge
		{
			get
			{
				if (this.pawn.RaceProps.Humanlike)
				{
					LifeStageAge lifeStageAge2 = this.pawn.RaceProps.lifeStageAges.FirstOrDefault((LifeStageAge lifeStageAge) => lifeStageAge.def.developmentalStage.Adult());
					if (lifeStageAge2 == null)
					{
						return 0f;
					}
					return lifeStageAge2.minAge;
				}
				else
				{
					if (this.pawn.RaceProps.lifeStageAges.Count <= 0)
					{
						return 0f;
					}
					return this.pawn.RaceProps.lifeStageAges.Last<LifeStageAge>().minAge;
				}
			}
		}

		// Token: 0x17000989 RID: 2441
		// (get) Token: 0x06003278 RID: 12920 RVA: 0x00107D4B File Offset: 0x00105F4B
		public long AdultMinAgeTicks
		{
			get
			{
				return (long)Mathf.FloorToInt(this.AdultMinAge * 3600000f);
			}
		}

		// Token: 0x1700098A RID: 2442
		// (get) Token: 0x06003279 RID: 12921 RVA: 0x00107D5F File Offset: 0x00105F5F
		private long TicksToAdulthood
		{
			get
			{
				return this.AdultMinAgeTicks - this.AgeBiologicalTicks;
			}
		}

		// Token: 0x1700098B RID: 2443
		// (get) Token: 0x0600327A RID: 12922 RVA: 0x00107D6E File Offset: 0x00105F6E
		public bool Adult
		{
			get
			{
				return this.TicksToAdulthood <= 0L;
			}
		}

		// Token: 0x1700098C RID: 2444
		// (get) Token: 0x0600327B RID: 12923 RVA: 0x00107D7D File Offset: 0x00105F7D
		public long AgeReversalDemandedDeadlineTicks
		{
			get
			{
				return this.ageReversalDemandedAtAgeTicks - this.AgeBiologicalTicks;
			}
		}

		// Token: 0x1700098D RID: 2445
		// (get) Token: 0x0600327C RID: 12924 RVA: 0x00107D8C File Offset: 0x00105F8C
		public Pawn_AgeTracker.AgeReversalReason LastAgeReversalReason
		{
			get
			{
				return this.lastAgeReversalReason;
			}
		}

		// Token: 0x1700098E RID: 2446
		// (get) Token: 0x0600327D RID: 12925 RVA: 0x00107D94 File Offset: 0x00105F94
		public float BiologicalTicksPerTick
		{
			get
			{
				if (!this.pawn.RaceProps.Humanlike)
				{
					return 1f;
				}
				if (ModsConfig.BiotechActive && this.pawn.ParentHolder != null && this.pawn.ParentHolder is Building_GrowthVat)
				{
					return 1f;
				}
				float num = 1f;
				float ageBiologicalYearsFloat = this.AgeBiologicalYearsFloat;
				if (ageBiologicalYearsFloat <= 11f)
				{
					num *= this.ChildAgingMultiplier;
				}
				else if (ageBiologicalYearsFloat >= 20f)
				{
					num *= this.AdultAgingMultiplier;
				}
				else
				{
					float t = Mathf.InverseLerp(11f, 20f, ageBiologicalYearsFloat);
					num *= Mathf.Lerp(this.ChildAgingMultiplier, this.AdultAgingMultiplier, t);
				}
				if (this.pawn.genes != null)
				{
					num *= this.pawn.genes.BiologicalAgeTickFactor;
				}
				return num;
			}
		}

		// Token: 0x1700098F RID: 2447
		// (get) Token: 0x0600327E RID: 12926 RVA: 0x00107E5F File Offset: 0x0010605F
		public float AdultAgingMultiplier
		{
			get
			{
				return Find.Storyteller.difficulty.adultAgingRate;
			}
		}

		// Token: 0x17000990 RID: 2448
		// (get) Token: 0x0600327F RID: 12927 RVA: 0x00107E70 File Offset: 0x00106070
		public float ChildAgingMultiplier
		{
			get
			{
				return Find.Storyteller.difficulty.childAgingRate;
			}
		}

		// Token: 0x17000991 RID: 2449
		// (get) Token: 0x06003280 RID: 12928 RVA: 0x00107E81 File Offset: 0x00106081
		public bool AtMaxGrowthTier
		{
			get
			{
				return this.GrowthTier >= GrowthUtility.GrowthTiers.Length - 1;
			}
		}

		// Token: 0x17000992 RID: 2450
		// (get) Token: 0x06003281 RID: 12929 RVA: 0x00107E98 File Offset: 0x00106098
		public float PercentToNextGrowthTier
		{
			get
			{
				if (this.growthPoints <= 0f)
				{
					return 0f;
				}
				if (this.AtMaxGrowthTier)
				{
					return 1f;
				}
				int growthTier = this.GrowthTier;
				return Mathf.InverseLerp(GrowthUtility.GrowthTiers[growthTier].pointsRequirement, GrowthUtility.GrowthTiers[growthTier + 1].pointsRequirement, this.growthPoints);
			}
		}

		// Token: 0x17000993 RID: 2451
		// (get) Token: 0x06003282 RID: 12930 RVA: 0x00107EFC File Offset: 0x001060FC
		public int GrowthTier
		{
			get
			{
				for (int i = GrowthUtility.GrowthTiers.Length - 1; i >= 0; i--)
				{
					if (this.growthPoints >= GrowthUtility.GrowthTiers[i].pointsRequirement)
					{
						return i;
					}
				}
				return 0;
			}
		}

		// Token: 0x17000994 RID: 2452
		// (get) Token: 0x06003283 RID: 12931 RVA: 0x00107F38 File Offset: 0x00106138
		public float GrowthPointsPerDay
		{
			get
			{
				if (!ModsConfig.BiotechActive)
				{
					return 0f;
				}
				float result;
				if (this.pawn.ParentHolder is Building_GrowthVat)
				{
					result = 2f;
				}
				else if (this.pawn.Suspended)
				{
					result = 0f;
				}
				else
				{
					Pawn_NeedsTracker needs = this.pawn.needs;
					Need_Learning need_Learning = (needs != null) ? needs.learning : null;
					if (need_Learning != null && !need_Learning.Suspended)
					{
						result = this.GrowthPointsPerDayAtLearningLevel(need_Learning.CurLevel);
					}
					else
					{
						result = 0f;
					}
				}
				return result;
			}
		}

		// Token: 0x17000995 RID: 2453
		// (get) Token: 0x06003284 RID: 12932 RVA: 0x00107FBA File Offset: 0x001061BA
		private float GrowthPointsFactor
		{
			get
			{
				if ((float)this.AgeBiologicalYears < 7f)
				{
					return 0.75f;
				}
				return 1f;
			}
		}

		// Token: 0x06003285 RID: 12933 RVA: 0x00107FD8 File Offset: 0x001061D8
		public Pawn_AgeTracker(Pawn newPawn)
		{
			this.pawn = newPawn;
		}

		// Token: 0x06003286 RID: 12934 RVA: 0x00108054 File Offset: 0x00106254
		public float LifeStageMinAge(LifeStageDef lifeStage)
		{
			foreach (LifeStageAge lifeStageAge in this.pawn.RaceProps.lifeStageAges)
			{
				if (lifeStageAge.def == lifeStage)
				{
					return lifeStageAge.minAge;
				}
			}
			Log.Error(string.Format("Life stage def {0} not found while searching for min age of {1}", lifeStage, this.pawn));
			return 0f;
		}

		// Token: 0x06003287 RID: 12935 RVA: 0x001080DC File Offset: 0x001062DC
		private float GrowthPointsPerDayAtLearningLevel(float level)
		{
			return level * this.GrowthPointsFactor * this.pawn.ageTracker.ChildAgingMultiplier;
		}

		// Token: 0x06003288 RID: 12936 RVA: 0x001080F8 File Offset: 0x001062F8
		public void TrySimulateGrowthPoints()
		{
			if (!ModsConfig.BiotechActive || !this.pawn.RaceProps.Humanlike || this.AgeBiologicalYears >= 13)
			{
				return;
			}
			if (Pawn_AgeTracker.growthMomentAges == null)
			{
				Pawn_AgeTracker.growthMomentAges = new List<int>();
				Pawn_AgeTracker.growthMomentAges.Add(3);
				Pawn_AgeTracker.growthMomentAges.AddRange(GrowthUtility.GrowthMomentAges);
			}
			this.growthPoints = 0f;
			int ageBiologicalYears = this.AgeBiologicalYears;
			for (int i = Pawn_AgeTracker.growthMomentAges.Count - 1; i >= 0; i--)
			{
				if (ageBiologicalYears >= Pawn_AgeTracker.growthMomentAges[i])
				{
					float num = this.GrowthPointsPerDayAtLearningLevel(Rand.Range(0.2f, 0.5f)) * (((float)Pawn_AgeTracker.growthMomentAges[i] < 7f) ? 0.75f : 1f);
					int num2 = Pawn_AgeTracker.growthMomentAges[i] * 3600000;
					long num3 = this.AgeBiologicalTicks;
					while (num3 > (long)num2)
					{
						num3 -= 60000L;
						this.growthPoints += num;
					}
					break;
				}
			}
			this.growthPoints *= 0.25f;
		}

		// Token: 0x06003289 RID: 12937 RVA: 0x00108218 File Offset: 0x00106418
		public void ExposeData()
		{
			Scribe_Values.Look<long>(ref this.ageBiologicalTicksInt, "ageBiologicalTicks", 0L, false);
			Scribe_Values.Look<long>(ref this.birthAbsTicksInt, "birthAbsTicks", 0L, false);
			Scribe_Values.Look<float>(ref this.growth, "growth", -1f, false);
			Scribe_Values.Look<float>(ref this.progressToNextBiologicalTick, "progressToNextBiologicalTick", 0f, false);
			Scribe_Values.Look<long>(ref this.nextGrowthCheckTick, "nextGrowthCheckTick", -1L, false);
			Scribe_Values.Look<long>(ref this.vatGrowTicks, "vatGrowTicks", 0L, false);
			Scribe_Values.Look<long>(ref this.ageReversalDemandedAtAgeTicks, "ageReversalDemandedAtAgeTicks", long.MaxValue, false);
			Scribe_Values.Look<Pawn_AgeTracker.AgeReversalReason>(ref this.lastAgeReversalReason, "lastAgeReversalReason", Pawn_AgeTracker.AgeReversalReason.Initial, false);
			Scribe_Values.Look<bool>(ref this.initializedAgeReversalDemand, "initializedAgeReversalDemand", false, false);
			Scribe_Values.Look<bool>(ref this.lifeStageChange, "lifeStageChange", false, false);
			Scribe_Values.Look<float>(ref this.growthPoints, "growthPoints", -1f, false);
			Scribe_Values.Look<bool>(ref this.canGainGrowthPoints, "canGainGrowthPoints", true, false);
			Scribe_Values.Look<int>(ref this.lockedLifeStageIndex, "lockedLifeStageIndex", -1, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.cachedLifeStageIndex = -1;
				if (this.growthPoints < 0f)
				{
					this.TrySimulateGrowthPoints();
				}
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.AgeBiologicalYearsFloat > 25f && (float)this.AgeReversalDemandedDeadlineTicks > 5400000f)
			{
				this.ResetAgeReversalDemand(Pawn_AgeTracker.AgeReversalReason.Initial, false);
			}
			if (this.ageReversalDemandedAtAgeTicks == 9223372036854775807L)
			{
				this.ResetAgeReversalDemand(Pawn_AgeTracker.AgeReversalReason.Initial, false);
			}
		}

		// Token: 0x0600328A RID: 12938 RVA: 0x00108390 File Offset: 0x00106590
		private void TickBiologicalAge(int interval)
		{
			this.progressToNextBiologicalTick += this.BiologicalTicksPerTick * (float)interval;
			int num = Mathf.FloorToInt(this.progressToNextBiologicalTick);
			if (num > 0)
			{
				this.progressToNextBiologicalTick -= (float)num;
				this.ageBiologicalTicksInt += (long)num;
			}
		}

		// Token: 0x0600328B RID: 12939 RVA: 0x001083E4 File Offset: 0x001065E4
		public void AgeTickInterval(int delta)
		{
			if (this.lifeStageChange)
			{
				this.PostResolveLifeStageChange();
			}
			int ageBiologicalYears = this.AgeBiologicalYears;
			this.TickBiologicalAge(delta);
			if (this.lockedLifeStageIndex >= 0)
			{
				return;
			}
			if (this.pawn.IsMutant && this.pawn.mutant.Def.disableAging)
			{
				return;
			}
			if ((long)Find.TickManager.TicksGame >= this.nextGrowthCheckTick)
			{
				this.CalculateGrowth(240);
			}
			if (this.pawn.IsHashIntervalTick(60000, delta))
			{
				this.CheckAgeReversalDemand();
			}
			if (ageBiologicalYears < this.AgeBiologicalYears)
			{
				this.BirthdayBiological(this.AgeBiologicalYears);
			}
			if (this.initializedAgeReversalDemand && this.pawn.MapHeld != null && this.pawn.IsHashIntervalTick(2500, delta) && ExpectationsUtility.CurrentExpectationFor(this.pawn.MapHeld).order < ThoughtDefOf.AgeReversalDemanded.minExpectation.order)
			{
				this.ageReversalDemandedAtAgeTicks += 2500L;
			}
			if (this.pawn.IsFreeColonist && this.pawn.IsHashIntervalTick(1000, delta))
			{
				if (this.CurLifeStage.developmentalStage == DevelopmentalStage.Baby)
				{
					LessonAutoActivator.TeachOpportunity(ConceptDefOf.Babies, OpportunityType.Important);
					return;
				}
				if (this.CurLifeStage.developmentalStage == DevelopmentalStage.Child)
				{
					LessonAutoActivator.TeachOpportunity(ConceptDefOf.Children, OpportunityType.Important);
				}
			}
		}

		// Token: 0x0600328C RID: 12940 RVA: 0x0010853C File Offset: 0x0010673C
		public void AgeTickMothballed(int interval)
		{
			long num = this.ageBiologicalTicksInt;
			this.TickBiologicalAge(interval);
			this.CalculateGrowth(interval);
			this.CheckAgeReversalDemand();
			for (int i = (int)(num / 3600000L) + 1; i <= this.AgeBiologicalYears; i++)
			{
				this.BirthdayBiological(i);
			}
		}

		// Token: 0x0600328D RID: 12941 RVA: 0x00108584 File Offset: 0x00106784
		public void Notify_TickedInGrowthVat(int ticks)
		{
			this.vatGrowTicks += 1L;
			this.AgeTickMothballed(ticks);
		}

		// Token: 0x0600328E RID: 12942 RVA: 0x0010859C File Offset: 0x0010679C
		public void LockCurrentLifeStageIndex(int index)
		{
			this.lockedLifeStageIndex = index;
			this.RecalculateLifeStageIndex();
		}

		// Token: 0x0600328F RID: 12943 RVA: 0x001085AC File Offset: 0x001067AC
		private void CheckAgeReversalDemand()
		{
			if (!ModsConfig.IdeologyActive || this.initializedAgeReversalDemand || this.pawn.Faction != Faction.OfPlayer || this.pawn.MapHeld == null)
			{
				return;
			}
			this.ResetAgeReversalDemand(this.lastAgeReversalReason, false);
			this.initializedAgeReversalDemand = true;
		}

		// Token: 0x06003290 RID: 12944 RVA: 0x001085FC File Offset: 0x001067FC
		private void CalculateInitialGrowth()
		{
			this.growth = Mathf.Clamp01(this.AgeBiologicalYearsFloat / this.pawn.RaceProps.lifeStageAges[this.pawn.RaceProps.lifeStageAges.Count - 1].minAge);
			this.nextGrowthCheckTick = (long)(Find.TickManager.TicksGame + 240);
		}

		// Token: 0x06003291 RID: 12945 RVA: 0x00108664 File Offset: 0x00106864
		private void CalculateGrowth(int interval)
		{
			if (this.growth >= 1f)
			{
				this.nextGrowthCheckTick = long.MaxValue;
				return;
			}
			this.growth += PawnUtility.BodyResourceGrowthSpeed(this.pawn) * (float)interval / Mathf.Max((float)this.AdultMinAgeTicks, 1f);
			this.growth = Mathf.Min(this.growth, 1f);
			this.nextGrowthCheckTick = (long)(Find.TickManager.TicksGame + 240);
			this.RecalculateLifeStageIndex();
		}

		// Token: 0x06003292 RID: 12946 RVA: 0x001086F0 File Offset: 0x001068F0
		private void RecalculateLifeStageIndex()
		{
			int num = -1;
			if (this.lockedLifeStageIndex >= 0)
			{
				num = this.lockedLifeStageIndex;
			}
			else if (this.growth < 0f)
			{
				this.CalculateInitialGrowth();
			}
			float num2;
			if (this.pawn.RaceProps.Humanlike)
			{
				num2 = (float)this.AgeBiologicalYears;
			}
			else
			{
				num2 = Mathf.Lerp(0f, this.pawn.RaceProps.lifeStageAges[this.pawn.RaceProps.lifeStageAges.Count - 1].minAge, this.growth);
			}
			if (num < 0)
			{
				List<LifeStageAge> lifeStageAges = this.pawn.RaceProps.lifeStageAges;
				for (int i = lifeStageAges.Count - 1; i >= 0; i--)
				{
					if (lifeStageAges[i].minAge <= num2 + 1E-06f)
					{
						num = i;
						break;
					}
				}
			}
			if (num == -1)
			{
				num = 0;
			}
			int index = this.cachedLifeStageIndex;
			bool flag = this.cachedLifeStageIndex != num;
			this.cachedLifeStageIndex = num;
			if (flag)
			{
				this.lifeStageChange = true;
				if (!this.pawn.RaceProps.Humanlike || ModsConfig.BiotechActive)
				{
					this.pawn.Drawer.renderer.SetAllGraphicsDirty();
					this.CheckChangePawnKindName();
				}
				LifeStageWorker worker = this.CurLifeStage.Worker;
				Pawn pawn = this.pawn;
				LifeStageAge lifeStageAge = this.GetLifeStageAge(index);
				worker.Notify_LifeStageStarted(pawn, (lifeStageAge != null) ? lifeStageAge.def : null);
				if (this.pawn.SpawnedOrAnyParentSpawned)
				{
					PawnComponentsUtility.AddAndRemoveDynamicComponents(this.pawn, false);
				}
			}
		}

		// Token: 0x06003293 RID: 12947 RVA: 0x00108868 File Offset: 0x00106A68
		private void BirthdayBiological(int birthdayAge)
		{
			Pawn_AgeTracker.tmpHediffsGained.Clear();
			Pawn_AgeTracker.tmpEnabledWorkTypes.Clear();
			bool flag = (float)birthdayAge == this.AdultMinAge;
			bool flag2 = this.pawn.DevelopmentalStage.Child() && (float)birthdayAge == this.CurLifeStageRace.minAge;
			if (flag || flag2)
			{
				this.RecalculateLifeStageIndex();
			}
			float age = (float)birthdayAge / this.pawn.GetStatValue(StatDefOf.LifespanFactor, true, -1);
			foreach (HediffGiver_Birthday hediffGiver_Birthday in AgeInjuryUtility.RandomHediffsToGainOnBirthday(this.pawn, age))
			{
				if (hediffGiver_Birthday.TryApply(this.pawn, null))
				{
					Pawn_AgeTracker.tmpHediffsGained.Add(hediffGiver_Birthday.hediff);
				}
			}
			Building_Bed ownedBed = this.pawn.ownership.OwnedBed;
			if (ownedBed != null && this.pawn.ageTracker.CurLifeStage.bodySizeFactor > ownedBed.def.building.bed_maxBodySize)
			{
				this.pawn.ownership.UnclaimBed();
			}
			if (this.pawn.RaceProps.Humanlike)
			{
				List<LifeStageWorkSettings> lifeStageWorkSettings = this.pawn.RaceProps.lifeStageWorkSettings;
				for (int i = 0; i < lifeStageWorkSettings.Count; i++)
				{
					if (lifeStageWorkSettings[i].minAge == birthdayAge)
					{
						Pawn_AgeTracker.tmpEnabledWorkTypes.Add(lifeStageWorkSettings[i].workType);
					}
				}
				if (Pawn_AgeTracker.tmpEnabledWorkTypes.Count > 0)
				{
					this.pawn.Notify_DisabledWorkTypesChanged();
				}
				int passionChoiceCount;
				int num;
				int num2;
				this.TryChildGrowthMoment(birthdayAge, out passionChoiceCount, out num, out num2);
				bool flag3 = !flag2 && (!Pawn_AgeTracker.tmpEnabledWorkTypes.NullOrEmpty<WorkTypeDef>() || num2 > 0 || num > 0);
				if (!PawnUtility.ShouldSendNotificationAbout(this.pawn) || this.pawn.Faction != Faction.OfPlayer || this.pawn.IsQuestLodger())
				{
					if (num2 > 0)
					{
						SkillDef skillDef = ChoiceLetter_GrowthMoment.PassionOptions(this.pawn, num2, true).FirstOrFallback(null);
						if (skillDef != null)
						{
							SkillRecord skill = this.pawn.skills.GetSkill(skillDef);
							if (skill != null)
							{
								skill.passion = skill.passion.IncrementPassion();
							}
						}
					}
					if (num > 0)
					{
						Trait trait = PawnGenerator.GenerateTraitsFor(this.pawn, 1, null, true).FirstOrFallback(null);
						if (trait != null)
						{
							this.pawn.story.traits.GainTrait(trait, false);
						}
					}
				}
				else if (Pawn_AgeTracker.tmpHediffsGained.Count > 0 || flag3)
				{
					TaggedString taggedString = "LetterBirthdayBiological".Translate(this.pawn, birthdayAge);
					if (Pawn_AgeTracker.tmpHediffsGained.Count > 0)
					{
						taggedString += "\n\n" + "BirthdayBiologicalAgeInjuries".Translate(this.pawn);
						taggedString += ":\n\n" + (from h in Pawn_AgeTracker.tmpHediffsGained
						select h.LabelCap.Resolve()).ToLineList("  - ", false);
					}
					if (ModsConfig.BiotechActive && flag3 && this.pawn.Spawned && (this.pawn.DevelopmentalStage.Juvenile() || flag))
					{
						EffecterDefOf.Birthday.SpawnAttached(this.pawn, this.pawn.Map, 1f);
					}
					if (Pawn_AgeTracker.tmpHediffsGained.Count > 0)
					{
						LetterDef letterDef = LetterDefOf.NegativeEvent;
						Find.LetterStack.ReceiveLetter("LetterLabelBirthday".Translate(), taggedString, letterDef, this.pawn, null, null, null, null, 0, true);
					}
					if (ModsConfig.BiotechActive && flag3)
					{
						Name name = this.pawn.Name;
						NameTriple nameTriple = this.pawn.Name as NameTriple;
						if (nameTriple != null && !nameTriple.NickSet)
						{
							Rand.PushState(Gen.HashCombine<int>(this.pawn.thingIDNumber, birthdayAge));
							try
							{
								if (Rand.Chance(0.5f))
								{
									string name2 = PawnNameDatabaseShuffled.BankOf(PawnNameCategory.HumanStandard).GetName(PawnNameSlot.Nick, this.pawn.gender, true);
									this.pawn.Name = new NameTriple(nameTriple.First, name2, nameTriple.Last);
								}
							}
							finally
							{
								Rand.PopState();
							}
						}
						LetterDef letterDef = flag ? LetterDefOf.ChildToAdult : LetterDefOf.ChildBirthday;
						ChoiceLetter_GrowthMoment choiceLetter_GrowthMoment = (ChoiceLetter_GrowthMoment)LetterMaker.MakeLetter(letterDef);
						List<string> enabledWorkTypes = (from w in Pawn_AgeTracker.tmpEnabledWorkTypes
						select w.labelShort.CapitalizeFirst()).ToList<string>();
						choiceLetter_GrowthMoment.ConfigureGrowthLetter(this.pawn, passionChoiceCount, num, num2, enabledWorkTypes, name);
						choiceLetter_GrowthMoment.Label = (flag ? "LetterLabelBecameAdult".Translate(this.pawn) : "BirthdayGrowthMoment".Translate(this.pawn, name.ToStringShort.Named("PAWNNAME")));
						choiceLetter_GrowthMoment.StartTimeout(120000);
						this.canGainGrowthPoints = false;
						Find.LetterStack.ReceiveLetter(choiceLetter_GrowthMoment, null, 0, true);
					}
					Pawn_AgeTracker.tmpEnabledWorkTypes.Clear();
				}
			}
			Pawn_AgeTracker.tmpHediffsGained.Clear();
		}

		// Token: 0x06003294 RID: 12948 RVA: 0x00108DCC File Offset: 0x00106FCC
		public void TryChildGrowthMoment(int birthdayAge, out int newPassionOptions, out int newTraitOptions, out int passionGainsCount)
		{
			newPassionOptions = 0;
			newTraitOptions = 0;
			passionGainsCount = 0;
			if (ModsConfig.BiotechActive && GrowthUtility.IsGrowthBirthday(birthdayAge))
			{
				int growthTier = this.GrowthTier;
				newPassionOptions = (int)GrowthUtility.GrowthTiers[growthTier].passionChoices;
				newTraitOptions = (int)GrowthUtility.GrowthTiers[growthTier].traitChoices;
				passionGainsCount = Mathf.Min(this.pawn.skills.skills.Count((SkillRecord s) => s.passion < Passion.Major), GrowthUtility.GrowthTiers[growthTier].PassionGainFor(this.pawn));
			}
		}

		// Token: 0x06003295 RID: 12949 RVA: 0x00108E74 File Offset: 0x00107074
		public void ResetAgeReversalDemand(Pawn_AgeTracker.AgeReversalReason reason, bool cancelInitialization = false)
		{
			int num;
			if (reason == Pawn_AgeTracker.AgeReversalReason.Recruited)
			{
				num = this.RecruitedPawnAgeReversalDemandInDays.RandomInRange;
			}
			else if (reason == Pawn_AgeTracker.AgeReversalReason.ViaTreatment)
			{
				num = 60;
			}
			else
			{
				num = this.NewPawnAgeReversalDemandInDays.RandomInRange;
			}
			long num2 = (long)(num * 60000);
			long num3 = Math.Max(this.AgeBiologicalTicks, 90000000L) + num2;
			if (reason == Pawn_AgeTracker.AgeReversalReason.Recruited && num3 < this.ageReversalDemandedAtAgeTicks)
			{
				return;
			}
			this.ageReversalDemandedAtAgeTicks = num3;
			this.lastAgeReversalReason = reason;
			if (cancelInitialization)
			{
				this.initializedAgeReversalDemand = false;
			}
		}

		// Token: 0x06003296 RID: 12950 RVA: 0x00108EF4 File Offset: 0x001070F4
		public void PostResolveLifeStageChange()
		{
			this.pawn.health.CheckForStateChange(null, null);
			this.lifeStageChange = false;
			MeditationFocusTypeAvailabilityCache.ClearFor(this.pawn);
		}

		// Token: 0x06003297 RID: 12951 RVA: 0x00108F2D File Offset: 0x0010712D
		public void DebugForceAgeReversalDemandNow()
		{
			this.ageReversalDemandedAtAgeTicks = this.AgeBiologicalTicks;
		}

		// Token: 0x06003298 RID: 12952 RVA: 0x00108F3B File Offset: 0x0010713B
		public void DebugResetAgeReversalDemand()
		{
			this.ResetAgeReversalDemand(Pawn_AgeTracker.AgeReversalReason.Initial, false);
		}

		// Token: 0x06003299 RID: 12953 RVA: 0x00108F45 File Offset: 0x00107145
		public void Notify_IdeoChanged()
		{
			Ideo ideo = this.pawn.Ideo;
			if (ideo != null && ideo.HasPrecept(PreceptDefOf.AgeReversal_Demanded))
			{
				this.ResetAgeReversalDemand(Pawn_AgeTracker.AgeReversalReason.Recruited, false);
			}
		}

		// Token: 0x0600329A RID: 12954 RVA: 0x00108F6D File Offset: 0x0010716D
		public void DebugForceBirthdayBiological()
		{
			this.BirthdayBiological(this.AgeBiologicalYears);
		}

		// Token: 0x0600329B RID: 12955 RVA: 0x00108F7C File Offset: 0x0010717C
		public void DebugSetAge(long finalAgeTicks)
		{
			long num = finalAgeTicks - this.AgeBiologicalTicks;
			while (num > 0L)
			{
				int ageBiologicalYears = this.AgeBiologicalYears;
				long num2 = Pawn_AgeTracker.<DebugSetAge>g__NextBirthdayTick|126_0(this.AgeBiologicalTicks) - this.AgeBiologicalTicks;
				long num3 = (num2 > num) ? num : num2;
				this.AgeBiologicalTicks += num3;
				num -= num3;
				if (this.AgeBiologicalYears > ageBiologicalYears)
				{
					this.BirthdayBiological(this.AgeBiologicalYears);
				}
			}
		}

		// Token: 0x0600329C RID: 12956 RVA: 0x00108FE3 File Offset: 0x001071E3
		public void DebugSetGrowth(float val)
		{
			this.growth = val;
			this.RecalculateLifeStageIndex();
		}

		// Token: 0x0600329D RID: 12957 RVA: 0x00108FF4 File Offset: 0x001071F4
		public void CheckChangePawnKindName()
		{
			NameSingle nameSingle = this.pawn.Name as NameSingle;
			if (nameSingle == null || !nameSingle.Numerical)
			{
				return;
			}
			string b = this.pawn.KindLabel.CapitalizeFirst();
			if (nameSingle.NameWithoutNumber == b)
			{
				return;
			}
			int number = nameSingle.Number;
			string text = this.pawn.KindLabel.CapitalizeFirst() + " " + number.ToString();
			if (!NameUseChecker.NameSingleIsUsed(text))
			{
				this.pawn.Name = new NameSingle(text, true);
				return;
			}
			this.pawn.Name = PawnBioAndNameGenerator.GeneratePawnName(this.pawn, NameStyle.Numeric, null, false, null);
		}

		// Token: 0x0600329E RID: 12958 RVA: 0x0010909C File Offset: 0x0010729C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private LifeStageAge GetLifeStageAge(int index)
		{
			List<LifeStageAge> lifeStageAges = this.pawn.def.race.lifeStageAges;
			if (index < 0 || index >= lifeStageAges.Count)
			{
				return null;
			}
			return lifeStageAges[index];
		}

		// Token: 0x060032A0 RID: 12960 RVA: 0x001090F1 File Offset: 0x001072F1
		[CompilerGenerated]
		internal static long <DebugSetAge>g__NextBirthdayTick|126_0(long ageTicks)
		{
			return (ageTicks / 3600000L + 1L) * 3600000L;
		}

		// Token: 0x04002658 RID: 9816
		private Pawn pawn;

		// Token: 0x04002659 RID: 9817
		private long ageBiologicalTicksInt = -1L;

		// Token: 0x0400265A RID: 9818
		private long birthAbsTicksInt = -1L;

		// Token: 0x0400265B RID: 9819
		private long nextGrowthCheckTick = -1L;

		// Token: 0x0400265C RID: 9820
		private float growth = -1f;

		// Token: 0x0400265D RID: 9821
		private float progressToNextBiologicalTick;

		// Token: 0x0400265E RID: 9822
		private long ageReversalDemandedAtAgeTicks;

		// Token: 0x0400265F RID: 9823
		public long vatGrowTicks;

		// Token: 0x04002660 RID: 9824
		private Pawn_AgeTracker.AgeReversalReason lastAgeReversalReason;

		// Token: 0x04002661 RID: 9825
		private bool initializedAgeReversalDemand;

		// Token: 0x04002662 RID: 9826
		private bool lifeStageChange;

		// Token: 0x04002663 RID: 9827
		public float growthPoints = -1f;

		// Token: 0x04002664 RID: 9828
		public bool canGainGrowthPoints = true;

		// Token: 0x04002665 RID: 9829
		public int lockedLifeStageIndex = -1;

		// Token: 0x04002666 RID: 9830
		private int cachedLifeStageIndex = -1;

		// Token: 0x04002667 RID: 9831
		private const float BornAtLongitude = 0f;

		// Token: 0x04002668 RID: 9832
		private const int GrowthInterval = 240;

		// Token: 0x04002669 RID: 9833
		public const int AgeReversalDemandMinAgeYears = 25;

		// Token: 0x0400266A RID: 9834
		private const int TreatedPawnAgeReversalDemandInDays = 60;

		// Token: 0x0400266B RID: 9835
		public const int MinGrowthBirthday = 3;

		// Token: 0x0400266C RID: 9836
		public const int MaxGrowthBirthday = 13;

		// Token: 0x0400266D RID: 9837
		private const float NicknameGainChance = 0.5f;

		// Token: 0x0400266E RID: 9838
		private readonly IntRange NewPawnAgeReversalDemandInDays = new IntRange(20, 40);

		// Token: 0x0400266F RID: 9839
		private readonly IntRange RecruitedPawnAgeReversalDemandInDays = new IntRange(15, 20);

		// Token: 0x04002670 RID: 9840
		private const int ColonistDevelopmentStageLessonInterval = 1000;

		// Token: 0x04002671 RID: 9841
		private const float GrowthPointsSuspended = 2f;

		// Token: 0x04002672 RID: 9842
		private const float YoungAgeCutoff = 7f;

		// Token: 0x04002673 RID: 9843
		private const float GrowthPointsFactor_Young = 0.75f;

		// Token: 0x04002674 RID: 9844
		private const float GeneratedGrowthPointsFactor = 0.25f;

		// Token: 0x04002675 RID: 9845
		private const float MaxAgeChildFactorAge = 11f;

		// Token: 0x04002676 RID: 9846
		private const float MinAgeAdultFactorAge = 20f;

		// Token: 0x04002677 RID: 9847
		private static List<int> growthMomentAges = null;

		// Token: 0x04002678 RID: 9848
		private static List<WorkTypeDef> tmpEnabledWorkTypes = new List<WorkTypeDef>();

		// Token: 0x04002679 RID: 9849
		private static List<HediffDef> tmpHediffsGained = new List<HediffDef>();

		// Token: 0x020007CB RID: 1995
		public enum AgeReversalReason
		{
			// Token: 0x0400267B RID: 9851
			Initial,
			// Token: 0x0400267C RID: 9852
			Recruited,
			// Token: 0x0400267D RID: 9853
			ViaTreatment
		}
	}
}
