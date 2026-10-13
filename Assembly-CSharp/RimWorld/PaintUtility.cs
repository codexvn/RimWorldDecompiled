using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200176A RID: 5994
	public static class PaintUtility
	{
		// Token: 0x06008C99 RID: 35993 RVA: 0x0029CFC0 File Offset: 0x0029B1C0
		public static List<Thing> FindNearbyDyes(Pawn pawn, bool forced = false)
		{
			List<Thing> list = new List<Thing>();
			List<Thing> list2 = pawn.Map.listerThings.ThingsOfDef(ThingDefOf.Dye);
			for (int i = 0; i < list2.Count; i++)
			{
				if (!list2[i].IsForbidden(pawn) && pawn.CanReserveAndReach(list2[i], PathEndMode.ClosestTouch, Danger.Deadly, 1, -1, null, forced))
				{
					list.Add(list2[i]);
				}
			}
			return list;
		}
	}
}
