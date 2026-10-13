using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200305E RID: 12382
	public class Alert_Analyzable_NeuralLump : Alert_Analyzable
	{
		// Token: 0x17002CFB RID: 11515
		// (get) Token: 0x06011631 RID: 71217 RVA: 0x004FCE8F File Offset: 0x004FB08F
		protected override ThingDef Def
		{
			get
			{
				return ThingDefOf.FleshmassNeuralLump;
			}
		}

		// Token: 0x06011632 RID: 71218 RVA: 0x004FCE96 File Offset: 0x004FB096
		public Alert_Analyzable_NeuralLump()
		{
			this.requireAnomaly = true;
			this.defaultLabel = "AlertNeuralLump".Translate();
			this.defaultExplanation = "AlertNeuralLumpDesc".Translate();
		}
	}
}
