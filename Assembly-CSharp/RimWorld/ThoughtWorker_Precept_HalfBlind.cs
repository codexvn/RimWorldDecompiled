using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200181A RID: 6170
	public class ThoughtWorker_Precept_HalfBlind : ThoughtWorker_Precept
	{
		// Token: 0x06008EF4 RID: 36596 RVA: 0x002A48FF File Offset: 0x002A2AFF
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			return ThoughtWorker_Precept_HalfBlind.IsHalfBlind(p);
		}

		// Token: 0x06008EF5 RID: 36597 RVA: 0x002A490C File Offset: 0x002A2B0C
		public static bool IsHalfBlind(Pawn p)
		{
			return !PawnUtility.IsBiologicallyOrArtificiallyBlind(p) && HealthUtility.IsMissingSightBodyPart(p);
		}
	}
}
