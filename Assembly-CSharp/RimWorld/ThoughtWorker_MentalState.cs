using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001884 RID: 6276
	public class ThoughtWorker_MentalState : ThoughtWorker
	{
		// Token: 0x06008FF7 RID: 36855 RVA: 0x002A7BB1 File Offset: 0x002A5DB1
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return p.MentalStateDef == this.def.mentalState;
		}
	}
}
