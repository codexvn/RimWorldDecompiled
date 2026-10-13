using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020037C1 RID: 14273
	public class SurgeryOutcomeComp_ClampToRange : SurgeryOutcomeComp
	{
		// Token: 0x06013C6B RID: 81003 RVA: 0x005DE6A7 File Offset: 0x005DC8A7
		public override void AffectQuality(RecipeDef recipe, Pawn surgeon, Pawn patient, List<Thing> ingredients, BodyPartRecord part, Bill bill, ref float quality)
		{
			quality = this.range.ClampToRange(quality);
		}

		// Token: 0x0400DB00 RID: 56064
		public FloatRange range;
	}
}
