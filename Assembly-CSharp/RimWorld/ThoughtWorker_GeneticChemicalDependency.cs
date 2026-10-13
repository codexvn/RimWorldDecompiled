using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018C3 RID: 6339
	public class ThoughtWorker_GeneticChemicalDependency : ThoughtWorker
	{
		// Token: 0x060090A7 RID: 37031 RVA: 0x002A9BFF File Offset: 0x002A7DFF
		public override string PostProcessLabel(Pawn p, string label)
		{
			return label.Formatted(this.def.chemicalDef.Named("CHEMICAL"));
		}

		// Token: 0x060090A8 RID: 37032 RVA: 0x002A9C21 File Offset: 0x002A7E21
		public override string PostProcessDescription(Pawn p, string description)
		{
			return base.PostProcessDescription(p, description.Formatted(this.def.chemicalDef.Named("CHEMICAL")));
		}

		// Token: 0x060090A9 RID: 37033 RVA: 0x002A9C4C File Offset: 0x002A7E4C
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!ModsConfig.BiotechActive || p.genes == null)
			{
				return ThoughtState.Inactive;
			}
			foreach (Gene gene in p.genes.GenesListForReading)
			{
				Gene_ChemicalDependency gene_ChemicalDependency = gene as Gene_ChemicalDependency;
				if (gene_ChemicalDependency != null && gene_ChemicalDependency.def.chemical == this.def.chemicalDef && gene_ChemicalDependency.LinkedHediff != null)
				{
					return gene_ChemicalDependency.LinkedHediff.Severity >= 1f;
				}
			}
			return ThoughtState.Inactive;
		}
	}
}
