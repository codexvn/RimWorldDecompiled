using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200181B RID: 6171
	public class ThoughtWorker_Precept_ArtificialBlind : ThoughtWorker_Precept
	{
		// Token: 0x06008EF7 RID: 36599 RVA: 0x002A491E File Offset: 0x002A2B1E
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			return PawnUtility.IsArtificiallyBlind(p);
		}
	}
}
