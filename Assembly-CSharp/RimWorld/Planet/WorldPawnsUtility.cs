using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003E4C RID: 15948
	public static class WorldPawnsUtility
	{
		// Token: 0x06015D03 RID: 89347 RVA: 0x00684C78 File Offset: 0x00682E78
		public static bool IsWorldPawn(this Pawn p)
		{
			return Find.WorldPawns.Contains(p);
		}
	}
}
