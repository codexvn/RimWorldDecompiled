using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001793 RID: 6035
	public class ThinkNode_ConditionalOfPlayerFactionOrPlayerGuest : ThinkNode_Conditional
	{
		// Token: 0x06008D6F RID: 36207 RVA: 0x002A0B16 File Offset: 0x0029ED16
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Faction == Faction.OfPlayer || (pawn.HostFaction == Faction.OfPlayer && !pawn.guest.IsPrisoner);
		}
	}
}
