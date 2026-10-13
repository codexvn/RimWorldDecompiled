using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002B1F RID: 11039
	public static class BrieflyDespawnedUtil
	{
		// Token: 0x0600F6B7 RID: 63159 RVA: 0x0047A7E1 File Offset: 0x004789E1
		public static bool BrieflyDespawned(this Pawn pawn)
		{
			return !pawn.Spawned && pawn.ParentHolder is PawnFlyer;
		}
	}
}
