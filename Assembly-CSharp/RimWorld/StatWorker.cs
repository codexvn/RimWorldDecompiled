using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003736 RID: 14134
	public class StatWorker
	{
		// Token: 0x06013976 RID: 80246 RVA: 0x005CE898 File Offset: 0x005CCA98
		public void InitSetStat(StatDef newStat)
		{
			this.stat = newStat;
		}

		// Token: 0x06013977 RID: 80247 RVA: 0x005CE8A1 File Offset: 0x005CCAA1
		public void SetCacheability(bool immutable)
		{
			this.immutableStatCache = (immutable ? new ConcurrentDictionary<Thing, float>() : null);
			if (this.stat.cacheable)
			{
				this.temporaryStatCache = new Dictionary<Thing, StatCacheEntry>();
			}
		}

		// Token: 0x06013978 RID: 80248 RVA: 0x005CE8CC File Offset: 0x005CCACC
		public float GetValue(Thing thing, bool applyPostProcess = true, int cacheStaleAfterTicks = -1)
		{
			if (this.stat.immutable)
			{
				float result;
				if (this.immutableStatCache.TryGetValue(thing, out result))
				{
					return result;
				}
				float value = this.GetValue(StatRequest.For(thing), applyPostProcess);
				this.immutableStatCache[thing] = value;
				return value;
			}
			else
			{
				int ticksGame = Find.TickManager.TicksGame;
				StatCacheEntry statCacheEntry;
				if (cacheStaleAfterTicks != -1 && this.temporaryStatCache != null && this.temporaryStatCache.TryGetValue(thing, out statCacheEntry) && ticksGame - statCacheEntry.gameTick < cacheStaleAfterTicks)
				{
					return statCacheEntry.statValue;
				}
				float value2 = this.GetValue(StatRequest.For(thing), true);
				if (this.temporaryStatCache != null)
				{
					if (!this.temporaryStatCache.ContainsKey(thing))
					{
						this.temporaryStatCache[thing] = new StatCacheEntry(value2, ticksGame);
					}
					else
					{
						statCacheEntry = this.temporaryStatCache[thing];
						statCacheEntry.statValue = value2;
						statCacheEntry.gameTick = ticksGame;
						this.temporaryStatCache[thing] = statCacheEntry;
					}
				}
				return value2;
			}
		}

		// Token: 0x06013979 RID: 80249 RVA: 0x005CE9B6 File Offset: 0x005CCBB6
		public float GetValue(Thing thing, Pawn pawn, bool applyPostProcess = true)
		{
			return this.GetValue(StatRequest.For(thing, pawn), applyPostProcess);
		}

		// Token: 0x0601397A RID: 80250 RVA: 0x005CE9C8 File Offset: 0x005CCBC8
		public float GetValue(StatRequest req, bool applyPostProcess = true)
		{
			if (this.stat.minifiedThingInherits)
			{
				MinifiedThing minifiedThing = req.Thing as MinifiedThing;
				if (minifiedThing != null)
				{
					if (minifiedThing.InnerThing != null)
					{
						return minifiedThing.InnerThing.GetStatValue(this.stat, applyPostProcess, -1);
					}
					Log.Error("MinifiedThing's inner thing is null.");
				}
			}
			float valueUnfinalized = this.GetValueUnfinalized(req, applyPostProcess);
			this.FinalizeValue(req, ref valueUnfinalized, applyPostProcess);
			return valueUnfinalized;
		}

		// Token: 0x0601397B RID: 80251 RVA: 0x005CEA2C File Offset: 0x005CCC2C
		public float GetValueAbstract(BuildableDef def, ThingDef stuffDef = null)
		{
			return this.GetValue(StatRequest.For(def, stuffDef, QualityCategory.Normal), true);
		}

		// Token: 0x0601397C RID: 80252 RVA: 0x005CEA3D File Offset: 0x005CCC3D
		public float GetValueAbstract(AbilityDef def, Pawn forPawn = null)
		{
			return this.GetValue(StatRequest.For(def, forPawn), true);
		}

		// Token: 0x0601397D RID: 80253 RVA: 0x005CEA50 File Offset: 0x005CCC50
		public virtual float GetValueUnfinalized(StatRequest req, bool applyPostProcess = true)
		{
			if (!this.stat.supressDisabledError && Prefs.DevMode && this.IsDisabledFor(req.Thing))
			{
				Log.ErrorOnce(string.Format("Attempted to calculate value for disabled stat {0}; this is meant as a consistency check, either set the stat to neverDisabled or ensure this pawn cannot accidentally use this stat (thing={1})", this.stat, req.Thing.ToStringSafe<Thing>()), 75193282 + (int)this.stat.index);
			}
			float num = this.GetBaseValueFor(req);
			Pawn pawn = req.Thing as Pawn;
			if (pawn != null)
			{
				if (pawn.skills != null)
				{
					if (this.stat.skillNeedOffsets != null)
					{
						for (int i = 0; i < this.stat.skillNeedOffsets.Count; i++)
						{
							num += this.stat.skillNeedOffsets[i].ValueFor(pawn);
						}
					}
				}
				else
				{
					num += this.stat.noSkillOffset;
				}
				if (this.stat.capacityOffsets != null)
				{
					for (int j = 0; j < this.stat.capacityOffsets.Count; j++)
					{
						PawnCapacityOffset pawnCapacityOffset = this.stat.capacityOffsets[j];
						num += pawnCapacityOffset.GetOffset(pawn.health.capacities.GetLevel(pawnCapacityOffset.capacity));
					}
				}
				if (pawn.story != null)
				{
					for (int k = 0; k < pawn.story.traits.allTraits.Count; k++)
					{
						if (!pawn.story.traits.allTraits[k].Suppressed)
						{
							num += pawn.story.traits.allTraits[k].OffsetOfStat(this.stat);
						}
					}
				}
				List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
				for (int l = 0; l < hediffs.Count; l++)
				{
					HediffStage curStage = hediffs[l].CurStage;
					if (curStage != null)
					{
						num += HediffStatsUtility.GetStatOffsetForSeverity(this.stat, curStage, pawn, hediffs[l].Severity);
					}
				}
				if (pawn.Ideo != null)
				{
					List<Precept> preceptsListForReading = pawn.Ideo.PreceptsListForReading;
					for (int m = 0; m < preceptsListForReading.Count; m++)
					{
						if (preceptsListForReading[m].def.statOffsets != null)
						{
							num += preceptsListForReading[m].def.statOffsets.GetStatOffsetFromList(this.stat);
						}
						if (preceptsListForReading[m].def.conditionalStatAffecters != null)
						{
							for (int n = 0; n < preceptsListForReading[m].def.conditionalStatAffecters.Count; n++)
							{
								ConditionalStatAffecter conditionalStatAffecter = preceptsListForReading[m].def.conditionalStatAffecters[n];
								if (conditionalStatAffecter.statOffsets != null && conditionalStatAffecter.Applies(req))
								{
									num += conditionalStatAffecter.statOffsets.GetStatOffsetFromList(this.stat);
								}
							}
						}
					}
					Precept_Role role = pawn.Ideo.GetRole(pawn);
					if (((role != null) ? role.def.roleEffects : null) != null)
					{
						foreach (RoleEffect roleEffect in role.def.roleEffects)
						{
							RoleEffect_PawnStatOffset roleEffect_PawnStatOffset = roleEffect as RoleEffect_PawnStatOffset;
							if (roleEffect_PawnStatOffset != null && roleEffect_PawnStatOffset.statDef == this.stat)
							{
								num += roleEffect_PawnStatOffset.modifier;
							}
						}
					}
				}
				if (ModsConfig.BiotechActive && pawn.genes != null)
				{
					List<Gene> genesListForReading = pawn.genes.GenesListForReading;
					for (int num2 = 0; num2 < genesListForReading.Count; num2++)
					{
						if (genesListForReading[num2].Active)
						{
							num += genesListForReading[num2].def.statOffsets.GetStatOffsetFromList(this.stat);
							if (genesListForReading[num2].def.conditionalStatAffecters != null)
							{
								for (int num3 = 0; num3 < genesListForReading[num2].def.conditionalStatAffecters.Count; num3++)
								{
									ConditionalStatAffecter conditionalStatAffecter2 = genesListForReading[num2].def.conditionalStatAffecters[num3];
									if (conditionalStatAffecter2.Applies(req))
									{
										num += conditionalStatAffecter2.statOffsets.GetStatOffsetFromList(this.stat);
									}
								}
							}
						}
					}
				}
				num += pawn.ageTracker.CurLifeStage.statOffsets.GetStatOffsetFromList(this.stat);
				if (pawn.apparel != null)
				{
					for (int num4 = 0; num4 < pawn.apparel.WornApparel.Count; num4++)
					{
						num += StatWorker.StatOffsetFromGear(pawn.apparel.WornApparel[num4], this.stat);
					}
				}
				Pawn_EquipmentTracker equipment = pawn.equipment;
				if (((equipment != null) ? equipment.Primary : null) != null)
				{
					num += StatWorker.StatOffsetFromGear(pawn.equipment.Primary, this.stat);
				}
				if (pawn.story != null)
				{
					for (int num5 = 0; num5 < pawn.story.traits.allTraits.Count; num5++)
					{
						if (!pawn.story.traits.allTraits[num5].Suppressed)
						{
							num *= pawn.story.traits.allTraits[num5].MultiplierOfStat(this.stat);
						}
					}
				}
				for (int num6 = 0; num6 < hediffs.Count; num6++)
				{
					HediffStage curStage2 = hediffs[num6].CurStage;
					if (curStage2 != null)
					{
						num *= HediffStatsUtility.GetStatFactorForSeverity(this.stat, curStage2, pawn, hediffs[num6].Severity);
					}
				}
				if (pawn.Ideo != null)
				{
					List<Precept> preceptsListForReading2 = pawn.Ideo.PreceptsListForReading;
					for (int num7 = 0; num7 < preceptsListForReading2.Count; num7++)
					{
						if (preceptsListForReading2[num7].def.statFactors != null)
						{
							num *= preceptsListForReading2[num7].def.statFactors.GetStatFactorFromList(this.stat);
						}
						if (preceptsListForReading2[num7].def.conditionalStatAffecters != null)
						{
							for (int num8 = 0; num8 < preceptsListForReading2[num7].def.conditionalStatAffecters.Count; num8++)
							{
								ConditionalStatAffecter conditionalStatAffecter3 = preceptsListForReading2[num7].def.conditionalStatAffecters[num8];
								if (conditionalStatAffecter3.statFactors != null && conditionalStatAffecter3.Applies(req))
								{
									num *= conditionalStatAffecter3.statFactors.GetStatFactorFromList(this.stat);
								}
							}
						}
					}
					Precept_Role role2 = pawn.Ideo.GetRole(pawn);
					if (((role2 != null) ? role2.def.roleEffects : null) != null)
					{
						foreach (RoleEffect roleEffect2 in role2.def.roleEffects)
						{
							RoleEffect_PawnStatFactor roleEffect_PawnStatFactor = roleEffect2 as RoleEffect_PawnStatFactor;
							if (roleEffect_PawnStatFactor != null && roleEffect_PawnStatFactor.statDef == this.stat)
							{
								num *= roleEffect_PawnStatFactor.modifier;
							}
						}
					}
				}
				if (ModsConfig.BiotechActive && pawn.genes != null)
				{
					List<Gene> genesListForReading2 = pawn.genes.GenesListForReading;
					for (int num9 = 0; num9 < genesListForReading2.Count; num9++)
					{
						if (genesListForReading2[num9].Active)
						{
							num *= genesListForReading2[num9].def.statFactors.GetStatFactorFromList(this.stat);
							if (genesListForReading2[num9].def.conditionalStatAffecters != null)
							{
								for (int num10 = 0; num10 < genesListForReading2[num9].def.conditionalStatAffecters.Count; num10++)
								{
									ConditionalStatAffecter conditionalStatAffecter4 = genesListForReading2[num9].def.conditionalStatAffecters[num10];
									if (conditionalStatAffecter4.Applies(req))
									{
										num *= conditionalStatAffecter4.statFactors.GetStatFactorFromList(this.stat);
									}
								}
							}
						}
					}
				}
				num *= pawn.ageTracker.CurLifeStage.statFactors.GetStatFactorFromList(this.stat);
			}
			if (req.StuffDef != null)
			{
				if (num > 0f || this.stat.applyFactorsIfNegative)
				{
					num *= req.StuffDef.stuffProps.statFactors.GetStatFactorFromList(this.stat);
					QualityCategory qc;
					if (req.Thing.TryGetQuality(out qc))
					{
						num *= req.StuffDef.stuffProps.statFactorsQuality.GetStatFactorFromList(this.stat, qc);
					}
				}
				num += req.StuffDef.stuffProps.statOffsets.GetStatOffsetFromList(this.stat);
				QualityCategory qc2;
				if (req.Thing.TryGetQuality(out qc2))
				{
					num += req.StuffDef.stuffProps.statOffsetsQuality.GetStatOffsetFromList(this.stat, qc2);
				}
			}
			if (req.ForAbility)
			{
				if (this.stat.statFactors != null)
				{
					for (int num11 = 0; num11 < this.stat.statFactors.Count; num11++)
					{
						num *= req.AbilityDef.statBases.GetStatValueFromList(this.stat.statFactors[num11], 1f);
					}
				}
				Pawn pawn2 = req.Pawn;
				if (((pawn2 != null) ? pawn2.Ideo : null) != null)
				{
					List<Precept> preceptsListForReading3 = pawn2.Ideo.PreceptsListForReading;
					for (int num12 = 0; num12 < preceptsListForReading3.Count; num12++)
					{
						if (preceptsListForReading3[num12].def.statFactors != null)
						{
							num *= preceptsListForReading3[num12].def.statFactors.GetStatFactorFromList(this.stat);
						}
						if (preceptsListForReading3[num12].def.abilityStatFactors != null)
						{
							foreach (AbilityStatModifiers abilityStatModifiers in preceptsListForReading3[num12].def.abilityStatFactors)
							{
								if (abilityStatModifiers.ability == req.AbilityDef)
								{
									num *= abilityStatModifiers.modifiers.GetStatFactorFromList(this.stat);
								}
							}
						}
					}
				}
			}
			if (req.HasThing)
			{
				ThingWithComps thingWithComps = req.Thing as ThingWithComps;
				if (thingWithComps != null)
				{
					List<ThingComp> allComps = thingWithComps.AllComps;
					for (int num13 = 0; num13 < allComps.Count; num13++)
					{
						num += allComps[num13].GetStatOffset(this.stat);
					}
					for (int num14 = 0; num14 < allComps.Count; num14++)
					{
						num *= allComps[num14].GetStatFactor(this.stat);
					}
				}
				if (this.stat.statFactors != null)
				{
					for (int num15 = 0; num15 < this.stat.statFactors.Count; num15++)
					{
						num *= req.Thing.GetStatValue(this.stat.statFactors[num15], true, -1);
					}
				}
				if (pawn != null)
				{
					if (pawn.skills != null)
					{
						if (this.stat.skillNeedFactors != null)
						{
							for (int num16 = 0; num16 < this.stat.skillNeedFactors.Count; num16++)
							{
								num *= this.stat.skillNeedFactors[num16].ValueFor(pawn);
							}
						}
					}
					else
					{
						num *= this.stat.noSkillFactor;
					}
					if (this.stat.capacityFactors != null)
					{
						for (int num17 = 0; num17 < this.stat.capacityFactors.Count; num17++)
						{
							PawnCapacityFactor pawnCapacityFactor = this.stat.capacityFactors[num17];
							float factor = pawnCapacityFactor.GetFactor(pawn.health.capacities.GetLevel(pawnCapacityFactor.capacity));
							num = Mathf.Lerp(num, num * factor, pawnCapacityFactor.weight);
						}
					}
					if (pawn.Inspired)
					{
						num += pawn.InspirationDef.statOffsets.GetStatOffsetFromList(this.stat);
						num *= pawn.InspirationDef.statFactors.GetStatFactorFromList(this.stat);
					}
				}
			}
			return num;
		}

		// Token: 0x0601397E RID: 80254 RVA: 0x005CF674 File Offset: 0x005CD874
		public virtual string GetExplanationUnfinalized(StatRequest req, ToStringNumberSense numberSense)
		{
			StringBuilder stringBuilder = new StringBuilder();
			float baseValueFor = this.GetBaseValueFor(req);
			if (baseValueFor != 0f || this.stat.showZeroBaseValue)
			{
				stringBuilder.AppendLine("StatsReport_BaseValue".Translate() + ": " + this.stat.ValueToString(baseValueFor, numberSense, true));
			}
			this.GetOffsetsAndFactorsExplanation(req, stringBuilder, baseValueFor, "");
			return stringBuilder.ToString();
		}

		// Token: 0x0601397F RID: 80255 RVA: 0x005CF6EC File Offset: 0x005CD8EC
		public void GetOffsetsAndFactorsExplanation(StatRequest req, StringBuilder sb, float baseValue, string whitespace = "")
		{
			Pawn pawn = req.Thing as Pawn;
			if (pawn != null)
			{
				if (pawn.skills != null)
				{
					if (this.stat.skillNeedOffsets != null)
					{
						sb.AppendLine(whitespace + "StatsReport_Skills".Translate());
						for (int i = 0; i < this.stat.skillNeedOffsets.Count; i++)
						{
							SkillNeed skillNeed = this.stat.skillNeedOffsets[i];
							int level = pawn.skills.GetSkill(skillNeed.skill).Level;
							float val = skillNeed.ValueFor(pawn);
							sb.AppendLine(string.Concat(new string[]
							{
								"    " + skillNeed.skill.LabelCap + " (",
								level.ToString(),
								"): ",
								val.ToStringSign(),
								this.ValueToString(val, false, ToStringNumberSense.Absolute)
							}));
						}
					}
				}
				else if (this.stat.noSkillOffset != 0f)
				{
					sb.AppendLine(whitespace + "StatsReport_Skills".Translate());
					sb.AppendLine(whitespace + "    " + "default".Translate().CapitalizeFirst() + " : " + this.stat.noSkillOffset.ToStringSign() + this.ValueToString(this.stat.noSkillOffset, false, ToStringNumberSense.Absolute));
				}
				if (this.stat.capacityOffsets != null)
				{
					sb.AppendLine(whitespace + ("StatsReport_Health".CanTranslate() ? "StatsReport_Health".Translate() : "StatsReport_HealthFactors".Translate()));
					foreach (PawnCapacityOffset pawnCapacityOffset in from hfa in this.stat.capacityOffsets
					orderby hfa.capacity.listOrder
					select hfa)
					{
						string text = pawnCapacityOffset.capacity.GetLabelFor(pawn).CapitalizeFirst();
						float level2 = pawn.health.capacities.GetLevel(pawnCapacityOffset.capacity);
						float offset = pawnCapacityOffset.GetOffset(pawn.health.capacities.GetLevel(pawnCapacityOffset.capacity));
						string text2 = this.ValueToString(offset, false, ToStringNumberSense.Absolute);
						string text3 = Mathf.Min(level2, pawnCapacityOffset.max).ToStringPercent() + ", " + "HealthOffsetScale".Translate(pawnCapacityOffset.scale.ToString() + "x");
						if (pawnCapacityOffset.max < 999f)
						{
							text3 += ", " + "HealthFactorMaxImpact".Translate(pawnCapacityOffset.max.ToStringPercent());
						}
						sb.AppendLine(string.Concat(new string[]
						{
							whitespace,
							"    ",
							text,
							": ",
							offset.ToStringSign(),
							text2,
							" (",
							text3,
							")"
						}));
					}
				}
				if (pawn.RaceProps.intelligence >= Intelligence.ToolUser)
				{
					Pawn_StoryTracker story = pawn.story;
					if (((story != null) ? story.traits : null) != null)
					{
						List<Trait> list = (from tr in pawn.story.traits.allTraits
						where !tr.Suppressed && tr.CurrentData.statOffsets != null && tr.CurrentData.statOffsets.Any((StatModifier se) => se.stat == this.stat)
						select tr).ToList<Trait>();
						List<Trait> list2 = (from tr in pawn.story.traits.allTraits
						where !tr.Suppressed && tr.CurrentData.statFactors != null && tr.CurrentData.statFactors.Any((StatModifier se) => se.stat == this.stat)
						select tr).ToList<Trait>();
						if (list.Count > 0 || list2.Count > 0)
						{
							sb.AppendLine(whitespace + "StatsReport_RelevantTraits".Translate());
							for (int j = 0; j < list.Count; j++)
							{
								Trait trait = list[j];
								string valueToStringAsOffset = trait.CurrentData.statOffsets.First((StatModifier se) => se.stat == this.stat).ValueToStringAsOffset;
								sb.AppendLine(string.Concat(new string[]
								{
									whitespace,
									"    ",
									trait.LabelCap,
									": ",
									valueToStringAsOffset
								}));
							}
							for (int k = 0; k < list2.Count; k++)
							{
								Trait trait2 = list2[k];
								string toStringAsFactor = trait2.CurrentData.statFactors.First((StatModifier se) => se.stat == this.stat).ToStringAsFactor;
								sb.AppendLine(string.Concat(new string[]
								{
									whitespace,
									"    ",
									trait2.LabelCap,
									": ",
									toStringAsFactor
								}));
							}
						}
					}
					if (StatWorker.RelevantGear(pawn, this.stat).Any<Thing>())
					{
						sb.AppendLine(whitespace + "StatsReport_RelevantGear".Translate());
						if (pawn.apparel != null)
						{
							for (int l = 0; l < pawn.apparel.WornApparel.Count; l++)
							{
								Apparel apparel = pawn.apparel.WornApparel[l];
								if (StatWorker.GearAffectsStat(apparel.def, this.stat))
								{
									sb.AppendLine(whitespace + StatWorker.InfoTextLineFromGear(apparel, this.stat));
								}
							}
						}
						Pawn_EquipmentTracker equipment = pawn.equipment;
						if (((equipment != null) ? equipment.Primary : null) != null && (StatWorker.GearAffectsStat(pawn.equipment.Primary.def, this.stat) || StatWorker.GearHasCompsThatAffectStat(pawn.equipment.Primary, this.stat)))
						{
							sb.AppendLine(whitespace + StatWorker.InfoTextLineFromGear(pawn.equipment.Primary, this.stat));
						}
					}
				}
				bool flag = false;
				List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
				for (int m = 0; m < hediffs.Count; m++)
				{
					HediffStage curStage = hediffs[m].CurStage;
					if (curStage != null)
					{
						float num = curStage.statOffsets.GetStatOffsetFromList(this.stat);
						if (num != 0f)
						{
							float val2 = num;
							if (curStage.statOffsetEffectMultiplier != null)
							{
								num *= pawn.GetStatValue(curStage.statOffsetEffectMultiplier, true, -1);
							}
							if (curStage.multiplyStatChangesBySeverity)
							{
								num *= hediffs[m].Severity;
							}
							if (!flag)
							{
								sb.AppendLine(whitespace + "StatsReport_RelevantHediffs".Translate());
								flag = true;
							}
							sb.Append(string.Concat(new string[]
							{
								whitespace,
								"    ",
								hediffs[m].LabelBaseCap,
								": ",
								this.ValueToString(num, false, ToStringNumberSense.Offset)
							}));
							if (curStage.statOffsetEffectMultiplier != null)
							{
								sb.Append(string.Concat(new string[]
								{
									whitespace,
									" (",
									this.ValueToString(val2, false, ToStringNumberSense.Offset),
									" x ",
									this.ValueToString(pawn.GetStatValue(curStage.statOffsetEffectMultiplier, true, -1), true, curStage.statOffsetEffectMultiplier.toStringNumberSense),
									" "
								}) + curStage.statOffsetEffectMultiplier.LabelCap + ")");
							}
							sb.AppendLine();
						}
						float num2 = curStage.statFactors.GetStatFactorFromList(this.stat);
						if (Math.Abs(num2 - 1f) > 1E-45f)
						{
							float val3 = num2;
							if (curStage.multiplyStatChangesBySeverity)
							{
								num2 = StatWorker.ScaleFactor(num2, hediffs[m].Severity);
							}
							if (curStage.statFactorEffectMultiplier != null)
							{
								num2 = StatWorker.ScaleFactor(num2, pawn.GetStatValue(curStage.statFactorEffectMultiplier, true, -1));
							}
							if (!flag)
							{
								sb.AppendLine(whitespace + "StatsReport_RelevantHediffs".Translate());
								flag = true;
							}
							sb.Append("    " + hediffs[m].LabelBaseCap + ": " + this.ValueToString(num2, false, ToStringNumberSense.Factor));
							if (curStage.statFactorEffectMultiplier != null)
							{
								sb.Append(string.Concat(new string[]
								{
									whitespace,
									" (",
									this.ValueToString(val3, false, ToStringNumberSense.Factor),
									" x ",
									this.ValueToString(pawn.GetStatValue(curStage.statFactorEffectMultiplier, true, -1), false, ToStringNumberSense.Absolute),
									" "
								}) + curStage.statFactorEffectMultiplier.LabelCap + ")");
							}
							sb.AppendLine();
						}
					}
				}
				if (pawn.Ideo != null)
				{
					List<Precept> preceptsListForReading = pawn.Ideo.PreceptsListForReading;
					for (int n = 0; n < preceptsListForReading.Count; n++)
					{
						float statOffsetFromList = preceptsListForReading[n].def.statOffsets.GetStatOffsetFromList(this.stat);
						if (statOffsetFromList != 0f)
						{
							sb.AppendLine(whitespace + "StatsReport_Ideoligion".Translate() + ": " + this.ValueToString(statOffsetFromList, false, ToStringNumberSense.Offset));
						}
						float statFactorFromList = preceptsListForReading[n].def.statFactors.GetStatFactorFromList(this.stat);
						if (Math.Abs(statFactorFromList - 1f) > 1E-45f)
						{
							sb.AppendLine(whitespace + "StatsReport_Ideoligion".Translate() + ": " + this.ValueToString(statFactorFromList, false, ToStringNumberSense.Factor));
						}
						if (preceptsListForReading[n].def.conditionalStatAffecters != null)
						{
							for (int num3 = 0; num3 < preceptsListForReading[n].def.conditionalStatAffecters.Count; num3++)
							{
								ConditionalStatAffecter conditionalStatAffecter = preceptsListForReading[n].def.conditionalStatAffecters[num3];
								if (conditionalStatAffecter.Applies(req))
								{
									float statOffsetFromList2 = conditionalStatAffecter.statOffsets.GetStatOffsetFromList(this.stat);
									if (statOffsetFromList2 != 0f)
									{
										sb.AppendLine(whitespace + "StatsReport_Ideoligion".Translate() + ": " + this.ValueToString(statOffsetFromList2, false, ToStringNumberSense.Offset));
									}
									float statFactorFromList2 = conditionalStatAffecter.statFactors.GetStatFactorFromList(this.stat);
									if (statFactorFromList2 != 1f)
									{
										sb.AppendLine(whitespace + "StatsReport_Ideoligion".Translate() + ": " + this.ValueToString(statFactorFromList2, false, ToStringNumberSense.Factor));
									}
								}
							}
						}
					}
					Precept_Role role = pawn.Ideo.GetRole(pawn);
					if (((role != null) ? role.def.roleEffects : null) != null)
					{
						foreach (RoleEffect roleEffect in role.def.roleEffects)
						{
							RoleEffect_PawnStatOffset roleEffect_PawnStatOffset = roleEffect as RoleEffect_PawnStatOffset;
							if (roleEffect_PawnStatOffset != null)
							{
								if (roleEffect_PawnStatOffset.statDef == this.stat)
								{
									sb.AppendLine(whitespace + role.LabelCap + ": " + this.ValueToString(roleEffect_PawnStatOffset.modifier, false, ToStringNumberSense.Offset));
								}
							}
							else
							{
								RoleEffect_PawnStatFactor roleEffect_PawnStatFactor = roleEffect as RoleEffect_PawnStatFactor;
								if (roleEffect_PawnStatFactor != null && roleEffect_PawnStatFactor.statDef == this.stat)
								{
									sb.AppendLine(whitespace + role.LabelCap + ": " + this.ValueToString(roleEffect_PawnStatFactor.modifier, false, ToStringNumberSense.Factor));
								}
							}
						}
					}
				}
				if (ModsConfig.BiotechActive && pawn.genes != null)
				{
					bool flag2 = false;
					List<Gene> genesListForReading = pawn.genes.GenesListForReading;
					for (int num4 = 0; num4 < genesListForReading.Count; num4++)
					{
						if (genesListForReading[num4].Active)
						{
							float statOffsetFromList3 = genesListForReading[num4].def.statOffsets.GetStatOffsetFromList(this.stat);
							if (statOffsetFromList3 != 0f)
							{
								if (!flag2)
								{
									sb.AppendLine(whitespace + "StatsReport_Genes".Translate());
									flag2 = true;
								}
								sb.AppendLine(string.Concat(new string[]
								{
									whitespace,
									"    ",
									genesListForReading[num4].LabelCap,
									": ",
									this.ValueToString(statOffsetFromList3, false, ToStringNumberSense.Offset)
								}));
							}
							float statFactorFromList3 = genesListForReading[num4].def.statFactors.GetStatFactorFromList(this.stat);
							if (statFactorFromList3 != 1f)
							{
								if (!flag2)
								{
									sb.AppendLine(whitespace + "StatsReport_Genes".Translate());
									flag2 = true;
								}
								sb.AppendLine(string.Concat(new string[]
								{
									whitespace,
									"    ",
									genesListForReading[num4].LabelCap,
									": ",
									this.ValueToString(statFactorFromList3, false, ToStringNumberSense.Factor)
								}));
							}
							if (genesListForReading[num4].def.conditionalStatAffecters != null)
							{
								for (int num5 = 0; num5 < genesListForReading[num4].def.conditionalStatAffecters.Count; num5++)
								{
									ConditionalStatAffecter conditionalStatAffecter2 = genesListForReading[num4].def.conditionalStatAffecters[num5];
									if (conditionalStatAffecter2.Applies(req))
									{
										float statOffsetFromList4 = conditionalStatAffecter2.statOffsets.GetStatOffsetFromList(this.stat);
										if (statOffsetFromList4 != 0f)
										{
											if (!flag2)
											{
												sb.AppendLine(whitespace + "StatsReport_Genes".Translate());
												flag2 = true;
											}
											sb.AppendLine(string.Concat(new string[]
											{
												whitespace,
												"    ",
												genesListForReading[num4].LabelCap,
												" (",
												conditionalStatAffecter2.Label,
												"): ",
												this.ValueToString(statOffsetFromList4, false, ToStringNumberSense.Offset)
											}));
										}
										float statFactorFromList4 = conditionalStatAffecter2.statFactors.GetStatFactorFromList(this.stat);
										if (statFactorFromList4 != 1f)
										{
											if (!flag2)
											{
												sb.AppendLine(whitespace + "StatsReport_Genes".Translate());
												flag2 = true;
											}
											sb.AppendLine(string.Concat(new string[]
											{
												whitespace,
												"    ",
												genesListForReading[num4].LabelCap,
												" (",
												conditionalStatAffecter2.Label,
												"): ",
												this.ValueToString(statFactorFromList4, false, ToStringNumberSense.Factor)
											}));
										}
									}
								}
							}
						}
					}
				}
				float statOffsetFromList5 = pawn.ageTracker.CurLifeStage.statOffsets.GetStatOffsetFromList(this.stat);
				if (statOffsetFromList5 != 0f)
				{
					sb.AppendLine(whitespace + "StatsReport_LifeStage".Translate() + " (" + pawn.ageTracker.CurLifeStage.label + "): " + statOffsetFromList5.ToStringByStyle(this.stat.toStringStyle, ToStringNumberSense.Offset));
				}
				float statFactorFromList5 = pawn.ageTracker.CurLifeStage.statFactors.GetStatFactorFromList(this.stat);
				if (statFactorFromList5 != 1f)
				{
					sb.AppendLine(whitespace + "StatsReport_LifeStage".Translate() + " (" + pawn.ageTracker.CurLifeStage.label + "): " + statFactorFromList5.ToStringByStyle(ToStringStyle.PercentZero, ToStringNumberSense.Factor));
				}
			}
			if (req.StuffDef != null)
			{
				if (baseValue > 0f || this.stat.applyFactorsIfNegative)
				{
					float statFactorFromList6 = req.StuffDef.stuffProps.statFactors.GetStatFactorFromList(this.stat);
					if (statFactorFromList6 != 1f)
					{
						sb.AppendLine(whitespace + "StatsReport_Material".Translate() + " (" + req.StuffDef.LabelCap + "): " + statFactorFromList6.ToStringByStyle(ToStringStyle.PercentZero, ToStringNumberSense.Factor));
					}
					FloatRange floatRange;
					if (req.StuffDef.stuffProps.statFactorsQuality != null && req.StuffDef.stuffProps.statFactorsQuality.TryGetStatFactorRangeFromList(this.stat, out floatRange))
					{
						string text4 = floatRange.min.ToStringByStyle(ToStringStyle.PercentZero, ToStringNumberSense.Factor);
						string text5 = floatRange.max.ToStringByStyle(ToStringStyle.PercentZero, ToStringNumberSense.Factor);
						sb.AppendLine(whitespace + string.Format("{0} ({1}): {2} ~ {3}", new object[]
						{
							"StatsReport_Material".Translate(),
							req.StuffDef.LabelCap,
							text4,
							text5
						}));
					}
				}
				float statOffsetFromList6 = req.StuffDef.stuffProps.statOffsets.GetStatOffsetFromList(this.stat);
				if (statOffsetFromList6 != 0f)
				{
					sb.AppendLine(whitespace + "StatsReport_Material".Translate() + " (" + req.StuffDef.LabelCap + "): " + statOffsetFromList6.ToStringByStyle(this.stat.toStringStyle, ToStringNumberSense.Offset));
				}
				FloatRange floatRange2;
				if (req.StuffDef.stuffProps.statOffsetsQuality != null && req.StuffDef.stuffProps.statOffsetsQuality.TryGetStatOffsetRangeFromList(this.stat, out floatRange2))
				{
					string text6 = floatRange2.min.ToStringByStyle(this.stat.toStringStyle, ToStringNumberSense.Offset);
					string text7 = floatRange2.max.ToStringByStyle(this.stat.toStringStyle, ToStringNumberSense.Offset);
					sb.AppendLine(whitespace + string.Format("{0} ({1}): {2} ~ {3}", new object[]
					{
						"StatsReport_Material".Translate(),
						req.StuffDef.LabelCap,
						text6,
						text7
					}));
				}
			}
			ThingWithComps thingWithComps = req.Thing as ThingWithComps;
			if (thingWithComps != null)
			{
				List<ThingComp> allComps = thingWithComps.AllComps;
				for (int num6 = 0; num6 < allComps.Count; num6++)
				{
					allComps[num6].GetStatsExplanation(this.stat, sb, whitespace);
				}
			}
			if (this.stat.statFactors != null)
			{
				sb.AppendLine(whitespace + this.stat.statFactorsExplanationHeader);
				for (int num7 = 0; num7 < this.stat.statFactors.Count; num7++)
				{
					StatDef statDef = this.stat.statFactors[num7];
					sb.AppendLine(whitespace + "    " + statDef.LabelCap + ": x" + statDef.Worker.GetValue(req, true).ToStringPercent());
				}
			}
			if (pawn != null)
			{
				if (pawn.skills != null)
				{
					if (this.stat.skillNeedFactors != null)
					{
						sb.AppendLine(whitespace + "StatsReport_Skills".Translate());
						for (int num8 = 0; num8 < this.stat.skillNeedFactors.Count; num8++)
						{
							SkillNeed skillNeed2 = this.stat.skillNeedFactors[num8];
							int level3 = pawn.skills.GetSkill(skillNeed2.skill).Level;
							sb.AppendLine(whitespace + "    " + skillNeed2.skill.LabelCap + " (" + level3.ToString() + "): x" + skillNeed2.ValueFor(pawn).ToStringPercent());
						}
					}
				}
				else if (this.stat.noSkillFactor != 1f)
				{
					sb.AppendLine(whitespace + "StatsReport_Skills".Translate());
					sb.AppendLine(whitespace + "    " + "default".Translate().CapitalizeFirst() + " : x" + this.stat.noSkillFactor.ToStringPercent());
				}
				if (this.stat.capacityFactors != null)
				{
					sb.AppendLine(whitespace + ("StatsReport_Health".CanTranslate() ? "StatsReport_Health".Translate() : "StatsReport_HealthFactors".Translate()));
					if (this.stat.capacityFactors != null)
					{
						foreach (PawnCapacityFactor pawnCapacityFactor in from hfa in this.stat.capacityFactors
						orderby hfa.capacity.listOrder
						select hfa)
						{
							string text8 = pawnCapacityFactor.capacity.GetLabelFor(pawn).CapitalizeFirst();
							string text9 = pawnCapacityFactor.GetFactor(pawn.health.capacities.GetLevel(pawnCapacityFactor.capacity)).ToStringPercent();
							string text10 = "HealthFactorPercentImpact".Translate(pawnCapacityFactor.weight.ToStringPercent());
							if (pawnCapacityFactor.max < 999f)
							{
								text10 += ", " + "HealthFactorMaxImpact".Translate(pawnCapacityFactor.max.ToStringPercent());
							}
							if (pawnCapacityFactor.allowedDefect != 0f)
							{
								text10 += ", " + "HealthFactorAllowedDefect".Translate((1f - pawnCapacityFactor.allowedDefect).ToStringPercent());
							}
							sb.AppendLine(string.Concat(new string[]
							{
								whitespace,
								"    ",
								text8,
								": x",
								text9,
								" (",
								text10,
								")"
							}));
						}
					}
				}
				if (pawn.Inspired)
				{
					float statOffsetFromList7 = pawn.InspirationDef.statOffsets.GetStatOffsetFromList(this.stat);
					if (statOffsetFromList7 != 0f)
					{
						sb.AppendLine(whitespace + "StatsReport_Inspiration".Translate(pawn.Inspiration.def.LabelCap) + ": " + this.ValueToString(statOffsetFromList7, false, ToStringNumberSense.Offset));
					}
					float statFactorFromList7 = pawn.InspirationDef.statFactors.GetStatFactorFromList(this.stat);
					if (statFactorFromList7 != 1f)
					{
						sb.AppendLine(whitespace + "StatsReport_Inspiration".Translate(pawn.Inspiration.def.LabelCap) + ": " + statFactorFromList7.ToStringByStyle(ToStringStyle.PercentZero, ToStringNumberSense.Factor));
					}
				}
			}
		}

		// Token: 0x06013980 RID: 80256 RVA: 0x005D0EE8 File Offset: 0x005CF0E8
		public virtual void FinalizeValue(StatRequest req, ref float val, bool applyPostProcess)
		{
			if (this.stat.parts != null)
			{
				for (int i = 0; i < this.stat.parts.Count; i++)
				{
					this.stat.parts[i].TransformValue(req, ref val);
				}
			}
			if (applyPostProcess && this.stat.postProcessCurve != null)
			{
				val = this.stat.postProcessCurve.Evaluate(val);
			}
			if (applyPostProcess && !this.stat.postProcessStatFactors.NullOrEmpty<StatDef>() && req.HasThing)
			{
				for (int j = 0; j < this.stat.postProcessStatFactors.Count; j++)
				{
					val *= req.Thing.GetStatValue(this.stat.postProcessStatFactors[j], true, -1);
				}
			}
			if (Find.Scenario != null)
			{
				val *= Find.Scenario.GetStatFactor(this.stat);
			}
			if (Mathf.Abs(val) > this.stat.roundToFiveOver)
			{
				val = Mathf.Round(val / 5f) * 5f;
			}
			if (this.stat.roundValue)
			{
				val = (float)Mathf.RoundToInt(val);
			}
			if (applyPostProcess)
			{
				val = Mathf.Clamp(val, this.stat.minValue, this.stat.maxValue);
			}
		}

		// Token: 0x06013981 RID: 80257 RVA: 0x005D1034 File Offset: 0x005CF234
		public virtual string GetExplanationFinalizePart(StatRequest req, ToStringNumberSense numberSense, float finalVal)
		{
			StringBuilder stringBuilder = new StringBuilder();
			this.GetAdditionalOffsetsAndFactorsExplanation(req, numberSense, stringBuilder, "");
			stringBuilder.Append("StatsReport_FinalValue".Translate() + ": " + this.stat.ValueToString(finalVal, this.stat.toStringNumberSense, true));
			if (this.stat.displayMaxWhenAboveOrEqual && finalVal >= this.stat.maxValue)
			{
				stringBuilder.AppendInNewLine("StatsReport_MaxValue".Translate() + ": " + this.stat.ValueToString(this.stat.maxValue, this.stat.toStringNumberSense, true));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06013982 RID: 80258 RVA: 0x005D10FC File Offset: 0x005CF2FC
		public void GetAdditionalOffsetsAndFactorsExplanation(StatRequest req, ToStringNumberSense numberSense, StringBuilder sb, string whitespace = "")
		{
			if (this.stat.parts != null)
			{
				for (int i = 0; i < this.stat.parts.Count; i++)
				{
					string text = this.stat.parts[i].ExplanationPart(req);
					if (!text.NullOrEmpty())
					{
						sb.AppendLine(whitespace + text);
					}
				}
			}
			if (this.stat.postProcessCurve != null)
			{
				float value = this.GetValue(req, false);
				float num = this.stat.postProcessCurve.Evaluate(value);
				if (!Mathf.Approximately(value, num))
				{
					string t = this.ValueToString(value, false, ToStringNumberSense.Absolute);
					string t2 = this.stat.ValueToString(num, numberSense, true);
					sb.AppendLine(whitespace + "StatsReport_PostProcessed".Translate() + ": " + t + " => " + t2);
				}
			}
			if (this.stat.postProcessStatFactors != null)
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int j = 0; j < this.stat.postProcessStatFactors.Count; j++)
				{
					if (this.stat.postProcessStatFactors[j].Worker.ShouldShowFor(req))
					{
						StatDef statDef = this.stat.postProcessStatFactors[j];
						stringBuilder.AppendLine(whitespace + string.Format("    {0}: x{1}", statDef.LabelCap, statDef.Worker.GetValue(req, true).ToStringPercent()));
					}
				}
				if (stringBuilder.Length > 0)
				{
					sb.AppendLine(whitespace + "StatsReport_OtherStats".Translate());
					sb.AppendLine(stringBuilder.ToString());
				}
			}
			float statFactor = Find.Scenario.GetStatFactor(this.stat);
			if (statFactor != 1f)
			{
				sb.AppendLine(whitespace + "StatsReport_ScenarioFactor".Translate() + ": " + statFactor.ToStringPercent());
			}
		}

		// Token: 0x06013983 RID: 80259 RVA: 0x005D1318 File Offset: 0x005CF518
		public string GetExplanationFull(StatRequest req, ToStringNumberSense numberSense, float value)
		{
			if (this.IsDisabledFor(req.Thing))
			{
				return "StatsReport_PermanentlyDisabled".Translate();
			}
			string text = this.stat.Worker.GetExplanationUnfinalized(req, numberSense).TrimEndNewlines();
			if (!text.NullOrEmpty())
			{
				text += "\n\n";
			}
			return text + this.stat.Worker.GetExplanationFinalizePart(req, numberSense, value);
		}

		// Token: 0x06013984 RID: 80260 RVA: 0x005D138C File Offset: 0x005CF58C
		public virtual bool ShouldShowFor(StatRequest req)
		{
			if (this.stat.alwaysHide)
			{
				return false;
			}
			Def def = req.Def;
			if (!this.stat.showIfUndefined && !req.StatBases.StatListContains(this.stat))
			{
				return false;
			}
			if (!this.stat.CanShowWithLoadedMods())
			{
				return false;
			}
			if (this.stat.hideInClassicMode && Find.IdeoManager.classicMode)
			{
				return false;
			}
			if (this.stat.parts != null)
			{
				using (List<StatPart>.Enumerator enumerator = this.stat.parts.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.ForceShow(req))
						{
							return true;
						}
					}
				}
			}
			Pawn pawn = req.Thing as Pawn;
			if (pawn != null)
			{
				if (pawn.health != null && !this.stat.showIfHediffsPresent.NullOrEmpty<HediffDef>())
				{
					for (int i = 0; i < this.stat.showIfHediffsPresent.Count; i++)
					{
						if (!pawn.health.hediffSet.HasHediff(this.stat.showIfHediffsPresent[i], false))
						{
							return false;
						}
					}
				}
				if (this.stat.showOnSlavesOnly && !pawn.IsSlave)
				{
					return false;
				}
			}
			if (this.stat == StatDefOf.MaxHitPoints && req.HasThing)
			{
				return false;
			}
			if (!this.stat.showOnUntradeables && !StatWorker.DisplayTradeStats(req))
			{
				return false;
			}
			ThingDef thingDef = def as ThingDef;
			if (thingDef != null)
			{
				if (thingDef.category == ThingCategory.Pawn)
				{
					if (!this.stat.showOnPawns)
					{
						return false;
					}
					if (!this.stat.showOnHumanlikes && thingDef.race.Humanlike)
					{
						return false;
					}
					if (!this.stat.showOnNonWildManHumanlikes && thingDef.race.Humanlike)
					{
						Pawn pawn2 = req.Thing as Pawn;
						if (pawn2 == null || !pawn2.IsWildMan())
						{
							return false;
						}
					}
					if (!this.stat.showOnAnimals && thingDef.race.Animal)
					{
						return false;
					}
					if (!this.stat.showOnEntities && thingDef.race.IsAnomalyEntity)
					{
						return false;
					}
					if (!this.stat.showOnMechanoids && thingDef.race.IsMechanoid)
					{
						return false;
					}
					if (!this.stat.showOnDrones && thingDef.race.IsDrone)
					{
						return false;
					}
					Pawn pawn3 = req.Thing as Pawn;
					if (pawn3 != null && !this.stat.showDevelopmentalStageFilter.Has(pawn3.DevelopmentalStage))
					{
						return false;
					}
				}
				if (!this.stat.showOnUnhaulables && !thingDef.EverHaulable && !thingDef.Minifiable)
				{
					return false;
				}
			}
			if (this.stat.category == StatCategoryDefOf.BasicsPawn || this.stat.category == StatCategoryDefOf.BasicsPawnImportant || this.stat.category == StatCategoryDefOf.PawnCombat || this.stat.category == StatCategoryDefOf.Animals || this.stat.category == StatCategoryDefOf.PawnResistances || this.stat.category == StatCategoryDefOf.PawnHealth || this.stat.category == StatCategoryDefOf.PawnFood || this.stat.category == StatCategoryDefOf.PawnPsyfocus)
			{
				return thingDef != null && thingDef.category == ThingCategory.Pawn;
			}
			if (this.stat.category == StatCategoryDefOf.PawnMisc || this.stat.category == StatCategoryDefOf.PawnSocial || this.stat.category == StatCategoryDefOf.PawnWork)
			{
				if (thingDef == null || thingDef.category != ThingCategory.Pawn)
				{
					return false;
				}
				Pawn pawn4 = req.Thing as Pawn;
				if (pawn4 != null)
				{
					if (pawn4.IsColonyMech && this.stat.showOnPlayerMechanoids)
					{
						return true;
					}
					if (this.stat.showOnPawnKind.NotNullAndContains(pawn4.kindDef))
					{
						return true;
					}
				}
				return thingDef.race.Humanlike;
			}
			else if (this.stat.category == StatCategoryDefOf.Building)
			{
				if (thingDef == null)
				{
					return false;
				}
				if (this.stat == StatDefOf.DoorOpenSpeed)
				{
					return thingDef.IsDoor;
				}
				return (this.stat.showOnNonWorkTables || thingDef.IsWorkTable) && (this.stat.showOnNonPowerPlants || thingDef.HasAssignableCompFrom(typeof(CompPowerPlant))) && thingDef.category == ThingCategory.Building;
			}
			else
			{
				if (this.stat.category == StatCategoryDefOf.Apparel)
				{
					return thingDef != null && (thingDef.IsApparel || thingDef.category == ThingCategory.Pawn);
				}
				if (this.stat.category == StatCategoryDefOf.Weapon)
				{
					return thingDef != null && (thingDef.IsMeleeWeapon || thingDef.IsRangedWeapon);
				}
				if (this.stat.category == StatCategoryDefOf.Weapon_Ranged)
				{
					return thingDef != null && thingDef.IsRangedWeapon;
				}
				if (this.stat.category == StatCategoryDefOf.Weapon_Melee)
				{
					return thingDef != null && thingDef.IsMeleeWeapon;
				}
				if (this.stat.category == StatCategoryDefOf.BasicsNonPawn || this.stat.category == StatCategoryDefOf.BasicsNonPawnImportant)
				{
					return (thingDef == null || thingDef.category != ThingCategory.Pawn) && !req.ForAbility;
				}
				if (this.stat.category == StatCategoryDefOf.Terrain)
				{
					return def is TerrainDef;
				}
				if (ModsConfig.AnomalyActive && this.stat.category == StatCategoryDefOf.PsychicRituals)
				{
					return false;
				}
				if (req.ForAbility)
				{
					return this.stat.category == StatCategoryDefOf.Ability;
				}
				if (this.stat.category.displayAllByDefault)
				{
					return true;
				}
				string str = "Unhandled case: ";
				StatDef statDef = this.stat;
				string str2 = (statDef != null) ? statDef.ToString() : null;
				string str3 = ", ";
				Def def2 = def;
				Log.Error(str + str2 + str3 + ((def2 != null) ? def2.ToString() : null));
				return false;
			}
		}

		// Token: 0x06013985 RID: 80261 RVA: 0x005D1958 File Offset: 0x005CFB58
		public virtual bool IsDisabledFor(Thing thing)
		{
			if (this.stat.neverDisabled)
			{
				return false;
			}
			if (this.stat.skillNeedFactors.NullOrEmpty<SkillNeed>() && this.stat.skillNeedOffsets.NullOrEmpty<SkillNeed>() && this.stat.disableIfSkillDisabled == null)
			{
				return false;
			}
			Pawn pawn = thing as Pawn;
			if (pawn != null && pawn.story != null)
			{
				if (this.stat.skillNeedFactors != null)
				{
					foreach (SkillNeed skillNeed in this.stat.skillNeedFactors)
					{
						if (skillNeed.required && pawn.skills.GetSkill(skillNeed.skill).TotallyDisabled)
						{
							return true;
						}
					}
				}
				if (this.stat.skillNeedOffsets != null)
				{
					foreach (SkillNeed skillNeed2 in this.stat.skillNeedOffsets)
					{
						if (skillNeed2.required && pawn.skills.GetSkill(skillNeed2.skill).TotallyDisabled)
						{
							return true;
						}
					}
				}
				if (this.stat.disableIfSkillDisabled != null && pawn.skills.GetSkill(this.stat.disableIfSkillDisabled).TotallyDisabled)
				{
					return true;
				}
				return false;
			}
			return false;
		}

		// Token: 0x06013986 RID: 80262 RVA: 0x005D1ADC File Offset: 0x005CFCDC
		public virtual string GetStatDrawEntryLabel(StatDef stat, float value, ToStringNumberSense numberSense, StatRequest optionalReq, bool finalized = true)
		{
			return stat.ValueToString(value, numberSense, finalized);
		}

		// Token: 0x06013987 RID: 80263 RVA: 0x005D1AE8 File Offset: 0x005CFCE8
		private static string InfoTextLineFromGear(Thing gear, StatDef stat)
		{
			float f = StatWorker.StatOffsetFromGear(gear, stat);
			return "    " + gear.LabelCap + ": " + f.ToStringByStyle(stat.finalizeEquippedStatOffset ? stat.toStringStyle : stat.ToStringStyleUnfinalized, ToStringNumberSense.Offset);
		}

		// Token: 0x06013988 RID: 80264 RVA: 0x005D1B30 File Offset: 0x005CFD30
		public static float StatOffsetFromGear(Thing gear, StatDef stat)
		{
			float num = gear.def.equippedStatOffsets.GetStatOffsetFromList(stat);
			CompBladelinkWeapon compBladelinkWeapon = gear.TryGetComp<CompBladelinkWeapon>();
			if (compBladelinkWeapon != null)
			{
				List<WeaponTraitDef> traitsListForReading = compBladelinkWeapon.TraitsListForReading;
				for (int i = 0; i < traitsListForReading.Count; i++)
				{
					num += traitsListForReading[i].equippedStatOffsets.GetStatOffsetFromList(stat);
				}
			}
			if (Math.Abs(num) > 1E-45f && !stat.parts.NullOrEmpty<StatPart>())
			{
				foreach (StatPart statPart in stat.parts)
				{
					statPart.TransformValue(StatRequest.For(gear), ref num);
				}
			}
			return num;
		}

		// Token: 0x06013989 RID: 80265 RVA: 0x005D1BF0 File Offset: 0x005CFDF0
		private static IEnumerable<Thing> RelevantGear(Pawn pawn, StatDef stat)
		{
			if (pawn.apparel != null)
			{
				foreach (Apparel apparel in pawn.apparel.WornApparel)
				{
					if (StatWorker.GearAffectsStat(apparel.def, stat))
					{
						yield return apparel;
					}
				}
				List<Apparel>.Enumerator enumerator = default(List<Apparel>.Enumerator);
			}
			if (pawn.equipment != null)
			{
				foreach (ThingWithComps thingWithComps in pawn.equipment.AllEquipmentListForReading)
				{
					if (StatWorker.GearAffectsStat(thingWithComps.def, stat) || StatWorker.GearHasCompsThatAffectStat(thingWithComps, stat))
					{
						yield return thingWithComps;
					}
				}
				List<ThingWithComps>.Enumerator enumerator2 = default(List<ThingWithComps>.Enumerator);
			}
			yield break;
			yield break;
		}

		// Token: 0x0601398A RID: 80266 RVA: 0x005D1C08 File Offset: 0x005CFE08
		private static bool GearAffectsStat(ThingDef gearDef, StatDef stat)
		{
			if (gearDef.equippedStatOffsets != null)
			{
				for (int i = 0; i < gearDef.equippedStatOffsets.Count; i++)
				{
					if (gearDef.equippedStatOffsets[i].stat == stat && gearDef.equippedStatOffsets[i].value != 0f)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0601398B RID: 80267 RVA: 0x005D1C64 File Offset: 0x005CFE64
		private static bool GearHasCompsThatAffectStat(Thing gear, StatDef stat)
		{
			CompBladelinkWeapon compBladelinkWeapon = gear.TryGetComp<CompBladelinkWeapon>();
			if (compBladelinkWeapon == null)
			{
				return false;
			}
			List<WeaponTraitDef> traitsListForReading = compBladelinkWeapon.TraitsListForReading;
			for (int i = 0; i < traitsListForReading.Count; i++)
			{
				if (!traitsListForReading[i].equippedStatOffsets.NullOrEmpty<StatModifier>())
				{
					for (int j = 0; j < traitsListForReading[i].equippedStatOffsets.Count; j++)
					{
						StatModifier statModifier = traitsListForReading[i].equippedStatOffsets[j];
						if (statModifier.stat == stat && statModifier.value != 0f)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x0601398C RID: 80268 RVA: 0x005D1CF4 File Offset: 0x005CFEF4
		public virtual float GetBaseValueFor(StatRequest request)
		{
			float result = this.stat.defaultBaseValue;
			if (request.StatBases != null)
			{
				for (int i = 0; i < request.StatBases.Count; i++)
				{
					if (request.StatBases[i].stat == this.stat)
					{
						result = request.StatBases[i].value;
						break;
					}
				}
			}
			return result;
		}

		// Token: 0x0601398D RID: 80269 RVA: 0x005D1D5D File Offset: 0x005CFF5D
		public virtual string ValueToStringFor(Thing thing)
		{
			return this.ValueToString(thing.GetStatValue(this.stat, true, -1), true, ToStringNumberSense.Absolute);
		}

		// Token: 0x0601398E RID: 80270 RVA: 0x005D1D78 File Offset: 0x005CFF78
		public virtual string ValueToString(float val, bool finalized, ToStringNumberSense numberSense = ToStringNumberSense.Absolute)
		{
			if (!finalized)
			{
				string text = val.ToStringByStyle(this.stat.ToStringStyleUnfinalized, numberSense);
				if (numberSense != ToStringNumberSense.Factor && !this.stat.formatStringUnfinalized.NullOrEmpty())
				{
					text = string.Format(this.stat.formatStringUnfinalized, text);
				}
				return text;
			}
			string text2 = val.ToStringByStyle(this.stat.toStringStyle, numberSense);
			if (numberSense != ToStringNumberSense.Factor && !this.stat.formatString.NullOrEmpty())
			{
				text2 = string.Format(this.stat.formatString, text2);
			}
			return text2;
		}

		// Token: 0x0601398F RID: 80271 RVA: 0x005D1E01 File Offset: 0x005D0001
		public virtual IEnumerable<Dialog_InfoCard.Hyperlink> GetInfoCardHyperlinks(StatRequest statRequest)
		{
			Thing thing = statRequest.Thing;
			Pawn pawn = thing as Pawn;
			if (pawn != null)
			{
				List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
				int num3;
				for (int i = 0; i < hediffs.Count; i = num3 + 1)
				{
					HediffStage curStage = hediffs[i].CurStage;
					if (curStage != null)
					{
						float num = curStage.statOffsets.GetStatOffsetFromList(this.stat);
						if (num != 0f && curStage.statOffsetEffectMultiplier != null)
						{
							num *= pawn.GetStatValue(curStage.statOffsetEffectMultiplier, true, -1);
						}
						float num2 = curStage.statFactors.GetStatFactorFromList(this.stat);
						if (Math.Abs(num2 - 1f) > 1E-45f && curStage.statFactorEffectMultiplier != null)
						{
							num2 = StatWorker.ScaleFactor(num2, pawn.GetStatValue(curStage.statFactorEffectMultiplier, true, -1));
						}
						if (Mathf.Abs(num) > 0f || Math.Abs(num2 - 1f) > 1E-45f)
						{
							yield return new Dialog_InfoCard.Hyperlink(hediffs[i].def, -1);
						}
					}
					num3 = i;
				}
				foreach (Thing thing2 in StatWorker.RelevantGear(pawn, this.stat))
				{
					yield return new Dialog_InfoCard.Hyperlink(thing2, -1, false);
				}
				IEnumerator<Thing> enumerator = null;
				if (this.stat.parts != null)
				{
					foreach (StatPart statPart in this.stat.parts)
					{
						foreach (Dialog_InfoCard.Hyperlink hyperlink in statPart.GetInfoCardHyperlinks(statRequest))
						{
							yield return hyperlink;
						}
						IEnumerator<Dialog_InfoCard.Hyperlink> enumerator3 = null;
					}
					List<StatPart>.Enumerator enumerator2 = default(List<StatPart>.Enumerator);
				}
				hediffs = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x06013990 RID: 80272 RVA: 0x005D1E18 File Offset: 0x005D0018
		public static float ScaleFactor(float factor, float scale)
		{
			return 1f - (1f - factor) * scale;
		}

		// Token: 0x06013991 RID: 80273 RVA: 0x005D1E2C File Offset: 0x005D002C
		private static bool DisplayTradeStats(StatRequest req)
		{
			ThingDef thingDef = req.Def as ThingDef;
			if (thingDef == null)
			{
				return false;
			}
			if (ModsConfig.BiotechActive && req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null && pawn.IsColonyMech)
				{
					return true;
				}
			}
			return (!req.HasThing || !CompBiocodable.IsBiocoded(req.Thing)) && ((thingDef.category == ThingCategory.Building && thingDef.Minifiable) || TradeUtility.EverPlayerSellable(thingDef) || (thingDef.tradeability.TraderCanSell() && (thingDef.category == ThingCategory.Item || thingDef.category == ThingCategory.Pawn)));
		}

		// Token: 0x06013992 RID: 80274 RVA: 0x005D1ECD File Offset: 0x005D00CD
		public void TryClearCache()
		{
			Dictionary<Thing, StatCacheEntry> dictionary = this.temporaryStatCache;
			if (dictionary != null)
			{
				dictionary.Clear();
			}
			ConcurrentDictionary<Thing, float> concurrentDictionary = this.immutableStatCache;
			if (concurrentDictionary == null)
			{
				return;
			}
			concurrentDictionary.Clear();
		}

		// Token: 0x06013993 RID: 80275 RVA: 0x005D1EF0 File Offset: 0x005D00F0
		public void ClearCacheForThing(Thing thing)
		{
			Dictionary<Thing, StatCacheEntry> dictionary = this.temporaryStatCache;
			if (dictionary != null)
			{
				dictionary.Remove(thing);
			}
			ConcurrentDictionary<Thing, float> concurrentDictionary = this.immutableStatCache;
			if (concurrentDictionary == null)
			{
				return;
			}
			float num;
			concurrentDictionary.Remove(thing, out num);
		}

		// Token: 0x06013994 RID: 80276 RVA: 0x005D1F24 File Offset: 0x005D0124
		public void DeleteStatCache()
		{
			this.temporaryStatCache = null;
			this.immutableStatCache = null;
		}

		// Token: 0x0400D91F RID: 55583
		public const int IGNORE_CACHE = -1;

		// Token: 0x0400D920 RID: 55584
		private Dictionary<Thing, StatCacheEntry> temporaryStatCache;

		// Token: 0x0400D921 RID: 55585
		private ConcurrentDictionary<Thing, float> immutableStatCache;

		// Token: 0x0400D922 RID: 55586
		protected StatDef stat;
	}
}
