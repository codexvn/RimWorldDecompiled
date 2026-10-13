using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020036E6 RID: 14054
	public class StatPart_MetabolismTotal : StatPart_BiostatTotal
	{
		// Token: 0x17003220 RID: 12832
		// (get) Token: 0x060137F8 RID: 79864 RVA: 0x005C9AD6 File Offset: 0x005C7CD6
		public override string ExplanationLabelBase
		{
			get
			{
				return "FactorForMetabolism".Translate();
			}
		}

		// Token: 0x060137F9 RID: 79865 RVA: 0x005C9AE8 File Offset: 0x005C7CE8
		protected override float CurveXGetter(StatRequest req)
		{
			Pawn pawn = (Pawn)req.Thing;
			int num = 0;
			foreach (Gene gene in pawn.genes.GenesListForReading)
			{
				if (!gene.Overridden)
				{
					num += gene.def.biostatMet;
				}
			}
			return (float)num;
		}
	}
}
