using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001799 RID: 6041
	public class ThinkNode_ConditionalNonPlayerNonHostileFactionOrFactionless : ThinkNode_Conditional
	{
		// Token: 0x06008D7C RID: 36220 RVA: 0x002A0BB7 File Offset: 0x0029EDB7
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Faction == null || (pawn.Faction != Faction.OfPlayer && !pawn.Faction.HostileTo(Faction.OfPlayer));
		}
	}
}
