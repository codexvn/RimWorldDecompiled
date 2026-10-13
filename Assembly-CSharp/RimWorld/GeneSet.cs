using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02002AE3 RID: 10979
	public class GeneSet : IExposable
	{
		// Token: 0x17002641 RID: 9793
		// (get) Token: 0x0600F4C6 RID: 62662 RVA: 0x004725CB File Offset: 0x004707CB
		public string Label
		{
			get
			{
				if (this.name.NullOrEmpty())
				{
					this.GenerateName();
				}
				return this.name;
			}
		}

		// Token: 0x17002642 RID: 9794
		// (get) Token: 0x0600F4C7 RID: 62663 RVA: 0x004725E6 File Offset: 0x004707E6
		private string LabelShortAdj
		{
			get
			{
				if (!this.genes.NullOrEmpty<GeneDef>())
				{
					return this.genes[0].LabelShortAdj;
				}
				return "ERR";
			}
		}

		// Token: 0x17002643 RID: 9795
		// (get) Token: 0x0600F4C8 RID: 62664 RVA: 0x0047260C File Offset: 0x0047080C
		public List<GeneDef> GenesListForReading
		{
			get
			{
				return this.genes;
			}
		}

		// Token: 0x17002644 RID: 9796
		// (get) Token: 0x0600F4C9 RID: 62665 RVA: 0x00472614 File Offset: 0x00470814
		public bool Empty
		{
			get
			{
				return this.genes.Count == 0;
			}
		}

		// Token: 0x17002645 RID: 9797
		// (get) Token: 0x0600F4CA RID: 62666 RVA: 0x00472624 File Offset: 0x00470824
		public int ComplexityTotal
		{
			get
			{
				this.RecacheOverridesIfNeeded();
				int num = 0;
				for (int i = 0; i < this.cachedNonOverriddenGenes.Count; i++)
				{
					num += this.cachedNonOverriddenGenes[i].biostatCpx;
				}
				return num;
			}
		}

		// Token: 0x17002646 RID: 9798
		// (get) Token: 0x0600F4CB RID: 62667 RVA: 0x00472664 File Offset: 0x00470864
		public int MetabolismTotal
		{
			get
			{
				this.RecacheOverridesIfNeeded();
				int num = 0;
				for (int i = 0; i < this.cachedNonOverriddenGenes.Count; i++)
				{
					num += this.cachedNonOverriddenGenes[i].biostatMet;
				}
				return num;
			}
		}

		// Token: 0x17002647 RID: 9799
		// (get) Token: 0x0600F4CC RID: 62668 RVA: 0x004726A4 File Offset: 0x004708A4
		public int ArchitesTotal
		{
			get
			{
				this.RecacheOverridesIfNeeded();
				int num = 0;
				for (int i = 0; i < this.cachedNonOverriddenGenes.Count; i++)
				{
					num += this.cachedNonOverriddenGenes[i].biostatArc;
				}
				return num;
			}
		}

		// Token: 0x17002648 RID: 9800
		// (get) Token: 0x0600F4CD RID: 62669 RVA: 0x004726E4 File Offset: 0x004708E4
		private IEnumerable<DefHyperlink> GeneDefHyperlinks
		{
			get
			{
				int num;
				for (int i = 0; i < this.genes.Count; i = num + 1)
				{
					yield return new DefHyperlink(this.genes[i]);
					num = i;
				}
				yield break;
			}
		}

		// Token: 0x0600F4CE RID: 62670 RVA: 0x004726F4 File Offset: 0x004708F4
		public void AddGene(GeneDef gene)
		{
			if (!this.genes.Contains(gene))
			{
				this.genes.Add(gene);
				this.DirtyCache();
			}
		}

		// Token: 0x0600F4CF RID: 62671 RVA: 0x00472716 File Offset: 0x00470916
		public void SetNameDirect(string name)
		{
			this.name = name;
		}

		// Token: 0x0600F4D0 RID: 62672 RVA: 0x0047271F File Offset: 0x0047091F
		public bool IsOverridden(GeneDef geneDef)
		{
			if (!ModsConfig.BiotechActive)
			{
				return false;
			}
			if (!this.genes.Contains(geneDef))
			{
				return false;
			}
			this.RecacheOverridesIfNeeded();
			return !this.cachedNonOverriddenGenes.Contains(geneDef);
		}

		// Token: 0x0600F4D1 RID: 62673 RVA: 0x00472750 File Offset: 0x00470950
		public void GenerateName()
		{
			if (ModsConfig.BiotechActive && this.genes.Any<GeneDef>())
			{
				this.name = GrammarResolver.Resolve("r_name", new GrammarRequest
				{
					Includes = 
					{
						RulePackDefOf.NamerGenepack
					},
					Rules = 
					{
						new Rule_String("geneWord", this.LabelShortAdj),
						new Rule_String("geneCountMinusOne", (this.genes.Count - 1).ToString())
					},
					Constants = 
					{
						{
							"geneCount",
							this.genes.Count.ToString()
						}
					}
				}, null, false, null, null, null, false);
				return;
			}
			this.name = "ERR";
		}

		// Token: 0x0600F4D2 RID: 62674 RVA: 0x00472820 File Offset: 0x00470A20
		public bool CanAddGeneDuringGeneration(GeneDef gene)
		{
			if (!ModsConfig.BiotechActive)
			{
				return false;
			}
			if (this.genes.Contains(gene))
			{
				return false;
			}
			if (this.genes.Count > 0 && !GeneTuning.BiostatRange.Includes(gene.biostatMet + this.MetabolismTotal))
			{
				return false;
			}
			if (!gene.canGenerateInGeneSet)
			{
				return false;
			}
			if (gene.prerequisite != null && !this.genes.Contains(gene.prerequisite))
			{
				return false;
			}
			for (int i = 0; i < this.genes.Count; i++)
			{
				if (gene.ConflictsWith(this.genes[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600F4D3 RID: 62675 RVA: 0x004728C6 File Offset: 0x00470AC6
		public void SortGenes()
		{
			this.genes.SortBy((GeneDef x) => -x.displayCategory.displayPriorityInGenepack);
		}

		// Token: 0x0600F4D4 RID: 62676 RVA: 0x004728F4 File Offset: 0x00470AF4
		public override string ToString()
		{
			string str = "GeneSet: ";
			string str2;
			if (!this.genes.NullOrEmpty<GeneDef>())
			{
				str2 = (from x in this.genes
				select x.LabelCap.ToString()).ToCommaList(false, false);
			}
			else
			{
				str2 = "None";
			}
			return str + str2;
		}

		// Token: 0x0600F4D5 RID: 62677 RVA: 0x00472950 File Offset: 0x00470B50
		public IEnumerable<StatDrawEntry> SpecialDisplayStats(Dialog_InfoCard.Hyperlink? inspectGenesHyperlink = null)
		{
			IEnumerable<Dialog_InfoCard.Hyperlink> enumerable = Dialog_InfoCard.DefsToHyperlinks(this.GeneDefHyperlinks);
			if (inspectGenesHyperlink != null)
			{
				enumerable = enumerable.Concat(inspectGenesHyperlink.Value);
			}
			yield return new StatDrawEntry(StatCategoryDefOf.Genetics, "Genes".Translate().CapitalizeFirst(), (from x in this.genes
			select x.label).ToCommaList(false, false).CapitalizeFirst(), "ContainedGenesDesc".Translate() + ":", 999, null, enumerable, false, false);
			int complexityTotal = this.ComplexityTotal;
			if (complexityTotal != 0)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Genetics, "ComplexityTotal".Translate(), complexityTotal.ToStringWithSign(), "ComplexityTotalDesc".Translate(), 998, null, null, false, false);
			}
			int metabolismTotal = this.MetabolismTotal;
			if (metabolismTotal != 0)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Genetics, "MetabolismTotal".Translate(), metabolismTotal.ToStringWithSign(), "MetabolismTotalDesc".Translate(), 997, null, null, false, false);
			}
			int num = this.genes.Sum((GeneDef x) => x.biostatArc);
			if (num != 0)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Genetics, "ArchitesRequired".Translate(), num.ToString(), "ArchitesRequiredDesc".Translate(), 995, null, null, false, false);
			}
			yield break;
		}

		// Token: 0x0600F4D6 RID: 62678 RVA: 0x00472967 File Offset: 0x00470B67
		public void Debug_RemoveGene(GeneDef gene)
		{
			if (this.genes.Remove(gene))
			{
				this.DirtyCache();
			}
		}

		// Token: 0x0600F4D7 RID: 62679 RVA: 0x0047297D File Offset: 0x00470B7D
		private void DirtyCache()
		{
			this.name = null;
			this.cachedNonOverriddenGenes = null;
		}

		// Token: 0x0600F4D8 RID: 62680 RVA: 0x00472990 File Offset: 0x00470B90
		private void RecacheOverridesIfNeeded()
		{
			if (this.cachedNonOverriddenGenes == null)
			{
				this.cachedNonOverriddenGenes = new List<GeneDef>();
				foreach (GeneDef geneDef in this.genes)
				{
					this.tmpGeneDefWithTypes.Add(new GeneDefWithType(geneDef, true));
				}
				this.cachedNonOverriddenGenes.AddRange(this.tmpGeneDefWithTypes.NonOverriddenGenes());
				this.tmpGeneDefWithTypes.Clear();
			}
		}

		// Token: 0x0600F4D9 RID: 62681 RVA: 0x00472A24 File Offset: 0x00470C24
		public GeneSet Copy()
		{
			GeneSet geneSet = new GeneSet();
			geneSet.name = this.name;
			geneSet.genes.AddRange(this.genes);
			geneSet.DirtyCache();
			return geneSet;
		}

		// Token: 0x0600F4DA RID: 62682 RVA: 0x00472A50 File Offset: 0x00470C50
		public bool Matches(GeneSet other)
		{
			if (this.genes.Count != other.genes.Count)
			{
				return false;
			}
			for (int i = 0; i < this.genes.Count; i++)
			{
				if (!other.genes.Contains(this.genes[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600F4DB RID: 62683 RVA: 0x00472AAC File Offset: 0x00470CAC
		public void ExposeData()
		{
			Scribe_Collections.Look<GeneDef>(ref this.genes, "genes", LookMode.Def, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.name, "name", null, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.genes.RemoveAll((GeneDef x) => x == null);
			}
		}

		// Token: 0x0400A14E RID: 41294
		private List<GeneDef> genes = new List<GeneDef>();

		// Token: 0x0400A14F RID: 41295
		private string name;

		// Token: 0x0400A150 RID: 41296
		[Unsaved(false)]
		private List<GeneDef> cachedNonOverriddenGenes;

		// Token: 0x0400A151 RID: 41297
		public const string ErrorSlugline = "ERR";

		// Token: 0x0400A152 RID: 41298
		private List<GeneDefWithType> tmpGeneDefWithTypes = new List<GeneDefWithType>();
	}
}
