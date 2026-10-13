using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001803 RID: 6147
	public class ThoughtWorker_Precept_LeaderResentment : ThoughtWorker_Precept
	{
		// Token: 0x06008E90 RID: 36496 RVA: 0x002A36F4 File Offset: 0x002A18F4
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			Faction faction = p.Faction;
			Pawn pawn = (faction != null) ? faction.leader : null;
			return pawn != null && p.Ideo != pawn.Ideo;
		}
	}
}
