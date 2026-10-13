using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000739 RID: 1849
	public class Hediff_ChemicalDependency : HediffWithComps
	{
		// Token: 0x1700080B RID: 2059
		// (get) Token: 0x06002D0A RID: 11530 RVA: 0x000E89C4 File Offset: 0x000E6BC4
		public override string LabelBase
		{
			get
			{
				return "ChemicalDependency".Translate(this.chemical.Named("CHEMICAL"));
			}
		}

		// Token: 0x1700080C RID: 2060
		// (get) Token: 0x06002D0B RID: 11531 RVA: 0x000E89E5 File Offset: 0x000E6BE5
		public override bool ShouldRemove
		{
			get
			{
				return this.LinkedGene == null || !this.LinkedGene.Active;
			}
		}

		// Token: 0x1700080D RID: 2061
		// (get) Token: 0x06002D0C RID: 11532 RVA: 0x000E89FF File Offset: 0x000E6BFF
		public bool ShouldSatify
		{
			get
			{
				return this.Severity >= this.def.stages[2].minSeverity - 0.1f;
			}
		}

		// Token: 0x1700080E RID: 2062
		// (get) Token: 0x06002D0D RID: 11533 RVA: 0x000E8A28 File Offset: 0x000E6C28
		public Gene_ChemicalDependency LinkedGene
		{
			get
			{
				if (this.cachedDependencyGene == null && this.pawn.genes != null)
				{
					List<Gene> genesListForReading = this.pawn.genes.GenesListForReading;
					for (int i = 0; i < genesListForReading.Count; i++)
					{
						Gene_ChemicalDependency gene_ChemicalDependency = genesListForReading[i] as Gene_ChemicalDependency;
						if (gene_ChemicalDependency != null && gene_ChemicalDependency.def.chemical == this.chemical)
						{
							this.cachedDependencyGene = gene_ChemicalDependency;
							break;
						}
					}
				}
				return this.cachedDependencyGene;
			}
		}

		// Token: 0x1700080F RID: 2063
		// (get) Token: 0x06002D0E RID: 11534 RVA: 0x000E8A9E File Offset: 0x000E6C9E
		// (set) Token: 0x06002D0F RID: 11535 RVA: 0x000E8AC7 File Offset: 0x000E6CC7
		public override float Severity
		{
			get
			{
				if (this.LinkedGene == null || !this.LinkedGene.Active)
				{
					return this.def.initialSeverity;
				}
				return base.Severity;
			}
			set
			{
				base.Severity = value;
			}
		}

		// Token: 0x17000810 RID: 2064
		// (get) Token: 0x06002D10 RID: 11536 RVA: 0x000E8AD0 File Offset: 0x000E6CD0
		public override string TipStringExtra
		{
			get
			{
				string text = base.TipStringExtra;
				Gene_ChemicalDependency linkedGene = this.LinkedGene;
				if (linkedGene != null)
				{
					if (!text.NullOrEmpty())
					{
						text += "\n\n";
					}
					text += "GeneDefChemicalNeedDurationDesc".Translate(this.chemical.label, this.pawn.Named("PAWN"), "PeriodDays".Translate(5f).Named("DEFICIENCYDURATION"), "PeriodDays".Translate(30f).Named("COMADURATION"), "PeriodDays".Translate(60f).Named("DEATHDURATION")).Resolve();
					text = text + "\n\n" + "LastIngestedDurationAgo".Translate(this.chemical.Named("CHEMICAL"), (Find.TickManager.TicksGame - linkedGene.lastIngestedTick).ToStringTicksToPeriod(true, false, true, true, false).Named("DURATION")).Resolve();
				}
				return text;
			}
		}

		// Token: 0x06002D11 RID: 11537 RVA: 0x000E8BFC File Offset: 0x000E6DFC
		public override bool TryMergeWith(Hediff other)
		{
			Hediff_ChemicalDependency hediff_ChemicalDependency = other as Hediff_ChemicalDependency;
			return hediff_ChemicalDependency != null && hediff_ChemicalDependency.chemical == this.chemical && base.TryMergeWith(other);
		}

		// Token: 0x06002D12 RID: 11538 RVA: 0x000E8C2C File Offset: 0x000E6E2C
		public override void CopyFrom(Hediff other)
		{
			base.CopyFrom(other);
			Hediff_ChemicalDependency hediff_ChemicalDependency = other as Hediff_ChemicalDependency;
			if (hediff_ChemicalDependency != null)
			{
				this.chemical = hediff_ChemicalDependency.chemical;
			}
		}

		// Token: 0x06002D13 RID: 11539 RVA: 0x000E8C56 File Offset: 0x000E6E56
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ChemicalDef>(ref this.chemical, "chemical");
		}

		// Token: 0x040023D4 RID: 9172
		public ChemicalDef chemical;

		// Token: 0x040023D5 RID: 9173
		[Unsaved(false)]
		private Gene_ChemicalDependency cachedDependencyGene;
	}
}
