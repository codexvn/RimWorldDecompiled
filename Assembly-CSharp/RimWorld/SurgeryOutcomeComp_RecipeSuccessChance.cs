using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020037C2 RID: 14274
	public class SurgeryOutcomeComp_RecipeSuccessChance : SurgeryOutcomeComp
	{
		// Token: 0x06013C6D RID: 81005 RVA: 0x005DE6C2 File Offset: 0x005DC8C2
		public override void AffectQuality(RecipeDef recipe, Pawn surgeon, Pawn patient, List<Thing> ingredients, BodyPartRecord part, Bill bill, ref float quality)
		{
			quality *= recipe.surgerySuccessChanceFactor;
		}
	}
}
