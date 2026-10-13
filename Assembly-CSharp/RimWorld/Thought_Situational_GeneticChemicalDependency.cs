using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018C4 RID: 6340
	public class Thought_Situational_GeneticChemicalDependency : Thought_Situational
	{
		// Token: 0x060090AB RID: 37035 RVA: 0x002A9D00 File Offset: 0x002A7F00
		public override float MoodOffset()
		{
			if (ThoughtUtility.ThoughtNullified(this.pawn, this.def))
			{
				return 0f;
			}
			foreach (Gene gene in this.pawn.genes.GenesListForReading)
			{
				Gene_ChemicalDependency gene_ChemicalDependency = gene as Gene_ChemicalDependency;
				if (gene_ChemicalDependency != null && gene_ChemicalDependency.def.chemical == this.def.chemicalDef && gene_ChemicalDependency.LinkedHediff != null)
				{
					return Thought_Situational_GeneticChemicalDependency.MoodOffsetCurve.Evaluate(gene_ChemicalDependency.LinkedHediff.Severity);
				}
			}
			return 0f;
		}

		// Token: 0x04005C4A RID: 23626
		private static readonly SimpleCurve MoodOffsetCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0f),
				true
			},
			{
				new CurvePoint(1f, -4f),
				true
			},
			{
				new CurvePoint(6f, -20f),
				true
			}
		};
	}
}
