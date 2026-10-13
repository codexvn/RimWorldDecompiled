using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020037C0 RID: 14272
	public class SurgeryOutcomeComp_Inspired : SurgeryOutcomeComp_Factor
	{
		// Token: 0x06013C68 RID: 81000 RVA: 0x005DE667 File Offset: 0x005DC867
		public override bool Affects(RecipeDef recipe, Pawn surgeon, Pawn patient, BodyPartRecord part)
		{
			return surgeon.InspirationDef == this.inspirationDef && !patient.RaceProps.IsMechanoid;
		}

		// Token: 0x06013C69 RID: 81001 RVA: 0x005DE687 File Offset: 0x005DC887
		public override void PreApply(float quality, RecipeDef recipe, Pawn surgeon, Pawn patient, List<Thing> ingredients, BodyPartRecord part, Bill bill)
		{
			surgeon.mindState.inspirationHandler.EndInspiration(this.inspirationDef);
		}

		// Token: 0x0400DAFF RID: 56063
		public InspirationDef inspirationDef;
	}
}
