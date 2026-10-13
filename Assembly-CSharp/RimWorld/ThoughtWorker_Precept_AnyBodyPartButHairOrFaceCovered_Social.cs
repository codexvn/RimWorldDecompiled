using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020017F0 RID: 6128
	public class ThoughtWorker_Precept_AnyBodyPartButHairOrFaceCovered_Social : ThoughtWorker_Precept_Social
	{
		// Token: 0x06008E63 RID: 36451 RVA: 0x002A2FB6 File Offset: 0x002A11B6
		protected override ThoughtState ShouldHaveThought(Pawn p, Pawn otherPawn)
		{
			return ThoughtWorker_Precept_AnyBodyPartButHairOrFaceCovered.HasCoveredBodyPartsButHairOrFace(otherPawn);
		}
	}
}
