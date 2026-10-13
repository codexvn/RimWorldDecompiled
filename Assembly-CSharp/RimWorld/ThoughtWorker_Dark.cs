using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001879 RID: 6265
	public class ThoughtWorker_Dark : ThoughtWorker
	{
		// Token: 0x06008FDB RID: 36827 RVA: 0x002A7614 File Offset: 0x002A5814
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return p.Awake() && p.needs.mood.recentMemory.TicksSinceLastLight > 240 && !PawnUtility.IsBiologicallyOrArtificiallyBlind(p) && (p.Ideo == null || !p.Ideo.IdeoPrefersDarkness());
		}
	}
}
