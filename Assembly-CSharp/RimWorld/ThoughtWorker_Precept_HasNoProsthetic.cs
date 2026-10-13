using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020017FE RID: 6142
	public class ThoughtWorker_Precept_HasNoProsthetic : ThoughtWorker_Precept
	{
		// Token: 0x06008E86 RID: 36486 RVA: 0x002A34B0 File Offset: 0x002A16B0
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			return !ThoughtWorker_Precept_HasProsthetic.HasProsthetic(p);
		}
	}
}
