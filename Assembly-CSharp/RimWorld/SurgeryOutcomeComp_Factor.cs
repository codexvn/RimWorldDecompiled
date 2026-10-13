using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020037BF RID: 14271
	public class SurgeryOutcomeComp_Factor : SurgeryOutcomeComp
	{
		// Token: 0x06013C66 RID: 80998 RVA: 0x005DE645 File Offset: 0x005DC845
		public override void AffectQuality(RecipeDef recipe, Pawn surgeon, Pawn patient, List<Thing> ingredients, BodyPartRecord part, Bill bill, ref float quality)
		{
			quality *= this.factor;
		}

		// Token: 0x0400DAFE RID: 56062
		public float factor = 1f;
	}
}
