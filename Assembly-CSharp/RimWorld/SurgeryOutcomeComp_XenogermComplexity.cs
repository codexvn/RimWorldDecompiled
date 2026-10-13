using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020037C7 RID: 14279
	public class SurgeryOutcomeComp_XenogermComplexity : SurgeryOutcomeComp_Curve
	{
		// Token: 0x06013C7A RID: 81018 RVA: 0x005DE7FC File Offset: 0x005DC9FC
		protected override float XGetter(RecipeDef recipe, Pawn surgeon, Pawn patient, List<Thing> ingredients, BodyPartRecord part, Bill bill)
		{
			Xenogerm xenogerm = bill.xenogerm;
			if (((xenogerm != null) ? xenogerm.GeneSet : null) == null)
			{
				return 0f;
			}
			return (float)xenogerm.GeneSet.ComplexityTotal;
		}
	}
}
