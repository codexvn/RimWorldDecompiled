using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001792 RID: 6034
	public class ThinkNode_ConditionalOfPlayerFactionOrPlayerPrisoner : ThinkNode_Conditional
	{
		// Token: 0x06008D6D RID: 36205 RVA: 0x002A0AEB File Offset: 0x0029ECEB
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Faction == Faction.OfPlayer || (pawn.HostFaction == Faction.OfPlayer && pawn.guest.IsPrisoner);
		}
	}
}
