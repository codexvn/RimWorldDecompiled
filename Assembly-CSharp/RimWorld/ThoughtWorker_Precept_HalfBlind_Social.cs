using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200181E RID: 6174
	public class ThoughtWorker_Precept_HalfBlind_Social : ThoughtWorker_Precept_Social
	{
		// Token: 0x06008EFD RID: 36605 RVA: 0x002A4948 File Offset: 0x002A2B48
		protected override ThoughtState ShouldHaveThought(Pawn p, Pawn otherPawn)
		{
			return ThoughtWorker_Precept_HalfBlind.IsHalfBlind(otherPawn);
		}
	}
}
