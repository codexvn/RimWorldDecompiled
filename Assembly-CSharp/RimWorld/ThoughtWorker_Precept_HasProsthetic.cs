using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020017FC RID: 6140
	public class ThoughtWorker_Precept_HasProsthetic : ThoughtWorker_Precept
	{
		// Token: 0x06008E81 RID: 36481 RVA: 0x002A348B File Offset: 0x002A168B
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			return ThoughtWorker_Precept_HasProsthetic.HasProsthetic(p);
		}

		// Token: 0x06008E82 RID: 36482 RVA: 0x002A3498 File Offset: 0x002A1698
		public static bool HasProsthetic(Pawn p)
		{
			return ThoughtWorker_Precept_HasProsthetic_Count.ProstheticsCount(p) > 0;
		}
	}
}
