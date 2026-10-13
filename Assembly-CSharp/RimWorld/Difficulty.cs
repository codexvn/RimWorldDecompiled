using System;
using System.Runtime.CompilerServices;
using System.Text;
using LudeonTK;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D25 RID: 7461
	public class Difficulty : IExposable
	{
		// Token: 0x0600A6E2 RID: 42722 RVA: 0x002FCBC8 File Offset: 0x002FADC8
		public Difficulty()
		{
		}

		// Token: 0x0600A6E3 RID: 42723 RVA: 0x002FCD68 File Offset: 0x002FAF68
		public Difficulty(DifficultyDef src)
		{
			this.CopyFrom(src);
		}

		// Token: 0x170019B9 RID: 6585
		// (get) Token: 0x0600A6E4 RID: 42724 RVA: 0x002FCF0E File Offset: 0x002FB10E
		public float MinThreatPointsCeiling
		{
			get
			{
				return this.minThreatPointsRangeCeilingCurveCached.Evaluate(GenDate.DaysPassedSinceSettleFloat);
			}
		}

		// Token: 0x170019BA RID: 6586
		// (get) Token: 0x0600A6E5 RID: 42725 RVA: 0x002FCF20 File Offset: 0x002FB120
		public float EffectiveQuestRewardValueFactor
		{
			get
			{
				return 1f / Mathf.Clamp(this.threatScale, 0.3f, 10f) * this.questRewardValueFactor;
			}
		}

		// Token: 0x170019BB RID: 6587
		// (get) Token: 0x0600A6E6 RID: 42726 RVA: 0x002FCF44 File Offset: 0x002FB144
		public float EffectiveRaidLootPointsFactor
		{
			get
			{
				return 1f / Mathf.Clamp(this.threatScale, 0.3f, 10f) * this.raidLootPointsFactor;
			}
		}

		// Token: 0x170019BC RID: 6588
		// (get) Token: 0x0600A6E7 RID: 42727 RVA: 0x002FCF68 File Offset: 0x002FB168
		public bool ChildrenAllowed
		{
			get
			{
				return ModsConfig.BiotechActive && !this.noBabiesOrChildren;
			}
		}

		// Token: 0x170019BD RID: 6589
		// (get) Token: 0x0600A6E8 RID: 42728 RVA: 0x002FCF7C File Offset: 0x002FB17C
		public bool ChildRaidersAllowed
		{
			get
			{
				return this.ChildrenAllowed && this.childRaidersAllowed;
			}
		}

		// Token: 0x170019BE RID: 6590
		// (get) Token: 0x0600A6E9 RID: 42729 RVA: 0x002FCF8E File Offset: 0x002FB18E
		// (set) Token: 0x0600A6EA RID: 42730 RVA: 0x002FCF9F File Offset: 0x002FB19F
		public AnomalyPlaystyleDef AnomalyPlaystyleDef
		{
			get
			{
				return this.anomalyPlaystyleDef ?? AnomalyPlaystyleDefOf.Standard;
			}
			set
			{
				this.anomalyPlaystyleDef = value;
			}
		}

		// Token: 0x0600A6EB RID: 42731 RVA: 0x002FCFA8 File Offset: 0x002FB1A8
		public bool AllowedToBuild(BuildableDef def)
		{
			ThingDef thingDef = def as ThingDef;
			if (thingDef != null)
			{
				if (!this.allowTraps && thingDef.building.isTrap)
				{
					return false;
				}
				if ((!this.allowMortars || !this.allowTurrets) && thingDef.building.IsTurret)
				{
					if (!thingDef.building.IsMortar)
					{
						return this.allowTurrets;
					}
					return this.allowMortars;
				}
			}
			return true;
		}

		// Token: 0x0600A6EC RID: 42732 RVA: 0x002FD010 File Offset: 0x002FB210
		public bool AllowedBy(DifficultyConditionConfig cfg)
		{
			return cfg == null || ((this.allowBigThreats || !cfg.bigThreatsDisabled) && (this.allowTraps || !cfg.trapsDisabled) && (this.allowTurrets || !cfg.turretsDisabled) && (this.allowMortars || !cfg.mortarsDisabled) && (this.allowExtremeWeatherIncidents || !cfg.extremeWeatherIncidentsDisabled));
		}

		// Token: 0x0600A6ED RID: 42733 RVA: 0x002FD080 File Offset: 0x002FB280
		public float CertaintyReductionFactor(Pawn initiator, Pawn recipient)
		{
			if (initiator != null && initiator.Faction == Faction.OfPlayer && initiator.Ideo != null && initiator.Ideo.initialPlayerIdeo)
			{
				int num = Difficulty.<CertaintyReductionFactor>g__ColonistsOfPlayerStartingIdeo|75_0();
				if (num > 2)
				{
					return 1f;
				}
				if (num == 2)
				{
					return this.lowPopConversionBoost / 2f;
				}
				return this.lowPopConversionBoost;
			}
			else
			{
				if (recipient == null || recipient.Faction != Faction.OfPlayer || recipient.Ideo == null || !recipient.Ideo.initialPlayerIdeo)
				{
					return 1f;
				}
				int num2 = Difficulty.<CertaintyReductionFactor>g__ColonistsOfPlayerStartingIdeo|75_0();
				if (num2 > 2)
				{
					return 1f;
				}
				if (num2 == 2)
				{
					return 1f / this.lowPopConversionBoost * 2f;
				}
				return 1f / this.lowPopConversionBoost;
			}
		}

		// Token: 0x0600A6EE RID: 42734 RVA: 0x002FD138 File Offset: 0x002FB338
		public void CopyFrom(DifficultyDef src)
		{
			this.threatScale = src.threatScale;
			this.allowBigThreats = src.allowBigThreats;
			this.allowIntroThreats = src.allowIntroThreats;
			this.allowCaveHives = src.allowCaveHives;
			this.peacefulTemples = src.peacefulTemples;
			this.allowViolentQuests = src.allowViolentQuests;
			this.predatorsHuntHumanlikes = src.predatorsHuntHumanlikes;
			this.scariaRotChance = src.scariaRotChance;
			this.colonistMoodOffset = src.colonistMoodOffset;
			this.tradePriceFactorLoss = src.tradePriceFactorLoss;
			this.cropYieldFactor = src.cropYieldFactor;
			this.mineYieldFactor = src.mineYieldFactor;
			this.butcherYieldFactor = src.butcherYieldFactor;
			this.fishingYieldFactor = src.fishingYieldFactor;
			this.researchSpeedFactor = src.researchSpeedFactor;
			this.diseaseIntervalFactor = src.diseaseIntervalFactor;
			this.enemyReproductionRateFactor = src.enemyReproductionRateFactor;
			this.playerPawnInfectionChanceFactor = src.playerPawnInfectionChanceFactor;
			this.manhunterChanceOnDamageFactor = src.manhunterChanceOnDamageFactor;
			this.deepDrillInfestationChanceFactor = src.deepDrillInfestationChanceFactor;
			this.wastepackInfestationChanceFactor = src.wastepackInfestationChanceFactor;
			this.nomadicMineableResourcesFactor = src.nomadicMineableResourcesFactor;
			this.foodPoisonChanceFactor = src.foodPoisonChanceFactor;
			this.maintenanceCostFactor = src.maintenanceCostFactor;
			this.enemyDeathOnDownedChanceFactor = src.enemyDeathOnDownedChanceFactor;
			this.adaptationGrowthRateFactorOverZero = src.adaptationGrowthRateFactorOverZero;
			this.adaptationEffectFactor = src.adaptationEffectFactor;
			this.questRewardValueFactor = src.questRewardValueFactor;
			this.raidLootPointsFactor = src.raidLootPointsFactor;
			this.allowTraps = src.allowTraps;
			this.allowTurrets = src.allowTurrets;
			this.allowMortars = src.allowMortars;
			this.classicMortars = src.classicMortars;
			this.allowExtremeWeatherIncidents = src.allowExtremeWeatherIncidents;
			this.fixedWealthMode = src.fixedWealthMode;
			this.fixedWealthTimeFactor = 1f;
			this.friendlyFireChanceFactor = 0.4f;
			this.allowInstantKillChance = 1f;
			this.lowPopConversionBoost = src.lowPopConversionBoost;
			this.minThreatPointsRangeCeiling = src.minThreatPointsRangeCeiling;
			this.babiesAreHealthy = src.babiesAreHealthy;
			this.noBabiesOrChildren = src.noBabiesOrChildren;
			this.childAgingRate = src.childAgingRate;
			this.adultAgingRate = src.adultAgingRate;
			this.unwaveringPrisoners = src.unwaveringPrisoners;
			this.childRaidersAllowed = src.childRaidersAllowed;
			this.anomalyThreatsInactiveFraction = src.anomalyThreatsInactiveFraction;
			this.anomalyThreatsActiveFraction = src.anomalyThreatsActiveFraction;
			this.studyEfficiencyFactor = src.studyEfficiencyFactor;
			this.SetMinThreatPointsCurve();
		}

		// Token: 0x0600A6EF RID: 42735 RVA: 0x002FD394 File Offset: 0x002FB594
		public void ExposeData()
		{
			Scribe_Values.Look<float>(ref this.threatScale, "threatScale", 0f, false);
			Scribe_Values.Look<bool>(ref this.allowBigThreats, "allowBigThreats", false, false);
			Scribe_Values.Look<bool>(ref this.allowIntroThreats, "allowIntroThreats", false, false);
			Scribe_Values.Look<bool>(ref this.allowCaveHives, "allowCaveHives", false, false);
			Scribe_Values.Look<bool>(ref this.peacefulTemples, "peacefulTemples", false, false);
			Scribe_Values.Look<bool>(ref this.allowViolentQuests, "allowViolentQuests", false, false);
			Scribe_Values.Look<bool>(ref this.babiesAreHealthy, "babiesAreHealthy", false, false);
			Scribe_Values.Look<bool>(ref this.noBabiesOrChildren, "noBabiesOrChildren", false, false);
			Scribe_Values.Look<bool>(ref this.predatorsHuntHumanlikes, "predatorsHuntHumanlikes", false, false);
			Scribe_Values.Look<float>(ref this.scariaRotChance, "scariaRotChance", 0f, false);
			Scribe_Values.Look<float>(ref this.colonistMoodOffset, "colonistMoodOffset", 0f, false);
			Scribe_Values.Look<float>(ref this.tradePriceFactorLoss, "tradePriceFactorLoss", 0f, false);
			Scribe_Values.Look<float>(ref this.cropYieldFactor, "cropYieldFactor", 0f, false);
			Scribe_Values.Look<float>(ref this.mineYieldFactor, "mineYieldFactor", 0f, false);
			Scribe_Values.Look<float>(ref this.butcherYieldFactor, "butcherYieldFactor", 0f, false);
			Scribe_Values.Look<float>(ref this.fishingYieldFactor, "fishingYieldFactor", 1f, false);
			Scribe_Values.Look<float>(ref this.researchSpeedFactor, "researchSpeedFactor", 0f, false);
			Scribe_Values.Look<float>(ref this.diseaseIntervalFactor, "diseaseIntervalFactor", 0f, false);
			Scribe_Values.Look<float>(ref this.enemyReproductionRateFactor, "enemyReproductionRateFactor", 0f, false);
			Scribe_Values.Look<float>(ref this.playerPawnInfectionChanceFactor, "playerPawnInfectionChanceFactor", 0f, false);
			Scribe_Values.Look<float>(ref this.manhunterChanceOnDamageFactor, "manhunterChanceOnDamageFactor", 0f, false);
			Scribe_Values.Look<float>(ref this.deepDrillInfestationChanceFactor, "deepDrillInfestationChanceFactor", 0f, false);
			Scribe_Values.Look<float>(ref this.wastepackInfestationChanceFactor, "wastepackInfestationChanceFactor", 0f, false);
			Scribe_Values.Look<float>(ref this.nomadicMineableResourcesFactor, "nomadicMineableResourcesFactor", 1f, false);
			Scribe_Values.Look<float>(ref this.foodPoisonChanceFactor, "foodPoisonChanceFactor", 0f, false);
			Scribe_Values.Look<float>(ref this.maintenanceCostFactor, "maintenanceCostFactor", 0f, false);
			Scribe_Values.Look<float>(ref this.enemyDeathOnDownedChanceFactor, "enemyDeathOnDownedChanceFactor", 0f, false);
			Scribe_Values.Look<float>(ref this.adaptationGrowthRateFactorOverZero, "adaptationGrowthRateFactorOverZero", 0f, false);
			Scribe_Values.Look<float>(ref this.adaptationEffectFactor, "adaptationEffectFactor", 0f, false);
			Scribe_Values.Look<float>(ref this.questRewardValueFactor, "questRewardValueFactor", 0f, false);
			Scribe_Values.Look<float>(ref this.raidLootPointsFactor, "raidLootPointsFactor", 1f, false);
			Scribe_Values.Look<bool>(ref this.allowTraps, "allowTraps", true, false);
			Scribe_Values.Look<bool>(ref this.allowTurrets, "allowTurrets", true, false);
			Scribe_Values.Look<bool>(ref this.allowMortars, "allowMortars", true, false);
			Scribe_Values.Look<bool>(ref this.classicMortars, "classicMortars", true, false);
			Scribe_Values.Look<bool>(ref this.allowExtremeWeatherIncidents, "allowExtremeWeatherIncidents", true, false);
			Scribe_Values.Look<bool>(ref this.fixedWealthMode, "fixedWealthMode", false, false);
			Scribe_Values.Look<float>(ref this.fixedWealthTimeFactor, "fixedWealthTimeFactor", 1f, false);
			Scribe_Values.Look<float>(ref this.friendlyFireChanceFactor, "friendlyFireChanceFactor", 0.4f, false);
			Scribe_Values.Look<float>(ref this.allowInstantKillChance, "allowInstantKillChance", 1f, false);
			Scribe_Values.Look<float>(ref this.lowPopConversionBoost, "lowPopConversionBoost", 3f, false);
			Scribe_Values.Look<float>(ref this.minThreatPointsRangeCeiling, "minThreatPointsRangeCeiling", 70f, false);
			Scribe_Values.Look<float>(ref this.adultAgingRate, "adultAgingRate", 1f, false);
			Scribe_Values.Look<float>(ref this.childAgingRate, "childAgingRate", 4f, false);
			Scribe_Values.Look<bool>(ref this.unwaveringPrisoners, "unwaveringPrisoners", false, false);
			Scribe_Values.Look<bool>(ref this.childRaidersAllowed, "childRaidersAllowed", true, false);
			Scribe_Values.Look<float>(ref this.anomalyThreatsInactiveFraction, "anomalyThreatsInactiveFraction", 0.08f, false);
			Scribe_Values.Look<float>(ref this.anomalyThreatsActiveFraction, "anomalyThreatsActiveFraction", 0.3f, false);
			Scribe_Values.Look<float?>(ref this.overrideAnomalyThreatsFraction, "overrideAnomalyThreatsFraction", null, false);
			Scribe_Values.Look<float>(ref this.studyEfficiencyFactor, "studyEfficiencyFactor", 1f, false);
			Scribe_Defs.Look<AnomalyPlaystyleDef>(ref this.anomalyPlaystyleDef, "anomalyPlaystyleDef");
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.maintenanceCostFactor = Mathf.Max(0.01f, this.maintenanceCostFactor);
				this.SetMinThreatPointsCurve();
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit && ModsConfig.AnomalyActive && this.anomalyPlaystyleDef == null)
			{
				this.anomalyPlaystyleDef = AnomalyPlaystyleDefOf.Standard;
			}
		}

		// Token: 0x0600A6F0 RID: 42736 RVA: 0x002FD808 File Offset: 0x002FBA08
		public string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("threatScale".PadRight(40)).Append(this.threatScale).AppendLine();
			stringBuilder.Append("allowBigThreats".PadRight(40)).Append(this.allowBigThreats).AppendLine();
			stringBuilder.Append("allowIntroThreats".PadRight(40)).Append(this.allowIntroThreats).AppendLine();
			stringBuilder.Append("allowCaveHives".PadRight(40)).Append(this.allowCaveHives).AppendLine();
			stringBuilder.Append("peacefulTemples".PadRight(40)).Append(this.peacefulTemples).AppendLine();
			stringBuilder.Append("allowViolentQuests".PadRight(40)).Append(this.allowViolentQuests).AppendLine();
			stringBuilder.Append("predatorsHuntHumanlikes".PadRight(40)).Append(this.predatorsHuntHumanlikes).AppendLine();
			stringBuilder.Append("scariaRotChance".PadRight(40)).Append(this.scariaRotChance).AppendLine();
			stringBuilder.Append("colonistMoodOffset".PadRight(40)).Append(this.colonistMoodOffset).AppendLine();
			stringBuilder.Append("tradePriceFactorLoss".PadRight(40)).Append(this.tradePriceFactorLoss).AppendLine();
			stringBuilder.Append("cropYieldFactor".PadRight(40)).Append(this.cropYieldFactor).AppendLine();
			stringBuilder.Append("mineYieldFactor".PadRight(40)).Append(this.mineYieldFactor).AppendLine();
			stringBuilder.Append("butcherYieldFactor".PadRight(40)).Append(this.butcherYieldFactor).AppendLine();
			stringBuilder.Append("fishingYieldFactor".PadRight(40)).Append(this.fishingYieldFactor).AppendLine();
			stringBuilder.Append("researchSpeedFactor".PadRight(40)).Append(this.researchSpeedFactor).AppendLine();
			stringBuilder.Append("diseaseIntervalFactor".PadRight(40)).Append(this.diseaseIntervalFactor).AppendLine();
			stringBuilder.Append("enemyReproductionRateFactor".PadRight(40)).Append(this.enemyReproductionRateFactor).AppendLine();
			stringBuilder.Append("playerPawnInfectionChanceFactor".PadRight(40)).Append(this.playerPawnInfectionChanceFactor).AppendLine();
			stringBuilder.Append("manhunterChanceOnDamageFactor".PadRight(40)).Append(this.manhunterChanceOnDamageFactor).AppendLine();
			stringBuilder.Append("deepDrillInfestationChanceFactor".PadRight(40)).Append(this.deepDrillInfestationChanceFactor).AppendLine();
			stringBuilder.Append("wastepackInfestationChanceFactor".PadRight(40)).Append(this.wastepackInfestationChanceFactor).AppendLine();
			stringBuilder.Append("nomadicMineableResourcesFactor".PadRight(40)).Append(this.nomadicMineableResourcesFactor).AppendLine();
			stringBuilder.Append("foodPoisonChanceFactor".PadRight(40)).Append(this.foodPoisonChanceFactor).AppendLine();
			stringBuilder.Append("maintenanceCostFactor".PadRight(40)).Append(this.maintenanceCostFactor).AppendLine();
			stringBuilder.Append("enemyDeathOnDownedChanceFactor".PadRight(40)).Append(this.enemyDeathOnDownedChanceFactor).AppendLine();
			stringBuilder.Append("adaptationGrowthRateFactorOverZero".PadRight(40)).Append(this.adaptationGrowthRateFactorOverZero).AppendLine();
			stringBuilder.Append("adaptationEffectFactor".PadRight(40)).Append(this.adaptationEffectFactor).AppendLine();
			stringBuilder.Append("questRewardValueFactor".PadRight(40)).Append(this.questRewardValueFactor).AppendLine();
			stringBuilder.Append("raidLootPointsFactor".PadRight(40)).Append(this.raidLootPointsFactor).AppendLine();
			stringBuilder.Append("allowTraps".PadRight(40)).Append(this.allowTraps).AppendLine();
			stringBuilder.Append("allowTurrets".PadRight(40)).Append(this.allowTurrets).AppendLine();
			stringBuilder.Append("allowMortars".PadRight(40)).Append(this.allowMortars).AppendLine();
			stringBuilder.Append("classicMortars".PadRight(40)).Append(this.classicMortars).AppendLine();
			stringBuilder.Append("allowExtremeWeatherIncidents".PadRight(40)).Append(this.allowExtremeWeatherIncidents).AppendLine();
			stringBuilder.Append("fixedWealthMode".PadRight(40)).Append(this.fixedWealthMode).AppendLine();
			stringBuilder.Append("fixedWealthTimeFactor".PadRight(40)).Append(this.fixedWealthTimeFactor).AppendLine();
			stringBuilder.Append("friendlyFireChanceFactor".PadRight(40)).Append(this.friendlyFireChanceFactor).AppendLine();
			stringBuilder.Append("allowInstantKillChance".PadRight(40)).Append(this.allowInstantKillChance).AppendLine();
			stringBuilder.Append("lowPopConversionBoost".PadRight(40)).Append(this.lowPopConversionBoost).AppendLine();
			stringBuilder.Append("minThreatPointsRangeCeiling".PadRight(40)).Append(this.minThreatPointsRangeCeiling).AppendLine();
			stringBuilder.Append("babiesAreHealthy".PadRight(40)).Append(this.babiesAreHealthy).AppendLine();
			stringBuilder.Append("noBabiesOrChildren".PadRight(40)).Append(this.noBabiesOrChildren).AppendLine();
			stringBuilder.Append("adultAgingRate".PadRight(40)).Append(this.adultAgingRate).AppendLine();
			stringBuilder.Append("childAgingRate".PadRight(40)).Append(this.childAgingRate).AppendLine();
			stringBuilder.Append("unwaveringPrisoners".PadRight(40)).Append(this.unwaveringPrisoners).AppendLine();
			stringBuilder.Append("childRaidersAllowed".PadRight(40)).Append(this.childRaidersAllowed).AppendLine();
			stringBuilder.Append("anomalyThreatsInactiveFraction".PadRight(40)).Append(this.anomalyThreatsInactiveFraction).AppendLine();
			stringBuilder.Append("anomalyThreatsActiveFraction".PadRight(40)).Append(this.anomalyThreatsActiveFraction).AppendLine();
			stringBuilder.Append("overrideAnomalyThreatsFraction".PadRight(40)).Append((this.overrideAnomalyThreatsFraction != null) ? this.overrideAnomalyThreatsFraction.ToString() : "null").AppendLine();
			stringBuilder.Append("studyEfficiencyFactor".PadRight(40)).Append(this.studyEfficiencyFactor).AppendLine();
			stringBuilder.Append("anomalyPlaystyleType".PadRight(40)).Append(this.anomalyPlaystyleDef.defName).AppendLine();
			return stringBuilder.ToString();
		}

		// Token: 0x0600A6F1 RID: 42737 RVA: 0x002FDF3C File Offset: 0x002FC13C
		[DebugOutput]
		public static void DifficultyDetails()
		{
			Log.Message(Find.Storyteller.difficulty.DebugString());
		}

		// Token: 0x0600A6F2 RID: 42738 RVA: 0x002FDF54 File Offset: 0x002FC154
		private void SetMinThreatPointsCurve()
		{
			if (this.minThreatPointsRangeCeiling < 35f)
			{
				Log.Warning(string.Format("Min threat points range ceiling is below {0}, resetting to {1}", 35f, 70f));
				this.minThreatPointsRangeCeiling = 70f;
			}
			this.minThreatPointsRangeCeilingCurveCached = new SimpleCurve
			{
				{
					new CurvePoint(12f, 35f),
					true
				},
				{
					new CurvePoint(35f, this.minThreatPointsRangeCeiling),
					true
				}
			};
		}

		// Token: 0x0600A6F3 RID: 42739 RVA: 0x002FDFD4 File Offset: 0x002FC1D4
		[CompilerGenerated]
		internal static int <CertaintyReductionFactor>g__ColonistsOfPlayerStartingIdeo|75_0()
		{
			int num = 0;
			foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_FreeColonists_NoLodgers)
			{
				if (pawn.Ideo != null && pawn.Ideo.initialPlayerIdeo)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x0400738C RID: 29580
		public const float DiseaseIntervalFactorCutoff = 100f;

		// Token: 0x0400738D RID: 29581
		public const float FriendlyFireDefault = 0.4f;

		// Token: 0x0400738E RID: 29582
		public const float AllowInstantKillChanceDefault = 1f;

		// Token: 0x0400738F RID: 29583
		public const float MaintenanceCostFactorMin = 0.01f;

		// Token: 0x04007390 RID: 29584
		public const float MinThreatPointsRangeCeilingDefault = 70f;

		// Token: 0x04007391 RID: 29585
		[Unsaved(false)]
		private SimpleCurve minThreatPointsRangeCeilingCurveCached;

		// Token: 0x04007392 RID: 29586
		public float threatScale = 1f;

		// Token: 0x04007393 RID: 29587
		public bool allowBigThreats = true;

		// Token: 0x04007394 RID: 29588
		public bool allowIntroThreats = true;

		// Token: 0x04007395 RID: 29589
		public bool allowCaveHives = true;

		// Token: 0x04007396 RID: 29590
		public bool peacefulTemples;

		// Token: 0x04007397 RID: 29591
		public bool allowViolentQuests = true;

		// Token: 0x04007398 RID: 29592
		public bool babiesAreHealthy;

		// Token: 0x04007399 RID: 29593
		public bool noBabiesOrChildren;

		// Token: 0x0400739A RID: 29594
		public bool childRaidersAllowed;

		// Token: 0x0400739B RID: 29595
		public bool childShamblersAllowed = true;

		// Token: 0x0400739C RID: 29596
		public bool predatorsHuntHumanlikes = true;

		// Token: 0x0400739D RID: 29597
		public float scariaRotChance;

		// Token: 0x0400739E RID: 29598
		public float colonistMoodOffset;

		// Token: 0x0400739F RID: 29599
		public float tradePriceFactorLoss;

		// Token: 0x040073A0 RID: 29600
		public float cropYieldFactor = 1f;

		// Token: 0x040073A1 RID: 29601
		public float mineYieldFactor = 1f;

		// Token: 0x040073A2 RID: 29602
		public float butcherYieldFactor = 1f;

		// Token: 0x040073A3 RID: 29603
		public float fishingYieldFactor = 1f;

		// Token: 0x040073A4 RID: 29604
		public float researchSpeedFactor = 1f;

		// Token: 0x040073A5 RID: 29605
		public float diseaseIntervalFactor = 1f;

		// Token: 0x040073A6 RID: 29606
		public float enemyReproductionRateFactor = 1f;

		// Token: 0x040073A7 RID: 29607
		public float playerPawnInfectionChanceFactor = 1f;

		// Token: 0x040073A8 RID: 29608
		public float manhunterChanceOnDamageFactor = 1f;

		// Token: 0x040073A9 RID: 29609
		public float deepDrillInfestationChanceFactor = 1f;

		// Token: 0x040073AA RID: 29610
		public float wastepackInfestationChanceFactor = 1f;

		// Token: 0x040073AB RID: 29611
		public float foodPoisonChanceFactor = 1f;

		// Token: 0x040073AC RID: 29612
		public float maintenanceCostFactor = 1f;

		// Token: 0x040073AD RID: 29613
		public float enemyDeathOnDownedChanceFactor = 1f;

		// Token: 0x040073AE RID: 29614
		public float adaptationGrowthRateFactorOverZero = 1f;

		// Token: 0x040073AF RID: 29615
		public float adaptationEffectFactor = 1f;

		// Token: 0x040073B0 RID: 29616
		public float questRewardValueFactor = 1f;

		// Token: 0x040073B1 RID: 29617
		public float raidLootPointsFactor = 1f;

		// Token: 0x040073B2 RID: 29618
		public bool allowTraps = true;

		// Token: 0x040073B3 RID: 29619
		public bool allowTurrets = true;

		// Token: 0x040073B4 RID: 29620
		public bool allowMortars = true;

		// Token: 0x040073B5 RID: 29621
		public bool classicMortars;

		// Token: 0x040073B6 RID: 29622
		public bool allowExtremeWeatherIncidents = true;

		// Token: 0x040073B7 RID: 29623
		public bool fixedWealthMode;

		// Token: 0x040073B8 RID: 29624
		public float fixedWealthTimeFactor = 1f;

		// Token: 0x040073B9 RID: 29625
		public float friendlyFireChanceFactor = 0.4f;

		// Token: 0x040073BA RID: 29626
		public float allowInstantKillChance = 1f;

		// Token: 0x040073BB RID: 29627
		public float nomadicMineableResourcesFactor = 1f;

		// Token: 0x040073BC RID: 29628
		public float lowPopConversionBoost = 3f;

		// Token: 0x040073BD RID: 29629
		public float minThreatPointsRangeCeiling;

		// Token: 0x040073BE RID: 29630
		public float childAgingRate = 4f;

		// Token: 0x040073BF RID: 29631
		public float adultAgingRate = 1f;

		// Token: 0x040073C0 RID: 29632
		public bool unwaveringPrisoners = true;

		// Token: 0x040073C1 RID: 29633
		public float anomalyThreatsInactiveFraction = 0.08f;

		// Token: 0x040073C2 RID: 29634
		public float anomalyThreatsActiveFraction = 0.3f;

		// Token: 0x040073C3 RID: 29635
		public float? overrideAnomalyThreatsFraction;

		// Token: 0x040073C4 RID: 29636
		public float studyEfficiencyFactor = 1f;

		// Token: 0x040073C5 RID: 29637
		private AnomalyPlaystyleDef anomalyPlaystyleDef;
	}
}
