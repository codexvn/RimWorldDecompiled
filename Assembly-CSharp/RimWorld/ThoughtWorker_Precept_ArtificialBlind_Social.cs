using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200181F RID: 6175
	public class ThoughtWorker_Precept_ArtificialBlind_Social : ThoughtWorker_Precept_Social
	{
		// Token: 0x06008EFF RID: 36607 RVA: 0x002A4955 File Offset: 0x002A2B55
		protected override ThoughtState ShouldHaveThought(Pawn p, Pawn otherPawn)
		{
			return PawnUtility.IsArtificiallyBlind(otherPawn);
		}
	}
}
