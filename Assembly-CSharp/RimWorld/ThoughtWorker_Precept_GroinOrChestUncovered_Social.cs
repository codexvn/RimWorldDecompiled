using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020017F6 RID: 6134
	public class ThoughtWorker_Precept_GroinOrChestUncovered_Social : ThoughtWorker_Precept_Social
	{
		// Token: 0x06008E72 RID: 36466 RVA: 0x002A31BB File Offset: 0x002A13BB
		protected override ThoughtState ShouldHaveThought(Pawn p, Pawn otherPawn)
		{
			return ThoughtWorker_Precept_GroinOrChestUncovered.HasUncoveredGroinOrChest(otherPawn);
		}
	}
}
