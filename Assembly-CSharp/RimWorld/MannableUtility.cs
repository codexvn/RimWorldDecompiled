using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002D2A RID: 11562
	public static class MannableUtility
	{
		// Token: 0x060103DC RID: 66524 RVA: 0x004B20C0 File Offset: 0x004B02C0
		public static Thing MannedThing(this Pawn pawn)
		{
			if (pawn.Dead)
			{
				return null;
			}
			Thing lastMannedThing = pawn.mindState.lastMannedThing;
			if (lastMannedThing == null || lastMannedThing.TryGetComp<CompMannable>().ManningPawn != pawn)
			{
				return null;
			}
			return lastMannedThing;
		}
	}
}
