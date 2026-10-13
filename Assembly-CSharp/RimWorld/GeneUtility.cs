using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Grammar;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200387C RID: 14460
	[StaticConstructorOnStartup]
	public static class GeneUtility
	{
		// Token: 0x17003307 RID: 13063
		// (get) Token: 0x06013FC5 RID: 81861 RVA: 0x005EF1C0 File Offset: 0x005ED3C0
		public static List<GeneDef> GenesInOrder
		{
			get
			{
				if (GeneUtility.cachedGeneDefsInOrder == null)
				{
					GeneUtility.cachedGeneDefsInOrder = new List<GeneDef>();
					foreach (GeneDef geneDef in DefDatabase<GeneDef>.AllDefs)
					{
						if (geneDef.endogeneCategory != EndogeneCategory.Melanin)
						{
							GeneUtility.cachedGeneDefsInOrder.Add(geneDef);
						}
					}
					GeneUtility.cachedGeneDefsInOrder.SortBy((GeneDef x) => -x.displayCategory.displayPriorityInXenotype, (GeneDef x) => x.displayCategory.label, (GeneDef x) => x.displayOrderInCategory);
				}
				return GeneUtility.cachedGeneDefsInOrder;
			}
		}

		// Token: 0x06013FC6 RID: 81862 RVA: 0x005EF29C File Offset: 0x005ED49C
		public static GeneSet GenerateGeneSet(int? seed = null)
		{
			if (!ModLister.CheckBiotech("geneset generation"))
			{
				return null;
			}
			GeneSet geneSet = new GeneSet();
			if (seed != null)
			{
				Rand.PushState(seed.Value);
			}
			GeneUtility.GeneCount geneCount = GeneUtility.GeneCountProbabilities.RandomElementByWeight((GeneUtility.GeneCount x) => x.chance);
			Func<GeneDef, bool> <>9__1;
			for (int i = 0; i < geneCount.architeCount; i++)
			{
				IEnumerable<GeneDef> allDefs = DefDatabase<GeneDef>.AllDefs;
				Func<GeneDef, bool> predicate;
				if ((predicate = <>9__1) == null)
				{
					predicate = (<>9__1 = ((GeneDef x) => x.biostatArc > 0 && geneSet.CanAddGeneDuringGeneration(x)));
				}
				GeneDef gene;
				if (allDefs.Where(predicate).TryRandomElementByWeight((GeneDef x) => x.selectionWeight, out gene))
				{
					geneSet.AddGene(gene);
				}
			}
			Func<GeneDef, bool> <>9__3;
			for (int j = 0; j < geneCount.nonArchiteCount; j++)
			{
				IEnumerable<GeneDef> allDefs2 = DefDatabase<GeneDef>.AllDefs;
				Func<GeneDef, bool> predicate2;
				if ((predicate2 = <>9__3) == null)
				{
					predicate2 = (<>9__3 = ((GeneDef x) => x.biostatArc == 0 && geneSet.CanAddGeneDuringGeneration(x)));
				}
				GeneDef gene2;
				if (allDefs2.Where(predicate2).TryRandomElementByWeight((GeneDef x) => x.selectionWeight, out gene2))
				{
					geneSet.AddGene(gene2);
				}
			}
			geneSet.GenerateName();
			if (seed != null)
			{
				Rand.PopState();
			}
			if (geneSet.Empty)
			{
				Log.Error("Generated gene pack with no genes.");
			}
			geneSet.SortGenes();
			return geneSet;
		}

		// Token: 0x06013FC7 RID: 81863 RVA: 0x005EF42D File Offset: 0x005ED62D
		public static bool PawnWouldDieFromReimplanting(Pawn pawn)
		{
			return ModsConfig.BiotechActive && pawn.health.hediffSet.HasHediff(HediffDefOf.XenogermReplicating, false);
		}

		// Token: 0x06013FC8 RID: 81864 RVA: 0x005EF450 File Offset: 0x005ED650
		public static void ReimplantXenogerm(Pawn caster, Pawn recipient)
		{
			if (!ModLister.CheckBiotech("xenogerm reimplantation"))
			{
				return;
			}
			QuestUtility.SendQuestTargetSignals(caster.questTags, "XenogermReimplanted", caster.Named("SUBJECT"));
			recipient.genes.SetXenotype(caster.genes.Xenotype);
			recipient.genes.xenotypeName = caster.genes.xenotypeName;
			recipient.genes.xenotypeName = caster.genes.xenotypeName;
			recipient.genes.iconDef = caster.genes.iconDef;
			recipient.genes.ClearXenogenes();
			foreach (Gene gene in caster.genes.Xenogenes)
			{
				recipient.genes.AddGene(gene.def, true);
			}
			if (!caster.genes.Xenotype.soundDefOnImplant.NullOrUndefined())
			{
				caster.genes.Xenotype.soundDefOnImplant.PlayOneShot(SoundInfo.InMap(recipient, MaintenanceType.None));
			}
			recipient.health.AddHediff(HediffDefOf.XenogerminationComa, null, null, null);
			GeneUtility.ExtractXenogerm(caster, -1);
			GeneUtility.UpdateXenogermReplication(recipient);
		}

		// Token: 0x06013FC9 RID: 81865 RVA: 0x005EF5A0 File Offset: 0x005ED7A0
		public static void ExtractXenogerm(Pawn pawn, int overrideDurationTicks = -1)
		{
			if (!ModLister.CheckBiotech("xenogerm extraction"))
			{
				return;
			}
			pawn.health.AddHediff(HediffDefOf.XenogermLossShock, null, null, null);
			if (GeneUtility.PawnWouldDieFromReimplanting(pawn))
			{
				pawn.genes.SetXenotype(XenotypeDefOf.Baseliner);
			}
			Hediff hediff = HediffMaker.MakeHediff(HediffDefOf.XenogermReplicating, pawn, null);
			if (overrideDurationTicks > 0)
			{
				hediff.TryGetComp<HediffComp_Disappears>().ticksToDisappear = overrideDurationTicks;
			}
			pawn.health.AddHediff(hediff, null, null, null);
		}

		// Token: 0x06013FCA RID: 81866 RVA: 0x005EF624 File Offset: 0x005ED824
		public static void ImplantXenogermItem(Pawn pawn, Xenogerm xenogerm)
		{
			if (!ModLister.CheckBiotech("xenogerm implantation"))
			{
				return;
			}
			GeneUtility.UpdateXenogermReplication(pawn);
			if (xenogerm.GeneSet != null && pawn.genes != null)
			{
				pawn.genes.SetXenotype(XenotypeDefOf.Baseliner);
				pawn.genes.xenotypeName = xenogerm.xenotypeName;
				pawn.genes.iconDef = xenogerm.iconDef;
				foreach (GeneDef geneDef in xenogerm.GeneSet.GenesListForReading)
				{
					pawn.genes.AddGene(geneDef, true);
				}
			}
		}

		// Token: 0x06013FCB RID: 81867 RVA: 0x005EF6D8 File Offset: 0x005ED8D8
		public static void UpdateXenogermReplication(Pawn pawn)
		{
			if (!ModsConfig.BiotechActive)
			{
				return;
			}
			Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.XenogermReplicating, false);
			if (firstHediffOfDef != null)
			{
				pawn.health.RemoveHediff(firstHediffOfDef);
			}
			pawn.health.AddHediff(HediffDefOf.XenogermReplicating, null, null, null);
		}

		// Token: 0x06013FCC RID: 81868 RVA: 0x005EF72F File Offset: 0x005ED92F
		public static bool IsBloodfeeder(this Pawn pawn)
		{
			return ModsConfig.BiotechActive && pawn.genes != null && pawn.genes.HasActiveGene(GeneDefOf.Bloodfeeder);
		}

		// Token: 0x06013FCD RID: 81869 RVA: 0x005EF752 File Offset: 0x005ED952
		public static bool CanDeathrest(this Pawn pawn)
		{
			return ModsConfig.BiotechActive && pawn.genes != null && pawn.genes.GetFirstGeneOfType<Gene_Deathrest>() != null;
		}

		// Token: 0x06013FCE RID: 81870 RVA: 0x005EF774 File Offset: 0x005ED974
		public static bool SterileGenes(this Pawn pawn)
		{
			if (!ModsConfig.BiotechActive || pawn.genes == null)
			{
				return false;
			}
			List<Gene> genesListForReading = pawn.genes.GenesListForReading;
			for (int i = 0; i < genesListForReading.Count; i++)
			{
				if (genesListForReading[i].Active && genesListForReading[i].def.sterilize)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06013FCF RID: 81871 RVA: 0x005EF7D4 File Offset: 0x005ED9D4
		public static BodyTypeDef ToBodyType(this GeneticBodyType bodyType, Pawn pawn)
		{
			switch (bodyType)
			{
			case GeneticBodyType.Fat:
				return BodyTypeDefOf.Fat;
			case GeneticBodyType.Thin:
				return BodyTypeDefOf.Thin;
			case GeneticBodyType.Hulk:
				return BodyTypeDefOf.Hulk;
			case GeneticBodyType.Standard:
				if (pawn.gender != Gender.Female)
				{
					return BodyTypeDefOf.Male;
				}
				return BodyTypeDefOf.Female;
			default:
				Log.Error("Undefined bodyType");
				return null;
			}
		}

		// Token: 0x06013FD0 RID: 81872 RVA: 0x005EF82C File Offset: 0x005EDA2C
		public static void OffsetHemogen(Pawn pawn, float offset, bool applyStatFactor = true)
		{
			if (!ModsConfig.BiotechActive)
			{
				return;
			}
			if (offset > 0f && applyStatFactor)
			{
				offset *= pawn.GetStatValue(StatDefOf.HemogenGainFactor, true, -1);
			}
			Pawn_GeneTracker genes = pawn.genes;
			Gene_HemogenDrain gene_HemogenDrain = (genes != null) ? genes.GetFirstGeneOfType<Gene_HemogenDrain>() : null;
			if (gene_HemogenDrain != null)
			{
				GeneResourceDrainUtility.OffsetResource(gene_HemogenDrain, offset);
				return;
			}
			Pawn_GeneTracker genes2 = pawn.genes;
			Gene_Hemogen gene_Hemogen = (genes2 != null) ? genes2.GetFirstGeneOfType<Gene_Hemogen>() : null;
			if (gene_Hemogen != null)
			{
				gene_Hemogen.Value += offset;
			}
		}

		// Token: 0x06013FD1 RID: 81873 RVA: 0x005EF8A0 File Offset: 0x005EDAA0
		public static string GenerateXenotypeNameFromGenes(List<GeneDef> genes)
		{
			GeneUtility.tmpSymbolPacks.Clear();
			string text = string.Empty;
			if (genes.Count == 0)
			{
				return text;
			}
			GrammarRequest request = default(GrammarRequest);
			request.Includes.Add(RulePackDefOf.NamerXenotype);
			for (int i = 0; i < genes.Count; i++)
			{
				if (genes[i].symbolPack != null)
				{
					GeneUtility.tmpSymbolPacks.Add(genes[i].symbolPack);
				}
			}
			for (int j = 0; j < 150; j++)
			{
				GeneSymbolPack geneSymbolPack;
				(from x in GeneUtility.tmpSymbolPacks
				where !x.wholeNameSymbols.NullOrEmpty<GeneSymbolPack.WeightedSymbol>()
				select x).TryRandomElementByWeight((GeneSymbolPack x) => x.wholeNameSymbols.Sum((GeneSymbolPack.WeightedSymbol y) => y.weight), out geneSymbolPack);
				GeneSymbolPack geneSymbolPack2;
				(from x in GeneUtility.tmpSymbolPacks
				where !x.prefixSymbols.NullOrEmpty<GeneSymbolPack.WeightedSymbol>()
				select x).TryRandomElementByWeight((GeneSymbolPack x) => x.prefixSymbols.Sum((GeneSymbolPack.WeightedSymbol y) => y.weight), out geneSymbolPack2);
				GeneSymbolPack geneSymbolPack3;
				(from x in GeneUtility.tmpSymbolPacks
				where !x.suffixSymbols.NullOrEmpty<GeneSymbolPack.WeightedSymbol>()
				select x).Except(geneSymbolPack2).TryRandomElementByWeight((GeneSymbolPack x) => x.suffixSymbols.Sum((GeneSymbolPack.WeightedSymbol y) => y.weight), out geneSymbolPack3);
				GeneSymbolPack.WeightedSymbol prefix = null;
				GeneSymbolPack.WeightedSymbol suffix = null;
				GeneSymbolPack.WeightedSymbol weightedSymbol = null;
				if (geneSymbolPack != null && geneSymbolPack.wholeNameSymbols.TryRandomElementByWeight((GeneSymbolPack.WeightedSymbol x) => base.<GenerateXenotypeNameFromGenes>g__SymbolWeight|6(x), out weightedSymbol))
				{
					request.Rules.Add(new Rule_String("wholeName", weightedSymbol.symbol));
				}
				if (geneSymbolPack2 != null && geneSymbolPack2.prefixSymbols.TryRandomElementByWeight((GeneSymbolPack.WeightedSymbol x) => base.<GenerateXenotypeNameFromGenes>g__SymbolWeight|6(x), out prefix))
				{
					request.Rules.Add(new Rule_String("genePrefix", prefix.symbol));
				}
				if (geneSymbolPack3 != null && geneSymbolPack3.suffixSymbols.TryRandomElementByWeight((GeneSymbolPack.WeightedSymbol x) => base.<GenerateXenotypeNameFromGenes>g__SymbolWeight|6(x), out suffix))
				{
					request.Rules.Add(new Rule_String("geneSuffix", suffix.symbol));
				}
				string result = GrammarResolver.Resolve("r_name", request, null, false, null, null, null, false);
				if (!NameUseChecker.XenotypeNameIsUsed(text))
				{
					return result;
				}
			}
			Log.Error("Failed to generate a unique xenotype name.");
			if (text.NullOrEmpty())
			{
				text = "ERR";
			}
			return text;
		}

		// Token: 0x06013FD2 RID: 81874 RVA: 0x005EFB44 File Offset: 0x005EDD44
		public static void SatisfyChemicalGenes(Pawn pawn)
		{
			if (!ModsConfig.BiotechActive || pawn.genes == null)
			{
				return;
			}
			foreach (Gene gene in pawn.genes.GenesListForReading)
			{
				Gene_ChemicalDependency gene_ChemicalDependency = gene as Gene_ChemicalDependency;
				if (gene_ChemicalDependency != null)
				{
					gene_ChemicalDependency.Reset();
				}
			}
		}

		// Token: 0x06013FD3 RID: 81875 RVA: 0x005EFBB4 File Offset: 0x005EDDB4
		public static void SortGeneDefs(this List<GeneDef> geneDefs)
		{
			geneDefs.SortBy((GeneDef x) => -x.displayCategory.displayPriorityInXenotype, (GeneDef x) => x.displayOrderInCategory, (GeneDef y) => y.label);
		}

		// Token: 0x06013FD4 RID: 81876 RVA: 0x005EFC24 File Offset: 0x005EDE24
		public static void SortGenes(this List<Gene> genes)
		{
			genes.SortBy((Gene x) => !x.Active, (Gene x) => -x.def.displayCategory.displayPriorityInXenotype, (Gene y) => y.def.displayOrderInCategory);
		}

		// Token: 0x06013FD5 RID: 81877 RVA: 0x005EFC94 File Offset: 0x005EDE94
		public static void SortGenepacks(this List<Genepack> genepacks)
		{
			genepacks.SortBy((Genepack x) => -x.GeneSet.GenesListForReading[0].displayCategory.displayPriorityInXenotype, (Genepack x) => x.GeneSet.GenesListForReading[0].displayCategory.label, (Genepack x) => x.GeneSet.GenesListForReading[0].displayOrderInCategory);
		}

		// Token: 0x06013FD6 RID: 81878 RVA: 0x005EFD04 File Offset: 0x005EDF04
		public static List<GeneDef> NonOverriddenGenes(this List<GeneDefWithType> geneDefWithTypes)
		{
			GeneUtility.tmpGenes.Clear();
			GeneUtility.tmpOverriddenGenes.Clear();
			if (!ModsConfig.BiotechActive)
			{
				return GeneUtility.tmpGenes;
			}
			foreach (GeneDefWithType geneDefWithType in geneDefWithTypes)
			{
				GeneUtility.tmpGenes.Add(geneDefWithType.geneDef);
			}
			for (int i = 0; i < geneDefWithTypes.Count; i++)
			{
				if (!geneDefWithTypes[i].RandomChosen)
				{
					for (int j = i + 1; j < geneDefWithTypes.Count; j++)
					{
						if (!geneDefWithTypes[j].RandomChosen && geneDefWithTypes[i].ConflictsWith(geneDefWithTypes[j]))
						{
							if (geneDefWithTypes[i].Overrides(geneDefWithTypes[j]))
							{
								GeneUtility.tmpOverriddenGenes.Add(geneDefWithTypes[j]);
							}
							else
							{
								GeneUtility.tmpOverriddenGenes.Add(geneDefWithTypes[i]);
							}
						}
					}
				}
			}
			foreach (GeneDefWithType geneDefWithType2 in GeneUtility.tmpOverriddenGenes)
			{
				GeneUtility.tmpGenes.Remove(geneDefWithType2.geneDef);
			}
			GeneUtility.tmpOverriddenGenes.Clear();
			return GeneUtility.tmpGenes;
		}

		// Token: 0x06013FD7 RID: 81879 RVA: 0x005EFE88 File Offset: 0x005EE088
		public static List<GeneDef> NonOverriddenGenes(this List<GeneDef> geneDefs, bool xenogene)
		{
			GeneUtility.tmpGeneDefsWithType.Clear();
			foreach (GeneDef geneDef in geneDefs)
			{
				GeneUtility.tmpGeneDefsWithType.Add(new GeneDefWithType(geneDef, xenogene));
			}
			return GeneUtility.tmpGeneDefsWithType.NonOverriddenGenes();
		}

		// Token: 0x06013FD8 RID: 81880 RVA: 0x005EFEF4 File Offset: 0x005EE0F4
		public static bool Overrides(this GeneDef gene, GeneDef other, bool isXenogene, bool otherIsXenogene)
		{
			if (gene.RandomChosen || other.RandomChosen || !gene.ConflictsWith(other))
			{
				return false;
			}
			if (isXenogene == otherIsXenogene)
			{
				return GeneUtility.GenesInOrder.IndexOf(gene) <= GeneUtility.GenesInOrder.IndexOf(other);
			}
			return isXenogene && !otherIsXenogene;
		}

		// Token: 0x06013FD9 RID: 81881 RVA: 0x005EFF48 File Offset: 0x005EE148
		public static bool IsBaseliner(this Pawn pawn)
		{
			if (ModsConfig.BiotechActive)
			{
				Pawn_GeneTracker genes = pawn.genes;
				if (((genes != null) ? genes.Xenotype : null) != null)
				{
					return pawn.genes.Xenotype == XenotypeDefOf.Baseliner && pawn.genes.CustomXenotype == null;
				}
			}
			return true;
		}

		// Token: 0x06013FDA RID: 81882 RVA: 0x005EFF94 File Offset: 0x005EE194
		public static bool SameHeritableXenotype(Pawn pawn1, Pawn pawn2)
		{
			if (((pawn1 != null) ? pawn1.genes : null) == null || ((pawn2 != null) ? pawn2.genes : null) == null)
			{
				return false;
			}
			if (pawn1.genes.UniqueXenotype || pawn2.genes.UniqueXenotype)
			{
				for (int i = 0; i < pawn1.genes.Endogenes.Count; i++)
				{
					Gene gene = pawn1.genes.Endogenes[i];
					if (gene.def != GeneDefOf.Inbred && gene.def.endogeneCategory != EndogeneCategory.Melanin && gene.def.endogeneCategory != EndogeneCategory.HairColor && !pawn2.genes.Endogenes.Any((Gene x) => x.def == gene.def))
					{
						return false;
					}
				}
				for (int j = 0; j < pawn2.genes.Endogenes.Count; j++)
				{
					Gene gene = pawn2.genes.Endogenes[j];
					if (gene.def != GeneDefOf.Inbred && gene.def.endogeneCategory != EndogeneCategory.Melanin && gene.def.endogeneCategory != EndogeneCategory.HairColor && !pawn1.genes.Endogenes.Any((Gene x) => x.def == gene.def))
					{
						return false;
					}
				}
				return true;
			}
			return pawn1.genes.Xenotype == pawn2.genes.Xenotype && pawn1.genes.Xenotype.inheritable;
		}

		// Token: 0x06013FDB RID: 81883 RVA: 0x005F0130 File Offset: 0x005EE330
		public static bool SameXenotype(Pawn pawn1, Pawn pawn2)
		{
			Pawn pawn3 = pawn1;
			if (((pawn3 != null) ? pawn3.genes : null) != null)
			{
				Pawn pawn4 = pawn2;
				if (((pawn4 != null) ? pawn4.genes : null) != null)
				{
					if (pawn1.genes.UniqueXenotype || pawn2.genes.UniqueXenotype)
					{
						int j;
						int k;
						for (k = 0; k < pawn1.genes.Xenogenes.Count; k = j + 1)
						{
							if (!pawn2.genes.Xenogenes.Any((Gene x) => x.def == pawn1.genes.Xenogenes[k].def))
							{
								return false;
							}
							j = k;
						}
						int i;
						for (i = 0; i < pawn2.genes.Xenogenes.Count; i = j + 1)
						{
							if (!pawn1.genes.Xenogenes.Any((Gene x) => x.def == pawn2.genes.Xenogenes[i].def))
							{
								return false;
							}
							j = i;
						}
						CustomXenotype customXenotype = pawn1.genes.CustomXenotype;
						CustomXenotype customXenotype2 = pawn2.genes.CustomXenotype;
						return customXenotype == customXenotype2;
					}
					return pawn1.genes.Xenotype == pawn2.genes.Xenotype;
				}
			}
			return false;
		}

		// Token: 0x06013FDC RID: 81884 RVA: 0x005F02DC File Offset: 0x005EE4DC
		public static bool PawnIsCustomXenotype(Pawn pawn, CustomXenotype custom)
		{
			if (!ModsConfig.BiotechActive || pawn.genes == null)
			{
				return false;
			}
			List<Gene> list = custom.inheritable ? pawn.genes.Endogenes : pawn.genes.Xenogenes;
			int i;
			Predicate<Gene> <>9__0;
			int k;
			for (i = 0; i < custom.genes.Count; i = k + 1)
			{
				if (custom.genes[i].passOnDirectly)
				{
					List<Gene> list2 = list;
					Predicate<Gene> predicate;
					if ((predicate = <>9__0) == null)
					{
						predicate = (<>9__0 = ((Gene x) => x.def == custom.genes[i]));
					}
					if (!list2.Any(predicate))
					{
						return false;
					}
				}
				k = i;
			}
			for (int j = 0; j < list.Count; j++)
			{
				if (list[j].def.passOnDirectly && !custom.genes.Contains(list[j].def))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06013FDD RID: 81885 RVA: 0x005F03F0 File Offset: 0x005EE5F0
		public static int MetabolismAfterImplanting(Pawn pawn, GeneSet geneSet)
		{
			GeneUtility.tmpTypedGenes.Clear();
			foreach (Gene gene in pawn.genes.Endogenes)
			{
				GeneUtility.tmpTypedGenes.Add(new GeneDefWithType(gene.def, false));
			}
			foreach (GeneDef geneDef in geneSet.GenesListForReading)
			{
				GeneUtility.tmpTypedGenes.Add(new GeneDefWithType(geneDef, true));
			}
			int result = GeneUtility.tmpTypedGenes.NonOverriddenGenes().Sum((GeneDef x) => x.biostatMet);
			GeneUtility.tmpTypedGenes.Clear();
			return result;
		}

		// Token: 0x06013FDE RID: 81886 RVA: 0x005F04E8 File Offset: 0x005EE6E8
		public static void GiveReimplantJob(Pawn pawn, Pawn targPawn)
		{
			pawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.AbsorbXenogerm, targPawn), new JobTag?(JobTag.Misc), false);
			if (targPawn.HomeFaction != null && !targPawn.HomeFaction.Hidden && targPawn.HomeFaction != pawn.Faction && !targPawn.HomeFaction.HostileTo(Faction.OfPlayer))
			{
				Messages.Message("MessageAbsorbingXenogermWillAngerFaction".Translate(targPawn.HomeFaction, targPawn.Named("PAWN")), pawn, MessageTypeDefOf.CautionInput, false);
			}
		}

		// Token: 0x06013FDF RID: 81887 RVA: 0x005F0584 File Offset: 0x005EE784
		public static bool CanAbsorbXenogerm(Pawn pawn)
		{
			return ((pawn != null) ? pawn.genes : null) != null && pawn.genes.HasActiveGene(GeneDefOf.XenogermReimplanter) && ((pawn.IsPrisonerOfColony && pawn.guest.PrisonerIsSecure) || (pawn.Downed && pawn.genes.Xenogenes.Any<Gene>()));
		}

		// Token: 0x06013FE0 RID: 81888 RVA: 0x005F05EC File Offset: 0x005EE7EC
		public static int AddedAndImplantedPartsWithXenogenesCount(Pawn pawn)
		{
			int num = pawn.health.hediffSet.CountAddedAndImplantedParts();
			if (ModsConfig.BiotechActive && pawn.genes != null && pawn.genes.Xenogenes.Any<Gene>())
			{
				num++;
			}
			return num;
		}

		// Token: 0x0400DCCB RID: 56523
		private static List<GeneDef> cachedGeneDefsInOrder = null;

		// Token: 0x0400DCCC RID: 56524
		private const int MaxTriesToGenerateSingleXenotypeName = 150;

		// Token: 0x0400DCCD RID: 56525
		private const string BiostatsTexPath = "UI/Icons/Biostats/";

		// Token: 0x0400DCCE RID: 56526
		private const string PrefixKeyword = "genePrefix";

		// Token: 0x0400DCCF RID: 56527
		private const string SuffixKeyword = "geneSuffix";

		// Token: 0x0400DCD0 RID: 56528
		private const string WholeNameKeyword = "wholeName";

		// Token: 0x0400DCD1 RID: 56529
		private const string RootKeyword = "r_name";

		// Token: 0x0400DCD2 RID: 56530
		public static readonly CachedTexture UniqueXenotypeTex = new CachedTexture("UI/Icons/Xenotypes/Other");

		// Token: 0x0400DCD3 RID: 56531
		public static readonly CachedTexture GCXTex = new CachedTexture("UI/Icons/Biostats/Complexity");

		// Token: 0x0400DCD4 RID: 56532
		public static readonly CachedTexture METTex = new CachedTexture("UI/Icons/Biostats/Metabolism");

		// Token: 0x0400DCD5 RID: 56533
		public static readonly CachedTexture ARCTex = new CachedTexture("UI/Icons/Biostats/ArchiteCapsuleRequired");

		// Token: 0x0400DCD6 RID: 56534
		private static readonly GeneUtility.GeneCount[] GeneCountProbabilities = new GeneUtility.GeneCount[]
		{
			new GeneUtility.GeneCount
			{
				nonArchiteCount = 1,
				chance = 0.66f
			},
			new GeneUtility.GeneCount
			{
				nonArchiteCount = 2,
				chance = 0.2f
			},
			new GeneUtility.GeneCount
			{
				nonArchiteCount = 3,
				chance = 0.05f
			},
			new GeneUtility.GeneCount
			{
				nonArchiteCount = 4,
				chance = 0.02f
			},
			new GeneUtility.GeneCount
			{
				nonArchiteCount = 1,
				architeCount = 1,
				chance = 0.05f
			},
			new GeneUtility.GeneCount
			{
				nonArchiteCount = 2,
				architeCount = 1,
				chance = 0.02f
			}
		};

		// Token: 0x0400DCD7 RID: 56535
		public static readonly Color GCXColor = new ColorInt(105, 213, 240).ToColor;

		// Token: 0x0400DCD8 RID: 56536
		public static readonly Color METColor = new ColorInt(247, 72, 160).ToColor;

		// Token: 0x0400DCD9 RID: 56537
		public static readonly Color ARCColor = new ColorInt(214, 219, 61).ToColor;

		// Token: 0x0400DCDA RID: 56538
		private static List<GeneSymbolPack> tmpSymbolPacks = new List<GeneSymbolPack>();

		// Token: 0x0400DCDB RID: 56539
		private static List<GeneDef> tmpGenes = new List<GeneDef>();

		// Token: 0x0400DCDC RID: 56540
		private static HashSet<GeneDefWithType> tmpOverriddenGenes = new HashSet<GeneDefWithType>();

		// Token: 0x0400DCDD RID: 56541
		private static List<GeneDefWithType> tmpGeneDefsWithType = new List<GeneDefWithType>();

		// Token: 0x0400DCDE RID: 56542
		private static List<GeneDefWithType> tmpTypedGenes = new List<GeneDefWithType>();

		// Token: 0x0200387D RID: 14461
		private struct GeneCount
		{
			// Token: 0x0400DCDF RID: 56543
			public int nonArchiteCount;

			// Token: 0x0400DCE0 RID: 56544
			public int architeCount;

			// Token: 0x0400DCE1 RID: 56545
			public float chance;
		}
	}
}
