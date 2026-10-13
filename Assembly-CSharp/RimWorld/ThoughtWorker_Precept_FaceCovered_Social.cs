using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020017F2 RID: 6130
	public class ThoughtWorker_Precept_FaceCovered_Social : ThoughtWorker_Precept_Social
	{
		// Token: 0x06008E68 RID: 36456 RVA: 0x002A304C File Offset: 0x002A124C
		protected override ThoughtState ShouldHaveThought(Pawn p, Pawn otherPawn)
		{
			return ThoughtWorker_Precept_FaceCovered.HasCoveredFace(otherPawn);
		}
	}
}
