using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x020018FF RID: 6399
	public static class GeneDefGenerator
	{
		// Token: 0x06009165 RID: 37221 RVA: 0x002AC757 File Offset: 0x002AA957
		public static IEnumerable<GeneDef> ImpliedGeneDefs(bool hotReload = false)
		{
			if (!ModsConfig.BiotechActive)
			{
				yield break;
			}
			foreach (GeneTemplateDef g in DefDatabase<GeneTemplateDef>.AllDefs)
			{
				GeneTemplateDef.GeneTemplateType geneTemplateType = g.geneTemplateType;
				if (geneTemplateType != GeneTemplateDef.GeneTemplateType.Skill)
				{
					if (geneTemplateType == GeneTemplateDef.GeneTemplateType.Chemical)
					{
						foreach (ChemicalDef chemicalDef in DefDatabase<ChemicalDef>.AllDefs)
						{
							if (chemicalDef.generateAddictionGenes)
							{
								yield return GeneDefGenerator.GetFromTemplate(g, chemicalDef, (int)(chemicalDef.index * 1000), hotReload);
							}
						}
						IEnumerator<ChemicalDef> enumerator2 = null;
					}
				}
				else
				{
					foreach (SkillDef skillDef in DefDatabase<SkillDef>.AllDefs)
					{
						yield return GeneDefGenerator.GetFromTemplate(g, skillDef, (int)(skillDef.index * 1000), hotReload);
					}
					IEnumerator<SkillDef> enumerator3 = null;
				}
				g = null;
			}
			IEnumerator<GeneTemplateDef> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06009166 RID: 37222 RVA: 0x002AC767 File Offset: 0x002AA967
		public static IEnumerable<ThoughtDef> ImpliedThoughtDefs(bool hotReload = false)
		{
			if (!ModsConfig.BiotechActive)
			{
				yield break;
			}
			foreach (GeneTemplateDef geneTemplateDef in DefDatabase<GeneTemplateDef>.AllDefs)
			{
				if (geneTemplateDef.geneTemplateType == GeneTemplateDef.GeneTemplateType.Chemical && geneTemplateDef.geneClass == typeof(Gene_ChemicalDependency))
				{
					foreach (ChemicalDef chemicalDef in DefDatabase<ChemicalDef>.AllDefs)
					{
						if (chemicalDef.generateAddictionGenes)
						{
							yield return GeneDefGenerator.DependencyThoughtDef(chemicalDef, hotReload);
						}
					}
					IEnumerator<ChemicalDef> enumerator2 = null;
				}
			}
			IEnumerator<GeneTemplateDef> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06009167 RID: 37223 RVA: 0x002AC778 File Offset: 0x002AA978
		private static GeneDef GetFromTemplate(GeneTemplateDef template, Def def, int displayOrderBase, bool hotReload = false)
		{
			GeneDefGenerator.<>c__DisplayClass2_0 CS$<>8__locals1;
			CS$<>8__locals1.template = template;
			CS$<>8__locals1.def = def;
			string defName = CS$<>8__locals1.template.defName + "_" + CS$<>8__locals1.def.defName;
			GeneDef geneDef = hotReload ? (DefDatabase<GeneDef>.GetNamed(defName, false) ?? new GeneDef()) : new GeneDef();
			geneDef.defName = defName;
			geneDef.geneClass = CS$<>8__locals1.template.geneClass;
			geneDef.label = CS$<>8__locals1.template.label.Formatted(CS$<>8__locals1.def.label);
			geneDef.iconPath = CS$<>8__locals1.template.iconPath.Formatted(CS$<>8__locals1.def.defName);
			geneDef.description = GeneDefGenerator.<GetFromTemplate>g__ResolveDescription|2_0(ref CS$<>8__locals1);
			geneDef.labelShortAdj = CS$<>8__locals1.template.labelShortAdj.Formatted(CS$<>8__locals1.def.label);
			geneDef.selectionWeight = CS$<>8__locals1.template.selectionWeight;
			geneDef.biostatCpx = CS$<>8__locals1.template.biostatCpx;
			geneDef.biostatMet = CS$<>8__locals1.template.biostatMet;
			geneDef.displayCategory = CS$<>8__locals1.template.displayCategory;
			geneDef.displayOrderInCategory = (float)(displayOrderBase + CS$<>8__locals1.template.displayOrderOffset);
			geneDef.minAgeActive = CS$<>8__locals1.template.minAgeActive;
			geneDef.modContentPack = CS$<>8__locals1.template.modContentPack;
			if (!CS$<>8__locals1.template.exclusionTagPrefix.NullOrEmpty())
			{
				geneDef.exclusionTags = new List<string>
				{
					CS$<>8__locals1.template.exclusionTagPrefix + "_" + CS$<>8__locals1.def.defName
				};
			}
			SkillDef skillDef = CS$<>8__locals1.def as SkillDef;
			if (skillDef != null)
			{
				if (CS$<>8__locals1.template.aptitudeOffset != 0)
				{
					geneDef.aptitudes = new List<Aptitude>
					{
						new Aptitude(skillDef, CS$<>8__locals1.template.aptitudeOffset)
					};
				}
				if (CS$<>8__locals1.template.passionModType != PassionMod.PassionModType.None)
				{
					geneDef.passionMod = new PassionMod(skillDef, CS$<>8__locals1.template.passionModType);
				}
			}
			else
			{
				ChemicalDef chemicalDef = CS$<>8__locals1.def as ChemicalDef;
				if (chemicalDef != null)
				{
					geneDef.chemical = chemicalDef;
					geneDef.addictionChanceFactor = CS$<>8__locals1.template.addictionChanceFactor;
					if (!CS$<>8__locals1.template.chemicalBiostatOverrides.NullOrEmpty<GeneTemplateDef.ChemicalBiostatOverride>())
					{
						foreach (GeneTemplateDef.ChemicalBiostatOverride chemicalBiostatOverride in CS$<>8__locals1.template.chemicalBiostatOverrides)
						{
							if (chemicalBiostatOverride.chemical == chemicalDef)
							{
								geneDef.biostatCpx = (chemicalBiostatOverride.biostatCpx ?? geneDef.biostatCpx);
								geneDef.biostatMet = (chemicalBiostatOverride.biostatMet ?? geneDef.biostatMet);
								geneDef.biostatArc = (chemicalBiostatOverride.biostatArc ?? geneDef.biostatArc);
							}
						}
					}
					if (geneDef.geneClass != typeof(Gene_ChemicalDependency))
					{
						if (geneDef.addictionChanceFactor <= 0f)
						{
							geneDef.overdoseChanceFactor = chemicalDef.geneOverdoseChanceFactorImmune;
							geneDef.toleranceBuildupFactor = chemicalDef.geneToleranceBuildupFactorImmune;
						}
						else
						{
							geneDef.overdoseChanceFactor = chemicalDef.geneOverdoseChanceFactorResist;
							geneDef.toleranceBuildupFactor = chemicalDef.geneToleranceBuildupFactorResist;
						}
						if (geneDef.overdoseChanceFactor != 1f)
						{
							GeneDef geneDef2 = geneDef;
							geneDef2.description += " " + ((geneDef.overdoseChanceFactor == 0f) ? "GeneOverdoseImmune" : "GeneOverdoseFactor").Translate(chemicalDef, geneDef.overdoseChanceFactor.ToStringPercent());
						}
						if (geneDef.toleranceBuildupFactor != 1f)
						{
							GeneDef geneDef3 = geneDef;
							geneDef3.description += " " + ((geneDef.toleranceBuildupFactor == 0f) ? "GeneToleranceBuildupImmune" : "GeneToleranceBuildupFactor").Translate(chemicalDef, geneDef.toleranceBuildupFactor.ToStringPercent());
						}
					}
				}
			}
			return geneDef;
		}

		// Token: 0x06009168 RID: 37224 RVA: 0x002ACBD0 File Offset: 0x002AADD0
		private static ThoughtDef DependencyThoughtDef(ChemicalDef chemicalDef, bool hotReload = false)
		{
			string defName = "GeneticChemicalDependency_" + chemicalDef.defName;
			ThoughtDef thoughtDef = hotReload ? (DefDatabase<ThoughtDef>.GetNamed(defName, false) ?? new ThoughtDef()) : new ThoughtDef();
			thoughtDef.defName = defName;
			thoughtDef.thoughtClass = typeof(Thought_Situational_GeneticChemicalDependency);
			thoughtDef.workerClass = typeof(ThoughtWorker_GeneticChemicalDependency);
			thoughtDef.developmentalStageFilter = (DevelopmentalStage.Child | DevelopmentalStage.Adult);
			thoughtDef.chemicalDef = chemicalDef;
			thoughtDef.stages = new List<ThoughtStage>
			{
				new ThoughtStage
				{
					label = "GeneticChemicalDependency_Label".Translate(chemicalDef.Named("CHEMICAL")),
					description = "GeneticChemicalDependency_Desc".Translate(chemicalDef.Named("CHEMICAL")),
					baseMoodEffect = -1f
				}
			};
			return thoughtDef;
		}

		// Token: 0x06009169 RID: 37225 RVA: 0x002ACCA0 File Offset: 0x002AAEA0
		[CompilerGenerated]
		internal static string <GetFromTemplate>g__ResolveDescription|2_0(ref GeneDefGenerator.<>c__DisplayClass2_0 A_0)
		{
			string result;
			if (A_0.template.geneClass == typeof(Gene_ChemicalDependency))
			{
				result = A_0.template.description.Formatted(A_0.def.label, "PeriodDays".Translate(5f).Named("DEFICIENCYDURATION"), "PeriodDays".Translate(30f).Named("COMADURATION"), "PeriodDays".Translate(60f).Named("DEATHDURATION"));
			}
			else
			{
				result = A_0.template.description.Formatted(A_0.def.label);
			}
			return result;
		}
	}
}
