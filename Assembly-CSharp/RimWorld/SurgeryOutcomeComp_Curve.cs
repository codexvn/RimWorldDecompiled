using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020037C5 RID: 14277
	public abstract class SurgeryOutcomeComp_Curve : SurgeryOutcomeComp
	{
		// Token: 0x06013C75 RID: 81013
		protected abstract float XGetter(RecipeDef recipe, Pawn surgeon, Pawn patient, List<Thing> ingredients, BodyPartRecord part, Bill bill);

		// Token: 0x06013C76 RID: 81014 RVA: 0x005DE724 File Offset: 0x005DC924
		public override void AffectQuality(RecipeDef recipe, Pawn surgeon, Pawn patient, List<Thing> ingredients, BodyPartRecord part, Bill bill, ref float quality)
		{
			quality *= this.curve.Evaluate(this.XGetter(recipe, surgeon, patient, ingredients, part, bill));
		}

		// Token: 0x0400DB01 RID: 56065
		public SimpleCurve curve;
	}
}
