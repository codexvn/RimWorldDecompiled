using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001819 RID: 6169
	public class ThoughtWorker_Precept_Blind : ThoughtWorker_Precept
	{
		// Token: 0x06008EF2 RID: 36594 RVA: 0x002A48F2 File Offset: 0x002A2AF2
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			return PawnUtility.IsBiologicallyBlind(p);
		}
	}
}
