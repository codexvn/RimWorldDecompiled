using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020017F8 RID: 6136
	public class ThoughtWorker_Precept_GroinChestOrHairUncovered_Social : ThoughtWorker_Precept_Social
	{
		// Token: 0x06008E77 RID: 36471 RVA: 0x002A32EA File Offset: 0x002A14EA
		protected override ThoughtState ShouldHaveThought(Pawn p, Pawn otherPawn)
		{
			return ThoughtWorker_Precept_GroinChestOrHairUncovered.HasUncoveredGroinChestOrHair(otherPawn);
		}
	}
}
