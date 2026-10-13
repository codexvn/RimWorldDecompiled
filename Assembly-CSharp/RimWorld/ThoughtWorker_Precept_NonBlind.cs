using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200181C RID: 6172
	public class ThoughtWorker_Precept_NonBlind : ThoughtWorker_Precept
	{
		// Token: 0x06008EF9 RID: 36601 RVA: 0x002A492B File Offset: 0x002A2B2B
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			return !PawnUtility.IsBiologicallyOrArtificiallyBlind(p);
		}
	}
}
