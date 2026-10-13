using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020037C4 RID: 14276
	public class SurgeryOutcomeComp_BedAndRoomQuality : SurgeryOutcomeComp
	{
		// Token: 0x06013C72 RID: 81010 RVA: 0x005DE6F7 File Offset: 0x005DC8F7
		public override bool Affects(RecipeDef recipe, Pawn surgeon, Pawn patient, BodyPartRecord part)
		{
			return !recipe.surgeryIgnoreEnvironment && patient.InBed();
		}

		// Token: 0x06013C73 RID: 81011 RVA: 0x005DE709 File Offset: 0x005DC909
		public override void AffectQuality(RecipeDef recipe, Pawn surgeon, Pawn patient, List<Thing> ingredients, BodyPartRecord part, Bill bill, ref float quality)
		{
			quality *= patient.CurrentBed().GetStatValue(StatDefOf.SurgerySuccessChanceFactor, true, -1);
		}
	}
}
