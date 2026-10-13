using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020017FF RID: 6143
	public class ThoughtWorker_Precept_HasNoProsthetic_Social : ThoughtWorker_Precept_Social
	{
		// Token: 0x06008E88 RID: 36488 RVA: 0x002A34C0 File Offset: 0x002A16C0
		protected override ThoughtState ShouldHaveThought(Pawn p, Pawn otherPawn)
		{
			return !ThoughtWorker_Precept_HasProsthetic.HasProsthetic(otherPawn);
		}
	}
}
