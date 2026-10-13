using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;

namespace Verse
{
	// Token: 0x02000290 RID: 656
	public static class HediffStatsUtility
	{
		// Token: 0x06001069 RID: 4201 RVA: 0x0004D1DB File Offset: 0x0004B3DB
		public static IEnumerable<StatDrawEntry> SpecialDisplayStats(HediffStage stage, Hediff instance)
		{
			if (instance != null && instance.Bleeding)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "BleedingRate".Translate(), instance.BleedRateScaled.ToStringPercent() + "/" + "LetterDay".Translate(), "Stat_Hediff_BleedingRate_Desc".Translate(), 4040, null, null, false, false);
			}
			if ((instance != null && instance.pawn.RaceProps.IsFlesh) || (stage != null && instance == null))
			{
				float num = 0f;
				if (instance != null)
				{
					num = instance.PainOffset;
				}
				else if (stage != null)
				{
					num = stage.painOffset;
				}
				if (num != 0f)
				{
					if (num > 0f && num < 0.01f)
					{
						num = 0.01f;
					}
					if (num < 0f && num > -0.01f)
					{
						num = -0.01f;
					}
					yield return new StatDrawEntry(StatCategoryDefOf.CapacityEffects, "Pain".Translate(), string.Format("{0:+###0;-###0}%", num * 100f), "Stat_Hediff_Pain_Desc".Translate(), 4050, null, null, false, false);
				}
				float num2 = 1f;
				if (instance != null)
				{
					num2 = instance.PainFactor;
				}
				else if (stage != null)
				{
					num2 = stage.painFactor;
				}
				if (num2 != 1f)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.CapacityEffects, "Pain".Translate(), "x" + num2.ToStringPercent(), "Stat_Hediff_Pain_Desc".Translate(), 4050, null, null, false, false);
				}
			}
			if (stage != null && stage.partEfficiencyOffset != 0f)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "PartEfficiency".Translate(), stage.partEfficiencyOffset.ToStringByStyle(ToStringStyle.PercentZero, ToStringNumberSense.Offset), "Stat_Hediff_PartEfficiency_Desc".Translate(), 4050, null, null, false, false);
			}
			if (((instance != null) ? instance.def.addedPartProps : null) != null && instance.def.addedPartProps.partEfficiency != 1f)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "PartEfficiency".Translate(), instance.def.addedPartProps.partEfficiency.ToStringByStyle(ToStringStyle.PercentZero, ToStringNumberSense.Absolute), "Stat_Hediff_PartEfficiencyAbsolute_Desc".Translate(), 5000, null, null, false, false);
			}
			List<PawnCapacityModifier> capModsToDisplay = null;
			if (instance != null)
			{
				capModsToDisplay = instance.CapMods;
			}
			else if (stage != null)
			{
				capModsToDisplay = stage.capMods;
			}
			if (capModsToDisplay != null)
			{
				int num3;
				for (int i = 0; i < capModsToDisplay.Count; i = num3 + 1)
				{
					PawnCapacityModifier capMod = capModsToDisplay[i];
					bool flag;
					if (instance == null)
					{
						flag = (null != null);
					}
					else
					{
						Pawn pawn = instance.pawn;
						flag = (((pawn != null) ? pawn.health : null) != null);
					}
					if (!flag || instance.pawn.health.capacities.CapableOf(capMod.capacity))
					{
						if (capMod.offset != 0f)
						{
							yield return new StatDrawEntry(StatCategoryDefOf.CapacityEffects, capMod.capacity.GetLabelFor().CapitalizeFirst(), string.Format("{0:+#;-#}%", capMod.offset * 100f), capMod.capacity.description, 4060, null, null, false, false);
						}
						if (capMod.postFactor != 1f)
						{
							yield return new StatDrawEntry(StatCategoryDefOf.CapacityEffects, capMod.capacity.GetLabelFor().CapitalizeFirst(), "x" + capMod.postFactor.ToStringPercent(), capMod.capacity.description, 4060, null, null, false, false);
						}
						if (capMod.SetMaxDefined && instance != null)
						{
							yield return new StatDrawEntry(StatCategoryDefOf.CapacityEffects, capMod.capacity.GetLabelFor().CapitalizeFirst(), "max".Translate().CapitalizeFirst() + " " + capMod.EvaluateSetMax(instance.pawn).ToStringPercent(), capMod.capacity.description, 4060, null, null, false, false);
						}
						capMod = null;
					}
					num3 = i;
				}
			}
			capModsToDisplay = null;
			if (stage != null)
			{
				if (stage.AffectsMemory || stage.AffectsSocialInteractions)
				{
					StringBuilder stringBuilder = new StringBuilder();
					if (stage.AffectsMemory)
					{
						string text = "MemoryLower".Translate();
						if (stringBuilder.Length != 0)
						{
							stringBuilder.Append(", ");
						}
						else
						{
							text = text.CapitalizeFirst();
						}
						stringBuilder.Append(text);
					}
					if (stage.AffectsSocialInteractions)
					{
						string text2 = "SocialInteractionsLower".Translate();
						if (stringBuilder.Length != 0)
						{
							stringBuilder.Append(", ");
						}
						else
						{
							text2 = text2.CapitalizeFirst();
						}
						stringBuilder.Append(text2);
					}
					yield return new StatDrawEntry(StatCategoryDefOf.CapacityEffects, "Affects".Translate(), stringBuilder.ToString(), "Stat_Hediff_Affects_Desc".Translate(), 4080, null, null, false, false);
				}
				if (stage.hungerRateFactor != 1f)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.CapacityEffects, "HungerRate".Translate(), "x" + stage.hungerRateFactor.ToStringPercent(), "Stat_Hediff_HungerRateFactor_Desc".Translate(), 4051, null, null, false, false);
				}
				if (stage.hungerRateFactorOffset != 0f)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.CapacityEffects, "Stat_Hediff_HungerRateOffset_Name".Translate(), stage.hungerRateFactorOffset.ToStringSign() + stage.hungerRateFactorOffset.ToStringPercent(), "Stat_Hediff_HungerRateOffset_Desc".Translate(), 4051, null, null, false, false);
				}
				if (stage.restFallFactor != 1f)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.CapacityEffects, "Tiredness".Translate(), "x" + stage.restFallFactor.ToStringPercent(), "Stat_Hediff_TirednessFactor_Desc".Translate(), 4050, null, null, false, false);
				}
				if (stage.restFallFactorOffset != 0f)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.CapacityEffects, "Stat_Hediff_TirednessOffset_Name".Translate(), stage.restFallFactorOffset.ToStringSign() + stage.restFallFactorOffset.ToStringPercent(), "Stat_Hediff_TirednessOffset_Desc".Translate(), 4050, null, null, false, false);
				}
				if (stage.makeImmuneTo != null)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.CapacityEffects, "PreventsInfection".Translate(), (from im in stage.makeImmuneTo
					select im.label).ToCommaList(false, false).CapitalizeFirst(), "Stat_Hediff_PreventsInfection_Desc".Translate(), 4050, null, null, false, false);
				}
				if (stage.totalBleedFactor != 1f)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.CapacityEffects, "Stat_Hediff_TotalBleedFactor_Name".Translate(), stage.totalBleedFactor.ToStringPercent(), "Stat_Hediff_TotalBleedFactor_Desc".Translate(), 4041, null, null, false, false);
				}
				if (stage.naturalHealingFactor != -1f)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.CapacityEffects, "Stat_Hediff_NaturalHealingFactor_Name".Translate(), stage.naturalHealingFactor.ToStringByStyle(ToStringStyle.FloatTwo, ToStringNumberSense.Factor), "Stat_Hediff_NaturalHealingFactor_Desc".Translate(), 4020, null, null, false, false);
				}
				if (ModsConfig.AnomalyActive && stage.regeneration != -1f && stage.showRegenerationStat)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.CapacityEffects, "Stat_Hediff_Regeneration_Name".Translate(), "Stat_Hediff_Regeneration_Stat".Translate(string.Format("{0:0}", stage.regeneration)), "Stat_Hediff_Regeneration_Desc".Translate(), 4025, null, null, false, false);
				}
				if (stage.foodPoisoningChanceFactor != 1f)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Stat_Hediff_FoodPoisoningChanceFactor_Name".Translate(), stage.foodPoisoningChanceFactor.ToStringByStyle(ToStringStyle.FloatTwo, ToStringNumberSense.Factor), "Stat_Hediff_FoodPoisoningChanceFactor_Desc".Translate(), 4030, null, null, false, false);
				}
				if (stage.statOffsets != null)
				{
					int num3;
					for (int i = 0; i < stage.statOffsets.Count; i = num3 + 1)
					{
						StatModifier statModifier = stage.statOffsets[i];
						if (statModifier.stat.CanShowWithLoadedMods())
						{
							float num4 = statModifier.value;
							if (instance != null && instance.CurStage.multiplyStatChangesBySeverity)
							{
								num4 *= instance.Severity;
							}
							yield return new StatDrawEntry(StatCategoryDefOf.CapacityEffects, statModifier.stat.LabelCap, statModifier.stat.Worker.ValueToString(num4, false, ToStringNumberSense.Offset), statModifier.stat.description, 4070, null, null, false, false);
						}
						num3 = i;
					}
				}
				if (stage.statOffsetsBySeverity != null && instance != null)
				{
					int num3;
					for (int i = 0; i < stage.statOffsetsBySeverity.Count; i = num3 + 1)
					{
						StatModifierBySeverity statModifierBySeverity = stage.statOffsetsBySeverity[i];
						if (statModifierBySeverity.stat.CanShowWithLoadedMods())
						{
							float val = statModifierBySeverity.valueBySeverity.Evaluate(instance.Severity);
							yield return new StatDrawEntry(StatCategoryDefOf.CapacityEffects, statModifierBySeverity.stat.LabelCap, statModifierBySeverity.stat.Worker.ValueToString(val, false, ToStringNumberSense.Offset), statModifierBySeverity.stat.description, 4070, null, null, false, false);
						}
						num3 = i;
					}
				}
				if (stage.statFactors != null)
				{
					int num3;
					for (int i = 0; i < stage.statFactors.Count; i = num3 + 1)
					{
						StatModifier statModifier2 = stage.statFactors[i];
						if (statModifier2.stat.CanShowWithLoadedMods())
						{
							float num5 = statModifier2.value;
							if (instance != null && instance.CurStage.multiplyStatChangesBySeverity)
							{
								num5 = StatWorker.ScaleFactor(num5, instance.Severity);
							}
							yield return new StatDrawEntry(StatCategoryDefOf.CapacityEffects, statModifier2.stat.LabelCap, statModifier2.stat.Worker.ValueToString(num5, false, ToStringNumberSense.Factor), statModifier2.stat.description, 4070, null, null, false, false);
						}
						num3 = i;
					}
				}
				if (stage.statFactorsBySeverity != null && instance != null)
				{
					int num3;
					for (int i = 0; i < stage.statFactorsBySeverity.Count; i = num3 + 1)
					{
						StatModifierBySeverity statModifierBySeverity2 = stage.statFactorsBySeverity[i];
						if (statModifierBySeverity2.stat.CanShowWithLoadedMods())
						{
							float val2 = statModifierBySeverity2.valueBySeverity.Evaluate(instance.Severity);
							yield return new StatDrawEntry(StatCategoryDefOf.CapacityEffects, statModifierBySeverity2.stat.LabelCap, statModifierBySeverity2.stat.Worker.ValueToString(val2, false, ToStringNumberSense.Factor), statModifierBySeverity2.stat.description, 4070, null, null, false, false);
						}
						num3 = i;
					}
				}
				if (!stage.damageFactors.NullOrEmpty<DamageFactor>())
				{
					int num3;
					for (int i = 0; i < stage.damageFactors.Count; i = num3 + 1)
					{
						DamageFactor damageFactor = stage.damageFactors[i];
						float num6 = damageFactor.factor;
						if (instance != null && instance.CurStage.multiplyStatChangesBySeverity)
						{
							num6 = StatWorker.ScaleFactor(num6, instance.Severity);
						}
						if (Math.Abs(num6 - 1f) >= 0.001f)
						{
							string label;
							if (num6 >= 1f)
							{
								label = "DamageFactorWeakness".Translate(damageFactor.damageDef.Named("DAMAGE")).CapitalizeFirst();
							}
							else
							{
								label = "DamageFactorResistance".Translate(damageFactor.damageDef.Named("DAMAGE")).CapitalizeFirst();
							}
							yield return new StatDrawEntry(StatCategoryDefOf.CapacityEffects, label, "x" + num6.ToStringPercent(), "DamageFactor".Translate(damageFactor.damageDef.Named("DAMAGE")).CapitalizeFirst(), 4075, null, null, false, false);
						}
						num3 = i;
					}
				}
			}
			yield break;
		}

		// Token: 0x0600106A RID: 4202 RVA: 0x0004D1F4 File Offset: 0x0004B3F4
		public static float GetStatOffsetForSeverity(StatDef stat, HediffStage stage, Pawn pawn, float severity)
		{
			float num = stage.statOffsets.GetStatOffsetFromList(stat);
			if (num != 0f)
			{
				if (stage.statOffsetEffectMultiplier != null)
				{
					num *= pawn.GetStatValue(stage.statOffsetEffectMultiplier, true, -1);
				}
				if (stage.multiplyStatChangesBySeverity)
				{
					num *= severity;
				}
				return num;
			}
			if (stage.statOffsetsBySeverity != null)
			{
				for (int i = 0; i < stage.statOffsetsBySeverity.Count; i++)
				{
					if (stage.statOffsetsBySeverity[i].stat == stat)
					{
						return stage.statOffsetsBySeverity[i].valueBySeverity.Evaluate(severity);
					}
				}
			}
			return 0f;
		}

		// Token: 0x0600106B RID: 4203 RVA: 0x0004D28C File Offset: 0x0004B48C
		public static float GetStatFactorForSeverity(StatDef stat, HediffStage stage, Pawn pawn, float severity)
		{
			float num = stage.statFactors.GetStatFactorFromList(stat);
			if (Math.Abs(num - 1f) > 1E-45f)
			{
				if (stage.statFactorEffectMultiplier != null)
				{
					num = StatWorker.ScaleFactor(num, pawn.GetStatValue(stage.statFactorEffectMultiplier, true, -1));
				}
				if (stage.multiplyStatChangesBySeverity)
				{
					num = StatWorker.ScaleFactor(num, severity);
				}
				return num;
			}
			if (stage.statFactorsBySeverity != null)
			{
				for (int i = 0; i < stage.statFactorsBySeverity.Count; i++)
				{
					if (stage.statFactorsBySeverity[i].stat == stat)
					{
						return stage.statFactorsBySeverity[i].valueBySeverity.Evaluate(severity);
					}
				}
			}
			return 1f;
		}
	}
}
