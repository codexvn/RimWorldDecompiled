using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020017F4 RID: 6132
	public class ThoughtWorker_Precept_GroinUncovered_Social : ThoughtWorker_Precept_Social
	{
		// Token: 0x06008E6D RID: 36461 RVA: 0x002A30E4 File Offset: 0x002A12E4
		protected override ThoughtState ShouldHaveThought(Pawn p, Pawn otherPawn)
		{
			return ThoughtWorker_Precept_GroinUncovered.HasUncoveredGroin(otherPawn);
		}
	}
}
