using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001798 RID: 6040
	public class ThinkNode_ConditionalNonPlayerNonHostileFaction : ThinkNode_Conditional
	{
		// Token: 0x06008D7A RID: 36218 RVA: 0x002A0B8B File Offset: 0x0029ED8B
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Faction != null && pawn.Faction != Faction.OfPlayer && !pawn.Faction.HostileTo(Faction.OfPlayer);
		}
	}
}
