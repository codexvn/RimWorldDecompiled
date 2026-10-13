using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200195B RID: 6491
	public static class DrugStatsUtility
	{
		// Token: 0x06009380 RID: 37760 RVA: 0x002B5E23 File Offset: 0x002B4023
		public static CompProperties_Drug GetDrugComp(ThingDef d)
		{
			return d.GetCompProperties<CompProperties_Drug>();
		}

		// Token: 0x06009381 RID: 37761 RVA: 0x002B5E2B File Offset: 0x002B402B
		public static ChemicalDef GetChemical(ThingDef d)
		{
			CompProperties_Drug drugComp = DrugStatsUtility.GetDrugComp(d);
			if (drugComp == null)
			{
				return null;
			}
			return drugComp.chemical;
		}

		// Token: 0x06009382 RID: 37762 RVA: 0x002B5E3E File Offset: 0x002B403E
		public static NeedDef GetNeed(ThingDef d)
		{
			ChemicalDef chemical = DrugStatsUtility.GetChemical(d);
			if (chemical == null)
			{
				return null;
			}
			HediffDef addictionHediff = chemical.addictionHediff;
			if (addictionHediff == null)
			{
				return null;
			}
			return addictionHediff.chemicalNeed;
		}

		// Token: 0x06009383 RID: 37763 RVA: 0x002B5E5C File Offset: 0x002B405C
		public static HediffDef GetTolerance(ThingDef d)
		{
			ChemicalDef chemical = DrugStatsUtility.GetChemical(d);
			if (chemical == null)
			{
				return null;
			}
			return chemical.toleranceHediff;
		}

		// Token: 0x06009384 RID: 37764 RVA: 0x002B5E70 File Offset: 0x002B4070
		public static float GetAddictivenessAtTolerance(ThingDef d, float tolerance)
		{
			float num = DrugStatsUtility.GetDrugComp(d).addictiveness;
			if (DrugStatsUtility.GetTolerance(d) != null && tolerance > 0f)
			{
				num *= DrugStatsUtility.ToleranceToAddictivenessFactorCurve.Evaluate(tolerance);
			}
			return num;
		}

		// Token: 0x06009385 RID: 37765 RVA: 0x002B5EA8 File Offset: 0x002B40A8
		public static IngestionOutcomeDoer_GiveHediff GetDrugHighGiver(ThingDef d)
		{
			if (d.ingestible == null || d.ingestible.outcomeDoers == null)
			{
				return null;
			}
			foreach (IngestionOutcomeDoer ingestionOutcomeDoer in d.ingestible.outcomeDoers)
			{
				IngestionOutcomeDoer_GiveHediff ingestionOutcomeDoer_GiveHediff = ingestionOutcomeDoer as IngestionOutcomeDoer_GiveHediff;
				if (ingestionOutcomeDoer_GiveHediff != null && typeof(Hediff_High).IsAssignableFrom(ingestionOutcomeDoer_GiveHediff.hediffDef.hediffClass))
				{
					return ingestionOutcomeDoer_GiveHediff;
				}
			}
			return null;
		}

		// Token: 0x06009386 RID: 37766 RVA: 0x002B5F3C File Offset: 0x002B413C
		public static IngestionOutcomeDoer_GiveHediff GetToleranceGiver(ThingDef d)
		{
			if (d.ingestible == null || d.ingestible.outcomeDoers == null)
			{
				return null;
			}
			foreach (IngestionOutcomeDoer ingestionOutcomeDoer in d.ingestible.outcomeDoers)
			{
				IngestionOutcomeDoer_GiveHediff ingestionOutcomeDoer_GiveHediff = ingestionOutcomeDoer as IngestionOutcomeDoer_GiveHediff;
				if (ingestionOutcomeDoer_GiveHediff != null && ingestionOutcomeDoer_GiveHediff.hediffDef == DrugStatsUtility.GetTolerance(d))
				{
					return ingestionOutcomeDoer_GiveHediff;
				}
			}
			return null;
		}

		// Token: 0x06009387 RID: 37767 RVA: 0x002B5FC4 File Offset: 0x002B41C4
		public static float GetHighOffsetPerDay(ThingDef d)
		{
			IngestionOutcomeDoer_GiveHediff drugHighGiver = DrugStatsUtility.GetDrugHighGiver(d);
			if (drugHighGiver == null)
			{
				return 0f;
			}
			HediffCompProperties_SeverityPerDay hediffCompProperties_SeverityPerDay = drugHighGiver.hediffDef.CompProps<HediffCompProperties_SeverityPerDay>();
			if (hediffCompProperties_SeverityPerDay == null)
			{
				return 0f;
			}
			return hediffCompProperties_SeverityPerDay.severityPerDay;
		}

		// Token: 0x06009388 RID: 37768 RVA: 0x002B5FFC File Offset: 0x002B41FC
		public static float GetToleranceGain(ThingDef d)
		{
			if (d.ingestible == null || d.ingestible.outcomeDoers == null)
			{
				return 0f;
			}
			HediffDef tolerance = DrugStatsUtility.GetTolerance(d);
			if (tolerance != null)
			{
				foreach (IngestionOutcomeDoer ingestionOutcomeDoer in d.ingestible.outcomeDoers)
				{
					IngestionOutcomeDoer_GiveHediff ingestionOutcomeDoer_GiveHediff = ingestionOutcomeDoer as IngestionOutcomeDoer_GiveHediff;
					if (ingestionOutcomeDoer_GiveHediff != null && ingestionOutcomeDoer_GiveHediff.hediffDef == tolerance)
					{
						return ingestionOutcomeDoer_GiveHediff.severity;
					}
				}
			}
			return 0f;
		}

		// Token: 0x06009389 RID: 37769 RVA: 0x002B6094 File Offset: 0x002B4294
		public static float GetToleranceOffsetPerDay(ThingDef d)
		{
			HediffDef tolerance = DrugStatsUtility.GetTolerance(d);
			if (tolerance == null)
			{
				return 0f;
			}
			HediffCompProperties_SeverityPerDay hediffCompProperties_SeverityPerDay = tolerance.CompProps<HediffCompProperties_SeverityPerDay>();
			if (hediffCompProperties_SeverityPerDay == null)
			{
				return 0f;
			}
			return hediffCompProperties_SeverityPerDay.severityPerDay;
		}

		// Token: 0x0600938A RID: 37770 RVA: 0x002B60C8 File Offset: 0x002B42C8
		public static float GetAddictionOffsetPerDay(ThingDef d)
		{
			ChemicalDef chemical = DrugStatsUtility.GetChemical(d);
			HediffDef hediffDef = (chemical != null) ? chemical.addictionHediff : null;
			if (hediffDef == null)
			{
				return 0f;
			}
			HediffCompProperties_SeverityPerDay hediffCompProperties_SeverityPerDay = hediffDef.CompProps<HediffCompProperties_SeverityPerDay>();
			if (hediffCompProperties_SeverityPerDay == null)
			{
				return 0f;
			}
			return hediffCompProperties_SeverityPerDay.severityPerDay;
		}

		// Token: 0x0600938B RID: 37771 RVA: 0x002B6108 File Offset: 0x002B4308
		public static float GetAddictionNeedCostPerDay(ThingDef d)
		{
			NeedDef need = DrugStatsUtility.GetNeed(d);
			if (need != null)
			{
				return d.BaseMarketValue * need.fallPerDay * (1f + (1f - DrugStatsUtility.GetDrugComp(d).needLevelOffset));
			}
			return 0f;
		}

		// Token: 0x0600938C RID: 37772 RVA: 0x002B614C File Offset: 0x002B434C
		public static float GetSafeDoseInterval(ThingDef d, float bodySizeFactor = 1f)
		{
			CompProperties_Drug drugComp = DrugStatsUtility.GetDrugComp(d);
			if (drugComp == null || !drugComp.Addictive)
			{
				return 0f;
			}
			if (drugComp.addictiveness >= 1f || DrugStatsUtility.GetToleranceGiver(d) == null)
			{
				return -1f;
			}
			if (bodySizeFactor == 0f)
			{
				return -1f;
			}
			float num = Mathf.Abs(DrugStatsUtility.GetToleranceOffsetPerDay(d));
			return Mathf.Max(drugComp.overdoseSeverityOffset.TrueMax, (num > 0f) ? (DrugStatsUtility.GetToleranceGiver(d).severity / num) : -1f) / bodySizeFactor;
		}

		// Token: 0x0600938D RID: 37773 RVA: 0x002B61D4 File Offset: 0x002B43D4
		public static string GetSafeDoseIntervalReadout(ThingDef d)
		{
			IngestionOutcomeDoer_GiveHediff toleranceGiver = DrugStatsUtility.GetToleranceGiver(d);
			float safeDoseInterval = DrugStatsUtility.GetSafeDoseInterval(d, 1f);
			float num = (toleranceGiver != null) ? (DrugStatsUtility.GetDrugComp(d).minToleranceToAddict / toleranceGiver.severity) : 0f;
			string result;
			if (safeDoseInterval == 0f)
			{
				result = "AlwaysSafe".Translate();
			}
			else if (num < 1f)
			{
				result = "NeverSafe".Translate();
			}
			else
			{
				result = "PeriodDays".Translate(safeDoseInterval.ToString("F1"));
			}
			return result;
		}

		// Token: 0x0600938E RID: 37774 RVA: 0x002B6267 File Offset: 0x002B4467
		public static IEnumerable<StatDrawEntry> SpecialDisplayStats(ThingDef def)
		{
			CompProperties_Drug drugComp = DrugStatsUtility.GetDrugComp(def);
			if (drugComp == null)
			{
				yield break;
			}
			IngestionOutcomeDoer_GiveHediff highGiver = DrugStatsUtility.GetDrugHighGiver(def);
			if (highGiver != null)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Drug, "HighGain".Translate(), highGiver.severity.ToStringPercent(), "Stat_Thing_Drug_HighGainPerDose_Desc".Translate(), 2480, null, null, false, false);
				float highFall = Mathf.Abs(DrugStatsUtility.GetHighOffsetPerDay(def));
				if (highFall > 0f)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.Drug, "HighFallRate".Translate(), "PerDay".Translate(highFall.ToStringPercent()), "Stat_Thing_Drug_HighFallPerDay_Desc".Translate(), 2470, null, null, false, false);
					yield return new StatDrawEntry(StatCategoryDefOf.Drug, "HighDuration".Translate(), "PeriodDays".Translate((highGiver.severity / highFall).ToString("F1")), "Stat_Thing_Drug_HighDurationPerDose_Desc".Translate(), 2460, null, null, false, false);
				}
			}
			if (DrugStatsUtility.GetTolerance(def) != null)
			{
				float toleranceGain = DrugStatsUtility.GetToleranceGain(def);
				if (toleranceGain > 0f)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.Drug, "ToleranceGain".Translate(), toleranceGain.ToStringPercent(), "Stat_Thing_Drug_ToleranceGainPerDose_Desc".Translate(), 2450, null, null, false, false);
				}
				float num = Mathf.Abs(DrugStatsUtility.GetToleranceOffsetPerDay(def));
				if (num > 0f)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.Drug, "ToleranceFallRate".Translate(), "PerDay".Translate(num.ToStringPercent()), "Stat_Thing_Drug_ToleranceFallPerDay_Desc".Translate(), 2440, null, null, false, false);
				}
			}
			if (drugComp.Addictive)
			{
				ChemicalDef chemical = DrugStatsUtility.GetChemical(def);
				HediffDef addictionHediff = (chemical != null) ? chemical.addictionHediff : null;
				if (addictionHediff != null)
				{
					float num2 = Mathf.Abs(DrugStatsUtility.GetAddictionOffsetPerDay(def));
					if (num2 > 0f)
					{
						float num3 = addictionHediff.initialSeverity / num2;
						yield return new StatDrawEntry(StatCategoryDefOf.DrugAddiction, "AddictionRecoveryTime".Translate(), "PeriodDays".Translate(num3.ToString("F1")), "Stat_Thing_Drug_AddictionRecoveryTime_Desc".Translate(), 2395, null, null, false, false);
					}
					yield return new StatDrawEntry(StatCategoryDefOf.DrugAddiction, "AddictionSeverityInitial".Translate(), addictionHediff.initialSeverity.ToStringPercent(), "Stat_Thing_Drug_AddictionSeverityInitial_Desc".Translate(), 2427, null, null, false, false);
				}
				yield return new StatDrawEntry(StatCategoryDefOf.DrugAddiction, "AddictionNeedFallRate".Translate(), "PerDay".Translate(DrugStatsUtility.GetNeed(def).fallPerDay.ToStringPercent()), "Stat_Thing_Drug_AddictionNeedFallRate_Desc".Translate(), 2410, null, null, false, false);
				yield return new StatDrawEntry(StatCategoryDefOf.DrugAddiction, "AddictionCost".Translate(), "PerDay".Translate(DrugStatsUtility.GetAddictionNeedCostPerDay(def).ToStringMoney(null)), "Stat_Thing_Drug_AddictionCost_Desc".Translate(), 2390, null, null, false, false);
				yield return new StatDrawEntry(StatCategoryDefOf.DrugAddiction, "AddictionNeedDoseInterval".Translate(), "PeriodDays".Translate((drugComp.needLevelOffset / DrugStatsUtility.GetNeed(def).fallPerDay).ToString("F1")), "Stat_Thing_Drug_AddictionNeedDoseInterval_Desc".Translate(), 2400, null, null, false, false);
				if (drugComp.chemical != null)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.Drug, "Chemical".Translate(), drugComp.chemical.LabelCap, "Stat_Thing_Drug_Chemical_Desc".Translate(), 2490, null, null, false, false);
				}
				yield return new StatDrawEntry(StatCategoryDefOf.DrugAddiction, "Addictiveness".Translate(), drugComp.addictiveness.ToStringPercent(), "Stat_Thing_Drug_Addictiveness_Desc".Translate(), 2428, null, null, false, false);
				yield return new StatDrawEntry(StatCategoryDefOf.DrugAddiction, "AddictionNeedOffset".Translate(), drugComp.needLevelOffset.ToStringPercent(), "Stat_Thing_Drug_AddictionNeedOffset_Desc".Translate(), 2420, null, null, false, false);
				yield return new StatDrawEntry(StatCategoryDefOf.DrugAddiction, "MinimumToleranceForAddiction".Translate(), drugComp.minToleranceToAddict.ToStringPercent(), "Stat_Thing_Drug_MinToleranceForAddiction_Desc".Translate(), 2437, null, null, false, false);
				yield return new StatDrawEntry(StatCategoryDefOf.DrugAddiction, "AddictionSeverityPerDose".Translate(), drugComp.existingAddictionSeverityOffset.ToStringPercent(), "Stat_Thing_Drug_AddictionSeverityPerDose_Desc".Translate(), 2424, null, null, false, false);
				addictionHediff = null;
			}
			yield return new StatDrawEntry(StatCategoryDefOf.Drug, "RandomODChance".Translate(), drugComp.largeOverdoseChance.ToStringPercent(), "Stat_Thing_Drug_RandomODChance_Desc".Translate(), 2380, null, null, false, false);
			string text = "Stat_Thing_Drug_SafeDoseInterval_Desc".Translate();
			if (ModsConfig.BiotechActive && DrugStatsUtility.GetSafeDoseInterval(def, 1f) > 0f)
			{
				IngestionOutcomeDoer_GiveHediff toleranceGiver = DrugStatsUtility.GetToleranceGiver(def);
				if (((toleranceGiver != null) ? (DrugStatsUtility.GetDrugComp(def).minToleranceToAddict / toleranceGiver.severity) : 0f) >= 1f)
				{
					for (int i = 0; i < ThingDefOf.Human.race.lifeStageAges.Count; i++)
					{
						LifeStageAge lifeStageAge = ThingDefOf.Human.race.lifeStageAges[i];
						string t = (i < ThingDefOf.Human.race.lifeStageAges.Count - 1) ? "AgeXToY".Translate(lifeStageAge.minAge, ThingDefOf.Human.race.lifeStageAges[i + 1].minAge - 1f) : "AgePlus".Translate(lifeStageAge.minAge);
						text += "\n  - " + lifeStageAge.def.LabelCap + " (" + t + ")" + ": " + "PeriodDays".Translate(DrugStatsUtility.GetSafeDoseInterval(def, lifeStageAge.def.bodySizeFactor).ToString("F1"));
					}
				}
			}
			yield return new StatDrawEntry(StatCategoryDefOf.Drug, "SafeDoseInterval".Translate(), DrugStatsUtility.GetSafeDoseIntervalReadout(def), text, 2435, null, null, false, false);
			yield break;
		}

		// Token: 0x04005EF0 RID: 24304
		public static readonly SimpleCurve ToleranceToAddictivenessFactorCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(0.5f, 5f),
				true
			},
			{
				new CurvePoint(0.8f, 15f),
				true
			}
		};
	}
}
