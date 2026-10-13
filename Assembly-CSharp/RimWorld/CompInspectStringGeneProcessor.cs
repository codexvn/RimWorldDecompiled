using System;
using System.Text;

namespace RimWorld
{
	// Token: 0x02002CF4 RID: 11508
	public class CompInspectStringGeneProcessor : CompInspectString
	{
		// Token: 0x060102A4 RID: 66212 RVA: 0x004AD480 File Offset: 0x004AB680
		public override string CompInspectStringExtra()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.Props.inspectString);
			stringBuilder.Append(": +");
			stringBuilder.Append(this.parent.GetStatValue(StatDefOf.GeneticComplexityIncrease, true, -1));
			return stringBuilder.ToString();
		}
	}
}
