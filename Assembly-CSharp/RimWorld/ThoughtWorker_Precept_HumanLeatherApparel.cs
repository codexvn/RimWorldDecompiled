using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020017E5 RID: 6117
	public class ThoughtWorker_Precept_HumanLeatherApparel : ThoughtWorker_Precept
	{
		// Token: 0x06008E3C RID: 36412 RVA: 0x002A273E File Offset: 0x002A093E
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			return ThoughtWorker_HumanLeatherApparel.CurrentThoughtState(p);
		}
	}
}
