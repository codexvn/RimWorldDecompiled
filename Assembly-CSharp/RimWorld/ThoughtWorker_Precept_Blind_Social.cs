using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200181D RID: 6173
	public class ThoughtWorker_Precept_Blind_Social : ThoughtWorker_Precept_Social
	{
		// Token: 0x06008EFB RID: 36603 RVA: 0x002A493B File Offset: 0x002A2B3B
		protected override ThoughtState ShouldHaveThought(Pawn p, Pawn otherPawn)
		{
			return PawnUtility.IsBiologicallyBlind(otherPawn);
		}
	}
}
