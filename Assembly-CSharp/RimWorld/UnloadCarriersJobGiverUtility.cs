using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200142A RID: 5162
	public static class UnloadCarriersJobGiverUtility
	{
		// Token: 0x06007D6F RID: 32111 RVA: 0x0025C420 File Offset: 0x0025A620
		public static bool HasJobOnThing(Pawn pawn, Thing t, bool forced)
		{
			Pawn pawn2 = t as Pawn;
			return pawn2 != null && pawn2 != pawn && !pawn2.IsFreeColonist && pawn2.inventory.UnloadEverything && (pawn2.Faction == pawn.Faction || pawn2.HostFaction == pawn.Faction) && !t.IsForbidden(pawn) && !t.IsBurning() && pawn.CanReserve(t, 1, -1, null, forced);
		}
	}
}
