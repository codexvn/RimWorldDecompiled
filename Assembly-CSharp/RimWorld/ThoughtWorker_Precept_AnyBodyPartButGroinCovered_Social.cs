using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020017EE RID: 6126
	public class ThoughtWorker_Precept_AnyBodyPartButGroinCovered_Social : ThoughtWorker_Precept_Social
	{
		// Token: 0x06008E5E RID: 36446 RVA: 0x002A2EA2 File Offset: 0x002A10A2
		protected override ThoughtState ShouldHaveThought(Pawn p, Pawn otherPawn)
		{
			return ThoughtWorker_Precept_AnyBodyPartButGroinCovered.HasCoveredBodyPartsButGroin(otherPawn);
		}
	}
}
