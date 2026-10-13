using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020037C8 RID: 14280
	public class SurgeryOutcomeComp_PatientAge : SurgeryOutcomeComp_Curve
	{
		// Token: 0x06013C7C RID: 81020 RVA: 0x005DE831 File Offset: 0x005DCA31
		protected override float XGetter(RecipeDef recipe, Pawn surgeon, Pawn patient, List<Thing> ingredients, BodyPartRecord part, Bill bill)
		{
			return patient.ageTracker.AgeBiologicalYearsFloat;
		}
	}
}
