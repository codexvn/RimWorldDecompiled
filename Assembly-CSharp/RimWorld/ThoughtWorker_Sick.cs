using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001882 RID: 6274
	public class ThoughtWorker_Sick : ThoughtWorker
	{
		// Token: 0x06008FF2 RID: 36850 RVA: 0x002A7A98 File Offset: 0x002A5C98
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return p.health.hediffSet.AnyHediffMakesSickThought;
		}
	}
}
