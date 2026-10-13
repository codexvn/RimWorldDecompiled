using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001794 RID: 6036
	public class ThinkNode_ConditionalOfPlayerFactionOrPlayerGuestOrPlayerPrisoner : ThinkNode_Conditional
	{
		// Token: 0x06008D71 RID: 36209 RVA: 0x00141B4E File Offset: 0x0013FD4E
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Faction == Faction.OfPlayer || pawn.HostFaction == Faction.OfPlayer;
		}
	}
}
