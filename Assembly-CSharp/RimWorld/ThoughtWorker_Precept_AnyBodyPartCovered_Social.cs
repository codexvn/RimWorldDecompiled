using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020017EC RID: 6124
	public class ThoughtWorker_Precept_AnyBodyPartCovered_Social : ThoughtWorker_Precept_Social
	{
		// Token: 0x06008E59 RID: 36441 RVA: 0x002A2D6A File Offset: 0x002A0F6A
		protected override ThoughtState ShouldHaveThought(Pawn p, Pawn otherPawn)
		{
			return ThoughtWorker_Precept_AnyBodyPartCovered.HasUnnecessarilyCoveredBodyParts(otherPawn);
		}
	}
}
