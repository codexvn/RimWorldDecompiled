using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001888 RID: 6280
	public class ThoughtWorker_PsychologicallyNude : ThoughtWorker
	{
		// Token: 0x06008FFF RID: 36863 RVA: 0x002A7D84 File Offset: 0x002A5F84
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (ModsConfig.IdeologyActive)
			{
				return false;
			}
			return p.apparel.PsychologicallyNude;
		}
	}
}
