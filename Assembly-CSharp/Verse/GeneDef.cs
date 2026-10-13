using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000175 RID: 373
	public class GeneDef : Def, IRenderNodePropertiesParent
	{
		// Token: 0x1700019B RID: 411
		// (get) Token: 0x06000ABC RID: 2748 RVA: 0x0003261C File Offset: 0x0003081C
		public Texture2D Icon
		{
			get
			{
				if (this.cachedIcon == null)
				{
					if (this.iconPath.NullOrEmpty())
					{
						this.cachedIcon = BaseContent.BadTex;
					}
					else
					{
						this.cachedIcon = (ContentFinder<Texture2D>.Get(this.iconPath, true) ?? BaseContent.BadTex);
					}
				}
				return this.cachedIcon;
			}
		}

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x06000ABD RID: 2749 RVA: 0x00032674 File Offset: 0x00030874
		public Color IconColor
		{
			get
			{
				if (this.iconColor != null)
				{
					return this.iconColor.Value;
				}
				if (this.skinColorBase != null)
				{
					return this.skinColorBase.Value;
				}
				if (this.skinColorOverride != null)
				{
					return this.skinColorOverride.Value;
				}
				if (this.hairColorOverride != null)
				{
					return this.hairColorOverride.Value;
				}
				return Color.white;
			}
		}

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x06000ABE RID: 2750 RVA: 0x000326EA File Offset: 0x000308EA
		public string LabelShortAdj
		{
			get
			{
				if (!this.labelShortAdj.NullOrEmpty())
				{
					return this.labelShortAdj;
				}
				return this.label;
			}
		}

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x06000ABF RID: 2751 RVA: 0x00032708 File Offset: 0x00030908
		public string DescriptionFull
		{
			get
			{
				string result;
				if ((result = this.cachedDescription) == null)
				{
					result = (this.cachedDescription = this.GetDescriptionFull());
				}
				return result;
			}
		}

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x06000AC0 RID: 2752 RVA: 0x0003272E File Offset: 0x0003092E
		public bool HasDefinedGraphicProperties
		{
			get
			{
				return !this.renderNodeProperties.NullOrEmpty<PawnRenderNodeProperties>();
			}
		}

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x06000AC1 RID: 2753 RVA: 0x0003273E File Offset: 0x0003093E
		public List<PawnRenderNodeProperties> RenderNodeProperties
		{
			get
			{
				return this.renderNodeProperties;
			}
		}

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x06000AC2 RID: 2754 RVA: 0x00032748 File Offset: 0x00030948
		public bool RandomChosen
		{
			get
			{
				return this.randomChosen || (this.biostatArc <= 0 && this.biostatCpx == 0 && this.biostatMet == 0 && (this.hairColorOverride != null || this.skinColorBase != null || this.skinColorOverride != null || this.bodyType != null || !this.forcedHeadTypes.NullOrEmpty<HeadTypeDef>() || this.hairTagFilter != null || this.beardTagFilter != null || this.HasDefinedGraphicProperties));
			}
		}

		// Token: 0x06000AC3 RID: 2755 RVA: 0x000327D4 File Offset: 0x000309D4
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			if (this.displayCategory == null)
			{
				this.displayCategory = GeneCategoryDefOf.Miscellaneous;
			}
			if (this.renderNodeProperties != null)
			{
				for (int i = 0; i < this.renderNodeProperties.Count; i++)
				{
					this.renderNodeProperties[i].ResolveReferencesRecursive();
				}
			}
		}

		// Token: 0x06000AC4 RID: 2756 RVA: 0x0003282C File Offset: 0x00030A2C
		public int AptitudeFor(SkillDef skill)
		{
			int num = 0;
			if (this.aptitudes.NullOrEmpty<Aptitude>())
			{
				return num;
			}
			for (int i = 0; i < this.aptitudes.Count; i++)
			{
				if (this.aptitudes[i].skill == skill)
				{
					num += this.aptitudes[i].level;
				}
			}
			return num;
		}

		// Token: 0x06000AC5 RID: 2757 RVA: 0x0003288C File Offset: 0x00030A8C
		private string GetDescriptionFull()
		{
			GeneDef.<>c__DisplayClass107_0 CS$<>8__locals1 = new GeneDef.<>c__DisplayClass107_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.sb = new StringBuilder();
			if (!this.description.NullOrEmpty())
			{
				CS$<>8__locals1.sb.Append(this.description).AppendLine().AppendLine();
			}
			bool flag = false;
			if (this.prerequisite != null)
			{
				CS$<>8__locals1.sb.AppendLine("Requires".Translate() + ": " + this.prerequisite.LabelCap);
				flag = true;
			}
			if (this.minAgeActive > 0f)
			{
				CS$<>8__locals1.sb.AppendLine("TakesEffectAfterAge".Translate() + ": " + this.minAgeActive.ToString());
				flag = true;
			}
			if (flag)
			{
				CS$<>8__locals1.sb.AppendLine();
			}
			bool flag2 = false;
			if (this.biostatCpx != 0)
			{
				CS$<>8__locals1.sb.AppendLineTagged("Complexity".Translate().Colorize(GeneUtility.GCXColor) + ": " + this.biostatCpx.ToStringWithSign());
				flag2 = true;
			}
			if (this.biostatMet != 0)
			{
				CS$<>8__locals1.sb.AppendLineTagged("Metabolism".Translate().CapitalizeFirst().Colorize(GeneUtility.METColor) + ": " + this.biostatMet.ToStringWithSign());
				flag2 = true;
			}
			if (this.biostatArc != 0)
			{
				CS$<>8__locals1.sb.AppendLineTagged("ArchitesRequired".Translate().Colorize(GeneUtility.ARCColor) + ": " + this.biostatArc.ToStringWithSign());
				flag2 = true;
			}
			if (flag2)
			{
				CS$<>8__locals1.sb.AppendLine();
			}
			if (this.forcedTraits != null)
			{
				CS$<>8__locals1.sb.AppendLineTagged(("ForcedTraits".Translate() + ":").Colorize(ColoredText.TipSectionTitleColor));
				CS$<>8__locals1.sb.Append((from x in this.forcedTraits
				select x.def.DataAtDegree(x.degree).label).ToLineList("  - ", true)).AppendLine().AppendLine();
			}
			if (this.suppressedTraits != null)
			{
				CS$<>8__locals1.sb.AppendLineTagged(("SuppressedTraits".Translate() + ":").Colorize(ColoredText.TipSectionTitleColor));
				CS$<>8__locals1.sb.Append((from x in this.suppressedTraits
				select x.def.DataAtDegree(x.degree).label).ToLineList("  - ", true)).AppendLine().AppendLine();
			}
			if (this.aptitudes != null)
			{
				CS$<>8__locals1.sb.AppendLineTagged(("Aptitudes".Translate().CapitalizeFirst() + ":").Colorize(ColoredText.TipSectionTitleColor));
				CS$<>8__locals1.sb.Append((from x in this.aptitudes
				select x.skill.LabelCap.ToString() + " " + x.level.ToStringWithSign()).ToLineList("  - ", true)).AppendLine().AppendLine();
			}
			CS$<>8__locals1.effectsTitleWritten = false;
			if (this.passionMod != null)
			{
				PassionMod.PassionModType modType = this.passionMod.modType;
				if (modType != PassionMod.PassionModType.AddOneLevel)
				{
					if (modType == PassionMod.PassionModType.DropAll)
					{
						CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3("PassionModDrop".Translate(this.passionMod.skill));
					}
				}
				else
				{
					CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3("PassionModAdd".Translate(this.passionMod.skill));
				}
			}
			if (!this.statFactors.NullOrEmpty<StatModifier>())
			{
				for (int i = 0; i < this.statFactors.Count; i++)
				{
					StatModifier statModifier = this.statFactors[i];
					if (statModifier.stat.CanShowWithLoadedMods())
					{
						CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3(statModifier.stat.LabelCap + " " + statModifier.ToStringAsFactor);
					}
				}
			}
			if (!this.conditionalStatAffecters.NullOrEmpty<ConditionalStatAffecter>())
			{
				for (int j = 0; j < this.conditionalStatAffecters.Count; j++)
				{
					if (!this.conditionalStatAffecters[j].statFactors.NullOrEmpty<StatModifier>())
					{
						for (int k = 0; k < this.conditionalStatAffecters[j].statFactors.Count; k++)
						{
							StatModifier statModifier2 = this.conditionalStatAffecters[j].statFactors[k];
							if (statModifier2.stat.CanShowWithLoadedMods())
							{
								CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3(statModifier2.stat.LabelCap + " " + statModifier2.ToStringAsFactor + " (" + this.conditionalStatAffecters[j].Label + ")");
							}
						}
					}
				}
			}
			if (!this.statOffsets.NullOrEmpty<StatModifier>())
			{
				for (int l = 0; l < this.statOffsets.Count; l++)
				{
					StatModifier statModifier3 = this.statOffsets[l];
					if (statModifier3.stat.CanShowWithLoadedMods())
					{
						CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3(statModifier3.stat.LabelCap + " " + statModifier3.ValueToStringAsOffset);
					}
				}
			}
			if (!this.conditionalStatAffecters.NullOrEmpty<ConditionalStatAffecter>())
			{
				for (int m = 0; m < this.conditionalStatAffecters.Count; m++)
				{
					if (!this.conditionalStatAffecters[m].statOffsets.NullOrEmpty<StatModifier>())
					{
						for (int n = 0; n < this.conditionalStatAffecters[m].statOffsets.Count; n++)
						{
							StatModifier statModifier4 = this.conditionalStatAffecters[m].statOffsets[n];
							if (statModifier4.stat.CanShowWithLoadedMods())
							{
								CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3(statModifier4.stat.LabelCap + " " + statModifier4.ValueToStringAsOffset + " (" + this.conditionalStatAffecters[m].Label.UncapitalizeFirst() + ")");
							}
						}
					}
				}
			}
			if (!this.capMods.NullOrEmpty<PawnCapacityModifier>())
			{
				for (int num = 0; num < this.capMods.Count; num++)
				{
					PawnCapacityModifier pawnCapacityModifier = this.capMods[num];
					if (pawnCapacityModifier.offset != 0f)
					{
						CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3(pawnCapacityModifier.capacity.GetLabelFor().CapitalizeFirst() + " " + (pawnCapacityModifier.offset * 100f).ToString("+#;-#") + "%");
					}
					if (pawnCapacityModifier.postFactor != 1f)
					{
						CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3(pawnCapacityModifier.capacity.GetLabelFor().CapitalizeFirst() + " x" + pawnCapacityModifier.postFactor.ToStringPercent());
					}
					if (pawnCapacityModifier.setMax != 999f)
					{
						CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3(pawnCapacityModifier.capacity.GetLabelFor().CapitalizeFirst() + " " + "max".Translate().CapitalizeFirst() + ": " + pawnCapacityModifier.setMax.ToStringPercent());
					}
				}
			}
			if (!this.customEffectDescriptions.NullOrEmpty<string>())
			{
				foreach (string str in this.customEffectDescriptions)
				{
					CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3(str.ResolveTags());
				}
			}
			if (!this.damageFactors.NullOrEmpty<DamageFactor>())
			{
				for (int num2 = 0; num2 < this.damageFactors.Count; num2++)
				{
					CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3("DamageType".Translate(this.damageFactors[num2].damageDef.label).CapitalizeFirst() + " x" + this.damageFactors[num2].factor.ToStringPercent());
				}
			}
			if (this.resourceLossPerDay != 0f && !this.resourceLabel.NullOrEmpty())
			{
				CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3("ResourceLossPerDay".Translate(this.resourceLabel.Named("RESOURCE"), (-Mathf.RoundToInt(this.resourceLossPerDay * 100f)).ToStringWithSign().Named("OFFSET")).CapitalizeFirst());
			}
			if (!Mathf.Approximately(this.painFactor, 1f))
			{
				CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3("Pain".Translate() + " x" + this.painFactor.ToStringPercent());
			}
			if (this.painOffset != 0f)
			{
				CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3("Pain".Translate() + " " + (this.painOffset * 100f).ToString("+###0;-###0") + "%");
			}
			if (this.chemical != null)
			{
				if (!Mathf.Approximately(this.addictionChanceFactor, 1f))
				{
					if (this.addictionChanceFactor <= 0f)
					{
						CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3("AddictionImmune".Translate(this.chemical).CapitalizeFirst());
					}
					else
					{
						CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3("AddictionChanceFactor".Translate(this.chemical).CapitalizeFirst() + " x" + this.addictionChanceFactor.ToStringPercent());
					}
				}
				if (this.overdoseChanceFactor != 1f)
				{
					CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3("OverdoseChanceFactor".Translate(this.chemical).CapitalizeFirst() + " x" + this.overdoseChanceFactor.ToStringPercent());
				}
				if (this.toleranceBuildupFactor != 1f)
				{
					CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3("ToleranceBuildupFactor".Translate(this.chemical).CapitalizeFirst() + " x" + this.toleranceBuildupFactor.ToStringPercent());
				}
			}
			if (!this.enablesNeeds.NullOrEmpty<NeedDef>())
			{
				if (this.enablesNeeds.Count == 1)
				{
					CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3(string.Format("{0}: {1}", "AddsNeed".Translate(), this.enablesNeeds[0].LabelCap));
				}
				else
				{
					CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3(string.Format("{0}: {1}", "AddsNeeds".Translate(), (from x in this.enablesNeeds
					select x.label).ToCommaList(false, false).CapitalizeFirst()));
				}
			}
			if (!this.disablesNeeds.NullOrEmpty<NeedDef>())
			{
				if (this.disablesNeeds.Count == 1)
				{
					CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3(string.Format("{0}: {1}", "DisablesNeed".Translate(), this.disablesNeeds[0].LabelCap));
				}
				else
				{
					CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3(string.Format("{0}: {1}", "DisablesNeeds".Translate(), (from x in this.disablesNeeds
					select x.label).ToCommaList(false, false).CapitalizeFirst()));
				}
			}
			if (this.missingGeneRomanceChanceFactor != 1f)
			{
				CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3("MissingGeneRomanceChance".Translate(this.label.Named("GENE")) + " x" + this.missingGeneRomanceChanceFactor.ToStringPercent());
			}
			if (this.ignoreDarkness)
			{
				CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3("UnaffectedByDarkness".Translate());
			}
			if (this.foodPoisoningChanceFactor != 1f)
			{
				if (this.foodPoisoningChanceFactor <= 0f)
				{
					CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3("FoodPoisoningImmune".Translate());
				}
				else
				{
					CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3("Stat_Hediff_FoodPoisoningChanceFactor_Name".Translate() + " x" + this.foodPoisoningChanceFactor.ToStringPercent());
				}
			}
			if (this.socialFightChanceFactor != 1f)
			{
				if (this.socialFightChanceFactor <= 0f)
				{
					CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3("WillNeverSocialFight".Translate());
				}
				else
				{
					CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3("SocialFightChanceFactor".Translate() + " x" + this.socialFightChanceFactor.ToStringPercent());
				}
			}
			if (this.aggroMentalBreakSelectionChanceFactor != 1f)
			{
				if (this.aggroMentalBreakSelectionChanceFactor >= 999f)
				{
					CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3("AlwaysAggroMentalBreak".Translate());
				}
				else if (this.aggroMentalBreakSelectionChanceFactor <= 0f)
				{
					CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3("NeverAggroMentalBreak".Translate());
				}
				else
				{
					CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3("AggroMentalBreakSelectionChanceFactor".Translate() + " x" + this.aggroMentalBreakSelectionChanceFactor.ToStringPercent());
				}
			}
			if (this.prisonBreakMTBFactor != 1f)
			{
				if (this.prisonBreakMTBFactor < 0f)
				{
					CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3("WillNeverPrisonBreak".Translate());
				}
				else
				{
					CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3("PrisonBreakIntervalFactor".Translate() + " x" + this.prisonBreakMTBFactor.ToStringPercent());
				}
			}
			bool flag3 = CS$<>8__locals1.effectsTitleWritten;
			if (!this.makeImmuneTo.NullOrEmpty<HediffDef>())
			{
				if (flag3)
				{
					CS$<>8__locals1.sb.AppendLine();
				}
				CS$<>8__locals1.sb.AppendLineTagged(("ImmuneTo".Translate() + ":").Colorize(ColoredText.TipSectionTitleColor));
				CS$<>8__locals1.sb.AppendLine((from x in this.makeImmuneTo
				select x.label).ToLineList("  - ", true));
				flag3 = true;
			}
			if (!this.hediffGiversCannotGive.NullOrEmpty<HediffDef>())
			{
				if (flag3)
				{
					CS$<>8__locals1.sb.AppendLine();
				}
				CS$<>8__locals1.sb.AppendLineTagged(("ImmuneTo".Translate() + ":").Colorize(ColoredText.TipSectionTitleColor));
				CS$<>8__locals1.sb.AppendLine((from x in this.hediffGiversCannotGive
				select x.label).ToLineList("  - ", true));
				flag3 = true;
			}
			if (this.biologicalAgeTickFactorFromAgeCurve != null)
			{
				if (flag3)
				{
					CS$<>8__locals1.sb.AppendLine();
				}
				CS$<>8__locals1.sb.AppendLineTagged(("AgeFactors".Translate().CapitalizeFirst() + ":").Colorize(ColoredText.TipSectionTitleColor));
				CS$<>8__locals1.sb.AppendLine((from p in this.biologicalAgeTickFactorFromAgeCurve
				select "PeriodYears".Translate(p.x).ToString() + ": x" + p.y.ToStringPercent()).ToLineList("  - ", true));
				flag3 = true;
			}
			if (this.disabledWorkTags != WorkTags.None)
			{
				if (flag3)
				{
					CS$<>8__locals1.sb.AppendLine();
				}
				IEnumerable<WorkTypeDef> source = from x in DefDatabase<WorkTypeDef>.AllDefsListForReading
				where (CS$<>8__locals1.<>4__this.disabledWorkTags & x.workTags) > WorkTags.None
				select x;
				CS$<>8__locals1.sb.AppendLineTagged(("DisabledWorkLabel".Translate().CapitalizeFirst() + ":").Colorize(ColoredText.TipSectionTitleColor));
				CS$<>8__locals1.sb.AppendLine("  - " + (from x in source
				select x.labelShort).ToCommaList(false, false).CapitalizeFirst());
				if (this.disabledWorkTags.ExactlyOneWorkTagSet())
				{
					CS$<>8__locals1.sb.AppendLine("  - " + this.disabledWorkTags.LabelTranslated().CapitalizeFirst());
				}
				flag3 = true;
			}
			if (!this.abilities.NullOrEmpty<AbilityDef>())
			{
				if (flag3)
				{
					CS$<>8__locals1.sb.AppendLine();
				}
				if (this.abilities.Count == 1)
				{
					CS$<>8__locals1.sb.AppendLineTagged(("GivesAbility".Translate().CapitalizeFirst() + ":").Colorize(ColoredText.TipSectionTitleColor));
				}
				else
				{
					CS$<>8__locals1.sb.AppendLineTagged(("GivesAbilities".Translate().CapitalizeFirst() + ":").Colorize(ColoredText.TipSectionTitleColor));
				}
				CS$<>8__locals1.sb.AppendLine((from x in this.abilities
				select x.label).ToLineList("  - ", true));
				flag3 = true;
			}
			IEnumerable<ThoughtDef> enumerable = DefDatabase<ThoughtDef>.AllDefs.Where(delegate(ThoughtDef x)
			{
				if ((x.requiredGenes.NotNullAndContains(CS$<>8__locals1.<>4__this) || x.nullifyingGenes.NotNullAndContains(CS$<>8__locals1.<>4__this)) && x.stages != null)
				{
					return x.stages.Any((ThoughtStage y) => y.baseMoodEffect != 0f);
				}
				return false;
			});
			if (enumerable.Any<ThoughtDef>())
			{
				if (flag3)
				{
					CS$<>8__locals1.sb.AppendLine();
				}
				CS$<>8__locals1.sb.AppendLineTagged(("Mood".Translate().CapitalizeFirst() + ":").Colorize(ColoredText.TipSectionTitleColor));
				foreach (ThoughtDef thoughtDef in enumerable)
				{
					ThoughtStage thoughtStage = thoughtDef.stages.FirstOrDefault((ThoughtStage x) => x.baseMoodEffect != 0f);
					if (thoughtStage != null)
					{
						string text = thoughtStage.LabelCap + ": " + thoughtStage.baseMoodEffect.ToStringWithSign("0.##");
						if (thoughtDef.requiredGenes.NotNullAndContains(this))
						{
							CS$<>8__locals1.sb.AppendLine("  - " + text);
						}
						else if (thoughtDef.nullifyingGenes.NotNullAndContains(this))
						{
							CS$<>8__locals1.sb.AppendLine("  - " + "Removes".Translate() + ": " + text);
						}
					}
				}
			}
			return CS$<>8__locals1.sb.ToString().TrimEndNewlines();
		}

		// Token: 0x06000AC6 RID: 2758 RVA: 0x00033BE4 File Offset: 0x00031DE4
		public bool ConflictsWith(GeneDef other)
		{
			if (this == other)
			{
				return true;
			}
			if (this.exclusionTags != null && other.exclusionTags != null)
			{
				for (int i = 0; i < this.exclusionTags.Count; i++)
				{
					if (other.exclusionTags.Contains(this.exclusionTags[i]))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000AC7 RID: 2759 RVA: 0x00033C39 File Offset: 0x00031E39
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			foreach (StatDrawEntry statDrawEntry in base.SpecialDisplayStats(req))
			{
				yield return statDrawEntry;
			}
			IEnumerator<StatDrawEntry> enumerator = null;
			if (this.painFactor != 1f)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.CapacityEffects, "Pain".Translate(), "x" + this.painFactor.ToStringPercent(), "Stat_Hediff_Pain_Desc".Translate(), 4050, null, null, false, false);
			}
			if (this.painOffset != 0f)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.CapacityEffects, "Pain".Translate(), (this.painOffset * 100f).ToString("+###0;-###0") + "%", "Stat_Hediff_Pain_Desc".Translate(), 4050, null, null, false, false);
			}
			if (this.missingGeneRomanceChanceFactor != 1f)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.CapacityEffects, "MissingGeneRomanceChance".Translate(this.LabelCap.Named("GENE")), "x" + this.missingGeneRomanceChanceFactor.ToStringPercent(), "StatsReport_MissingGeneRomanceChance".Translate(), 4050, null, null, false, false);
			}
			if (this.forcedTraits != null)
			{
				string text = (from x in this.forcedTraits
				select x.def.DataAtDegree(x.degree).label).ToLineList(null, true);
				yield return new StatDrawEntry(StatCategoryDefOf.CapacityEffects, "ForcedTraits".Translate(), text, "ForcedTraitsDesc".Translate() + "\n\n" + text, 4080, null, null, false, false);
			}
			if (this.aptitudes != null)
			{
				string text2 = (from x in this.aptitudes
				select x.skill.LabelCap.ToString() + " " + x.level.ToStringWithSign()).ToLineList(null, true);
				yield return new StatDrawEntry(StatCategoryDefOf.CapacityEffects, "Aptitudes".Translate().CapitalizeFirst(), text2, "AptitudesDesc".Translate() + "\n\n" + text2, 4090, null, null, false, false);
			}
			if (this.statOffsets != null)
			{
				int num;
				for (int i = 0; i < this.statOffsets.Count; i = num + 1)
				{
					StatModifier statModifier = this.statOffsets[i];
					if (statModifier.stat.CanShowWithLoadedMods())
					{
						yield return new StatDrawEntry(StatCategoryDefOf.CapacityEffects, statModifier.stat.LabelCap, statModifier.ValueToStringAsOffset, statModifier.stat.description, 4070, null, null, false, false);
					}
					num = i;
				}
			}
			if (this.statFactors != null)
			{
				int num;
				for (int i = 0; i < this.statFactors.Count; i = num + 1)
				{
					StatModifier statModifier2 = this.statFactors[i];
					if (statModifier2.stat.CanShowWithLoadedMods())
					{
						yield return new StatDrawEntry(StatCategoryDefOf.CapacityEffects, statModifier2.stat.LabelCap, statModifier2.ToStringAsFactor, statModifier2.stat.description, 4070, null, null, false, false);
					}
					num = i;
				}
			}
			if (this.capMods != null)
			{
				int num;
				for (int i = 0; i < this.capMods.Count; i = num + 1)
				{
					PawnCapacityModifier capMod = this.capMods[i];
					if (capMod.offset != 0f)
					{
						yield return new StatDrawEntry(StatCategoryDefOf.CapacityEffects, capMod.capacity.GetLabelFor().CapitalizeFirst(), (capMod.offset * 100f).ToString("+#;-#") + "%", capMod.capacity.description, 4060, null, null, false, false);
					}
					if (capMod.postFactor != 1f)
					{
						yield return new StatDrawEntry(StatCategoryDefOf.CapacityEffects, capMod.capacity.GetLabelFor().CapitalizeFirst(), "x" + capMod.postFactor.ToStringPercent(), capMod.capacity.description, 4060, null, null, false, false);
					}
					if (capMod.SetMaxDefined && req.Pawn != null)
					{
						yield return new StatDrawEntry(StatCategoryDefOf.CapacityEffects, capMod.capacity.GetLabelFor().CapitalizeFirst(), "max".Translate().CapitalizeFirst() + " " + capMod.EvaluateSetMax(req.Pawn).ToStringPercent(), capMod.capacity.description, 4060, null, null, false, false);
					}
					capMod = null;
					num = i;
				}
			}
			if (this.biostatCpx != 0)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Genetics, "Complexity".Translate().CapitalizeFirst(), this.biostatCpx.ToString(), "ComplexityDesc".Translate(), 998, null, null, false, false);
			}
			if (this.biostatMet != 0)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Genetics, "Metabolism".Translate().CapitalizeFirst(), this.biostatMet.ToString(), "MetabolismDesc".Translate(), 997, null, null, false, false);
			}
			if (this.biostatArc != 0)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Genetics, "ArchitesRequired".Translate().CapitalizeFirst(), this.biostatArc.ToString(), "ArchitesRequiredDesc".Translate(), 995, null, null, false, false);
			}
			yield break;
			yield break;
		}

		// Token: 0x06000AC8 RID: 2760 RVA: 0x00033C50 File Offset: 0x00031E50
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.geneClass == null)
			{
				yield return "geneClass is null";
			}
			if (!typeof(Gene).IsAssignableFrom(this.geneClass))
			{
				yield return "geneClass is not Gene or subclass thereof";
			}
			if (this.mentalBreakMtbDays > 0f && this.mentalBreakDef == null)
			{
				yield return "mentalBreakMtbDays is >0 with null mentalBreakDef";
			}
			if (this.HasDefinedGraphicProperties)
			{
				foreach (PawnRenderNodeProperties pawnRenderNodeProperties in this.renderNodeProperties)
				{
					foreach (string text in pawnRenderNodeProperties.ConfigErrors())
					{
						yield return text;
					}
					IEnumerator<string> enumerator2 = null;
				}
				List<PawnRenderNodeProperties>.Enumerator enumerator = default(List<PawnRenderNodeProperties>.Enumerator);
			}
			yield break;
			yield break;
		}

		// Token: 0x040005A9 RID: 1449
		public Type geneClass = typeof(Gene);

		// Token: 0x040005AA RID: 1450
		[MustTranslate]
		public string labelShortAdj;

		// Token: 0x040005AB RID: 1451
		[MustTranslate]
		public List<string> customEffectDescriptions;

		// Token: 0x040005AC RID: 1452
		[NoTranslate]
		public string iconPath;

		// Token: 0x040005AD RID: 1453
		private Color? iconColor;

		// Token: 0x040005AE RID: 1454
		public GeneCategoryDef displayCategory;

		// Token: 0x040005AF RID: 1455
		public float displayOrderInCategory;

		// Token: 0x040005B0 RID: 1456
		public List<PawnRenderNodeProperties> renderNodeProperties;

		// Token: 0x040005B1 RID: 1457
		public bool neverGrayHair;

		// Token: 0x040005B2 RID: 1458
		public bool skinIsHairColor;

		// Token: 0x040005B3 RID: 1459
		public bool tattoosVisible = true;

		// Token: 0x040005B4 RID: 1460
		public FurDef fur;

		// Token: 0x040005B5 RID: 1461
		public SoundDef soundCall;

		// Token: 0x040005B6 RID: 1462
		public SoundDef soundDeath;

		// Token: 0x040005B7 RID: 1463
		public SoundDef soundWounded;

		// Token: 0x040005B8 RID: 1464
		public Type resourceGizmoType = typeof(GeneGizmo_Resource);

		// Token: 0x040005B9 RID: 1465
		public float resourceLossPerDay;

		// Token: 0x040005BA RID: 1466
		[MustTranslate]
		public string resourceLabel;

		// Token: 0x040005BB RID: 1467
		[MustTranslate]
		public string resourceDescription;

		// Token: 0x040005BC RID: 1468
		public List<float> resourceGizmoThresholds;

		// Token: 0x040005BD RID: 1469
		public bool showGizmoOnWorldView;

		// Token: 0x040005BE RID: 1470
		public bool showGizmoWhenDrafted;

		// Token: 0x040005BF RID: 1471
		public bool showGizmoOnMultiSelect;

		// Token: 0x040005C0 RID: 1472
		public List<AbilityDef> abilities;

		// Token: 0x040005C1 RID: 1473
		public List<GeneticTraitData> forcedTraits;

		// Token: 0x040005C2 RID: 1474
		public List<GeneticTraitData> suppressedTraits;

		// Token: 0x040005C3 RID: 1475
		public List<NeedDef> enablesNeeds;

		// Token: 0x040005C4 RID: 1476
		public List<NeedDef> disablesNeeds;

		// Token: 0x040005C5 RID: 1477
		public WorkTags disabledWorkTags;

		// Token: 0x040005C6 RID: 1478
		public bool ignoreDarkness;

		// Token: 0x040005C7 RID: 1479
		public EndogeneCategory endogeneCategory;

		// Token: 0x040005C8 RID: 1480
		public bool dislikesSunlight;

		// Token: 0x040005C9 RID: 1481
		public float minAgeActive;

		// Token: 0x040005CA RID: 1482
		public float lovinMTBFactor = 1f;

		// Token: 0x040005CB RID: 1483
		public bool immuneToToxGasExposure;

		// Token: 0x040005CC RID: 1484
		public bool immuneToVacuumBurns;

		// Token: 0x040005CD RID: 1485
		public bool randomChosen;

		// Token: 0x040005CE RID: 1486
		public int? waterCellCost;

		// Token: 0x040005CF RID: 1487
		public HistoryEventDef deathHistoryEvent;

		// Token: 0x040005D0 RID: 1488
		public List<Aptitude> aptitudes;

		// Token: 0x040005D1 RID: 1489
		public PassionMod passionMod;

		// Token: 0x040005D2 RID: 1490
		public List<StatModifier> statOffsets;

		// Token: 0x040005D3 RID: 1491
		public List<StatModifier> statFactors;

		// Token: 0x040005D4 RID: 1492
		public List<ConditionalStatAffecter> conditionalStatAffecters;

		// Token: 0x040005D5 RID: 1493
		public float painOffset;

		// Token: 0x040005D6 RID: 1494
		public float painFactor = 1f;

		// Token: 0x040005D7 RID: 1495
		public float foodPoisoningChanceFactor = 1f;

		// Token: 0x040005D8 RID: 1496
		public List<DamageFactor> damageFactors;

		// Token: 0x040005D9 RID: 1497
		public SimpleCurve biologicalAgeTickFactorFromAgeCurve;

		// Token: 0x040005DA RID: 1498
		public List<HediffDef> makeImmuneTo;

		// Token: 0x040005DB RID: 1499
		public List<HediffDef> hediffGiversCannotGive;

		// Token: 0x040005DC RID: 1500
		public ChemicalDef chemical;

		// Token: 0x040005DD RID: 1501
		public float addictionChanceFactor = 1f;

		// Token: 0x040005DE RID: 1502
		public float overdoseChanceFactor = 1f;

		// Token: 0x040005DF RID: 1503
		public float toleranceBuildupFactor = 1f;

		// Token: 0x040005E0 RID: 1504
		public bool sterilize;

		// Token: 0x040005E1 RID: 1505
		public List<PawnCapacityModifier> capMods;

		// Token: 0x040005E2 RID: 1506
		public bool preventPermanentWounds;

		// Token: 0x040005E3 RID: 1507
		public bool dontMindRawFood;

		// Token: 0x040005E4 RID: 1508
		public Color? hairColorOverride;

		// Token: 0x040005E5 RID: 1509
		public Color? skinColorBase;

		// Token: 0x040005E6 RID: 1510
		public Color? skinColorOverride;

		// Token: 0x040005E7 RID: 1511
		public float randomBrightnessFactor;

		// Token: 0x040005E8 RID: 1512
		public TagFilter hairTagFilter;

		// Token: 0x040005E9 RID: 1513
		public TagFilter beardTagFilter;

		// Token: 0x040005EA RID: 1514
		public GeneticBodyType? bodyType;

		// Token: 0x040005EB RID: 1515
		public List<HeadTypeDef> forcedHeadTypes;

		// Token: 0x040005EC RID: 1516
		public float minMelanin = -1f;

		// Token: 0x040005ED RID: 1517
		public HairDef forcedHair;

		// Token: 0x040005EE RID: 1518
		public bool womenCanHaveBeards;

		// Token: 0x040005EF RID: 1519
		public float socialFightChanceFactor = 1f;

		// Token: 0x040005F0 RID: 1520
		public float aggroMentalBreakSelectionChanceFactor = 1f;

		// Token: 0x040005F1 RID: 1521
		public float mentalBreakMtbDays;

		// Token: 0x040005F2 RID: 1522
		public MentalBreakDef mentalBreakDef;

		// Token: 0x040005F3 RID: 1523
		public float missingGeneRomanceChanceFactor = 1f;

		// Token: 0x040005F4 RID: 1524
		public float prisonBreakMTBFactor = 1f;

		// Token: 0x040005F5 RID: 1525
		public int biostatCpx = 1;

		// Token: 0x040005F6 RID: 1526
		public int biostatMet;

		// Token: 0x040005F7 RID: 1527
		public int biostatArc;

		// Token: 0x040005F8 RID: 1528
		public List<string> exclusionTags;

		// Token: 0x040005F9 RID: 1529
		public GeneDef prerequisite;

		// Token: 0x040005FA RID: 1530
		public float selectionWeight = 1f;

		// Token: 0x040005FB RID: 1531
		public bool canGenerateInGeneSet = true;

		// Token: 0x040005FC RID: 1532
		public GeneSymbolPack symbolPack;

		// Token: 0x040005FD RID: 1533
		public float marketValueFactor = 1f;

		// Token: 0x040005FE RID: 1534
		public bool removeOnRedress;

		// Token: 0x040005FF RID: 1535
		public bool passOnDirectly = true;

		// Token: 0x04000600 RID: 1536
		public float selectionWeightFactorDarkSkin = 1f;

		// Token: 0x04000601 RID: 1537
		public float selectionWeightCultist = 1f;

		// Token: 0x04000602 RID: 1538
		[Unsaved(false)]
		private string cachedDescription;

		// Token: 0x04000603 RID: 1539
		[Unsaved(false)]
		private Texture2D cachedIcon;
	}
}
