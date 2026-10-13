using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001820 RID: 6176
	public class ThoughtWorker_Precept_NonBlind_Social : ThoughtWorker_Precept_Social
	{
		// Token: 0x06008F01 RID: 36609 RVA: 0x002A4962 File Offset: 0x002A2B62
		protected override ThoughtState ShouldHaveThought(Pawn p, Pawn otherPawn)
		{
			return !PawnUtility.IsBiologicallyBlind(otherPawn);
		}
	}
}
