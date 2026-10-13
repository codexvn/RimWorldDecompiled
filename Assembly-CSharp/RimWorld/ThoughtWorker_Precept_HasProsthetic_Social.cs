using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020017FD RID: 6141
	public class ThoughtWorker_Precept_HasProsthetic_Social : ThoughtWorker_Precept_Social
	{
		// Token: 0x06008E84 RID: 36484 RVA: 0x002A34A3 File Offset: 0x002A16A3
		protected override ThoughtState ShouldHaveThought(Pawn p, Pawn otherPawn)
		{
			return ThoughtWorker_Precept_HasProsthetic.HasProsthetic(otherPawn);
		}
	}
}
