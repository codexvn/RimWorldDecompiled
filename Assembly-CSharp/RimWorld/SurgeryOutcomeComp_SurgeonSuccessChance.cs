using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020037C3 RID: 14275
	public class SurgeryOutcomeComp_SurgeonSuccessChance : SurgeryOutcomeComp
	{
		// Token: 0x06013C6F RID: 81007 RVA: 0x005DE6D1 File Offset: 0x005DC8D1
		public override bool Affects(RecipeDef recipe, Pawn surgeon, Pawn patient, BodyPartRecord part)
		{
			return !patient.RaceProps.IsMechanoid;
		}

		// Token: 0x06013C70 RID: 81008 RVA: 0x005DE6E1 File Offset: 0x005DC8E1
		public override void AffectQuality(RecipeDef recipe, Pawn surgeon, Pawn patient, List<Thing> ingredients, BodyPartRecord part, Bill bill, ref float quality)
		{
			quality *= surgeon.GetStatValue(StatDefOf.MedicalSurgerySuccessChance, true, -1);
		}
	}
}
